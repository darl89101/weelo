using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;
using Weelo.API.Facades.Interfaces;
using Weelo.Domain.Entities;
using Weelo.Domain.Exceptions;
using Weelo.Domain.Interfaces.Base;
using Weelo.Domain.Interfaces.Repositories;
using Weelo.Domain.Interfaces.Services;

namespace Weelo.tests
{
    /// <summary>
    /// Test of property service
    /// </summary>
    internal class PropertyServiceTest : BaseTest
    {
        /// <summary>
        /// Create an owner
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task Create_Owner_Success()
        {
            var property = new Owner
            {
                DocumentNumber = (new Random()).Next().ToString(),
                Name = "Owner Test",
                Address = $"Cra {(new Random()).Next()}"
            };
            var service = Resolve<IOwnerService>();
            var repository = Resolve<IOwnerRepository>();
            var res = await service.CreateAsync(property);
            var lastCreated = await repository.Query().OrderBy(m => m.Id).LastOrDefaultAsync();

            Assert.True(res?.Id > 0);
            Assert.AreEqual(res.Id, lastCreated.Id);
        }

        /// <summary>
        /// Create a property
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task Create_Property_Success()
        {
            var property = new Property
            {
                Name = $"Property test {(new Random()).Next()}",
                Address = $"Cra {(new Random()).Next()}",
                CodeInternal = Guid.NewGuid().ToString(),
                Price = 1,
                Year = 2022,
                IdOwner = 1
            };
            var service = Resolve<IPropertyService>();
            var repository = Resolve<IPropertyRepository>();
            var res = await service.CreateAsync(property);
            var lastCreated = await repository.Query().OrderBy(m => m.Id).LastOrDefaultAsync();

            Assert.True(res?.Id > 0);
            Assert.AreEqual(res.Id, lastCreated.Id);
        }

        /// <summary>
        /// Update a property
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task Update_Property_Success()
        {
            var repository = Resolve<IPropertyRepository>();
            var service = Resolve<IPropertyService>();
            var property = await repository.Query()
                .OrderBy(m => m.Id)
                .LastOrDefaultAsync();
            string propertyName = $"Property Updated {(new Random()).Next()}";
            property.Name = propertyName;
            var res = await service.UpdateAsync(property);

            Assert.AreEqual(res.Name, propertyName);
        }

        /// <summary>
        /// Change price of property
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task Change_Price_Property_Success()
        {
            decimal extectedPrice = (new Random()).Next();
            var repository = Resolve<IPropertyRepository>();
            var service = Resolve<IPropertyService>();
            var propertyId = await repository.Query()
                .OrderBy(m => m.Id)
                .Select(m => m.Id)
                .LastOrDefaultAsync();
            await service.ChangePrice(propertyId, extectedPrice);
            var property = await repository.FindByIdAsync(propertyId);

            Assert.AreEqual(property.Price, extectedPrice);
        }

        /// <summary>
        /// Change price of property
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task Run_Sale_Success()
        {
            var facade = Resolve<IPropertyFacade>();
            var repositoryTrace = Resolve<IGenericRepository<PropertyTrace, long>>();
            var repository = Resolve<IPropertyRepository>();
            var propertyId = await repository.Query()
                .OrderBy(m => m.Id)
                .Select(m => m.Id)
                .LastOrDefaultAsync();

            decimal salePrice = (new Random()).Next();
            var res = await facade.SaleAsync(propertyId, new API.Models.SaleModel
            {
                Name = "Vendedor test",
                Price = salePrice
            });
            var lastSale = await repositoryTrace.Query().OrderBy(m => m.Id).LastOrDefaultAsync();

            Assert.IsNotNull(res);
            Assert.AreEqual(lastSale.Value, salePrice);
        }

        /// <summary>
        /// Change price with throws exception
        /// </summary>
        /// <returns></returns>
        [Test]
        public void Change_Price_Property_Throws_Not_Found_Exception()
        {
            var service = Resolve<IPropertyService>();
            Assert.ThrowsAsync<NotFoundException>(() => service.ChangePrice(-1, 0));
        }

        /// <summary>
        /// Change price with throws exception
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task Add_Images_With_Throws_Not_Found_Exception()
        {
            var repository = Resolve<IPropertyRepository>();
            var propertyId = await repository.Query()
                .OrderBy(m => m.Id)
                .Select(m => m.Id)
                .LastOrDefaultAsync();
            var service = Resolve<IPropertyService>();
            Assert.ThrowsAsync<NotFoundException>(() => service.AddImagesAsync(propertyId, null));
        }
    }
}
