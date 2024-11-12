using Technico.Main.Data;
using Technico.Main.Models;
using Technico.Main.Models.Enums;
using Technico.Main.Repositories;
using Microsoft.EntityFrameworkCore;
using Technico.Main.DTOs;
using Microsoft.IdentityModel.Tokens;
using Technico.Main.Mappers;

namespace Technico.Main.Services.Implementations;

public class PropertyService : IPropertyService
{

   private readonly IPropertyRepository _propertyRepo;
   private readonly TechnicoDbContext _context;

    public PropertyService(IPropertyRepository propertyRepo, TechnicoDbContext context)
    {
        _propertyRepo = propertyRepo;
        _context = context;
    }

    public async Task<List<PropertyDtoResponse>> GetAllAsync()
    {
        var properties = await _propertyRepo.GetAll();
        return properties.MapToListOfPropertiesDtos();

    }
    public async Task<List<PropertyDtoResponse>> GetAllAsync(Guid ownerId)
    {

        var properties= await _propertyRepo.GetAll(ownerId);
        return properties.MapToListOfPropertiesDtos();

    }

    public async Task<PropertyDtoResponse?> GetByIdAsync(Guid propertyId)
    {
        Property? property = await _propertyRepo.GetById(propertyId);

        if (property == null) { 
        return null;
        }
        return property.MapToPropertyDtos();
    }

    public async Task<PropertyDtoResponse?> CreateAsync(PropertyDtoRequest property)
    {

        // check if the E9 exists
        if (property.E9.IsNullOrEmpty())
        {
            return null;
        }
        // Check if the property E9 is unique
        var existingE9 = await _context.Properties.AnyAsync(e => e.E9.ToLower() == property.E9.ToLower());
        if (existingE9) {
            return null;
        }

        // Returns all the existing owners 
        var ownerIds = property.OwnersIds;
        if (ownerIds == null || ownerIds.IsNullOrEmpty())
        {
            return null;
        }
        var existingOwners = await _context.Owners
            .Where(x => ownerIds.Contains(x.Id))
            .ToListAsync();

        var newProperty = property.MapToProperty();
        newProperty.Id= Guid.NewGuid();
        newProperty.Owners = existingOwners;

        var createdProperty = await _propertyRepo.Create(newProperty, existingOwners);

        if (createdProperty == null ) {
            return null;
        }
        return createdProperty.MapToPropertyDtos();

    }

    public async Task<PropertyDtoResponse?> UpdateAsync(PropertyDtoRequest property)
    {

        var ownerIds = property.OwnersIds;
        // Update the Owners navigation property
        if (ownerIds == null || ownerIds.IsNullOrEmpty())
        {
            return null;
        }
        // Fetch existing owners from the database based on provided list
        var existingOwners = await _context.Owners
                .Where(x => ownerIds.Contains(x.Id))
                .ToListAsync();


        var updatedProperty = await _propertyRepo.Update(property, existingOwners);

        if (updatedProperty == null)
        {
            return null;
        }
        
        return updatedProperty.MapToPropertyDtos();

    }

    public async Task<List<PropertyDtoResponse>> SearchAsync(string? E9, TypeOfProperty? type, string? vat)
    {
        var properties = await _propertyRepo.Search(E9,type,vat);
        return properties.MapToListOfPropertiesDtos();
    }

    public async Task<bool> DeleteAsync(Guid propertyid)
    {
        return await _propertyRepo.DeleteById(propertyid);

    }


}
