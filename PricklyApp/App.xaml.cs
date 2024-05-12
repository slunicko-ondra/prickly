using System.Configuration;
using System.Data;
using System.Windows;

namespace PricklyApp;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    public const string DatabaseConnectionString = @".\prickly.db";
}