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


using EBay.MarketingAndPromotions.Configuration;
using Microsoft.Kiota.Abstractions.Authentication;

namespace EBay.MarketingAndPromotions;

public partial class MarketingAndPromotionsClient : IDisposable
{
    // Named client keys used by ServiceCollectionExtensions
    internal const string MarketingClientName = "MarketingAndPromotionsClient.Marketing";
    internal const string RecommendationClientName = "MarketingAndPromotionsClient.Recommendation";

    private readonly HttpClient[] _ownedClients;

    /// <summary>
    /// DI-friendly constructor: each HttpClient is a named client from IHttpClientFactory
    /// with DiagnosticLoggingHandler and BaseAddress already configured.
    /// </summary>
    internal MarketingAndPromotionsClient(
        MarketingAndPromotionsConfiguration config,
        HttpClient marketingClient,
        HttpClient recommendationClient,
        ITokenContext tokenContext)
    {
        _ownedClients = [];

        var auth = new BaseBearerTokenAuthenticationProvider(new KiotaAuthProvider(config, tokenContext));

        InitializeRestClients(auth, marketingClient, recommendationClient, config, tokenContext);
    }

    /// <summary>
    /// Standalone constructor for use outside DI (tests, scripts).
    /// Creates its own HttpClients with DiagnosticLoggingHandler.
    /// </summary>
    public MarketingAndPromotionsClient(MarketingAndPromotionsConfiguration config)
    {
        var tokenContext = new AsyncLocalTokenContext();
        var auth = new BaseBearerTokenAuthenticationProvider(new KiotaAuthProvider(config, tokenContext));

        var marketingClient = BuildStandaloneHttpClient(config, config.MarketingBaseUrl);
        var recommendationClient = BuildStandaloneHttpClient(config, config.RecommendationBaseUrl);
        _ownedClients = [marketingClient, recommendationClient];

        InitializeRestClients(auth, marketingClient, recommendationClient, config, tokenContext);
    }

    private static HttpClient BuildStandaloneHttpClient(MarketingAndPromotionsConfiguration config, string baseUrl)
    {
        var handler = new DiagnosticLoggingHandler(config)
        {
            InnerHandler = new SocketsHttpHandler { PooledConnectionLifetime = TimeSpan.FromMinutes(2) }
        };
        return new HttpClient(handler) { BaseAddress = new Uri(baseUrl), Timeout = config.HttpTimeout };
    }

    public void Dispose()
    {
        foreach (var client in _ownedClients)
            client.Dispose();
    }
}
