using Technico.Main.Models;
using Technico.Main.Models.Enums;
using Technico.Main.Models.Domain;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Technico.Main.Repositories;

public interface IRepairRepository
{
    Task<Repair?> CreateAsync(Repair repair);
    Task<List<Repair>> GetAllAsync();
    Task<Repair?> GetByIdAsync(Guid guid);
    Task<List<Repair>> GetByPropertyAsync(Guid propertyId);
    Task<List<Repair>> GetByOwnerAsync(Guid ownerId);
    Task<Repair?> UpdateAsync(Repair updatedRepair);
    Task<List<Repair>> SearchAdminAsync(string? Vat, StatusOfRepair? status, DateTime? FromDate, DateTime? ToDate);
    Task<List<Repair>> SearchOwnerPropertyAsync(TypeOfRepair? type, StatusOfRepair? status, Guid propertyId);
    Task<List<Repair>> SearchOwnerAsync(TypeOfRepair? type, StatusOfRepair? status, Guid ownerId);
    Task<bool> DeleteAsync(Guid guid);

}
