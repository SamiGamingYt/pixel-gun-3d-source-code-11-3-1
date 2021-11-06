using System;
using Prime31;
using UnityEngine;

namespace Rilisoft
{
	// Token: 0x020006BB RID: 1723
	internal sealed class GoogleMarketProduct : IMarketProduct
	{
		// Token: 0x06003C18 RID: 15384 RVA: 0x001380DC File Offset: 0x001362DC
		public GoogleMarketProduct(GoogleSkuInfo googleSkuInfo)
		{
			this._marketProduct = googleSkuInfo;
		}

		// Token: 0x170009DE RID: 2526
		// (get) Token: 0x06003C19 RID: 15385 RVA: 0x001380EC File Offset: 0x001362EC
		public string Id
		{
			get
			{
				return this._marketProduct.productId;
			}
		}

		// Token: 0x170009DF RID: 2527
		// (get) Token: 0x06003C1A RID: 15386 RVA: 0x001380FC File Offset: 0x001362FC
		public string Title
		{
			get
			{
				return this._marketProduct.title;
			}
		}

		// Token: 0x170009E0 RID: 2528
		// (get) Token: 0x06003C1B RID: 15387 RVA: 0x0013810C File Offset: 0x0013630C
		public string Description
		{
			get
			{
				return this._marketProduct.description;
			}
		}

		// Token: 0x170009E1 RID: 2529
		// (get) Token: 0x06003C1C RID: 15388 RVA: 0x0013811C File Offset: 0x0013631C
		public string Price
		{
			get
			{
				return this._marketProduct.price;
			}
		}

		// Token: 0x170009E2 RID: 2530
		// (get) Token: 0x06003C1D RID: 15389 RVA: 0x0013812C File Offset: 0x0013632C
		public object PlatformProduct
		{
			get
			{
				return this._marketProduct;
			}
		}

		// Token: 0x170009E3 RID: 2531
		// (get) Token: 0x06003C1E RID: 15390 RVA: 0x00138134 File Offset: 0x00136334
		public decimal PriceValue
		{
			get
			{
				if (this._marketProduct == null)
				{
					Debug.LogErrorFormat("GoogleMarketProduct.PriceValue: _marketProduct == null", new object[0]);
					return 0m;
				}
				decimal result = 0m;
				try
				{
					result = StoreKitEventListener.GetPriceFromPriceAmountMicros(this._marketProduct.priceAmountMicros);
				}
				catch (Exception ex)
				{
					Debug.LogErrorFormat("Exception in PriceValue: {0}", new object[]
					{
						ex
					});
				}
				return result;
			}
		}

		// Token: 0x170009E4 RID: 2532
		// (get) Token: 0x06003C1F RID: 15391 RVA: 0x001381B8 File Offset: 0x001363B8
		public string Currency
		{
			get
			{
				if (this._marketProduct == null)
				{
					Debug.LogErrorFormat("GoogleMarketProduct.Currency: _marketProduct == null", new object[0]);
					return string.Empty;
				}
				string result = string.Empty;
				try
				{
					result = (this._marketProduct.priceCurrencyCode ?? string.Empty);
				}
				catch (Exception ex)
				{
					Debug.LogErrorFormat("Exception in Currency: {0}", new object[]
					{
						ex
					});
				}
				return result;
			}
		}

		// Token: 0x06003C20 RID: 15392 RVA: 0x00138240 File Offset: 0x00136440
		public override bool Equals(object obj)
		{
			GoogleMarketProduct googleMarketProduct = obj as GoogleMarketProduct;
			if (googleMarketProduct == null)
			{
				return false;
			}
			GoogleSkuInfo marketProduct = googleMarketProduct._marketProduct;
			return this._marketProduct.Equals(marketProduct);
		}

		// Token: 0x06003C21 RID: 15393 RVA: 0x00138270 File Offset: 0x00136470
		public override int GetHashCode()
		{
			return this._marketProduct.GetHashCode();
		}

		// Token: 0x06003C22 RID: 15394 RVA: 0x00138280 File Offset: 0x00136480
		public override string ToString()
		{
			return this._marketProduct.ToString();
		}

		// Token: 0x04002C6C RID: 11372
		private readonly GoogleSkuInfo _marketProduct;
	}
}
