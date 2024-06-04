using PricklyApp.Models;

namespace PricklyApp.ViewModels;

public class DetailViewModel
{
    private DatabaseManager _databaseManager;
    public string ProjectName { get; set; }
    public string TaskName { get; set; }
    public string ProjectTodayTime { get; set; }
    public string TaskTodayTime { get; set; }
    public string ProjectMonthTime { get; set; }
    public string TaskMonthTime { get; set; }
    public string ProjectTotalTime { get; set; }
    public string TaskTotalTime { get; set; }

    public DetailViewModel()
    {
        _databaseManager = new DatabaseManager(App.Config.ConnectionStrings.ConnectionStrings["litedb"].ConnectionString);
        ProjectName = "";
        TaskName = "";
        ProjectTodayTime = "0:00:00";
        TaskTodayTime = "0:00:00";
        ProjectMonthTime = "0:00:00";
        TaskMonthTime = "0:00:00";
        ProjectTotalTime = "0:00:00";
        TaskTotalTime = "0:00:00";
    }
    
    public DetailViewModel(string projectName, string taskName)
    {
        _databaseManager = new DatabaseManager(App.Config.ConnectionStrings.ConnectionStrings["litedb"].ConnectionString);
        ProjectName = projectName;
        TaskName = taskName;
        ProjectTodayTime = "0:00:00";
        TaskTodayTime = "0:00:00";
        ProjectMonthTime = "0:00:00";
        TaskMonthTime = "0:00:00";
        ProjectTotalTime = "0:00:00";
        TaskTotalTime = "0:00:00";
        SetData();
    }

    private void SetData()
    {
        var project = _databaseManager.GetProjects().First(p => p.Name == ProjectName);
        var task = project.Tasks.First(t => t.Name == TaskName);
        
        TaskTodayTime = TimeSpanToString(GetTaskTimeToday(task));
        ProjectTodayTime = TimeSpanToString(project.Tasks.Aggregate(TimeSpan.Zero, (acc, t) => acc + GetTaskTimeToday(t)));
        TaskMonthTime = TimeSpanToString(GetTaskTimeMonth(task));
        ProjectMonthTime = TimeSpanToString(project.Tasks.Aggregate(TimeSpan.Zero, (acc, t) => acc + GetTaskTimeMonth(t)));
        TaskTotalTime = TimeSpanToString(GetTaskTimeTotal(task));
        ProjectTotalTime = TimeSpanToString(project.Tasks.Aggregate(TimeSpan.Zero, (acc, t) => acc + GetTaskTimeTotal(t)));
    }

    private static TimeSpan GetTaskTimeToday(ProjectTask task)
    {
        var time = TimeSpan.Zero;
        foreach (var workInterval in task.WorkIntervals)
        {
            if (workInterval.CreatedAt.Date == DateTime.Today)
            {
                var duration = workInterval.Duration ?? TimeSpan.Zero;
                time += duration;
            }
        }

        return time;
    }
    
    private static TimeSpan GetTaskTimeMonth(ProjectTask task)
    {
        var time = TimeSpan.Zero;
        foreach (var workInterval in task.WorkIntervals)
        {
            if (workInterval.CreatedAt.Month == DateTime.Today.Month)
            {
                var duration = workInterval.Duration ?? TimeSpan.Zero;
                time += duration;
            }
        }

        return time;
    }
    
    private static TimeSpan GetTaskTimeTotal(ProjectTask task)
    {
        var time = TimeSpan.Zero;
        foreach (var workInterval in task.WorkIntervals)
        {
            var duration = workInterval.Duration ?? TimeSpan.Zero;
            time += duration;
        }

        return time;
    }
    
    private static string TimeSpanToString(TimeSpan timeSpan)
    {
        if (timeSpan.Days > 0)
        {
            return $"{timeSpan.Days*24 + timeSpan.Hours}:{timeSpan.Minutes}:{timeSpan.Seconds}";
        }
        return timeSpan.ToString(@"hh\:mm\:ss");
    }
}