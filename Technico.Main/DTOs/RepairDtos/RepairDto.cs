﻿using Microsoft.Build.Framework;
using Microsoft.EntityFrameworkCore;
using Technico.Main.DTOs.PropertyDtos;
using Technico.Main.Models.Enums;

namespace Technico.Main.DTOs.RepairDtos;

public class RepairDto
{
    public Guid Id { get; set; }

    public TypeOfRepair Type { get; set; }
    public string Description { get; set; } = string.Empty;

    public StatusOfRepair Status { get; set; } = StatusOfRepair.Pending;

    public decimal Cost { get; set; }

    public DateTime CreatedAt { get; set; }

    required public PropertyDtoResponse Property {  get; set; } 

}
