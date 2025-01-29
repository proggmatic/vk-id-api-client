using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;


namespace Proggmatic.VkIDApiClient;

/// <summary>
/// VK ID API client
/// </summary>
public class VkIDApiClientFactory
{
    private readonly HttpClient _httpClient;
    private readonly IOptionsSnapshot<VkIDApiConfig> _options;

    /// <summary>
    /// Constructor for dependency injection
    /// </summary>
    [ActivatorUtilitiesConstructor]
    public VkIDApiClientFactory(HttpClient httpClient, IOptionsSnapshot<VkIDApiConfig> options)
    {
        _httpClient = httpClient;
        _options = options;
    }

    /// <summary>
    /// Constructor for creating with inline config
    /// </summary>
    /// <param name="httpClient"></param>
    /// <param name="config"></param>
    public VkIDApiClientFactory(HttpClient httpClient, VkIDApiConfig config)
    {
        _httpClient = httpClient;
        _options = new StaticOptionsSnapshot<VkIDApiConfig>(config);
    }


    /// <summary>
    /// Create VK ID API client and configure it by selected platform
    /// </summary>
    /// <param name="platform">One of <see cref="VkIDAppPlatform"/> values</param>
    /// <exception cref="VkIDApiException">If platform is not configured</exception>
    public VkIDApiClient CreateApiClient(VkIDAppPlatform platform)
    {
        VkIDApiClientConfig? config = platform switch
        {
            VkIDAppPlatform.Web => _options.Value.Web,
            VkIDAppPlatform.Android => _options.Value.Android,
            VkIDAppPlatform.Ios => _options.Value.Ios,
            _ => null
        };

        if (config == null)
            throw new VkIDApiException($"Config for platform {platform:G} is not found. Fill it first.");

        return new VkIDApiClient(_httpClient, config);
    }
}