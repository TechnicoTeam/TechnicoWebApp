﻿using Microsoft.EntityFrameworkCore;
using Technico.Main.Data;
using Technico.Main.DTOs;
using Technico.Main.Mappers;
using Technico.Main.DTOs.RepairDtos;
using Technico.Main.Models;
using Technico.Main.Models.Enums;
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
            var repairs = await _repairRepo.GetAllAsync();
            return repairs.Select(r => r.ConvertToDto()).ToList();
        }

        public async Task<RepairDto?> CreateAsync(PostRepairDto repairDto)
        {
            // Validate input data
            if (repairDto == null) throw new ArgumentNullException(nameof(repairDto), "Repair data cannot be null.");
            if (repairDto.Cost <= 0) throw new ArgumentException("Cost must be greater than zero.", nameof(repairDto.Cost));
            if (string.IsNullOrWhiteSpace(repairDto.Description))
                throw new ArgumentException("Description cannot be null or empty.", nameof(repairDto.Description));
            if (repairDto.PropertyId == Guid.Empty)
                throw new ArgumentException("Property ID must be a valid GUID.", nameof(repairDto.PropertyId));

            // Check if the associated property exists
            var property = await _context.Properties
                .Include(p => p.Owners)
                .Where(p => p.Id == repairDto.PropertyId)
                .FirstOrDefaultAsync();

            if (property == null) return null;

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
            if (id == Guid.Empty) throw new ArgumentException("ID must be a valid GUID.", nameof(id));

            var repair = await _repairRepo.GetByIdAsync(id);
            return repair?.ConvertToDto();
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            if (id == Guid.Empty) throw new ArgumentException("ID must be a valid GUID.", nameof(id));
            return await _repairRepo.DeleteAsync(id);
        }

        public async Task<RepairDto?> UpdateAsync(UpdateRepairDto repairDto)
        {
            // Validate input data
            if (repairDto == null) throw new ArgumentNullException(nameof(repairDto), "Repair data cannot be null.");
            if (repairDto.Id == Guid.Empty) throw new ArgumentException("ID must be a valid GUID.", nameof(repairDto.Id));
            if (repairDto.Cost <= 0) throw new ArgumentException("Cost must be greater than zero.", nameof(repairDto.Cost));
            if (string.IsNullOrWhiteSpace(repairDto.Description))
                throw new ArgumentException("Description cannot be null or empty.", nameof(repairDto.Description));

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

        public async Task<List<RepairDto>> SearchForDateAsync(DateTime CreatedAt)
        {
            var repairs = await _repairRepo.SearchForDateAsync(CreatedAt);
            return repairs.MapToListOfRepairDtos();
        }

        public async Task<List<RepairDto>> SearchForActiveAsync()
        {
            var repairs = await _repairRepo.SearchForActiveAsync();
            return repairs.MapToListOfRepairDtos();
        }
        public async Task<List<RepairDto>> SearchWithVatAsync(string Vat)
        {
            var repairs = await _repairRepo.SearchWithVatAsync(Vat);
            return repairs.MapToListOfRepairDtos();
        }
        public async Task<List<RepairDto>> SearchForScheduledDateAsync(DateTime ScheduledAt)
        {
            var repairs = await _repairRepo.SearchForScheduledDateAsync(ScheduledAt);
            return repairs.MapToListOfRepairDtos();
        }

    }
}