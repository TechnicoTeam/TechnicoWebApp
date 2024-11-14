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
                ConstructionYear = 0,
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
    }
}