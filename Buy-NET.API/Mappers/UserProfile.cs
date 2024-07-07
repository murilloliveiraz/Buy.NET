using AutoMapper;
using Buy_NET.API.Contracts.User;
using Buy_NET.API.Domain.Models;

namespace Buy_NET.API.Mappers;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User, UserRequestContract>().ReverseMap();
        CreateMap<User, UserUpdateRequestContract>().ReverseMap();
        CreateMap<User, UserResponseContract>().ReverseMap();
    }
}