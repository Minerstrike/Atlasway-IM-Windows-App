using Atlasway_Internal_Management.Core;
using Atlasway_Internal_Management.Models;
using Atlasway_Internal_Management.Services;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

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

    private bool _canSearch = false;
    public bool canSearch
    {
        get => _canSearch;
        set
        {
            _canSearch = value;
            NotifyPropertyChanged();
            NotifyPropertyChanged(nameof(filteredStaff));
        }
    }

    #endregion

    #region Constructor

    public StaffPage()
    {
        InitializeComponent();

        Loaded += InitialNetworkRequests;
        Loaded += InitializeSearchBox;
    }

    public StaffPage(bool isPopable = true)
    {
        InitializeComponent();

        Loaded += InitialNetworkRequests;
        Loaded += InitializeSearchBox;

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

            if (string.IsNullOrWhiteSpace(generalSearchString) is not true && canSearch)
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

    #region SearchBox

    private void InitializeSearchBox(object? sender, EventArgs e)
    {
        SearchBox.Foreground = Brushes.Gray;
        SearchBox.Text = "Search";
        canSearch = false;
        SearchBox.GotKeyboardFocus += new KeyboardFocusChangedEventHandler(textBox_GotKeyboardFocus);
        SearchBox.LostKeyboardFocus += new KeyboardFocusChangedEventHandler(textBox_LostKeyboardFocus);
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

    private async void Refresh_click(object sender, RoutedEventArgs e)
    {
        await RefreshData();
    }

    #endregion

    #region DataGrid events

    private void StaffDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        if (selectedStaff is not null)
        {
            new GenericPageWindow(new StaffDetailPage(selectedStaff.Value)).Show();
        }
        else
        {
            MessageBox.Show("Please select a staff member.", "No staff member selected.", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    } 

    #endregion

    #region Custom events

    private void textBox_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
    {
        if (sender is TextBox)
        {
            //If nothing has been entered yet.
            if (((TextBox)sender).Foreground == Brushes.Gray)
            {
                ((TextBox)sender).Text = "";
                ((TextBox)sender).SetResourceReference(Control.ForegroundProperty, "MahApps.Brushes.ThemeForeground");
                canSearch = true;
            }
        }
    }

    private void textBox_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
    {
        //Make sure sender is the correct Control.
        if (sender is TextBox)
        {
            //If nothing was entered, reset default text.
            if (((TextBox)sender).Text.Trim().Equals(""))
            {
                ((TextBox)sender).Foreground = Brushes.Gray;
                ((TextBox)sender).Text = "Search";
                canSearch = false;
            }
        }
    }

    #endregion

    #region ITitledObject

    public override string title => $"Staff";

    #endregion
}
