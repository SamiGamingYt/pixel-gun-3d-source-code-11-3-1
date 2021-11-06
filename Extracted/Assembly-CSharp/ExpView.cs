using System;
using System.Collections;
using Rilisoft;
using UnityEngine;

// Token: 0x020005F8 RID: 1528
public sealed class ExpView : MonoBehaviour
{
	// Token: 0x170008B3 RID: 2227
	// (get) Token: 0x06003465 RID: 13413 RVA: 0x0010F068 File Offset: 0x0010D268
	public bool LevelUpPanelOpened
	{
		get
		{
			return this._levelUpPanel.ObjectIsActive || this._levelUpPanelTier.ObjectIsActive;
		}
	}

	// Token: 0x170008B4 RID: 2228
	// (get) Token: 0x06003466 RID: 13414 RVA: 0x0010F094 File Offset: 0x0010D294
	public LevelUpWithOffers CurrentVisiblePanel
	{
		get
		{
			if (this._levelUpPanel.ObjectIsActive)
			{
				return this._levelUpPanel.Value;
			}
			if (this._levelUpPanelTier.ObjectIsActive)
			{
				return this._levelUpPanelTier.Value;
			}
			return null;
		}
	}

	// Token: 0x170008B5 RID: 2229
	// (get) Token: 0x06003467 RID: 13415 RVA: 0x0010F0DC File Offset: 0x0010D2DC
	public LazyObject<LevelUpWithOffers> _levelUpPanel
	{
		get
		{
			if (this._levelUpPanelValue == null)
			{
				this._levelUpPanelValue = new LazyObject<LevelUpWithOffers>(this._levelUpPanelPrefab.ResourcePath, this._levelUpPanelsContainer);
			}
			return this._levelUpPanelValue;
		}
	}

	// Token: 0x170008B6 RID: 2230
	// (get) Token: 0x06003468 RID: 13416 RVA: 0x0010F10C File Offset: 0x0010D30C
	public LazyObject<LevelUpWithOffers> _levelUpPanelTier
	{
		get
		{
			if (this._levelUpPanelTierValue == null)
			{
				this._levelUpPanelTierValue = new LazyObject<LevelUpWithOffers>(this._levelUpPanelTierPrefab.ResourcePath, this._levelUpPanelsContainer);
			}
			return this._levelUpPanelTierValue;
		}
	}

	// Token: 0x170008B7 RID: 2231
	// (get) Token: 0x06003469 RID: 13417 RVA: 0x0010F13C File Offset: 0x0010D33C
	// (set) Token: 0x0600346A RID: 13418 RVA: 0x0010F14C File Offset: 0x0010D34C
	public bool VisibleHUD
	{
		get
		{
			return this.objHUD.activeSelf;
		}
		set
		{
			this.objHUD.SetActive(value);
		}
	}

	// Token: 0x170008B8 RID: 2232
	// (get) Token: 0x0600346B RID: 13419 RVA: 0x0010F15C File Offset: 0x0010D35C
	// (set) Token: 0x0600346C RID: 13420 RVA: 0x0010F190 File Offset: 0x0010D390
	public string ExperienceLabel
	{
		get
		{
			return (!(this.experienceLabel != null)) ? string.Empty : this.experienceLabel.text;
		}
		set
		{
			if (this.experienceLabel != null)
			{
				this.experienceLabel.text = (value ?? string.Empty);
			}
		}
	}

	// Token: 0x170008B9 RID: 2233
	// (get) Token: 0x0600346D RID: 13421 RVA: 0x0010F1BC File Offset: 0x0010D3BC
	// (set) Token: 0x0600346E RID: 13422 RVA: 0x0010F1FC File Offset: 0x0010D3FC
	public float CurrentProgress
	{
		get
		{
			return (!(this.currentProgress != null)) ? 0f : this.currentProgress.transform.localScale.x;
		}
		set
		{
			if (this.currentProgress != null)
			{
				this.currentProgress.transform.localScale = new Vector3(Mathf.Clamp(value, 0f, 1f), base.transform.localScale.y, base.transform.localScale.z);
			}
		}
	}

	// Token: 0x170008BA RID: 2234
	// (get) Token: 0x0600346F RID: 13423 RVA: 0x0010F268 File Offset: 0x0010D468
	// (set) Token: 0x06003470 RID: 13424 RVA: 0x0010F2A8 File Offset: 0x0010D4A8
	public float OldProgress
	{
		get
		{
			return (!(this.oldProgress != null)) ? 0f : this.oldProgress.transform.localScale.x;
		}
		set
		{
			if (this.oldProgress != null)
			{
				this.oldProgress.transform.localScale = new Vector3(Mathf.Clamp(value, 0f, 1f), base.transform.localScale.y, base.transform.localScale.z);
			}
		}
	}

	// Token: 0x170008BB RID: 2235
	// (get) Token: 0x06003471 RID: 13425 RVA: 0x0010F314 File Offset: 0x0010D514
	// (set) Token: 0x06003472 RID: 13426 RVA: 0x0010F368 File Offset: 0x0010D568
	public int RankSprite
	{
		get
		{
			if (this.rankSprite == null)
			{
				return 1;
			}
			string s = this.rankSprite.spriteName.Replace("Rank_", string.Empty);
			int num = 0;
			return (!int.TryParse(s, out num)) ? 1 : num;
		}
		set
		{
			if (this.rankSprite != null)
			{
				string spriteName = string.Format("Rank_{0}", value);
				this.rankSprite.spriteName = spriteName;
			}
		}
	}

	// Token: 0x170008BC RID: 2236
	// (get) Token: 0x06003473 RID: 13427 RVA: 0x0010F3A4 File Offset: 0x0010D5A4
	public LeveUpPanelShowOptions LevelUpPanelOptions
	{
		get
		{
			if (this._levelUpPanelOptions == null)
			{
				this._levelUpPanelOptions = new LeveUpPanelShowOptions();
			}
			return this._levelUpPanelOptions;
		}
	}

	// Token: 0x06003474 RID: 13428 RVA: 0x0010F3C4 File Offset: 0x0010D5C4
	private void Awake()
	{
		Singleton<SceneLoader>.Instance.OnSceneLoading += delegate(SceneLoadInfo loadInfo)
		{
			this._levelUpPanel.DestroyValue();
			this._levelUpPanelTier.DestroyValue();
		};
	}

	// Token: 0x06003475 RID: 13429 RVA: 0x0010F3DC File Offset: 0x0010D5DC
	public void ShowLevelUpPanel()
	{
		this._currentLevelUpPanel = ((!this.LevelUpPanelOptions.ShowTierView) ? this._levelUpPanel.Value : this._levelUpPanelTier.Value);
		this._currentLevelUpPanel.SetCurrentRank(this.LevelUpPanelOptions.CurrentRank.ToString());
		this._currentLevelUpPanel.SetRewardPrice(string.Concat(new object[]
		{
			"+",
			this.LevelUpPanelOptions.CoinsReward,
			"\n",
			LocalizationStore.Get("Key_0275")
		}));
		this._currentLevelUpPanel.SetGemsRewardPrice(string.Concat(new object[]
		{
			"+",
			this.LevelUpPanelOptions.GemsReward,
			"\n",
			LocalizationStore.Get("Key_0951")
		}));
		this._currentLevelUpPanel.SetAddHealthCount(string.Format(LocalizationStore.Get("Key_1856"), ExperienceController.sharedController.AddHealthOnCurLevel.ToString()));
		this._currentLevelUpPanel.SetItems(this.LevelUpPanelOptions.NewItems);
		this._currentLevelUpPanel.shareScript.share.IsChecked = this.LevelUpPanelOptions.ShareButtonEnabled;
		ExpController.ShowTierPanel(this._currentLevelUpPanel.gameObject);
	}

	// Token: 0x06003476 RID: 13430 RVA: 0x0010F534 File Offset: 0x0010D734
	public void ToBonus(int starterGemsReward, int starterCoinsReward)
	{
		if (this._currentLevelUpPanel != null)
		{
			this._currentLevelUpPanel.SetStarterBankValues(starterGemsReward, starterCoinsReward);
			this._currentLevelUpPanel.shareScript.animatorLevel.SetTrigger("Bonus");
		}
	}

	// Token: 0x06003477 RID: 13431 RVA: 0x0010F57C File Offset: 0x0010D77C
	public void StopAnimation()
	{
		if (this.currentProgress.gameObject.activeInHierarchy)
		{
			this.currentProgress.StopAllCoroutines();
		}
		if (this.oldProgress != null && this.oldProgress.gameObject.activeInHierarchy)
		{
			this.oldProgress.StopAllCoroutines();
			this.oldProgress.enabled = true;
			this.oldProgress.transform.localScale = this.currentProgress.transform.localScale;
		}
	}

	// Token: 0x06003478 RID: 13432 RVA: 0x0010F608 File Offset: 0x0010D808
	public IDisposable StartBlinkingWithNewProgress()
	{
		if (this.currentProgress == null || !this.currentProgress.gameObject.activeInHierarchy)
		{
			Debug.LogWarning("(currentProgress == null || !currentProgress.gameObject.activeInHierarchy)");
			return new ActionDisposable(delegate()
			{
			});
		}
		this.currentProgress.StopAllCoroutines();
		IEnumerator c = this.StartBlinkingCoroutine();
		this.currentProgress.StartCoroutine(c);
		return new ActionDisposable(delegate()
		{
			this.currentProgress.StopCoroutine(c);
			if (this.currentProgress != null)
			{
				this.currentProgress.enabled = true;
			}
		});
	}

	// Token: 0x06003479 RID: 13433 RVA: 0x0010F6B0 File Offset: 0x0010D8B0
	public void WaitAndUpdateOldProgress(AudioClip sound)
	{
		if (this.oldProgress == null || !this.oldProgress.gameObject.activeInHierarchy)
		{
			return;
		}
		this.oldProgress.StopAllCoroutines();
		this.oldProgress.StartCoroutine(this.WaitAndUpdateCoroutine(sound));
	}

	// Token: 0x0600347A RID: 13434 RVA: 0x0010F704 File Offset: 0x0010D904
	private void OnEnable()
	{
	}

	// Token: 0x0600347B RID: 13435 RVA: 0x0010F708 File Offset: 0x0010D908
	private void OnDisable()
	{
		this.oldProgress.enabled = true;
		this.oldProgress.transform.localScale = this.currentProgress.transform.localScale;
		if (this.currentProgress != null && this.currentProgress.gameObject.activeInHierarchy)
		{
			this.currentProgress.StopAllCoroutines();
		}
	}

	// Token: 0x0600347C RID: 13436 RVA: 0x0010F774 File Offset: 0x0010D974
	private IEnumerator StartBlinkingCoroutine()
	{
		for (int i = 0; i != 4; i++)
		{
			this.currentProgress.enabled = false;
			yield return new WaitForSeconds(0.15f);
			this.currentProgress.enabled = true;
			yield return new WaitForSeconds(0.15f);
		}
		yield break;
	}

	// Token: 0x0600347D RID: 13437 RVA: 0x0010F790 File Offset: 0x0010D990
	private IEnumerator WaitAndUpdateCoroutine(AudioClip sound)
	{
		yield return new WaitForSeconds(1.2f);
		if (this.currentProgress != null)
		{
			this.oldProgress.transform.localScale = this.currentProgress.transform.localScale;
		}
		if (Defs.isSoundFX)
		{
			NGUITools.PlaySound(sound);
		}
		yield break;
	}

	// Token: 0x0400267D RID: 9853
	public GameObject rankIndicatorContainer;

	// Token: 0x0400267E RID: 9854
	public UIRoot interfaceHolder;

	// Token: 0x0400267F RID: 9855
	public Camera experienceCamera;

	// Token: 0x04002680 RID: 9856
	public UISprite experienceFrame;

	// Token: 0x04002681 RID: 9857
	public UILabel experienceLabel;

	// Token: 0x04002682 RID: 9858
	public UISprite currentProgress;

	// Token: 0x04002683 RID: 9859
	public UISprite oldProgress;

	// Token: 0x04002684 RID: 9860
	public UISprite rankSprite;

	// Token: 0x04002685 RID: 9861
	[SerializeField]
	private PrefabHandler _levelUpPanelPrefab;

	// Token: 0x04002686 RID: 9862
	[SerializeField]
	private PrefabHandler _levelUpPanelTierPrefab;

	// Token: 0x04002687 RID: 9863
	public GameObject objHUD;

	// Token: 0x04002688 RID: 9864
	[SerializeField]
	private GameObject _levelUpPanelsContainer;

	// Token: 0x04002689 RID: 9865
	private LazyObject<LevelUpWithOffers> _levelUpPanelValue;

	// Token: 0x0400268A RID: 9866
	private LazyObject<LevelUpWithOffers> _levelUpPanelTierValue;

	// Token: 0x0400268B RID: 9867
	private LevelUpWithOffers _currentLevelUpPanel;

	// Token: 0x0400268C RID: 9868
	private LeveUpPanelShowOptions _levelUpPanelOptions;
}
