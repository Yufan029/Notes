using UserMgr.Domain;

namespace UserMgr.Infrastructure;

public class MockSmsCodeSender : ISmsCodeSender
{
    public Task SendCodeAsync(PhoneNumber phoneNumber, string code)
    {
        Console.WriteLine($"向手机号 {phoneNumber.RegionNumber}-{phoneNumber.Number} 发送验证码：{code}");
        return Task.CompletedTask;
    }
}