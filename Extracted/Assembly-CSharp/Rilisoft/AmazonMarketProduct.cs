using System;
using com.amazon.device.iap.cpt;

namespace Rilisoft
{
	// Token: 0x020006BC RID: 1724
	internal sealed class AmazonMarketProduct : IMarketProduct
	{
		// Token: 0x06003C23 RID: 15395 RVA: 0x00138290 File Offset: 0x00136490
		public AmazonMarketProduct(ProductData amazonItem)
		{
			this._marketProduct = amazonItem;
		}

		// Token: 0x170009E5 RID: 2533
		// (get) Token: 0x06003C24 RID: 15396 RVA: 0x001382A0 File Offset: 0x001364A0
		public string Id
		{
			get
			{
				return this._marketProduct.Sku;
			}
		}

		// Token: 0x170009E6 RID: 2534
		// (get) Token: 0x06003C25 RID: 15397 RVA: 0x001382B0 File Offset: 0x001364B0
		public string Title
		{
			get
			{
				return this._marketProduct.Title;
			}
		}

		// Token: 0x170009E7 RID: 2535
		// (get) Token: 0x06003C26 RID: 15398 RVA: 0x001382C0 File Offset: 0x001364C0
		public string Description
		{
			get
			{
				return this._marketProduct.Description;
			}
		}

		// Token: 0x170009E8 RID: 2536
		// (get) Token: 0x06003C27 RID: 15399 RVA: 0x001382D0 File Offset: 0x001364D0
		public string Price
		{
			get
			{
				return this._marketProduct.Price;
			}
		}

		// Token: 0x170009E9 RID: 2537
		// (get) Token: 0x06003C28 RID: 15400 RVA: 0x001382E0 File Offset: 0x001364E0
		public object PlatformProduct
		{
			get
			{
				return this._marketProduct;
			}
		}

		// Token: 0x170009EA RID: 2538
		// (get) Token: 0x06003C29 RID: 15401 RVA: 0x001382E8 File Offset: 0x001364E8
		public decimal PriceValue
		{
			get
			{
				return 0m;
			}
		}

		// Token: 0x170009EB RID: 2539
		// (get) Token: 0x06003C2A RID: 15402 RVA: 0x001382F0 File Offset: 0x001364F0
		public string Currency
		{
			get
			{
				return string.Empty;
			}
		}

		// Token: 0x06003C2B RID: 15403 RVA: 0x001382F8 File Offset: 0x001364F8
		public override string ToString()
		{
			return this._marketProduct.ToString();
		}

		// Token: 0x06003C2C RID: 15404 RVA: 0x00138308 File Offset: 0x00136508
		public override bool Equals(object obj)
		{
			AmazonMarketProduct amazonMarketProduct = obj as AmazonMarketProduct;
			if (amazonMarketProduct == null)
			{
				return false;
			}
			ProductData marketProduct = amazonMarketProduct._marketProduct;
			return this._marketProduct.Equals(marketProduct);
		}

		// Token: 0x06003C2D RID: 15405 RVA: 0x00138338 File Offset: 0x00136538
		public override int GetHashCode()
		{
			return this._marketProduct.GetHashCode();
		}

		// Token: 0x04002C6D RID: 11373
		private readonly ProductData _marketProduct;
	}
}
