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
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            // Call the service to get the owner by Id
            var response = await _ownerService.GetByIdAsync(id);
            if (response == null) return BadRequest(response);
            return Ok(response);  // Return the found owner
        }

        [HttpPut]
        public async Task<IActionResult> UpdateOwner(OwnerDtoResponse owner)
        {

            // Call the service layer to update the owner
            var response = await _ownerService.Update(owner);


            // If the owner is not found or the update fails, return a 404
            if (response == null)
            {
                return NotFound("Owner not found.");
            }

            // Return the updated owner in the response
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
