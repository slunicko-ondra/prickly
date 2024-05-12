using System.Collections.ObjectModel;
using System.Windows.Threading;
using PricklyApp.Models;

namespace PricklyApp.ViewModels;

public class MainWindowViewModel
{
    private DatabaseManager _databaseManager;
    public ObservableCollection<string> ProjectNames { get; }
    public ObservableCollection<string> TaskNames { get; }
    public DisplayTime DisplayTime { get; set; }
    
    public MainWindowViewModel()
    {
        _databaseManager = new DatabaseManager(App.DatabaseConnectionString);
        ProjectNames = new ObservableCollection<string>(GetProjectNames());
        TaskNames = new ObservableCollection<string>();
        DisplayTime = new DisplayTime();
        UpdateDisplayTime();
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
    
    public void UpdateTaskNames(string projectName)
    {
        TaskNames.Clear();
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
    
    public bool StopWork(string projectName, string taskName)
    {
        return _databaseManager.StopInterval(projectName, taskName, DateTime.Now);
    }
    
    public void UpdateDisplayTime()
    {
        DisplayTime.Time = DateTime.Now.TimeOfDay;
    }
}