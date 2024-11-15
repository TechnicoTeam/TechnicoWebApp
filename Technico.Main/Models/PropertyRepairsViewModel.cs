using Technico.Main.DTOs;
using Technico.Main.DTOs.PropertyDtos;
using Technico.Main.DTOs.RepairDtos;


namespace Technico.Main.Models;

public class PropertyRepairsViewModel
{

    public PropertyDtoResponse? Property { get; set; }
    public List<RepairDto> Repairs { get; set; } = new List<RepairDto>();
}
