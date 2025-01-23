using System.Text.Json.Serialization;


namespace Proggmatic.VkIDApiClient.DTO;

public class VkErrorResponse
{
    /// <summary>
    /// Ошибка с одним из значений
    /// </summary>
    [JsonPropertyName("error")]
    public string Error { get; set; } = null!;

    /// <summary>
    /// Описание ошибки
    /// </summary>
    [JsonPropertyName("error_description")]
    public string? ErrorDescription { get; set; }
}