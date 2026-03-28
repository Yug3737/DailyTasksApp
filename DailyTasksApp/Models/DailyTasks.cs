namespace DailyTasksApp.Models;

public class DailyTasks
{
    public Guid Id { get; set; }
    public string DayOfWeek { get; set; }
    public string Message { get; set; }
    public string GifPath { get; set; }
    public int TasksDone { get; set; }
    public List<SingleTask> TaskList { get; set; }
    
    public Guid WeeklyTasksId { get; set; }
    public WeeklyTasks WeeklyTask { get; set; }
}