using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using PricklyApp.ViewModels;

namespace PricklyApp.Views;

public partial class EditTimeWindow : Window
{
    private EditTimeViewModel _viewModel;
    public EditTimeWindow(string projectName, string taskName)
    {
        InitializeComponent();
        _viewModel = new EditTimeViewModel(projectName, taskName);
        DataContext = _viewModel;
    }

    private void AddIntervalButton_OnClick(object sender, RoutedEventArgs e)
    {
        var start = StartTimePicker.Value;
        var end = EndTimePicker.Value;
        if (start == null || end == null)
        {
            MessageBox.Show("Please select start and end time", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
        else
        {
            var result = _viewModel.AddTime(start.Value, end.Value);
            if (result)
            {
                MessageBox.Show("Time added", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Invalid interval", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }

    private void AddTimeButton_OnClick(object sender, RoutedEventArgs e)
    {
        var time = TimePicker.Value;
        if (time == null)
        {
            MessageBox.Show("Please select time", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
        else
        {
            _viewModel.AddTime(time.Value.TimeOfDay);
            MessageBox.Show("Time added", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }

    private void EditTimeWindow_OnClosing(object? sender, CancelEventArgs e)
    {
        DialogResult = true;
    }
}