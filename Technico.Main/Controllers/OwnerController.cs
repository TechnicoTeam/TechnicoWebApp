using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Technico.Main.DTOs;
using Technico.Main.Models;
using Technico.Main.Services;

namespace Technico.Main.Controllers
{
    public class OwnerController : Controller
    {
         readonly IOwnerService _ownerService;

        public OwnerController(IOwnerService ownerService)
        {
            _ownerService = ownerService;
        }

        [HttpGet("Profile")]
        public async Task<IActionResult> Profile()
        {
            // Ensure this profile object is properly instantiated and not null
            var ownerResponse = await _ownerService.GetOwnerByVAT("123455");
            var ownerModelView = new OwnerViewModel
            {
                Owner = ownerResponse
            };

            
            return View(ownerModelView); 
        }

     

    }

}