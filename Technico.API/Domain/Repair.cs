using Microsoft.EntityFrameworkCore;
using Technico.API.Domain.Enums;

namespace Technico.API.Domain;

public class Repair
{
    public Guid Id { get; set; }

    required public TypeOfRepair Type { get; set; }
    public string Description { get; set; } = string.Empty;

    public StatusOfRepair Status { get; set; } = StatusOfRepair.Pending;

    [Precision(8, 2)]
    public decimal Cost { get; set; }

    required public Property Property { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;
}
