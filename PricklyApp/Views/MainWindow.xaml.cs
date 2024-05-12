using System.Windows;

namespace PricklyApp;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        string [] projects = new []{"project1", "project2", "project3"};
        ProjectComboBox.ItemsSource = projects;
    }


    private void StartStopButton_OnClick(object sender, RoutedEventArgs e)
    {
        if (StartStopButton.Content.ToString() == "Stop")
        {
            StartStopButton.Content = "Start";
            return;
        }
        StartStopButton.Content = "Stop";
    }
}