using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Technico.Main.DTOs;
using Technico.Main.DTOs.RepairDtos;
using Technico.Main.Models;
using Technico.Main.Models.Enums;
using Technico.Main.Services;
using Technico.Main.Services.Implementations;

namespace Technico.Main.Controllers;

public class AdminRepairsController : Controller
{

    readonly IRepairService _repairService;
    readonly IPropertyService _propertyService;


    public AdminRepairsController(IPropertyService propertyService, IRepairService repairService)
    {

        _repairService = repairService;
        _propertyService = propertyService;
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


    [HttpGet]
    public async Task<IActionResult> Search(string? vat, StatusOfRepair? status, DateTime? fromDate, DateTime? toDate)
    {
        // Provide default values if fromDate or toDate are null
        DateTime defaultFromDate = DateTime.MinValue;
        DateTime defaultToDate = DateTime.MaxValue;

        // Use nullable DateTime with the null-coalescing operator
        fromDate = fromDate ?? defaultFromDate;
        toDate = toDate ?? defaultToDate;

        // Call the service with the resolved values
        List<RepairDto> repairResponse = await _repairService.SearchAdminAsync(vat, status, fromDate.Value, toDate.Value);

        var repairsViewModel = new RepairsViewModel
        {
            Repairs = repairResponse
        };

        return View("~/Views/AdminRepair/index.cshtml", repairsViewModel);
    }


    [HttpGet]
    public async Task<IActionResult> UpdateForm(Guid repairId)
    {
        var repair = await _repairService.GetAsync(repairId);

        return View("~/Views/AdminRepair/Update.cshtml", repair);

    }

    [HttpPost]
    public async Task<IActionResult> Update(UpdateRepairDto repairDto)
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


    public async Task<IActionResult> Delete(Guid repairId)
    {
        if (repairId == Guid.Empty)
        {
            return BadRequest("Invalid repair ID.");
        }
        bool delete = await _repairService.DeleteAsync(repairId);
        if (!delete)
        {
            return NotFound("Repair not found or could not be deleted.");
        }

        return RedirectToAction("Index");
    }

    [HttpGet]
    public async Task<IActionResult> CreateForm()
    {
        var properties = await _propertyService.GetAllAsync();
        ViewData["Properties"] = properties;
        return View("~/Views/AdminRepair/Create.cshtml");
    }
   
    [HttpPost]
    public async Task<IActionResult> Create(PostRepairDto repairDto)
    {
        var create = await _repairService.CreateAsync(repairDto);
        if (create == null) {
            return NotFound("not found the property's id.");
        }
        return RedirectToAction("Index");
    }

}
