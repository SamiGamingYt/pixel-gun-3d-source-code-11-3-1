using System;
using System.Collections.Generic;
using Rilisoft;
using UnityEngine;

// Token: 0x020006B8 RID: 1720
public sealed class MarafonBonusController
{
	// Token: 0x06003BF4 RID: 15348 RVA: 0x00136E00 File Offset: 0x00135000
	public MarafonBonusController()
	{
		this.CurrentBonus = null;
		this.InitializeBonusItems();
		this._currentBonusIndex = Storager.getInt(Defs.NextMarafonBonusIndex, false);
	}

	// Token: 0x170009D4 RID: 2516
	// (get) Token: 0x06003BF7 RID: 15351 RVA: 0x001375C4 File Offset: 0x001357C4
	// (set) Token: 0x06003BF6 RID: 15350 RVA: 0x001375B8 File Offset: 0x001357B8
	public List<BonusMarafonItem> BonusItems { get; private set; }

	// Token: 0x170009D5 RID: 2517
	// (get) Token: 0x06003BF9 RID: 15353 RVA: 0x001375D8 File Offset: 0x001357D8
	// (set) Token: 0x06003BF8 RID: 15352 RVA: 0x001375CC File Offset: 0x001357CC
	public BonusMarafonItem CurrentBonus { get; private set; }

	// Token: 0x170009D6 RID: 2518
	// (get) Token: 0x06003BFA RID: 15354 RVA: 0x001375E0 File Offset: 0x001357E0
	public static MarafonBonusController Get
	{
		get
		{
			if (MarafonBonusController._instance == null)
			{
				MarafonBonusController._instance = new MarafonBonusController();
			}
			return MarafonBonusController._instance;
		}
	}

	// Token: 0x06003BFB RID: 15355 RVA: 0x001375FC File Offset: 0x001357FC
	public void TakeMarafonBonus()
	{
		int value = this._currentBonusIndex.Value;
		this.TakeBonusPlayer(value);
	}

	// Token: 0x06003BFC RID: 15356 RVA: 0x0013761C File Offset: 0x0013581C
	private static int GetCountForDayForCurrentPremiumLevel(int day)
	{
		int num = (int)((!(PremiumAccountController.Instance != null)) ? PremiumAccountController.AccountType.None : PremiumAccountController.Instance.GetCurrentAccount());
		day--;
		if (MarafonBonusController.countsForPremiumAccountLevels.Count > num && MarafonBonusController.countsForPremiumAccountLevels[num].Count > day && day >= 0)
		{
			return MarafonBonusController.countsForPremiumAccountLevels[num][day].Value;
		}
		return 0;
	}

	// Token: 0x06003BFD RID: 15357 RVA: 0x00137698 File Offset: 0x00135898
	public void InitializeBonusItems()
	{
		this.BonusItems = new List<BonusMarafonItem>();
		this.BonusItems.Add(new BonusMarafonItem(BonusItemType.Gold, MarafonBonusController.GetCountForDayForCurrentPremiumLevel(1), "bonus_coins", null));
		this.BonusItems.Add(new BonusMarafonItem(BonusItemType.Gold, MarafonBonusController.GetCountForDayForCurrentPremiumLevel(2), "bonus_coins", null));
		this.BonusItems.Add(new BonusMarafonItem(BonusItemType.Real, MarafonBonusController.GetCountForDayForCurrentPremiumLevel(3), "bonus_gems", null));
		this.BonusItems.Add(new BonusMarafonItem(BonusItemType.Gold, MarafonBonusController.GetCountForDayForCurrentPremiumLevel(4), "bonus_coins", null));
		this.BonusItems.Add(new BonusMarafonItem(BonusItemType.Granade, MarafonBonusController.GetCountForDayForCurrentPremiumLevel(5), "bonus_grenade", null));
		this.BonusItems.Add(new BonusMarafonItem(BonusItemType.Gold, MarafonBonusController.GetCountForDayForCurrentPremiumLevel(6), "bonus_coins", null));
		this.BonusItems.Add(new BonusMarafonItem(BonusItemType.PotionInvisible, MarafonBonusController.GetCountForDayForCurrentPremiumLevel(7), "bonus_potion", null));
		this.BonusItems.Add(new BonusMarafonItem(BonusItemType.Gold, MarafonBonusController.GetCountForDayForCurrentPremiumLevel(8), "bonus_coins", null));
		this.BonusItems.Add(new BonusMarafonItem(BonusItemType.Gold, MarafonBonusController.GetCountForDayForCurrentPremiumLevel(9), "bonus_coins", null));
		this.BonusItems.Add(new BonusMarafonItem(BonusItemType.Real, MarafonBonusController.GetCountForDayForCurrentPremiumLevel(10), "bonus_gems", null));
		this.BonusItems.Add(new BonusMarafonItem(BonusItemType.Gold, MarafonBonusController.GetCountForDayForCurrentPremiumLevel(11), "bonus_coins", null));
		this.BonusItems.Add(new BonusMarafonItem(BonusItemType.Granade, MarafonBonusController.GetCountForDayForCurrentPremiumLevel(12), "bonus_grenade", null));
		this.BonusItems.Add(new BonusMarafonItem(BonusItemType.Gold, MarafonBonusController.GetCountForDayForCurrentPremiumLevel(13), "bonus_coins", null));
		this.BonusItems.Add(new BonusMarafonItem(BonusItemType.JetPack, MarafonBonusController.GetCountForDayForCurrentPremiumLevel(14), "bonus_jetpack", null));
		this.BonusItems.Add(new BonusMarafonItem(BonusItemType.Gold, MarafonBonusController.GetCountForDayForCurrentPremiumLevel(15), "bonus_coins", null));
		this.BonusItems.Add(new BonusMarafonItem(BonusItemType.Gold, MarafonBonusController.GetCountForDayForCurrentPremiumLevel(16), "bonus_coins", null));
		this.BonusItems.Add(new BonusMarafonItem(BonusItemType.Real, MarafonBonusController.GetCountForDayForCurrentPremiumLevel(17), "bonus_gems", null));
		this.BonusItems.Add(new BonusMarafonItem(BonusItemType.Gold, MarafonBonusController.GetCountForDayForCurrentPremiumLevel(18), "bonus_coins", null));
		this.BonusItems.Add(new BonusMarafonItem(BonusItemType.Granade, MarafonBonusController.GetCountForDayForCurrentPremiumLevel(19), "bonus_grenade", null));
		this.BonusItems.Add(new BonusMarafonItem(BonusItemType.Gold, MarafonBonusController.GetCountForDayForCurrentPremiumLevel(20), "bonus_coins", null));
		this.BonusItems.Add(new BonusMarafonItem(BonusItemType.Turret, MarafonBonusController.GetCountForDayForCurrentPremiumLevel(21), "bonus_turret", null));
		this.BonusItems.Add(new BonusMarafonItem(BonusItemType.Gold, MarafonBonusController.GetCountForDayForCurrentPremiumLevel(22), "bonus_coins", null));
		this.BonusItems.Add(new BonusMarafonItem(BonusItemType.Gold, MarafonBonusController.GetCountForDayForCurrentPremiumLevel(23), "bonus_coins", null));
		this.BonusItems.Add(new BonusMarafonItem(BonusItemType.Real, MarafonBonusController.GetCountForDayForCurrentPremiumLevel(24), "bonus_gems", null));
		this.BonusItems.Add(new BonusMarafonItem(BonusItemType.Gold, MarafonBonusController.GetCountForDayForCurrentPremiumLevel(25), "bonus_coins", null));
		this.BonusItems.Add(new BonusMarafonItem(BonusItemType.Granade, MarafonBonusController.GetCountForDayForCurrentPremiumLevel(26), "bonus_grenade", null));
		this.BonusItems.Add(new BonusMarafonItem(BonusItemType.Gold, MarafonBonusController.GetCountForDayForCurrentPremiumLevel(27), "bonus_coins", null));
		this.BonusItems.Add(new BonusMarafonItem(BonusItemType.Mech, MarafonBonusController.GetCountForDayForCurrentPremiumLevel(28), "bonus_mech", null));
		this.BonusItems.Add(new BonusMarafonItem(BonusItemType.Gold, MarafonBonusController.GetCountForDayForCurrentPremiumLevel(29), "bonus_coins", null));
		this.BonusItems.Add(new BonusMarafonItem(BonusItemType.Mech, MarafonBonusController.GetCountForDayForCurrentPremiumLevel(30), "bonus_mech", null));
	}

	// Token: 0x06003BFE RID: 15358 RVA: 0x00137A2C File Offset: 0x00135C2C
	private void AddGearForPlayer(string gearId, int addCount)
	{
		int @int = Storager.getInt(gearId, false);
		int val = @int + addCount;
		Storager.setInt(gearId, val, false);
	}

	// Token: 0x06003BFF RID: 15359 RVA: 0x00137A50 File Offset: 0x00135C50
	private void TakeBonusPlayer(int indexBonus)
	{
		if (this.BonusItems.Count == 0)
		{
			return;
		}
		this.CurrentBonus = this.BonusItems[indexBonus];
		switch (this.CurrentBonus.type)
		{
		case BonusItemType.Gold:
			BankController.AddCoins(this.CurrentBonus.count.Value, true, AnalyticsConstants.AccrualType.Earned);
			break;
		case BonusItemType.Real:
			BankController.AddGems(this.CurrentBonus.count.Value, true, AnalyticsConstants.AccrualType.Earned);
			break;
		case BonusItemType.PotionInvisible:
			this.AddGearForPlayer(GearManager.InvisibilityPotion, this.CurrentBonus.count.Value);
			break;
		case BonusItemType.JetPack:
			this.AddGearForPlayer(GearManager.Jetpack, this.CurrentBonus.count.Value);
			break;
		case BonusItemType.Granade:
			this.AddGearForPlayer(GearManager.Grenade, this.CurrentBonus.count.Value);
			break;
		case BonusItemType.Turret:
			this.AddGearForPlayer(GearManager.Turret, this.CurrentBonus.count.Value);
			break;
		case BonusItemType.Mech:
			this.AddGearForPlayer(GearManager.Mech, this.CurrentBonus.count.Value);
			break;
		case BonusItemType.TemporaryWeapon:
		{
			ShopNGUIController.CategoryNames itemCategory = (ShopNGUIController.CategoryNames)ItemDb.GetItemCategory(this.CurrentBonus.tag);
			TempItemsController.sharedController.TakeTemporaryItemToPlayer(itemCategory, this.CurrentBonus.tag, TempItemsController.RentIndexFromDays(this.CurrentBonus.count.Value));
			break;
		}
		}
		if (indexBonus + 1 >= this.BonusItems.Count)
		{
			Storager.setInt(Defs.NextMarafonBonusIndex, 0, false);
		}
		else
		{
			Storager.setInt(Defs.NextMarafonBonusIndex, indexBonus + 1, false);
		}
		this._currentBonusIndex.Value = Storager.getInt(Defs.NextMarafonBonusIndex, false);
		Storager.setInt(Defs.NeedTakeMarathonBonus, 0, false);
		if (Application.platform != RuntimePlatform.IPhonePlayer)
		{
			PlayerPrefs.Save();
		}
	}

	// Token: 0x06003C00 RID: 15360 RVA: 0x00137C34 File Offset: 0x00135E34
	public bool IsAvailable()
	{
		int value = this._currentBonusIndex.Value;
		return value != -1;
	}

	// Token: 0x06003C01 RID: 15361 RVA: 0x00137C54 File Offset: 0x00135E54
	public bool IsNeedShow()
	{
		return Storager.getInt(Defs.NeedTakeMarathonBonus, false) == 1;
	}

	// Token: 0x06003C02 RID: 15362 RVA: 0x00137C64 File Offset: 0x00135E64
	public int GetCurrentBonusIndex()
	{
		return this._currentBonusIndex.Value;
	}

	// Token: 0x06003C03 RID: 15363 RVA: 0x00137C74 File Offset: 0x00135E74
	public bool IsBonusTemporaryWeapon()
	{
		BonusMarafonItem bonusMarafonItem = this.BonusItems[this._currentBonusIndex.Value];
		return bonusMarafonItem.type == BonusItemType.TemporaryWeapon;
	}

	// Token: 0x06003C04 RID: 15364 RVA: 0x00137CA4 File Offset: 0x00135EA4
	public BonusMarafonItem GetBonusForIndex(int index)
	{
		if (this.BonusItems == null || this.BonusItems.Count == 0)
		{
			return null;
		}
		if (index < 0 || index >= this.BonusItems.Count)
		{
			return null;
		}
		return this.BonusItems[index];
	}

	// Token: 0x06003C05 RID: 15365 RVA: 0x00137CF4 File Offset: 0x00135EF4
	public void ResetSessionState()
	{
		if (BannerWindowController.SharedController != null)
		{
			BannerWindowController.SharedController.ResetStateBannerShowed(BannerWindowType.GiftBonuse);
		}
	}

	// Token: 0x04002C60 RID: 11360
	private SaltedInt _currentBonusIndex;

	// Token: 0x04002C61 RID: 11361
	private static MarafonBonusController _instance;

	// Token: 0x04002C62 RID: 11362
	private static List<List<SaltedInt>> countsForPremiumAccountLevels = new List<List<SaltedInt>>
	{
		new List<SaltedInt>
		{
			6,
			6,
			6,
			6,
			6,
			6,
			2,
			6,
			6,
			6,
			6,
			6,
			6,
			2,
			6,
			6,
			6,
			6,
			6,
			6,
			2,
			6,
			6,
			6,
			6,
			12,
			6,
			2,
			6,
			6
		},
		new List<SaltedInt>
		{
			7,
			7,
			7,
			7,
			7,
			7,
			3,
			7,
			7,
			7,
			7,
			7,
			7,
			3,
			7,
			7,
			7,
			7,
			7,
			7,
			3,
			7,
			7,
			7,
			7,
			14,
			7,
			3,
			7,
			7
		},
		new List<SaltedInt>
		{
			8,
			8,
			8,
			8,
			8,
			8,
			4,
			8,
			8,
			8,
			8,
			8,
			8,
			4,
			8,
			8,
			8,
			8,
			8,
			8,
			4,
			8,
			8,
			8,
			8,
			16,
			8,
			4,
			8,
			8
		},
		new List<SaltedInt>
		{
			10,
			10,
			10,
			10,
			10,
			10,
			5,
			10,
			10,
			10,
			10,
			10,
			10,
			5,
			10,
			10,
			10,
			10,
			10,
			10,
			5,
			10,
			10,
			10,
			10,
			20,
			10,
			5,
			10,
			10
		},
		new List<SaltedInt>
		{
			5,
			5,
			5,
			5,
			5,
			5,
			1,
			5,
			5,
			5,
			5,
			5,
			5,
			1,
			5,
			5,
			5,
			5,
			5,
			5,
			1,
			5,
			5,
			5,
			5,
			10,
			5,
			1,
			5,
			5
		}
	};
}
