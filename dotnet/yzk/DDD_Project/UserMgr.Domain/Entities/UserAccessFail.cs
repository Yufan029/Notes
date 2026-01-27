public record UserAccessFail
{
    public Guid Id { get; init; }

    // 和 User 是强关联关系，同一个聚合，所以直接引用 User 实体
    public User User { get; init; }
    public Guid UserId { get; init; }
    private bool isLocked;
    public DateTime? LockEndTime { get; private set; }
    public int AccessFailCount { get; private set; }

    // EF Core 加载数据专用构造函数
    private UserAccessFail() { }

    public UserAccessFail(User user)
    {
        Id = Guid.NewGuid();
        User = user;
        UserId = user.Id;
        isLocked = false;
        AccessFailCount = 0;
    }

    public void Reset()
    {
        AccessFailCount = 0;
        isLocked = false;
        LockEndTime = null;
    }

    public void Fail()
    {
        if (isLocked)
        {
            throw new InvalidOperationException("用户已被锁定，无法记录失败次数");
        }

        AccessFailCount++;

        if (AccessFailCount >= 3)
        {
            isLocked = true;
            LockEndTime = DateTime.UtcNow.AddMinutes(5);
        }
    }

    public bool IsLocked()
    {
        if (isLocked && LockEndTime.HasValue && DateTime.UtcNow >= LockEndTime.Value)
        {
            // 解锁用户
            this.Reset();
        }

        return isLocked;
    }
}