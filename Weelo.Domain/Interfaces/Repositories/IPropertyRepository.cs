using Weelo.Domain.Entities;
using Weelo.Domain.Interfaces.Base;
using Weelo.Domain.Models;

namespace Weelo.Domain.Interfaces.Repositories
{
    public interface IPropertyRepository : IGenericRepository<Property, long>
    {
        Task<IEnumerable<Property>> GetByFilterAsync(PropertyFilter filter);
    }
}
