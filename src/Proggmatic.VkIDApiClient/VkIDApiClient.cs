using System.Text.Json;


namespace Proggmatic.VkIDApiClient;

/// <summary>
/// VK ID API client
/// </summary>
public partial class VkIDApiClient : IDisposable
{
    private readonly HttpClient _httpClient;
    private readonly VkIDApiClientConfig _apiClientConfig;
    private static readonly JsonSerializerOptions _serializerOptions = new(JsonSerializerDefaults.Web);

    /// <summary>
    /// Constructor for creating with inline config
    /// </summary>
    /// <param name="httpClient"></param>
    /// <param name="config"></param>
    public VkIDApiClient(HttpClient httpClient, VkIDApiClientConfig config)
    {
        _httpClient = httpClient;
        _apiClientConfig = config;
    }


    /// <summary>
    /// Releases the unmanaged resources and disposes of the managed resources
    /// </summary>
    public void Dispose()
    {
        _httpClient.Dispose();
    }
}