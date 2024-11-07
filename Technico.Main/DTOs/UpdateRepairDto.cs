﻿using Technico.Main.Models.Enums;

namespace Technico.Main.DTOs;

public class UpdateRepairDto
{
    public Guid Id { get; set; }

    public TypeOfRepair Type { get; set; }
    public string Description { get; set; } = string.Empty;

    public StatusOfRepair Status { get; set; } = StatusOfRepair.Pending;

    public decimal Cost { get; set; }
}
