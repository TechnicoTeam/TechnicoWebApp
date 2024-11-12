using Technico.Main.Models.Enums;
using Technico.Main.Models;

namespace Technico.Main.DTOs.PropertyDtos;

public class PropertyDtoResponse
{
    public Guid Id { get; set; }
    public string Address { get; set; } = string.Empty;
    public int ConstructionYear { get; set; } // this could be an int
    public string E9 { get; set; } = string.Empty;
    public TypeOfProperty Type { get; set; }
    public List<PropertyOwnerDtoResponse> Owners { get; set; } = [];
}


