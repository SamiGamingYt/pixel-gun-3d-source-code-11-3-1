using System;
using System.Collections.Generic;

// Token: 0x02000760 RID: 1888
public class StarterPackItemData
{
	// Token: 0x0600426F RID: 17007 RVA: 0x00160DB8 File Offset: 0x0015EFB8
	private bool IsInvalidArmorTag(string tag)
	{
		List<string> list = null;
		if (Wear.wear[ShopNGUIController.CategoryNames.ArmorCategory][0].Contains(tag))
		{
			list = new List<string>();
			list.AddRange(Wear.wear[ShopNGUIController.CategoryNames.ArmorCategory][0]);
		}
		else if (Wear.wear[ShopNGUIController.CategoryNames.HatsCategory][0].Contains(tag))
		{
			list = new List<string>();
			list.AddRange(Wear.wear[ShopNGUIController.CategoryNames.HatsCategory][0]);
		}
		if (list == null)
		{
			return false;
		}
		List<string> list2 = PromoActionsGUIController.FilterPurchases(list, true, true, false, true);
		foreach (string item in list2)
		{
			list.Remove(item);
		}
		return list.Count == 0 || list[0] != tag;
	}

	// Token: 0x17000AF5 RID: 2805
	// (get) Token: 0x06004270 RID: 17008 RVA: 0x00160EC0 File Offset: 0x0015F0C0
	public string validTag
	{
		get
		{
			for (int i = 0; i < this.variantTags.Count; i++)
			{
				if (!ItemDb.IsItemInInventory(this.variantTags[i]) && !this.IsInvalidArmorTag(this.variantTags[i]))
				{
					return this.variantTags[i];
				}
			}
			return string.Empty;
		}
	}

	// Token: 0x04003079 RID: 12409
	public List<string> variantTags = new List<string>();

	// Token: 0x0400307A RID: 12410
	public int count;
}
