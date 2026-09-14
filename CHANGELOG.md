# ChangeLog

## 2.0.0 (09/14/2026)

**Breaking changes**
- The following operations changed signature on `MarketingClient`:
  - `FindCampaignByAdReferenceAsync`: `(string?, string?, string?, string?, RequestOptions?, CancellationToken)` → `(string?, Generated.Marketing.Ad_campaign.Find_campaign_by_ad_reference.GetInventory_reference_typeQueryParameterType?, string?, string?, RequestOptions?, CancellationToken)`
  - `GetAdAsync`: `(string adId, string campaignId, string? accessToken, RequestOptions? requestOptions, CancellationToken ct)` → `(string adId, string campaignId, Generated.Marketing.Ad_campaign.Item.Ad.Item.GetFieldgroupsQueryParameterType? fieldgroups, string? accessToken, RequestOptions? requestOptions, CancellationToken ct)`
  - `GetAdGroupsAsync`: `(string, string?, string?, string?, string?, RequestOptions?, CancellationToken)` → `(string, Generated.Marketing.Ad_campaign.Item.Ad_group.GetAd_group_statusQueryParameterType?, string?, string?, string?, RequestOptions?, CancellationToken)`
  - `GetAdsAsync`: `(string campaignId, string? adGroupIds, string? adStatus, string? limit, string? listingIds, string? offset, string? accessToken, RequestOptions? requestOptions, CancellationToken ct)` → `(string campaignId, string? adGroupIds, Generated.Marketing.Ad_campaign.Item.Ad.GetAd_statusQueryParameterType? adStatus, string? limit, string? listingIds, string? offset, Generated.Marketing.Ad_campaign.Item.Ad.GetFieldgroupsQueryParameterType? fieldgroups, string? accessToken, RequestOptions? requestOptions, CancellationToken ct)`
  - `GetAdsByInventoryReferenceAsync`: `(string, string, string, string?, RequestOptions?, CancellationToken)` → `(string, string, Generated.Marketing.Ad_campaign.Item.Get_ads_by_inventory_reference.GetInventory_reference_typeQueryParameterType, string?, RequestOptions?, CancellationToken)`
  - `GetCampaignsAsync`: `(string? campaignName, string? campaignStatus, string? campaignTargetingTypes, string? channels, string? endDateRange, string? fundingStrategy, string? limit, string? offset, string? startDateRange, string? accessToken, RequestOptions? requestOptions, CancellationToken ct)` → `(string? campaignName, Generated.Marketing.Ad_campaign.GetCampaign_statusQueryParameterType? campaignStatus, Generated.Marketing.Ad_campaign.GetCampaign_targeting_typesQueryParameterType? campaignTargetingTypes, Generated.Marketing.Ad_campaign.GetChannelsQueryParameterType? channels, string? endDateRange, Generated.Marketing.Ad_campaign.GetFunding_strategyQueryParameterType? fundingStrategy, string? limit, string? offset, string? startDateRange, string? marketplaceId, string? accessToken, RequestOptions? requestOptions, CancellationToken ct)`
  - `GetKeywordsAsync`: `(string, string?, string?, string?, string?, string?, RequestOptions?, CancellationToken)` → `(string, string?, Generated.Marketing.Ad_campaign.Item.Keyword.GetKeyword_statusQueryParameterType?, string?, string?, string?, RequestOptions?, CancellationToken)`
  - `GetListingSetAsync`: `(string, string?, string?, string?, string?, string?, string?, RequestOptions?, CancellationToken)` → `(string, string?, string?, string?, string?, ItemMarkdownStatusEnum?, string?, RequestOptions?, CancellationToken)`
  - `GetNegativeKeywordsAsync`: `(string, string?, string?, string?, string?, string?, RequestOptions?, CancellationToken)` → `(string, string?, string?, Generated.Marketing.Negative_keyword.GetNegative_keyword_statusQueryParameterType?, string?, string?, RequestOptions?, CancellationToken)`
- The following operations changed signature on `RecommendationClient`:
  - `FindListingRecommendationsAsync`: `(string, FindListingRecommendationRequest, string?, string?, string?, string?, RequestOptions?, CancellationToken)` → `(string, FindListingRecommendationRequest, string?, int?, int?, string?, RequestOptions?, CancellationToken)`

- The following operations were added to `MarketingClient`:
  - `BulkUpsertVideoAdsPreferencesAsync(string, BulkUpsertVideoAdsPreferencesRequest, string?, RequestOptions?, CancellationToken)`
  - `UpsertVideoAdsPreferencesAsync(string, UpsertVideoAdsPreferencesRequest, string?, RequestOptions?, CancellationToken)`

## 1.0.0 (09/14/2026)

**Breaking changes**
- The following operations were removed from `MarketingClient`:
  - `BulkCreateKeywordsAsync(string, BulkCreateKeywordRequest, string?, RequestOptions?, CancellationToken)`
  - `BulkCreateNegativeKeywordsAsync(BulkCreateNegativeKeywordRequest, string?, RequestOptions?, CancellationToken)`
  - `BulkUpdateKeywordsAsync(string, BulkUpdateKeywordRequest, string?, RequestOptions?, CancellationToken)`
  - `BulkUpdateNegativeKeywordsAsync(BulkUpdateNegativeKeywordRequest, string?, RequestOptions?, CancellationToken)`
  - `CreateAdAsync(string, CreateAdRequest, string?, RequestOptions?, CancellationToken)`
  - `CreateItemPriceMarkdownAsync(ItemPriceMarkdown, string?, RequestOptions?, CancellationToken)`
  - `DeleteItemPriceMarkdownAsync(string, string?, RequestOptions?, CancellationToken)`
  - `DownloadReportAsync(string, string?, RequestOptions?, CancellationToken)`
  - `GetEmailCampaignAudiencesAsync(Action<RequestConfiguration<AudienceRequestBuilder.AudienceRequestBuilderGetQueryParameters>>?, string?, RequestOptions?, CancellationToken)`
  - `GetItemPriceMarkdownAsync(string, string?, RequestOptions?, CancellationToken)`
  - `GetPromotionListingSetAsync(string, Action<RequestConfiguration<Get_listing_setRequestBuilder.Get_listing_setRequestBuilderGetQueryParameters>>?, string?, RequestOptions?, CancellationToken)`
  - `GetReportMetadatasAsync(Action<RequestConfiguration<Ad_report_metadataRequestBuilder.Ad_report_metadataRequestBuilderGetQueryParameters>>?, string?, RequestOptions?, CancellationToken)`
  - `GetSuggestedItemsAsync(string, Action<RequestConfiguration<Suggest_itemsRequestBuilder.Suggest_itemsRequestBuilderGetQueryParameters>>?, string?, RequestOptions?, CancellationToken)`
  - `UpdateAdBidAsync(string, string, UpdateBidPercentageRequest, string?, RequestOptions?, CancellationToken)`
  - `UpdateItemPriceMarkdownAsync(string, ItemPriceMarkdown, string?, RequestOptions?, CancellationToken)`
- The following operations changed signature on `MarketingClient`:
  - `CreateEmailCampaignAsync`: `(CreateEmailCampaignRequest, string?, RequestOptions?, CancellationToken)` → `(string, CreateEmailCampaignRequest, string?, RequestOptions?, CancellationToken)`
  - `FindCampaignByAdReferenceAsync`: `(Action<RequestConfiguration<Find_campaign_by_ad_referenceRequestBuilder.Find_campaign_by_ad_referenceRequestBuilderGetQueryParameters>>?, string?, RequestOptions?, CancellationToken)` → `(string?, string?, string?, string?, RequestOptions?, CancellationToken)`
  - `GetAdGroupsAsync`: `(string, Action<RequestConfiguration<Ad_groupRequestBuilder.Ad_groupRequestBuilderGetQueryParameters>>?, string?, RequestOptions?, CancellationToken)` → `(string, string?, string?, string?, string?, RequestOptions?, CancellationToken)`
  - `GetAdsAsync`: `(string, Action<RequestConfiguration<AdRequestBuilder.AdRequestBuilderGetQueryParameters>>?, string?, RequestOptions?, CancellationToken)` → `(string, string?, string?, string?, string?, string?, string?, RequestOptions?, CancellationToken)`
  - `GetAdsByInventoryReferenceAsync`: `(string, Action<RequestConfiguration<Get_ads_by_inventory_referenceRequestBuilder.Get_ads_by_inventory_referenceRequestBuilderGetQueryParameters>>?, string?, RequestOptions?, CancellationToken)` → `(string, string, string, string?, RequestOptions?, CancellationToken)`
  - `GetCampaignByNameAsync`: `(Action<RequestConfiguration<Get_campaign_by_nameRequestBuilder.Get_campaign_by_nameRequestBuilderGetQueryParameters>>?, string?, RequestOptions?, CancellationToken)` → `(string, string?, RequestOptions?, CancellationToken)`
  - `GetCampaignsAsync`: `(Action<RequestConfiguration<Ad_campaignRequestBuilder.Ad_campaignRequestBuilderGetQueryParameters>>?, string?, RequestOptions?, CancellationToken)` → `(string?, string?, string?, string?, string?, string?, string?, string?, string?, string?, RequestOptions?, CancellationToken)`
  - `GetEmailCampaignsAsync`: `(Action<RequestConfiguration<Email_campaignRequestBuilder.Email_campaignRequestBuilderGetQueryParameters>>?, string?, RequestOptions?, CancellationToken)` → `(string?, string?, string?, string?, string?, RequestOptions?, CancellationToken)`
  - `GetEmailReportAsync`: `(Action<RequestConfiguration<ReportRequestBuilder.ReportRequestBuilderGetQueryParameters>>?, string?, RequestOptions?, CancellationToken)` → `(string, string, string?, RequestOptions?, CancellationToken)`
  - `GetKeywordsAsync`: `(string, Action<RequestConfiguration<KeywordRequestBuilder.KeywordRequestBuilderGetQueryParameters>>?, string?, RequestOptions?, CancellationToken)` → `(string, string?, string?, string?, string?, string?, RequestOptions?, CancellationToken)`
  - `GetNegativeKeywordsAsync`: `(Action<RequestConfiguration<Negative_keywordRequestBuilder.Negative_keywordRequestBuilderGetQueryParameters>>?, string?, RequestOptions?, CancellationToken)` → `(string, string?, string?, string?, string?, string?, RequestOptions?, CancellationToken)`
  - `GetPromotionReportsAsync`: `(Action<RequestConfiguration<Promotion_reportRequestBuilder.Promotion_reportRequestBuilderGetQueryParameters>>?, string?, RequestOptions?, CancellationToken)` → `(string, string?, string?, string?, string?, string?, string?, RequestOptions?, CancellationToken)`
  - `GetPromotionsAsync`: `(Action<RequestConfiguration<PromotionRequestBuilder.PromotionRequestBuilderGetQueryParameters>>?, string?, RequestOptions?, CancellationToken)` → `(string, string?, string?, string?, string?, string?, string?, string?, RequestOptions?, CancellationToken)`
  - `GetPromotionSummaryReportAsync`: `(Action<RequestConfiguration<Promotion_summary_reportRequestBuilder.Promotion_summary_reportRequestBuilderGetQueryParameters>>?, string?, RequestOptions?, CancellationToken)` → `(string, string?, RequestOptions?, CancellationToken)`
  - `GetReportMetadataAsync`: `(string, Action<RequestConfiguration<WithReport_typeItemRequestBuilder.WithReport_typeItemRequestBuilderGetQueryParameters>>?, string?, RequestOptions?, CancellationToken)` → `(string?, string?, string?, RequestOptions?, CancellationToken)`
  - `GetReportTasksAsync`: `(Action<RequestConfiguration<Ad_report_taskRequestBuilder.Ad_report_taskRequestBuilderGetQueryParameters>>?, string?, RequestOptions?, CancellationToken)` → `(string?, string?, string?, string?, RequestOptions?, CancellationToken)`
  - `SuggestBudgetAsync`: `(string?, RequestOptions?, CancellationToken)` → `(string, string?, RequestOptions?, CancellationToken)`
- The following operations changed signature on `RecommendationClient`:
  - `FindListingRecommendationsAsync`: `(FindListingRecommendationRequest, Action<RequestConfiguration<FindRequestBuilder.FindRequestBuilderPostQueryParameters>>?, string?, RequestOptions?, CancellationToken)` → `(string, FindListingRecommendationRequest, string?, string?, string?, string?, RequestOptions?, CancellationToken)`

- The following operations were added to `MarketingClient`:
  - `BulkCreateKeywordAsync(string, BulkCreateKeywordRequest, string?, RequestOptions?, CancellationToken)`
  - `BulkCreateNegativeKeywordAsync(BulkCreateNegativeKeywordRequest, string?, RequestOptions?, CancellationToken)`
  - `BulkUpdateKeywordAsync(string, BulkUpdateKeywordRequest, string?, RequestOptions?, CancellationToken)`
  - `BulkUpdateNegativeKeywordAsync(BulkUpdateNegativeKeywordRequest, string?, RequestOptions?, CancellationToken)`
  - `CreateAdByListingIdAsync(string, CreateAdRequest, string?, RequestOptions?, CancellationToken)`
  - `CreateItemPriceMarkdownPromotionAsync(ItemPriceMarkdown, string?, RequestOptions?, CancellationToken)`
  - `DeleteItemPriceMarkdownPromotionAsync(string, string?, RequestOptions?, CancellationToken)`
  - `GetAudiencesAsync(string, string?, string?, string?, RequestOptions?, CancellationToken)`
  - `GetItemPriceMarkdownPromotionAsync(string, string?, RequestOptions?, CancellationToken)`
  - `GetListingSetAsync(string, string?, string?, string?, string?, string?, string?, RequestOptions?, CancellationToken)`
  - `GetReportAsync(string, string?, RequestOptions?, CancellationToken)`
  - `GetReportMetadataForReportTypeAsync(string, string?, string?, string?, RequestOptions?, CancellationToken)`
  - `SuggestItemsAsync(string, string?, string?, string?, string?, RequestOptions?, CancellationToken)`
  - `UpdateBidAsync(string, string, UpdateBidPercentageRequest, string?, RequestOptions?, CancellationToken)`
  - `UpdateItemPriceMarkdownPromotionAsync(string, ItemPriceMarkdown, string?, RequestOptions?, CancellationToken)`
