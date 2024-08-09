using AutoMapper;
using ToDoApp_API.DTOs;
using Task = ToDoApp_API.Models.Task;

namespace ToDoApp_API.Helpers;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<TaskCreationDTO, Task>();
    }
}