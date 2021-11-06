using System;
using System.Collections;
using Rilisoft;
using UnityEngine;

// Token: 0x02000644 RID: 1604
public class AnimationGift : MonoBehaviour
{
	// Token: 0x1400005E RID: 94
	// (add) Token: 0x0600374E RID: 14158 RVA: 0x0011D430 File Offset: 0x0011B630
	// (remove) Token: 0x0600374F RID: 14159 RVA: 0x0011D448 File Offset: 0x0011B648
	public static event Action onEndAnimOpen;

	// Token: 0x06003750 RID: 14160 RVA: 0x0011D460 File Offset: 0x0011B660
	private void Awake()
	{
		AnimationGift.instance = this;
		this._animator = this.objGift.GetComponent<Animator>();
		this.SetVisibleObjGift(false);
	}

	// Token: 0x06003751 RID: 14161 RVA: 0x0011D480 File Offset: 0x0011B680
	private void OnEnable()
	{
		GiftBannerWindow.onGetGift += this.OpenGift;
		GiftBannerWindow.onHideInfoGift += this.CloseGift;
		GiftBannerWindow.onHideInfoGift += this.CheckStateGift;
		GiftBannerWindow.onOpenInfoGift += this.OnOpenInfoGift;
		GiftController.OnTimerEnded += this.CheckStateGift;
		GiftController.OnChangeSlots += this.CheckVisibleGift;
		MainMenuController.onLoadMenu += this.OnLoadMenu;
		TrainingController.onChangeTraining += this.CheckVisibleGift;
		FriendsController.ServerTimeUpdated += this.CheckVisibleGift;
		MainMenuHeroCamera.onEndOpenGift += this.EventOpenEndCam;
		MainMenuHeroCamera.onEndCloseGift += this.EventCloseEndCam;
		MainMenuController.onActiveMainMenu += this.ChangeActiveMainMenu;
	}

	// Token: 0x06003752 RID: 14162 RVA: 0x0011D55C File Offset: 0x0011B75C
	private void OnDisable()
	{
		GiftBannerWindow.onHideInfoGift -= this.CloseGift;
		GiftBannerWindow.onGetGift -= this.OpenGift;
		GiftBannerWindow.onHideInfoGift -= this.CheckStateGift;
		GiftBannerWindow.onOpenInfoGift -= this.OnOpenInfoGift;
		GiftController.OnTimerEnded -= this.CheckStateGift;
		FriendsController.ServerTimeUpdated -= this.CheckVisibleGift;
		GiftController.OnChangeSlots -= this.CheckVisibleGift;
		MainMenuController.onLoadMenu -= this.OnLoadMenu;
		TrainingController.onChangeTraining -= this.CheckVisibleGift;
		MainMenuHeroCamera.onEndOpenGift -= this.EventOpenEndCam;
		MainMenuHeroCamera.onEndCloseGift -= this.EventCloseEndCam;
		MainMenuController.onActiveMainMenu -= this.ChangeActiveMainMenu;
	}

	// Token: 0x06003753 RID: 14163 RVA: 0x0011D638 File Offset: 0x0011B838
	private void OnDetstroy()
	{
		AnimationGift.instance = null;
	}

	// Token: 0x06003754 RID: 14164 RVA: 0x0011D640 File Offset: 0x0011B840
	private void OnLoadMenu()
	{
		this.CheckVisibleGift();
	}

	// Token: 0x06003755 RID: 14165 RVA: 0x0011D648 File Offset: 0x0011B848
	public void OpenGift()
	{
		base.StartCoroutine(this.WaitOpenGift());
		if (Defs.isSoundFX && base.GetComponent<AudioSource>() && this.soundOpen)
		{
			base.GetComponent<AudioSource>().PlayOneShot(this.soundOpen);
		}
		TutorialQuestManager.Instance.AddFulfilledQuest("getGotcha");
		QuestMediator.NotifyGetGotcha();
	}

	// Token: 0x06003756 RID: 14166 RVA: 0x0011D6B4 File Offset: 0x0011B8B4
	private IEnumerator WaitOpenGift()
	{
		yield return new WaitForSeconds(this.timeoutShowGift);
		if (AnimationGift.onEndAnimOpen != null)
		{
			AnimationGift.onEndAnimOpen();
		}
		yield break;
	}

	// Token: 0x06003757 RID: 14167 RVA: 0x0011D6D0 File Offset: 0x0011B8D0
	public void OnOpenInfoGift()
	{
		if (this._animator != null)
		{
			this._animator.SetBool("IsOpen", true);
		}
		if (Defs.isSoundFX && base.GetComponent<AudioSource>() && this.soundGetGift)
		{
			base.GetComponent<AudioSource>().PlayOneShot(this.soundGetGift);
		}
	}

	// Token: 0x06003758 RID: 14168 RVA: 0x0011D73C File Offset: 0x0011B93C
	public void CloseGift()
	{
		if (this._animator != null)
		{
			this._animator.SetBool("IsOpen", false);
		}
		if (Defs.isSoundFX && base.GetComponent<AudioSource>() && this.soundClose)
		{
			base.GetComponent<AudioSource>().PlayOneShot(this.soundClose);
		}
	}

	// Token: 0x06003759 RID: 14169 RVA: 0x0011D7A8 File Offset: 0x0011B9A8
	public void CheckStateGift()
	{
		bool flag = false;
		if (ButOpenGift.instance != null)
		{
			ButOpenGift.instance.IsCanGetGift = (GiftController.Instance.CanGetTimerGift || flag);
		}
		if (this._animator != null)
		{
			this._animator.SetBool("IsEnabled", GiftController.Instance.CanGetTimerGift || flag);
		}
	}

	// Token: 0x0600375A RID: 14170 RVA: 0x0011D818 File Offset: 0x0011BA18
	public void CheckVisibleGift()
	{
		if (TrainingController.TrainingCompleted && GiftController.Instance.ActiveGift && !MainMenuController.sharedController.FreePanelIsActive && !MainMenuController.sharedController.settingsPanel.activeSelf)
		{
			this.SetVisibleObjGift(true);
			this.CheckStateGift();
		}
		else
		{
			this.SetVisibleObjGift(false);
		}
	}

	// Token: 0x0600375B RID: 14171 RVA: 0x0011D87C File Offset: 0x0011BA7C
	public void ResetAnimation()
	{
		if (this._animator != null)
		{
			this._animator.SetBool("IsOpen", false);
		}
		base.StopCoroutine("WaitOpenGift");
		if (ButOpenGift.instance != null)
		{
			ButOpenGift.instance.CanShowLabel = false;
		}
	}

	// Token: 0x0600375C RID: 14172 RVA: 0x0011D8D4 File Offset: 0x0011BAD4
	public void StartAnimForGetGift()
	{
		if (this._animator != null)
		{
			this._animator.SetBool("IsEnabled", true);
		}
	}

	// Token: 0x0600375D RID: 14173 RVA: 0x0011D904 File Offset: 0x0011BB04
	public void StopAnimForGetGift()
	{
		this.CheckStateGift();
	}

	// Token: 0x0600375E RID: 14174 RVA: 0x0011D90C File Offset: 0x0011BB0C
	private void SetVisibleObjGift(bool val)
	{
		if (this.objGift.activeSelf == val)
		{
			return;
		}
		if (GiftBannerWindow.instance != null && GiftBannerWindow.instance.IsShow && GiftBannerWindow.instance.curStateAnimAward != GiftBannerWindow.StepAnimation.none)
		{
			return;
		}
		if (!TrainingController.TrainingCompleted || GiftController.Instance == null || !GiftController.Instance.ActiveGift)
		{
			val = false;
		}
		if (ButOpenGift.instance != null)
		{
			ButOpenGift.instance.ActiveHighLight = val;
		}
		this.objGift.SetActive(val);
		if (val)
		{
			base.Invoke("WaitEndAnimShow", 1f);
		}
		else
		{
			if (ButOpenGift.instance != null)
			{
				ButOpenGift.instance.CanShowLabel = false;
			}
			if (GiftBannerWindow.instance != null && GiftBannerWindow.instance.bannerObj.activeInHierarchy)
			{
				GiftBannerWindow.instance.ForceCloseAll();
			}
		}
	}

	// Token: 0x0600375F RID: 14175 RVA: 0x0011DA14 File Offset: 0x0011BC14
	private void WaitEndAnimShow()
	{
		base.CancelInvoke("WaitEndAnimShow");
		if (ButOpenGift.instance != null)
		{
			ButOpenGift.instance.CanShowLabel = true;
		}
	}

	// Token: 0x06003760 RID: 14176 RVA: 0x0011DA48 File Offset: 0x0011BC48
	private void OnApplicationPause(bool pausing)
	{
		if (!pausing)
		{
			base.Invoke("CheckVisibleGift", 0.1f);
		}
	}

	// Token: 0x06003761 RID: 14177 RVA: 0x0011DA60 File Offset: 0x0011BC60
	private void ChangeLayer(string nameLayer)
	{
		Renderer[] componentsInChildren = base.GetComponentsInChildren<Renderer>(true);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].gameObject.layer = LayerMask.NameToLayer(nameLayer);
		}
	}

	// Token: 0x06003762 RID: 14178 RVA: 0x0011DA9C File Offset: 0x0011BC9C
	private void EventOpenEndCam()
	{
		if (ButOpenGift.instance != null)
		{
			ButOpenGift.instance.OpenGift();
		}
	}

	// Token: 0x06003763 RID: 14179 RVA: 0x0011DAB8 File Offset: 0x0011BCB8
	private void EventCloseEndCam()
	{
		if (ButOpenGift.instance != null)
		{
			ButOpenGift.instance.CloseGift();
		}
	}

	// Token: 0x06003764 RID: 14180 RVA: 0x0011DAD4 File Offset: 0x0011BCD4
	private void ChangeActiveMainMenu(bool val)
	{
		if (ButOpenGift.instance != null)
		{
			if (val)
			{
				ButOpenGift.instance.UpdateHUDStateGift();
			}
			else
			{
				ButOpenGift.instance.HideLabelTap();
			}
		}
	}

	// Token: 0x04002841 RID: 10305
	public static AnimationGift instance;

	// Token: 0x04002842 RID: 10306
	public GameObject objGift;

	// Token: 0x04002843 RID: 10307
	public AudioClip soundOpen;

	// Token: 0x04002844 RID: 10308
	public AudioClip soundClose;

	// Token: 0x04002845 RID: 10309
	public AudioClip soundGetGift;

	// Token: 0x04002846 RID: 10310
	private Animator _animator;

	// Token: 0x04002847 RID: 10311
	private float timeoutShowGift = 2f;
}
