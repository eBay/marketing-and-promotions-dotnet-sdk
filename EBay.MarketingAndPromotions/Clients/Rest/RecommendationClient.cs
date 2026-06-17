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


using System.Net;
using System.Threading;
using System.Threading.Tasks;
using EBay.MarketingAndPromotions.Configuration;
using EBay.MarketingAndPromotions.Exceptions;
using EBay.MarketingAndPromotions.Generated.Recommendation;
using EBay.MarketingAndPromotions.Generated.Recommendation.Find;
using EBay.MarketingAndPromotions.Generated.Recommendation.Models;
using Microsoft.Kiota.Abstractions;

namespace EBay.MarketingAndPromotions.Clients.Rest;

public sealed class RecommendationClient
{
    private readonly RecommendationApiClient _kiota;
    private readonly MarketingAndPromotionsConfiguration _config;
    private readonly ITokenContext _tokenContext;

    internal RecommendationClient(RecommendationApiClient kiota, MarketingAndPromotionsConfiguration config, ITokenContext tokenContext)
        => (_kiota, _config, _tokenContext) = (kiota, config, tokenContext);

    private void ApplyHeaders(RequestHeaders headers, RequestOptions? callOptions)
    {
        foreach (var (k, v) in _config.DefaultRequestOptions.Headers)
            headers.TryAdd(k, v);
        if (callOptions is null) return;
        foreach (var (k, v) in callOptions.Headers)
            headers[k] = new[] { v };
    }

    public async Task<PagedListingRecommendationCollection?> FindListingRecommendationsAsync(
        FindListingRecommendationRequest body,
        Action<RequestConfiguration<FindRequestBuilder.FindRequestBuilderPostQueryParameters>>? configure = null,
        string? accessToken = null,
        RequestOptions? requestOptions = null,
        CancellationToken ct = default)
    {
        try
        {
            return await _tokenContext.RunAsync(accessToken, () => _kiota.Find.PostAsync(body, cfg =>
            {
                configure?.Invoke(cfg);
                ApplyHeaders(cfg.Headers, requestOptions);
            }, ct));
        }
        catch (ApiException ex) when (ex.ResponseStatusCode == (int)HttpStatusCode.Unauthorized)
        {
            throw new UnauthorizedException(ex);
        }
        catch (ApiException ex)
        {
            throw new SdkException($"REST API error ({ex.ResponseStatusCode})", ex);
        }
    }
}
