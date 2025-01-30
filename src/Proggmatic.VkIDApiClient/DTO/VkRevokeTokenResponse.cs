using System.Text.Json.Serialization;


namespace Proggmatic.VkIDApiClient.DTO;

/// <summary>
/// Revoke token response
/// </summary>
public class VkRevokeTokenResponse
{
    /// <summary>
    /// Если запрос выполнен успешно, выданные пользователем разрешения доступов будут отозваны у приложения — в ответе вернется значение 1.
    /// </summary>
    [JsonPropertyName("response")]
    public int Response { get; set; }
}