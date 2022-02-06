using AutoMapper;
using Weelo.API.Models;
using Weelo.Domain.Entities;

namespace Weelo.API.Profiles
{
    public class UpdateModelProfile : Profile
    {
        public UpdateModelProfile()
        {
            CreateMap<UpdatePropertyModel, Property>();
        }
    }
}
