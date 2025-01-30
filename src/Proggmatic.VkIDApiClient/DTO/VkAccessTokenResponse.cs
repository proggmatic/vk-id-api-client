using System.Text.Json.Serialization;


namespace Proggmatic.VkIDApiClient.DTO;

/// <summary>
/// Ответ метода обмена кода на токен
/// </summary>
public class VkAccessTokenResponse : VkRefreshTokenResponse
{
    /// <summary>
    /// JSON Web Token (JWT) пользователя, который был получен после первичной авторизации вместе с Access и Refresh token
    /// </summary>
    [JsonPropertyName("id_token")]
    public string IdToken { get; set; } = null!;
}