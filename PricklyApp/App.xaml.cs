using System.Configuration;
using System.Windows;

namespace PricklyApp;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    private const string DatabaseConnectionString = @".\prickly.db";
    private const string AddProjectWindowTaskDelimiter = ",";
    public static readonly Configuration Config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
    
    public App()
    {
        Config.ConnectionStrings.ConnectionStrings.Add(new ConnectionStringSettings("litedb", DatabaseConnectionString));
        if (Config.AppSettings.Settings["addProjectWindowTaskDelimiter"] == null)
        {
            Config.AppSettings.Settings.Add(new KeyValueConfigurationElement("addProjectWindowTaskDelimiter", AddProjectWindowTaskDelimiter));
        }

        if (Config.AppSettings.Settings["defaultAfkSeconds"] == null)
        {
            Config.AppSettings.Settings.Add(new KeyValueConfigurationElement("defaultAfkSeconds", "300"));
        }

        if (Config.AppSettings.Settings["afkSeconds"] == null)
        {
            Config.AppSettings.Settings.Add(new KeyValueConfigurationElement("afkSeconds", "300"));
        }
        Config.Save();
    }
}