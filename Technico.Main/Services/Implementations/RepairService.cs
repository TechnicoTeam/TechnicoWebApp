using Microsoft.EntityFrameworkCore;
using Technico.Main.Data;
using Technico.Main.DTOs;
using Technico.Main.Mappers;
using Technico.Main.Models;
using Technico.Main.Repositories;

namespace Technico.Main.Services.Implementations
{
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
            //Gets all repairs
            var repairs = await _repairRepo.GetAllAsync();

            //Converts each repair to RepairDto. (RepairMapper extension class)
            return repairs.Select(r => r.ConvertToDto()).ToList();
        }

        public async Task<RepairDto?> CreateAsync(RepairDto repairDto)
        {
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

            return createdRepair.ConvertToDto();
        }

        public async Task<RepairDto?> GetAsync(Guid id)
        {
            var repair = await _repairRepo.GetByIdAsync(id);

            if (repair == null) { return null; }

            return repair.ConvertToDto();
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            return await _repairRepo.DeleteAsync(id);
        }

        public async Task<RepairDto?> UpdateAsync(UpdateRepairDto repairDto)
        {
            var repair = await _repairRepo.GetByIdAsync(repairDto.Id);

            if (repair == null)
            {
                return null;
            }

            var property = await _context.Properties
                .Include(p => p.Owners)
                .Where(p => p.Id == repair.Property.Id)
                .FirstOrDefaultAsync();

            if (property == null) 
            { 
                return null; 
            }

            var repairToUpdate = new Repair
            {
                Property = property,
                Type = repairDto.Type,
                Cost = repairDto.Cost,
                Description = repairDto.Description,
                Status = repairDto.Status,
                Id = repair.Id
            };

            var updatedRepair = await _repairRepo.UpdateAsync(repairToUpdate);

            return updatedRepair?.ConvertToDto();
        }
    }
}
