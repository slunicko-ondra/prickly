using System.Collections.ObjectModel;
using System.Windows;
using PricklyApp.ViewModels;

namespace PricklyApp.Views;

public partial class AddProjectWindow : Window
{
    private AddProjectViewModel _viewModel;
    private ObservableCollection<string> _projectNames;
    
    public AddProjectWindow(ObservableCollection<string> projectNames)
    {
        InitializeComponent();
        _viewModel = new AddProjectViewModel();
        _projectNames = projectNames;
    }

    private void OkButton_OnClick(object sender, RoutedEventArgs e)
    {
        var result = _viewModel.AddProject(ProjectNameTextBox.Text, TasksTextBox.Text);
        if (result)
        {
            _projectNames.Add(ProjectNameTextBox.Text);
            Close();
        }
    }
}