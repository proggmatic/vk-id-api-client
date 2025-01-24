using System.Net.Http.Json;
using System.Text.Json;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

using Proggmatic.VkIDApiClient.DTO;


namespace Proggmatic.VkIDApiClient;

/// <summary>
/// VK ID API client
/// </summary>
public class VkIDApiClient : IDisposable
{
    private readonly HttpClient _httpClient;
    private readonly IOptionsSnapshot<VkIDApiClientConfig> _options;
    private static readonly JsonSerializerOptions _serializerOptions = new(JsonSerializerDefaults.Web);

    /// <summary>
    /// Constructor for dependency injection
    /// </summary>
    [ActivatorUtilitiesConstructor]
    public VkIDApiClient(HttpClient httpClient, IOptionsSnapshot<VkIDApiClientConfig> options)
    {
        _httpClient = httpClient;
        _options = options;
    }

    /// <summary>
    /// Constructor for creating with inline config
    /// </summary>
    /// <param name="httpClient"></param>
    /// <param name="config"></param>
    public VkIDApiClient(HttpClient httpClient, VkIDApiClientConfig config)
    {
        _httpClient = httpClient;
        _options = new StaticOptionsSnapshot<VkIDApiClientConfig>(config);
    }


    /// <summary>
    /// Exchange authentication code to access tokens
    /// </summary>
    /// <exception cref="HttpRequestException">If status code is not success</exception>
    /// <exception cref="VkIDApiException">In case of error response</exception>
    public async Task<VkExchangeCodeResponse> ExchangeCode(string code, string deviceId, string codeVerifier, string state, CancellationToken cancellationToken = default)
    {
        var parameters = new Dictionary<string, string>
        {
            { "grant_type", "authorization_code" },
            { "code_verifier", codeVerifier },
            { "redirect_uri", _options.Value.RedirectUrl },
            { "code", code },
            { "client_id", _options.Value.ApplicationId.ToString() },
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
            throw new VkIDApiException(errorResponse?.Error ?? "Exchange code error response could not be parsed.", errorResponse?.ErrorDescription, (int)response.StatusCode);
        }

        var stringResponse = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
        if (stringResponse.Contains("\"error\":"))
        {
            var errorResponse = JsonSerializer.Deserialize<VkErrorResponse>(stringResponse, _serializerOptions);
            throw new VkIDApiException(errorResponse?.Error ?? "Exchange code error response could not be parsed.", errorResponse?.ErrorDescription, (int)response.StatusCode);
        }

        var result = JsonSerializer.Deserialize<VkExchangeCodeResponse>(stringResponse, _serializerOptions);
        if (result == null)
            throw new VkIDApiException("Exchange code response could not be parsed.", responseCode: (int)response.StatusCode);

        return result;
    }

    /// <summary>
    /// Exchange authentication code to access tokens
    /// </summary>
    /// <exception cref="HttpRequestException">If status code is not success</exception>
    /// <exception cref="VkIDApiException">In case of error response</exception>
    public async Task<VkUserInfoResponse> GetUserInfo(string accessToken, CancellationToken cancellationToken = default)
    {
        var parameters = new Dictionary<string, string>
        {
            { "access_token", accessToken },
            { "client_id", _options.Value.ApplicationId.ToString() }
        };

        var response = await _httpClient.PostAsync(
            "https://id.vk.com/oauth2/user_info",
            new FormUrlEncodedContent(parameters),
            cancellationToken
        ).ConfigureAwait(false);

        if (!response.IsSuccessStatusCode)
        {
            var errorResponse = await response.Content.ReadFromJsonAsync<VkErrorResponse>((JsonSerializerOptions?)null, cancellationToken).ConfigureAwait(false);
            throw new VkIDApiException(errorResponse?.Error ?? "User info error response could not be parsed.", errorResponse?.ErrorDescription, (int)response.StatusCode);
        }

        var stringResponse = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
        if (stringResponse.Contains("\"error\":"))
        {
            var errorResponse = JsonSerializer.Deserialize<VkErrorResponse>(stringResponse, _serializerOptions);
            throw new VkIDApiException(errorResponse?.Error ?? "User info error response could not be parsed.", errorResponse?.ErrorDescription, (int)response.StatusCode);
        }

        var result = JsonSerializer.Deserialize<VkUserResponseWrapper<VkUserInfoResponse>>(stringResponse, _serializerOptions);
        if (result == null)
            throw new VkIDApiException("User info response could not be parsed.", responseCode: (int)response.StatusCode);

        return result.User;
    }

    /// <summary>
    /// Exchange authentication code to access tokens
    /// </summary>
    /// <param name="idToken">JSON Web Token (JWT) of the user, that was got after primary authorization altogether with Access and Refresh token</param>
    /// <param name="cancellationToken">Token for request cancellation</param>
    /// <exception cref="HttpRequestException">If status code is not success</exception>
    /// <exception cref="VkIDApiException">In case of error response</exception>
    public async Task<VkUserInfoResponse> GetPublicInfo(string idToken, CancellationToken cancellationToken = default)
    {
        var parameters = new Dictionary<string, string>
        {
            { "id_token", idToken },
            { "client_id", _options.Value.ApplicationId.ToString() }
        };

        var response = await _httpClient.PostAsync(
            "https://id.vk.com/oauth2/public_info",
            new FormUrlEncodedContent(parameters),
            cancellationToken
        ).ConfigureAwait(false);

        if (!response.IsSuccessStatusCode)
        {
            var errorResponse = await response.Content.ReadFromJsonAsync<VkErrorResponse>((JsonSerializerOptions?)null, cancellationToken).ConfigureAwait(false);
            throw new VkIDApiException(errorResponse?.Error ?? "Public user info error response could not be parsed.", errorResponse?.ErrorDescription, (int)response.StatusCode);
        }

        var stringResponse = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
        if (stringResponse.Contains("\"error\":"))
        {
            var errorResponse = JsonSerializer.Deserialize<VkErrorResponse>(stringResponse, _serializerOptions);
            throw new VkIDApiException(errorResponse?.Error ?? "Public user info error response could not be parsed.", errorResponse?.ErrorDescription, (int)response.StatusCode);
        }

        var result = JsonSerializer.Deserialize<VkUserResponseWrapper<VkUserInfoResponse>>(stringResponse, _serializerOptions);
        if (result == null)
            throw new VkIDApiException("Public user info response could not be parsed.", responseCode: (int)response.StatusCode);

        return result.User;
    }


    /// <summary>
    /// Releases the unmanaged resources and disposes of the managed resources
    /// </summary>
    public void Dispose()
    {
        _httpClient.Dispose();
    }
}