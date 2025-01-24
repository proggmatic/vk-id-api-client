using System.Text.Json.Serialization;


namespace Proggmatic.VkIDApiClient.DTO;

/// <summary>
/// Response wrapper over "user"
/// </summary>
/// <typeparam name="TResponse"></typeparam>
public record VkUserResponseWrapper<TResponse>
{
    /// <summary>
    /// User object
    /// </summary>
    [JsonPropertyName("user")]
    public TResponse User { get; set; } = default!;
}