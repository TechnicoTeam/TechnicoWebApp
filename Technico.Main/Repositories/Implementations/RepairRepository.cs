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

        var repairs =  await _context.Repairs
            .Include(r => r.Property)
            .ThenInclude(p => p.Owners)
            .ToListAsync();

        _logger.LogInformation($"Retrieved a total of repairs: {repairs.Count}.");

        return repairs;
    }

    public async Task<Repair?> GetByIdAsync(Guid guid)
    {
        _logger.LogInformation("Invoked RepairRepository.GetByIdAsync().");

        return await _context.Repairs
            .Include(r => r.Property)
            .ThenInclude(p => p.Owners)
            .FirstOrDefaultAsync(r => r.Id == guid);
        
    }

    public async Task<Repair?> UpdateAsync (Repair updatedRepair)
    {
        _logger.LogInformation("Invoked RepairRepository.UpdateAsync().");

        var repair = await _context.Repairs
            .Include(r => r.Property)
            .ThenInclude(p => p.Owners)
            .FirstOrDefaultAsync(r => r.Id == updatedRepair.Id);

        if (repair == null)
        {
            _logger.LogWarning($"Cannot update Repair with Guid: {updatedRepair.Id}. Not Found.");
            return null;
        }

        repair.Type = updatedRepair.Type;
        repair.Description = updatedRepair.Description;
        repair.Status = updatedRepair.Status;
        repair.Cost = updatedRepair.Cost;

        
        await _context.SaveChangesAsync();
        

        _logger.LogInformation($"Successfully updated the repair.");
        return repair;
    }

    public async Task<bool> DeleteAsync(Guid guid)
    {
        _logger.LogInformation("Invoked RepairRepository.DeleteAsync().");

        var repair = await _context.Repairs.FirstOrDefaultAsync(r => r.Id == guid);

        if (repair is null)
        {
            _logger.LogWarning($"Cannot delete Repair with Guid: {guid}. Not Found.");
            return false;
        }

        _context.Repairs.Remove(repair);
        await _context.SaveChangesAsync();


        return true;
    }
}
