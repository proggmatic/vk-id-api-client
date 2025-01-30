using System.Net.Http.Json;
using System.Text.Json;

using Proggmatic.VkIDApiClient.DTO;


namespace Proggmatic.VkIDApiClient;

public partial class VkIDApiClient
{
    /// <summary>
    /// Exchange authentication code to access tokens
    /// </summary>
    /// <param name="code">Authorization code received after authorization with backend</param>
    /// <param name="deviceId">Unique device ID received together with <paramref name="code"/></param>
    /// <param name="codeVerifier">Параметр, который обеспечивает защиту передаваемых данных. Параметр применяется при PKCE. Случайно сгенерированная строка, новая на каждый запрос авторизации. Может состоять из следующих символов: a-z, A-Z, 0-9, _, -. Длина от 43 до 128 символов. На основании строки формируется code_challenge: сервер преобразует code_verifier методом code_challenge_method, полученным в запросе на отправку кода подтверждения, и сравнивает результат с code_challenge из того же запроса. Параметр обязателен для обмена кода на токен</param>
    /// <param name="state">Строка состояния в виде случайного набора символов: a-z, A-Z, 0-9, _, -, длиной не менее 32 символа. Генерируется сервисом и передается в начале авторизации. Строка должна возвращаться сервису без изменений. Затем сервису нужно сравнить полученный state с сохраненным: значения должны полностью совпадать, иначе ответ можно считать подменённым. Не передавайте данные в виде значения этого параметра. Вы можете хранить эти данные в сервисе и использовать state для их получения, например ключ — это хэш от state</param>
    /// <param name="cancellationToken">Cancellation token to cancel request</param>
    /// <exception cref="HttpRequestException">If status code is not success</exception>
    /// <exception cref="VkIDApiException">In case of error response</exception>
    public async Task<VkAccessTokenResponse> GetAccessTokenByAuthCode(string code, string deviceId, string codeVerifier, string state, CancellationToken cancellationToken = default)
    {
        var parameters = new Dictionary<string, string>
        {
            { "grant_type", "authorization_code" },
            { "code_verifier", codeVerifier },
            { "redirect_uri", _apiClientConfig.RedirectUrl },
            { "code", code },
            { "client_id", _apiClientConfig.ApplicationId.ToString() },
            { "device_id", deviceId },
            { "state", state }
        };

        var response = await _httpClient.PostAsync(
            "https://id.vk.com/oauth2/auth",
            new FormUrlEncodedContent(parameters),
            cancellationToken
        ).ConfigureAwait(false);

        if (!response.IsSuccessStatusCode)
        {
            var errorResponse = await response.Content.ReadFromJsonAsync<VkErrorResponse>((JsonSerializerOptions?)null, cancellationToken).ConfigureAwait(false);
            throw new VkIDApiException(errorResponse?.Error ?? "Access token error response could not be parsed.", errorResponse?.ErrorDescription, (int)response.StatusCode);
        }

        var stringResponse = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
        if (stringResponse.Contains("\"error\":"))
        {
            var errorResponse = JsonSerializer.Deserialize<VkErrorResponse>(stringResponse, _serializerOptions);
            throw new VkIDApiException(errorResponse?.Error ?? "Access token error response could not be parsed.", errorResponse?.ErrorDescription, (int)response.StatusCode);
        }

        var result = JsonSerializer.Deserialize<VkAccessTokenResponse>(stringResponse, _serializerOptions);
        if (result == null)
            throw new VkIDApiException("Access token response could not be parsed.", responseCode: (int)response.StatusCode);

        return result;
    }

    /// <summary>
    /// Exchange authentication code to access tokens
    /// </summary>
    /// <param name="refreshToken">Токен, который используется для обмена на новую пару Access token + Refresh token</param>
    /// <param name="deviceId">Уникальный идентификатор устройства, полученный вместе с авторизационным кодом</param>
    /// <param name="state">Строка состояния в виде случайного набора символов: a-z, A-Z, 0-9, _, -, длиной не менее 32 символа. Генерируется сервисом и передается в начале авторизации. Строка должна возвращаться сервису без изменений. Затем сервису нужно сравнить полученный state с сохраненным: значения должны полностью совпадать, иначе ответ можно считать подменённым. Не передавайте данные в виде значения этого параметра. Вы можете хранить эти данные в сервисе и использовать state для их получения, например ключ — это хэш от state</param>
    /// <param name="scope">Список прав доступа, которые необходимы приложению. Значения в списке разделяются пробелами, например: wall email. Если список не указан, то он будет взят из пришедшего Refresh token</param>
    /// <param name="cancellationToken">Cancellation token to cancel request</param>
    /// <exception cref="HttpRequestException">If status code is not success</exception>
    /// <exception cref="VkIDApiException">In case of error response</exception>
    public async Task<VkRefreshTokenResponse> RefreshToken(string refreshToken, string deviceId, string state, string? scope = null, CancellationToken cancellationToken = default)
    {
        var parameters = new Dictionary<string, string>
        {
            { "grant_type", "refresh_token" },
            { "refresh_token", refreshToken },
            { "client_id", _apiClientConfig.ApplicationId.ToString() },
            { "device_id", deviceId },
            { "state", state }
        };

        if (!string.IsNullOrEmpty(scope))
            parameters.Add("scope", scope);

        var response = await _httpClient.PostAsync(
            "https://id.vk.com/oauth2/auth",
            new FormUrlEncodedContent(parameters),
            cancellationToken
        ).ConfigureAwait(false);

        if (!response.IsSuccessStatusCode)
        {
            var errorResponse = await response.Content.ReadFromJsonAsync<VkErrorResponse>((JsonSerializerOptions?)null, cancellationToken).ConfigureAwait(false);
            throw new VkIDApiException(errorResponse?.Error ?? "Refresh token code error response could not be parsed.", errorResponse?.ErrorDescription, (int)response.StatusCode);
        }

        var stringResponse = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
        if (stringResponse.Contains("\"error\":"))
        {
            var errorResponse = JsonSerializer.Deserialize<VkErrorResponse>(stringResponse, _serializerOptions);
            throw new VkIDApiException(errorResponse?.Error ?? "Refresh token error response could not be parsed.", errorResponse?.ErrorDescription, (int)response.StatusCode);
        }

        var result = JsonSerializer.Deserialize<VkRefreshTokenResponse>(stringResponse, _serializerOptions);
        if (result == null)
            throw new VkIDApiException("Refresh token response could not be parsed.", responseCode: (int)response.StatusCode);

        return result;
    }


    /// <summary>
    /// Revoke application permissions
    /// </summary>
    /// <param name="accessToken">Access token of user</param>
    /// <param name="cancellationToken">Cancellation token to cancel request</param>
    /// <exception cref="HttpRequestException">If status code is not success</exception>
    /// <exception cref="VkIDApiException">In case of error response</exception>
    public async Task<VkRevokeTokenResponse> RevokeAppPermissions(string accessToken, CancellationToken cancellationToken = default)
    {
        var parameters = new Dictionary<string, string>
        {
            { "access_token", accessToken },
            { "client_id", _apiClientConfig.ApplicationId.ToString() },
        };

        var response = await _httpClient.PostAsync(
            "https://id.vk.com/oauth2/revoke",
            new FormUrlEncodedContent(parameters),
            cancellationToken
        ).ConfigureAwait(false);

        if (!response.IsSuccessStatusCode)
        {
            var errorResponse = await response.Content.ReadFromJsonAsync<VkErrorResponse>((JsonSerializerOptions?)null, cancellationToken).ConfigureAwait(false);
            throw new VkIDApiException(errorResponse?.Error ?? "Revoke app permissions error response could not be parsed.", errorResponse?.ErrorDescription, (int)response.StatusCode);
        }

        var stringResponse = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
        if (stringResponse.Contains("\"error\":"))
        {
            var errorResponse = JsonSerializer.Deserialize<VkErrorResponse>(stringResponse, _serializerOptions);
            throw new VkIDApiException(errorResponse?.Error ?? "Revoke app permissions error response could not be parsed.", errorResponse?.ErrorDescription, (int)response.StatusCode);
        }

        var result = JsonSerializer.Deserialize<VkRevokeTokenResponse>(stringResponse, _serializerOptions);
        if (result == null)
            throw new VkIDApiException("Revoke app permissions response could not be parsed.", responseCode: (int)response.StatusCode);

        return result;
    }


    /// <summary>
    /// Logout user and invalidate tokens
    /// </summary>
    /// <param name="accessToken">Access token of user</param>
    /// <param name="cancellationToken">Cancellation token to cancel request</param>
    /// <exception cref="HttpRequestException">If status code is not success</exception>
    /// <exception cref="VkIDApiException">In case of error response</exception>
    public async Task<VkRevokeTokenResponse> Logout(string accessToken, CancellationToken cancellationToken = default)
    {
        var parameters = new Dictionary<string, string>
        {
            { "access_token", accessToken },
            { "client_id", _apiClientConfig.ApplicationId.ToString() },
        };

        var response = await _httpClient.PostAsync(
            "https://id.vk.com/oauth2/logout",
            new FormUrlEncodedContent(parameters),
            cancellationToken
        ).ConfigureAwait(false);

        if (!response.IsSuccessStatusCode)
        {
            var errorResponse = await response.Content.ReadFromJsonAsync<VkErrorResponse>((JsonSerializerOptions?)null, cancellationToken).ConfigureAwait(false);
            throw new VkIDApiException(errorResponse?.Error ?? "Logout user error response could not be parsed.", errorResponse?.ErrorDescription, (int)response.StatusCode);
        }

        var stringResponse = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
        if (stringResponse.Contains("\"error\":"))
        {
            var errorResponse = JsonSerializer.Deserialize<VkErrorResponse>(stringResponse, _serializerOptions);
            throw new VkIDApiException(errorResponse?.Error ?? "Logout user error response could not be parsed.", errorResponse?.ErrorDescription, (int)response.StatusCode);
        }

        var result = JsonSerializer.Deserialize<VkRevokeTokenResponse>(stringResponse, _serializerOptions);
        if (result == null)
            throw new VkIDApiException("Logout user response could not be parsed.", responseCode: (int)response.StatusCode);

        return result;
    }
}