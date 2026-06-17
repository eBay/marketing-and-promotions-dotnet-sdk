# Marketing and Promotions .NET SDK

A production-ready .NET SDK for eBay Marketing and Recommendation APIs, providing unified access to advertising campaigns, promotions, and listing recommendations.

## Features

- **Marketing API**: Complete advertising campaign management (81+ operations)
  - Ad campaigns and ad management
  - Keywords and negative keywords
  - Promotions and price markdowns
  - Email campaigns
  - Reporting and analytics
- **Recommendation API**: Listing promotion recommendations (1 operation)
- **OAuth Authentication**: Token refresh handler interface for seamless authentication
- **Dependency Injection**: First-class support for ASP.NET Core DI
- **Strong Typing**: Internal generated types with public abstraction layer

## Architecture

```
EBay.MarketingAndPromotions/
├── Generated/                    # Kiota-generated REST clients (internal)
│   ├── Marketing/
│   └── Recommendation/
├── Clients/
│   ├── Rest/
│   │   ├── MarketingClient.cs   # REST client wrappers (public API)
│   │   └── RecommendationClient.cs
├── Configuration/                # SDK configuration and auth
├── Exceptions/                   # SDK exceptions
└── MarketingAndPromotionsClient.cs  # Main SDK facade
```

## Installation

```bash
# From source
dotnet pack EBay.MarketingAndPromotions/EBay.MarketingAndPromotions.csproj
dotnet add reference path/to/EBay.MarketingAndPromotions.1.0.0.nupkg

# Or add project reference
dotnet add reference EBay.MarketingAndPromotions/EBay.MarketingAndPromotions.csproj
```

## Quick Start

### 1. Implement Token Refresh Handler

```csharp
using MarketingAndPromotions.Configuration;

public class MyTokenRefreshHandler : ITokenRefreshHandler
{
    private readonly string _clientId;
    private readonly string _clientSecret;
    private string? _accessToken;
    private DateTime _tokenExpiry;

    public MyTokenRefreshHandler(string clientId, string clientSecret)
    {
        _clientId = clientId;
        _clientSecret = clientSecret;
    }

    public async Task<string> GetAccessTokenAsync(CancellationToken cancellationToken = default)
    {
        if (_accessToken != null && DateTime.UtcNow < _tokenExpiry.AddMinutes(-5))
        {
            return _accessToken;
        }

        // Refresh token using eBay OAuth API
        using var client = new HttpClient();
        var request = new HttpRequestMessage(HttpMethod.Post, "https://api.ebay.com/identity/v1/oauth2/token");
        request.Headers.Authorization = new AuthenticationHeaderValue(
            "Basic",
            Convert.ToBase64String(Encoding.UTF8.GetBytes($"{_clientId}:{_clientSecret}")));
        
        request.Content = new FormUrlEncodedContent(new[]
        {
            new KeyValuePair<string, string>("grant_type", "client_credentials"),
            new KeyValuePair<string, string>("scope", "https://api.ebay.com/oauth/api_scope https://api.ebay.com/oauth/api_scope/sell.marketing")
        });

        var response = await client.SendAsync(request, cancellationToken);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync(cancellationToken);
        var tokenResponse = JsonSerializer.Deserialize<TokenResponse>(json);

        _accessToken = tokenResponse.AccessToken;
        _tokenExpiry = DateTime.UtcNow.AddSeconds(tokenResponse.ExpiresIn);

        return _accessToken;
    }
}
```

### 2. Configure and Use the SDK

```csharp
using EBay.MarketingAndPromotions;
using EBay.MarketingAndPromotions.Configuration;

// Create configuration
var config = new MarketingAndPromotionsConfiguration
{
    TokenRefreshHandler = new MyTokenRefreshHandler(clientId, secret),
    MarketingBaseUrl = "https://api.ebay.com/sell/marketing/v1",
    RecommendationBaseUrl = "https://api.ebay.com/sell/recommendation/v1"
};

// Create SDK client
using var client = new MarketingAndPromotionsClient(config);

// Campaign management
var campaign = await client.Marketing.CreateCampaignAsync(campaignJson);
var campaigns = await client.Marketing.GetCampaignsAsync();
await client.Marketing.EndCampaignAsync(campaignId);

// Ad management
var ad = await client.Marketing.CreateAdByListingIdAsync(campaignId, adJson);
var ads = await client.Marketing.GetAdsAsync(campaignId);
await client.Marketing.DeleteAdAsync(campaignId, adId);

// Keyword management
var keyword = await client.Marketing.CreateKeywordAsync(campaignId, keywordJson);
var keywords = await client.Marketing.GetKeywordsAsync(campaignId);

// Promotions
var promotion = await client.Marketing.CreateItemPromotionAsync(promotionJson);
var markdown = await client.Marketing.CreateItemPriceMarkdownPromotionAsync(markdownJson);

// Email campaigns
var emailCampaign = await client.Marketing.CreateEmailCampaignAsync(emailCampaignJson);
var emailCampaigns = await client.Marketing.GetEmailCampaignsAsync();

// Recommendations
var recommendations = await client.Recommendation.FindListingRecommendationsAsync();
```

### 3. Dependency Injection (ASP.NET Core)

```csharp
using MarketingAndPromotions.Extensions;
using MarketingAndPromotions.Configuration;

// In Program.cs or Startup.cs
builder.Services.AddSingleton<ITokenRefreshHandler, MyTokenRefreshHandler>();

builder.Services.AddMarketingAndPromotionsSdk(config =>
{
    config.TokenRefreshHandler = sp.GetRequiredService<ITokenRefreshHandler>();
});

// Inject the SDK or sub-clients
public class MarketingController : ControllerBase
{
    private readonly MarketingAndPromotionsClient _sdk;
    private readonly MarketingClient _marketing;
    private readonly RecommendationClient _recommendation;

    public MarketingController(
        MarketingAndPromotionsClient sdk,
        MarketingClient marketing,
        RecommendationClient recommendation)
    {
        _sdk = sdk;
        _marketing = marketing;
        _recommendation = recommendation;
    }

    [HttpGet("campaigns")]
    public async Task<IActionResult> GetCampaigns()
    {
        var campaigns = await _marketing.GetCampaignsAsync();
        return Ok(campaigns);
    }

    [HttpPost("campaigns")]
    public async Task<IActionResult> CreateCampaign([FromBody] string campaignJson)
    {
        var campaign = await _marketing.CreateCampaignAsync(campaignJson);
        return Ok(campaign);
    }

    [HttpGet("recommendations")]
    public async Task<IActionResult> GetRecommendations()
    {
        var recommendations = await _recommendation.FindListingRecommendationsAsync();
        return Ok(recommendations);
    }
}
```

## API Operations

### Marketing API

#### Campaign Operations
```csharp
// Create campaign
var campaign = await client.Marketing.CreateCampaignAsync(campaignJson);

// Get campaigns
var campaigns = await client.Marketing.GetCampaignsAsync();
var campaign = await client.Marketing.GetCampaignAsync(campaignId);
var campaign = await client.Marketing.GetCampaignByNameAsync();

// Manage campaigns
await client.Marketing.EndCampaignAsync(campaignId);
await client.Marketing.DeleteCampaignAsync(campaignId);
var clone = await client.Marketing.CloneCampaignAsync(campaignId, cloneRequestJson);

// Find campaign
var campaign = await client.Marketing.FindCampaignByAdReferenceAsync();
```

#### Ad Operations
```csharp
// Create ads
var ad = await client.Marketing.CreateAdByListingIdAsync(campaignId, adJson);
var ads = await client.Marketing.CreateAdsByInventoryReferenceAsync(campaignId, requestJson);

// Bulk create ads
var bulkAds = await client.Marketing.BulkCreateAdsByListingIdAsync(campaignId, requestJson);
var bulkAds = await client.Marketing.BulkCreateAdsByInventoryReferenceAsync(campaignId, requestJson);

// Get ads
var ad = await client.Marketing.GetAdAsync(campaignId, adId);
var ads = await client.Marketing.GetAdsAsync(campaignId);

// Delete ads
await client.Marketing.DeleteAdAsync(campaignId, adId);
```

#### Keyword Operations
```csharp
// Create keywords
var keyword = await client.Marketing.CreateKeywordAsync(campaignId, keywordJson);
var keywords = await client.Marketing.BulkCreateKeywordAsync(campaignId, requestJson);

// Get keywords
var keyword = await client.Marketing.GetKeywordAsync(campaignId, keywordId);
var keywords = await client.Marketing.GetKeywordsAsync(campaignId);
```

#### Promotion Operations
```csharp
// Item promotions
var promotion = await client.Marketing.CreateItemPromotionAsync(promotionJson);
var promotion = await client.Marketing.GetItemPromotionAsync(promotionId);
await client.Marketing.DeleteItemPromotionAsync(promotionId);

// Price markdown promotions
var markdown = await client.Marketing.CreateItemPriceMarkdownPromotionAsync(markdownJson);
var markdown = await client.Marketing.GetItemPriceMarkdownPromotionAsync(promotionId);
await client.Marketing.DeleteItemPriceMarkdownPromotionAsync(promotionId);
```

#### Email Campaign Operations
```csharp
// Create and manage email campaigns
var emailCampaign = await client.Marketing.CreateEmailCampaignAsync(emailCampaignJson);
var emailCampaign = await client.Marketing.GetEmailCampaignAsync(emailCampaignId);
var emailCampaigns = await client.Marketing.GetEmailCampaignsAsync();
await client.Marketing.DeleteEmailCampaignAsync(emailCampaignId);
```

#### Report Operations
```csharp
// Create report task
await client.Marketing.CreateReportTaskAsync(taskJson);

// Get report task
var reportTask = await client.Marketing.GetReportTaskAsync(reportTaskId);

// Delete report task
await client.Marketing.DeleteReportTaskAsync(reportTaskId);
```

### Recommendation API

#### Find Listing Recommendations
```csharp
// Get recommendations for all active listings
var recommendations = await client.Recommendation.FindListingRecommendationsAsync();

// Get recommendations for specific listings
var requestJson = @"{
    ""listingIds"": [""item1"", ""item2"", ""item3""]
}";
var recommendations = await client.Recommendation.FindListingRecommendationsAsync(requestJson);
```

The recommendation API returns:
- **promoteWithAd indicator**: Whether eBay recommends promoting the listing
- **bidPercentage**: Current trending bid percentage for similar promoted items

## Error Handling

```csharp
using MarketingAndPromotions.Exceptions;

try
{
    var campaign = await client.Marketing.GetCampaignAsync(campaignId);
}
catch (SdkException ex)
{
    Console.WriteLine($"SDK Error: {ex.Message}");
}
catch (HttpRequestException ex)
{
    Console.WriteLine($"HTTP Error: {ex.Message}");
}
```

## Regenerating the SDK

When OpenAPI specs change:

```bash
# Run the regeneration script
./generate.sh

# Or manually:
dotnet tool restore
dotnet kiota generate -l csharp -d specs/sell_marketing_v1_oas3.yaml ...
dotnet build EBay.MarketingAndPromotions/EBay.MarketingAndPromotions.csproj
```

## Configuration Options

| Property | Default | Description |
|----------|---------|-------------|
| `MarketingBaseUrl` | `https://api.ebay.com/sell/marketing/v1` | Marketing API base URL |
| `RecommendationBaseUrl` | `https://api.ebay.com/sell/recommendation/v1` | Recommendation API base URL |
| `TokenRefreshHandler` | (required) | OAuth token refresh handler |

## Project Structure

- **Generated/**: Kiota-generated REST clients (internal, committed to git)
  - Marketing/: 81+ operations for advertising campaigns
  - Recommendation/: Listing promotion recommendations
- **Clients/Rest/**: Hand-crafted wrappers exposing public API
- **Configuration/**: SDK config, auth providers
- **Exceptions/**: SDK exceptions
- **Extensions/**: DI extension methods

## Development

### Prerequisites

- .NET 10.0 SDK
- Kiota CLI tool (auto-installed via dotnet tools)

### Building

```bash
dotnet build EBay.MarketingAndPromotions/EBay.MarketingAndPromotions.csproj
```

### Packaging

```bash
dotnet pack EBay.MarketingAndPromotions/EBay.MarketingAndPromotions.csproj
```

## Dependencies

- **Microsoft.Kiota.Bundle**: REST client generation and runtime
- **Microsoft.Extensions.DependencyInjection.Abstractions**: DI support
- **Microsoft.Extensions.Http**: HttpClient factory integration

## License

[Apache 2.0](LICENSE) © 2026 eBay Inc.

## Contributing

1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Run `./generate.sh` if specs changed
5. Ensure `dotnet build` succeeds
6. Submit a pull request
