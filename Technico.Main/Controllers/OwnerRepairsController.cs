using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using Technico.Main.DTOs.RepairDtos;
using Technico.Main.Models;
using Technico.Main.Services;

namespace Technico.Main.Controllers;

public class OwnerRepairsController : Controller
{

    readonly IRepairService _repairService;
    readonly IPropertyService _propertyService;


    public OwnerRepairsController(IPropertyService propertyService, IRepairService repairService)
    {

        _repairService = repairService;
        _propertyService = propertyService;
    }

    [HttpGet("{controller}/{ownerId:guid}")]
    public async Task<IActionResult> Index()
    {
        var routeDataOwnerId = RouteData.Values["ownerId"]?.ToString();
        Console.WriteLine($"RouteData ownerId: {routeDataOwnerId}");

        if (!Guid.TryParse(routeDataOwnerId, out var ownerId))
        {
            return BadRequest("Invalid ownerId");
        }

        List<RepairDto> repairsResponse = await _repairService.GetByOwnerAsync(ownerId);
        var RepairsViewModel = new RepairsViewModel
        {
            Repairs = repairsResponse
        };
        return View("~/Views/Owner/Repairs.cshtml", RepairsViewModel);
    }
}
