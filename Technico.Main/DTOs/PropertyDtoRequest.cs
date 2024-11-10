using Technico.Main.Models.Enums;

namespace Technico.Main.DTOs;

public class PropertyDtoRequest
{
    public Guid Id { get; set; }
    public string Address { get; set; } = string.Empty;
    public int ConstructionYear { get; set; }
    public string? E9 { get; set; } 
    public TypeOfProperty Type { get; set; } 
    public List<Guid> OwnersIds { get; set; } = new List<Guid>();
}
