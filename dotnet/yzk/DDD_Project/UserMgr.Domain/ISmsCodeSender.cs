namespace UserMgr.Domain;

public interface ISmsCodeSender
{
    public Task SendCodeAsync(PhoneNumber phoneNumber, string code);
}