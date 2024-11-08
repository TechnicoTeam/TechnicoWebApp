using Microsoft.AspNetCore.Mvc;

namespace Technico.Main.Controllers
{
    public class PropertiesController : Controller
    {

        public IActionResult MyProperties()
        {
            return View("~/Views/Owner/Properties.cshtml");
        }

        // Action method to handle form submission and pass data to the modal
        [HttpPost]
        public IActionResult MyProperties(string E9,string address, string yearOfConstruction,string propertyType,string vatNumber)
        {
            // Store the submitted name in ViewData
            ViewData["E9"] = E9;
            ViewData["address"] = address;
            ViewData["yearOfConstruction"] = yearOfConstruction;
            ViewData["propertyType"] = propertyType;
            ViewData["vatNumber"] = vatNumber;

            // Return the view with the updated ViewData
            return View("~/Views/Owner/Properties.cshtml");
        }

    }
}