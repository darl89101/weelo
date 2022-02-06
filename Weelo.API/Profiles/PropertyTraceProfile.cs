using AutoMapper;
using Weelo.API.Models;
using Weelo.Domain.Entities;

namespace Weelo.API.Profiles
{
    public class PropertyTraceProfile : Profile
    {
        public PropertyTraceProfile()
        {
            CreateMap<PropertyTrace, PropertyTraceModel>()
                .ForMember(m => m.Price, s => s.MapFrom(m => m.Value))
                .ForMember(m => m.Property, s => s.MapFrom(m => m.Property))
                ;
        }
    }
}
