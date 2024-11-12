using Technico.Main.Models.Enums;

namespace Technico.Main.DTOs.PropertyDtos;

public class PropertyDtoCreateRequest
{
    public string Address { get; set; } = string.Empty;
    public int ConstructionYear { get; set; }
    required public string E9 { get; set; }
    public TypeOfProperty Type { get; set; }
    required public List<Guid> OwnersIds { get; set; } = [];
}
