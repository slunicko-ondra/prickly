using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using PricklyApp.Models;
using PricklyApp.Utils;

namespace PricklyApp.ViewModels;

public class SettingsViewModel
{
    private readonly DatabaseManager _databaseManager;
    public ObservableCollection<string> ProjectNames { get; }
    public string[] TimeUnits { get; } = ["seconds", "minutes", "hours"];

    public string AfkTime { get; set; }
    
    public SettingsViewModel()
    {
        _databaseManager = new DatabaseManager(App.Config.ConnectionStrings.ConnectionStrings["litedb"].ConnectionString);
        ProjectNames = new ObservableCollection<string>(_databaseManager.GetProjects().Select(p => p.Name).ToList());
        AfkTime = App.Config.AppSettings.Settings["afkSeconds"].Value;
    }

    public void Save(string timeUnit)
    {
        var afkSeconds = int.Parse(AfkTime);
        switch (timeUnit)
        {
            case "hours":
                afkSeconds *= 3600;
                break;
            case "minutes":
                afkSeconds *= 60;
                break;
            case "seconds":
                break;
        }
        App.Config.AppSettings.Settings["afkSeconds"].Value = afkSeconds.ToString();
        App.Config.Save();
    }

    public string ConvertTime(string oldTimeUnit, string newTimeUnit)
    {
        var afkTime = int.Parse(AfkTime);
        switch (oldTimeUnit)
        {
            case "hours":
                afkTime *= 3600;
                break;
            case "minutes":
                afkTime *= 60;
                break;
            case "seconds":
                break;
        }
        switch (newTimeUnit)
        {
            case "hours":
                afkTime /= 3600;
                break;
            case "minutes":
                afkTime /= 60;
                break;
            case "seconds":
                break;
        }
        AfkTime = afkTime.ToString();
        return AfkTime;
    }

    public void Export(string fileName, List<string> projectNames)
    {
        var projects = _databaseManager.GetProjects().Where(p => projectNames.Contains(p.Name)).ToList();
        CsvManager.Export(projects, fileName);
        MessageBox.Show("Exported successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
    }

    public void Import(string fileName)
    {
        if (string.IsNullOrEmpty(fileName))
        {
            MessageBox.Show("Select a file to import.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }
        
        try
        {
            File.Copy(fileName, App.Config.ConnectionStrings.ConnectionStrings["litedb"].ConnectionString, true);
        }
        catch (Exception e)
        {
            MessageBox.Show(e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        if (!_databaseManager.TestConnection())
        {
            MessageBox.Show("Failed to import. File is not valid.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        MessageBox.Show("Imported successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        
        ProjectNames.Clear();
        foreach (var projectName in _databaseManager.GetProjects().Select(p => p.Name))
        {
            ProjectNames.Add(projectName);
        }
    }
    
    public void Backup(string fileName)
    {
        try
        {
            File.Copy(App.Config.ConnectionStrings.ConnectionStrings["litedb"].ConnectionString, fileName, true);
        }
        catch (Exception e)
        {
            MessageBox.Show(e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        MessageBox.Show("Backup created successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
    }

    public void DeleteAllProjects()
    {
        _databaseManager.DeleteAll();
        ProjectNames.Clear();
    }

    public void DeleteSelectedProjects(List<string> projectNames)
    {
        _databaseManager.DeleteProjects(projectNames);
        foreach (var projectName in projectNames)
        {
            ProjectNames.Remove(projectName);
        }
    }
}