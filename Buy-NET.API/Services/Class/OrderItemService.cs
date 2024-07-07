using AutoMapper;
using Buy_NET.API.Contracts.OrderItem;
using Buy_NET.API.Domain.Models;
using Buy_NET.API.Repositories.Interfaces.OrderItemsRepositoryInterfaces;
using Buy_NET.API.Services.Interfaces.OrderItemServiceInterfaces;

namespace Buy_NET.API.Services.Class;

public class OrderItemService : IOrderItemServiceInterface
{
    private readonly IOrderItemsRepository _orderItemRepository;
    private readonly IMapper _mapper;

    public OrderItemService(IOrderItemsRepository orderItemRepository, IMapper mapper)
    {
        _orderItemRepository = orderItemRepository;
        _mapper = mapper;
    }

    public async Task<OrderItemResponseContract> Create(long orderId, OrderItemRequestContract model)
    {
        OrderItem order = _mapper.Map<OrderItem>(model);
        order.OrderId = orderId;
        order = await _orderItemRepository.Create(order);
        OrderItemResponseContract orderResponse =  _mapper.Map<OrderItemResponseContract>(order);
        orderResponse.ProductName = order.Product.Name;
        orderResponse.Price = order.Product.Price;

        return orderResponse;
    }

    public async Task<IEnumerable<OrderItemResponseContract>> Get()
    {
        var orderItems = await _orderItemRepository.Get();
        return orderItems.Select(o => _mapper.Map<OrderItemResponseContract>(o));
    }

    public async Task<OrderItemResponseContract> GetById(long id)
    {
        var order = await _orderItemRepository.GetById(id);
        if (order == null)
        {
            throw new KeyNotFoundException("Order não encontrada");
        }

        OrderItemResponseContract orderDto = _mapper.Map<OrderItemResponseContract>(order);
        orderDto.ProductName = order.Product.Name;
        orderDto.Price = order.Product.Price;
        return orderDto;
    }
}