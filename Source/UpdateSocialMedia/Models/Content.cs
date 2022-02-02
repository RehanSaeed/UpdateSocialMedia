namespace UpdateSocialMedia.Models;

public class Content
{
    public IEnumerable<string>? Subreddits { get; set; }

    public string? Title { get; set; }

    public Uri? ThumbnailUrl { get; set; }

    public Uri Url { get; set; } = default!;

    public static Content GetContent(Uri url, string subreddits)
    {
        Content content;
        if (url.Host.Contains("youtube"))
        {
            content = new YouTubeVideo()
            {
                Url = url,
            };
        }
        else
        {
            content = new BlogPost()
            {
                Url = url,
            };
        }

        if (subreddits is not null)
        {
            content.Subreddits = subreddits.Split(",", StringSplitOptions.RemoveEmptyEntries);
        }

        return content;
    }
}
