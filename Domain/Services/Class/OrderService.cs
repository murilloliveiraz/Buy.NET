using AutoMapper;
using Buy_NET.API.Contracts.Order;
using Buy_NET.API.Contracts.OrderItem;
using Buy_NET.API.Domain.Exceptions;
using Buy_NET.API.Domain.Models;
using Buy_NET.API.Repositories.Interfaces.OrderRepositoryInterfaces;
using Buy_NET.API.Repositories.Interfaces.ProductRepositoryInterface;
using Buy_NET.API.Services.Interfaces.OrderServiceInterfaces;

namespace Buy_NET.API.Services.Class;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;

    public OrderService(IOrderRepository orderRepository, IMapper mapper)
    {
        _orderRepository = orderRepository;
        _mapper = mapper;
    }

    public async Task<OrderResponseContract> Create(OrderRequestContract model)
    {
        if (model is null)
        {
            throw new BadRequestException("O pedido não pode ser nulo");
        }
        Order order = _mapper.Map<Order>(model);
        order.OrderDate = DateTime.Now;
        order.Status = "Pendente";

        order = await _orderRepository.Create(order);

        return _mapper.Map<OrderResponseContract>(order);
    }

    public async Task Delete(long id)
    {
        Order order = await _orderRepository.GetById(id);
        if (order is null)
        {
            throw new NotFoundException("Pedido não encontrado");
        }
        await _orderRepository.Delete(order);
    }

    public async Task<IEnumerable<OrderResponseContract>> Get()
    {
        var orders = await _orderRepository.Get();
        if (orders is null)
        {
            throw new NotFoundException("Nenhum pedido encontrado");
        }

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
        if (order is null)
        {
            throw new NotFoundException("Pedido não encontrado");
        }
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

    public async Task<IEnumerable<OrderResponseContract>> GetByUserId(long userId)
    {
        var orders = await _orderRepository.GetByUserId(userId);
        if (orders is null)
        {
            throw new NotFoundException("Nenhum pedido não encontrado");
        }
        return orders.Select(o => _mapper.Map<OrderResponseContract>(o));
    }

    public async Task<OrderResponseContract> GetByIdAndUserId(long id, long userId)
    {
        var order = await _orderRepository.GetByIdAndUserId(id, userId);
        if (order is null)
        {
            throw new NotFoundException("Pedido não encontrado");
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