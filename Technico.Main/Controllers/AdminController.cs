using Microsoft.AspNetCore.Mvc;
using Technico.Main.Services;

namespace Technico.Main.Controllers
{
    public class AdminController(IPropertyService _propertyService) : Controller
    {
       
        public async Task<IActionResult> Index()
        {
            var properties = await _propertyService.GetAllAsync();
            return View(properties);
        }
    }
}
