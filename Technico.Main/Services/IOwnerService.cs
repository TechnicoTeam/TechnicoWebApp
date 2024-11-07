using Technico.Main.Models;

namespace Technico.Main.Services
{
    public interface IOwnerService
    {
        Task<Owner> Create(Owner owner);
        Task<bool> Delete(Guid id);
        Task<IEnumerable<Owner>> GetAllOwners();
        Task<Owner> GetOwnerByVAT(string VAT);
    }
}