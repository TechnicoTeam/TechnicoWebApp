using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using Technico.Main.DTOs;
using Technico.Main.DTOs.RepairDtos;
using Technico.Main.Models;
using Technico.Main.Models.Enums;
using Technico.Main.Services;
using static Technico.Main.Controllers.AuthController;

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

    [HttpGet]
    public async Task<IActionResult> Index([FromQuery]Guid id)
    {
        List<RepairDto> repairsResponse = await _repairService.GetByOwnerAsync(id);
        var RepairsViewModel = new RepairsViewModel
        {
            Repairs = repairsResponse,
            ownerId = id
        };
        return View("~/Views/OwnerRepairs/Index.cshtml", RepairsViewModel);
    }


    [HttpGet]
    public async Task<IActionResult> Search(TypeOfRepair? type, StatusOfRepair? status, [FromRoute] Guid Id)
    {

        // Call the service with the resolved values
        List<RepairDto> repairResponse = await _repairService.SearchOwnerAsync(type, status, Id);

        var repairsViewModel = new RepairsViewModel
        {
            Repairs = repairResponse,
            ownerId = Id
        };

        return View("~/Views/OwnerRepairs/Index.cshtml", repairsViewModel);
    }


    [HttpGet]
    public async Task<IActionResult> UpdateForm(Guid userId, Guid repairId)
    {
        ViewData["UserId"] = userId;
        var repair = await _repairService.GetAsync(repairId);
        ViewData["Repair"] = repair;
        return View("~/Views/OwnerRepairs/Update.cshtml");
    }

    [HttpPost]
    public async Task<IActionResult> Update([FromRoute] Guid id,UpdateRepairDto repairDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest("Invalid data provided.");
        }

        var repairResponse = await _repairService.UpdateAsync(repairDto);

        return Redirect($"~/OwnerRepairs/Index?id={id}");
    }

  
    public async Task<IActionResult> Delete([FromRoute] Guid id, [FromQuery] Guid userid)
    {
        if (id == Guid.Empty)
        {
            return BadRequest("Invalid repair ID.");
        }

        var result = await _repairService.DeleteAsync(id);

        if (!result)
        {
            return NotFound("Repair not found or could not be deleted.");
        }

        return Redirect($"~/OwnerRepairs/Index?id={userid}");
    }

    [HttpGet]
    public async Task<IActionResult> Create([FromQuery] Guid id)
    {
        ViewData["UserId"] = id;
        var properties = await _propertyService.GetAllAsync(id);
        ViewData["Properties"] = properties;
        return View("Create");
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromRoute] Guid id,PostRepairDto repairDto)
    {
        var create = await _repairService.CreateAsync(repairDto);

        if (create == null)
        {
            return NotFound("not found the property's id.");
        }
        return Redirect($"/OwnerRepairs/Index?id={id}");
    }

    [HttpGet]
    public async Task<IActionResult> GetRepair(Guid repairId)
    {
        var repair = await _repairService.GetAsync(repairId);
       
        return View(repair);

    }
}
