﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using Technico.Main.DTOs;
using Technico.Main.DTOs.RepairDtos;
using Technico.Main.Models;
using Technico.Main.Models.Enums;
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
            Repairs = repairsResponse,
            ownerId =ownerId
        };
        return View("~/Views/Owner/Repairs.cshtml", RepairsViewModel);
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

        return View("~/Views/Owner/Repairs.cshtml", repairsViewModel);
    }

    //public async Task<IActionResult> Create([FromQuery] Guid OwnerId)
    //{
    //    return View()

    //}

    [HttpPatch]
    public async Task<IActionResult> Update(UpdateRepairDto repairDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest("Invalid data provided.");
        }

        var repairResponse = await _repairService.UpdateAsync(repairDto);

        //if (repairResponse == null){}
        //TempData["SuccessMessage"] = "Repair updated successfully!";
        return RedirectToAction("Index");
    }

    [HttpDelete]
    public async Task<IActionResult> Delete( Guid repairId)
    {
       bool delete = await _repairService.DeleteAsync(repairId);
        //if (!delete){}
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
