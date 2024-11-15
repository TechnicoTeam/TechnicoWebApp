using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata;
using Technico.Main.DTOs;
using Technico.Main.DTOs.RepairDtos;

using Technico.Main.Models;
using Technico.Main.Models.Domain;
using Technico.Main.Models.Enums;
using Technico.Main.Services;

namespace Technico.Main.Controllers;

public class PropertyRepairsController : Controller
{

    readonly IRepairService _repairService;
    readonly IPropertyService _propertyService;


    public PropertyRepairsController(IPropertyService propertyService,IRepairService repairService)
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
        List<RepairDto> repairsResponse = await _repairService.GetByPropertyAsync(propertyId);
        var propertyModelView = new PropertyRepairsViewModel
        {
            Property = propertyResponse,
            Repairs = repairsResponse 
        };
        return View("~/Views/Owner/PropertyRepairs.cshtml", propertyModelView);
    }


    [HttpGet]
    public async Task<IActionResult> Search(TypeOfRepair? type, StatusOfRepair? status, Guid propertyId)
    {

        var propertyResponse = await _propertyService.GetByIdAsync(propertyId);
        List<RepairDto> repairsResponse = await _repairService.SearchOwnerPropertyAsync(type, status, propertyId);

        var propertyModelView = new PropertyRepairsViewModel
        {
            Property = propertyResponse,
            Repairs = repairsResponse
        };

        return View("~/Views/Owner/PropertyRepairs.cshtml", propertyModelView);
    }

    [HttpPatch]
    public async Task<IActionResult> Update([FromBody] UpdateRepairDto repairDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest("Invalid data provided.");
        }

        var repairResponse = await _repairService.UpdateAsync(repairDto);

        if (repairResponse == null)
        {
            return BadRequest("Failed to update the repair.");
        }


        //TempData["SuccessMessage"] = "Repair updated successfully!";
        return RedirectToAction("Index");
    }

    [HttpDelete]
    public async Task<IActionResult> Delete(Guid repairId)
    {
        bool delete = await _repairService.DeleteAsync(repairId);
        //if (!delete)
        //{

        //}

        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> create(PostRepairDto repairDto)
    {
        var create = await _repairService.CreateAsync(repairDto);
        //if (create != null) {}
        return RedirectToAction("Index");
    }

    [HttpGet]
    public async Task<IActionResult> GetRepair(Guid repairId)
    {
        var repair = await _repairService.GetAsync(repairId);

        return View(repair);

    }
}
