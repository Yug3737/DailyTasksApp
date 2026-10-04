namespace DailyTasksApp.Models;

public class WeeklyTasks
{
    public Guid Id { get; set; }
    public int WorkingDays { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    
    public List<DailyTasks> DailyTasksList { get; set; }
}