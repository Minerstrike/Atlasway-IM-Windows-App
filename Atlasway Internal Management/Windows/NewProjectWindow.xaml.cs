using Atlasway_Internal_Management.Core;
using Atlasway_Internal_Management.Models;
using Atlasway_Internal_Management.Services;
using System.Windows;

namespace Atlasway_Internal_Management.Windows;


/// <summary>
/// Interaction logic for NewProjectWindow.xaml
/// </summary>
public partial class NewProjectWindow : ObservableWindow
{
    #region Properties

    private Client _client;
    private Client client
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

    public const int INCOMPLETE_PROJECT_STATUS_NO = 100;

    #endregion

    #region Constructor

    public NewProjectWindow(Client client)
    {
        InitializeComponent();

        this.client = client;
    }

    #endregion

    #region Bindings

    public string titleLabel => $"Add new project for {client.ClientName}";

    public string clientDetailsLabel =>
        $"Client        : {client.ClientName}\n" +
        $"Contact       : {client.ContactNo}\n" +
        $"Email address : {client.EmailAddress}";

    public string projectDetailsLabel =>
        $"Status        : Incomplete";

    private string _projectName = string.Empty;
    public string projectName
    {
        get => _projectName;
        set 
        { 
            _projectName = value;
            NotifyPropertyChanged();
        }
    }

    #endregion

    #region Network request

    public async Task PostProject()
    {
        CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

        try
        {
            await NetworkService.PostProject(new NewProject(projectName: projectName, clientNo: client.ClientNo, statusNo: INCOMPLETE_PROJECT_STATUS_NO), cancellationTokenSource.Token);
            MessageBox.Show("New project was added.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.StackTrace, ex.Message, MessageBoxButton.OK, MessageBoxImage.Error);
            cancellationTokenSource.Cancel();
        }
    }

    #endregion

    #region Button events

    private async void BtnProjectAdd_Click(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(projectName))
        {
            MessageBox.Show("Please enter the project name.", "Missing field", MessageBoxButton.OK, MessageBoxImage.Warning); 
        }
        else
        {
            await PostProject();
            Close();
        }
    }

    #endregion
}
