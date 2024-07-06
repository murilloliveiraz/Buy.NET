using Buy_NET.API.Data.Contexts;
using Buy_NET.API.Domain.Models;
using Buy_NET.API.Repositories.Interfaces.OrderRepositoryInterfaces;
using Microsoft.EntityFrameworkCore;

namespace Buy_NET.API.Repositories.Class;

public class OrderRepository : IOrderRepository
{
    private ApplicationContext _context;

    public OrderRepository(ApplicationContext context)
    {
        _context = context;
    }

    public async Task<Order> Create(Order model)
    {
        await _context.Order.AddAsync(model);
        await _context.SaveChangesAsync();

        return model;
    }

    public async Task Delete(Order model)
    {
        model.Status = "Cancelada";
        await Update(model);
    }

    public async Task<IEnumerable<Order?>> Get()
    {
        return await _context.Order.AsNoTracking().OrderBy(o => o.Id).ToListAsync();
    }

    public async Task<Order?> GetById(long id)
    {
        return await _context.Order.AsNoTracking().Where(o => o.Id == id).FirstOrDefaultAsync();
    }

    public async Task<Order> Update(Order model)
    {
        Order entityAtDatabase = await _context.Order.Where(o => o.Id == model.Id).FirstOrDefaultAsync();
        _context.Entry(entityAtDatabase).CurrentValues.SetValues(model);
        _context.Update<Order>(entityAtDatabase);
        await _context.SaveChangesAsync();
        return entityAtDatabase;
    }
}