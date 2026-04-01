using System.Diagnostics;
using System.Runtime.InteropServices.JavaScript;
using Microsoft.AspNetCore.Mvc;
using DailyTasksApp.Models;
using Microsoft.EntityFrameworkCore;

namespace DailyTasksApp.Controllers;

public class HomeController : Controller
{
    private readonly AppDbContext _context;
    public HomeController(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var weeklyData = await _context.WeeklyTasksTable
            .Include(w => w.DailyTasksList)
            .ThenInclude(d => d.TaskList)
            .Where(w => w.StartDate <= DateTime.Today && w.EndDate >= DateTime.Today)
            .FirstOrDefaultAsync();

        // for now this means that the last week has passed and
        // we need to copy last week's tasks to this week
        if (weeklyData == null)
            weeklyData = await CreateInitialWeekAsync();
        
        return View(weeklyData);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ResetWeek()
    {
        var lastWeeklyTask = await _context.WeeklyTasksTable.OrderByDescending(w => w.EndDate).FirstOrDefaultAsync();
        
        // only populate data if last end date is before today
        if (lastWeeklyTask == null || lastWeeklyTask.EndDate > DateTime.Today)
            return null;
        var lastWeeklyTaskId = lastWeeklyTask.Id;
        var oldDailyTasks = _context.DailyTasksTable.Where(dt => dt.WeeklyTasksId == lastWeeklyTaskId).Include(dt => dt.TaskList);

        WeeklyTasks lastWeeklyTasks = _context.WeeklyTasksTable.Where(w => w.Id == lastWeeklyTaskId).Single();
        WeeklyTasks newWeeklyTask = new WeeklyTasks
        {
            WorkingDays = lastWeeklyTasks.WorkingDays,
            StartDate = lastWeeklyTasks.StartDate.AddDays(7),
            EndDate = lastWeeklyTasks.EndDate.AddDays(7),
            DailyTasksList = new List<DailyTasks>()
        };
        
        _context.WeeklyTasksTable.Add(newWeeklyTask);
        _context.SaveChanges();

        // populate with new Daily Tasks
        foreach (var oldDailyTask in oldDailyTasks)
        {
            DailyTasks newDailyTask = new DailyTasks
            {
                WeeklyTasksId = newWeeklyTask.Id,
                DayOfWeek = oldDailyTask.DayOfWeek,
                Message = oldDailyTask.Message,
                GifPath = oldDailyTask.GifPath,
                TaskList = new List<SingleTask>()
            };
            
            List<SingleTask> newSingleTasks = new List<SingleTask>();
            foreach (var oldSingleTask in oldDailyTask.TaskList)
            {
                var newSingleTask = new SingleTask
                {
                    Title = oldSingleTask.Title,
                    IsDone = false,
                    DailyTask = newDailyTask
                };
                newSingleTasks.Add(newSingleTask);
                _context.SingleTaskTable.Add(newSingleTask);
            }
            
            newDailyTask.TaskList = newSingleTasks;
            _context.DailyTasksTable.Add(newDailyTask);
        }
        _context.SaveChanges();
        return RedirectToAction("Index");
    }

    private async Task<WeeklyTasks> CreateInitialWeekAsync()
    {
        var lastMonday = DateTime.Today;
        while (lastMonday.DayOfWeek != DayOfWeek.Monday)
        {
            lastMonday = lastMonday.AddDays(-1);
        }
        
        WeeklyTasks newWeeklyTask = new WeeklyTasks()
        {
            StartDate = lastMonday,
            EndDate = lastMonday.AddDays(6),
            WorkingDays = 7,
            DailyTasksList = new List<DailyTasks>()
        };

        _context.WeeklyTasksTable.Add(newWeeklyTask);
        await _context.SaveChangesAsync();

        var days = new List<string> { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };
        var genericTaskTitles = new List<string>
        {
            "Literature Reading",
            "CS article reading",
            "Interest programming",
            "TK business reading",
            "Random article",
            "Gym/5K steps & call a friend",
            "Course study",
            "Gita daily shlokas/meaning writing"
        };

        foreach (var day in days)
        {
            DailyTasks newDailyTask = new DailyTasks
            {
                WeeklyTasksId = newWeeklyTask.Id,
                DayOfWeek = day,
                Message = "Let's get started!",
                GifPath = "",
                TaskList = new List<SingleTask>()
            };

            _context.DailyTasksTable.Add(newDailyTask);
            await _context.SaveChangesAsync();

            foreach (var title in genericTaskTitles)
            {
                _context.SingleTaskTable.Add(new SingleTask
                {
                    Title = title,
                    IsDone = false,
                    DailyTask = newDailyTask
                });
            }
        }

        await _context.SaveChangesAsync();
        return newWeeklyTask;
    }
    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}