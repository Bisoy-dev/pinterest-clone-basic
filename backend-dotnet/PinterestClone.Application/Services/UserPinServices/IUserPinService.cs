using PinterestClone.Infrastructure.Data.Models;

namespace PinterestClone.Application.Services.UserPinServices;

public interface IUserPinService
{
    Task<UserPin> Create(UserPin userPin);
    Task<UserPin> Update(string userPinId, UserPin userPin);
    Task<UserPin> FindById(string userPinId);
    Task<UserPin> FindByUserId(string userId);

}