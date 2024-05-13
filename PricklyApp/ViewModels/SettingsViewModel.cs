using PricklyApp.Models;

namespace PricklyApp.ViewModels;

public class SettingsViewModel
{
    private DatabaseManager _databaseManager;
    public string[] ProjectNames { get; }
    public string[] TimeUnits { get; } = ["seconds", "minutes", "hours"];

    public string AfkTime { get; set; }
    
    public SettingsViewModel()
    {
        _databaseManager = new DatabaseManager(App.Config.ConnectionStrings.ConnectionStrings["litedb"].ConnectionString);
        ProjectNames = _databaseManager.GetProjects().Select(p => p.Name).ToArray();
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
}