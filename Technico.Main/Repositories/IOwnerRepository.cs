using Technico.Main.Models.Domain;

namespace Technico.Main.Repositories.Implementations
{
    public interface IOwnerRepository
    {
        Task<Owner> AddAsync(Owner owner);
        Task<bool> DeleteAsync(Guid id);
        Task<IEnumerable<Owner>> GetAllAsync();
        Task<Owner?> GetByIdAsync(Guid id);
        Task<Owner> UpdateAsync(Owner owner);
        Task<Owner> GetByVatAsync(string VAT);
    }
}