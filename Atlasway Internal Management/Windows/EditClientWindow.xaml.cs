using Atlasway_Internal_Management.Core;
using Atlasway_Internal_Management.Models;
using Atlasway_Internal_Management.Services;
using System.Windows;

namespace Atlasway_Internal_Management.Windows;


/// <summary>
/// Interaction logic for EditClientWindow.xaml
/// </summary>
public partial class EditClientWindow : ObservableWindow
{
    #region Properties

    private Client _client;
    public Client client
    {
        get => _client;
        set
        {
            _client = value;
            NotifyPropertyChanged();
            NotifyPropertyChanged(nameof(titleLabel));
            NotifyPropertyChanged(nameof(clientDetailsLabel));
        }
    }

    #endregion

    #region Constructor

    public EditClientWindow(Client client)
    {
        InitializeComponent();

        this.client = client;
    }

    #endregion

    #region Bindings

    public string titleLabel => $"Edit {client.ClientName} detail";

    public string clientDetailsLabel =>
        $"Client        : {client.ClientName}\n" +
        $"Contact       : {client.ContactNo}\n" +
        $"Email address : {client.EmailAddress}";

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

    private async Task UpdateClient(string? contactNo, string? emailAddress)
    {
        CancellationToken cancellationToken = new CancellationToken();

        Client client = new Client(
            clientNo        : this.client.ClientNo,
            clientName      : this.client.ClientName,
            contactNo       : contactNo,
            emailAddress    : emailAddress);

        await NetworkService.UpdateClient(client, cancellationToken);
    }

    #endregion

    #region Button Events

    private async void ClientUpdate_click(object sender, System.Windows.RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(contactNo) && string.IsNullOrWhiteSpace(emailAddress))
        {
            MessageBox.Show("Please update at least one of the fields.", "Missing entries", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
        else
        {
            if (string.IsNullOrWhiteSpace(contactNo))
            {
                await UpdateClient(client.ContactNo, emailAddress);
            }
            else if (string.IsNullOrWhiteSpace(emailAddress))
            {
                await UpdateClient(contactNo, client.EmailAddress);
            }
            else
            {
                await UpdateClient(contactNo, emailAddress);
            }

            Close();
        }
    }


    #endregion
}
