using Technico.Main.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Technico.Main.Models.Domain;

namespace Technico.Main.Repositories.Implementations
{
    public class OwnerRepository : IOwnerRepository
    {
        private readonly TechnicoDbContext _context;
        private readonly ILogger<OwnerRepository> _logger;

        public OwnerRepository(TechnicoDbContext context, ILogger<OwnerRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<Owner>> GetAllAsync()
        {
            _logger.LogInformation("Retrieving all owners from the database.");
            var owners = await _context.Owners.Include(o => o.Properties).ToListAsync();
            _logger.LogInformation("Retrieved {Count} owners.", owners.Count);
            return owners;
        }

        public async Task<Owner?> GetByIdAsync(Guid id)
        {
            _logger.LogInformation("Retrieving owner with ID: {Id}", id);
            return await _context.Owners
                .Where(o => o.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task<Owner?> GetByVatAsync(string VAT)
        {
            _logger.LogInformation("Retrieving owner with VAT: {Id}", VAT);
            var owner = await _context.Owners
                .Include(o => o.Properties)
                .FirstOrDefaultAsync(o => o.Vat.Equals(VAT));
            return owner;
        }

        public async Task<Owner> AddAsync(Owner owner)
        {
            _logger.LogInformation("Adding a new owner with ID: {Id}", owner.Id);
            await _context.Owners.AddAsync(owner);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Successfully added owner with ID: {Id}", owner.Id);
            return owner;
        }

        public async Task<Owner> UpdateAsync(Owner owner)
        {

            _logger.LogInformation("Updating owner with ID: {Id}", owner.Id);
            _context.Owners.Update(owner);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Successfully updated owner with ID: {Id}", owner.Id);
            return owner;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {

            _logger.LogInformation("Deleting owner with ID: {Id}", id);
            var owner = await _context.Owners.FindAsync(id);
            if (owner != null)
            {
                _context.Owners.Remove(owner);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Successfully deleted owner with ID: {Id}", id);
                return true;
            }
            return false;

        }

    }


}
