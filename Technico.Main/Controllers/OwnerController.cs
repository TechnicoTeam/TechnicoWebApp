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
        public IActionResult Profile()
        {
            // Ensure this profile object is properly instantiated and not null
            var profile = _ownerService.GetOwnerByVAT("11111");

            if (profile == null)
            {
                // Provide a fallback to avoid a null reference
                profile = new Profile
                {
                    Firstname = "",
                    Lastname = "",
                    Email = "",
                    Phone = "",
                    Address = "",
                    Vat = ""
                };
            }
            return View(profile); // Pass the profile object to the view
        }

        // POST request to update profile data
        [HttpPost("Profile")]
        public IActionResult Profile([FromBody] Profile updatedProfile)
        {
            if (updatedProfile != null)
            {
                // Here you would typically update the profile data in a database
                _profiles[0] = updatedProfile; // Update the in-memory profile for demonstration

                return Json(new { success = true, message = "Profile saved successfully!" });
            }

            return Json(new { success = false, message = "Failed to save profile." });
        }

    }

}