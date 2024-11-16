using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
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


    public PropertyRepairsController(IPropertyService propertyService, IRepairService repairService)
    {

        _repairService = repairService;
        _propertyService = propertyService;
    }

    [HttpGet]
    public async Task<IActionResult> Index([FromQuery]Guid propertyId)
    {
      
        var propertyResponse = await _propertyService.GetByIdAsync(propertyId);
        List<RepairDto> repairsResponse = await _repairService.GetByPropertyAsync(propertyId);
        var propertyModelView = new PropertyRepairsViewModel
        {
            Property = propertyResponse,
            Repairs = repairsResponse
        };
        return View("~/Views/PropertyRepairs/Index.cshtml", propertyModelView);
    }


    [HttpGet]
    public async Task<IActionResult> Search(TypeOfRepair? type, StatusOfRepair? status, Guid propertyId)
    {
        // Validate property ID
        if (propertyId == Guid.Empty)
        {
            return BadRequest("Invalid property ID.");
        }
        var propertyResponse = await _propertyService.GetByIdAsync(propertyId);
        List<RepairDto> repairsResponse = await _repairService.SearchOwnerPropertyAsync(type, status, propertyId);

        var propertyModelView = new PropertyRepairsViewModel
        {
            Property = propertyResponse,
            Repairs = repairsResponse
        };

        return View("~/Views/PropertyRepairs/Index.cshtml", propertyModelView);
    }


    [HttpGet]
    public async Task<IActionResult> UpdateForm([FromQuery]Guid propertyId, [FromQuery]Guid repairId)
    {
        var repair = await _repairService.GetAsync(repairId);
        ViewData["Repair"]= repair;
        ViewData["PropertyId"] = propertyId;
        return View("~/Views/PropertyRepairs/Update.cshtml");
        
    }

    [HttpPost]
    public async Task<IActionResult> Update([FromQuery] Guid PropertyId,UpdateRepairDto repairDto)
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

        return RedirectToAction("Index", new { propertyId = PropertyId });
    }


    public async Task<IActionResult> Delete([FromRoute] Guid id, [FromQuery] Guid PropertyId)
    {

        if (id == Guid.Empty)
        {
            return BadRequest("Invalid Repair ID.");
        }
        if (PropertyId == Guid.Empty)
        {
            return BadRequest("Invalid Property ID.");
        }


        var result = await _repairService.DeleteAsync(id);

        if (!result)
        {
            return NotFound("Property not found or could not be deleted.");
        }

        return RedirectToAction("Index", new { propertyId = PropertyId });
    }

    [HttpGet]
    public IActionResult CreateForm([FromQuery] Guid propertyId )
    {
        ViewData["PropertyId"]=propertyId;
        return View("~/Views/PropertyRepairs/Create.cshtml");
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromQuery] Guid PropertyId,PostRepairDto repairDto)
    {
        var create = await _repairService.CreateAsync(repairDto);
        if (create == null)
        {
            return NotFound("not found the property's id.");
        }
        return RedirectToAction("Index",new { propertyId = PropertyId });
    }


    [HttpGet]
    public async Task<IActionResult> GetRepair(Guid repairId)
    {
        var repair = await _repairService.GetAsync(repairId);

        return View(repair);

    }
}
