using System;
using Rilisoft;
using UnityEngine;

// Token: 0x020006B7 RID: 1719
public sealed class BonusMarafonItem
{
	// Token: 0x06003BF0 RID: 15344 RVA: 0x00136BC4 File Offset: 0x00134DC4
	public BonusMarafonItem(BonusItemType elementType, int countElements, string iconPreviewName, string tagWeapon = null)
	{
		this.type = elementType;
		this.count = countElements;
		this.iconPreviewFileName = iconPreviewName;
		this.tag = tagWeapon;
	}

	// Token: 0x06003BF1 RID: 15345 RVA: 0x00136BFC File Offset: 0x00134DFC
	public string GetShortDescription()
	{
		string text = string.Empty;
		switch (this.type)
		{
		case BonusItemType.Gold:
			text = LocalizationStore.Get("Key_0936");
			break;
		case BonusItemType.Real:
			text = LocalizationStore.Get("Key_0771");
			break;
		case BonusItemType.PotionInvisible:
			text = LocalizationStore.Get("Key_0775");
			break;
		case BonusItemType.JetPack:
			text = LocalizationStore.Get("Key_0772");
			break;
		case BonusItemType.Granade:
			text = LocalizationStore.Get("Key_0776");
			break;
		case BonusItemType.Turret:
			text = LocalizationStore.Get("Key_0773");
			break;
		case BonusItemType.Mech:
			text = LocalizationStore.Get("Key_0774");
			break;
		case BonusItemType.TemporaryWeapon:
			text = ItemDb.GetItemNameByTag(this.tag);
			break;
		}
		if (string.IsNullOrEmpty(text) || this.count.Value == 0)
		{
			return string.Empty;
		}
		if (this.count.Value > 1)
		{
			return string.Format("{0} {1}", this.count.Value, text);
		}
		return text;
	}

	// Token: 0x06003BF2 RID: 15346 RVA: 0x00136D14 File Offset: 0x00134F14
	public string GetLongDescription()
	{
		string result = string.Empty;
		switch (this.type)
		{
		case BonusItemType.PotionInvisible:
			result = LocalizationStore.Get("Key_0851");
			break;
		case BonusItemType.JetPack:
			result = LocalizationStore.Get("Key_0850");
			break;
		case BonusItemType.Turret:
			result = LocalizationStore.Get("Key_0852");
			break;
		case BonusItemType.Mech:
			result = LocalizationStore.Get("Key_0849");
			break;
		case BonusItemType.TemporaryWeapon:
			result = LocalizationStore.Get("Key_1200");
			break;
		}
		return result;
	}

	// Token: 0x06003BF3 RID: 15347 RVA: 0x00136DA4 File Offset: 0x00134FA4
	public Texture2D GetIcon()
	{
		if (this.type == BonusItemType.TemporaryWeapon)
		{
			ShopNGUIController.CategoryNames itemCategory = (ShopNGUIController.CategoryNames)ItemDb.GetItemCategory(this.tag);
			return ItemDb.GetItemIcon(this.tag, itemCategory, null, true) as Texture2D;
		}
		string path = "OfferIcons/Marathon/" + this.iconPreviewFileName;
		return Resources.Load<Texture2D>(path);
	}

	// Token: 0x04002C5B RID: 11355
	private const string PathToBonusesIcons = "OfferIcons/Marathon/";

	// Token: 0x04002C5C RID: 11356
	public BonusItemType type;

	// Token: 0x04002C5D RID: 11357
	public SaltedInt count;

	// Token: 0x04002C5E RID: 11358
	public string iconPreviewFileName;

	// Token: 0x04002C5F RID: 11359
	public string tag;
}
