using Technico.Main.DTOs;
using Technico.Main.DTOs.PropertyDtos;
using Technico.Main.Models.Domain;

namespace Technico.Main.Mappers;
//TODO: maybe need to change the implementation to AutoMapper ???
public static class OwnerMapper
{
    public static OwnerDtoRequest ConvertToOwnerDtoRequest(this Owner owner)
    {
        return new OwnerDtoRequest
        {
            Vat = owner.Vat,
            Firstname = owner.Firstname,
            Lastname = owner.Lastname,
            Email = owner.Email,
            Phone = owner.Phone,
            Address = owner.Address,
            Role = owner.Role,
            Password = owner.Password,
        };
    }
    public static OwnerDtoResponse ConvertToOwnerDtoResponse(this Owner owner)
    {
        return new OwnerDtoResponse
        {
            Id = owner.Id,
            Vat = owner.Vat,
            Firstname = owner.Firstname,
            Lastname = owner.Lastname,
            Email = owner.Email,
            Phone = owner.Phone,
            Address = owner.Address,
            Role = owner.Role,
        };
    }
    public static Owner ConvertToOwner(this OwnerDtoRequest owner)
    {
        return new Owner
        {
            Vat = owner.Vat,
            Firstname = owner.Firstname,
            Lastname = owner.Lastname,
            Email = owner.Email,
            Phone = owner.Phone,
            Address = owner.Address,
            Role = owner.Role,
            Password = owner.Password,
        };
    }

    public static List<OwnerDtoResponse> MapToOwnersDtoResponse(this List<Owner> owners)
    {
        // Handle null or empty list
        if (owners == null || !owners.Any())
        {
            return new List<OwnerDtoResponse>();
        }

        return owners.Select(owner => new OwnerDtoResponse
        {
            Id = owner.Id,
            Firstname = owner.Firstname,
            Lastname = owner.Lastname,
            Email = owner.Email,
            Vat = owner.Vat,
            Phone = owner.Phone,
            Role = owner.Role
        }).ToList();
    }

}