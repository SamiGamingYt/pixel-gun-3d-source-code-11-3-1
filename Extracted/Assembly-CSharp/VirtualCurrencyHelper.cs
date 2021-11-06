using System;
using System.Collections.Generic;
using System.Linq;
using Rilisoft;
using UnityEngine;

// Token: 0x02000782 RID: 1922
public sealed class VirtualCurrencyHelper
{
	// Token: 0x06004399 RID: 17305 RVA: 0x00168FB0 File Offset: 0x001671B0
	static VirtualCurrencyHelper()
	{
		VirtualCurrencyHelper.AddPrice(PremiumAccountController.AccountType.OneDay.ToString(), 5);
		VirtualCurrencyHelper.AddPrice(PremiumAccountController.AccountType.ThreeDay.ToString(), 10);
		VirtualCurrencyHelper.AddPrice(PremiumAccountController.AccountType.SevenDays.ToString(), 20);
		VirtualCurrencyHelper.AddPrice(PremiumAccountController.AccountType.Month.ToString(), 60);
		VirtualCurrencyHelper.AddPrice("crystalsword", 185);
		VirtualCurrencyHelper.AddPrice("Fullhealth", 15);
		VirtualCurrencyHelper.AddPrice("bigammopack", 15);
		VirtualCurrencyHelper.AddPrice("MinerWeapon", 35);
		VirtualCurrencyHelper.AddPrice(StoreKitEventListener.elixirID, 15);
		VirtualCurrencyHelper.AddPrice(StoreKitEventListener.armor, 10);
		VirtualCurrencyHelper.AddPrice(StoreKitEventListener.armor2, 15);
		VirtualCurrencyHelper.AddPrice(StoreKitEventListener.armor3, 20);
		VirtualCurrencyHelper.AddPrice("CustomSkinID", Defs.skinsMakerPrice);
		VirtualCurrencyHelper.AddPrice("cape_Archimage", 60);
		VirtualCurrencyHelper.AddPrice("cape_BloodyDemon", 60);
		VirtualCurrencyHelper.AddPrice("cape_RoyalKnight", 60);
		VirtualCurrencyHelper.AddPrice("cape_SkeletonLord", 60);
		VirtualCurrencyHelper.AddPrice("cape_EliteCrafter", 60);
		VirtualCurrencyHelper.AddPrice("cape_Custom", 75);
		VirtualCurrencyHelper.AddPrice("HitmanCape_Up1", 30);
		VirtualCurrencyHelper.AddPrice("BerserkCape_Up1", 30);
		VirtualCurrencyHelper.AddPrice("DemolitionCape_Up1", 30);
		VirtualCurrencyHelper.AddPrice("SniperCape_Up1", 30);
		VirtualCurrencyHelper.AddPrice("StormTrooperCape_Up1", 30);
		VirtualCurrencyHelper.AddPrice("HitmanCape_Up2", 45);
		VirtualCurrencyHelper.AddPrice("BerserkCape_Up2", 45);
		VirtualCurrencyHelper.AddPrice("DemolitionCape_Up2", 45);
		VirtualCurrencyHelper.AddPrice("SniperCape_Up2", 45);
		VirtualCurrencyHelper.AddPrice("StormTrooperCape_Up2", 45);
		VirtualCurrencyHelper.AddPrice("cape_Engineer", 60);
		VirtualCurrencyHelper.AddPrice("cape_Engineer_Up1", 30);
		VirtualCurrencyHelper.AddPrice("cape_Engineer_Up2", 45);
		VirtualCurrencyHelper.AddPrice("hat_DiamondHelmet", 65);
		VirtualCurrencyHelper.AddPrice("hat_Adamant_3", 3);
		VirtualCurrencyHelper.AddPrice("hat_Headphones", 50);
		VirtualCurrencyHelper.AddPrice("hat_ManiacMask", 65);
		VirtualCurrencyHelper.AddPrice("hat_KingsCrown", 150);
		VirtualCurrencyHelper.AddPrice("hat_SeriousManHat", 50);
		VirtualCurrencyHelper.AddPrice("hat_Samurai", 95);
		VirtualCurrencyHelper.AddPrice("league1_hat_hitman", 75);
		VirtualCurrencyHelper.AddPrice("league2_hat_cowboyhat", 100);
		VirtualCurrencyHelper.AddPrice("league3_hat_afro", 150);
		VirtualCurrencyHelper.AddPrice("league4_hat_mushroom", 200);
		VirtualCurrencyHelper.AddPrice("league5_hat_brain", 250);
		VirtualCurrencyHelper.AddPrice("league6_hat_tiara", 300);
		VirtualCurrencyHelper.AddPrice("hat_AlmazHelmet", 95);
		VirtualCurrencyHelper.AddPrice("hat_ArmyHelmet", 95);
		VirtualCurrencyHelper.AddPrice("hat_SteelHelmet", 95);
		VirtualCurrencyHelper.AddPrice("hat_GoldHelmet", 95);
		VirtualCurrencyHelper.AddPrice("hat_Army_1", 70);
		VirtualCurrencyHelper.AddPrice("hat_Army_2", 70);
		VirtualCurrencyHelper.AddPrice("hat_Army_3", 70);
		VirtualCurrencyHelper.AddPrice("hat_Army_4", 70);
		VirtualCurrencyHelper.AddPrice("hat_Steel_1", 85);
		VirtualCurrencyHelper.AddPrice("hat_Steel_2", 85);
		VirtualCurrencyHelper.AddPrice("hat_Steel_3", 85);
		VirtualCurrencyHelper.AddPrice("hat_Steel_4", 85);
		VirtualCurrencyHelper.AddPrice("hat_Royal_1", 100);
		VirtualCurrencyHelper.AddPrice("hat_Royal_2", 100);
		VirtualCurrencyHelper.AddPrice("hat_Royal_3", 100);
		VirtualCurrencyHelper.AddPrice("hat_Royal_4", 100);
		VirtualCurrencyHelper.AddPrice("hat_Almaz_1", 120);
		VirtualCurrencyHelper.AddPrice("hat_Almaz_2", 120);
		VirtualCurrencyHelper.AddPrice("hat_Almaz_3", 120);
		VirtualCurrencyHelper.AddPrice("hat_Almaz_4", 120);
		VirtualCurrencyHelper.AddPrice(PotionsController.HastePotion, 2);
		VirtualCurrencyHelper.AddPrice(PotionsController.MightPotion, 2);
		VirtualCurrencyHelper.AddPrice(PotionsController.RegenerationPotion, 5);
		VirtualCurrencyHelper.AddPrice(GearManager.UpgradeIDForGear("InvisibilityPotion", 1), 1);
		VirtualCurrencyHelper.AddPrice("InvisibilityPotion" + GearManager.UpgradeSuffix + 2, 1);
		VirtualCurrencyHelper.AddPrice("InvisibilityPotion" + GearManager.UpgradeSuffix + 3, 1);
		VirtualCurrencyHelper.AddPrice("InvisibilityPotion" + GearManager.UpgradeSuffix + 4, 1);
		VirtualCurrencyHelper.AddPrice("InvisibilityPotion" + GearManager.UpgradeSuffix + 5, 1);
		VirtualCurrencyHelper.AddPrice("GrenadeID" + GearManager.UpgradeSuffix + 1, 1);
		VirtualCurrencyHelper.AddPrice("GrenadeID" + GearManager.UpgradeSuffix + 2, 1);
		VirtualCurrencyHelper.AddPrice("GrenadeID" + GearManager.UpgradeSuffix + 3, 1);
		VirtualCurrencyHelper.AddPrice("GrenadeID" + GearManager.UpgradeSuffix + 4, 1);
		VirtualCurrencyHelper.AddPrice("GrenadeID" + GearManager.UpgradeSuffix + 5, 1);
		VirtualCurrencyHelper.AddPrice(GearManager.Turret + GearManager.UpgradeSuffix + 1, 1);
		VirtualCurrencyHelper.AddPrice(GearManager.Turret + GearManager.UpgradeSuffix + 2, 1);
		VirtualCurrencyHelper.AddPrice(GearManager.Turret + GearManager.UpgradeSuffix + 3, 1);
		VirtualCurrencyHelper.AddPrice(GearManager.Turret + GearManager.UpgradeSuffix + 4, 1);
		VirtualCurrencyHelper.AddPrice(GearManager.Turret + GearManager.UpgradeSuffix + 5, 1);
		VirtualCurrencyHelper.AddPrice(GearManager.Mech + GearManager.UpgradeSuffix + 1, 1);
		VirtualCurrencyHelper.AddPrice(GearManager.Mech + GearManager.UpgradeSuffix + 2, 1);
		VirtualCurrencyHelper.AddPrice(GearManager.Mech + GearManager.UpgradeSuffix + 3, 1);
		VirtualCurrencyHelper.AddPrice(GearManager.Mech + GearManager.UpgradeSuffix + 4, 1);
		VirtualCurrencyHelper.AddPrice(GearManager.Mech + GearManager.UpgradeSuffix + 5, 1);
		VirtualCurrencyHelper.AddPrice(GearManager.Jetpack + GearManager.UpgradeSuffix + 1, 1);
		VirtualCurrencyHelper.AddPrice(GearManager.Jetpack + GearManager.UpgradeSuffix + 2, 1);
		VirtualCurrencyHelper.AddPrice(GearManager.Jetpack + GearManager.UpgradeSuffix + 3, 1);
		VirtualCurrencyHelper.AddPrice(GearManager.Jetpack + GearManager.UpgradeSuffix + 4, 1);
		VirtualCurrencyHelper.AddPrice(GearManager.Jetpack + GearManager.UpgradeSuffix + 5, 1);
		VirtualCurrencyHelper.AddPrice(GearManager.Wings + GearManager.OneItemSuffix + 0, 3);
		VirtualCurrencyHelper.AddPrice(GearManager.Bear + GearManager.OneItemSuffix + 0, 2);
		VirtualCurrencyHelper.AddPrice(GearManager.BigHeadPotion + GearManager.OneItemSuffix + 0, 1);
		VirtualCurrencyHelper.AddPrice(GearManager.MusicBox + GearManager.OneItemSuffix + 0, 5);
		VirtualCurrencyHelper.AddPrice(GearManager.Like + GearManager.OneItemSuffix + 0, 3);
		VirtualCurrencyHelper.AddPrice("InvisibilityPotion" + GearManager.OneItemSuffix + 0, 3);
		VirtualCurrencyHelper.AddPrice("InvisibilityPotion" + GearManager.OneItemSuffix + 1, 3);
		VirtualCurrencyHelper.AddPrice("InvisibilityPotion" + GearManager.OneItemSuffix + 2, 3);
		VirtualCurrencyHelper.AddPrice("InvisibilityPotion" + GearManager.OneItemSuffix + 3, 3);
		VirtualCurrencyHelper.AddPrice("InvisibilityPotion" + GearManager.OneItemSuffix + 4, 3);
		VirtualCurrencyHelper.AddPrice("InvisibilityPotion" + GearManager.OneItemSuffix + 5, 3);
		VirtualCurrencyHelper.AddPrice("GrenadeID" + GearManager.OneItemSuffix + 0, 3);
		VirtualCurrencyHelper.AddPrice("GrenadeID" + GearManager.OneItemSuffix + 1, 3);
		VirtualCurrencyHelper.AddPrice("GrenadeID" + GearManager.OneItemSuffix + 2, 3);
		VirtualCurrencyHelper.AddPrice("GrenadeID" + GearManager.OneItemSuffix + 3, 3);
		VirtualCurrencyHelper.AddPrice("GrenadeID" + GearManager.OneItemSuffix + 4, 3);
		VirtualCurrencyHelper.AddPrice("GrenadeID" + GearManager.OneItemSuffix + 5, 3);
		VirtualCurrencyHelper.AddPrice(GearManager.Turret + GearManager.OneItemSuffix + 0, 5);
		VirtualCurrencyHelper.AddPrice(GearManager.Turret + GearManager.OneItemSuffix + 1, 5);
		VirtualCurrencyHelper.AddPrice(GearManager.Turret + GearManager.OneItemSuffix + 2, 5);
		VirtualCurrencyHelper.AddPrice(GearManager.Turret + GearManager.OneItemSuffix + 3, 5);
		VirtualCurrencyHelper.AddPrice(GearManager.Turret + GearManager.OneItemSuffix + 4, 5);
		VirtualCurrencyHelper.AddPrice(GearManager.Turret + GearManager.OneItemSuffix + 5, 5);
		VirtualCurrencyHelper.AddPrice(GearManager.Mech + GearManager.OneItemSuffix + 0, 7);
		VirtualCurrencyHelper.AddPrice(GearManager.Mech + GearManager.OneItemSuffix + 1, 7);
		VirtualCurrencyHelper.AddPrice(GearManager.Mech + GearManager.OneItemSuffix + 2, 7);
		VirtualCurrencyHelper.AddPrice(GearManager.Mech + GearManager.OneItemSuffix + 3, 7);
		VirtualCurrencyHelper.AddPrice(GearManager.Mech + GearManager.OneItemSuffix + 4, 7);
		VirtualCurrencyHelper.AddPrice(GearManager.Mech + GearManager.OneItemSuffix + 5, 7);
		VirtualCurrencyHelper.AddPrice(GearManager.Jetpack + GearManager.OneItemSuffix + 0, 4);
		VirtualCurrencyHelper.AddPrice(GearManager.Jetpack + GearManager.OneItemSuffix + 1, 4);
		VirtualCurrencyHelper.AddPrice(GearManager.Jetpack + GearManager.OneItemSuffix + 2, 4);
		VirtualCurrencyHelper.AddPrice(GearManager.Jetpack + GearManager.OneItemSuffix + 3, 4);
		VirtualCurrencyHelper.AddPrice(GearManager.Jetpack + GearManager.OneItemSuffix + 4, 4);
		VirtualCurrencyHelper.AddPrice(GearManager.Jetpack + GearManager.OneItemSuffix + 5, 4);
		VirtualCurrencyHelper.AddPrice("boots_red", 50);
		VirtualCurrencyHelper.AddPrice("boots_gray", 50);
		VirtualCurrencyHelper.AddPrice("boots_blue", 50);
		VirtualCurrencyHelper.AddPrice("boots_green", 50);
		VirtualCurrencyHelper.AddPrice("boots_black", 50);
		VirtualCurrencyHelper.AddPrice("boots_tabi", 120);
		VirtualCurrencyHelper.AddPrice("HitmanBoots_Up1", 25);
		VirtualCurrencyHelper.AddPrice("StormTrooperBoots_Up1", 25);
		VirtualCurrencyHelper.AddPrice("SniperBoots_Up1", 25);
		VirtualCurrencyHelper.AddPrice("DemolitionBoots_Up1", 25);
		VirtualCurrencyHelper.AddPrice("BerserkBoots_Up1", 25);
		VirtualCurrencyHelper.AddPrice("HitmanBoots_Up2", 40);
		VirtualCurrencyHelper.AddPrice("StormTrooperBoots_Up2", 40);
		VirtualCurrencyHelper.AddPrice("SniperBoots_Up2", 40);
		VirtualCurrencyHelper.AddPrice("DemolitionBoots_Up2", 40);
		VirtualCurrencyHelper.AddPrice("BerserkBoots_Up2", 40);
		VirtualCurrencyHelper.AddPrice("mask_sniper", 40);
		VirtualCurrencyHelper.AddPrice("mask_sniper_up1", 15);
		VirtualCurrencyHelper.AddPrice("mask_sniper_up2", 30);
		VirtualCurrencyHelper.AddPrice("mask_demolition", 40);
		VirtualCurrencyHelper.AddPrice("mask_demolition_up1", 15);
		VirtualCurrencyHelper.AddPrice("mask_demolition_up2", 30);
		VirtualCurrencyHelper.AddPrice("mask_hitman_1", 40);
		VirtualCurrencyHelper.AddPrice("mask_hitman_1_up1", 15);
		VirtualCurrencyHelper.AddPrice("mask_hitman_1_up2", 30);
		VirtualCurrencyHelper.AddPrice("mask_berserk", 40);
		VirtualCurrencyHelper.AddPrice("mask_berserk_up1", 15);
		VirtualCurrencyHelper.AddPrice("mask_berserk_up2", 30);
		VirtualCurrencyHelper.AddPrice("mask_trooper", 40);
		VirtualCurrencyHelper.AddPrice("mask_trooper_up1", 15);
		VirtualCurrencyHelper.AddPrice("mask_trooper_up2", 30);
		VirtualCurrencyHelper.AddPrice("mask_engineer", 40);
		VirtualCurrencyHelper.AddPrice("mask_engineer_up1", 15);
		VirtualCurrencyHelper.AddPrice("mask_engineer_up2", 30);
		VirtualCurrencyHelper.AddPrice("EngineerBoots", 50);
		VirtualCurrencyHelper.AddPrice("EngineerBoots_Up1", 25);
		VirtualCurrencyHelper.AddPrice("EngineerBoots_Up2", 40);
		VirtualCurrencyHelper.AddPriceForArmor("Armor_Army_1", 70);
		VirtualCurrencyHelper.AddPriceForArmor("Armor_Army_2", 70);
		VirtualCurrencyHelper.AddPriceForArmor("Armor_Army_3", 70);
		VirtualCurrencyHelper.AddPriceForArmor("Armor_Steel_1", 85);
		VirtualCurrencyHelper.AddPriceForArmor("Armor_Steel_2", 85);
		VirtualCurrencyHelper.AddPriceForArmor("Armor_Steel_3", 85);
		VirtualCurrencyHelper.AddPriceForArmor("Armor_Royal_1", 100);
		VirtualCurrencyHelper.AddPriceForArmor("Armor_Royal_2", 100);
		VirtualCurrencyHelper.AddPriceForArmor("Armor_Royal_3", 100);
		VirtualCurrencyHelper.AddPriceForArmor("Armor_Almaz_1", 120);
		VirtualCurrencyHelper.AddPriceForArmor("Armor_Almaz_2", 120);
		VirtualCurrencyHelper.AddPriceForArmor("Armor_Almaz_3", 120);
		VirtualCurrencyHelper.AddPriceForArmor("Armor_Novice", 10);
		VirtualCurrencyHelper.AddPriceForArmor("Armor_Rubin_1", 135);
		VirtualCurrencyHelper.AddPriceForArmor("Armor_Rubin_2", 135);
		VirtualCurrencyHelper.AddPriceForArmor("Armor_Rubin_3", 135);
		VirtualCurrencyHelper.AddPriceForArmor("Armor_Adamant_Const_1", 170);
		VirtualCurrencyHelper.AddPriceForArmor("Armor_Adamant_Const_2", 170);
		VirtualCurrencyHelper.AddPriceForArmor("Armor_Adamant_Const_3", 170);
		VirtualCurrencyHelper.AddPriceForArmor("Armor_Army_4", 120);
		VirtualCurrencyHelper.AddPriceForArmor("Armor_Steel_4", 120);
		VirtualCurrencyHelper.AddPriceForArmor("Armor_Royal_4", 135);
		VirtualCurrencyHelper.AddPriceForArmor("Armor_Almaz_4", 120);
		VirtualCurrencyHelper.AddPriceForArmor("Armor_Adamant_3", 3);
		VirtualCurrencyHelper.AddPrice("hat_Rubin_1", 135);
		VirtualCurrencyHelper.AddPrice("hat_Rubin_2", 135);
		VirtualCurrencyHelper.AddPrice("hat_Rubin_3", 135);
		VirtualCurrencyHelper.AddPrice("hat_Adamant_Const_1", 170);
		VirtualCurrencyHelper.AddPrice("hat_Adamant_Const_2", 170);
		VirtualCurrencyHelper.AddPrice("hat_Adamant_Const_3", 170);
		VirtualCurrencyHelper.AddPrice(StickersController.KeyForBuyPack(TypePackSticker.classic), 20);
		VirtualCurrencyHelper.AddPrice(StickersController.KeyForBuyPack(TypePackSticker.christmas), 30);
		VirtualCurrencyHelper.AddPrice(StickersController.KeyForBuyPack(TypePackSticker.easter), 40);
		VirtualCurrencyHelper.AddPrice(Defs.BuyGiftKey, 5);
	}

	// Token: 0x0600439A RID: 17306 RVA: 0x00169E3C File Offset: 0x0016803C
	public static int[] InitCoinInappsQuantity(int[] _mass)
	{
		int[] array = new int[_mass.Length];
		Array.Copy(_mass, array, _mass.Length);
		return array;
	}

	// Token: 0x17000B2B RID: 2859
	// (get) Token: 0x0600439B RID: 17307 RVA: 0x00169E60 File Offset: 0x00168060
	public static int[] coinInappsQuantity
	{
		get
		{
			return VirtualCurrencyHelper._coinInappsQuantity;
		}
	}

	// Token: 0x0600439C RID: 17308 RVA: 0x00169E68 File Offset: 0x00168068
	public static int[] InitGemsInappsQuantity(int[] _mass)
	{
		int[] array = new int[_mass.Length];
		Array.Copy(_mass, array, _mass.Length);
		return array;
	}

	// Token: 0x17000B2C RID: 2860
	// (get) Token: 0x0600439D RID: 17309 RVA: 0x00169E8C File Offset: 0x0016808C
	public static int[] gemsInappsQuantity
	{
		get
		{
			return VirtualCurrencyHelper._gemsInappsQuantity;
		}
	}

	// Token: 0x0600439E RID: 17310 RVA: 0x00169E94 File Offset: 0x00168094
	public static void ResetInappsQuantityOnDefault()
	{
		VirtualCurrencyHelper._gemsInappsQuantity = VirtualCurrencyHelper.InitGemsInappsQuantity(VirtualCurrencyHelper._gemsInappsQuantityDefault);
		VirtualCurrencyHelper._coinInappsQuantity = VirtualCurrencyHelper.InitCoinInappsQuantity(VirtualCurrencyHelper._coinInappsQuantityDefault);
	}

	// Token: 0x0600439F RID: 17311 RVA: 0x00169EB4 File Offset: 0x001680B4
	public static void RewriteInappsQuantity(int _priceId, int _coinQuantity, int _gemsQuantity, int _bonusCoins, int _bonusGems)
	{
		for (int i = 0; i < VirtualCurrencyHelper.coinPriceIds.Length; i++)
		{
			if (VirtualCurrencyHelper.coinPriceIds[i] == _priceId)
			{
				VirtualCurrencyHelper._coinInappsQuantity[i] = _coinQuantity;
				VirtualCurrencyHelper._gemsInappsQuantity[i] = _gemsQuantity;
				AbstractBankView.discountsCoins[i] = _bonusCoins;
				AbstractBankView.discountsGems[i] = _bonusGems;
				return;
			}
		}
	}

	// Token: 0x060043A0 RID: 17312 RVA: 0x00169F08 File Offset: 0x00168108
	public static ItemPrice Price(string key)
	{
		if (key == null)
		{
			return null;
		}
		if (key == "Eggs.SuperIncubatorId")
		{
			return new ItemPrice(200, "Coins");
		}
		if (PetsInfo.info.Keys.Contains(key))
		{
			return new ItemPrice(200, "Coins");
		}
		if (Wear.wear[ShopNGUIController.CategoryNames.ArmorCategory].SelectMany((List<string> list) => list).ToList<string>().Contains(key))
		{
			if (Defs2.ArmorPricesFromServer != null && Defs2.ArmorPricesFromServer.ContainsKey(key))
			{
				ItemPrice itemPrice = Defs2.ArmorPricesFromServer[key];
				if (itemPrice != null)
				{
					return itemPrice;
				}
				Debug.LogError("armorPriceFromServer == null, armor = " + key);
			}
			return VirtualCurrencyHelper._armorPricesDefault[key];
		}
		if (GadgetsInfo.info.Keys.Contains(key))
		{
			List<ItemPrice> list2;
			if (BalanceController.GadgetPricesFromServer != null && BalanceController.GadgetPricesFromServer.TryGetValue(key, out list2) && list2 != null)
			{
				string a = GadgetsInfo.FirstForOurTier(key);
				int num = (!(a == key)) ? 1 : 0;
				if (list2.Count > num && list2[num] != null)
				{
					return list2[num];
				}
				Debug.LogError("listServerPrices.Count > index && listServerPrices [index] != null: key = " + key);
			}
			return new ItemPrice(200, "Coins");
		}
		if (!VirtualCurrencyHelper.prices.ContainsKey(key))
		{
			return null;
		}
		int value = VirtualCurrencyHelper.prices[key].Value;
		string currency = "Coins";
		string text = GearManager.HolderQuantityForID(key);
		bool flag = text != null && (GearManager.Gear.Contains(text) || GearManager.DaterGear.Contains(text)) && !key.Contains(GearManager.UpgradeSuffix) && !text.Equals(GearManager.Grenade);
		if (key == "cape_Archimage" || key == "cape_BloodyDemon" || key == "cape_RoyalKnight" || key == "cape_SkeletonLord" || key == "cape_EliteCrafter" || key == "HitmanCape_Up1" || key == "BerserkCape_Up1" || key == "DemolitionCape_Up1" || key == "SniperCape_Up1" || key == "StormTrooperCape_Up1" || key == "HitmanCape_Up2" || key == "BerserkCape_Up2" || key == "DemolitionCape_Up2" || key == "SniperCape_Up2" || key == "StormTrooperCape_Up2" || key == "cape_Engineer" || key == "cape_Engineer_Up1" || key == "cape_Engineer_Up2")
		{
			flag = true;
		}
		if (key == "boots_red" || key == "boots_gray" || key == "boots_blue" || key == "boots_green" || key == "boots_black" || key == "HitmanBoots_Up1" || key == "StormTrooperBoots_Up1" || key == "SniperBoots_Up1" || key == "DemolitionBoots_Up1" || key == "BerserkBoots_Up1" || key == "HitmanBoots_Up2" || key == "StormTrooperBoots_Up2" || key == "SniperBoots_Up2" || key == "DemolitionBoots_Up2" || key == "BerserkBoots_Up2" || key == "EngineerBoots" || key == "EngineerBoots_Up1" || key == "EngineerBoots_Up2")
		{
			flag = true;
		}
		IEnumerable<string> source = Wear.wear[ShopNGUIController.CategoryNames.MaskCategory].SelectMany((List<string> l) => l);
		if (key != "hat_ManiacMask" && source.Contains(key))
		{
			flag = true;
		}
		if (key == StickersController.KeyForBuyPack(TypePackSticker.classic))
		{
			flag = true;
		}
		if (key == StickersController.KeyForBuyPack(TypePackSticker.christmas))
		{
			flag = true;
		}
		if (key == StickersController.KeyForBuyPack(TypePackSticker.easter))
		{
			flag = true;
		}
		if (key == Defs.BuyGiftKey)
		{
			flag = true;
		}
		if (TempItemsController.PriceCoefs.ContainsKey(key))
		{
			flag = true;
		}
		if (key != null && (key.Equals(PremiumAccountController.AccountType.OneDay.ToString()) || key.Equals(PremiumAccountController.AccountType.ThreeDay.ToString()) || key.Equals(PremiumAccountController.AccountType.SevenDays.ToString()) || key.Equals(PremiumAccountController.AccountType.Month.ToString())))
		{
			flag = true;
		}
		if (flag)
		{
			currency = "GemsCurrency";
		}
		return new ItemPrice(value, currency);
	}

	// Token: 0x060043A1 RID: 17313 RVA: 0x0016A474 File Offset: 0x00168674
	private static int CoefInappsQuantity()
	{
		if (PromoActionsManager.sharedManager != null && PromoActionsManager.sharedManager.IsEventX3Active)
		{
			return 3;
		}
		return 1;
	}

	// Token: 0x060043A2 RID: 17314 RVA: 0x0016A4A4 File Offset: 0x001686A4
	public static int GetCoinInappsQuantity(int inappIndex)
	{
		if (PromoActionsManager.sharedManager == null)
		{
			Debug.LogError("GetCoinInappsQuantity: PromoActionsManager.sharedManager == null when calculating");
		}
		return VirtualCurrencyHelper.CoefInappsQuantity() * VirtualCurrencyHelper.coinInappsQuantity[inappIndex];
	}

	// Token: 0x060043A3 RID: 17315 RVA: 0x0016A4D0 File Offset: 0x001686D0
	public static int GetGemsInappsQuantity(int inappIndex)
	{
		if (PromoActionsManager.sharedManager == null)
		{
			Debug.LogError("GetGemsInappsQuantity: PromoActionsManager.sharedManager == null when calculating");
		}
		return VirtualCurrencyHelper.CoefInappsQuantity() * VirtualCurrencyHelper.gemsInappsQuantity[inappIndex];
	}

	// Token: 0x060043A4 RID: 17316 RVA: 0x0016A4FC File Offset: 0x001686FC
	public static void AddPrice(string key, int value)
	{
		VirtualCurrencyHelper.prices.Add(key, new SaltedInt(VirtualCurrencyHelper._prng.Next(), value));
	}

	// Token: 0x060043A5 RID: 17317 RVA: 0x0016A51C File Offset: 0x0016871C
	private static void AddPriceForArmor(string armor, int amount)
	{
		if (armor == null)
		{
			Debug.LogError("AddPriceForArmor armor == null");
			return;
		}
		VirtualCurrencyHelper._armorPricesDefault.Add(armor, new ItemPrice(amount, (!(armor == "Armor_Adamant_3")) ? "Coins" : "GemsCurrency"));
	}

	// Token: 0x17000B2D RID: 2861
	// (get) Token: 0x060043A6 RID: 17318 RVA: 0x0016A56C File Offset: 0x0016876C
	internal static Dictionary<string, decimal> ReferencePricesInUsd
	{
		get
		{
			if (VirtualCurrencyHelper._referencePricesInUsd != null && VirtualCurrencyHelper._referencePricesInUsd.IsAlive)
			{
				return (Dictionary<string, decimal>)VirtualCurrencyHelper._referencePricesInUsd.Target;
			}
			Dictionary<string, decimal> dictionary = VirtualCurrencyHelper.InitializeReferencePrices();
			VirtualCurrencyHelper._referencePricesInUsd = new WeakReference(dictionary, false);
			return dictionary;
		}
	}

	// Token: 0x060043A7 RID: 17319 RVA: 0x0016A5B8 File Offset: 0x001687B8
	private static Dictionary<string, decimal> InitializeReferencePrices()
	{
		Dictionary<string, decimal> dictionary = new Dictionary<string, decimal>
		{
			{
				"coin1",
				0.99m
			},
			{
				"coin7",
				2.99m
			},
			{
				"coin2",
				4.99m
			},
			{
				"coin4",
				19.99m
			},
			{
				"coin5",
				49.99m
			},
			{
				"coin8",
				99.99m
			},
			{
				"gem1",
				0.99m
			},
			{
				"gem2",
				2.99m
			},
			{
				"gem3",
				4.99m
			},
			{
				"gem4",
				9.99m
			},
			{
				"gem5",
				19.99m
			},
			{
				"gem6",
				49.99m
			},
			{
				"gem7",
				99.99m
			},
			{
				"starterpack8",
				0.99m
			},
			{
				"starterpack7",
				0.99m
			},
			{
				"starterpack5",
				1.99m
			},
			{
				"starterpack3",
				3.99m
			},
			{
				"starterpack1",
				5.99m
			}
		};
		if (BuildSettings.BuildTargetPlatform == RuntimePlatform.Android)
		{
			dictionary.Add("coin3.", 9.99m);
			dictionary.Add("starterpack6", 0.99m);
			dictionary.Add("starterpack4", 2.99m);
			dictionary.Add("starterpack2", 4.99m);
		}
		else if (BuildSettings.BuildTargetPlatform == RuntimePlatform.IPhonePlayer)
		{
			dictionary.Add("coin3", 9.99m);
			dictionary.Add("starterpack10", 2.99m);
			dictionary.Add("starterpack9", 4.99m);
		}
		return dictionary;
	}

	// Token: 0x0400314F RID: 12623
	private static int[] _coinInappsQuantityDefault = new int[]
	{
		15,
		45,
		80,
		165,
		335,
		865,
		2000
	};

	// Token: 0x04003150 RID: 12624
	public static int[] _coinInappsQuantityStaticBank = new int[]
	{
		15,
		50,
		90,
		185,
		390,
		1050,
		2250
	};

	// Token: 0x04003151 RID: 12625
	public static string[] coinInappsLocalizationKeys = new string[]
	{
		"Key_2106",
		"Key_2107",
		"Key_2108",
		"Key_2109",
		"Key_2110",
		"Key_2111",
		"Key_2112"
	};

	// Token: 0x04003152 RID: 12626
	private static int[] _coinInappsQuantity = VirtualCurrencyHelper.InitCoinInappsQuantity(VirtualCurrencyHelper._coinInappsQuantityDefault);

	// Token: 0x04003153 RID: 12627
	private static int[] _gemsInappsQuantityDefault = new int[]
	{
		9,
		27,
		48,
		100,
		200,
		517,
		1200
	};

	// Token: 0x04003154 RID: 12628
	public static int[] _gemsInappsQuantityStaticBank = new int[]
	{
		9,
		30,
		60,
		120,
		260,
		700,
		1500
	};

	// Token: 0x04003155 RID: 12629
	public static string[] gemsInappsLocalizationKeys = new string[]
	{
		"Key_2113",
		"Key_2114",
		"Key_2115",
		"Key_2116",
		"Key_2117",
		"Key_2118",
		"Key_2119"
	};

	// Token: 0x04003156 RID: 12630
	private static int[] _gemsInappsQuantity = VirtualCurrencyHelper.InitGemsInappsQuantity(VirtualCurrencyHelper._gemsInappsQuantityDefault);

	// Token: 0x04003157 RID: 12631
	public static readonly int[] coinPriceIds = new int[]
	{
		1,
		3,
		5,
		10,
		20,
		50,
		100
	};

	// Token: 0x04003158 RID: 12632
	public static readonly int[] gemsPriceIds = new int[]
	{
		1,
		3,
		5,
		10,
		20,
		50,
		100
	};

	// Token: 0x04003159 RID: 12633
	public static readonly int[] starterPackFakePrice = new int[]
	{
		6,
		5,
		4,
		3,
		2,
		1,
		1,
		1
	};

	// Token: 0x0400315A RID: 12634
	private static Dictionary<string, SaltedInt> prices = new Dictionary<string, SaltedInt>();

	// Token: 0x0400315B RID: 12635
	private static System.Random _prng = new System.Random(4103);

	// Token: 0x0400315C RID: 12636
	private static WeakReference _referencePricesInUsd;

	// Token: 0x0400315D RID: 12637
	private static Dictionary<string, ItemPrice> _armorPricesDefault = new Dictionary<string, ItemPrice>();
}
