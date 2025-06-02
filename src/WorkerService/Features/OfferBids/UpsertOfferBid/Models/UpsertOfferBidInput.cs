namespace WorkerService.Features.OfferBids.UpsertOfferBid.Models
{
    public record UpsertOfferBidInput (
        long OfferCode,
        bool ApplicationEnable,
        bool RedemptionEnable,
        int? BrandCode,
        decimal ApplicationMinValue,
        decimal ApplicationMaxValue,
        int? SpreadCode,
        int? OriginCode,
        long? AssetCode,
        string OfferName,
        DateTime? CreateDate,
        DateTime? AssetMaturityDate,
        IEnumerable<OfferEligibility> Eligibilities,
        IEnumerable<OfferUnitPrice> UnitPrices);
}
