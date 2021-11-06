using System;
using System.Collections.Generic;
using System.Linq;
using Rilisoft;
using UnityEngine;

// Token: 0x02000648 RID: 1608
[Serializable]
public class GiftCategory
{
	// Token: 0x1700091A RID: 2330
	// (get) Token: 0x060037B5 RID: 14261 RVA: 0x0011EF30 File Offset: 0x0011D130
	// (set) Token: 0x060037B6 RID: 14262 RVA: 0x0011EF5C File Offset: 0x0011D15C
	private List<GiftInfo> _allGifts
	{
		get
		{
			List<GiftInfo> result;
			if ((result = this._ag) == null)
			{
				result = (this._ag = this.GetAvailableGifts());
			}
			return result;
		}
		set
		{
			this._ag = value;
		}
	}

	// Token: 0x1700091B RID: 2331
	// (get) Token: 0x060037B7 RID: 14263 RVA: 0x0011EF68 File Offset: 0x0011D168
	public bool AnyGifts
	{
		get
		{
			return this._allGifts.Any<GiftInfo>();
		}
	}

	// Token: 0x1700091C RID: 2332
	// (get) Token: 0x060037B8 RID: 14264 RVA: 0x0011EF78 File Offset: 0x0011D178
	public float PercentChance
	{
		get
		{
			if (this.Type == GiftCategoryType.Guns_gray || this.Type == GiftCategoryType.Masks || this.Type == GiftCategoryType.Boots || this.Type == GiftCategoryType.Capes || this.Type == GiftCategoryType.Hats_random || this.Type == GiftCategoryType.ArmorAndHat || this.Type == GiftCategoryType.WeaponSkin || this.Type == GiftCategoryType.Gadgets)
			{
				return this._allGifts[0].PercentAddInSlot;
			}
			return this._allGifts.Sum((GiftInfo g) => g.PercentAddInSlot);
		}
	}

	// Token: 0x1700091D RID: 2333
	// (get) Token: 0x060037B9 RID: 14265 RVA: 0x0011F028 File Offset: 0x0011D228
	private List<GiftInfo> _availableGifts
	{
		get
		{
			return (from g in this._allGifts
			where this.AvailableGift(g.Id, this.Type)
			select g).ToList<GiftInfo>();
		}
	}

	// Token: 0x1700091E RID: 2334
	// (get) Token: 0x060037BA RID: 14266 RVA: 0x0011F048 File Offset: 0x0011D248
	public int AvaliableGiftsCount
	{
		get
		{
			return this._availableGifts.Count;
		}
	}

	// Token: 0x1700091F RID: 2335
	// (get) Token: 0x060037BB RID: 14267 RVA: 0x0011F058 File Offset: 0x0011D258
	private float _availableGiftsPercentSum
	{
		get
		{
			return this._availableGifts.Sum((GiftInfo g) => g.PercentAddInSlot);
		}
	}

	// Token: 0x060037BC RID: 14268 RVA: 0x0011F090 File Offset: 0x0011D290
	public void AddGift(GiftInfo info)
	{
		this._rootGifts.Add(info);
	}

	// Token: 0x060037BD RID: 14269 RVA: 0x0011F0A0 File Offset: 0x0011D2A0
	public void CheckGifts()
	{
		this._allGifts = this.GetAvailableGifts();
		foreach (GiftInfo giftInfo in this._allGifts)
		{
			if (this.Type == GiftCategoryType.ArmorAndHat)
			{
				giftInfo.Id = Wear.ArmorOrArmorHatAvailableForBuy(ShopNGUIController.CategoryNames.ArmorCategory);
			}
			if (this.Type == GiftCategoryType.Skins)
			{
				giftInfo.IsRandomSkin = true;
				giftInfo.Id = SkinsController.RandomUnboughtSkinId();
			}
		}
	}

	// Token: 0x060037BE RID: 14270 RVA: 0x0011F144 File Offset: 0x0011D344
	public bool AvailableGift(string idGift, GiftCategoryType curType)
	{
		if (string.IsNullOrEmpty(idGift))
		{
			return false;
		}
		switch (curType)
		{
		case GiftCategoryType.Coins:
		case GiftCategoryType.Gems:
		case GiftCategoryType.Gear:
		case GiftCategoryType.Event_content:
		case GiftCategoryType.Freespins:
			return true;
		case GiftCategoryType.Skins:
		{
			bool flag = false;
			return !SkinsController.IsSkinBought(idGift, out flag);
		}
		case GiftCategoryType.ArmorAndHat:
		{
			string b = Wear.ArmorOrArmorHatAvailableForBuy(ShopNGUIController.CategoryNames.ArmorCategory);
			return idGift == b;
		}
		case GiftCategoryType.Wear:
			return !ItemDb.IsItemInInventory(idGift);
		case GiftCategoryType.Editor:
			return !idGift.IsNullOrEmpty() && (!(idGift != "editor_Cape") || !(idGift != "editor_Skin")) && (!(idGift == "editor_Skin") || Storager.getInt(Defs.SkinsMakerInProfileBought, false) <= 0) && (!(idGift == "editor_Cape") || Storager.getInt("cape_Custom", false) <= 0);
		case GiftCategoryType.Masks:
		case GiftCategoryType.Boots:
		case GiftCategoryType.Hats_random:
			return !idGift.IsNullOrEmpty() && Storager.getInt(idGift, true) == 0;
		case GiftCategoryType.Capes:
			return !idGift.IsNullOrEmpty() && Storager.getInt(idGift, true) == 0;
		case GiftCategoryType.Gun1:
		case GiftCategoryType.Gun2:
		case GiftCategoryType.Gun3:
		case GiftCategoryType.Gun4:
			return Storager.getInt(idGift, true) == 0;
		case GiftCategoryType.Guns_gray:
		{
			if (idGift.IsNullOrEmpty())
			{
				return false;
			}
			ItemRecord itemRecord = GiftController.GrayCategoryWeapons[ExpController.OurTierForAnyPlace()].FirstOrDefault((ItemRecord rec) => rec.Tag == idGift);
			return itemRecord != null && Storager.getInt(itemRecord.StorageId, true) == 0;
		}
		case GiftCategoryType.Stickers:
		{
			TypePackSticker? typePackSticker = idGift.ToEnum(null);
			return typePackSticker != null && !StickersController.IsBuyPack(typePackSticker.Value);
		}
		case GiftCategoryType.WeaponSkin:
			return !WeaponSkinsManager.IsBoughtSkin(idGift);
		case GiftCategoryType.Gadgets:
			return !GadgetsInfo.IsBought(idGift);
		}
		return false;
	}

	// Token: 0x060037BF RID: 14271 RVA: 0x0011F3AC File Offset: 0x0011D5AC
	private static List<string> GetAvailableProducts(ShopNGUIController.CategoryNames category, int maxTier = -1, string[] withoutIds = null)
	{
		if (maxTier < 0)
		{
			maxTier = ExpController.OurTierForAnyPlace();
		}
		List<string> list = Wear.AllWears(category, maxTier, true, true);
		List<string> list2 = PromoActionsGUIController.FilterPurchases(list, true, false, false, true);
		if (withoutIds != null)
		{
			list2.AddRange(withoutIds);
		}
		return list.Except(list2).ToList<string>();
	}

	// Token: 0x060037C0 RID: 14272 RVA: 0x0011F3F8 File Offset: 0x0011D5F8
	public GiftInfo GetRandomGift()
	{
		if (this._availableGifts == null || this._availableGifts.Count == 0)
		{
			return null;
		}
		if (this._availableGiftsPercentSum < 0f)
		{
			return null;
		}
		float num = UnityEngine.Random.Range(0f, this._availableGiftsPercentSum);
		float num2 = 0f;
		GiftInfo result = null;
		for (int i = 0; i < this._availableGifts.Count; i++)
		{
			GiftInfo giftInfo = this._availableGifts[i];
			num2 += giftInfo.PercentAddInSlot;
			if (num2 > num)
			{
				result = giftInfo;
				break;
			}
		}
		return result;
	}

	// Token: 0x060037C1 RID: 14273 RVA: 0x0011F494 File Offset: 0x0011D694
	private List<GiftInfo> GetAvailableGifts()
	{
		GiftCategory.<GetAvailableGifts>c__AnonStorey321 <GetAvailableGifts>c__AnonStorey = new GiftCategory.<GetAvailableGifts>c__AnonStorey321();
		<GetAvailableGifts>c__AnonStorey.res = new List<GiftInfo>();
		GiftInfo root;
		foreach (GiftInfo root2 in this._rootGifts)
		{
			root = root2;
			if (root.Id == "guns_gray")
			{
				List<string> availableGrayWeaponsTags = GiftController.GetAvailableGrayWeaponsTags();
				availableGrayWeaponsTags.ForEach(delegate(string w)
				{
					<GetAvailableGifts>c__AnonStorey.res.Add(GiftInfo.CreateInfo(root, w, 1));
				});
			}
			else if (root.Id == "equip_Mask")
			{
				List<string> availableProducts = GiftCategory.GetAvailableProducts(ShopNGUIController.CategoryNames.MaskCategory, -1, null);
				availableProducts.ForEach(delegate(string p)
				{
					<GetAvailableGifts>c__AnonStorey.res.Add(GiftInfo.CreateInfo(root, p, 1));
				});
			}
			else if (root.Id == "equip_Cape")
			{
				List<string> availableProducts2 = GiftCategory.GetAvailableProducts(ShopNGUIController.CategoryNames.CapesCategory, -1, new string[]
				{
					"cape_Custom"
				});
				availableProducts2.ForEach(delegate(string p)
				{
					<GetAvailableGifts>c__AnonStorey.res.Add(GiftInfo.CreateInfo(root, p, 1));
				});
			}
			else if (root.Id == "equip_Boots")
			{
				List<string> availableProducts3 = GiftCategory.GetAvailableProducts(ShopNGUIController.CategoryNames.BootsCategory, -1, null);
				availableProducts3.ForEach(delegate(string p)
				{
					<GetAvailableGifts>c__AnonStorey.res.Add(GiftInfo.CreateInfo(root, p, 1));
				});
			}
			else if (root.Id == "equip_Hat")
			{
				List<string> availableProducts4 = GiftCategory.GetAvailableProducts(ShopNGUIController.CategoryNames.HatsCategory, -1, null);
				availableProducts4.ForEach(delegate(string p)
				{
					<GetAvailableGifts>c__AnonStorey.res.Add(GiftInfo.CreateInfo(root, p, 1));
				});
			}
			else if (root.Id == "random")
			{
				List<WeaponSkin> availableForBuySkins = WeaponSkinsManager.GetAvailableForBuySkins();
				availableForBuySkins.ForEach(delegate(WeaponSkin s)
				{
					<GetAvailableGifts>c__AnonStorey.res.Add(GiftInfo.CreateInfo(root, s.Id, 1));
				});
			}
			else if (root.Id == "gadget_random")
			{
				IEnumerable<string> enumeration = GadgetsInfo.AvailableForBuyGadgets(ExpController.OurTierForAnyPlace());
				enumeration.ForEach(delegate(string g)
				{
					<GetAvailableGifts>c__AnonStorey.res.Add(GiftInfo.CreateInfo(root, g, 1));
				});
			}
			else
			{
				<GetAvailableGifts>c__AnonStorey.res.Add(root);
			}
		}
		return <GetAvailableGifts>c__AnonStorey.res;
	}

	// Token: 0x04002884 RID: 10372
	public GiftCategoryType Type;

	// Token: 0x04002885 RID: 10373
	public int ScrollPosition;

	// Token: 0x04002886 RID: 10374
	public string KeyTranslateInfoCommon = string.Empty;

	// Token: 0x04002887 RID: 10375
	private readonly List<GiftInfo> _rootGifts = new List<GiftInfo>();

	// Token: 0x04002888 RID: 10376
	private List<GiftInfo> _ag;

	// Token: 0x04002889 RID: 10377
	private List<string> _availableRandomProducts;
}
