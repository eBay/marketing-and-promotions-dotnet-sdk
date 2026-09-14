#!/usr/bin/env bash

# Copyright 2026 eBay Inc.
#
# Licensed under the Apache License, Version 2.0 (the "License");
# you may not use this file except in compliance with the License.
# You may obtain a copy of the License at
#
#     http://www.apache.org/licenses/LICENSE-2.0
#
# Unless required by applicable law or agreed to in writing, software
# distributed under the License is distributed on an "AS IS" BASIS,
# WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
# See the License for the specific language governing permissions and
# limitations under the License.

set -euo pipefail

echo "Restoring tools..."
dotnet tool restore

echo "Regenerating REST clients via Kiota..."
rm -rf ./EBay.MarketingAndPromotions/Generated

dotnet kiota generate \
  -l csharp \
  -d specs/openapi/marketing.yaml \
  -c MarketingApiClient \
  -n EBay.MarketingAndPromotions.Generated.Marketing \
  -o EBay.MarketingAndPromotions/Generated/Marketing \
  --ebc --co

dotnet kiota generate \
  -l csharp \
  -d specs/openapi/recommendation.yaml \
  -c RecommendationApiClient \
  -n EBay.MarketingAndPromotions.Generated.Recommendation \
  -o EBay.MarketingAndPromotions/Generated/Recommendation \
  --ebc --co

echo "Adding license headers to generated files..."
./add-license-headers.sh ./EBay.MarketingAndPromotions/Generated

echo "Building (triggers StrawberryShake GraphQL codegen)..."
DOTNET_ROLL_FORWARD=LatestMajor dotnet build EBay.MarketingAndPromotions/EBay.MarketingAndPromotions.csproj

echo "Done."
