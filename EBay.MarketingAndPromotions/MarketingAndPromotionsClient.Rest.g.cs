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

public partial class MarketingAndPromotionsClient
{
    public MarketingClient Marketing { get; private set; } = null!;
    public RecommendationClient Recommendation { get; private set; } = null!;

    private void InitializeRestClients(
        IAuthenticationProvider auth,
        MarketingAndPromotionsConfiguration config,
        ITokenContext tokenContext)
    {
        Marketing = new MarketingClient(new MarketingApiClient(
            new HttpClientRequestAdapter(auth, httpClient: BuildKiotaClient(config.MarketingBaseUrl, _marketingHttpClient))), config, tokenContext);

        Recommendation = new RecommendationClient(new RecommendationApiClient(
            new HttpClientRequestAdapter(auth, httpClient: BuildKiotaClient(config.RecommendationBaseUrl, _recommendationHttpClient))), config, tokenContext);
    }
}
