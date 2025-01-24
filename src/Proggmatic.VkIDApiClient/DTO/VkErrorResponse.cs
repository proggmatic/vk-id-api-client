using System.Text.Json.Serialization;


namespace Proggmatic.VkIDApiClient.DTO;

/// <summary>
/// Response with error from VK
/// </summary>
public class VkErrorResponse
{
    /// <summary>
    /// Error title
    /// </summary>
    [JsonPropertyName("error")]
    public string Error { get; set; } = null!;

    /// <summary>
    /// Error description
    /// </summary>
    [JsonPropertyName("error_description")]
    public string? ErrorDescription { get; set; }
}