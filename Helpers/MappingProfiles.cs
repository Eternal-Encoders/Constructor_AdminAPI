using AutoMapper;
using Constructor_API.Models.DTOs.Create;
using Constructor_API.Models.DTOs.Read;
using Constructor_API.Models.Entities;
using Constructor_API.Models.Objects;

namespace Constructor_API.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles() 
        {
            CreateMap<CreateFloorDto, Floor>()
                .ForMember(x => x.CreatedAt, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(x => x.UpdatedAt, opt => opt.MapFrom(src => DateTime.Now));

            CreateMap<Floor, GetFloorDto>().ForMember(x => x.GraphPoints, opt => opt.Ignore());

            CreateMap<CreateBuildingDto, Building>()
                .ForMember(x => x.CreatedAt, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(x => x.UpdatedAt, opt => opt.MapFrom(src => DateTime.Now));

            CreateMap<CreateGraphPointFromFloorDto, GraphPoint>()
                .ForMember(x => x.CreatedAt, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(x => x.UpdatedAt, opt => opt.MapFrom(src => DateTime.Now));

            CreateMap<GraphPoint, CreateGraphPointFromFloorDto>();

            CreateMap<CreateGraphPointDto, GraphPoint>()
                .ForMember(x => x.CreatedAt, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(x => x.UpdatedAt, opt => opt.MapFrom(src => DateTime.Now));

            CreateMap<CreateProjectDto, Project>()
                .ForMember(x => x.CreatedAt, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(x => x.UpdatedAt, opt => opt.MapFrom(src => DateTime.Now));

            CreateMap<CreateProjectUserDto, ProjectUser>()
                .ForMember(x => x.AddedAt, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(x => x.UpdatedAt, opt => opt.MapFrom(src => DateTime.Now));

            CreateMap<CreateFloorsTransitionDto, FloorsTransition>()
                .ForMember(x => x.CreatedAt, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(x => x.UpdatedAt, opt => opt.MapFrom(src => DateTime.Now));

            CreateMap<User, GetUserDto>();

            //CreateMap<Project, GetProjectDto>();
        }
    }
}
