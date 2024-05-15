using PricklyApp.Models;

namespace PricklyApp.ViewModels;

public class EditTimeViewModel
{
    private readonly DatabaseManager _databaseManager;
    private readonly string _projectName;
    private readonly string _taskName;
    public string time { get; set; }
    
    public EditTimeViewModel()
    {
        _databaseManager = new DatabaseManager(App.Config.ConnectionStrings.ConnectionStrings["litedb"].ConnectionString);
        _projectName = "";
        _taskName = "";
        time = "00:00:00";
    }
    
    public EditTimeViewModel(string projectName, string taskName)
    {
        _databaseManager = new DatabaseManager(App.Config.ConnectionStrings.ConnectionStrings["litedb"].ConnectionString);
        _projectName = projectName;
        _taskName = taskName;
        time = "00:00:00";
    }
    
    public bool AddTime(DateTime start, DateTime end)
    {
        if (start > end)
        {
            return false;
        }
        _databaseManager.AddInterval(_projectName, _taskName, new WorkInterval(start, end));
        return true;
    }
    
    public void AddTime(TimeSpan time)
    {
        _databaseManager.AddInterval(_projectName, _taskName, new WorkInterval(time));
    }
}