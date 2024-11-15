using Technico.Main.DTOs.RepairDtos;
using Technico.Main.Models.Domain;

namespace Technico.Main.Mappers;


public static class RepairMapper
{
    public static RepairDto ConvertToDto(this Repair repair)
    {
        var dto = new RepairDto
        {
            Id = repair.Id,
            Type = repair.Type,
            Description = repair.Description,
            Status = repair.Status,
            Cost = repair.Cost,
            Property = repair.Property.MapToPropertyDtos(),
            CreatedAt = repair.CreatedAt,

        };
        //dto.Property = repair.Property.MapToPropertyDtos();

        return dto;
    }
    public static List<RepairDto> MapToListOfRepairDtos(this List<Repair> repairs)
    {
        return repairs.Select(p => new RepairDto
        {
            Id = p.Id,
            Type = p.Type,
            Description = p.Description,
            Status = p.Status,
            Cost = p.Cost,
            Property = p.Property.MapToPropertyDtos(),
            CreatedAt = p.CreatedAt,
        }).ToList();
    }

}
