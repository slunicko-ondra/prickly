using System.Windows;
using PricklyApp.Models;

namespace PricklyApp.ViewModels;

public class AddProjectViewModel
{
    private DatabaseManager _databaseManager;
    
    public AddProjectViewModel()
    {
        _databaseManager = new DatabaseManager(App.Config.ConnectionStrings.ConnectionStrings["litedb"].ConnectionString);
    }
    
    public bool AddProject(string projectName, string taskNames)
    {
        var taskNamesList = taskNames
            .Split(',')
            .Select(t => t.Trim())
            .ToList();
        return _databaseManager.AddProject(projectName, taskNamesList);
    }
}