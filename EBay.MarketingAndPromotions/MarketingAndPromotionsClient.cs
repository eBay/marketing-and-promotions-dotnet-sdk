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
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace EBay.MarketingAndPromotions;

public class MarketingAndPromotionsClient : IDisposable
{
    internal const string MarketingClientName = "MarketingAndPromotionsClient.Marketing";
    internal const string RecommendationClientName = "MarketingAndPromotionsClient.Recommendation";

    private readonly HttpClient[] _ownedClients;

    public MarketingClient Marketing { get; private set; } = null!;
    public RecommendationClient Recommendation { get; private set; } = null!;

    /// <summary>
    /// DI-friendly constructor. Named HttpClients come from <see cref="IHttpClientFactory"/>.
    /// </summary>
    internal MarketingAndPromotionsClient(
        MarketingAndPromotionsConfiguration config,
        IHttpClientFactory factory,
        ITokenContext tokenContext)
    {
        _ownedClients = [];
        var auth = new BaseBearerTokenAuthenticationProvider(new KiotaAuthProvider(config, tokenContext));
        InitializeRestClients(auth, config, tokenContext, endpoint => factory.CreateClient(endpoint.HttpClientName));
    }

    /// <summary>
    /// Standalone constructor for use outside DI (tests, scripts).
    /// Creates its own HttpClients with DiagnosticLoggingHandler.
    /// </summary>
    public MarketingAndPromotionsClient(MarketingAndPromotionsConfiguration config)
    {
        var tokenContext = new AsyncLocalTokenContext();
        var auth = new BaseBearerTokenAuthenticationProvider(new KiotaAuthProvider(config, tokenContext));
        var owned = new List<HttpClient>();
        InitializeRestClients(auth, config, tokenContext, endpoint =>
        {
            var client = BuildStandaloneHttpClient(config, endpoint.BaseUrl);
            owned.Add(client);
            return client;
        });
        _ownedClients = [.. owned];
    }

    private static HttpClient BuildStandaloneHttpClient(MarketingAndPromotionsConfiguration config, string baseUrl)
    {
        var handler = new DiagnosticLoggingHandler(config)
        {
            InnerHandler = new SocketsHttpHandler { PooledConnectionLifetime = TimeSpan.FromMinutes(2) }
        };
        return new HttpClient(handler) { BaseAddress = new Uri(baseUrl), Timeout = config.HttpTimeout };
    }

    internal static IReadOnlyList<(string ApiId, string HttpClientName)> RestEndpointNames { get; } =
    [
        ("marketing", MarketingClientName),
        ("recommendation", RecommendationClientName)
    ];

    internal static IReadOnlyList<RestEndpoint> RestEndpoints(MarketingAndPromotionsConfiguration config) =>
        [.. RestEndpointNames.Select(name => new RestEndpoint(name.ApiId, name.HttpClientName, config.RestBaseUrl(name.ApiId)))];

    private void InitializeRestClients(
        IAuthenticationProvider auth,
        MarketingAndPromotionsConfiguration config,
        ITokenContext tokenContext,
        RestHttpClientProvider httpClientFor)
    {
        var endpoints = RestEndpoints(config);
        var marketingAdapter = new HttpClientRequestAdapter(auth, httpClient: httpClientFor(endpoints.First(endpoint => endpoint.ApiId == "marketing")));
        Marketing = new MarketingClient(new MarketingApiClient(marketingAdapter), config, tokenContext, marketingAdapter);
        var recommendationAdapter = new HttpClientRequestAdapter(auth, httpClient: httpClientFor(endpoints.First(endpoint => endpoint.ApiId == "recommendation")));
        Recommendation = new RecommendationClient(new RecommendationApiClient(recommendationAdapter), config, tokenContext, recommendationAdapter);
    }

    public void Dispose()
    {
        foreach (var client in _ownedClients)
        {
            client.Dispose();
        }
        GC.SuppressFinalize(this);
    }
}

internal sealed record RestEndpoint(string ApiId, string HttpClientName, string BaseUrl);

internal delegate HttpClient RestHttpClientProvider(RestEndpoint endpoint);
