using Microsoft.AspNetCore.Mvc;

namespace DailyTasksApp.Controllers;

public class WeeklyTasksController : Controller
{
    // GET
    public IActionResult Index()
    {
        List<string> DaysOfWeek = new List<string>
            { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };

        ViewData["DaysOfWeek"] = DaysOfWeek;
        return View();
    }

    public IActionResult Edit()
    {
        return View();
    }
}