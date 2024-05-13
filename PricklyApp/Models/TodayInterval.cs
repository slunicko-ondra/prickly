namespace PricklyApp.Models;

public class TodayInterval
{
    public DateTime CreatedAt { get; set; }
    public TimeSpan Duration { get; set; }
    
    public TodayInterval(DateTime createdAt, TimeSpan duration)
    {
        CreatedAt = createdAt;
        Duration = duration;
    }
}