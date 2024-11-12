namespace Technico.Main.DTOs.PropertyDtos;

public class PropertyOwnerDtoResponse
{
    required public Guid Id { get; set; }
    public string Firstname { get; set; } = string.Empty;
    public string Lastname { get; set; } = string.Empty;

    required public string Vat { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;

    public string Email = string.Empty;
    public string Address { get; set; } = string.Empty;

}
