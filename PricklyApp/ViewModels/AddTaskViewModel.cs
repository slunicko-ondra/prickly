using System.Windows;
using PricklyApp.Models;

namespace PricklyApp.ViewModels;

public class AddTaskViewModel
{
    private DatabaseManager _databaseManager;
    
    public AddTaskViewModel()
    {
        _databaseManager = new DatabaseManager(App.Config.ConnectionStrings.ConnectionStrings["litedb"].ConnectionString);
    }
    
    public bool AddTask(string projectName, string taskName)
    {
        if (taskName == "")
        {
            MessageBox.Show("Task name cannot be empty.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            return false;
        }
        var result = _databaseManager.AddTask(projectName, taskName);
        if (!result)
        {
            MessageBox.Show("Task already exists.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        return result;
    }
}