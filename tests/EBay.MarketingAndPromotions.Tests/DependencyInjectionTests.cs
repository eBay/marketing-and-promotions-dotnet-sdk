using EBay.MarketingAndPromotions.Clients.Rest;
using EBay.MarketingAndPromotions.Configuration;
using EBay.MarketingAndPromotions.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace EBay.MarketingAndPromotions.Tests;

public sealed class DependencyInjectionTests
{
    [Fact]
    public void Registers_one_shared_entry_client_and_no_individual_api_clients()
    {
        var services = new ServiceCollection();
        services.AddMarketingAndPromotionsSdk(new MarketingAndPromotionsConfiguration { AccessToken = "t" });

        using var provider = services.BuildServiceProvider();
        var client = provider.GetRequiredService<MarketingAndPromotionsClient>();

        Assert.Same(client, provider.CreateScope().ServiceProvider.GetRequiredService<MarketingAndPromotionsClient>());
        Assert.NotNull(client.Marketing);
        Assert.Null(provider.GetService<MarketingClient>());
    }

    [Fact]
    public void Rejects_a_null_primary_handler()
    {
        Assert.Throws<ArgumentNullException>(() =>
            new MarketingAndPromotionsClient(new MarketingAndPromotionsConfiguration(), (HttpMessageHandler)null!));
    }
}
