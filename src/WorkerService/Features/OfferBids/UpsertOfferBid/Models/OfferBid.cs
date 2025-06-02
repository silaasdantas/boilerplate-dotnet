namespace WorkerService.Features.OfferBids.UpsertOfferBid.Models
{
    public record OfferBid(long AssetCode, long OfferCode, decimal UnitPrice, decimal Rate);
}

