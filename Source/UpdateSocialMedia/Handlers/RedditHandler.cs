namespace UpdateSocialMedia.Handlers;

using System.Threading.Tasks;
using Reddit;
using UpdateSocialMedia.Models;

internal class RedditHandler : IHandler
{
    public bool CanHandle(Content content) => true;

    public async Task HandleAsync(Content content, CancellationToken cancellationToken)
    {
        var redditClient = new RedditClient("YourRedditAppID", "YourBotUserRefreshToken");

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
