using System;

namespace Rilisoft
{
	// Token: 0x020006BA RID: 1722
	public interface IMarketProduct
	{
		// Token: 0x170009D7 RID: 2519
		// (get) Token: 0x06003C11 RID: 15377
		string Id { get; }

		// Token: 0x170009D8 RID: 2520
		// (get) Token: 0x06003C12 RID: 15378
		string Title { get; }

		// Token: 0x170009D9 RID: 2521
		// (get) Token: 0x06003C13 RID: 15379
		string Description { get; }

		// Token: 0x170009DA RID: 2522
		// (get) Token: 0x06003C14 RID: 15380
		string Price { get; }

		// Token: 0x170009DB RID: 2523
		// (get) Token: 0x06003C15 RID: 15381
		object PlatformProduct { get; }

		// Token: 0x170009DC RID: 2524
		// (get) Token: 0x06003C16 RID: 15382
		decimal PriceValue { get; }

		// Token: 0x170009DD RID: 2525
		// (get) Token: 0x06003C17 RID: 15383
		string Currency { get; }
	}
}
