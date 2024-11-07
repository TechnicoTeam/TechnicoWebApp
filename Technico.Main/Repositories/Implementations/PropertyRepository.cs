using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Technico.Main.Data;
using Technico.Main.DTOs;
using Technico.Main.Models;
using Technico.Main.Models.Enums;

namespace Technico.Main.Repositories.Implementations;

public class PropertyRepository : IPropertyRepository
{

    private readonly TechnicoDbContext _context;
    private readonly ILogger _logger;

    
    public PropertyRepository (TechnicoDbContext context, ILogger logger)
    {
        _context = context;
        _logger = logger;
    }

    // Add a co owner to a property
    //public async Task<Property?> AddCoOwner(Property property,Owner owner)
    //{
        
    //        // Check if the Property exists in the database
    //        var existingProperty = await _context.Properties
    //            .Include(p => p.Owners) // Include Owners for proper association
    //            .FirstOrDefaultAsync(p => p.Id == property.Id);

    //        // ------------------not sure about these -  maybe i need _logger.LogWarning for these-------------//

    //        if (existingProperty == null){
    //            return null; // Return null if the property does not exist
    //        }

    //        // Check if the Owner already exists in the database
    //        var existingOwner = await _context.Owner.FirstOrDefaultAsync(o => o.Id == owner.Id);

    //        if (existingOwner == null){
    //            return null;
    //        }

    //        // Check if the Owner is already associated with the Property
    //        if (existingProperty.Owners.Any(o => o.Id == existingOwner.Id)) {
    //            return null;
    //        }

    //        //-----------------------------------------------------//

    //        // Add the Owner to the Property's Owners collection
    //        existingProperty.Owners.Add(existingOwner);

    //        // Save changes to the database
    //        await _context.SaveChangesAsync();

    //        // Return the updated Property
    //        return existingProperty;
    //}

    // Create a new property for an owner
    public async Task<Property?> Create(Property property,List<Owner> owners)
    {

     
        if (owners == null || !owners.Any())
        {
            // Log or handle the case where no owners are provided
            return null;
        }

        var ownerIds= owners.Select(x => x.Id).ToList();
        var existingOwners = await _context.Owners
            .Where(x => ownerIds.Contains(x.Id))
            .ToListAsync();

        if (existingOwners.Count != owners.Count)
        {
            // Log or handle the case where some owners do not exist
            return null;
        }

        property.Owners = existingOwners;

        // Add the Property to each owner's properties list
        foreach (var owner in existingOwners)
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

    public async Task<Property?> DeleteById(Guid propertyid)
    {
        var Property = await _context.Properties.FindAsync(propertyid);
            if (Property != null){

                _context.Properties.Remove(Property);
                await _context.SaveChangesAsync();
                return Property; // Return the deleted property
            }

           // _logger.LogWarning("Property with ID {PropertyId} not found.", propertyid);
            return null;

    }

    

    // Get all the properties of a owner (with all the onwers)
    public async Task<List<Property>> GetAll(Guid ownerId)
    {
        
            return await _context.Properties
                .Where(p => p.Owners.Any(o=>o.Id == ownerId))
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
            if (E9 is not null) search = search.Where(a => a.E9 == E9);
            if (type is not null) search = search.Where(b => b.Type == type);
            if (vat is not null) search = search.Include(c => c.Owners).Where(c => c.Owners.Any(o => o.Vat == vat));

            return await search.ToListAsync();
      

    }

    // Update a property (this is not including the relation user - property)
    public async Task<Property?> Update(Property property, List<Owner> owners)
    {

        // Fetch the existing property from the database along with its associated Owners
        var existingProperty = await _context.Properties
            .Include(p => p.Owners)
            .FirstOrDefaultAsync(p => p.Id == property.Id);

        if (existingProperty == null)
        {
            return null; // Return null if the property does not exist
        }

        // Update scalar fields
        existingProperty.Address = property.Address;
        existingProperty.ConstructionYear = property.ConstructionYear;
        existingProperty.E9 = property.E9;
        existingProperty.Type = property.Type;

        // Update the Owners navigation property
        if (owners != null && owners.Any())
        {
            // Fetch existing owners from the database based on provided list
            var ownerIds = owners.Select(o => o.Id).ToList();
            var existingOwners = await _context.Owners
                .Where(o => ownerIds.Contains(o.Id))
                .ToListAsync();

            // Replace the existing owners with the new list
            existingProperty.Owners.Clear();
            foreach (var owner in existingOwners)
            {
                existingProperty.Owners.Add(owner);
            }
        }
        else
        {
            // Clear all owners if the list is empty or null
            existingProperty.Owners.Clear();
        }

        // Save changes to the database
        await _context.SaveChangesAsync();

        // Return the updated property
        return existingProperty;

    }

}
