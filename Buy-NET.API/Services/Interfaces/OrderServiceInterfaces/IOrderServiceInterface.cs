using Buy_NET.API.Contracts.Order;

namespace Buy_NET.API.Services.Interfaces.OrderServiceInterfaces;

public interface IOrderServiceInterface
{
    Task<IEnumerable<OrderResponseContract>> Get ();
    Task<OrderResponseContract> GetById (long id);
    Task<OrderResponseContract> Create (OrderRequestContract model);
    Task Delete (long id);
}