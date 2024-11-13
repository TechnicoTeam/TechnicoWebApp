using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics;
using Technico.Main.Models;
using Technico.Main.Services;
using Technico.Main.Services.Implementations;

namespace Technico.Main.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        readonly IOwnerService _ownerService;

        public HomeController(ILogger<HomeController> logger, IOwnerService ownerService)
        {
            _logger = logger;
            _ownerService = ownerService;
        }

        public IActionResult Index()
        {

            return View();
        }

        public IActionResult LogIn()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpGet]
        public IActionResult LogOut()
        {
            return View();
        }


        [HttpGet]
        public IActionResult Repairs()
        {
            return View("~/Views/Owner/Repairs.cshtml");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        [HttpGet]
        public IActionResult LoadModalContent(string folder, string viewName)
        {
            // Validate the input and ensure the view exists
            if (string.IsNullOrEmpty(folder) || string.IsNullOrEmpty(viewName))
            {
                return Content("Invalid parameters.");
            }

            // Build the view path dynamically
            var viewPath = $"~/Views/{folder}/{viewName}.cshtml";

            // Example of creating and passing the Profile model

            // Return the partial view with the Profile model
            return PartialView(viewPath);  // If the the view you are trying to return needs data you should include it otherwise it will bring up and error
        }




    }
}
