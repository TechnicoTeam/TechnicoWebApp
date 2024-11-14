using Microsoft.AspNetCore.Mvc;
using Technico.Main.DTOs;
using Technico.Main.Models;
using Technico.Main.Services;
using Technico.Main.Services.Implementations;

namespace Technico.Main.Controllers;

public class AdminOwnerController : Controller
{
    private readonly IOwnerService _ownerService;

    public AdminOwnerController(IOwnerService ownerService)
    {
        _ownerService = ownerService;
    }

    public async Task<IActionResult> Index()
    {
        var owners = await _ownerService.GetAllOwners();
        return View(owners.ToList());
    }

      public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        var result = await _ownerService.Delete(id);

        return RedirectToAction("Index");
    }
    public  IActionResult Create()
    {
        return View(); 
    }

    [HttpPost]
    public async Task<IActionResult> Create(OwnerDtoRequest ownerDto)
    {
        var owner = await _ownerService.Create(ownerDto);
        var ownerModelView = new OwnerViewModel
        {
            Owner = owner
        };

        return RedirectToAction("Index");
    }
    public async Task<IActionResult> Update(Guid id)
    {
        var owner = await _ownerService.GetByIdAsync(id);
        return View(owner);
    }

    [HttpPost]
    public async Task<IActionResult> Update(Guid id, OwnerDtoResponse owner)
    {
        var result = await _ownerService.Update(owner);
        return RedirectToAction("Index");
    }


    [HttpPost]
    public async Task<IActionResult> Search(string? vat, string? email)
    {
        List<OwnerDtoResponse>? result = await _ownerService.Search(vat, email);

        return View("Search", result);
    }
}
