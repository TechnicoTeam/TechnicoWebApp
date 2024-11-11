using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Technico.Main.Models.Enums;
using static System.Runtime.InteropServices.JavaScript.JSType;

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

        private List<Owner> _owners = new List<Owner>
        {
            new Owner
            {
                Id = Guid.NewGuid(),
                Vat = "123456789",
                Firstname = "John",
                Lastname = "Doe",
                Email = "john.doe@example.com",
                Password = "mypassword123",
                Phone = "1234567890",
                Address = "123 Main St, Anytown USA",
                Role = TypeOfUser.Admin
            },
            new Owner
            {
                Id = Guid.NewGuid(),
                Vat = "987654321",
                Firstname = "Jane",
                Lastname = "Smith",
                Email = "jane.smith@example.com",
                Password = "mypassword456",
                Phone = "0987654321",
                Address = "456 Oak Rd, Someplace",
                Role = TypeOfUser.User
            },
            new Owner
            {
                Id = Guid.NewGuid(),
                Vat = "123789456",
                Firstname = "Bob",
                Lastname = "Johnson",
                Email = "bob.johnson@example.com",
                Password = "mypassword789",
                Phone = "5555555555",
                Address = "789 Elm St, Elsewhere",
                Role = TypeOfUser.Undedified
            }
        };

        [HttpGet("AdminView")]
        public IActionResult AdminView()
        {
            return View(_owners);
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