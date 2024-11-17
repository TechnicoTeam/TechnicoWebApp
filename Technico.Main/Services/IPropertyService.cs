using Technico.Main.DTOs.PropertyDtos;
using Technico.Main.Models;
using Technico.Main.Models.Enums;
namespace Technico.Main.Services;

public interface IPropertyService
{
    public Task<List<PropertyDtoResponse>> GetAllAsync();
    public Task<List<PropertyDtoResponse>> GetAllAsync(Guid ownerId);
    public Task<PropertyDtoResponse?> GetByIdAsync(Guid propertyId);

    public Task<PropertyDtoResponse?> CreateAsync(PropertyDtoCreateRequest property);
    public Task<PropertyDtoResponse?> UpdateAsync(PropertyDtoUpdateRequest property);
    public Task<List<PropertyDtoResponse>> SearchAsync(string? E9, TypeOfProperty? type, string? vat);
    public Task<bool> DeleteAsync(Guid propertyid);
    Task<PropertyDtoResponse?> AddOwnerToPropertyAsync(Guid propertyId, Guid ownerId);
}
