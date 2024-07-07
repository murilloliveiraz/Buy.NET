using AutoMapper;
using Buy_NET.API.Contracts.Order;
using Buy_NET.API.Contracts.OrderItem;
using Buy_NET.API.Domain.Models;
using Buy_NET.API.Repositories.Interfaces.OrderItemsRepositoryInterfaces;
using Buy_NET.API.Repositories.Interfaces.OrderRepositoryInterfaces;
using Buy_NET.API.Repositories.Interfaces.ProductRepositoryInterface;
using Buy_NET.API.Services.Interfaces.OrderItemServiceInterfaces;
using Buy_NET.API.Services.Interfaces.OrderServiceInterfaces;

namespace Buy_NET.API.Services.Class;

public class OrderService : IOrderServiceInterface
{
    private readonly IOrderRepository _orderRepository;
    private readonly IOrderItemServiceInterface _orderItemService;
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;

    public OrderService(IOrderRepository orderRepository, IMapper mapper, IProductRepository productRepository, IOrderItemServiceInterface orderItemService)
    {
        _orderRepository = orderRepository;
        _mapper = mapper;
        _productRepository = productRepository;
        _orderItemService = orderItemService;
    }

    public async Task<OrderResponseContract> Create(OrderRequestContract model)
    {
        Order order = _mapper.Map<Order>(model);
        order.OrderDate = DateTime.Now;
        order.Status = "Pendente";

        foreach (var item in model.Items)
        {
            var product = await _productRepository.GetById(item.ProductId);
            if (product == null)
            {
                throw new ArgumentException($"Produto com ID {item.ProductId} não encontrado.");
            }

            OrderItemRequestContract orderItem = new OrderItemRequestContract
            {
                ProductId = item.ProductId,
                Quantity = item.Quantity
            };

            await _orderItemService.Create(order.Id,orderItem);
        }

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