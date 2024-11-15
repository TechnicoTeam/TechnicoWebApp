using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using Technico.Main.Data;
using Technico.Main.Models;
using Technico.Main.Models.Domain;
using Technico.Main.Models.Enums;

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
    public async Task<List<Repair>> GetByPropertyAsync(Guid propertyId)
    {
        var repairs= await _context.Repairs
            .Include(r => r.Property)
            .ThenInclude(p => p.Owners)
            .Where(r=>r.Property.Id == propertyId).ToListAsync();
        return repairs;
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

        if (!string.IsNullOrWhiteSpace(updatedRepair.Type.ToString()))
        {
            repair.Type = updatedRepair.Type;
        }

        if (!string.IsNullOrWhiteSpace(updatedRepair.Description))
        {
            repair.Description = updatedRepair.Description;
        }

        if (!string.IsNullOrWhiteSpace(updatedRepair.Status.ToString()))
        {
            repair.Status = updatedRepair.Status;
        }

        if (!string.IsNullOrWhiteSpace(updatedRepair.Cost.ToString()))
        {
            repair.Cost = updatedRepair.Cost;
        }

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


    public async Task<List<Repair>> SearchAdminAsync(string? Vat, StatusOfRepair? status, DateTime? FromDate, DateTime? ToDate)
    {
        IQueryable<Repair> repairs = _context.Repairs
            .Include(r => r.Property)
            .ThenInclude(p => p.Owners);

        // Filter by Vat if provided
        if (!string.IsNullOrEmpty(Vat))  repairs = repairs.Where(repair =>repair.Property.Owners.Any(owner => owner.Vat == Vat));
      

        // Filter by status if provided
        if (status != null)  repairs = repairs.Where(r => r.Status == status);


        // Filter by date range
        if (FromDate != null && ToDate != null)
        {
            repairs = repairs.Where(r => r.CreatedAt >= FromDate && r.CreatedAt <= ToDate);
        }
        else if (FromDate != null)
        {
            repairs = repairs.Where(r => r.CreatedAt >= FromDate);
        }
        else if (ToDate != null)
        {
            repairs = repairs.Where(r => r.CreatedAt <= ToDate);
        }

        // Execute the query and return the results
        return await repairs.ToListAsync();
    }


    public async Task<List<Repair>> SearchOwnerPropertyAsync(TypeOfRepair? type, StatusOfRepair? status, Guid propertyId)
    {
        var repairs = _context.Repairs.Include(r=>r.Property).ThenInclude(r=>r.Owners).Where(r=>r.Property.Id == propertyId);

        if (status != null) repairs = repairs.Where(r => r.Status == status);
        if (type != null) repairs = repairs.Where(r=>r.Type == type);
        return await repairs.ToListAsync();
    }

    public async Task<List<Repair>> SearchOwnerAsync(TypeOfRepair? type, StatusOfRepair? status, Guid ownerId)
    {
        var repairs = _context.Repairs.Include(r => r.Property).ThenInclude(r => r.Owners).Where(r => r.Property.Owners.Any(r => r.Id == ownerId));

        if (status != null) repairs = repairs.Where(r => r.Status == status);
        if (type != null) repairs = repairs.Where(r => r.Type == type);

        return await repairs.ToListAsync();

    }

    public async Task <List<Repair>> GetByOwnerAsync(Guid ownerId)
    {
        return await _context.Repairs
            .Include(r => r.Property)
            .ThenInclude(r => r.Owners)
            .Where(r => r.Property.Owners.Any(r => r.Id == ownerId)).ToListAsync();
    }

}
