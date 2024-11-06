using Microsoft.AspNetCore.Mvc;

namespace Technico.API.Controllers;

public class OwnerController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
