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


using System;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using EBay.MarketingAndPromotions.Configuration;
using EBay.MarketingAndPromotions.Exceptions;
using EBay.MarketingAndPromotions.Generated.Marketing;
using EBay.MarketingAndPromotions.Generated.Marketing.Ad_campaign;
using EBay.MarketingAndPromotions.Generated.Marketing.Ad_campaign.Find_campaign_by_ad_reference;
using EBay.MarketingAndPromotions.Generated.Marketing.Ad_campaign.Get_campaign_by_name;
using EBay.MarketingAndPromotions.Generated.Marketing.Ad_campaign.Item.Ad;
using EBay.MarketingAndPromotions.Generated.Marketing.Ad_campaign.Item.Ad_group;
using EBay.MarketingAndPromotions.Generated.Marketing.Ad_campaign.Item.Clone;
using EBay.MarketingAndPromotions.Generated.Marketing.Ad_campaign.Item.Get_ads_by_inventory_reference;
using EBay.MarketingAndPromotions.Generated.Marketing.Ad_campaign.Item.Keyword;
using EBay.MarketingAndPromotions.Generated.Marketing.Ad_campaign.Item.Suggest_items;
using EBay.MarketingAndPromotions.Generated.Marketing.Ad_campaign.Item.Update_campaign_budget;
using EBay.MarketingAndPromotions.Generated.Marketing.Ad_report_metadata;
using EBay.MarketingAndPromotions.Generated.Marketing.Ad_report_metadata.Item;
using EBay.MarketingAndPromotions.Generated.Marketing.Ad_report_task;
using EBay.MarketingAndPromotions.Generated.Marketing.Email_campaign;
using EBay.MarketingAndPromotions.Generated.Marketing.Email_campaign.Audience;
using EBay.MarketingAndPromotions.Generated.Marketing.Email_campaign.Report;
using EBay.MarketingAndPromotions.Generated.Marketing.Item_price_markdown;
using EBay.MarketingAndPromotions.Generated.Marketing.Item_price_markdown.Item;
using EBay.MarketingAndPromotions.Generated.Marketing.Models;
using EBay.MarketingAndPromotions.Generated.Marketing.Negative_keyword;
using EBay.MarketingAndPromotions.Generated.Marketing.Promotion;
using EBay.MarketingAndPromotions.Generated.Marketing.Promotion.Item.Get_listing_set;
using EBay.MarketingAndPromotions.Generated.Marketing.Promotion_report;
using EBay.MarketingAndPromotions.Generated.Marketing.Promotion_summary_report;
using Microsoft.Kiota.Abstractions;

namespace EBay.MarketingAndPromotions.Clients.Rest;

public sealed class MarketingClient
{
    private readonly MarketingApiClient _kiota;
    private readonly MarketingAndPromotionsConfiguration _config;
    private readonly ITokenContext _tokenContext;

    internal MarketingClient(MarketingApiClient kiota, MarketingAndPromotionsConfiguration config, ITokenContext tokenContext)
        => (_kiota, _config, _tokenContext) = (kiota, config, tokenContext);

    private void ApplyHeaders(RequestHeaders headers, RequestOptions? callOptions)
    {
        foreach (var (k, v) in _config.DefaultRequestOptions.Headers)
            headers.TryAdd(k, v);
        if (callOptions is null) return;
        foreach (var (k, v) in callOptions.Headers)
            headers[k] = new[] { v };
    }

    private async Task<T?> ExecuteAsync<T>(Func<Task<T?>> op)
    {
        try { return await op(); }
        catch (ApiException ex) when (ex.ResponseStatusCode == (int)HttpStatusCode.Unauthorized)
        {
            throw new UnauthorizedException(ex);
        }
        catch (ApiException ex)
        {
            throw new SdkException($"REST API error ({ex.ResponseStatusCode})", ex);
        }
    }

    private async Task ExecuteAsync(Func<Task> op)
    {
        try { await op(); }
        catch (ApiException ex) when (ex.ResponseStatusCode == (int)HttpStatusCode.Unauthorized)
        {
            throw new UnauthorizedException(ex);
        }
        catch (ApiException ex)
        {
            throw new SdkException($"REST API error ({ex.ResponseStatusCode})", ex);
        }
    }

    // ── Campaigns ─────────────────────────────────────────────────────────────

    public Task<CampaignPagedCollectionResponse?> GetCampaignsAsync(
        Action<RequestConfiguration<Ad_campaignRequestBuilder.Ad_campaignRequestBuilderGetQueryParameters>>? configure = null,
        string? accessToken = null,
        RequestOptions? requestOptions = null,
        CancellationToken ct = default)
        => ExecuteAsync(() => _tokenContext.RunAsync(accessToken, () => _kiota.Ad_campaign.GetAsync(cfg =>
        {
            configure?.Invoke(cfg);
            ApplyHeaders(cfg.Headers, requestOptions);
        }, ct)));

    public Task<Campaign?> GetCampaignAsync(string campaignId, string? accessToken = null, RequestOptions? requestOptions = null, CancellationToken ct = default)
        => ExecuteAsync(() => _tokenContext.RunAsync(accessToken, () => _kiota.Ad_campaign[campaignId].GetAsync(cfg => ApplyHeaders(cfg.Headers, requestOptions), ct)));

    public Task<Ad_campaignPostResponse?> CreateCampaignAsync(CreateCampaignRequest body, string? accessToken = null, RequestOptions? requestOptions = null, CancellationToken ct = default)
        => ExecuteAsync(() => _tokenContext.RunAsync(accessToken, () => _kiota.Ad_campaign.PostAsync(body, cfg => ApplyHeaders(cfg.Headers, requestOptions), ct)));

    public Task DeleteCampaignAsync(string campaignId, string? accessToken = null, RequestOptions? requestOptions = null, CancellationToken ct = default)
        => ExecuteAsync(() => _tokenContext.RunAsync(accessToken, () => _kiota.Ad_campaign[campaignId].DeleteAsync(cfg => ApplyHeaders(cfg.Headers, requestOptions), ct)));

    public Task EndCampaignAsync(string campaignId, string? accessToken = null, RequestOptions? requestOptions = null, CancellationToken ct = default)
        => ExecuteAsync(() => _tokenContext.RunAsync(accessToken, () => _kiota.Ad_campaign[campaignId].End.PostAsync(cfg => ApplyHeaders(cfg.Headers, requestOptions), ct)));

    public Task PauseCampaignAsync(string campaignId, string? accessToken = null, RequestOptions? requestOptions = null, CancellationToken ct = default)
        => ExecuteAsync(() => _tokenContext.RunAsync(accessToken, () => _kiota.Ad_campaign[campaignId].Pause.PostAsync(cfg => ApplyHeaders(cfg.Headers, requestOptions), ct)));

    public Task ResumeCampaignAsync(string campaignId, string? accessToken = null, RequestOptions? requestOptions = null, CancellationToken ct = default)
        => ExecuteAsync(() => _tokenContext.RunAsync(accessToken, () => _kiota.Ad_campaign[campaignId].Resume.PostAsync(cfg => ApplyHeaders(cfg.Headers, requestOptions), ct)));

    public Task<ClonePostResponse?> CloneCampaignAsync(string campaignId, CloneCampaignRequest body, string? accessToken = null, RequestOptions? requestOptions = null, CancellationToken ct = default)
        => ExecuteAsync(() => _tokenContext.RunAsync(accessToken, () => _kiota.Ad_campaign[campaignId].Clone.PostAsync(body, cfg => ApplyHeaders(cfg.Headers, requestOptions), ct)));

    public Task<Update_campaign_budgetPostResponse?> UpdateCampaignBudgetAsync(string campaignId, UpdateCampaignBudgetRequest body, string? accessToken = null, RequestOptions? requestOptions = null, CancellationToken ct = default)
        => ExecuteAsync(() => _tokenContext.RunAsync(accessToken, () => _kiota.Ad_campaign[campaignId].Update_campaign_budget.PostAsync(body, cfg => ApplyHeaders(cfg.Headers, requestOptions), ct)));

    public Task UpdateCampaignIdentificationAsync(string campaignId, UpdateCampaignIdentificationRequest body, string? accessToken = null, RequestOptions? requestOptions = null, CancellationToken ct = default)
        => ExecuteAsync(() => _tokenContext.RunAsync(accessToken, () => _kiota.Ad_campaign[campaignId].Update_campaign_identification.PostAsync(body, cfg => ApplyHeaders(cfg.Headers, requestOptions), ct)));

    public Task UpdateBiddingStrategyAsync(string campaignId, UpdateBiddingStrategyRequest body, string? accessToken = null, RequestOptions? requestOptions = null, CancellationToken ct = default)
        => ExecuteAsync(() => _tokenContext.RunAsync(accessToken, () => _kiota.Ad_campaign[campaignId].Update_bidding_strategy.PostAsync(body, cfg => ApplyHeaders(cfg.Headers, requestOptions), ct)));

    public Task UpdateAdRateStrategyAsync(string campaignId, UpdateAdrateStrategyRequest body, string? accessToken = null, RequestOptions? requestOptions = null, CancellationToken ct = default)
        => ExecuteAsync(() => _tokenContext.RunAsync(accessToken, () => _kiota.Ad_campaign[campaignId].Update_ad_rate_strategy.PostAsync(body, cfg => ApplyHeaders(cfg.Headers, requestOptions), ct)));

    public Task<SuggestBudgetResponse?> SuggestBudgetAsync(string? accessToken = null, RequestOptions? requestOptions = null, CancellationToken ct = default)
        => ExecuteAsync(() => _tokenContext.RunAsync(accessToken, () => _kiota.Ad_campaign.Suggest_budget.GetAsync(cfg => ApplyHeaders(cfg.Headers, requestOptions), ct)));

    public Task<SuggestMaxCpcResponse?> SuggestMaxCpcAsync(SuggestMaxCpcRequest body, string? accessToken = null, RequestOptions? requestOptions = null, CancellationToken ct = default)
        => ExecuteAsync(() => _tokenContext.RunAsync(accessToken, () => _kiota.Ad_campaign.Suggest_max_cpc.PostAsync(body, cfg => ApplyHeaders(cfg.Headers, requestOptions), ct)));

    public Task<Campaigns?> FindCampaignByAdReferenceAsync(
        Action<RequestConfiguration<Find_campaign_by_ad_referenceRequestBuilder.Find_campaign_by_ad_referenceRequestBuilderGetQueryParameters>>? configure = null,
        string? accessToken = null,
        RequestOptions? requestOptions = null,
        CancellationToken ct = default)
        => ExecuteAsync(() => _tokenContext.RunAsync(accessToken, () => _kiota.Ad_campaign.Find_campaign_by_ad_reference.GetAsync(cfg =>
        {
            configure?.Invoke(cfg);
            ApplyHeaders(cfg.Headers, requestOptions);
        }, ct)));

    public Task<Campaign?> GetCampaignByNameAsync(
        Action<RequestConfiguration<Get_campaign_by_nameRequestBuilder.Get_campaign_by_nameRequestBuilderGetQueryParameters>>? configure = null,
        string? accessToken = null,
        RequestOptions? requestOptions = null,
        CancellationToken ct = default)
        => ExecuteAsync(() => _tokenContext.RunAsync(accessToken, () => _kiota.Ad_campaign.Get_campaign_by_name.GetAsync(cfg =>
        {
            configure?.Invoke(cfg);
            ApplyHeaders(cfg.Headers, requestOptions);
        }, ct)));

    // ── Ad Groups ─────────────────────────────────────────────────────────────

    public Task<AdGroupPagedCollectionResponse?> GetAdGroupsAsync(
        string campaignId,
        Action<RequestConfiguration<Ad_groupRequestBuilder.Ad_groupRequestBuilderGetQueryParameters>>? configure = null,
        string? accessToken = null,
        RequestOptions? requestOptions = null,
        CancellationToken ct = default)
        => ExecuteAsync(() => _tokenContext.RunAsync(accessToken, () => _kiota.Ad_campaign[campaignId].Ad_group.GetAsync(cfg =>
        {
            configure?.Invoke(cfg);
            ApplyHeaders(cfg.Headers, requestOptions);
        }, ct)));

    public Task<AdGroup?> GetAdGroupAsync(string campaignId, string adGroupId, string? accessToken = null, RequestOptions? requestOptions = null, CancellationToken ct = default)
        => ExecuteAsync(() => _tokenContext.RunAsync(accessToken, () => _kiota.Ad_campaign[campaignId].Ad_group[adGroupId].GetAsync(cfg => ApplyHeaders(cfg.Headers, requestOptions), ct)));

    public Task<Ad_groupPostResponse?> CreateAdGroupAsync(string campaignId, CreateAdGroupRequest body, string? accessToken = null, RequestOptions? requestOptions = null, CancellationToken ct = default)
        => ExecuteAsync(() => _tokenContext.RunAsync(accessToken, () => _kiota.Ad_campaign[campaignId].Ad_group.PostAsync(body, cfg => ApplyHeaders(cfg.Headers, requestOptions), ct)));

    public Task UpdateAdGroupAsync(string campaignId, string adGroupId, UpdateAdGroupRequest body, string? accessToken = null, RequestOptions? requestOptions = null, CancellationToken ct = default)
        => ExecuteAsync(() => _tokenContext.RunAsync(accessToken, () => _kiota.Ad_campaign[campaignId].Ad_group[adGroupId].PutAsync(body, cfg => ApplyHeaders(cfg.Headers, requestOptions), ct)));

    public Task<TargetedBidsPagedCollection?> SuggestBidsAsync(string campaignId, string adGroupId, TargetedBidRequest body, string? accessToken = null, RequestOptions? requestOptions = null, CancellationToken ct = default)
        => ExecuteAsync(() => _tokenContext.RunAsync(accessToken, () => _kiota.Ad_campaign[campaignId].Ad_group[adGroupId].Suggest_bids.PostAsync(body, cfg => ApplyHeaders(cfg.Headers, requestOptions), ct)));

    public Task<TargetedKeywordsPagedCollection?> SuggestKeywordsAsync(string campaignId, string adGroupId, TargetedKeywordRequest body, string? accessToken = null, RequestOptions? requestOptions = null, CancellationToken ct = default)
        => ExecuteAsync(() => _tokenContext.RunAsync(accessToken, () => _kiota.Ad_campaign[campaignId].Ad_group[adGroupId].Suggest_keywords.PostAsync(body, cfg => ApplyHeaders(cfg.Headers, requestOptions), ct)));

    // ── Ads ───────────────────────────────────────────────────────────────────

    public Task<AdPagedCollectionResponse?> GetAdsAsync(
        string campaignId,
        Action<RequestConfiguration<AdRequestBuilder.AdRequestBuilderGetQueryParameters>>? configure = null,
        string? accessToken = null,
        RequestOptions? requestOptions = null,
        CancellationToken ct = default)
        => ExecuteAsync(() => _tokenContext.RunAsync(accessToken, () => _kiota.Ad_campaign[campaignId].Ad.GetAsync(cfg =>
        {
            configure?.Invoke(cfg);
            ApplyHeaders(cfg.Headers, requestOptions);
        }, ct)));

    public Task<Generated.Marketing.Models.Ad?> GetAdAsync(string campaignId, string adId, string? accessToken = null, RequestOptions? requestOptions = null, CancellationToken ct = default)
        => ExecuteAsync(() => _tokenContext.RunAsync(accessToken, () => _kiota.Ad_campaign[campaignId].Ad[adId].GetAsync(cfg => ApplyHeaders(cfg.Headers, requestOptions), ct)));

    public Task<AdPostResponse?> CreateAdAsync(string campaignId, CreateAdRequest body, string? accessToken = null, RequestOptions? requestOptions = null, CancellationToken ct = default)
        => ExecuteAsync(() => _tokenContext.RunAsync(accessToken, () => _kiota.Ad_campaign[campaignId].Ad.PostAsync(body, cfg => ApplyHeaders(cfg.Headers, requestOptions), ct)));

    public Task DeleteAdAsync(string campaignId, string adId, string? accessToken = null, RequestOptions? requestOptions = null, CancellationToken ct = default)
        => ExecuteAsync(() => _tokenContext.RunAsync(accessToken, () => _kiota.Ad_campaign[campaignId].Ad[adId].DeleteAsync(cfg => ApplyHeaders(cfg.Headers, requestOptions), ct)));

    public Task UpdateAdBidAsync(string campaignId, string adId, UpdateBidPercentageRequest body, string? accessToken = null, RequestOptions? requestOptions = null, CancellationToken ct = default)
        => ExecuteAsync(() => _tokenContext.RunAsync(accessToken, () => _kiota.Ad_campaign[campaignId].Ad[adId].Update_bid.PostAsync(body, cfg => ApplyHeaders(cfg.Headers, requestOptions), ct)));

    public Task<AdReferences?> CreateAdsByInventoryReferenceAsync(string campaignId, CreateAdsByInventoryReferenceRequest body, string? accessToken = null, RequestOptions? requestOptions = null, CancellationToken ct = default)
        => ExecuteAsync(() => _tokenContext.RunAsync(accessToken, () => _kiota.Ad_campaign[campaignId].Create_ads_by_inventory_reference.PostAsync(body, cfg => ApplyHeaders(cfg.Headers, requestOptions), ct)));

    public Task<Ads?> GetAdsByInventoryReferenceAsync(
        string campaignId,
        Action<RequestConfiguration<Get_ads_by_inventory_referenceRequestBuilder.Get_ads_by_inventory_referenceRequestBuilderGetQueryParameters>>? configure = null,
        string? accessToken = null,
        RequestOptions? requestOptions = null,
        CancellationToken ct = default)
        => ExecuteAsync(() => _tokenContext.RunAsync(accessToken, () => _kiota.Ad_campaign[campaignId].Get_ads_by_inventory_reference.GetAsync(cfg =>
        {
            configure?.Invoke(cfg);
            ApplyHeaders(cfg.Headers, requestOptions);
        }, ct)));

    public Task<AdIds?> DeleteAdsByInventoryReferenceAsync(string campaignId, DeleteAdsByInventoryReferenceRequest body, string? accessToken = null, RequestOptions? requestOptions = null, CancellationToken ct = default)
        => ExecuteAsync(() => _tokenContext.RunAsync(accessToken, () => _kiota.Ad_campaign[campaignId].Delete_ads_by_inventory_reference.PostAsync(body, cfg => ApplyHeaders(cfg.Headers, requestOptions), ct)));

    public Task<TargetedAdsPagedCollection?> GetSuggestedItemsAsync(
        string campaignId,
        Action<RequestConfiguration<Suggest_itemsRequestBuilder.Suggest_itemsRequestBuilderGetQueryParameters>>? configure = null,
        string? accessToken = null,
        RequestOptions? requestOptions = null,
        CancellationToken ct = default)
        => ExecuteAsync(() => _tokenContext.RunAsync(accessToken, () => _kiota.Ad_campaign[campaignId].Suggest_items.GetAsync(cfg =>
        {
            configure?.Invoke(cfg);
            ApplyHeaders(cfg.Headers, requestOptions);
        }, ct)));

    public Task<BulkAdResponse?> BulkCreateAdsByListingIdAsync(string campaignId, BulkCreateAdRequest body, string? accessToken = null, RequestOptions? requestOptions = null, CancellationToken ct = default)
        => ExecuteAsync(() => _tokenContext.RunAsync(accessToken, () => _kiota.Ad_campaign[campaignId].Bulk_create_ads_by_listing_id.PostAsync(body, cfg => ApplyHeaders(cfg.Headers, requestOptions), ct)));

    public Task<BulkCreateAdsByInventoryReferenceResponse?> BulkCreateAdsByInventoryReferenceAsync(string campaignId, BulkCreateAdsByInventoryReferenceRequest body, string? accessToken = null, RequestOptions? requestOptions = null, CancellationToken ct = default)
        => ExecuteAsync(() => _tokenContext.RunAsync(accessToken, () => _kiota.Ad_campaign[campaignId].Bulk_create_ads_by_inventory_reference.PostAsync(body, cfg => ApplyHeaders(cfg.Headers, requestOptions), ct)));

    public Task<BulkDeleteAdResponse?> BulkDeleteAdsByListingIdAsync(string campaignId, BulkDeleteAdRequest body, string? accessToken = null, RequestOptions? requestOptions = null, CancellationToken ct = default)
        => ExecuteAsync(() => _tokenContext.RunAsync(accessToken, () => _kiota.Ad_campaign[campaignId].Bulk_delete_ads_by_listing_id.PostAsync(body, cfg => ApplyHeaders(cfg.Headers, requestOptions), ct)));

    public Task<BulkDeleteAdsByInventoryReferenceResponse?> BulkDeleteAdsByInventoryReferenceAsync(string campaignId, BulkDeleteAdsByInventoryReferenceRequest body, string? accessToken = null, RequestOptions? requestOptions = null, CancellationToken ct = default)
        => ExecuteAsync(() => _tokenContext.RunAsync(accessToken, () => _kiota.Ad_campaign[campaignId].Bulk_delete_ads_by_inventory_reference.PostAsync(body, cfg => ApplyHeaders(cfg.Headers, requestOptions), ct)));

    public Task<BulkAdUpdateResponse?> BulkUpdateAdsBidByListingIdAsync(string campaignId, BulkCreateAdRequest body, string? accessToken = null, RequestOptions? requestOptions = null, CancellationToken ct = default)
        => ExecuteAsync(() => _tokenContext.RunAsync(accessToken, () => _kiota.Ad_campaign[campaignId].Bulk_update_ads_bid_by_listing_id.PostAsync(body, cfg => ApplyHeaders(cfg.Headers, requestOptions), ct)));

    public Task<BulkUpdateAdsByInventoryReferenceResponse?> BulkUpdateAdsBidByInventoryReferenceAsync(string campaignId, BulkCreateAdsByInventoryReferenceRequest body, string? accessToken = null, RequestOptions? requestOptions = null, CancellationToken ct = default)
        => ExecuteAsync(() => _tokenContext.RunAsync(accessToken, () => _kiota.Ad_campaign[campaignId].Bulk_update_ads_bid_by_inventory_reference.PostAsync(body, cfg => ApplyHeaders(cfg.Headers, requestOptions), ct)));

    public Task<BulkAdUpdateStatusResponse?> BulkUpdateAdsStatusAsync(string campaignId, BulkUpdateAdStatusRequest body, string? accessToken = null, RequestOptions? requestOptions = null, CancellationToken ct = default)
        => ExecuteAsync(() => _tokenContext.RunAsync(accessToken, () => _kiota.Ad_campaign[campaignId].Bulk_update_ads_status.PostAsync(body, cfg => ApplyHeaders(cfg.Headers, requestOptions), ct)));

    public Task<BulkAdUpdateStatusByListingIdResponse?> BulkUpdateAdsStatusByListingIdAsync(string campaignId, BulkUpdateAdStatusByListingIdRequest body, string? accessToken = null, RequestOptions? requestOptions = null, CancellationToken ct = default)
        => ExecuteAsync(() => _tokenContext.RunAsync(accessToken, () => _kiota.Ad_campaign[campaignId].Bulk_update_ads_status_by_listing_id.PostAsync(body, cfg => ApplyHeaders(cfg.Headers, requestOptions), ct)));

    // ── Keywords ──────────────────────────────────────────────────────────────

    public Task<KeywordPagedCollectionResponse?> GetKeywordsAsync(
        string campaignId,
        Action<RequestConfiguration<KeywordRequestBuilder.KeywordRequestBuilderGetQueryParameters>>? configure = null,
        string? accessToken = null,
        RequestOptions? requestOptions = null,
        CancellationToken ct = default)
        => ExecuteAsync(() => _tokenContext.RunAsync(accessToken, () => _kiota.Ad_campaign[campaignId].Keyword.GetAsync(cfg =>
        {
            configure?.Invoke(cfg);
            ApplyHeaders(cfg.Headers, requestOptions);
        }, ct)));

    public Task<Keyword?> GetKeywordAsync(string campaignId, string keywordId, string? accessToken = null, RequestOptions? requestOptions = null, CancellationToken ct = default)
        => ExecuteAsync(() => _tokenContext.RunAsync(accessToken, () => _kiota.Ad_campaign[campaignId].Keyword[keywordId].GetAsync(cfg => ApplyHeaders(cfg.Headers, requestOptions), ct)));

    public Task<KeywordPostResponse?> CreateKeywordAsync(string campaignId, CreateKeywordRequest body, string? accessToken = null, RequestOptions? requestOptions = null, CancellationToken ct = default)
        => ExecuteAsync(() => _tokenContext.RunAsync(accessToken, () => _kiota.Ad_campaign[campaignId].Keyword.PostAsync(body, cfg => ApplyHeaders(cfg.Headers, requestOptions), ct)));

    public Task<UpdateKeywordResponse?> UpdateKeywordAsync(string campaignId, string keywordId, UpdateKeywordRequest body, string? accessToken = null, RequestOptions? requestOptions = null, CancellationToken ct = default)
        => ExecuteAsync(() => _tokenContext.RunAsync(accessToken, () => _kiota.Ad_campaign[campaignId].Keyword[keywordId].PutAsync(body, cfg => ApplyHeaders(cfg.Headers, requestOptions), ct)));

    public Task<BulkCreateKeywordResponse?> BulkCreateKeywordsAsync(string campaignId, BulkCreateKeywordRequest body, string? accessToken = null, RequestOptions? requestOptions = null, CancellationToken ct = default)
        => ExecuteAsync(() => _tokenContext.RunAsync(accessToken, () => _kiota.Ad_campaign[campaignId].Bulk_create_keyword.PostAsync(body, cfg => ApplyHeaders(cfg.Headers, requestOptions), ct)));

    public Task<BulkUpdateKeywordResponse?> BulkUpdateKeywordsAsync(string campaignId, BulkUpdateKeywordRequest body, string? accessToken = null, RequestOptions? requestOptions = null, CancellationToken ct = default)
        => ExecuteAsync(() => _tokenContext.RunAsync(accessToken, () => _kiota.Ad_campaign[campaignId].Bulk_update_keyword.PostAsync(body, cfg => ApplyHeaders(cfg.Headers, requestOptions), ct)));

    // ── Negative Keywords ─────────────────────────────────────────────────────

    public Task<NegativeKeywordPagedCollectionResponse?> GetNegativeKeywordsAsync(
        Action<RequestConfiguration<Negative_keywordRequestBuilder.Negative_keywordRequestBuilderGetQueryParameters>>? configure = null,
        string? accessToken = null,
        RequestOptions? requestOptions = null,
        CancellationToken ct = default)
        => ExecuteAsync(() => _tokenContext.RunAsync(accessToken, () => _kiota.Negative_keyword.GetAsync(cfg =>
        {
            configure?.Invoke(cfg);
            ApplyHeaders(cfg.Headers, requestOptions);
        }, ct)));

    public Task<NegativeKeyword?> GetNegativeKeywordAsync(string negativeKeywordId, string? accessToken = null, RequestOptions? requestOptions = null, CancellationToken ct = default)
        => ExecuteAsync(() => _tokenContext.RunAsync(accessToken, () => _kiota.Negative_keyword[negativeKeywordId].GetAsync(cfg => ApplyHeaders(cfg.Headers, requestOptions), ct)));

    public Task<Negative_keywordPostResponse?> CreateNegativeKeywordAsync(CreateNegativeKeywordRequest body, string? accessToken = null, RequestOptions? requestOptions = null, CancellationToken ct = default)
        => ExecuteAsync(() => _tokenContext.RunAsync(accessToken, () => _kiota.Negative_keyword.PostAsync(body, cfg => ApplyHeaders(cfg.Headers, requestOptions), ct)));

    public Task UpdateNegativeKeywordAsync(string negativeKeywordId, UpdateNegativeKeywordRequest body, string? accessToken = null, RequestOptions? requestOptions = null, CancellationToken ct = default)
        => ExecuteAsync(() => _tokenContext.RunAsync(accessToken, () => _kiota.Negative_keyword[negativeKeywordId].PutAsync(body, cfg => ApplyHeaders(cfg.Headers, requestOptions), ct)));

    public Task<BulkCreateNegativeKeywordResponse?> BulkCreateNegativeKeywordsAsync(BulkCreateNegativeKeywordRequest body, string? accessToken = null, RequestOptions? requestOptions = null, CancellationToken ct = default)
        => ExecuteAsync(() => _tokenContext.RunAsync(accessToken, () => _kiota.Bulk_create_negative_keyword.PostAsync(body, cfg => ApplyHeaders(cfg.Headers, requestOptions), ct)));

    public Task<BulkUpdateNegativeKeywordResponse?> BulkUpdateNegativeKeywordsAsync(BulkUpdateNegativeKeywordRequest body, string? accessToken = null, RequestOptions? requestOptions = null, CancellationToken ct = default)
        => ExecuteAsync(() => _tokenContext.RunAsync(accessToken, () => _kiota.Bulk_update_negative_keyword.PostAsync(body, cfg => ApplyHeaders(cfg.Headers, requestOptions), ct)));

    // ── Item Promotions ───────────────────────────────────────────────────────

    public Task<BaseResponse?> CreateItemPromotionAsync(ItemPromotion body, string? accessToken = null, RequestOptions? requestOptions = null, CancellationToken ct = default)
        => ExecuteAsync(() => _tokenContext.RunAsync(accessToken, () => _kiota.Item_promotion.PostAsync(body, cfg => ApplyHeaders(cfg.Headers, requestOptions), ct)));

    public Task<ItemPromotionResponse?> GetItemPromotionAsync(string promotionId, string? accessToken = null, RequestOptions? requestOptions = null, CancellationToken ct = default)
        => ExecuteAsync(() => _tokenContext.RunAsync(accessToken, () => _kiota.Item_promotion[promotionId].GetAsync(cfg => ApplyHeaders(cfg.Headers, requestOptions), ct)));

    public Task<BaseResponse?> UpdateItemPromotionAsync(string promotionId, ItemPromotion body, string? accessToken = null, RequestOptions? requestOptions = null, CancellationToken ct = default)
        => ExecuteAsync(() => _tokenContext.RunAsync(accessToken, () => _kiota.Item_promotion[promotionId].PutAsync(body, cfg => ApplyHeaders(cfg.Headers, requestOptions), ct)));

    public Task DeleteItemPromotionAsync(string promotionId, string? accessToken = null, RequestOptions? requestOptions = null, CancellationToken ct = default)
        => ExecuteAsync(() => _tokenContext.RunAsync(accessToken, () => _kiota.Item_promotion[promotionId].DeleteAsync(cfg => ApplyHeaders(cfg.Headers, requestOptions), ct)));

    // ── Item Price Markdown ───────────────────────────────────────────────────

    public Task<Item_price_markdownPostResponse?> CreateItemPriceMarkdownAsync(ItemPriceMarkdown body, string? accessToken = null, RequestOptions? requestOptions = null, CancellationToken ct = default)
        => ExecuteAsync(() => _tokenContext.RunAsync(accessToken, () => _kiota.Item_price_markdown.PostAsync(body, cfg => ApplyHeaders(cfg.Headers, requestOptions), ct)));

    public Task<ItemPriceMarkdown?> GetItemPriceMarkdownAsync(string promotionId, string? accessToken = null, RequestOptions? requestOptions = null, CancellationToken ct = default)
        => ExecuteAsync(() => _tokenContext.RunAsync(accessToken, () => _kiota.Item_price_markdown[promotionId].GetAsync(cfg => ApplyHeaders(cfg.Headers, requestOptions), ct)));

    public Task<WithPromotion_PutResponse?> UpdateItemPriceMarkdownAsync(string promotionId, ItemPriceMarkdown body, string? accessToken = null, RequestOptions? requestOptions = null, CancellationToken ct = default)
        => ExecuteAsync(() => _tokenContext.RunAsync(accessToken, () => _kiota.Item_price_markdown[promotionId].PutAsync(body, cfg => ApplyHeaders(cfg.Headers, requestOptions), ct)));

    public Task DeleteItemPriceMarkdownAsync(string promotionId, string? accessToken = null, RequestOptions? requestOptions = null, CancellationToken ct = default)
        => ExecuteAsync(() => _tokenContext.RunAsync(accessToken, () => _kiota.Item_price_markdown[promotionId].DeleteAsync(cfg => ApplyHeaders(cfg.Headers, requestOptions), ct)));

    // ── Promotions ────────────────────────────────────────────────────────────

    public Task<PromotionsPagedCollection?> GetPromotionsAsync(
        Action<RequestConfiguration<PromotionRequestBuilder.PromotionRequestBuilderGetQueryParameters>>? configure = null,
        string? accessToken = null,
        RequestOptions? requestOptions = null,
        CancellationToken ct = default)
        => ExecuteAsync(() => _tokenContext.RunAsync(accessToken, () => _kiota.Promotion.GetAsync(cfg =>
        {
            configure?.Invoke(cfg);
            ApplyHeaders(cfg.Headers, requestOptions);
        }, ct)));

    public Task<ItemsPagedCollection?> GetPromotionListingSetAsync(
        string promotionId,
        Action<RequestConfiguration<Get_listing_setRequestBuilder.Get_listing_setRequestBuilderGetQueryParameters>>? configure = null,
        string? accessToken = null,
        RequestOptions? requestOptions = null,
        CancellationToken ct = default)
        => ExecuteAsync(() => _tokenContext.RunAsync(accessToken, () => _kiota.Promotion[promotionId].Get_listing_set.GetAsync(cfg =>
        {
            configure?.Invoke(cfg);
            ApplyHeaders(cfg.Headers, requestOptions);
        }, ct)));

    public Task PausePromotionAsync(string promotionId, string? accessToken = null, RequestOptions? requestOptions = null, CancellationToken ct = default)
        => ExecuteAsync(() => _tokenContext.RunAsync(accessToken, () => _kiota.Promotion[promotionId].Pause.PostAsync(cfg => ApplyHeaders(cfg.Headers, requestOptions), ct)));

    public Task ResumePromotionAsync(string promotionId, string? accessToken = null, RequestOptions? requestOptions = null, CancellationToken ct = default)
        => ExecuteAsync(() => _tokenContext.RunAsync(accessToken, () => _kiota.Promotion[promotionId].Resume.PostAsync(cfg => ApplyHeaders(cfg.Headers, requestOptions), ct)));

    public Task<PromotionsReportPagedCollection?> GetPromotionReportsAsync(
        Action<RequestConfiguration<Promotion_reportRequestBuilder.Promotion_reportRequestBuilderGetQueryParameters>>? configure = null,
        string? accessToken = null,
        RequestOptions? requestOptions = null,
        CancellationToken ct = default)
        => ExecuteAsync(() => _tokenContext.RunAsync(accessToken, () => _kiota.Promotion_report.GetAsync(cfg =>
        {
            configure?.Invoke(cfg);
            ApplyHeaders(cfg.Headers, requestOptions);
        }, ct)));

    public Task<SummaryReportResponse?> GetPromotionSummaryReportAsync(
        Action<RequestConfiguration<Promotion_summary_reportRequestBuilder.Promotion_summary_reportRequestBuilderGetQueryParameters>>? configure = null,
        string? accessToken = null,
        RequestOptions? requestOptions = null,
        CancellationToken ct = default)
        => ExecuteAsync(() => _tokenContext.RunAsync(accessToken, () => _kiota.Promotion_summary_report.GetAsync(cfg =>
        {
            configure?.Invoke(cfg);
            ApplyHeaders(cfg.Headers, requestOptions);
        }, ct)));

    // ── Email Campaigns ───────────────────────────────────────────────────────

    public Task<GetEmailCampaignsResponse?> GetEmailCampaignsAsync(
        Action<RequestConfiguration<Email_campaignRequestBuilder.Email_campaignRequestBuilderGetQueryParameters>>? configure = null,
        string? accessToken = null,
        RequestOptions? requestOptions = null,
        CancellationToken ct = default)
        => ExecuteAsync(() => _tokenContext.RunAsync(accessToken, () => _kiota.Email_campaign.GetAsync(cfg =>
        {
            configure?.Invoke(cfg);
            ApplyHeaders(cfg.Headers, requestOptions);
        }, ct)));

    public Task<GetEmailCampaignResponse?> GetEmailCampaignAsync(string emailCampaignId, string? accessToken = null, RequestOptions? requestOptions = null, CancellationToken ct = default)
        => ExecuteAsync(() => _tokenContext.RunAsync(accessToken, () => _kiota.Email_campaign[emailCampaignId].GetAsync(cfg => ApplyHeaders(cfg.Headers, requestOptions), ct)));

    public Task<CreateEmailCampaignResponse?> CreateEmailCampaignAsync(CreateEmailCampaignRequest body, string? accessToken = null, RequestOptions? requestOptions = null, CancellationToken ct = default)
        => ExecuteAsync(() => _tokenContext.RunAsync(accessToken, () => _kiota.Email_campaign.PostAsync(body, cfg => ApplyHeaders(cfg.Headers, requestOptions), ct)));

    public Task<UpdateEmailCampaignResponse?> UpdateEmailCampaignAsync(string emailCampaignId, UpdateCampaignRequest body, string? accessToken = null, RequestOptions? requestOptions = null, CancellationToken ct = default)
        => ExecuteAsync(() => _tokenContext.RunAsync(accessToken, () => _kiota.Email_campaign[emailCampaignId].PutAsync(body, cfg => ApplyHeaders(cfg.Headers, requestOptions), ct)));

    public Task<DeleteEmailCampaignResponse?> DeleteEmailCampaignAsync(string emailCampaignId, string? accessToken = null, RequestOptions? requestOptions = null, CancellationToken ct = default)
        => ExecuteAsync(() => _tokenContext.RunAsync(accessToken, () => _kiota.Email_campaign[emailCampaignId].DeleteAsync(cfg => ApplyHeaders(cfg.Headers, requestOptions), ct)));

    public Task<GetEmailCampaignAudiencesResponse?> GetEmailCampaignAudiencesAsync(
        Action<RequestConfiguration<AudienceRequestBuilder.AudienceRequestBuilderGetQueryParameters>>? configure = null,
        string? accessToken = null,
        RequestOptions? requestOptions = null,
        CancellationToken ct = default)
        => ExecuteAsync(() => _tokenContext.RunAsync(accessToken, () => _kiota.Email_campaign.Audience.GetAsync(cfg =>
        {
            configure?.Invoke(cfg);
            ApplyHeaders(cfg.Headers, requestOptions);
        }, ct)));

    public Task<GetEmailReportResponse?> GetEmailReportAsync(
        Action<RequestConfiguration<ReportRequestBuilder.ReportRequestBuilderGetQueryParameters>>? configure = null,
        string? accessToken = null,
        RequestOptions? requestOptions = null,
        CancellationToken ct = default)
        => ExecuteAsync(() => _tokenContext.RunAsync(accessToken, () => _kiota.Email_campaign.Report.GetAsync(cfg =>
        {
            configure?.Invoke(cfg);
            ApplyHeaders(cfg.Headers, requestOptions);
        }, ct)));

    public Task<GetEmailPreviewResponse?> GetEmailPreviewAsync(string emailCampaignId, string? accessToken = null, RequestOptions? requestOptions = null, CancellationToken ct = default)
        => ExecuteAsync(() => _tokenContext.RunAsync(accessToken, () => _kiota.Email_campaign[emailCampaignId].Email_preview.GetAsync(cfg => ApplyHeaders(cfg.Headers, requestOptions), ct)));

    // ── Ad Reports ────────────────────────────────────────────────────────────

    public Task<Stream?> DownloadReportAsync(string reportId, string? accessToken = null, RequestOptions? requestOptions = null, CancellationToken ct = default)
        => ExecuteAsync(() => _tokenContext.RunAsync(accessToken, () => _kiota.Ad_report[reportId].GetAsync(cfg => ApplyHeaders(cfg.Headers, requestOptions), ct)));

    public Task<ReportMetadatas?> GetReportMetadatasAsync(
        Action<RequestConfiguration<Ad_report_metadataRequestBuilder.Ad_report_metadataRequestBuilderGetQueryParameters>>? configure = null,
        string? accessToken = null,
        RequestOptions? requestOptions = null,
        CancellationToken ct = default)
        => ExecuteAsync(() => _tokenContext.RunAsync(accessToken, () => _kiota.Ad_report_metadata.GetAsync(cfg =>
        {
            configure?.Invoke(cfg);
            ApplyHeaders(cfg.Headers, requestOptions);
        }, ct)));

    public Task<ReportMetadata?> GetReportMetadataAsync(
        string reportType,
        Action<RequestConfiguration<WithReport_typeItemRequestBuilder.WithReport_typeItemRequestBuilderGetQueryParameters>>? configure = null,
        string? accessToken = null,
        RequestOptions? requestOptions = null,
        CancellationToken ct = default)
        => ExecuteAsync(() => _tokenContext.RunAsync(accessToken, () => _kiota.Ad_report_metadata[reportType].GetAsync(cfg =>
        {
            configure?.Invoke(cfg);
            ApplyHeaders(cfg.Headers, requestOptions);
        }, ct)));

    // ── Ad Report Tasks ───────────────────────────────────────────────────────

    public Task CreateReportTaskAsync(CreateReportTask body, string? accessToken = null, RequestOptions? requestOptions = null, CancellationToken ct = default)
        => ExecuteAsync(() => _tokenContext.RunAsync(accessToken, () => _kiota.Ad_report_task.PostAsync(body, cfg => ApplyHeaders(cfg.Headers, requestOptions), ct)));

    public Task<ReportTaskPagedCollection?> GetReportTasksAsync(
        Action<RequestConfiguration<Ad_report_taskRequestBuilder.Ad_report_taskRequestBuilderGetQueryParameters>>? configure = null,
        string? accessToken = null,
        RequestOptions? requestOptions = null,
        CancellationToken ct = default)
        => ExecuteAsync(() => _tokenContext.RunAsync(accessToken, () => _kiota.Ad_report_task.GetAsync(cfg =>
        {
            configure?.Invoke(cfg);
            ApplyHeaders(cfg.Headers, requestOptions);
        }, ct)));

    public Task<ReportTask?> GetReportTaskAsync(string reportTaskId, string? accessToken = null, RequestOptions? requestOptions = null, CancellationToken ct = default)
        => ExecuteAsync(() => _tokenContext.RunAsync(accessToken, () => _kiota.Ad_report_task[reportTaskId].GetAsync(cfg => ApplyHeaders(cfg.Headers, requestOptions), ct)));

    public Task DeleteReportTaskAsync(string reportTaskId, string? accessToken = null, RequestOptions? requestOptions = null, CancellationToken ct = default)
        => ExecuteAsync(() => _tokenContext.RunAsync(accessToken, () => _kiota.Ad_report_task[reportTaskId].DeleteAsync(cfg => ApplyHeaders(cfg.Headers, requestOptions), ct)));
}
