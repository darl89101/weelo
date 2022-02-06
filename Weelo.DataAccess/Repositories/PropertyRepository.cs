using Microsoft.EntityFrameworkCore;
using Weelo.DataAccess.Repositories.Base;
using Weelo.Domain.Entities;
using Weelo.Domain.Interfaces.Entities;
using Weelo.Domain.Interfaces.Repositories;
using Weelo.Domain.Models;

namespace Weelo.DataAccess.Repositories
{
    public class PropertyRepository : GenericRepository<Property, long>, IPropertyRepository, Inject
    {
        private new readonly AppContext _context;
        public PropertyRepository(AppContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Property>> GetByFilterAsync(PropertyFilter filter)
            => await (from m in _context.Properties
                      where (string.IsNullOrEmpty(filter.Name) || m.Name.Contains(filter.Name))
                         && (string.IsNullOrEmpty(filter.Address) || m.Address.Contains(filter.Address))
                         && (!filter.Year.HasValue || m.Year == filter.Year.Value)
                         && (!filter.IdOwner.HasValue || m.IdOwner == filter.IdOwner.Value)
                      select m).Include("Owner").Include(o => o.PropertyImages.Where(x => x.Enabled)).ToListAsync();
    }
}
