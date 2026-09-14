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
using EBay.MarketingAndPromotions.Generated.Recommendation.Models;
using EBay.MarketingAndPromotions.Generated.Recommendation;
using Microsoft.Kiota.Abstractions;

namespace EBay.MarketingAndPromotions.Clients.Rest;

public sealed class RecommendationClient
{
    private readonly RecommendationApiClient _kiota;
    private readonly MarketingAndPromotionsConfiguration _config;
    private readonly ITokenContext _tokenContext;
    private readonly IRequestAdapter _adapter;

    internal RecommendationClient(RecommendationApiClient kiota, MarketingAndPromotionsConfiguration config, ITokenContext tokenContext, IRequestAdapter adapter)
    {
        _kiota = kiota;
        _config = config;
        _tokenContext = tokenContext;
        _adapter = adapter;
    }

    /// <summary>
    /// The <b>find</b> method currently returns information for a single recommendation type (<c>AD</c>) which contains information that sellers can use to configure <see href="/api-docs/sell/static/marketing/promoted-listings.html">Promoted Listings</see> ad campaigns.
    /// The response from this method includes an array of the seller's listing IDs, where each element in the array contains recommendations related to the associated listing ID. For details on how to use this method, see <see href="/api-docs/sell/static/marketing/pl-reco-api.html">Using the Recommendation API to help configure campaigns</see>.
    /// <b>The AD recommendation type</b>
    /// The <c>AD</c> type contains two sets of information:
    /// - <b>The promoteWithAd indicator</b>
    /// The <b>promoteWithAd</b> response field indicates whether or not eBay recommends you place the associated listing in a Promoted Listings ad campaign.
    /// The returned value is set to either <c>RECOMMENDED</c> or <c>UNDETERMINED</c>, where <c>RECOMMENDED</c> identifies the listings that will benefit the most from having them included in an ad campaign.
    /// - <b>The bid percentage</b>
    /// Also known as the "ad rate," the <b>bidPercentage</b> field provides the current trending bid percentage of similarly promoted items in the marketplace.
    /// The ad rate is a user-specified value that indicates the level of promotion that eBay applies to the campaign across the marketplace. The value is also used to calculate the Promotion Listings fee, which is assessed to the seller if a Promoted Listings action results in the sale of an item.
    /// <b>Configuring the request</b>
    /// You can configure a request to review all of a seller's currently active listings, or just a subset of them.
    /// - <b>All active listings</b> &amp;ndash; If you leave the request body empty, the request targets <i>all</i> the items currently listed by the seller.
    /// Here, the response is filtered to contain only the items where <b>promoteWithAd</b> equals <c>RECOMMENDED</c>. In this case, eBay recommends that all the returned listings should be included in a Promoted Listings ad campaign.
    /// - <b>Selected listing IDs</b> &amp;ndash; If you populate the request body with a set of <b>listingIds</b>, the response contains data for all the specified listing IDs.
    /// In this scenario, the response provides you with information on listings where the <b>promoteWithAd</b> can be either <c>RECOMMENDED</c> or <c>UNDETERMINED</c>.
    /// <b>The paginated response</b>
    /// Because the response can contain many listing IDs, the <b>findListingRecommendations</b> method paginates the response set.
    /// You can control size of the returned pages, as well as an offset that dictates where to start the pagination, using query parameters in the request.
    /// </summary>
    public Task FindListingRecommendationsAsync(
        string xEBAYCMARKETPLACEID,
        FindListingRecommendationRequest body,
        string? filter = null,
        int? limit = null,
        int? offset = null,
        string? accessToken = null,
        RequestOptions? requestOptions = null,
        CancellationToken ct = default)
        => RestInvocation.CallAsync(
            _tokenContext,
            accessToken,
            () => _kiota.Find.PostAsync(body, cfg =>
        {
            if (filter is not null)
            {
                cfg.QueryParameters.Filter = filter;
            }
            if (limit is not null)
            {
                cfg.QueryParameters.Limit = limit;
            }
            if (offset is not null)
            {
                cfg.QueryParameters.Offset = offset;
            }
            cfg.Headers.TryAdd("X-EBAY-C-MARKETPLACE-ID", xEBAYCMARKETPLACEID);
            RestInvocation.ApplyHeaders(cfg.Headers, _config, requestOptions);
        }, ct));
}
