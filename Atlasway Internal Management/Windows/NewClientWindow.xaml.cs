using Atlasway_Internal_Management.Core;
using Atlasway_Internal_Management.Models;
using Atlasway_Internal_Management.Services;
using System.Windows;

namespace Atlasway_Internal_Management.Windows;


/// <summary>
/// Interaction logic for NewClientWindow.xaml
/// </summary>
public partial class NewClientWindow : ObservableWindow
{
    #region Constructor
    
    public NewClientWindow()
    {
        InitializeComponent();
    }

    #endregion

    #region Bindings

    private string _clientName = string.Empty;
    public string clientName
    {
        get => _clientName;
        set
        {
            _clientName = value;
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

    private async Task PostClient(string clientName, string? contactNo, string? emailAddress)
    {
        CancellationToken cancellationToken = new CancellationToken();

        try
        {
            NewClient client = new NewClient(
                clientName      : clientName,
                contactNo       : contactNo,
                emailAddress    : emailAddress);

            await NetworkService.PostClient(client, cancellationToken);
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.ToString(), ex.Message, MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    #endregion

    #region Button Events

    private async void ClientPost_click(object sender, System.Windows.RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(clientName))
        {
            MessageBox.Show("Please enter a name for the new client.", "Client's name is missing", MessageBoxButton.OK, MessageBoxImage.Warning);
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
            await PostClient(clientName, contactNo, emailAddress);
            MessageBox.Show("Client added.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            Close();
        }
    }


    #endregion
}
