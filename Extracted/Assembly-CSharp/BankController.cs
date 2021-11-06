using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Rilisoft;
using Rilisoft.NullExtensions;
using UnityEngine;

// Token: 0x0200065B RID: 1627
internal sealed class BankController : MonoBehaviour
{
	// Token: 0x1400006A RID: 106
	// (add) Token: 0x06003899 RID: 14489 RVA: 0x00124CA8 File Offset: 0x00122EA8
	// (remove) Token: 0x0600389A RID: 14490 RVA: 0x00124CC0 File Offset: 0x00122EC0
	public static event Action onUpdateMoney;

	// Token: 0x1400006B RID: 107
	// (add) Token: 0x0600389B RID: 14491 RVA: 0x00124CD8 File Offset: 0x00122ED8
	// (remove) Token: 0x0600389C RID: 14492 RVA: 0x00124D1C File Offset: 0x00122F1C
	public event EventHandler BackRequested
	{
		add
		{
			if (this.bankViewCommon != null)
			{
				this.bankViewCommon.BackButtonPressed += value;
			}
			this.EscapePressed = (EventHandler)Delegate.Combine(this.EscapePressed, value);
		}
		remove
		{
			if (this.bankViewCommon != null)
			{
				this.bankViewCommon.BackButtonPressed -= value;
			}
			this.EscapePressed = (EventHandler)Delegate.Remove(this.EscapePressed, value);
		}
	}

	// Token: 0x1400006C RID: 108
	// (add) Token: 0x0600389D RID: 14493 RVA: 0x00124D60 File Offset: 0x00122F60
	// (remove) Token: 0x0600389E RID: 14494 RVA: 0x00124D7C File Offset: 0x00122F7C
	private event EventHandler EscapePressed;

	// Token: 0x0600389F RID: 14495 RVA: 0x00124D98 File Offset: 0x00122F98
	public static void UpdateAllIndicatorsMoney()
	{
		if (BankController.onUpdateMoney != null)
		{
			BankController.onUpdateMoney();
		}
	}

	// Token: 0x060038A0 RID: 14496 RVA: 0x00124DB0 File Offset: 0x00122FB0
	public static void GiveInitialNumOfCoins()
	{
		if (!Storager.hasKey("Coins"))
		{
			Storager.setInt("Coins", 0, false);
			if (Application.platform != RuntimePlatform.IPhonePlayer)
			{
				PlayerPrefs.Save();
			}
		}
		if (!Storager.hasKey("GemsCurrency"))
		{
			switch (BuildSettings.BuildTargetPlatform)
			{
			case RuntimePlatform.IPhonePlayer:
				Storager.setInt("GemsCurrency", 0, false);
				break;
			case RuntimePlatform.Android:
				Storager.setInt("GemsCurrency", 0, false);
				break;
			}
			if (Application.platform != RuntimePlatform.IPhonePlayer)
			{
				PlayerPrefs.Save();
			}
		}
	}

	// Token: 0x17000952 RID: 2386
	// (get) Token: 0x060038A1 RID: 14497 RVA: 0x00124E54 File Offset: 0x00123054
	private AbstractBankView ActualBankView
	{
		get
		{
			return this.bankViewCommon;
		}
	}

	// Token: 0x17000953 RID: 2387
	// (get) Token: 0x060038A2 RID: 14498 RVA: 0x00124E5C File Offset: 0x0012305C
	// (set) Token: 0x060038A3 RID: 14499 RVA: 0x00124E64 File Offset: 0x00123064
	public AbstractBankView CurrentBankView
	{
		get
		{
			return this.m_currentBankView;
		}
		private set
		{
			this.m_currentBankView = value;
		}
	}

	// Token: 0x17000954 RID: 2388
	// (get) Token: 0x060038A4 RID: 14500 RVA: 0x00124E70 File Offset: 0x00123070
	// (set) Token: 0x060038A5 RID: 14501 RVA: 0x00124EA4 File Offset: 0x001230A4
	public bool InterfaceEnabled
	{
		get
		{
			return this.CurrentBankView != null && this.CurrentBankView.gameObject.activeInHierarchy;
		}
		set
		{
			this.SetInterfaceEnabledWithDesiredCurrency(value, null);
		}
	}

	// Token: 0x060038A6 RID: 14502 RVA: 0x00124EB0 File Offset: 0x001230B0
	public void SetInterfaceEnabledWithDesiredCurrency(bool enabled, string desiredCurrency)
	{
		this.SetInterfaceEnabledCore(enabled, desiredCurrency);
	}

	// Token: 0x17000955 RID: 2389
	// (get) Token: 0x060038A7 RID: 14503 RVA: 0x00124EBC File Offset: 0x001230BC
	public bool InterfaceEnabledCoroutineLocked
	{
		get
		{
			return this._lockInterfaceEnabledCoroutine;
		}
	}

	// Token: 0x060038A8 RID: 14504 RVA: 0x00124EC4 File Offset: 0x001230C4
	private void SetInterfaceEnabledCore(bool value, string desiredCurrency)
	{
		if (!value && this._backSubscription != null)
		{
			this._backSubscription.Dispose();
			this._backSubscription = null;
		}
		this._lockInterfaceEnabledCoroutine = true;
		int num = this._counterEn++;
		Debug.Log(string.Concat(new object[]
		{
			"InterfaceEnabledCoroutine ",
			num,
			" start: ",
			value
		}));
		try
		{
			if (value && !this.firsEnterToBankOccured)
			{
				this.firsEnterToBankOccured = true;
				this.tmOfFirstEnterTheBank = Time.realtimeSinceStartup;
			}
			if (value)
			{
				this.UpdateCurrenntInappBonus(null);
			}
			if (this.ActualBankView != this.CurrentBankView && this.CurrentBankView != null)
			{
				this.CurrentBankView.gameObject.SetActive(false);
				this.CurrentBankView = null;
			}
			if (this.ActualBankView != null)
			{
				if (value)
				{
					this.ActualBankView.DesiredCurrency = desiredCurrency;
				}
				this.ActualBankView.gameObject.SetActive(value);
				this.CurrentBankView = ((!value) ? null : this.ActualBankView);
			}
			this.uiRoot.SetActive(value);
			if (!value)
			{
				ActivityIndicator.IsActiveIndicator = false;
			}
			FreeAwardShowHandler.CheckShowChest(value);
			if (value)
			{
				coinsShop.thisScript.RefreshProductsIfNeed(false);
				this.OnEventX3AmazonBonusUpdated();
			}
		}
		finally
		{
			if (value)
			{
				if (this._backSubscription != null)
				{
					this._backSubscription.Dispose();
				}
				this._backSubscription = BackSystem.Instance.Register(new Action(this.HandleEscape), "Bank");
			}
			if (Device.IsLoweMemoryDevice)
			{
				ActivityIndicator.ClearMemory();
			}
			this._lockInterfaceEnabledCoroutine = false;
			Debug.Log(string.Concat(new object[]
			{
				"InterfaceEnabledCoroutine ",
				num,
				" stop: ",
				value
			}));
		}
	}

	// Token: 0x060038A9 RID: 14505 RVA: 0x001250D8 File Offset: 0x001232D8
	private void HandleEscape()
	{
		if (FreeAwardController.Instance != null && !FreeAwardController.Instance.IsInState<FreeAwardController.IdleState>())
		{
			FreeAwardController.Instance.HandleClose();
			this._escapePressed = false;
			return;
		}
		this._escapePressed = true;
	}

	// Token: 0x17000956 RID: 2390
	// (get) Token: 0x060038AA RID: 14506 RVA: 0x00125120 File Offset: 0x00123320
	public static BankController Instance
	{
		get
		{
			return BankController._instance;
		}
	}

	// Token: 0x060038AB RID: 14507 RVA: 0x00125128 File Offset: 0x00123328
	private void Awake()
	{
		BalanceController.UpdatedBankView += this.BalanceController_UpdatedBankView;
		InappBonuessController.OnGiveInappBonus += this.InappBonuessController_OnGiveInappBonus;
	}

	// Token: 0x060038AC RID: 14508 RVA: 0x00125158 File Offset: 0x00123358
	private void InappBonuessController_OnGiveInappBonus(InappRememberedBonus bonus)
	{
		this.TimeWhenReturnedFromPause = float.MinValue;
	}

	// Token: 0x060038AD RID: 14509 RVA: 0x00125168 File Offset: 0x00123368
	private void Start()
	{
		BankController._instance = this;
		PromoActionsManager.EventX3Updated += this.OnEventX3Updated;
		if (this.bankViewCommon != null)
		{
			this.bankViewCommon.PurchaseButtonPressed += this.HandlePurchaseButtonPressed;
		}
		PromoActionsManager.EventAmazonX3Updated += this.OnEventX3AmazonBonusUpdated;
		this.bankViewCommon.freeAwardButton.gameObject.SetActiveSafeSelf(false);
	}

	// Token: 0x060038AE RID: 14510 RVA: 0x001251DC File Offset: 0x001233DC
	private void BalanceController_UpdatedBankView()
	{
		try
		{
			this.CurrentInappBonuses = BalanceController.GetCurrentInnapBonus();
			if (this.CurrentBankView != null && this.InterfaceEnabled)
			{
				this.CurrentBankView.UpdateUi();
			}
		}
		catch (Exception ex)
		{
			Debug.LogErrorFormat("Exception in BalanceController_UpdatedBankView: {0}", new object[]
			{
				ex
			});
		}
	}

	// Token: 0x060038AF RID: 14511 RVA: 0x00125258 File Offset: 0x00123458
	private void OnEventX3Updated()
	{
		try
		{
			if (this.CurrentBankView != null)
			{
				this.CurrentBankView.UpdateUi();
			}
		}
		catch (Exception ex)
		{
			Debug.LogErrorFormat("Exception in OnEventX3Updated: {0}", new object[]
			{
				ex
			});
		}
	}

	// Token: 0x060038B0 RID: 14512 RVA: 0x001252BC File Offset: 0x001234BC
	private void OnEventX3AmazonBonusUpdated()
	{
		if (this.CurrentBankView == null || this.CurrentBankView.eventX3AmazonBonusWidget == null)
		{
			return;
		}
		try
		{
			GameObject gameObject = this.CurrentBankView.eventX3AmazonBonusWidget.gameObject;
			gameObject.SetActive(PromoActionsManager.sharedManager.IsAmazonEventX3Active);
			UILabel[] componentsInChildren = gameObject.GetComponentsInChildren<UILabel>();
			UILabel uilabel;
			if ((uilabel = this.CurrentBankView.Map((AbstractBankView b) => b.amazonEventCaptionLabel)) == null)
			{
				uilabel = componentsInChildren.FirstOrDefault((UILabel l) => "CaptionLabel".Equals(l.name, StringComparison.OrdinalIgnoreCase));
			}
			UILabel uilabel2 = uilabel;
			PromoActionsManager.AmazonEventInfo o = PromoActionsManager.sharedManager.Map((PromoActionsManager p) => p.AmazonEvent);
			if (uilabel2 != null)
			{
				uilabel2.text = (o.Map((PromoActionsManager.AmazonEventInfo e) => e.Caption) ?? string.Empty);
			}
			UILabel uilabel3;
			if ((uilabel3 = this.CurrentBankView.Map((AbstractBankView b) => b.amazonEventTitleLabel)) == null)
			{
				uilabel3 = componentsInChildren.FirstOrDefault((UILabel l) => "TitleLabel".Equals(l.name, StringComparison.OrdinalIgnoreCase));
			}
			UILabel o2 = uilabel3;
			UILabel[] array = o2.Map((UILabel t) => t.GetComponentsInChildren<UILabel>()) ?? new UILabel[0];
			float num = o.Map((PromoActionsManager.AmazonEventInfo e) => e.Percentage);
			string text = LocalizationStore.Get("Key_1672");
			foreach (UILabel uilabel4 in array)
			{
				uilabel4.text = ("Key_1672".Equals(text, StringComparison.OrdinalIgnoreCase) ? string.Empty : string.Format(text, num));
			}
		}
		catch (Exception ex)
		{
			Debug.LogErrorFormat("Exception in OnEventX3AmazonBonusUpdated: {0}", new object[]
			{
				ex
			});
		}
	}

	// Token: 0x060038B1 RID: 14513 RVA: 0x00125520 File Offset: 0x00123720
	private void UpdateCurrenntInappBonus(Action onUpdate)
	{
		List<Dictionary<string, object>> currentInnapBonus = BalanceController.GetCurrentInnapBonus();
		if (!InappBonuessController.AreInappBonusesEquals(currentInnapBonus, this.CurrentInappBonuses))
		{
			if (onUpdate != null)
			{
				onUpdate();
			}
			this.CurrentInappBonuses = currentInnapBonus;
		}
	}

	// Token: 0x060038B2 RID: 14514 RVA: 0x00125558 File Offset: 0x00123758
	private void CheckInappBonusActionChanged()
	{
		try
		{
			Action onUpdate = delegate()
			{
				if (this.InterfaceEnabled && this.CurrentBankView != null)
				{
					this.CurrentBankView.UpdateUi();
				}
			};
			this.UpdateCurrenntInappBonus(onUpdate);
		}
		catch (Exception ex)
		{
			Debug.LogErrorFormat("Exception in BankController.Update: {0}", new object[]
			{
				ex
			});
		}
	}

	// Token: 0x060038B3 RID: 14515 RVA: 0x001255B4 File Offset: 0x001237B4
	private void OnApplicationPause(bool pause)
	{
		if (!pause)
		{
			this.TimeWhenReturnedFromPause = Time.realtimeSinceStartup;
		}
	}

	// Token: 0x060038B4 RID: 14516 RVA: 0x001255C8 File Offset: 0x001237C8
	private void Update()
	{
		if (!this.InterfaceEnabled)
		{
			this._escapePressed = false;
			return;
		}
		if (FriendsController.ServerTime != -1L || Time.realtimeSinceStartup - this.TimeWhenReturnedFromPause > 10f)
		{
			this.CheckInappBonusActionChanged();
		}
		string text = string.Empty;
		if (FreeAwardController.Instance == null)
		{
			this.CurrentBankView.freeAwardButton.gameObject.SetActiveSafeSelf(false);
		}
		else if (Defs.MainMenuScene != SceneManagerHelper.ActiveSceneName)
		{
			this.CurrentBankView.freeAwardButton.gameObject.SetActiveSafeSelf(false);
		}
		else if (!FreeAwardController.Instance.IsInState<FreeAwardController.IdleState>())
		{
			this.CurrentBankView.freeAwardButton.gameObject.SetActiveSafeSelf(false);
		}
		else if (FriendsController.ServerTime == -1L)
		{
			this.CurrentBankView.freeAwardButton.gameObject.SetActiveSafeSelf(false);
		}
		else if (!TrainingController.TrainingCompleted)
		{
			this.CurrentBankView.freeAwardButton.gameObject.SetActiveSafeSelf(false);
		}
		else
		{
			text = MobileAdManager.GetReasonToDismissVideoChestInLobby();
			if (string.IsNullOrEmpty(text))
			{
				if (this._timeTamperingDetected.Value)
				{
					text = "Time tampering detected.";
				}
				else if (!FreeAwardController.Instance.AdvertCountLessThanLimit())
				{
					text = "AdvertCountLessThanLimit == false";
					this.CurrentBankView.freeAwardButton.gameObject.SetActiveSafeSelf(false);
				}
				else
				{
					this.CurrentBankView.freeAwardButton.gameObject.SetActiveSafeSelf(true);
				}
			}
		}
		if (!string.IsNullOrEmpty(text))
		{
			this.CurrentBankView.freeAwardButton.gameObject.SetActiveSafeSelf(false);
			if (text != this.m_lastPrintedDismissReason)
			{
				string format = (!Application.isEditor) ? "Dismissing interstitial. {0}" : "<color=magenta>Dismissing interstitial. {0}</color>";
				Debug.LogFormat(format, new object[]
				{
					text
				});
				this.m_lastPrintedDismissReason = text;
			}
		}
		this.UpdateMiscUiOnView(this.bankViewCommon);
		EventHandler escapePressed = this.EscapePressed;
		if (this._escapePressed && escapePressed != null)
		{
			escapePressed(this, EventArgs.Empty);
			this._escapePressed = false;
		}
	}

	// Token: 0x060038B5 RID: 14517 RVA: 0x001257F4 File Offset: 0x001239F4
	private void LateUpdate()
	{
		if (this.InterfaceEnabled && ExperienceController.sharedController != null && !this._lockInterfaceEnabledCoroutine)
		{
			ExperienceController.sharedController.isShowRanks = false;
		}
	}

	// Token: 0x060038B6 RID: 14518 RVA: 0x00125828 File Offset: 0x00123A28
	private void UpdateMiscUiOnView(AbstractBankView bankView)
	{
		if (bankView == null || !bankView.gameObject.activeSelf)
		{
			return;
		}
		if (coinsShop.IsWideLayoutAvailable)
		{
			bankView.ConnectionProblemLabelEnabled = false;
			bankView.CrackersWarningLabelEnabled = true;
			bankView.NotEnoughCoinsLabelEnabled = false;
			bankView.NotEnoughGemsLabelEnabled = false;
			bankView.AreBankContentsEnabled = false;
			bankView.PurchaseSuccessfulLabelEnabled = false;
		}
		else if (coinsShop.thisScript != null)
		{
			bankView.NotEnoughCoinsLabelEnabled = (coinsShop.thisScript.notEnoughCurrency == "Coins");
			bankView.NotEnoughGemsLabelEnabled = (coinsShop.thisScript.notEnoughCurrency == "GemsCurrency");
			ActivityIndicator.IsActiveIndicator = StoreKitEventListener.purchaseInProcess;
			if (coinsShop.IsNoConnection)
			{
				if (Time.realtimeSinceStartup - this.tmOfFirstEnterTheBank > 3f)
				{
					bankView.ConnectionProblemLabelEnabled = true;
				}
				bankView.NotEnoughCoinsLabelEnabled = false;
				bankView.NotEnoughGemsLabelEnabled = false;
				bankView.AreBankContentsEnabled = false;
				bankView.PurchaseSuccessfulLabelEnabled = false;
			}
			else
			{
				bankView.ConnectionProblemLabelEnabled = false;
				bankView.AreBankContentsEnabled = true;
			}
			bankView.PurchaseSuccessfulLabelEnabled = coinsShop.thisScript.ProductPurchasedRecently;
		}
	}

	// Token: 0x060038B7 RID: 14519 RVA: 0x00125940 File Offset: 0x00123B40
	private void OnDestroy()
	{
		PromoActionsManager.EventX3Updated -= this.OnEventX3Updated;
		PromoActionsManager.EventAmazonX3Updated -= this.OnEventX3AmazonBonusUpdated;
		if (this.bankViewCommon != null)
		{
			this.bankViewCommon.PurchaseButtonPressed -= this.HandlePurchaseButtonPressed;
		}
		BalanceController.UpdatedBankView -= this.BalanceController_UpdatedBankView;
		InappBonuessController.OnGiveInappBonus -= this.InappBonuessController_OnGiveInappBonus;
	}

	// Token: 0x060038B8 RID: 14520 RVA: 0x001259BC File Offset: 0x00123BBC
	private void HandlePurchaseButtonPressed(AbstractBankViewItem item)
	{
		if (BuildSettings.BuildTargetPlatform == RuntimePlatform.MetroPlayerX64 && Time.realtimeSinceStartup - BankController._lastTimePurchaseButtonPressed < 1f)
		{
			Debug.Log("Bank button pressed but ignored");
			return;
		}
		BankController._lastTimePurchaseButtonPressed = Time.realtimeSinceStartup;
		if (StoreKitEventListener.purchaseInProcess)
		{
			Debug.Log("Cannot perform request while purchase is in progress.");
		}
		if (coinsShop.thisScript != null)
		{
			coinsShop.thisScript.HandlePurchaseButton(item.purchaseInfo.Index, item.purchaseInfo.Currency, item);
		}
		else
		{
			Debug.LogErrorFormat("HandlePurchaseButtonPressed coinsShop.thisScript == null", new object[0]);
		}
	}

	// Token: 0x060038B9 RID: 14521 RVA: 0x00125A5C File Offset: 0x00123C5C
	public static void AddCoins(int count, bool needIndication = true, AnalyticsConstants.AccrualType accrualType = AnalyticsConstants.AccrualType.Earned)
	{
		int @int = Storager.getInt("Coins", false);
		Storager.setInt("Coins", @int + count, false);
		AnalyticsFacade.CurrencyAccrual(count, "Coins", accrualType);
		if (needIndication)
		{
			CoinsMessage.FireCoinsAddedEvent(false, 2);
		}
	}

	// Token: 0x060038BA RID: 14522 RVA: 0x00125A9C File Offset: 0x00123C9C
	public static void AddGems(int count, bool needIndication = true, AnalyticsConstants.AccrualType accrualType = AnalyticsConstants.AccrualType.Earned)
	{
		int @int = Storager.getInt("GemsCurrency", false);
		Storager.setInt("GemsCurrency", @int + count, false);
		AnalyticsFacade.CurrencyAccrual(count, "GemsCurrency", accrualType);
		if (needIndication)
		{
			CoinsMessage.FireCoinsAddedEvent(true, 2);
		}
	}

	// Token: 0x060038BB RID: 14523 RVA: 0x00125ADC File Offset: 0x00123CDC
	public static IEnumerator WaitForIndicationGems(bool isGems)
	{
		while (!BankController.canShowIndication)
		{
			yield return null;
		}
		CoinsMessage.FireCoinsAddedEvent(isGems, 2);
		yield break;
	}

	// Token: 0x060038BC RID: 14524 RVA: 0x00125B00 File Offset: 0x00123D00
	public void FreeAwardButtonClick()
	{
		ButtonClickSound.TryPlayClick();
		if (FreeAwardController.Instance == null)
		{
			return;
		}
		if (!FreeAwardController.Instance.AdvertCountLessThanLimit())
		{
			return;
		}
		if (AdsConfigManager.Instance.LastLoadedConfig == null)
		{
			return;
		}
		ChestInLobbyPointMemento chestInLobby = AdsConfigManager.Instance.LastLoadedConfig.AdPointsConfig.ChestInLobby;
		if (chestInLobby == null)
		{
			return;
		}
		string playerCategory = AdsConfigManager.GetPlayerCategory(AdsConfigManager.Instance.LastLoadedConfig);
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
		FreeAwardController.Instance.SetWatchState(d + TimeSpan.FromMinutes(finalRewardedVideoDelayMinutes[num]));
	}

	// Token: 0x17000957 RID: 2391
	// (get) Token: 0x060038BD RID: 14525 RVA: 0x00125C00 File Offset: 0x00123E00
	// (set) Token: 0x060038BE RID: 14526 RVA: 0x00125C08 File Offset: 0x00123E08
	private List<Dictionary<string, object>> CurrentInappBonuses { get; set; }

	// Token: 0x17000958 RID: 2392
	// (get) Token: 0x060038BF RID: 14527 RVA: 0x00125C14 File Offset: 0x00123E14
	// (set) Token: 0x060038C0 RID: 14528 RVA: 0x00125C1C File Offset: 0x00123E1C
	private float TimeWhenReturnedFromPause
	{
		get
		{
			return this.m_timeWhenReutrnedFromPause;
		}
		set
		{
			this.m_timeWhenReutrnedFromPause = value;
		}
	}

	// Token: 0x0400296F RID: 10607
	public const int InitialIosGems = 0;

	// Token: 0x04002970 RID: 10608
	public const int InitialIosCoins = 0;

	// Token: 0x04002971 RID: 10609
	public AbstractBankView bankViewCommon;

	// Token: 0x04002972 RID: 10610
	public GameObject uiRoot;

	// Token: 0x04002973 RID: 10611
	public ChestBonusView bonusDetailView;

	// Token: 0x04002974 RID: 10612
	public static bool canShowIndication = true;

	// Token: 0x04002975 RID: 10613
	private bool firsEnterToBankOccured;

	// Token: 0x04002976 RID: 10614
	private float tmOfFirstEnterTheBank;

	// Token: 0x04002977 RID: 10615
	private bool _lockInterfaceEnabledCoroutine;

	// Token: 0x04002978 RID: 10616
	private int _counterEn;

	// Token: 0x04002979 RID: 10617
	private IDisposable _backSubscription;

	// Token: 0x0400297A RID: 10618
	private string m_lastPrintedDismissReason = string.Empty;

	// Token: 0x0400297B RID: 10619
	private AbstractBankView m_currentBankView;

	// Token: 0x0400297C RID: 10620
	private string _debugMessage = string.Empty;

	// Token: 0x0400297D RID: 10621
	private bool _escapePressed;

	// Token: 0x0400297E RID: 10622
	private static float _lastTimePurchaseButtonPressed;

	// Token: 0x0400297F RID: 10623
	private float m_timeWhenReutrnedFromPause = float.MinValue;

	// Token: 0x04002980 RID: 10624
	private static BankController _instance;

	// Token: 0x04002981 RID: 10625
	private readonly Lazy<bool> _timeTamperingDetected = new Lazy<bool>(delegate()
	{
		bool flag = FreeAwardController.Instance.TimeTamperingDetected();
		if (flag && Defs.IsDeveloperBuild)
		{
			Debug.LogWarning("FreeAwardController: time tampering detected in Bank.");
		}
		return flag;
	});
}
