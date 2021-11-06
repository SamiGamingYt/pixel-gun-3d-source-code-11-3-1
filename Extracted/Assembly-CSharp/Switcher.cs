using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using Prime31;
using Rilisoft;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x02000826 RID: 2086
[RequireComponent(typeof(FrameStopwatchScript))]
internal sealed class Switcher : MonoBehaviour
{
	// Token: 0x06004BCA RID: 19402 RVA: 0x001B4610 File Offset: 0x001B2810
	static Switcher()
	{
		Switcher.sceneNameToGameNum.Add("Training", 0);
		Switcher.sceneNameToGameNum.Add("Cementery", 1);
		Switcher.sceneNameToGameNum.Add("Maze", 2);
		Switcher.sceneNameToGameNum.Add("City", 3);
		Switcher.sceneNameToGameNum.Add("Hospital", 4);
		Switcher.sceneNameToGameNum.Add("Jail", 5);
		Switcher.sceneNameToGameNum.Add("Gluk_2", 6);
		Switcher.sceneNameToGameNum.Add("Arena", 7);
		Switcher.sceneNameToGameNum.Add("Area52", 8);
		Switcher.sceneNameToGameNum.Add("Slender", 9);
		Switcher.sceneNameToGameNum.Add("Castle", 10);
		Switcher.sceneNameToGameNum.Add("Farm", 11);
		Switcher.sceneNameToGameNum.Add("Bridge", 12);
		Switcher.sceneNameToGameNum.Add("School", 13);
		Switcher.sceneNameToGameNum.Add("Utopia", 14);
		Switcher.sceneNameToGameNum.Add("Sky_islands", 15);
		Switcher.sceneNameToGameNum.Add("Winter", 16);
		Switcher.sceneNameToGameNum.Add("Swamp_campaign3", 17);
		Switcher.sceneNameToGameNum.Add("Castle_campaign3", 18);
		Switcher.sceneNameToGameNum.Add("Space_campaign3", 19);
		Switcher.sceneNameToGameNum.Add("Parkour", 20);
		Switcher.sceneNameToGameNum.Add("Code_campaign3", 21);
		Switcher.counCreateMobsInLevel.Add("Farm", 10);
		Switcher.counCreateMobsInLevel.Add("Cementery", 15);
		Switcher.counCreateMobsInLevel.Add("City", 20);
		Switcher.counCreateMobsInLevel.Add("Hospital", 25);
		Switcher.counCreateMobsInLevel.Add("Bridge", 25);
		Switcher.counCreateMobsInLevel.Add("Jail", 30);
		Switcher.counCreateMobsInLevel.Add("Slender", 30);
		Switcher.counCreateMobsInLevel.Add("Area52", 35);
		Switcher.counCreateMobsInLevel.Add("School", 35);
		Switcher.counCreateMobsInLevel.Add("Utopia", 25);
		Switcher.counCreateMobsInLevel.Add("Maze", 30);
		Switcher.counCreateMobsInLevel.Add("Sky_islands", 30);
		Switcher.counCreateMobsInLevel.Add("Winter", 30);
		Switcher.counCreateMobsInLevel.Add("Castle", 35);
		Switcher.counCreateMobsInLevel.Add("Gluk_2", 35);
		Switcher.counCreateMobsInLevel.Add("Swamp_campaign3", 30);
		Switcher.counCreateMobsInLevel.Add("Castle_campaign3", 35);
		Switcher.counCreateMobsInLevel.Add("Space_campaign3", 25);
		Switcher.counCreateMobsInLevel.Add("Parkour", 15);
		Switcher.counCreateMobsInLevel.Add("Code_campaign3", 35);
	}

	// Token: 0x06004BCB RID: 19403 RVA: 0x001B49B4 File Offset: 0x001B2BB4
	internal static IEnumerable<float> InitializeStorager()
	{
		float progress = 0f;
		if (Application.isEditor)
		{
			if (!PlayerPrefs.HasKey(Defs.initValsInKeychain15))
			{
				Storager.setString(Defs.CapeEquppedSN, Defs.CapeNoneEqupped, false);
				Storager.setString(Defs.HatEquppedSN, Defs.HatNoneEqupped, false);
				yield return progress;
			}
			if (!PlayerPrefs.HasKey(Defs.initValsInKeychain46))
			{
				Storager.setString("MaskEquippedSN", "MaskNoneEquipped", false);
				yield return progress;
			}
		}
		if (!Storager.hasKey(Defs.keysInappBonusGivenkey))
		{
			Storager.setString(Defs.keysInappBonusGivenkey, string.Empty, false);
		}
		if (!Storager.hasKey(Defs.keyInappPresentIDWeaponRedkey))
		{
			Storager.setString(Defs.keyInappBonusStartActionForPresentIDWeaponRedkey, string.Empty, false);
			Storager.setString(Defs.keyInappPresentIDWeaponRedkey, string.Empty, false);
			Storager.setString(Defs.keyInappBonusStartActionForPresentIDPetkey, string.Empty, false);
			Storager.setString(Defs.keyInappPresentIDPetkey, string.Empty, false);
			Storager.setString(Defs.keyInappBonusStartActionForPresentIDGadgetkey, string.Empty, false);
			Storager.setString(Defs.keyInappPresentIDGadgetkey, string.Empty, false);
		}
		if (!Storager.hasKey(Defs.initValsInKeychain15))
		{
			Storager.setInt(Defs.initValsInKeychain15, 0, false);
			Storager.setInt(Defs.LobbyLevelApplied, 1, false);
			Storager.setString(Defs.CapeEquppedSN, Defs.CapeNoneEqupped, false);
			Storager.setString(Defs.HatEquppedSN, Defs.HatNoneEqupped, false);
			Storager.setInt(Defs.IsFirstLaunchFreshInstall, 1, false);
			yield return progress;
		}
		else if (Storager.getInt(Defs.LobbyLevelApplied, false) == 0)
		{
			Storager.setInt(Defs.ShownLobbyLevelSN, 3, false);
		}
		try
		{
			string hat = Storager.getString(Defs.HatEquppedSN, false);
			if (hat != null && (TempItemsController.PriceCoefs.ContainsKey(hat) || Wear.wear[ShopNGUIController.CategoryNames.HatsCategory][0].Contains(hat)))
			{
				Storager.setString(Defs.HatEquppedSN, Defs.HatNoneEqupped, false);
			}
		}
		catch (Exception ex)
		{
			Exception e = ex;
			UnityEngine.Debug.LogError("Exception in Trying to unequip armor hat or temp armor hat (mistakenly got from gocha as a gift): " + e);
		}
		if (!Storager.hasKey(Defs.IsFirstLaunchFreshInstall))
		{
			Storager.setInt(Defs.IsFirstLaunchFreshInstall, 0, false);
		}
		progress = 0.25f;
		if (Application.isEditor || (Application.platform == RuntimePlatform.IPhonePlayer && UnityEngine.Debug.isDebugBuild) || (Application.platform == RuntimePlatform.IPhonePlayer && !Storager.hasKey(Defs.initValsInKeychain17)))
		{
			Storager.setInt(Defs.initValsInKeychain17, 0, false);
			float seconds = Switcher.SecondsFrom1970();
			PlayerPrefs.SetFloat(Defs.TimeFromWhichShowEnder_SN, seconds);
		}
		if (Application.isEditor && !PlayerPrefs.HasKey(Defs.initValsInKeychain27))
		{
			Storager.setString(Defs.BootsEquppedSN, Defs.BootsNoneEqupped, false);
		}
		if (!Storager.hasKey(Defs.initValsInKeychain27))
		{
			Storager.setInt(Defs.initValsInKeychain27, 0, false);
			Storager.setString(Defs.BootsEquppedSN, Defs.BootsNoneEqupped, false);
			yield return progress;
		}
		progress = 0.5f;
		yield return progress;
		if (!Storager.hasKey(Defs.initValsInKeychain40))
		{
			Storager.setInt(Defs.initValsInKeychain40, 0, false);
			Storager.setString(Defs.ArmorNewEquppedSN, Defs.ArmorNewNoneEqupped, false);
			Storager.setInt("GrenadeID", 5, false);
			yield return progress;
		}
		if (!Storager.IsInitialized(Defs.initValsInKeychain41))
		{
			Storager.setInt(Defs.initValsInKeychain41, 0, false);
			string hatBought = null;
			string visualHatArmor = null;
			if (Storager.getInt("hat_Almaz_1", false) > 0)
			{
				hatBought = "hat_Army_3";
				Storager.setInt("hat_Almaz_1", 0, false);
				Storager.setInt("hat_Royal_1", 0, false);
				Storager.setInt("hat_Steel_1", 0, false);
				visualHatArmor = "hat_Almaz_1";
				yield return progress;
			}
			else if (Storager.getInt("hat_Royal_1", false) > 0)
			{
				hatBought = "hat_Army_2";
				Storager.setInt("hat_Royal_1", 0, false);
				Storager.setInt("hat_Steel_1", 0, false);
				visualHatArmor = "hat_Royal_1";
				yield return progress;
			}
			else if (Storager.getInt("hat_Steel_1", false) > 0)
			{
				hatBought = "hat_Army_1";
				Storager.setInt("hat_Steel_1", 0, false);
				visualHatArmor = "hat_Steel_1";
				yield return progress;
			}
			if (hatBought != null)
			{
				string hatEquipped = Storager.getString(Defs.HatEquppedSN, false);
				if (hatEquipped.Equals("hat_Almaz_1") || hatEquipped.Equals("hat_Royal_1") || hatEquipped.Equals("hat_Steel_1"))
				{
					Storager.setString(Defs.HatEquppedSN, hatBought, false);
				}
				for (int i = 0; i <= Wear.wear[ShopNGUIController.CategoryNames.HatsCategory][0].IndexOf(hatBought); i++)
				{
					Storager.setInt(Wear.wear[ShopNGUIController.CategoryNames.HatsCategory][0][i], 1, false);
					yield return progress;
				}
			}
			if (BuildSettings.BuildTargetPlatform == RuntimePlatform.IPhonePlayer)
			{
				Storager.setString(Defs.VisualHatArmor, string.Empty, false);
			}
			if (visualHatArmor != null)
			{
				Storager.setString(Defs.VisualHatArmor, visualHatArmor, false);
			}
			if (!Storager.hasKey("LikeID"))
			{
				Storager.setInt("LikeID", 5, false);
			}
			yield return progress;
			string armorBought = null;
			string visualArmor = null;
			if (Storager.getInt("Armor_Almaz_1", false) > 0)
			{
				armorBought = "Armor_Army_3";
				Storager.setInt("Armor_Almaz_1", 0, false);
				Storager.setInt("Armor_Royal_1", 0, false);
				Storager.setInt("Armor_Steel_1", 0, false);
				visualArmor = "Armor_Almaz_1";
				yield return progress;
			}
			else if (Storager.getInt("Armor_Royal_1", false) > 0)
			{
				armorBought = "Armor_Army_2";
				Storager.setInt("Armor_Royal_1", 0, false);
				Storager.setInt("Armor_Steel_1", 0, false);
				visualArmor = "Armor_Royal_1";
				yield return progress;
			}
			else if (Storager.getInt("Armor_Steel_1", false) > 0)
			{
				armorBought = "Armor_Army_1";
				Storager.setInt("Armor_Steel_1", 0, false);
				visualArmor = "Armor_Steel_1";
				yield return progress;
			}
			if (BuildSettings.BuildTargetPlatform == RuntimePlatform.IPhonePlayer)
			{
				Storager.setString(Defs.VisualArmor, string.Empty, false);
			}
			if (visualArmor != null)
			{
				Storager.setString(Defs.VisualArmor, visualArmor, false);
			}
			yield return progress;
			if (armorBought != null)
			{
				string armorEquipped = Storager.getString(Defs.ArmorNewEquppedSN, false);
				if (armorEquipped.Equals("Armor_Almaz_1") || armorEquipped.Equals("Armor_Royal_1") || armorEquipped.Equals("Armor_Steel_1"))
				{
					Storager.setString(Defs.ArmorNewEquppedSN, armorBought, false);
					yield return progress;
				}
				for (int j = 0; j <= Wear.wear[ShopNGUIController.CategoryNames.ArmorCategory][0].IndexOf(armorBought); j++)
				{
					Storager.setInt(Wear.wear[ShopNGUIController.CategoryNames.ArmorCategory][0][j], 1, false);
					yield return progress;
				}
			}
		}
		progress = 0.75f;
		if (!Storager.IsInitialized(Defs.initValsInKeychain43))
		{
			Storager.SetInitialized(Defs.initValsInKeychain43);
			PlayerPrefs.SetString(Defs.StartTimeShowBannersKey, DateTimeOffset.UtcNow.ToString("s"));
			PlayerPrefs.Save();
			yield return progress;
			if (BuildSettings.BuildTargetPlatform == RuntimePlatform.IPhonePlayer)
			{
				Storager.setInt(Defs.NeedTakeMarathonBonus, 0, false);
				Storager.setInt(Defs.NextMarafonBonusIndex, 0, false);
				yield return progress;
			}
		}
		if (!Storager.hasKey(GearManager.MusicBox))
		{
			Storager.setInt(GearManager.MusicBox, 2, false);
			Storager.setInt(GearManager.Wings, 2, false);
			Storager.setInt(GearManager.Bear, 2, false);
			Storager.setInt(GearManager.BigHeadPotion, 2, false);
		}
		Defs.StartTimeShowBannersString = PlayerPrefs.GetString(Defs.StartTimeShowBannersKey, string.Empty);
		UnityEngine.Debug.Log(" StartTimeShowBannersString=" + Defs.StartTimeShowBannersString);
		if (!Storager.IsInitialized(Defs.initValsInKeychain44))
		{
			Storager.SetInitialized(Defs.initValsInKeychain44);
			if (Storager.hasKey(Defs.NextMarafonBonusIndex) && Storager.getInt(Defs.NextMarafonBonusIndex, false) == -1)
			{
				Storager.setInt(Defs.NextMarafonBonusIndex, 0, false);
			}
			yield return progress;
		}
		if (!Storager.IsInitialized(Defs.initValsInKeychain45))
		{
			Storager.SetInitialized(Defs.initValsInKeychain45);
			Storager.setInt(Defs.PremiumEnabledFromServer, 0, false);
			if (Storager.getInt("currentLevel2", true) == 0)
			{
				PlayerPrefs.SetString(Defs.DateOfInstallAppForInAppPurchases041215, DateTime.UtcNow.ToString("s"));
			}
			yield return progress;
		}
		if (!Storager.IsInitialized(Defs.initValsInKeychain46))
		{
			Storager.SetInitialized(Defs.initValsInKeychain46);
			Storager.setString("MaskEquippedSN", "MaskNoneEquipped", false);
			yield return progress;
		}
		if (!Storager.hasKey("Win Count Timestamp"))
		{
			Storager.setString("Win Count Timestamp", "{ \"1970-01-01\": 0 }", false);
		}
		if (!Storager.hasKey("StartTimeShowStarterPack"))
		{
			Storager.setString("StartTimeShowStarterPack", string.Empty, false);
			yield return progress;
		}
		if (!Storager.hasKey("TimeEndStarterPack"))
		{
			Storager.setString("TimeEndStarterPack", string.Empty, false);
			yield return progress;
		}
		if (!Storager.hasKey("NextNumberStarterPack"))
		{
			Storager.setInt("NextNumberStarterPack", 0, false);
			yield return progress;
		}
		if (!Storager.hasKey(Defs.ArmorEquppedSN))
		{
			Storager.setString(Defs.ArmorEquppedSN, Defs.ArmorNoneEqupped, false);
		}
		if (!Storager.hasKey(Defs.ShowSorryWeaponAndArmor))
		{
			Storager.setInt(Defs.ShowSorryWeaponAndArmor, 0, false);
		}
		if (Storager.getInt(Defs.IsFirstLaunchFreshInstall, false) > 0)
		{
			Storager.setInt(Defs.IsFirstLaunchFreshInstall, 0, false);
		}
		if (!Storager.hasKey(Defs.NewbieEventX3StartTime))
		{
			Storager.setString(Defs.NewbieEventX3StartTime, 0L.ToString(), false);
			Storager.setString(Defs.NewbieEventX3StartTimeAdditional, 0L.ToString(), false);
			Storager.setString(Defs.NewbieEventX3LastLoggedTime, 0L.ToString(), false);
			PlayerPrefs.SetInt(Defs.WasNewbieEventX3, 0);
		}
		if (!PlayerPrefs.HasKey(Defs.LastTimeUpdateAvailableShownSN))
		{
			DateTime myDate = new DateTime(1970, 1, 9, 0, 0, 0);
			DateTimeOffset _1970 = new DateTimeOffset(myDate);
			PlayerPrefs.SetString(Defs.LastTimeUpdateAvailableShownSN, _1970.ToString("s"));
			PlayerPrefs.Save();
		}
		string lastTimeUpdateShownString = PlayerPrefs.GetString(Defs.LastTimeUpdateAvailableShownSN);
		DateTimeOffset lastTimeUpdateShown = default(DateTimeOffset);
		if (!DateTimeOffset.TryParse(lastTimeUpdateShownString, out lastTimeUpdateShown) && UnityEngine.Debug.isDebugBuild)
		{
			UnityEngine.Debug.LogWarning("Cannot parse " + lastTimeUpdateShownString);
		}
		if (DateTimeOffset.Now - lastTimeUpdateShown > TimeSpan.FromHours(12.0))
		{
			PlayerPrefs.SetInt(Defs.UpdateAvailableShownTimesSN, 3);
			PlayerPrefs.SetString(Defs.LastTimeUpdateAvailableShownSN, DateTimeOffset.Now.ToString("s"));
			yield return progress;
		}
		float eventX3ShowTimeoutHours = 12f;
		if (!PlayerPrefs.HasKey(Defs.EventX3WindowShownLastTime))
		{
			PlayerPrefs.SetInt(Defs.EventX3WindowShownCount, 1);
			PlayerPrefs.SetString(Defs.EventX3WindowShownLastTime, PromoActionsManager.CurrentUnixTime.ToString());
			yield return progress;
		}
		long eventX3WindowShownLastTime;
		long.TryParse(PlayerPrefs.GetString(Defs.EventX3WindowShownLastTime), out eventX3WindowShownLastTime);
		if (PromoActionsManager.CurrentUnixTime - eventX3WindowShownLastTime > (long)TimeSpan.FromHours((double)eventX3ShowTimeoutHours).TotalSeconds)
		{
			PlayerPrefs.SetInt(Defs.EventX3WindowShownCount, 1);
			PlayerPrefs.SetString(Defs.EventX3WindowShownLastTime, PromoActionsManager.CurrentUnixTime.ToString());
		}
		PlayerPrefs.Save();
		yield return progress;
		if (!PlayerPrefs.HasKey(Defs.AdvertWindowShownLastTime))
		{
			PlayerPrefs.SetInt(Defs.AdvertWindowShownCount, 3);
			PlayerPrefs.SetString(Defs.AdvertWindowShownLastTime, PromoActionsManager.CurrentUnixTime.ToString());
		}
		long advertWindowShownLastTime;
		long.TryParse(PlayerPrefs.GetString(Defs.AdvertWindowShownLastTime), out advertWindowShownLastTime);
		float advertShowTimeoutHours = (!Defs.IsDeveloperBuild) ? 12f : 0.083333336f;
		if (PromoActionsManager.CurrentUnixTime - advertWindowShownLastTime > (long)TimeSpan.FromHours((double)advertShowTimeoutHours).TotalSeconds)
		{
			PlayerPrefs.SetInt(Defs.AdvertWindowShownCount, 3);
			PlayerPrefs.SetString(Defs.AdvertWindowShownLastTime, PromoActionsManager.CurrentUnixTime.ToString());
		}
		yield return progress;
		if (BuildSettings.BuildTargetPlatform == RuntimePlatform.IPhonePlayer)
		{
			if (!Storager.hasKey(Defs.LevelsWhereGetCoinS))
			{
				CoinBonus.SetLevelsWhereGotBonus(string.Empty, VirtualCurrencyBonusType.Coin);
			}
			if (!Storager.hasKey(Defs.LevelsWhereGotGems))
			{
				CoinBonus.SetLevelsWhereGotBonus("[]", VirtualCurrencyBonusType.Gem);
			}
			if (!Storager.hasKey(Defs.RatingFlag))
			{
				Storager.setInt(Defs.RatingDeathmatch, 0, false);
				Storager.setInt(Defs.RatingTeamBattle, 0, false);
				Storager.setInt(Defs.RatingHunger, 0, false);
				Storager.setInt(Defs.RatingFlag, 0, false);
			}
			if (!Storager.hasKey(Defs.RatingCapturePoint))
			{
				Storager.setInt(Defs.RatingCapturePoint, 0, false);
			}
		}
		PlayerPrefs.Save();
		progress = 1f;
		yield return progress;
		yield break;
	}

	// Token: 0x06004BCC RID: 19404 RVA: 0x001B49D0 File Offset: 0x001B2BD0
	private static double Hypot(double x, double y)
	{
		x = Math.Abs(x);
		y = Math.Abs(y);
		double num = Math.Max(x, y);
		double num2 = Math.Min(x, y) / num;
		return num * Math.Sqrt(1.0 + num2 * num2);
	}

	// Token: 0x06004BCD RID: 19405 RVA: 0x001B4A14 File Offset: 0x001B2C14
	private IEnumerator ParseConfigsCoroutine()
	{
		float start = Time.realtimeSinceStartup;
		using (new ScopeLogger("Switcher.Start()", "Parsing advert config", Defs.IsDeveloperBuild))
		{
			if (Storager.hasKey("abTestAdvertConfigKey"))
			{
				FriendsController.ParseABTestAdvertConfig(false);
			}
			else
			{
				Storager.setString("abTestAdvertConfigKey", string.Empty, false);
			}
		}
		if (Time.realtimeSinceStartup - start > 0.016666668f)
		{
			start = Time.realtimeSinceStartup;
			yield return null;
		}
		yield break;
	}

	// Token: 0x06004BCE RID: 19406 RVA: 0x001B4A28 File Offset: 0x001B2C28
	private IEnumerator Start()
	{
		this.oldProgress = 0f;
		UnityEngine.Debug.LogFormat("> Switcher.Start(): {0:f3}, {1}", new object[]
		{
			Time.realtimeSinceStartup,
			Time.frameCount
		});
		yield return base.StartCoroutine(this.ParseConfigsCoroutine());
		if (!TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage == TrainingController.NewTrainingCompletedStage.None)
		{
			Switcher.PlayComicsSound();
		}
		UnityEngine.Debug.Log("Switcher.Start() > InitializeSwitcher()");
		bool armyArmor1ComesFromCloud = false;
		foreach (float num in this.InitializeSwitcher(delegate
		{
			armyArmor1ComesFromCloud = true;
		}))
		{
			float step = num;
			Switcher.timer.Reset();
			Switcher.timer.Start();
			this.oldProgress = this._progress;
			ActivityIndicator.LoadingProgress = this._progress;
			yield return step;
		}
		using (new ScopeLogger("Switcher.Start()", "Loading main menu asynchronously", Defs.IsDeveloperBuild))
		{
			foreach (float num2 in this.LoadMainMenu(armyArmor1ComesFromCloud))
			{
				float step2 = num2;
				Switcher.timer.Reset();
				Switcher.timer.Start();
				this.oldProgress = this._progress;
				ActivityIndicator.LoadingProgress = this._progress;
				yield return step2;
			}
		}
		UnityEngine.Debug.LogFormat("< Switcher.Start(): {0:f3}, {1}", new object[]
		{
			Time.realtimeSinceStartup,
			Time.frameCount
		});
		yield break;
	}

	// Token: 0x17000C73 RID: 3187
	// (get) Token: 0x06004BCF RID: 19407 RVA: 0x001B4A44 File Offset: 0x001B2C44
	// (set) Token: 0x06004BD0 RID: 19408 RVA: 0x001B4A78 File Offset: 0x001B2C78
	public static string InitialAppVersion
	{
		get
		{
			if (!Switcher._initialAppVersionInitialized)
			{
				Switcher._InitialAppVersion = PlayerPrefs.GetString(Defs.InitialAppVersionKey);
				Switcher._initialAppVersionInitialized = true;
			}
			return Switcher._InitialAppVersion;
		}
		private set
		{
			Switcher._InitialAppVersion = value;
			Switcher._initialAppVersionInitialized = true;
		}
	}

	// Token: 0x06004BD1 RID: 19409 RVA: 0x001B4A88 File Offset: 0x001B2C88
	public static string LoadingCupTexture(int number)
	{
		return "loading_cups_" + number.ToString() + ((!Device.isRetinaAndStrong) ? string.Empty : "-hd");
	}

	// Token: 0x06004BD2 RID: 19410 RVA: 0x001B4AC4 File Offset: 0x001B2CC4
	public IEnumerable<float> InitializeSwitcher(Action setArmorArmy1ComesFromCloud = null)
	{
		UnityEngine.Debug.Log("> InitializeSwitcher()");
		Stopwatch _stopwatch = new Stopwatch();
		ProgressBounds bounds = new ProgressBounds();
		Action logBounds = delegate()
		{
		};
		Action<string> log = delegate(string s)
		{
		};
		Func<float, long, string> format = delegate(float progress, long ms)
		{
			int num7 = Mathf.RoundToInt(progress * 100f);
			return string.Format(" << {0}%: {1}", num7, ms);
		};
		InGameTimeKeeper.Instance.Initialize();
		if (!TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage == TrainingController.NewTrainingCompletedStage.None)
		{
			string bgTextureName = Switcher.LoadingCupTexture(1);
			this.fonToDraw = Resources.Load<Texture>(bgTextureName);
			string legendLocalization = LocalizationStore.Get("Key_1925");
			string legendText = (!("Key_1925" == legendLocalization)) ? legendLocalization : "PLEASE REBOOT YOUR DEVICE IF FROZEN.";
			ActivityIndicator.instance.legendLabel.text = legendText;
			ActivityIndicator.instance.legendLabel.gameObject.SetActive(true);
		}
		else
		{
			string bgTextureName2 = ConnectSceneNGUIController.MainLoadingTexture();
			this.fonToDraw = Resources.Load<Texture>(bgTextureName2);
		}
		ActivityIndicator.SetLoadingFon(this.fonToDraw);
		ActivityIndicator.IsShowWindowLoading = true;
		ActivityIndicator.instance.panelProgress.SetActive(true);
		bounds.SetBounds(0f, 0.09f);
		logBounds();
		this._progress = bounds.LowerBound;
		yield return this._progress;
		if (!PlayerPrefs.HasKey("First Launch (Advertisement)"))
		{
			PlayerPrefs.SetString("First Launch (Advertisement)", DateTimeOffset.UtcNow.ToString("s"));
		}
		if (!PlayerPrefs.HasKey(Defs.InitialAppVersionKey))
		{
			if (!PlayerPrefs.HasKey("NamePlayer"))
			{
				PlayerPrefs.SetString(Defs.InitialAppVersionKey, GlobalGameController.AppVersion);
			}
			else
			{
				PlayerPrefs.SetString(Defs.InitialAppVersionKey, "1.0.0");
			}
		}
		Switcher.InitialAppVersion = PlayerPrefs.GetString(Defs.InitialAppVersionKey);
		if (BuildSettings.BuildTargetPlatform == RuntimePlatform.Android)
		{
			AbstractManager.initialize(typeof(GoogleIABManager));
		}
		if (Defs.AndroidEdition == Defs.RuntimeAndroidEdition.GoogleLite)
		{
			try
			{
				if (Defs.IsDeveloperBuild)
				{
					UnityEngine.Debug.Log("Switcher: Trying to initialize Google Play Games...");
				}
				GpgFacade.Instance.Initialize();
			}
			catch (Exception ex2)
			{
				Exception ex = ex2;
				UnityEngine.Debug.LogException(ex);
			}
		}
		this._progress = bounds.Clamp(this._progress + 0.005f);
		yield return this._progress;
		if (this.sponsorPayPluginHolderPrefab != null)
		{
			UnityEngine.Object.Instantiate<GameObject>(this.sponsorPayPluginHolderPrefab);
		}
		this._progress = bounds.Clamp(this._progress + 0.005f);
		yield return this._progress;
		UnityEngine.Object.Instantiate<GameObject>(this.balanceControllerPrefab);
		this._progress = bounds.Clamp(this._progress + 0.01f);
		yield return this._progress;
		GlobalGameController.LeftHanded = (PlayerPrefs.GetInt(Defs.LeftHandedSN, 1) == 1);
		if (!PlayerPrefs.HasKey(Defs.SwitchingWeaponsSwipeRegimSN))
		{
			double diagonalInPixels = Switcher.Hypot((double)Screen.width, (double)Screen.height);
			int switchingWeaponMode = 0;
			if (Screen.dpi > 0f)
			{
				double diagonalInInches = diagonalInPixels / (double)Screen.dpi;
				if (UnityEngine.Debug.isDebugBuild)
				{
					UnityEngine.Debug.Log(string.Format("Device dpi: {0},    diagonal: {1} px ({2}\")", Screen.dpi, diagonalInPixels, diagonalInInches));
				}
				switchingWeaponMode = ((diagonalInInches >= 6.8) ? 1 : 0);
			}
			else if (UnityEngine.Debug.isDebugBuild)
			{
				UnityEngine.Debug.Log(string.Format("Device dpi: {0},    diagonal: {1} px", Screen.dpi, diagonalInPixels));
			}
			PlayerPrefs.SetInt(Defs.SwitchingWeaponsSwipeRegimSN, switchingWeaponMode);
		}
		GlobalGameController.switchingWeaponSwipe = (PlayerPrefs.GetInt(Defs.SwitchingWeaponsSwipeRegimSN, 0) == 1);
		string oldV = Load.LoadString("keyOldVersion");
		string curV = GlobalGameController.AppVersion;
		if (oldV != curV)
		{
			PlayerPrefs.SetInt("countSessionDayOnStartCorrentVersion", PlayerPrefs.GetInt(Defs.SessionDayNumberKey, 1));
			ReviewController.IsSendReview = false;
			ReviewController.ExistReviewForSend = false;
			ReviewController.CheckActiveReview();
			Save.SaveString("keyOldVersion", curV);
		}
		Tools.AddSessionNumber();
		CoroutineRunner.Instance.StartCoroutine(AnalyticsStuff.WaitInitializationThenLogGameDayCountCoroutine());
		if (!Storager.hasKey(Defs.WeaponsGotInCampaign))
		{
			Storager.setString(Defs.WeaponsGotInCampaign, string.Empty, false);
		}
		this._progress = bounds.Clamp(this._progress + 0.01f);
		yield return this._progress;
		Screen.sleepTimeout = 180;
		if (SkinsController.sharedController == null && this.skinsManagerPrefab)
		{
			UnityEngine.Object.Instantiate(this.skinsManagerPrefab, Vector3.zero, Quaternion.identity);
			this._progress = bounds.Clamp(this._progress + 0.01f);
			yield return this._progress;
			foreach (float num in SkinsController.sharedController.LoadSkinsInTexture())
			{
				float step = num;
				yield return this._progress;
			}
		}
		if (PromoActionsManager.sharedManager == null && this.promoActionsManagerPrefab != null)
		{
			UnityEngine.Object.Instantiate<GameObject>(this.promoActionsManagerPrefab);
		}
		this._progress = bounds.Clamp(this._progress + 0.01f);
		yield return this._progress;
		if (this.nickStackPrefab == null)
		{
			UnityEngine.Debug.LogError("Switcher.InitializeSwitcher():    nickStackPrefab == null");
		}
		else if (NickLabelStack.sharedStack == null)
		{
			UnityEngine.Object nicklabelStack = UnityEngine.Object.Instantiate(this.nickStackPrefab, Vector3.zero, Quaternion.identity);
		}
		this._progress = bounds.Clamp(this._progress + 0.01f);
		yield return this._progress;
		if (this.sceneInfoController == null)
		{
			UnityEngine.Debug.LogError("Switcher.InitializeSwitcher():    sceneInfoController == null");
		}
		else
		{
			UnityEngine.Object.Instantiate(this.sceneInfoController, Vector3.zero, Quaternion.identity);
		}
		if (this.ExperienceControllerPrefab == null)
		{
			UnityEngine.Debug.LogError("Switcher.InitializeSwitcher():    ExperienceControllerPrefab == null");
		}
		else if (ExperienceController.sharedController == null)
		{
			UnityEngine.Object experienceController = UnityEngine.Object.Instantiate(this.ExperienceControllerPrefab, Vector3.zero, Quaternion.identity);
			this._progress = bounds.Lerp(this._progress, 0.6f);
			yield return this._progress;
			foreach (float num2 in ExperienceController.sharedController.InitController())
			{
				float step2 = num2;
				this._progress = bounds.Clamp(this._progress + 0.01f);
				yield return this._progress;
			}
		}
		bounds.SetBounds(0.1f, 0.19f);
		logBounds();
		this._progress = bounds.LowerBound;
		yield return this._progress;
		if (this.experienceGuiPrefab != null)
		{
			if (ExpController.Instance == null)
			{
				UnityEngine.Object expGui = UnityEngine.Object.Instantiate(this.experienceGuiPrefab, Vector3.zero, Quaternion.identity);
				UnityEngine.Object.DontDestroyOnLoad(expGui);
			}
		}
		else
		{
			UnityEngine.Debug.LogWarning("ExperienceGuiPrefab == null");
		}
		this._progress = bounds.Clamp(this._progress + 0.01f);
		yield return this._progress;
		if (this.bankGuiPrefab != null)
		{
			if (BankController.Instance == null)
			{
				UnityEngine.Object bankGui = UnityEngine.Object.Instantiate(this.bankGuiPrefab, Vector3.zero, Quaternion.identity);
				UnityEngine.Object.DontDestroyOnLoad(bankGui);
			}
		}
		else
		{
			UnityEngine.Debug.LogWarning("BankGuiPrefab == null");
		}
		this._progress = bounds.Clamp(this._progress + 0.01f);
		yield return this._progress;
		if (this.freeAwardGuiPrefab != null)
		{
			if (FreeAwardController.Instance == null)
			{
				UnityEngine.Object freeAwardGui = UnityEngine.Object.Instantiate(this.freeAwardGuiPrefab, Vector3.zero, Quaternion.identity);
				UnityEngine.Object.DontDestroyOnLoad(freeAwardGui);
			}
		}
		else
		{
			UnityEngine.Debug.LogWarning("freeAwardGuiPrefab == null");
		}
		this._progress = bounds.Clamp(this._progress + 0.01f);
		yield return this._progress;
		AnalyticsFacade.Initialize();
		PersistentCache persistentCache = PersistentCache.Instance;
		if (Defs.IsDeveloperBuild)
		{
			UnityEngine.Debug.LogFormat("Persistent cache: '{0}'", new object[]
			{
				persistentCache.PersistentDataPath
			});
		}
		this._progress = bounds.Clamp(this._progress + 0.01f);
		yield return this._progress;
		if (RemotePushNotificationController.Instance == null && this.remotePushNotificationControllerPrefab)
		{
			UnityEngine.Object.Instantiate<GameObject>(this.remotePushNotificationControllerPrefab);
		}
		this._progress = bounds.Clamp(this._progress + 0.01f);
		yield return this._progress;
		this._progress = bounds.Clamp(this._progress + 0.01f);
		yield return this._progress;
		if (ShopNGUIController.sharedShop == null)
		{
			ResourceRequest shopTask = Resources.LoadAsync("ShopNGUI");
			while (!shopTask.isDone)
			{
				yield return this._progress;
			}
			UnityEngine.Object shopP = shopTask.asset;
			this._progress = bounds.Clamp(this._progress + 0.01f);
			yield return this._progress;
			UnityEngine.Object.Instantiate(shopP, Vector3.zero, Quaternion.identity);
		}
		bounds.SetBounds(0.2f, 0.29f);
		logBounds();
		this._progress = bounds.LowerBound;
		yield return this._progress;
		this._progress = bounds.Clamp(this._progress + 0.01f);
		yield return this._progress;
		if (Storager.getInt("InitBanerAwardCompition", false) == 0)
		{
			if (RatingSystem.instance.currentLeague == RatingSystem.RatingLeague.Adamant)
			{
				TournamentAvailableBannerWindow.CanShow = true;
				int threshold = RatingSystem.instance.TrophiesSeasonThreshold;
				if (RatingSystem.instance.currentRating > threshold)
				{
					int compensate = RatingSystem.instance.currentRating - threshold;
					RatingSystem.instance.negativeRating += compensate;
					RatingSystem.instance.UpdateLeagueEvent(null, null);
				}
			}
			Storager.setInt("InitBanerAwardCompition", 1, false);
		}
		this._progress = bounds.Clamp(this._progress + 0.01f);
		yield return this._progress;
		if (FriendsController.sharedController == null)
		{
			ResourceRequest friendsControllerTask = Resources.LoadAsync("FriendsController");
			while (!friendsControllerTask.isDone)
			{
				yield return this._progress;
			}
			UnityEngine.Object fcp = friendsControllerTask.asset;
			this._progress = bounds.Clamp(this._progress + 0.01f);
			yield return this._progress;
			UnityEngine.Object.Instantiate(fcp, Vector3.zero, Quaternion.identity);
			yield return this._progress;
			foreach (float num3 in FriendsController.sharedController.InitController())
			{
				float step3 = num3;
				yield return this._progress;
			}
		}
		this._progress = bounds.Clamp(this._progress + 0.01f);
		yield return this._progress;
		string token = null;
		Storager.Initialize(!token.IsNullOrEmpty());
		this._progress = bounds.Clamp(this._progress + 0.01f);
		yield return this._progress;
		if (!TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage == TrainingController.NewTrainingCompletedStage.None)
		{
			this.fonToDraw = Resources.Load<Texture>(Switcher.LoadingCupTexture(2));
			foreach (float num4 in ActivityIndicator.instance.ReplaceLoadingFon(this.fonToDraw, 0.3f))
			{
				float step4 = num4;
				yield return this._progress;
			}
			ActivityIndicator.instance.legendLabel.text = LocalizationStore.Get("Key_1926");
		}
		this._progress = bounds.Clamp(this._progress + 0.01f);
		yield return this._progress;
		_stopwatch.Start();
		foreach (float num5 in Switcher.InitializeStorager())
		{
			float storagerInitProgress = num5;
			if (_stopwatch.ElapsedMilliseconds > 100L)
			{
				_stopwatch.Reset();
				_stopwatch.Start();
				yield return this._progress;
			}
		}
		_stopwatch.Reset();
		this._progress = bounds.Clamp(this._progress + 0.01f);
		yield return this._progress;
		if (this.disabler != null)
		{
			UnityEngine.Object.Instantiate<GameObject>(this.disabler);
		}
		bounds.SetBounds(0.3f, 0.39f);
		logBounds();
		this._progress = bounds.LowerBound;
		yield return this._progress;
		if (BuildSettings.BuildTargetPlatform == RuntimePlatform.IPhonePlayer)
		{
			List<string> weaponsForWhichSetRememberedTier = new List<string>();
			bool armorArmy1Comes;
			Storager.SynchronizeIosWithCloud(ref weaponsForWhichSetRememberedTier, out armorArmy1Comes);
			if (armorArmy1Comes && setArmorArmy1ComesFromCloud != null)
			{
				setArmorArmy1ComesFromCloud();
			}
			this._progress = bounds.Clamp(this._progress + 0.01f);
			yield return this._progress;
			Storager.SyncWithCloud(Defs.SkinsMakerInProfileBought);
			Storager.SyncWithCloud(Defs.code010110_Key);
			Storager.SyncWithCloud(Defs.smallAsAntKey);
			Storager.SyncWithCloud(Defs.UnderwaterKey);
			this._progress = bounds.Clamp(this._progress + 0.01f);
			yield return this._progress;
			ShopNGUIController.CategoryNames[] firstCats = new ShopNGUIController.CategoryNames[]
			{
				ShopNGUIController.CategoryNames.HatsCategory,
				ShopNGUIController.CategoryNames.CapesCategory
			};
			foreach (ShopNGUIController.CategoryNames cat in firstCats)
			{
				foreach (List<string> ll in Wear.wear[cat])
				{
					foreach (string item in ll)
					{
						Storager.SyncWithCloud(item);
					}
				}
			}
			yield return this._progress;
			IEnumerable<ShopNGUIController.CategoryNames> secondCats = Wear.wear.Keys.Except(firstCats);
			foreach (ShopNGUIController.CategoryNames cat2 in secondCats)
			{
				foreach (List<string> ll2 in Wear.wear[cat2])
				{
					foreach (string item2 in ll2)
					{
						Storager.SyncWithCloud(item2);
					}
				}
			}
			this._progress = bounds.Clamp(this._progress + 0.01f);
			yield return this._progress;
			foreach (SkinItem _skinInfo in SkinsController.sharedController.skinItems)
			{
				Storager.SyncWithCloud(_skinInfo.name);
			}
			yield return this._progress;
			foreach (string weaponSkin in WeaponSkinsManager.SkinIds)
			{
				Storager.SyncWithCloud(weaponSkin);
			}
			this._progress = bounds.Clamp(this._progress + 0.01f);
			yield return this._progress;
			int levelBefore = (!(ExperienceController.sharedController != null)) ? 1 : ExperienceController.sharedController.currentLevel;
			WeaponManager.RefreshExpControllers();
			ExperienceController.SendAnalyticsForLevelsFromCloud(levelBefore);
			try
			{
				WeaponManager.SetRememberedTiersForWeaponsComesFromCloud(weaponsForWhichSetRememberedTier);
			}
			catch (Exception ex3)
			{
				Exception e = ex3;
				UnityEngine.Debug.LogError("SetRememberedTiersForWeaponsComesFromCloud exception: " + e);
			}
			this._progress = bounds.Clamp(this._progress + 0.01f);
			yield return this._progress;
			Dictionary<string, GadgetInfo>.KeyCollection gadgetIds = GadgetsInfo.info.Keys;
			foreach (string gadgetId in gadgetIds)
			{
				Storager.SyncWithCloud(gadgetId);
			}
			List<string> canBuyWeaponStorageIds = ItemDb.GetCanBuyWeaponStorageIds(true);
			this._progress = bounds.Clamp(this._progress + 0.01f);
			yield return this._progress;
			_stopwatch.Start();
			for (int i = 0; i < canBuyWeaponStorageIds.Count; i++)
			{
				string storageId = canBuyWeaponStorageIds[i];
				if (!string.IsNullOrEmpty(storageId))
				{
					Storager.SyncWithCloud(storageId);
				}
				if (i % 100 == 0)
				{
					this._progress = bounds.Clamp(this._progress + 0.01f);
					yield return this._progress;
					_stopwatch.Reset();
					_stopwatch.Start();
				}
				if (_stopwatch.ElapsedMilliseconds > 100L)
				{
					yield return this._progress;
					_stopwatch.Reset();
					_stopwatch.Start();
				}
			}
		}
		if (!TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage == TrainingController.NewTrainingCompletedStage.None)
		{
			AnalyticsStuff.Tutorial(AnalyticsConstants.TutorialState.Started, 0);
			AnalyticsStuff.TrySendOnceToAppsFlyer("first_launch");
			AnalyticsStuff.TrySendOnceToFacebook("app_launch_first", null, null);
		}
		bounds.SetBounds(0.4f, 0.49f);
		logBounds();
		this._progress = bounds.LowerBound;
		yield return this._progress;
		Switcher.CountMoneyForRemovedGear();
		this._progress = bounds.Clamp(this._progress + 0.001f);
		yield return this._progress;
		Switcher.CountMoneyForArmorHats();
		if (Storager.hasKey(Defs.HatEquppedSN) && Storager.getString(Defs.HatEquppedSN, false) == "hat_ManiacMask")
		{
			Storager.setString(Defs.HatEquppedSN, ShopNGUIController.NoneEquippedForWearCategory(ShopNGUIController.CategoryNames.HatsCategory), false);
			Storager.setString("MaskEquippedSN", "hat_ManiacMask", false);
		}
		this._progress = bounds.Clamp(this._progress + 0.001f);
		yield return this._progress;
		WeaponManager.ActualizeWeaponsForCampaignProgress();
		this._progress = bounds.Clamp(0.41f);
		yield return this._progress;
		if (coinsShop.thisScript == null && this.coinsShopPrefab)
		{
			UnityEngine.Object.Instantiate<GameObject>(this.coinsShopPrefab);
		}
		this._progress = bounds.Clamp(this._progress + 0.01f);
		yield return this._progress;
		if (FacebookController.sharedController == null && FacebookController.FacebookSupported && this.faceBookControllerPrefab != null)
		{
			UnityEngine.Object.Instantiate<GameObject>(this.faceBookControllerPrefab);
		}
		this._progress = bounds.Clamp(this._progress + 0.01f);
		yield return this._progress;
		if (ButtonClickSound.Instance == null && this.buttonClickSoundPrefab != null)
		{
			UnityEngine.Object.Instantiate<GameObject>(this.buttonClickSoundPrefab);
		}
		this._progress = bounds.Clamp(this._progress + 0.005f);
		yield return this._progress;
		this._progress = bounds.Clamp(this._progress + 0.01f);
		yield return this._progress;
		bool needInstantiateLicenseVerification = false;
		if (Defs.IsDeveloperBuild)
		{
			UnityEngine.Debug.LogFormat("Loading {0:P1} > Instantiate License Verification Controller: {1}", new object[]
			{
				this._progress,
				needInstantiateLicenseVerification
			});
		}
		if (needInstantiateLicenseVerification)
		{
			UnityEngine.Object.Instantiate<GameObject>(this.licenseVerificationControllerPrefab);
		}
		this._progress = bounds.Clamp(this._progress + 0.01f);
		yield return this._progress;
		bool needInstantiateTempItems = TempItemsController.sharedController == null && this.tempItemsControllerPrefab != null;
		if (Defs.IsDeveloperBuild)
		{
			UnityEngine.Debug.LogFormat("Loading {0:P1} > Instantiate Temp Items: {1}", new object[]
			{
				this._progress,
				needInstantiateTempItems
			});
		}
		if (needInstantiateTempItems)
		{
			UnityEngine.Object.Instantiate<GameObject>(this.tempItemsControllerPrefab);
		}
		this._progress = bounds.Clamp(this._progress + 0.01f);
		yield return this._progress;
		if (this.updateCheckerPrefab != null)
		{
			UnityEngine.Object.Instantiate<GameObject>(this.updateCheckerPrefab);
		}
		bounds.SetBounds(0.5f, 0.52f);
		logBounds();
		this._progress = bounds.LowerBound;
		if (!TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage == TrainingController.NewTrainingCompletedStage.None)
		{
			yield return this._progress;
			this.fonToDraw = Resources.Load<Texture>(Switcher.LoadingCupTexture(3));
			foreach (float num6 in ActivityIndicator.instance.ReplaceLoadingFon(this.fonToDraw, 0.3f))
			{
				float step5 = num6;
				yield return this._progress;
			}
			ActivityIndicator.instance.legendLabel.text = LocalizationStore.Get("Key_1927");
		}
		yield return this._progress;
		this._progress = bounds.Clamp(this._progress + 0.01f);
		if (Defs.IsDeveloperBuild)
		{
			UnityEngine.Debug.LogFormat("Loading {0:P1} > Instantiate TwitterController.", new object[]
			{
				this._progress
			});
		}
		if (TwitterController.Instance == null && this.twitterControllerPrefab != null)
		{
			UnityEngine.Object.Instantiate<GameObject>(this.twitterControllerPrefab);
		}
		yield return this._progress;
		this._progress = bounds.Clamp(this._progress + 0.01f);
		WeaponManager wm = null;
		using (new ScopeLogger("Loading " + this._progress.ToString("P1", CultureInfo.InvariantCulture), "Instantiate WeaponManager.", Defs.IsDeveloperBuild))
		{
			GameObject o = (GameObject)UnityEngine.Object.Instantiate(this.weaponManagerPrefab, Vector3.zero, Quaternion.identity);
			wm = o.GetComponent<WeaponManager>();
		}
		bounds.SetBounds(0.52f, 0.88f);
		logBounds();
		this._progress = bounds.LowerBound;
		yield return this._progress;
		if (wm != null)
		{
			int j = 0;
			while (!wm.Initialized)
			{
				this._progress = bounds.Clamp(this._progress + 0.01f);
				yield return this._progress;
				if (Launcher.UsingNewLauncher)
				{
					yield return -1f;
				}
				j++;
			}
		}
		yield return this._progress;
		bounds.SetBounds(0.89f, 0.99f);
		logBounds();
		this._progress = bounds.LowerBound;
		yield return this._progress;
		this.SetUpPhoton(MiscAppsMenu.Instance.misc);
		this._progress = bounds.Clamp(this._progress + 0.01f);
		yield return this._progress;
		Switcher.CheckHugeUpgrade();
		Switcher.PerformEssentialInitialization("Coins", AbuseMetod.Coins);
		Switcher.PerformEssentialInitialization("GemsCurrency", AbuseMetod.Gems);
		Switcher.PerformExpendablesInitialization();
		this._progress = bounds.Clamp(this._progress + 0.01f);
		yield return this._progress;
		CampaignProgress.OpenNewBoxIfPossible();
		if (StarterPackController.Get == null && this.starterPackManagerPrefab != null)
		{
			UnityEngine.Object.Instantiate<GameObject>(this.starterPackManagerPrefab);
		}
		if (PotionsController.sharedController == null && this.potionsControllerPrefab != null)
		{
			UnityEngine.Object.Instantiate<GameObject>(this.potionsControllerPrefab);
		}
		this._progress = bounds.Clamp(this._progress + 0.01f);
		yield return this._progress;
		QuestSystem.Instance.Initialize();
		this._progress = bounds.Clamp(this._progress + 0.01f);
		yield return this._progress;
		if (PremiumAccountController.Instance == null && this.premiumAccountControllerPrefab != null)
		{
			UnityEngine.Object.Instantiate<GameObject>(this.premiumAccountControllerPrefab);
		}
		this._progress = bounds.Clamp(this._progress + 0.01f);
		yield return this._progress;
		if (BuildSettings.BuildTargetPlatform == RuntimePlatform.IPhonePlayer)
		{
			Storager.SyncWithCloud("PayingUser");
			Storager.SyncWithCloud(Defs.IsFacebookLoginRewardaGained);
			Storager.SyncWithCloud(Defs.IsTwitterLoginRewardaGained);
			foreach (string gochaGun in WeaponManager.GotchaGuns)
			{
				Storager.SyncWithCloud(gochaGun);
			}
		}
		if (GiftController.Instance == null && this.giftControllerPrefab != null)
		{
			UnityEngine.Object.Instantiate<GameObject>(this.giftControllerPrefab);
		}
		Screen.sleepTimeout = 180;
		this._progress = bounds.Clamp(this._progress + 0.01f);
		while (!Singleton<AchievementsManager>.Instance.IsReady)
		{
			yield return this._progress;
		}
		this._progress = 0.96f;
		yield return this._progress;
		yield break;
	}

	// Token: 0x06004BD3 RID: 19411 RVA: 0x001B4AF8 File Offset: 0x001B2CF8
	private void SetUpPhoton(HiddenSettings settings)
	{
		string text = Switcher.SelectPhotonAppId(settings);
		if (Defs.IsDeveloperBuild)
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			dictionary.Add("appId", text);
			dictionary.Add("defaultAppId", PhotonNetwork.PhotonServerSettings.AppID);
		}
		PhotonNetwork.PhotonServerSettings.AppID = text;
	}

	// Token: 0x06004BD4 RID: 19412 RVA: 0x001B4B4C File Offset: 0x001B2D4C
	private static string SelectPhotonAppId(HiddenSettings settings)
	{
		byte[] bytes = Convert.FromBase64String(settings.PhotonAppIdSignaturePad);
		byte[] array = Convert.FromBase64String(settings.PhotonAppIdSignatureEncoded);
		byte[] signatureHash = AndroidSystem.Instance.GetSignatureHash();
		byte[] bytes2 = Enumerable.Repeat<byte[]>(signatureHash, int.MaxValue).SelectMany((byte[] bs) => bs).Take(array.Length).ToArray<byte>();
		byte[] array2 = new byte[36];
		new BitArray(bytes).Xor(new BitArray(array)).Xor(new BitArray(bytes2)).CopyTo(array2, 0);
		return Encoding.UTF8.GetString(array2, 0, array2.Length);
	}

	// Token: 0x06004BD5 RID: 19413 RVA: 0x001B4BFC File Offset: 0x001B2DFC
	public static void PlayComicsSound()
	{
		if (Switcher.comicsSound != null)
		{
			return;
		}
		GameObject gameObject = Resources.Load<GameObject>("BackgroundMusic/Background_Comics");
		if (gameObject == null)
		{
			UnityEngine.Debug.LogWarning("ComicsSoundPrefab is null.");
			return;
		}
		Switcher.comicsSound = UnityEngine.Object.Instantiate<GameObject>(gameObject);
		UnityEngine.Object.DontDestroyOnLoad(Switcher.comicsSound);
	}

	// Token: 0x06004BD6 RID: 19414 RVA: 0x001B4C54 File Offset: 0x001B2E54
	private static void CheckHugeUpgrade()
	{
		bool flag = Storager.hasKey("Coins");
		bool flag2 = Storager.hasKey(Defs.ArmorNewEquppedSN);
		if (flag && !flag2)
		{
			Switcher.AppendAbuseMethod(AbuseMetod.UpgradeFromVulnerableVersion);
			UnityEngine.Debug.LogError("Upgrade tampering detected: " + Switcher.AbuseMethod);
		}
	}

	// Token: 0x06004BD7 RID: 19415 RVA: 0x001B4CA4 File Offset: 0x001B2EA4
	private static void PerformEssentialInitialization(string currencyKey, AbuseMetod abuseMethod)
	{
		if (Storager.hasKey(currencyKey))
		{
			int @int = Storager.getInt(currencyKey, false);
			if (DigestStorager.Instance.ContainsKey(currencyKey))
			{
				if (!DigestStorager.Instance.Verify(currencyKey, @int))
				{
					Switcher.AppendAbuseMethod(abuseMethod);
					UnityEngine.Debug.LogError("Currency tampering detected: " + Switcher.AbuseMethod);
				}
			}
			else
			{
				DigestStorager.Instance.Set(currencyKey, @int);
			}
		}
	}

	// Token: 0x06004BD8 RID: 19416 RVA: 0x001B4D18 File Offset: 0x001B2F18
	[Obsolete("Because of issues with CryptoPlayerPrefs")]
	private static void PerformWeaponInitialization()
	{
		IEnumerable<string> source = from w in WeaponManager.storeIDtoDefsSNMapping.Values
		where Storager.getInt(w, false) == 1
		select w;
		int value = source.Count<string>();
		if (DigestStorager.Instance.ContainsKey("WeaponsCount"))
		{
			if (!DigestStorager.Instance.Verify("WeaponsCount", value))
			{
				Switcher.AppendAbuseMethod(AbuseMetod.Weapons);
				UnityEngine.Debug.LogError("Weapon tampering detected: " + Switcher.AbuseMethod);
			}
		}
		else
		{
			DigestStorager.Instance.Set("WeaponsCount", value);
		}
	}

	// Token: 0x06004BD9 RID: 19417 RVA: 0x001B4DBC File Offset: 0x001B2FBC
	private static void PerformExpendablesInitialization()
	{
		string[] source = new string[]
		{
			GearManager.InvisibilityPotion,
			GearManager.Jetpack,
			GearManager.Turret,
			GearManager.Mech
		};
		byte[] value = source.SelectMany((string key) => BitConverter.GetBytes(Storager.getInt(key, false))).ToArray<byte>();
		if (DigestStorager.Instance.ContainsKey("ExpendablesCount"))
		{
			if (!DigestStorager.Instance.Verify("ExpendablesCount", value))
			{
				Switcher.AppendAbuseMethod(AbuseMetod.Expendables);
				UnityEngine.Debug.LogError("Expendables tampering detected: " + Switcher.AbuseMethod);
			}
		}
		else
		{
			DigestStorager.Instance.Set("ExpendablesCount", value);
		}
	}

	// Token: 0x06004BDA RID: 19418 RVA: 0x001B4E78 File Offset: 0x001B3078
	private static void ClearProgress()
	{
	}

	// Token: 0x06004BDB RID: 19419 RVA: 0x001B4E7C File Offset: 0x001B307C
	public IEnumerable<float> LoadMainMenu(bool armyArmor1ComesFromCloud = false)
	{
		using (new ScopeLogger("Switcher.LoadMainMenu()", Defs.IsDeveloperBuild))
		{
			if ((ExperienceController.sharedController != null && ExperienceController.sharedController.currentLevel > 1) || armyArmor1ComesFromCloud)
			{
				if (!TrainingController.TrainingCompleted)
				{
					TrainingController.OnGetProgress();
				}
				else if (Storager.getInt("Training.ShouldRemoveNoviceArmorInShopKey", false) == 1 && armyArmor1ComesFromCloud)
				{
					if (ShopNGUIController.NoviceArmorAvailable)
					{
						ShopNGUIController.UnequipCurrentWearInCategory(ShopNGUIController.CategoryNames.ArmorCategory, false);
						ShopNGUIController.ProvideItem(ShopNGUIController.CategoryNames.ArmorCategory, "Armor_Army_1", 1, false, 0, null, null, true, false, false);
					}
					Storager.setInt("Training.ShouldRemoveNoviceArmorInShopKey", 0, false);
				}
			}
			if (!TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage == TrainingController.NewTrainingCompletedStage.None)
			{
				Defs.isFlag = false;
				Defs.isCOOP = false;
				Defs.isMulti = false;
				Defs.isHunger = false;
				Defs.isCompany = false;
				Defs.IsSurvival = false;
				Defs.isCapturePoints = false;
				GlobalGameController.Score = 0;
				WeaponManager.sharedManager.CurrentWeaponIndex = WeaponManager.sharedManager.playerWeapons.OfType<Weapon>().ToList<Weapon>().FindIndex((Weapon w) => w.weaponPrefab.GetComponent<WeaponSounds>().categoryNabor - 1 == 1);
			}
			string sceneName = (TrainingController.TrainingCompleted || TrainingController.CompletedTrainingStage != TrainingController.NewTrainingCompletedStage.None) ? Switcher.DetermineSceneName() : Defs.TrainingSceneName;
			this._progress = 0.96f;
			yield return this._progress;
			AsyncOperation loadLevelTask = Singleton<SceneLoader>.Instance.LoadSceneAsync(sceneName, LoadSceneMode.Single);
			CoroutineRunner.Instance.StartCoroutine(this.WaitLoadSceneAsyncOperation(loadLevelTask, sceneName, 0.96f));
		}
		yield break;
	}

	// Token: 0x06004BDC RID: 19420 RVA: 0x001B4EB0 File Offset: 0x001B30B0
	private IEnumerator WaitLoadSceneAsyncOperation(AsyncOperation loadSceneAsyncOperation, string sceneName, float leftBound)
	{
		using (new ScopeLogger("Switcher.WaitLoadSceneAsyncOperation(): " + sceneName, Defs.IsDeveloperBuild))
		{
			while (!loadSceneAsyncOperation.isDone)
			{
				this._progress = leftBound + loadSceneAsyncOperation.progress / 50f;
				yield return this._progress;
			}
		}
		yield break;
	}

	// Token: 0x06004BDD RID: 19421 RVA: 0x001B4EF8 File Offset: 0x001B30F8
	private static bool IsWeaponBought(string weaponTag)
	{
		string text;
		string text2;
		return WeaponManager.tagToStoreIDMapping.TryGetValue(weaponTag, out text) && text != null && WeaponManager.storeIDtoDefsSNMapping.TryGetValue(text, out text2) && text2 != null && Storager.hasKey(text2) && Storager.getInt(text2, true) > 0;
	}

	// Token: 0x06004BDE RID: 19422 RVA: 0x001B4F50 File Offset: 0x001B3150
	private static void CountMoneyForRemovedGear()
	{
		Storager.hasKey(Defs.GemsGivenRemovedGear);
		if (Storager.getInt(Defs.GemsGivenRemovedGear, false) != 0)
		{
			return;
		}
		int num = 0;
		Dictionary<string, int> dictionary = new Dictionary<string, int>
		{
			{
				GearManager.Turret,
				5
			},
			{
				GearManager.Mech,
				7
			},
			{
				GearManager.InvisibilityPotion,
				3
			},
			{
				GearManager.Jetpack,
				4
			}
		};
		foreach (string key in dictionary.Keys)
		{
			num += Storager.getInt(key, false) * dictionary[key];
		}
		Storager.setInt(Defs.GemsGivenRemovedGear, 1, false);
		foreach (string key2 in dictionary.Keys)
		{
			Storager.setInt(key2, 0, false);
		}
	}

	// Token: 0x06004BDF RID: 19423 RVA: 0x001B508C File Offset: 0x001B328C
	private static void CountMoneyForGunsFrom831To901()
	{
		Storager.hasKey(Defs.MoneyGiven831to901);
		Storager.SyncWithCloud(Defs.MoneyGiven831to901);
		Storager.hasKey(Defs.Weapons831to901);
		if (Storager.getInt(Defs.Weapons831to901, false) != 0)
		{
			return;
		}
		bool flag = Storager.getInt(Defs.MoneyGiven831to901, true) == 0;
		int num = 0;
		int num2 = 0;
		if (flag)
		{
			Dictionary<string, int> dictionary = new Dictionary<string, int>
			{
				{
					WeaponTags.CrossbowTag,
					120
				},
				{
					WeaponTags.CrystalCrossbowTag,
					155
				},
				{
					WeaponTags.SteelCrossbowTag,
					120
				},
				{
					WeaponTags.Bow_3_Tag,
					185
				},
				{
					WeaponTags.WoodenBowTag,
					100
				},
				{
					WeaponTags.Staff2Tag,
					200
				},
				{
					WeaponTags.Staff_3_Tag,
					235
				}
			};
			foreach (KeyValuePair<string, int> keyValuePair in dictionary)
			{
				string key = keyValuePair.Key;
				int value = keyValuePair.Value;
				if (Switcher.IsWeaponBought(key))
				{
					num += value;
				}
			}
			Dictionary<string, int> dictionary2 = new Dictionary<string, int>
			{
				{
					WeaponTags.AutoShotgun_Tag,
					255
				},
				{
					WeaponTags.TwoRevolvers_Tag,
					267
				},
				{
					WeaponTags.TwoBolters_Tag,
					249
				},
				{
					WeaponTags.SnowballGun_Tag,
					281
				}
			};
			foreach (KeyValuePair<string, int> keyValuePair2 in dictionary2)
			{
				string key2 = keyValuePair2.Key;
				int value2 = keyValuePair2.Value;
				if (Switcher.IsWeaponBought(key2))
				{
					num2 += value2;
				}
			}
			Dictionary<string, int> dictionary3 = new Dictionary<string, int>
			{
				{
					"cape_EliteCrafter",
					50
				},
				{
					"cape_Archimage",
					65
				},
				{
					"cape_BloodyDemon",
					50
				},
				{
					"cape_SkeletonLord",
					75
				},
				{
					"cape_RoyalKnight",
					65
				}
			};
			foreach (KeyValuePair<string, int> keyValuePair3 in dictionary3)
			{
				string key3 = keyValuePair3.Key;
				int value3 = keyValuePair3.Value;
				if (Storager.hasKey(key3) && Storager.getInt(key3, false) != 0)
				{
					num += value3;
				}
			}
			Dictionary<string, int> dictionary4 = new Dictionary<string, int>
			{
				{
					"boots_gray",
					50
				},
				{
					"boots_red",
					50
				},
				{
					"boots_black",
					100
				},
				{
					"boots_blue",
					50
				},
				{
					"boots_green",
					75
				}
			};
			foreach (KeyValuePair<string, int> keyValuePair4 in dictionary4)
			{
				string key4 = keyValuePair4.Key;
				int value4 = keyValuePair4.Value;
				if (Storager.hasKey(key4) && Storager.getInt(key4, false) != 0)
				{
					num += value4;
				}
			}
		}
		Storager.setInt(Defs.Weapons831to901, 1, false);
		Storager.setInt(Defs.MoneyGiven831to901, 1, true);
	}

	// Token: 0x06004BE0 RID: 19424 RVA: 0x001B5454 File Offset: 0x001B3654
	private static void CountMoneyForArmorHats()
	{
		Storager.hasKey("MoneyGivenRemovedArmorHat");
		Storager.SyncWithCloud("MoneyGivenRemovedArmorHat");
		Storager.hasKey("RemovedArmorHatMethodExecuted");
		if (Storager.getInt("RemovedArmorHatMethodExecuted", false) != 0)
		{
			return;
		}
		bool flag = Storager.getInt("MoneyGivenRemovedArmorHat", true) == 0;
		int num = 0;
		if (flag)
		{
			foreach (string key in Wear.wear[ShopNGUIController.CategoryNames.HatsCategory][0])
			{
				if (Storager.getInt(key, true) > 0)
				{
					num += VirtualCurrencyHelper.Price(key).Price;
				}
			}
		}
		Storager.hasKey(Defs.HatEquppedSN);
		string item = Storager.getString(Defs.HatEquppedSN, false) ?? string.Empty;
		if (Wear.wear[ShopNGUIController.CategoryNames.HatsCategory][0].Contains(item))
		{
			Storager.setString(Defs.HatEquppedSN, ShopNGUIController.NoneEquippedForWearCategory(ShopNGUIController.CategoryNames.HatsCategory), false);
		}
		Storager.setInt("RemovedArmorHatMethodExecuted", 1, false);
		Storager.setInt("MoneyGivenRemovedArmorHat", 1, true);
	}

	// Token: 0x06004BE1 RID: 19425 RVA: 0x001B5594 File Offset: 0x001B3794
	public static float SecondsFrom1970()
	{
		DateTime d = new DateTime(1970, 1, 9, 0, 0, 0);
		DateTime now = DateTime.Now;
		return (float)(now - d).TotalSeconds;
	}

	// Token: 0x06004BE2 RID: 19426 RVA: 0x001B55CC File Offset: 0x001B37CC
	private void OnDestroy()
	{
		ActivityIndicator.IsShowWindowLoading = false;
	}

	// Token: 0x06004BE3 RID: 19427 RVA: 0x001B55D4 File Offset: 0x001B37D4
	private static string DetermineSceneName()
	{
		int currentLevel = GlobalGameController.currentLevel;
		switch (currentLevel + 1)
		{
		case 0:
			return Defs.MainMenuScene;
		case 1:
			return "Cementery";
		case 2:
			return "Maze";
		case 3:
			return "City";
		case 4:
			return "Hospital";
		case 5:
			return "Jail";
		case 6:
			return "Gluk_2";
		case 7:
			return "Arena";
		case 8:
			return "Area52";
		case 9:
			return "Slender";
		case 10:
			return "Castle";
		default:
			if (currentLevel != 101)
			{
				return Defs.MainMenuScene;
			}
			return "Training";
		}
	}

	// Token: 0x17000C74 RID: 3188
	// (get) Token: 0x06004BE4 RID: 19428 RVA: 0x001B5678 File Offset: 0x001B3878
	internal static AbuseMetod AbuseMethod
	{
		get
		{
			if (Switcher._abuseMethod == null)
			{
				Switcher._abuseMethod = new AbuseMetod?((AbuseMetod)Storager.getInt("AbuseMethod", false));
			}
			return Switcher._abuseMethod.Value;
		}
	}

	// Token: 0x06004BE5 RID: 19429 RVA: 0x001B56B4 File Offset: 0x001B38B4
	internal static void AppendAbuseMethod(AbuseMetod f)
	{
		Switcher._abuseMethod = new AbuseMetod?(Switcher.AbuseMethod | f);
		string key = "AbuseMethod";
		AbuseMetod? abuseMethod = Switcher._abuseMethod;
		Storager.setInt(key, (int)abuseMethod.Value, false);
	}

	// Token: 0x04003AF0 RID: 15088
	internal const string AbuseMethodKey = "AbuseMethod";

	// Token: 0x04003AF1 RID: 15089
	public static Dictionary<string, int> sceneNameToGameNum = new Dictionary<string, int>();

	// Token: 0x04003AF2 RID: 15090
	public static Dictionary<string, int> counCreateMobsInLevel = new Dictionary<string, int>();

	// Token: 0x04003AF3 RID: 15091
	public static string LoadingInResourcesPath = "LevelLoadings";

	// Token: 0x04003AF4 RID: 15092
	public static string[] loadingNames = new string[]
	{
		"Loading_coliseum",
		"loading_Cementery",
		"Loading_Maze",
		"Loading_City",
		"Loading_Hospital",
		"Loading_Jail",
		"Loading_end_world_2",
		"Loading_Arena",
		"Loading_Area52",
		"Loading_Slender",
		"Loading_Hell",
		"Loading_bloody_farm",
		"Loading_most",
		"Loading_school",
		"Loading_utopia",
		"Loading_sky",
		"Loading_winter"
	};

	// Token: 0x04003AF5 RID: 15093
	public GameObject balanceControllerPrefab;

	// Token: 0x04003AF6 RID: 15094
	public GameObject coinsShopPrefab;

	// Token: 0x04003AF7 RID: 15095
	public GameObject nickStackPrefab;

	// Token: 0x04003AF8 RID: 15096
	public GameObject skinsManagerPrefab;

	// Token: 0x04003AF9 RID: 15097
	public GameObject ExperienceControllerPrefab;

	// Token: 0x04003AFA RID: 15098
	public GameObject weaponManagerPrefab;

	// Token: 0x04003AFB RID: 15099
	public GameObject experienceGuiPrefab;

	// Token: 0x04003AFC RID: 15100
	public GameObject bankGuiPrefab;

	// Token: 0x04003AFD RID: 15101
	public GameObject freeAwardGuiPrefab;

	// Token: 0x04003AFE RID: 15102
	public GameObject buttonClickSoundPrefab;

	// Token: 0x04003AFF RID: 15103
	public GameObject faceBookControllerPrefab;

	// Token: 0x04003B00 RID: 15104
	public GameObject licenseVerificationControllerPrefab;

	// Token: 0x04003B01 RID: 15105
	public GameObject potionsControllerPrefab;

	// Token: 0x04003B02 RID: 15106
	public GameObject protocolListGetterPrefab;

	// Token: 0x04003B03 RID: 15107
	public GameObject updateCheckerPrefab;

	// Token: 0x04003B04 RID: 15108
	public GameObject promoActionsManagerPrefab;

	// Token: 0x04003B05 RID: 15109
	public GameObject starterPackManagerPrefab;

	// Token: 0x04003B06 RID: 15110
	public GameObject tempItemsControllerPrefab;

	// Token: 0x04003B07 RID: 15111
	public GameObject remotePushNotificationControllerPrefab;

	// Token: 0x04003B08 RID: 15112
	public GameObject premiumAccountControllerPrefab;

	// Token: 0x04003B09 RID: 15113
	public GameObject twitterControllerPrefab;

	// Token: 0x04003B0A RID: 15114
	public GameObject sponsorPayPluginHolderPrefab;

	// Token: 0x04003B0B RID: 15115
	public GameObject giftControllerPrefab;

	// Token: 0x04003B0C RID: 15116
	public GameObject disabler;

	// Token: 0x04003B0D RID: 15117
	public GameObject sceneInfoController;

	// Token: 0x04003B0E RID: 15118
	private Rect plashkaCoinsRect;

	// Token: 0x04003B0F RID: 15119
	private Texture fonToDraw;

	// Token: 0x04003B10 RID: 15120
	private bool _newLaunchingApproach;

	// Token: 0x04003B11 RID: 15121
	public static Stopwatch timer = new Stopwatch();

	// Token: 0x04003B12 RID: 15122
	private static bool _initialAppVersionInitialized = false;

	// Token: 0x04003B13 RID: 15123
	private static string _InitialAppVersion = string.Empty;

	// Token: 0x04003B14 RID: 15124
	public static GameObject comicsSound;

	// Token: 0x04003B15 RID: 15125
	private float _progress;

	// Token: 0x04003B16 RID: 15126
	private float oldProgress;

	// Token: 0x04003B17 RID: 15127
	private static AbuseMetod? _abuseMethod;
}
