using Atlasway_Internal_Management.Core;
using Atlasway_Internal_Management.Models;
using Atlasway_Internal_Management.Services;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Atlasway_Internal_Management.Windows.Pages;


/// <summary>
/// Interaction logic for ClientsPage.xaml
/// </summary>
public partial class ClientsPage : BasePage, INotifyPropertyChanged
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
            NotifyPropertyChanged(nameof(filteredClients));
        }
    }

    #endregion

    #region Constructor

    public ClientsPage()
    {
        InitializeComponent();

        Loaded += InitialNetworkRequests;
        Loaded += InitializeSearchBox;
    }

    public ClientsPage(bool isPopable)
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
            NotifyPropertyChanged(nameof(filteredClients));
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

    public List<Client> filteredClients
    {
        get
        {
            List<Client> clients = this.clients;

            if (string.IsNullOrWhiteSpace(generalSearchString) is not true && canSearch)
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

    #region DataGrid events

    private void DataGrid_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
        if (selectedClient is not null)
        {
            //ClientsDetailWindow clientsDetailWindow = new ClientsDetailWindow(selectedClient.Value);
            //clientsDetailWindow.Show();
            new GenericPageWindow(new ClientsDetailPage(selectedClient.Value)).Show();
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

    private void PopToWindow_Click(object sender, RoutedEventArgs e)
    {
        new GenericPageWindow(new ClientsPage(isPopable: false)).Show();
    }

    private async void Refresh_click(object sender, RoutedEventArgs e)
    {
        await RefreshData();
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

    public override string title => $"Clients";

    #endregion
}
