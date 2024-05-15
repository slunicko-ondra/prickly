using System.Collections.ObjectModel;
using PricklyApp.Models;

namespace PricklyApp.ViewModels;

public class MainWindowViewModel
{
    private DatabaseManager _databaseManager;
    public ObservableCollection<string> ProjectNames { get; }
    public ObservableCollection<string> TaskNames { get; }
    public DisplayTime DisplayTime { get; set; }
    private List<TodayInterval> _todayIntervals;
    private DateTime _today;
    
    public MainWindowViewModel()
    {
        _databaseManager = new DatabaseManager(App.Config.ConnectionStrings.ConnectionStrings["litedb"].ConnectionString);
        ProjectNames = new ObservableCollection<string>(GetProjectNames());
        TaskNames = new ObservableCollection<string>();
        DisplayTime = new DisplayTime();
        ResetDisplayTime();
        _todayIntervals = new List<TodayInterval>();
        _today = DateTime.Today;
    }
    
    private List<string> GetProjectNames()
    {
        var projects = _databaseManager.GetProjects();
        return projects.Select(p => p.Name).ToList();
    }
    
    private List<string> GetTasks(string projectName)
    {
        var project = _databaseManager.GetProjects().FirstOrDefault(p => p.Name == projectName);
        if (project == null)
        {
            return [];
        }
        return project.Tasks.Select(t => t.Name).ToList();
    }
    
    public void UpdateProjectNames()
    {
        ProjectNames.Clear();
        var projects = GetProjectNames();
        foreach (var project in projects)
        {
            TaskNames.Add(project);
        }
    }
    
    public void UpdateTaskNames(string? projectName=null)
    {
        TaskNames.Clear();
        if (projectName == null) return;
        var tasks = GetTasks(projectName);
        foreach (var task in tasks)
        {
            TaskNames.Add(task);
        }
    }

    public bool StartWork(string projectName, string taskName)
    {
        return _databaseManager.AddInterval(projectName, taskName, new WorkInterval(DateTime.Now));
    }
    
    public bool StopWork(string projectName, string taskName, DateTime? end=null)
    {
        var endTime = end ?? DateTime.Now;
        return _databaseManager.StopInterval(projectName, taskName, endTime);
    }
    
    public void UpdateDisplayTime(string projectName, string taskName)
    {
        var intervals = _databaseManager.GetIntervals(projectName, taskName);
        var todayCreatedAndEnded = intervals
            .Where(i => i.CreatedAt.Date == DateTime.Today && (i.End?.Date == DateTime.Today || (i.End?.Date == null && i.Start?.Date == null)))
            .Select(i => new TodayInterval(i.CreatedAt, i.Duration ?? TimeSpan.Zero))
            .ToList();
        var todayEnded = intervals
            .Where(i => i.CreatedAt.Date != DateTime.Today && i.End?.Date == DateTime.Today)
            .Select(i => new TodayInterval(i.End ?? DateTime.Today, DateTime.Today - i.End ?? TimeSpan.Zero))
            .ToList();
        var total = todayCreatedAndEnded.Concat(todayEnded)
            .Select(i => i.Duration)
            .Aggregate(TimeSpan.Zero, (acc, duration) => acc + duration);
        total += _databaseManager.GetIntervals(projectName, taskName)
            .Where(i => i.CreatedAt.Date == DateTime.Today && i.End == null && i.Duration == null)
            .Select(i => DateTime.Now - i.CreatedAt)
            .Aggregate(TimeSpan.Zero, (acc, duration) => acc + duration);
        DisplayTime.Time = total;
    }
    
    private void ResetDisplayTime()
    {
        DisplayTime.Time = TimeSpan.Zero;
    }
    
    public void StopAllWork()
    {
        var projects = _databaseManager.GetProjects();
        foreach (var project in projects)
        {
            foreach (var task in project.Tasks)
            {
                if (task.WorkIntervals.LastOrDefault(i => i.End == null) != null)
                {
                    StopWork(project.Name, task.Name);
                }
            }
        }
    }
    
    private void GetTodayIntervals(string projectName, string taskName)
    {
        var intervals = _databaseManager.GetIntervals(projectName, taskName);
        var todayCreatedAndEnded = intervals
            .Where(i => i.CreatedAt.Date == DateTime.Today && i.End?.Date == DateTime.Today)
            .Select(i => new TodayInterval(i.CreatedAt, i.Duration ?? TimeSpan.Zero))
            .ToList();
        var todayEnded = intervals
            .Where(i => i.CreatedAt.Date != DateTime.Today && i.End?.Date == DateTime.Today)
            .Select(i => new TodayInterval(i.End ?? DateTime.Today, DateTime.Today - i.End ?? TimeSpan.Zero))
            .ToList();
        _todayIntervals = todayCreatedAndEnded.Concat(todayEnded).ToList();
        _today = DateTime.Today;
    }
}