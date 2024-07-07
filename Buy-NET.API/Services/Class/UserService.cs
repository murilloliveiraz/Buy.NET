using System.Security.Authentication;
using System.Security.Cryptography;
using System.Text;
using AutoMapper;
using Buy_NET.API.Contracts.User;
using Buy_NET.API.Domain.Models;
using Buy_NET.API.Repositories.Interfaces.UserRepositoryInterface;
using Buy_NET.API.Services.Interfaces.UserService;

namespace Buy_NET.API.Services.Class;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly TokenService _tokenService;

    public UserService(IUserRepository userRepository, IMapper mapper, TokenService tokenService)
    {
        _userRepository = userRepository;
        _mapper = mapper;
        _tokenService = tokenService;
    }

    public async Task<UserLoginResponseContract> Authenticate(UserLoginRequestContract userRequest)
    {
        UserResponseContract user = await GetByEmail(userRequest.Email);
        var passwordHash = CreatePasswordHash(userRequest.Password);

        if (user is null || user.Password != passwordHash)
        {
            throw new AuthenticationException("Usuário ou senha inválida");
        }

        return new UserLoginResponseContract{
            Id = user.Id,
            Email = user.Email,
            Role = user.Role,
            Token = _tokenService.GenerateToken(_mapper.Map<User>(user)),
        };
    }

    public async Task<UserResponseContract> Create(UserRequestContract model)
    {
        var user = _mapper.Map<User>(model);
        user.Password = CreatePasswordHash(user.Password);
        user.RegistrationDate = DateTime.Now;
        user.Role ??= "Customer";
        user = await _userRepository.Create(user);
        return _mapper.Map<UserResponseContract>(user);
    }

    public async Task Delete(long id)
    {
        var user = await _userRepository.GetById(id) ?? throw new Exception("usuário não encontrado");
        await _userRepository.Delete(_mapper.Map<User>(user));

    }

    public async Task<IEnumerable<UserResponseContract>> Get()
    {
        var users = await _userRepository.Get();
        return users.Select(user => _mapper.Map<UserResponseContract>(user));
    }

    public async Task<UserResponseContract> GetByEmail(string email)
    {
        var user = await _userRepository.GetByEmail(email);
        return _mapper.Map<UserResponseContract>(user);
    }

    public async Task<UserResponseContract> GetById(long id)
    {
        var user = await _userRepository.GetById(id);
        return _mapper.Map<UserResponseContract>(user);
    }

    public async Task<UserResponseContract> Update(long id, UserUpdateRequestContract model)
    {
        var userAtdatabase = await _userRepository.GetById(id) ?? throw new Exception("usuário não encontrado");

        var user = _mapper.Map<User>(model);
        user.Id = id;
        user.Role = userAtdatabase.Role;
        user.Password = CreatePasswordHash(model.Password);

        user = await _userRepository.Update(user);
        return _mapper.Map<UserResponseContract>(user);
    }

    private string CreatePasswordHash(string password)
    {
        string passwordHash;

        using (SHA256 sha256 = SHA256.Create())
        {
            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
            byte[] passwordHashBytes = sha256.ComputeHash(passwordBytes);
            passwordHash = BitConverter.ToString(passwordHashBytes).Replace("-", "").ToLower();
        }

        return passwordHash;
    }
}