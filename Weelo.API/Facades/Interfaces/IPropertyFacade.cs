using Weelo.API.Models;
using Weelo.Domain.Models;

namespace Weelo.API.Facades.Interfaces
{
    /// <summary>
    /// Interface of property facade
    /// </summary>
    public interface IPropertyFacade
    {
        /// <summary>
        /// Run a sale
        /// </summary>
        /// <param name="propertyId"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<PropertyTraceModel> SaleAsync(long propertyId, SaleModel model);
        /// <summary>
        /// Create a propery and owner
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<PropertyModel> CreateAsync(PropertyModel model);
        /// <summary>
        /// Add a list of images
        /// </summary>
        /// <param name="propertyId"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        Task AddImageAsync(long propertyId, PropertyImageModel model);
        /// <summary>
        /// Change price of property
        /// </summary>
        /// <param name="propertyId"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        Task ChangePrice(long propertyId, ChangePriceModel model);
        /// <summary>
        /// Update a property
        /// </summary>
        /// <param name="propertyId"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<UpdatePropertyModel> UpdateAsync(long propertyId, UpdatePropertyModel model);
        /// <summary>
        /// List all properties by filter
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        Task<IEnumerable<PropertyWithImageModel>?> GetPropertiesByFilterAsync(PropertyFilter filter);
    }
}
