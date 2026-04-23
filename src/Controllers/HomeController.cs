using Microsoft.AspNetCore.Mvc;

namespace WarehouseManagementSchool.Controllers;

public sealed class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Error()
    {
        return View();
    }
}
