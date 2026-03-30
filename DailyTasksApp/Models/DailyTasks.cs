using System.ComponentModel.DataAnnotations.Schema;

namespace DailyTasksApp.Models;

public class DailyTasks
{
    public Guid Id { get; set; }
    public string DayOfWeek { get; set; }
    public string Message { get; set; }
    public string GifPath { get; set; }

    [NotMapped] 
    public int TasksDone => TaskList?.Count(t => t.IsDone) ?? 0;
    public List<SingleTask> TaskList { get; set; }
    
    public Guid WeeklyTasksId { get; set; }
    public WeeklyTasks WeeklyTask { get; set; }
}