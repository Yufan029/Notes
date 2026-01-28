using MediatR;

namespace UserMgr.Domain.Events;

public record class UserAccessResultEvent(PhoneNumber PhoneNumber, UserAccessResult Result) : INotification;