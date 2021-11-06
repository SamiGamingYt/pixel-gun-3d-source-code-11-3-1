using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using FyberPlugin;
using Rilisoft;
using Rilisoft.MiniJson;
using UnityEngine;

// Token: 0x020005FF RID: 1535
internal sealed class FreeAwardController : MonoBehaviour
{
	// Token: 0x14000052 RID: 82
	// (add) Token: 0x060034AF RID: 13487 RVA: 0x00110218 File Offset: 0x0010E418
	// (remove) Token: 0x060034B0 RID: 13488 RVA: 0x00110234 File Offset: 0x0010E434
	public event EventHandler<FreeAwardController.StateEventArgs> StateChanged;

	// Token: 0x170008BE RID: 2238
	// (get) Token: 0x060034B1 RID: 13489 RVA: 0x00110250 File Offset: 0x0010E450
	public static bool FreeAwardChestIsInIdleState
	{
		get
		{
			return FreeAwardController.Instance == null || FreeAwardController.Instance.IsInState<FreeAwardController.IdleState>();
		}
	}

	// Token: 0x170008BF RID: 2239
	// (get) Token: 0x060034B2 RID: 13490 RVA: 0x00110270 File Offset: 0x0010E470
	public static FreeAwardController Instance
	{
		get
		{
			return FreeAwardController._instance;
		}
	}

	// Token: 0x170008C0 RID: 2240
	// (get) Token: 0x060034B3 RID: 13491 RVA: 0x00110278 File Offset: 0x0010E478
	public AdProvider Provider
	{
		get
		{
			return this.GetProviderByIndex(this._adProviderIndex);
		}
	}

	// Token: 0x060034B4 RID: 13492 RVA: 0x00110288 File Offset: 0x0010E488
	public AdProvider GetProviderByIndex(int index)
	{
		return AdProvider.Fyber;
	}

	// Token: 0x060034B5 RID: 13493 RVA: 0x0011028C File Offset: 0x0010E48C
	internal int SwitchAdProvider()
	{
		int adProviderIndex = this._adProviderIndex;
		AdProvider provider = this.Provider;
		this._adProviderIndex++;
		if (provider == AdProvider.GoogleMobileAds)
		{
			MobileAdManager.Instance.DestroyVideoInterstitial();
		}
		if (this.Provider == AdProvider.GoogleMobileAds)
		{
			MobileAdManager.Instance.SwitchVideoIdGroup();
		}
		if (Defs.IsDeveloperBuild)
		{
			string message = string.Format("Switching provider from {0} ({1}) to {2} ({3})", new object[]
			{
				adProviderIndex,
				provider,
				this._adProviderIndex,
				this.Provider
			});
			Debug.Log(message);
		}
		return this._adProviderIndex;
	}

	// Token: 0x060034B6 RID: 13494 RVA: 0x00110334 File Offset: 0x0010E534
	private void ResetAdProvider()
	{
		int adProviderIndex = this._adProviderIndex;
		AdProvider provider = this.Provider;
		this._adProviderIndex = 0;
		AdProvider provider2 = this.Provider;
		if (provider == AdProvider.GoogleMobileAds && provider != provider2)
		{
			MobileAdManager.Instance.DestroyVideoInterstitial();
		}
		if (Defs.IsDeveloperBuild)
		{
			string message = string.Format("Resetting AdProvider from {0} ({1}) to {2} ({3})", new object[]
			{
				adProviderIndex,
				provider,
				this._adProviderIndex,
				this.Provider
			});
			Debug.Log(message);
		}
		MobileAdManager.Instance.ResetVideoAdUnitId();
	}

	// Token: 0x060034B7 RID: 13495 RVA: 0x001103D0 File Offset: 0x0010E5D0
	public T TryGetState<T>() where T : FreeAwardController.State
	{
		return this.CurrentState as T;
	}

	// Token: 0x060034B8 RID: 13496 RVA: 0x001103E4 File Offset: 0x0010E5E4
	public bool IsInState<T>() where T : FreeAwardController.State
	{
		return this.CurrentState is T;
	}

	// Token: 0x170008C1 RID: 2241
	// (get) Token: 0x060034B9 RID: 13497 RVA: 0x001103F4 File Offset: 0x0010E5F4
	// (set) Token: 0x060034BA RID: 13498 RVA: 0x001103FC File Offset: 0x0010E5FC
	private FreeAwardController.State CurrentState
	{
		get
		{
			return this._currentState;
		}
		set
		{
			if (value == null)
			{
				return;
			}
			this.SetCacheDirty();
			if (this._backSubscription != null)
			{
				this._backSubscription.Dispose();
				this._backSubscription = null;
			}
			if (!(value is FreeAwardController.IdleState))
			{
				this._backSubscription = BackSystem.Instance.Register(new Action(this.HandleClose), "Rewarded Video");
			}
			if (this.view != null)
			{
				this.view.CurrentState = value;
			}
			FreeAwardController.State currentState = this._currentState;
			this._currentState = value;
			EventHandler<FreeAwardController.StateEventArgs> stateChanged = this.StateChanged;
			if (stateChanged != null)
			{
				stateChanged(this, new FreeAwardController.StateEventArgs
				{
					State = value,
					OldState = currentState
				});
			}
		}
	}

	// Token: 0x060034BB RID: 13499 RVA: 0x001104B4 File Offset: 0x0010E6B4
	internal void SetWatchState(DateTime nextTimeEnabled)
	{
		this.ResetAdProvider();
		FreeAwardController.WatchState watchState = new FreeAwardController.WatchState(nextTimeEnabled);
		this.CurrentState = watchState;
		if (this.SimplifiedInterface)
		{
			TimeSpan estimatedTimeSpan = watchState.GetEstimatedTimeSpan();
			bool flag = estimatedTimeSpan <= TimeSpan.FromMinutes(0.0);
			if (flag)
			{
				this.HandleWatch();
			}
		}
	}

	// Token: 0x060034BC RID: 13500 RVA: 0x00110508 File Offset: 0x0010E708
	private void LoadVideo(string callerName = null)
	{
		if (callerName == null)
		{
			callerName = string.Empty;
		}
		if (FreeAwardController.Instance.Provider == AdProvider.Fyber)
		{
			FreeAwardController.FyberVideoLoaded = FreeAwardController.LoadFyberVideo(callerName);
		}
	}

	// Token: 0x060034BD RID: 13501 RVA: 0x00110540 File Offset: 0x0010E740
	public void HandleClose()
	{
		ButtonClickSound.TryPlayClick();
		if (this.IsInState<FreeAwardController.CloseState>())
		{
			this.HideButtonsShowAward();
		}
		if (!this.IsInState<FreeAwardController.AwardState>())
		{
			if (this.Provider == AdProvider.GoogleMobileAds)
			{
				MobileAdManager.Instance.DestroyVideoInterstitial();
			}
			this.CurrentState = FreeAwardController.IdleState.Instance;
		}
		else
		{
			this.HandleGetAward();
		}
	}

	// Token: 0x060034BE RID: 13502 RVA: 0x0011059C File Offset: 0x0010E79C
	public void HandleWatch()
	{
		this.LoadVideo("HandleWatch");
		this.CurrentState = new FreeAwardController.WaitingState();
	}

	// Token: 0x060034BF RID: 13503 RVA: 0x001105B4 File Offset: 0x0010E7B4
	public void HandleDeveloperSkip()
	{
		this.CurrentState = new FreeAwardController.WatchingState();
	}

	// Token: 0x060034C0 RID: 13504 RVA: 0x001105C4 File Offset: 0x0010E7C4
	public int GiveAwardAndIncrementCount()
	{
		int result = this.AddAdvertTime(DateTime.UtcNow);
		if (this.CurrencyForAward == "GemsCurrency")
		{
			BankController.AddGems(FreeAwardController.CountMoneyForAward, true, AnalyticsConstants.AccrualType.Earned);
		}
		else
		{
			BankController.AddCoins(FreeAwardController.CountMoneyForAward, true, AnalyticsConstants.AccrualType.Earned);
		}
		Storager.setInt("PendingFreeAward", 0, false);
		if (Application.platform != RuntimePlatform.IPhonePlayer)
		{
			PlayerPrefs.Save();
		}
		return result;
	}

	// Token: 0x060034C1 RID: 13505 RVA: 0x0011062C File Offset: 0x0010E82C
	public void HandleGetAward()
	{
		int num = this.GiveAwardAndIncrementCount();
		AnalyticsStuff.LogDailyVideoRewarded(num);
		if (AdsConfigManager.Instance.LastLoadedConfig == null)
		{
			this.CurrentState = FreeAwardController.CloseState.Instance;
			return;
		}
		ChestInLobbyPointMemento chestInLobby = AdsConfigManager.Instance.LastLoadedConfig.AdPointsConfig.ChestInLobby;
		if (chestInLobby == null)
		{
			this.CurrentState = FreeAwardController.CloseState.Instance;
			return;
		}
		string playerCategory = AdsConfigManager.GetPlayerCategory(AdsConfigManager.Instance.LastLoadedConfig);
		List<double> finalRewardedVideoDelayMinutes = chestInLobby.GetFinalRewardedVideoDelayMinutes(playerCategory);
		if (num < finalRewardedVideoDelayMinutes.Count)
		{
			this.ResetAdProvider();
			DateTime utcNow = DateTime.UtcNow;
			TimeSpan t = TimeSpan.FromMinutes(finalRewardedVideoDelayMinutes[num]);
			DateTime dateTime = utcNow + t;
			bool flag = utcNow.Date < dateTime.Date;
			this.CurrentState = ((!flag) ? new FreeAwardController.WatchState(dateTime) : FreeAwardController.CloseState.Instance);
			if (Defs.IsDeveloperBuild)
			{
				string text = Json.Serialize(finalRewardedVideoDelayMinutes);
				Debug.LogFormat("HandleGetAward(): `utcNow`: {0:s}, `delay`: {1:f2}, `nextTimeEnabled`: {2:s}, `CurrentState`: {3}, `delays`: {4}, `newCount`: {5}", new object[]
				{
					utcNow,
					t.TotalMinutes,
					dateTime,
					this.CurrentState,
					text,
					num
				});
			}
		}
		else
		{
			this.CurrentState = FreeAwardController.CloseState.Instance;
		}
	}

	// Token: 0x170008C2 RID: 2242
	// (get) Token: 0x060034C2 RID: 13506 RVA: 0x00110774 File Offset: 0x0010E974
	// (set) Token: 0x060034C3 RID: 13507 RVA: 0x0011077C File Offset: 0x0010E97C
	internal static Future<object> FyberVideoLoaded { get; set; }

	// Token: 0x060034C4 RID: 13508 RVA: 0x00110784 File Offset: 0x0010E984
	internal static Future<object> LoadFyberVideo(string callerName = null)
	{
		if (callerName == null)
		{
			callerName = string.Empty;
		}
		Promise<object> promise = new Promise<object>();
		Action<Ad> onAdAvailable = null;
		Action<AdFormat> onAdNotAvailable = null;
		Action<RequestError> onRequestFail = null;
		onAdAvailable = delegate(Ad ad)
		{
			if (Defs.IsDeveloperBuild)
			{
				Debug.LogFormat("LoadFyberVideo > AdAvailable: {{ format: {0}, placementId: '{1}' }}", new object[]
				{
					ad.AdFormat,
					ad.PlacementId
				});
			}
			promise.SetResult(ad);
			FyberCallback.AdAvailable -= onAdAvailable;
			FyberCallback.AdNotAvailable -= onAdNotAvailable;
			FyberCallback.RequestFail -= onRequestFail;
		};
		onAdNotAvailable = delegate(AdFormat adFormat)
		{
			if (Defs.IsDeveloperBuild)
			{
				Debug.LogFormat("LoadFyberVideo > AdNotAvailable: {{ format: {0} }}", new object[]
				{
					adFormat
				});
			}
			promise.SetResult(adFormat);
			FyberCallback.AdAvailable -= onAdAvailable;
			FyberCallback.AdNotAvailable -= onAdNotAvailable;
			FyberCallback.RequestFail -= onRequestFail;
		};
		onRequestFail = delegate(RequestError requestError)
		{
			if (Defs.IsDeveloperBuild)
			{
				Debug.LogFormat("LoadFyberVideo > RequestFail: {{ requestError: {0} }}", new object[]
				{
					requestError.Description
				});
			}
			promise.SetResult(requestError);
			FyberCallback.AdAvailable -= onAdAvailable;
			FyberCallback.AdNotAvailable -= onAdNotAvailable;
			FyberCallback.RequestFail -= onRequestFail;
		};
		FyberCallback.AdAvailable += onAdAvailable;
		FyberCallback.AdNotAvailable += onAdNotAvailable;
		FyberCallback.RequestFail += onRequestFail;
		FreeAwardController.RequestFyberRewardedVideo(0);
		return promise.Future;
	}

	// Token: 0x060034C5 RID: 13509 RVA: 0x0011082C File Offset: 0x0010EA2C
	private static void RequestFyberRewardedVideo(int roundIndex)
	{
		RewardedVideoRequester.Create().NotifyUserOnCompletion(false).Request();
	}

	// Token: 0x060034C6 RID: 13510 RVA: 0x00110840 File Offset: 0x0010EA40
	private void HideButtonsShowAward()
	{
		BankController instance = BankController.Instance;
		if (instance != null && instance.InterfaceEnabled)
		{
			instance.CurrentBankView.freeAwardButton.gameObject.SetActiveSafeSelf(false);
		}
	}

	// Token: 0x060034C7 RID: 13511 RVA: 0x00110880 File Offset: 0x0010EA80
	internal bool AdvertCountLessThanLimit()
	{
		if (AdsConfigManager.Instance.LastLoadedConfig == null)
		{
			return false;
		}
		ChestInLobbyPointMemento chestInLobby = AdsConfigManager.Instance.LastLoadedConfig.AdPointsConfig.ChestInLobby;
		if (chestInLobby == null)
		{
			return false;
		}
		string playerCategory = AdsConfigManager.GetPlayerCategory(AdsConfigManager.Instance.LastLoadedConfig);
		List<double> finalRewardedVideoDelayMinutes = chestInLobby.GetFinalRewardedVideoDelayMinutes(playerCategory);
		int count = finalRewardedVideoDelayMinutes.Count;
		int advertCountDuringCurrentPeriod;
		using (new ProfilerSample("AdvertCountLessThanLimit()->GetAdvertCountDuringCurrentPeriod()"))
		{
			advertCountDuringCurrentPeriod = this.GetAdvertCountDuringCurrentPeriod();
		}
		if (advertCountDuringCurrentPeriod >= count)
		{
			return false;
		}
		DateTime currentTime = this.CurrentTime;
		TimeSpan t = TimeSpan.FromMinutes(finalRewardedVideoDelayMinutes[advertCountDuringCurrentPeriod]);
		bool flag = (currentTime + t).Date > currentTime.Date;
		return !flag;
	}

	// Token: 0x060034C8 RID: 13512 RVA: 0x00110960 File Offset: 0x0010EB60
	internal bool TimeTamperingDetected()
	{
		if (!Storager.hasKey("AdvertTimeDuringCurrentPeriod"))
		{
			return false;
		}
		string @string = Storager.getString("AdvertTimeDuringCurrentPeriod", false);
		Dictionary<string, object> dictionary = Json.Deserialize(@string) as Dictionary<string, object>;
		if (dictionary == null)
		{
			return false;
		}
		string strB = dictionary.Keys.Min<string>();
		string text = this.CurrentTime.ToString("yyyy-MM-dd");
		return text.CompareTo(strB) < 0;
	}

	// Token: 0x060034C9 RID: 13513 RVA: 0x001109D0 File Offset: 0x0010EBD0
	private static void RemoveOldEntriesForAdvertTimes()
	{
		if (!Storager.hasKey("AdvertTimeDuringCurrentPeriod"))
		{
			return;
		}
		Dictionary<string, object> dictionary = Json.Deserialize(Storager.getString("AdvertTimeDuringCurrentPeriod", false)) as Dictionary<string, object>;
		if (dictionary == null)
		{
			return;
		}
		if (dictionary.Keys.Count < 2)
		{
			return;
		}
		string maxKey = dictionary.Keys.Max<string>();
		string[] array = (from k in dictionary.Keys
		where !k.Equals(maxKey, StringComparison.Ordinal)
		select k).ToArray<string>();
		foreach (string key in array)
		{
			dictionary.Remove(key);
		}
		string val = Json.Serialize(dictionary);
		Storager.setString("AdvertTimeDuringCurrentPeriod", val, false);
	}

	// Token: 0x060034CA RID: 13514 RVA: 0x00110A90 File Offset: 0x0010EC90
	private static void AddEmptyEntryForAdvertTime(DateTime date)
	{
		string dateKey = date.ToString("yyyy-MM-dd");
		Action action = delegate()
		{
			Dictionary<string, object> obj = new Dictionary<string, object>
			{
				{
					dateKey,
					new string[0]
				}
			};
			string val2 = Json.Serialize(obj);
			Storager.setString("AdvertTimeDuringCurrentPeriod", val2, false);
		};
		if (!Storager.hasKey("AdvertTimeDuringCurrentPeriod"))
		{
			action();
			return;
		}
		Dictionary<string, object> dictionary = Json.Deserialize(Storager.getString("AdvertTimeDuringCurrentPeriod", false)) as Dictionary<string, object>;
		if (dictionary == null)
		{
			action();
			return;
		}
		if (dictionary.ContainsKey(dateKey))
		{
			return;
		}
		dictionary.Add(dateKey, new string[0]);
		string val = Json.Serialize(dictionary);
		Storager.setString("AdvertTimeDuringCurrentPeriod", val, false);
	}

	// Token: 0x060034CB RID: 13515 RVA: 0x00110B34 File Offset: 0x0010ED34
	private void Awake()
	{
		this._currentTime = DateTime.UtcNow;
		if (FreeAwardController._instance == null)
		{
			FreeAwardController._instance = this;
		}
		this.CurrentState = FreeAwardController.IdleState.Instance;
	}

	// Token: 0x060034CC RID: 13516 RVA: 0x00110B70 File Offset: 0x0010ED70
	private void OnDestroy()
	{
		FreeAwardController._instance = null;
	}

	// Token: 0x060034CD RID: 13517 RVA: 0x00110B78 File Offset: 0x0010ED78
	public static IEnumerator InitFyber()
	{
		while (!FriendsController.isReadABTestAdvertConfig)
		{
			yield return null;
		}
		yield return null;
		if (!FreeAwardController._initializedOnce)
		{
			FreeAwardController.SetCookieAcceptPolicy();
			FyberLogger.EnableLogging(true);
			string userId = FriendsController.sharedController.id;
			if (!Application.isEditor)
			{
				AppsFlyer.setCustomerUserID(userId);
			}
			if (!TrainingController.TrainingCompleted || Initializer.Instance != null)
			{
				string messageFormat = (!Application.isEditor) ? "{0}" : "<color=olive>{0}</color>";
				Debug.LogFormat(messageFormat, new object[]
				{
					"FreeAwardController: Postponing Fyber initialization till training is completed..."
				});
				while (!TrainingController.TrainingCompleted || Initializer.Instance != null)
				{
					yield return null;
				}
			}
			string idTail = (userId.Length <= 4) ? userId : userId.Substring(userId.Length - 4, 4);
			string payingBin = Storager.getInt("PayingUser", true).ToString(CultureInfo.InvariantCulture);
			Dictionary<string, string> parameters = new Dictionary<string, string>
			{
				{
					"pub0",
					SystemInfo.deviceModel
				},
				{
					"pub1",
					idTail
				},
				{
					"pub2",
					userId
				},
				{
					"pub3",
					payingBin
				}
			};
			if (Defs.IsDeveloperBuild)
			{
				Debug.LogFormat("{0}", new object[]
				{
					"FreeAwardController: Initializing Fyber..."
				});
			}
			Fyber fyber = Fyber.With(FreeAwardController.appId).WithSecurityToken(FreeAwardController.securityToken).WithUserId(userId).WithParameters(parameters);
			if (BuildSettings.BuildTargetPlatform != RuntimePlatform.IPhonePlayer)
			{
				fyber = fyber.WithManualPrecaching();
			}
			Settings settings = fyber.Start();
			if (Defs.IsDeveloperBuild)
			{
				Debug.Log("Start Fyber with parameters: appId=" + FreeAwardController.appId + " securityToken=" + FreeAwardController.securityToken);
			}
			User.SetAppVersion(GlobalGameController.AppVersion);
			User.SetDevice(SystemInfo.deviceModel);
			User.PutCustomValue("pg3d_paying", payingBin);
			User.PutCustomValue("RandomKey", userId);
			FreeAwardController._initializedOnce = true;
		}
		yield break;
	}

	// Token: 0x060034CE RID: 13518 RVA: 0x00110B8C File Offset: 0x0010ED8C
	private IEnumerator Start()
	{
		while (FriendsController.sharedController == null)
		{
			yield return null;
		}
		while (string.IsNullOrEmpty(FriendsController.sharedController.id))
		{
			yield return null;
		}
		CoroutineRunner.Instance.StartCoroutine(FreeAwardController.InitFyber());
		while (!FreeAwardController._initializedOnce)
		{
			yield return null;
		}
		while (FriendsController.ServerTime < 0L)
		{
			yield return null;
		}
		try
		{
			DateTime currentTime = StarterPackModel.GetCurrentTimeByUnixTime((int)FriendsController.ServerTime);
			FreeAwardController.AddEmptyEntryForAdvertTime(currentTime);
		}
		finally
		{
			FreeAwardController.RemoveOldEntriesForAdvertTimes();
		}
		yield break;
	}

	// Token: 0x060034CF RID: 13519 RVA: 0x00110BA0 File Offset: 0x0010EDA0
	private void Update()
	{
		double num = 3.0;
		if (AdsConfigManager.Instance.LastLoadedConfig != null && AdsConfigManager.Instance.LastLoadedConfig.VideoConfig != null)
		{
			num = AdsConfigManager.Instance.LastLoadedConfig.VideoConfig.TimeoutWaitInSeconds;
		}
		FreeAwardController.WaitingState waitingState = this.TryGetState<FreeAwardController.WaitingState>();
		if (waitingState != null)
		{
			if (Application.isEditor || Tools.RuntimePlatform == RuntimePlatform.MetroPlayerX64)
			{
				if ((double)(Time.realtimeSinceStartup - waitingState.StartTime) > num)
				{
					if (this.Provider == AdProvider.GoogleMobileAds)
					{
						if (MobileAdManager.Instance.SwitchVideoAdUnitId())
						{
							this.SwitchAdProvider();
						}
					}
					else
					{
						this.SwitchAdProvider();
					}
					this.CurrentState = new FreeAwardController.ConnectionState();
					return;
				}
			}
			else if (this.Provider == AdProvider.GoogleMobileAds)
			{
				if (MobileAdManager.Instance.VideoInterstitialState == MobileAdManager.State.Loaded)
				{
					this.CurrentState = new FreeAwardController.WatchingState();
					return;
				}
				if (!string.IsNullOrEmpty(MobileAdManager.Instance.VideoAdFailedToLoadMessage))
				{
					if (Defs.IsDeveloperBuild)
					{
						string message = string.Format("Admob loading failed after {0:F3}s of {1}. Keep waiting.", Time.realtimeSinceStartup - waitingState.StartTime, num);
						Debug.Log(message);
					}
					bool flag = MobileAdManager.Instance.SwitchVideoAdUnitId();
					if (flag)
					{
						int num2 = this.SwitchAdProvider();
						if (PromoActionsManager.MobileAdvert.AdProviders.Count > 0 && num2 >= PromoActionsManager.MobileAdvert.CountRoundReplaceProviders * PromoActionsManager.MobileAdvert.AdProviders.Count)
						{
							string message2 = string.Format("Reporting connection issues after {0} switches.  Providers count {1}, rounds count {2}", num2, PromoActionsManager.MobileAdvert.AdProviders.Count, PromoActionsManager.MobileAdvert.CountRoundReplaceProviders);
							Debug.Log(message2);
							this.CurrentState = new FreeAwardController.ConnectionState();
							return;
						}
					}
					this.LoadVideo("Update");
					this.CurrentState = new FreeAwardController.WaitingState(waitingState.StartTime);
					return;
				}
				if ((double)(Time.realtimeSinceStartup - waitingState.StartTime) > num)
				{
					bool flag2 = MobileAdManager.Instance.SwitchVideoAdUnitId();
					if (flag2)
					{
						this.SwitchAdProvider();
					}
					this.CurrentState = new FreeAwardController.ConnectionState();
					return;
				}
			}
			else if (this.Provider == AdProvider.Fyber)
			{
				if (FreeAwardController.FyberVideoLoaded != null && FreeAwardController.FyberVideoLoaded.IsCompleted)
				{
					Ad ad = FreeAwardController.FyberVideoLoaded.Result as Ad;
					if (ad != null)
					{
						this.CurrentState = new FreeAwardController.WatchingState();
						return;
					}
					RequestError requestError = FreeAwardController.FyberVideoLoaded.Result as RequestError;
					if (Defs.IsDeveloperBuild)
					{
						Debug.LogFormat("Fyber loading failed: {0}. Keep waiting.", new object[]
						{
							(requestError == null) ? ((!(FreeAwardController.FyberVideoLoaded.Result is AdFormat)) ? "?" : "Not available") : requestError.Description
						});
					}
					int num3 = this.SwitchAdProvider();
					if (PromoActionsManager.MobileAdvert.AdProviders.Count > 0 && num3 >= PromoActionsManager.MobileAdvert.CountRoundReplaceProviders * PromoActionsManager.MobileAdvert.AdProviders.Count)
					{
						this.CurrentState = new FreeAwardController.ConnectionState();
						return;
					}
					this.LoadVideo("Update");
					this.CurrentState = new FreeAwardController.WaitingState(waitingState.StartTime);
					return;
				}
				else if ((double)(Time.realtimeSinceStartup - waitingState.StartTime) > num)
				{
					this.SwitchAdProvider();
					this.CurrentState = new FreeAwardController.ConnectionState();
					return;
				}
			}
			else if ((double)(Time.realtimeSinceStartup - waitingState.StartTime) > num)
			{
				this.CurrentState = new FreeAwardController.ConnectionState();
			}
			return;
		}
		FreeAwardController.WatchingState watchingState = this.TryGetState<FreeAwardController.WatchingState>();
		if (watchingState != null)
		{
			if (Application.isEditor && Time.realtimeSinceStartup - watchingState.StartTime > 1f)
			{
				watchingState.SimulateCallbackInEditor("CLOSE_FINISHED");
			}
			if (watchingState.AdClosed.IsCompleted)
			{
				if (Defs.IsDeveloperBuild)
				{
					Debug.LogFormat("[Rilisoft] Watching rewarded video completed: '{0}'", new object[]
					{
						watchingState.AdClosed.Result
					});
				}
				Storager.setInt("PendingFreeAward", 0, false);
				if (watchingState.AdClosed.Result.Equals("CLOSE_FINISHED", StringComparison.Ordinal))
				{
					this.CurrentState = FreeAwardController.AwardState.Instance;
				}
				else if (watchingState.AdClosed.Result.Equals("ERROR", StringComparison.Ordinal))
				{
					this.ResetAdProvider();
					this.CurrentState = new FreeAwardController.WatchState(DateTime.MinValue);
				}
				else if (watchingState.AdClosed.Result.Equals("CLOSE_ABORTED", StringComparison.Ordinal))
				{
					this.CurrentState = new FreeAwardController.WatchState(DateTime.MinValue);
				}
				else
				{
					string message3 = string.Format("[Rilisoft] Unsupported result for rewarded video: “{0}”", watchingState.AdClosed.Result);
					Debug.LogWarning(message3);
					this.CurrentState = new FreeAwardController.WatchState(DateTime.MinValue);
				}
			}
			return;
		}
		FreeAwardController.ConnectionState connectionState = this.TryGetState<FreeAwardController.ConnectionState>();
		if (connectionState != null)
		{
			if (Time.realtimeSinceStartup - connectionState.StartTime > 3f)
			{
				this.CurrentState = FreeAwardController.IdleState.Instance;
			}
			return;
		}
	}

	// Token: 0x060034D0 RID: 13520 RVA: 0x0011109C File Offset: 0x0010F29C
	public int GetAdvertCountDuringCurrentPeriod()
	{
		if (!Storager.hasKey("AdvertTimeDuringCurrentPeriod"))
		{
			return 0;
		}
		string @string = Storager.getString("AdvertTimeDuringCurrentPeriod", false);
		if (this._advertCountCache.Key == @string)
		{
			return this._advertCountCache.Value;
		}
		Dictionary<string, object> dictionary = Json.Deserialize(@string) as Dictionary<string, object>;
		if (dictionary == null)
		{
			if (Application.isEditor || Defs.IsDeveloperBuild)
			{
				Debug.LogWarningFormat("Cannot parse '{0}' to dictionary: {1}", new object[]
				{
					"AdvertTimeDuringCurrentPeriod",
					@string
				});
			}
			int num = 0;
			this._advertCountCache = new KeyValuePair<string, int>(@string, num);
			return num;
		}
		string text = this.CurrentTime.ToString("yyyy-MM-dd");
		string text2 = dictionary.Keys.Max<string>();
		if (text.CompareTo(text2) < 0)
		{
			int num2 = int.MaxValue;
			int.TryParse(text2.Replace("-", string.Empty), out num2);
			num2 = Math.Max(10000000, num2);
			this._advertCountCache = new KeyValuePair<string, int>(@string, num2);
			return num2;
		}
		object obj;
		if (dictionary.TryGetValue(text, out obj))
		{
			List<object> source = (obj as List<object>) ?? new List<object>();
			int num3 = source.OfType<string>().Count<string>();
			this._advertCountCache = new KeyValuePair<string, int>(@string, num3);
			return num3;
		}
		int num4 = 0;
		this._advertCountCache = new KeyValuePair<string, int>(@string, num4);
		return num4;
	}

	// Token: 0x060034D1 RID: 13521 RVA: 0x00111200 File Offset: 0x0010F400
	public int AddAdvertTime(DateTime time)
	{
		string key = time.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
		string item = time.ToString("T", CultureInfo.InvariantCulture);
		bool flag = Storager.hasKey("AdvertTimeDuringCurrentPeriod");
		if (!flag)
		{
			Dictionary<string, object> advertTime = new Dictionary<string, object>
			{
				{
					key,
					new List<string>(1)
					{
						item
					}
				}
			};
			this.SetAdvertTime(advertTime);
			return 1;
		}
		string json = (!flag) ? "{}" : Storager.getString("AdvertTimeDuringCurrentPeriod", false);
		Dictionary<string, object> dictionary = (Json.Deserialize(json) as Dictionary<string, object>) ?? new Dictionary<string, object>();
		object obj;
		if (dictionary.TryGetValue(key, out obj))
		{
			List<object> source = (obj as List<object>) ?? new List<object>();
			List<string> list = source.OfType<string>().ToList<string>();
			list.Add(item);
			dictionary[key] = list.ToList<string>();
			this.SetAdvertTime(dictionary);
			return list.Count;
		}
		dictionary[key] = new List<string>(1)
		{
			item
		};
		this.SetAdvertTime(dictionary);
		return 1;
	}

	// Token: 0x060034D2 RID: 13522 RVA: 0x00111324 File Offset: 0x0010F524
	public KeyValuePair<int, DateTime> LastAdvertShow(DateTime date)
	{
		KeyValuePair<int, DateTime> result = new KeyValuePair<int, DateTime>(int.MinValue, DateTime.MinValue);
		string key = date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
		bool flag = Storager.hasKey("AdvertTimeDuringCurrentPeriod");
		if (!flag)
		{
			return result;
		}
		string json = (!flag) ? "{}" : Storager.getString("AdvertTimeDuringCurrentPeriod", false);
		Dictionary<string, object> dictionary = (Json.Deserialize(json) as Dictionary<string, object>) ?? new Dictionary<string, object>();
		object obj;
		if (!dictionary.TryGetValue(key, out obj))
		{
			return result;
		}
		List<object> list = (obj as List<object>) ?? new List<object>();
		if (list.Count == 0)
		{
			return result;
		}
		List<string> list2 = list.OfType<string>().ToList<string>();
		if (list2.Count == 0)
		{
			return result;
		}
		string text = list2.Max<string>();
		DateTime dateTime;
		if (DateTime.TryParseExact(text, "T", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTime))
		{
			DateTime value = new DateTime(date.Year, date.Month, date.Day, dateTime.Hour, dateTime.Minute, dateTime.Second, DateTimeKind.Utc);
			return new KeyValuePair<int, DateTime>(list2.Count - 1, value);
		}
		Debug.LogWarning("Couldnot parse last time advert shown: " + text);
		return result;
	}

	// Token: 0x060034D3 RID: 13523 RVA: 0x00111464 File Offset: 0x0010F664
	private void SetAdvertTime(Dictionary<string, object> d)
	{
		if (d == null)
		{
			d = new Dictionary<string, object>();
		}
		string val = Json.Serialize(d) ?? "{}";
		Storager.setString("AdvertTimeDuringCurrentPeriod", val, false);
	}

	// Token: 0x060034D4 RID: 13524 RVA: 0x001114A0 File Offset: 0x0010F6A0
	private static void SetCookieAcceptPolicy()
	{
		if (Defs.IsDeveloperBuild)
		{
			Debug.Log(string.Format("Setting cookie accept policy is dumb on {0}.", Application.platform));
		}
	}

	// Token: 0x170008C3 RID: 2243
	// (get) Token: 0x060034D5 RID: 13525 RVA: 0x001114C8 File Offset: 0x0010F6C8
	private DateTime CurrentTime
	{
		get
		{
			return DateTime.UtcNow;
		}
	}

	// Token: 0x170008C4 RID: 2244
	// (get) Token: 0x060034D6 RID: 13526 RVA: 0x001114D0 File Offset: 0x0010F6D0
	public static int CountMoneyForAward
	{
		get
		{
			if (AdsConfigManager.Instance.LastLoadedConfig == null)
			{
				return ChestInLobbyPointMemento.DefaultAward;
			}
			ChestInLobbyPointMemento chestInLobby = AdsConfigManager.Instance.LastLoadedConfig.AdPointsConfig.ChestInLobby;
			if (chestInLobby == null)
			{
				return ChestInLobbyPointMemento.DefaultAward;
			}
			string playerCategory = AdsConfigManager.GetPlayerCategory(AdsConfigManager.Instance.LastLoadedConfig);
			return chestInLobby.GetFinalAward(playerCategory);
		}
	}

	// Token: 0x170008C5 RID: 2245
	// (get) Token: 0x060034D7 RID: 13527 RVA: 0x0011152C File Offset: 0x0010F72C
	public string CurrencyForAward
	{
		get
		{
			if (AdsConfigManager.Instance.LastLoadedConfig == null)
			{
				return ChestInLobbyPointMemento.DefaultCurrency;
			}
			ChestInLobbyPointMemento chestInLobby = AdsConfigManager.Instance.LastLoadedConfig.AdPointsConfig.ChestInLobby;
			if (chestInLobby == null)
			{
				return ChestInLobbyPointMemento.DefaultCurrency;
			}
			string playerCategory = AdsConfigManager.GetPlayerCategory(AdsConfigManager.Instance.LastLoadedConfig);
			return chestInLobby.GetFinalAwardCurrency(playerCategory);
		}
	}

	// Token: 0x170008C6 RID: 2246
	// (get) Token: 0x060034D8 RID: 13528 RVA: 0x00111588 File Offset: 0x0010F788
	internal bool SimplifiedInterface
	{
		get
		{
			if (this._simplifiedInterfaceCache == null)
			{
				if (AdsConfigManager.Instance.LastLoadedConfig == null)
				{
					return ChestInLobbyPointMemento.DefaultSimplifiedInterface;
				}
				ChestInLobbyPointMemento chestInLobby = AdsConfigManager.Instance.LastLoadedConfig.AdPointsConfig.ChestInLobby;
				if (chestInLobby == null)
				{
					return ChestInLobbyPointMemento.DefaultSimplifiedInterface;
				}
				string playerCategory = AdsConfigManager.GetPlayerCategory(AdsConfigManager.Instance.LastLoadedConfig);
				bool finalSimplifiedInterface = chestInLobby.GetFinalSimplifiedInterface(playerCategory);
				this._simplifiedInterfaceCache = new bool?(finalSimplifiedInterface);
			}
			return this._simplifiedInterfaceCache.Value;
		}
	}

	// Token: 0x060034D9 RID: 13529 RVA: 0x0011160C File Offset: 0x0010F80C
	private void SetCacheDirty()
	{
		this._simplifiedInterfaceCache = null;
	}

	// Token: 0x040026A8 RID: 9896
	private const string AdvertTimeDuringCurrentPeriodKey = "AdvertTimeDuringCurrentPeriod";

	// Token: 0x040026A9 RID: 9897
	public const string PendingFreeAwardKey = "PendingFreeAward";

	// Token: 0x040026AA RID: 9898
	public Camera renderCamera;

	// Token: 0x040026AB RID: 9899
	public FreeAwardView view;

	// Token: 0x040026AC RID: 9900
	private static FreeAwardController _instance;

	// Token: 0x040026AD RID: 9901
	private int _adProviderIndex;

	// Token: 0x040026AE RID: 9902
	private FreeAwardController.State _currentState = FreeAwardController.IdleState.Instance;

	// Token: 0x040026AF RID: 9903
	private IDisposable _backSubscription;

	// Token: 0x040026B0 RID: 9904
	private static bool _initializedOnce;

	// Token: 0x040026B1 RID: 9905
	public static string appId = "32897";

	// Token: 0x040026B2 RID: 9906
	public static string securityToken = "cf77aeadd83faf98e0cad61a1f1403c8";

	// Token: 0x040026B3 RID: 9907
	private KeyValuePair<string, int> _advertCountCache = new KeyValuePair<string, int>(string.Empty, 0);

	// Token: 0x040026B4 RID: 9908
	private bool? _simplifiedInterfaceCache;

	// Token: 0x040026B5 RID: 9909
	private DateTime _currentTime;

	// Token: 0x02000600 RID: 1536
	public class StateEventArgs : EventArgs
	{
		// Token: 0x170008C7 RID: 2247
		// (get) Token: 0x060034DB RID: 13531 RVA: 0x00111630 File Offset: 0x0010F830
		// (set) Token: 0x060034DC RID: 13532 RVA: 0x00111638 File Offset: 0x0010F838
		public FreeAwardController.State State { get; set; }

		// Token: 0x170008C8 RID: 2248
		// (get) Token: 0x060034DD RID: 13533 RVA: 0x00111644 File Offset: 0x0010F844
		// (set) Token: 0x060034DE RID: 13534 RVA: 0x0011164C File Offset: 0x0010F84C
		public FreeAwardController.State OldState { get; set; }
	}

	// Token: 0x02000601 RID: 1537
	public abstract class State
	{
	}

	// Token: 0x02000602 RID: 1538
	public sealed class IdleState : FreeAwardController.State
	{
		// Token: 0x060034E0 RID: 13536 RVA: 0x00111660 File Offset: 0x0010F860
		private IdleState()
		{
		}

		// Token: 0x170008C9 RID: 2249
		// (get) Token: 0x060034E2 RID: 13538 RVA: 0x00111674 File Offset: 0x0010F874
		internal static FreeAwardController.IdleState Instance
		{
			get
			{
				return FreeAwardController.IdleState._instance;
			}
		}

		// Token: 0x040026BA RID: 9914
		private static readonly FreeAwardController.IdleState _instance = new FreeAwardController.IdleState();
	}

	// Token: 0x02000603 RID: 1539
	public sealed class WatchState : FreeAwardController.State
	{
		// Token: 0x060034E3 RID: 13539 RVA: 0x0011167C File Offset: 0x0010F87C
		public WatchState(DateTime nextTimeEnabled)
		{
			DateTime utcNow = DateTime.UtcNow;
			if (Defs.IsDeveloperBuild && utcNow < nextTimeEnabled)
			{
				Debug.Log("Watching state inactive: need to wait till UTC " + nextTimeEnabled.ToString("T", CultureInfo.InvariantCulture));
			}
			this._nextTimeEnabled = nextTimeEnabled;
		}

		// Token: 0x060034E4 RID: 13540 RVA: 0x001116D4 File Offset: 0x0010F8D4
		public TimeSpan GetEstimatedTimeSpan()
		{
			return this._nextTimeEnabled - DateTime.UtcNow;
		}

		// Token: 0x040026BB RID: 9915
		private readonly DateTime _nextTimeEnabled;
	}

	// Token: 0x02000604 RID: 1540
	public sealed class WaitingState : FreeAwardController.State
	{
		// Token: 0x060034E5 RID: 13541 RVA: 0x001116E8 File Offset: 0x0010F8E8
		public WaitingState(float startTime)
		{
			this._startTime = startTime;
		}

		// Token: 0x060034E6 RID: 13542 RVA: 0x001116F8 File Offset: 0x0010F8F8
		public WaitingState() : this(Time.realtimeSinceStartup)
		{
		}

		// Token: 0x170008CA RID: 2250
		// (get) Token: 0x060034E7 RID: 13543 RVA: 0x00111708 File Offset: 0x0010F908
		public float StartTime
		{
			get
			{
				return this._startTime;
			}
		}

		// Token: 0x040026BC RID: 9916
		private readonly float _startTime;
	}

	// Token: 0x02000605 RID: 1541
	public sealed class WatchingState : FreeAwardController.State
	{
		// Token: 0x060034E8 RID: 13544 RVA: 0x00111710 File Offset: 0x0010F910
		public WatchingState()
		{
			this._startTime = Time.realtimeSinceStartup;
			string arg = FreeAwardController.WatchingState.DetermineContext();
			Storager.setInt("PendingFreeAward", (int)FreeAwardController.Instance.Provider, false);
			if (FreeAwardController.Instance.Provider == AdProvider.GoogleMobileAds)
			{
				Debug.Log("[Rilisoft] GoogleMobileAds are not supported directly.");
			}
			else if (FreeAwardController.Instance.Provider == AdProvider.UnityAds)
			{
				Debug.Log("[Rilisoft] UnityAds are not supported directly.");
			}
			else if (FreeAwardController.Instance.Provider == AdProvider.Vungle)
			{
				Debug.Log("[Rilisoft] Vungle is not supported directly.");
			}
			else if (FreeAwardController.Instance.Provider == AdProvider.Fyber)
			{
				AdvertisementInfo advertisementInfo = new AdvertisementInfo(0, 0, 0, null);
				if (!FreeAwardController.FyberVideoLoaded.IsCompleted)
				{
					Debug.LogWarning("FyberVideoLoaded.IsCompleted: False");
					return;
				}
				Ad ad = FreeAwardController.FyberVideoLoaded.Result as Ad;
				if (ad == null)
				{
					Debug.LogWarningFormat("FyberVideoLoaded.Result: {0}", new object[]
					{
						FreeAwardController.FyberVideoLoaded.Result
					});
					return;
				}
				Action<AdResult> adFinished = null;
				adFinished = delegate(AdResult adResult)
				{
					FyberCallback.AdFinished -= adFinished;
					FreeAwardController.WatchingState.LogImpressionDetails(advertisementInfo);
					if (adResult.Message == "CLOSE_FINISHED")
					{
						AnalyticsFacade.SendCustomEventToFacebook("rewarded_ads_watched_count", null);
					}
					this._adClosed.SetResult(adResult.Message);
				};
				FyberCallback.AdFinished += adFinished;
				ad.Start();
				FreeAwardController.FyberVideoLoaded = null;
				Dictionary<string, string> eventParams = new Dictionary<string, string>
				{
					{
						"af_content_type",
						"Rewarded video"
					},
					{
						"af_content_id",
						string.Format("Rewarded video ({0})", arg)
					}
				};
				AnalyticsFacade.SendCustomEventToAppsFlyer("af_content_view", eventParams);
			}
		}

		// Token: 0x060034E9 RID: 13545 RVA: 0x001118A0 File Offset: 0x0010FAA0
		private static void LogImpressionDetails(AdvertisementInfo advertisementInfo)
		{
			if (advertisementInfo == null)
			{
				advertisementInfo = AdvertisementInfo.Default;
			}
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("Round {0}", advertisementInfo.Round + 1);
			stringBuilder.AppendFormat(", Slot {0} ({1})", advertisementInfo.Slot + 1, AnalyticsHelper.GetAdProviderName(FreeAwardController.Instance.GetProviderByIndex(advertisementInfo.Slot)));
			if (InterstitialManager.Instance.Provider == AdProvider.GoogleMobileAds)
			{
				stringBuilder.AppendFormat(", Unit {0}", advertisementInfo.Unit + 1);
			}
			if (string.IsNullOrEmpty(advertisementInfo.Details))
			{
				stringBuilder.Append(" - Impression");
			}
			else
			{
				stringBuilder.AppendFormat(" - Impression: {0}", advertisementInfo.Details);
			}
		}

		// Token: 0x170008CB RID: 2251
		// (get) Token: 0x060034EA RID: 13546 RVA: 0x00111964 File Offset: 0x0010FB64
		public Future<string> AdClosed
		{
			get
			{
				return this._adClosed.Future;
			}
		}

		// Token: 0x060034EB RID: 13547 RVA: 0x00111974 File Offset: 0x0010FB74
		internal void SimulateCallbackInEditor(string result)
		{
			if (Application.isEditor)
			{
				this._adClosed.SetResult(result ?? string.Empty);
			}
		}

		// Token: 0x170008CC RID: 2252
		// (get) Token: 0x060034EC RID: 13548 RVA: 0x001119A4 File Offset: 0x0010FBA4
		public float StartTime
		{
			get
			{
				return this._startTime;
			}
		}

		// Token: 0x060034ED RID: 13549 RVA: 0x001119AC File Offset: 0x0010FBAC
		private static string DetermineContext()
		{
			if (!(BankController.Instance != null) || !BankController.Instance.InterfaceEnabled)
			{
				return "At Lobby";
			}
			if (Defs.isMulti)
			{
				return "Bank (Multiplayer)";
			}
			if (Defs.isCompany)
			{
				return "Bank (Campaign)";
			}
			if (Defs.IsSurvival)
			{
				return "Bank (Survival)";
			}
			return "Bank";
		}

		// Token: 0x040026BD RID: 9917
		private readonly float _startTime;

		// Token: 0x040026BE RID: 9918
		private readonly Promise<string> _adClosed = new Promise<string>();
	}

	// Token: 0x02000606 RID: 1542
	public sealed class ConnectionState : FreeAwardController.State
	{
		// Token: 0x060034EE RID: 13550 RVA: 0x00111A14 File Offset: 0x0010FC14
		public ConnectionState()
		{
			this._startTime = Time.realtimeSinceStartup;
		}

		// Token: 0x170008CD RID: 2253
		// (get) Token: 0x060034EF RID: 13551 RVA: 0x00111A28 File Offset: 0x0010FC28
		public float StartTime
		{
			get
			{
				return this._startTime;
			}
		}

		// Token: 0x040026BF RID: 9919
		private readonly float _startTime;
	}

	// Token: 0x02000607 RID: 1543
	public sealed class AwardState : FreeAwardController.State
	{
		// Token: 0x060034F0 RID: 13552 RVA: 0x00111A30 File Offset: 0x0010FC30
		private AwardState()
		{
		}

		// Token: 0x170008CE RID: 2254
		// (get) Token: 0x060034F2 RID: 13554 RVA: 0x00111A44 File Offset: 0x0010FC44
		internal static FreeAwardController.AwardState Instance
		{
			get
			{
				return FreeAwardController.AwardState._instance;
			}
		}

		// Token: 0x040026C0 RID: 9920
		private static readonly FreeAwardController.AwardState _instance = new FreeAwardController.AwardState();
	}

	// Token: 0x02000608 RID: 1544
	public sealed class CloseState : FreeAwardController.State
	{
		// Token: 0x060034F3 RID: 13555 RVA: 0x00111A4C File Offset: 0x0010FC4C
		private CloseState()
		{
		}

		// Token: 0x170008CF RID: 2255
		// (get) Token: 0x060034F5 RID: 13557 RVA: 0x00111A60 File Offset: 0x0010FC60
		internal static FreeAwardController.CloseState Instance
		{
			get
			{
				return FreeAwardController.CloseState._instance;
			}
		}

		// Token: 0x040026C1 RID: 9921
		private static readonly FreeAwardController.CloseState _instance = new FreeAwardController.CloseState();
	}
}
