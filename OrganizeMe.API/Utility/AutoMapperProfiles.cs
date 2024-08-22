using AutoMapper;
using OrganizeMe.API.Models;
using OrganizeMe.API.Models.Dto;

namespace OrganizeMe.API.Utility;

public class AutoMapperProfiles : Profile
{
    public AutoMapperProfiles()
    {
        CreateMap<User, UserDto>().ReverseMap();
        CreateMap<TodoItem, TodoItemDto>().ReverseMap();
    }
}