using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Technico.Main.DTOs;
using Technico.Main.Models;
using Technico.Main.Services;

namespace Technico.Main.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OwnerController : ControllerBase
    {
       private readonly IOwnerService _ownerService;

        public OwnerController(IOwnerService ownerService)
        {
            _ownerService = ownerService;
        }
        [HttpPost]
        public async Task<IActionResult> CreateOwner(OwnerDtoRequest owner)
        {
            var response = await _ownerService.Create(owner);
            if (response == null) return BadRequest(response);
          return Ok(response);
        }
        [HttpGet]
        public async Task<IActionResult> GetAllOwners()
        {
            var response = await _ownerService.GetAllOwners();
            if (response == null) return BadRequest(response);
            return Ok(response);
        }
        //[HttpGet]
        //public async Task<IActionResult> GetOwnerByVAT(string VAT)
        //{
        //    var response = await _ownerService.GetOwnerByVAT(VAT);
        //    if (response == null) return BadRequest(response);
        //    return Ok(response);
        //}
    }
}
