using System;
using System.Collections.Generic;
using System.Linq;
using I2.Loc;
using Rilisoft;
using UnityEngine;

// Token: 0x02000659 RID: 1625
internal abstract class AbstractBankView : MonoBehaviour
{
	// Token: 0x14000068 RID: 104
	// (add) Token: 0x0600385F RID: 14431 RVA: 0x00123E50 File Offset: 0x00122050
	// (remove) Token: 0x06003860 RID: 14432 RVA: 0x00123E6C File Offset: 0x0012206C
	public event Action<AbstractBankViewItem> PurchaseButtonPressed;

	// Token: 0x14000069 RID: 105
	// (add) Token: 0x06003861 RID: 14433 RVA: 0x00123E88 File Offset: 0x00122088
	// (remove) Token: 0x06003862 RID: 14434 RVA: 0x00123EA8 File Offset: 0x001220A8
	public event EventHandler BackButtonPressed
	{
		add
		{
			if (this.backButton != null)
			{
				this.backButton.Clicked += value;
			}
		}
		remove
		{
			if (this.backButton != null)
			{
				this.backButton.Clicked -= value;
			}
		}
	}

	// Token: 0x17000941 RID: 2369
	// (get) Token: 0x06003863 RID: 14435 RVA: 0x00123EC8 File Offset: 0x001220C8
	public static IList<PurchaseEventArgs> goldPurchasesInfo
	{
		get
		{
			return new List<PurchaseEventArgs>
			{
				new PurchaseEventArgs(0, 0, 0m, "Coins", AbstractBankView.discountsCoins[0]),
				new PurchaseEventArgs(1, 0, 0m, "Coins", AbstractBankView.discountsCoins[1]),
				new PurchaseEventArgs(2, 0, 0m, "Coins", AbstractBankView.discountsCoins[2]),
				new PurchaseEventArgs(3, 0, 0m, "Coins", AbstractBankView.discountsCoins[3]),
				new PurchaseEventArgs(4, 0, 0m, "Coins", AbstractBankView.discountsCoins[4]),
				new PurchaseEventArgs(5, 0, 0m, "Coins", AbstractBankView.discountsCoins[5]),
				new PurchaseEventArgs(6, 0, 0m, "Coins", AbstractBankView.discountsCoins[6])
			};
		}
	}

	// Token: 0x17000942 RID: 2370
	// (get) Token: 0x06003864 RID: 14436 RVA: 0x00123FB8 File Offset: 0x001221B8
	public static IList<PurchaseEventArgs> gemsPurchasesInfo
	{
		get
		{
			return new List<PurchaseEventArgs>
			{
				new PurchaseEventArgs(0, 0, 0m, "GemsCurrency", AbstractBankView.discountsGems[0]),
				new PurchaseEventArgs(1, 0, 0m, "GemsCurrency", AbstractBankView.discountsGems[1]),
				new PurchaseEventArgs(2, 0, 0m, "GemsCurrency", AbstractBankView.discountsGems[2]),
				new PurchaseEventArgs(3, 0, 0m, "GemsCurrency", AbstractBankView.discountsGems[3]),
				new PurchaseEventArgs(4, 0, 0m, "GemsCurrency", AbstractBankView.discountsGems[4]),
				new PurchaseEventArgs(5, 0, 0m, "GemsCurrency", AbstractBankView.discountsGems[5]),
				new PurchaseEventArgs(6, 0, 0m, "GemsCurrency", AbstractBankView.discountsGems[6])
			};
		}
	}

	// Token: 0x17000943 RID: 2371
	// (get) Token: 0x06003865 RID: 14437 RVA: 0x001240A8 File Offset: 0x001222A8
	// (set) Token: 0x06003866 RID: 14438 RVA: 0x001240B0 File Offset: 0x001222B0
	public string DesiredCurrency { get; set; }

	// Token: 0x17000944 RID: 2372
	// (get) Token: 0x06003867 RID: 14439 RVA: 0x001240BC File Offset: 0x001222BC
	// (set) Token: 0x06003868 RID: 14440 RVA: 0x001240E8 File Offset: 0x001222E8
	public bool ConnectionProblemLabelEnabled
	{
		get
		{
			return this.connectionProblemLabel != null && this.connectionProblemLabel.gameObject.GetActive();
		}
		set
		{
			if (this.connectionProblemLabel != null)
			{
				this.connectionProblemLabel.gameObject.SetActive(value);
			}
		}
	}

	// Token: 0x17000945 RID: 2373
	// (get) Token: 0x06003869 RID: 14441 RVA: 0x00124118 File Offset: 0x00122318
	// (set) Token: 0x0600386A RID: 14442 RVA: 0x00124144 File Offset: 0x00122344
	public bool CrackersWarningLabelEnabled
	{
		get
		{
			return this.crackersWarningLabel != null && this.crackersWarningLabel.gameObject.GetActive();
		}
		set
		{
			if (this.crackersWarningLabel != null)
			{
				this.crackersWarningLabel.gameObject.SetActive(value);
			}
		}
	}

	// Token: 0x17000946 RID: 2374
	// (get) Token: 0x0600386B RID: 14443 RVA: 0x00124174 File Offset: 0x00122374
	// (set) Token: 0x0600386C RID: 14444 RVA: 0x001241A0 File Offset: 0x001223A0
	public bool NotEnoughCoinsLabelEnabled
	{
		get
		{
			return this.notEnoughCoinsLabel != null && this.notEnoughCoinsLabel.gameObject.GetActive();
		}
		set
		{
			if (this.notEnoughCoinsLabel != null)
			{
				this.notEnoughCoinsLabel.gameObject.SetActive(value);
			}
		}
	}

	// Token: 0x17000947 RID: 2375
	// (get) Token: 0x0600386D RID: 14445 RVA: 0x001241D0 File Offset: 0x001223D0
	// (set) Token: 0x0600386E RID: 14446 RVA: 0x001241FC File Offset: 0x001223FC
	public bool NotEnoughGemsLabelEnabled
	{
		get
		{
			return this.notEnoughGemsLabel != null && this.notEnoughGemsLabel.gameObject.GetActive();
		}
		set
		{
			if (this.notEnoughGemsLabel != null)
			{
				this.notEnoughGemsLabel.gameObject.SetActive(value);
			}
		}
	}

	// Token: 0x17000948 RID: 2376
	// (get) Token: 0x0600386F RID: 14447 RVA: 0x0012422C File Offset: 0x0012242C
	// (set) Token: 0x06003870 RID: 14448 RVA: 0x00124258 File Offset: 0x00122458
	public bool PurchaseSuccessfulLabelEnabled
	{
		get
		{
			return this.purchaseSuccessfulLabel != null && this.purchaseSuccessfulLabel.gameObject.GetActive();
		}
		set
		{
			if (this.purchaseSuccessfulLabel != null)
			{
				this.purchaseSuccessfulLabel.gameObject.SetActive(value);
			}
		}
	}

	// Token: 0x17000949 RID: 2377
	// (get) Token: 0x06003871 RID: 14449 RVA: 0x00124288 File Offset: 0x00122488
	// (set) Token: 0x06003872 RID: 14450 RVA: 0x00124290 File Offset: 0x00122490
	public virtual bool IsX3Bank
	{
		get
		{
			return this.m_isX3Bank;
		}
		set
		{
			this.m_isX3Bank = value;
			for (int i = 0; i < this.x3BankElements.Length; i++)
			{
				if (this.x3BankElements[i] != null)
				{
					this.x3BankElements[i].SetActiveSafeSelf(value);
				}
			}
			for (int j = 0; j < this.usualBankElements.Length; j++)
			{
				if (this.usualBankElements[j] != null)
				{
					this.usualBankElements[j].SetActiveSafeSelf(!value);
				}
			}
			IEnumerable<AbstractBankViewItem> enumerable = this.AllItems();
			foreach (AbstractBankViewItem abstractBankViewItem in enumerable)
			{
				abstractBankViewItem.isX3Item = value;
			}
		}
	}

	// Token: 0x1700094A RID: 2378
	// (get) Token: 0x06003873 RID: 14451
	// (set) Token: 0x06003874 RID: 14452
	public abstract bool AreBankContentsEnabled { get; set; }

	// Token: 0x06003875 RID: 14453
	public abstract void UpdateUi();

	// Token: 0x06003876 RID: 14454 RVA: 0x00124378 File Offset: 0x00122578
	protected void Awake()
	{
		this.BackButtonPressed += this.AbstractBankView_BackButtonPressed;
		this._storeKitEventListener = UnityEngine.Object.FindObjectOfType<StoreKitEventListener>();
		if (this._storeKitEventListener == null)
		{
			Debug.LogError("storeKitEventListener == null");
			this.HandleNoStoreKitEventListener();
		}
		else
		{
			this.OnEnable();
		}
	}

	// Token: 0x06003877 RID: 14455 RVA: 0x001243D0 File Offset: 0x001225D0
	private void AbstractBankView_BackButtonPressed(object sender, EventArgs e)
	{
		try
		{
			if (Device.IsLoweMemoryDevice)
			{
				ActivityIndicator.ClearMemory();
			}
		}
		catch (Exception ex)
		{
			Debug.LogErrorFormat("Exception in AbstractBankView_BackButtonPressed: {0}", new object[]
			{
				ex
			});
		}
	}

	// Token: 0x06003878 RID: 14456 RVA: 0x00124428 File Offset: 0x00122628
	protected virtual void OnEnable()
	{
		this._localizeSaleLabel = LocalizationStore.Get("Key_0419");
	}

	// Token: 0x06003879 RID: 14457 RVA: 0x0012443C File Offset: 0x0012263C
	protected virtual void Start()
	{
		bool state = StoreKitEventListener.IsPayingUser();
		foreach (GameObject go in this.AdFreeLabels)
		{
			go.SetActiveSafeSelf(state);
		}
	}

	// Token: 0x0600387A RID: 14458 RVA: 0x00124478 File Offset: 0x00122678
	protected virtual void Update()
	{
		if (Time.realtimeSinceStartup - this._lastUpdateTime >= 0.5f)
		{
			long eventX3RemainedTime = PromoActionsManager.sharedManager.EventX3RemainedTime;
			TimeSpan timeSpan = TimeSpan.FromSeconds((double)eventX3RemainedTime);
			string text = string.Empty;
			if (timeSpan.Days > 0)
			{
				text = string.Format("{0}: {1} {2} {3:00}:{4:00}:{5:00}", new object[]
				{
					this._localizeSaleLabel,
					timeSpan.Days,
					(timeSpan.Days != 1) ? "Days" : "Day",
					timeSpan.Hours,
					timeSpan.Minutes,
					timeSpan.Seconds
				});
			}
			else
			{
				text = string.Format("{0}: {1:00}:{2:00}:{3:00}", new object[]
				{
					this._localizeSaleLabel,
					timeSpan.Hours,
					timeSpan.Minutes,
					timeSpan.Seconds
				});
			}
			if (this.eventX3RemainTime != null)
			{
				for (int i = 0; i < this.eventX3RemainTime.Length; i++)
				{
					this.eventX3RemainTime[i].text = text;
				}
			}
			if (this.colorBlinkForX3 != null && timeSpan.TotalHours < (double)Defs.HoursToEndX3ForIndicate && !this.colorBlinkForX3.enabled)
			{
				this.colorBlinkForX3.enabled = true;
			}
			this._lastUpdateTime = Time.realtimeSinceStartup;
		}
		if (this._freeAwardButtonLabel != null && this.freeAwardButton.isActiveAndEnabled)
		{
			this._freeAwardButtonLabel.text = ((!(FreeAwardController.Instance.CurrencyForAward == "GemsCurrency")) ? string.Format("[FFA300FF]{0}[-]", ScriptLocalization.Get("Key_1155")) : string.Format("[50CEFFFF]{0}[-]", ScriptLocalization.Get("Key_2046")));
		}
	}

	// Token: 0x0600387B RID: 14459 RVA: 0x00124670 File Offset: 0x00122870
	protected virtual void OnDestroy()
	{
		this.BackButtonPressed -= this.AbstractBankView_BackButtonPressed;
	}

	// Token: 0x0600387C RID: 14460
	protected abstract void HandleNoStoreKitEventListener();

	// Token: 0x0600387D RID: 14461
	protected abstract IEnumerable<AbstractBankViewItem> AllItems();

	// Token: 0x0600387E RID: 14462 RVA: 0x00124684 File Offset: 0x00122884
	protected virtual void UpdateItem(AbstractBankViewItem item, PurchaseEventArgs purchaseInfo)
	{
		bool flag = purchaseInfo.Currency == "GemsCurrency";
		string[] array = (!flag) ? StoreKitEventListener.coinIds : StoreKitEventListener.gemsIds;
		if (purchaseInfo.Index >= array.Length)
		{
			Debug.LogErrorFormat("UpdateItem: purchaseInfo.Index < inAppIds.Length", new object[0]);
			return;
		}
		string inappId = array[purchaseInfo.Index];
		purchaseInfo.Count = ((!flag) ? VirtualCurrencyHelper.coinInappsQuantity[purchaseInfo.Index] : VirtualCurrencyHelper.gemsInappsQuantity[purchaseInfo.Index]);
		decimal d = (!flag) ? VirtualCurrencyHelper.coinPriceIds[purchaseInfo.Index] : VirtualCurrencyHelper.gemsPriceIds[purchaseInfo.Index];
		purchaseInfo.CurrencyAmount = d - 0.01m;
		string price = string.Format("${0}", purchaseInfo.CurrencyAmount);
		IMarketProduct marketProduct = this._storeKitEventListener.Products.FirstOrDefault((IMarketProduct p) => p.Id == inappId);
		if (marketProduct != null)
		{
			price = marketProduct.Price;
		}
		else
		{
			Debug.LogWarning("marketProduct == null,    inappId: " + inappId);
		}
		item.Price = price;
		try
		{
			item.Setup(marketProduct, purchaseInfo, delegate(object sender, EventArgs e)
			{
				Action<AbstractBankViewItem> purchaseButtonPressed = this.PurchaseButtonPressed;
				if (purchaseButtonPressed != null)
				{
					purchaseButtonPressed(item);
				}
			});
		}
		catch (Exception ex)
		{
			Debug.LogErrorFormat("Exception in setup of BankViewItem: {0}", new object[]
			{
				ex
			});
		}
	}

	// Token: 0x1700094B RID: 2379
	// (get) Token: 0x0600387F RID: 14463 RVA: 0x00124830 File Offset: 0x00122A30
	private UILabel _freeAwardButtonLabel
	{
		get
		{
			if (this._freeAwardButtonLagelCont != null)
			{
				return this._freeAwardButtonLagelCont;
			}
			if (this.freeAwardButton == null)
			{
				return this._freeAwardButtonLagelCont;
			}
			return this._freeAwardButtonLagelCont = this.freeAwardButton.GetComponentInChildren<UILabel>();
		}
	}

	// Token: 0x0400294A RID: 10570
	public static int[] discountsCoins = new int[]
	{
		0,
		0,
		7,
		10,
		12,
		15,
		33
	};

	// Token: 0x0400294B RID: 10571
	public static int[] discountsGems = new int[]
	{
		0,
		0,
		7,
		10,
		12,
		15,
		33
	};

	// Token: 0x0400294C RID: 10572
	public GameObject[] x3BankElements;

	// Token: 0x0400294D RID: 10573
	public GameObject[] usualBankElements;

	// Token: 0x0400294E RID: 10574
	public TweenColor colorBlinkForX3;

	// Token: 0x0400294F RID: 10575
	public ButtonHandler backButton;

	// Token: 0x04002950 RID: 10576
	public UILabel connectionProblemLabel;

	// Token: 0x04002951 RID: 10577
	public UILabel crackersWarningLabel;

	// Token: 0x04002952 RID: 10578
	public UILabel notEnoughCoinsLabel;

	// Token: 0x04002953 RID: 10579
	public UILabel notEnoughGemsLabel;

	// Token: 0x04002954 RID: 10580
	public UISprite purchaseSuccessfulLabel;

	// Token: 0x04002955 RID: 10581
	public UILabel[] eventX3RemainTime;

	// Token: 0x04002956 RID: 10582
	public UIButton freeAwardButton;

	// Token: 0x04002957 RID: 10583
	public UIWidget eventX3AmazonBonusWidget;

	// Token: 0x04002958 RID: 10584
	public UILabel amazonEventCaptionLabel;

	// Token: 0x04002959 RID: 10585
	public UILabel amazonEventTitleLabel;

	// Token: 0x0400295A RID: 10586
	public GameObject[] AdFreeLabels;

	// Token: 0x0400295B RID: 10587
	private UILabel _freeAwardButtonLagelCont;

	// Token: 0x0400295C RID: 10588
	private float _lastUpdateTime;

	// Token: 0x0400295D RID: 10589
	private string _localizeSaleLabel;

	// Token: 0x0400295E RID: 10590
	private StoreKitEventListener _storeKitEventListener;

	// Token: 0x0400295F RID: 10591
	private bool m_isX3Bank;
}
