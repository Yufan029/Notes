using Zack.Commons;

namespace UserMgr.Domain.Entities;

public record User : IAggregateRoot
{
    public Guid Id { get; init; }
    public PhoneNumber PhoneNumber { get; private set; }
    private string? passwordHash;

    // 和 UserAccessFail 是强关联关系，同一个聚合，所以直接引用 UserAccessFail 实体
    public UserAccessFail UserAccessFail { get; private set; }

    // EF Core 加载数据专用构造函数
    private User() { }

    public User(PhoneNumber phoneNumber)
    {
        Id = Guid.NewGuid();
        PhoneNumber = phoneNumber;
        UserAccessFail = new UserAccessFail(this);
    }

    public bool HasPassword()
    {
        return !string.IsNullOrEmpty(passwordHash);
    }

    public void ChangePassword(string newPassword)
    {
        if (string.IsNullOrWhiteSpace(newPassword) || newPassword.Length <= 3)
        {
            throw new ArgumentException("密码长度不能少于6位");
        }

        this.passwordHash = HashHelper.ComputeMd5Hash(newPassword);
    }

    public bool CheckPassword(string password)
    {
        if (string.IsNullOrEmpty(this.passwordHash))
        {
            throw new InvalidOperationException("用户未设置密码");
        }

        var hash = HashHelper.ComputeMd5Hash(password);
        return hash == this.passwordHash;
    }

    public void ChangePhoneNumber(PhoneNumber newPhoneNumber)
    {
        PhoneNumber = newPhoneNumber;
    }
}