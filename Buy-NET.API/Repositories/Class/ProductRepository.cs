using Buy_NET.API.Data.Contexts;
using Buy_NET.API.Domain.Models;
using Buy_NET.API.Exceptions;
using Buy_NET.API.Repositories.Interfaces.ProductRepositoryInterface;
using Microsoft.EntityFrameworkCore;

namespace Buy_NET.API.Repositories.Class;

public class ProductRepository : IProductRepository
{
    private ApplicationContext _context;

    public ProductRepository(ApplicationContext context)
    {
        _context = context;
    }

    public async Task<Product> Create(Product model)
    {
        await _context.Product.AddAsync(model);
        await _context.SaveChangesAsync();

        return model;
    }

    public async Task Delete(Product model)
    {
        model.InactivationDate = DateTime.Now;
        await Update(model);
    }

    public async Task<IEnumerable<Product?>> Get()
    {
        return await _context.Product.AsNoTracking().OrderBy(c => c.Id).ToListAsync() ?? throw new NotFoundException("Nenhum produto encontrado");;
    }

    public async Task<Product?> GetById(long id)
    {
        return await _context.Product.AsNoTracking().Where(c => c.Id == id).FirstOrDefaultAsync() ?? throw new NotFoundException("Nenhum produto encontrado");;
    }

    public async Task<Product?> GetByName(string name)
    {
        return await _context.Product.AsNoTracking().Where(c => c.Name == name).FirstOrDefaultAsync() ?? throw new NotFoundException("Nenhum produto encontrado");;
    }

    public async Task<Product> Update(Product model)
    {
        Product entityAtDatabase = await _context.Product.Where(c => c.Id == model.Id).FirstOrDefaultAsync();
        _context.Entry(entityAtDatabase).CurrentValues.SetValues(model);
        _context.Update<Product>(entityAtDatabase);
        await _context.SaveChangesAsync();
        return entityAtDatabase;
    }
}