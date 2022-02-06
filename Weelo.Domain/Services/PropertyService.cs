using Weelo.Domain.Entities;
using Weelo.Domain.Exceptions;
using Weelo.Domain.Interfaces.Base;
using Weelo.Domain.Interfaces.Entities;
using Weelo.Domain.Interfaces.Repositories;
using Weelo.Domain.Interfaces.Services;
using Weelo.Domain.Models;

namespace Weelo.Domain.Services
{
    /// <summary>
    /// Service of properties
    /// </summary>
    public class PropertyService : IPropertyService, Inject
    {
        #region Properties
        /// <summary>
        /// Property repository
        /// </summary>
        private readonly IPropertyRepository _propertyRepository;
        /// <summary>
        /// Repository of property images
        /// </summary>
        private readonly IGenericRepository<PropertyImage, long> _propertyImageRepository;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public PropertyService(
            IPropertyRepository propertyRepository,
            IGenericRepository<PropertyImage, long> propertyImageRepository
        )
        {
            _propertyRepository = propertyRepository;
            _propertyImageRepository = propertyImageRepository;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Add a new property
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<Property> CreateAsync(Property property)
        {
            await ValidateToCreateAsync(property);

            property.CodeInternal = Guid.NewGuid().ToString();

            await _propertyRepository.AddAsync(property);
            await _propertyRepository.UnitOfWork.CommitAsync();

            return property;
        }

        /// <summary>
        /// Find property by id
        /// </summary>
        /// <param name="propertyId"></param>
        /// <returns></returns>
        public async Task<Property?> FindByIdAsync(long propertyId)
            => await _propertyRepository.FindByIdAsync(propertyId, tracking: false);

        /// <summary>
        /// Update a property
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        public async Task<Property> UpdateAsync(Property property)
        {
            await _propertyRepository.UpdateAsync(property);
            await _propertyRepository.UnitOfWork.CommitAsync();

            return property;
        }

        /// <summary>
        /// Change price of property
        /// </summary>
        /// <param name="propertyId"></param>
        /// <param name="price"></param>
        /// <returns></returns>
        /// <exception cref="NotFoundException"></exception>
        public async Task ChangePrice(long propertyId, decimal price)
        {
            var property = await _propertyRepository.FindByIdAsync(propertyId, tracking: false);

            if (property == null) throw new NotFoundException("Property not found");

            property.Price = price;

            await UpdateAsync(property);
        }

        /// <summary>
        /// Add files to property
        /// </summary>
        /// <param name="propertyId"></param>
        /// <param name="files"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task AddImagesAsync(long propertyId, List<string>? files)
        {
            await ValidateAddImagesAsync(propertyId, files);

            await _propertyImageRepository.AddRangeAsync(files!.Select(file => new PropertyImage
            {
                IdProperty = propertyId,
                File = System.Text.Encoding.UTF8.GetBytes(file),
                Enabled = true
            }).ToList());

            await _propertyImageRepository.UnitOfWork.CommitAsync();
        }

        /// <summary>
        /// Search properties by filter
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<IEnumerable<Property>> SearchByFilterAsync(PropertyFilter filter)
            => await _propertyRepository.GetByFilterAsync(filter);
        #endregion

        #region Privates
        /// <summary>
        /// Validate entity
        /// </summary>
        /// <param name="property"></param>
        private async Task ValidateToCreateAsync(Property property)
        {
            var findedProperty = await _propertyRepository
                .FirstOrDefaultAsync(m => m.Address == property.Address, tracking: false);

            if (findedProperty != null)
                throw new BadRequestException($"Property with address ({property.Address}) already exists.");

            if (property.Year > DateTime.Now.Year) throw new BadRequestException($"Year ({property.Year}) invalid.");

        }

        /// <summary>
        /// Validations of images
        /// </summary>
        /// <param name="propertyId"></param>
        /// <param name="files"></param>
        /// <returns></returns>
        /// <exception cref="NotFoundException"></exception>
        private async Task ValidateAddImagesAsync(long propertyId, List<string>? files)
        {
            var exists = await _propertyRepository.ExistsAsync(m => m.Id == propertyId);

            if (!exists) throw new NotFoundException($"Property with id ({propertyId}) does'nt exists.");

            if (files == null) throw new NotFoundException("Files is empty");
        }
        #endregion
    }
}
