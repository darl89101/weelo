using Weelo.Domain.Entities;
using Weelo.Domain.Interfaces.Base;
using Weelo.Domain.Interfaces.Entities;
using Weelo.Domain.Interfaces.Services;

namespace Weelo.Domain.Services
{
    /// <summary>
    /// Property trace service
    /// </summary>
    public class PropertyTraceService : IPropertyTraceService, Inject
    {
        /// <summary>
        /// Repository of property trace
        /// </summary>
        private readonly IGenericRepository<PropertyTrace, long> _propertyTraceRepository;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="propertyTraceRepository"></param>
        public PropertyTraceService(IGenericRepository<PropertyTrace, long> propertyTraceRepository)
        {
            _propertyTraceRepository = propertyTraceRepository;
        }

        /// <summary>
        /// Add an entity
        /// </summary>
        /// <param name="propertyTrace"></param>
        /// <returns></returns>
        public async Task<PropertyTrace> AddAsync(PropertyTrace propertyTrace)
        {
            await _propertyTraceRepository.AddAsync(propertyTrace);
            await _propertyTraceRepository.UnitOfWork.CommitAsync();

            return propertyTrace;
        }

        /// <summary>
        /// Find property trace by id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        public Task<PropertyTrace?> FindByIdAsync(long id, string[]? includes = null)
            =>  _propertyTraceRepository.FindByIdAsync(id, includes: includes);
    }
}
