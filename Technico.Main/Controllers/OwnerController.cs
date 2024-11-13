using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Technico.Main.Models.Enums;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Technico.Main.DTOs;
using Technico.Main.Models;
using Technico.Main.Services;
using Technico.Main.Models.Domain;
using Microsoft.AspNetCore.Authorization;

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
            return View("~/Views/Home/Login.cshtml");
        }


        [HttpGet("AdminView")]
        public async Task<IActionResult> AdminView()
        {
            var owners = await _ownerService.GetAllOwners();
            return View(owners.ToList());
        }

    }

}