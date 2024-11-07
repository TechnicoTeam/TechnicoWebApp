using Technico.Main.Models;
using Technico.Main.Models.Enums;
using Technico.Main.Repositories.Implementations;

namespace Technico.Main.Services
{
    public class OwnerService : IOwnerService
    {
        private readonly IOwnerRepository _ownerRepository;
        public OwnerService(IOwnerRepository ownerRepository)
        {
            _ownerRepository = ownerRepository;
        }

        public async Task<Owner> Create(Owner owner)
        {
            //check if the owner is in the database
            var foundOwner = await _ownerRepository.GetByVatAsync(owner.Vat);
            if (foundOwner is not null)
            {
                return null;
            }
            //convert to DTO
            return await _ownerRepository.AddAsync(owner);
        }
        public async Task<bool> Delete(Guid id)
        {
            return await _ownerRepository.DeleteAsync(id);
        }
        public async Task<IEnumerable<Owner>> GetAllOwners()
        {
            return await _ownerRepository.GetAllAsync();
        }

        public async Task<Owner> GetOwnerByVAT(string VAT)
        {
            return await _ownerRepository.GetByVatAsync(VAT);
        }




    }
}
