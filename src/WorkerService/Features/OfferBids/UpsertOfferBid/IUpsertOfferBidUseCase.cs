namespace WorkerService.Features.OfferBids.UpsertOfferBid
{
    public interface IUpsertOfferBidUseCase
    {
        Task HandleAsync(UpsertOfferBidInput input, CancellationToken ct);
    }
}
