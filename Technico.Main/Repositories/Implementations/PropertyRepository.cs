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
    public async Task<Property?> AddCoOwner(Property property,Owner owner)
    {
        try
        {
            // Check if the Property exists in the database
            var existingProperty = await _context.Properties
                .Include(p => p.Owners) // Include Owners for proper association
                .FirstOrDefaultAsync(p => p.Id == property.Id);

            // ------------------not sure about these -  maybe i need _logger.LogWarning for these-------------//

            if (existingProperty == null)
            {
                //_logger.LogWarning("Property with ID {PropertyId} not found.", property.Id);
                return null; // Return null if the property does not exist
            }

            // Check if the Owner already exists in the database
            var existingOwner = await _context.Owner.FirstOrDefaultAsync(o => o.Id == owner.Id);

            if (existingOwner == null)
            {
               // _logger.LogWarning("Owner with ID {OwnerId} not found.", owner.Id);
                return null;
            }

            // Check if the Owner is already associated with the Property
            if (existingProperty.Owners.Any(o => o.Id == existingOwner.Id))
            {
                //_logger.LogWarning("Owner with ID {OwnerId} is already associated with the property ID {PropertyId}.", owner.Id, property.Id);
                return null;
            }

            //-----------------------------------------------------//


            // Add the Owner to the Property's Owners collection
            existingProperty.Owners.Add(existingOwner);

            // Save changes to the database
            await _context.SaveChangesAsync();

            // Return the updated Property
            return existingProperty;
        }
        catch (DbUpdateException ex)
        {
            _logger.LogWarning("Database error occurred, adding a co Owner failed: {message}", ex.Message);
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError("An invalid operation occurred: {message}", ex.Message);
            return null;
        }
    }

    // Create a new property for an owner
    public async Task<Property?> Create(Property property,Owner owner)
    {
        try
        {
            // Check if the Owner already exists in the database
            var existingOwner = await _context.Owner.FirstOrDefaultAsync(o => o.Id == owner.Id);

            if (existingOwner == null)
            {
                //_logger.LogInformation("Owner with ID {OwnerId} does not exist. Adding to the database.", owner.Id);
                return null;
            }

            // Associate the Property with the Owner
            property.Owners = new List<Owner> { existingOwner };

            // Add the Property to the database
            _context.Properties.Add(property);

            // Save changes to the database
            await _context.SaveChangesAsync();

            // Return the created Property
            return property;
        }
        catch (DbUpdateException ex)
        {
            _logger.LogWarning("Database error occurred, Create failed: {message}", ex.Message);
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError("An invalid operation occurred: {message}", ex.Message);
            return null;
        }
    }

    public async Task<Property?> DeleteById(Guid propertyid)
    {
        var Property = await _context.Properties.FindAsync(propertyid);

        try
        {
            if (Property != null)
            {

                _context.Properties.Remove(Property);
                await _context.SaveChangesAsync();
                return Property; // Return the deleted property
            }

           // _logger.LogWarning("Property with ID {PropertyId} not found.", propertyid);
            return null;
        }
        catch(DbUpdateException ex){
            _logger.LogWarning("Database error occurred, Delete failed: {message}", ex.Message);
            return null;

        }
        catch (Exception ex) {
            _logger.LogWarning("An invalid operation occurred: {message}", ex.Message);
            return null;
        }
        

    }

    

    // Get all the properties of a owner (with all the onwers)
    public async Task<List<Property>> GetAll(Guid ownerId)
    {
        try
        {
            return await _context.Properties
                .Where(p => p.Owners.Any(o=>o.Id == ownerId))
                .Include(p => p.Owners)
                .ToListAsync();

        }
        catch (InvalidOperationException ex){
            // Log or handle invalid operations, such as query structure
            _logger.LogWarning("An invalid operation occurred: {message}", ex.Message);
            return [];
        }
        catch (DbUpdateConcurrencyException ex){
            // Log or handle database-related issues
            _logger.LogWarning("Database error occurred while retrieving the data failed: {message}", ex.Message);
            return [];
        }
        catch (Exception ex){
            // Generic fallback for unexpected exceptions
            _logger.LogWarning("An invalid operation occurred: {message}", ex.Message);
            return [];
        }
    }

    // get a property by id (this is for the page of a specific property - maybe is not needed )
    public async Task<Property?> GetById(Guid propertyid)
    {
        try
        {
            return await _context.Properties
                .Include(p => p.Owners)
                .FirstOrDefaultAsync(p => p.Id == propertyid);
        }
        catch(InvalidOperationException ex){
            // Log or handle invalid operations, such as query structure
            _logger.LogWarning("An invalid operation occurred: {message}", ex.Message);
            return null;
        }
        catch (DbUpdateConcurrencyException ex){
            // Log or handle database-related issues
            _logger.LogWarning("Database error occurred while retrieving the data failed: {message}", ex.Message);
            return null;
        }
        catch (Exception ex){
            // Generic fallback for unexpected exceptions
            _logger.LogWarning("An invalid operation occurred: {message}", ex.Message);
            return null;
        }

    
    }


    // Search a property by the E9 or by type or by owner's vat 
    public async Task<List<Property>> Search(string? E9, TypeOfProperty? type, string? vat)
    {
       
        try
        {
            IQueryable<Property> search = _context.Properties;
            if (E9 is not null) search = search.Where(a => a.E9 == E9);
            if (type is not null) search = search.Where(b => b.Type == type);
            if (vat is not null) search = search.Include(c => c.Owners).Where(c => c.Owners.Any(o => o.Vat == vat));

            return await search.ToListAsync();
        }
        catch (InvalidOperationException ex) {
            // Log or handle invalid operations, such as query structure
            _logger.LogWarning("An invalid operation occurred: {message}", ex.Message);
            return [];
        }
        catch (DbUpdateConcurrencyException ex) {
            // Log or handle database-related issues
            _logger.LogWarning("Database error occurred while retrieving the data failed: {message}", ex.Message);
            return [];
        }
        catch (Exception ex) {
            // Generic fallback for unexpected exceptions
            _logger.LogWarning("An error occurred: {message}", ex.Message);
            return [];
        }

    }

    // Update a property (this is not including the relation user - property)
    public async Task<Property?> Update(Property property)
    {

        try
        {
            // Fetch the existing property from the database without loading Owners
            var existingProperty = await _context.Properties
                .FirstOrDefaultAsync(p => p.Id == property.Id);

            if (existingProperty == null)
            {
               // _logger.LogWarning("Property with ID {Id} not found.", property.Id);
                return null; // Return null if the property does not exist
            }

            // Update scalar fields only
            existingProperty.Address = property.Address;
            existingProperty.ConstructionYear = property.ConstructionYear;
            existingProperty.E9 = property.E9;
            existingProperty.Type = property.Type;

            // Do NOT modify the Owners navigation property
            // Save changes to the database
            await _context.SaveChangesAsync();
            return existingProperty; // Return the updated property
        }
        catch (DbUpdateConcurrencyException ex)
        {
            _logger.LogWarning("Database error occurred while retrieving the data failed: {message}", ex.Message);
            return null;
        }
        catch (DbUpdateException ex)
        {
            _logger.LogWarning("Database error occurred while retrieving the data failed: {message}", ex.Message);
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError("An error occurred: {message}", ex.Message);
            return null;
        }
    }

}
