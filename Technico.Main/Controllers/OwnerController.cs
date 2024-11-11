using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Technico.Main.Models.Enums;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Technico.Main.DTOs;
using Technico.Main.Models;
using Technico.Main.Services;
using Technico.Main.Models.Domain;

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

}