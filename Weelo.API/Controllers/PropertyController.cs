using Microsoft.AspNetCore.Mvc;
using Weelo.API.Facades.Interfaces;
using Weelo.API.Models;
using Weelo.Domain.Attributes;
using Weelo.Domain.Models;

namespace Weelo.API.Controllers
{
    /// <summary>
    /// Property controller
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class PropertyController : ControllerBase
    {
        #region Properties        
        /// <summary>
        /// Property service instance
        /// </summary>
        private readonly IPropertyFacade _facade;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="facade"></param>
        public PropertyController(
            IPropertyFacade facade
        ) => _facade = facade;
        #endregion

        #region Methods
        /// <summary>
        /// Run a property sale
        /// </summary>        
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST /1
        ///     {
        ///         "name": "Maria Paula Perdomo",
        ///         "price": 45.2
        ///     }
        /// 
        /// </remarks>
        /// <param name="propertyId"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("sale/{propertyId}"), RequestValidation, UseTransaction]
        public async Task<IActionResult> SaleAsync(long propertyId, [FromBody] SaleModel model)
            => Ok(await _facade.SaleAsync(propertyId, model));


        /// <summary>
        /// Create a new propery and owner
        /// </summary>        
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST /
        ///     {
        ///         "name": "House model 1",
        ///         "address": "Town street",
        ///         "price": 45.2,
        ///         "year": 2022,
        ///         "owner": {
        ///             "documentNumber": "1075245401",
        ///             "name": "Diego Roldán",
        ///             "address": "Cra 25",
        ///             "photo": "",
        ///             "birthDay": "1989-09-04"
        ///         }
        ///     }
        /// 
        /// </remarks>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost, RequestValidation, UseTransaction]
        public async Task<IActionResult> CreateAsync([FromBody] PropertyModel model)
            => Ok(await _facade.CreateAsync(model));

        /// <summary>
        /// Update a property
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     PUT /1
        ///     {
        ///         "name": "House model 1",
        ///         "address": "Town street",
        ///         "year": 2022
        ///     }
        /// 
        /// </remarks>
        /// <returns></returns>
        [HttpPut("{propertyId}"), RequestValidation]
        public async Task<IActionResult> UpdateAsync(long propertyId, [FromBody] UpdatePropertyModel model)
        {
            return Ok(await _facade.UpdateAsync(propertyId, model));
        }

        /// <summary>
        /// Add an image to property
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST /addImage/1
        ///     {
        ///         "files": [
        ///             "data:image/jpeg;base64,/9j/4AAQSkZJRgABAQAAAQABAAD/...",
        ///             ""data:image/jpeg;base64,/9j/4AAQSkZJRgABAQAAJHGJGJA/...""
        ///         ]
        ///     }
        /// 
        /// </remarks>
        /// <returns></returns>
        [HttpPost("addImage/{propertyId}"), RequestValidation]
        public async Task<IActionResult> AddImageToPropertyAsync(long propertyId, [FromBody] PropertyImageModel model)
        {
            await _facade.AddImageAsync(propertyId, model);
            return Ok();
        }

        /// <summary>
        /// Change price of property
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     PUT /changePrice/1
        ///     {
        ///         "price": 45.87
        ///     }
        /// 
        /// </remarks>
        /// <param name="propertyId"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut("changePrice/{propertyId}")]
        public async Task<IActionResult> ChangePriceAsync(long propertyId, [FromBody] ChangePriceModel model)
        {
            await _facade.ChangePrice(propertyId, model);
            return Ok();
        }

        /// <summary>
        /// Find properties by filter
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     GET /
        ///     {
        ///         "name": "Diego",
        ///         "address": "Cra 1",
        ///         "year": 2022,
        ///         "idOwner": 5
        ///     }
        /// 
        /// </remarks>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetByFilterAsync([FromBody] PropertyFilter filter)
            => Ok(await _facade.GetPropertiesByFilterAsync(filter));
        #endregion
    }
}
