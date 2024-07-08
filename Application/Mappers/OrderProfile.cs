using AutoMapper;
using Buy_NET.API.Contracts.Order;
using Buy_NET.API.Domain.Models;

namespace Buy_NET.API.Mappers;

public class OrderProfile : Profile
{
    public OrderProfile()
    {
        CreateMap<Order, OrderRequestContract>().ReverseMap();
        CreateMap<Order, OrderResponseContract>().ReverseMap();
    }
}