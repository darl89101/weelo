using Weelo.Domain.Entities;
using Weelo.Domain.Exceptions;
using Weelo.Domain.Interfaces.Entities;
using Weelo.Domain.Interfaces.Repositories;
using Weelo.Domain.Interfaces.Services;

namespace Weelo.Domain.Services
{
    /// <summary>
    /// OwnerService
    /// </summary>
    public class OwnerService : IOwnerService, Inject
    {
        #region Properties
        /// <summary>
        /// Owner repository
        /// </summary>
        private readonly IOwnerRepository _ownerRepository;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ownerRepository"></param>
        public OwnerService(IOwnerRepository ownerRepository)
        {
            _ownerRepository = ownerRepository;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Create a owner
        /// </summary>
        /// <param name="owner"></param>
        /// <returns></returns>
        public async Task<Owner> CreateAsync(Owner owner)
        {
            await ValidateToCreateAsync(owner);

            await _ownerRepository.AddAsync(owner);
            await _ownerRepository.UnitOfWork.CommitAsync();

            return owner;
        }

        /// <summary>
        /// Find owner by document number
        /// </summary>
        /// <param name="documentNumber"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task<Owner?> GetOwnerByDocumentAsync(string documentNumber)
            => _ownerRepository.FirstOrDefaultAsync(m => m.DocumentNumber == documentNumber, tracking: false);
        #endregion

        #region Privates
        /// <summary>
        /// Validations for owner entity
        /// </summary>
        /// <param name="owner"></param>
        /// <returns></returns>
        private async Task ValidateToCreateAsync(Owner owner)
        {
            var exists = await _ownerRepository.ExistsAsync(m => m.DocumentNumber == owner.DocumentNumber);

            if (exists) throw new BadRequestException($"Owner with document ({owner.DocumentNumber}) already exists.");
        }
        #endregion
    }
}
