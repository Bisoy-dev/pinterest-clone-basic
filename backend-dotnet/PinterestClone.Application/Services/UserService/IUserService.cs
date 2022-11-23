using PinterestClone.Infrastructure.Data.Models;
namespace PinterestClone.Application.Services.UserService;

public interface IUserService 
{
    Task<User> Create(User user);
    Task<bool> IsEmailUnique(string email);
    Task<User> FindById(string id);
    Task<User> FindByEmail(string email);
    Task<User> Update(string id, User user);
}