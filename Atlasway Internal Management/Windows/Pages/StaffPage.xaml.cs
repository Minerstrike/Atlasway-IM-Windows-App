using Atlasway_Internal_Management.Models;
using Atlasway_Internal_Management.Services;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows;
using System.Windows.Controls;

namespace Atlasway_Internal_Management.Windows.Pages;

/// <summary>
/// Interaction logic for StaffPage.xaml
/// </summary>
public partial class StaffPage : Page, INotifyPropertyChanged
{
    #region Constructor

    public StaffPage()
    {
        InitializeComponent();

        Loaded += InitialNetworkRequests;
    }

    #endregion

    #region Bindings

    private List<Staff> _staff = [];
    public List<Staff> staff
    {
        get => _staff;
        set
        {
            _staff = value;
            NotifyPropertyChanged();
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

    #region INotifyPropertyChnaged

    public event PropertyChangedEventHandler? PropertyChanged;

    public void NotifyPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    #endregion
}
