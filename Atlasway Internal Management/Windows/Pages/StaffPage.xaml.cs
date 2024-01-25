using Atlasway_Internal_Management.Core;
using Atlasway_Internal_Management.Models;
using Atlasway_Internal_Management.Services;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

namespace Atlasway_Internal_Management.Windows.Pages;


/// <summary>
/// Interaction logic for StaffPage.xaml
/// </summary>
public partial class StaffPage : BasePage
{
    #region Properties

    private List<Staff> _staff = [];
    public List<Staff> staff
    {
        get => _staff;
        set
        {
            _staff = value;
            NotifyPropertyChanged();
            NotifyPropertyChanged(nameof(filteredStaff));
        }
    }

    private bool _isPopable = true;
    public bool isPopable
    {
        get => _isPopable;
        set
        {
            _isPopable = value;
            NotifyPropertyChanged();
            NotifyPropertyChanged(nameof(popToWindowButtonVisibility));
        }
    }

    #endregion

    #region Constructor

    public StaffPage()
    {
        InitializeComponent();

        Loaded += InitialNetworkRequests;
    }

    public StaffPage(bool isPopable = true)
    {
        InitializeComponent();

        Loaded += InitialNetworkRequests;

        this.isPopable = isPopable;
    }

    #endregion

    #region Bindings

    private string _generalSearchString = string.Empty;
    public string generalSearchString
    {
        get => _generalSearchString;
        set
        {
            _generalSearchString = value;
            NotifyPropertyChanged();
            NotifyPropertyChanged(nameof(filteredStaff));
        }
    }

    public Visibility popToWindowButtonVisibility
    {
        get
        {
            if (isPopable)
            {
                return Visibility.Visible;
            }
            else
            {
                return Visibility.Collapsed;
            }
        }
    }

    public List<Staff> filteredStaff
    {
        get
        {
            List<Staff> staff = this.staff;

            if (string.IsNullOrWhiteSpace(generalSearchString) is not true)
            {
                staff = staff.Where(
                        staffMember => staffMember.StaffNo.ToString().Contains(generalSearchString)
                        || staffMember.Firstname.IndexOf(generalSearchString, StringComparison.OrdinalIgnoreCase) != -1
                        || staffMember.Surname.IndexOf(generalSearchString, StringComparison.OrdinalIgnoreCase) != -1
                        || staffMember.EmailAddress.IndexOf(generalSearchString, StringComparison.OrdinalIgnoreCase) != -1
                        || staffMember.ContactNo.Contains(generalSearchString)
                    ).ToList();
            }

            return staff;
        }
    }

    private Staff? _selectedStaff;
    public Staff? selectedStaff
    {
        get => _selectedStaff;
        set
        {
            _selectedStaff = value;
            NotifyPropertyChanged();
        }
    }


    #endregion

    #region Network Requests

    private async void InitialNetworkRequests(object? sender, EventArgs e)
    {
        await RefreshData();
    }

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
            MessageBox.Show(ex.Message);
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

    #endregion

    #region Button events

    private void BtnStaffAdd_Click(object sender, RoutedEventArgs e)
    {
        new NewStaffWindow().Show();
    }

    private void PopToWindow_Click(object sender, RoutedEventArgs e)
    {
        new GenericPageWindow(new StaffPage(isPopable: false)).Show();
    }

    #endregion

    #region ITitledObject

    public override string title => $"Staff";

    #endregion
}
