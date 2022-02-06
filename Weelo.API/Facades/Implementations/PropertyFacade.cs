using AutoMapper;
using Weelo.API.Abstract;
using Weelo.API.Facades.Interfaces;
using Weelo.API.Models;
using Weelo.Domain.Entities;
using Weelo.Domain.Exceptions;
using Weelo.Domain.Interfaces.Entities;
using Weelo.Domain.Interfaces.Services;
using Weelo.Domain.Models;

namespace Weelo.API.Facades.Implementations
{
    /// <summary>
    /// Facade of property
    /// </summary>
    public class PropertyFacade : IPropertyFacade, Inject
    {
        #region Properties
        /// <summary>
        /// Automapper intance
        /// </summary>
        private readonly IMapper _mapper;
        /// <summary>
        /// Service of owners
        /// </summary>
        private readonly IOwnerService _ownerService;
        /// <summary>
        /// Configuration file
        /// </summary>
        private readonly IConfiguration _configuration;
        /// <summary>
        /// Service of property
        /// </summary>
        private readonly IPropertyService _propertyService;
        /// <summary>
        /// Property trace service
        /// </summary>
        private readonly IPropertyTraceService _propertyTraceService;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="mapper"></param>
        /// <param name="ownerService"></param>
        /// <param name="configuration"></param>
        /// <param name="propertyService"></param>
        /// <param name="propertyTraceService"></param>
        public PropertyFacade(
            IMapper mapper,
            IOwnerService ownerService,
            IConfiguration configuration,
            IPropertyService propertyService,
            IPropertyTraceService propertyTraceService
        )
        {
            _mapper = mapper;
            _ownerService = ownerService;
            _configuration = configuration;
            _propertyService = propertyService;
            _propertyTraceService = propertyTraceService;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Create a property
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<PropertyModel> CreateAsync(PropertyModel model)
        {
            var owner = await _ownerService.GetOwnerByDocumentAsync(model.Owner.DocumentNumber);

            if (owner == null) owner = await _ownerService.CreateAsync(_mapper.To<Owner>(model.Owner));

            var propertyToCreate = _mapper.To<Property>(model);
            propertyToCreate.IdOwner = owner.Id;
            var property = await _propertyService.CreateAsync(propertyToCreate);
            property.Owner = owner;

            return _mapper.To<PropertyModel>(property);
        }

        /// <summary>
        /// Run a sale
        /// </summary>
        /// <param name="propertyId"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<PropertyTraceModel> SaleAsync(long propertyId, SaleModel model)
        {
            var property = await _propertyService.FindByIdAsync(propertyId);

            if (property == null) throw new NotFoundException("Property not found");

            var tax = _configuration.GetValue<decimal>("Tax");
            var propertyTrace = new PropertyTrace
            {
                IdProperty = propertyId,
                DateSale = DateTime.Now,
                Name = model.Name,
                Value = model.Price,
                Tax = model.Price * tax
            };
            await _propertyTraceService.AddAsync(propertyTrace);
            await _propertyService.ChangePrice(propertyId, model.Price);
            var created = await _propertyTraceService.FindByIdAsync(propertyTrace.Id, new[] { "Property.Owner" });
            
            return _mapper.To<PropertyTraceModel>(created!);
        }

        /// <summary>
        /// Add an image to property
        /// </summary>
        /// <param name="propertyId"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task AddImageAsync(long propertyId, PropertyImageModel model)
            => await _propertyService.AddImagesAsync(propertyId, model.Files);

        /// <summary>
        /// Change price of property
        /// </summary>
        /// <param name="propertyId"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task ChangePrice(long propertyId, ChangePriceModel model)
            => await _propertyService.ChangePrice(propertyId, model.Price);

        /// <summary>
        /// Update a property
        /// </summary>
        /// <param name="propertyId"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<UpdatePropertyModel> UpdateAsync(long propertyId, UpdatePropertyModel model)
        {
            var property = await _propertyService.FindByIdAsync(propertyId);

            if (property == null) throw new NotFoundException("property not found");

            property = await _propertyService.UpdateAsync(_mapper.Map(model, property));

            return _mapper.To<UpdatePropertyModel>(property);
        }

        /// <summary>
        /// /List all properties by filter
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<IEnumerable<PropertyWithImageModel>?> GetPropertiesByFilterAsync(PropertyFilter filter)
        {
            var res = await _propertyService.SearchByFilterAsync(filter);
            return _mapper.To<IEnumerable<PropertyWithImageModel>>(res);
        }
        #endregion

        #region Privates

        #endregion
    }
}
