
// 独立的聚合，aggregate root
public record UserLoginHistory : IAggregateRoot
{
    public Guid Id { get; init; }
    
    // 没有加 User, 因为他们两个是独立的聚合，聚合之间没有强关系，可以方便以后微服务拆分
    public Guid? UserId { get; init; }
    public PhoneNumber PhoneNumber { get; init; }
    public DateTime CreatedDateTime { get; init; }
    public string Message { get; init; }
    
    private UserLoginHistory() { }
    public UserLoginHistory(Guid? userId, PhoneNumber phoneNumber, string message)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        PhoneNumber = phoneNumber;
        CreatedDateTime = DateTime.UtcNow;
        Message = message;
    }
}