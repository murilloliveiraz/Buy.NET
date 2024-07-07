using AutoMapper;
using Buy_NET.API.Contracts.Order;
using Buy_NET.API.Contracts.OrderItem;
using Buy_NET.API.Domain.Models;
using Buy_NET.API.Repositories.Interfaces.OrderRepositoryInterfaces;
using Buy_NET.API.Services.Interfaces.OrderServiceInterfaces;

namespace Buy_NET.API.Services.Class;

public class OrderService : IOrderService
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

        var orderResponseList = orders.Select(o => 
        {
            var orderResponse = _mapper.Map<OrderResponseContract>(o);
            orderResponse.Total = o.Items.Sum(item => item.Quantity * item.Product.Price);
            orderResponse.Items = o.Items.Select(item => new OrderItemResponseContract
            {
                ProductId = item.ProductId,
                ProductName = item.Product.Name,
                Quantity = item.Quantity,
                Price = item.Product.Price
            }).ToList();
            return orderResponse;
        }).ToList();

        return orderResponseList;
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

        orderDto.Items = order.Items.Select(item => new OrderItemResponseContract
        {
            ProductId = item.ProductId,
            ProductName = item.Product.Name,
            Quantity = item.Quantity,
            Price = item.Product.Price
        }).ToList();

        orderDto.Total = total;
        return orderDto;
    }
}