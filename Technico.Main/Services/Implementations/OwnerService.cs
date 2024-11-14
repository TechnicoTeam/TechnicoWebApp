using System.Text.RegularExpressions;
using Technico.Main.DTOs;
using Technico.Main.Mappers;
using Technico.Main.Models;
using Technico.Main.Models.Enums;
using Technico.Main.Repositories.Implementations;
using Technico.Main.Services;

//the main logic is 
//Take as argument a OwnerRequest
// return a OwnerResponse.

//TODO: Validations 
//TODO: Update operation

//namespace Technico.Main.Services.Implementations
//{
//    public class OwnerService : IOwnerService
//    {
//        private readonly IOwnerRepository _ownerRepository;
//        public OwnerService(IOwnerRepository ownerRepository)
//        {
//            _ownerRepository = ownerRepository;
//        }

//        public async Task<OwnerDtoResponse> Create(OwnerDtoRequest ownerDtoResponse)
//        {
//            //check if the owner is in the database
//            var foundOwner = await _ownerRepository.GetByVatAsync(ownerDtoResponse.Vat); //find a better way 
//            if (foundOwner is not null)
//            {
//                return null;
//            }
//            var createdOwner = await _ownerRepository.
//                AddAsync(ownerDtoResponse.ConvertToOwner());
//            return createdOwner.ConvertToOwnerDtoResponse();
//        }
//        public async Task<bool> Delete(Guid id)
//        {
//            return await _ownerRepository.DeleteAsync(id);
//        }
//        public async Task<IEnumerable<OwnerDtoResponse>> GetAllOwners()
//        {
//            var owners = await _ownerRepository.GetAllAsync();
//            owners.Select(owner => owner.ConvertToOwnerDtoResponse());
//            return owners.Select(owner => owner.ConvertToOwnerDtoResponse());
//        }

//        public async Task<OwnerDtoResponse> GetOwnerByVAT(string VAT)
//        {
//            var owner = await _ownerRepository.GetByVatAsync(VAT);

//            return owner.ConvertToOwnerDtoResponse();
//        }
//    }
//}
namespace Technico.Main.Services.Implementations

{
    public class OwnerService : IOwnerService
{
    private readonly IOwnerRepository _ownerRepository;

    public OwnerService(IOwnerRepository ownerRepository)
    {
        _ownerRepository = ownerRepository;
    }

        public async Task<OwnerDtoResponse> Create(OwnerDtoRequest ownerDtoRequest)
        {
            //check if the owner is in the database
            var foundOwner = await _ownerRepository.GetByVatAsync(ownerDtoRequest.Vat); //find a better way 
            if (foundOwner is not null)
            {
                return null;
            }
            var createdOwner = await _ownerRepository.
                AddAsync(ownerDtoRequest.ConvertToOwner());
            return createdOwner.ConvertToOwnerDtoResponse();
        }
        public async Task<bool> Delete(Guid id)
        {
            return await _ownerRepository.DeleteAsync(id);
        }
        public async Task<IEnumerable<OwnerDtoResponse>> GetAllOwners()
        {
            var owners = await _ownerRepository.GetAllAsync();
            owners.Select(owner => owner.ConvertToOwnerDtoResponse());
            return owners.Select(owner => owner.ConvertToOwnerDtoResponse());
        }

        public async Task<OwnerDtoResponse?> GetOwnerByVAT(string vat)
        {
            if (string.IsNullOrWhiteSpace(vat))
            {
                throw new ArgumentException("VAT cannot be null or empty.", nameof(vat));
            }


            var owner = await _ownerRepository.GetByVatAsync(vat);

            if (owner is null)
            {
                return null;
            }

            return owner.ConvertToOwnerDtoResponse();
        }

        public async Task<OwnerDtoResponse> GetByIdAsync(Guid id)
        {
            var owner = await _ownerRepository.GetByIdAsync(id);

            return owner.ConvertToOwnerDtoResponse();
        }

        public async Task<OwnerDtoResponse> Update(OwnerDtoResponse ownerDtoResponse)
        {
            // Find the owner in the database using the provided ID
            var foundOwner = await _ownerRepository.GetByIdAsync(ownerDtoResponse.Id);

            if (foundOwner == null)
            {
                return null;  // Return null if the owner is not found
            }

            // Update the existing owner's properties with values from the DTO
            foundOwner.Vat = ownerDtoResponse.Vat;
            foundOwner.Firstname = ownerDtoResponse.Firstname;
            foundOwner.Lastname = ownerDtoResponse.Lastname;
            foundOwner.Email = ownerDtoResponse.Email;
            foundOwner.Phone = ownerDtoResponse.Phone;
            foundOwner.Address = ownerDtoResponse.Address;
            foundOwner.Role = ownerDtoResponse.Role;

            // Call the repository's UpdateAsync method to persist changes in the database
            var updatedOwner = await _ownerRepository.UpdateAsync(foundOwner);

            // Return the updated owner as a DTO response
            return updatedOwner.ConvertToOwnerDtoResponse();
        }

        public async Task<OwnerDtoResponse?> GetOwnerWithIdByEmailAndPassword(string email, string password)
        {
            var owner = await _ownerRepository.GetByEmailAndPasswordAsync(email, password);
            if (owner == null)
            {
                return null;
            }
            return owner.ConvertToOwnerDtoResponse();
        }

    }
}
