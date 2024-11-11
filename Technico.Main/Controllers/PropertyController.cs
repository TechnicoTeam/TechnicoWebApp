using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Technico.Main.DTOs;
using Technico.Main.Models;
using Technico.Main.Models.Enums;
using Technico.Main.Services;


namespace Technico.Main.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PropertyController : ControllerBase
{
    private readonly IPropertyService _propertyService;

    public PropertyController(IPropertyService propertyService)
    {
        _propertyService = propertyService;
    }

    //Get all the properties 
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var properties = await _propertyService.GetAllAsync();

        if (properties == null || !properties.Any())
        {
            NotFound();
        }

        return Ok(properties);
    }

    //Get all the properties of a user
    [HttpGet("{ownerId:guid}")]
    public async Task<IActionResult> GetAll(Guid ownerId)
    {
        var properties = await _propertyService.GetAllAsync(ownerId);

        if (properties == null || !properties.Any())
        {
            NotFound();
        }

        return Ok(properties);
    }

    // Get a property by Id
    [HttpGet("property/{propertyId:guid}")]
    public async Task<IActionResult> GetById(Guid propertyId)
    {
        var property = await _propertyService.GetByIdAsync(propertyId);

        if (property == null)
        {
            NotFound("Not property found with the id.");
        }

        return Ok(property);
    }


    // Create a new property
    [HttpPost]
    public async Task<IActionResult> Create(PropertyDtoRequest property)
    {
        var response = await _propertyService.CreateAsync(property);
        if (response == null)
        {
            NotFound("Failed to create the property.");
        }
        return Ok(response);
    }

    // Update a property
    [HttpPatch]
    public async Task<IActionResult> Update([FromBody] PropertyDtoRequest property)
    {
        var response = await _propertyService.UpdateAsync(property);
        if (response == null)
        {
            NotFound("Failed to update the property.");
        }
        return Ok(response);
    }

    // Delete a property
    [HttpDelete("{propertyId:guid}")]
    public async Task<IActionResult> Delete(Guid propertyId)
    {
        var response = await _propertyService.DeleteAsync(propertyId);
        if (response == false)
        {
            NotFound("Failed to delete the property. Not property found with the id.");
        }
        return Ok("The property has been successfully deleted.");
    }

    //Search Properties
   [HttpGet, Route("search")]
    public async Task<IActionResult> Search(string? propertyE9 = null, TypeOfProperty? propertyType = null, string? OwnerVat = null)
    {
        var properties = await _propertyService.SearchAsync(propertyE9, propertyType, OwnerVat);

        if (properties == null || !properties.Any())
        {
            NotFound();
        }

        return Ok(properties);

    }


}
