using Microsoft.AspNetCore.Mvc;

namespace DailyTasksApp.Controllers;

public class WeeklyTasksController : Controller
{
    // GET
    public IActionResult Index()
    {
        List<string> DaysOfWeek = new List<string>();
        DaysOfWeek.Add("Monday");
        DaysOfWeek.Add("Tuesday");
        DaysOfWeek.Add("Wednesday");
        DaysOfWeek.Add("Thursday");
        DaysOfWeek.Add("Friday");
        DaysOfWeek.Add("Saturday");
        DaysOfWeek.Add("Sunday");

        ViewData["DaysOfWeek"] = DaysOfWeek;

        return View();
    }

    public IActionResult Edit()
    {
        return View();
    }
}