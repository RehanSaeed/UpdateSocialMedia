namespace UpdateSocialMedia.Enrichers;

using System.Linq;
using System.Threading.Tasks;
using UpdateSocialMedia.Models;
using YoutubeExplode;

public class YouTubeVideoEnricher : IEnricher
{
    public bool CanEnrich(Content content) => content is YouTubeVideo;

    public async Task EnrichAsync(Content content, CancellationToken cancellationToken)
    {
        var youtube = new YoutubeClient();

        var video = await youtube.Videos.GetAsync(content.Url.ToString(), cancellationToken).ConfigureAwait(false);

        content.Title = video.Title;
        content.ThumbnailUrl = new Uri(video.Thumbnails.OrderByDescending(x => x.Resolution.Area).First().Url);
    }
}
