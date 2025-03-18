using AutoMapper;
using Constructor_API.Models.DTOs;
using Constructor_API.Models.Entities;

namespace Constructor_API.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles() 
        {
            CreateMap<CreateFloorDto, Floor>().ForMember(x => x.Rooms, opt => opt.Ignore());
            CreateMap<Floor, GetFloorDto>();
            CreateMap<CreateBuildingDto, Building>();
            CreateMap<GraphPointFromFloorDto, GraphPoint>();
            CreateMap<CreateProjectDto, Project>();
        }
    }
}
