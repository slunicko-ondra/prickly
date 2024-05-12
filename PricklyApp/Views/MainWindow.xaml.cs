using System.Windows;
using System.Windows.Threading;
using PricklyApp.ViewModels;
using PricklyApp.Views;

namespace PricklyApp;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private MainWindowViewModel _viewModel;
    private DispatcherTimer _timer;

    public MainWindow()
    {
        InitializeComponent();
        _viewModel = new MainWindowViewModel();
        DataContext = _viewModel;
        
        _timer = new DispatcherTimer();
        _timer.Interval = TimeSpan.FromSeconds(1);
        _timer.Tick += TimerOnTick;
    }

    private void StartStopButton_OnClick(object sender, RoutedEventArgs e)
    {
        var project = ProjectComboBox.SelectedItem;
        var task = TaskComboBox.SelectedItem;
        if (project == null || task == null)
        {
            MessageBox.Show(this, "Project and task must be selected.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }
        var projectString = project.ToString();
        var taskString = task.ToString();
        if (projectString == null || taskString == null)
        {
            return; 
        }
        if (StartStopButton.Content.ToString() == "Start")
        {
            StartButtonClicked(projectString, taskString);
            _timer.Start();
            StartStopButton.Content = "Stop";
        }
        else
        {
            StopButtonClicked(projectString, taskString);
            _timer.Stop();
            StartStopButton.Content = "Start";
        }
    }

    private void StartButtonClicked(string project, string task)
    {
        var result = _viewModel.StartWork(project, task);
        if (!result)
        {
            MessageBox.Show(this, "Failed to start work.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void StopButtonClicked(string project, string task)
    {
        var result = _viewModel.StopWork(project, task);
        if (!result)
        {
            MessageBox.Show(this, "Failed to stop work.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
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

    private void TimerOnTick(object? sender, EventArgs e)
    {
        _viewModel.UpdateDisplayTime();
    }
}