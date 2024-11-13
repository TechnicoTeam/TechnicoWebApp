using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata;
using Technico.Main.DTOs.RepairDtos;

using Technico.Main.Models;
using Technico.Main.Services;

namespace Technico.Main.Controllers;

public class RepairsController : Controller
{

    readonly IRepairService _repairService;
    readonly IPropertyService _propertyService;


    public RepairsController(IPropertyService propertyService,IRepairService repairService)
    {

        _repairService = repairService;
        _propertyService = propertyService;
    }

    [HttpGet("{controller}/{propertyId:guid}")]
    public async Task<IActionResult> Index()
    {
        var routeDataPropertyId = RouteData.Values["propertyId"]?.ToString();
        Console.WriteLine($"RouteData propertyId: {routeDataPropertyId}");

        if (!Guid.TryParse(routeDataPropertyId, out var propertyId))
        {
            return BadRequest("Invalid propertyId");
        }
        var propertyResponse = await _propertyService.GetByIdAsync(propertyId);
        List<RepairDto> repairsResponse = await _repairService.GetByIdAsync(propertyId);
        var propertyModelView = new PropertyRepairsViewModel
        {
            Property = propertyResponse,
            Repairs = repairsResponse 
        };
        return View("~/Views/Owner/Repairs.cshtml", propertyModelView);
    }

    //[HttpGet("{propertyId:guid}/repairs")]
    //public async Task<IActionResult> Index(Guid propertyId)
    //{
    //    var propertyResponse = await _propertyService.GetByIdAsync(propertyId);
    //    var repairsResponse = await _repairService.GetByIdAsync(propertyId);
    //    var propertyModelView = new PropertyRepairsViewModel
    //    {
    //        Property = propertyResponse,
    //        Repairs = repairsResponse
    //    };
    //    return View("~/Views/Owner/Repairs.cshtml",propertyResponse);
    //}
}
