using System.Net.Http;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using Atlasway_Internal_Management.Models;
using Newtonsoft.Json;

namespace Atlasway_Internal_Management.Services;


public static class NetworkService
{
    #region Properties

    static string baseUri = "https://localhost:8083/api/";

    #endregion

    #region Requests

    public static async Task<List<Client>> GetClients(CancellationToken cancellationToken)
    {
        using (HttpResponseMessage response = await ApiHelper.ApiClient.GetAsync(baseUri + "clients"))
        {
            if (response.IsSuccessStatusCode)
            {
                List<Client> clients;
                clients = await response.Content.ReadAsAsync<List<Client>>(cancellationToken: cancellationToken);

                return clients;
            }
            else
            {
                throw new Exception(response.ReasonPhrase);
            }
        }
    }

    public static async Task<List<Staff>> GetStaff(CancellationToken cancellationToken)
    {
        using (HttpResponseMessage response = await ApiHelper.ApiClient.GetAsync(baseUri + "staff"))
        {
            if (response.IsSuccessStatusCode)
            {
                List<Staff> staff;
                staff = await response.Content.ReadAsAsync<List<Staff>>(cancellationToken: cancellationToken);

                return staff;
            }
            else
            {
                throw new Exception(response.ReasonPhrase);
            }
        }
    }

    public static async Task<List<Project>> GetProjects(CancellationToken cancellationToken)
    {
        using (HttpResponseMessage response = await ApiHelper.ApiClient.GetAsync(baseUri + "projects"))
        {
            if (response.IsSuccessStatusCode)
            {
                List<Project> projects;
                projects = await response.Content.ReadAsAsync<List<Project>>(cancellationToken: cancellationToken);

                return projects;
            }
            else
            {
                throw new Exception(response.ReasonPhrase);
            }
        }
    }

    public static async void PostProject(NewProject project, CancellationToken cancellationToken)
    {
        string json = JsonConvert.SerializeObject(project);

        StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

        using (HttpResponseMessage response = await ApiHelper.ApiClient.PostAsync(baseUri + "projects", content))
        {
            if (response.IsSuccessStatusCode is not true)
            {
                throw new Exception(response.ToString());
            }
        }
    }

    public static async Task<List<ProjectStatusType>> GetProjectStatusTypes(CancellationToken cancellationToken)
    {
        using (HttpResponseMessage response = await ApiHelper.ApiClient.GetAsync(baseUri + "projectstatustypes"))
        {
            if (response.IsSuccessStatusCode)
            {
                List<ProjectStatusType> projectStatusTypes;
                projectStatusTypes = await response.Content.ReadAsAsync<List<ProjectStatusType>>(cancellationToken: cancellationToken);

                return projectStatusTypes;
            }
            else
            {
                throw new Exception(response.ReasonPhrase);
            }
        }
    }

    #endregion
}
