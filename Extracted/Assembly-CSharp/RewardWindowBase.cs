using System;
using System.Collections;
using System.Collections.Generic;
using Facebook.Unity;
using I2.Loc;
using Rilisoft;
using UnityEngine;

// Token: 0x020004CD RID: 1229
public sealed class RewardWindowBase : MonoBehaviour
{
	// Token: 0x1400003A RID: 58
	// (add) Token: 0x06002BC0 RID: 11200 RVA: 0x000E6408 File Offset: 0x000E4608
	// (remove) Token: 0x06002BC1 RID: 11201 RVA: 0x000E6420 File Offset: 0x000E4620
	public static event Action Shown;

	// Token: 0x1700078E RID: 1934
	// (get) Token: 0x06002BC2 RID: 11202 RVA: 0x000E6438 File Offset: 0x000E4638
	// (set) Token: 0x06002BC3 RID: 11203 RVA: 0x000E6440 File Offset: 0x000E4640
	public FacebookController.StoryPriority facebookPriority
	{
		get
		{
			return this._facebookPriority;
		}
		set
		{
			this._facebookPriority = value;
		}
	}

	// Token: 0x1700078F RID: 1935
	// (get) Token: 0x06002BC4 RID: 11204 RVA: 0x000E644C File Offset: 0x000E464C
	// (set) Token: 0x06002BC5 RID: 11205 RVA: 0x000E6454 File Offset: 0x000E4654
	public FacebookController.StoryPriority twitterPriority
	{
		get
		{
			return this._twiiterPriority;
		}
		set
		{
			this._twiiterPriority = value;
		}
	}

	// Token: 0x17000790 RID: 1936
	// (set) Token: 0x06002BC6 RID: 11206 RVA: 0x000E6460 File Offset: 0x000E4660
	public FacebookController.StoryPriority priority
	{
		set
		{
			this.facebookPriority = value;
			this.twitterPriority = value;
		}
	}

	// Token: 0x17000791 RID: 1937
	// (get) Token: 0x06002BC7 RID: 11207 RVA: 0x000E6470 File Offset: 0x000E4670
	// (set) Token: 0x06002BC8 RID: 11208 RVA: 0x000E6478 File Offset: 0x000E4678
	public Func<string> twitterStatus { get; set; }

	// Token: 0x17000792 RID: 1938
	// (get) Token: 0x06002BC9 RID: 11209 RVA: 0x000E6484 File Offset: 0x000E4684
	// (set) Token: 0x06002BCA RID: 11210 RVA: 0x000E648C File Offset: 0x000E468C
	public bool HasReward { get; set; }

	// Token: 0x17000793 RID: 1939
	// (get) Token: 0x06002BCB RID: 11211 RVA: 0x000E6498 File Offset: 0x000E4698
	// (set) Token: 0x06002BCC RID: 11212 RVA: 0x000E64A0 File Offset: 0x000E46A0
	public bool CollectOnlyNoShare
	{
		get
		{
			return this._collectOnlyNoShare;
		}
		set
		{
			this._collectOnlyNoShare = value;
		}
	}

	// Token: 0x17000794 RID: 1940
	// (get) Token: 0x06002BCD RID: 11213 RVA: 0x000E64AC File Offset: 0x000E46AC
	// (set) Token: 0x06002BCE RID: 11214 RVA: 0x000E64B4 File Offset: 0x000E46B4
	public string EventTitle { get; set; }

	// Token: 0x06002BCF RID: 11215 RVA: 0x000E64C0 File Offset: 0x000E46C0
	private void Awake()
	{
		if (this.share != null)
		{
			this.share.IsChecked = true;
		}
		this.animatorLevel = base.GetComponent<Animator>();
		LocalizationStore.AddEventCallAfterLocalize(new LocalizationManager.OnLocalizeCallback(this.HandleLocalizationChanged));
	}

	// Token: 0x06002BD0 RID: 11216 RVA: 0x000E6508 File Offset: 0x000E4708
	private void OnDestroy()
	{
		LocalizationStore.DelEventCallAfterLocalize(new LocalizationManager.OnLocalizeCallback(this.HandleLocalizationChanged));
	}

	// Token: 0x06002BD1 RID: 11217 RVA: 0x000E651C File Offset: 0x000E471C
	private void HandleLocalizationChanged()
	{
		this.SetConnectToSocialText();
	}

	// Token: 0x06002BD2 RID: 11218 RVA: 0x000E6524 File Offset: 0x000E4724
	private void OnEnable()
	{
		this.SetConnectToSocialText();
		if (this.soundsController)
		{
			this.soundsController.SetActive(Defs.isSoundFX);
		}
		if (Application.isEditor)
		{
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
		}
		Action shown = RewardWindowBase.Shown;
		if (shown != null)
		{
			shown();
		}
		if (this.shouldHideExpGui)
		{
			RentScreenController.SetDepthForExpGUI(0);
		}
		base.StartCoroutine(this.SendAnalyticsOnShowCoroutine());
		if (this._backSubscription != null)
		{
			this._backSubscription.Dispose();
		}
		this._backSubscription = BackSystem.Instance.Register(delegate
		{
			if (this.hideButton != null)
			{
				List<EventDelegate> onClick = this.hideButton.onClick;
				EventDelegate.Execute(onClick);
			}
		}, "Reward Window");
		if (this.animatorLevel != null)
		{
			if (ExperienceController.sharedController.currentLevel == 2)
			{
				this.animatorLevel.SetTrigger("Weapons");
			}
			else if (ExperienceController.sharedController.AddHealthOnCurLevel == 0)
			{
				this.animatorLevel.SetTrigger("is2items");
			}
			else
			{
				this.animatorLevel.SetTrigger("is3items");
			}
			this.animatorLevel.SetBool("IsRatingSystem", true);
		}
	}

	// Token: 0x06002BD3 RID: 11219 RVA: 0x000E6650 File Offset: 0x000E4850
	private void OnDisable()
	{
		if (this.shouldHideExpGui)
		{
			RentScreenController.SetDepthForExpGUI(99);
		}
		if (this._backSubscription != null)
		{
			this._backSubscription.Dispose();
			this._backSubscription = null;
		}
	}

	// Token: 0x06002BD4 RID: 11220 RVA: 0x000E6684 File Offset: 0x000E4884
	private IEnumerator SendAnalyticsOnShowCoroutine()
	{
		for (int i = 0; i < 2; i++)
		{
			yield return null;
		}
		Dictionary<string, object> parameters = new Dictionary<string, object>
		{
			{
				"Show Event",
				this.EventTitle
			}
		};
		if (!this.CollectOnlyNoShare && FacebookController.FacebookSupported && FacebookController.sharedController.CanPostStoryWithPriority(this.facebookPriority))
		{
			parameters.Add("Total Facebook", "Shows");
		}
		if (!this.CollectOnlyNoShare && TwitterController.TwitterSupported && TwitterController.Instance.CanPostStatusUpdateWithPriority(this.twitterPriority))
		{
			parameters.Add("Total Twitter", "Shows");
		}
		AnalyticsFacade.SendCustomEvent("Virality", parameters);
		yield break;
	}

	// Token: 0x06002BD5 RID: 11221 RVA: 0x000E66A0 File Offset: 0x000E48A0
	private bool ShouldShowFacebookWithRewardButton()
	{
		return !FB.IsLoggedIn && Storager.getInt(Defs.IsFacebookLoginRewardaGained, true) == 0 && !this.CollectOnlyNoShare && this.ShowLoginsButton && !Device.isPixelGunLow;
	}

	// Token: 0x06002BD6 RID: 11222 RVA: 0x000E66E0 File Offset: 0x000E48E0
	private bool ShouldShowTwitterWithRewardButton()
	{
		return !TwitterController.IsLoggedIn && Storager.getInt(Defs.IsTwitterLoginRewardaGained, true) == 0 && !this.CollectOnlyNoShare && this.ShowLoginsButton && !Device.isPixelGunLow;
	}

	// Token: 0x06002BD7 RID: 11223 RVA: 0x000E6720 File Offset: 0x000E4920
	private void SetConnectToSocialText()
	{
		this._timeSinceUpdateConnetToSocialText = Time.realtimeSinceStartup;
		foreach (UILabel uilabel in this.connectToSocial)
		{
			if (uilabel != null)
			{
				uilabel.text = string.Format(LocalizationStore.Get("Key_1460"), 10);
			}
		}
	}

	// Token: 0x06002BD8 RID: 11224 RVA: 0x000E67B4 File Offset: 0x000E49B4
	private void Start()
	{
		this.SetButtonPositionsAndActive();
	}

	// Token: 0x06002BD9 RID: 11225 RVA: 0x000E67BC File Offset: 0x000E49BC
	private void SetButtonPositionsAndActive()
	{
		if (!this.ShowLoginsButton)
		{
			return;
		}
		bool flag = false;
		bool flag2 = ((FacebookController.FacebookSupported && Storager.getInt(Defs.IsFacebookLoginRewardaGained, true) == 0) || (TwitterController.TwitterSupported && Storager.getInt(Defs.IsTwitterLoginRewardaGained, true) == 0)) && !this.CollectOnlyNoShare && this.ShowLoginsButton && !Device.isPixelGunLow;
		if (this.connectToSocialContainer != null && this.connectToSocialContainer.activeSelf != flag2)
		{
			this.connectToSocialContainer.SetActive(flag2);
			flag = true;
		}
		bool flag3 = false;
		bool flag4 = false;
		bool flag5 = FacebookController.FacebookSupported && this.ShouldShowFacebookWithRewardButton() && TrainingController.TrainingCompleted;
		if (this.facebook != null && this.facebook.gameObject.activeSelf != flag5)
		{
			this.facebook.gameObject.SetActive(flag5);
			flag3 = true;
		}
		bool flag6 = FacebookController.FacebookSupported && !FB.IsLoggedIn && Storager.getInt(Defs.IsFacebookLoginRewardaGained, true) == 1 && !this.CollectOnlyNoShare && TrainingController.TrainingCompleted && this.ShowLoginsButton && !Device.isPixelGunLow;
		if (this.facebookNoReward != null && this.facebookNoReward.gameObject.activeSelf != flag6)
		{
			this.facebookNoReward.gameObject.SetActive(flag6);
			flag3 = true;
		}
		bool flag7 = TwitterController.TwitterSupported && this.ShouldShowTwitterWithRewardButton() && TrainingController.TrainingCompleted;
		if (this.twitter != null && this.twitter.gameObject.activeSelf != flag7)
		{
			this.twitter.gameObject.SetActive(flag7);
			flag3 = true;
		}
		bool flag8 = TwitterController.TwitterSupported && !TwitterController.IsLoggedIn && Storager.getInt(Defs.IsTwitterLoginRewardaGained, true) == 1 && !this.CollectOnlyNoShare && TrainingController.TrainingCompleted && this.ShowLoginsButton && !Device.isPixelGunLow;
		if (this.twitterNoReward != null && this.twitterNoReward.gameObject.activeSelf != flag8)
		{
			this.twitterNoReward.gameObject.SetActive(flag8);
			flag3 = true;
		}
		bool flag9 = (FacebookController.FacebookSupported && (flag5 || flag6)) || (TwitterController.TwitterSupported && (flag7 || flag8));
		if (this.innerGrid != null && this.innerGrid.gameObject.activeSelf != flag9)
		{
			this.innerGrid.gameObject.SetActive(flag9);
			flag4 = true;
		}
		if (this.innerGrid != null && flag9 && flag3)
		{
			this.innerGrid.Reposition();
		}
		bool flag10 = TwitterController.TwitterSupported && TwitterController.Instance != null && TwitterController.Instance.CanPostStatusUpdateWithPriority(this.twitterPriority) && TwitterController.IsLoggedIn;
		bool flag11 = FacebookController.FacebookSupported && FacebookController.sharedController != null && FacebookController.sharedController.CanPostStoryWithPriority(this.facebookPriority) && FB.IsLoggedIn;
		bool flag12 = !this.HasReward && !flag10 && !flag11 && !this.CollectOnlyNoShare;
		if (this.continueButton != null && this.continueButton.gameObject.activeSelf != flag12)
		{
			this.continueButton.gameObject.SetActive(flag12);
		}
		bool flag13 = (this.HasReward && !flag10 && !flag11) || this.CollectOnlyNoShare;
		if (this.collect != null && this.collect.gameObject.activeSelf != flag13)
		{
			this.collect.gameObject.SetActive(flag13);
		}
		bool flag14 = !this.HasReward && (flag10 || flag11) && !this.CollectOnlyNoShare;
		if (this.continueAndShare != null && this.continueAndShare.gameObject.activeSelf != flag14)
		{
			this.continueAndShare.gameObject.SetActive(flag14);
		}
		bool flag15 = this.HasReward && (flag10 || flag11) && !this.CollectOnlyNoShare;
		if (this.collectAndShare != null && this.collectAndShare.gameObject.activeSelf != flag15)
		{
			this.collectAndShare.gameObject.SetActive(flag15);
		}
		if (this.containerWidget != null && flag)
		{
			if (flag2)
			{
				this.containerWidget.height = (int)this.widgetExpanded;
			}
			else if (flag9)
			{
				this.containerWidget.height = (int)this.widgetNoConnectToSocial;
			}
			else if (!flag9)
			{
				this.containerWidget.height = (int)this.widgetCollapsed;
			}
		}
		if (this.containerWidget != null && flag4)
		{
			if (flag9)
			{
				if (flag2)
				{
					this.containerWidget.height = (int)this.widgetExpanded;
				}
				else
				{
					this.containerWidget.height = (int)this.widgetNoConnectToSocial;
				}
			}
			else if (!flag9)
			{
				this.containerWidget.height = (int)this.widgetCollapsed;
			}
		}
		if (this.hideButton != null)
		{
			bool active = flag15 || flag14;
			this.hideButton.gameObject.SetActive(active);
		}
	}

	// Token: 0x06002BDA RID: 11226 RVA: 0x000E6DC8 File Offset: 0x000E4FC8
	public void ShowAuthorizationSucceded()
	{
		this.ShowAuthorizationResultWindow(true);
	}

	// Token: 0x06002BDB RID: 11227 RVA: 0x000E6DD4 File Offset: 0x000E4FD4
	public void ShowAuthorizationFailed()
	{
		this.ShowAuthorizationResultWindow(false);
	}

	// Token: 0x06002BDC RID: 11228 RVA: 0x000E6DE0 File Offset: 0x000E4FE0
	private void ShowAuthorizationResultWindow(bool success)
	{
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(Resources.Load<GameObject>("NguiWindows/" + ((!success) ? "PanelAuthFailed" : "PanelAuthSucces")));
		gameObject.transform.parent = base.transform;
		Player_move_c.SetLayerRecursively(gameObject, base.gameObject.layer);
		gameObject.transform.localPosition = new Vector3(0f, 0f, -130f);
		gameObject.transform.localRotation = Quaternion.identity;
		gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
	}

	// Token: 0x17000795 RID: 1941
	// (get) Token: 0x06002BDD RID: 11229 RVA: 0x000E6E88 File Offset: 0x000E5088
	private string CallerContext
	{
		get
		{
			return (!string.IsNullOrEmpty(this.EventTitle)) ? string.Format("{0}: {1}", "Reward Window", this.EventTitle) : "Reward Window";
		}
	}

	// Token: 0x06002BDE RID: 11230 RVA: 0x000E6EC4 File Offset: 0x000E50C4
	public void HandleFacebookButton()
	{
		ButtonClickSound.TryPlayClick();
		MainMenuController.DoMemoryConsumingTaskInEmptyScene(delegate
		{
			FacebookController.Login(new Action(this.ShowAuthorizationSucceded), new Action(this.ShowAuthorizationFailed), this.CallerContext, null);
		}, delegate
		{
			string callerContext = this.CallerContext;
			FacebookController.Login(null, null, callerContext, null);
		});
	}

	// Token: 0x06002BDF RID: 11231 RVA: 0x000E6EF4 File Offset: 0x000E50F4
	public void HandleTwitterButton()
	{
		ButtonClickSound.TryPlayClick();
		MainMenuController.DoMemoryConsumingTaskInEmptyScene(delegate
		{
			if (TwitterController.Instance != null)
			{
				TwitterController.Instance.Login(new Action(this.ShowAuthorizationSucceded), new Action(this.ShowAuthorizationFailed), this.CallerContext);
			}
		}, delegate
		{
			if (TwitterController.Instance != null)
			{
				TwitterController instance = TwitterController.Instance;
				string callerContext = this.CallerContext;
				instance.Login(null, null, callerContext);
			}
		});
	}

	// Token: 0x06002BE0 RID: 11232 RVA: 0x000E6F24 File Offset: 0x000E5124
	public void HandleShareToggle(UIToggle toggle)
	{
	}

	// Token: 0x06002BE1 RID: 11233 RVA: 0x000E6F28 File Offset: 0x000E5128
	private void Update()
	{
		this.SetButtonPositionsAndActive();
	}

	// Token: 0x06002BE2 RID: 11234 RVA: 0x000E6F30 File Offset: 0x000E5130
	public void HandleContinueButtonNoHide()
	{
		ButtonClickSound.TryPlayClick();
		if (TwitterController.TwitterSupported && TwitterController.IsLoggedIn && TwitterController.Instance.CanPostStatusUpdateWithPriority(this.twitterPriority))
		{
			TwitterController.Instance.PostStatusUpdate(this.twitterStatus(), this.twitterPriority);
			AnalyticsFacade.SendCustomEvent("Virality", new Dictionary<string, object>
			{
				{
					"Post Twitter",
					this.EventTitle
				},
				{
					"Total Twitter",
					"Posts"
				}
			});
		}
		if (FacebookController.FacebookSupported && FB.IsLoggedIn && FacebookController.sharedController.CanPostStoryWithPriority(this.facebookPriority))
		{
			Action action = this.shareAction;
			if (action != null)
			{
				action();
				AnalyticsFacade.SendCustomEvent("Virality", new Dictionary<string, object>
				{
					{
						"Post Facebook",
						this.EventTitle
					},
					{
						"Total Facebook",
						"Posts"
					}
				});
			}
		}
	}

	// Token: 0x06002BE3 RID: 11235 RVA: 0x000E7028 File Offset: 0x000E5228
	public void HandleContinueButton()
	{
		this.HandleContinueButtonNoHide();
		this.Hide();
	}

	// Token: 0x06002BE4 RID: 11236 RVA: 0x000E7038 File Offset: 0x000E5238
	public void Hide()
	{
		ButtonClickSound.TryPlayClick();
		Action action = this.customHide;
		if (action != null)
		{
			action();
			this.customHide = null;
		}
		base.transform.parent = null;
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x06002BE5 RID: 11237 RVA: 0x000E707C File Offset: 0x000E527C
	public void StartGemsStarterAnimation()
	{
		base.StartCoroutine(base.transform.parent.GetComponent<LevelUpWithOffers>().GemsStarterAnimation());
	}

	// Token: 0x06002BE6 RID: 11238 RVA: 0x000E70A8 File Offset: 0x000E52A8
	public void StartCoinsStarterAnimation()
	{
		base.StartCoroutine(base.transform.parent.GetComponent<LevelUpWithOffers>().CoinsStarterAnimation());
	}

	// Token: 0x040020B0 RID: 8368
	private const string DefaultCallerContext = "Reward Window";

	// Token: 0x040020B1 RID: 8369
	public bool ShowLoginsButton = true;

	// Token: 0x040020B2 RID: 8370
	public GameObject connectToSocialContainer;

	// Token: 0x040020B3 RID: 8371
	public List<UILabel> connectToSocial;

	// Token: 0x040020B4 RID: 8372
	public UIWidget containerWidget;

	// Token: 0x040020B5 RID: 8373
	public float widgetExpanded;

	// Token: 0x040020B6 RID: 8374
	public float widgetCollapsed;

	// Token: 0x040020B7 RID: 8375
	public float widgetNoConnectToSocial;

	// Token: 0x040020B8 RID: 8376
	public UIButton facebook;

	// Token: 0x040020B9 RID: 8377
	public UIButton twitter;

	// Token: 0x040020BA RID: 8378
	public UIButton continueButton;

	// Token: 0x040020BB RID: 8379
	public UIButton hideButton;

	// Token: 0x040020BC RID: 8380
	public UIButton facebookNoReward;

	// Token: 0x040020BD RID: 8381
	public UIButton twitterNoReward;

	// Token: 0x040020BE RID: 8382
	public UIButton continueAndShare;

	// Token: 0x040020BF RID: 8383
	public UIButton collect;

	// Token: 0x040020C0 RID: 8384
	public UIButton collectAndShare;

	// Token: 0x040020C1 RID: 8385
	public UIGrid innerGrid;

	// Token: 0x040020C2 RID: 8386
	public ToggleButton share;

	// Token: 0x040020C3 RID: 8387
	public GameObject shareContainer;

	// Token: 0x040020C4 RID: 8388
	public bool shouldHideExpGui = true;

	// Token: 0x040020C5 RID: 8389
	public GameObject soundsController;

	// Token: 0x040020C6 RID: 8390
	[Header("Not Connected")]
	public Vector3 continue_NotConnected;

	// Token: 0x040020C7 RID: 8391
	[Header("Not Connected")]
	public Vector3 facebook_NotConnected;

	// Token: 0x040020C8 RID: 8392
	[Header("Not Connected")]
	public Vector3 twitter_NotConnected;

	// Token: 0x040020C9 RID: 8393
	[Header("Twitter Connected")]
	public Vector3 continue_TwiiterConnected;

	// Token: 0x040020CA RID: 8394
	[Header("Twitter Connected")]
	public Vector3 facebook_TwiiterConnected;

	// Token: 0x040020CB RID: 8395
	[Header("Facebook Connected")]
	public Vector3 continue_FacebookConnected;

	// Token: 0x040020CC RID: 8396
	[Header("Facebook Connected")]
	public Vector3 twitter_FacebookConnected;

	// Token: 0x040020CD RID: 8397
	[Header("All Connected")]
	public Vector3 continue_AllConnected;

	// Token: 0x040020CE RID: 8398
	public Action shareAction;

	// Token: 0x040020CF RID: 8399
	public Action customHide;

	// Token: 0x040020D0 RID: 8400
	private bool _collectOnlyNoShare;

	// Token: 0x040020D1 RID: 8401
	[HideInInspector]
	public Animator animatorLevel;

	// Token: 0x040020D2 RID: 8402
	private IDisposable _backSubscription;

	// Token: 0x040020D3 RID: 8403
	private float _timeSinceUpdateConnetToSocialText;

	// Token: 0x040020D4 RID: 8404
	private FacebookController.StoryPriority _facebookPriority;

	// Token: 0x040020D5 RID: 8405
	private FacebookController.StoryPriority _twiiterPriority;
}
