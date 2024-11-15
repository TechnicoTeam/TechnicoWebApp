using Microsoft.AspNetCore.Mvc;
using Technico.Main.DTOs.RepairDtos;
using Technico.Main.Models;
using Technico.Main.Models.Enums;
using Technico.Main.Services;

namespace Technico.Main.Controllers
{
    public class AdminController(IRepairService _repairService) : Controller
    {
       
        public async Task<IActionResult> Index()
        {
            List<RepairDto> repairsResponse = await _repairService.SearchAdminAsync(null, StatusOfRepair.In_progress, null, null);
            var repairsViewModel = new RepairsViewModel
             {
                Repairs = repairsResponse
            };
            return View(repairsViewModel);
        }
    }
}
