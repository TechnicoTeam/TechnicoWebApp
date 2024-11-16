using Microsoft.EntityFrameworkCore;
using Technico.Main.Data;
using Technico.Main.DTOs;
using Technico.Main.Mappers;
using Technico.Main.DTOs.RepairDtos;
using Technico.Main.Models;
using Technico.Main.Models.Enums;
using Technico.Main.Models.Domain;
using Technico.Main.Repositories;

namespace Technico.Main.Services.Implementations;

public class RepairService : IRepairService
{
    private readonly IRepairRepository _repairRepo;
    private readonly TechnicoDbContext _context;

    public RepairService(IRepairRepository repairRepo, TechnicoDbContext context)
    {
        _repairRepo = repairRepo;
        _context = context;
    }

    public async Task<List<RepairDto>> GetAllAsync()
    {
        var repairs = await _repairRepo.GetAllAsync();
        return repairs.Select(r => r.ConvertToDto()).ToList();
    }

    public async Task<RepairDto?> CreateAsync(PostRepairDto repairDto)
    {

        // Check if the associated property exists
        var property = await _context.Properties
            .Include(p => p.Owners)
            .Where(p => p.Id == repairDto.PropertyId)
            .FirstOrDefaultAsync();

        if (property == null)
        {
            return null;
        }

        var newRepair = new Repair
        {
            Cost = repairDto.Cost,
            Description = repairDto.Description,
            Status = repairDto.Status,
            Type = repairDto.Type,
            Property = property
        };

        var createdRepair = await _repairRepo.CreateAsync(newRepair);
        return createdRepair?.ConvertToDto();
    }

    public async Task<RepairDto?> GetAsync(Guid id)
    {
        var repair = await _repairRepo.GetByIdAsync(id);
        return repair?.ConvertToDto();
    }

    public async Task<List<RepairDto>> GetByPropertyAsync(Guid propertyId)
    {
        var repairs= await _repairRepo.GetByPropertyAsync(propertyId);
        return repairs.Select(r => r.ConvertToDto()).ToList();
    }
    public async Task<List<RepairDto>> GetByOwnerAsync(Guid ownerId)
    {
        var repairs = await _repairRepo.GetByOwnerAsync(ownerId);
        return repairs.Select(r => r.ConvertToDto()).ToList();
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        return await _repairRepo.DeleteAsync(id);
    }

    public async Task<RepairDto?> UpdateAsync(UpdateRepairDto repairDto)
    {
        // Check if the repair exists
        var repair = await _repairRepo.GetByIdAsync(repairDto.Id);
        if (repair == null) return null;

        if (repair.Property == null) return null; // Property is a required association

        var repairToUpdate = new Repair
        {
            Property = repair.Property,
            Type = repairDto.Type,
            Cost = repairDto.Cost,
            Description = repairDto.Description,
            Status = repairDto.Status,
            Id = repair.Id
        };

        var updatedRepair = await _repairRepo.UpdateAsync(repairToUpdate);
        return updatedRepair?.ConvertToDto();
    }
    public async Task <List<RepairDto>>SearchAdminAsync(string? Vat, StatusOfRepair? status, DateTime? FromDate, DateTime? ToDate)
    {
        var repairs  = await _repairRepo.SearchAdminAsync( Vat, status,FromDate,ToDate);
        return repairs.Select (rep =>rep.ConvertToDto()).ToList();
    }

    public async Task<List<RepairDto>> SearchOwnerPropertyAsync(TypeOfRepair? type, StatusOfRepair? status, Guid propertyId)
    {
        var repairs = await _repairRepo.SearchOwnerPropertyAsync( type, status, propertyId);
        return repairs.Select (rep =>rep.ConvertToDto()).ToList();
    }

    public async Task<List<RepairDto>> SearchOwnerAsync(TypeOfRepair? type, StatusOfRepair? status, Guid ownerId)
    {
        var repairs = await _repairRepo.SearchOwnerAsync( type, status, ownerId);
        return repairs.Select(rep => rep.ConvertToDto()).ToList();
    }
}