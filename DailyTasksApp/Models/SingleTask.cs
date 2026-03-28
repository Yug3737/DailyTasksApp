namespace DailyTasksApp.Models;

public class SingleTask
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public bool IsDone { get; set; }
    
    public Guid DailyTasksId { get; set; }
    public DailyTasks DailyTask { get; set; }
}