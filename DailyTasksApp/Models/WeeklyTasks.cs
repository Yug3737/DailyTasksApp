namespace DailyTasksApp.Models;

public class WeeklyTasks
{
    public Guid Id { get; set; }
    public int WorkingDays { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    
    public List<DailyTasks> DailyTasksList { get; set; }
}