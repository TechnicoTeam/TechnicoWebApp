using Technico.Main.Models.Enums;

namespace Technico.Main.Models;

public class Property
{
    public Guid Id { get; set; }
    required public string Address { get; set; }
    required public int ConstructionYear { get; set; } // this could be an int
    required public string E9 { get; set; }

    public TypeOfProperty Type { get; set; }
    public List<Owner> Owners { get; set; } = [];
    public List<Repair> Repairs { get; set; } = [];
}
