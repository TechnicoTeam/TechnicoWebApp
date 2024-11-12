using Microsoft.AspNetCore.Http.HttpResults;
using Technico.Main.DTOs;
using Technico.Main.DTOs.RepairDtos;
using Technico.Main.Models;
using Technico.Main.Models.Enums;

namespace Technico.Main.Services;

public interface IRepairService
{
    Task<List<RepairDto>> GetAllAsync();
    Task<RepairDto?> CreateAsync(PostRepairDto repairDto);
    Task<RepairDto?> GetAsync(Guid id);
    Task<bool> DeleteAsync(Guid id);
    Task<RepairDto?> UpdateAsync(UpdateRepairDto repairDto);
    Task<List<RepairDto>> SearchForDateAsync(DateTime CreatedAt);
    Task<List<RepairDto>> SearchForActiveAsync();
    Task<List<RepairDto>> SearchWithVatAsync(string Vat);

}
