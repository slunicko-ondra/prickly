using PricklyApp.Models;

namespace PricklyApp.ViewModels;

public class AddTaskViewModel
{
    private DatabaseManager _databaseManager;
    
    public AddTaskViewModel()
    {
        _databaseManager = new DatabaseManager(App.DatabaseConnectionString);
    }
    
    public bool AddTask(string projectName, string taskName)
    {
        return _databaseManager.AddTask(projectName, taskName);
    }
}