namespace Proggmatic.VkIDApiClient;

public class VkIDApiClientConfig
{
    public int ApplicationId { get; set; }                // set from here https://id.vk.com/about/business/go/accounts/{your ID}/apps
    // public string SignInServiceId { get; set; } = null!;  // set from here https://developer.apple.com/account/resources/identifiers/list/serviceId
    // public string ServiceKeyId { get; set; } = null!;     // set from here https://developer.apple.com/account/resources/authkeys/list "2FCS3ZTX7H",
    // public string PrivateKey { get; set; } = null!;       // https://developer.apple.com/account/resources/authkeys/list "-----BEGIN PRIVATE KEY-----\nMIGTAgEAMBMGByqGSM49Ag...5KbraaQXQFFjuxqUljYUx/6p\n-----END PRIVATE KEY-----"
}