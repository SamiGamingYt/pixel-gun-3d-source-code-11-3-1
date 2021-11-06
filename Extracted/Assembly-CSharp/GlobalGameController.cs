using System;
using System.Collections.Generic;
using Rilisoft;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x02000650 RID: 1616
public class GlobalGameController
{
	// Token: 0x1700092F RID: 2351
	// (get) Token: 0x06003823 RID: 14371 RVA: 0x0012208C File Offset: 0x0012028C
	// (set) Token: 0x06003824 RID: 14372 RVA: 0x00122094 File Offset: 0x00120294
	public static float armorMyPlayer { get; set; }

	// Token: 0x17000930 RID: 2352
	// (get) Token: 0x06003825 RID: 14373 RVA: 0x0012209C File Offset: 0x0012029C
	// (set) Token: 0x06003826 RID: 14374 RVA: 0x001220A4 File Offset: 0x001202A4
	public static int currentLevel
	{
		get
		{
			return GlobalGameController._currentLevel;
		}
		set
		{
			GlobalGameController._currentLevel = value;
		}
	}

	// Token: 0x06003827 RID: 14375 RVA: 0x001220AC File Offset: 0x001202AC
	public static void SetMultiMode()
	{
		Defs.isMulti = true;
		Defs.isCOOP = false;
		Defs.isHunger = false;
		Defs.isDuel = false;
		Defs.isCompany = false;
		Defs.isFlag = false;
		Defs.IsSurvival = false;
		Defs.isCapturePoints = false;
	}

	// Token: 0x17000931 RID: 2353
	// (get) Token: 0x06003828 RID: 14376 RVA: 0x001220EC File Offset: 0x001202EC
	// (set) Token: 0x06003829 RID: 14377 RVA: 0x001220F4 File Offset: 0x001202F4
	public static int AllLevelsCompleted
	{
		get
		{
			return GlobalGameController._allLevelsCompleted;
		}
		set
		{
			GlobalGameController._allLevelsCompleted = value;
		}
	}

	// Token: 0x17000932 RID: 2354
	// (get) Token: 0x0600382A RID: 14378 RVA: 0x001220FC File Offset: 0x001202FC
	// (set) Token: 0x0600382B RID: 14379 RVA: 0x00122144 File Offset: 0x00120344
	public static bool is60FPSEnable
	{
		get
		{
			if (!GlobalGameController.is60FPSEnableInit)
			{
				GlobalGameController._is60FPSEnable = (PlayerPrefs.GetInt("fps60Enable", (!Device.isPixelGunLow) ? 1 : 0) == 1);
				GlobalGameController.is60FPSEnableInit = true;
			}
			return GlobalGameController._is60FPSEnable;
		}
		set
		{
			GlobalGameController._is60FPSEnable = value;
			PlayerPrefs.SetInt("fps60Enable", (!GlobalGameController._is60FPSEnable) ? 0 : 1);
			GlobalGameController.is60FPSEnableInit = true;
			Application.targetFrameRate = ((!GlobalGameController._is60FPSEnable) ? 30 : 60);
		}
	}

	// Token: 0x0600382C RID: 14380 RVA: 0x00122190 File Offset: 0x00120390
	private static void Swap(IList<int> list, int indexA, int indexB)
	{
		int value = list[indexA];
		list[indexA] = list[indexB];
		list[indexB] = value;
	}

	// Token: 0x17000933 RID: 2355
	// (get) Token: 0x0600382D RID: 14381 RVA: 0x001221BC File Offset: 0x001203BC
	public static int ZombiesInWave
	{
		get
		{
			return 4;
		}
	}

	// Token: 0x0600382E RID: 14382 RVA: 0x001221C0 File Offset: 0x001203C0
	internal static int GetEnemiesToKill(string sceneName)
	{
		if (!TrainingController.TrainingCompleted || StringComparer.OrdinalIgnoreCase.Equals(sceneName, Defs.TrainingSceneName))
		{
			return 3;
		}
		if (GlobalGameController._enemiesToKillOverride != null)
		{
			return GlobalGameController._enemiesToKillOverride.Value;
		}
		if (!Defs.IsSurvival)
		{
			return ZombieCreator.GetCountMobsForLevel();
		}
		return 35;
	}

	// Token: 0x17000934 RID: 2356
	// (get) Token: 0x0600382F RID: 14383 RVA: 0x0012221C File Offset: 0x0012041C
	// (set) Token: 0x06003830 RID: 14384 RVA: 0x0012223C File Offset: 0x0012043C
	public static int EnemiesToKill
	{
		get
		{
			return GlobalGameController.GetEnemiesToKill(SceneManager.GetActiveScene().name);
		}
		set
		{
			GlobalGameController._enemiesToKillOverride = new int?(value);
		}
	}

	// Token: 0x06003831 RID: 14385 RVA: 0x0012224C File Offset: 0x0012044C
	public static void ResetParameters()
	{
		GlobalGameController.AllLevelsCompleted = 0;
		GlobalGameController.numOfCompletedLevels = -1;
		GlobalGameController.totalNumOfCompletedLevels = -1;
	}

	// Token: 0x17000935 RID: 2357
	// (get) Token: 0x06003832 RID: 14386 RVA: 0x00122260 File Offset: 0x00120460
	// (set) Token: 0x06003833 RID: 14387 RVA: 0x0012226C File Offset: 0x0012046C
	internal static int Score
	{
		get
		{
			return GlobalGameController._saltedScore.Value;
		}
		set
		{
			GlobalGameController._saltedScore.Value = value;
		}
	}

	// Token: 0x17000936 RID: 2358
	// (get) Token: 0x06003834 RID: 14388 RVA: 0x0012227C File Offset: 0x0012047C
	// (set) Token: 0x06003835 RID: 14389 RVA: 0x00122288 File Offset: 0x00120488
	internal static int CountKills
	{
		get
		{
			return GlobalGameController._saltedCountKills.Value;
		}
		set
		{
			GlobalGameController._saltedCountKills.Value = value;
		}
	}

	// Token: 0x17000937 RID: 2359
	// (get) Token: 0x06003836 RID: 14390 RVA: 0x00122298 File Offset: 0x00120498
	// (set) Token: 0x06003837 RID: 14391 RVA: 0x001222D4 File Offset: 0x001204D4
	public static int CountDaySessionInCurrentVersion
	{
		get
		{
			if (GlobalGameController._countDaySessionInCurrentVersion == -1)
			{
				GlobalGameController._countDaySessionInCurrentVersion = PlayerPrefs.GetInt(Defs.SessionDayNumberKey, 1) - PlayerPrefs.GetInt("countSessionDayOnStartCorrentVersion", 1);
			}
			return GlobalGameController._countDaySessionInCurrentVersion;
		}
		set
		{
			GlobalGameController._countDaySessionInCurrentVersion = value;
		}
	}

	// Token: 0x17000938 RID: 2360
	// (get) Token: 0x06003838 RID: 14392 RVA: 0x001222DC File Offset: 0x001204DC
	public static int SimultaneousEnemiesOnLevelConstraint
	{
		get
		{
			return 20;
		}
	}

	// Token: 0x17000939 RID: 2361
	// (get) Token: 0x06003839 RID: 14393 RVA: 0x001222E0 File Offset: 0x001204E0
	// (set) Token: 0x0600383A RID: 14394 RVA: 0x001222E8 File Offset: 0x001204E8
	internal static bool NewVersionAvailable { get; set; }

	// Token: 0x1700093A RID: 2362
	// (get) Token: 0x0600383B RID: 14395 RVA: 0x001222F0 File Offset: 0x001204F0
	public static string MultiplayerProtocolVersion
	{
		get
		{
			return "11.3.0";
		}
	}

	// Token: 0x0600383C RID: 14396 RVA: 0x001222F8 File Offset: 0x001204F8
	public static void GoInBattle()
	{
		Defs.isFlag = false;
		Defs.isCOOP = false;
		Defs.isMulti = true;
		Defs.isHunger = false;
		Defs.isDuel = false;
		Defs.isCompany = false;
		Defs.IsSurvival = false;
		Defs.isFlag = false;
		MenuBackgroundMusic.keepPlaying = true;
		LoadConnectScene.textureToShow = null;
		LoadConnectScene.sceneToLoad = "ConnectScene";
		LoadConnectScene.noteToShow = null;
		SceneManager.LoadScene(Defs.PromSceneName);
	}

	// Token: 0x040028F4 RID: 10484
	public static bool HasSurvivalRecord;

	// Token: 0x040028F5 RID: 10485
	public static bool LeftHanded = true;

	// Token: 0x040028F6 RID: 10486
	public static bool switchingWeaponSwipe = false;

	// Token: 0x040028F7 RID: 10487
	public static List<int> survScoreThresh = new List<int>();

	// Token: 0x040028F8 RID: 10488
	public static int curThr;

	// Token: 0x040028F9 RID: 10489
	public static int thrStep = 10000;

	// Token: 0x040028FA RID: 10490
	public static Font fontHolder = null;

	// Token: 0x040028FB RID: 10491
	public static int EditingLogo = 0;

	// Token: 0x040028FC RID: 10492
	public static string TempClanName;

	// Token: 0x040028FD RID: 10493
	public static Texture2D LogoToEdit;

	// Token: 0x040028FE RID: 10494
	public static List<Texture2D> Logos;

	// Token: 0x040028FF RID: 10495
	public static readonly int NumOfLevels = 11;

	// Token: 0x04002900 RID: 10496
	private static int _currentLevel = -1;

	// Token: 0x04002901 RID: 10497
	private static int _allLevelsCompleted = 0;

	// Token: 0x04002902 RID: 10498
	public static bool showTableMyPlayer = false;

	// Token: 0x04002903 RID: 10499
	public static bool imDeadInHungerGame = false;

	// Token: 0x04002904 RID: 10500
	public static bool isFullVersion = true;

	// Token: 0x04002905 RID: 10501
	public static Vector3 posMyPlayer;

	// Token: 0x04002906 RID: 10502
	public static Quaternion rotMyPlayer;

	// Token: 0x04002907 RID: 10503
	public static float healthMyPlayer;

	// Token: 0x04002908 RID: 10504
	public static bool is60FPSEnableInit = false;

	// Token: 0x04002909 RID: 10505
	public static bool _is60FPSEnable;

	// Token: 0x0400290A RID: 10506
	public static int numOfCompletedLevels = 0;

	// Token: 0x0400290B RID: 10507
	public static int totalNumOfCompletedLevels = 0;

	// Token: 0x0400290C RID: 10508
	public static int countKillsBlue = 0;

	// Token: 0x0400290D RID: 10509
	public static int countKillsRed = 0;

	// Token: 0x0400290E RID: 10510
	public static int EditingCape = 0;

	// Token: 0x0400290F RID: 10511
	public static bool EditedCapeSaved = false;

	// Token: 0x04002910 RID: 10512
	private static int? _enemiesToKillOverride;

	// Token: 0x04002911 RID: 10513
	private static SaltedInt _saltedScore = new SaltedInt(233495534);

	// Token: 0x04002912 RID: 10514
	private static SaltedInt _saltedCountKills = new SaltedInt(233495534);

	// Token: 0x04002913 RID: 10515
	private static int _countDaySessionInCurrentVersion = -1;

	// Token: 0x04002914 RID: 10516
	public static int coinsBase = 1;

	// Token: 0x04002915 RID: 10517
	public static int coinsBaseAdding = 0;

	// Token: 0x04002916 RID: 10518
	public static int levelsToGetCoins = 1;

	// Token: 0x04002917 RID: 10519
	public static readonly string AppVersion = "11.3.1";
}
