namespace Proggmatic.VkIDApiClient;

/// <summary>
/// Configuration for all API clients
/// </summary>
public class VkIDApiConfig
{
    public VkIDApiClientConfig? Web { get; set; }

    public VkIDApiClientConfig? Android { get; set; }

    public VkIDApiClientConfig? Ios { get; set; }
}