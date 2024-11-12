using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Technico.Main.Data;
using Technico.Main.DTOs.PropertyDtos;
using Technico.Main.Models.Domain;
using Technico.Main.Models.Enums;

namespace Technico.Main.Repositories.Implementations;

public class PropertyRepository : IPropertyRepository
{

    private readonly TechnicoDbContext _context;
    private readonly ILogger <Property> _logger;

    
    public PropertyRepository (TechnicoDbContext context, ILogger <Property> logger)
    {
        _context = context;
        _logger = logger;
    }


    // Create a new property for an owner
    public async Task<Property?> Create(Property property,List<Owner> owners)
    {

        // Add the Property to each owner's properties list
        foreach (var owner in owners)
        {
            owner.Properties.Add(property);
          
        }
        // Add the Property to the database
        _context.Properties.Add(property);

        // Save changes to the database
        await _context.SaveChangesAsync();

        // Return the created Property
        return property;
       
    }

    public async Task<bool> DeleteById(Guid propertyid)
    {
        var Property = await _context.Properties.FindAsync(propertyid);
            if (Property != null){

                _context.Properties.Remove(Property);
                await _context.SaveChangesAsync();
                return true; // Return the deleted property
            }

        _logger.LogWarning("The Delete Failed. The property does not exist.");
        return false;

    }

    

    // Get all the properties of a owner (with all the owners)
    public async Task<List<Property>> GetAll(Guid ownerId)
    {

        return await _context.Properties
            .Where(p => p.Owners.Any(o => o.Id == ownerId))
            .Include(p => p.Owners)
            .ToListAsync();


    }

    // Get all the properties 
    public async Task<List<Property>> GetAll()
    {

        return await _context.Properties
            .Include(p => p.Owners)
            .ToListAsync();
    }


    // get a property by id (this is for the page of a specific property - maybe is not needed )
    public async Task<Property?> GetById(Guid propertyid)
    {
        return await _context.Properties
                .Include(p => p.Owners)
                .FirstOrDefaultAsync(p => p.Id == propertyid);
    }


    // Search a property by the E9 or by type or by owner's vat 
    public async Task<List<Property>> Search(string? E9, TypeOfProperty? type, string? vat)
    {
       
      
            IQueryable<Property> search = _context.Properties;
            if (E9 is not null) search = search.Include(c => c.Owners).Where(a => a.E9 == E9);
            if (type is not null) search = search.Include(c => c.Owners).Where(b => b.Type == type);
            if (vat is not null) search = search.Include(c => c.Owners).Where(c => c.Owners.Any(o => o.Vat == vat));

            return await search.ToListAsync();
      

    }

    // Update a property (this is not including the relation user - property)
    public async Task<Property?> Update(PropertyDtoUpdateRequest property, List<Owner> owners)
    {
        // Fetch the existing property from the database along with its associated Owners
        var existingProperty = await _context.Properties
            .Include(p => p.Owners)
            .FirstOrDefaultAsync(p => p.Id == property.Id);

        // Check if the property exists
        if (existingProperty == null)
        {
            _logger.LogWarning("The Update Failed. The property does not exist.");
            return null;
        }

        // Update scalar fields
        existingProperty.Address = property.Address;
        existingProperty.ConstructionYear = property.ConstructionYear;
        existingProperty.Type = property.Type;


        // Replace the existing owners with the new list
        existingProperty.Owners.Clear();

        foreach (var owner in owners)
        {
            existingProperty.Owners.Add(owner);
        }


        // Save changes to the database
        await _context.SaveChangesAsync();

        // Return the updated property
        return existingProperty;

    }

}
