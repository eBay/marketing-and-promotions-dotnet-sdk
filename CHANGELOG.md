# ChangeLog

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
