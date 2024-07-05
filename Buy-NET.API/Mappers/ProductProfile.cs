using AutoMapper;
using Buy_NET.API.Contracts.Product;
using Buy_NET.API.Domain.Models;

namespace Buy_NET.API.Mappers;

public class ProductProfile : Profile
{
    public ProductProfile()
    {
        CreateMap<Product, ProductRequestContract>().ReverseMap();
        CreateMap<Product, ProductResponseContract>().ReverseMap();
    }
}