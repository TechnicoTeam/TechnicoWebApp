using Microsoft.EntityFrameworkCore;
using Technico.Main.DTOs.RepairDtos;
using Technico.Main.Models.Enums;

namespace Technico.Main.Models;

public class Repair
{
    public Guid Id { get; set; }
    public string? Vat { get; set; }

    required public TypeOfRepair Type { get; set; }
    public string Description { get; set; } = string.Empty;

    public StatusOfRepair Status { get; set; } = StatusOfRepair.Pending;

    [Precision(8, 2)]
    public decimal Cost { get; set; }

    required public Property Property { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? ScheduledAt { get; set; }
    
}
