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
        // Validate input request
        if (ownerDtoRequest == null)
        {
            throw new ArgumentNullException(nameof(ownerDtoRequest), "Owner request cannot be null.");
        }

        if (string.IsNullOrWhiteSpace(ownerDtoRequest.Vat))
        {
            throw new ArgumentException("VAT cannot be null or empty.", nameof(ownerDtoRequest.Vat));
        }

        // Validate VAT format (example pattern; modify based on actual requirements)
        if (!Regex.IsMatch(ownerDtoRequest.Vat, @"^\d{9}$")) // Example: 9-digit VAT
        {
            throw new ArgumentException("VAT format is invalid.", nameof(ownerDtoRequest.Vat));
        }

        // Check if the owner is already in the database
        var foundOwner = await _ownerRepository.GetByVatAsync(ownerDtoRequest.Vat);
        if (foundOwner != null)
        {
            // You might want to handle this scenario differently based on requirements
            throw new InvalidOperationException("An owner with this VAT already exists.");
        }

        // Create new owner if validation passes
        var createdOwner = await _ownerRepository.AddAsync(ownerDtoRequest.ConvertToOwner());
        return createdOwner.ConvertToOwnerDtoResponse();
    }

    public async Task<bool> Delete(Guid id)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("Invalid owner ID.", nameof(id));
        }

        return await _ownerRepository.DeleteAsync(id);
    }

    public async Task<IEnumerable<OwnerDtoResponse>> GetAllOwners()
    {
        var owners = await _ownerRepository.GetAllAsync();
        return owners.Select(owner => owner.ConvertToOwnerDtoResponse());
    }

    public async Task<OwnerDtoResponse> GetOwnerByVAT(string vat)
    {
        if (string.IsNullOrWhiteSpace(vat))
        {
            throw new ArgumentException("VAT cannot be null or empty.", nameof(vat));
        }

        // Validate VAT format
        if (!Regex.IsMatch(vat, @"^\d{9}$")) // Modify pattern as needed
        {
            throw new ArgumentException("VAT format is invalid.", nameof(vat));
        }

        var owner = await _ownerRepository.GetByVatAsync(vat);
        return owner.ConvertToOwnerDtoResponse();
    }
}
}