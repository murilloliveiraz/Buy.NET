using AutoMapper;
using Buy_NET.API.Contracts.Product;
using Buy_NET.API.Domain.Models;

namespace Buy_NET.API.Mappers;

public class ProductProfile : Profile
{
    public ProductProfile()
    {
        CreateMap<Product, ProductRequestContract>().ReverseMap().ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));;
        CreateMap<Product, ProductResponseContract>().ReverseMap().ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));;
    }
}