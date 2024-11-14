using Technico.Main.DTOs;

namespace Technico.Main.Services
{
    public interface IOwnerService
    {
        Task<OwnerDtoResponse> Create(OwnerDtoRequest ownerDtoResponse);
        Task<bool> Delete(Guid id);
        Task<IEnumerable<OwnerDtoResponse>> GetAllOwners();
        Task<OwnerDtoResponse?> GetOwnerByVAT(string VAT);
        Task<OwnerDtoResponse> GetByIdAsync(Guid id);
        Task<OwnerDtoResponse> Update(OwnerDtoResponse owner);
        Task<OwnerDtoResponse> GetOwnerWithIdByEmailAndPassword(string email, string password);
    }
}