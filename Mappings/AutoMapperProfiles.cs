using AutoMapper;
using ToDoList.Models.DTOs;
using ToDoListModel = ToDoList.Models.Domain.ToDoList;
using ToDoList.Models.Domain;

namespace ToDoList.Mappings;

public class AutoMapperProfiles : Profile
{
    public AutoMapperProfiles()
    {
        //create or update mappings
        CreateMap<ToDoListModel, CreateOrUpdateToDoListDto>().ReverseMap();
        CreateMap<ToDoTask, CreateOrUpdateToDoTaskDto>().ReverseMap();
        CreateMap<User, CreateOrUpdateUserDto>().ReverseMap();

        //read mappings
        CreateMap<ToDoListModel, ToDoListDto>().ReverseMap();
        CreateMap<ToDoTask, ToDoTaskDto>().ReverseMap();
        CreateMap<User, UserDto>().ReverseMap();
    }
}