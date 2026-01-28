using UserMgr.Domain.Entities;
using UserMgr.Domain.Events;

namespace UserMgr.Domain;

public interface IUserRepository
{
    public Task<User?> FindOneAsync(PhoneNumber phoneNumber);
    public Task<User?> FindOneAsync(Guid id);
    public Task AddNewLoginHistoryAsync(PhoneNumber phoneNumber, string message);
    public Task SavePhoneNumberCodeAsync(PhoneNumber phoneNumber, string code);
    public Task<string?> FindPhoneNumberCodeAsync(PhoneNumber phoneNumber);
    public Task PublishEventAsync(UserAccessResultEvent accessResultEvent);
}