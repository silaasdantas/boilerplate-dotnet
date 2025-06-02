namespace WorkerService.Features.OfferBids.UpsertOfferBid.Configuration;

public static class Configuration
{
    public static IServiceCollection ConfigureUpsertOfferBid(this IServiceCollection services)
    {
        services.AddScoped<IUpsertOfferBidUseCase, UpsertOfferBidUseCase>();
        services.AddScoped<IUpsertOfferBidRepository, UpsertOfferBidRepository>();

        return services;
    }
}
