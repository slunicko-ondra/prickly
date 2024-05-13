using System.Windows;
using PricklyApp.ViewModels;

namespace PricklyApp.Views;

public partial class DetailWindow : Window
{
    private DetailViewModel _viewModel;
    
    public DetailWindow(string projectName, string taskName)
    {
        InitializeComponent();
        _viewModel = new DetailViewModel(projectName, taskName);
        DataContext = _viewModel;
    }
}