using System;
using System.Collections.Generic;
using System.Linq;
using Rilisoft;
using UnityEngine;

// Token: 0x0200078D RID: 1933
public static class Wear
{
	// Token: 0x060044DD RID: 17629 RVA: 0x001732D8 File Offset: 0x001714D8
	static Wear()
	{
		Wear.bootsMethods.Add("boots_red", new KeyValuePair<Action<Player_move_c, Dictionary<string, object>>, Action<Player_move_c, Dictionary<string, object>>>(new Action<Player_move_c, Dictionary<string, object>>(Wear.ActivateBoots_red), new Action<Player_move_c, Dictionary<string, object>>(Wear.deActivateBoots_red)));
		Wear.bootsMethods.Add("boots_gray", new KeyValuePair<Action<Player_move_c, Dictionary<string, object>>, Action<Player_move_c, Dictionary<string, object>>>(new Action<Player_move_c, Dictionary<string, object>>(Wear.ActivateBoots_grey), new Action<Player_move_c, Dictionary<string, object>>(Wear.deActivateBoots_grey)));
		Wear.bootsMethods.Add("boots_blue", new KeyValuePair<Action<Player_move_c, Dictionary<string, object>>, Action<Player_move_c, Dictionary<string, object>>>(new Action<Player_move_c, Dictionary<string, object>>(Wear.ActivateBoots_blue), new Action<Player_move_c, Dictionary<string, object>>(Wear.deActivateBoots_blue)));
		Wear.bootsMethods.Add("boots_green", new KeyValuePair<Action<Player_move_c, Dictionary<string, object>>, Action<Player_move_c, Dictionary<string, object>>>(new Action<Player_move_c, Dictionary<string, object>>(Wear.ActivateBoots_green), new Action<Player_move_c, Dictionary<string, object>>(Wear.deActivateBoots_green)));
		Wear.bootsMethods.Add("boots_black", new KeyValuePair<Action<Player_move_c, Dictionary<string, object>>, Action<Player_move_c, Dictionary<string, object>>>(new Action<Player_move_c, Dictionary<string, object>>(Wear.ActivateBoots_black), new Action<Player_move_c, Dictionary<string, object>>(Wear.deActivateBoots_black)));
		Wear.capesMethods.Add("cape_BloodyDemon", new KeyValuePair<Action<Player_move_c, Dictionary<string, object>>, Action<Player_move_c, Dictionary<string, object>>>(new Action<Player_move_c, Dictionary<string, object>>(Wear.Activate_cape_BloodyDemon), new Action<Player_move_c, Dictionary<string, object>>(Wear.deActivate_cape_BloodyDemon)));
		Wear.capesMethods.Add("cape_RoyalKnight", new KeyValuePair<Action<Player_move_c, Dictionary<string, object>>, Action<Player_move_c, Dictionary<string, object>>>(new Action<Player_move_c, Dictionary<string, object>>(Wear.Activate_cape_RoyalKnight), new Action<Player_move_c, Dictionary<string, object>>(Wear.deActivate_cape_RoyalKnight)));
		Wear.capesMethods.Add("cape_SkeletonLord", new KeyValuePair<Action<Player_move_c, Dictionary<string, object>>, Action<Player_move_c, Dictionary<string, object>>>(new Action<Player_move_c, Dictionary<string, object>>(Wear.Activate_cape_SkeletonLord), new Action<Player_move_c, Dictionary<string, object>>(Wear.deActivate_cape_SkeletonLord)));
		Wear.capesMethods.Add("cape_Archimage", new KeyValuePair<Action<Player_move_c, Dictionary<string, object>>, Action<Player_move_c, Dictionary<string, object>>>(new Action<Player_move_c, Dictionary<string, object>>(Wear.Activate_cape_Archimage), new Action<Player_move_c, Dictionary<string, object>>(Wear.deActivate_cape_Archimage)));
		Wear.capesMethods.Add("cape_EliteCrafter", new KeyValuePair<Action<Player_move_c, Dictionary<string, object>>, Action<Player_move_c, Dictionary<string, object>>>(new Action<Player_move_c, Dictionary<string, object>>(Wear.Activate_cape_EliteCrafter), new Action<Player_move_c, Dictionary<string, object>>(Wear.deActivate_cape_EliteCrafter)));
		Wear.capesMethods.Add("cape_Custom", new KeyValuePair<Action<Player_move_c, Dictionary<string, object>>, Action<Player_move_c, Dictionary<string, object>>>(new Action<Player_move_c, Dictionary<string, object>>(Wear.Activate_cape_Custom), new Action<Player_move_c, Dictionary<string, object>>(Wear.deActivate_cape_Custom)));
		Wear.hatsMethods.Add("hat_Adamant_3", new KeyValuePair<Action<Player_move_c, Dictionary<string, object>>, Action<Player_move_c, Dictionary<string, object>>>(new Action<Player_move_c, Dictionary<string, object>>(Wear.Activate_hat_EMPTY), new Action<Player_move_c, Dictionary<string, object>>(Wear.deActivate_hat_EMPTY)));
		Wear.hatsMethods.Add("hat_Headphones", new KeyValuePair<Action<Player_move_c, Dictionary<string, object>>, Action<Player_move_c, Dictionary<string, object>>>(new Action<Player_move_c, Dictionary<string, object>>(Wear.Activate_hat_EMPTY), new Action<Player_move_c, Dictionary<string, object>>(Wear.deActivate_hat_EMPTY)));
		Wear.hatsMethods.Add("hat_ManiacMask", new KeyValuePair<Action<Player_move_c, Dictionary<string, object>>, Action<Player_move_c, Dictionary<string, object>>>(new Action<Player_move_c, Dictionary<string, object>>(Wear.Activate_hat_ManiacMask), new Action<Player_move_c, Dictionary<string, object>>(Wear.deActivate_hat_ManiacMask)));
		Wear.hatsMethods.Add("hat_KingsCrown", new KeyValuePair<Action<Player_move_c, Dictionary<string, object>>, Action<Player_move_c, Dictionary<string, object>>>(new Action<Player_move_c, Dictionary<string, object>>(Wear.Activate_hat_KingsCrown), new Action<Player_move_c, Dictionary<string, object>>(Wear.deActivate_hat_KingsCrown)));
		Wear.hatsMethods.Add("hat_Samurai", new KeyValuePair<Action<Player_move_c, Dictionary<string, object>>, Action<Player_move_c, Dictionary<string, object>>>(new Action<Player_move_c, Dictionary<string, object>>(Wear.Activate_hat_Samurai), new Action<Player_move_c, Dictionary<string, object>>(Wear.deActivate_hat_Samurai)));
		Wear.hatsMethods.Add("hat_DiamondHelmet", new KeyValuePair<Action<Player_move_c, Dictionary<string, object>>, Action<Player_move_c, Dictionary<string, object>>>(new Action<Player_move_c, Dictionary<string, object>>(Wear.Activate_hat_DiamondHelmet), new Action<Player_move_c, Dictionary<string, object>>(Wear.deActivate_hat_DiamondHelmet)));
		Wear.hatsMethods.Add("hat_SeriousManHat", new KeyValuePair<Action<Player_move_c, Dictionary<string, object>>, Action<Player_move_c, Dictionary<string, object>>>(new Action<Player_move_c, Dictionary<string, object>>(Wear.Activate_hat_SeriousManHat), new Action<Player_move_c, Dictionary<string, object>>(Wear.deActivate_hat_SeriousManHat)));
		Wear.hatsMethods.Add("league1_hat_hitman", new KeyValuePair<Action<Player_move_c, Dictionary<string, object>>, Action<Player_move_c, Dictionary<string, object>>>(new Action<Player_move_c, Dictionary<string, object>>(Wear.Activate_Armor_EMPTY), new Action<Player_move_c, Dictionary<string, object>>(Wear.deActivate_Armor_EMPTY)));
		Wear.hatsMethods.Add("league2_hat_cowboyhat", new KeyValuePair<Action<Player_move_c, Dictionary<string, object>>, Action<Player_move_c, Dictionary<string, object>>>(new Action<Player_move_c, Dictionary<string, object>>(Wear.Activate_Armor_EMPTY), new Action<Player_move_c, Dictionary<string, object>>(Wear.deActivate_Armor_EMPTY)));
		Wear.hatsMethods.Add("league3_hat_afro", new KeyValuePair<Action<Player_move_c, Dictionary<string, object>>, Action<Player_move_c, Dictionary<string, object>>>(new Action<Player_move_c, Dictionary<string, object>>(Wear.Activate_Armor_EMPTY), new Action<Player_move_c, Dictionary<string, object>>(Wear.deActivate_Armor_EMPTY)));
		Wear.hatsMethods.Add("league4_hat_mushroom", new KeyValuePair<Action<Player_move_c, Dictionary<string, object>>, Action<Player_move_c, Dictionary<string, object>>>(new Action<Player_move_c, Dictionary<string, object>>(Wear.Activate_Armor_EMPTY), new Action<Player_move_c, Dictionary<string, object>>(Wear.deActivate_Armor_EMPTY)));
		Wear.hatsMethods.Add("league5_hat_brain", new KeyValuePair<Action<Player_move_c, Dictionary<string, object>>, Action<Player_move_c, Dictionary<string, object>>>(new Action<Player_move_c, Dictionary<string, object>>(Wear.Activate_Armor_EMPTY), new Action<Player_move_c, Dictionary<string, object>>(Wear.deActivate_Armor_EMPTY)));
		Wear.hatsMethods.Add("league6_hat_tiara", new KeyValuePair<Action<Player_move_c, Dictionary<string, object>>, Action<Player_move_c, Dictionary<string, object>>>(new Action<Player_move_c, Dictionary<string, object>>(Wear.Activate_Armor_EMPTY), new Action<Player_move_c, Dictionary<string, object>>(Wear.deActivate_Armor_EMPTY)));
		Wear.hatsMethods.Add("hat_AlmazHelmet", new KeyValuePair<Action<Player_move_c, Dictionary<string, object>>, Action<Player_move_c, Dictionary<string, object>>>(new Action<Player_move_c, Dictionary<string, object>>(Wear.Activate_hat_AlmazHelmet), new Action<Player_move_c, Dictionary<string, object>>(Wear.deActivate_hat_AlmazHelmet)));
		Wear.hatsMethods.Add("hat_ArmyHelmet", new KeyValuePair<Action<Player_move_c, Dictionary<string, object>>, Action<Player_move_c, Dictionary<string, object>>>(new Action<Player_move_c, Dictionary<string, object>>(Wear.Activate_hat_ArmyHelmet), new Action<Player_move_c, Dictionary<string, object>>(Wear.deActivate_hat_ArmyHelmet)));
		Wear.hatsMethods.Add("hat_GoldHelmet", new KeyValuePair<Action<Player_move_c, Dictionary<string, object>>, Action<Player_move_c, Dictionary<string, object>>>(new Action<Player_move_c, Dictionary<string, object>>(Wear.Activate_hat_GoldHelmet), new Action<Player_move_c, Dictionary<string, object>>(Wear.deActivate_hat_GoldHelmet)));
		Wear.hatsMethods.Add("hat_SteelHelmet", new KeyValuePair<Action<Player_move_c, Dictionary<string, object>>, Action<Player_move_c, Dictionary<string, object>>>(new Action<Player_move_c, Dictionary<string, object>>(Wear.Activate_hat_SteelHelmet), new Action<Player_move_c, Dictionary<string, object>>(Wear.deActivate_hat_SteelHelmet)));
		Wear.hatsMethods.Add("hat_Steel_1", new KeyValuePair<Action<Player_move_c, Dictionary<string, object>>, Action<Player_move_c, Dictionary<string, object>>>(new Action<Player_move_c, Dictionary<string, object>>(Wear.Activate_hat_Steel_1), new Action<Player_move_c, Dictionary<string, object>>(Wear.deActivate_hat_Steel_1)));
		Wear.hatsMethods.Add("hat_Royal_1", new KeyValuePair<Action<Player_move_c, Dictionary<string, object>>, Action<Player_move_c, Dictionary<string, object>>>(new Action<Player_move_c, Dictionary<string, object>>(Wear.Activate_hat_Royal_1), new Action<Player_move_c, Dictionary<string, object>>(Wear.deActivate_hat_Royal_1)));
		Wear.hatsMethods.Add("hat_Almaz_1", new KeyValuePair<Action<Player_move_c, Dictionary<string, object>>, Action<Player_move_c, Dictionary<string, object>>>(new Action<Player_move_c, Dictionary<string, object>>(Wear.Activate_hat_Almaz_1), new Action<Player_move_c, Dictionary<string, object>>(Wear.deActivate_hat_Almaz_1)));
		Wear.bootsMethods.Add("boots_tabi", new KeyValuePair<Action<Player_move_c, Dictionary<string, object>>, Action<Player_move_c, Dictionary<string, object>>>(new Action<Player_move_c, Dictionary<string, object>>(Wear.Activate_boots_tabi), new Action<Player_move_c, Dictionary<string, object>>(Wear.deActivate_boots_tabi)));
		Wear.armorMethods.Add("Armor_Adamant_3", new KeyValuePair<Action<Player_move_c, Dictionary<string, object>>, Action<Player_move_c, Dictionary<string, object>>>(new Action<Player_move_c, Dictionary<string, object>>(Wear.Activate_Armor_EMPTY), new Action<Player_move_c, Dictionary<string, object>>(Wear.deActivate_Armor_EMPTY)));
		Wear.armorMethods.Add("Armor_Steel_1", new KeyValuePair<Action<Player_move_c, Dictionary<string, object>>, Action<Player_move_c, Dictionary<string, object>>>(new Action<Player_move_c, Dictionary<string, object>>(Wear.Activate_Armor_EMPTY), new Action<Player_move_c, Dictionary<string, object>>(Wear.deActivate_Armor_EMPTY)));
		Wear.armorMethods.Add("Armor_Steel_2", new KeyValuePair<Action<Player_move_c, Dictionary<string, object>>, Action<Player_move_c, Dictionary<string, object>>>(new Action<Player_move_c, Dictionary<string, object>>(Wear.Activate_Armor_Steel_2), new Action<Player_move_c, Dictionary<string, object>>(Wear.deActivate_Armor_Steel_2)));
		Wear.armorMethods.Add("Armor_Steel_3", new KeyValuePair<Action<Player_move_c, Dictionary<string, object>>, Action<Player_move_c, Dictionary<string, object>>>(new Action<Player_move_c, Dictionary<string, object>>(Wear.Activate_Armor_Steel_3), new Action<Player_move_c, Dictionary<string, object>>(Wear.deActivate_Armor_Steel_3)));
		Wear.armorMethods.Add("Armor_Steel_4", new KeyValuePair<Action<Player_move_c, Dictionary<string, object>>, Action<Player_move_c, Dictionary<string, object>>>(new Action<Player_move_c, Dictionary<string, object>>(Wear.Activate_Armor_Steel_4), new Action<Player_move_c, Dictionary<string, object>>(Wear.deActivate_Armor_Steel_4)));
		Wear.armorMethods.Add("Armor_Royal_1", new KeyValuePair<Action<Player_move_c, Dictionary<string, object>>, Action<Player_move_c, Dictionary<string, object>>>(new Action<Player_move_c, Dictionary<string, object>>(Wear.Activate_Armor_Royal_1), new Action<Player_move_c, Dictionary<string, object>>(Wear.deActivate_Armor_Royal_1)));
		Wear.armorMethods.Add("Armor_Royal_2", new KeyValuePair<Action<Player_move_c, Dictionary<string, object>>, Action<Player_move_c, Dictionary<string, object>>>(new Action<Player_move_c, Dictionary<string, object>>(Wear.Activate_Armor_Royal_2), new Action<Player_move_c, Dictionary<string, object>>(Wear.deActivate_Armor_Royal_2)));
		Wear.armorMethods.Add("Armor_Royal_3", new KeyValuePair<Action<Player_move_c, Dictionary<string, object>>, Action<Player_move_c, Dictionary<string, object>>>(new Action<Player_move_c, Dictionary<string, object>>(Wear.Activate_Armor_Royal_3), new Action<Player_move_c, Dictionary<string, object>>(Wear.deActivate_Armor_Royal_3)));
		Wear.armorMethods.Add("Armor_Royal_4", new KeyValuePair<Action<Player_move_c, Dictionary<string, object>>, Action<Player_move_c, Dictionary<string, object>>>(new Action<Player_move_c, Dictionary<string, object>>(Wear.Activate_Armor_Royal_4), new Action<Player_move_c, Dictionary<string, object>>(Wear.deActivate_Armor_Royal_4)));
		Wear.armorMethods.Add("Armor_Almaz_1", new KeyValuePair<Action<Player_move_c, Dictionary<string, object>>, Action<Player_move_c, Dictionary<string, object>>>(new Action<Player_move_c, Dictionary<string, object>>(Wear.Activate_Armor_Almaz_1), new Action<Player_move_c, Dictionary<string, object>>(Wear.deActivate_Armor_Almaz_1)));
		Wear.armorMethods.Add("Armor_Almaz_2", new KeyValuePair<Action<Player_move_c, Dictionary<string, object>>, Action<Player_move_c, Dictionary<string, object>>>(new Action<Player_move_c, Dictionary<string, object>>(Wear.Activate_Armor_Almaz_2), new Action<Player_move_c, Dictionary<string, object>>(Wear.deActivate_Armor_Almaz_2)));
		Wear.armorMethods.Add("Armor_Almaz_3", new KeyValuePair<Action<Player_move_c, Dictionary<string, object>>, Action<Player_move_c, Dictionary<string, object>>>(new Action<Player_move_c, Dictionary<string, object>>(Wear.Activate_Armor_Almaz_3), new Action<Player_move_c, Dictionary<string, object>>(Wear.deActivate_Armor_Almaz_3)));
		Wear.armorMethods.Add("Armor_Almaz_4", new KeyValuePair<Action<Player_move_c, Dictionary<string, object>>, Action<Player_move_c, Dictionary<string, object>>>(new Action<Player_move_c, Dictionary<string, object>>(Wear.Activate_Armor_Almaz_4), new Action<Player_move_c, Dictionary<string, object>>(Wear.deActivate_Armor_Almaz_4)));
		Wear.armorMethods.Add("Armor_Novice", new KeyValuePair<Action<Player_move_c, Dictionary<string, object>>, Action<Player_move_c, Dictionary<string, object>>>(new Action<Player_move_c, Dictionary<string, object>>(Wear.Activate_Armor_Almaz_2), new Action<Player_move_c, Dictionary<string, object>>(Wear.deActivate_Armor_Almaz_2)));
		Wear.armorMethods.Add("Armor_Army_1", new KeyValuePair<Action<Player_move_c, Dictionary<string, object>>, Action<Player_move_c, Dictionary<string, object>>>(new Action<Player_move_c, Dictionary<string, object>>(Wear.Activate_Armor_Army_1), new Action<Player_move_c, Dictionary<string, object>>(Wear.deActivate_Armor_Army_1)));
		Wear.armorMethods.Add("Armor_Army_2", new KeyValuePair<Action<Player_move_c, Dictionary<string, object>>, Action<Player_move_c, Dictionary<string, object>>>(new Action<Player_move_c, Dictionary<string, object>>(Wear.Activate_Armor_Army_2), new Action<Player_move_c, Dictionary<string, object>>(Wear.deActivate_Armor_Army_2)));
		Wear.armorMethods.Add("Armor_Army_3", new KeyValuePair<Action<Player_move_c, Dictionary<string, object>>, Action<Player_move_c, Dictionary<string, object>>>(new Action<Player_move_c, Dictionary<string, object>>(Wear.Activate_Armor_Army_3), new Action<Player_move_c, Dictionary<string, object>>(Wear.deActivate_Armor_Army_3)));
		Wear.armorMethods.Add("Armor_Army_4", new KeyValuePair<Action<Player_move_c, Dictionary<string, object>>, Action<Player_move_c, Dictionary<string, object>>>(new Action<Player_move_c, Dictionary<string, object>>(Wear.Activate_Armor_Army_4), new Action<Player_move_c, Dictionary<string, object>>(Wear.deActivate_Armor_Army_4)));
		Wear.armorMethods.Add("Armor_Rubin_1", new KeyValuePair<Action<Player_move_c, Dictionary<string, object>>, Action<Player_move_c, Dictionary<string, object>>>(new Action<Player_move_c, Dictionary<string, object>>(Wear.Activate_Armor_Army_1), new Action<Player_move_c, Dictionary<string, object>>(Wear.deActivate_Armor_Army_1)));
		Wear.armorMethods.Add("Armor_Rubin_2", new KeyValuePair<Action<Player_move_c, Dictionary<string, object>>, Action<Player_move_c, Dictionary<string, object>>>(new Action<Player_move_c, Dictionary<string, object>>(Wear.Activate_Armor_Army_2), new Action<Player_move_c, Dictionary<string, object>>(Wear.deActivate_Armor_Army_2)));
		Wear.armorMethods.Add("Armor_Rubin_3", new KeyValuePair<Action<Player_move_c, Dictionary<string, object>>, Action<Player_move_c, Dictionary<string, object>>>(new Action<Player_move_c, Dictionary<string, object>>(Wear.Activate_Armor_Army_3), new Action<Player_move_c, Dictionary<string, object>>(Wear.deActivate_Armor_Army_3)));
		Wear.armorMethods.Add("Armor_Adamant_Const_1", new KeyValuePair<Action<Player_move_c, Dictionary<string, object>>, Action<Player_move_c, Dictionary<string, object>>>(new Action<Player_move_c, Dictionary<string, object>>(Wear.Activate_Armor_Army_1), new Action<Player_move_c, Dictionary<string, object>>(Wear.deActivate_Armor_Army_1)));
		Wear.armorMethods.Add("Armor_Adamant_Const_2", new KeyValuePair<Action<Player_move_c, Dictionary<string, object>>, Action<Player_move_c, Dictionary<string, object>>>(new Action<Player_move_c, Dictionary<string, object>>(Wear.Activate_Armor_Army_2), new Action<Player_move_c, Dictionary<string, object>>(Wear.deActivate_Armor_Army_2)));
		Wear.armorMethods.Add("Armor_Adamant_Const_3", new KeyValuePair<Action<Player_move_c, Dictionary<string, object>>, Action<Player_move_c, Dictionary<string, object>>>(new Action<Player_move_c, Dictionary<string, object>>(Wear.Activate_Armor_Army_3), new Action<Player_move_c, Dictionary<string, object>>(Wear.deActivate_Armor_Army_3)));
		Wear.armorMethods.Add("hat_Rubin_1", new KeyValuePair<Action<Player_move_c, Dictionary<string, object>>, Action<Player_move_c, Dictionary<string, object>>>(new Action<Player_move_c, Dictionary<string, object>>(Wear.Activate_Armor_Army_1), new Action<Player_move_c, Dictionary<string, object>>(Wear.deActivate_Armor_Army_1)));
		Wear.armorMethods.Add("hat_Rubin_2", new KeyValuePair<Action<Player_move_c, Dictionary<string, object>>, Action<Player_move_c, Dictionary<string, object>>>(new Action<Player_move_c, Dictionary<string, object>>(Wear.Activate_Armor_Army_2), new Action<Player_move_c, Dictionary<string, object>>(Wear.deActivate_Armor_Army_2)));
		Wear.armorMethods.Add("hat_Rubin_3", new KeyValuePair<Action<Player_move_c, Dictionary<string, object>>, Action<Player_move_c, Dictionary<string, object>>>(new Action<Player_move_c, Dictionary<string, object>>(Wear.Activate_Armor_Army_3), new Action<Player_move_c, Dictionary<string, object>>(Wear.deActivate_Armor_Army_3)));
		Wear.armorMethods.Add("hat_Adamant_Const_1", new KeyValuePair<Action<Player_move_c, Dictionary<string, object>>, Action<Player_move_c, Dictionary<string, object>>>(new Action<Player_move_c, Dictionary<string, object>>(Wear.Activate_Armor_Army_1), new Action<Player_move_c, Dictionary<string, object>>(Wear.deActivate_Armor_Army_1)));
		Wear.armorMethods.Add("hat_Adamant_Const_2", new KeyValuePair<Action<Player_move_c, Dictionary<string, object>>, Action<Player_move_c, Dictionary<string, object>>>(new Action<Player_move_c, Dictionary<string, object>>(Wear.Activate_Armor_Army_2), new Action<Player_move_c, Dictionary<string, object>>(Wear.deActivate_Armor_Army_2)));
		Wear.armorMethods.Add("hat_Adamant_Const_3", new KeyValuePair<Action<Player_move_c, Dictionary<string, object>>, Action<Player_move_c, Dictionary<string, object>>>(new Action<Player_move_c, Dictionary<string, object>>(Wear.Activate_Armor_Army_3), new Action<Player_move_c, Dictionary<string, object>>(Wear.deActivate_Armor_Army_3)));
		Wear.armorNum.Add("Armor_Novice", 0f);
		Wear.armorNum.Add("Armor_Army_1", 1f);
		Wear.armorNum.Add("Armor_Army_2", 2f);
		Wear.armorNum.Add("Armor_Army_3", 3f);
		Wear.armorNum.Add("Armor_Army_4", 9f);
		Wear.armorNum.Add("Armor_Steel_1", 4f);
		Wear.armorNum.Add("Armor_Steel_2", 5f);
		Wear.armorNum.Add("Armor_Steel_3", 8f);
		Wear.armorNum.Add("Armor_Steel_4", 27f);
		Wear.armorNum.Add("Armor_Royal_1", 9f);
		Wear.armorNum.Add("Armor_Royal_2", 10f);
		Wear.armorNum.Add("Armor_Royal_3", 14f);
		Wear.armorNum.Add("Armor_Royal_4", 63f);
		Wear.armorNum.Add("Armor_Almaz_1", 15f);
		Wear.armorNum.Add("Armor_Almaz_2", 16f);
		Wear.armorNum.Add("Armor_Almaz_3", 18f);
		Wear.armorNum.Add("Armor_Almaz_4", 133f);
		Wear.armorNum.Add("Armor_Rubin_1", 19f);
		Wear.armorNum.Add("Armor_Rubin_2", 20f);
		Wear.armorNum.Add("Armor_Rubin_3", 22f);
		Wear.armorNum.Add("Armor_Adamant_Const_1", 24f);
		Wear.armorNum.Add("Armor_Adamant_Const_2", 26f);
		Wear.armorNum.Add("Armor_Adamant_Const_3", 28f);
		Wear.armorNum.Add("hat_Army_1", 1f);
		Wear.armorNum.Add("hat_Steel_1", 4f);
		Wear.armorNum.Add("hat_Royal_1", 9f);
		Wear.armorNum.Add("hat_Almaz_1", 15f);
		Wear.armorNum.Add("hat_Army_2", 2f);
		Wear.armorNum.Add("hat_Steel_2", 5f);
		Wear.armorNum.Add("hat_Royal_2", 10f);
		Wear.armorNum.Add("hat_Almaz_2", 16f);
		Wear.armorNum.Add("hat_Army_3", 3f);
		Wear.armorNum.Add("hat_Steel_3", 8f);
		Wear.armorNum.Add("hat_Royal_3", 14f);
		Wear.armorNum.Add("hat_Almaz_3", 18f);
		Wear.armorNum.Add("hat_Army_4", 1f);
		Wear.armorNum.Add("hat_Steel_4", 1f);
		Wear.armorNum.Add("hat_Royal_4", 2f);
		Wear.armorNum.Add("hat_Almaz_4", 3f);
		Wear.armorNum.Add("hat_Rubin_1", 19f);
		Wear.armorNum.Add("hat_Rubin_2", 20f);
		Wear.armorNum.Add("hat_Rubin_3", 22f);
		Wear.armorNum.Add("hat_Adamant_Const_1", 24f);
		Wear.armorNum.Add("hat_Adamant_Const_2", 26f);
		Wear.armorNum.Add("hat_Adamant_Const_3", 28f);
	}

	// Token: 0x060044DE RID: 17630 RVA: 0x00174D20 File Offset: 0x00172F20
	public static void RemoveTemporaryWear(string item)
	{
	}

	// Token: 0x060044DF RID: 17631 RVA: 0x00174D24 File Offset: 0x00172F24
	public static string ArmorOrArmorHatAvailableForBuy(ShopNGUIController.CategoryNames category)
	{
		if (category != ShopNGUIController.CategoryNames.ArmorCategory && category != ShopNGUIController.CategoryNames.HatsCategory)
		{
			Debug.LogError("ArmorOrArmorHatAvailableForBuy incorrect category " + category);
			return string.Empty;
		}
		if (category == ShopNGUIController.CategoryNames.ArmorCategory && ShopNGUIController.NoviceArmorAvailable)
		{
			return string.Empty;
		}
		string result;
		try
		{
			string text = WeaponManager.LastBoughtTag(Wear.wear[category][0][0], null);
			string text2 = WeaponManager.FirstUnboughtTag(Wear.wear[category][0][0]);
			if (text != null && text == text2)
			{
				result = string.Empty;
			}
			else if (Wear.TierForWear(text2) <= ExpController.OurTierForAnyPlace())
			{
				result = text2;
			}
			else
			{
				result = string.Empty;
			}
		}
		catch (Exception arg)
		{
			Debug.LogError("ArmorOrArmorHatAvailableForBuy Exception: " + arg);
			result = string.Empty;
		}
		return result;
	}

	// Token: 0x060044E0 RID: 17632 RVA: 0x00174E30 File Offset: 0x00173030
	public static bool NonArmorHat(string showHatTag)
	{
		if (showHatTag != null)
		{
			if (Wear.wear[ShopNGUIController.CategoryNames.HatsCategory].SelectMany((List<string> list) => list).Contains(showHatTag) && showHatTag != "hat_Adamant_3")
			{
				return !Wear.wear[ShopNGUIController.CategoryNames.HatsCategory][0].Contains(showHatTag);
			}
		}
		return false;
	}

	// Token: 0x060044E1 RID: 17633 RVA: 0x00174EA8 File Offset: 0x001730A8
	public static float MaxArmorForItem(string armorName, int roomTier)
	{
		float num = 0f;
		if (armorName != null && TempItemsController.PriceCoefs.ContainsKey(armorName) && ExpController.Instance != null)
		{
			if (Wear.armorNumTemp.ContainsKey(armorName) && Wear.armorNumTemp[armorName].Count > ExpController.Instance.OurTier)
			{
				num = Wear.armorNumTemp[armorName][Math.Min(roomTier, ExpController.Instance.OurTier)];
			}
		}
		else
		{
			int num2 = Math.Min((!(ExpController.Instance != null)) ? (ExpController.LevelsForTiers.Length - 1) : ExpController.Instance.OurTier, roomTier);
			bool flag = false;
			List<string> list = null;
			foreach (List<List<string>> list2 in Wear.wear.Values)
			{
				foreach (List<string> list3 in list2)
				{
					if (list3.Contains(armorName ?? string.Empty))
					{
						flag = true;
						list = list3;
						break;
					}
				}
				if (flag)
				{
					break;
				}
			}
			if (list != null)
			{
				int num3 = list.IndexOf(armorName ?? string.Empty);
				if (num3 > 3 * (num2 + 1) - 1)
				{
					armorName = list[3 * (num2 + 1) - 1];
				}
			}
			Wear.armorNum.TryGetValue(armorName ?? string.Empty, out num);
		}
		num *= EffectsController.IcnreaseEquippedArmorPercentage;
		return num;
	}

	// Token: 0x060044E2 RID: 17634 RVA: 0x00175098 File Offset: 0x00173298
	public static int GetArmorCountFor(string armorTag, string hatTag)
	{
		return (int)(Wear.MaxArmorForItem(armorTag, ExpController.LevelsForTiers.Length - 1) + Wear.MaxArmorForItem(hatTag, ExpController.LevelsForTiers.Length - 1));
	}

	// Token: 0x060044E3 RID: 17635 RVA: 0x001750C8 File Offset: 0x001732C8
	public static List<string> AllWears(ShopNGUIController.CategoryNames category, bool onlyNonLeagueItems = true)
	{
		List<string> list = new List<string>();
		list = (from l in Wear.wear[category]
		from i in l
		select i).ToList<string>();
		if (onlyNonLeagueItems)
		{
			try
			{
				list = (from item in list
				where Wear.LeagueForWear(item, category) == 0 && item != "league1_hat_hitman"
				select item).ToList<string>();
			}
			catch (Exception arg)
			{
				Debug.LogError("Exception in AllWears filtering onlyNonLeagueItems: " + arg);
			}
		}
		return list;
	}

	// Token: 0x060044E4 RID: 17636 RVA: 0x00175194 File Offset: 0x00173394
	public static List<string> AllWears(ShopNGUIController.CategoryNames category, int tier, bool includePreviousTiers_UNUSED = false, bool withoutUpgrades = false)
	{
		List<int> list = new List<int>();
		for (int i = 0; i <= tier; i++)
		{
			list.Add(i);
		}
		List<string> list2 = new List<string>();
		foreach (string text in Wear.AllWears(category, true))
		{
			int item2 = Wear.TierForWear(text);
			if (list.Contains(item2))
			{
				list2.Add(text);
			}
		}
		if (withoutUpgrades)
		{
			List<List<string>> source = Wear.wear[category];
			for (int j = list2.Count; j > 0; j--)
			{
				string item = list2[j - 1];
				if (source.All((List<string> l) => l[0] != item))
				{
					list2.Remove(item);
				}
			}
		}
		return list2;
	}

	// Token: 0x060044E5 RID: 17637 RVA: 0x001752A8 File Offset: 0x001734A8
	public static int LeagueForWear(string name, ShopNGUIController.CategoryNames category)
	{
		if (name == null)
		{
			Debug.LogError("LeagueForWear: name == null");
			return 0;
		}
		ShopPositionParams shopPositionParams = null;
		try
		{
			shopPositionParams = ItemDb.GetInfoForNonWeaponItem(name, category);
		}
		catch (Exception arg)
		{
			Debug.LogError("LeagueForWear: Exception: " + arg);
		}
		return (!(shopPositionParams != null)) ? 0 : shopPositionParams.League;
	}

	// Token: 0x060044E6 RID: 17638 RVA: 0x00175320 File Offset: 0x00173520
	public static Dictionary<RatingSystem.RatingLeague, List<string>> UnboughtLeagueItemsByLeagues()
	{
		Dictionary<RatingSystem.RatingLeague, List<string>> dictionary = Wear.LeagueItemsByLeagues();
		try
		{
			dictionary = dictionary.ToDictionary((KeyValuePair<RatingSystem.RatingLeague, List<string>> kvp) => kvp.Key, (KeyValuePair<RatingSystem.RatingLeague, List<string>> kvp) => (from item in kvp.Value
			where Storager.getInt(item, true) == 0
			select item).ToList<string>(), RatingSystem.RatingLeagueComparer.Instance);
		}
		catch (Exception arg)
		{
			Debug.LogError("Exception in UnboughtLeagueItemsByLeagues: " + arg);
		}
		return dictionary;
	}

	// Token: 0x060044E7 RID: 17639 RVA: 0x001753B4 File Offset: 0x001735B4
	public static Dictionary<RatingSystem.RatingLeague, List<string>> LeagueItemsByLeagues()
	{
		Dictionary<RatingSystem.RatingLeague, List<string>> dictionary = new Dictionary<RatingSystem.RatingLeague, List<string>>(RatingSystem.RatingLeagueComparer.Instance);
		try
		{
			dictionary = (from item in Wear.wear[ShopNGUIController.CategoryNames.HatsCategory].SelectMany((List<string> list) => list)
			group item by Wear.LeagueForWear(item, ShopNGUIController.CategoryNames.HatsCategory) into grouping
			where grouping.Key > 0
			select grouping).ToDictionary((IGrouping<int, string> grouping) => (RatingSystem.RatingLeague)grouping.Key, (IGrouping<int, string> grouping) => grouping.ToList<string>(), RatingSystem.RatingLeagueComparer.Instance);
			dictionary[RatingSystem.RatingLeague.Wood] = new List<string>
			{
				"league1_hat_hitman"
			};
		}
		catch (Exception arg)
		{
			Debug.LogError("Exception in LeagueItemsByLeagues: " + arg);
		}
		return dictionary;
	}

	// Token: 0x060044E8 RID: 17640 RVA: 0x001754D4 File Offset: 0x001736D4
	public static Dictionary<Wear.LeagueItemState, List<string>> LeagueItems()
	{
		Dictionary<Wear.LeagueItemState, List<string>> dictionary = new Dictionary<Wear.LeagueItemState, List<string>>();
		try
		{
			IEnumerable<string> enumerable = from item in Wear.wear[ShopNGUIController.CategoryNames.HatsCategory].SelectMany((List<string> list) => list)
			where Wear.LeagueForWear(item, ShopNGUIController.CategoryNames.HatsCategory) > 0 || item == "league1_hat_hitman"
			select item;
			dictionary[Wear.LeagueItemState.Open] = (from item in enumerable
			where Wear.LeagueForWear(item, ShopNGUIController.CategoryNames.HatsCategory) <= (int)RatingSystem.instance.currentLeague
			orderby Wear.LeagueForWear(item, ShopNGUIController.CategoryNames.HatsCategory)
			select item).ToList<string>();
			dictionary[Wear.LeagueItemState.Closed] = (from item in enumerable.Except(dictionary[Wear.LeagueItemState.Open])
			orderby Wear.LeagueForWear(item, ShopNGUIController.CategoryNames.HatsCategory)
			select item).ToList<string>();
			dictionary[Wear.LeagueItemState.Purchased] = (from item in enumerable
			where Storager.getInt(item, true) > 0
			orderby Wear.LeagueForWear(item, ShopNGUIController.CategoryNames.HatsCategory)
			select item).ToList<string>();
		}
		catch (Exception arg)
		{
			Debug.LogError("Exception in LeagueItems: " + arg);
		}
		return dictionary;
	}

	// Token: 0x060044E9 RID: 17641 RVA: 0x0017564C File Offset: 0x0017384C
	public static int TierForWear(string w)
	{
		if (w == null)
		{
			return 0;
		}
		if (Wear.wear[ShopNGUIController.CategoryNames.HatsCategory][0].Contains(w))
		{
			return Wear.wear[ShopNGUIController.CategoryNames.HatsCategory][0].IndexOf(w) / 3;
		}
		if (Wear.wear[ShopNGUIController.CategoryNames.ArmorCategory][0].Contains(w))
		{
			return Wear.wear[ShopNGUIController.CategoryNames.ArmorCategory][0].IndexOf(w) / 3;
		}
		foreach (KeyValuePair<ShopNGUIController.CategoryNames, List<List<string>>> keyValuePair in Wear.wear)
		{
			foreach (List<string> list in keyValuePair.Value)
			{
				if (list.Contains(w))
				{
					return (keyValuePair.Key != ShopNGUIController.CategoryNames.MaskCategory) ? list.IndexOf(w) : (list.IndexOf(w) * 2);
				}
			}
		}
		return 0;
	}

	// Token: 0x060044EA RID: 17642 RVA: 0x001757A8 File Offset: 0x001739A8
	public static void ActivateBoots_red(Player_move_c move, Dictionary<string, object> p)
	{
		if (move == null)
		{
			return;
		}
	}

	// Token: 0x060044EB RID: 17643 RVA: 0x001757B8 File Offset: 0x001739B8
	public static void deActivateBoots_red(Player_move_c move, Dictionary<string, object> p)
	{
		if (move == null)
		{
			return;
		}
	}

	// Token: 0x060044EC RID: 17644 RVA: 0x001757C8 File Offset: 0x001739C8
	public static void Activate_boots_tabi(Player_move_c move, Dictionary<string, object> p)
	{
		if (move == null)
		{
			return;
		}
	}

	// Token: 0x060044ED RID: 17645 RVA: 0x001757D8 File Offset: 0x001739D8
	public static void deActivate_boots_tabi(Player_move_c move, Dictionary<string, object> p)
	{
		if (move == null)
		{
			return;
		}
	}

	// Token: 0x060044EE RID: 17646 RVA: 0x001757E8 File Offset: 0x001739E8
	public static void ActivateBoots_grey(Player_move_c move, Dictionary<string, object> p)
	{
		if (move == null)
		{
			return;
		}
	}

	// Token: 0x060044EF RID: 17647 RVA: 0x001757F8 File Offset: 0x001739F8
	public static void deActivateBoots_grey(Player_move_c move, Dictionary<string, object> p)
	{
		if (move == null)
		{
			return;
		}
	}

	// Token: 0x060044F0 RID: 17648 RVA: 0x00175808 File Offset: 0x00173A08
	public static void ActivateBoots_blue(Player_move_c move, Dictionary<string, object> p)
	{
		if (move == null)
		{
			return;
		}
	}

	// Token: 0x060044F1 RID: 17649 RVA: 0x00175818 File Offset: 0x00173A18
	public static void deActivateBoots_blue(Player_move_c move, Dictionary<string, object> p)
	{
		if (move == null)
		{
			return;
		}
	}

	// Token: 0x060044F2 RID: 17650 RVA: 0x00175828 File Offset: 0x00173A28
	public static void ActivateBoots_green(Player_move_c move, Dictionary<string, object> p)
	{
		if (move == null)
		{
			return;
		}
	}

	// Token: 0x060044F3 RID: 17651 RVA: 0x00175838 File Offset: 0x00173A38
	public static void deActivateBoots_green(Player_move_c move, Dictionary<string, object> p)
	{
		if (move == null)
		{
			return;
		}
	}

	// Token: 0x060044F4 RID: 17652 RVA: 0x00175848 File Offset: 0x00173A48
	public static void ActivateBoots_black(Player_move_c move, Dictionary<string, object> p)
	{
		if (move == null)
		{
			return;
		}
	}

	// Token: 0x060044F5 RID: 17653 RVA: 0x00175858 File Offset: 0x00173A58
	public static void deActivateBoots_black(Player_move_c move, Dictionary<string, object> p)
	{
		if (move == null)
		{
			return;
		}
	}

	// Token: 0x060044F6 RID: 17654 RVA: 0x00175868 File Offset: 0x00173A68
	public static void Activate_cape_BloodyDemon(Player_move_c move, Dictionary<string, object> p)
	{
		if (move == null)
		{
			return;
		}
	}

	// Token: 0x060044F7 RID: 17655 RVA: 0x00175878 File Offset: 0x00173A78
	public static void deActivate_cape_BloodyDemon(Player_move_c move, Dictionary<string, object> p)
	{
		if (move == null)
		{
			return;
		}
	}

	// Token: 0x060044F8 RID: 17656 RVA: 0x00175888 File Offset: 0x00173A88
	public static void Activate_cape_RoyalKnight(Player_move_c move, Dictionary<string, object> p)
	{
		if (move == null)
		{
			return;
		}
	}

	// Token: 0x060044F9 RID: 17657 RVA: 0x00175898 File Offset: 0x00173A98
	public static void deActivate_cape_RoyalKnight(Player_move_c move, Dictionary<string, object> p)
	{
		if (move == null)
		{
			return;
		}
	}

	// Token: 0x060044FA RID: 17658 RVA: 0x001758A8 File Offset: 0x00173AA8
	public static void Activate_cape_SkeletonLord(Player_move_c move, Dictionary<string, object> p)
	{
		if (move == null)
		{
			return;
		}
	}

	// Token: 0x060044FB RID: 17659 RVA: 0x001758B8 File Offset: 0x00173AB8
	public static void deActivate_cape_SkeletonLord(Player_move_c move, Dictionary<string, object> p)
	{
		if (move == null)
		{
			return;
		}
	}

	// Token: 0x060044FC RID: 17660 RVA: 0x001758C8 File Offset: 0x00173AC8
	public static void Activate_cape_Archimage(Player_move_c move, Dictionary<string, object> p)
	{
		if (move == null)
		{
			return;
		}
	}

	// Token: 0x060044FD RID: 17661 RVA: 0x001758D8 File Offset: 0x00173AD8
	public static void deActivate_cape_Archimage(Player_move_c move, Dictionary<string, object> p)
	{
		if (move == null)
		{
			return;
		}
	}

	// Token: 0x060044FE RID: 17662 RVA: 0x001758E8 File Offset: 0x00173AE8
	public static void Activate_cape_EliteCrafter(Player_move_c move, Dictionary<string, object> p)
	{
		if (move == null)
		{
			return;
		}
	}

	// Token: 0x060044FF RID: 17663 RVA: 0x001758F8 File Offset: 0x00173AF8
	public static void deActivate_cape_EliteCrafter(Player_move_c move, Dictionary<string, object> p)
	{
		if (move == null)
		{
			return;
		}
	}

	// Token: 0x06004500 RID: 17664 RVA: 0x00175908 File Offset: 0x00173B08
	public static void Activate_cape_Custom(Player_move_c move, Dictionary<string, object> p)
	{
		if (move == null || Defs.isHunger)
		{
			return;
		}
		move.koofDamageWeaponFromPotoins += 0.05f;
	}

	// Token: 0x06004501 RID: 17665 RVA: 0x00175934 File Offset: 0x00173B34
	public static void deActivate_cape_Custom(Player_move_c move, Dictionary<string, object> p)
	{
		if (move == null || Defs.isHunger)
		{
			return;
		}
		move.koofDamageWeaponFromPotoins -= 0.05f;
	}

	// Token: 0x06004502 RID: 17666 RVA: 0x00175960 File Offset: 0x00173B60
	public static void Activate_hat_EMPTY(Player_move_c move, Dictionary<string, object> p)
	{
		if (move == null)
		{
			return;
		}
	}

	// Token: 0x06004503 RID: 17667 RVA: 0x00175970 File Offset: 0x00173B70
	public static void deActivate_hat_EMPTY(Player_move_c move, Dictionary<string, object> p)
	{
		if (move == null)
		{
			return;
		}
	}

	// Token: 0x06004504 RID: 17668 RVA: 0x00175980 File Offset: 0x00173B80
	public static void Activate_hat_ManiacMask(Player_move_c move, Dictionary<string, object> p)
	{
		if (move == null)
		{
			return;
		}
	}

	// Token: 0x06004505 RID: 17669 RVA: 0x00175990 File Offset: 0x00173B90
	public static void deActivate_hat_ManiacMask(Player_move_c move, Dictionary<string, object> p)
	{
		if (move == null)
		{
			return;
		}
	}

	// Token: 0x06004506 RID: 17670 RVA: 0x001759A0 File Offset: 0x00173BA0
	public static void Activate_hat_KingsCrown(Player_move_c move, Dictionary<string, object> p)
	{
		if (move == null || Defs.isHunger)
		{
			return;
		}
		move.koofDamageWeaponFromPotoins += 0.05f;
	}

	// Token: 0x06004507 RID: 17671 RVA: 0x001759CC File Offset: 0x00173BCC
	public static void deActivate_hat_KingsCrown(Player_move_c move, Dictionary<string, object> p)
	{
		if (move == null || Defs.isHunger)
		{
			return;
		}
		move.koofDamageWeaponFromPotoins -= 0.05f;
	}

	// Token: 0x06004508 RID: 17672 RVA: 0x001759F8 File Offset: 0x00173BF8
	public static void Activate_hat_Samurai(Player_move_c move, Dictionary<string, object> p)
	{
		if (move == null || Defs.isHunger)
		{
			return;
		}
		move.koofDamageWeaponFromPotoins += 0.05f;
	}

	// Token: 0x06004509 RID: 17673 RVA: 0x00175A24 File Offset: 0x00173C24
	public static void deActivate_hat_Samurai(Player_move_c move, Dictionary<string, object> p)
	{
		if (move == null || Defs.isHunger)
		{
			return;
		}
		move.koofDamageWeaponFromPotoins -= 0.05f;
	}

	// Token: 0x0600450A RID: 17674 RVA: 0x00175A50 File Offset: 0x00173C50
	public static void Activate_hat_DiamondHelmet(Player_move_c move, Dictionary<string, object> p)
	{
		if (move == null)
		{
			return;
		}
	}

	// Token: 0x0600450B RID: 17675 RVA: 0x00175A60 File Offset: 0x00173C60
	public static void deActivate_hat_DiamondHelmet(Player_move_c move, Dictionary<string, object> p)
	{
		if (move == null)
		{
			return;
		}
	}

	// Token: 0x0600450C RID: 17676 RVA: 0x00175A70 File Offset: 0x00173C70
	public static void Activate_hat_SeriousManHat(Player_move_c move, Dictionary<string, object> p)
	{
		if (move == null)
		{
			return;
		}
	}

	// Token: 0x0600450D RID: 17677 RVA: 0x00175A80 File Offset: 0x00173C80
	public static void deActivate_hat_SeriousManHat(Player_move_c move, Dictionary<string, object> p)
	{
		if (move == null)
		{
			return;
		}
	}

	// Token: 0x0600450E RID: 17678 RVA: 0x00175A90 File Offset: 0x00173C90
	public static void Activate_Armor_EMPTY(Player_move_c move, Dictionary<string, object> p)
	{
		if (move == null)
		{
			return;
		}
	}

	// Token: 0x0600450F RID: 17679 RVA: 0x00175AA0 File Offset: 0x00173CA0
	public static void deActivate_Armor_EMPTY(Player_move_c move, Dictionary<string, object> p)
	{
		if (move == null)
		{
			return;
		}
	}

	// Token: 0x06004510 RID: 17680 RVA: 0x00175AB0 File Offset: 0x00173CB0
	public static void Activate_Armor_Steel_2(Player_move_c move, Dictionary<string, object> p)
	{
		if (move == null)
		{
			return;
		}
	}

	// Token: 0x06004511 RID: 17681 RVA: 0x00175AC0 File Offset: 0x00173CC0
	public static void deActivate_Armor_Steel_2(Player_move_c move, Dictionary<string, object> p)
	{
		if (move == null)
		{
			return;
		}
	}

	// Token: 0x06004512 RID: 17682 RVA: 0x00175AD0 File Offset: 0x00173CD0
	public static void Activate_Armor_Steel_3(Player_move_c move, Dictionary<string, object> p)
	{
		if (move == null)
		{
			return;
		}
	}

	// Token: 0x06004513 RID: 17683 RVA: 0x00175AE0 File Offset: 0x00173CE0
	public static void deActivate_Armor_Steel_3(Player_move_c move, Dictionary<string, object> p)
	{
		if (move == null)
		{
			return;
		}
	}

	// Token: 0x06004514 RID: 17684 RVA: 0x00175AF0 File Offset: 0x00173CF0
	public static void Activate_Armor_Steel_4(Player_move_c move, Dictionary<string, object> p)
	{
		if (move == null)
		{
			return;
		}
	}

	// Token: 0x06004515 RID: 17685 RVA: 0x00175B00 File Offset: 0x00173D00
	public static void deActivate_Armor_Steel_4(Player_move_c move, Dictionary<string, object> p)
	{
		if (move == null)
		{
			return;
		}
	}

	// Token: 0x06004516 RID: 17686 RVA: 0x00175B10 File Offset: 0x00173D10
	public static void Activate_Armor_Royal_4(Player_move_c move, Dictionary<string, object> p)
	{
		if (move == null)
		{
			return;
		}
	}

	// Token: 0x06004517 RID: 17687 RVA: 0x00175B20 File Offset: 0x00173D20
	public static void deActivate_Armor_Royal_4(Player_move_c move, Dictionary<string, object> p)
	{
		if (move == null)
		{
			return;
		}
	}

	// Token: 0x06004518 RID: 17688 RVA: 0x00175B30 File Offset: 0x00173D30
	public static void Activate_Armor_Royal_1(Player_move_c move, Dictionary<string, object> p)
	{
		if (move == null)
		{
			return;
		}
	}

	// Token: 0x06004519 RID: 17689 RVA: 0x00175B40 File Offset: 0x00173D40
	public static void deActivate_Armor_Royal_1(Player_move_c move, Dictionary<string, object> p)
	{
		if (move == null)
		{
			return;
		}
	}

	// Token: 0x0600451A RID: 17690 RVA: 0x00175B50 File Offset: 0x00173D50
	public static void Activate_Armor_Royal_2(Player_move_c move, Dictionary<string, object> p)
	{
		if (move == null)
		{
			return;
		}
	}

	// Token: 0x0600451B RID: 17691 RVA: 0x00175B60 File Offset: 0x00173D60
	public static void deActivate_Armor_Royal_2(Player_move_c move, Dictionary<string, object> p)
	{
		if (move == null)
		{
			return;
		}
	}

	// Token: 0x0600451C RID: 17692 RVA: 0x00175B70 File Offset: 0x00173D70
	public static void Activate_Armor_Royal_3(Player_move_c move, Dictionary<string, object> p)
	{
		if (move == null)
		{
			return;
		}
	}

	// Token: 0x0600451D RID: 17693 RVA: 0x00175B80 File Offset: 0x00173D80
	public static void deActivate_Armor_Royal_3(Player_move_c move, Dictionary<string, object> p)
	{
		if (move == null)
		{
			return;
		}
	}

	// Token: 0x0600451E RID: 17694 RVA: 0x00175B90 File Offset: 0x00173D90
	public static void Activate_Armor_Almaz_1(Player_move_c move, Dictionary<string, object> p)
	{
		if (move == null)
		{
			return;
		}
	}

	// Token: 0x0600451F RID: 17695 RVA: 0x00175BA0 File Offset: 0x00173DA0
	public static void deActivate_Armor_Almaz_1(Player_move_c move, Dictionary<string, object> p)
	{
		if (move == null)
		{
			return;
		}
	}

	// Token: 0x06004520 RID: 17696 RVA: 0x00175BB0 File Offset: 0x00173DB0
	public static void Activate_Armor_Almaz_2(Player_move_c move, Dictionary<string, object> p)
	{
		if (move == null)
		{
			return;
		}
	}

	// Token: 0x06004521 RID: 17697 RVA: 0x00175BC0 File Offset: 0x00173DC0
	public static void deActivate_Armor_Almaz_2(Player_move_c move, Dictionary<string, object> p)
	{
		if (move == null)
		{
			return;
		}
	}

	// Token: 0x06004522 RID: 17698 RVA: 0x00175BD0 File Offset: 0x00173DD0
	public static void Activate_Armor_Almaz_3(Player_move_c move, Dictionary<string, object> p)
	{
		if (move == null)
		{
			return;
		}
	}

	// Token: 0x06004523 RID: 17699 RVA: 0x00175BE0 File Offset: 0x00173DE0
	public static void deActivate_Armor_Almaz_3(Player_move_c move, Dictionary<string, object> p)
	{
		if (move == null)
		{
			return;
		}
	}

	// Token: 0x06004524 RID: 17700 RVA: 0x00175BF0 File Offset: 0x00173DF0
	public static void Activate_Armor_Almaz_4(Player_move_c move, Dictionary<string, object> p)
	{
		if (move == null)
		{
			return;
		}
	}

	// Token: 0x06004525 RID: 17701 RVA: 0x00175C00 File Offset: 0x00173E00
	public static void deActivate_Armor_Almaz_4(Player_move_c move, Dictionary<string, object> p)
	{
		if (move == null)
		{
			return;
		}
	}

	// Token: 0x06004526 RID: 17702 RVA: 0x00175C10 File Offset: 0x00173E10
	public static void Activate_Armor_Army_1(Player_move_c move, Dictionary<string, object> p)
	{
		if (move == null)
		{
			return;
		}
	}

	// Token: 0x06004527 RID: 17703 RVA: 0x00175C20 File Offset: 0x00173E20
	public static void deActivate_Armor_Army_1(Player_move_c move, Dictionary<string, object> p)
	{
		if (move == null)
		{
			return;
		}
	}

	// Token: 0x06004528 RID: 17704 RVA: 0x00175C30 File Offset: 0x00173E30
	public static void Activate_Armor_Army_2(Player_move_c move, Dictionary<string, object> p)
	{
		if (move == null)
		{
			return;
		}
	}

	// Token: 0x06004529 RID: 17705 RVA: 0x00175C40 File Offset: 0x00173E40
	public static void deActivate_Armor_Army_2(Player_move_c move, Dictionary<string, object> p)
	{
		if (move == null)
		{
			return;
		}
	}

	// Token: 0x0600452A RID: 17706 RVA: 0x00175C50 File Offset: 0x00173E50
	public static void Activate_Armor_Army_3(Player_move_c move, Dictionary<string, object> p)
	{
		if (move == null)
		{
			return;
		}
	}

	// Token: 0x0600452B RID: 17707 RVA: 0x00175C60 File Offset: 0x00173E60
	public static void deActivate_Armor_Army_3(Player_move_c move, Dictionary<string, object> p)
	{
		if (move == null)
		{
			return;
		}
	}

	// Token: 0x0600452C RID: 17708 RVA: 0x00175C70 File Offset: 0x00173E70
	public static void Activate_Armor_Army_4(Player_move_c move, Dictionary<string, object> p)
	{
		if (move == null)
		{
			return;
		}
	}

	// Token: 0x0600452D RID: 17709 RVA: 0x00175C80 File Offset: 0x00173E80
	public static void deActivate_Armor_Army_4(Player_move_c move, Dictionary<string, object> p)
	{
		if (move == null)
		{
			return;
		}
	}

	// Token: 0x0600452E RID: 17710 RVA: 0x00175C90 File Offset: 0x00173E90
	public static void Activate_hat_SteelHelmet(Player_move_c move, Dictionary<string, object> p)
	{
		if (move == null)
		{
			return;
		}
	}

	// Token: 0x0600452F RID: 17711 RVA: 0x00175CA0 File Offset: 0x00173EA0
	public static void deActivate_hat_SteelHelmet(Player_move_c move, Dictionary<string, object> p)
	{
		if (move == null)
		{
			return;
		}
	}

	// Token: 0x06004530 RID: 17712 RVA: 0x00175CB0 File Offset: 0x00173EB0
	public static void Activate_hat_GoldHelmet(Player_move_c move, Dictionary<string, object> p)
	{
		if (move == null)
		{
			return;
		}
	}

	// Token: 0x06004531 RID: 17713 RVA: 0x00175CC0 File Offset: 0x00173EC0
	public static void deActivate_hat_GoldHelmet(Player_move_c move, Dictionary<string, object> p)
	{
		if (move == null)
		{
			return;
		}
	}

	// Token: 0x06004532 RID: 17714 RVA: 0x00175CD0 File Offset: 0x00173ED0
	public static void Activate_hat_ArmyHelmet(Player_move_c move, Dictionary<string, object> p)
	{
		if (move == null)
		{
			return;
		}
	}

	// Token: 0x06004533 RID: 17715 RVA: 0x00175CE0 File Offset: 0x00173EE0
	public static void deActivate_hat_ArmyHelmet(Player_move_c move, Dictionary<string, object> p)
	{
		if (move == null)
		{
			return;
		}
	}

	// Token: 0x06004534 RID: 17716 RVA: 0x00175CF0 File Offset: 0x00173EF0
	public static void Activate_hat_AlmazHelmet(Player_move_c move, Dictionary<string, object> p)
	{
		if (move == null)
		{
			return;
		}
	}

	// Token: 0x06004535 RID: 17717 RVA: 0x00175D00 File Offset: 0x00173F00
	public static void deActivate_hat_AlmazHelmet(Player_move_c move, Dictionary<string, object> p)
	{
		if (move == null)
		{
			return;
		}
	}

	// Token: 0x06004536 RID: 17718 RVA: 0x00175D10 File Offset: 0x00173F10
	public static void Activate_hat_Steel_1(Player_move_c move, Dictionary<string, object> p)
	{
		if (move == null)
		{
			return;
		}
	}

	// Token: 0x06004537 RID: 17719 RVA: 0x00175D20 File Offset: 0x00173F20
	public static void deActivate_hat_Steel_1(Player_move_c move, Dictionary<string, object> p)
	{
		if (move == null)
		{
			return;
		}
	}

	// Token: 0x06004538 RID: 17720 RVA: 0x00175D30 File Offset: 0x00173F30
	public static void Activate_hat_Royal_1(Player_move_c move, Dictionary<string, object> p)
	{
		if (move == null)
		{
			return;
		}
	}

	// Token: 0x06004539 RID: 17721 RVA: 0x00175D40 File Offset: 0x00173F40
	public static void deActivate_hat_Royal_1(Player_move_c move, Dictionary<string, object> p)
	{
		if (move == null)
		{
			return;
		}
	}

	// Token: 0x0600453A RID: 17722 RVA: 0x00175D50 File Offset: 0x00173F50
	public static void Activate_hat_Almaz_1(Player_move_c move, Dictionary<string, object> p)
	{
		if (move == null)
		{
			return;
		}
	}

	// Token: 0x0600453B RID: 17723 RVA: 0x00175D60 File Offset: 0x00173F60
	public static void deActivate_hat_Almaz_1(Player_move_c move, Dictionary<string, object> p)
	{
		if (move == null)
		{
			return;
		}
	}

	// Token: 0x0600453C RID: 17724 RVA: 0x00175D70 File Offset: 0x00173F70
	public static void RenewCurArmor(int roomTier)
	{
		Wear.curArmor.Clear();
		foreach (string text in Wear.armorNum.Keys)
		{
			Wear.curArmor.Add(text, new SaltedFloat((!Defs.isHunger) ? Wear.MaxArmorForItem(text, roomTier) : 0f));
		}
		foreach (string text2 in Wear.armorNumTemp.Keys)
		{
			Wear.curArmor.Add(text2, new SaltedFloat((!Defs.isHunger) ? Wear.MaxArmorForItem(text2, roomTier) : 0f));
		}
	}

	// Token: 0x0400322D RID: 12845
	public const int IndexOfArmorHatsList = 0;

	// Token: 0x0400322E RID: 12846
	public const int NumberOfArmorsPerTier = 3;

	// Token: 0x0400322F RID: 12847
	public const string BerserkCape = "cape_BloodyDemon";

	// Token: 0x04003230 RID: 12848
	public const string DemolitionCape = "cape_RoyalKnight";

	// Token: 0x04003231 RID: 12849
	public const string SniperCape = "cape_SkeletonLord";

	// Token: 0x04003232 RID: 12850
	public const string HitmanCape = "cape_Archimage";

	// Token: 0x04003233 RID: 12851
	public const string StormTrooperCape = "cape_EliteCrafter";

	// Token: 0x04003234 RID: 12852
	public const string cape_Custom = "cape_Custom";

	// Token: 0x04003235 RID: 12853
	public const string hat_Headphones = "hat_Headphones";

	// Token: 0x04003236 RID: 12854
	public const string hat_ManiacMask = "hat_ManiacMask";

	// Token: 0x04003237 RID: 12855
	public const string hat_KingsCrown = "hat_KingsCrown";

	// Token: 0x04003238 RID: 12856
	public const string hat_Samurai = "hat_Samurai";

	// Token: 0x04003239 RID: 12857
	public const string hat_DiamondHelmet = "hat_DiamondHelmet";

	// Token: 0x0400323A RID: 12858
	public const string hat_SeriousManHat = "hat_SeriousManHat";

	// Token: 0x0400323B RID: 12859
	public const string hat_AlmazHelmet = "hat_AlmazHelmet";

	// Token: 0x0400323C RID: 12860
	public const string hat_ArmyHelmet = "hat_ArmyHelmet";

	// Token: 0x0400323D RID: 12861
	public const string hat_GoldHelmet = "hat_GoldHelmet";

	// Token: 0x0400323E RID: 12862
	public const string hat_SteelHelmet = "hat_SteelHelmet";

	// Token: 0x0400323F RID: 12863
	public const string league1_hat_hitman = "league1_hat_hitman";

	// Token: 0x04003240 RID: 12864
	public const string league2_hat_cowboyhat = "league2_hat_cowboyhat";

	// Token: 0x04003241 RID: 12865
	public const string league3_hat_afro = "league3_hat_afro";

	// Token: 0x04003242 RID: 12866
	public const string league4_hat_mushroom = "league4_hat_mushroom";

	// Token: 0x04003243 RID: 12867
	public const string league5_hat_brain = "league5_hat_brain";

	// Token: 0x04003244 RID: 12868
	public const string league6_hat_tiara = "league6_hat_tiara";

	// Token: 0x04003245 RID: 12869
	public const string hat_Army_1 = "hat_Army_1";

	// Token: 0x04003246 RID: 12870
	public const string hat_Royal_1 = "hat_Royal_1";

	// Token: 0x04003247 RID: 12871
	public const string hat_Almaz_1 = "hat_Almaz_1";

	// Token: 0x04003248 RID: 12872
	public const string hat_Steel_1 = "hat_Steel_1";

	// Token: 0x04003249 RID: 12873
	public const string hat_Army_2 = "hat_Army_2";

	// Token: 0x0400324A RID: 12874
	public const string hat_Royal_2 = "hat_Royal_2";

	// Token: 0x0400324B RID: 12875
	public const string hat_Almaz_2 = "hat_Almaz_2";

	// Token: 0x0400324C RID: 12876
	public const string hat_Steel_2 = "hat_Steel_2";

	// Token: 0x0400324D RID: 12877
	public const string hat_Army_3 = "hat_Army_3";

	// Token: 0x0400324E RID: 12878
	public const string hat_Royal_3 = "hat_Royal_3";

	// Token: 0x0400324F RID: 12879
	public const string hat_Almaz_3 = "hat_Almaz_3";

	// Token: 0x04003250 RID: 12880
	public const string hat_Steel_3 = "hat_Steel_3";

	// Token: 0x04003251 RID: 12881
	public const string hat_Army_4 = "hat_Army_4";

	// Token: 0x04003252 RID: 12882
	public const string hat_Royal_4 = "hat_Royal_4";

	// Token: 0x04003253 RID: 12883
	public const string hat_Almaz_4 = "hat_Almaz_4";

	// Token: 0x04003254 RID: 12884
	public const string hat_Steel_4 = "hat_Steel_4";

	// Token: 0x04003255 RID: 12885
	public const string hat_Rubin_1 = "hat_Rubin_1";

	// Token: 0x04003256 RID: 12886
	public const string hat_Rubin_2 = "hat_Rubin_2";

	// Token: 0x04003257 RID: 12887
	public const string hat_Rubin_3 = "hat_Rubin_3";

	// Token: 0x04003258 RID: 12888
	public const string BerserkBoots = "boots_black";

	// Token: 0x04003259 RID: 12889
	public const string SniperBoots = "boots_blue";

	// Token: 0x0400325A RID: 12890
	public const string StormTrooperBoots = "boots_gray";

	// Token: 0x0400325B RID: 12891
	public const string DemolitionBoots = "boots_green";

	// Token: 0x0400325C RID: 12892
	public const string HitmanBoots = "boots_red";

	// Token: 0x0400325D RID: 12893
	public const string boots_tabi = "boots_tabi";

	// Token: 0x0400325E RID: 12894
	public const string Armor_Steel = "Armor_Steel_1";

	// Token: 0x0400325F RID: 12895
	public const string Armor_Steel_2 = "Armor_Steel_2";

	// Token: 0x04003260 RID: 12896
	public const string Armor_Steel_3 = "Armor_Steel_3";

	// Token: 0x04003261 RID: 12897
	public const string Armor_Steel_4 = "Armor_Steel_4";

	// Token: 0x04003262 RID: 12898
	public const string Armor_Royal_1 = "Armor_Royal_1";

	// Token: 0x04003263 RID: 12899
	public const string Armor_Royal_2 = "Armor_Royal_2";

	// Token: 0x04003264 RID: 12900
	public const string Armor_Royal_3 = "Armor_Royal_3";

	// Token: 0x04003265 RID: 12901
	public const string Armor_Royal_4 = "Armor_Royal_4";

	// Token: 0x04003266 RID: 12902
	public const string Armor_Almaz_1 = "Armor_Almaz_1";

	// Token: 0x04003267 RID: 12903
	public const string Armor_Almaz_2 = "Armor_Almaz_2";

	// Token: 0x04003268 RID: 12904
	public const string Armor_Almaz_3 = "Armor_Almaz_3";

	// Token: 0x04003269 RID: 12905
	public const string Armor_Almaz_4 = "Armor_Almaz_4";

	// Token: 0x0400326A RID: 12906
	public const string Armor_Army_1 = "Armor_Army_1";

	// Token: 0x0400326B RID: 12907
	public const string Armor_Army_2 = "Armor_Army_2";

	// Token: 0x0400326C RID: 12908
	public const string Armor_Army_3 = "Armor_Army_3";

	// Token: 0x0400326D RID: 12909
	public const string Armor_Army_4 = "Armor_Army_4";

	// Token: 0x0400326E RID: 12910
	public const string Armor_Rubin_1 = "Armor_Rubin_1";

	// Token: 0x0400326F RID: 12911
	public const string Armor_Rubin_2 = "Armor_Rubin_2";

	// Token: 0x04003270 RID: 12912
	public const string Armor_Rubin_3 = "Armor_Rubin_3";

	// Token: 0x04003271 RID: 12913
	public const string StormTrooperCape_Up1 = "StormTrooperCape_Up1";

	// Token: 0x04003272 RID: 12914
	public const string StormTrooperCape_Up2 = "StormTrooperCape_Up2";

	// Token: 0x04003273 RID: 12915
	public const string HitmanCape_Up1 = "HitmanCape_Up1";

	// Token: 0x04003274 RID: 12916
	public const string HitmanCape_Up2 = "HitmanCape_Up2";

	// Token: 0x04003275 RID: 12917
	public const string BerserkCape_Up1 = "BerserkCape_Up1";

	// Token: 0x04003276 RID: 12918
	public const string BerserkCape_Up2 = "BerserkCape_Up2";

	// Token: 0x04003277 RID: 12919
	public const string SniperCape_Up1 = "SniperCape_Up1";

	// Token: 0x04003278 RID: 12920
	public const string SniperCape_Up2 = "SniperCape_Up2";

	// Token: 0x04003279 RID: 12921
	public const string EngineerCape = "cape_Engineer";

	// Token: 0x0400327A RID: 12922
	public const string EngineerCape_Up1 = "cape_Engineer_Up1";

	// Token: 0x0400327B RID: 12923
	public const string EngineerCape_Up2 = "cape_Engineer_Up2";

	// Token: 0x0400327C RID: 12924
	public const string DemolitionCape_Up1 = "DemolitionCape_Up1";

	// Token: 0x0400327D RID: 12925
	public const string DemolitionCape_Up2 = "DemolitionCape_Up2";

	// Token: 0x0400327E RID: 12926
	public const string hat_Headphones_Up1 = "hat_Headphones_Up1";

	// Token: 0x0400327F RID: 12927
	public const string hat_Headphones_Up2 = "hat_Headphones_Up2";

	// Token: 0x04003280 RID: 12928
	public const string hat_ManiacMask_Up1 = "hat_ManiacMask_Up1";

	// Token: 0x04003281 RID: 12929
	public const string hat_ManiacMask_Up2 = "hat_ManiacMask_Up2";

	// Token: 0x04003282 RID: 12930
	public const string hat_KingsCrown_Up1 = "hat_KingsCrown_Up1";

	// Token: 0x04003283 RID: 12931
	public const string hat_KingsCrown_Up2 = "hat_KingsCrown_Up2";

	// Token: 0x04003284 RID: 12932
	public const string hat_Samurai_Up1 = "hat_Samurai_Up1";

	// Token: 0x04003285 RID: 12933
	public const string hat_Samurai_Up2 = "hat_Samurai_Up2";

	// Token: 0x04003286 RID: 12934
	public const string hat_DiamondHelmet_Up1 = "hat_DiamondHelmet_Up1";

	// Token: 0x04003287 RID: 12935
	public const string hat_DiamondHelmet_Up2 = "hat_DiamondHelmet_Up2";

	// Token: 0x04003288 RID: 12936
	public const string hat_SeriousManHat_Up1 = "hat_SeriousManHat_Up1";

	// Token: 0x04003289 RID: 12937
	public const string hat_SeriousManHat_Up2 = "hat_SeriousManHat_Up2";

	// Token: 0x0400328A RID: 12938
	public const string StormTrooperBoots_Up1 = "StormTrooperBoots_Up1";

	// Token: 0x0400328B RID: 12939
	public const string StormTrooperBoots_Up2 = "StormTrooperBoots_Up2";

	// Token: 0x0400328C RID: 12940
	public const string HitmanBoots_Up1 = "HitmanBoots_Up1";

	// Token: 0x0400328D RID: 12941
	public const string HitmanBoots_Up2 = "HitmanBoots_Up2";

	// Token: 0x0400328E RID: 12942
	public const string BerserkBoots_Up1 = "BerserkBoots_Up1";

	// Token: 0x0400328F RID: 12943
	public const string BerserkBoots_Up2 = "BerserkBoots_Up2";

	// Token: 0x04003290 RID: 12944
	public const string SniperBoots_Up1 = "SniperBoots_Up1";

	// Token: 0x04003291 RID: 12945
	public const string SniperBoots_Up2 = "SniperBoots_Up2";

	// Token: 0x04003292 RID: 12946
	public const string DemolitionBoots_Up1 = "DemolitionBoots_Up1";

	// Token: 0x04003293 RID: 12947
	public const string DemolitionBoots_Up2 = "DemolitionBoots_Up2";

	// Token: 0x04003294 RID: 12948
	public const string EngineerBoots = "EngineerBoots";

	// Token: 0x04003295 RID: 12949
	public const string EngineerBoots_Up1 = "EngineerBoots_Up1";

	// Token: 0x04003296 RID: 12950
	public const string EngineerBoots_Up2 = "EngineerBoots_Up2";

	// Token: 0x04003297 RID: 12951
	public const string Armor_Novice = "Armor_Novice";

	// Token: 0x04003298 RID: 12952
	public const string Armor_Adamant_3 = "Armor_Adamant_3";

	// Token: 0x04003299 RID: 12953
	public const string hat_Adamant_3 = "hat_Adamant_3";

	// Token: 0x0400329A RID: 12954
	public const string Armor_Adamant_Const_1 = "Armor_Adamant_Const_1";

	// Token: 0x0400329B RID: 12955
	public const string Armor_Adamant_Const_2 = "Armor_Adamant_Const_2";

	// Token: 0x0400329C RID: 12956
	public const string Armor_Adamant_Const_3 = "Armor_Adamant_Const_3";

	// Token: 0x0400329D RID: 12957
	public const string hat_Adamant_Const_1 = "hat_Adamant_Const_1";

	// Token: 0x0400329E RID: 12958
	public const string hat_Adamant_Const_2 = "hat_Adamant_Const_2";

	// Token: 0x0400329F RID: 12959
	public const string hat_Adamant_Const_3 = "hat_Adamant_Const_3";

	// Token: 0x040032A0 RID: 12960
	public const string SniperMask = "mask_sniper";

	// Token: 0x040032A1 RID: 12961
	public const string SniperMask_Up1 = "mask_sniper_up1";

	// Token: 0x040032A2 RID: 12962
	public const string SniperMask_Up2 = "mask_sniper_up2";

	// Token: 0x040032A3 RID: 12963
	public const string DemolitionMask = "mask_demolition";

	// Token: 0x040032A4 RID: 12964
	public const string DemolitionMask_Up1 = "mask_demolition_up1";

	// Token: 0x040032A5 RID: 12965
	public const string DemolitionMask_Up2 = "mask_demolition_up2";

	// Token: 0x040032A6 RID: 12966
	public const string BerserkMask = "mask_berserk";

	// Token: 0x040032A7 RID: 12967
	public const string BerserkMask_Up1 = "mask_berserk_up1";

	// Token: 0x040032A8 RID: 12968
	public const string BerserkMask_Up2 = "mask_berserk_up2";

	// Token: 0x040032A9 RID: 12969
	public const string StormTrooperMask = "mask_trooper";

	// Token: 0x040032AA RID: 12970
	public const string StormTrooperMask_Up1 = "mask_trooper_up1";

	// Token: 0x040032AB RID: 12971
	public const string StormTrooperMask_Up2 = "mask_trooper_up2";

	// Token: 0x040032AC RID: 12972
	public const string HitmanMask = "mask_hitman_1";

	// Token: 0x040032AD RID: 12973
	public const string HitmanMask_Up1 = "mask_hitman_1_up1";

	// Token: 0x040032AE RID: 12974
	public const string HitmanMask_Up2 = "mask_hitman_1_up2";

	// Token: 0x040032AF RID: 12975
	public const string EngineerMask = "mask_engineer";

	// Token: 0x040032B0 RID: 12976
	public const string EngineerMask_Up1 = "mask_engineer_up1";

	// Token: 0x040032B1 RID: 12977
	public const string EngineerMask_Up2 = "mask_engineer_up2";

	// Token: 0x040032B2 RID: 12978
	public static Dictionary<string, string> descriptionLocalizationKeys = new Dictionary<string, string>
	{
		{
			"boots_tabi",
			"Key_1816"
		},
		{
			"boots_blue",
			"Key_1164"
		},
		{
			"SniperBoots_Up1",
			"Key_1165"
		},
		{
			"SniperBoots_Up2",
			"Key_1166"
		},
		{
			"boots_green",
			"Key_1167"
		},
		{
			"DemolitionBoots_Up1",
			"Key_1168"
		},
		{
			"DemolitionBoots_Up2",
			"Key_1169"
		},
		{
			"boots_black",
			"Key_1170"
		},
		{
			"BerserkBoots_Up1",
			"Key_1171"
		},
		{
			"BerserkBoots_Up2",
			"Key_1172"
		},
		{
			"boots_gray",
			"Key_1173"
		},
		{
			"StormTrooperBoots_Up1",
			"Key_1174"
		},
		{
			"StormTrooperBoots_Up2",
			"Key_1175"
		},
		{
			"boots_red",
			"Key_1176"
		},
		{
			"HitmanBoots_Up1",
			"Key_1177"
		},
		{
			"HitmanBoots_Up2",
			"Key_1178"
		},
		{
			"EngineerBoots",
			"Key_1686"
		},
		{
			"EngineerBoots_Up1",
			"Key_1687"
		},
		{
			"EngineerBoots_Up2",
			"Key_1688"
		},
		{
			"cape_Custom",
			"Key_1817"
		},
		{
			"cape_SkeletonLord",
			"Key_1179"
		},
		{
			"SniperCape_Up1",
			"Key_1180"
		},
		{
			"SniperCape_Up2",
			"Key_1181"
		},
		{
			"cape_RoyalKnight",
			"Key_1182"
		},
		{
			"DemolitionCape_Up1",
			"Key_1183"
		},
		{
			"DemolitionCape_Up2",
			"Key_1184"
		},
		{
			"cape_BloodyDemon",
			"Key_1185"
		},
		{
			"BerserkCape_Up1",
			"Key_1186"
		},
		{
			"BerserkCape_Up2",
			"Key_1187"
		},
		{
			"cape_EliteCrafter",
			"Key_1188"
		},
		{
			"StormTrooperCape_Up1",
			"Key_1189"
		},
		{
			"StormTrooperCape_Up2",
			"Key_1190"
		},
		{
			"cape_Archimage",
			"Key_1191"
		},
		{
			"HitmanCape_Up1",
			"Key_1192"
		},
		{
			"HitmanCape_Up2",
			"Key_1193"
		},
		{
			"cape_Engineer",
			"Key_1683"
		},
		{
			"cape_Engineer_Up1",
			"Key_1684"
		},
		{
			"cape_Engineer_Up2",
			"Key_1685"
		},
		{
			"hat_DiamondHelmet",
			"Key_1822"
		},
		{
			"hat_ManiacMask",
			"Key_1819"
		},
		{
			"hat_KingsCrown",
			"Key_1820"
		},
		{
			"hat_Samurai",
			"Key_1821"
		},
		{
			"hat_SeriousManHat",
			"Key_1823"
		},
		{
			"hat_Headphones",
			"Key_1818"
		},
		{
			"league1_hat_hitman",
			"Key_2462"
		},
		{
			"league2_hat_cowboyhat",
			"Key_2174"
		},
		{
			"league3_hat_afro",
			"Key_2175"
		},
		{
			"league4_hat_mushroom",
			"Key_2176"
		},
		{
			"league5_hat_brain",
			"Key_2177"
		},
		{
			"league6_hat_tiara",
			"Key_2178"
		},
		{
			"mask_sniper",
			"Key_1845"
		},
		{
			"mask_sniper_up1",
			"Key_1896"
		},
		{
			"mask_sniper_up2",
			"Key_1897"
		},
		{
			"mask_demolition",
			"Key_1846"
		},
		{
			"mask_demolition_up1",
			"Key_1898"
		},
		{
			"mask_demolition_up2",
			"Key_1899"
		},
		{
			"mask_berserk",
			"Key_1847"
		},
		{
			"mask_berserk_up1",
			"Key_1900"
		},
		{
			"mask_berserk_up2",
			"Key_1901"
		},
		{
			"mask_trooper",
			"Key_1848"
		},
		{
			"mask_trooper_up1",
			"Key_1902"
		},
		{
			"mask_trooper_up2",
			"Key_1903"
		},
		{
			"mask_hitman_1",
			"Key_1849"
		},
		{
			"mask_hitman_1_up1",
			"Key_1904"
		},
		{
			"mask_hitman_1_up2",
			"Key_1905"
		},
		{
			"mask_engineer",
			"Key_1850"
		},
		{
			"mask_engineer_up1",
			"Key_1906"
		},
		{
			"mask_engineer_up2",
			"Key_1907"
		}
	};

	// Token: 0x040032B3 RID: 12979
	public static readonly Dictionary<ShopNGUIController.CategoryNames, List<List<string>>> wear = new Dictionary<ShopNGUIController.CategoryNames, List<List<string>>>(5, ShopNGUIController.CategoryNameComparer.Instance)
	{
		{
			ShopNGUIController.CategoryNames.CapesCategory,
			new List<List<string>>
			{
				new List<string>
				{
					"cape_Custom"
				},
				new List<string>
				{
					"cape_EliteCrafter",
					"StormTrooperCape_Up1",
					"StormTrooperCape_Up2"
				},
				new List<string>
				{
					"cape_Archimage",
					"HitmanCape_Up1",
					"HitmanCape_Up2"
				},
				new List<string>
				{
					"cape_BloodyDemon",
					"BerserkCape_Up1",
					"BerserkCape_Up2"
				},
				new List<string>
				{
					"cape_Engineer",
					"cape_Engineer_Up1",
					"cape_Engineer_Up2"
				},
				new List<string>
				{
					"cape_SkeletonLord",
					"SniperCape_Up1",
					"SniperCape_Up2"
				},
				new List<string>
				{
					"cape_RoyalKnight",
					"DemolitionCape_Up1",
					"DemolitionCape_Up2"
				}
			}
		},
		{
			ShopNGUIController.CategoryNames.HatsCategory,
			new List<List<string>>
			{
				new List<string>
				{
					"hat_Army_1",
					"hat_Army_2",
					"hat_Army_3",
					"hat_Steel_1",
					"hat_Steel_2",
					"hat_Steel_3",
					"hat_Royal_1",
					"hat_Royal_2",
					"hat_Royal_3",
					"hat_Almaz_1",
					"hat_Almaz_2",
					"hat_Almaz_3",
					"hat_Rubin_1",
					"hat_Rubin_2",
					"hat_Rubin_3",
					"hat_Adamant_Const_1",
					"hat_Adamant_Const_2",
					"hat_Adamant_Const_3"
				},
				new List<string>
				{
					"league1_hat_hitman"
				},
				new List<string>
				{
					"league2_hat_cowboyhat"
				},
				new List<string>
				{
					"league3_hat_afro"
				},
				new List<string>
				{
					"league4_hat_mushroom"
				},
				new List<string>
				{
					"league5_hat_brain"
				},
				new List<string>
				{
					"league6_hat_tiara"
				},
				new List<string>
				{
					"hat_Adamant_3"
				},
				new List<string>
				{
					"hat_Headphones"
				},
				new List<string>
				{
					"hat_KingsCrown"
				},
				new List<string>
				{
					"hat_Samurai"
				},
				new List<string>
				{
					"hat_DiamondHelmet"
				},
				new List<string>
				{
					"hat_SeriousManHat"
				}
			}
		},
		{
			ShopNGUIController.CategoryNames.BootsCategory,
			new List<List<string>>
			{
				new List<string>
				{
					"boots_gray",
					"StormTrooperBoots_Up1",
					"StormTrooperBoots_Up2"
				},
				new List<string>
				{
					"boots_red",
					"HitmanBoots_Up1",
					"HitmanBoots_Up2"
				},
				new List<string>
				{
					"boots_black",
					"BerserkBoots_Up1",
					"BerserkBoots_Up2"
				},
				new List<string>
				{
					"EngineerBoots",
					"EngineerBoots_Up1",
					"EngineerBoots_Up2"
				},
				new List<string>
				{
					"boots_blue",
					"SniperBoots_Up1",
					"SniperBoots_Up2"
				},
				new List<string>
				{
					"boots_green",
					"DemolitionBoots_Up1",
					"DemolitionBoots_Up2"
				},
				new List<string>
				{
					"boots_tabi"
				}
			}
		},
		{
			ShopNGUIController.CategoryNames.ArmorCategory,
			new List<List<string>>
			{
				new List<string>
				{
					"Armor_Army_1",
					"Armor_Army_2",
					"Armor_Army_3",
					"Armor_Steel_1",
					"Armor_Steel_2",
					"Armor_Steel_3",
					"Armor_Royal_1",
					"Armor_Royal_2",
					"Armor_Royal_3",
					"Armor_Almaz_1",
					"Armor_Almaz_2",
					"Armor_Almaz_3",
					"Armor_Rubin_1",
					"Armor_Rubin_2",
					"Armor_Rubin_3",
					"Armor_Adamant_Const_1",
					"Armor_Adamant_Const_2",
					"Armor_Adamant_Const_3"
				},
				new List<string>
				{
					"Armor_Novice"
				},
				new List<string>
				{
					"Armor_Adamant_3"
				}
			}
		},
		{
			ShopNGUIController.CategoryNames.MaskCategory,
			new List<List<string>>
			{
				new List<string>
				{
					"mask_trooper",
					"mask_trooper_up1",
					"mask_trooper_up2"
				},
				new List<string>
				{
					"mask_hitman_1",
					"mask_hitman_1_up1",
					"mask_hitman_1_up2"
				},
				new List<string>
				{
					"mask_berserk",
					"mask_berserk_up1",
					"mask_berserk_up2"
				},
				new List<string>
				{
					"mask_engineer",
					"mask_engineer_up1",
					"mask_engineer_up2"
				},
				new List<string>
				{
					"mask_sniper",
					"mask_sniper_up1",
					"mask_sniper_up2"
				},
				new List<string>
				{
					"mask_demolition",
					"mask_demolition_up1",
					"mask_demolition_up2"
				},
				new List<string>
				{
					"hat_ManiacMask"
				}
			}
		}
	};

	// Token: 0x040032B4 RID: 12980
	public static Dictionary<string, float> armorNum = new Dictionary<string, float>();

	// Token: 0x040032B5 RID: 12981
	public static Dictionary<string, List<float>> armorNumTemp = new Dictionary<string, List<float>>
	{
		{
			"Armor_Adamant_3",
			new List<float>
			{
				5f,
				10f,
				16f,
				20f,
				25f
			}
		},
		{
			"hat_Adamant_3",
			new List<float>
			{
				5f,
				10f,
				16f,
				20f,
				25f
			}
		}
	};

	// Token: 0x040032B6 RID: 12982
	public static Dictionary<string, KeyValuePair<Action<Player_move_c, Dictionary<string, object>>, Action<Player_move_c, Dictionary<string, object>>>> bootsMethods = new Dictionary<string, KeyValuePair<Action<Player_move_c, Dictionary<string, object>>, Action<Player_move_c, Dictionary<string, object>>>>();

	// Token: 0x040032B7 RID: 12983
	public static Dictionary<string, KeyValuePair<Action<Player_move_c, Dictionary<string, object>>, Action<Player_move_c, Dictionary<string, object>>>> capesMethods = new Dictionary<string, KeyValuePair<Action<Player_move_c, Dictionary<string, object>>, Action<Player_move_c, Dictionary<string, object>>>>();

	// Token: 0x040032B8 RID: 12984
	public static Dictionary<string, KeyValuePair<Action<Player_move_c, Dictionary<string, object>>, Action<Player_move_c, Dictionary<string, object>>>> hatsMethods = new Dictionary<string, KeyValuePair<Action<Player_move_c, Dictionary<string, object>>, Action<Player_move_c, Dictionary<string, object>>>>();

	// Token: 0x040032B9 RID: 12985
	public static Dictionary<string, KeyValuePair<Action<Player_move_c, Dictionary<string, object>>, Action<Player_move_c, Dictionary<string, object>>>> armorMethods = new Dictionary<string, KeyValuePair<Action<Player_move_c, Dictionary<string, object>>, Action<Player_move_c, Dictionary<string, object>>>>();

	// Token: 0x040032BA RID: 12986
	public static Dictionary<string, SaltedFloat> curArmor = new Dictionary<string, SaltedFloat>();

	// Token: 0x0200078E RID: 1934
	public enum LeagueItemState
	{
		// Token: 0x040032CE RID: 13006
		Open,
		// Token: 0x040032CF RID: 13007
		Closed,
		// Token: 0x040032D0 RID: 13008
		Purchased
	}
}
