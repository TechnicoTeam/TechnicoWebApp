using Technico.Main.Data;
using Technico.Main.Models.Enums;
using Technico.Main.Repositories;
using Microsoft.EntityFrameworkCore;
using Technico.Main.Mappers;
using Microsoft.IdentityModel.Tokens;
using Technico.Main.DTOs.PropertyDtos;
using Technico.Main.Validators;
using Technico.Main.Models.Domain;

namespace Technico.Main.Services.Implementations;

public class PropertyService : IPropertyService
{

   private readonly IPropertyRepository _propertyRepo;
   private readonly IPropertyValidator _validator;
   private readonly TechnicoDbContext _context;
   private readonly ILogger<Property> _logger;

    public PropertyService(IPropertyRepository propertyRepo, TechnicoDbContext context, IPropertyValidator validator, ILogger <Property> logger)
    {
        _propertyRepo = propertyRepo;
        _context = context;
        _validator = validator;
        _logger = logger;
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

        if (property == null) return null;

        return property.MapToPropertyDtos();
    }

    public async Task<PropertyDtoResponse?> CreateAsync(PropertyDtoCreateRequest property)
    {
        
        // check if the E9 is valid
        var validE9 =  _validator.E9Valid(property.E9);
        if (validE9 is null) {
            _logger.LogWarning("The Create Failed. Invalid E9.");
            return null; 
        }
        
        // Check if the property E9 is unique
        var existingE9 = await _context.Properties.AnyAsync(e => e.E9.ToLower() == validE9.ToLower());
        if (existingE9){
            _logger.LogWarning("The Create Failed. This E9 already exists.");
            return null;
        }
    
        // Check is Owners'Ids are valid
        var validOwnerIds = _validator.OwnerIdValid(property.OwnersIds);
        if (validOwnerIds is null) {
            _logger.LogWarning("The Create Failed. Invalid owners' Ids.");
            return null; 
        }
     

        var existingOwners = await _context.Owners
            .Where(x => validOwnerIds.Contains(x.Id))
            .ToListAsync();

        var newProperty = property.MapToPropertyCreate();
        newProperty.Id= Guid.NewGuid();
        newProperty.Owners = existingOwners;

        var createdProperty = await _propertyRepo.Create(newProperty, existingOwners);

        if (createdProperty == null ) return null;

        return createdProperty.MapToPropertyDtos();

    }

    public async Task<PropertyDtoResponse?> UpdateAsync(PropertyDtoUpdateRequest property)
    {
        // Check is Owners'Ids are valid
        var validOwnerIds = _validator.OwnerIdValid(property.OwnersIds);
        if (validOwnerIds is null)
        {
            _logger.LogWarning("The Update Failed. Invalid owners' Ids.");
            return null;
        }


        // Fetch existing owners from the database based on provided list
        var existingOwners = await _context.Owners
                .Where(x => validOwnerIds.Contains(x.Id))
                .ToListAsync();


        var updatedProperty = await _propertyRepo.Update(property, existingOwners);

        if (updatedProperty == null) return null;

        
        return updatedProperty.MapToPropertyDtos();

    }

    public async Task<List<PropertyDtoResponse>> SearchAsync(string? E9, TypeOfProperty? type, string? vat)
    {
        var properties = await _propertyRepo.Search(E9,type,vat);
        return properties.MapToListOfPropertiesDtos();
    }

    public async Task<bool> DeleteAsync(Guid propertyId)
    {
        return await _propertyRepo.DeleteById(propertyId);
    }

    public async Task<PropertyDtoResponse?> AddOwnerToPropertyAsync(Guid propertyId, Guid ownerId)
    {
        var owner = await _context.Owners.FindAsync(ownerId);
        if (owner == null) return null;

        var updatedProperty = await _propertyRepo.AddOwnerToProperty(propertyId, owner);
        if (updatedProperty == null) return null;

        return updatedProperty.MapToPropertyDtos();
    }
}
