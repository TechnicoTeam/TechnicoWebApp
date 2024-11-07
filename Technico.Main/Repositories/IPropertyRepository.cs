using Technico.Main.Data;
using Microsoft.AspNetCore.Mvc;
using Technico.Main.Models;
using Technico.Main.Models.Enums;

namespace Technico.Main.Repositories;

public interface IPropertyRepository
{

    // Get all the properties of the Owner
    public Task<List<Property>> GetAll(Guid ownerId);

    // Get a specific owner's property with the repairs by the id of the property
    public Task<Property?> GetById(Guid propertyid);

    // Update a property
    public Task<Property?>Update(Property property);

    // Delete a Property 
    public Task<Property?> DeleteById(Guid propertyid);

    // Search a property by E9 or the type or the vat of the owner
    public Task<List<Property>> Search( string? E9, TypeOfProperty? type, string? vat);

    // create a property
    public Task<Property?> Create(Property property, Owner owner);

    public Task<Property?>AddCoOwner(Property property, Owner owner);
    


}
