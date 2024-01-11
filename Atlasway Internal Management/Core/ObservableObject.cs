using MahApps.Metro.Controls;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Atlasway_Internal_Management.Core;

public class ObservableObject : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    public void NotifyPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

public class ObservableWindow : MetroWindow, INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    public void NotifyPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
