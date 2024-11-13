using Technico.Main.DTOs;
using Technico.Main.DTOs.RepairDtos;


namespace Technico.Main.Services
{
    public interface IRepairService
    {
        Task<RepairDto?> CreateAsync(PostRepairDto repairDto);
        Task<bool> DeleteAsync(Guid id);
        Task<List<RepairDto>> GetAllAsync();
        Task<RepairDto?> GetAsync(Guid id);
        Task<RepairDto?> UpdateAsync(UpdateRepairDto repairDto);
    }
}