
using Technico.Main.DTOs;
using Technico.Main.Models;
using Technico.Main.Models.Enums;
namespace Technico.Main.Services;

public interface IPropertyService
{
    public Task<List<PropertyDtoResponse>> GetAllAsync();
    public Task<List<PropertyDtoResponse>> GetAllAsync(Guid ownerId);
    public Task<PropertyDtoResponse?> GetByIdAsync(Guid propertyId);

    public Task<PropertyDtoResponse?> CreateAsync(PropertyDtoRequest property);
    public Task<PropertyDtoResponse?> UpdateAsync(PropertyDtoRequest property);
    public Task<List<PropertyDtoResponse>> SearchAsync(string? E9, TypeOfProperty? type, string? vat);
    public Task<bool> DeleteAsync(Guid propertyid);
}
