namespace UpdateSocialMedia;

using Microsoft.Extensions.Logging;

#pragma warning disable IDE0060 // Remove unused parameter
internal static partial class LoggerExtensions
{
    [LoggerMessage(
        EventId = 5000,
        Level = LogLevel.Information,
        Message = "Enriched blog post.\r\nTitle: {Title}\r\nThumbnailUrl: {ThumbnailUrl}")]
    public static partial void EnrichedBlogPost(
        this ILogger logger,
        string? title,
        Uri? thumbnailUrl);

    [LoggerMessage(
        EventId = 5001,
        Level = LogLevel.Information,
        Message = "Enriched YouTube video.\r\nTitle: {Title}\r\nThumbnailUrl: {ThumbnailUrl}")]
    public static partial void EnrichedYouTubeVideo(
        this ILogger logger,
        string? title,
        Uri? thumbnailUrl);
}
#pragma warning restore IDE0060 // Remove unused parameter
