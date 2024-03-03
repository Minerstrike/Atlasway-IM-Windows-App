using System.Net.Http;
using System.Text;
using Atlasway_Internal_Management.Models;
using AtlaswayInternalAPI.Authentication;
using Newtonsoft.Json;
using Windows.Media.Protection.PlayReady;

namespace Atlasway_Internal_Management.Services;


public static class NetworkService
{
    #region Properties

    static string baseUri = "https://localhost:8083/api/";

    #endregion

    #region Requests

    #region Clients

    public static async Task<List<Client>> GetClients(CancellationToken cancellationToken)
    {
        using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, baseUri + "clientsV2"))
        {
            request.Headers.Add(AuthConstants.ApiKeyHeaderName, AuthConstants.ApiKey);

            HttpResponseMessage response = await ApiHelper.ApiClient.SendAsync(request, cancellationToken);

            response.EnsureSuccessStatusCode();

            List<Client> clients;
            clients = await response.Content.ReadAsAsync<List<Client>>(cancellationToken: cancellationToken);

            return clients;
        }
    }

    public static async Task PostClient(NewClient client, CancellationToken cancellationToken)
    {
        string json = JsonConvert.SerializeObject(client);

        StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

        using (var request = new HttpRequestMessage(HttpMethod.Post, baseUri + "clientsV2"))
        {
            request.Headers.Add(AuthConstants.ApiKeyHeaderName, AuthConstants.ApiKey);
            request.Content = content;

            HttpResponseMessage response = await ApiHelper.ApiClient.SendAsync(request: request, cancellationToken: cancellationToken);

            response.EnsureSuccessStatusCode();
        }
    }

    public static async Task UpdateClient(Client client, CancellationToken cancellationToken)
    {
        string json = JsonConvert.SerializeObject(client);

        StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

        using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Put, baseUri + "clientsV2"))
        {
            request.Headers.Add(AuthConstants.ApiKeyHeaderName, AuthConstants.ApiKey);
            request.Content = content;

            HttpResponseMessage response = await ApiHelper.ApiClient.SendAsync(request: request, cancellationToken: cancellationToken);

            response.EnsureSuccessStatusCode();
        }
    }

    #endregion

    #region Staff

    public static async Task<List<Staff>> GetStaff(CancellationToken cancellationToken)
    {
        using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, baseUri + "staffV2"))
        {
            request.Headers.Add(AuthConstants.ApiKeyHeaderName, AuthConstants.ApiKey);

            HttpResponseMessage response = await ApiHelper.ApiClient.SendAsync(request, cancellationToken);

            response.EnsureSuccessStatusCode();

            List<Staff> staff;
            staff = await response.Content.ReadAsAsync<List<Staff>>(cancellationToken: cancellationToken);

            return staff;
        }
    }

    public static async Task PostStaff(NewStaff staff, CancellationToken cancellationToken)
    {
        string json = JsonConvert.SerializeObject(staff);

        StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

        using (var request = new HttpRequestMessage(HttpMethod.Post, baseUri + "staffV2"))
        {
            request.Headers.Add(AuthConstants.ApiKeyHeaderName, AuthConstants.ApiKey);
            request.Content = content;

            HttpResponseMessage response = await ApiHelper.ApiClient.SendAsync(request: request, cancellationToken: cancellationToken);

            response.EnsureSuccessStatusCode();
        }
    }

    public static async Task UpdateStaff(Staff staff, CancellationToken cancellationToken)
    {
        string json = JsonConvert.SerializeObject(staff);

        StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

        using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Put, baseUri + "staffV2"))
        {
            request.Headers.Add(AuthConstants.ApiKeyHeaderName, AuthConstants.ApiKey);
            request.Content = content;

            HttpResponseMessage response = await ApiHelper.ApiClient.SendAsync(request: request, cancellationToken: cancellationToken);

            response.EnsureSuccessStatusCode();
        }
    }

    #endregion

    #region Projects

    public static async Task<List<Project>> GetProjects(CancellationToken cancellationToken)
    {
        using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, baseUri + "projectsV2"))
        {
            request.Headers.Add(AuthConstants.ApiKeyHeaderName, AuthConstants.ApiKey);

            HttpResponseMessage response = await ApiHelper.ApiClient.SendAsync(request, cancellationToken);

            response.EnsureSuccessStatusCode();

            List<Project> projects;
            projects = await response.Content.ReadAsAsync<List<Project>>(cancellationToken: cancellationToken);

            return projects;
        }
    }

    public static async Task PostProject(NewProject project, CancellationToken cancellationToken)
    {
        string json = JsonConvert.SerializeObject(project);

        StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

        using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, baseUri + "projectsV2"))
        {
            request.Headers.Add(AuthConstants.ApiKeyHeaderName, AuthConstants.ApiKey);
            request.Content = content;

            HttpResponseMessage response = await ApiHelper.ApiClient.SendAsync(request, cancellationToken);

            response.EnsureSuccessStatusCode();
        }
    }

    public static async Task UpdateProject(Project project, CancellationToken cancellationToken)
    {
        string json = JsonConvert.SerializeObject(project);

        StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

        using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Put, baseUri + "projectsV2"))
        {
            request.Headers.Add(AuthConstants.ApiKeyHeaderName, AuthConstants.ApiKey);
            request.Content = content;

            HttpResponseMessage response = await ApiHelper.ApiClient.SendAsync(request: request, cancellationToken: cancellationToken);

            response.EnsureSuccessStatusCode();
        }
    }

    public static async Task<List<ProjectStatusType>> GetProjectStatusTypes(CancellationToken cancellationToken)
    {
        using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, baseUri + "projectstatustypesV2"))
        {
            request.Headers.Add(AuthConstants.ApiKeyHeaderName, AuthConstants.ApiKey);

            HttpResponseMessage response = await ApiHelper.ApiClient.SendAsync(request, cancellationToken);

            response.EnsureSuccessStatusCode();

            List<ProjectStatusType> projectStatusTypes;
            projectStatusTypes = await response.Content.ReadAsAsync<List<ProjectStatusType>>(cancellationToken);

            return projectStatusTypes;
        }
    }

    #endregion

    #endregion
}
