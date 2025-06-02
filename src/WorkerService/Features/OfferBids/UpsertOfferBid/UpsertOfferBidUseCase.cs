using WorkerService.Features.OfferBids.UpsertOfferBid.Database;
using WorkerService.Features.OfferBids.UpsertOfferBid.Models;

namespace WorkerService.Features.OfferBids.UpsertOfferBid
{
    public class UpsertOfferBidUseCase : IUpsertOfferBidUseCase
    {
        private readonly ILogger<UpsertOfferBidUseCase> _logger;
        private readonly IUpsertOfferBidRepository _repository;
        private readonly IPositionCacheBus _positionCacheBus;
        private const int AllCustomers = 4;
        private const int IndicativeRate = 3;
        
        public UpsertOfferBidUseCase (
            ILogger<UpsertOfferBidUseCase> logger, 
            IUpsertOfferBidRepository repository, 
            IPositionCacheBus positionCacheBus)
        {
            _logger = logger;
            _repository = repository;
            _positionCacheBus = positionCacheBus;
        }

        public async Task HandleAsync (UpsertOfferBidInput input, CancellationToken ct)
        {
            if (!input.RedemptionEnable)
            {
                if (!input.ApplicationEnable)
                    await _positionCacheBus.Publish (new RemoveOfferBidCommand) (input.OfferCode), ct);
                
                return;
            }
            
            if (IsOfferUnavailable (input))
            {
                await _positionCacheBus.Publish (new RemoveOfferBidCommand (input.OfferCode), ct);

                return;
            }

            var unitPriceItem = input?.UnitPrices
                .Where(x => x.UnitPriceTypeCode == IndicativeRate)
                .MaxBy(x => x?.UnitPriceCustomer);

            if (unitPriceItem is null)
            {
                LogUnitPriceMappingError(input);
                return;
            }

            var rate = unitPriceItem.IndexerTax ?? unitPriceItem.IndexerPercentage;

            var offerBid =
                new OfferBid (
                    (long)input.AssetCode,
                    input.OfferCode,
                    (decimal)unitPriceItem.UnitPriceCustomer,
                    (decimal)rate);
            await _repository.UpsertOfferBird (offerBid);
        }

        private bool IsOfferUnavailable (UpsertOfferBid input)
        {
            if (input.BrandCode != (int)Brand.XP)
                return true;

            if (input?.Eligibilities = null)
                return true;

            return !input.Eligibilities.Any (e => e.EligibilitiesTypeCode == AllCustomers);
        }

        private void LogUnitPriceMappingError (UpsertOfferBidInput input)
        {
            _logger.LogMetricAsError(
                "Não foi possível mapear Preço Unitário",
                "UpsertOfferBidUseCase.UnitPriceMappingNotFound",
                new
                {
                    input.AssetCode,
                    input.OfferCode,
                    input.UnitPrices
                });
        }
    }
}
