using System.Windows;
using PricklyApp.Models;

namespace PricklyApp.ViewModels;

public class AddProjectViewModel
{
    private DatabaseManager _databaseManager;
    
    public AddProjectViewModel()
    {
        _databaseManager = new DatabaseManager(App.DatabaseConnectionString);
    }
    
    public bool AddProject(string projectName, string taskNames)
    {
        var taskNamesList = taskNames.Split().ToList();
        return _databaseManager.AddProject(projectName, taskNamesList);
    }
}