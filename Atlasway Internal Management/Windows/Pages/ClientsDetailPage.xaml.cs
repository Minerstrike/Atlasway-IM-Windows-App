﻿using Atlasway_Internal_Management.Core;
using Atlasway_Internal_Management.Models;
using Atlasway_Internal_Management.Services;
using Atlasway_Internal_Management.Windows.Interfaces;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Atlasway_Internal_Management.Windows.Pages;


/// <summary>
/// Interaction logic for ClientsDetailPage.xaml
/// </summary>
public partial class ClientsDetailPage : BasePage
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
            NotifyPropertyChanged(nameof(title));
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

    private bool _canSearch = false;
    public bool canSearch
    {
        get => _canSearch;
        set
        {
            _canSearch = value;
            NotifyPropertyChanged();
            NotifyPropertyChanged(nameof(filteredProjects));
        }
    }


    #endregion

    #region Constructor

    public ClientsDetailPage(Client client)
    {
        InitializeComponent();

        Loaded += InitialNetworkRequests;
        Loaded += InitializeSearchBox;

        this.client = client;
    }

    #endregion

    #region Bindings

    public string clientDetailsLabel =>
        $"Client        : {client.ClientName}\n" +
        $"Contact       : {client.ContactNo}\n" +
        $"Email address : {client.EmailAddress}";

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

            if (string.IsNullOrWhiteSpace(generalSearchString) is not true && canSearch)
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

    #region Button Events

    private async void BtnClientEdit_Click(object sender, RoutedEventArgs e)
    {
        new EditClientWindow(client).ShowDialog();
        await RefreshData(true);
    }

    private async void BtnProjectAdd_Click(object sender, RoutedEventArgs e)
    {
        new NewProjectWindow(client).ShowDialog();
        await RefreshData();
    }

    private async void Refresh_click(object sender, RoutedEventArgs e)
    {
        if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
        {
            await RefreshData(true);
        }
        else
        {
            await RefreshData(false);
        }
    }

    #endregion

    #region ITitledObject

    public override string title => $"{client.ClientName} detail";

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

    #region ITwoDimentional

    private double _height = 450;
    public override double height
    {
        get => _height;
        set
        {
            _height = value;
            NotifyPropertyChanged();
        }
    }

    private double _width = 800;
    public override double width
    {
        get => _width;
        set
        {
            _width = value;
            NotifyPropertyChanged();
        }
    }

    public override double minHeight => 300;
    public override double minWidth => 480;

    #endregion
}
