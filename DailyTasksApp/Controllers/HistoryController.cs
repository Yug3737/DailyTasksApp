using Microsoft.AspNetCore.Mvc;

namespace DailyTasksApp.Controllers;

public class HistoryController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}