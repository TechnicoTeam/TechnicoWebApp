using Technico.Main.DTOs;
using Technico.Main.Models;

namespace Technico.Main.Services
{
    public interface IOwnerService
    {
        Task<OwnerDtoResponse> Create(OwnerDtoRequest owner);
        Task<bool> Delete(Guid id);
        Task<IEnumerable<OwnerDtoResponse>> GetAllOwners();
        Task<OwnerDtoResponse> GetOwnerByVAT(string VAT);
        Task<OwnerDtoResponse> GetByIdAsync(Guid id);
        Task<OwnerDtoResponse> Update(OwnerDtoResponse owner);
    }
}