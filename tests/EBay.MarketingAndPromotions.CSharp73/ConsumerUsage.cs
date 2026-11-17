using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EBay.MarketingAndPromotions;
using EBay.MarketingAndPromotions.Configuration;
using EBay.MarketingAndPromotions.Exceptions;

namespace EBay.MarketingAndPromotions.CSharp73
{
    /// <summary>Typical consumer code written in C# 7.3; it only has to compile.</summary>
    public static class ConsumerUsage
    {
        public static async Task<string> RunAsync()
        {
            var config = new MarketingAndPromotionsConfiguration
            {
                AccessToken = "token",
                HttpTimeout = TimeSpan.FromSeconds(60),
                DiagnosticLogging = true,
                MarketingBaseUrl = "https://ebay-proxy.example.com/sell/marketing/v1",
                AllowedHosts = { "ebay-proxy.example.com" },
                DefaultRequestOptions = new RequestOptions
                {
                    Headers = new Dictionary<string, string> { { "X-EBAY-C-MARKETPLACE-ID", "EBAY_US" } }
                }
            };

            using (var client = new MarketingAndPromotionsClient(config))
            {
                try
                {
                    var result = await client.Marketing.GetCampaignsAsync();
                    return result != null ? "ok" : "empty";
                }
                catch (UnauthorizedException)
                {
                    return "unauthorized";
                }
            }
        }
    }
}
