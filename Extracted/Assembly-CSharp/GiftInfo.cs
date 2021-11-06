using System;
using Rilisoft;
using UnityEngine;

// Token: 0x0200064C RID: 1612
[Serializable]
public class GiftInfo
{
	// Token: 0x1700092D RID: 2349
	// (get) Token: 0x0600380B RID: 14347 RVA: 0x00121B00 File Offset: 0x0011FD00
	public ShopNGUIController.CategoryNames? TypeShopCat
	{
		get
		{
			ItemDb.GetByTag(this.Id);
			if (this.Id.IsNullOrEmpty())
			{
				return null;
			}
			int itemCategory = ItemDb.GetItemCategory(this.Id);
			if (itemCategory < 0)
			{
				return null;
			}
			return new ShopNGUIController.CategoryNames?((ShopNGUIController.CategoryNames)itemCategory);
		}
	}

	// Token: 0x0600380C RID: 14348 RVA: 0x00121B58 File Offset: 0x0011FD58
	public static GiftInfo CreateInfo(GiftInfo rootGift, string giftId, int count = 1)
	{
		return new GiftInfo
		{
			Count = count,
			Id = giftId,
			KeyTranslateInfo = rootGift.KeyTranslateInfo,
			PercentAddInSlot = rootGift.PercentAddInSlot,
			RootInfo = rootGift
		};
	}

	// Token: 0x040028DB RID: 10459
	public string Id;

	// Token: 0x040028DC RID: 10460
	public SaltedInt Count = new SaltedInt(12499947, 0);

	// Token: 0x040028DD RID: 10461
	public float PercentAddInSlot;

	// Token: 0x040028DE RID: 10462
	public string KeyTranslateInfo = string.Empty;

	// Token: 0x040028DF RID: 10463
	[HideInInspector]
	public bool IsRandomSkin;

	// Token: 0x040028E0 RID: 10464
	[ReadOnly]
	public GiftInfo RootInfo;
}
