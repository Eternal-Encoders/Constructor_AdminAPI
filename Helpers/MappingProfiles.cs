using AutoMapper;
using Constructor_API.Models.DTOs.Create;
using Constructor_API.Models.DTOs.Read;
using Constructor_API.Models.Entities;

namespace Constructor_API.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles() 
        {
            CreateMap<CreateFloorDto, Floor>()
                .ForMember(x => x.CreatedAt, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(x => x.UpdatedAt, opt => opt.MapFrom(src => DateTime.Now));

            CreateMap<Floor, GetFloorDto>();

            CreateMap<CreateBuildingDto, Building>()
                .ForMember(x => x.CreatedAt, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(x => x.UpdatedAt, opt => opt.MapFrom(src => DateTime.Now));

            CreateMap<CreateGraphPointFromFloorDto, GraphPoint>()
                .ForMember(x => x.CreatedAt, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(x => x.UpdatedAt, opt => opt.MapFrom(src => DateTime.Now));

            CreateMap<CreateGraphPointDto, GraphPoint>()
                .ForMember(x => x.CreatedAt, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(x => x.UpdatedAt, opt => opt.MapFrom(src => DateTime.Now));

            CreateMap<CreateProjectDto, Project>()
                .ForMember(x => x.CreatedAt, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(x => x.UpdatedAt, opt => opt.MapFrom(src => DateTime.Now));

            //CreateMap<CreateRoomDto, Room>();
        }
    }
}
