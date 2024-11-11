using Technico.Main.Models.Domain;

namespace Technico.Main.Repositories;

public interface IRepairRepository
{
    Task<Repair?> CreateAsync(Repair repair);
    Task<List<Repair>> GetAllAsync();
    Task<Repair?> GetByIdAsync(Guid guid);
    Task<Repair?> UpdateAsync(Repair updatedRepair);
    Task<bool> DeleteAsync(Guid guid);
}
