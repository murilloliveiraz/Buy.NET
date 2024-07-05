using Buy_NET.API.Data.Contexts;
using Buy_NET.API.Domain.Models;
using Buy_NET.API.Repositories.Interfaces.CategoryRepositoryInterface;
using Microsoft.EntityFrameworkCore;

namespace Buy_NET.API.Repositories.Class;

public class CategoryRepository : ICategoryRepository
{
    private ApplicationContext _context;

    public CategoryRepository(ApplicationContext context)
    {
        _context = context;
    }

    public async Task<Category> Create(Category model)
    {
        await _context.Category.AddAsync(model);
        await _context.SaveChangesAsync();

        return model;
    }

    public async Task Delete(Category model)
    {
        model.InactivationDate = DateTime.Now;
        await Update(model);
    }

    public async Task<IEnumerable<Category?>> Get()
    {
        return await _context.Category.AsNoTracking().OrderBy(c => c.Id).ToListAsync();
    }

    public async Task<Category?> GetById(long id)
    {
        return await _context.Category.AsNoTracking().Where(c => c.Id == id).FirstOrDefaultAsync();
    }
    
    public async Task<Category?> GetByName(string name)
    {
        return await _context.Category.AsNoTracking().Where(c => c.Name == name).FirstOrDefaultAsync();
    }

    public async Task<Category> Update(Category model)
    {
        Category? entityAtDatabase = await _context.Category.Where(c => c.Id == model.Id).FirstOrDefaultAsync();
        _context.Entry(entityAtDatabase).CurrentValues.SetValues(model);
        await _context.SaveChangesAsync();
        return entityAtDatabase;
    }
}