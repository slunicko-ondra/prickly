using System.Collections.ObjectModel;
using System.Windows;
using PricklyApp.ViewModels;

namespace PricklyApp.Views;

public partial class AddTaskWindow : Window
{
    private AddTaskViewModel _viewModel;
    private string _projectName;
    private ObservableCollection<string> _taskNames;

    public AddTaskWindow(string projectName, ObservableCollection<string> taskNames)
    {
        InitializeComponent();
        _viewModel = new AddTaskViewModel();
        _projectName = projectName;
        _taskNames = taskNames;
    }

    private void OkButton_OnClick(object sender, RoutedEventArgs e)
    {
        var result = _viewModel.AddTask(_projectName, TaskNameTextBox.Text);
        if (result)
        {
            _taskNames.Add(TaskNameTextBox.Text);
            Close();
        }
    }
}