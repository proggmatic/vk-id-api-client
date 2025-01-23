# VK ID api client

[![](https://buildstats.info/nuget/Proggmatic.VkIDApiClient)](https://www.nuget.org/packages/Proggmatic.VkIDApiClient/)

Provides API client for [VK ID](https://id.vk.com/about/business/go/docs/ru/vkid) in .NET.

Only .NET 8.0 and higher will be supported.

# Usage

## .NET application or ASP.NET application

Install the `Proggmatic.VkIDApiClient` NuGet package on the
.NET Core project, then modify the `Program.cs` file similar to the following.

```cs
using Proggmatic.VkIDApiClient;                  //-- new addition --//

// For .NET app
var builder = Host.CreateApplicationBuilder(args);

// Or for ASP.NET app
var builder = WebApplication.CreateBuilder(args);

// ... other .NET configuration skipped

//-- Configure instantly --//
builder.Services.AddVkIDApiClient(new VkIDApiClientConfig { ApplicationId = 123456, RedirectUrl = "https://your-valid-redirect-url" });

//-- OR configure from config file --//
builder.Configuration
    // other addings of config files 
    .AddJsonFile("config/vkApi.json", optional: true, reloadOnChange: true);

builder.Services.Configure<VkIDApiClientConfig>(builder.Configuration.GetSection("vkApi"));

builder.Services.AddVkIDApiClient(builder.Configuration);
```

Config file `vkApi.json` example. You can change values while application is running, changes will be applied immediately.
```json5
{
  "vkApi": {
      /* ID of VK application. Set from here https://id.vk.com/about/business/go/accounts/{my user id}/apps */
      "applicationId": 12345678,

      /* Valid redirect URL. You can set it up on https://id.vk.com/about/business/go/accounts/{your user ID}/apps/{your app ID}/edit */
      "redirectUrl": "https://your-valid-redirect-url"
  }
}
```

Then you can use `VkIDApiClient` as service from dependency injection.
```cs
public async Task SomeMethodOfController(VkExchangeCodeRequest request, [FromServices] VkIDApiClient vkIdApiClient)
{
    ...
}
```