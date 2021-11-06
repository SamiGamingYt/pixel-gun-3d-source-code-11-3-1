using System;
using Rilisoft;
using UnityEngine;

// Token: 0x02000762 RID: 1890
public class StarterPackModel
{
	// Token: 0x06004274 RID: 17012 RVA: 0x00160FE0 File Offset: 0x0015F1E0
	public static DateTime GetTimeDataEvent(string timeEventKey)
	{
		DateTime result = default(DateTime);
		string @string = Storager.getString(timeEventKey, false);
		DateTime.TryParse(@string, out result);
		return result;
	}

	// Token: 0x06004275 RID: 17013 RVA: 0x00161008 File Offset: 0x0015F208
	public static string GetUrlForDownloadEventData()
	{
		string arg = "https://secure.pixelgunserver.com/pixelgun3d-config/starterPack/";
		string arg2 = string.Empty;
		if (Defs.IsDeveloperBuild)
		{
			arg2 = "starter_pack_test.json";
		}
		else if (BuildSettings.BuildTargetPlatform == RuntimePlatform.Android)
		{
			if (Defs.AndroidEdition == Defs.RuntimeAndroidEdition.Amazon)
			{
				arg2 = "starter_pack_amazon.json";
			}
			else
			{
				arg2 = "starter_pack_android.json";
			}
		}
		else if (BuildSettings.BuildTargetPlatform == RuntimePlatform.MetroPlayerX64)
		{
			arg2 = "starter_pack_wp8.json";
		}
		else
		{
			arg2 = "starter_pack_ios.json";
		}
		return string.Format("{0}{1}", arg, arg2);
	}

	// Token: 0x06004276 RID: 17014 RVA: 0x0016108C File Offset: 0x0015F28C
	public static DateTime GetCurrentTimeByUnixTime(int unixTime)
	{
		DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
		return dateTime.AddSeconds((double)unixTime);
	}

	// Token: 0x04003083 RID: 12419
	public const int MaxCountShownWindow = 1;

	// Token: 0x04003084 RID: 12420
	private const float HoursToShowWindow = 8f;

	// Token: 0x04003085 RID: 12421
	public const string pathToCoinsImage = "Textures/Bank/Coins_Shop_5";

	// Token: 0x04003086 RID: 12422
	public const string pathToGemsImage = "Textures/Bank/Coins_Shop_Gem_5";

	// Token: 0x04003087 RID: 12423
	public const string pathToGemsPackImage = "Textures/Bank/StarterPack_Gem";

	// Token: 0x04003088 RID: 12424
	public const string pathToCoinsPackImage = "Textures/Bank/StarterPack_Gold";

	// Token: 0x04003089 RID: 12425
	public const string pathToItemsPackImage = "Textures/Bank/StarterPack_Weapon";

	// Token: 0x0400308A RID: 12426
	public static TimeSpan TimeOutShownWindow = TimeSpan.FromHours(8.0);

	// Token: 0x0400308B RID: 12427
	public static TimeSpan MaxLiveTimeEvent = TimeSpan.FromDays(1.0);

	// Token: 0x0400308C RID: 12428
	public static TimeSpan CooldownTimeEvent = TimeSpan.FromDays(1.5);

	// Token: 0x0400308D RID: 12429
	public static string[] packNameLocalizeKey = new string[]
	{
		"Key_1049",
		"Key_1050",
		"Key_1051",
		"Key_1052",
		"Key_1053",
		"Key_1054",
		"Key_1055",
		"Key_1056"
	};

	// Token: 0x0400308E RID: 12430
	public static int[] savingMoneyForBuyPack = new int[]
	{
		7,
		5,
		17,
		14,
		27,
		21,
		22,
		42
	};

	// Token: 0x02000763 RID: 1891
	public enum TypePack
	{
		// Token: 0x04003090 RID: 12432
		Items,
		// Token: 0x04003091 RID: 12433
		Coins,
		// Token: 0x04003092 RID: 12434
		Gems,
		// Token: 0x04003093 RID: 12435
		None
	}

	// Token: 0x02000764 RID: 1892
	public enum TypeCost
	{
		// Token: 0x04003095 RID: 12437
		Money,
		// Token: 0x04003096 RID: 12438
		Gems,
		// Token: 0x04003097 RID: 12439
		InApp,
		// Token: 0x04003098 RID: 12440
		None
	}
}
