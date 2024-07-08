using Buy_NET.API.Contracts.User;

namespace Buy_NET.API.Services.Interfaces.UserService;

public interface IUserService
{
    Task<IEnumerable<UserResponseContract>> Get ();
    Task<UserResponseContract> GetById (long id);
    Task<UserResponseContract> Create (UserRequestContract model);
    Task<UserResponseContract> Update (long id, UserUpdateRequestContract model);
    Task Delete (long id);
    Task<UserLoginResponseContract> Authenticate(UserLoginRequestContract userRequest);    
    Task<UserResponseContract> GetByEmail(string email);       
}