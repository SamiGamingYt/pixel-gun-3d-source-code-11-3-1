using System;
using System.Collections.Generic;
using I2.Loc;
using UnityEngine;

// Token: 0x02000804 RID: 2052
public sealed class RespawnWindowItemToBuy : MonoBehaviour
{
	// Token: 0x06004ABF RID: 19135 RVA: 0x001A86EC File Offset: 0x001A68EC
	public void SetWeaponTag(string itemTag, int? upgradeNum = null)
	{
		if (string.IsNullOrEmpty(itemTag))
		{
			base.gameObject.SetActive(false);
			return;
		}
		base.gameObject.SetActive(true);
		int num = ItemDb.GetItemCategory(itemTag);
		ShopNGUIController.CategoryNames categoryNames = (ShopNGUIController.CategoryNames)num;
		this.itemTag = itemTag;
		this.itemCategory = num;
		this.itemPrice = null;
		this.itemImage.mainTexture = ItemDb.GetItemIcon(itemTag, categoryNames, upgradeNum, true);
		this.itemNameLabel.text = RespawnWindowItemToBuy.GetItemName(itemTag, categoryNames, (upgradeNum == null) ? 0 : upgradeNum.Value);
		this.nonLocalizedName = RespawnWindowItemToBuy.GetItemNonLocalizedName(itemTag, categoryNames, (upgradeNum == null) ? 0 : upgradeNum.Value);
		if (!RespawnWindowItemToBuy.IsCanBuy(itemTag, categoryNames))
		{
			this.itemPriceBtnBuyContainer.gameObject.SetActive(false);
			this.needTierLabel.gameObject.SetActive(false);
		}
		else
		{
			this.itemPriceBtnBuyContainer.SetActive(true);
			this.needTierLabel.gameObject.SetActive(false);
			if (ShopNGUIController.IsWeaponCategory(categoryNames))
			{
				int tier = ItemDb.GetWeaponInfo(itemTag).tier;
				bool flag = ExpController.Instance != null && ExpController.Instance.OurTier < tier;
				if (flag)
				{
					this.itemPriceBtnBuyContainer.SetActive(false);
					this.needTierLabel.gameObject.SetActive(true);
					int num2 = (tier < 0 || tier >= ExpController.LevelsForTiers.Length) ? ExpController.LevelsForTiers[ExpController.LevelsForTiers.Length - 1] : ExpController.LevelsForTiers[tier];
					string text = string.Format("{0} {1} {2}", LocalizationStore.Key_0226, num2, LocalizationStore.Get("Key_1022"));
					this.needTierLabel.text = text;
				}
			}
			this.itemPrice = ShopNGUIController.GetItemPrice(itemTag, categoryNames, false, true, false);
			RespawnWindowItemToBuy.SetPrice(this.itemPriceIcon, this.itemPriceLabel, this.itemPrice);
		}
	}

	// Token: 0x06004AC0 RID: 19136 RVA: 0x001A88D4 File Offset: 0x001A6AD4
	public static string GetItemName(string itemTag, ShopNGUIController.CategoryNames itemCategory, int upgradeNum = 0)
	{
		string text = itemTag;
		if (text != null)
		{
			if (RespawnWindowItemToBuy.<>f__switch$map15 == null)
			{
				RespawnWindowItemToBuy.<>f__switch$map15 = new Dictionary<string, int>(1)
				{
					{
						"LikeID",
						0
					}
				};
			}
			int num;
			if (RespawnWindowItemToBuy.<>f__switch$map15.TryGetValue(text, out num))
			{
				if (num == 0)
				{
					return ScriptLocalization.Get("Key_1796");
				}
			}
		}
		if (GearManager.IsItemGear(itemTag))
		{
			upgradeNum = Mathf.Min(upgradeNum, GearManager.NumOfGearUpgrades);
			itemTag = GearManager.UpgradeIDForGear(itemTag, upgradeNum);
		}
		return ItemDb.GetItemName(itemTag, itemCategory);
	}

	// Token: 0x06004AC1 RID: 19137 RVA: 0x001A895C File Offset: 0x001A6B5C
	public static string GetItemNonLocalizedName(string itemTag, ShopNGUIController.CategoryNames itemCategory, int upgradeNum = 0)
	{
		string shopId = itemTag;
		if (ShopNGUIController.IsWeaponCategory(itemCategory))
		{
			shopId = ItemDb.GetShopIdByTag(itemTag);
		}
		if (GearManager.IsItemGear(itemTag))
		{
			upgradeNum = Mathf.Min(upgradeNum, GearManager.NumOfGearUpgrades);
			shopId = GearManager.UpgradeIDForGear(itemTag, upgradeNum);
		}
		return ItemDb.GetItemNameNonLocalized(itemTag, shopId, itemCategory, itemTag);
	}

	// Token: 0x06004AC2 RID: 19138 RVA: 0x001A89A8 File Offset: 0x001A6BA8
	private static bool IsCanBuy(string itemTag, ShopNGUIController.CategoryNames itemCategory)
	{
		if (ShopNGUIController.IsWeaponCategory(itemCategory))
		{
			bool flag = ItemDb.IsCanBuy(itemTag) && !ItemDb.IsTemporaryGun(itemTag);
			bool flag2 = ItemDb.IsItemInInventory(itemTag);
			bool flag3 = ItemDb.HasWeaponNeedUpgradesForBuyNext(itemTag);
			string text = WeaponManager.FirstTagForOurTier(itemTag, null);
			return flag && !flag2 && (flag3 || (text != null && text.Equals(itemTag)));
		}
		return !GearManager.IsItemGear(itemTag) && itemCategory == ShopNGUIController.CategoryNames.ArmorCategory && !ItemDb.IsItemInInventory(itemTag) && !TempItemsController.PriceCoefs.ContainsKey(itemTag);
	}

	// Token: 0x06004AC3 RID: 19139 RVA: 0x001A8A50 File Offset: 0x001A6C50
	private static void SetPrice(UISprite priceIcon, UILabel priceLabel, ItemPrice price)
	{
		bool flag = price.Currency == "GemsCurrency";
		priceIcon.spriteName = ((!flag) ? "ingame_coin" : "gem_znachek");
		priceIcon.width = ((!flag) ? 30 : 24);
		priceIcon.height = ((!flag) ? 30 : 24);
		priceLabel.text = price.Price.ToString();
		priceLabel.color = ((!flag) ? RespawnWindowItemToBuy.priceCoinColor : RespawnWindowItemToBuy.priceGemColor);
	}

	// Token: 0x06004AC4 RID: 19140 RVA: 0x001A8AE4 File Offset: 0x001A6CE4
	public void Reset()
	{
		this.itemImage.mainTexture = null;
		this.itemTag = string.Empty;
		this.nonLocalizedName = string.Empty;
	}

	// Token: 0x04003757 RID: 14167
	public UITexture itemImage;

	// Token: 0x04003758 RID: 14168
	public UILabel itemNameLabel;

	// Token: 0x04003759 RID: 14169
	public GameObject itemPriceBtnBuyContainer;

	// Token: 0x0400375A RID: 14170
	public UILabel needTierLabel;

	// Token: 0x0400375B RID: 14171
	public UISprite itemPriceIcon;

	// Token: 0x0400375C RID: 14172
	public UILabel itemPriceLabel;

	// Token: 0x0400375D RID: 14173
	public UIButton btnBuy;

	// Token: 0x0400375E RID: 14174
	[NonSerialized]
	public string itemTag;

	// Token: 0x0400375F RID: 14175
	[NonSerialized]
	public int itemCategory;

	// Token: 0x04003760 RID: 14176
	[NonSerialized]
	public ItemPrice itemPrice;

	// Token: 0x04003761 RID: 14177
	[NonSerialized]
	public string nonLocalizedName;

	// Token: 0x04003762 RID: 14178
	private static Color priceGemColor = new Color32(100, 230, byte.MaxValue, byte.MaxValue);

	// Token: 0x04003763 RID: 14179
	private static Color priceCoinColor = new Color32(byte.MaxValue, byte.MaxValue, 0, byte.MaxValue);
}
