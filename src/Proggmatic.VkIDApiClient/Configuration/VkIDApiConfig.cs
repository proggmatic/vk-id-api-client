namespace Proggmatic.VkIDApiClient;

/// <summary>
/// Configuration for all API clients
/// </summary>
public class VkIDApiConfig
{
    /// <summary>
    /// Configuration for web applications
    /// </summary>
    public VkIDApiClientConfig? Web { get; set; }

    /// <summary>
    /// Configuration for Android applications
    /// </summary>
    public VkIDApiClientConfig? Android { get; set; }

    /// <summary>
    /// Configuration for Ios applications
    /// </summary>
    public VkIDApiClientConfig? Ios { get; set; }
}