using System.Net.Http;
using System.Net.NetworkInformation;
using Atlasway_Internal_Management.Models;

namespace Atlasway_Internal_Management.Services;


public static class NetworkService
{
    #region Properties

    static string baseUri = "https://localhost:8083/api/";

    #endregion

    #region Requests

    public static async Task<List<Client>> GetClients()
    {
        using (HttpResponseMessage response = await ApiHelper.ApiClient.GetAsync(baseUri + "clients"))
        {
            if (response.IsSuccessStatusCode)
            {
                List<Client> clients;
                clients = await response.Content.ReadAsAsync<List<Client>>();

                return clients;
            }
            else
            {
                throw new Exception(response.ReasonPhrase);
            }
        }
    }

    public static async Task<List<Staff>> GetStaff()
    {
        using (HttpResponseMessage response = await ApiHelper.ApiClient.GetAsync(baseUri + "staff"))
        {
            if (response.IsSuccessStatusCode)
            {
                List<Staff> staff;
                staff = await response.Content.ReadAsAsync<List<Staff>>();

                return staff;
            }
            else
            {
                throw new Exception(response.ReasonPhrase);
            }
        }
    }

    public static async Task<List<Project>> GetProjects()
    {
        using (HttpResponseMessage response = await ApiHelper.ApiClient.GetAsync(baseUri + "projects"))
        {
            if (response.IsSuccessStatusCode)
            {
                List<Project> projects;
                projects = await response.Content.ReadAsAsync<List<Project>>();

                return projects;
            }
            else
            {
                throw new Exception(response.ReasonPhrase);
            }
        }
    }

    #endregion
}
