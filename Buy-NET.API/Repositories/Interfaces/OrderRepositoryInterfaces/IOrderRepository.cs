using Buy_NET.API.Domain.Models;

namespace Buy_NET.API.Repositories.Interfaces.OrderRepositoryInterfaces;

public interface IOrderRepository : IRepository<Order, long>
{
    Task<IEnumerable<Order?>> GetByUserId(long id);
    Task<Order?> GetByIdAndUserId(long id, long userId);
}