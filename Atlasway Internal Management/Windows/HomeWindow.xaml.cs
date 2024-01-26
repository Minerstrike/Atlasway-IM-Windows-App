using Atlasway_Internal_Management.Controls;
using Atlasway_Internal_Management.Core;
using System.Windows;

namespace Atlasway_Internal_Management.Windows;


/// <summary>
/// Interaction logic for HomeWindow.xaml
/// </summary>
public partial class HomeWindow : ObservableWindow
{
    #region Properties

    private NavBarButton? _selectedNavButton;
    public NavBarButton? selectedNavButton
    {
        get => _selectedNavButton;
        set 
        { 
            _selectedNavButton = value;
            NotifyPropertyChanged();
        }
    }

    #endregion

    #region Constructor

    public HomeWindow()
    {
        InitializeComponent();
    }

    #endregion

    #region Events
    
    private void NavBar_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
    {
        PageViewer.Navigate(selectedNavButton?.NavLink);
    }

    #endregion

    #region Window events
    
    private void ObservableWindow_ContentRendered(object sender, EventArgs e)
    {
        HomePageButton.IsSelected = true;
    }

    private void ObservableWindow_Closed(object sender, EventArgs e)
    {
        Application.Current.Shutdown();
    }

    #endregion
}
