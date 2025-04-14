using AutoMapper;
using miniCRM_back.DTOs;
using miniCRM_back.Models;
using System.Net.Sockets;

namespace miniCRM_back.Configs {
    public class MapperProfile : Profile {
        public MapperProfile() {
            CreateMap<TaskItem, TaskItemDto>().ReverseMap();
            CreateMap<TaskItem, TaskItemForCreationDto>().ReverseMap();
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<User, UserForCreationDto>().ReverseMap();
            CreateMap<User, UserForUpdateDto>().ReverseMap();
        }
    }
}
