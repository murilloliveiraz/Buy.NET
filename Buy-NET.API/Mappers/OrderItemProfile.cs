using AutoMapper;
using Buy_NET.API.Contracts.OrderItem;
using Buy_NET.API.Domain.Models;

namespace Buy_NET.API.Mappers;

public class OrderItemProfile : Profile
{
    public OrderItemProfile()
    {
        CreateMap<OrderItem, OrderItemRequestContract>().ReverseMap();
        CreateMap<OrderItem, OrderItemResponseContract>().ReverseMap();
    }
}