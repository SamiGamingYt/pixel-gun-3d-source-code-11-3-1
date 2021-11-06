using System;
using com.amazon.device.iap.cpt;
using Prime31;

namespace Rilisoft
{
	// Token: 0x020006BD RID: 1725
	internal static class MarketProductFactory
	{
		// Token: 0x06003C2E RID: 15406 RVA: 0x00138348 File Offset: 0x00136548
		internal static GoogleMarketProduct CreateGoogleMarketProduct(GoogleSkuInfo googleSkuInfo)
		{
			return new GoogleMarketProduct(googleSkuInfo);
		}

		// Token: 0x06003C2F RID: 15407 RVA: 0x00138350 File Offset: 0x00136550
		internal static AmazonMarketProduct CreateAmazonMarketProduct(ProductData amazonItem)
		{
			return new AmazonMarketProduct(amazonItem);
		}
	}
}
