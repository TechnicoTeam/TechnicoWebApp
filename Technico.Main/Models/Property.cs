﻿using Technico.Main.Models.Enums;

namespace Technico.Main.Models;

public class Property
{
    public Guid Id { get; set; }
    required public string Address { get; set; }
    required public int ConstructionYear { get; set; } // this could be an int

    public TypeOfProperty Type { get; set; }
    required public List<Owner> Owners { get; set; }
    public List<Repair> Repairs { get; set; } = [];
}
