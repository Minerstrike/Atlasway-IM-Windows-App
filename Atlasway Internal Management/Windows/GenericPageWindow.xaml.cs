using Atlasway_Internal_Management.Core;
using System.Windows;
using System.Windows.Controls;

namespace Atlasway_Internal_Management.Windows;


/// <summary>
/// Interaction logic for GenericPageWindow.xaml
/// </summary>
public partial class GenericPageWindow : ObservableWindow
{
    #region Properties

    private BasePage? _page;
    public BasePage? page
    {
        get => _page;
        set 
        {
            _page = value;
            NotifyPropertyChanged();
        }
    }

    #endregion

    #region Constructor

    public GenericPageWindow(BasePage page)
    {
        InitializeComponent();

        this.page = page;

        PageViewer.Navigate(page);
    } 

    #endregion
}
