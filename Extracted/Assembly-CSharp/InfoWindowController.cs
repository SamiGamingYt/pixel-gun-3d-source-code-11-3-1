using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using I2.Loc;
using Rilisoft;
using UnityEngine;

// Token: 0x02000673 RID: 1651
[DisallowMultipleComponent]
public sealed class InfoWindowController : MonoBehaviour
{
	// Token: 0x17000966 RID: 2406
	// (get) Token: 0x0600396F RID: 14703 RVA: 0x0012A830 File Offset: 0x00128A30
	public static InfoWindowController Instance
	{
		get
		{
			if (InfoWindowController._instance == null)
			{
				UnityEngine.Object original = Resources.Load("InfoWindows");
				GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(original, Vector3.down * 567f, Quaternion.identity);
				InfoWindowController._instance = gameObject.GetComponent<InfoWindowController>();
				return InfoWindowController._instance;
			}
			return InfoWindowController._instance;
		}
	}

	// Token: 0x17000967 RID: 2407
	// (get) Token: 0x06003970 RID: 14704 RVA: 0x0012A890 File Offset: 0x00128A90
	public static bool IsActive
	{
		get
		{
			return InfoWindowController._instance != null && InfoWindowController._instance.infoBoxContainer != null && InfoWindowController._instance.infoBoxContainer.gameObject != null && InfoWindowController._instance.infoBoxContainer.gameObject.activeInHierarchy;
		}
	}

	// Token: 0x06003971 RID: 14705 RVA: 0x0012A8F4 File Offset: 0x00128AF4
	private void Awake()
	{
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
	}

	// Token: 0x06003972 RID: 14706 RVA: 0x0012A904 File Offset: 0x00128B04
	private void Start()
	{
		this.processingDataBoxLabel.text = LocalizationStore.Key_0348;
		LocalizationStore.AddEventCallAfterLocalize(new LocalizationManager.OnLocalizeCallback(this.HandleLocalizationChanged));
	}

	// Token: 0x06003973 RID: 14707 RVA: 0x0012A928 File Offset: 0x00128B28
	private void OnDisable()
	{
		if (InfoWindowController._showTopWindowQueue.Count > 0)
		{
			InfoWindowController.TopWindowData topWindowData = InfoWindowController._showTopWindowQueue.Dequeue();
			topWindowData.Opened = false;
			topWindowData.Box.isOpened = false;
		}
	}

	// Token: 0x06003974 RID: 14708 RVA: 0x0012A964 File Offset: 0x00128B64
	private void OnDestroy()
	{
		if (this._backSubscription != null)
		{
			this._backSubscription.Dispose();
			this._backSubscription = null;
		}
		LocalizationStore.DelEventCallAfterLocalize(new LocalizationManager.OnLocalizeCallback(this.HandleLocalizationChanged));
		if (this._unsubscribe != null)
		{
			this._unsubscribe();
		}
	}

	// Token: 0x06003975 RID: 14709 RVA: 0x0012A9B8 File Offset: 0x00128BB8
	private void HandleLocalizationChanged()
	{
		this.processingDataBoxLabel.text = LocalizationStore.Key_0348;
	}

	// Token: 0x06003976 RID: 14710 RVA: 0x0012A9CC File Offset: 0x00128BCC
	private void ActivateDevConsole()
	{
	}

	// Token: 0x06003977 RID: 14711 RVA: 0x0012A9D0 File Offset: 0x00128BD0
	private void ActivateInfoBox(string text)
	{
		if (InfoWindowController.Instance._backSubscription != null)
		{
			InfoWindowController.Instance._backSubscription.Dispose();
		}
		InfoWindowController.Instance._backSubscription = BackSystem.Instance.Register(new Action(InfoWindowController.Instance.HandleEscape), "Info Window");
		this.infoBoxLabel.text = text;
		this.infoBoxContainer.gameObject.SetActive(true);
		this.background.gameObject.SetActive(true);
	}

	// Token: 0x06003978 RID: 14712 RVA: 0x0012AA54 File Offset: 0x00128C54
	public void OnClickOkDialog()
	{
		if (this.DialogBoxOkClick != null)
		{
			this.DialogBoxOkClick();
		}
		this.Hide();
	}

	// Token: 0x06003979 RID: 14713 RVA: 0x0012AA74 File Offset: 0x00128C74
	public void OnClickCancelDialog()
	{
		if (this.DialogBoxCancelClick != null)
		{
			this.DialogBoxCancelClick();
		}
		this.Hide();
	}

	// Token: 0x0600397A RID: 14714 RVA: 0x0012AA94 File Offset: 0x00128C94
	private void ActivateDialogBox(string text, Action onOkClick, Action onCancelClick)
	{
		this.dialogBoxText.text = text;
		this.dialogBoxContainer.gameObject.SetActive(true);
		this.SetActiveBackground(true);
		this.DialogBoxOkClick = onOkClick;
		this.DialogBoxCancelClick = onCancelClick;
		if (InfoWindowController.Instance._backSubscription != null)
		{
			InfoWindowController.Instance._backSubscription.Dispose();
		}
		InfoWindowController.Instance._backSubscription = BackSystem.Instance.Register(new Action(InfoWindowController.Instance.HandleEscape), "Dialog Box");
	}

	// Token: 0x0600397B RID: 14715 RVA: 0x0012AB1C File Offset: 0x00128D1C
	public void ActivateRestorePanel(Action okCallback)
	{
		if (this.restoreWindowPanel == null)
		{
			return;
		}
		this.restoreWindowPanel.SetActive(true);
		this.SetActiveBackground(false);
		this.DialogBoxOkClick = okCallback;
		if (InfoWindowController.Instance._backSubscription != null)
		{
			InfoWindowController.Instance._backSubscription.Dispose();
		}
		InfoWindowController.Instance._backSubscription = BackSystem.Instance.Register(new Action(InfoWindowController.Instance.BackButtonFromRestoreClick), "Restore Panel");
	}

	// Token: 0x0600397C RID: 14716 RVA: 0x0012AB9C File Offset: 0x00128D9C
	private void BackButtonFromRestoreClick()
	{
	}

	// Token: 0x0600397D RID: 14717 RVA: 0x0012ABA0 File Offset: 0x00128DA0
	private void ShowNextTopWindow()
	{
		if (InfoWindowController._showTopWindowQueue.Any((InfoWindowController.TopWindowData i) => i.Opened))
		{
			return;
		}
		if (InfoWindowController._showTopWindowQueue.Count < 1)
		{
			return;
		}
		InfoWindowController.TopWindowData topWindowData = InfoWindowController._showTopWindowQueue.Peek();
		topWindowData.ShowThis();
		CoroutineRunner.Instance.StartCoroutine(this.HideBoxAfter(topWindowData, 3f));
		if (Defs.isSoundFX)
		{
			NGUITools.PlaySound(this.questCompleteSound);
		}
	}

	// Token: 0x0600397E RID: 14718 RVA: 0x0012AC2C File Offset: 0x00128E2C
	private IEnumerator HideBoxAfter(InfoWindowController.TopWindowData tw, float secs)
	{
		yield return new WaitForRealSeconds(secs);
		tw.Box.HideBox();
		CoroutineRunner.Instance.StartCoroutine(this.WaitAchieveBoxHided(tw));
		yield break;
	}

	// Token: 0x0600397F RID: 14719 RVA: 0x0012AC64 File Offset: 0x00128E64
	private IEnumerator WaitAchieveBoxHided(InfoWindowController.TopWindowData tw)
	{
		while (tw.Box.isOpened)
		{
			yield return null;
		}
		tw.Opened = false;
		if (InfoWindowController._showTopWindowQueue.Count > 0)
		{
			InfoWindowController._showTopWindowQueue.Dequeue();
		}
		this.ShowNextTopWindow();
		yield break;
	}

	// Token: 0x06003980 RID: 14720 RVA: 0x0012AC90 File Offset: 0x00128E90
	private void DeactivateQuestBox()
	{
		this.questBox.HideBox();
	}

	// Token: 0x06003981 RID: 14721 RVA: 0x0012ACA0 File Offset: 0x00128EA0
	private void DeactivateRestorePanel()
	{
		this.DialogBoxOkClick = null;
		this.DialogBoxCancelClick = null;
		if (this.restoreWindowPanel != null)
		{
			this.restoreWindowPanel.SetActive(false);
		}
	}

	// Token: 0x06003982 RID: 14722 RVA: 0x0012ACD0 File Offset: 0x00128ED0
	private void DeactivateDialogBox()
	{
		this.DialogBoxOkClick = null;
		this.DialogBoxCancelClick = null;
		this.dialogBoxContainer.gameObject.SetActive(false);
	}

	// Token: 0x06003983 RID: 14723 RVA: 0x0012ACF4 File Offset: 0x00128EF4
	private void DeactivateInfoBox()
	{
		this.background.gameObject.SetActive(false);
		this.infoBoxContainer.gameObject.SetActive(false);
	}

	// Token: 0x06003984 RID: 14724 RVA: 0x0012AD24 File Offset: 0x00128F24
	private void SetActiveProcessDataBox(bool enable)
	{
		this.processindDataBoxContainer.gameObject.SetActive(enable);
	}

	// Token: 0x06003985 RID: 14725 RVA: 0x0012AD38 File Offset: 0x00128F38
	private void SetActiveBackground(bool enable)
	{
		this.background.gameObject.SetActive(enable);
	}

	// Token: 0x06003986 RID: 14726 RVA: 0x0012AD4C File Offset: 0x00128F4C
	private void Initialize(InfoWindowController.WindowType typeWindow)
	{
		this._typeCurrentWindow = typeWindow;
		base.gameObject.SetActive(true);
	}

	// Token: 0x06003987 RID: 14727 RVA: 0x0012AD64 File Offset: 0x00128F64
	private void HideInfoAndProcessingBox()
	{
		if (this._unsubscribe != null)
		{
			this._unsubscribe();
		}
		if (this._typeCurrentWindow == InfoWindowController.WindowType.None || (this._typeCurrentWindow != InfoWindowController.WindowType.infoBox && this._typeCurrentWindow != InfoWindowController.WindowType.processDataBox))
		{
			return;
		}
		if (this._typeCurrentWindow == InfoWindowController.WindowType.infoBox)
		{
			this.DeactivateInfoBox();
		}
		else if (this._typeCurrentWindow == InfoWindowController.WindowType.processDataBox)
		{
			this.SetActiveProcessDataBox(false);
		}
		this.SetActiveBackground(false);
		this._typeCurrentWindow = InfoWindowController.WindowType.None;
		base.gameObject.SetActive(false);
	}

	// Token: 0x06003988 RID: 14728 RVA: 0x0012ADF0 File Offset: 0x00128FF0
	private void Hide()
	{
		if (this._backSubscription != null)
		{
			this._backSubscription.Dispose();
			this._backSubscription = null;
		}
		if (this._unsubscribe != null)
		{
			this._unsubscribe();
		}
		if (this._typeCurrentWindow == InfoWindowController.WindowType.None)
		{
			return;
		}
		if (this._typeCurrentWindow == InfoWindowController.WindowType.infoBox)
		{
			this.DeactivateInfoBox();
		}
		else if (this._typeCurrentWindow == InfoWindowController.WindowType.processDataBox)
		{
			this.SetActiveProcessDataBox(false);
		}
		else if (this._typeCurrentWindow == InfoWindowController.WindowType.DialogBox)
		{
			this.DeactivateDialogBox();
		}
		else if (this._typeCurrentWindow == InfoWindowController.WindowType.RestoreInventory)
		{
			this.DeactivateRestorePanel();
		}
		this.SetActiveBackground(false);
		this._typeCurrentWindow = InfoWindowController.WindowType.None;
		base.gameObject.SetActive(false);
	}

	// Token: 0x06003989 RID: 14729 RVA: 0x0012AEB0 File Offset: 0x001290B0
	public static void ShowInfoBox(string text)
	{
		InfoWindowController.Instance.Initialize(InfoWindowController.WindowType.infoBox);
		InfoWindowController.Instance.ActivateInfoBox(text);
	}

	// Token: 0x0600398A RID: 14730 RVA: 0x0012AEC8 File Offset: 0x001290C8
	public static void ShowDevConsole()
	{
		InfoWindowController.Instance.Initialize(InfoWindowController.WindowType.DeveloperConsoleMini);
		InfoWindowController.Instance.ActivateDevConsole();
	}

	// Token: 0x0600398B RID: 14731 RVA: 0x0012AEE0 File Offset: 0x001290E0
	private void HandleEscape()
	{
		this.OnClickCancelDialog();
	}

	// Token: 0x0600398C RID: 14732 RVA: 0x0012AEE8 File Offset: 0x001290E8
	public static void ShowProcessingDataBox()
	{
		InfoWindowController.Instance.Initialize(InfoWindowController.WindowType.processDataBox);
		InfoWindowController.Instance.SetActiveProcessDataBox(true);
		InfoWindowController.Instance.SetActiveBackground(false);
	}

	// Token: 0x0600398D RID: 14733 RVA: 0x0012AF18 File Offset: 0x00129118
	public static void BlockAllClick()
	{
		InfoWindowController.Instance.Initialize(InfoWindowController.WindowType.blockClick);
		InfoWindowController.Instance.SetActiveBackground(true);
	}

	// Token: 0x0600398E RID: 14734 RVA: 0x0012AF30 File Offset: 0x00129130
	public static void ShowDialogBox(string text, Action callbackOkButton, Action callbackCancelButton = null)
	{
		InfoWindowController.Instance.Initialize(InfoWindowController.WindowType.DialogBox);
		InfoWindowController.Instance.ActivateDialogBox(text, callbackOkButton, callbackCancelButton);
	}

	// Token: 0x0600398F RID: 14735 RVA: 0x0012AF58 File Offset: 0x00129158
	public static void ShowRestorePanel(Action okCallback)
	{
		InfoWindowController.Instance.Initialize(InfoWindowController.WindowType.RestoreInventory);
		InfoWindowController.Instance.ActivateRestorePanel(okCallback);
	}

	// Token: 0x06003990 RID: 14736 RVA: 0x0012AF70 File Offset: 0x00129170
	public static void ShowQuestBox(string header, string text)
	{
		InfoWindowController.Instance.Initialize(InfoWindowController.WindowType.QuestMessage);
		InfoWindowController._showTopWindowQueue.Enqueue(new InfoWindowController.ShowQuestData(InfoWindowController.Instance.questBox)
		{
			Header = header,
			Text = text
		});
		InfoWindowController.Instance.ShowNextTopWindow();
	}

	// Token: 0x06003991 RID: 14737 RVA: 0x0012AFBC File Offset: 0x001291BC
	public static void ShowAchievementsBox(string text, Texture bgTexture, string spriteIcon)
	{
		InfoWindowController.Instance.Initialize(InfoWindowController.WindowType.AchievementMessage);
		InfoWindowController._showTopWindowQueue.Enqueue(new InfoWindowController.ShowAchievementData(InfoWindowController.Instance.achievementBox)
		{
			Text = text,
			BgTexture = bgTexture,
			SpriteIcon = spriteIcon
		});
		InfoWindowController.Instance.ShowNextTopWindow();
	}

	// Token: 0x06003992 RID: 14738 RVA: 0x0012B010 File Offset: 0x00129210
	public void ShowBattleInvite(string friendNickname)
	{
		if (this._battleInviteBox == null)
		{
			return;
		}
		this.Initialize(InfoWindowController.WindowType.BattleInvite);
		InfoWindowController.TopWindowData item = new InfoWindowController.ShowBattleInviteData(this._battleInviteBox, friendNickname);
		InfoWindowController._showTopWindowQueue.Enqueue(item);
		InfoWindowController.Instance.ShowNextTopWindow();
	}

	// Token: 0x06003993 RID: 14739 RVA: 0x0012B05C File Offset: 0x0012925C
	public static void HideCurrentWindow()
	{
		InfoWindowController.Instance.Hide();
	}

	// Token: 0x06003994 RID: 14740 RVA: 0x0012B068 File Offset: 0x00129268
	public static void HideProcessing(float time)
	{
		InfoWindowController.Instance.Invoke("HideInfoAndProcessingBox", time);
	}

	// Token: 0x06003995 RID: 14741 RVA: 0x0012B07C File Offset: 0x0012927C
	public static void HideProcessing()
	{
		InfoWindowController.Instance.HideInfoAndProcessingBox();
	}

	// Token: 0x06003996 RID: 14742 RVA: 0x0012B088 File Offset: 0x00129288
	public void OnClickExitButton()
	{
		if (this._typeCurrentWindow == InfoWindowController.WindowType.blockClick)
		{
			return;
		}
		this.Hide();
	}

	// Token: 0x06003997 RID: 14743 RVA: 0x0012B0A0 File Offset: 0x001292A0
	public static void CheckShowRequestServerInfoBox(bool isComplete, bool isRequestExist)
	{
		if (!isComplete)
		{
			InfoWindowController.ShowInfoBox(LocalizationStore.Get("Key_1528"));
		}
		else if (isRequestExist)
		{
			InfoWindowController.ShowInfoBox(LocalizationStore.Get("Key_1563"));
		}
	}

	// Token: 0x04002A2C RID: 10796
	public Camera infoWindowCamera;

	// Token: 0x04002A2D RID: 10797
	public UIWidget background;

	// Token: 0x04002A2E RID: 10798
	[Header("Processing data box")]
	public UIWidget processindDataBoxContainer;

	// Token: 0x04002A2F RID: 10799
	public UILabel processingDataBoxLabel;

	// Token: 0x04002A30 RID: 10800
	[Header("Info box")]
	public UIWidget infoBoxContainer;

	// Token: 0x04002A31 RID: 10801
	public UILabel infoBoxLabel;

	// Token: 0x04002A32 RID: 10802
	[Header("Dialog box Warning")]
	public UIWidget dialogBoxContainer;

	// Token: 0x04002A33 RID: 10803
	public UILabel dialogBoxText;

	// Token: 0x04002A34 RID: 10804
	[Header("Restore Window")]
	public GameObject restoreWindowPanel;

	// Token: 0x04002A35 RID: 10805
	[Header("quest box")]
	public AchieveBox questBox;

	// Token: 0x04002A36 RID: 10806
	public UILabel questText;

	// Token: 0x04002A37 RID: 10807
	public AudioClip questCompleteSound;

	// Token: 0x04002A38 RID: 10808
	[Header("achievement box")]
	public AchieveBox achievementBox;

	// Token: 0x04002A39 RID: 10809
	public UILabel achievementText;

	// Token: 0x04002A3A RID: 10810
	public UITexture achievementTextureBg;

	// Token: 0x04002A3B RID: 10811
	public UISprite achievementSpriteIcon;

	// Token: 0x04002A3C RID: 10812
	[SerializeField]
	[Header("Battle Invite Box")]
	private AchieveBox _battleInviteBox;

	// Token: 0x04002A3D RID: 10813
	[SerializeField]
	private UILabel _friendNickname;

	// Token: 0x04002A3E RID: 10814
	[Header("")]
	public Transform InfoWindowsRoot;

	// Token: 0x04002A3F RID: 10815
	private GameObject developerConsole;

	// Token: 0x04002A40 RID: 10816
	private Action DialogBoxOkClick;

	// Token: 0x04002A41 RID: 10817
	private Action DialogBoxCancelClick;

	// Token: 0x04002A42 RID: 10818
	private InfoWindowController.WindowType _typeCurrentWindow;

	// Token: 0x04002A43 RID: 10819
	private static InfoWindowController _instance;

	// Token: 0x04002A44 RID: 10820
	private static readonly Queue<InfoWindowController.TopWindowData> _showTopWindowQueue = new Queue<InfoWindowController.TopWindowData>();

	// Token: 0x04002A45 RID: 10821
	private IDisposable _backSubscription;

	// Token: 0x04002A46 RID: 10822
	private Action _unsubscribe;

	// Token: 0x02000674 RID: 1652
	private enum WindowType
	{
		// Token: 0x04002A49 RID: 10825
		None,
		// Token: 0x04002A4A RID: 10826
		infoBox,
		// Token: 0x04002A4B RID: 10827
		processDataBox,
		// Token: 0x04002A4C RID: 10828
		blockClick,
		// Token: 0x04002A4D RID: 10829
		DialogBox,
		// Token: 0x04002A4E RID: 10830
		QuestMessage,
		// Token: 0x04002A4F RID: 10831
		RestoreInventory,
		// Token: 0x04002A50 RID: 10832
		DeveloperConsoleMini,
		// Token: 0x04002A51 RID: 10833
		AchievementMessage,
		// Token: 0x04002A52 RID: 10834
		BattleInvite
	}

	// Token: 0x02000675 RID: 1653
	private abstract class TopWindowData
	{
		// Token: 0x06003999 RID: 14745 RVA: 0x0012B0DC File Offset: 0x001292DC
		public TopWindowData(AchieveBox box)
		{
			this.Box = box;
		}

		// Token: 0x17000968 RID: 2408
		// (get) Token: 0x0600399A RID: 14746 RVA: 0x0012B0EC File Offset: 0x001292EC
		// (set) Token: 0x0600399B RID: 14747 RVA: 0x0012B0F4 File Offset: 0x001292F4
		public bool Opened { get; set; }

		// Token: 0x17000969 RID: 2409
		// (get) Token: 0x0600399C RID: 14748 RVA: 0x0012B100 File Offset: 0x00129300
		// (set) Token: 0x0600399D RID: 14749 RVA: 0x0012B108 File Offset: 0x00129308
		public AchieveBox Box { get; protected set; }

		// Token: 0x0600399E RID: 14750 RVA: 0x0012B114 File Offset: 0x00129314
		public virtual void ShowThis()
		{
			this.Opened = true;
		}
	}

	// Token: 0x02000676 RID: 1654
	private sealed class ShowBattleInviteData : InfoWindowController.TopWindowData
	{
		// Token: 0x0600399F RID: 14751 RVA: 0x0012B120 File Offset: 0x00129320
		public ShowBattleInviteData(AchieveBox box, string friendNickname) : base(box)
		{
			this._friendNickname = (friendNickname ?? string.Empty);
		}

		// Token: 0x060039A0 RID: 14752 RVA: 0x0012B13C File Offset: 0x0012933C
		public override void ShowThis()
		{
			if (InfoWindowController.Instance._friendNickname == null)
			{
				return;
			}
			InfoWindowController.Instance._friendNickname.text = this._friendNickname;
			base.Box.ShowBox();
		}

		// Token: 0x04002A55 RID: 10837
		private readonly string _friendNickname;
	}

	// Token: 0x02000677 RID: 1655
	private sealed class ShowQuestData : InfoWindowController.TopWindowData
	{
		// Token: 0x060039A1 RID: 14753 RVA: 0x0012B180 File Offset: 0x00129380
		public ShowQuestData(AchieveBox box) : base(box)
		{
		}

		// Token: 0x060039A2 RID: 14754 RVA: 0x0012B18C File Offset: 0x0012938C
		public override void ShowThis()
		{
			InfoWindowController.Instance.questText.text = this.Text;
			base.Box.ShowBox();
		}

		// Token: 0x04002A56 RID: 10838
		public string Header;

		// Token: 0x04002A57 RID: 10839
		public string Text;
	}

	// Token: 0x02000678 RID: 1656
	private sealed class ShowAchievementData : InfoWindowController.TopWindowData
	{
		// Token: 0x060039A3 RID: 14755 RVA: 0x0012B1BC File Offset: 0x001293BC
		public ShowAchievementData(AchieveBox box) : base(box)
		{
		}

		// Token: 0x060039A4 RID: 14756 RVA: 0x0012B1C8 File Offset: 0x001293C8
		public override void ShowThis()
		{
			InfoWindowController.Instance.achievementText.text = this.Text;
			InfoWindowController.Instance.achievementTextureBg.mainTexture = this.BgTexture;
			InfoWindowController.Instance.achievementTextureBg.fixedAspect = true;
			InfoWindowController.Instance.achievementSpriteIcon.spriteName = this.SpriteIcon;
			base.Box.ShowBox();
		}

		// Token: 0x04002A58 RID: 10840
		public string Text;

		// Token: 0x04002A59 RID: 10841
		public Texture BgTexture;

		// Token: 0x04002A5A RID: 10842
		public string SpriteIcon;
	}
}
