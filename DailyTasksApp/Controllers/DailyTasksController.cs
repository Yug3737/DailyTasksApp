using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DailyTasksApp.Models;

namespace DailyTasksApp.Controllers;

public class DailyTasksController : Controller
{
    private readonly AppDbContext _context;

    public DailyTasksController(AppDbContext context)
    {
        _context = context;
    }
    // GET
    // public IActionResult Index()
    // {
    //     return View();
    // }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SaveCheckedDailyTasks(Guid dailyTasksId, List<Guid> singleTaskIds)
    {
        Console.WriteLine($"Inside save post method");
        if (!ModelState.IsValid)
        {
            throw new Exception("Daily Tasks model is invalid");
        }

        // fetch all tasks in the SingleTaskTable that belong to DailyTasks Table Id = dailyTasksId
        var tasks = await _context.SingleTaskTable
            .Where(x => x.DailyTasksId == dailyTasksId)
            .ToListAsync();

        // foreach (var task in tasks)
        for(int i = 0; i < tasks.Count; ++i)
        {
            var task = tasks[i];
            task.IsDone = dailyTasksId != null && singleTaskIds.Contains(task.Id);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    public IActionResult ShowEditTasksModal(Guid currentDailyTaskId)
    {
        var currentDailyTask = _context.DailyTasksTable
            .Include(x => x.TaskList)
            .FirstOrDefault(x => x.Id == currentDailyTaskId);
        // the partial view method, evaluates the logic in the cshtml file and the returns only static html
        return PartialView("~/Views/Home/_EditSingleTasks.cshtml", currentDailyTask);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SaveEditedDailyTasks(DailyTasks updatedDailyTasks)
    {
        foreach (var st in updatedDailyTasks.TaskList)
        {
            var task = await _context.SingleTaskTable.FirstOrDefaultAsync(x => x.Id == st.Id);
            task.Title = st.Title;
        }
        await _context.SaveChangesAsync();
        return RedirectToAction("Index", "Home");
    }
}