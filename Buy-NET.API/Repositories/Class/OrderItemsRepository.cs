using Buy_NET.API.Data.Contexts;
using Buy_NET.API.Domain.Models;
using Buy_NET.API.Repositories.Interfaces.OrderItemsRepositoryInterfaces;
using Microsoft.EntityFrameworkCore;

namespace Buy_NET.API.Repositories.Class;

public class OrderItemsRepository : IOrderItemsRepository
{
    private ApplicationContext _context;

    public OrderItemsRepository(ApplicationContext context)
    {
        _context = context;
    }

    public async Task<OrderItem> Create(OrderItem model)
    {
        await _context.OrderItem.AddAsync(model);
        await _context.SaveChangesAsync();

        return model;
    }

    public async Task<IEnumerable<OrderItem?>> Get()
    {
        return await _context.OrderItem.AsNoTracking().OrderBy(o => o.Id).Include(o => o.Product).ToListAsync();
    }

    public async Task<OrderItem?> GetById(long id)
    {
        return await _context.OrderItem.AsNoTracking().Where(o => o.Id == id).Include(o => o.Product).FirstOrDefaultAsync();
    }
}