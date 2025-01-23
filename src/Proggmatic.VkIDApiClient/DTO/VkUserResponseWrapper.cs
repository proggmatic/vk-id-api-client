using System.Text.Json.Serialization;


namespace Proggmatic.VkIDApiClient.DTO;

public record VkUserResponseWrapper<TResponse>
{
    /// <summary>
    /// User object
    /// </summary>
    [JsonPropertyName("user")]
    public TResponse User { get; set; } = default!;
}