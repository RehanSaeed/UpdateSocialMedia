namespace UpdateSocialMedia.Enrichers;

using System.Linq;
using System.Threading.Tasks;
using HtmlAgilityPack;
using UpdateSocialMedia.Models;

public class BlogPostEnricher : IEnricher
{
    public bool CanEnrich(Content content) => content is BlogPost;

    public async Task EnrichAsync(Content content, CancellationToken cancellationToken)
    {
        var web = new HtmlWeb();
        var document = await web
            .LoadFromWebAsync(content.Url.ToString(), cancellationToken)
            .ConfigureAwait(false);
        var metaTags = document.DocumentNode.SelectNodes("//meta");

        content.Title = GetMetaTag(metaTags, "og:title");
        content.ThumbnailUrl = GetMetaTagUri(metaTags, "og:image");
    }

    private static string? GetMetaTag(HtmlNodeCollection metaTags, string propertyName) =>
        metaTags
            .FirstOrDefault(x => string.Equals(x.Attributes["property"]?.Value, propertyName, StringComparison.Ordinal))
            ?.Attributes["content"]
            ?.Value;

    private static Uri? GetMetaTagUri(HtmlNodeCollection metaTags, string propertyName)
    {
        var value = GetMetaTag(metaTags, propertyName);
        if (value == null)
        {
            return null;
        }

        if (Uri.TryCreate(value, UriKind.RelativeOrAbsolute, out var uri))
        {
            return uri;
        }

        return null;
    }
}
