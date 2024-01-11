using System.Net.Http.Headers;
using System.Net.Http;

namespace Atlasway_Internal_Management;


public static class ApiHelper
{
    public static HttpClient ApiClient { get; private set; } = new HttpClient();

    public static void InitializeClient()
    {
        ApiClient = new HttpClient();
        ApiClient.DefaultRequestHeaders.Accept.Clear();
        ApiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    }
}
