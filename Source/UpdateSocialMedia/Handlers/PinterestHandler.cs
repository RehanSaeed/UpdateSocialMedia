namespace UpdateSocialMedia.Handlers;

using System.Threading.Tasks;
using PinSharp;
using UpdateSocialMedia.Models;
using UpdateSocialMedia.Options;

public class PinterestHandler : IHandler
{
    private readonly PinterestOptions pinterestOptions;

    public PinterestHandler(PinterestOptions pinterestOptions) => this.pinterestOptions = pinterestOptions;

    public bool CanHandle(Content content) => false;

    public async Task HandleAsync(Content content, CancellationToken cancellationToken)
    {
        var client = new PinSharpClient(this.pinterestOptions.AccessToken);
        await client.Pins
            .CreatePinAsync(
                this.pinterestOptions.Board,
                content.ThumbnailUrl?.ToString(),
                content.Title,
                content.Url.ToString())
            .ConfigureAwait(false);
    }
}
