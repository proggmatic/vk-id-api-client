namespace Proggmatic.VkIDApiClient;

public class VkIDApiException : ApplicationException
{
    public string Error { get; }

    public string? ErrorDescription { get; }

    public int ResponseCode { get; set; } = 200;


    public VkIDApiException(string error, string? errorDescription = null, int responseCode = 200, Exception? innerException = null) : base(error, innerException)
    {
        this.Error = error;
        this.ErrorDescription = errorDescription;
        this.ResponseCode = responseCode;
    }

    // public VkIDApiException(string? message) : base(message)
    // {
    // }
    //
    // public VkIDApiException(string? message, Exception? innerException) : base(message, innerException)
    // {
    // }
}