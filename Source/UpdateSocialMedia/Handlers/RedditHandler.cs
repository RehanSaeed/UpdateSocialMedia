namespace UpdateSocialMedia.Handlers;

using System.Threading.Tasks;
using Reddit;
using UpdateSocialMedia.Models;
using UpdateSocialMedia.Options;

internal class RedditHandler : IHandler
{
    private readonly RedditOptions redditOptions;

    public RedditHandler(RedditOptions redditOptions) => this.redditOptions = redditOptions;

    public bool CanHandle(Content content) => true;

    public async Task HandleAsync(Content content, CancellationToken cancellationToken)
    {
        var redditClient = new RedditClient(this.redditOptions.ApplicationId, this.redditOptions.Token);

        await redditClient
            .Subreddit()
            .LinkPost(content.Title, content.Url.ToString())
            .SubmitAsync()
            .ConfigureAwait(false);

        if (content.Subreddits is not null)
        {
            foreach (var subredditName in content.Subreddits)
            {
                var subreddit = redditClient.Subreddit(subredditName);
                await subreddit
                    .LinkPost(content.Title, content.Url.ToString())
                    .SubmitAsync()
                    .ConfigureAwait(false);
            }
        }
    }
}
