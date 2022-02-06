using Weelo.Domain.Entities;

namespace Weelo.Domain.Interfaces.Services
{
    public interface IPropertyTraceService
    {
        Task<PropertyTrace> AddAsync(PropertyTrace propertyTrace);
        Task<PropertyTrace?> FindByIdAsync(long id, string[]? includes = null);
    }
}
