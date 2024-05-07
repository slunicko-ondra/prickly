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
    }


    private void StartButton_OnClick(object sender, RoutedEventArgs e)
    {
        if (StartButton.Content.ToString() == "Stop")
        {
            StartButton.Content = "Start";
            return;
        }
        StartButton.Content = "Stop";
    }
}