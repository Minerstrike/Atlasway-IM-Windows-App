using Atlasway_Internal_Management.Core;
using Atlasway_Internal_Management.Models;
using Atlasway_Internal_Management.Services;
using ControlzEx;
using Microsoft.Toolkit.Uwp.Notifications;
using System.Reflection.Emit;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace Atlasway_Internal_Management.Windows;


/// <summary>
/// Interaction logic for ClientsDetailWindow.xaml
/// </summary>
public partial class ClientsDetailWindow : ObservableWindow
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
            NotifyPropertyChanged(nameof(clientNameLabel));
            NotifyPropertyChanged(nameof(clientContactLabel));
            NotifyPropertyChanged(nameof(clientEmailAddressLabel));
        }
    }

    private List<Project> _projects = [];
    public List<Project> projects
    {
        get => _projects;
        set
        {
            _projects = value;
            NotifyPropertyChanged();
            NotifyPropertyChanged(nameof(filteredProjects));
        }
    }

    #endregion

    #region Constrcutor

    public ClientsDetailWindow(Client client)
    {
        InitializeComponent();

        ContentRendered += InitialNetworkRequests;

        this.client = client;
    }

    #endregion

    #region Bindings

    public string titleLabel => $"{client.ClientName} detail";
    public string clientNameLabel => $"Client: {client.ClientName}";
    public string clientContactLabel => $"Contact: {client.ContactNo}";
    public string clientEmailAddressLabel => $"Email address: {client.EmailAddress}";

    private string _generalSearchString;
    public string generalSearchString
    {
        get => _generalSearchString;
        set
        {
            _generalSearchString = value;
            NotifyPropertyChanged();
            NotifyPropertyChanged(nameof(filteredProjects));
        }
    }

    private Project? _selectedProject;
    public Project? selectedProject
    {
        get => _selectedProject;
        set
        {
            _selectedProject = value;
            NotifyPropertyChanged();
        }
    }

    public List<Project> filteredProjects
    {
        get
        {
            List<Project> projects = this.projects;

            projects = projects.Where(project => project.ClientNo == client.ClientNo).ToList();

            if (string.IsNullOrWhiteSpace(generalSearchString) is not true)
            {
                projects = projects.Where(
                        project => project.ProjectNo.ToString().Contains(generalSearchString)
                        || project.ProjectName.IndexOf(generalSearchString, StringComparison.OrdinalIgnoreCase) != -1
                        || project.ClientNo.ToString().Contains(generalSearchString)
                    ).ToList();
            }

            return projects;
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
                GetProjects()
            );
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
    }

    public async Task GetProjects()
    {
        CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

        try
        {
            projects = await NetworkService.GetProjects(cancellationTokenSource.Token);
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.StackTrace, ex.Message, MessageBoxButton.OK, MessageBoxImage.Error);
            cancellationTokenSource.Cancel();
        }
    }

    #endregion

    #region Label events

    private void nameLabel_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
        Clipboard.SetText($"{client.ClientName}");
        new ToastContentBuilder()
            .AddText($"Copied {client.ClientName}")
            .Show();
    }

    private void contactLabel_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
        Clipboard.SetText($"{client.ContactNo}");
        new ToastContentBuilder()
            .AddText($"Copied {client.ContactNo}")
            .Show();
    }

    private void emailAddressLabel_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
        Clipboard.SetText($"{client.EmailAddress}");
        new ToastContentBuilder()
            .AddText($"Copied {client.EmailAddress}")
            .Show();
    }

    #endregion
}
