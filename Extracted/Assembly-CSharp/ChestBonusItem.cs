using System;
using UnityEngine;

// Token: 0x020005B6 RID: 1462
public class ChestBonusItem : MonoBehaviour
{
	// Token: 0x060032A1 RID: 12961 RVA: 0x001066F4 File Offset: 0x001048F4
	public void SetData(ChestBonusItemData itemData)
	{
		string text = string.Empty;
		if (itemData.timeLife == -1)
		{
			if (itemData.count > 1)
			{
				text = string.Format("{0} {1}", itemData.count, LocalizationStore.Get("Key_1230"));
			}
			else
			{
				text = LocalizationStore.Get("Key_1059");
			}
		}
		else
		{
			text = itemData.GetTimeLabel(false);
		}
		this.timeLifeLabel.text = text;
		this.itemImageHolder.mainTexture = ItemDb.GetTextureForShopItem(itemData.tag);
		this.itemNameLabel.text = ItemDb.GetItemNameByTag(itemData.tag);
	}

	// Token: 0x060032A2 RID: 12962 RVA: 0x00106794 File Offset: 0x00104994
	public void SetVisible(bool visible)
	{
		base.gameObject.SetActive(visible);
	}

	// Token: 0x0400252A RID: 9514
	public UILabel timeLifeLabel;

	// Token: 0x0400252B RID: 9515
	public UILabel itemNameLabel;

	// Token: 0x0400252C RID: 9516
	public UITexture itemImageHolder;
}
