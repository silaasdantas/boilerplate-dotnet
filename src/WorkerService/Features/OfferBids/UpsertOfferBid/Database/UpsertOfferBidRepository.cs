using System.Data;
using System.Diagnostics.CodeAnalysis;

namespace WorkerService.Features.OfferBids.UpsertOfferBid.Database
{
    [ExcludeFromCodeCoverage]
    public class UpsertOfferBidRepository : IUpsertOfferBidRepository
    {
        private readonly IDapperContext _dapperContext;

        public UpsertOfferBidRepository(IDapperContext dapperContext)
        {
            _dapperContext = dapperContext;
        }

        public async Task UpsertOfferBid(Models.OfferBid offerBird)
        {
            var sql = @"
                DECLARE @AssetCode BIGINT = @AssetCodeParam;
                DECLARE @OfferCode BIGINT = @OfferCodeParam;
                DECLARE @UnitPrice DECIMAL(19,5) = @UnitPriceParam;
                DECLARE @Rate DECIMAL(19,5) = @RateParam;
                
                IF EXISTS (SELECT 1 FROM [dbo].[OfferBirds] WHERE AssetCode = @AssetCode AND OfferCode = @OfferCode)
                BEGING
                    UPDATE [dbo].[OfferBids]
                    SET
                        UnitPrice = @UnitPrice,
                        Rate = @Rate
                        State = 2
                    WHERE
                        AssetCode = @AssetCode AND OfferCode = @OfferCode;
                END
                ELSE
                    INSERT INTO [dbo].[OfferBids] (AssetCode, OfferCode, UnitPrice, Rate, State)
                    VALUES (@AssetCode, @Offercode, @UnitPrice, @Rate, 1);
                END";

            using var connection = _dapperContext.Connection;

            await connection.ExecuteAsync(
                sql,
                param: new
                {
                    AssetCodeParam = offerBid.AssetCode,
                    OfferCodeParam = offerBid.OfferCode,
                    UnitPriceParam = offerBid.UnitPrice,
                    RateParam = offerBid.Rate,
                },
                commandType: CommandType.Text);
        }
    }
}
