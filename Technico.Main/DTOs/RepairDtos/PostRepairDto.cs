using Technico.Main.Models.Enums;

namespace Technico.Main.DTOs.RepairDtos;

public class PostRepairDto
{
    public TypeOfRepair Type { get; set; }
    public string Description { get; set; } = string.Empty;
    public StatusOfRepair Status { get; set; } = StatusOfRepair.Pending;

    public decimal Cost { get; set; }

    public Guid PropertyId { get; set; }
}
