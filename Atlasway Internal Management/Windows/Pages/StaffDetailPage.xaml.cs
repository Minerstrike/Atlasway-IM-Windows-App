using Atlasway_Internal_Management.Core;
using Atlasway_Internal_Management.Models;
using Atlasway_Internal_Management.Services;
using System.Net;
using System.Windows;
using System.Windows.Resources;
using Windows.Media.ClosedCaptioning;

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

    private Staff _selectedStaffMember;
    public Staff selectedStaffMember
    {
        get => _selectedStaffMember;
        set
        {
            _selectedStaffMember = value;
            NotifyPropertyChanged();
            NotifyPropertyChanged(nameof(staffDetailsLabel));
        }
    }

    private List<Staff> _staff;
    public List<Staff> staff
    {
        get => _staff;
        set 
        {
            _staff = value;
            NotifyPropertyChanged();
        }
    }

    #endregion

    #region Constructor

    public StaffDetailPage(Staff staff)
    {
        InitializeComponent();

        this.selectedStaffMember = staff;
    }

    #endregion

    #region Binding

    public string staffDetailsLabel =>
        $"ID            : {selectedStaffMember.StaffNo}\n" +
        $"Firstname     : {selectedStaffMember.Firstname}\n" +
        $"Surname       : {selectedStaffMember.Surname}\n" +
        $"Contact       : {selectedStaffMember.ContactNo}\n" +
        $"Email address : {selectedStaffMember.EmailAddress}";

    private string _firstname = string.Empty;
    public string firstname
    {
        get => _firstname;
        set 
        {
            _firstname = value;
            NotifyPropertyChanged();
        }
    }

    private string _lastname = string.Empty;
    public string lastname
    {
        get => _lastname;
        set 
        {
            _lastname = value;
            NotifyPropertyChanged();
        }
    }

    private string _contactNo = string.Empty;
    public string contactNo
    {
        get => _contactNo;
        set 
        {
            _contactNo = value;
            NotifyPropertyChanged();
        }
    }

    private string _emailAddress = string.Empty;
    public string emailAddress
    {
        get => _emailAddress;
        set
        {
            _emailAddress = value;
            NotifyPropertyChanged();
        }
    }

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

    #region Network Requests

    internal async Task RefreshData()
    {
        try
        {
            await Task.WhenAll(
                GetStaff()
            );
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.StackTrace, ex.Message, MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    public async Task GetStaff()
    {
        CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

        try
        {
            staff = await NetworkService.GetStaff(cancellationTokenSource.Token);
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.StackTrace, ex.Message, MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private async Task UpdateStaffMember()
    {
        Staff updatedStaffMember = new Staff(
            staffNo         : selectedStaffMember.StaffNo,
            surname         : lastname,
            firstname       : firstname,
            contactNo       : contactNo,
            emailAddress    : emailAddress);

        await NetworkService.UpdateStaff(updatedStaffMember, CancellationToken.None);

        isEditable = false;
        await RefreshData();
        selectedStaffMember = staff.Where(staffMember => staffMember.StaffNo == selectedStaffMember.StaffNo).FirstOrDefault();
    }

    #endregion

    #region Button events

    private async void StaffUpdate_click(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(firstname))
        {
            MessageBox.Show("Please enter a first name.", "First name is missing.", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        if (string.IsNullOrWhiteSpace(lastname))
        {
            MessageBox.Show("Please enter a last name.", "Last name is missing.", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        if (string.IsNullOrWhiteSpace(contactNo))
        {
            MessageBox.Show("Please enter a contact number.", "Contact number is missing.", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        if (string.IsNullOrWhiteSpace(emailAddress))
        {
            MessageBox.Show("Please enter a email address.", "Email address is missing.", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        await UpdateStaffMember();

        firstname       = string.Empty;
        lastname        = string.Empty;
        contactNo       = string.Empty;
        emailAddress    = string.Empty;
    }

    private void ShowEditContent_Click(object sender, RoutedEventArgs e)
    {
        isEditable = !isEditable;

        firstname       = selectedStaffMember.Firstname;
        lastname        = selectedStaffMember.Surname;
        contactNo       = selectedStaffMember.ContactNo;
        emailAddress    = selectedStaffMember.EmailAddress;
    }

    private void ResetContent_Click(object sender, RoutedEventArgs e)
    {
        firstname = selectedStaffMember.Firstname;
        lastname = selectedStaffMember.Surname;
        contactNo = selectedStaffMember.ContactNo;
        emailAddress = selectedStaffMember.EmailAddress;
    }

    private void FillFirstname_Click(object sender, RoutedEventArgs e)
    {
        firstname = selectedStaffMember.Firstname;
    }

    private void FillLastname_Click(object sender, RoutedEventArgs e)
    {
        lastname = selectedStaffMember.Surname;
    }

    private void FillContactNo_Click(object sender, RoutedEventArgs e)
    {
        contactNo = selectedStaffMember.ContactNo;
    }

    private void FillEmailAddress_Click(object sender, RoutedEventArgs e)
    {
        emailAddress = selectedStaffMember.EmailAddress;
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
