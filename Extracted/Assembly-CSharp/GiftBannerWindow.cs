using System;
using System.Collections;
using Rilisoft;
using UnityEngine;

// Token: 0x02000646 RID: 1606
public class GiftBannerWindow : BannerWindow
{
	// Token: 0x14000060 RID: 96
	// (add) Token: 0x0600378B RID: 14219 RVA: 0x0011E26C File Offset: 0x0011C46C
	// (remove) Token: 0x0600378C RID: 14220 RVA: 0x0011E284 File Offset: 0x0011C484
	public static event Action onClose;

	// Token: 0x14000061 RID: 97
	// (add) Token: 0x0600378D RID: 14221 RVA: 0x0011E29C File Offset: 0x0011C49C
	// (remove) Token: 0x0600378E RID: 14222 RVA: 0x0011E2B4 File Offset: 0x0011C4B4
	public static event Action onGetGift;

	// Token: 0x14000062 RID: 98
	// (add) Token: 0x0600378F RID: 14223 RVA: 0x0011E2CC File Offset: 0x0011C4CC
	// (remove) Token: 0x06003790 RID: 14224 RVA: 0x0011E2E4 File Offset: 0x0011C4E4
	public static event Action onHideInfoGift;

	// Token: 0x14000063 RID: 99
	// (add) Token: 0x06003791 RID: 14225 RVA: 0x0011E2FC File Offset: 0x0011C4FC
	// (remove) Token: 0x06003792 RID: 14226 RVA: 0x0011E314 File Offset: 0x0011C514
	public static event Action onOpenInfoGift;

	// Token: 0x06003793 RID: 14227 RVA: 0x0011E32C File Offset: 0x0011C52C
	private void Awake()
	{
		GiftBannerWindow.instance = this;
		if (this.animatorBanner != null)
		{
			this.animatorBanner = base.GetComponent<Animator>();
		}
		GiftController.OnUpdateTimer += this.UpdateLabelTimer;
		GiftController.OnTimerEnded += this.OnEndTimer;
	}

	// Token: 0x06003794 RID: 14228 RVA: 0x0011E380 File Offset: 0x0011C580
	private void Start()
	{
		MainMenuHeroCamera.onEndOpenGift += this.OpenBannerWindow;
	}

	// Token: 0x06003795 RID: 14229 RVA: 0x0011E394 File Offset: 0x0011C594
	private void OnDestroy()
	{
		GiftController.OnUpdateTimer -= this.UpdateLabelTimer;
		GiftController.OnTimerEnded -= this.OnEndTimer;
		MainMenuHeroCamera.onEndOpenGift -= this.OpenBannerWindow;
		GiftBannerWindow.instance = null;
	}

	// Token: 0x06003796 RID: 14230 RVA: 0x0011E3D0 File Offset: 0x0011C5D0
	private void OnEnable()
	{
		if (!this.bannerObj.activeSelf)
		{
			this.needPlayStartAnim = true;
		}
		if (this.objSound)
		{
			this.objSound.SetActive(Defs.isSoundFX);
		}
		this.SetViewState();
		if (GiftController.Instance != null)
		{
			if (GiftController.Instance.ActiveGift)
			{
				GiftController.Instance.CheckAvaliableSlots();
			}
			else
			{
				this.SetVisibleBanner(false);
				if (GiftBannerWindow.onClose != null)
				{
					GiftBannerWindow.onClose();
				}
			}
		}
		if (this._backSubscription != null)
		{
			this._backSubscription.Dispose();
		}
		this._backSubscription = BackSystem.Instance.Register(new Action(this.CloseBanner), "Gift (Gotcha)");
	}

	// Token: 0x06003797 RID: 14231 RVA: 0x0011E49C File Offset: 0x0011C69C
	private void OnDisable()
	{
		this.needPlayStartAnim = true;
		if (this._backSubscription != null)
		{
			this._backSubscription.Dispose();
			this._backSubscription = null;
		}
	}

	// Token: 0x06003798 RID: 14232 RVA: 0x0011E4D0 File Offset: 0x0011C6D0
	public void ShowShop()
	{
		if (GiftBannerWindow.blockedButton)
		{
			return;
		}
		this.SetVisibleBanner(false);
		this._waitBankClose = true;
		MainMenuController.sharedController.ShowBankWindow();
	}

	// Token: 0x06003799 RID: 14233 RVA: 0x0011E4F8 File Offset: 0x0011C6F8
	private void Update()
	{
		if (this._waitBankClose && !BankController.Instance.InterfaceEnabled)
		{
			this._waitBankClose = false;
			this.needPlayStartAnim = true;
			this.OpenBannerWindow();
		}
	}

	// Token: 0x0600379A RID: 14234 RVA: 0x0011E534 File Offset: 0x0011C734
	public void GetGift()
	{
		if (GiftBannerWindow.blockedButton)
		{
			return;
		}
		this.GetGiftCore(false);
	}

	// Token: 0x0600379B RID: 14235 RVA: 0x0011E548 File Offset: 0x0011C748
	private void OpenBannerWindow()
	{
		if (this._waitBankClose)
		{
			return;
		}
		Debug.Log("=>>>> OpenBannerWindow " + GiftBannerWindow.isForceClose);
		if (GiftBannerWindow.isForceClose)
		{
			return;
		}
		Debug.Log("=>>>> OpenBannerWindow " + this.needPlayStartAnim);
		if (this.needPlayStartAnim)
		{
			this.SetVisibleBanner(true);
			this.needPlayStartAnim = false;
			this.scrollGift.SetCanDraggable(true);
			this.HideDarkFon();
			this.animatorBanner.SetTrigger("OpenGiftPanel");
		}
	}

	// Token: 0x0600379C RID: 14236 RVA: 0x0011E5DC File Offset: 0x0011C7DC
	private void GetGiftCore(bool isForMoneyGift)
	{
		if (!isForMoneyGift && !GiftController.Instance.CanGetGift)
		{
			return;
		}
		ButtonClickSound.Instance.PlayClick();
		BankController.UpdateAllIndicatorsMoney();
		BankController.canShowIndication = false;
		SlotInfo gift = GiftController.Instance.GetGift(isForMoneyGift);
		if (gift == null)
		{
			throw new Exception("failed get gift");
		}
		this.awardSlot = this.CopySlot(gift);
		if (this.awardSlot != null)
		{
			if (this.awardSlot.gift != null)
			{
				AnalyticsStuff.LogDailyGift(this.awardSlot.gift.Id, this.awardSlot.category.Type, this.awardSlot.CountGift, isForMoneyGift);
			}
			else
			{
				Debug.LogError("GetGiftCore: awardSlot.gift = null");
			}
		}
		else
		{
			Debug.LogError("GetGiftCore: awardSlot = null");
		}
		GiftBannerWindow.blockedButton = true;
		this.scrollGift.SetCanDraggable(false);
		this.scrollGift.AnimScrollGift(this.awardSlot.numInScroll);
		this.animatorBanner.SetTrigger("OpenGiftBtnRelease");
		GiftScroll.canReCreateSlots = false;
		GiftController.Instance.ReCreateSlots();
		this.ShowDarkFon();
		this.StartShowAwardGift();
		if (GiftBannerWindow.onGetGift != null)
		{
			GiftBannerWindow.onGetGift();
		}
		this._freeSpinsText.gameObject.SetActive(false);
		if (this.objSound != null)
		{
			GameObject childGameObject = this.objSound.GetChildGameObject("SoundGatchaInfoShow", true);
			if (childGameObject != null)
			{
				AudioSource component = childGameObject.GetComponent<AudioSource>();
				if (component != null)
				{
					component.enabled = (this.awardSlot.category.Type != GiftCategoryType.Coins && this.awardSlot.category.Type != GiftCategoryType.Gems);
				}
			}
		}
	}

	// Token: 0x0600379D RID: 14237 RVA: 0x0011E798 File Offset: 0x0011C998
	private SlotInfo CopySlot(SlotInfo curSlot)
	{
		SlotInfo slotInfo = new SlotInfo();
		slotInfo.gift = new GiftInfo();
		slotInfo.gift.Id = curSlot.gift.Id;
		slotInfo.gift.Count.Value = curSlot.gift.Count.Value;
		slotInfo.gift.KeyTranslateInfo = curSlot.gift.KeyTranslateInfo;
		slotInfo.CountGift = curSlot.CountGift;
		slotInfo.numInScroll = curSlot.numInScroll;
		slotInfo.category = curSlot.category;
		slotInfo.isActiveEvent = curSlot.isActiveEvent;
		return slotInfo;
	}

	// Token: 0x0600379E RID: 14238 RVA: 0x0011E834 File Offset: 0x0011CA34
	public void BuyCanGetGift()
	{
		if (GiftBannerWindow.blockedButton)
		{
			return;
		}
		bool buySuccess = false;
		ItemPrice price = new ItemPrice(GiftController.Instance.CostBuyCanGetGift.Value, "GemsCurrency");
		ShopNGUIController.TryToBuy(base.transform.root.gameObject, price, delegate
		{
			buySuccess = true;
			this.GetGiftCore(true);
		}, null, null, null, null, delegate
		{
			this._waitBankClose = true;
		});
		if (!buySuccess)
		{
			this.SetVisibleBanner(false);
		}
		this.SetViewState();
	}

	// Token: 0x0600379F RID: 14239 RVA: 0x0011E8C4 File Offset: 0x0011CAC4
	public void StartShowAwardGift()
	{
		if (this.awardSlot != null)
		{
			this.canTapOnGift = false;
			this.curStateAnimAward = GiftBannerWindow.StepAnimation.WaitForShowAward;
			AnimationGift.instance.StartAnimForGetGift();
			base.CancelInvoke("StartNextStep");
			base.Invoke("StartNextStep", this.delayBeforeNextStep);
		}
		else
		{
			this.CloseInfoGift(false);
		}
	}

	// Token: 0x060037A0 RID: 14240 RVA: 0x0011E91C File Offset: 0x0011CB1C
	public void OnClickGift()
	{
		if (this.canTapOnGift)
		{
			this.StartNextStep();
		}
	}

	// Token: 0x060037A1 RID: 14241 RVA: 0x0011E930 File Offset: 0x0011CB30
	private void StartNextStep()
	{
		switch (this.curStateAnimAward)
		{
		case GiftBannerWindow.StepAnimation.WaitForShowAward:
			base.CancelInvoke("StartNextStep");
			this.curStateAnimAward = GiftBannerWindow.StepAnimation.ShowAward;
			this.StartNextStep();
			break;
		case GiftBannerWindow.StepAnimation.ShowAward:
			this.crtForShowAward = base.StartCoroutine(this.OnAnimOpenGift());
			break;
		case GiftBannerWindow.StepAnimation.waitForClose:
			this.CloseInfoGift(false);
			break;
		}
	}

	// Token: 0x060037A2 RID: 14242 RVA: 0x0011E9A0 File Offset: 0x0011CBA0
	private IEnumerator OnAnimOpenGift()
	{
		base.CancelInvoke("StartNextStep");
		this.HideDarkFon();
		AnimationGift.instance.StopAnimForGetGift();
		if (GiftBannerWindow.onOpenInfoGift != null)
		{
			GiftBannerWindow.onOpenInfoGift();
		}
		this.panelInfoGift.SetInfoButton(this.awardSlot);
		this.awardSlot = null;
		yield return new WaitForSeconds(1f);
		BankController.canShowIndication = true;
		this.animatorBanner.SetTrigger("GiftInfoShow");
		yield return new WaitForSeconds(1.5f);
		this.curStateAnimAward = GiftBannerWindow.StepAnimation.waitForClose;
		this.canTapOnGift = true;
		base.Invoke("StartNextStep", this.delayBeforeNextStep);
		yield break;
	}

	// Token: 0x060037A3 RID: 14243 RVA: 0x0011E9BC File Offset: 0x0011CBBC
	public void CloseInfoGift(bool isForce = false)
	{
		this.canTapOnGift = true;
		base.CancelInvoke("StartNextStep");
		SpringPanel component = this.scrollGift.GetComponent<SpringPanel>();
		if (component)
		{
			UnityEngine.Object.Destroy(component);
		}
		if (this.crtForShowAward != null)
		{
			base.StopCoroutine(this.crtForShowAward);
		}
		this.animatorBanner.SetTrigger("GiftInfoClose");
		this.crtForShowAward = null;
		this.curStateAnimAward = GiftBannerWindow.StepAnimation.none;
		GiftScroll.canReCreateSlots = true;
		this.scrollGift.SetCanDraggable(true);
		this.SetViewState();
		this.HideDarkFon();
		base.Invoke("UnlockedBut", 1.5f);
		if (this.scrollGift != null)
		{
			this.scrollGift.UpdateListButton();
		}
		if (GiftBannerWindow.onHideInfoGift != null)
		{
			GiftBannerWindow.onHideInfoGift();
		}
		base.StartCoroutine(this.WaitAndSort());
		if (!isForce && FriendsController.ServerTime < 0L)
		{
			AnimationGift.instance.CheckVisibleGift();
			this.ForceCloseAll();
		}
	}

	// Token: 0x060037A4 RID: 14244 RVA: 0x0011EABC File Offset: 0x0011CCBC
	private IEnumerator WaitAndSort()
	{
		yield return null;
		this.scrollGift.transform.parent.localScale = Vector3.one;
		this.scrollGift.transform.localScale = Vector3.one;
		this.scrollGift.Sort();
		yield return null;
		while (this.scrollGift.transform.parent.localScale.Equals(Vector3.one))
		{
			yield return null;
		}
		this.scrollGift.Sort();
		yield break;
	}

	// Token: 0x060037A5 RID: 14245 RVA: 0x0011EAD8 File Offset: 0x0011CCD8
	private void UnlockedBut()
	{
		GiftBannerWindow.blockedButton = false;
	}

	// Token: 0x060037A6 RID: 14246 RVA: 0x0011EAE0 File Offset: 0x0011CCE0
	public void CloseBanner()
	{
		if (this._waitBankClose)
		{
			return;
		}
		if (GiftBannerWindow.blockedButton)
		{
			return;
		}
		if (BannerWindowController.SharedController && this.bannerObj != null && this.bannerObj.activeSelf)
		{
			ButtonClickSound.Instance.PlayClick();
			this.SetVisibleBanner(false);
			if (GiftBannerWindow.onClose != null)
			{
				GiftBannerWindow.onClose();
			}
			GiftBannerWindow.isActiveBanner = false;
		}
	}

	// Token: 0x060037A7 RID: 14247 RVA: 0x0011EB60 File Offset: 0x0011CD60
	public void CloseBannerEndAnimtion()
	{
		BannerWindowController.SharedController.HideBannerWindow();
		this.SetVisibleBanner(true);
		if (MainMenuController.sharedController != null)
		{
			MainMenuController.sharedController.ShowSavePanel(true);
		}
	}

	// Token: 0x060037A8 RID: 14248 RVA: 0x0011EB9C File Offset: 0x0011CD9C
	public void SetVisibleBanner(bool val)
	{
		if (this.bannerObj != null)
		{
			this.bannerObj.SetActive(val);
		}
	}

	// Token: 0x060037A9 RID: 14249 RVA: 0x0011EBBC File Offset: 0x0011CDBC
	public string GetNameSpriteForSlot(SlotInfo curSlot)
	{
		GiftCategoryType type = curSlot.category.Type;
		if (type != GiftCategoryType.ArmorAndHat)
		{
			return string.Empty;
		}
		return "shop_icons_armor";
	}

	// Token: 0x060037AA RID: 14250 RVA: 0x0011EBEC File Offset: 0x0011CDEC
	private void SetViewState()
	{
		this.lbPriceForBuy.text = GiftController.Instance.CostBuyCanGetGift.Value.ToString();
		string text = LocalizationStore.Get("Key_2196");
		int num = GiftController.Instance.FreeSpins + ((!GiftController.Instance.CanGetTimerGift) ? 0 : 1);
		if (GiftController.Instance.CanGetFreeSpinGift)
		{
			text = string.Format(text, (num <= 1) ? string.Empty : num.ToString());
		}
		this._freeSpinsText.Text = text;
		this._freeSpinsText.gameObject.SetActiveSafe(GiftController.Instance.CanGetFreeSpinGift && num > 1);
		this.butBuy.SetActiveSafe(!GiftController.Instance.CanGetGift);
		this.butGift.SetActiveSafe(GiftController.Instance.CanGetGift);
		this.lbTimer.gameObject.SetActive(!GiftController.Instance.CanGetTimerGift);
		this.objLabelTapGift.SetActive(!GiftController.Instance.CanGetTimerGift);
	}

	// Token: 0x060037AB RID: 14251 RVA: 0x0011ED0C File Offset: 0x0011CF0C
	public void ShowDarkFon()
	{
	}

	// Token: 0x060037AC RID: 14252 RVA: 0x0011ED10 File Offset: 0x0011CF10
	public void HideDarkFon()
	{
	}

	// Token: 0x060037AD RID: 14253 RVA: 0x0011ED14 File Offset: 0x0011CF14
	public void AnimFonShow(bool val)
	{
		if (this.sprDarkFon != null)
		{
			if (val)
			{
				TweenAlpha.Begin(this.sprDarkFon.gameObject, 1f, 0.4f);
			}
			else
			{
				TweenAlpha.Begin(this.sprDarkFon.gameObject, 0.1f, 0f);
			}
		}
	}

	// Token: 0x060037AE RID: 14254 RVA: 0x0011ED74 File Offset: 0x0011CF74
	private void UpdateLabelTimer(string curTime)
	{
		this.SetViewState();
		if (this.lbTimer != null)
		{
			this.lbTimer.text = curTime;
		}
	}

	// Token: 0x060037AF RID: 14255 RVA: 0x0011ED9C File Offset: 0x0011CF9C
	private void OnEndTimer()
	{
		this.SetViewState();
	}

	// Token: 0x060037B0 RID: 14256 RVA: 0x0011EDA4 File Offset: 0x0011CFA4
	private void OnApplicationPause(bool pausing)
	{
		if (!pausing)
		{
			this.ForceCloseAll();
		}
	}

	// Token: 0x060037B1 RID: 14257 RVA: 0x0011EDB4 File Offset: 0x0011CFB4
	public void ForceCloseAll()
	{
		if (GiftBannerWindow.instance == null || this.curStateAnimAward != GiftBannerWindow.StepAnimation.none || !GiftBannerWindow.isActiveBanner)
		{
			return;
		}
		GiftBannerWindow.isActiveBanner = false;
		GiftBannerWindow.isForceClose = true;
		this.needPlayStartAnim = true;
		GiftBannerWindow.blockedButton = false;
		BankController.canShowIndication = true;
		this.canTapOnGift = true;
		this.CloseInfoGift(true);
		this.HideDarkFon();
		this.SetVisibleBanner(false);
		MainMenuController.canRotationLobbyPlayer = true;
		if (GiftBannerWindow.onClose != null)
		{
			GiftBannerWindow.onClose();
		}
		if (AnimationGift.instance != null)
		{
			AnimationGift.instance.ResetAnimation();
		}
		this.curStateAnimAward = GiftBannerWindow.StepAnimation.none;
	}

	// Token: 0x060037B2 RID: 14258 RVA: 0x0011EE5C File Offset: 0x0011D05C
	public void UpdateSizeScroll()
	{
		int num = this.scrollGift.listButton.Count;
		if (num > 8)
		{
			num = 8;
		}
		num--;
		int num2 = num * this.scrollGift.wrapScript.itemSize;
		UIPanel panel = this.scrollGift.scView.panel;
		this.sprFonScroll.SetDimensions(num2 + 30, (int)this.sprFonScroll.localSize.y);
		panel.baseClipRegion = new Vector4(panel.baseClipRegion.x, panel.baseClipRegion.y, (float)num2, panel.baseClipRegion.w);
	}

	// Token: 0x060037B3 RID: 14259 RVA: 0x0011EF08 File Offset: 0x0011D108
	[ContextMenu("simulate pause")]
	private void SimPause()
	{
		this.ForceCloseAll();
	}

	// Token: 0x0400285B RID: 10331
	public const string keyTrigOpenBanner = "OpenGiftPanel";

	// Token: 0x0400285C RID: 10332
	public const string keyTrigTapButton = "OpenGiftBtnRelease";

	// Token: 0x0400285D RID: 10333
	public const string keyTrigShowInfoGift = "GiftInfoShow";

	// Token: 0x0400285E RID: 10334
	public const string keyTrigCloseInfoGift = "GiftInfoClose";

	// Token: 0x0400285F RID: 10335
	public static GiftBannerWindow instance;

	// Token: 0x04002860 RID: 10336
	public GameObject butBuy;

	// Token: 0x04002861 RID: 10337
	public GameObject butGift;

	// Token: 0x04002862 RID: 10338
	public GameObject bannerObj;

	// Token: 0x04002863 RID: 10339
	public GiftScroll scrollGift;

	// Token: 0x04002864 RID: 10340
	public UILabel lbPriceForBuy;

	// Token: 0x04002865 RID: 10341
	public UILabel lbTimer;

	// Token: 0x04002866 RID: 10342
	public UISprite sprDarkFon;

	// Token: 0x04002867 RID: 10343
	public GameObject objSound;

	// Token: 0x04002868 RID: 10344
	public UISprite sprFonScroll;

	// Token: 0x04002869 RID: 10345
	public GiftHUDItem panelInfoGift;

	// Token: 0x0400286A RID: 10346
	public Animator animatorBanner;

	// Token: 0x0400286B RID: 10347
	public GameObject objLabelTapGift;

	// Token: 0x0400286C RID: 10348
	public float speedAnimCenter = 2f;

	// Token: 0x0400286D RID: 10349
	[SerializeField]
	private TextGroup _freeSpinsText;

	// Token: 0x0400286E RID: 10350
	public static bool blockedButton;

	// Token: 0x0400286F RID: 10351
	private SlotInfo awardSlot;

	// Token: 0x04002870 RID: 10352
	private float delayBeforeNextStep = 5f;

	// Token: 0x04002871 RID: 10353
	private bool canTapOnGift = true;

	// Token: 0x04002872 RID: 10354
	private Coroutine crtForShowAward;

	// Token: 0x04002873 RID: 10355
	private bool needShowGift;

	// Token: 0x04002874 RID: 10356
	[HideInInspector]
	public bool needForceShowAwardGift;

	// Token: 0x04002875 RID: 10357
	private IDisposable _backSubscription;

	// Token: 0x04002876 RID: 10358
	private bool needPlayStartAnim = true;

	// Token: 0x04002877 RID: 10359
	public static bool isForceClose;

	// Token: 0x04002878 RID: 10360
	public static bool isActiveBanner;

	// Token: 0x04002879 RID: 10361
	public GiftBannerWindow.StepAnimation curStateAnimAward;

	// Token: 0x0400287A RID: 10362
	private bool _waitBankClose;

	// Token: 0x02000647 RID: 1607
	public enum StepAnimation
	{
		// Token: 0x04002880 RID: 10368
		none,
		// Token: 0x04002881 RID: 10369
		WaitForShowAward,
		// Token: 0x04002882 RID: 10370
		ShowAward,
		// Token: 0x04002883 RID: 10371
		waitForClose
	}
}
