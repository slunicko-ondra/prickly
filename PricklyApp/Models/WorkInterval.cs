using LiteDB;

namespace PricklyApp.Models;

public class WorkInterval
{
    public ObjectId Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? Start { get; set; }
    public DateTime? End { get; set; }
    public TimeSpan? Duration { get; set; }
    
    public WorkInterval()
    {
        Id = ObjectId.NewObjectId();
        CreatedAt = DateTime.Now;
        Start = null;
        End = null;
        Duration = null;
    }
    
    public WorkInterval(DateTime start)
    {
        Id = ObjectId.NewObjectId();
        CreatedAt = start;
        Start = start;
        End = null;
        Duration = null;
    }
    
    public WorkInterval(DateTime start, DateTime end)
    {
        Id = ObjectId.NewObjectId();
        CreatedAt = start;
        Start = start;
        End = end;
        Duration = end - start;
    }
    
    public WorkInterval(TimeSpan duration)
    {
        Id = ObjectId.NewObjectId();
        CreatedAt = DateTime.Now;
        Start = null;
        End = null;
        Duration = duration;
    }
    
    public void Stop(DateTime end)
    {
        if (Duration != null)
        {
            throw new InvalidOperationException("Interval is already stopped.");
        }
        End = end;
        Duration = end - Start;
    }
}