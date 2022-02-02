namespace UpdateSocialMedia.Handlers;

using System.Threading.Tasks;
using UpdateSocialMedia.Models;

public interface IHandler
{
    bool CanHandle(Content content);

    Task HandleAsync(Content content, CancellationToken cancellationToken);
}
