using Buy_NET.API.Contracts.Order;

namespace Buy_NET.API.Services.Interfaces.OrderServiceInterfaces;

public interface IOrderService
{
    Task<IEnumerable<OrderResponseContract>> Get ();
    Task<OrderResponseContract> GetById (long id);
    Task<IEnumerable<OrderResponseContract>> GetByUserId(long userId);
    Task<OrderResponseContract> GetByIdAndUserId(long id, long userId);

    Task<OrderResponseContract> Create (OrderRequestContract model);
    Task Delete (long id);
}