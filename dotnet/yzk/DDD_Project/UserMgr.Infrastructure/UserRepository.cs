using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using UserMgr.Domain;
using UserMgr.Domain.Entities;
using UserMgr.Domain.Events;

namespace UserMgr.Infrastructure;

public class UserRepository : IUserRepository
{
    private readonly UserDbContext dbContext;
    private readonly IDistributedCache distributedCache;
    private readonly IMediator mediator;

    public UserRepository(UserDbContext dbContext, IDistributedCache distributedCache, IMediator mediator)
    {
        this.dbContext = dbContext;
        this.distributedCache = distributedCache;
        this.mediator = mediator;
    }
    
    public async Task AddNewLoginHistoryAsync(PhoneNumber phoneNumber, string message)
    {
        var user = await FindOneAsync(phoneNumber);
        Guid? userId = null;
        if (user != null)
        {
            userId = user.Id;
        }

        var loginHistory = new UserLoginHistory(userId, phoneNumber, message);
        dbContext.UserLoginHistories.Add(loginHistory);

        // DDD中一般不在Repository里直接保存更改，交给应用服务层统一处理
        //await dbContext.SaveChangesAsync();
    }

    public async Task<User?> FindOneAsync(PhoneNumber phoneNumber)
    {
        return await dbContext.Users.SingleOrDefaultAsync(u => u.PhoneNumber.RegionNumber == phoneNumber.RegionNumber 
                                                            && u.PhoneNumber.Number == phoneNumber.Number);
    }

    public async Task<User?> FindOneAsync(Guid id)
    {
        return await dbContext.Users.SingleOrDefaultAsync(u => u.Id == id);
    }

    public async Task<string?> FindPhoneNumberCodeAsync(PhoneNumber phoneNumber)
    {
        string key = $"PhoneNumberCode_{phoneNumber.RegionNumber}_{phoneNumber.Number}";
        var code = await distributedCache.GetStringAsync(key);
        _ = distributedCache.RemoveAsync(key);
        return code;
    }

    public Task PublishEventAsync(UserAccessResultEvent accessResultEvent)
    {
        return mediator.Publish(accessResultEvent);
    }

    public Task SavePhoneNumberCodeAsync(PhoneNumber phoneNumber, string code)
    {
        string key = $"PhoneNumberCode_{phoneNumber.RegionNumber}_{phoneNumber.Number}";

        // 最后一个返回的方法是async, 可以直接return，方法名也可以省略Async
        return distributedCache.SetStringAsync(key, code, new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
        });
    }
}