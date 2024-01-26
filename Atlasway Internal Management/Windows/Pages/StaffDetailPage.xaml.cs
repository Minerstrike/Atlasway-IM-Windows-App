using Atlasway_Internal_Management.Core;
using Atlasway_Internal_Management.Models;
using Atlasway_Internal_Management.Windows.Interfaces;
using System.Windows;
using System.Windows.Controls;
using Windows.Media.Protection.PlayReady;

namespace Atlasway_Internal_Management.Windows.Pages;


/// <summary>
/// Interaction logic for StaffDetailPage.xaml
/// </summary>
public partial class StaffDetailPage : BasePage
{
    #region Properties

    private bool _isEditable = false;
    public bool isEditable
    {
        get => _isEditable;
        set
        {
            _isEditable = value;
            NotifyPropertyChanged();
            NotifyPropertyChanged(nameof(editVisibility));
            NotifyPropertyChanged(nameof(minHeight));
            NotifyPropertyChanged(nameof(height));
        }
    }

    private Staff _staff;
    public Staff staff
    {
        get => _staff;
        set
        {
            _staff = value;
            NotifyPropertyChanged();
            NotifyPropertyChanged(nameof(staffDetailsLabel));
        }
    }

    #endregion

    #region Constructor

    public StaffDetailPage(Staff staff)
    {
        InitializeComponent();

        this.staff = staff;
    }

    #endregion

    #region Binding

    public string staffDetailsLabel =>
        $"ID            : {staff.StaffNo}\n" +
        $"Firstname     : {staff.Firstname}\n" +
        $"Surname       : {staff.Surname}\n" +
        $"Contact       : {staff.ContactNo}\n" +
        $"Email address : {staff.EmailAddress}";

    public Visibility editVisibility
    {
        get
        {
            if (isEditable)
            {
                return Visibility.Visible;
            }
            else
            {
                return Visibility.Collapsed;
            }
        }
    }

    #endregion

    #region Button events

    private void StaffUpdate_click(object sender, RoutedEventArgs e)
    {

    }

    private void ShowEditContent_Click(object sender, RoutedEventArgs e)
    {
        isEditable = !isEditable;
    }

    #endregion

    #region ITitledObject

    public override string title => "Staff detail";

    #endregion

    #region ITwoDimensional

    private double _height = 200;
    public override double height
    {
        get
        {
            if (isEditable)
            {
                _height = 340;
            }
            else
            {
                _height = 150;
            }

            return _height;
        }

        set
        {
            _height = value;
            NotifyPropertyChanged();
        }
    }

    private double _width = 600;
    public override double width
    {
        get => _width;
        set
        {
            _width = value;
            NotifyPropertyChanged();
        }
    }


    private double _minHeight = 150;
    public override double minHeight
    {
        get
        {
            if (isEditable)
            {
                _minHeight = 340;
            }
            else
            {
                _minHeight = 150;
            }

            return _minHeight;
        }
    }
    public override double minWidth => 400;

    #endregion
}
