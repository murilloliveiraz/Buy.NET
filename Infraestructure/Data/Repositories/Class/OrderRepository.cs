using Buy_NET.API.Data.Contexts;
using Buy_NET.API.Domain.Models;
using Buy_NET.API.Domain.Exceptions;
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
        return await _context.Order
                            .AsNoTracking()
                            .Include(o => o.Items)
                            .ThenInclude(oi => oi.Product)
                            .OrderBy(o => o.Id)
                            .ToListAsync() ?? throw new NotFoundException("Nenhum pedido encontrado");;
    }

     public async Task<Order?> GetById(long id)
    {
        return await _context.Order
            .Include(o => o.Items)
                .ThenInclude(oi => oi.Product)
            .FirstOrDefaultAsync(o => o.Id == id) ?? throw new NotFoundException("Nenhum pedido encontrado");;
    }
    
    public async Task<IEnumerable<Order?>> GetByUserId(long id)
    {
        return await _context.Order
                            .AsNoTracking()
                            .Where(o => o.CustomerId == id)
                            .Include(o => o.Items)
                            .ThenInclude(oi => oi.Product)
                            .OrderBy(o => o.Id)
                            .ToListAsync() ?? throw new NotFoundException("Nenhum pedido encontrado");;
    }

    public async Task<Order?> GetByIdAndUserId(long id, long userId)
    {
        return await _context.Order
            .Include(o => o.Items)
                .ThenInclude(oi => oi.Product)
            .FirstOrDefaultAsync(o => o.Id == id && o.CustomerId == userId) ?? throw new NotFoundException("Nenhum pedido encontrado");;
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