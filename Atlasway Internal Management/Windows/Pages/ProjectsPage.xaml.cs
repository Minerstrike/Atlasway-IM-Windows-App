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
public partial class ProjectsPage : Page, INotifyPropertyChanged
{
    #region Constructor

    public ProjectsPage()
    {
        InitializeComponent();

        Loaded += InitialNetworkRequests;
    }

    #endregion

    #region Bindings

    private List<Project> _projects = [];
    public List<Project> projects
    {
        get => _projects;
        set
        {
            _projects = value;
            NotifyPropertyChanged();
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
        try
        {
            projects = await NetworkService.GetProjects();
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
