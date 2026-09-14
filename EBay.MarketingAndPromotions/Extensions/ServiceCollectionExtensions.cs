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

using EBay.MarketingAndPromotions;
using EBay.MarketingAndPromotions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EBay.MarketingAndPromotions.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMarketingAndPromotionsSdk(
        this IServiceCollection services,
        MarketingAndPromotionsConfiguration config)
    {
        services.AddSingleton(config);
        services.AddSingleton<ITokenContext, AsyncLocalTokenContext>();

        foreach (var endpoint in MarketingAndPromotionsClient.RestEndpointNames)
        {
            var id = endpoint.ApiId;
            services.AddHttpClient(endpoint.HttpClientName, (sp, c) =>
                    c.BaseAddress = new Uri(sp.GetRequiredService<MarketingAndPromotionsConfiguration>().RestBaseUrl(id)))
                .ConfigureHttpClient((sp, c) =>
                    c.Timeout = sp.GetRequiredService<MarketingAndPromotionsConfiguration>().HttpTimeout)
                .AddHttpMessageHandler(sp =>
                    new DiagnosticLoggingHandler(sp.GetRequiredService<MarketingAndPromotionsConfiguration>()));
        }

        services.AddScoped<MarketingAndPromotionsClient>(sp =>
        {
            var factory = sp.GetRequiredService<IHttpClientFactory>();
            return new MarketingAndPromotionsClient(
                sp.GetRequiredService<MarketingAndPromotionsConfiguration>(),
                factory,
                sp.GetRequiredService<ITokenContext>());
        });

        services.AddScoped(sp => sp.GetRequiredService<MarketingAndPromotionsClient>().Marketing);
        services.AddScoped(sp => sp.GetRequiredService<MarketingAndPromotionsClient>().Recommendation);
        return services;
    }
}
