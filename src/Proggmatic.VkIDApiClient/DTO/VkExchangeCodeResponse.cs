using System.Text.Json.Serialization;


namespace Proggmatic.VkIDApiClient.DTO;

/// <summary>
/// Ответ метода обмена кода на токен
/// </summary>
public class VkExchangeCodeResponse
{
    /// <summary>
    /// Токен, который используется для обмена на новую пару Access token + Refresh token. Передается в теле запроса
    /// </summary>
    [JsonPropertyName("refresh_token")]
    public string RefreshToken { get; set; } = null!;

    /// <summary>
    /// Access token пользователя
    /// </summary>
    [JsonPropertyName("access_token")]
    public string AccessToken { get; set; } = null!;

    /// <summary>
    /// JSON Web Token (JWT) пользователя, который был получен после первичной авторизации вместе с Access и Refresh token
    /// </summary>
    [JsonPropertyName("id_token")]
    public string IdToken { get; set; } = null!;

    /// <summary>
    /// Тип токена — по умолчанию Bearer
    /// </summary>
    [JsonPropertyName("token_type")]
    public string TokenType { get; set; } = "Bearer";

    /// <summary>
    /// Срок действия токена (в секундах)
    /// </summary>
    [JsonPropertyName("expires_in")]
    public int ExpiresIn { get; set; }

    /// <summary>
    /// Идентификатор пользователя
    /// </summary>
    [JsonPropertyName("user_id")]
    public int UserId { get; set; }

    /// <summary>
    /// Строка состояния. Должна совпадать с той, которая передана в начале авторизации. Если значение отличается — ответ считается скомпроментированным
    /// </summary>
    [JsonPropertyName("state")]
    public string State { get; set; } = null!;

    /// <summary>
    /// Список прав доступа, которые нужны приложению. Значения в списке разделяются пробелами, например: email phone.
    /// Если параметр не указан, то берется минимальное значение прав доступа по умолчанию для приложений — vkid.personal_info
    /// </summary>
    [JsonPropertyName("scope")]
    public string Scope { get; set; } = null!;
}