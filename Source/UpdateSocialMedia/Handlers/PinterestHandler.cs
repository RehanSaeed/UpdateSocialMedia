namespace UpdateSocialMedia.Handlers;

using System.Threading.Tasks;
using PinSharp;
using UpdateSocialMedia.Models;

public class PinterestHandler : IHandler
{
    private const string Board = "Test";

    public bool CanHandle(Content content) => false;

    public async Task HandleAsync(Content content, CancellationToken cancellationToken)
    {
        var client = new PinSharpClient("fsdf");
        await client.Pins
            .CreatePinAsync(
                Board,
                content.ThumbnailUrl?.ToString(),
                content.Title,
                content.Url.ToString())
            .ConfigureAwait(false);
    }
}
