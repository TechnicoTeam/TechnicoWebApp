using Technico.Main.DTOs.RepairDtos;

namespace Technico.Main.Models;

public class RepairsViewModel
{
    public List<RepairDto> Repairs { get; set; } = new List<RepairDto>();
    public Guid? ownerId { get; set; }
}
