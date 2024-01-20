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
    #region Properties

    private List<Client> _clients = [];
    public List<Client> clients
    {
        get => _clients;
        set
        {
            _clients = value;
            NotifyPropertyChanged();
            NotifyPropertyChanged(nameof(filteredClients));
        }
    }

    #endregion

    #region Constructor

    public ClientsPage()
    {
        InitializeComponent();

        Loaded += InitialNetworkRequests;
    }

    #endregion

    #region Bindings

    private string _generalSearchString;
    public string generalSearchString
    {
        get => _generalSearchString;
        set
        {
            _generalSearchString = value;
            NotifyPropertyChanged();
            NotifyPropertyChanged(nameof(filteredClients));
        }
    }

    public List<Client> filteredClients
    {
        get
        {
            List<Client> clients = this.clients;

            if (string.IsNullOrWhiteSpace(generalSearchString) is not true)
            {
                clients = clients.Where(
                        client => client.ClientNo.ToString().Contains(generalSearchString)
                        || client.ClientName.IndexOf(generalSearchString, StringComparison.OrdinalIgnoreCase) != -1
                        || client.ContactNo.Contains(generalSearchString)
                        || client.EmailAddress.IndexOf(generalSearchString, StringComparison.OrdinalIgnoreCase) != -1
                    ).ToList(); 
            }

            return clients;
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
            MessageBox.Show(ex.StackTrace, ex.Message, MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    public async Task GetClients()
    {
        CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

        try
        {
            clients = await NetworkService.GetClients(cancellationTokenSource.Token);
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.StackTrace, ex.Message, MessageBoxButton.OK, MessageBoxImage.Error);
            cancellationTokenSource.Cancel();
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

    #region DataGrid events

    private void DataGrid_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
        if (selectedClient is not null)
        {
            ClientsDetailWindow clientsDetailWindow = new ClientsDetailWindow(selectedClient.Value);
            clientsDetailWindow.Show();
        }
        else
        {
            MessageBox.Show("Please select a client.", "No client selected.", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }

    #endregion

    #region Button events
    
    private void BtnClientAdd_Click(object sender, RoutedEventArgs e)
    {
        new NewClientWindow().Show();
    }

    #endregion
}
