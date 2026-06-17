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
using EBay.MarketingAndPromotions.Generated.Marketing;
using EBay.MarketingAndPromotions.Generated.Recommendation;
using Microsoft.Kiota.Abstractions.Authentication;
using Microsoft.Kiota.Http.HttpClientLibrary;

namespace EBay.MarketingAndPromotions;

public sealed class MarketingAndPromotionsClient : IDisposable
{
    private readonly HttpClient _marketingHttpClient;
    private readonly HttpClient _recommendationHttpClient;
    private readonly bool _ownsHttpClients;

    public MarketingClient Marketing { get; }
    public RecommendationClient Recommendation { get; }

    /// <summary>
    /// DI-friendly constructor: accepts an HttpClient and ITokenContext from the container.
    /// The HttpClient should be created via IHttpClientFactory with DiagnosticLoggingHandler
    /// already wired in (see ServiceCollectionExtensions).
    /// </summary>
    internal MarketingAndPromotionsClient(
        MarketingAndPromotionsConfiguration config,
        HttpClient marketingHttpClient,
        HttpClient recommendationHttpClient,
        ITokenContext tokenContext)
    {
        ArgumentNullException.ThrowIfNull(config);
        ArgumentNullException.ThrowIfNull(marketingHttpClient);
        ArgumentNullException.ThrowIfNull(recommendationHttpClient);
        ArgumentNullException.ThrowIfNull(tokenContext);

        _marketingHttpClient = marketingHttpClient;
        _recommendationHttpClient = recommendationHttpClient;
        _ownsHttpClients = false;

        var auth = new BaseBearerTokenAuthenticationProvider(new KiotaAuthProvider(config, tokenContext));

        Marketing = new MarketingClient(new MarketingApiClient(
            new HttpClientRequestAdapter(auth, httpClient: BuildKiotaClient(config.MarketingBaseUrl, _marketingHttpClient))), config, tokenContext);

        Recommendation = new RecommendationClient(new RecommendationApiClient(
            new HttpClientRequestAdapter(auth, httpClient: BuildKiotaClient(config.RecommendationBaseUrl, _recommendationHttpClient))), config, tokenContext);
    }

    /// <summary>
    /// Standalone constructor for use outside DI (tests, scripts).
    /// Builds its own handler chain including DiagnosticLoggingHandler.
    /// </summary>
    public MarketingAndPromotionsClient(MarketingAndPromotionsConfiguration config)
        : this(
            config,
            BuildStandaloneHttpClient(config),
            BuildStandaloneHttpClient(config),
            new AsyncLocalTokenContext())
    {
        _ownsHttpClients = true;
    }

    private static HttpClient BuildKiotaClient(string baseUrl, HttpClient httpClient)
    {
        httpClient.BaseAddress = new Uri(baseUrl);
        return httpClient;
    }

    private static HttpClient BuildStandaloneHttpClient(MarketingAndPromotionsConfiguration config)
    {
        var handler = new DiagnosticLoggingHandler(config)
        {
            InnerHandler = new SocketsHttpHandler { PooledConnectionLifetime = TimeSpan.FromMinutes(2) }
        };
        return new HttpClient(handler) { Timeout = config.HttpTimeout };
    }

    public void Dispose()
    {
        if (_ownsHttpClients)
        {
            _marketingHttpClient.Dispose();
            _recommendationHttpClient.Dispose();
        }
    }
}
