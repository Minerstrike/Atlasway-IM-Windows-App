using Atlasway_Internal_Management.Core;
using System.Windows.Data;

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

        SetBinding(WidthProperty, new Binding("width")
        {
            Source = page,
            Mode = BindingMode.TwoWay
        });

        SetBinding(HeightProperty, new Binding("height")
        {
            Source = page,
            Mode = BindingMode.TwoWay
        });

        SetBinding(MinWidthProperty, new Binding("minWidth")
        {
            Source = page,
            Mode = BindingMode.OneWay
        });

        SetBinding(MinHeightProperty, new Binding("minHeight")
        {
            Source = page,
            Mode = BindingMode.OneWay
        });
    }

    #endregion

    #region ITitledObject

    public string titleLabel
    {
        get
        {
            if (page is null || string.IsNullOrWhiteSpace(page.title))
            {
                return "Generic window";
            }

            return page.title;
        }
    }

    #endregion
}
