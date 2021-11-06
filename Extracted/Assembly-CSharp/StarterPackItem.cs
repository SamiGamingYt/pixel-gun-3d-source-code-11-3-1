using System;
using UnityEngine;

// Token: 0x0200075F RID: 1887
public class StarterPackItem : MonoBehaviour
{
	// Token: 0x0600426D RID: 17005 RVA: 0x00160C28 File Offset: 0x0015EE28
	public void SetData(StarterPackItemData itemData)
	{
		bool flag = itemData.count > 1;
		this.countItems.gameObject.SetActive(flag);
		if (flag)
		{
			this.countItems.text = itemData.count.ToString();
		}
		string validTag = itemData.validTag;
		int num = 0;
		string text = validTag;
		bool flag2 = GearManager.IsItemGear(validTag);
		if (flag2)
		{
			text = GearManager.HolderQuantityForID(validTag);
			num = GearManager.CurrentNumberOfUphradesForGear(text);
		}
		if (flag2 && (text == GearManager.Turret || text == GearManager.Mech))
		{
			int? upgradeNum = new int?(num);
			this.imageItem.mainTexture = ItemDb.GetTextureItemByTag(text, upgradeNum);
		}
		else
		{
			this.imageItem.mainTexture = ItemDb.GetTextureItemByTag(text, null);
		}
		this.nameItem.text = ItemDb.GetItemNameByTag(validTag);
		string text2 = ItemDb.GetShopIdByTag(validTag);
		if (string.IsNullOrEmpty(text2))
		{
			if (flag2)
			{
				text2 = GearManager.OneItemIDForGear(text, num);
			}
			else
			{
				text2 = validTag;
			}
		}
		ItemPrice priceByShopId = ItemDb.GetPriceByShopId(text2, (ShopNGUIController.CategoryNames)(-1));
		if (priceByShopId != null)
		{
			int num2 = priceByShopId.Price * itemData.count;
			string arg = (!(priceByShopId.Currency == "Coins")) ? LocalizationStore.Get("Key_0771") : LocalizationStore.Get("Key_0936");
			this.realPriceItem.text = string.Format("{0} {1}", num2, arg);
		}
	}

	// Token: 0x04003075 RID: 12405
	public UITexture imageItem;

	// Token: 0x04003076 RID: 12406
	public UILabel nameItem;

	// Token: 0x04003077 RID: 12407
	public UILabel countItems;

	// Token: 0x04003078 RID: 12408
	public UILabel realPriceItem;
}
