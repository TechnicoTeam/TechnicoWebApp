using Technico.Main.DTOs.PropertyDtos;
using Technico.Main.Models.Domain;
using Technico.Main.Models.Enums;

namespace Technico.Main.Mappers;

public static class PropertyMapper
{
    public static List<PropertyDtoResponse> MapToListOfPropertiesDtos( this List<Property> properties)
    {
        return properties.Select(p => new PropertyDtoResponse
        {
            Id = p.Id,
            Address = p.Address,
            ConstructionYear = p.ConstructionYear,
            E9 = p.E9,
            Type = p.Type,
            Owners = p.Owners.Select(o => new PropertyOwnerDtoResponse
            {
                Id = o.Id,
                Firstname = o.Firstname,
                Lastname = o.Lastname,
                Vat = o.Vat,
               
            }).ToList()
        }).ToList();
    }

    public static PropertyDtoResponse MapToPropertyDtos(this Property property)
    {
        return new PropertyDtoResponse
        {
            Id = property.Id,
            Address = property.Address,
            ConstructionYear = property.ConstructionYear,
            E9 = property.E9,
            Type = property.Type,
            Owners = property.Owners.Select(o => new PropertyOwnerDtoResponse
            {
                Id = o.Id,
                Firstname = o.Firstname,
                Lastname = o.Lastname,
                Email = o.Email,
                Phone = o.Phone,
                Vat = o.Vat,
                Address = o.Address

            }).ToList()
        };
    }

    public static Property MapToPropertyUpdate(this PropertyDtoUpdateRequest property) {


        return new Property
        {
            Address = property.Address,
            ConstructionYear = (int)property.ConstructionYear,
            E9 = string.Empty,
            Type = property.Type
        };
    }

    public static Property MapToPropertyCreate(this PropertyDtoCreateRequest property)
    {


        return new Property
        {
            Address = property.Address,
            ConstructionYear = (int)property.ConstructionYear,
            E9 = property.E9,
            Type = property.Type
        };
    }

}



