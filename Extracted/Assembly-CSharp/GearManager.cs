using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x020007F4 RID: 2036
public static class GearManager
{
	// Token: 0x06004A0C RID: 18956 RVA: 0x0019B2FC File Offset: 0x001994FC
	public static string AnalyticsIDForOneItemOfGear(string itemName, bool changeGrenade = false)
	{
		if (itemName == null)
		{
			return string.Empty;
		}
		string text = GearManager.HolderQuantityForID(itemName);
		if (text == null || !GearManager.AllGear.Contains(text))
		{
			return string.Empty;
		}
		int num = GearManager.CurrentNumberOfUphradesForGear(text);
		string text2 = text;
		if (changeGrenade && text.Equals(GearManager.Grenade) && num == 0)
		{
			text2 = "Grenade";
		}
		if (num > 0)
		{
			text2 = text2 + "_" + num;
		}
		return text2;
	}

	// Token: 0x17000C25 RID: 3109
	// (get) Token: 0x06004A0D RID: 18957 RVA: 0x0019B380 File Offset: 0x00199580
	public static List<string> AllGear
	{
		get
		{
			return GearManager.Gear.Concat(GearManager.DaterGear).ToList<string>();
		}
	}

	// Token: 0x17000C26 RID: 3110
	// (get) Token: 0x06004A0E RID: 18958 RVA: 0x0019B398 File Offset: 0x00199598
	public static int NumOfGearUpgrades
	{
		get
		{
			return ExpController.LevelsForTiers.Length - 1;
		}
	}

	// Token: 0x06004A0F RID: 18959 RVA: 0x0019B3A4 File Offset: 0x001995A4
	public static int CurrentNumberOfUphradesForGear(string id)
	{
		if (Defs.isDaterRegim)
		{
			return 0;
		}
		int a = 0;
		if (ExpController.Instance != null)
		{
			a = ExpController.Instance.OurTier;
		}
		return Mathf.Min(a, GearManager.NumOfGearUpgrades);
	}

	// Token: 0x06004A10 RID: 18960 RVA: 0x0019B3E8 File Offset: 0x001995E8
	public static string OneItemIDForGear(string id, int i)
	{
		if (id == null)
		{
			return null;
		}
		return id + GearManager.OneItemSuffix + i;
	}

	// Token: 0x06004A11 RID: 18961 RVA: 0x0019B404 File Offset: 0x00199604
	public static string UpgradeIDForGear(string id, int i)
	{
		if (id == null)
		{
			return null;
		}
		return id + GearManager.UpgradeSuffix + i;
	}

	// Token: 0x06004A12 RID: 18962 RVA: 0x0019B420 File Offset: 0x00199620
	public static string HolderQuantityForID(string id)
	{
		if (id == null)
		{
			return string.Empty;
		}
		foreach (string text in GearManager.AllGear)
		{
			if (GearManager.ItemIsGear(id, text))
			{
				return text;
			}
		}
		return id;
	}

	// Token: 0x17000C27 RID: 3111
	// (get) Token: 0x06004A13 RID: 18963 RVA: 0x0019B4A0 File Offset: 0x001996A0
	public static string OneItemSuffix
	{
		get
		{
			return "_OneItem_";
		}
	}

	// Token: 0x17000C28 RID: 3112
	// (get) Token: 0x06004A14 RID: 18964 RVA: 0x0019B4A8 File Offset: 0x001996A8
	public static string UpgradeSuffix
	{
		get
		{
			return "_Up_";
		}
	}

	// Token: 0x06004A15 RID: 18965 RVA: 0x0019B4B0 File Offset: 0x001996B0
	public static int ItemsInPackForGear(string id)
	{
		return (id == null || !id.Equals(GearManager.Grenade)) ? 3 : 5;
	}

	// Token: 0x06004A16 RID: 18966 RVA: 0x0019B4D0 File Offset: 0x001996D0
	public static int MaxCountForGear(string id)
	{
		return (id == null || !id.Equals(GearManager.Grenade)) ? 1000000 : 10;
	}

	// Token: 0x06004A17 RID: 18967 RVA: 0x0019B500 File Offset: 0x00199700
	public static string NameForUpgrade(string item, int num)
	{
		return item + GearManager.UpgradeSuffix + num;
	}

	// Token: 0x06004A18 RID: 18968 RVA: 0x0019B514 File Offset: 0x00199714
	private static bool ItemIsGear(string item, string gear)
	{
		if (gear == null || item == null)
		{
			return false;
		}
		string text = gear + GearManager.UpgradeSuffix;
		string text2 = gear + GearManager.OneItemSuffix;
		int num;
		return item == gear || (item.StartsWith(text) && item.Length > text.Length && int.TryParse(string.Empty + item[text.Length], out num)) || (item.StartsWith(text2) && item.Length > text2.Length && int.TryParse(string.Empty + item[text2.Length], out num));
	}

	// Token: 0x06004A19 RID: 18969 RVA: 0x0019B5DC File Offset: 0x001997DC
	public static bool IsItemGear(string tag)
	{
		if (tag == null)
		{
			return false;
		}
		for (int i = 0; i < GearManager.AllGear.Count; i++)
		{
			if (GearManager.ItemIsGear(tag, GearManager.AllGear[i]))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x040036DD RID: 14045
	public const int MaxGrenadeCount = 10;

	// Token: 0x040036DE RID: 14046
	public const int MaxGearCount = 1000000;

	// Token: 0x040036DF RID: 14047
	public const int NumberOfGearInPack = 3;

	// Token: 0x040036E0 RID: 14048
	public static readonly string InvisibilityPotion = "InvisibilityPotion";

	// Token: 0x040036E1 RID: 14049
	public static readonly string Grenade = "GrenadeID";

	// Token: 0x040036E2 RID: 14050
	public static readonly string Like = "LikeID";

	// Token: 0x040036E3 RID: 14051
	public static readonly string Jetpack = "Jetpack";

	// Token: 0x040036E4 RID: 14052
	public static readonly string Turret = "Turret";

	// Token: 0x040036E5 RID: 14053
	public static readonly string Mech = "Mech";

	// Token: 0x040036E6 RID: 14054
	public static readonly string BigHeadPotion = "BigHeadPotion";

	// Token: 0x040036E7 RID: 14055
	public static readonly string Wings = "Wings";

	// Token: 0x040036E8 RID: 14056
	public static readonly string Bear = "Bear";

	// Token: 0x040036E9 RID: 14057
	public static readonly string MusicBox = "MusicBox";

	// Token: 0x040036EA RID: 14058
	public static readonly string[] Gear = new string[]
	{
		GearManager.Grenade,
		GearManager.InvisibilityPotion,
		GearManager.Jetpack,
		GearManager.Turret,
		GearManager.Mech
	};

	// Token: 0x040036EB RID: 14059
	public static readonly string[] DaterGear = new string[]
	{
		GearManager.BigHeadPotion,
		GearManager.Wings,
		GearManager.MusicBox,
		GearManager.Bear
	};
}
