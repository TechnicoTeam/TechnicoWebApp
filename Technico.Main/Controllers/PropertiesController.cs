using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Technico.Main.DTOs;
using Technico.Main.DTOs.PropertyDtos;
using Technico.Main.Models;
using Technico.Main.Models.Enums;
using Technico.Main.Services;
using Technico.Main.Services.Implementations;

namespace Technico.Main.Controllers
{
    public class PropertiesController : Controller
    {

        readonly IPropertyService _propertyService;
        readonly IOwnerService _ownerService;

        public PropertiesController(IPropertyService propertyService, IOwnerService ownerService)
        {
            _propertyService = propertyService;
            _ownerService = ownerService;
        }

        [HttpGet]
        public async Task<IActionResult> MyProperties(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return Unauthorized("Id not found in request.");
            }

            var properties = await _propertyService.GetAllAsync(Guid.Parse(id));

            return View("~/Views/Owner/Properties.cshtml", properties);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest("Invalid property ID.");
            }

            var result = await _propertyService.DeleteAsync(id);

            if (!result)
            {
                return NotFound("Property not found or could not be deleted.");
            }

            return Ok("Property deleted successfully.");
        }

        [HttpGet]
        public IActionResult Create([FromQuery] Guid id)
        {
            var property = new PropertyDtoCreateRequest
            {
                Address = string.Empty,
                E9 = string.Empty,
                Type = 0,
                OwnersIds = [id]
            };
            return View("~/Views/Owner/CreateProperty.cshtml",property);
        }

        [HttpPost]
        public async Task<IActionResult> Create(PropertyDtoCreateRequest propertyViewModel,Guid id)
        {
            propertyViewModel.OwnersIds = [id];
            await _propertyService.CreateAsync(propertyViewModel);
            return Redirect($"/Properties/MyProperties?id={id}");
        }

        [HttpGet]
        public async Task<IActionResult> Update([FromQuery] Guid id)
        {
            var property = await _propertyService.GetByIdAsync(id);

            if (property == null)
            {
                return NotFound();
            }

            var propertyDtoUpdateRequest = new PropertyDtoUpdateRequest
            {
                Id = property.Id,
                Address = property.Address,
                ConstructionYear = property.ConstructionYear,
                Type = property.Type,
                OwnersIds = property.Owners.Select(o => o.Id).ToList()
            };

            return View("~/Views/Owner/UpdateProperty.cshtml", propertyDtoUpdateRequest);
        }


        [HttpPost]
        public async Task<IActionResult> Update(PropertyDtoUpdateRequest property)
        {
            await _propertyService.UpdateAsync(property);
            return Redirect($"/Properties/MyProperties?id={property.OwnersIds.FirstOrDefault()}");
        }
    }
}