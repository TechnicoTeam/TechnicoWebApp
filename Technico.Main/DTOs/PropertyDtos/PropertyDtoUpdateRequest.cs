using Technico.Main.Models.Enums;

namespace Technico.Main.DTOs.PropertyDtos;

public class PropertyDtoUpdateRequest
{
    required public Guid Id { get; set; }
    public string Address { get; set; } = string.Empty;
    public int ConstructionYear { get; set; }
    public TypeOfProperty Type { get; set; }
    required public List<Guid> OwnersIds { get; set; } = [];
}
