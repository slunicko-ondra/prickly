using System.Collections.ObjectModel;
using PricklyApp.Models;

namespace PricklyApp.ViewModels;

public class MainWindowViewModel
{
    private DatabaseManager _databaseManager;
    public ObservableCollection<string> ProjectNames { get; }
    public ObservableCollection<string> TaskNames { get; }
    
    public MainWindowViewModel()
    {
        _databaseManager = new DatabaseManager(App.DatabaseConnectionString);
        ProjectNames = new ObservableCollection<string>(GetProjectNames());
        TaskNames = new ObservableCollection<string>();
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
}