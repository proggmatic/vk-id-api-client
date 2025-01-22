using System.Text.Json.Serialization;


namespace Proggmatic.VkIDApiClient.DTO;

public class UserInfoResponse
{
    /// <summary>
    /// ID пользователя
    /// </summary>
    [JsonPropertyName("user_id")]
    public string UserId { get; set; } = null!;

    /// <summary>
    /// Имя пользователя
    /// </summary>
    [JsonPropertyName("first_name")]
    public string FirstName { get; set; } = null!;

    /// <summary>
    /// Фамилия пользователя
    /// </summary>
    [JsonPropertyName("last_name")]
    public string LastName { get; set; } = null!;

    /// <summary>
    /// Ссылка на фото профиля
    /// </summary>
    [JsonPropertyName("avatar")]
    public string? Avatar { get; set; }

    /// <summary>
    /// Пол
    /// </summary>
    [JsonPropertyName("sex")]
    public SexType Sex { get; set; } = SexType.Unknown;

    /// <summary>
    /// День рождения
    /// </summary>
    [JsonPropertyName("birthday")]
    public DateOnly? Birthday { get; set; }

    /// <summary>
    /// Телефон пользователя
    /// </summary>
    [JsonPropertyName("phone")]
    public string? Phone { get; set; }

    /// <summary>
    /// Почта пользователя
    /// </summary>
    [JsonPropertyName("email")]
    public string? Email { get; set; }

    /// <summary>
    /// Статус верификации профиля — синяя галочка рядом с именем пользователя, которая выдается известным личностям: музыкантам, блогерам, спортсменам, и.т.д.
    /// Возможные значения: true — верифицирован, false — не верифицирован
    /// </summary>
    [JsonPropertyName("verified")]
    public bool? Verified { get; set; }
}