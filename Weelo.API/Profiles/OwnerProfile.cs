using AutoMapper;
using Weelo.API.Models;
using Weelo.Domain.Abstract;
using Weelo.Domain.Entities;

namespace Weelo.API.Profiles
{
    /// <summary>
    /// Owner profile
    /// </summary>
    public class OwnerProfile : Profile
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public OwnerProfile()
        {
            CreateMap<Owner, OwnerModel>()
                .ForMember(m => m.Photo, s => s.MapFrom(m => m.Photo == null ? "" : System.Text.Encoding.UTF8.GetString(m.Photo)))
                ;
            CreateMap<OwnerModel, Owner>()
                .ForMember(m => m.Photo, s => s.MapFrom(m => m.Photo.IsNullOrEmpty() ? default : System.Text.Encoding.UTF8.GetBytes(m.Photo)))
                ;
        }
    }
}
