using System.Net.Http.Json;
using System.Text.Json;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

using Proggmatic.VkIDApiClient.DTO;


namespace Proggmatic.VkIDApiClient;

public class VkIDApiClient : IDisposable
{
    private readonly IHttpClientFactory? _httpClientFactory;
    private readonly HttpClient? _httpClient;
    private readonly IOptionsSnapshot<VkIDApiClientConfig> _options;


    [ActivatorUtilitiesConstructor]
    public VkIDApiClient(HttpClient httpClient, IOptionsSnapshot<VkIDApiClientConfig> options)
    {
        _httpClient = httpClient;
        _options = options;
    }

    //[ActivatorUtilitiesConstructor]
    public VkIDApiClient(IHttpClientFactory httpClientFactory, IOptionsSnapshot<VkIDApiClientConfig> options)
    {
        _httpClientFactory = httpClientFactory;
    }


    public VkIDApiClient(HttpClient httpClient, VkIDApiClientConfig config)
    {
        _httpClient = httpClient;
        _options = new StaticOptionsSnapshot<VkIDApiClientConfig>(config);
    }


    /// <summary>
    /// Exchange authentication code to access tokens
    /// </summary>
    /// <exception cref="HttpRequestException">If status code is not success</exception>
    /// <exception cref="VkIDApiException"></exception>
    public async Task<ExchangeCodeResponse> ExchangeCode(string code, string deviceId, string codeVerifier, string state, CancellationToken cancellationToken = default)
    {
        var parameters = new Dictionary<string, string>
        {
            { "grant_type", "authorization_code" },
            { "code_verifier", codeVerifier },
            // { "redirect_uri", redirectUri },
            { "code", code },
            { "client_id", _options.Value.ApplicationId.ToString() },
            { "device_id", deviceId },
            { "state", state }
        };

        using var httpClient = GetHttpClient();
        var response = await httpClient.PostAsync(
            "https://id.vk.com/oauth2/auth",
            new FormUrlEncodedContent(parameters),
            cancellationToken
        ).ConfigureAwait(false);

        if (!response.IsSuccessStatusCode)
        {
            var errorResponse = await response.Content.ReadFromJsonAsync<ErrorResponse>((JsonSerializerOptions?)null, cancellationToken);
            throw new VkIDApiException(errorResponse?.Error ?? "Exchange code error response could not be parsed.", errorResponse?.ErrorDescription);
        }

        var result = await response.Content.ReadFromJsonAsync<ExchangeCodeResponse>((JsonSerializerOptions?)null, cancellationToken);
        if (result == null)
            throw new VkIDApiException("Exchange code response could not be parsed.");

        return result;
    }

    /// <summary>
    /// Exchange authentication code to access tokens
    /// </summary>
    /// <exception cref="HttpRequestException">If status code is not success</exception>
    /// <exception cref="VkIDApiException"></exception>
    public async Task<UserInfoResponse> GetUserInfo(string accessToken, CancellationToken cancellationToken = default)
    {
        var parameters = new Dictionary<string, string>
        {
            { "access_token", accessToken },
            { "client_id", _options.Value.ApplicationId.ToString() }
        };

        using var httpClient = GetHttpClient();
        var response = await httpClient.PostAsync(
            "https://id.vk.com/oauth2/user_info",
            new FormUrlEncodedContent(parameters),
            cancellationToken
        ).ConfigureAwait(false);

        if (!response.IsSuccessStatusCode)
        {
            var errorResponse = await response.Content.ReadFromJsonAsync<ErrorResponse>((JsonSerializerOptions?)null, cancellationToken);
            throw new VkIDApiException(errorResponse?.Error ?? "User info error response could not be parsed.", errorResponse?.ErrorDescription);
        }

        var result = await response.Content.ReadFromJsonAsync<UserResponseWrapper<UserInfoResponse>>((JsonSerializerOptions?)null, cancellationToken);
        if (result == null)
            throw new VkIDApiException("User info response could not be parsed.");

        return result.User;
    }

    /// <summary>
    /// Exchange authentication code to access tokens
    /// </summary>
    /// <param name="idToken">JSON Web Token (JWT) пользователя, который был получен после первичной авторизации вместе с Access и Refresh token</param>
    /// <param name="cancellationToken">Токен отмены запроса</param>
    public async Task<UserInfoResponse> GetPublicInfo(string idToken, CancellationToken cancellationToken = default)
    {
        var parameters = new Dictionary<string, string>
        {
            { "id_token", idToken },
            { "client_id", _options.Value.ApplicationId.ToString() }
        };

        using var httpClient = GetHttpClient();
        var response = await httpClient.PostAsync(
            "https://id.vk.com/oauth2/public_info",
            new FormUrlEncodedContent(parameters),
            cancellationToken
        ).ConfigureAwait(false);

        if (!response.IsSuccessStatusCode)
        {
            var errorResponse = await response.Content.ReadFromJsonAsync<ErrorResponse>((JsonSerializerOptions?)null, cancellationToken);
            throw new VkIDApiException(errorResponse?.Error ?? "Public user info error response could not be parsed.", errorResponse?.ErrorDescription);
        }

        var result = await response.Content.ReadFromJsonAsync<UserResponseWrapper<UserInfoResponse>>((JsonSerializerOptions?)null, cancellationToken);
        if (result == null)
            throw new VkIDApiException("Public user info response could not be parsed.");

        return result.User;
    }


    private HttpClient GetHttpClient() => _httpClientFactory?.CreateClient(ServiceRegistrationExtensions.HTTP_CLIENT_NAME) ?? _httpClient!;


    public void Dispose()
    {
        _httpClient?.Dispose();
    }
}