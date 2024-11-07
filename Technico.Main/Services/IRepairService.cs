using Technico.Main.DTOs;

namespace Technico.Main.Services;

public interface IRepairService
{
    Task<List<RepairDto>> GetAllAsync();
    Task<RepairDto?> CreateAsync(RepairDto repairDto);
    Task<RepairDto?> GetAsync(Guid id);
    Task<bool> DeleteAsync(Guid id);
    Task<RepairDto?> UpdateAsync(UpdateRepairDto repairDto);
}
