using AutoMapper;
using Buy_NET.API.Contracts.Category;
using Buy_NET.API.Domain.Models;

namespace Buy_NET.API.Mappers;

public class CategoryProfile : Profile
{
    public CategoryProfile()
    {
        CreateMap<Category, CategoryRequestContract>().ReverseMap();
        CreateMap<Category, CategoryResponseContract>().ReverseMap();
    }
}