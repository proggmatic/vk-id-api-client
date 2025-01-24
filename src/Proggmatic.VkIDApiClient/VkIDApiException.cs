namespace Proggmatic.VkIDApiClient;

/// <summary>
/// Base exception for VK ID error responses
/// </summary>
public class VkIDApiException : ApplicationException
{
    /// <summary>
    /// Error title
    /// </summary>
    public string Error { get; }

    /// <summary>
    /// Error description
    /// </summary>
    public string? ErrorDescription { get; }

    /// <summary>
    /// HTTP response code
    /// </summary>
    public int ResponseCode { get; }


    /// <summary>
    /// Primary constructor
    /// </summary>
    public VkIDApiException(string error, string? errorDescription = null, int responseCode = 200, Exception? innerException = null) : base(error, innerException)
    {
        this.Error = error;
        this.ErrorDescription = errorDescription;
        this.ResponseCode = responseCode;
    }
}