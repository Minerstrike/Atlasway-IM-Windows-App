using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Atlasway_Internal_Management.Controls;

/// <summary>
/// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
///
/// Step 1a) Using this custom control in a XAML file that exists in the current project.
/// Add this XmlNamespace attribute to the root element of the markup file where it is 
/// to be used:
///
///     xmlns:MyNamespace="clr-namespace:Atlasway_Internal_Management.Controls"
///
///
/// Step 1b) Using this custom control in a XAML file that exists in a different project.
/// Add this XmlNamespace attribute to the root element of the markup file where it is 
/// to be used:
///
///     xmlns:MyNamespace="clr-namespace:Atlasway_Internal_Management.Controls;assembly=Atlasway_Internal_Management.Controls"
///
/// You will also need to add a project reference from the project where the XAML file lives
/// to this project and Rebuild to avoid compilation errors:
///
///     Right click on the target project in the Solution Explorer and
///     "Add Reference"->"Projects"->[Browse to and select this project]
///
///
/// Step 2)
/// Go ahead and use your control in the XAML file.
///
///     <MyNamespace:NavBarButton/>
///
/// </summary>
public class NavBarButton : ListBoxItem
{
    static NavBarButton()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(NavBarButton), new FrameworkPropertyMetadata(typeof(NavBarButton)));
    }

    public Uri NavLink
    {
        get { return (Uri)GetValue(NavLinkProperty); }
        set { SetValue(NavLinkProperty, value); }
    }
    // Using a DependencyProperty as the backing store for NavLink.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty NavLinkProperty =
        DependencyProperty.Register("NavLink", typeof(Uri), typeof(NavBarButton), new PropertyMetadata(null));

    public Geometry Icon
    {
        get { return (Geometry)GetValue(IconProperty); }
        set { SetValue(IconProperty, value); }
    }
    // Using a DependencyProperty as the backing store for Icon.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty IconProperty =
        DependencyProperty.Register("Icon", typeof(Geometry), typeof(NavBarButton), new PropertyMetadata(null));
}
