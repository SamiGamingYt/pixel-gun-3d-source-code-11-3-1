using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using Rilisoft;
using UnityEngine;

// Token: 0x0200081C RID: 2076
public sealed class Defs2
{
	// Token: 0x17000C67 RID: 3175
	// (get) Token: 0x06004B8C RID: 19340 RVA: 0x001B2A34 File Offset: 0x001B0C34
	// (set) Token: 0x06004B8D RID: 19341 RVA: 0x001B2A3C File Offset: 0x001B0C3C
	public static Dictionary<string, List<ItemPrice>> GadgetPricesFromServer { get; set; }

	// Token: 0x17000C68 RID: 3176
	// (get) Token: 0x06004B8E RID: 19342 RVA: 0x001B2A44 File Offset: 0x001B0C44
	// (set) Token: 0x06004B8F RID: 19343 RVA: 0x001B2A4C File Offset: 0x001B0C4C
	public static Dictionary<string, ItemPrice> ArmorPricesFromServer { get; set; }

	// Token: 0x17000C69 RID: 3177
	// (get) Token: 0x06004B90 RID: 19344 RVA: 0x001B2A54 File Offset: 0x001B0C54
	public static bool CanShowPremiumAccountExpiredWindow
	{
		get
		{
			return TrainingController.TrainingCompleted;
		}
	}

	// Token: 0x06004B91 RID: 19345 RVA: 0x001B2A5C File Offset: 0x001B0C5C
	public static void InitializeTier8_3_0Key()
	{
		if (Defs2.TierAfter8_3_0Initialized)
		{
			return;
		}
		if (!Storager.hasKey(Defs.TierAfter8_3_0Key))
		{
			Storager.setInt(Defs.TierAfter8_3_0Key, ExpController.GetOurTier(), false);
		}
		Defs2.TierAfter8_3_0Initialized = true;
	}

	// Token: 0x17000C6A RID: 3178
	// (get) Token: 0x06004B92 RID: 19346 RVA: 0x001B2A9C File Offset: 0x001B0C9C
	public static int MaxGrenadeCount
	{
		get
		{
			return 10;
		}
	}

	// Token: 0x17000C6B RID: 3179
	// (get) Token: 0x06004B93 RID: 19347 RVA: 0x001B2AA0 File Offset: 0x001B0CA0
	public static int GrenadeOnePrice
	{
		get
		{
			return VirtualCurrencyHelper.Price("GrenadeID" + GearManager.OneItemSuffix + GearManager.CurrentNumberOfUphradesForGear("GrenadeID")).Price;
		}
	}

	// Token: 0x06004B94 RID: 19348 RVA: 0x001B2AD8 File Offset: 0x001B0CD8
	public static bool IsAvalibleAddFrends()
	{
		return FriendsController.sharedController.friends.Count + FriendsController.sharedController.invitesToUs.Count < Defs.maxCountFriend;
	}

	// Token: 0x17000C6C RID: 3180
	// (get) Token: 0x06004B95 RID: 19349 RVA: 0x001B2B0C File Offset: 0x001B0D0C
	public static string ApplicationUrl
	{
		get
		{
			return (BuildSettings.BuildTargetPlatform != RuntimePlatform.Android || Defs.AndroidEdition != Defs.RuntimeAndroidEdition.Amazon) ? ((BuildSettings.BuildTargetPlatform != RuntimePlatform.MetroPlayerX64) ? "http://pixelgun3d.com/get.html" : "https://www.microsoft.com/ru-ru/store/games/pixel-gun-3d/9wzdncrdzvbf") : "http://www.amazon.com/RiliSoft-Games-Pixel-Gun-3D/dp/B00I6IKSZ0";
		}
	}

	// Token: 0x17000C6D RID: 3181
	// (get) Token: 0x06004B96 RID: 19350 RVA: 0x001B2B58 File Offset: 0x001B0D58
	internal static SignedPreferences SignedPreferences
	{
		get
		{
			if (Defs2._signedPreferences == null)
			{
				RSACryptoServiceProvider rsacryptoServiceProvider = new RSACryptoServiceProvider(512);
				rsacryptoServiceProvider.ImportCspBlob(Defs2._rsaParameters);
				Defs2._signedPreferences = new RsaSignedPreferences(new PersistentPreferences(), rsacryptoServiceProvider, SystemInfo.deviceUniqueIdentifier);
			}
			return Defs2._signedPreferences;
		}
	}

	// Token: 0x04003A9E RID: 15006
	private static bool _ourGunPricesCogorteInitialized = false;

	// Token: 0x04003A9F RID: 15007
	private static bool TierAfter8_3_0Initialized = false;

	// Token: 0x04003AA0 RID: 15008
	private static SignedPreferences _signedPreferences;

	// Token: 0x04003AA1 RID: 15009
	private static readonly byte[] _rsaParameters = new byte[]
	{
		7,
		2,
		0,
		0,
		0,
		164,
		0,
		0,
		82,
		83,
		65,
		50,
		0,
		2,
		0,
		0,
		17,
		0,
		0,
		0,
		1,
		24,
		67,
		211,
		214,
		189,
		210,
		144,
		254,
		145,
		230,
		212,
		19,
		254,
		185,
		112,
		117,
		120,
		142,
		89,
		80,
		227,
		74,
		157,
		136,
		99,
		204,
		254,
		117,
		105,
		106,
		52,
		143,
		219,
		180,
		55,
		4,
		174,
		130,
		222,
		59,
		143,
		80,
		32,
		56,
		220,
		204,
		215,
		254,
		202,
		38,
		42,
		34,
		141,
		116,
		38,
		68,
		147,
		247,
		71,
		65,
		49,
		18,
		153,
		205,
		10,
		30,
		210,
		118,
		97,
		196,
		36,
		168,
		88,
		201,
		246,
		230,
		160,
		110,
		13,
		124,
		85,
		105,
		5,
		43,
		72,
		1,
		158,
		28,
		194,
		234,
		109,
		169,
		124,
		57,
		167,
		5,
		106,
		4,
		145,
		166,
		174,
		181,
		8,
		222,
		238,
		193,
		247,
		67,
		4,
		63,
		158,
		68,
		238,
		149,
		46,
		126,
		245,
		244,
		34,
		194,
		82,
		16,
		202,
		202,
		47,
		85,
		234,
		177,
		145,
		103,
		107,
		6,
		167,
		139,
		19,
		113,
		83,
		144,
		51,
		172,
		211,
		28,
		133,
		56,
		20,
		84,
		65,
		236,
		67,
		16,
		239,
		26,
		32,
		10,
		254,
		38,
		72,
		99,
		157,
		197,
		181,
		106,
		238,
		33,
		247,
		188,
		47,
		35,
		40,
		87,
		193,
		215,
		151,
		33,
		197,
		170,
		220,
		239,
		73,
		82,
		102,
		162,
		100,
		132,
		69,
		125,
		74,
		225,
		224,
		235,
		68,
		230,
		233,
		9,
		162,
		182,
		97,
		205,
		7,
		35,
		71,
		107,
		239,
		213,
		14,
		6,
		135,
		7,
		137,
		140,
		150,
		80,
		39,
		253,
		197,
		12,
		101,
		164,
		157,
		109,
		89,
		10,
		134,
		225,
		17,
		130,
		168,
		84,
		111,
		116,
		89,
		20,
		67,
		132,
		7,
		204,
		191,
		33,
		103,
		113,
		0,
		12,
		11,
		19,
		139,
		190,
		49,
		110,
		98,
		16,
		209,
		75,
		236,
		139,
		213,
		86,
		4,
		8,
		182,
		121,
		126,
		53,
		5,
		123,
		132,
		234,
		114,
		1,
		125,
		120,
		63,
		150,
		29,
		192,
		102,
		100,
		11,
		230,
		161,
		170,
		133,
		253,
		231,
		199,
		89,
		5,
		45
	};
}
