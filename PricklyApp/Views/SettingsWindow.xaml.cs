using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Win32;
using PricklyApp.ViewModels;

namespace PricklyApp.Views;

public partial class SettingsWindow : Window
{
    private SettingsViewModel _viewModel;
    private string _previousTimeUnit;
    private bool _reloadProjects;
    
    public SettingsWindow()
    {
        InitializeComponent();
        _viewModel = new SettingsViewModel();
        _previousTimeUnit = "seconds";
        _reloadProjects = false;
        TimeUnitComboBox.SelectedIndex = 0;
        DataContext = _viewModel;
    }

    private void AfkTimeTextBox_OnPreviewTextInput(object sender, TextCompositionEventArgs e)
    {
        var regex = new Regex("[^0-9]+");
        e.Handled = regex.IsMatch(e.Text);
    }

    private void SaveButton_OnClick(object sender, RoutedEventArgs e)
    {
        var timeUnit = TimeUnitComboBox.SelectedItem.ToString() ?? "";
        _viewModel.Save(timeUnit);
        Close();

    }

    private void TimeUnitComboBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var timeUnit = TimeUnitComboBox.SelectedItem.ToString() ?? "";
        AfkTimeTextBox.Text = _viewModel.ConvertTime(_previousTimeUnit, timeUnit);
        _previousTimeUnit = timeUnit;
    }

    private void ExportButton_OnClick(object sender, RoutedEventArgs e)
    {
        SaveFileDialog saveFileDialog = new SaveFileDialog
        {
            Filter = "CSV files (*.csv)|*.csv",
            FilterIndex = 1,
            RestoreDirectory = true
        };
        if (saveFileDialog.ShowDialog() == true)
        {
            _viewModel.Export(saveFileDialog.FileName, ProjectsListBox.SelectedItems.Cast<string>().ToList());
        }
    }

    private void SelectFileButton_OnClick(object sender, RoutedEventArgs e)
    {
        OpenFileDialog openFileDialog = new OpenFileDialog
        {
            Filter = "DB files (*.db)|*.db",
            FilterIndex = 1,
            RestoreDirectory = true
        };
        if (openFileDialog.ShowDialog() == true)
        {
            ImportTextBox.Text = openFileDialog.FileName;
        }
    }

    private void ImportButton_OnClick(object sender, RoutedEventArgs e)
    {
        _viewModel.Import(ImportTextBox.Text);
        _reloadProjects = true;
    }

    private void DeleteButton_OnClick(object sender, RoutedEventArgs e)
    {
        var result = MessageBox.Show("Are you sure you want to delete all projects?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);
        if (result == MessageBoxResult.Yes)
        {
            _viewModel.DeleteAllProjects();
            _reloadProjects = true;
        }
    }

    private void SettingsWindow_OnClosing(object? sender, CancelEventArgs e)
    {
        DialogResult = _reloadProjects;
    }
}