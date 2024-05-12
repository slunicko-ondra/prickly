using System.Windows;
using PricklyApp.ViewModels;
using PricklyApp.Views;

namespace PricklyApp;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private MainWindowViewModel _viewModel;

    public MainWindow()
    {
        InitializeComponent();
        _viewModel = new MainWindowViewModel();
        DataContext = _viewModel;
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

    private void ProjectComboBox_OnSelected(object sender, RoutedEventArgs e)
    {
        var projetcName = ProjectComboBox.SelectedItem.ToString();
        if (projetcName == null)
        {
            return;
        }
        _viewModel.UpdateTaskNames(projetcName);
    }

    private void AddProjectButton_OnClick(object sender, RoutedEventArgs e)
    {
        var addProjectWindow = new AddProjectWindow(_viewModel.ProjectNames)
        {
            Owner = this
        };
        addProjectWindow.ShowDialog();
    }

    private void AddTaskButton_OnClick(object sender, RoutedEventArgs e)
    {
        var project = ProjectComboBox.SelectedItem.ToString();
        if (project == null)
        {
            MessageBox.Show(this, "Project must be selected.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
        else
        {
            var addTaskWindow = new AddTaskWindow(project, _viewModel.TaskNames)
            {
                Owner = this
            };
            addTaskWindow.ShowDialog();
        }
    }
}