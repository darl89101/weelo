using Weelo.Domain.Entities;
using Weelo.Domain.Models;

namespace Weelo.Domain.Interfaces.Services
{
    public interface IPropertyService
    {
        Task<Property> CreateAsync(Property property);
        Task AddImagesAsync(long propertyId, List<string>? files);
        Task<Property> UpdateAsync(Property property);
        Task ChangePrice(long propertyId, decimal price);
        Task<Property?> FindByIdAsync(long propertyId);
        Task<IEnumerable<Property>> SearchByFilterAsync(PropertyFilter filter);
    }
}
