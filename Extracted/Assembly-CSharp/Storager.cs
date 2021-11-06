using System;
using System.Collections.Generic;
using System.Linq;
using Rilisoft;
using UnityEngine;

// Token: 0x02000769 RID: 1897
public static class Storager
{
	// Token: 0x0600429B RID: 17051 RVA: 0x00161DA8 File Offset: 0x0015FFA8
	static Storager()
	{
		if (BuildSettings.BuildTargetPlatform == RuntimePlatform.IPhonePlayer)
		{
			IEnumerable<string> enumerable = PurchasesSynchronizer.AllItemIds();
			foreach (string key in enumerable)
			{
				Storager.iosCloudSyncBuffer.Add(key, 0);
			}
		}
	}

	// Token: 0x1400009B RID: 155
	// (add) Token: 0x0600429C RID: 17052 RVA: 0x00161E94 File Offset: 0x00160094
	// (remove) Token: 0x0600429D RID: 17053 RVA: 0x00161EAC File Offset: 0x001600AC
	public static event EventHandler RatingUpdated;

	// Token: 0x0600429E RID: 17054 RVA: 0x00161EC4 File Offset: 0x001600C4
	public static void SubscribeToChanged(string key, Action act)
	{
		if (key.IsNullOrEmpty())
		{
			return;
		}
		if (Storager._onValueChanged.ContainsKey(key))
		{
			Dictionary<string, Action> onValueChanged;
			Dictionary<string, Action> dictionary = onValueChanged = Storager._onValueChanged;
			Action a = onValueChanged[key];
			dictionary[key] = (Action)Delegate.Combine(a, act);
		}
		else
		{
			Storager._onValueChanged.Add(key, act);
		}
	}

	// Token: 0x0600429F RID: 17055 RVA: 0x00161F24 File Offset: 0x00160124
	public static void UnSubscribeToChanged(string key, Action act)
	{
		if (key.IsNullOrEmpty())
		{
			return;
		}
		if (!Storager._onValueChanged.ContainsKey(key))
		{
			return;
		}
		Action action = Storager._onValueChanged[key];
		foreach (Delegate d in action.GetInvocationList().ToArray<Delegate>())
		{
			if (d == act)
			{
				action = (Action)Delegate.Remove(action, act);
			}
		}
	}

	// Token: 0x060042A0 RID: 17056 RVA: 0x00161F98 File Offset: 0x00160198
	private static void InvokeSubscribers(string key)
	{
		if (!Application.isPlaying)
		{
			return;
		}
		if (Storager._onValueChanged.ContainsKey(key))
		{
			Action action = Storager._onValueChanged[key];
			if (action != null)
			{
				action();
			}
		}
	}

	// Token: 0x17000AF7 RID: 2807
	// (get) Token: 0x060042A1 RID: 17057 RVA: 0x00161FD8 File Offset: 0x001601D8
	public static bool ICloudAvailable
	{
		get
		{
			return Storager.iCloudAvailable;
		}
	}

	// Token: 0x060042A2 RID: 17058 RVA: 0x00161FE0 File Offset: 0x001601E0
	public static void SynchronizeIosWithCloud(ref List<string> weaponsForWhichSetRememberedTier, out bool armorArmy1Comes)
	{
		armorArmy1Comes = false;
		if (BuildSettings.BuildTargetPlatform != RuntimePlatform.IPhonePlayer || Storager.iCloudAvailable)
		{
		}
	}

	// Token: 0x060042A3 RID: 17059 RVA: 0x00161FFC File Offset: 0x001601FC
	private static int GetIntFromICloud(string key, int defaultValue)
	{
		return defaultValue;
	}

	// Token: 0x060042A4 RID: 17060 RVA: 0x0016200C File Offset: 0x0016020C
	private static void SetIntToICloud(string key, int value)
	{
	}

	// Token: 0x060042A5 RID: 17061 RVA: 0x00162010 File Offset: 0x00160210
	private static void SynchronizeTrophiesWithIosCloud()
	{
		int intFromICloud = Storager.GetIntFromICloud("RatingNegative_CLOUD", 0);
		int intFromICloud2 = Storager.GetIntFromICloud("RatingPositive_CLOUD", 0);
		int num = intFromICloud2 - intFromICloud;
		int intFromICloud3 = Storager.GetIntFromICloud("Season_CLOUD", 0);
		int negativeRating = RatingSystem.instance.negativeRating;
		int positiveRating = RatingSystem.instance.positiveRating;
		int num2 = positiveRating - negativeRating;
		int currentCompetition = FriendsController.sharedController.currentCompetition;
		bool flag = false;
		bool flag2 = false;
		if (intFromICloud3 == 0)
		{
			if (RatingSystem.instance.currentLeague == RatingSystem.RatingLeague.Adamant)
			{
				flag2 = true;
			}
			else
			{
				int num3 = negativeRating;
				if (intFromICloud > negativeRating)
				{
					num3 = intFromICloud;
					RatingSystem.instance.negativeRating = num3;
					flag = true;
				}
				else if (intFromICloud < negativeRating)
				{
					flag2 = true;
				}
				int num4 = positiveRating;
				if (intFromICloud2 > positiveRating)
				{
					num4 = intFromICloud2;
					RatingSystem.instance.positiveRating = num4;
					flag = true;
				}
				else if (intFromICloud2 < positiveRating)
				{
					flag2 = true;
				}
				int num5 = num4 - num3;
				int trophiesSeasonThreshold = RatingSystem.instance.TrophiesSeasonThreshold;
				if (num5 > trophiesSeasonThreshold)
				{
					int num6 = num5 - trophiesSeasonThreshold;
					num3 += num6;
					RatingSystem.instance.negativeRating = num3;
					flag = true;
					flag2 = true;
					TournamentAvailableBannerWindow.CanShow = true;
				}
			}
		}
		else if (intFromICloud3 > currentCompetition)
		{
			FriendsController.sharedController.currentCompetition = intFromICloud3;
			RatingSystem.instance.negativeRating = intFromICloud;
			RatingSystem.instance.positiveRating = intFromICloud2;
			flag = true;
		}
		else if (intFromICloud3 == currentCompetition)
		{
			if (intFromICloud > negativeRating)
			{
				int negativeRating2 = intFromICloud;
				RatingSystem.instance.negativeRating = negativeRating2;
				flag = true;
			}
			else if (intFromICloud < negativeRating)
			{
				flag2 = true;
			}
			if (intFromICloud2 > positiveRating)
			{
				int positiveRating2 = intFromICloud2;
				RatingSystem.instance.positiveRating = positiveRating2;
				flag = true;
			}
			else if (intFromICloud2 < positiveRating)
			{
				flag2 = true;
			}
		}
		else
		{
			flag2 = true;
		}
		EventHandler ratingUpdated = Storager.RatingUpdated;
		if (flag && ratingUpdated != null)
		{
			ratingUpdated(null, EventArgs.Empty);
		}
		if (flag2)
		{
			Storager.SetIntToICloud("RatingNegative_CLOUD", RatingSystem.instance.negativeRating);
			Storager.SetIntToICloud("RatingPositive_CLOUD", RatingSystem.instance.positiveRating);
			Storager.SetIntToICloud("Season_CLOUD", FriendsController.sharedController.currentCompetition);
		}
	}

	// Token: 0x17000AF8 RID: 2808
	// (get) Token: 0x060042A6 RID: 17062 RVA: 0x00162248 File Offset: 0x00160448
	public static bool UseSignedPreferences
	{
		get
		{
			return false;
		}
	}

	// Token: 0x060042A7 RID: 17063 RVA: 0x0016224C File Offset: 0x0016044C
	public static void Initialize(bool cloudAvailable)
	{
	}

	// Token: 0x060042A8 RID: 17064 RVA: 0x00162250 File Offset: 0x00160450
	public static bool hasKey(string key)
	{
		return CryptoPlayerPrefsFacade.HasKey(key);
	}

	// Token: 0x060042A9 RID: 17065 RVA: 0x00162268 File Offset: 0x00160468
	public static void setInt(string key, int val, bool useICloud)
	{
		if (Application.isEditor)
		{
			PlayerPrefs.SetInt(key, val);
		}
		else
		{
			CryptoPlayerPrefsFacade.SetInt(key, val);
			Storager._protectedIntCache[key] = new SaltedInt(Storager._prng.Next(), val);
		}
		if (key.Equals("Coins") || key.Equals("GemsCurrency"))
		{
			DigestStorager.Instance.Set(key, val);
		}
		if (Storager._expendableKeys.Contains(key))
		{
			Storager.RefreshExpendablesDigest();
		}
		if (WeaponManager.PurchasableWeaponSetContains(key))
		{
			Storager._weaponDigestIsDirty = true;
		}
		Storager.InvokeSubscribers(key);
	}

	// Token: 0x060042AA RID: 17066 RVA: 0x00162308 File Offset: 0x00160508
	public static int getInt(string key, bool useICloud)
	{
		if (Application.isEditor)
		{
			return PlayerPrefs.GetInt(key);
		}
		SaltedInt saltedInt;
		if (Storager._protectedIntCache.TryGetValue(key, out saltedInt))
		{
			return saltedInt.Value;
		}
		bool flag = CryptoPlayerPrefsFacade.HasKey(key);
		if (flag)
		{
			int @int = CryptoPlayerPrefsFacade.GetInt(key);
			Storager._protectedIntCache.Add(key, new SaltedInt(Storager._prng.Next(), @int));
			return @int;
		}
		return 0;
	}

	// Token: 0x060042AB RID: 17067 RVA: 0x00162374 File Offset: 0x00160574
	public static void setString(string key, string val, bool useICloud)
	{
		Storager._keychainStringCache[key] = val;
		if (Application.isEditor)
		{
			PlayerPrefs.SetString(key, val);
			Storager.InvokeSubscribers(key);
			return;
		}
		CryptoPlayerPrefsFacade.SetString(key, val);
		Storager.InvokeSubscribers(key);
	}

	// Token: 0x060042AC RID: 17068 RVA: 0x001623A8 File Offset: 0x001605A8
	public static string getString(string key, bool useICloud)
	{
		string result;
		if (Storager._keychainStringCache.TryGetValue(key, out result))
		{
			return result;
		}
		if (BuildSettings.BuildTargetPlatform == RuntimePlatform.IPhonePlayer)
		{
			return PlayerPrefs.GetString(key);
		}
		bool flag = CryptoPlayerPrefsFacade.HasKey(key);
		if (flag)
		{
			string @string = CryptoPlayerPrefsFacade.GetString(key);
			Storager._keychainStringCache.Add(key, @string);
			return @string;
		}
		return string.Empty;
	}

	// Token: 0x060042AD RID: 17069 RVA: 0x00162404 File Offset: 0x00160604
	public static bool IsInitialized(string flagName)
	{
		if (Application.isEditor)
		{
			return PlayerPrefs.HasKey(flagName);
		}
		return Storager.hasKey(flagName);
	}

	// Token: 0x060042AE RID: 17070 RVA: 0x00162420 File Offset: 0x00160620
	public static void SetInitialized(string flagName)
	{
		Storager.setInt(flagName, 0, false);
	}

	// Token: 0x060042AF RID: 17071 RVA: 0x0016242C File Offset: 0x0016062C
	public static void SyncWithCloud(string storageId)
	{
		Storager.getInt(storageId, true);
	}

	// Token: 0x060042B0 RID: 17072 RVA: 0x00162438 File Offset: 0x00160638
	private static void RefreshExpendablesDigest()
	{
		byte[] value = Storager._expendableKeys.SelectMany((string key) => BitConverter.GetBytes(Storager.getInt(key, false))).ToArray<byte>();
		DigestStorager.Instance.Set("ExpendablesCount", value);
	}

	// Token: 0x060042B1 RID: 17073 RVA: 0x00162484 File Offset: 0x00160684
	public static void RefreshWeaponDigestIfDirty()
	{
		if (!Storager._weaponDigestIsDirty)
		{
			return;
		}
		if (Defs.IsDeveloperBuild)
		{
			Debug.LogFormat("[Rilisoft] > RefreshWeaponsDigest: {0:F3}", new object[]
			{
				Time.realtimeSinceStartup
			});
		}
		Storager.RefreshWeaponsDigest();
		if (Defs.IsDeveloperBuild)
		{
			Debug.LogFormat("[Rilisoft] < RefreshWeaponsDigest: {0:F3}", new object[]
			{
				Time.realtimeSinceStartup
			});
		}
	}

	// Token: 0x060042B2 RID: 17074 RVA: 0x001624F0 File Offset: 0x001606F0
	private static void RefreshWeaponsDigest()
	{
		IEnumerable<string> source = from w in WeaponManager.storeIDtoDefsSNMapping.Values
		where Storager.getInt(w, false) == 1
		select w;
		int value = source.Count<string>();
		DigestStorager.Instance.Set("WeaponsCount", value);
		Storager._weaponDigestIsDirty = false;
	}

	// Token: 0x040030B5 RID: 12469
	private const bool useCryptoPlayerPrefs = true;

	// Token: 0x040030B6 RID: 12470
	private const bool _useSignedPreferences = false;

	// Token: 0x040030B7 RID: 12471
	private static readonly Dictionary<string, Action> _onValueChanged = new Dictionary<string, Action>();

	// Token: 0x040030B8 RID: 12472
	private static bool iCloudAvailable = false;

	// Token: 0x040030B9 RID: 12473
	private static readonly Dictionary<string, SaltedInt> _keychainCache = new Dictionary<string, SaltedInt>();

	// Token: 0x040030BA RID: 12474
	private static readonly Dictionary<string, string> _keychainStringCache = new Dictionary<string, string>();

	// Token: 0x040030BB RID: 12475
	private static Dictionary<string, int> iosCloudSyncBuffer = new Dictionary<string, int>();

	// Token: 0x040030BC RID: 12476
	private static bool _weaponDigestIsDirty;

	// Token: 0x040030BD RID: 12477
	private static HashSet<string> m_keysInKeychainIos = new HashSet<string>();

	// Token: 0x040030BE RID: 12478
	private static readonly IDictionary<string, SaltedInt> _protectedIntCache = new Dictionary<string, SaltedInt>();

	// Token: 0x040030BF RID: 12479
	private static readonly System.Random _prng = new System.Random();

	// Token: 0x040030C0 RID: 12480
	private static readonly string[] _expendableKeys = new string[]
	{
		GearManager.InvisibilityPotion,
		GearManager.Jetpack,
		GearManager.Turret,
		GearManager.Mech
	};
}
