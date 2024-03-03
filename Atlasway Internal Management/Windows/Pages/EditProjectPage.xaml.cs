using Atlasway_Internal_Management.Core;
using Atlasway_Internal_Management.Models;
using Atlasway_Internal_Management.Services;
using Atlasway_Internal_Management.Windows.Interfaces;
using System.Windows;

namespace Atlasway_Internal_Management.Windows.Pages;


/// <summary>
/// Interaction logic for EditProjectPage.xaml
/// </summary>
public partial class EditProjectPage : BasePage
{
    #region Properties

    private bool _isEditable = false;
    public bool isEditable
    {
        get => _isEditable;
        set
        {
            _isEditable = value;
            NotifyPropertyChanged();
            NotifyPropertyChanged(nameof(editVisibility));
            NotifyPropertyChanged(nameof(minHeight));
            NotifyPropertyChanged(nameof(height));
        }
    }

    private Client _client;
    private Client client
    {
        get => _client;
        set
        {
            _client = value;
            NotifyPropertyChanged();
            NotifyPropertyChanged(nameof(title));
            NotifyPropertyChanged(nameof(clientDetailsLabel));
        }
    }

    private Project _project;
    private Project project
    {
        get => _project;
        set
        {
            _project = value;
            NotifyPropertyChanged();
            NotifyPropertyChanged(nameof(title));
            NotifyPropertyChanged(nameof(projectDetailsLabel));
        }
    }

    private ICloseable _container;
    public ICloseable container
    {
        get => _container;
        set 
        {
            _container = value;
            NotifyPropertyChanged();
        }
    }


    #endregion

    #region Constructor

    public EditProjectPage(Client client, Project project, ICloseable container)
    {
        InitializeComponent();

        Loaded += InitialNetworkRequests;

        this.client = client;
        this.project = project;
        this.container = container;
    }

    #endregion

    #region Bindings

    public string clientDetailsLabel =>
        $"Client        : {client.ClientName}\n" +
        $"Contact       : {client.ContactNo}\n" +
        $"Email address : {client.EmailAddress}";

    public string projectDetailsLabel =>
        $"Project no    : {project.ProjectNo}\n"  +
        $"Project name  : {project.ProjectName}\n" +
        $"Status        : {project.StatusNo}";

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

    private ProjectStatusType? _selectedProjectStatusType = null;
    public ProjectStatusType? selectedProjectStatusType
    {
        get => _selectedProjectStatusType;
        set
        {
            _selectedProjectStatusType = value;
            NotifyPropertyChanged();
        }
    }

    public Visibility editVisibility
    {
        get
        {
            if (isEditable)
            {
                return Visibility.Visible;
            }
            else
            {
                return Visibility.Collapsed;
            }
        }
    }

    #endregion

    #region Network request

    private async void InitialNetworkRequests(object? sender, EventArgs e)
    {
        await RefreshData();
    }

    internal async Task RefreshData()
    {
        try
        {
            Task[] tasks = { GetProjectStatusTypes() };

            await Task.WhenAll(tasks);
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
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

    public async Task UpdateProject()
    {
        CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

        try
        {
            await NetworkService.UpdateProject(
                new Project(
                    projectNo   : project.ProjectNo,
                    projectName : projectName,
                    clientNo    : project.ClientNo,
                    statusNo    : selectedProjectStatusType.Value.TypeNo),
                cancellationTokenSource.Token);
            MessageBox.Show("Project was updated.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.StackTrace, ex.Message, MessageBoxButton.OK, MessageBoxImage.Error);
            cancellationTokenSource.Cancel();
        }
    }

    #endregion

    #region Button events

    private async void UpdateProject_Button(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(projectName))
        {
            MessageBox.Show("Missing project name. \nPlease enter a project name.", "Project name needed", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        if (selectedProjectStatusType == null)
        {
            MessageBox.Show("Missing project status. \nPlease select a project status.", "Project status needed", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        await UpdateProject();
        container.Close();
    }

    private void ShowEditContent_Click(object sender, RoutedEventArgs e)
    {
        // Enable editing
        isEditable = !isEditable;

        // Initialise data for editing
        projectName = project.ProjectName;
        selectedProjectStatusType = projectStatusTypes.FirstOrDefault(type => type.TypeNo == project.StatusNo);
    }

    private void ResetContent_Click(object sender, RoutedEventArgs e)
    {
        projectName = project.ProjectName;
        selectedProjectStatusType = projectStatusTypes.FirstOrDefault(type => type.TypeNo == project.StatusNo);
    }

    private void FillProjectName_Click(object sender, RoutedEventArgs e)
    {
        projectName = project.ProjectName;
    }

    private void FillProjectStatus_Click(object sender, RoutedEventArgs e)
    {
        selectedProjectStatusType = projectStatusTypes.FirstOrDefault(type => type.TypeNo == project.StatusNo);
    }

    #endregion

    #region ITitledObject

    public override string title => $"Edit '{project.ProjectName}' for {client.ClientName}";

    #endregion

    #region ITwoDimensional

    private double _height = 200;
    public override double height
    {
        get
        {
            if (isEditable)
            {
                _height = 340;
            }
            else
            {
                _height = 150;
            }

            return _height;
        }

        set
        {
            _height = value;
            NotifyPropertyChanged();
        }
    }

    private double _width = 600;
    public override double width
    {
        get => _width;
        set
        {
            _width = value;
            NotifyPropertyChanged();
        }
    }


    private double _minHeight = 300;
    public override double minHeight
    {
        get
        {
            if (isEditable)
            {
                _minHeight = 340;
            }
            else
            {
                _minHeight = 200;
            }

            return _minHeight;
        }
    }
    public override double minWidth => 400;

    #endregion
}
