using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020005B8 RID: 1464
public class ChestBonusData
{
	// Token: 0x060032A6 RID: 12966 RVA: 0x0010681C File Offset: 0x00104A1C
	public string GetItemCountOrTime()
	{
		if (this.items == null || this.items.Count == 0)
		{
			return string.Empty;
		}
		if (this.items.Count == 1)
		{
			ChestBonusItemData chestBonusItemData = this.items[0];
			int num = chestBonusItemData.timeLife / 24;
			return (chestBonusItemData.timeLife != -1) ? chestBonusItemData.GetTimeLabel(true) : chestBonusItemData.count.ToString();
		}
		return string.Empty;
	}

	// Token: 0x060032A7 RID: 12967 RVA: 0x001068A0 File Offset: 0x00104AA0
	public Texture GetImage()
	{
		if (this.items == null || this.items.Count == 0)
		{
			return null;
		}
		string path = string.Empty;
		if (this.items.Count == 1)
		{
			ChestBonusItemData chestBonusItemData = this.items[0];
			return ItemDb.GetTextureForShopItem(chestBonusItemData.tag);
		}
		path = "Textures/Bank/StarterPack_Weapon";
		return Resources.Load<Texture>(path);
	}

	// Token: 0x04002530 RID: 9520
	public string linkKey;

	// Token: 0x04002531 RID: 9521
	public bool isVisible;

	// Token: 0x04002532 RID: 9522
	public List<ChestBonusItemData> items;
}
