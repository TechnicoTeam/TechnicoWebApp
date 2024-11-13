using Technico.Main.Models;
using Technico.Main.Models.Enums;

namespace Technico.Main.Repositories;

public interface IRepairRepository
{
    Task<Repair?> CreateAsync(Repair repair);
    Task<List<Repair>> GetAllAsync();
    Task<Repair?> GetByIdAsync(Guid guid);
    Task<Repair?> UpdateAsync(Repair updatedRepair);
    Task<List<Repair>> SearchForDateAsync(DateTime CreatedAt);
    Task<List<Repair>> SearchForActiveAsync();
    Task<bool> DeleteAsync(Guid guid);
    Task<List<Repair>> SearchWithVatAsync(string Vat);
    Task<List<Repair>> SearchForScheduledDateAsync(DateTime ScheduledAt);
}
