namespace Proggmatic.VkIDApiClient;

/// <summary>
/// Configuration for API client
/// </summary>
public class VkIDApiClientConfig
{
    /// <summary>
    /// ID of VK application. Set from here https://id.vk.com/about/business/go/accounts/{my user id}/apps
    /// </summary>
    public int ApplicationId { get; set; }

    /// <summary>
    /// Valid redirect URL. You can set it up on https://id.vk.com/about/business/go/accounts/{your user ID}/apps/{your app ID}/edit
    /// </summary>
    public string RedirectUrl { get; set; } = null!;
}