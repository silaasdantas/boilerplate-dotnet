namespace WorkerService.Features.OfferBids.UpsertOfferBid.Database
{
    public interface IUpsertOfferBidRepository
    {
        Task UpsertOfferBid(Models.OfferBid offerBid);
    }
}
