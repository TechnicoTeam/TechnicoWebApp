using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Technico.Main.Controllers
{
    public class OwnerController : Controller
    {
        private readonly List<Profile> _profiles = new List<Profile>
        {
            new Profile
            {
                Firstname = "makis",
                Lastname = "kotsmpasis",
                Email  = "example@mail.com",
                Phone = "6948407861",
                Address = "Dwdekanhsou 16,Drama",
                Vat = "113900386"
            }
        };

        [HttpGet("Profile")]
        public IActionResult Profile()
        {
            // Ensure this profile object is properly instantiated and not null
            var profile = _profiles.FirstOrDefault(); // Retrieve the first profile, if available

            if (profile == null)
            {
                // Provide a fallback to avoid a null reference
                profile = new Profile
                {
                    Firstname = "Makis",
                    Lastname = "Kotsmpasis",
                    Email = "example@mail.com",
                    Phone = "6948407861",
                    Address = "Dwdekanhsou 16, Drama",
                    Vat = "113900386"
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
    public class Profile
    {
        required public string Vat { get; set; }
        required public string Firstname { get; set; }
        required public string Lastname { get; set; }
        required public string Email { get; set; }

        required public string Phone { get; set; }

        public string Address { get; set; } = string.Empty; // the user is not required to fill this 
    }
}