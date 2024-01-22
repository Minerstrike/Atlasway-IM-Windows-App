using Atlasway_Internal_Management.Windows.Interfaces;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;

namespace Atlasway_Internal_Management.Core;


public abstract class BasePage : Page, INotifyPropertyChanged, ITitledObject
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
}
