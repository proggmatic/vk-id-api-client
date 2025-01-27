﻿using System.Net;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Polly;


namespace Proggmatic.VkIDApiClient;

/// <summary>
/// Service registration extensions
/// </summary>
public static class ServiceRegistrationExtensions
{
    internal const string HTTP_CLIENT_NAME = "VkIDHttpClient";

    /// <summary>
    /// Add VK ID API client as transient service with inline configuration
    /// </summary>
    public static IServiceCollection AddVkIDApiClient(this IServiceCollection services, VkIDApiClientConfig config)
    {
        services.Configure<VkIDApiClientConfig>(configure => { configure.ApplicationId = config.ApplicationId; });

        services.AddHttpClient<VkIDApiClient>(HTTP_CLIENT_NAME, (_, client) =>
            {
                client.Timeout = TimeSpan.FromSeconds(30);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new("application/json"));
                //client.DefaultRequestHeaders.Add("Content-Type", "application/json");
            })
            .ConfigurePrimaryHttpMessageHandler(() =>
                new HttpClientHandler { AutomaticDecompression = DecompressionMethods.All, AllowAutoRedirect = false })
            .AddTransientHttpErrorPolicy(p =>
                p.WaitAndRetryAsync(3, _ => TimeSpan.FromMilliseconds(600))
            );

        services.AddScoped<VkIDApiClient>();

        return services;
    }

    /// <summary>
    /// Add VK ID API client as transient service with configuration from <see cref="IConfiguration"/> object
    /// </summary>
    public static IServiceCollection AddVkIDApiClient(this IServiceCollection services, IConfiguration? configuration = null, string configurationSection = "vkidApi")
    {
        if (configuration != null && !string.IsNullOrEmpty(configurationSection))
            services.Configure<VkIDApiClientConfig>(configuration.GetSection(configurationSection));

        services.AddHttpClient<VkIDApiClient>(HTTP_CLIENT_NAME, (_, client) =>
            {
                client.Timeout = TimeSpan.FromSeconds(30);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new("application/json"));
                //client.DefaultRequestHeaders.Add("Content-Type", "application/json");
            })
            .ConfigurePrimaryHttpMessageHandler(() =>
                new HttpClientHandler { AutomaticDecompression = DecompressionMethods.All, AllowAutoRedirect = false })
            .AddTransientHttpErrorPolicy(p =>
                p.WaitAndRetryAsync(3, _ => TimeSpan.FromMilliseconds(600))
            );

        services.AddTransient<VkIDApiClient>();

        return services;
    }
}