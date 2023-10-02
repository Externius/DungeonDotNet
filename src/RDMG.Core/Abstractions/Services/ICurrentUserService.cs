namespace RDMG.Core.Abstractions.Services;
public interface ICurrentUserService
{
    int GetUserIdAsInt();
    string UserId { get; }
    string UserName { get; }
}