using Buy_NET.API.Domain.Models;

namespace Buy_NET.API.Repositories.Interfaces.OrderItemsRepositoryInterfaces;

public interface IOrderItemsRepository
{
    Task<IEnumerable<OrderItem?>> Get();
    Task<OrderItem?> GetById(long id);
    Task<OrderItem> Create(OrderItem model);
}