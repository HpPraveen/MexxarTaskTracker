using AutoMapper;
using MexxarTaskTracker.Domain;

namespace MexxarTaskTracker.Api
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            CreateMap<Domain.UserTask, UserTaskDto>().ReverseMap();
            CreateMap<ToDoList, ToDoListDto>().ReverseMap();

        }
    }
}