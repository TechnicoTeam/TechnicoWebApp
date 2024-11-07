using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Technico.Main.Models;

namespace Technico.Main.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly List<Profile> _profiles = new List<Profile>   ///Here  will be the database connection
        {
            new Profile
            {
                Firstname = "Makis",
                Lastname = "Kotsmpasis",
                Email = "example@mail.com",
                Phone = "6948407861",
                Address = "Dwdekanhsou 16, Drama",
                Vat = "113900386"
            }
        };


        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
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
        public IActionResult Profile()
        {
            var profile = _profiles.FirstOrDefault(); // Get the first profile in the list
            if (profile == null)
            {
                // Optional: handle the case when profile data is not found
                return NotFound("Profile not found.");
            }

            return View(profile); // Pass the profile object to the view
        }

        [HttpGet]
        public IActionResult LogOut()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
       
    }
}
