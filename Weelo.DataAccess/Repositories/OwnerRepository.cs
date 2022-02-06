using Weelo.DataAccess.Repositories.Base;
using Weelo.Domain.Entities;
using Weelo.Domain.Interfaces.Entities;
using Weelo.Domain.Interfaces.Repositories;

namespace Weelo.DataAccess.Repositories
{
    public class OwnerRepository : GenericRepository<Owner, long>, IOwnerRepository, Inject
    {
        public OwnerRepository(AppContext context) : base(context)
        {
        }
    }
}
