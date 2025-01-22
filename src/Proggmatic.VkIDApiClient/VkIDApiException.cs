namespace Proggmatic.VkIDApiClient;

public class VkIDApiException : ApplicationException
{
    public string Error { get; }

    public string? ErrorDescription { get; }


    public VkIDApiException(string error, string? errorDescription = null, Exception? innerException = null) : base(error, innerException)
    {
        this.Error = error;
        this.ErrorDescription = errorDescription;
    }

    // public VkIDApiException(string? message) : base(message)
    // {
    // }
    //
    // public VkIDApiException(string? message, Exception? innerException) : base(message, innerException)
    // {
    // }
}