using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DailyTasksApp.Controllers;

public class HistoryController : Controller
{
    private readonly AppDbContext _context;

    public HistoryController(AppDbContext context)
    {
        _context = context;
    }
    public IActionResult Index()
    {
        var pastWeeks = _context.WeeklyTasksTable
            .Include(w => w.DailyTasksList)
            .ThenInclude(d => d.TaskList).ToList();
        return View(pastWeeks);
    }

    public IActionResult PastWeek(string pastWeekId)
    {
        var pastWeek = _context.WeeklyTasksTable
            .Where(w => w.Id.ToString() == pastWeekId)
            .Include(w => w.DailyTasksList)
            .ThenInclude(d => d.TaskList)
            .FirstOrDefault();
        return View(pastWeek);
    }
}