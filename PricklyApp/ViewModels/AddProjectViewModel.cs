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
        if (projectName == "")
        {
            MessageBox.Show("Project name cannot be empty.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            return false;
        }
        
        var taskNamesList = taskNames
            .Split(App.Config.AppSettings.Settings["addProjectWindowTaskDelimiter"].Value)
            .Select(t => t.Trim())
            .Where(t => t != "")
            .ToList();
        
        if (taskNamesList.Count != taskNamesList.Distinct().Count())
        {
            MessageBox.Show("Task names must be unique.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            return false;
        }
        
        var result = _databaseManager.AddProject(projectName, taskNamesList);
        if (!result)
        {
            MessageBox.Show("Project or some tasks already exist.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        return result;
    }
}