using Technico.Main.DTOs;
using Technico.Main.DTOs.RepairDtos;

namespace Technico.Main.Services;

public interface IRepairService
{
    Task<List<RepairDto>> GetAllAsync();
    Task<RepairDto?> CreateAsync(PostRepairDto repairDto);
    Task<RepairDto?> GetAsync(Guid id);
    Task<bool> DeleteAsync(Guid id);
    Task<RepairDto?> UpdateAsync(UpdateRepairDto repairDto);
}
