using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Rilisoft;
using Unity.Linq;
using UnityEngine;

// Token: 0x0200056C RID: 1388
internal sealed class BannerWindowController : MonoBehaviour
{
	// Token: 0x0600300B RID: 12299 RVA: 0x000FA9E0 File Offset: 0x000F8BE0
	private BannerWindowController()
	{
		this._bannerShowed = new bool[this.BannerWindowCount];
		this._needShowBanner = new bool[this.BannerWindowCount];
	}

	// Token: 0x1700084B RID: 2123
	// (get) Token: 0x0600300E RID: 12302 RVA: 0x000FAA3C File Offset: 0x000F8C3C
	// (set) Token: 0x0600300D RID: 12301 RVA: 0x000FAA34 File Offset: 0x000F8C34
	public static BannerWindowController SharedController { get; private set; }

	// Token: 0x0600300F RID: 12303 RVA: 0x000FAA44 File Offset: 0x000F8C44
	private void Awake()
	{
		BannerWindowController.SharedController = this;
	}

	// Token: 0x06003010 RID: 12304 RVA: 0x000FAA4C File Offset: 0x000F8C4C
	private void Start()
	{
		this._currentBanner = null;
		this._bannerQueue = new Queue<BannerWindow>();
		this._someBannerShown = false;
		this._whenStart = Time.realtimeSinceStartup + 3f;
		if (StarterPackController.Get != null)
		{
			StarterPackController.Get.CheckShowStarterPack();
			StarterPackController.Get.UpdateCountShownWindowByTimeCondition();
		}
		PromoActionsManager.UpdateDaysOfValorShownCondition();
		this._isBlockShowForNewPlayer = !this.IsBannersCanShowAfterNewInstall();
		this._isConnectScene = SceneLoader.ActiveSceneName.Equals("ConnectScene");
	}

	// Token: 0x06003011 RID: 12305 RVA: 0x000FAAD0 File Offset: 0x000F8CD0
	public void AddBannersTimeout(float seconds)
	{
		this._lastCheckTime = Time.realtimeSinceStartup + seconds;
	}

	// Token: 0x06003012 RID: 12306 RVA: 0x000FAAE0 File Offset: 0x000F8CE0
	private void OnDestroy()
	{
		BannerWindowController.SharedController = null;
		this._bannerQueue = null;
		this.advertiseController = null;
		BannerWindowController.firstScreen = false;
	}

	// Token: 0x06003013 RID: 12307 RVA: 0x000FAAFC File Offset: 0x000F8CFC
	private IEnumerator OnApplicationPause(bool pause)
	{
		if (!pause)
		{
			yield return null;
			yield return null;
			yield return null;
			if (StarterPackController.Get != null)
			{
				StarterPackController.Get.UpdateCountShownWindowByTimeCondition();
			}
			PromoActionsManager.UpdateDaysOfValorShownCondition();
			this._isBlockShowForNewPlayer = !this.IsBannersCanShowAfterNewInstall();
		}
		yield break;
	}

	// Token: 0x06003014 RID: 12308 RVA: 0x000FAB28 File Offset: 0x000F8D28
	public void RegisterWindow(BannerWindow window, BannerWindowType windowType)
	{
		if ((BannerWindowType)this.bannerWindows.Length < windowType + 1)
		{
			List<BannerWindow> list = this.bannerWindows.ToList<BannerWindow>();
			while (list.Count<BannerWindow>() < (int)(windowType + 1))
			{
				list.Add(null);
			}
			this.bannerWindows = list.ToArray();
		}
		this.bannerWindows[(int)windowType] = window;
		int layer = LayerMask.NameToLayer("Banners");
		window.gameObject.Descendants().ForEach(delegate(GameObject go)
		{
			go.layer = layer;
		});
	}

	// Token: 0x06003015 RID: 12309 RVA: 0x000FABB4 File Offset: 0x000F8DB4
	private BannerWindow ShowBannerWindow(BannerWindowType windowType)
	{
		if (this._isConnectScene && this._viewedBannersCount > 0)
		{
			return null;
		}
		if (this.bannerWindows.Length < 0 || windowType > (BannerWindowType)this.bannerWindows.Length - 1)
		{
			return null;
		}
		if (this.bannerWindows[(int)windowType] == null)
		{
			return null;
		}
		if (this.bannerWindows[(int)windowType].gameObject.activeSelf)
		{
			return null;
		}
		BannerWindow bannerWindow = this.bannerWindows[(int)windowType];
		if (this._currentBanner == null)
		{
			this._currentBanner = bannerWindow;
			this._currentBanner.type = windowType;
			if (this._isConnectScene)
			{
				this._viewedBannersCount++;
			}
			bannerWindow.Show();
		}
		else
		{
			this._bannerQueue.Enqueue(bannerWindow);
		}
		return bannerWindow;
	}

	// Token: 0x06003016 RID: 12310 RVA: 0x000FAC88 File Offset: 0x000F8E88
	public void HideBannerWindowNoShowNext()
	{
		if (this._currentBanner != null)
		{
			this._currentBanner.Hide();
			this._currentBanner = null;
		}
	}

	// Token: 0x06003017 RID: 12311 RVA: 0x000FACB0 File Offset: 0x000F8EB0
	public void ClearBannerStates()
	{
		this._bannerShowed = new bool[this.BannerWindowCount];
		this._needShowBanner = new bool[this.BannerWindowCount];
	}

	// Token: 0x06003018 RID: 12312 RVA: 0x000FACE0 File Offset: 0x000F8EE0
	public void HideBannerWindow()
	{
		BuySmileBannerController.openedFromPromoActions = false;
		this.HideBannerWindowNoShowNext();
		if (this._isConnectScene && this._viewedBannersCount > 0)
		{
			return;
		}
		if (this._bannerQueue.Count > 0)
		{
			BannerWindow bannerWindow = this._bannerQueue.Dequeue();
			this._currentBanner = bannerWindow;
			bannerWindow.Show();
		}
	}

	// Token: 0x06003019 RID: 12313 RVA: 0x000FAD3C File Offset: 0x000F8F3C
	private void ShowAdmobBanner()
	{
		if (AdmobPerelivWindow.admobTexture == null || string.IsNullOrEmpty(AdmobPerelivWindow.admobUrl))
		{
			return;
		}
		this.ShowBannerWindow(BannerWindowType.Admob);
	}

	// Token: 0x0600301A RID: 12314 RVA: 0x000FAD74 File Offset: 0x000F8F74
	public void AdmobBannerExitClick()
	{
		ButtonClickSound.Instance.PlayClick();
		this.HideBannerWindow();
		this._bannerShowed[2] = false;
		this._needShowBanner[2] = false;
		this.ResetStateBannerShowed(BannerWindowType.Admob);
	}

	// Token: 0x0600301B RID: 12315 RVA: 0x000FADA0 File Offset: 0x000F8FA0
	public void AdmobBannerApplyClick()
	{
		Application.OpenURL(AdmobPerelivWindow.admobUrl);
	}

	// Token: 0x0600301C RID: 12316 RVA: 0x000FADAC File Offset: 0x000F8FAC
	private void ShowAdvertisementBanner(AdvertisementController advertisementController)
	{
		if (advertisementController.AdvertisementTexture == null)
		{
			return;
		}
		this.advertiseController = advertisementController;
		BannerWindow bannerWindow = this.ShowBannerWindow(BannerWindowType.Advertisement);
		if (bannerWindow == null)
		{
			return;
		}
		if (AdsConfigManager.Instance.LastLoadedConfig == null)
		{
			return;
		}
		string playerCategory = AdsConfigManager.GetPlayerCategory(AdsConfigManager.Instance.LastLoadedConfig);
		PerelivSettings perelivSettings = PerelivConfigManager.Instance.GetPerelivSettings(playerCategory);
		bannerWindow.SetEnableExitButton(perelivSettings.ButtonClose);
		bannerWindow.SetBackgroundImage(advertisementController.AdvertisementTexture);
	}

	// Token: 0x0600301D RID: 12317 RVA: 0x000FAE2C File Offset: 0x000F902C
	public void AdvertBannerExitClick()
	{
		ButtonClickSound.Instance.PlayClick();
		this.advertiseController.Close();
		this.UpdateAdvertShownCount();
		this.HideBannerWindow();
	}

	// Token: 0x0600301E RID: 12318 RVA: 0x000FAE50 File Offset: 0x000F9050
	public void NewVersionBannerExitClick()
	{
		ButtonClickSound.Instance.PlayClick();
		BannerWindowController.UpdateNewVersionShownCount();
		this.HideBannerWindow();
	}

	// Token: 0x0600301F RID: 12319 RVA: 0x000FAE68 File Offset: 0x000F9068
	private static void UpdateNewVersionShownCount()
	{
		PlayerPrefs.SetInt(Defs.UpdateAvailableShownTimesSN, PlayerPrefs.GetInt(Defs.UpdateAvailableShownTimesSN, 3) - 1);
		PlayerPrefs.SetString(Defs.LastTimeUpdateAvailableShownSN, DateTimeOffset.Now.ToString("s"));
		PlayerPrefs.Save();
	}

	// Token: 0x06003020 RID: 12320 RVA: 0x000FAEB0 File Offset: 0x000F90B0
	private static void ClearNewVersionShownCount()
	{
		PlayerPrefs.SetInt(Defs.UpdateAvailableShownTimesSN, 0);
		PlayerPrefs.SetString(Defs.LastTimeUpdateAvailableShownSN, DateTimeOffset.Now.ToString("s"));
		PlayerPrefs.Save();
	}

	// Token: 0x06003021 RID: 12321 RVA: 0x000FAEEC File Offset: 0x000F90EC
	public void AdvertBannerApplyClick()
	{
		ButtonClickSound.Instance.PlayClick();
		this.advertiseController.Close();
		this.UpdateAdvertShownCount();
		if (AdsConfigManager.Instance.LastLoadedConfig != null)
		{
			string playerCategory = AdsConfigManager.GetPlayerCategory(AdsConfigManager.Instance.LastLoadedConfig);
			PerelivSettings perelivSettings = PerelivConfigManager.Instance.GetPerelivSettings(playerCategory);
			Application.OpenURL(perelivSettings.RedirectUrl);
		}
		this.HideBannerWindow();
	}

	// Token: 0x06003022 RID: 12322 RVA: 0x000FAF50 File Offset: 0x000F9150
	public void NewVersionBannerApplyClick()
	{
		ButtonClickSound.Instance.PlayClick();
		BannerWindowController.ClearNewVersionShownCount();
		Application.OpenURL(MainMenu.RateUsURL);
		this.HideBannerWindow();
	}

	// Token: 0x06003023 RID: 12323 RVA: 0x000FAF74 File Offset: 0x000F9174
	public void EverydayRewardApplyClick()
	{
		ButtonClickSound.TryPlayClick();
		this.TakeEverydayRewardForPlayer();
		this.HideBannerWindow();
	}

	// Token: 0x06003024 RID: 12324 RVA: 0x000FAF88 File Offset: 0x000F9188
	private void TakeEverydayRewardForPlayer()
	{
		NotificationController.isGetEveryDayMoney = false;
		if (MainMenu.sharedMenu != null)
		{
			MainMenu.sharedMenu.isShowAvard = false;
		}
		int @int = Storager.getInt("Coins", false);
		Storager.setInt("Coins", @int + 3, false);
		AnalyticsFacade.CurrencyAccrual(3, "Coins", AnalyticsConstants.AccrualType.Earned);
		CoinsMessage.FireCoinsAddedEvent(false, 2);
		AudioClip audioClip = Resources.Load<AudioClip>("coin_get");
		if (audioClip != null && Defs.isSoundFX)
		{
			NGUITools.PlaySound(audioClip);
		}
	}

	// Token: 0x06003025 RID: 12325 RVA: 0x000FB00C File Offset: 0x000F920C
	public void SorryBannerExitButtonClick()
	{
		MainMenuController.sharedController.stubLoading.SetActive(false);
		this.HideBannerWindow();
	}

	// Token: 0x06003026 RID: 12326 RVA: 0x000FB024 File Offset: 0x000F9224
	public void EventX3ExitClick()
	{
		ButtonClickSound.TryPlayClick();
		this.UpdateEventX3ShownCount();
		this.HideBannerWindow();
	}

	// Token: 0x06003027 RID: 12327 RVA: 0x000FB038 File Offset: 0x000F9238
	public void EventX3ApplyClick()
	{
		this.EventX3ExitClick();
		if (MainMenuController.sharedController != null)
		{
			MainMenuController.sharedController.ShowBankWindow();
			return;
		}
		if (ConnectSceneNGUIController.sharedController != null)
		{
			ConnectSceneNGUIController.sharedController.ShowBankWindow();
			return;
		}
	}

	// Token: 0x06003028 RID: 12328 RVA: 0x000FB084 File Offset: 0x000F9284
	private void UpdateEventX3ShownCount()
	{
		PlayerPrefs.SetInt(Defs.EventX3WindowShownCount, PlayerPrefs.GetInt(Defs.EventX3WindowShownCount, 1) - 1);
		PlayerPrefs.Save();
	}

	// Token: 0x06003029 RID: 12329 RVA: 0x000FB0A4 File Offset: 0x000F92A4
	private void UpdateAdvertShownCount()
	{
		if (AdsConfigManager.Instance.LastLoadedConfig == null)
		{
			return;
		}
		string playerCategory = AdsConfigManager.GetPlayerCategory(AdsConfigManager.Instance.LastLoadedConfig);
		PerelivSettings perelivSettings = PerelivConfigManager.Instance.GetPerelivSettings(playerCategory);
		if (perelivSettings.ShowAlways)
		{
			return;
		}
		PlayerPrefs.SetInt(Defs.AdvertWindowShownCount, PlayerPrefs.GetInt(Defs.AdvertWindowShownCount, 3) - 1);
		PlayerPrefs.SetString(Defs.AdvertWindowShownLastTime, PromoActionsManager.CurrentUnixTime.ToString());
		PlayerPrefs.Save();
	}

	// Token: 0x0600302A RID: 12330 RVA: 0x000FB11C File Offset: 0x000F931C
	private bool IsBannersCanShowAfterNewInstall()
	{
		DateTime d;
		return string.IsNullOrEmpty(Defs.StartTimeShowBannersString) || !DateTime.TryParse(Defs.StartTimeShowBannersString, out d) || (DateTime.UtcNow - d).TotalMinutes >= 1440.0 || Defs.countReturnInConnectScene >= 4;
	}

	// Token: 0x0600302B RID: 12331 RVA: 0x000FB180 File Offset: 0x000F9380
	private void Update()
	{
		if (this._isConnectScene && this._viewedBannersCount > 0)
		{
			return;
		}
		if (Time.realtimeSinceStartup < this._whenStart)
		{
			return;
		}
		if (Time.realtimeSinceStartup - this._lastCheckTime >= 1f)
		{
			this.CheckBannersShowConditions();
			for (int i = 0; i < this._needShowBanner.Length; i++)
			{
				if (!this._someBannerShown || i == 2)
				{
					if (this._needShowBanner[i])
					{
						if (!ActivityIndicator.IsActiveIndicator)
						{
							if (MainMenuController.IsShowRentExpiredPoint() || (MainMenuController.sharedController != null && (MainMenuController.sharedController.FreePanelIsActive || MainMenuController.sharedController.singleModePanel.activeSelf)))
							{
								break;
							}
							if (BankController.Instance != null && BankController.Instance.InterfaceEnabled)
							{
								break;
							}
							if (!FreeAwardController.FreeAwardChestIsInIdleState)
							{
								break;
							}
							if (MainMenuController.SavedShwonLobbyLevelIsLessThanActual())
							{
								break;
							}
							if (ExpController.Instance != null && ExpController.Instance.IsLevelUpShown)
							{
								break;
							}
							if (i != 6 || !SceneLoader.ActiveSceneName.Equals(Defs.MainMenuScene) || Storager.getInt(Defs.ShownLobbyLevelSN, false) >= 3)
							{
								if (i != 1 || SceneLoader.ActiveSceneName.Equals(Defs.MainMenuScene))
								{
									if (i != 0 || SceneLoader.ActiveSceneName.Equals(Defs.MainMenuScene))
									{
										if (!this._isConnectScene || !this._needShowBanner[10] || i == 10)
										{
											this._needShowBanner[i] = false;
											if (i == 2)
											{
												this.ShowAdmobBanner();
											}
											else if (i == 5)
											{
												this.ShowAdvertisementBanner(this.advertiseController);
											}
											else
											{
												this.ShowBannerWindow((BannerWindowType)i);
											}
											this._someBannerShown = true;
											break;
										}
									}
								}
							}
						}
					}
				}
			}
			this._lastCheckTime = Time.realtimeSinceStartup;
		}
	}

	// Token: 0x0600302C RID: 12332 RVA: 0x000FB3B4 File Offset: 0x000F95B4
	private void CheckDownloadAdvertisement()
	{
		if (ExperienceController.sharedController == null)
		{
			return;
		}
		if (AdsConfigManager.Instance.LastLoadedConfig == null)
		{
			return;
		}
		string playerCategory = AdsConfigManager.GetPlayerCategory(AdsConfigManager.Instance.LastLoadedConfig);
		PerelivSettings perelivSettings = PerelivConfigManager.Instance.GetPerelivSettings(playerCategory);
		int currentLevel = ExperienceController.sharedController.currentLevel;
		bool flag = perelivSettings.MinLevel == -1 || currentLevel >= perelivSettings.MinLevel;
		bool flag2 = perelivSettings.MaxLevel == -1 || currentLevel <= perelivSettings.MaxLevel;
		bool flag3 = perelivSettings.ShowAlways || PlayerPrefs.GetInt(Defs.AdvertWindowShownCount, 3) > 0;
		bool flag4 = perelivSettings.Enabled && this.advertiseController.CurrentState == AdvertisementController.State.Idle;
		if (flag4 && flag && flag2 && flag3)
		{
			this.advertiseController.Run();
		}
	}

	// Token: 0x0600302D RID: 12333 RVA: 0x000FB4A8 File Offset: 0x000F96A8
	private bool IsAdvertisementDownloading()
	{
		if (this.advertiseController == null)
		{
			return false;
		}
		AdvertisementController.State currentState = this.advertiseController.CurrentState;
		return currentState != AdvertisementController.State.Idle && currentState != AdvertisementController.State.Complete && currentState != AdvertisementController.State.Error;
	}

	// Token: 0x0600302E RID: 12334 RVA: 0x000FB4EC File Offset: 0x000F96EC
	private void CheckBannersShowConditions()
	{
		if (PromoActionsManager.sharedManager == null)
		{
			return;
		}
		if (AdmobPerelivWindow.admobTexture != null && !string.IsNullOrEmpty(AdmobPerelivWindow.admobUrl))
		{
			if (Nest.Instance != null && Nest.Instance.BannerIsVisible)
			{
				if (Application.isEditor)
				{
					Debug.Log("Skipping fake interstitial while Nest Banner is active.");
				}
			}
			else if (!this._bannerShowed[2])
			{
				this._bannerShowed[2] = true;
				this._needShowBanner[2] = true;
			}
		}
		this.CheckDownloadAdvertisement();
		if (this.IsAdvertisementDownloading())
		{
			return;
		}
		if (AdsConfigManager.Instance.LastLoadedConfig == null)
		{
			return;
		}
		string playerCategory = AdsConfigManager.GetPlayerCategory(AdsConfigManager.Instance.LastLoadedConfig);
		PerelivSettings perelivSettings = PerelivConfigManager.Instance.GetPerelivSettings(playerCategory);
		if (perelivSettings.Enabled && this.advertiseController.CurrentState == AdvertisementController.State.Complete)
		{
			if (ConnectSceneNGUIController.sharedController != null)
			{
				if (Application.isEditor)
				{
					Debug.Log("Skipping pereliv in Connect Scene.");
				}
			}
			else if (Nest.Instance != null && Nest.Instance.BannerIsVisible)
			{
				if (Application.isEditor)
				{
					Debug.Log("Skipping pereliv while Nest Banner is active.");
				}
			}
			else if (!this._bannerShowed[5])
			{
				this._bannerShowed[5] = true;
				this._needShowBanner[5] = true;
			}
		}
		if (PlayerPrefs.GetInt("leave_from_duel_penalty") != 0 && (Nest.Instance == null || !Nest.Instance.BannerIsVisible) && !this._bannerShowed[10])
		{
			this._bannerShowed[10] = true;
			this._needShowBanner[10] = true;
		}
		if (Nest.Instance == null || !Nest.Instance.BannerIsVisible)
		{
			if (TournamentAvailableBannerWindow.CanShow && !this._bannerShowed[16])
			{
				this._bannerShowed[16] = true;
				this._needShowBanner[16] = true;
			}
			if (TournamentLooserBannerWindow.CanShow && !this._bannerShowed[18])
			{
				this._bannerShowed[18] = true;
				this._needShowBanner[18] = true;
			}
			if (TournamentWinnerBannerWindow.CanShow && !this._bannerShowed[17])
			{
				this._bannerShowed[17] = true;
				this._needShowBanner[17] = true;
			}
		}
		if (this._isBlockShowForNewPlayer)
		{
			return;
		}
		if (!BannerWindowController.firstScreen && PromoActionsManager.sharedManager.IsDayOfValorEventActive && TrainingController.TrainingCompleted && PlayerPrefs.GetInt("DaysOfValorShownCount", 1) > 0 && !this._bannerShowed[6])
		{
			this._bannerShowed[6] = true;
			this._needShowBanner[6] = true;
		}
		if (ConnectSceneNGUIController.sharedController != null && ConnectSceneNGUIController.isReturnFromGame && !ReviewController.IsNeedActive && !ReviewHUDWindow.isShow && PromoActionsManager.sharedManager.IsEventX3Active && TrainingController.TrainingCompleted && PlayerPrefs.GetInt(Defs.EventX3WindowShownCount, 1) > 0 && !this._bannerShowed[7])
		{
			this._bannerShowed[7] = true;
			this._needShowBanner[7] = true;
		}
		if (GlobalGameController.NewVersionAvailable && PlayerPrefs.GetInt(Defs.UpdateAvailableShownTimesSN, 3) > 0 && !this._bannerShowed[9])
		{
			this._bannerShowed[9] = true;
			this._needShowBanner[9] = true;
		}
		if (!BannerWindowController.firstScreen && StarterPackController.Get.IsNeedShowEventWindow() && !this._bannerShowed[11])
		{
			this._bannerShowed[11] = true;
			this._needShowBanner[11] = true;
		}
	}

	// Token: 0x0600302F RID: 12335 RVA: 0x000FB89C File Offset: 0x000F9A9C
	public void ResetStateBannerShowed(BannerWindowType windowType)
	{
		if (this.bannerWindows.Length < 0 || windowType > (BannerWindowType)this.bannerWindows.Length - 1)
		{
			return;
		}
		this._bannerShowed[(int)windowType] = false;
		this._someBannerShown = false;
	}

	// Token: 0x06003030 RID: 12336 RVA: 0x000FB8DC File Offset: 0x000F9ADC
	public bool IsBannerShow(BannerWindowType bannerType)
	{
		return !(this._currentBanner == null) && this._currentBanner.type == bannerType;
	}

	// Token: 0x06003031 RID: 12337 RVA: 0x000FB900 File Offset: 0x000F9B00
	public void ForceShowBanner(BannerWindowType windowType)
	{
		if (this._currentBanner == null)
		{
			this.ShowBannerWindow(windowType);
			return;
		}
		if (this._currentBanner.type != windowType)
		{
			this.HideBannerWindow();
			this.ShowBannerWindow(windowType);
		}
	}

	// Token: 0x06003032 RID: 12338 RVA: 0x000FB948 File Offset: 0x000F9B48
	internal void SubmitCurrentBanner()
	{
		if (this._currentBanner == null)
		{
			return;
		}
		this._currentBanner.Submit();
	}

	// Token: 0x1700084C RID: 2124
	// (get) Token: 0x06003033 RID: 12339 RVA: 0x000FB968 File Offset: 0x000F9B68
	internal bool IsAnyBannerShown
	{
		get
		{
			return this._currentBanner != null;
		}
	}

	// Token: 0x04002358 RID: 9048
	private const float StartBannerShowDelay = 3f;

	// Token: 0x04002359 RID: 9049
	public BannerWindow[] bannerWindows;

	// Token: 0x0400235A RID: 9050
	public static bool firstScreen = true;

	// Token: 0x0400235B RID: 9051
	[NonSerialized]
	public AdvertisementController advertiseController;

	// Token: 0x0400235C RID: 9052
	private readonly int BannerWindowCount = Enum.GetNames(typeof(BannerWindowType)).Length;

	// Token: 0x0400235D RID: 9053
	private Queue<BannerWindow> _bannerQueue;

	// Token: 0x0400235E RID: 9054
	private BannerWindow _currentBanner;

	// Token: 0x0400235F RID: 9055
	private bool[] _bannerShowed;

	// Token: 0x04002360 RID: 9056
	private bool[] _needShowBanner;

	// Token: 0x04002361 RID: 9057
	private bool _someBannerShown;

	// Token: 0x04002362 RID: 9058
	private float _lastCheckTime;

	// Token: 0x04002363 RID: 9059
	private float _whenStart;

	// Token: 0x04002364 RID: 9060
	private bool _isBlockShowForNewPlayer;

	// Token: 0x04002365 RID: 9061
	private bool _isConnectScene;

	// Token: 0x04002366 RID: 9062
	private int _viewedBannersCount;
}
