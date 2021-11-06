using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Rilisoft;
using Rilisoft.MiniJson;
using UnityEngine;

// Token: 0x020000B2 RID: 178
public sealed class ExperienceController : MonoBehaviour
{
	// Token: 0x06000534 RID: 1332 RVA: 0x0002A1BC File Offset: 0x000283BC
	public ExperienceController()
	{
		this.currentLevel = 1;
	}

	// Token: 0x1400000A RID: 10
	// (add) Token: 0x06000536 RID: 1334 RVA: 0x0002A2C8 File Offset: 0x000284C8
	// (remove) Token: 0x06000537 RID: 1335 RVA: 0x0002A2E0 File Offset: 0x000284E0
	public static event Action onLevelChange;

	// Token: 0x06000538 RID: 1336 RVA: 0x0002A2F8 File Offset: 0x000284F8
	public static int[] InitMaxLevelMass(int[] _mass)
	{
		int[] array = new int[_mass.Length];
		Array.Copy(_mass, array, _mass.Length);
		return array;
	}

	// Token: 0x17000054 RID: 84
	// (get) Token: 0x06000539 RID: 1337 RVA: 0x0002A31C File Offset: 0x0002851C
	public int AddHealthOnCurLevel
	{
		get
		{
			int currentLevel = this.currentLevel;
			if (ExperienceController.HealthByLevel.Length > currentLevel && currentLevel > 0)
			{
				return (int)(ExperienceController.HealthByLevel[currentLevel] - ExperienceController.HealthByLevel[currentLevel - 1]);
			}
			return 0;
		}
	}

	// Token: 0x17000055 RID: 85
	// (get) Token: 0x0600053A RID: 1338 RVA: 0x0002A358 File Offset: 0x00028558
	// (set) Token: 0x0600053B RID: 1339 RVA: 0x0002A360 File Offset: 0x00028560
	public int currentLevel
	{
		get
		{
			return this.currentLevelForEditor;
		}
		private set
		{
			bool flag = false;
			if (this.currentLevelForEditor != value)
			{
				flag = true;
			}
			this.currentLevelForEditor = value;
			if (value >= 4)
			{
				ReviewController.CheckActiveReview();
			}
			if (flag && ExperienceController.onLevelChange != null)
			{
				ExperienceController.onLevelChange();
			}
		}
	}

	// Token: 0x0600053C RID: 1340 RVA: 0x0002A3AC File Offset: 0x000285AC
	public static int[] InitAddCoinsFromLevels(int[] _mass)
	{
		int[] array = new int[_mass.Length];
		Array.Copy(_mass, array, _mass.Length);
		return array;
	}

	// Token: 0x17000056 RID: 86
	// (get) Token: 0x0600053D RID: 1341 RVA: 0x0002A3D0 File Offset: 0x000285D0
	public static int[] addCoinsFromLevels
	{
		get
		{
			return ExperienceController._addCoinsFromLevels;
		}
	}

	// Token: 0x0600053E RID: 1342 RVA: 0x0002A3D8 File Offset: 0x000285D8
	public static int[] InitAddGemsFromLevels(int[] _mass)
	{
		int[] array = new int[_mass.Length];
		Array.Copy(_mass, array, _mass.Length);
		return array;
	}

	// Token: 0x17000057 RID: 87
	// (get) Token: 0x0600053F RID: 1343 RVA: 0x0002A3FC File Offset: 0x000285FC
	public static int[] addGemsFromLevels
	{
		get
		{
			return ExperienceController._addGemsFromLevels;
		}
	}

	// Token: 0x06000540 RID: 1344 RVA: 0x0002A404 File Offset: 0x00028604
	public static void ResetLevelingOnDefault()
	{
		ExperienceController.MaxExpLevels = ExperienceController.InitMaxLevelMass(ExperienceController.MaxExpLevelsDefault);
		ExperienceController._addCoinsFromLevels = ExperienceController.InitAddCoinsFromLevels(ExperienceController._addCoinsFromLevelsDefault);
		ExperienceController._addGemsFromLevels = ExperienceController.InitAddGemsFromLevels(ExperienceController._addGemsFromLevelsDefault);
	}

	// Token: 0x06000541 RID: 1345 RVA: 0x0002A434 File Offset: 0x00028634
	public static void RewriteLevelingParametersForLevel(int _level, int _exp, int _coins, int _gems)
	{
		ExperienceController.MaxExpLevels[_level] = _exp;
		ExperienceController._addCoinsFromLevels[_level] = _coins;
		ExperienceController._addGemsFromLevels[_level] = _gems;
	}

	// Token: 0x06000542 RID: 1346 RVA: 0x0002A450 File Offset: 0x00028650
	public static void RewriteLevelingParametersForLevel(int _level, int _coins, int _gems)
	{
		ExperienceController._addCoinsFromLevels[_level] = _coins;
		ExperienceController._addGemsFromLevels[_level] = _gems;
	}

	// Token: 0x17000058 RID: 88
	// (get) Token: 0x06000543 RID: 1347 RVA: 0x0002A464 File Offset: 0x00028664
	public int CurrentExperience
	{
		get
		{
			return this.currentExperience.Value;
		}
	}

	// Token: 0x06000544 RID: 1348 RVA: 0x0002A474 File Offset: 0x00028674
	public void SetCurrentExperience(int _exp)
	{
		this.currentExperience = _exp;
		Storager.setInt("currentExperience", _exp, false);
		Debug.Log(this.currentExperience.Value);
	}

	// Token: 0x06000545 RID: 1349 RVA: 0x0002A4A4 File Offset: 0x000286A4
	private static void InitializeStoragerKeysIfNeeded()
	{
		if (ExperienceController._storagerKeysInitialized)
		{
			return;
		}
		if (!Storager.hasKey("currentLevel1"))
		{
			Storager.setInt("currentLevel1", 1, true);
		}
		ExperienceController._storagerKeysInitialized = true;
	}

	// Token: 0x06000546 RID: 1350 RVA: 0x0002A4E0 File Offset: 0x000286E0
	public static int GetCurrentLevelWithUpdateCorrection()
	{
		ExperienceController.InitializeStoragerKeysIfNeeded();
		int num = ExperienceController.GetCurrentLevel();
		if (num < 31 && Storager.getInt("currentExperience", false) >= ExperienceController.MaxExpLevels[num])
		{
			num++;
		}
		return num;
	}

	// Token: 0x06000547 RID: 1351 RVA: 0x0002A51C File Offset: 0x0002871C
	public static int GetCurrentLevel()
	{
		int result = 1;
		for (int i = 1; i <= 31; i++)
		{
			string currentLevelKey = ExperienceController.GetCurrentLevelKey(i);
			if (Storager.getInt(currentLevelKey, true) == 1)
			{
				result = i;
				Storager.setInt(currentLevelKey, 1, true);
			}
		}
		return result;
	}

	// Token: 0x06000548 RID: 1352 RVA: 0x0002A560 File Offset: 0x00028760
	public void Refresh()
	{
		this.currentExperience = Storager.getInt("currentExperience", false);
		this.currentLevel = ExperienceController.GetCurrentLevel();
	}

	// Token: 0x17000059 RID: 89
	// (get) Token: 0x06000549 RID: 1353 RVA: 0x0002A584 File Offset: 0x00028784
	// (set) Token: 0x0600054A RID: 1354 RVA: 0x0002A58C File Offset: 0x0002878C
	public bool isShowRanks
	{
		get
		{
			return this._isShowRanks;
		}
		set
		{
			this._isShowRanks = value;
			if (ExpController.Instance != null)
			{
				ExpController.Instance.InterfaceEnabled = value;
			}
		}
	}

	// Token: 0x0600054B RID: 1355 RVA: 0x0002A5BC File Offset: 0x000287BC
	private void AddCurrenciesForLevelUP()
	{
		int count = ExperienceController.addGemsFromLevels[this.currentLevel - 1];
		BankController.canShowIndication = false;
		BankController.AddGems(count, false, AnalyticsConstants.AccrualType.Earned);
		if (this.currentLevel == 2 && BalanceController.startCapitalEnabled)
		{
			int startCapitalGems = BalanceController.startCapitalGems;
			BankController.AddGems(startCapitalGems, false, AnalyticsConstants.AccrualType.Earned);
		}
		base.StartCoroutine(BankController.WaitForIndicationGems(true));
		int count2 = ExperienceController.addCoinsFromLevels[this.currentLevel - 1];
		BankController.AddCoins(count2, false, AnalyticsConstants.AccrualType.Earned);
		if (this.currentLevel == 2 && BalanceController.startCapitalEnabled)
		{
			int startCapitalCoins = BalanceController.startCapitalCoins;
			BankController.AddCoins(startCapitalCoins, false, AnalyticsConstants.AccrualType.Earned);
		}
		base.StartCoroutine(BankController.WaitForIndicationGems(false));
	}

	// Token: 0x0600054C RID: 1356 RVA: 0x0002A660 File Offset: 0x00028860
	private void Awake()
	{
		ExperienceController.sharedController = this;
		CoroutineRunner.Instance.StartCoroutine(this.WaitInitializationThenLogProgressInExperience());
	}

	// Token: 0x0600054D RID: 1357 RVA: 0x0002A67C File Offset: 0x0002887C
	private IEnumerator WaitInitializationThenLogProgressInExperience()
	{
		while (!this._initialized)
		{
			yield return null;
		}
		while (AnalyticsFacade.FlurryFacade == null)
		{
			yield return null;
		}
		int levelBase = this.currentLevel;
		int tierBase = ExpController.OurTierForAnyPlace() + 1;
		AnalyticsStuff.LogProgressInExperience(levelBase, tierBase);
		yield break;
	}

	// Token: 0x0600054E RID: 1358 RVA: 0x0002A698 File Offset: 0x00028898
	public IEnumerable<float> InitController()
	{
		for (int i = 0; i < 31; i++)
		{
			for (int d = 0; d < 31; d++)
			{
				ExperienceController.accessByLevels[i, d] = 0;
			}
		}
		for (int j = 0; j < 31; j++)
		{
			for (int k = this.limitsLeveling.GetLowerBound(0); k <= this.limitsLeveling.GetUpperBound(0); k++)
			{
				int min = this.limitsLeveling[k, 0] - 1;
				int max = this.limitsLeveling[k, 1] - 1;
				if (j >= min && j <= max)
				{
					for (int d2 = min; d2 <= max; d2++)
					{
						ExperienceController.accessByLevels[j, d2] = 1;
					}
					break;
				}
			}
		}
		yield return 0f;
		try
		{
			ExperienceController.InitializeStoragerKeysIfNeeded();
			UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
			this.Refresh();
			if (this.currentLevel < 31 && this.currentExperience.Value >= ExperienceController.MaxExpLevels[this.currentLevel])
			{
				this.currentExperience = 0;
				this.currentLevel++;
				Storager.setInt(ExperienceController.GetCurrentLevelKey(this.currentLevel), 1, true);
				Storager.setInt("currentExperience", this.currentExperience.Value, false);
				this.AddCurrenciesForLevelUP();
				BankController.canShowIndication = true;
				AnalyticsFacade.LevelUp(this.currentLevel);
			}
			this.isShowRanks = false;
		}
		catch (Exception ex2)
		{
			Exception ex = ex2;
			Debug.LogError("<<< ExperienceController.Start() failed.");
			Debug.LogException(ex);
			yield break;
		}
		finally
		{
			this._initialized = true;
		}
		yield break;
	}

	// Token: 0x0600054F RID: 1359 RVA: 0x0002A6BC File Offset: 0x000288BC
	public static void SendAnalyticsForLevelsFromCloud(int levelBefore)
	{
		if (ExperienceController.sharedController == null)
		{
			Debug.LogError("SendAnalyticsForLevelsFromCloud ExperienceController.sharedController == null");
			return;
		}
		for (int i = levelBefore + 1; i <= ExperienceController.sharedController.currentLevel; i++)
		{
			AnalyticsFacade.LevelUp(i);
		}
	}

	// Token: 0x06000550 RID: 1360 RVA: 0x0002A708 File Offset: 0x00028908
	public void addExperience(int experience)
	{
		if (this.currentLevel == 31)
		{
			return;
		}
		this.oldCurrentLevel = this.currentLevel;
		this.oldCurrentExperience = this.currentExperience.Value;
		if (this.currentLevel < 31 && experience >= ExperienceController.MaxExpLevels[this.currentLevel] - this.currentExperience.Value + ExperienceController.MaxExpLevels[this.currentLevel + 1])
		{
			experience = ExperienceController.MaxExpLevels[this.currentLevel + 1] + ExperienceController.MaxExpLevels[this.currentLevel] - this.currentExperience.Value - 5;
		}
		string key = "Statistics.ExpInMode.Level" + ExperienceController.sharedController.currentLevel;
		if (PlayerPrefs.HasKey(key) && Initializer.lastGameMode != -1)
		{
			string key2 = Initializer.lastGameMode.ToString();
			string @string = PlayerPrefs.GetString(key, "{}");
			try
			{
				Dictionary<string, object> dictionary = (Json.Deserialize(@string) as Dictionary<string, object>) ?? new Dictionary<string, object>();
				object value;
				if (dictionary.TryGetValue(key2, out value))
				{
					int num = Convert.ToInt32(value) + experience;
					dictionary[key2] = num;
				}
				else
				{
					dictionary.Add(key2, experience);
				}
				string value2 = Json.Serialize(dictionary);
				PlayerPrefs.SetString(key, value2);
			}
			catch (OverflowException exception)
			{
				Debug.LogError("Cannot deserialize exp-in-mode: " + @string);
				Debug.LogException(exception);
			}
			catch (Exception exception2)
			{
				Debug.LogError("Unknown exception: " + @string);
				Debug.LogException(exception2);
			}
		}
		this.currentExperience = this.currentExperience.Value + experience;
		Storager.setInt("currentExperience", this.currentExperience.Value, false);
		if (this.currentLevel < 31 && this.currentExperience.Value >= ExperienceController.MaxExpLevels[this.currentLevel])
		{
			DateTime utcNow = DateTime.UtcNow;
			string key3 = "Statistics.TimeInRank.Level" + (this.currentLevel + 1);
			PlayerPrefs.SetString(key3, utcNow.ToString("s"));
			string key4 = "Statistics.MatchCount.Level" + ExperienceController.sharedController.currentLevel;
			int @int = PlayerPrefs.GetInt(key4, 0);
			string key5 = "Statistics.WinCount.Level" + ExperienceController.sharedController.currentLevel;
			int int2 = PlayerPrefs.GetInt(key5, 0);
			AnalyticsFacade.SendCustomEventToAppsFlyer("af_level_achieved", new Dictionary<string, string>
			{
				{
					"af_level",
					this.currentLevel.ToString()
				}
			});
			AnalyticsFacade.SendCustomEventToFacebook("level_reached", new Dictionary<string, object>
			{
				{
					"level",
					this.currentLevel
				}
			});
			this.currentExperience = this.currentExperience.Value - ExperienceController.MaxExpLevels[this.currentLevel];
			this.currentLevel++;
			PlayerPrefs.SetString(LeaderboardScript.LeaderboardsResponseCacheTimestamp, DateTime.UtcNow.Subtract(TimeSpan.FromHours(2.0)).ToString("s", CultureInfo.InvariantCulture));
			if (this.currentLevel == 3)
			{
				AnalyticsStuff.TrySendOnceToAppsFlyer("levelup_3");
			}
			if (!TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage == TrainingController.NewTrainingCompletedStage.ShopCompleted && this.currentLevel > 1)
			{
				if (Storager.getInt("Training.NoviceArmorUsedKey", false) == 1)
				{
					Storager.setInt("Training.ShouldRemoveNoviceArmorInShopKey", 1, false);
					if (HintController.instance != null)
					{
						HintController.instance.ShowHintByName("shop_remove_novice_armor", 2.5f);
					}
				}
				TrainingController.CompletedTrainingStage = TrainingController.NewTrainingCompletedStage.FirstMatchCompleted;
				AnalyticsStuff.Tutorial(AnalyticsConstants.TutorialState.Finished, 0);
				if (!Storager.hasKey("Analytics:tutorial_levelup"))
				{
					Storager.setString("Analytics:tutorial_levelup", "{}", false);
					AnalyticsFacade.SendCustomEventToAppsFlyer("tutorial_levelup", new Dictionary<string, string>());
					Storager.setString("Analytics:af_tutorial_completion", "{}", false);
					AnalyticsFacade.SendCustomEventToAppsFlyer("af_tutorial_completion", new Dictionary<string, string>());
				}
				try
				{
					if (WeaponManager.sharedManager != null && WeaponManager.sharedManager.playerWeapons != null)
					{
						IEnumerable<Weapon> source = WeaponManager.sharedManager.playerWeapons.OfType<Weapon>();
						if (source.FirstOrDefault((Weapon w) => w.weaponPrefab.GetComponent<WeaponSounds>().categoryNabor - 1 == 3) == null)
						{
							WeaponManager.sharedManager.EquipWeapon(WeaponManager.sharedManager.allAvailablePlayerWeapons.OfType<Weapon>().First((Weapon w) => w.weaponPrefab.name.Replace("(Clone)", string.Empty) == WeaponManager.SimpleFlamethrower_WN), true, false);
						}
						if (source.FirstOrDefault((Weapon w) => w.weaponPrefab.GetComponent<WeaponSounds>().categoryNabor - 1 == 5) == null)
						{
							WeaponManager.sharedManager.EquipWeapon(WeaponManager.sharedManager.allAvailablePlayerWeapons.OfType<Weapon>().First((Weapon w) => w.weaponPrefab.name.Replace("(Clone)", string.Empty) == WeaponManager.Rocketnitza_WN), true, false);
						}
					}
				}
				catch (Exception arg)
				{
					Debug.LogError("Exception in gequipping flamethrower and rocketniza: " + arg);
				}
			}
			if (this.currentLevel == 2)
			{
				try
				{
					EggData eggData = Singleton<EggsManager>.Instance.GetEggData("egg_champion_steel");
					Singleton<EggsManager>.Instance.AddEgg(eggData);
				}
				catch (Exception ex)
				{
					Debug.LogErrorFormat("Exception in giving after-training steel league egg: {0}", new object[]
					{
						ex
					});
				}
			}
			Storager.setInt(ExperienceController.GetCurrentLevelKey(this.currentLevel), 1, true);
			Storager.setInt("currentExperience", this.currentExperience.Value, false);
			ShopNGUIController.SynchronizeAndroidPurchases("Current level: " + this.currentLevel);
			this.AddCurrenciesForLevelUP();
			FriendsController.sharedController.rank = this.currentLevel;
			FriendsController.sharedController.SendOurData(false);
			FriendsController.sharedController.UpdatePopularityMaps();
			AnalyticsFacade.LevelUp(this.currentLevel);
		}
		if (Defs.isSoundFX)
		{
			NGUITools.PlaySound(this.exp_1);
		}
		if (ExpController.Instance != null)
		{
			ExpController.Instance.AddExperience(this.oldCurrentLevel, this.oldCurrentExperience, experience, this.exp_2, this.exp_3, this.Tierup);
		}
	}

	// Token: 0x06000551 RID: 1361 RVA: 0x0002AD88 File Offset: 0x00028F88
	private void HideNextPlashka()
	{
		this.isShowNextPlashka = false;
		this.isShowAdd = false;
	}

	// Token: 0x06000552 RID: 1362 RVA: 0x0002AD98 File Offset: 0x00028F98
	private void DoOnGUI()
	{
	}

	// Token: 0x06000553 RID: 1363 RVA: 0x0002AD9C File Offset: 0x00028F9C
	public static void SetEnable(bool enable)
	{
		if (ExperienceController.sharedController == null)
		{
			return;
		}
		ExperienceController.sharedController.isShowRanks = enable;
	}

	// Token: 0x06000554 RID: 1364 RVA: 0x0002ADBC File Offset: 0x00028FBC
	private static string GetCurrentLevelKey(int levelIndex)
	{
		ExperienceController._sharedStringBuilder.Length = 0;
		ExperienceController._sharedStringBuilder.Append("currentLevel").Append(levelIndex);
		string result = ExperienceController._sharedStringBuilder.ToString();
		ExperienceController._sharedStringBuilder.Length = 0;
		return result;
	}

	// Token: 0x040005B0 RID: 1456
	public const int maxLevel = 31;

	// Token: 0x040005B1 RID: 1457
	public static int[] MaxExpLevelsDefault = new int[]
	{
		0,
		15,
		35,
		50,
		90,
		100,
		110,
		115,
		120,
		125,
		130,
		135,
		140,
		150,
		160,
		170,
		180,
		200,
		220,
		250,
		290,
		340,
		400,
		470,
		550,
		640,
		740,
		850,
		970,
		1100,
		1240,
		100000
	};

	// Token: 0x040005B2 RID: 1458
	public static int[] MaxExpLevels = ExperienceController.InitMaxLevelMass(ExperienceController.MaxExpLevelsDefault);

	// Token: 0x040005B3 RID: 1459
	public static readonly float[] HealthByLevel = new float[]
	{
		0f,
		9f,
		10f,
		10f,
		11f,
		11f,
		12f,
		13f,
		13f,
		14f,
		14f,
		15f,
		16f,
		16f,
		17f,
		17f,
		18f,
		19f,
		19f,
		20f,
		20f,
		21f,
		22f,
		22f,
		23f,
		23f,
		24f,
		25f,
		25f,
		26f,
		26f,
		27f
	};

	// Token: 0x040005B4 RID: 1460
	public bool isMenu;

	// Token: 0x040005B5 RID: 1461
	public bool isConnectScene;

	// Token: 0x040005B6 RID: 1462
	public int currentLevelForEditor;

	// Token: 0x040005B7 RID: 1463
	public int[,] limitsLeveling = new int[,]
	{
		{
			1,
			6
		},
		{
			7,
			11
		},
		{
			12,
			16
		},
		{
			17,
			21
		},
		{
			22,
			26
		},
		{
			27,
			31
		}
	};

	// Token: 0x040005B8 RID: 1464
	public static int[,] accessByLevels = new int[31, 31];

	// Token: 0x040005B9 RID: 1465
	public Texture2D[] marks;

	// Token: 0x040005BA RID: 1466
	private SaltedInt currentExperience = new SaltedInt(12512238, 0);

	// Token: 0x040005BB RID: 1467
	private static int[] _addCoinsFromLevelsDefault = new int[]
	{
		0,
		5,
		10,
		15,
		20,
		25,
		25,
		25,
		30,
		30,
		30,
		35,
		35,
		40,
		40,
		40,
		45,
		45,
		50,
		50,
		50,
		55,
		55,
		60,
		60,
		60,
		65,
		65,
		70,
		70,
		70,
		0
	};

	// Token: 0x040005BC RID: 1468
	private static int[] _addCoinsFromLevels = ExperienceController.InitAddCoinsFromLevels(ExperienceController._addCoinsFromLevelsDefault);

	// Token: 0x040005BD RID: 1469
	private static int[] _addGemsFromLevelsDefault = new int[]
	{
		0,
		4,
		4,
		5,
		5,
		6,
		6,
		7,
		7,
		8,
		8,
		9,
		9,
		10,
		10,
		11,
		11,
		12,
		12,
		13,
		13,
		14,
		14,
		15,
		15,
		16,
		16,
		17,
		17,
		18,
		18,
		0
	};

	// Token: 0x040005BE RID: 1470
	private static int[] _addGemsFromLevels = ExperienceController.InitAddGemsFromLevels(ExperienceController._addGemsFromLevelsDefault);

	// Token: 0x040005BF RID: 1471
	private static bool _storagerKeysInitialized = false;

	// Token: 0x040005C0 RID: 1472
	private bool _isShowRanks = true;

	// Token: 0x040005C1 RID: 1473
	public bool isShowNextPlashka;

	// Token: 0x040005C2 RID: 1474
	public Vector2 posRanks = Vector2.zero;

	// Token: 0x040005C3 RID: 1475
	private int oldCurrentExperience;

	// Token: 0x040005C4 RID: 1476
	public int oldCurrentLevel;

	// Token: 0x040005C5 RID: 1477
	public bool isShowAdd;

	// Token: 0x040005C6 RID: 1478
	public AudioClip exp_1;

	// Token: 0x040005C7 RID: 1479
	public AudioClip exp_2;

	// Token: 0x040005C8 RID: 1480
	public AudioClip exp_3;

	// Token: 0x040005C9 RID: 1481
	public AudioClip Tierup;

	// Token: 0x040005CA RID: 1482
	public static ExperienceController sharedController;

	// Token: 0x040005CB RID: 1483
	private bool _initialized;

	// Token: 0x040005CC RID: 1484
	private static readonly StringBuilder _sharedStringBuilder = new StringBuilder();
}
