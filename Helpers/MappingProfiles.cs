using AutoMapper;
using ConstructorAdminAPI.Models.DTOs;
using ConstructorAdminAPI.Models.Entities;

namespace ConstructorAdminAPI.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles() 
        {
            CreateMap<CreateFloorDto, Floor>().ForMember(x => x.Rooms, opt => opt.Ignore());
            //CreateMap<CreateGraphPointDto, GraphPoint>();
            //CreateMap<CreateStairDto, Stair>();
        }
    }
}
