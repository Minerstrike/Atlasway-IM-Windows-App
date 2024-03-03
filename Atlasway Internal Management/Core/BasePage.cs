using Atlasway_Internal_Management.Windows.Interfaces;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;

namespace Atlasway_Internal_Management.Core;


public abstract class BasePage : Page, INotifyPropertyChanged, ITitledObject, ITwoDimentional
{
    #region INotifyPropertyChnaged

    public event PropertyChangedEventHandler? PropertyChanged;

    public void NotifyPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    #endregion

    #region ITitledObject

    public virtual string title => throw new NotImplementedException();

    #endregion

    #region ITwoDimentional

    private double _height = 450;
    public virtual double height
    {
        get => _height;
        set
        {
            _height = value;
            NotifyPropertyChanged();
        }
    }

    private double _width = 800;
    public virtual double width
    {
        get => _width;
        set
        {
            _width = value;
            NotifyPropertyChanged();
        }
    }

    public virtual double minHeight => 0;
    public virtual double minWidth => 0;

    #endregion

    public delegate void Close(object source, EventArgs e);
}
