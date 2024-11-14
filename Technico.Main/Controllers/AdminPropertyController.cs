using Microsoft.AspNetCore.Mvc;
using Technico.Main.DTOs;
using Technico.Main.DTOs.PropertyDtos;
using Technico.Main.Models;
using Technico.Main.Models.Domain;
using Technico.Main.Services;
using Technico.Main.Services.Implementations;

namespace Technico.Main.Controllers
{
    public class AdminPropertyController(IPropertyService _propertyService, IOwnerService _ownerService) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var properties = await _propertyService.GetAllAsync();
            return View(properties);
        }

        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var result = await _propertyService.DeleteAsync(id);

            return RedirectToAction("Index");
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(AdminPropertyCreateModel propertyViewModel)
        {
            var owner = await _ownerService.GetOwnerByVAT(propertyViewModel.Vat);

            if(owner is null)
            {
                return View("NotFoundView");
            }

            PropertyDtoCreateRequest property = new PropertyDtoCreateRequest {
                E9 = propertyViewModel.E9,
                ConstructionYear = propertyViewModel.ConstructionYear,
                Address = propertyViewModel.Address,
                Type = propertyViewModel.Type,
                OwnersIds = [ owner.Id ]
            };
           await _propertyService.CreateAsync(property);
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Update(Guid id)
        {
            var property = await _propertyService.GetByIdAsync(id);
            if(property == null) return NotFound();
            var propertyUpdateModel = new AdminPropertyUpdateModel
            {
                ConstructionYear = property.ConstructionYear,
                Address = property.Address,
                Type = property.Type,
                Vat = property.Owners.SingleOrDefault().Vat
            };
            return View(propertyUpdateModel);
        }

        [HttpPost]
        public async Task<IActionResult> Update([FromRoute]Guid id, AdminPropertyUpdateModel propertyViewModel)
        {
            var owner = await _ownerService.GetOwnerByVAT(propertyViewModel.Vat);
            if (owner == null) return NotFound();
            var property = new PropertyDtoUpdateRequest
            {
                Id = id,
                ConstructionYear = propertyViewModel.ConstructionYear,
                Address = propertyViewModel.Address,
                Type = propertyViewModel.Type,
                OwnersIds = [owner.Id]
            };
            var result = await _propertyService.UpdateAsync(property);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Search(string? e9, string? vat)
        {
            var result = await _propertyService.SearchAsync(e9, null, vat);
            return View("Search", result);
        }
    }
}

