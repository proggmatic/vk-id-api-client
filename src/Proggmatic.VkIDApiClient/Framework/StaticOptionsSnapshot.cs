using Microsoft.Extensions.Options;


namespace Proggmatic.VkIDApiClient;

internal class StaticOptionsSnapshot<TOptions> : IOptionsSnapshot<TOptions> where TOptions : class
{
    public TOptions Value { get; }

    public TOptions Get(string? name) => Value;


    internal StaticOptionsSnapshot(TOptions value)
    {
        Value = value;
    }

    internal static StaticOptionsSnapshot<TOptions> Create(TOptions options) => new(options);
}

internal class StaticOptions<TOptions> : IOptions<TOptions> where TOptions : class
{
    public TOptions Value { get; }

    public TOptions Get(string? name) => Value;


    internal StaticOptions(TOptions value)
    {
        Value = value;
    }

    internal static StaticOptions<TOptions> Create(TOptions options) => new(options);
}