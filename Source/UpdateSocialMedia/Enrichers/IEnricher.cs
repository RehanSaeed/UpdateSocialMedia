namespace UpdateSocialMedia.Enrichers;

using System.Threading.Tasks;
using UpdateSocialMedia.Models;

public interface IEnricher
{
    bool CanEnrich(Content content);

    Task EnrichAsync(Content content, CancellationToken cancellationToken);
}
