using System.Security.Cryptography;
using System.Text;
using AutoMapper;
using Buy_NET.API.Contracts.User;
using Buy_NET.API.Domain.Models;
using Buy_NET.API.Repositories.Interfaces.UserRepositoryInterface;
using Buy_NET.API.Services.Interfaces.IUserService;

namespace Buy_NET.API.Services.Class;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public UserService(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public Task<UserLoginResponseContract> Authenticate(UserLoginRequestContract user)
    {
        throw new NotImplementedException();
    }

    public async Task<UserResponseContract> Create(UserRequestContract model, long idUser)
    {
        var user = _mapper.Map<User>(model);
        user.Password = CreatePasswordHash(user.Password);
        user.RegistrationDate = DateTime.Now;
        user = await _userRepository.Create(user);
        return _mapper.Map<UserResponseContract>(user);
    }

    public async Task Delete(long id, long idUser)
    {
        var user = await _userRepository.GetById(id) ?? throw new Exception("usuário não encontrado");
        await _userRepository.Delete(_mapper.Map<User>(user));

    }

    public async Task<IEnumerable<UserResponseContract>> Get(long idUser)
    {
        var users = await _userRepository.Get();
        return users.Select(user => _mapper.Map<UserResponseContract>(user));
    }

    public async Task<UserResponseContract> GetByEmail(string email)
    {
        var user = await _userRepository.GetByEmail(email);
        return _mapper.Map<UserResponseContract>(user);
    }

    public async Task<UserResponseContract> GetById(long idUser, long id)
    {
        var user = await _userRepository.GetById(id);
        return _mapper.Map<UserResponseContract>(user);
    }

    public async Task<UserResponseContract> Update(long id, UserRequestContract model, long idUser)
    {
        _ = await _userRepository.GetById(id) ?? throw new Exception("usuário não encontrado");

        var user = _mapper.Map<User>(model);
        user.Id = id;
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
            passwordHash = BitConverter.ToString(passwordHashBytes).ToLower();
        }

        return passwordHash;
    }
}