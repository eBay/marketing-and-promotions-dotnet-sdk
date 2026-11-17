using EBay.MarketingAndPromotions.Configuration;
using Xunit;

namespace EBay.MarketingAndPromotions.Tests;

public sealed class TrustedHostsTests
{
    private const string Sandbox = "https://api.sandbox.ebay.com/sell/marketing/v1";
    private const string Proxy = "https://ebay-proxy.example.com/sell/marketing/v1";

    [Fact]
    public void Untrusted_base_url_is_rejected_by_both_constructors()
    {
        var config = new MarketingAndPromotionsConfiguration { AccessToken = "t", MarketingBaseUrl = Proxy };

        var standalone = Assert.Throws<InvalidOperationException>(() => new MarketingAndPromotionsClient(config));
        Assert.Contains("AllowedHosts", standalone.Message);
        Assert.Throws<InvalidOperationException>(() => new TestSdk(configure: c => c.MarketingBaseUrl = Proxy));
    }

    [Fact]
    public async Task Production_requests_carry_the_token()
    {
        var sdk = new TestSdk();

        await sdk.Client.Marketing.GetCampaignsAsync();

        Assert.StartsWith("https://api.ebay.com/sell/marketing/v1/", sdk.Handler.RequestUri!.AbsoluteUri);
        Assert.Equal("Bearer config-token", sdk.Handler.Authorization);
    }

    [Fact]
    public async Task Sandbox_hosts_are_trusted_by_default()
    {
        var sdk = new TestSdk(configure: c => c.MarketingBaseUrl = Sandbox);

        await sdk.Client.Marketing.GetCampaignsAsync();

        Assert.Equal("api.sandbox.ebay.com", sdk.Handler.RequestUri!.Host);
        Assert.Equal("Bearer config-token", sdk.Handler.Authorization);
    }

    [Fact]
    public async Task Additional_allowed_host_receives_requests_with_token()
    {
        var sdk = new TestSdk(configure: c =>
        {
            c.MarketingBaseUrl = Proxy;
            c.AllowedHosts.Add("ebay-proxy.example.com");
        });

        await sdk.Client.Marketing.GetCampaignsAsync();

        Assert.Equal("ebay-proxy.example.com", sdk.Handler.RequestUri!.Host);
        Assert.Equal("Bearer config-token", sdk.Handler.Authorization);
    }

    [Fact]
    public void Default_hosts_are_read_only()
    {
        Assert.Contains("api.ebay.com", MarketingAndPromotionsConfiguration.DefaultAllowedHosts);
        Assert.Throws<NotSupportedException>(() =>
            ((IList<string>)MarketingAndPromotionsConfiguration.DefaultAllowedHosts).Add("evil.example.com"));
    }
}
