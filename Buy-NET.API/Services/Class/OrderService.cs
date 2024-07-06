using AutoMapper;
using Buy_NET.API.Contracts.Order;
using Buy_NET.API.Domain.Models;
using Buy_NET.API.Repositories.Interfaces.OrderRepositoryInterfaces;
using Buy_NET.API.Services.Interfaces.OrderServiceInterfaces;

namespace Buy_NET.API.Services.Class;

public class OrderService : IOrderServiceInterface
{
    private readonly IOrderRepository _orderRepository;
    private readonly IMapper _mapper;

    public OrderService(IOrderRepository orderRepository, IMapper mapper)
    {
        _orderRepository = orderRepository;
        _mapper = mapper;
    }

    public async Task<OrderResponseContract> Create(OrderRequestContract model)
    {
        Order order = _mapper.Map<Order>(model);
        order.OrderDate = DateTime.Now;
        order.Status = "Pendente";
        order = await _orderRepository.Create(order);
        return _mapper.Map<OrderResponseContract>(order);
    }

    public async Task Delete(long id)
    {
        Order order = await _orderRepository.GetById(id);
        await _orderRepository.Delete(order);
    }

    public async Task<IEnumerable<OrderResponseContract>> Get()
    {
        var orders = await _orderRepository.Get();
        return orders.Select(o => _mapper.Map<OrderResponseContract>(o));
    }

    public async Task<OrderResponseContract> GetById(long id)
    {
        var order = await _orderRepository.GetById(id);
        if (order == null)
        {
            throw new KeyNotFoundException("Order não encontrada");
        }
        double total = order.Items.Sum(item => item.Quantity * item.Product.Price);

        var orderDto = _mapper.Map<OrderResponseContract>(order);
        orderDto.Total = total;
        return orderDto;
    }
}