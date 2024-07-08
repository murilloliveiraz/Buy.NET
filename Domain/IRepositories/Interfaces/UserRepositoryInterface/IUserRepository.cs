using Buy_NET.API.Domain.Models;

namespace Buy_NET.API.Repositories.Interfaces.UserRepositoryInterface;

public interface IUserRepository : IRepository<User, long>
{
    Task<User?> GetByEmail(string email);
}