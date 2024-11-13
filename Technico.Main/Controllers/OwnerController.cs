using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Technico.Main.Models.Enums;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Technico.Main.DTOs;
using Technico.Main.Models;
using Technico.Main.Services;
using Technico.Main.Models.Domain;
using Microsoft.AspNetCore.Authorization;
using Technico.Main.Services.Implementations;

namespace Technico.Main.Controllers
{
    public class OwnerController : Controller
    {
        readonly IOwnerService _ownerService;

        public OwnerController(IOwnerService ownerService)
        {
            _ownerService = ownerService;
        }

        [HttpGet]
        public async Task<IActionResult> Profile(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return Unauthorized("Id not found in request.");
            }

            // Proceed with the rest of the code using the 'token'
            var owner = await _ownerService.GetByIdAsync(Guid.Parse(id));

            var ownerModelView = new OwnerViewModel
            {
                Owner = owner
            };

            return View("~/Views/Owner/Profile.cshtml", ownerModelView);
        }

        [HttpPost]
        public async Task<IActionResult> Register(OwnerDtoRequest ownerDto)
        {
            var owner = await _ownerService.Create(ownerDto);
            var ownerModelView = new OwnerViewModel
            {
                Owner = owner
            };

            return View("~/Views/Owner/Profile.cshtml", ownerModelView);
        }


        [HttpGet("AdminView")]
        public async Task<IActionResult> AdminView()
        {
            var owners = await _ownerService.GetAllOwners();
            return View(owners.ToList());
        }

        public async Task<IActionResult> Update(Guid id)
        {
            var owner = await _ownerService.GetByIdAsync(id);
            return View(owner);
        }

        [HttpPost]
        public async Task<IActionResult> Update(Guid id, OwnerDtoResponse owner)
        {
            var result = await _ownerService.Update(owner);
            var ownerModelView = new OwnerViewModel
            {
                Owner = result
            };

            return View("~/Views/Owner/Profile.cshtml", ownerModelView);
        }
        public async Task<IActionResult> Delete( Guid id)
        {
            var result = await _ownerService.Delete(id);

            return  View("~/Views/Home/LogIn.cshtml");
        }


    }
    

}