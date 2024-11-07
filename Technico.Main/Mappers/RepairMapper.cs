using Technico.Main.DTOs;
using Technico.Main.Models;

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
            PropertyId = repair.Property.Id
        };

        return dto;
    }
}
