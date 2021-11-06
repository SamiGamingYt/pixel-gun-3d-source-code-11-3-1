using System;
using Holoville.HOTween;
using Holoville.HOTween.Core;
using Rilisoft;
using UnityEngine;

// Token: 0x02000645 RID: 1605
public class ButOpenGift : MonoBehaviour
{
	// Token: 0x1400005F RID: 95
	// (add) Token: 0x06003767 RID: 14183 RVA: 0x0011DB4C File Offset: 0x0011BD4C
	// (remove) Token: 0x06003768 RID: 14184 RVA: 0x0011DB64 File Offset: 0x0011BD64
	public static event Action onOpen;

	// Token: 0x17000915 RID: 2325
	// (get) Token: 0x06003769 RID: 14185 RVA: 0x0011DB7C File Offset: 0x0011BD7C
	// (set) Token: 0x0600376A RID: 14186 RVA: 0x0011DB84 File Offset: 0x0011BD84
	public bool ActiveHighLight
	{
		get
		{
			return this._activeHighLight;
		}
		set
		{
			if (this._activeHighLight != value)
			{
				this._activeHighLight = value;
				if (GiftBannerWindow.instance != null && GiftBannerWindow.instance.IsShow)
				{
					this._activeHighLight = false;
				}
				this.UpdateHUDStateGift();
			}
		}
	}

	// Token: 0x17000916 RID: 2326
	// (get) Token: 0x0600376B RID: 14187 RVA: 0x0011DBD0 File Offset: 0x0011BDD0
	// (set) Token: 0x0600376C RID: 14188 RVA: 0x0011DBD8 File Offset: 0x0011BDD8
	public bool IsPressBut
	{
		get
		{
			return this._isPressBut;
		}
		set
		{
			this._isPressBut = value;
			this.UpdateHUDStateGift();
		}
	}

	// Token: 0x17000917 RID: 2327
	// (get) Token: 0x0600376D RID: 14189 RVA: 0x0011DBE8 File Offset: 0x0011BDE8
	// (set) Token: 0x0600376E RID: 14190 RVA: 0x0011DBF0 File Offset: 0x0011BDF0
	public bool IsCanGetGift
	{
		get
		{
			return this._isCanGetGift;
		}
		set
		{
			this._isCanGetGift = value;
			this.SetStateColliderActive(this._isCanGetGift);
			this.UpdateHUDStateGift();
		}
	}

	// Token: 0x17000918 RID: 2328
	// (get) Token: 0x0600376F RID: 14191 RVA: 0x0011DC0C File Offset: 0x0011BE0C
	// (set) Token: 0x06003770 RID: 14192 RVA: 0x0011DC14 File Offset: 0x0011BE14
	public bool CanShowLabel
	{
		get
		{
			return this._canShowLabel;
		}
		set
		{
			this._canShowLabel = value;
			this.UpdateHUDStateGift();
		}
	}

	// Token: 0x06003771 RID: 14193 RVA: 0x0011DC24 File Offset: 0x0011BE24
	private void OnClick()
	{
		if (this.CanTap)
		{
			GiftScroll.canReCreateSlots = true;
			ButtonClickSound.Instance.PlayClick();
			MainMenuController.canRotationLobbyPlayer = false;
			GiftBannerWindow.isForceClose = false;
			GiftBannerWindow.isActiveBanner = true;
			if (ButOpenGift.onOpen != null)
			{
				ButOpenGift.onOpen();
			}
			MainMenuController.sharedController.SaveShowPanelAndClose();
			MainMenuController.sharedController.OnShowBannerGift();
			GiftBannerWindow.instance.SetVisibleBanner(false);
			this.ActiveHighLight = false;
			this.UpdateHUDStateGift();
		}
	}

	// Token: 0x06003772 RID: 14194 RVA: 0x0011DCA0 File Offset: 0x0011BEA0
	private void OnPress(bool isDown)
	{
		if (this.CanTap)
		{
			this.IsPressBut = isDown;
			this.UpdateHUDStateGift();
		}
	}

	// Token: 0x17000919 RID: 2329
	// (get) Token: 0x06003773 RID: 14195 RVA: 0x0011DCBC File Offset: 0x0011BEBC
	private bool CanTap
	{
		get
		{
			return !MainMenuController.SavedShwonLobbyLevelIsLessThanActual() && TrainingController.TrainingCompleted && GiftController.Instance.ActiveGift && AnimationGift.instance.objGift.activeSelf && (MainMenuController.sharedController == null || !MainMenuController.sharedController.LeaderboardsIsOpening) && (FriendsWindowGUI.Instance == null || !FriendsWindowGUI.Instance.InterfaceEnabled) && (MainMenuController.sharedController == null || !MainMenuController.sharedController.rotateCamera.IsAnimPlaying);
		}
	}

	// Token: 0x06003774 RID: 14196 RVA: 0x0011DD68 File Offset: 0x0011BF68
	private void OnDragOut()
	{
		this.OnPress(false);
	}

	// Token: 0x06003775 RID: 14197 RVA: 0x0011DD74 File Offset: 0x0011BF74
	private void Awake()
	{
		ButOpenGift.instance = this;
		HOTween.Init();
		this._activeHighLight = false;
		this._isPressBut = false;
		this._isCanGetGift = false;
	}

	// Token: 0x06003776 RID: 14198 RVA: 0x0011DDA4 File Offset: 0x0011BFA4
	private void OnEnable()
	{
		if (this.Billboard != null)
		{
			this.Billboard.BindTo(() => AnimationGift.instance.objGift.transform);
		}
	}

	// Token: 0x06003777 RID: 14199 RVA: 0x0011DDE0 File Offset: 0x0011BFE0
	private void Start()
	{
		GiftController.Instance.TryGetData();
		this.UpdateHUDStateGift();
	}

	// Token: 0x06003778 RID: 14200 RVA: 0x0011DDF4 File Offset: 0x0011BFF4
	private void OnDestroy()
	{
		ButOpenGift.instance = null;
	}

	// Token: 0x06003779 RID: 14201 RVA: 0x0011DDFC File Offset: 0x0011BFFC
	public void UpdateHUDStateGift()
	{
		if (TrainingController.TrainingCompleted && GiftController.Instance.ActiveGift)
		{
			if (this.ActiveHighLight)
			{
				if (this.IsPressBut)
				{
					this.SetStateClick(true);
				}
				else if (this.IsCanGetGift)
				{
					this.SetStateAnim();
				}
				else
				{
					this.SetStateClick(false);
				}
				if (this.CanShowLabel)
				{
					this.ShowLabelTap();
				}
				else
				{
					this.HideLabelTap();
				}
			}
			else
			{
				this.SetActiveHighLight(false);
				this.HideLabelTap();
			}
		}
		else
		{
			this.SetActiveHighLight(false);
			this.HideLabelTap();
			this._isPressBut = false;
			this._isCanGetGift = false;
			this._activeHighLight = false;
		}
	}

	// Token: 0x0600377A RID: 14202 RVA: 0x0011DEB8 File Offset: 0x0011C0B8
	public void OpenGift()
	{
		GiftBannerWindow.instance.SetVisibleBanner(true);
	}

	// Token: 0x0600377B RID: 14203 RVA: 0x0011DEC8 File Offset: 0x0011C0C8
	public void CloseGift()
	{
		GiftBannerWindow.instance.CloseBannerEndAnimtion();
		this.ActiveHighLight = true;
		MainMenuController.canRotationLobbyPlayer = true;
	}

	// Token: 0x0600377C RID: 14204 RVA: 0x0011DEE4 File Offset: 0x0011C0E4
	private void SetStateClick(bool val)
	{
		this.StopAnim();
		if (val)
		{
			this.SetColor(this.pressColor);
		}
		else
		{
			this.SetColor(this.normalColor);
		}
	}

	// Token: 0x0600377D RID: 14205 RVA: 0x0011DF10 File Offset: 0x0011C110
	private void SetStateAnim()
	{
		this.AnimHS2();
	}

	// Token: 0x0600377E RID: 14206 RVA: 0x0011DF18 File Offset: 0x0011C118
	private void SetActiveHighLight(bool val)
	{
		if (!val)
		{
			this.StopAnim();
			this.SetColor(new Color(0f, 0f, 0f, 0f));
		}
	}

	// Token: 0x0600377F RID: 14207 RVA: 0x0011DF58 File Offset: 0x0011C158
	private void StopAnim()
	{
		HOTween.Kill(this);
	}

	// Token: 0x06003780 RID: 14208 RVA: 0x0011DF64 File Offset: 0x0011C164
	private void AnimHS2()
	{
		HOTween.Kill(this);
		HOTween.To(this, this.speedAnim, new TweenParms().Prop("curColorHS", this.animColor).OnUpdate(delegate()
		{
			this.SetColor(this.curColorHS);
		}).OnComplete(new TweenDelegate.TweenCallback(this.AnimHS1)));
	}

	// Token: 0x06003781 RID: 14209 RVA: 0x0011DFC4 File Offset: 0x0011C1C4
	private void AnimHS1()
	{
		HOTween.Kill(this);
		HOTween.To(this, this.speedAnim, new TweenParms().Prop("curColorHS", this.normalColor).OnUpdate(delegate()
		{
			this.SetColor(this.curColorHS);
		}).OnComplete(new TweenDelegate.TweenCallback(this.AnimHS2)));
	}

	// Token: 0x06003782 RID: 14210 RVA: 0x0011E024 File Offset: 0x0011C224
	private void SetColor(Color needColor)
	{
		this.curColorHS = needColor;
		if (this.allHighlightMaterial != null)
		{
			for (int i = 0; i < this.allHighlightMaterial.Length; i++)
			{
				this.allHighlightMaterial[i].SetColor(this.nameShaderColor, needColor);
			}
		}
	}

	// Token: 0x06003783 RID: 14211 RVA: 0x0011E070 File Offset: 0x0011C270
	private void ShowLabelTap()
	{
		if (Nest.Instance != null)
		{
			Nest.Instance.SetNickLabelVisible(true);
		}
		if (this.labelOverGift != null && this.labelOverGift.gameObject.activeSelf)
		{
			return;
		}
		if (this.labelOverGift == null)
		{
			this.labelOverGift = NickLabelStack.sharedStack.GetNextCurrentLabel();
			this.labelOverGift.StartShow(NickLabelController.TypeNickLabel.GetGift, AnimationGift.instance.transform);
		}
		if (!this.labelOverGift.gameObject.activeSelf)
		{
			this.labelOverGift.gameObject.SetActive(true);
		}
	}

	// Token: 0x06003784 RID: 14212 RVA: 0x0011E11C File Offset: 0x0011C31C
	public void HideLabelTap()
	{
		if (TrainingController.TrainingCompleted && GiftController.Instance.ActiveGift && Nest.Instance != null)
		{
			Nest.Instance.SetNickLabelVisible(false);
		}
		if (this.labelOverGift == null || !this.labelOverGift.gameObject.activeSelf)
		{
			return;
		}
		if (this.labelOverGift != null && this.labelOverGift.gameObject.activeSelf)
		{
			this.labelOverGift.gameObject.SetActive(false);
		}
	}

	// Token: 0x06003785 RID: 14213 RVA: 0x0011E1BC File Offset: 0x0011C3BC
	private void SetStateColliderActive(bool val)
	{
		if (this.colActive)
		{
			this.colActive.enabled = val;
		}
		if (this.colNormal)
		{
			this.colNormal.enabled = !val;
		}
	}

	// Token: 0x04002849 RID: 10313
	public static ButOpenGift instance;

	// Token: 0x0400284A RID: 10314
	public string nameShaderColor = "_Color";

	// Token: 0x0400284B RID: 10315
	public float speedAnim = 1f;

	// Token: 0x0400284C RID: 10316
	public Color normalColor = Color.white;

	// Token: 0x0400284D RID: 10317
	public Color pressColor = Color.white;

	// Token: 0x0400284E RID: 10318
	public Color animColor = Color.white;

	// Token: 0x0400284F RID: 10319
	public Material[] allHighlightMaterial;

	// Token: 0x04002850 RID: 10320
	public Collider colActive;

	// Token: 0x04002851 RID: 10321
	public Collider colNormal;

	// Token: 0x04002852 RID: 10322
	public bool _activeHighLight;

	// Token: 0x04002853 RID: 10323
	public bool _isPressBut;

	// Token: 0x04002854 RID: 10324
	public bool _isCanGetGift;

	// Token: 0x04002855 RID: 10325
	public Color curColorHS;

	// Token: 0x04002856 RID: 10326
	private NickLabelController labelOverGift;

	// Token: 0x04002857 RID: 10327
	public BindedBillboard Billboard;

	// Token: 0x04002858 RID: 10328
	private bool _canShowLabel;
}
