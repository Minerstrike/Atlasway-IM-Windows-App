using Atlasway_Internal_Management.Core;
using Atlasway_Internal_Management.Models;
using Atlasway_Internal_Management.Services;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

namespace Atlasway_Internal_Management.Windows.Pages;

/// <summary>
/// Interaction logic for ProjectsPage.xaml
/// </summary>
public partial class ProjectsPage : BasePage
{
    #region Properties

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

    private List<ProjectStatusType> _projectStatusTypes = [];
    public List<ProjectStatusType> projectStatusTypes
    {
        get => _projectStatusTypes;
        set
        {
            _projectStatusTypes = value;
            NotifyPropertyChanged();
        }
    }

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

    #endregion

    #region Constructor

    public ProjectsPage()
    {
        InitializeComponent();

        Loaded += InitialNetworkRequests;
    }

    public ProjectsPage(bool isPopable = true)
    {
        InitializeComponent();

        Loaded += InitialNetworkRequests;

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
            NotifyPropertyChanged(nameof(filteredProjects));
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

    public List<Project> filteredProjects
    {
        get
        {
            List<Project> projects = this.projects;

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

    #endregion

    #region Network Requests

    private async void InitialNetworkRequests(object? sender, EventArgs e)
    {
        await RefreshData(refreshAll: true);
    }

    internal async Task RefreshData(bool refreshAll)
    {
        try
        {
            Task[] tasks = { GetProjects() };

            if (refreshAll)
            {
                tasks.Append(GetClients());
                tasks.Append(GetProjectTypes());
            }

            await Task.WhenAll(
                tasks
            );
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Failure to access data", MessageBoxButton.OK, MessageBoxImage.Error);
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
            MessageBox.Show(ex.Message, "Failure to get data", MessageBoxButton.OK, MessageBoxImage.Error);
            cancellationTokenSource.Cancel();
        }
    }

    public async Task GetProjectTypes()
    {
        CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

        try
        {
            projectStatusTypes = await NetworkService.GetProjectStatusTypes(cancellationTokenSource.Token);
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Failure to get data", MessageBoxButton.OK, MessageBoxImage.Error);
            cancellationTokenSource.Cancel();
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
            MessageBox.Show(ex.Message, "Failure to get data", MessageBoxButton.OK, MessageBoxImage.Error);
            cancellationTokenSource.Cancel();
        }
    }

    #endregion

    #region Button events

    private void PopToWindow_Click(object sender, RoutedEventArgs e)
    {
        new GenericPageWindow(new ProjectsPage(isPopable: false)).Show();
    }

    #endregion

    #region ITitledObject

    public override string title => $"Projects";

    #endregion
}
