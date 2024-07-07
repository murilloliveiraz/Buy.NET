using Buy_NET.API.Contracts.OrderItem;

namespace Buy_NET.API.Services.Interfaces.OrderItemServiceInterfaces;

public interface IOrderItemServiceInterface
{
    Task<IEnumerable<OrderItemResponseContract>> Get ();
    Task<OrderItemResponseContract> GetById (long id);
    Task<OrderItemResponseContract> Create (long id, OrderItemRequestContract model);
}