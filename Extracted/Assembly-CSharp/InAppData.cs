using System;
using System.Collections.Generic;
using System.Linq;

// Token: 0x02000657 RID: 1623
public static class InAppData
{
	// Token: 0x06003858 RID: 14424 RVA: 0x00122898 File Offset: 0x00120A98
	static InAppData()
	{
		InAppData.inappReadableNames.Add("bigammopack", "Big Pack of Ammo");
		InAppData.inappReadableNames.Add("Fullhealth", "Full Health");
		InAppData.inappReadableNames.Add(StoreKitEventListener.elixirID, "Elixir of Resurrection");
		InAppData.inappReadableNames.Add(StoreKitEventListener.armor, "Armor");
		InAppData.inappReadableNames.Add(StoreKitEventListener.armor2, "Armor2");
		InAppData.inappReadableNames.Add(StoreKitEventListener.armor3, "Armor3");
		InAppData.inappReadableNames.Add("cape_Archimage", "Archimage Cape");
		InAppData.inappReadableNames.Add("cape_BloodyDemon", "Bloody Demon Cape");
		InAppData.inappReadableNames.Add("cape_RoyalKnight", "Royal Knight Cape");
		InAppData.inappReadableNames.Add("cape_SkeletonLord", "Skeleton Lord Cape");
		InAppData.inappReadableNames.Add("cape_EliteCrafter", "Elite Crafter Cape");
		InAppData.inappReadableNames.Add("cape_Custom", "Custom Cape");
		InAppData.inappReadableNames.Add("HitmanCape_Up1", "HitmanCape_Up1");
		InAppData.inappReadableNames.Add("BerserkCape_Up1", "BerserkCape_Up1");
		InAppData.inappReadableNames.Add("DemolitionCape_Up1", "DemolitionCape_Up1");
		InAppData.inappReadableNames.Add("cape_Engineer", "EngineerCape");
		InAppData.inappReadableNames.Add("cape_Engineer_Up1", "EngineerCape_Up1");
		InAppData.inappReadableNames.Add("cape_Engineer_Up2", "EngineerCape_Up2");
		InAppData.inappReadableNames.Add("SniperCape_Up1", "SniperCape_Up1");
		InAppData.inappReadableNames.Add("StormTrooperCape_Up1", "StormTrooperCape_Up1");
		InAppData.inappReadableNames.Add("HitmanCape_Up2", "HitmanCape_Up2");
		InAppData.inappReadableNames.Add("BerserkCape_Up2", "BerserkCape_Up2");
		InAppData.inappReadableNames.Add("DemolitionCape_Up2", "DemolitionCape_Up2");
		InAppData.inappReadableNames.Add("SniperCape_Up2", "SniperCape_Up2");
		InAppData.inappReadableNames.Add("StormTrooperCape_Up2", "StormTrooperCape_Up2");
		InAppData.inappReadableNames.Add("hat_Adamant_3", "hat_Adamant_3");
		InAppData.inappReadableNames.Add("hat_DiamondHelmet", "Diamond Helmet");
		InAppData.inappReadableNames.Add("hat_Headphones", "Headphones");
		InAppData.inappReadableNames.Add("hat_KingsCrown", "King's Crown");
		InAppData.inappReadableNames.Add("hat_SeriousManHat", "Leprechaun's Hat");
		InAppData.inappReadableNames.Add("hat_Samurai", "Samurais Helm");
		InAppData.inappReadableNames.Add("league1_hat_hitman", "league1_hat_hitman");
		InAppData.inappReadableNames.Add("league2_hat_cowboyhat", "league2_hat_cowboyhat");
		InAppData.inappReadableNames.Add("league3_hat_afro", "league3_hat_afro");
		InAppData.inappReadableNames.Add("league4_hat_mushroom", "league4_hat_mushroom");
		InAppData.inappReadableNames.Add("league5_hat_brain", "league5_hat_brain");
		InAppData.inappReadableNames.Add("league6_hat_tiara", "league6_hat_tiara");
		InAppData.inappReadableNames.Add("hat_AlmazHelmet", "hat_AlmazHelmet");
		InAppData.inappReadableNames.Add("hat_ArmyHelmet", "hat_ArmyHelmet");
		InAppData.inappReadableNames.Add("hat_SteelHelmet", "hat_SteelHelmet");
		InAppData.inappReadableNames.Add("hat_GoldHelmet", "hat_GoldHelmet");
		InAppData.inappReadableNames.Add("hat_Army_1", "hat_Army_1");
		InAppData.inappReadableNames.Add("hat_Almaz_1", "hat_Almaz_1");
		InAppData.inappReadableNames.Add("hat_Steel_1", "hat_Steel_1");
		InAppData.inappReadableNames.Add("hat_Royal_1", "hat_Royal_1");
		InAppData.inappReadableNames.Add("hat_Army_2", "hat_Army_2");
		InAppData.inappReadableNames.Add("hat_Almaz_2", "hat_Almaz_2");
		InAppData.inappReadableNames.Add("hat_Steel_2", "hat_Steel_2");
		InAppData.inappReadableNames.Add("hat_Royal_2", "hat_Royal_2");
		InAppData.inappReadableNames.Add("hat_Army_3", "hat_Army_3");
		InAppData.inappReadableNames.Add("hat_Almaz_3", "hat_Almaz_3");
		InAppData.inappReadableNames.Add("hat_Steel_3", "hat_Steel_3");
		InAppData.inappReadableNames.Add("hat_Royal_3", "hat_Royal_3");
		InAppData.inappReadableNames.Add("hat_Army_4", "hat_Army_4");
		InAppData.inappReadableNames.Add("hat_Almaz_4", "hat_Almaz_4");
		InAppData.inappReadableNames.Add("hat_Steel_4", "hat_Steel_4");
		InAppData.inappReadableNames.Add("hat_Royal_4", "hat_Royal_4");
		InAppData.inappReadableNames.Add("hat_Rubin_1", "hat_Rubin_1");
		InAppData.inappReadableNames.Add("hat_Rubin_2", "hat_Rubin_2");
		InAppData.inappReadableNames.Add("hat_Rubin_3", "hat_Rubin_3");
		InAppData.inappReadableNames.Add("Armor_Steel_1", "Armor_Steel_1");
		InAppData.inappReadableNames.Add("Armor_Steel_2", "Armor_Steel_2");
		InAppData.inappReadableNames.Add("Armor_Steel_3", "Armor_Steel_3");
		InAppData.inappReadableNames.Add("Armor_Steel_4", "Armor_Steel_4");
		InAppData.inappReadableNames.Add("Armor_Royal_1", "Armor_Royal_1");
		InAppData.inappReadableNames.Add("Armor_Royal_2", "Armor_Royal_2");
		InAppData.inappReadableNames.Add("Armor_Royal_3", "Armor_Royal_3");
		InAppData.inappReadableNames.Add("Armor_Royal_4", "Armor_Royal_4");
		InAppData.inappReadableNames.Add("Armor_Almaz_1", "Armor_Almaz_1");
		InAppData.inappReadableNames.Add("Armor_Almaz_2", "Armor_Almaz_2");
		InAppData.inappReadableNames.Add("Armor_Almaz_3", "Armor_Almaz_3");
		InAppData.inappReadableNames.Add("Armor_Almaz_4", "Armor_Almaz_4");
		InAppData.inappReadableNames.Add("Armor_Army_1", "Armor_Army_1");
		InAppData.inappReadableNames.Add("Armor_Army_2", "Armor_Army_2");
		InAppData.inappReadableNames.Add("Armor_Army_3", "Armor_Army_3");
		InAppData.inappReadableNames.Add("Armor_Army_4", "Armor_Army_4");
		InAppData.inappReadableNames.Add("Armor_Novice", "Armor_Novice");
		InAppData.inappReadableNames.Add("Armor_Rubin_1", "Armor_Rubin_1");
		InAppData.inappReadableNames.Add("Armor_Rubin_2", "Armor_Rubin_2");
		InAppData.inappReadableNames.Add("Armor_Rubin_3", "Armor_Rubin_3");
		InAppData.inappReadableNames.Add("Armor_Adamant_Const_1", "Armor_Adamant_Const_1");
		InAppData.inappReadableNames.Add("Armor_Adamant_Const_2", "Armor_Adamant_Const_2");
		InAppData.inappReadableNames.Add("Armor_Adamant_Const_3", "Armor_Adamant_Const_3");
		InAppData.inappReadableNames.Add("hat_Adamant_Const_1", "hat_Adamant_Const_1");
		InAppData.inappReadableNames.Add("hat_Adamant_Const_2", "hat_Adamant_Const_2");
		InAppData.inappReadableNames.Add("hat_Adamant_Const_3", "hat_Adamant_Const_3");
		InAppData.inappReadableNames.Add("Armor_Adamant_3", "Armor_Adamant_3");
		foreach (string text in PotionsController.potions)
		{
			InAppData.inappReadableNames.Add(text, text);
		}
		InAppData.inappReadableNames.Add("boots_red", "boots_red");
		InAppData.inappReadableNames.Add("boots_gray", "boots_gray");
		InAppData.inappReadableNames.Add("boots_blue", "boots_blue");
		InAppData.inappReadableNames.Add("boots_green", "boots_green");
		InAppData.inappReadableNames.Add("boots_black", "boots_black");
		InAppData.inappReadableNames.Add("boots_tabi", "boots ninja");
		InAppData.inappReadableNames.Add("HitmanBoots_Up1", "HitmanBoots_Up1");
		InAppData.inappReadableNames.Add("StormTrooperBoots_Up1", "StormTrooperBoots_Up1");
		InAppData.inappReadableNames.Add("SniperBoots_Up1", "SniperBoots_Up1");
		InAppData.inappReadableNames.Add("DemolitionBoots_Up1", "DemolitionBoots_Up1");
		InAppData.inappReadableNames.Add("BerserkBoots_Up1", "BerserkBoots_Up1");
		InAppData.inappReadableNames.Add("HitmanBoots_Up2", "HitmanBoots_Up2");
		InAppData.inappReadableNames.Add("StormTrooperBoots_Up2", "StormTrooperBoots_Up2");
		InAppData.inappReadableNames.Add("SniperBoots_Up2", "SniperBoots_Up2");
		InAppData.inappReadableNames.Add("DemolitionBoots_Up2", "DemolitionBoots_Up2");
		InAppData.inappReadableNames.Add("BerserkBoots_Up2", "BerserkBoots_Up2");
		InAppData.inappReadableNames.Add("EngineerBoots", "EngineerBoots");
		InAppData.inappReadableNames.Add("EngineerBoots_Up1", "EngineerBoots_Up1");
		InAppData.inappReadableNames.Add("EngineerBoots_Up2", "EngineerBoots_Up2");
		foreach (string text2 in Wear.wear[ShopNGUIController.CategoryNames.MaskCategory].SelectMany((List<string> list) => list))
		{
			if (!(text2 == "hat_ManiacMask"))
			{
				InAppData.inappReadableNames.Add(text2, text2);
			}
		}
		InAppData.inappReadableNames.Add("hat_ManiacMask", "Maniac Mask");
		InAppData.inappReadableNames.Add("InvisibilityPotion" + GearManager.UpgradeSuffix + 1, "InvisibilityPotion" + GearManager.UpgradeSuffix + 1);
		InAppData.inappReadableNames.Add("InvisibilityPotion" + GearManager.UpgradeSuffix + 2, "InvisibilityPotion" + GearManager.UpgradeSuffix + 2);
		InAppData.inappReadableNames.Add("InvisibilityPotion" + GearManager.UpgradeSuffix + 3, "InvisibilityPotion" + GearManager.UpgradeSuffix + 3);
		InAppData.inappReadableNames.Add("InvisibilityPotion" + GearManager.UpgradeSuffix + 4, "InvisibilityPotion" + GearManager.UpgradeSuffix + 4);
		InAppData.inappReadableNames.Add("InvisibilityPotion" + GearManager.UpgradeSuffix + 5, "InvisibilityPotion" + GearManager.UpgradeSuffix + 5);
		InAppData.inappReadableNames.Add("GrenadeID" + GearManager.UpgradeSuffix + 1, "GrenadeID" + GearManager.UpgradeSuffix + 1);
		InAppData.inappReadableNames.Add("GrenadeID" + GearManager.UpgradeSuffix + 2, "GrenadeID" + GearManager.UpgradeSuffix + 2);
		InAppData.inappReadableNames.Add("GrenadeID" + GearManager.UpgradeSuffix + 3, "GrenadeID" + GearManager.UpgradeSuffix + 3);
		InAppData.inappReadableNames.Add("GrenadeID" + GearManager.UpgradeSuffix + 4, "GrenadeID" + GearManager.UpgradeSuffix + 4);
		InAppData.inappReadableNames.Add("GrenadeID" + GearManager.UpgradeSuffix + 5, "GrenadeID" + GearManager.UpgradeSuffix + 5);
		InAppData.inappReadableNames.Add(GearManager.Turret + GearManager.UpgradeSuffix + 1, GearManager.Turret + GearManager.UpgradeSuffix + 1);
		InAppData.inappReadableNames.Add(GearManager.Turret + GearManager.UpgradeSuffix + 2, GearManager.Turret + GearManager.UpgradeSuffix + 2);
		InAppData.inappReadableNames.Add(GearManager.Turret + GearManager.UpgradeSuffix + 3, GearManager.Turret + GearManager.UpgradeSuffix + 3);
		InAppData.inappReadableNames.Add(GearManager.Turret + GearManager.UpgradeSuffix + 4, GearManager.Turret + GearManager.UpgradeSuffix + 4);
		InAppData.inappReadableNames.Add(GearManager.Turret + GearManager.UpgradeSuffix + 5, GearManager.Turret + GearManager.UpgradeSuffix + 5);
		InAppData.inappReadableNames.Add(GearManager.Mech + GearManager.UpgradeSuffix + 1, GearManager.Mech + GearManager.UpgradeSuffix + 1);
		InAppData.inappReadableNames.Add(GearManager.Mech + GearManager.UpgradeSuffix + 2, GearManager.Mech + GearManager.UpgradeSuffix + 2);
		InAppData.inappReadableNames.Add(GearManager.Mech + GearManager.UpgradeSuffix + 3, GearManager.Mech + GearManager.UpgradeSuffix + 3);
		InAppData.inappReadableNames.Add(GearManager.Mech + GearManager.UpgradeSuffix + 4, GearManager.Mech + GearManager.UpgradeSuffix + 4);
		InAppData.inappReadableNames.Add(GearManager.Mech + GearManager.UpgradeSuffix + 5, GearManager.Mech + GearManager.UpgradeSuffix + 5);
		InAppData.inappReadableNames.Add(GearManager.Jetpack + GearManager.UpgradeSuffix + 1, GearManager.Jetpack + GearManager.UpgradeSuffix + 1);
		InAppData.inappReadableNames.Add(GearManager.Jetpack + GearManager.UpgradeSuffix + 2, GearManager.Jetpack + GearManager.UpgradeSuffix + 2);
		InAppData.inappReadableNames.Add(GearManager.Jetpack + GearManager.UpgradeSuffix + 3, GearManager.Jetpack + GearManager.UpgradeSuffix + 3);
		InAppData.inappReadableNames.Add(GearManager.Jetpack + GearManager.UpgradeSuffix + 4, GearManager.Jetpack + GearManager.UpgradeSuffix + 4);
		InAppData.inappReadableNames.Add(GearManager.Jetpack + GearManager.UpgradeSuffix + 5, GearManager.Jetpack + GearManager.UpgradeSuffix + 5);
		InAppData.inappReadableNames.Add("InvisibilityPotion" + GearManager.OneItemSuffix + 0, "InvisibilityPotion" + GearManager.OneItemSuffix + 0);
		InAppData.inappReadableNames.Add("InvisibilityPotion" + GearManager.OneItemSuffix + 1, "InvisibilityPotion" + GearManager.OneItemSuffix + 1);
		InAppData.inappReadableNames.Add("InvisibilityPotion" + GearManager.OneItemSuffix + 2, "InvisibilityPotion" + GearManager.OneItemSuffix + 2);
		InAppData.inappReadableNames.Add("InvisibilityPotion" + GearManager.OneItemSuffix + 3, "InvisibilityPotion" + GearManager.OneItemSuffix + 3);
		InAppData.inappReadableNames.Add("InvisibilityPotion" + GearManager.OneItemSuffix + 4, "InvisibilityPotion" + GearManager.OneItemSuffix + 4);
		InAppData.inappReadableNames.Add("InvisibilityPotion" + GearManager.OneItemSuffix + 5, "InvisibilityPotion" + GearManager.OneItemSuffix + 5);
		InAppData.inappReadableNames.Add("GrenadeID" + GearManager.OneItemSuffix + 0, "GrenadeID" + GearManager.OneItemSuffix + 0);
		InAppData.inappReadableNames.Add("GrenadeID" + GearManager.OneItemSuffix + 1, "GrenadeID" + GearManager.OneItemSuffix + 1);
		InAppData.inappReadableNames.Add("GrenadeID" + GearManager.OneItemSuffix + 2, "GrenadeID" + GearManager.OneItemSuffix + 2);
		InAppData.inappReadableNames.Add("GrenadeID" + GearManager.OneItemSuffix + 3, "GrenadeID" + GearManager.OneItemSuffix + 3);
		InAppData.inappReadableNames.Add("GrenadeID" + GearManager.OneItemSuffix + 4, "GrenadeID" + GearManager.OneItemSuffix + 4);
		InAppData.inappReadableNames.Add("GrenadeID" + GearManager.OneItemSuffix + 5, "GrenadeID" + GearManager.OneItemSuffix + 5);
		InAppData.inappReadableNames.Add(GearManager.Turret + GearManager.OneItemSuffix + 0, GearManager.Turret + GearManager.OneItemSuffix + 0);
		InAppData.inappReadableNames.Add(GearManager.Turret + GearManager.OneItemSuffix + 1, GearManager.Turret + GearManager.OneItemSuffix + 1);
		InAppData.inappReadableNames.Add(GearManager.Turret + GearManager.OneItemSuffix + 2, GearManager.Turret + GearManager.OneItemSuffix + 2);
		InAppData.inappReadableNames.Add(GearManager.Turret + GearManager.OneItemSuffix + 3, GearManager.Turret + GearManager.OneItemSuffix + 3);
		InAppData.inappReadableNames.Add(GearManager.Turret + GearManager.OneItemSuffix + 4, GearManager.Turret + GearManager.OneItemSuffix + 4);
		InAppData.inappReadableNames.Add(GearManager.Turret + GearManager.OneItemSuffix + 5, GearManager.Turret + GearManager.OneItemSuffix + 5);
		InAppData.inappReadableNames.Add(GearManager.Mech + GearManager.OneItemSuffix + 0, GearManager.Mech + GearManager.OneItemSuffix + 0);
		InAppData.inappReadableNames.Add(GearManager.Mech + GearManager.OneItemSuffix + 1, GearManager.Mech + GearManager.OneItemSuffix + 1);
		InAppData.inappReadableNames.Add(GearManager.Mech + GearManager.OneItemSuffix + 2, GearManager.Mech + GearManager.OneItemSuffix + 2);
		InAppData.inappReadableNames.Add(GearManager.Mech + GearManager.OneItemSuffix + 3, GearManager.Mech + GearManager.OneItemSuffix + 3);
		InAppData.inappReadableNames.Add(GearManager.Mech + GearManager.OneItemSuffix + 4, GearManager.Mech + GearManager.OneItemSuffix + 4);
		InAppData.inappReadableNames.Add(GearManager.Mech + GearManager.OneItemSuffix + 5, GearManager.Mech + GearManager.OneItemSuffix + 5);
		InAppData.inappReadableNames.Add(GearManager.Jetpack + GearManager.OneItemSuffix + 0, GearManager.Jetpack + GearManager.OneItemSuffix + 0);
		InAppData.inappReadableNames.Add(GearManager.Jetpack + GearManager.OneItemSuffix + 1, GearManager.Jetpack + GearManager.OneItemSuffix + 1);
		InAppData.inappReadableNames.Add(GearManager.Jetpack + GearManager.OneItemSuffix + 2, GearManager.Jetpack + GearManager.OneItemSuffix + 2);
		InAppData.inappReadableNames.Add(GearManager.Jetpack + GearManager.OneItemSuffix + 3, GearManager.Jetpack + GearManager.OneItemSuffix + 3);
		InAppData.inappReadableNames.Add(GearManager.Jetpack + GearManager.OneItemSuffix + 4, GearManager.Jetpack + GearManager.OneItemSuffix + 4);
		InAppData.inappReadableNames.Add(GearManager.Jetpack + GearManager.OneItemSuffix + 5, GearManager.Jetpack + GearManager.OneItemSuffix + 5);
	}

	// Token: 0x04002946 RID: 10566
	public static Dictionary<string, string> inappReadableNames = new Dictionary<string, string>();
}
