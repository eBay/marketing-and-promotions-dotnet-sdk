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
| `CreateEmailCampaignAsync` | `(CreateEmailCampaignRequest, string?, RequestOptions?, CancellationToken)` | `(string, CreateEmailCampaignRequest, string?, RequestOptions?, CancellationToken)` |
| `FindCampaignByAdReferenceAsync` | `(Action<RequestConfiguration<Find_campaign_by_ad_referenceRequestBuilder.Find_campaign_by_ad_referenceRequestBuilderGetQueryParameters>>?, string?, RequestOptions?, CancellationToken)` | `(string?, Generated.Marketing.Ad_campaign.Find_campaign_by_ad_reference.GetInventory_reference_typeQueryParameterType?, string?, string?, RequestOptions?, CancellationToken)` |
| `GetAdAsync` | `(string, string, string?, RequestOptions?, CancellationToken)` | `(string, string, Generated.Marketing.Ad_campaign.Item.Ad.Item.GetFieldgroupsQueryParameterType?, string?, RequestOptions?, CancellationToken)` |
| `GetAdGroupsAsync` | `(string, Action<RequestConfiguration<Ad_groupRequestBuilder.Ad_groupRequestBuilderGetQueryParameters>>?, string?, RequestOptions?, CancellationToken)` | `(string, Generated.Marketing.Ad_campaign.Item.Ad_group.GetAd_group_statusQueryParameterType?, string?, string?, string?, RequestOptions?, CancellationToken)` |
| `GetAdsAsync` | `(string, Action<RequestConfiguration<AdRequestBuilder.AdRequestBuilderGetQueryParameters>>?, string?, RequestOptions?, CancellationToken)` | `(string, string?, Generated.Marketing.Ad_campaign.Item.Ad.GetAd_statusQueryParameterType?, string?, string?, string?, Generated.Marketing.Ad_campaign.Item.Ad.GetFieldgroupsQueryParameterType?, string?, RequestOptions?, CancellationToken)` |
| `GetAdsByInventoryReferenceAsync` | `(string, Action<RequestConfiguration<Get_ads_by_inventory_referenceRequestBuilder.Get_ads_by_inventory_referenceRequestBuilderGetQueryParameters>>?, string?, RequestOptions?, CancellationToken)` | `(string, string, Generated.Marketing.Ad_campaign.Item.Get_ads_by_inventory_reference.GetInventory_reference_typeQueryParameterType, string?, RequestOptions?, CancellationToken)` |
| `GetAudiencesAsync` | `(Action<RequestConfiguration<AudienceRequestBuilder.AudienceRequestBuilderGetQueryParameters>>?, string?, RequestOptions?, CancellationToken)` | `(string, string?, string?, string?, RequestOptions?, CancellationToken)` |
| `GetCampaignByNameAsync` | `(Action<RequestConfiguration<Get_campaign_by_nameRequestBuilder.Get_campaign_by_nameRequestBuilderGetQueryParameters>>?, string?, RequestOptions?, CancellationToken)` | `(string, string?, RequestOptions?, CancellationToken)` |
| `GetCampaignsAsync` | `(Action<RequestConfiguration<Ad_campaignRequestBuilder.Ad_campaignRequestBuilderGetQueryParameters>>?, string?, RequestOptions?, CancellationToken)` | `(string?, Generated.Marketing.Ad_campaign.GetCampaign_statusQueryParameterType?, Generated.Marketing.Ad_campaign.GetCampaign_targeting_typesQueryParameterType?, Generated.Marketing.Ad_campaign.GetChannelsQueryParameterType?, string?, Generated.Marketing.Ad_campaign.GetFunding_strategyQueryParameterType?, string?, string?, string?, string?, string?, RequestOptions?, CancellationToken)` |
| `GetEmailCampaignsAsync` | `(Action<RequestConfiguration<Email_campaignRequestBuilder.Email_campaignRequestBuilderGetQueryParameters>>?, string?, RequestOptions?, CancellationToken)` | `(string?, string?, string?, string?, string?, RequestOptions?, CancellationToken)` |
| `GetEmailReportAsync` | `(Action<RequestConfiguration<ReportRequestBuilder.ReportRequestBuilderGetQueryParameters>>?, string?, RequestOptions?, CancellationToken)` | `(string, string, string?, RequestOptions?, CancellationToken)` |
| `GetKeywordsAsync` | `(string, Action<RequestConfiguration<KeywordRequestBuilder.KeywordRequestBuilderGetQueryParameters>>?, string?, RequestOptions?, CancellationToken)` | `(string, string?, Generated.Marketing.Ad_campaign.Item.Keyword.GetKeyword_statusQueryParameterType?, string?, string?, string?, RequestOptions?, CancellationToken)` |
| `GetListingSetAsync` | `(string, Action<RequestConfiguration<Get_listing_setRequestBuilder.Get_listing_setRequestBuilderGetQueryParameters>>?, string?, RequestOptions?, CancellationToken)` | `(string, string?, string?, string?, string?, ItemMarkdownStatusEnum?, string?, RequestOptions?, CancellationToken)` |
| `GetNegativeKeywordsAsync` | `(Action<RequestConfiguration<Negative_keywordRequestBuilder.Negative_keywordRequestBuilderGetQueryParameters>>?, string?, RequestOptions?, CancellationToken)` | `(string, string?, string?, Generated.Marketing.Negative_keyword.GetNegative_keyword_statusQueryParameterType?, string?, string?, RequestOptions?, CancellationToken)` |
| `GetPromotionReportsAsync` | `(Action<RequestConfiguration<Promotion_reportRequestBuilder.Promotion_reportRequestBuilderGetQueryParameters>>?, string?, RequestOptions?, CancellationToken)` | `(string, string?, string?, string?, string?, string?, string?, RequestOptions?, CancellationToken)` |
| `GetPromotionsAsync` | `(Action<RequestConfiguration<PromotionRequestBuilder.PromotionRequestBuilderGetQueryParameters>>?, string?, RequestOptions?, CancellationToken)` | `(string, string?, string?, string?, string?, string?, string?, string?, RequestOptions?, CancellationToken)` |
| `GetPromotionSummaryReportAsync` | `(Action<RequestConfiguration<Promotion_summary_reportRequestBuilder.Promotion_summary_reportRequestBuilderGetQueryParameters>>?, string?, RequestOptions?, CancellationToken)` | `(string, string?, RequestOptions?, CancellationToken)` |
| `GetReportMetadataAsync` | `(Action<RequestConfiguration<Ad_report_metadataRequestBuilder.Ad_report_metadataRequestBuilderGetQueryParameters>>?, string?, RequestOptions?, CancellationToken)` | `(string?, string?, string?, RequestOptions?, CancellationToken)` |
| `GetReportMetadataForReportTypeAsync` | `(string, Action<RequestConfiguration<WithReport_typeItemRequestBuilder.WithReport_typeItemRequestBuilderGetQueryParameters>>?, string?, RequestOptions?, CancellationToken)` | `(string, string?, string?, string?, RequestOptions?, CancellationToken)` |
| `GetReportTasksAsync` | `(Action<RequestConfiguration<Ad_report_taskRequestBuilder.Ad_report_taskRequestBuilderGetQueryParameters>>?, string?, RequestOptions?, CancellationToken)` | `(string?, string?, string?, string?, RequestOptions?, CancellationToken)` |
| `SuggestBudgetAsync` | `(string?, RequestOptions?, CancellationToken)` | `(string, string?, RequestOptions?, CancellationToken)` |
| `SuggestItemsAsync` | `(string, Action<RequestConfiguration<Suggest_itemsRequestBuilder.Suggest_itemsRequestBuilderGetQueryParameters>>?, string?, RequestOptions?, CancellationToken)` | `(string, string?, string?, string?, string?, RequestOptions?, CancellationToken)` |

`RecommendationClient`:

| Operation | Previous parameters | Current parameters |
| --- | --- | --- |
| `FindListingRecommendationsAsync` | `(FindListingRecommendationRequest, Action<RequestConfiguration<FindRequestBuilder.FindRequestBuilderPostQueryParameters>>?, string?, RequestOptions?, CancellationToken)` | `(string, FindListingRecommendationRequest, string?, int?, int?, string?, RequestOptions?, CancellationToken)` |

### Added

- `MarketingClient.BulkUpsertVideoAdsPreferencesAsync(string, BulkUpsertVideoAdsPreferencesRequest, string?, RequestOptions?, CancellationToken)`
- `MarketingClient.UpsertVideoAdsPreferencesAsync(string, UpsertVideoAdsPreferencesRequest, string?, RequestOptions?, CancellationToken)`
