using System;
using Rilisoft;
using UnityEngine;

// Token: 0x020006CE RID: 1742
public class PauseGUIController : MonoBehaviour
{
	// Token: 0x17000A00 RID: 2560
	// (get) Token: 0x06003C96 RID: 15510 RVA: 0x0013AF1C File Offset: 0x0013911C
	// (set) Token: 0x06003C97 RID: 15511 RVA: 0x0013AF24 File Offset: 0x00139124
	public static PauseGUIController Instance { get; private set; }

	// Token: 0x17000A01 RID: 2561
	// (get) Token: 0x06003C98 RID: 15512 RVA: 0x0013AF2C File Offset: 0x0013912C
	public PauseNGUIController SettingsPanel
	{
		get
		{
			if (this._pauseNguiLazy == null)
			{
				this._pauseNguiLazy = new LazyObject<PauseNGUIController>(this._settingsPrefab.ResourcePath, InGameGUI.sharedInGameGUI.SubpanelsContainer);
			}
			return this._pauseNguiLazy.Value;
		}
	}

	// Token: 0x17000A02 RID: 2562
	// (get) Token: 0x06003C99 RID: 15513 RVA: 0x0013AF70 File Offset: 0x00139170
	public bool IsPaused
	{
		get
		{
			return base.gameObject.activeSelf;
		}
	}

	// Token: 0x06003C9A RID: 15514 RVA: 0x0013AF80 File Offset: 0x00139180
	private void HandleButtonExitClick()
	{
		ButtonClickSound.Instance.PlayClick();
		if (InGameGUI.sharedInGameGUI != null && InGameGUI.sharedInGameGUI.playerMoveC != null)
		{
			InGameGUI.sharedInGameGUI.blockedCollider.SetActive(true);
			InGameGUI.sharedInGameGUI.playerMoveC.QuitGame();
		}
		else if (PhotonNetwork.room != null)
		{
			coinsShop.hideCoinsShop();
			coinsPlashka.hidePlashka();
			Defs.typeDisconnectGame = Defs.DisconectGameType.Exit;
			PhotonNetwork.LeaveRoom();
		}
	}

	// Token: 0x06003C9B RID: 15515 RVA: 0x0013B000 File Offset: 0x00139200
	private void HandleDialogBackClick()
	{
		ButtonClickSound.Instance.PlayClick();
		this.ShowConfirmDialog(false, 0);
	}

	// Token: 0x06003C9C RID: 15516 RVA: 0x0013B014 File Offset: 0x00139214
	private void ShowConfirmDialog(bool visible, int ratingValue)
	{
		ButtonClickSound.Instance.PlayClick();
		this._btnsPanel.SetActive(!visible);
		this._confirmDialogPanel.SetActive(visible);
		if (visible)
		{
			this._btnDialogText.text = string.Format(LocalizationStore.Get("Key_2851"), Mathf.Abs(ratingValue).ToString());
			if (string.IsNullOrEmpty(this._btnDialogText.text))
			{
				this._btnDialogText.text = ratingValue.ToString();
			}
		}
	}

	// Token: 0x06003C9D RID: 15517 RVA: 0x0013B09C File Offset: 0x0013929C
	private void Awake()
	{
		PauseGUIController.Instance = this;
		this._btnResume.GetComponent<ButtonHandler>().Clicked += delegate(object sender, EventArgs e)
		{
			this.Close();
		};
		this._btnExit.GetComponent<ButtonHandler>().Clicked += delegate(object sender, EventArgs e)
		{
			if (this.InPauseShop)
			{
				return;
			}
			if (Time.realtimeSinceStartup < this._lastBackFromShopTime + 0.5f)
			{
				return;
			}
			if (Defs.isDuel && WeaponManager.sharedManager.myNetworkStartTable != null)
			{
				int currentRatingChange = WeaponManager.sharedManager.myNetworkStartTable.GetCurrentRatingChange(true);
				if (currentRatingChange < 0)
				{
					this.ShowConfirmDialog(true, currentRatingChange);
					return;
				}
			}
			this.HandleButtonExitClick();
		};
		this._btnDialogYes.GetComponent<ButtonHandler>().Clicked += delegate(object sender, EventArgs e)
		{
			if (Defs.isDuel && WeaponManager.sharedManager.myNetworkStartTable != null)
			{
				WeaponManager.sharedManager.myNetworkStartTable.exitFromMenu = true;
			}
			this.HandleButtonExitClick();
		};
		this._btnDialogNo.GetComponent<ButtonHandler>().Clicked += delegate(object sender, EventArgs e)
		{
			this.HandleDialogBackClick();
		};
		this._btnBank.GetComponent<ButtonHandler>().Clicked += delegate(object sender, EventArgs e)
		{
			if (this.InPauseShop)
			{
				return;
			}
			ButtonClickSound.Instance.PlayClick();
			ExperienceController.sharedController.isShowRanks = false;
			if (InGameGUI.sharedInGameGUI != null && InGameGUI.sharedInGameGUI.playerMoveC != null)
			{
				InGameGUI.sharedInGameGUI.playerMoveC.GoToShopFromPause();
			}
		};
		this._btnSettings.GetComponent<ButtonHandler>().Clicked += delegate(object sender, EventArgs e)
		{
			this.SettingsPanel.gameObject.SetActive(true);
		};
	}

	// Token: 0x06003C9E RID: 15518 RVA: 0x0013B158 File Offset: 0x00139358
	private void OnEnable()
	{
		if (!TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage == TrainingController.NewTrainingCompletedStage.None)
		{
			this._btnBank.gameObject.SetActive(false);
		}
		if (this._backSubscription != null)
		{
			this._backSubscription.Dispose();
		}
		this._backSubscription = BackSystem.Instance.Register(new Action(this.Close), "Pause window");
	}

	// Token: 0x06003C9F RID: 15519 RVA: 0x0013B1C4 File Offset: 0x001393C4
	private void Update()
	{
		if (!this.InPauseShop)
		{
			if (this._shopOpened)
			{
				this._lastBackFromShopTime = Time.realtimeSinceStartup;
			}
			this._shopOpened = false;
			if (!Defs.isMulti)
			{
				ExperienceController.sharedController.isShowRanks = true;
			}
		}
		else
		{
			this._shopOpened = true;
			this._lastBackFromShopTime = float.PositiveInfinity;
		}
	}

	// Token: 0x06003CA0 RID: 15520 RVA: 0x0013B228 File Offset: 0x00139428
	private void OnDisable()
	{
		if (this._backSubscription != null)
		{
			this._backSubscription.Dispose();
			this._backSubscription = null;
		}
	}

	// Token: 0x17000A03 RID: 2563
	// (get) Token: 0x06003CA1 RID: 15521 RVA: 0x0013B248 File Offset: 0x00139448
	private bool InPauseShop
	{
		get
		{
			return InGameGUI.sharedInGameGUI != null && InGameGUI.sharedInGameGUI.playerMoveC != null && InGameGUI.sharedInGameGUI.playerMoveC.isInappWinOpen;
		}
	}

	// Token: 0x06003CA2 RID: 15522 RVA: 0x0013B284 File Offset: 0x00139484
	private void Close()
	{
		if (this.InPauseShop)
		{
			return;
		}
		this.ShowConfirmDialog(false, 0);
		ButtonClickSound.Instance.PlayClick();
		Debug.Log((InGameGUI.sharedInGameGUI != null) + " " + (InGameGUI.sharedInGameGUI.playerMoveC != null));
		if (InGameGUI.sharedInGameGUI != null && InGameGUI.sharedInGameGUI.playerMoveC != null)
		{
			InGameGUI.sharedInGameGUI.playerMoveC.SetPause(true);
		}
		else
		{
			base.gameObject.SetActive(false);
		}
		ExperienceController.sharedController.isShowRanks = false;
		ExpController.Instance.InterfaceEnabled = false;
	}

	// Token: 0x04002CCF RID: 11471
	[SerializeField]
	private UIButton _btnResume;

	// Token: 0x04002CD0 RID: 11472
	[SerializeField]
	private UIButton _btnExit;

	// Token: 0x04002CD1 RID: 11473
	[SerializeField]
	private UIButton _btnSettings;

	// Token: 0x04002CD2 RID: 11474
	[SerializeField]
	private UIButton _btnBank;

	// Token: 0x04002CD3 RID: 11475
	[SerializeField]
	private GameObject _btnsPanel;

	// Token: 0x04002CD4 RID: 11476
	[SerializeField]
	private GameObject _confirmDialogPanel;

	// Token: 0x04002CD5 RID: 11477
	[SerializeField]
	private UIButton _btnDialogYes;

	// Token: 0x04002CD6 RID: 11478
	[SerializeField]
	private UIButton _btnDialogNo;

	// Token: 0x04002CD7 RID: 11479
	[SerializeField]
	private UILabel _btnDialogText;

	// Token: 0x04002CD8 RID: 11480
	[SerializeField]
	private PrefabHandler _settingsPrefab;

	// Token: 0x04002CD9 RID: 11481
	private IDisposable _backSubscription;

	// Token: 0x04002CDA RID: 11482
	private LazyObject<PauseNGUIController> _pauseNguiLazy;

	// Token: 0x04002CDB RID: 11483
	private bool _shopOpened;

	// Token: 0x04002CDC RID: 11484
	private float _lastBackFromShopTime;
}
