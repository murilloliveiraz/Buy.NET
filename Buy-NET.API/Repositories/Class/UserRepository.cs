using Buy_NET.API.Data.Contexts;
using Buy_NET.API.Domain.Models;
using Buy_NET.API.Exceptions;
using Buy_NET.API.Repositories.Interfaces.UserRepositoryInterface;
using Microsoft.EntityFrameworkCore;

namespace Buy_NET.API.Repositories.Class;

public class UserRepository : IUserRepository
{
    private ApplicationContext _context;

    public UserRepository(ApplicationContext context)
    {
        _context = context;
    }

    public async Task<User> Create(User model)
    {
        await _context.User.AddAsync(model);
        await _context.SaveChangesAsync();

        return model;
    }

    public async Task Delete(User model)
    {
        _context.Entry(model).State = EntityState.Deleted;
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<User?>> Get()
    {
        return await _context.User.AsNoTracking().OrderBy(u => u.Id).ToListAsync() ?? throw new NotFoundException("Nenhum usuário encontrado");;
    }

    public async Task<User?> GetByEmail(string email)
    {
        return await _context.User.AsNoTracking().Where(u => u.Email == email).FirstOrDefaultAsync() ?? throw new NotFoundException("Nenhum usuário encontrado");;
    }

    public async Task<User?> GetById(long id)
    {
        return await _context.User.AsNoTracking().Where(u => u.Id == id).FirstOrDefaultAsync() ?? throw new NotFoundException("Nenhum usuário encontrado");;
    }

    public async Task<User> Update(User model)
    {
        User? entityAtDatabase = await _context.User.Where(u => u.Id == model.Id).FirstOrDefaultAsync();
        _context.Entry(entityAtDatabase).CurrentValues.SetValues(model);
        await _context.SaveChangesAsync();
        return entityAtDatabase;
    }
}