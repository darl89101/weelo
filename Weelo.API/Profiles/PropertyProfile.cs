using AutoMapper;
using Weelo.API.Models;
using Weelo.Domain.Entities;

namespace Weelo.API.Profiles
{
    /// <summary>
    /// Property profile
    /// </summary>
    public class PropertyProfile : Profile
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public PropertyProfile()
        {
            CreateMap<PropertyModel, Property>()
                .ForMember(m => m.Owner, s => s.Ignore())
                ;
            CreateMap<Property, PropertyModel>()
                ;
            CreateMap<Property, PropertyWithImageModel>()
                .ForMember(m => m.Images, s => s.MapFrom(m => m.PropertyImages
                    .Select(o => System.Text.Encoding.UTF8.GetString(o.File)).ToList()))
                ;
        }
    }
}
