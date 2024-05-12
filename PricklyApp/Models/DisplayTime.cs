using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace PricklyApp.Models;

public class DisplayTime : INotifyPropertyChanged
{
    private TimeSpan _time;
    public TimeSpan Time
    {
        get { return _time; }
        set
        {
            if (_time != value)
            {
                _time = value;
                TimeString = _time.ToString(@"hh\:mm\:ss");
                OnPropertyChanged(nameof(Time));
            }
        }
    }
    private string _timeString;
    public string TimeString
    {
        get { return _timeString; }
        set
        {
            if (_timeString != value)
            {
                _timeString = value;
                OnPropertyChanged(nameof(TimeString));
            }
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }
}