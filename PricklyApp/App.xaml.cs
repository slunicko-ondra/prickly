using System.Configuration;
using System.IO;
using System.Windows;

namespace PricklyApp;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    private const string DatabaseFileName = "prickly.db";
    private const string AddProjectWindowTaskDelimiter = ",";
    private const string ConnectionStringName = "litedb";
    private static readonly string AppDataDirectory = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
        "PricklyApp");
    private static readonly string DatabaseConnectionString = Path.Combine(AppDataDirectory, DatabaseFileName);

    public static readonly Configuration Config = LoadConfiguration();

    private static Configuration LoadConfiguration()
    {
        Directory.CreateDirectory(AppDataDirectory);

        var configMap = new ExeConfigurationFileMap
        {
            ExeConfigFilename = Path.Combine(AppDataDirectory, "PricklyApp.config")
        };

        var config = ConfigurationManager.OpenMappedExeConfiguration(configMap, ConfigurationUserLevel.None);
        var connectionString = config.ConnectionStrings.ConnectionStrings[ConnectionStringName];
        if (connectionString == null)
        {
            config.ConnectionStrings.ConnectionStrings.Add(new ConnectionStringSettings(ConnectionStringName, DatabaseConnectionString));
        }
        else
        {
            connectionString.ConnectionString = DatabaseConnectionString;
        }

        EnsureAppSetting(config, "addProjectWindowTaskDelimiter", AddProjectWindowTaskDelimiter);
        EnsureAppSetting(config, "defaultAfkSeconds", "300");
        EnsureAppSetting(config, "afkSeconds", "300");
        EnsureAppSetting(config, "mainWindowLocationX", "100");
        EnsureAppSetting(config, "mainWindowLocationY", "100");
        EnsureAppSetting(config, "selectedProjectIndex", "-1");
        EnsureAppSetting(config, "selectedTaskIndex", "-1");

        config.Save();
        return config;
    }

    private static void EnsureAppSetting(Configuration config, string key, string value)
    {
        if (config.AppSettings.Settings[key] == null)
        {
            config.AppSettings.Settings.Add(new KeyValueConfigurationElement(key, value));
        }
    }

    public App()
    {
        var legacyDatabasePath = Path.Combine(AppContext.BaseDirectory, DatabaseFileName);
        var currentDatabasePath = Path.Combine(AppDataDirectory, DatabaseFileName);

        if (File.Exists(legacyDatabasePath) && !File.Exists(currentDatabasePath))
        {
            File.Copy(legacyDatabasePath, currentDatabasePath, true);
        }

        if (Config.ConnectionStrings.ConnectionStrings[ConnectionStringName] == null)
        {
            Config.ConnectionStrings.ConnectionStrings.Add(new ConnectionStringSettings(ConnectionStringName, DatabaseConnectionString));
        }
        else
        {
            Config.ConnectionStrings.ConnectionStrings[ConnectionStringName].ConnectionString = DatabaseConnectionString;
        }

        Config.Save();
    }
}