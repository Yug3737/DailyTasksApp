namespace DailyTasksApp.Utilities;

public static class Constants
{
    public static Dictionary<int, string> TaskCompletionMessages = new Dictionary<int, string>()
    {
        { 0, "Lets get started!" },
        { 1, "Keep going!" },
        { 2, "Keep going!" },
        { 3, "Keep going!" },
        { 4, "Don't give up!" },
        { 5, "Don't give up!" },
        { 6, "Almost there!" },
        { 7, "Almost there!" },
        { 8, "Killed it!" }
    };

    public static Dictionary<string, int> DayOfWeekNumbers = new Dictionary<string, int>()
    {
        { "Monday", 0 },
        { "Tuesday", 1 },
        { "Wednesday", 2 },
        { "Thursday", 3 },
        { "Friday", 4 },
        { "Saturday", 5 },
        { "Sunday", 6 }
    };
}