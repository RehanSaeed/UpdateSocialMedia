namespace UpdateSocialMedia;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using UpdateSocialMedia.Enrichers;
using UpdateSocialMedia.Handlers;
using UpdateSocialMedia.Options;

public static class HostBuilderExtensions
{
    public static void ConfigureHost(this IHostBuilder hostBuilder) =>
        hostBuilder
            .UseContentRoot(Directory.GetCurrentDirectory())
            .ConfigureAppConfiguration(
                (hostingContext, configurationBuilder) =>
                {
                    hostingContext.HostingEnvironment.ApplicationName = AssemblyInformation.Current.Product;
                    configurationBuilder.AddCustomConfiguration(hostingContext.HostingEnvironment);
                })
            .ConfigureLogging(loggingBuilder => loggingBuilder.AddDebug().AddConsole())
            .ConfigureServices(ConfigureServices)
            .UseDefaultServiceProvider(
                (context, options) =>
                {
                    var isDevelopment = context.HostingEnvironment.IsDevelopment();
                    options.ValidateScopes = isDevelopment;
                    options.ValidateOnBuild = isDevelopment;
                });

    private static void ConfigureServices(HostBuilderContext context, IServiceCollection services) =>
        services
            .ConfigureAndValidateSingleton<ApplicationOptions>(context.Configuration)
            .ConfigureAndValidateSingleton<PinterestOptions>(context.Configuration.GetRequiredSection(nameof(ApplicationOptions.Reddit)))
            .ConfigureAndValidateSingleton<RedditOptions>(context.Configuration.GetRequiredSection(nameof(ApplicationOptions.Reddit)))
            .AddSingleton<IEnricher, BlogPostEnricher>()
            .AddSingleton<IEnricher, YouTubeVideoEnricher>()
            .AddSingleton<IHandler, PinterestHandler>()
            .AddSingleton<IHandler, RedditHandler>();
}
