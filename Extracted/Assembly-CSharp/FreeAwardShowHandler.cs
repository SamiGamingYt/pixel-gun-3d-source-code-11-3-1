using System;
using System.Collections;
using System.Collections.Generic;
using I2.Loc;
using Rilisoft;
using Rilisoft.NullExtensions;
using UnityEngine;

// Token: 0x0200060B RID: 1547
[DisallowMultipleComponent]
internal sealed class FreeAwardShowHandler : MonoBehaviour
{
	// Token: 0x060034F6 RID: 13558 RVA: 0x00111A68 File Offset: 0x0010FC68
	private FreeAwardShowHandler()
	{
	}

	// Token: 0x170008D0 RID: 2256
	// (get) Token: 0x060034F7 RID: 13559 RVA: 0x00111A70 File Offset: 0x0010FC70
	// (set) Token: 0x060034F8 RID: 13560 RVA: 0x00111A78 File Offset: 0x0010FC78
	public static FreeAwardShowHandler Instance { get; private set; }

	// Token: 0x170008D1 RID: 2257
	// (get) Token: 0x060034F9 RID: 13561 RVA: 0x00111A80 File Offset: 0x0010FC80
	private NickLabelController NickLabel
	{
		get
		{
			if (this._nickLabelValue == null)
			{
				if (NickLabelStack.sharedStack != null)
				{
					this._nickLabelValue = NickLabelStack.sharedStack.GetNextCurrentLabel();
				}
				if (this._nickLabelValue != null)
				{
					this._nickLabelValue.StartShow(NickLabelController.TypeNickLabel.FreeCoins, base.gameObject.transform);
				}
			}
			return this._nickLabelValue;
		}
	}

	// Token: 0x170008D2 RID: 2258
	// (get) Token: 0x060034FA RID: 13562 RVA: 0x00111AEC File Offset: 0x0010FCEC
	// (set) Token: 0x060034FB RID: 13563 RVA: 0x00111B18 File Offset: 0x0010FD18
	public bool IsInteractable
	{
		get
		{
			return !(this.HitCollider == null) && this.HitCollider.enabled;
		}
		set
		{
			if (this.HitCollider == null)
			{
				return;
			}
			this.HitCollider.enabled = value;
		}
	}

	// Token: 0x060034FC RID: 13564 RVA: 0x00111B38 File Offset: 0x0010FD38
	private void OnLocalizationChanged()
	{
	}

	// Token: 0x060034FD RID: 13565 RVA: 0x00111B3C File Offset: 0x0010FD3C
	private FreeAwardShowHandler.SkipReason GetReasonToDisableRewardedVideoInterface()
	{
		if (FriendsWindowGUI.Instance != null && FriendsWindowGUI.Instance.InterfaceEnabled)
		{
			return FreeAwardShowHandler.SkipReason.FriendsInterfaceEnabled;
		}
		if (BankController.Instance != null && BankController.Instance.InterfaceEnabled)
		{
			return FreeAwardShowHandler.SkipReason.BankInterfaceEnabled;
		}
		if (ShopNGUIController.sharedShop != null && ShopNGUIController.GuiActive)
		{
			return FreeAwardShowHandler.SkipReason.ShopInterfaceEnabled;
		}
		if (FreeAwardController.Instance != null)
		{
			if (!FreeAwardController.Instance.IsInState<FreeAwardController.IdleState>())
			{
				return FreeAwardShowHandler.SkipReason.RewardedVideoInterfaceEnabled;
			}
			if (!FreeAwardController.Instance.AdvertCountLessThanLimit())
			{
				return FreeAwardShowHandler.SkipReason.AdvertCountLessThanLimit;
			}
			if (FreeAwardController.Instance.TimeTamperingDetected())
			{
				return FreeAwardShowHandler.SkipReason.TimeTamperingDetected;
			}
		}
		if (BannerWindowController.SharedController != null && BannerWindowController.SharedController.IsAnyBannerShown)
		{
			return FreeAwardShowHandler.SkipReason.BannerEnabled;
		}
		if (AskNameManager.instance != null && !AskNameManager.isComplete)
		{
			return FreeAwardShowHandler.SkipReason.AskNameWindow;
		}
		MainMenuController sharedController = MainMenuController.sharedController;
		if (sharedController != null)
		{
			if (sharedController.stubLoading.activeInHierarchy)
			{
				return FreeAwardShowHandler.SkipReason.MainMenuComponentEnabled;
			}
			if (sharedController.FreePanelIsActive)
			{
				return FreeAwardShowHandler.SkipReason.MainMenuComponentEnabled;
			}
			if (sharedController.SettingsJoysticksPanel.activeInHierarchy || sharedController.settingsPanel.activeInHierarchy)
			{
				return FreeAwardShowHandler.SkipReason.MainMenuComponentEnabled;
			}
			if (sharedController.singleModePanel.gameObject.activeInHierarchy)
			{
				return FreeAwardShowHandler.SkipReason.MainMenuComponentEnabled;
			}
			if (sharedController.RentExpiredPoint.Map((Transform r) => r.childCount > 0))
			{
				return FreeAwardShowHandler.SkipReason.MainMenuComponentEnabled;
			}
			if (FeedbackMenuController.Instance != null && FeedbackMenuController.Instance.gameObject.activeInHierarchy)
			{
				return FreeAwardShowHandler.SkipReason.MainMenuComponentEnabled;
			}
		}
		if (LeaderboardScript.Instance != null && LeaderboardScript.Instance.UIEnabled)
		{
			return FreeAwardShowHandler.SkipReason.LeaderboardEnabled;
		}
		if (FriendsController.sharedController.Map((FriendsController c) => c.ProfileInterfaceActive))
		{
			return FreeAwardShowHandler.SkipReason.ProfileEnabled;
		}
		if (FriendsController.ServerTime == -1L)
		{
			return FreeAwardShowHandler.SkipReason.ServerTime;
		}
		if (NewsLobbyController.sharedController != null && NewsLobbyController.sharedController.gameObject.activeInHierarchy)
		{
			return FreeAwardShowHandler.SkipReason.NewsEnabled;
		}
		if (ExpController.Instance != null && ExpController.Instance.IsLevelUpShown)
		{
			return FreeAwardShowHandler.SkipReason.LevelUpShown;
		}
		if (!TrainingController.TrainingCompleted)
		{
			return FreeAwardShowHandler.SkipReason.TrainingMotCompleted;
		}
		return FreeAwardShowHandler.SkipReason.None;
	}

	// Token: 0x060034FE RID: 13566 RVA: 0x00111DA0 File Offset: 0x0010FFA0
	private void Awake()
	{
		FreeAwardShowHandler.Instance = this;
		if (FreeAwardController.Instance == null && this.freeAwardGuiPrefab != null)
		{
			UnityEngine.Object.Instantiate(this.freeAwardGuiPrefab, Vector3.zero, Quaternion.identity);
		}
		LocalizationManager.OnLocalizeEvent += this.OnLocalizationChanged;
	}

	// Token: 0x060034FF RID: 13567 RVA: 0x00111DFC File Offset: 0x0010FFFC
	private void OnEnable()
	{
		if (this._collider == null)
		{
			this._collider = base.gameObject.GetComponent<Collider>();
		}
		FreeAwardShowHandler.SkipReason reasonToDisableRewardedVideoInterface = this.GetReasonToDisableRewardedVideoInterface();
		if (reasonToDisableRewardedVideoInterface != FreeAwardShowHandler.SkipReason.None)
		{
			Debug.LogFormat("Reason to disable rewarded video interface in lobby: {0}", new object[]
			{
				reasonToDisableRewardedVideoInterface
			});
			this.SetInterfaceActive(false);
			return;
		}
		string reasonToDismissVideoChestInLobby = MobileAdManager.GetReasonToDismissVideoChestInLobby();
		if (!string.IsNullOrEmpty(reasonToDismissVideoChestInLobby))
		{
			Debug.LogFormat("Reason to dismiss rewarded video in lobby: {0}", new object[]
			{
				reasonToDismissVideoChestInLobby
			});
			this.SetInterfaceActive(false);
			return;
		}
		this.SetInterfaceActive(true);
		base.StartCoroutine(this.SetupBillboard());
	}

	// Token: 0x06003500 RID: 13568 RVA: 0x00111E9C File Offset: 0x0011009C
	private void Update()
	{
		FreeAwardShowHandler.SkipReason reasonToDisableRewardedVideoInterface = this.GetReasonToDisableRewardedVideoInterface();
		if (reasonToDisableRewardedVideoInterface != FreeAwardShowHandler.SkipReason.None)
		{
			this.SetInterfaceActive(false);
			if (Application.isEditor && reasonToDisableRewardedVideoInterface != this._lastSkipReasonLogged)
			{
				Debug.LogFormat("Reason to disable rewarded video interface: {0}", new object[]
				{
					reasonToDisableRewardedVideoInterface
				});
				this._lastSkipReasonLogged = reasonToDisableRewardedVideoInterface;
			}
			return;
		}
		if (Time.frameCount % 60 == 0)
		{
			string reasonToDismissVideoChestInLobby = MobileAdManager.GetReasonToDismissVideoChestInLobby();
			this._cachedCheckIfVideoEnabled = string.IsNullOrEmpty(reasonToDismissVideoChestInLobby);
		}
		this.SetInterfaceActive(this._cachedCheckIfVideoEnabled);
	}

	// Token: 0x06003501 RID: 13569 RVA: 0x00111F20 File Offset: 0x00110120
	private void OnDestroy()
	{
		LocalizationManager.OnLocalizeEvent -= this.OnLocalizationChanged;
		FreeAwardShowHandler.Instance = null;
	}

	// Token: 0x06003502 RID: 13570 RVA: 0x00111F3C File Offset: 0x0011013C
	private IEnumerator SetupBillboard()
	{
		while (MainMenuController.sharedController == null)
		{
			yield return null;
		}
		MainMenuController.sharedController.FreeAwardBindedBillboard.BindTo(() => base.gameObject.transform);
		this._collider = MainMenuController.sharedController.FreeAwardBindedBillboard.Collider;
		yield break;
	}

	// Token: 0x06003503 RID: 13571 RVA: 0x00111F58 File Offset: 0x00110158
	private void SetInterfaceActive(bool active)
	{
		if (this.HitCollider == null)
		{
			this.chestModelCoins.SetActiveSafe(false);
			this.chestModelGems.SetActiveSafe(false);
			this.NickLabel.gameObject.SetActiveSafe(false);
			return;
		}
		if (!active)
		{
			this.HitCollider.enabled = false;
			this.chestModelCoins.SetActiveSafe(false);
			this.chestModelGems.SetActiveSafe(false);
			this.NickLabel.gameObject.SetActiveSafe(false);
			return;
		}
		if (FreeAwardController.Instance == null)
		{
			this.HitCollider.enabled = false;
			this.chestModelCoins.SetActiveSafe(false);
			this.chestModelGems.SetActiveSafe(false);
			this.NickLabel.gameObject.SetActiveSafe(false);
			return;
		}
		this.HitCollider.enabled = true;
		this.chestModelCoins.SetActiveSafe(FreeAwardController.Instance.CurrencyForAward == "Coins");
		this.chestModelGems.SetActiveSafe(FreeAwardController.Instance.CurrencyForAward == "GemsCurrency");
		if (!this.NickLabel.gameObject.activeInHierarchy)
		{
			this.NickLabel.gameObject.SetActive(true);
			if (FreeAwardController.Instance.CurrencyForAward == "GemsCurrency")
			{
				this.NickLabel.freeAwardGemsLabel.SetActive(true);
				this.NickLabel.freeAwardCoinsLabel.SetActive(false);
			}
			else if (FreeAwardController.Instance.CurrencyForAward == "Coins")
			{
				this.NickLabel.freeAwardGemsLabel.SetActive(false);
				this.NickLabel.freeAwardCoinsLabel.SetActive(true);
			}
			else
			{
				this.NickLabel.freeAwardGemsLabel.SetActive(false);
				this.NickLabel.freeAwardCoinsLabel.SetActive(false);
			}
		}
	}

	// Token: 0x06003504 RID: 13572 RVA: 0x00112134 File Offset: 0x00110334
	public void OnClick()
	{
		FreeAwardShowHandler.SkipReason reasonToDisableRewardedVideoInterface = this.GetReasonToDisableRewardedVideoInterface();
		if (reasonToDisableRewardedVideoInterface != FreeAwardShowHandler.SkipReason.None)
		{
			Debug.LogFormat("Reason to disabled rewarded video interface: {0}", new object[]
			{
				reasonToDisableRewardedVideoInterface
			});
			return;
		}
		AdsConfigMemento lastLoadedConfig = AdsConfigManager.Instance.LastLoadedConfig;
		if (lastLoadedConfig == null)
		{
			Debug.LogWarning("config == null");
			return;
		}
		if (lastLoadedConfig.Exception != null)
		{
			Debug.LogWarning(lastLoadedConfig.Exception.Message);
			return;
		}
		string videoDisabledReason = AdsConfigManager.GetVideoDisabledReason(lastLoadedConfig);
		if (!string.IsNullOrEmpty(videoDisabledReason))
		{
			Debug.LogWarning(videoDisabledReason);
			return;
		}
		ChestInLobbyPointMemento chestInLobby = lastLoadedConfig.AdPointsConfig.ChestInLobby;
		if (chestInLobby == null)
		{
			Debug.LogWarning("pointConfig == null");
			return;
		}
		string playerCategory = AdsConfigManager.GetPlayerCategory(lastLoadedConfig);
		List<double> finalRewardedVideoDelayMinutes = chestInLobby.GetFinalRewardedVideoDelayMinutes(playerCategory);
		if (finalRewardedVideoDelayMinutes.Count == 0)
		{
			return;
		}
		DateTime date = DateTime.UtcNow.Date;
		KeyValuePair<int, DateTime> keyValuePair = FreeAwardController.Instance.LastAdvertShow(date);
		int num = Math.Max(0, keyValuePair.Key + 1);
		if (num > finalRewardedVideoDelayMinutes.Count)
		{
			return;
		}
		DateTime d = (!(keyValuePair.Value < date)) ? keyValuePair.Value : date;
		TimeSpan t = TimeSpan.FromMinutes(finalRewardedVideoDelayMinutes[num]);
		DateTime watchState = d + t;
		FreeAwardController.Instance.SetWatchState(watchState);
		if (ButtonClickSound.Instance != null)
		{
			ButtonClickSound.Instance.PlayClick();
		}
	}

	// Token: 0x06003505 RID: 13573 RVA: 0x00112298 File Offset: 0x00110498
	private void PlayeAnimationTitle(bool isReverse, GameObject titleLabel)
	{
		UIPlayTween component = titleLabel.GetComponent<UIPlayTween>();
		component.resetOnPlay = true;
		component.tweenGroup = ((!isReverse) ? 0 : 1);
		component.Play(true);
	}

	// Token: 0x06003506 RID: 13574 RVA: 0x001122D0 File Offset: 0x001104D0
	private void PlayAnimationShowChest(bool isReserse)
	{
		GameObject gameObject = (!(FreeAwardController.Instance.CurrencyForAward == "GemsCurrency")) ? this.chestModelCoins : this.chestModelGems;
		Animation component = gameObject.GetComponent<Animation>();
		if (component == null)
		{
			return;
		}
		if (isReserse)
		{
			component["Animate"].speed = -1f;
			component["Animate"].time = component["Animate"].length;
		}
		else
		{
			component["Animate"].speed = 1f;
			component["Animate"].time = 0f;
		}
		component.Play();
	}

	// Token: 0x06003507 RID: 13575 RVA: 0x00112390 File Offset: 0x00110590
	public static void CheckShowChest(bool interfaceEnabled)
	{
		if (!interfaceEnabled)
		{
			return;
		}
		if (FreeAwardShowHandler.Instance == null)
		{
			return;
		}
		Collider hitCollider = FreeAwardShowHandler.Instance.HitCollider;
		if (hitCollider == null)
		{
			return;
		}
		if (!hitCollider.enabled)
		{
			return;
		}
		FreeAwardShowHandler.Instance.SetInterfaceActive(false);
	}

	// Token: 0x170008D3 RID: 2259
	// (get) Token: 0x06003508 RID: 13576 RVA: 0x001123E4 File Offset: 0x001105E4
	private Collider HitCollider
	{
		get
		{
			return this._collider;
		}
	}

	// Token: 0x040026CB RID: 9931
	public GameObject chestModelCoins;

	// Token: 0x040026CC RID: 9932
	public GameObject chestModelGems;

	// Token: 0x040026CD RID: 9933
	public GameObject freeAwardGuiPrefab;

	// Token: 0x040026CE RID: 9934
	private NickLabelController _nickLabelValue;

	// Token: 0x040026CF RID: 9935
	private FreeAwardShowHandler.SkipReason _lastSkipReasonLogged;

	// Token: 0x040026D0 RID: 9936
	private bool _cachedCheckIfVideoEnabled;

	// Token: 0x040026D1 RID: 9937
	private Collider _collider;

	// Token: 0x0200060C RID: 1548
	private enum SkipReason
	{
		// Token: 0x040026D6 RID: 9942
		None,
		// Token: 0x040026D7 RID: 9943
		CameraTouchOverGui,
		// Token: 0x040026D8 RID: 9944
		FriendsInterfaceEnabled,
		// Token: 0x040026D9 RID: 9945
		BankInterfaceEnabled,
		// Token: 0x040026DA RID: 9946
		ShopInterfaceEnabled,
		// Token: 0x040026DB RID: 9947
		RewardedVideoInterfaceEnabled,
		// Token: 0x040026DC RID: 9948
		BannerEnabled,
		// Token: 0x040026DD RID: 9949
		MainMenuComponentEnabled,
		// Token: 0x040026DE RID: 9950
		LeaderboardEnabled,
		// Token: 0x040026DF RID: 9951
		ProfileEnabled,
		// Token: 0x040026E0 RID: 9952
		NewsEnabled,
		// Token: 0x040026E1 RID: 9953
		LevelUpShown,
		// Token: 0x040026E2 RID: 9954
		AskNameWindow,
		// Token: 0x040026E3 RID: 9955
		TrainingMotCompleted,
		// Token: 0x040026E4 RID: 9956
		AdvertCountLessThanLimit,
		// Token: 0x040026E5 RID: 9957
		TimeTamperingDetected,
		// Token: 0x040026E6 RID: 9958
		ServerTime
	}
}
