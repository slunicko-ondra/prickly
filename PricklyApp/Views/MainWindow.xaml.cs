using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using Gma.System.MouseKeyHook;
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
    private int _afkSeconds;
    private const int MaxAfkSeconds = 10;
    private IKeyboardMouseEvents _globalHook;
    private bool _userIsAfk;

    public MainWindow()
    {
        InitializeComponent();
        _viewModel = new MainWindowViewModel();
        DataContext = _viewModel;
        
        _afkSeconds = 0;
        _userIsAfk = false;
        _timer = new DispatcherTimer();
        _timer.Interval = TimeSpan.FromSeconds(1);
        _timer.Tick += TimerOnTick;
        
        _globalHook = Hook.GlobalEvents();
        _globalHook.MouseDownExt += GlobalHook_OnUserActivity;
        _globalHook.KeyPress += GlobalHook_OnUserActivity;
        _globalHook.MouseMove += GlobalHook_OnUserActivity;
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
        }
        else
        {
            StopButtonClicked(projectString, taskString);
        }
    }

    private void StartButtonClicked(string project, string task)
    {
        var result = _viewModel.StartWork(project, task);
        if (!result)
        {
            MessageBox.Show(this, "Failed to start work.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        else
        {
            StartStopButton.Content = "Stop";
            _timer.Start();
        }
    }

    private void StopButtonClicked(string project, string task, DateTime? end=null)
    {
        var result = _viewModel.StopWork(project, task, end);
        if (!result)
        {
            MessageBox.Show(this, "Failed to stop work.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        else
        {
            StartStopButton.Content = "Start";
            _timer.Stop();
        }
    }

    private void ProjectComboBox_OnSelected(object sender, RoutedEventArgs e)
    {
        var projectName = ProjectComboBox.SelectedItem?.ToString();
        if (projectName == null)
        {
            return;
        }
        _viewModel.UpdateTaskNames(projectName);
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
        var projectName = ProjectComboBox.SelectedItem?.ToString();
        if (projectName == null)
        {
            MessageBox.Show(this, "Project must be selected.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
        else
        {
            var addTaskWindow = new AddTaskWindow(projectName, _viewModel.TaskNames)
            {
                Owner = this
            };
            addTaskWindow.ShowDialog();
        }
    }

    private void TimerOnTick(object? sender, EventArgs e)
    {
        _afkSeconds++;
        if (_afkSeconds >= MaxAfkSeconds)
        {
            UserIsAfk();
        }
        UpdateDisplayTime();
    }

    private void TaskComboBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        UpdateDisplayTime();
    }
    
    private void UpdateDisplayTime()
    {
        var projectName = ProjectComboBox.SelectedItem?.ToString();
        var taskName = TaskComboBox.SelectedItem?.ToString();
        if (projectName == null || taskName == null)
        {
            return;
        }
        _viewModel.UpdateDisplayTime(projectName, taskName);
    }

    private void UserIsAfk()
    {
        var projectName = ProjectComboBox.SelectedItem?.ToString();
        var taskName = TaskComboBox.SelectedItem?.ToString();
        if (projectName == null || taskName == null)
        {
            return;
        }
        StopButtonClicked(projectName, taskName, DateTime.Now.AddSeconds(-_afkSeconds));
        _userIsAfk = true;
    }
    
    private void GlobalHook_OnUserActivity(object? sender, EventArgs e)
    {
        _afkSeconds = 0;
        if (_userIsAfk)
        {
            _userIsAfk = false;
            var projectName = ProjectComboBox.SelectedItem?.ToString();
            var taskName = TaskComboBox.SelectedItem?.ToString();
            if (projectName == null || taskName == null)
            {
                return;
            }
            StartButtonClicked(projectName, taskName);
        }
    }

    private void MainWindow_OnClosing(object? sender, CancelEventArgs e)
    {
        _viewModel.StopAllWork();
    }
}