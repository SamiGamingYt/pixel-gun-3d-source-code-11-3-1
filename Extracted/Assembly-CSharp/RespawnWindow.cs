using System;
using System.Collections.Generic;
using System.Linq;
using Rilisoft;
using UnityEngine;

// Token: 0x02000802 RID: 2050
public sealed class RespawnWindow : MonoBehaviour
{
	// Token: 0x17000C42 RID: 3138
	// (get) Token: 0x06004AA0 RID: 19104 RVA: 0x001A7B0C File Offset: 0x001A5D0C
	public static RespawnWindow Instance
	{
		get
		{
			return RespawnWindow._instance;
		}
	}

	// Token: 0x17000C43 RID: 3139
	// (get) Token: 0x06004AA1 RID: 19105 RVA: 0x001A7B14 File Offset: 0x001A5D14
	public bool isShown
	{
		get
		{
			return base.gameObject.activeSelf;
		}
	}

	// Token: 0x06004AA2 RID: 19106 RVA: 0x001A7B24 File Offset: 0x001A5D24
	private void Start()
	{
		if (this.coinsShopButton != null)
		{
			ButtonHandler component = this.coinsShopButton.GetComponent<ButtonHandler>();
			if (component != null)
			{
				component.Clicked += this.OnToBankClicked;
			}
		}
	}

	// Token: 0x06004AA3 RID: 19107 RVA: 0x001A7B6C File Offset: 0x001A5D6C
	public void Show(KillerInfo inKillerInfo)
	{
		KillerInfo killerInfo = new KillerInfo();
		inKillerInfo.CopyTo(killerInfo);
		this.killerLevelNicknameLabel.text = killerInfo.nickname;
		this.killerRank.mainTexture = killerInfo.rankTex;
		this.killerClanLogo.mainTexture = killerInfo.clanLogoTex;
		this.killerClanNameLabel.text = killerInfo.clanName;
		this.FillItemsToBuy(killerInfo);
		this.FillEquipments(killerInfo);
		this.FillStats(killerInfo);
		Defs.inRespawnWindow = true;
		base.gameObject.SetActive(true);
		RespawnWindow._instance = this;
		this.characterViewHolder.SetActive(true);
		this.SetKillerNameVisible(false);
		this._originalTimeout = 15f;
	}

	// Token: 0x06004AA4 RID: 19108 RVA: 0x001A7C18 File Offset: 0x001A5E18
	public void ShowCharacter(KillerInfo killerInfo)
	{
		this.characterView.ShowCharacterType(CharacterView.CharacterType.Player);
		this.characterView.characterInterface.usePetFromStorager = false;
		this.characterView.SetWeaponAndSkin(killerInfo.weapon, killerInfo.skinTex, true);
		if (!string.IsNullOrEmpty(killerInfo.hat))
		{
			this.characterView.UpdateHat(killerInfo.hat);
		}
		else
		{
			this.characterView.RemoveHat();
		}
		if (!string.IsNullOrEmpty(killerInfo.cape))
		{
			this.characterView.UpdateCape(killerInfo.cape, killerInfo.capeTex);
		}
		else
		{
			this.characterView.RemoveCape();
		}
		if (!string.IsNullOrEmpty(killerInfo.mask))
		{
			this.characterView.UpdateMask(killerInfo.mask);
		}
		else
		{
			this.characterView.RemoveMask();
		}
		if (!string.IsNullOrEmpty(killerInfo.boots))
		{
			this.characterView.UpdateBoots(killerInfo.boots);
		}
		else
		{
			this.characterView.RemoveBoots();
		}
		if (!string.IsNullOrEmpty(killerInfo.armor))
		{
			this.characterView.UpdateArmor(killerInfo.armor);
		}
		else
		{
			this.characterView.RemoveArmor();
		}
		this.characterViewHolder.SetActive(true);
		this.characterViewCamera.gameObject.SetActive(true);
		this.characterView.gameObject.SetActive(true);
		this.SetKillerNameVisible(true);
	}

	// Token: 0x06004AA5 RID: 19109 RVA: 0x001A7D8C File Offset: 0x001A5F8C
	public void CloseRespawnWindow()
	{
		this.RespawnPlayer();
		this.Hide();
	}

	// Token: 0x06004AA6 RID: 19110 RVA: 0x001A7D9C File Offset: 0x001A5F9C
	public void SetCurrentTime(float sec)
	{
		this.autoRespawnTimerLabel.text = Mathf.CeilToInt(Mathf.Max(0f, sec)).ToString();
	}

	// Token: 0x06004AA7 RID: 19111 RVA: 0x001A7DCC File Offset: 0x001A5FCC
	private void RespawnPlayer()
	{
		Player_move_c myPlayerMoveC = WeaponManager.sharedManager.myPlayerMoveC;
		if (myPlayerMoveC != null)
		{
			myPlayerMoveC.RespawnPlayer();
		}
	}

	// Token: 0x06004AA8 RID: 19112 RVA: 0x001A7DF8 File Offset: 0x001A5FF8
	public void Hide()
	{
		base.gameObject.SetActive(false);
		this.characterViewHolder.SetActive(false);
		this.characterView.gameObject.SetActive(false);
		this.Reset();
		Defs.inRespawnWindow = false;
		RespawnWindow._instance = null;
	}

	// Token: 0x06004AA9 RID: 19113 RVA: 0x001A7E40 File Offset: 0x001A6040
	public void OnBtnGoBattleClick()
	{
		this.RespawnPlayer();
		this.Hide();
	}

	// Token: 0x06004AAA RID: 19114 RVA: 0x001A7E50 File Offset: 0x001A6050
	private void FillEquipments(KillerInfo killerInfo)
	{
		this.hatItem.SetItemTag(killerInfo.hat, 6);
		this.maskItem.SetItemTag(killerInfo.mask, 12);
		this.armorItem.SetItemTag(killerInfo.armor, 7);
		this.capeItem.SetItemTag(killerInfo.cape, 9);
		this.bootsItem.SetItemTag(killerInfo.boots, 10);
		this.petItem.SetItemTag(killerInfo.pet, 25000);
		this.gadgetSupportItem.SetItemTag(killerInfo.gadgetSupport, 13500);
		this.gadgetTrowingItem.SetItemTag(killerInfo.gadgetTrowing, 12500);
		this.gadgetToolsItem.SetItemTag(killerInfo.gadgetTools, 13000);
	}

	// Token: 0x06004AAB RID: 19115 RVA: 0x001A7F14 File Offset: 0x001A6114
	private void FillItemsToBuy(KillerInfo killerInfo)
	{
		try
		{
			List<string> list = (BuildSettings.BuildTargetPlatform != RuntimePlatform.MetroPlayerX64) ? this.GetWeaponsForBuy() : new List<string>();
			string itemTag = (list.Count <= 0) ? null : list[0];
			string text = (BuildSettings.BuildTargetPlatform != RuntimePlatform.MetroPlayerX64) ? this.GetArmorForBuy() : null;
			if (string.IsNullOrEmpty(text))
			{
				text = ((list.Count <= 1) ? null : list[1]);
			}
			if (killerInfo != null && killerInfo.weapon != null)
			{
				string weapon = killerInfo.weapon;
				int? upgradeNum = null;
				bool flag = GearManager.IsItemGear(weapon);
				if (flag)
				{
					if (weapon == GearManager.Turret)
					{
						upgradeNum = new int?(1 + killerInfo.turretUpgrade);
					}
					else if (weapon == GearManager.Mech)
					{
						upgradeNum = new int?(1 + killerInfo.mechUpgrade);
					}
				}
				this.killerWeapon.SetWeaponTag(weapon, upgradeNum);
			}
			else
			{
				this.killerWeapon.SetWeaponTag(string.Empty, new int?(0));
			}
			this.recommendedWeapon.SetWeaponTag(itemTag, null);
			this.recommendedArmor.SetWeaponTag(text, null);
		}
		catch (Exception exception)
		{
			Debug.LogException(exception);
		}
		finally
		{
		}
	}

	// Token: 0x06004AAC RID: 19116 RVA: 0x001A80AC File Offset: 0x001A62AC
	public void OnItemToBuyClick(RespawnWindowItemToBuy itemToBuy)
	{
		if (itemToBuy.itemTag == null || itemToBuy.itemPrice == null)
		{
			return;
		}
		int priceAmount = itemToBuy.itemPrice.Price;
		string priceCurrency = itemToBuy.itemPrice.Currency;
		ShopNGUIController.TryToBuy(base.gameObject, itemToBuy.itemPrice, delegate
		{
			if (Defs.isSoundFX)
			{
				UIPlaySound component = itemToBuy.btnBuy.GetComponent<UIPlaySound>();
				if (component != null)
				{
					component.Play();
				}
			}
			ShopNGUIController.CategoryNames itemCategory = (ShopNGUIController.CategoryNames)itemToBuy.itemCategory;
			if (itemCategory == ShopNGUIController.CategoryNames.ArmorCategory || ShopNGUIController.IsWeaponCategory(itemCategory))
			{
				ShopNGUIController.FireWeaponOrArmorBought();
			}
			int num = 1;
			if (GearManager.IsItemGear(itemToBuy.itemTag))
			{
				num = GearManager.ItemsInPackForGear(itemToBuy.itemTag);
				if (itemToBuy.itemTag == GearManager.Grenade)
				{
					int b = Defs2.MaxGrenadeCount - Storager.getInt(itemToBuy.itemTag, false);
					num = Mathf.Min(num, b);
				}
			}
			ShopNGUIController.ProvideItem(itemCategory, (!ShopNGUIController.IsWeaponCategory(itemCategory)) ? itemToBuy.itemTag : WeaponManager.FirstUnboughtOrForOurTier(itemToBuy.itemTag), num, false, 0, delegate(string item)
			{
				if (ShopNGUIController.sharedShop != null)
				{
					ShopNGUIController.sharedShop.FireBuyAction(item);
				}
			}, null, true, true, false);
			this.killerWeapon.SetWeaponTag(this.killerWeapon.itemTag, null);
			this.recommendedWeapon.SetWeaponTag(this.recommendedWeapon.itemTag, null);
			this.recommendedArmor.SetWeaponTag(this.recommendedArmor.itemTag, null);
			try
			{
				string empty = string.Empty;
				string itemNameNonLocalized = ItemDb.GetItemNameNonLocalized(WeaponManager.LastBoughtTag(itemToBuy.itemTag, null) ?? WeaponManager.FirstUnboughtTag(itemToBuy.itemTag), empty, itemCategory, null);
				bool isDaterWeapon = false;
				if (ShopNGUIController.IsWeaponCategory(itemCategory))
				{
					WeaponSounds weaponInfo = ItemDb.GetWeaponInfo(itemToBuy.itemTag);
					isDaterWeapon = (weaponInfo != null && weaponInfo.IsAvalibleFromFilter(3));
				}
				string categoryParameterName = AnalyticsConstants.GetSalesName(itemCategory) ?? itemCategory.ToString();
				AnalyticsStuff.LogSales(itemNameNonLocalized, categoryParameterName, isDaterWeapon);
				AnalyticsFacade.InAppPurchase(itemNameNonLocalized, AnalyticsStuff.AnalyticsReadableCategoryNameFromOldCategoryName(categoryParameterName), 1, priceAmount, priceCurrency);
			}
			catch (Exception arg)
			{
				Debug.LogError("Exception in loggin in Respawn Window: " + arg);
			}
		}, null, null, null, delegate
		{
			this.SetPaused(true);
		}, delegate
		{
			this.SetPaused(false);
		});
	}

	// Token: 0x06004AAD RID: 19117 RVA: 0x001A8158 File Offset: 0x001A6358
	private void FillStats(KillerInfo killerInfo)
	{
		int armorCountFor = Wear.GetArmorCountFor(killerInfo.armor, killerInfo.hat);
		if (armorCountFor > 0)
		{
			this.armorObj.SetActive(true);
			this.healthBackground.SetActive(false);
			this.healtharmorBackground.SetActive(true);
			this.armorCountLabel.text = string.Format("{0}/{1}", Mathf.Min(armorCountFor, killerInfo.armorValue), armorCountFor);
			this.healthTable.repositionNow = true;
		}
		else
		{
			this.armorObj.SetActive(false);
			this.healthBackground.SetActive(true);
			this.healtharmorBackground.SetActive(false);
			this.healthTable.repositionNow = true;
		}
		this.healthIcon.SetActive(true);
		this.healthCountLabel.text = string.Format("{0}/{1}", killerInfo.healthValue, ExperienceController.HealthByLevel[killerInfo.rank]);
	}

	// Token: 0x06004AAE RID: 19118 RVA: 0x001A824C File Offset: 0x001A644C
	private void OnBackFromBankClicked(object sender, EventArgs e)
	{
		BankController.Instance.BackRequested -= this.OnBackFromBankClicked;
		BankController.Instance.InterfaceEnabled = false;
		if (this != null)
		{
			base.gameObject.SetActive(true);
		}
		this.SetPaused(false);
	}

	// Token: 0x06004AAF RID: 19119 RVA: 0x001A829C File Offset: 0x001A649C
	private void OnToBankClicked(object sender, EventArgs e)
	{
		this.ShowBankWindow();
	}

	// Token: 0x06004AB0 RID: 19120 RVA: 0x001A82A4 File Offset: 0x001A64A4
	private void ShowBankWindow()
	{
		ButtonClickSound.Instance.PlayClick();
		BankController.Instance.BackRequested += this.OnBackFromBankClicked;
		BankController.Instance.InterfaceEnabled = true;
		this.SetPaused(true);
		if (this != null)
		{
			base.gameObject.SetActive(false);
		}
	}

	// Token: 0x06004AB1 RID: 19121 RVA: 0x001A82FC File Offset: 0x001A64FC
	private void SetPaused(bool paused)
	{
	}

	// Token: 0x06004AB2 RID: 19122 RVA: 0x001A8300 File Offset: 0x001A6500
	private List<string> GetWeaponsForBuy()
	{
		List<string> weaponsForBuy = WeaponManager.GetWeaponsForBuy();
		this.SortWeaponsByDps(weaponsForBuy);
		return weaponsForBuy;
	}

	// Token: 0x06004AB3 RID: 19123 RVA: 0x001A831C File Offset: 0x001A651C
	private string GetArmorForBuy()
	{
		List<string> list = new List<string>();
		list.AddRange(Wear.wear[ShopNGUIController.CategoryNames.ArmorCategory][0]);
		bool filterNextTierUpgrades = true;
		List<string> list2 = PromoActionsGUIController.FilterPurchases(list, filterNextTierUpgrades, true, false, true);
		foreach (string text in list)
		{
			if (TempItemsController.PriceCoefs.ContainsKey(text) && !list2.Contains(text))
			{
				list2.Add(text);
			}
		}
		foreach (string item in list2)
		{
			list.Remove(item);
		}
		return list.FirstOrDefault<string>();
	}

	// Token: 0x06004AB4 RID: 19124 RVA: 0x001A8420 File Offset: 0x001A6620
	private void SortWeaponsByDps(List<string> weaponTags)
	{
		weaponTags.Sort(delegate(string weaponTag1, string weaponTag2)
		{
			WeaponSounds weaponInfo = ItemDb.GetWeaponInfo(weaponTag1);
			if (weaponInfo == null)
			{
				return 1;
			}
			WeaponSounds weaponInfo2 = ItemDb.GetWeaponInfo(weaponTag2);
			if (weaponInfo2 == null)
			{
				return -1;
			}
			return weaponInfo2.DPS.CompareTo(weaponInfo.DPS);
		});
	}

	// Token: 0x06004AB5 RID: 19125 RVA: 0x001A8448 File Offset: 0x001A6648
	private void SetKillerNameVisible(bool visible)
	{
		this.killerLevelNicknameLabel.gameObject.SetActive(visible);
		this.killerRank.gameObject.SetActive(visible);
		this.killerClanNameLabel.gameObject.SetActive(visible);
		this.killerClanLogo.gameObject.SetActive(visible);
	}

	// Token: 0x06004AB6 RID: 19126 RVA: 0x001A849C File Offset: 0x001A669C
	private void Reset()
	{
		this.killerWeapon.Reset();
		this.recommendedWeapon.Reset();
		this.recommendedArmor.Reset();
		this.hatItem.ResetImage();
		this.maskItem.ResetImage();
		this.armorItem.ResetImage();
		this.capeItem.ResetImage();
		this.bootsItem.ResetImage();
		this.petItem.ResetImage();
		this.gadgetSupportItem.ResetImage();
		this.gadgetTrowingItem.ResetImage();
		this.gadgetToolsItem.ResetImage();
	}

	// Token: 0x06004AB7 RID: 19127 RVA: 0x001A8530 File Offset: 0x001A6730
	private void OnEnable()
	{
		if (!Defs.inRespawnWindow)
		{
			base.gameObject.SetActive(false);
		}
	}

	// Token: 0x0400372D RID: 14125
	public UILabel killerLevelNicknameLabel;

	// Token: 0x0400372E RID: 14126
	public UITexture killerRank;

	// Token: 0x0400372F RID: 14127
	public UILabel killerClanNameLabel;

	// Token: 0x04003730 RID: 14128
	public UITexture killerClanLogo;

	// Token: 0x04003731 RID: 14129
	public UILabel autoRespawnTitleLabel;

	// Token: 0x04003732 RID: 14130
	public UILabel autoRespawnTimerLabel;

	// Token: 0x04003733 RID: 14131
	public GameObject characterViewHolder;

	// Token: 0x04003734 RID: 14132
	public Camera characterViewCamera;

	// Token: 0x04003735 RID: 14133
	public UITexture characterViewTexture;

	// Token: 0x04003736 RID: 14134
	public CharacterView characterView;

	// Token: 0x04003737 RID: 14135
	public RespawnWindowItemToBuy killerWeapon;

	// Token: 0x04003738 RID: 14136
	public RespawnWindowItemToBuy recommendedWeapon;

	// Token: 0x04003739 RID: 14137
	public RespawnWindowItemToBuy recommendedArmor;

	// Token: 0x0400373A RID: 14138
	public GameObject coinsShopButton;

	// Token: 0x0400373B RID: 14139
	public GameObject armorObj;

	// Token: 0x0400373C RID: 14140
	public GameObject healthIcon;

	// Token: 0x0400373D RID: 14141
	public GameObject mechIcon;

	// Token: 0x0400373E RID: 14142
	public GameObject healthBackground;

	// Token: 0x0400373F RID: 14143
	public GameObject healtharmorBackground;

	// Token: 0x04003740 RID: 14144
	public UITable healthTable;

	// Token: 0x04003741 RID: 14145
	public RespawnWindowEquipmentItem hatItem;

	// Token: 0x04003742 RID: 14146
	public RespawnWindowEquipmentItem maskItem;

	// Token: 0x04003743 RID: 14147
	public RespawnWindowEquipmentItem armorItem;

	// Token: 0x04003744 RID: 14148
	public RespawnWindowEquipmentItem capeItem;

	// Token: 0x04003745 RID: 14149
	public RespawnWindowEquipmentItem bootsItem;

	// Token: 0x04003746 RID: 14150
	public RespawnWindowEquipmentItem petItem;

	// Token: 0x04003747 RID: 14151
	public RespawnWindowEquipmentItem gadgetSupportItem;

	// Token: 0x04003748 RID: 14152
	public RespawnWindowEquipmentItem gadgetTrowingItem;

	// Token: 0x04003749 RID: 14153
	public RespawnWindowEquipmentItem gadgetToolsItem;

	// Token: 0x0400374A RID: 14154
	public UILabel armorCountLabel;

	// Token: 0x0400374B RID: 14155
	public UILabel healthCountLabel;

	// Token: 0x0400374C RID: 14156
	public GameObject characterDrag;

	// Token: 0x0400374D RID: 14157
	public GameObject cameraDrag;

	// Token: 0x0400374E RID: 14158
	private static RespawnWindow _instance;

	// Token: 0x0400374F RID: 14159
	private float _originalTimeout;

	// Token: 0x04003750 RID: 14160
	private float _remained;

	// Token: 0x04003751 RID: 14161
	[NonSerialized]
	public RenderTexture respawnWindowRT;
}
