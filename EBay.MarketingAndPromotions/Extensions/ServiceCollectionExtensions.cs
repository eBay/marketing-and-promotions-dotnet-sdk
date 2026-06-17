// Copyright 2026 eBay Inc.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.


using EBay.MarketingAndPromotions.Clients.Rest;
using EBay.MarketingAndPromotions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EBay.MarketingAndPromotions.Extensions;

public static class ServiceCollectionExtensions
{
    private const string MarketingClientName = nameof(MarketingAndPromotionsClient) + ".Marketing";
    private const string RecommendationClientName = nameof(MarketingAndPromotionsClient) + ".Recommendation";

    public static IServiceCollection AddMarketingAndPromotionsSdk(
        this IServiceCollection services,
        MarketingAndPromotionsConfiguration config)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(config);

        services.AddSingleton(config);
        services.AddSingleton<ITokenContext, AsyncLocalTokenContext>();

        services.AddHttpClient(MarketingClientName)
            .AddHttpMessageHandler(sp => new DiagnosticLoggingHandler(sp.GetRequiredService<MarketingAndPromotionsConfiguration>()))
            .ConfigureHttpClient(c => c.Timeout = config.HttpTimeout);

        services.AddHttpClient(RecommendationClientName)
            .AddHttpMessageHandler(sp => new DiagnosticLoggingHandler(sp.GetRequiredService<MarketingAndPromotionsConfiguration>()))
            .ConfigureHttpClient(c => c.Timeout = config.HttpTimeout);

        services.AddScoped<MarketingAndPromotionsClient>(sp =>
            new MarketingAndPromotionsClient(
                sp.GetRequiredService<MarketingAndPromotionsConfiguration>(),
                sp.GetRequiredService<IHttpClientFactory>().CreateClient(MarketingClientName),
                sp.GetRequiredService<IHttpClientFactory>().CreateClient(RecommendationClientName),
                sp.GetRequiredService<ITokenContext>()));
        services.AddScoped(sp => sp.GetRequiredService<MarketingAndPromotionsClient>().Marketing);
        services.AddScoped(sp => sp.GetRequiredService<MarketingAndPromotionsClient>().Recommendation);

        return services;
    }

    public static IServiceCollection AddMarketingAndPromotionsSdk(
        this IServiceCollection services,
        Action<MarketingAndPromotionsConfiguration> configBuilder)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configBuilder);

        var config = new MarketingAndPromotionsConfiguration();
        configBuilder(config);

        return services.AddMarketingAndPromotionsSdk(config);
    }
}
