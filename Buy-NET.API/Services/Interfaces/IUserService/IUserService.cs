using Buy_NET.API.Contracts.User;

namespace Buy_NET.API.Services.Interfaces.IUserService;

public interface IUserService : IService<UserRequestContract, UserResponseContract, long>
{
    Task<UserLoginResponseContract> Authenticate(UserLoginRequestContract user);    
}