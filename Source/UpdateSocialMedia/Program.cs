namespace UpdateSocialMedia;

using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Parsing;
using UpdateSocialMedia.Enrichers;
using UpdateSocialMedia.Models;
using UpdateSocialMedia.Handlers;

// --url=https://www.youtube.com/watch?v=Ir1ZCMoqfWc --subreddit=dotnet,kubernetes
// --url=https://rehansaeed.com/the-problem-with-csharp-10-implicit-usings/ --subreddit=dotnet,kubernetes
public static class Program
{
    private static readonly List<IEnricher> Enrichers = new()
    {
        new BlogPostEnricher(),
        new YouTubeVideoEnricher(),
    };

    private static readonly List<IHandler> Handlers = new()
    {
        new PinterestHandler(),
        new RedditHandler(),
    };

    public static async Task<int> Main(string[] arguments)
    {
        var urlOption = new Option<Uri>(
            new string[] { "-u", "--url" },
            "The URL of the blog post or YouTube video.");
        var subredditOption = new Option<string>(
            new string[] { "-s", "--subreddit" },
            "The name of a Reddit subreddit.");

        var rootCommand = new RootCommand("Create posts for a blog post or YouTube video.")
        {
            urlOption,
            subredditOption,
        };
        rootCommand.SetHandler(
            async (Uri url, string subreddits, CancellationToken cancellationToken) =>
            {
                if (url is null)
                {
                    return;
                }

                var content = Content.GetContent(url, subreddits);

                foreach (var enricher in Enrichers)
                {
                    if (enricher.CanEnrich(content))
                    {
                        await enricher.EnrichAsync(content, cancellationToken).ConfigureAwait(false);
                    }
                }

                foreach (var handlers in Handlers)
                {
                    if (handlers.CanHandle(content))
                    {
                        await handlers.HandleAsync(content, cancellationToken).ConfigureAwait(false);
                    }
                }
            },
            urlOption,
            subredditOption);

        var commandLineBuilder = new CommandLineBuilder(rootCommand)
            .UseDefaults();
        var parser = commandLineBuilder.Build();
        return await parser.InvokeAsync(arguments).ConfigureAwait(false);
    }
}
