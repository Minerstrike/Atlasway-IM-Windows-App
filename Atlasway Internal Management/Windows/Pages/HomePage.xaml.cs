using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Atlasway_Internal_Management.Windows.Pages;


/// <summary>
/// Interaction logic for HomePage.xaml
/// </summary>
public partial class HomePage : Page, INotifyPropertyChanged
{
    #region Constructor

    public HomePage()
    {
        InitializeComponent();
    }

    #endregion

    #region INotifyPropertyChnaged

    public event PropertyChangedEventHandler? PropertyChanged;

    public void NotifyPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    #endregion
}
