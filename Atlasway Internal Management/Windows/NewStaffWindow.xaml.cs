using Atlasway_Internal_Management.Core;
using Atlasway_Internal_Management.Models;
using Atlasway_Internal_Management.Services;
using System.Windows;

namespace Atlasway_Internal_Management.Windows;


/// <summary>
/// Interaction logic for NewStaffWindow.xaml
/// </summary>
public partial class NewStaffWindow : ObservableWindow
{
    #region Constructor

    public NewStaffWindow()
    {
        InitializeComponent();
    }

    #endregion

    #region Bindings

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

    #endregion

    #region Network requests

    private async Task PostStaff()
    {
        CancellationToken cancellationToken = new CancellationToken();

        try
        {
            NewStaff staff = new NewStaff(
                surname         : lastname,
                firstname       : firstname,
                contactNo       : contactNo,
                emailAddress    : emailAddress);

            await NetworkService.PostStaff(staff, cancellationToken);
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Source, ex.Message, MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    #endregion

    #region Button events

    private async void StaffPost_click(object sender, System.Windows.RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(firstname))
        {
            MessageBox.Show("Please enter a first name for the new staff member.", "Staff member's first name is missing", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
        else if (string.IsNullOrWhiteSpace(firstname))
        {
            MessageBox.Show("Please enter a last name for the new staff member.", "Staff member's last name is missing", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
        else if (string.IsNullOrWhiteSpace(contactNo))
        {
            MessageBox.Show("Please enter a contact number.", "Client's contact number is missing", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
        else if (string.IsNullOrWhiteSpace(emailAddress))
        {
            MessageBox.Show("Please enter an email address.", "Client's email address is missing", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
        else
        {
            await PostStaff();
            MessageBox.Show("Staff member added.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            Close();
        }
    }

    #endregion
}
