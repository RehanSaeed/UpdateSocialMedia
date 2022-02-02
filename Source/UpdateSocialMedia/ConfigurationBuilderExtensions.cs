namespace UpdateSocialMedia;

using System;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

public static class ConfigurationBuilderExtensions
{
    public static IConfigurationBuilder AddCustomConfiguration(
        this IConfigurationBuilder configurationBuilder,
        IHostEnvironment hostEnvironment) =>
        configurationBuilder
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: false)
            .AddJsonFile($"appsettings.{hostEnvironment.EnvironmentName}.json", optional: true, reloadOnChange: false)
            .AddIf(
                hostEnvironment.IsDevelopment() && !string.IsNullOrEmpty(hostEnvironment.ApplicationName),
                x => x.AddUserSecrets(Assembly.GetExecutingAssembly(), optional: true, reloadOnChange: false))
            .AddEnvironmentVariables();

    /// <summary>
    /// Executes the specified action if the specified <paramref name="condition"/> is <c>true</c> which can be
    /// used to conditionally add to the configuration pipeline.
    /// </summary>
    /// <param name="configurationBuilder">The configuration builder.</param>
    /// <param name="condition">If set to <c>true</c> the action is executed.</param>
    /// <param name="action">The action used to add to the request execution pipeline.</param>
    /// <returns>The same configuration builder.</returns>
    public static IConfigurationBuilder AddIf(
        this IConfigurationBuilder configurationBuilder,
        bool condition,
        Func<IConfigurationBuilder, IConfigurationBuilder> action)
    {
        ArgumentNullException.ThrowIfNull(configurationBuilder);
        ArgumentNullException.ThrowIfNull(action);

        if (condition)
        {
            configurationBuilder = action(configurationBuilder);
        }

        return configurationBuilder;
    }
}
