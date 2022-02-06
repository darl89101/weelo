using Weelo.Domain.Entities;

namespace Weelo.Domain.Interfaces.Services
{
    public interface IOwnerService
    {
        Task<Owner> CreateAsync(Owner owner);
        Task<Owner?> GetOwnerByDocumentAsync(string documentNumber);
    }
}
