using System;
using Rilisoft;
using UnityEngine;

// Token: 0x02000803 RID: 2051
public class RespawnWindowEquipmentItem : MonoBehaviour
{
	// Token: 0x06004ABA RID: 19130 RVA: 0x001A859C File Offset: 0x001A679C
	public void SetItemTag(string itemTag, int itemCategory)
	{
		bool flag = RespawnWindowEquipmentItem.IsNoneEquipment(itemTag);
		this.itemImage.gameObject.SetActiveSafeSelf(!flag);
		this.emptyImage.gameObject.SetActiveSafeSelf(flag);
		this.itemTag = ((!flag) ? itemTag : null);
		this.itemCategory = ((!flag) ? itemCategory : -1);
		if (!flag)
		{
			this.itemImage.mainTexture = ItemDb.GetItemIcon(itemTag, (ShopNGUIController.CategoryNames)itemCategory, null, true);
		}
	}

	// Token: 0x06004ABB RID: 19131 RVA: 0x001A8620 File Offset: 0x001A6820
	public void ResetImage()
	{
		this.itemImage.mainTexture = null;
	}

	// Token: 0x06004ABC RID: 19132 RVA: 0x001A8630 File Offset: 0x001A6830
	private static bool IsNoneEquipment(string itemTag)
	{
		return string.IsNullOrEmpty(itemTag) || itemTag == Defs.HatNoneEqupped || itemTag == Defs.ArmorNewNoneEqupped || itemTag == Defs.CapeNoneEqupped || itemTag == Defs.BootsNoneEqupped || itemTag == "MaskNoneEquipped";
	}

	// Token: 0x04003753 RID: 14163
	public UITexture itemImage;

	// Token: 0x04003754 RID: 14164
	public UISprite emptyImage;

	// Token: 0x04003755 RID: 14165
	[NonSerialized]
	public string itemTag;

	// Token: 0x04003756 RID: 14166
	[NonSerialized]
	public int itemCategory;
}
