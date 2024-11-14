using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Technico.Main.DTOs.RepairDtos;
using Technico.Main.Models;
using Technico.Main.Services;

namespace Technico.Main.Controllers
{
    public class AdminRepairsController : Controller
    {

        readonly IRepairService _repairService;
   


        public AdminRepairsController(IPropertyService propertyService, IRepairService repairService)
        {

            _repairService = repairService;
           
        }

        [HttpGet("{controller}")]
        public async Task<IActionResult> Index()
        {

            List<RepairDto> repairsResponse = await _repairService.GetAllAsync();
            var repairsViewModel = new RepairsViewModel
            {
                Repairs = repairsResponse
            };

            return View("~/Views/AdminRepair/index.cshtml", repairsViewModel);
        }
    }
}
