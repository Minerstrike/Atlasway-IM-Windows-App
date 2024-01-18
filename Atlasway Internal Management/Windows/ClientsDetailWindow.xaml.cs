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
            NotifyPropertyChanged(nameof(clientDetailsLabel));
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

    private List<ProjectStatusType> _projectStatusTypes;
    public List<ProjectStatusType> projectStatusTypes
    {
        get => _projectStatusTypes;
        set
        {
            _projectStatusTypes = value;
            NotifyPropertyChanged();
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
    public string clientDetailsLabel =>
        $"Client: {client.ClientName}\n" +
        $"Contact: {client.ContactNo}\n" +
        $"Email address: {client.EmailAddress}";

    private string _generalSearchString = string.Empty;
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
        await RefreshData(true);
    }

    internal async Task RefreshData(bool refreshAll = false)
    {
        try
        {
            Task[] tasks = { GetProjects() };

            if (refreshAll)
            {
                tasks.Append(GetProjectStatusTypes());
            }

            await Task.WhenAll(tasks);
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

    public async Task GetProjectStatusTypes()
    {
        CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

        try
        {
            projectStatusTypes = await NetworkService.GetProjectStatusTypes(cancellationTokenSource.Token);
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.StackTrace, ex.Message, MessageBoxButton.OK, MessageBoxImage.Error);
            cancellationTokenSource.Cancel();
        }
    }

    #endregion

    #region Button Events

    private void BtnClientEdit_Click(object sender, RoutedEventArgs e)
    {
        MessageBox.Show("The ability to edit a client is not yet available", "Feature not implemented", MessageBoxButton.OK, MessageBoxImage.Warning);
    }

    private async void BtnProjectAdd_Click(object sender, RoutedEventArgs e)
    {
        new NewProjectWindow(client).ShowDialog();
        await RefreshData();
    }

    #endregion

}
