using Atlasway_Internal_Management.Models;
using Atlasway_Internal_Management.Services;
using System.ComponentModel;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

namespace Atlasway_Internal_Management.Windows.Pages;


/// <summary>
/// Interaction logic for ClientsPage.xaml
/// </summary>
public partial class ClientsPage : Page, INotifyPropertyChanged
{
    #region Constructor
    
    public ClientsPage()
    {
        InitializeComponent();

        Loaded += InitialNetworkRequests;
    }

    #endregion

    #region Bindings

    private List<Client> _clients = [];
    public List<Client> clients
    {
        get => _clients;
        set 
        { 
            _clients = value;
            NotifyPropertyChanged();
        }
    }

    private Client? _selectedClient;
    public Client? selectedClient
    {
        get => _selectedClient;
        set 
        {
            _selectedClient = value;
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
                GetClients()
            );
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
    }

    public async Task GetClients()
    {
        try
        {
            clients = await NetworkService.GetClients();
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
