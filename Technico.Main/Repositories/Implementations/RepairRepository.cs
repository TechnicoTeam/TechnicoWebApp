using Microsoft.EntityFrameworkCore;
using Technico.Main.Data;
using Technico.Main.Models;

namespace Technico.Main.Repositories.Implementations;

public class RepairRepository : IRepairRepository
{
    private readonly TechnicoDbContext _context;
    private readonly ILogger<Repair> _logger;

    public RepairRepository(TechnicoDbContext context, ILogger<Repair> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Repair?> CreateAsync(Repair repair)
    {
        _logger.LogInformation("Invoked RepairRepository.CreateAsync().");

        await _context.AddAsync(repair);
        await _context.SaveChangesAsync();
        return repair;
    }

    public async Task<List<Repair>> GetAllAsync()
    {
        _logger.LogInformation("Invoked RepairRepository.GetAllAsync().");

        try
        {
            return await _context.Repairs
                .Include(r => r.Property)
                .ThenInclude(p => p.Owners)
                .ToListAsync();
        }
        catch(Exception e)
        {
            _logger.LogError(e.Message);
            return new List<Repair>();
        }
    }

    public async Task<Repair?> GetByIdAsync(Guid guid)
    {
        _logger.LogInformation("Invoked RepairRepository.GetByIdAsync().");

        try
        {
            return await _context.Repairs
            .Include(r => r.Property)
            .ThenInclude(p => p.Owners)
            .FirstOrDefaultAsync(r => r.Id == guid);
        }
        catch(Exception e)
        {
            _logger.LogError(e.Message);
            return null;
        }
        
    }

    public async Task<Repair?> UpdateAsync (Repair updatedRepair)
    {
        _logger.LogInformation("Invoked RepairRepository.UpdateAsync().");

        var repair = await _context.Repairs
            .Include(r => r.Property)
            .ThenInclude(p => p.Owners)
            .FirstOrDefaultAsync(r => r.Id == updatedRepair.Id);

        repair.Type = updatedRepair.Type;
        repair.Description = updatedRepair.Description;
        repair.Status = updatedRepair.Status;
        repair.Cost = updatedRepair.Cost;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch(Exception ex)
        {
            _logger.LogError(ex.Message);
            return null;
        }

        _logger.LogInformation($"Successfully updated the repair.");
        return repair;
    }

    public async Task<bool> DeleteAsync(Guid guid)
    {
        _logger.LogInformation("Invoked RepairRepository.DeleteAsync().");

        var repair = _context.Repairs.FirstOrDefault(r => r.Id == guid);

        if (repair is null)
        {
            _logger.LogInformation($"Cannot delete Repair with Guid: {guid}. Not Found.");
            return false;
        }

        _context.Repairs.Remove(repair);
        await _context.SaveChangesAsync();


        return true;
    }
}
