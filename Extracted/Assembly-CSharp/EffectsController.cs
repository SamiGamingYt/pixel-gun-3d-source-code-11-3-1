using System;
using System.Collections.Generic;

// Token: 0x020007EE RID: 2030
public static class EffectsController
{
	// Token: 0x17000C14 RID: 3092
	// (get) Token: 0x060049B7 RID: 18871 RVA: 0x00197AA4 File Offset: 0x00195CA4
	// (set) Token: 0x060049B8 RID: 18872 RVA: 0x00197AAC File Offset: 0x00195CAC
	public static float SlowdownCoeff
	{
		get
		{
			return EffectsController.slowdownCoeff;
		}
		set
		{
			EffectsController.slowdownCoeff = value;
		}
	}

	// Token: 0x17000C15 RID: 3093
	// (get) Token: 0x060049B9 RID: 18873 RVA: 0x00197AB4 File Offset: 0x00195CB4
	public static float JumpModifier
	{
		get
		{
			float num = 1f;
			if (Defs.isHunger)
			{
				return num;
			}
			num += ((!Storager.getString(Defs.HatEquppedSN, false).Equals("hat_Samurai")) ? 0f : 0.05f);
			num += ((!Storager.getString(Defs.CapeEquppedSN, false).Equals("cape_Custom")) ? 0f : 0.05f);
			num += ((!Storager.getString(Defs.BootsEquppedSN, false).Equals("boots_gray")) ? 0f : 0.05f);
			num += ((!Storager.getString(Defs.BootsEquppedSN, false).Equals("StormTrooperBoots_Up1")) ? 0f : 0.1f);
			num += ((!Storager.getString(Defs.BootsEquppedSN, false).Equals("StormTrooperBoots_Up2")) ? 0f : 0.15f);
			num += ((!Storager.getString("MaskEquippedSN", false).Equals("mask_demolition")) ? 0f : 0.05f);
			num += ((!Storager.getString("MaskEquippedSN", false).Equals("mask_demolition_up1")) ? 0f : 0.1f);
			num += ((!Storager.getString("MaskEquippedSN", false).Equals("mask_demolition_up2")) ? 0f : 0.15f);
			num += ((!Storager.getString(Defs.HatEquppedSN, false).Equals("league3_hat_afro")) ? 0f : 0.08f);
			num += ((!Storager.getString(Defs.HatEquppedSN, false).Equals("league6_hat_tiara")) ? 0f : 0.08f);
			return num * EffectsController.SlowdownCoeff;
		}
	}

	// Token: 0x060049BA RID: 18874 RVA: 0x00197C94 File Offset: 0x00195E94
	public static float AmmoModForCategory(int i)
	{
		float num = 1f;
		if (Defs.isHunger)
		{
			return num;
		}
		if (Storager.getString(Defs.HatEquppedSN, false).Equals("league2_hat_cowboyhat"))
		{
			num += 0.13f;
		}
		if (i == 0)
		{
			num += ((!Storager.getString(Defs.BootsEquppedSN, false).Equals("boots_gray")) ? 0f : 0.1f);
			num += ((!Storager.getString(Defs.BootsEquppedSN, false).Equals("StormTrooperBoots_Up1")) ? 0f : 0.15f);
			num += ((!Storager.getString(Defs.BootsEquppedSN, false).Equals("StormTrooperBoots_Up2")) ? 0f : 0.25f);
		}
		else if (i == 3)
		{
			num += ((!Storager.getString("MaskEquippedSN", false).Equals("mask_engineer")) ? 0f : 0.1f);
			num += ((!Storager.getString("MaskEquippedSN", false).Equals("mask_engineer_up1")) ? 0f : 0.15f);
			num += ((!Storager.getString("MaskEquippedSN", false).Equals("mask_engineer_up2")) ? 0f : 0.25f);
		}
		return num;
	}

	// Token: 0x060049BB RID: 18875 RVA: 0x00197DF0 File Offset: 0x00195FF0
	public static float DamageModifsByCats(int i)
	{
		List<float> list = new List<float>(6);
		for (int j = 0; j < 6; j++)
		{
			list.Add(0f);
		}
		if (Defs.isHunger)
		{
			return (i < 0 || i >= list.Count) ? 0f : list[i];
		}
		int index2;
		float num;
		if (Storager.getString(Defs.HatEquppedSN, false).Equals("league6_hat_tiara"))
		{
			for (int k = 0; k < list.Count; k++)
			{
				List<float> list3;
				List<float> list2 = list3 = list;
				int index = index2 = k;
				num = list3[index2];
				list2[index] = num + 0.13f;
			}
		}
		List<float> list5;
		List<float> list4 = list5 = list;
		int index3 = index2 = 0;
		num = list5[index2];
		list4[index3] = num + ((!Storager.getString(Defs.HatEquppedSN, false).Equals("hat_Headphones")) ? 0f : 0.1f);
		List<float> list7;
		List<float> list6 = list7 = list;
		int index4 = index2 = 2;
		num = list7[index2];
		list6[index4] = num + ((!Storager.getString("MaskEquippedSN", false).Equals("hat_ManiacMask")) ? 0f : 0.1f);
		List<float> list9;
		List<float> list8 = list9 = list;
		int index5 = index2 = 2;
		num = list9[index2];
		list8[index5] = num + ((!Storager.getString(Defs.HatEquppedSN, false).Equals("hat_Samurai")) ? 0f : 0.1f);
		List<float> list11;
		List<float> list10 = list11 = list;
		int index6 = index2 = 1;
		num = list11[index2];
		list10[index6] = num + ((!Storager.getString(Defs.HatEquppedSN, false).Equals("hat_SeriousManHat")) ? 0f : 0.1f);
		List<float> list13;
		List<float> list12 = list13 = list;
		int index7 = index2 = 0;
		num = list13[index2];
		list12[index7] = num + ((!Storager.getString(Defs.CapeEquppedSN, false).Equals("cape_EliteCrafter")) ? 0f : 0.1f);
		List<float> list15;
		List<float> list14 = list15 = list;
		int index8 = index2 = 0;
		num = list15[index2];
		list14[index8] = num + ((!Storager.getString(Defs.CapeEquppedSN, false).Equals("StormTrooperCape_Up1")) ? 0f : 0.15f);
		List<float> list17;
		List<float> list16 = list17 = list;
		int index9 = index2 = 0;
		num = list17[index2];
		list16[index9] = num + ((!Storager.getString(Defs.CapeEquppedSN, false).Equals("StormTrooperCape_Up2")) ? 0f : 0.25f);
		List<float> list19;
		List<float> list18 = list19 = list;
		int index10 = index2 = 1;
		num = list19[index2];
		list18[index10] = num + ((!Storager.getString(Defs.CapeEquppedSN, false).Equals("cape_Archimage")) ? 0f : 0.1f);
		List<float> list21;
		List<float> list20 = list21 = list;
		int index11 = index2 = 1;
		num = list21[index2];
		list20[index11] = num + ((!Storager.getString(Defs.CapeEquppedSN, false).Equals("HitmanCape_Up1")) ? 0f : 0.15f);
		List<float> list23;
		List<float> list22 = list23 = list;
		int index12 = index2 = 1;
		num = list23[index2];
		list22[index12] = num + ((!Storager.getString(Defs.CapeEquppedSN, false).Equals("HitmanCape_Up2")) ? 0f : 0.25f);
		List<float> list25;
		List<float> list24 = list25 = list;
		int index13 = index2 = 2;
		num = list25[index2];
		list24[index13] = num + ((!Storager.getString(Defs.CapeEquppedSN, false).Equals("cape_BloodyDemon")) ? 0f : 0.1f);
		List<float> list27;
		List<float> list26 = list27 = list;
		int index14 = index2 = 2;
		num = list27[index2];
		list26[index14] = num + ((!Storager.getString(Defs.CapeEquppedSN, false).Equals("BerserkCape_Up1")) ? 0f : 0.15f);
		List<float> list29;
		List<float> list28 = list29 = list;
		int index15 = index2 = 2;
		num = list29[index2];
		list28[index15] = num + ((!Storager.getString(Defs.CapeEquppedSN, false).Equals("BerserkCape_Up2")) ? 0f : 0.25f);
		List<float> list31;
		List<float> list30 = list31 = list;
		int index16 = index2 = 5;
		num = list31[index2];
		list30[index16] = num + ((!Storager.getString(Defs.HatEquppedSN, false).Equals("league1_hat_hitman")) ? 0f : 0.15f);
		return (i < 0 || i >= list.Count) ? 0f : list[i];
	}

	// Token: 0x17000C16 RID: 3094
	// (get) Token: 0x060049BC RID: 18876 RVA: 0x0019827C File Offset: 0x0019647C
	public static bool NinjaJumpEnabled
	{
		get
		{
			return !Defs.isHunger && (!(WeaponManager.sharedManager.myPlayerMoveC != null) || !WeaponManager.sharedManager.myPlayerMoveC.isMechActive) && (Storager.getString(Defs.BootsEquppedSN, false).Equals("boots_tabi") || Storager.getString(Defs.BootsEquppedSN, false).Equals("boots_black") || Storager.getString(Defs.BootsEquppedSN, false).Equals("BerserkBoots_Up1") || Storager.getString(Defs.BootsEquppedSN, false).Equals("BerserkBoots_Up2"));
		}
	}

	// Token: 0x17000C17 RID: 3095
	// (get) Token: 0x060049BD RID: 18877 RVA: 0x00198324 File Offset: 0x00196524
	public static float ExplosionImpulseRadiusIncreaseCoef
	{
		get
		{
			if (Defs.isHunger)
			{
				return 0f;
			}
			return ((!Storager.getString(Defs.BootsEquppedSN, false).Equals("boots_green")) ? 0f : 0.05f) + ((!Storager.getString(Defs.BootsEquppedSN, false).Equals("DemolitionBoots_Up1")) ? 0f : 0.1f) + ((!Storager.getString(Defs.BootsEquppedSN, false).Equals("DemolitionBoots_Up2")) ? 0f : 0.15f) + ((!Storager.getString(Defs.HatEquppedSN, false).Equals("league3_hat_afro")) ? 0f : 0.08f);
		}
	}

	// Token: 0x17000C18 RID: 3096
	// (get) Token: 0x060049BE RID: 18878 RVA: 0x001983E8 File Offset: 0x001965E8
	public static float GrenadeExplosionDamageIncreaseCoef
	{
		get
		{
			if (Defs.isHunger)
			{
				return 0f;
			}
			return ((!Storager.getString(Defs.BootsEquppedSN, false).Equals("boots_green")) ? 0f : 0.1f) + ((!Storager.getString(Defs.BootsEquppedSN, false).Equals("DemolitionBoots_Up1")) ? 0f : 0.15f) + ((!Storager.getString(Defs.BootsEquppedSN, false).Equals("DemolitionBoots_Up2")) ? 0f : 0.25f);
		}
	}

	// Token: 0x17000C19 RID: 3097
	// (get) Token: 0x060049BF RID: 18879 RVA: 0x00198484 File Offset: 0x00196684
	public static float GrenadeExplosionRadiusIncreaseCoef
	{
		get
		{
			float num = 1f;
			if (Defs.isHunger)
			{
				return num;
			}
			num += ((!Storager.getString(Defs.CapeEquppedSN, false).Equals("cape_RoyalKnight")) ? 0f : 0.2f);
			num += ((!Storager.getString(Defs.CapeEquppedSN, false).Equals("DemolitionCape_Up1")) ? 0f : 0.3f);
			return num + ((!Storager.getString(Defs.CapeEquppedSN, false).Equals("DemolitionCape_Up2")) ? 0f : 0.5f);
		}
	}

	// Token: 0x17000C1A RID: 3098
	// (get) Token: 0x060049C0 RID: 18880 RVA: 0x00198528 File Offset: 0x00196728
	public static float SelfExplosionDamageDecreaseCoef
	{
		get
		{
			if (Defs.isHunger)
			{
				return 1f;
			}
			return 1f * ((!Storager.getString(Defs.HatEquppedSN, false).Equals("hat_KingsCrown")) ? 1f : 0.8f) * ((!Storager.getString(Defs.HatEquppedSN, false).Equals("hat_DiamondHelmet")) ? 1f : 0.8f) * ((!Storager.getString(Defs.CapeEquppedSN, false).Equals("cape_RoyalKnight")) ? 1f : 0.8f) * ((!Storager.getString(Defs.CapeEquppedSN, false).Equals("DemolitionCape_Up1")) ? 1f : 0.7f) * ((!Storager.getString(Defs.CapeEquppedSN, false).Equals("DemolitionCape_Up2")) ? 1f : 0.5f) * ((!Storager.getString(Defs.HatEquppedSN, false).Equals("league4_hat_mushroom")) ? 1f : 0.5f);
		}
	}

	// Token: 0x060049C1 RID: 18881 RVA: 0x00198648 File Offset: 0x00196848
	public static float SpeedModifier(int i)
	{
		if (Defs.isHunger || (WeaponManager.sharedManager.myPlayerMoveC != null && WeaponManager.sharedManager.myPlayerMoveC.isMechActive))
		{
			return 1f;
		}
		float num = WeaponManager.sharedManager.currentWeaponSounds.speedModifier * ((!PotionsController.sharedController.PotionIsActive(PotionsController.HastePotion)) ? 1f : 1.25f) * ((!Storager.getString(Defs.HatEquppedSN, false).Equals("hat_KingsCrown")) ? 1f : 1.05f) * ((!Storager.getString(Defs.HatEquppedSN, false).Equals("hat_Samurai")) ? 1f : 1.05f) * ((!Storager.getString(Defs.CapeEquppedSN, false).Equals("cape_Custom")) ? 1f : 1.05f) * ((!Storager.getString(Defs.HatEquppedSN, false).Equals("league6_hat_tiara")) ? 1f : 1.08f) * ((!Storager.getString(Defs.HatEquppedSN, false).Equals("league3_hat_afro")) ? 1f : 1.08f) * EffectsController.SlowdownCoeff;
		if (i == 1 && Storager.getString(Defs.BootsEquppedSN, false).Equals("boots_red"))
		{
			num *= 1.05f;
		}
		if (i == 1 && Storager.getString(Defs.BootsEquppedSN, false).Equals("HitmanBoots_Up1"))
		{
			num *= 1.1f;
		}
		if (i == 1 && Storager.getString(Defs.BootsEquppedSN, false).Equals("HitmanBoots_Up2"))
		{
			num *= 1.15f;
		}
		if (i == 2 && Storager.getString(Defs.BootsEquppedSN, false).Equals("boots_black"))
		{
			num *= 1.05f;
		}
		if (i == 2 && Storager.getString(Defs.BootsEquppedSN, false).Equals("BerserkBoots_Up1"))
		{
			num *= 1.1f;
		}
		if (i == 2 && Storager.getString(Defs.BootsEquppedSN, false).Equals("BerserkBoots_Up2"))
		{
			num *= 1.15f;
		}
		if (i == 3 && Storager.getString(Defs.BootsEquppedSN, false).Equals("EngineerBoots"))
		{
			num *= 1.05f;
		}
		if (i == 3 && Storager.getString(Defs.BootsEquppedSN, false).Equals("EngineerBoots_Up1"))
		{
			num *= 1.1f;
		}
		if (i == 3 && Storager.getString(Defs.BootsEquppedSN, false).Equals("EngineerBoots_Up2"))
		{
			num *= 1.15f;
		}
		if (i == 4 && Storager.getString(Defs.BootsEquppedSN, false).Equals("boots_blue"))
		{
			num *= 1.05f;
		}
		if (i == 4 && Storager.getString(Defs.BootsEquppedSN, false).Equals("SniperBoots_Up1"))
		{
			num *= 1.1f;
		}
		if (i == 4 && Storager.getString(Defs.BootsEquppedSN, false).Equals("SniperBoots_Up2"))
		{
			num *= 1.15f;
		}
		if (i == 5 && Storager.getString(Defs.BootsEquppedSN, false).Equals("boots_green"))
		{
			num *= 1.05f;
		}
		if (i == 5 && Storager.getString(Defs.BootsEquppedSN, false).Equals("DemolitionBoots_Up1"))
		{
			num *= 1.1f;
		}
		if (i == 5 && Storager.getString(Defs.BootsEquppedSN, false).Equals("DemolitionBoots_Up2"))
		{
			num *= 1.15f;
		}
		if (i == 0 && Storager.getString("MaskEquippedSN", false).Equals("mask_trooper"))
		{
			num *= 1.05f;
		}
		if (i == 0 && Storager.getString("MaskEquippedSN", false).Equals("mask_trooper_up1"))
		{
			num *= 1.1f;
		}
		if (i == 0 && Storager.getString("MaskEquippedSN", false).Equals("mask_trooper_up2"))
		{
			num *= 1.15f;
		}
		return num;
	}

	// Token: 0x17000C1B RID: 3099
	// (get) Token: 0x060049C2 RID: 18882 RVA: 0x00198A7C File Offset: 0x00196C7C
	public static bool WeAreStealth
	{
		get
		{
			return !Defs.isHunger && (Storager.getString(Defs.BootsEquppedSN, false).Equals("boots_blue") || Storager.getString(Defs.BootsEquppedSN, false).Equals("SniperBoots_Up1") || Storager.getString(Defs.BootsEquppedSN, false).Equals("SniperBoots_Up2") || Storager.getString(Defs.HatEquppedSN, false).Equals("league4_hat_mushroom"));
		}
	}

	// Token: 0x17000C1C RID: 3100
	// (get) Token: 0x060049C3 RID: 18883 RVA: 0x00198AFC File Offset: 0x00196CFC
	public static float ArmorBonus
	{
		get
		{
			float num = 0f;
			if (Defs.isHunger)
			{
				return num;
			}
			if (Storager.getString(Defs.HatEquppedSN, false).Equals("league6_hat_tiara"))
			{
				num += 3f;
			}
			if (Storager.getString(Defs.BootsEquppedSN, false).Equals("boots_red"))
			{
				num += 1f;
			}
			if (Storager.getString(Defs.BootsEquppedSN, false).Equals("HitmanBoots_Up1"))
			{
				num += 2f;
			}
			if (Storager.getString(Defs.BootsEquppedSN, false).Equals("HitmanBoots_Up2"))
			{
				num += 3f;
			}
			if (Storager.getString("MaskEquippedSN", false).Equals("mask_berserk"))
			{
				num += 1f;
			}
			else if (Storager.getString("MaskEquippedSN", false).Equals("mask_berserk_up1"))
			{
				num += 2f;
			}
			else if (Storager.getString("MaskEquippedSN", false).Equals("mask_berserk_up2"))
			{
				num += 3f;
			}
			return num;
		}
	}

	// Token: 0x17000C1D RID: 3101
	// (get) Token: 0x060049C4 RID: 18884 RVA: 0x00198C14 File Offset: 0x00196E14
	public static float IcnreaseEquippedArmorPercentage
	{
		get
		{
			float num = 1f;
			if (Defs.isHunger)
			{
				return num;
			}
			num += ((!Storager.getString(Defs.CapeEquppedSN, false).Equals("cape_BloodyDemon")) ? 0f : 0.1f);
			num += ((!Storager.getString(Defs.CapeEquppedSN, false).Equals("BerserkCape_Up1")) ? 0f : 0.15f);
			return num + ((!Storager.getString(Defs.CapeEquppedSN, false).Equals("BerserkCape_Up2")) ? 0f : 0.25f);
		}
	}

	// Token: 0x17000C1E RID: 3102
	// (get) Token: 0x060049C5 RID: 18885 RVA: 0x00198CB8 File Offset: 0x00196EB8
	public static float RegeneratingArmorTime
	{
		get
		{
			float result = 0f;
			if (Defs.isHunger)
			{
				return result;
			}
			if (Defs.isHunger)
			{
				return 0f;
			}
			if (Storager.getString(Defs.CapeEquppedSN, false).Equals("cape_Archimage"))
			{
				result = 12f;
			}
			if (Storager.getString(Defs.CapeEquppedSN, false).Equals("HitmanCape_Up1"))
			{
				result = 10f;
			}
			if (Storager.getString(Defs.HatEquppedSN, false).Equals("league5_hat_brain"))
			{
				result = 9f;
			}
			if (Storager.getString(Defs.CapeEquppedSN, false).Equals("HitmanCape_Up2"))
			{
				result = 8f;
			}
			return result;
		}
	}

	// Token: 0x060049C6 RID: 18886 RVA: 0x00198D68 File Offset: 0x00196F68
	public static float AddingForPotionDuration(string potion)
	{
		float num = 0f;
		if (Defs.isHunger)
		{
			return num;
		}
		if (potion == null)
		{
			return num;
		}
		if (potion.Equals("InvisibilityPotion") && Storager.getString(Defs.BootsEquppedSN, false).Equals("boots_blue"))
		{
			num += 5f;
		}
		if (potion.Equals("InvisibilityPotion") && Storager.getString(Defs.BootsEquppedSN, false).Equals("SniperBoots_Up1"))
		{
			num += 10f;
		}
		if (potion.Equals("InvisibilityPotion") && Storager.getString(Defs.BootsEquppedSN, false).Equals("SniperBoots_Up2"))
		{
			num += 15f;
		}
		if (!Defs.isDaterRegim)
		{
			num += ((!Storager.getString(Defs.CapeEquppedSN, false).Equals("cape_Engineer")) ? 0f : 10f);
			num += ((!Storager.getString(Defs.CapeEquppedSN, false).Equals("cape_Engineer_Up1")) ? 0f : 15f);
			num += ((!Storager.getString(Defs.CapeEquppedSN, false).Equals("cape_Engineer_Up2")) ? 0f : 20f);
			num += ((!Storager.getString(Defs.BootsEquppedSN, false).Equals("EngineerBoots")) ? 0f : 10f);
			num += ((!Storager.getString(Defs.BootsEquppedSN, false).Equals("EngineerBoots_Up1")) ? 0f : 15f);
			num += ((!Storager.getString(Defs.BootsEquppedSN, false).Equals("EngineerBoots_Up2")) ? 0f : 20f);
			num += ((!Storager.getString(Defs.HatEquppedSN, false).Equals("league5_hat_brain")) ? 0f : 13f);
		}
		return num;
	}

	// Token: 0x060049C7 RID: 18887 RVA: 0x00198F64 File Offset: 0x00197164
	public static float GetReloadAnimationSpeed(int categoryNabor, string currentCape, string currentMask, string currentHat)
	{
		if (currentCape == null)
		{
			currentCape = string.Empty;
		}
		if (currentMask == null)
		{
			currentMask = string.Empty;
		}
		if (currentHat == null)
		{
			currentHat = string.Empty;
		}
		float num = 1f;
		if (Defs.isHunger)
		{
			return num;
		}
		if (currentHat.Equals("league2_hat_cowboyhat"))
		{
			num += 0.13f;
		}
		switch (categoryNabor)
		{
		case 1:
			num += ((!currentCape.Equals("cape_EliteCrafter")) ? 0f : 0.1f);
			num += ((!currentCape.Equals("StormTrooperCape_Up1")) ? 0f : 0.15f);
			num += ((!currentCape.Equals("StormTrooperCape_Up2")) ? 0f : 0.25f);
			break;
		case 2:
			num += ((!currentMask.Equals("mask_hitman_1")) ? 0f : 0.1f);
			num += ((!currentMask.Equals("mask_hitman_1_up1")) ? 0f : 0.15f);
			num += ((!currentMask.Equals("mask_hitman_1_up2")) ? 0f : 0.25f);
			break;
		case 4:
			num += ((!currentCape.Equals("cape_Engineer")) ? 0f : 0.1f);
			num += ((!currentCape.Equals("cape_Engineer_Up1")) ? 0f : 0.15f);
			num += ((!currentCape.Equals("cape_Engineer_Up2")) ? 0f : 0.25f);
			break;
		case 5:
			num += ((!currentCape.Equals("cape_SkeletonLord")) ? 0f : 0.1f);
			num += ((!currentCape.Equals("SniperCape_Up1")) ? 0f : 0.15f);
			num += ((!currentCape.Equals("SniperCape_Up2")) ? 0f : 0.25f);
			break;
		case 6:
			num += ((!currentCape.Equals("cape_RoyalKnight")) ? 0f : 0.1f);
			num += ((!currentCape.Equals("DemolitionCape_Up1")) ? 0f : 0.15f);
			num += ((!currentCape.Equals("DemolitionCape_Up2")) ? 0f : 0.25f);
			break;
		}
		return num;
	}

	// Token: 0x17000C1F RID: 3103
	// (get) Token: 0x060049C8 RID: 18888 RVA: 0x00199204 File Offset: 0x00197404
	public static bool IsRegeneratingArmor
	{
		get
		{
			return EffectsController.RegeneratingArmorTime > 0f;
		}
	}

	// Token: 0x060049C9 RID: 18889 RVA: 0x00199214 File Offset: 0x00197414
	public static float AddingForHeadshot(int cat)
	{
		if (Defs.isHunger)
		{
			return 0f;
		}
		List<float> list = new List<float>(6);
		for (int i = 0; i < 6; i++)
		{
			list.Add(0f);
		}
		int index2;
		float num;
		if (Storager.getString(Defs.HatEquppedSN, false).Equals("league5_hat_brain"))
		{
			for (int j = 0; j < 6; j++)
			{
				List<float> list3;
				List<float> list2 = list3 = list;
				int index = index2 = j;
				num = list3[index2];
				list2[index] = num + 0.13f;
			}
		}
		List<float> list5;
		List<float> list4 = list5 = list;
		int index3 = index2 = 4;
		num = list5[index2];
		list4[index3] = num + ((!Storager.getString(Defs.CapeEquppedSN, false).Equals("cape_SkeletonLord")) ? 0f : 0.1f);
		List<float> list7;
		List<float> list6 = list7 = list;
		int index4 = index2 = 4;
		num = list7[index2];
		list6[index4] = num + ((!Storager.getString(Defs.CapeEquppedSN, false).Equals("SniperCape_Up1")) ? 0f : 0.15f);
		List<float> list9;
		List<float> list8 = list9 = list;
		int index5 = index2 = 4;
		num = list9[index2];
		list8[index5] = num + ((!Storager.getString(Defs.CapeEquppedSN, false).Equals("SniperCape_Up2")) ? 0f : 0.25f);
		return (cat < 0 || cat >= list.Count) ? 0f : list[cat];
	}

	// Token: 0x060049CA RID: 18890 RVA: 0x00199390 File Offset: 0x00197590
	public static float GetChanceToIgnoreHeadshot(int categoryNabor, string currentCape, string currentMask, string currentHat)
	{
		if (currentCape == null)
		{
			currentCape = string.Empty;
		}
		if (currentMask == null)
		{
			currentMask = string.Empty;
		}
		if (currentHat == null)
		{
			currentHat = string.Empty;
		}
		float num = 0f;
		if (Defs.isHunger)
		{
			return num;
		}
		if (currentHat.Equals("league4_hat_mushroom"))
		{
			num += 0.13f;
		}
		switch (categoryNabor - 1)
		{
		case 2:
			if (currentCape.Equals("cape_BloodyDemon"))
			{
				num += 0.1f;
			}
			else if (currentCape.Equals("BerserkCape_Up1"))
			{
				num += 0.15f;
			}
			else if (currentCape.Equals("BerserkCape_Up2"))
			{
				num += 0.25f;
			}
			break;
		case 4:
			if (currentMask.Equals("mask_sniper"))
			{
				num += 0.1f;
			}
			else if (currentMask.Equals("mask_sniper_up1"))
			{
				num += 0.15f;
			}
			else if (currentMask.Equals("mask_sniper_up2"))
			{
				num += 0.25f;
			}
			break;
		}
		return num;
	}

	// Token: 0x040036A9 RID: 13993
	private static float slowdownCoeff = 1f;
}
