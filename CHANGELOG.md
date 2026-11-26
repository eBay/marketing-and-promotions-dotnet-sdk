# ChangeLog

## 0.5.0 (11/26/2026)

### Added

- Local mock servers: base URL properties accept plain `http` when the host is a loopback host (`127.0.0.1`, `::1` or `localhost`) that you also add to `MarketingAndPromotionsConfiguration.AllowedHosts`. Before, every base URL had to use HTTPS. Every other host still requires HTTPS. `MarketingAndPromotionsConfiguration.LoopbackHosts` lists the loopback hosts.

### Changed

- Hosts are compared case-insensitively and without IPv6 brackets, so `[::1]` and `::1` match.

## 0.4.0 (11/17/2026)

### Breaking changes

#### Marketing API changes

- `MarketingClient.GetCampaignsAsync`: `marketplaceId` is now a `MarketplaceIdEnum?` instead of a `string?`.
- `MarketplaceIdEnum` members are reordered (`EBAY_US` is now first), so their numeric values change. Code that stores or compares the enum as a number must be updated; code that uses the member names is unaffected.

#### Simpler dependency injection

- `AddMarketingAndPromotionsSdk` now registers only the configuration and a singleton `MarketingAndPromotionsClient`. `MarketingClient` and `RecommendationClient` are no longer registered individually: inject `MarketingAndPromotionsClient` and use its `Marketing` and `Recommendation` properties.
- The SDK no longer uses `IHttpClientFactory` or registers named `HttpClient`s, and no longer depends on `Microsoft.Extensions.Http`. To customise HTTP (proxy, client certificates, test doubles), use the new `MarketingAndPromotionsClient(MarketingAndPromotionsConfiguration, HttpMessageHandler)` constructor.

#### Trusted hosts

- The client constructors throw `InvalidOperationException` when a base URL uses a host that is not trusted (see Security). Add such hosts, for example a proxy, to `MarketingAndPromotionsConfiguration.AllowedHosts`.

### Security

- The access token is sent only to trusted hosts. Before, it was attached to requests for any HTTPS host configured as a base URL.

### Added

- `MarketingAndPromotionsConfiguration.AllowedHosts` for additional hosts trusted with the access token, and `MarketingAndPromotionsConfiguration.DefaultAllowedHosts`. The eBay production hosts and their sandbox counterparts (for example `api.sandbox.ebay.com`) are trusted by default, so the sandbox needs no extra configuration.
- Target framework `net472` (.NET Framework 4.7.2).
- `MarketingClient.GetSellerPromotionalOffersAsync` and `MarketingClient.ActivatePromotionAsync`, with their models (`SellerPromotionalOffersResponse`, `SellerPromotionalOffer`, `PromotionSignUpRequest`, `PromotionSignUpResponse`, `Activation`, `OfferStateEnum`, `OfferTypeEnum`).

### Changed

- Dependency minimums are lowered to the versions Kiota itself requires, so projects can keep older compatible versions: `Microsoft.Kiota.Bundle` >= 1.22.0 and `Microsoft.Extensions.DependencyInjection.Abstractions` >= 6.0.0. Previously the SDK required the newest 10.0.x packages. On .NET Framework the SDK requires `System.Text.Json` >= 10.0.0, matching the oldest supported .NET LTS (.NET 10). None of these minimums has a known vulnerability, and each release is tested at both the minimum and the latest versions.
- On .NET Framework the SDK's HttpClients allow 64 connections per host instead of the `ServicePointManager` default of 2.
- `MarketingAndPromotionsConfiguration.HttpTimeout` and `RequestOptions.Headers` have `set` accessors instead of `init`, so projects compiled with C# 7.3 can assign them. Existing source compiles unchanged; recompile code that was built against 0.3.0.

## 0.3.0 (09/25/2026)

### Added

- Target frameworks `net8.0` and `net9.0`, in addition to `net10.0`.

## 0.2.0 (09/14/2026)

### Breaking changes

#### Renamed operations

| Client | Previous | Current |
| --- | --- | --- |
| `MarketingClient` | `BulkCreateKeywordsAsync` | `BulkCreateKeywordAsync` |
| `MarketingClient` | `BulkCreateNegativeKeywordsAsync` | `BulkCreateNegativeKeywordAsync` |
| `MarketingClient` | `BulkUpdateKeywordsAsync` | `BulkUpdateKeywordAsync` |
| `MarketingClient` | `BulkUpdateNegativeKeywordsAsync` | `BulkUpdateNegativeKeywordAsync` |
| `MarketingClient` | `CreateAdAsync` | `CreateAdByListingIdAsync` |
| `MarketingClient` | `CreateItemPriceMarkdownAsync` | `CreateItemPriceMarkdownPromotionAsync` |
| `MarketingClient` | `DeleteItemPriceMarkdownAsync` | `DeleteItemPriceMarkdownPromotionAsync` |
| `MarketingClient` | `DownloadReportAsync` | `GetReportAsync` |
| `MarketingClient` | `GetEmailCampaignAudiencesAsync` | `GetAudiencesAsync` |
| `MarketingClient` | `GetItemPriceMarkdownAsync` | `GetItemPriceMarkdownPromotionAsync` |
| `MarketingClient` | `GetPromotionListingSetAsync` | `GetListingSetAsync` |
| `MarketingClient` | `GetReportMetadatasAsync` | `GetReportMetadataAsync` |
| `MarketingClient` | `GetReportMetadataAsync` | `GetReportMetadataForReportTypeAsync` |
| `MarketingClient` | `GetSuggestedItemsAsync` | `SuggestItemsAsync` |
| `MarketingClient` | `UpdateAdBidAsync` | `UpdateBidAsync` |
| `MarketingClient` | `UpdateItemPriceMarkdownAsync` | `UpdateItemPriceMarkdownPromotionAsync` |

#### Changed signatures

`MarketingClient`:

| Operation | Previous parameters | Current parameters |
| --- | --- | --- |
| `CreateEmailCampaignAsync` | `(CreateEmailCampaignRequest body, string? accessToken, RequestOptions? requestOptions, CancellationToken ct)` | `(string xEBAYCMARKETPLACEID, CreateEmailCampaignRequest body, string? accessToken, RequestOptions? requestOptions, CancellationToken ct)` |
| `FindCampaignByAdReferenceAsync` | `(Action<RequestConfiguration<Find_campaign_by_ad_referenceRequestBuilder.Find_campaign_by_ad_referenceRequestBuilderGetQueryParameters>>? configure, string? accessToken, RequestOptions? requestOptions, CancellationToken ct)` | `(string? inventoryReferenceId, Generated.Marketing.Ad_campaign.Find_campaign_by_ad_reference.GetInventory_reference_typeQueryParameterType? inventoryReferenceType, string? listingId, string? accessToken, RequestOptions? requestOptions, CancellationToken ct)` |
| `GetAdAsync` | `(string campaignId, string adId, string? accessToken, RequestOptions? requestOptions, CancellationToken ct)` | `(string adId, string campaignId, Generated.Marketing.Ad_campaign.Item.Ad.Item.GetFieldgroupsQueryParameterType? fieldgroups, string? accessToken, RequestOptions? requestOptions, CancellationToken ct)` |
| `GetAdGroupsAsync` | `(string campaignId, Action<RequestConfiguration<Ad_groupRequestBuilder.Ad_groupRequestBuilderGetQueryParameters>>? configure, string? accessToken, RequestOptions? requestOptions, CancellationToken ct)` | `(string campaignId, Generated.Marketing.Ad_campaign.Item.Ad_group.GetAd_group_statusQueryParameterType? adGroupStatus, string? limit, string? offset, string? accessToken, RequestOptions? requestOptions, CancellationToken ct)` |
| `GetAdsAsync` | `(string campaignId, Action<RequestConfiguration<AdRequestBuilder.AdRequestBuilderGetQueryParameters>>? configure, string? accessToken, RequestOptions? requestOptions, CancellationToken ct)` | `(string campaignId, string? adGroupIds, Generated.Marketing.Ad_campaign.Item.Ad.GetAd_statusQueryParameterType? adStatus, string? limit, string? listingIds, string? offset, Generated.Marketing.Ad_campaign.Item.Ad.GetFieldgroupsQueryParameterType? fieldgroups, string? accessToken, RequestOptions? requestOptions, CancellationToken ct)` |
| `GetAdsByInventoryReferenceAsync` | `(string campaignId, Action<RequestConfiguration<Get_ads_by_inventory_referenceRequestBuilder.Get_ads_by_inventory_referenceRequestBuilderGetQueryParameters>>? configure, string? accessToken, RequestOptions? requestOptions, CancellationToken ct)` | `(string campaignId, string inventoryReferenceId, Generated.Marketing.Ad_campaign.Item.Get_ads_by_inventory_reference.GetInventory_reference_typeQueryParameterType inventoryReferenceType, string? accessToken, RequestOptions? requestOptions, CancellationToken ct)` |
| `GetAudiencesAsync` | `(Action<RequestConfiguration<AudienceRequestBuilder.AudienceRequestBuilderGetQueryParameters>>? configure, string? accessToken, RequestOptions? requestOptions, CancellationToken ct)` | `(string emailCampaignType, string? limit, string? offset, string? accessToken, RequestOptions? requestOptions, CancellationToken ct)` |
| `GetCampaignByNameAsync` | `(Action<RequestConfiguration<Get_campaign_by_nameRequestBuilder.Get_campaign_by_nameRequestBuilderGetQueryParameters>>? configure, string? accessToken, RequestOptions? requestOptions, CancellationToken ct)` | `(string campaignName, string? accessToken, RequestOptions? requestOptions, CancellationToken ct)` |
| `GetCampaignsAsync` | `(Action<RequestConfiguration<Ad_campaignRequestBuilder.Ad_campaignRequestBuilderGetQueryParameters>>? configure, string? accessToken, RequestOptions? requestOptions, CancellationToken ct)` | `(string? campaignName, Generated.Marketing.Ad_campaign.GetCampaign_statusQueryParameterType? campaignStatus, Generated.Marketing.Ad_campaign.GetCampaign_targeting_typesQueryParameterType? campaignTargetingTypes, Generated.Marketing.Ad_campaign.GetChannelsQueryParameterType? channels, string? endDateRange, Generated.Marketing.Ad_campaign.GetFunding_strategyQueryParameterType? fundingStrategy, string? limit, string? offset, string? startDateRange, string? marketplaceId, string? accessToken, RequestOptions? requestOptions, CancellationToken ct)` |
| `GetEmailCampaignsAsync` | `(Action<RequestConfiguration<Email_campaignRequestBuilder.Email_campaignRequestBuilderGetQueryParameters>>? configure, string? accessToken, RequestOptions? requestOptions, CancellationToken ct)` | `(string? limit, string? offset, string? q, string? sort, string? accessToken, RequestOptions? requestOptions, CancellationToken ct)` |
| `GetEmailReportAsync` | `(Action<RequestConfiguration<ReportRequestBuilder.ReportRequestBuilderGetQueryParameters>>? configure, string? accessToken, RequestOptions? requestOptions, CancellationToken ct)` | `(string endDate, string startDate, string? accessToken, RequestOptions? requestOptions, CancellationToken ct)` |
| `GetKeywordsAsync` | `(string campaignId, Action<RequestConfiguration<KeywordRequestBuilder.KeywordRequestBuilderGetQueryParameters>>? configure, string? accessToken, RequestOptions? requestOptions, CancellationToken ct)` | `(string campaignId, string? adGroupIds, Generated.Marketing.Ad_campaign.Item.Keyword.GetKeyword_statusQueryParameterType? keywordStatus, string? limit, string? offset, string? accessToken, RequestOptions? requestOptions, CancellationToken ct)` |
| `GetListingSetAsync` | `(string promotionId, Action<RequestConfiguration<Get_listing_setRequestBuilder.Get_listing_setRequestBuilderGetQueryParameters>>? configure, string? accessToken, RequestOptions? requestOptions, CancellationToken ct)` | `(string promotionId, string? limit, string? offset, string? q, string? sort, ItemMarkdownStatusEnum? status, string? accessToken, RequestOptions? requestOptions, CancellationToken ct)` |
| `GetNegativeKeywordsAsync` | `(Action<RequestConfiguration<Negative_keywordRequestBuilder.Negative_keywordRequestBuilderGetQueryParameters>>? configure, string? accessToken, RequestOptions? requestOptions, CancellationToken ct)` | `(string adGroupIds, string? campaignIds, string? limit, Generated.Marketing.Negative_keyword.GetNegative_keyword_statusQueryParameterType? negativeKeywordStatus, string? offset, string? accessToken, RequestOptions? requestOptions, CancellationToken ct)` |
| `GetPromotionReportsAsync` | `(Action<RequestConfiguration<Promotion_reportRequestBuilder.Promotion_reportRequestBuilderGetQueryParameters>>? configure, string? accessToken, RequestOptions? requestOptions, CancellationToken ct)` | `(string marketplaceId, string? limit, string? offset, string? promotionStatus, string? promotionType, string? q, string? accessToken, RequestOptions? requestOptions, CancellationToken ct)` |
| `GetPromotionsAsync` | `(Action<RequestConfiguration<PromotionRequestBuilder.PromotionRequestBuilderGetQueryParameters>>? configure, string? accessToken, RequestOptions? requestOptions, CancellationToken ct)` | `(string marketplaceId, string? limit, string? offset, string? promotionStatus, string? promotionType, string? q, string? sort, string? accessToken, RequestOptions? requestOptions, CancellationToken ct)` |
| `GetPromotionSummaryReportAsync` | `(Action<RequestConfiguration<Promotion_summary_reportRequestBuilder.Promotion_summary_reportRequestBuilderGetQueryParameters>>? configure, string? accessToken, RequestOptions? requestOptions, CancellationToken ct)` | `(string marketplaceId, string? accessToken, RequestOptions? requestOptions, CancellationToken ct)` |
| `GetReportMetadataAsync` | `(Action<RequestConfiguration<Ad_report_metadataRequestBuilder.Ad_report_metadataRequestBuilderGetQueryParameters>>? configure, string? accessToken, RequestOptions? requestOptions, CancellationToken ct)` | `(string? fundingModel, string? channel, string? accessToken, RequestOptions? requestOptions, CancellationToken ct)` |
| `GetReportMetadataForReportTypeAsync` | `(string reportType, Action<RequestConfiguration<WithReport_typeItemRequestBuilder.WithReport_typeItemRequestBuilderGetQueryParameters>>? configure, string? accessToken, RequestOptions? requestOptions, CancellationToken ct)` | `(string reportType, string? fundingModel, string? channel, string? accessToken, RequestOptions? requestOptions, CancellationToken ct)` |
| `GetReportTasksAsync` | `(Action<RequestConfiguration<Ad_report_taskRequestBuilder.Ad_report_taskRequestBuilderGetQueryParameters>>? configure, string? accessToken, RequestOptions? requestOptions, CancellationToken ct)` | `(string? limit, string? offset, string? reportTaskStatuses, string? accessToken, RequestOptions? requestOptions, CancellationToken ct)` |
| `SuggestBudgetAsync` | `(string? accessToken, RequestOptions? requestOptions, CancellationToken ct)` | `(string xEBAYCMARKETPLACEID, string? accessToken, RequestOptions? requestOptions, CancellationToken ct)` |
| `SuggestItemsAsync` | `(string campaignId, Action<RequestConfiguration<Suggest_itemsRequestBuilder.Suggest_itemsRequestBuilderGetQueryParameters>>? configure, string? accessToken, RequestOptions? requestOptions, CancellationToken ct)` | `(string campaignId, string? categoryIds, string? limit, string? offset, string? accessToken, RequestOptions? requestOptions, CancellationToken ct)` |

`RecommendationClient`:

| Operation | Previous parameters | Current parameters |
| --- | --- | --- |
| `FindListingRecommendationsAsync` | `(FindListingRecommendationRequest body, Action<RequestConfiguration<FindRequestBuilder.FindRequestBuilderPostQueryParameters>>? configure, string? accessToken, RequestOptions? requestOptions, CancellationToken ct)` | `(string xEBAYCMARKETPLACEID, FindListingRecommendationRequest body, string? filter, int? limit, int? offset, string? accessToken, RequestOptions? requestOptions, CancellationToken ct)` |

### Added

- `MarketingClient.BulkUpsertVideoAdsPreferencesAsync(string campaignId, BulkUpsertVideoAdsPreferencesRequest body, string? accessToken, RequestOptions? requestOptions, CancellationToken ct)`
- `MarketingClient.UpsertVideoAdsPreferencesAsync(string campaignId, UpsertVideoAdsPreferencesRequest body, string? accessToken, RequestOptions? requestOptions, CancellationToken ct)`
