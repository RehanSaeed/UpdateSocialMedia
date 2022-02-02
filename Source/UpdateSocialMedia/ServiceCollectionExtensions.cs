namespace UpdateSocialMedia;

using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection ConfigureAndValidateSingleton<TOptions>(
        this IServiceCollection services,
        IConfiguration configuration)
        where TOptions : class, new()
    {
        services
            .AddOptions<TOptions>()
            .Bind(configuration)
            .ValidateDataAnnotations()
            .ValidateOnStart();
        return services.AddSingleton((IServiceProvider x) => x.GetRequiredService<IOptions<TOptions>>().Value);
    }
}
