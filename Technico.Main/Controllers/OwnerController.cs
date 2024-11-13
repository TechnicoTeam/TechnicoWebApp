﻿using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Technico.Main.Models.Enums;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Technico.Main.DTOs;
using Technico.Main.Models;
using Technico.Main.Services;
using Technico.Main.Models.Domain;
using Microsoft.AspNetCore.Authorization;
using System.Net;
using Azure.Core;

namespace Technico.Main.Controllers
{
    public class OwnerController : Controller
    {
         readonly IOwnerService _ownerService;

        public OwnerController(IOwnerService ownerService)
        {
            _ownerService = ownerService;
        }

        public class TokenRequest
        {
            public string Token { get; set; }
        }

        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            var id = Guid.Parse("83AE364C-68D9-420B-48E4-08DD02835C36");

            // Await the asynchronous call to get the owner
            var owner = await _ownerService.GetOwnerByVAT("amt");

            // Create a view model to send the owner data to the view
            var ownerModelView = new OwnerViewModel
            {
                Owner = owner
            };

            // Return the view with the model data
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


        [Route("{controller}/{id}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var result = await _ownerService.Delete(id);

            return RedirectToAction("AdminView");
        }

        //actually update object.
        [HttpPost]
        [Route("Owner/Update/{id}")]
        public async Task<IActionResult> Update(Guid id, OwnerDtoResponse owner)
        {
            var result = await _ownerService.Update(owner);
            return RedirectToAction("AdminView");
        }


        //view update page
        [Route("{controller}/{id}/update")]
        public async Task<IActionResult> UpdateOwnerPage([FromRoute]Guid id)
        {
            var owner = await _ownerService.GetByIdAsync(id);

            return View("UpdateOwnerPage", owner);
        }

        //returns the view for adding new owner
        [Route("admin/owner/add")]
        public async Task<IActionResult> AdminOwnerCreate()
        {
            return View("CreateOwnerPage");
        }

        //post for create
        [HttpPost]
        [Route("admin/owner/add")]
        public async Task<IActionResult> RegisterOwner(OwnerDtoRequest ownerDto)
        {
            var owner = await _ownerService.Create(ownerDto);
            var ownerModelView = new OwnerViewModel
            {
                Owner = owner
            };

            return RedirectToAction("AdminView");
        }
    }

}