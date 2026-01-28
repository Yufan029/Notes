using UserMgr.Domain;
using UserMgr.Domain.Entities;

public class UserDomainService
{
    private readonly IUserRepository userRepository;
    private readonly ISmsCodeSender smsCodeSender;

    public UserDomainService(IUserRepository userRepository, ISmsCodeSender smsCodeSender)
    {
        this.userRepository = userRepository;
        this.smsCodeSender = smsCodeSender;
    }

    public async Task<UserAccessResult> CheckPasswordAsync(PhoneNumber phoneNumber, string password)
    {
        UserAccessResult result;
        var user = await userRepository.FindOneAsync(phoneNumber);
        if (user == null)
        {
            result = UserAccessResult.PhoneNumberNotFound;
        }
        else if (this.IsLockOut(user))
        {
            result = UserAccessResult.Lockout;
        }
        else if (!user.HasPassword())
        {
            result = UserAccessResult.NoPassword;
        }
        else if (!user.CheckPassword(password))
        {
            result = UserAccessResult.PasswordError;
        }
        else
        {
            result = UserAccessResult.OK;
        }

        if (user != null)
        {
            if (result == UserAccessResult.OK)
            {
                this.ResetUserAccessFail(user);
            }
            else
            {
                this.AccessFail(user);
            }
        }

        return result;
    }

    public async Task<CheckCodeResult> CheckPhoneNumberCodeAsync(PhoneNumber phoneNumber, string code)
    {
        var user = await userRepository.FindOneAsync(phoneNumber);
        if (user == null)
        {
            return CheckCodeResult.PhoneNumberNotFound;
        }

        if (this.IsLockOut(user))
        {
            return CheckCodeResult.Lockout;
        }

        var savedCode = await userRepository.FindPhoneNumberCodeAsync(phoneNumber);
        if (savedCode != code)
        {
            this.AccessFail(user);
            return CheckCodeResult.CodeError;
        }

        this.ResetUserAccessFail(user);
        return CheckCodeResult.OK;
    }

    public void ResetUserAccessFail(User user)
    {
        user.UserAccessFail.Reset();
    }

    public bool IsLockOut(User user)
    {
        return user.UserAccessFail.IsLocked();
    }

    public void AccessFail(User user)
    {
        user.UserAccessFail.Fail();
    }
}