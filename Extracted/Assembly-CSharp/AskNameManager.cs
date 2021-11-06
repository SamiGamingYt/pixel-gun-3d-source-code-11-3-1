using System;
using System.Collections;
using Rilisoft;
using UnityEngine;

// Token: 0x020006A9 RID: 1705
public class AskNameManager : MonoBehaviour
{
	// Token: 0x14000072 RID: 114
	// (add) Token: 0x06003B93 RID: 15251 RVA: 0x00135698 File Offset: 0x00133898
	// (remove) Token: 0x06003B94 RID: 15252 RVA: 0x001356B0 File Offset: 0x001338B0
	public static event Action onComplete;

	// Token: 0x06003B95 RID: 15253 RVA: 0x001356C8 File Offset: 0x001338C8
	private void Awake()
	{
		AskNameManager.instance = this;
		AskNameManager.isComplete = false;
		AskNameManager.isShow = false;
		this.objWindow.SetActive(false);
		this.objPanelSetName.SetActive(false);
		this.objPanelEnterName.SetActive(false);
		this.objLbWarning.SetActive(false);
		this.AskIsCompleted();
		MainMenuController.onEnableMenuForAskname += this.ShowWindow;
	}

	// Token: 0x06003B96 RID: 15254 RVA: 0x00135730 File Offset: 0x00133930
	private void OnEnable()
	{
	}

	// Token: 0x06003B97 RID: 15255 RVA: 0x00135734 File Offset: 0x00133934
	private void OnDisable()
	{
	}

	// Token: 0x06003B98 RID: 15256 RVA: 0x00135738 File Offset: 0x00133938
	private void OnDestroy()
	{
		MainMenuController.onEnableMenuForAskname -= this.ShowWindow;
		AskNameManager.instance = null;
	}

	// Token: 0x06003B99 RID: 15257 RVA: 0x00135754 File Offset: 0x00133954
	public void ShowWindow()
	{
		base.StopCoroutine("WaitAndShowWindow");
		base.StartCoroutine("WaitAndShowWindow");
	}

	// Token: 0x06003B9A RID: 15258 RVA: 0x00135770 File Offset: 0x00133970
	private IEnumerator WaitAndShowWindow()
	{
		if (this.AskIsCompleted())
		{
			yield break;
		}
		while (!this.CanShowWindow)
		{
			if (this.AskIsCompleted())
			{
				yield break;
			}
			yield return null;
			yield return null;
		}
		this.OnShowWindowSetName();
		yield break;
	}

	// Token: 0x06003B9B RID: 15259 RVA: 0x0013578C File Offset: 0x0013398C
	private bool AskIsCompleted()
	{
		bool flag = this.NameAlreadySet || TrainingController.TrainingCompleted;
		if (flag)
		{
			AskNameManager.isComplete = true;
			if (AskNameManager.onComplete != null)
			{
				AskNameManager.onComplete();
			}
		}
		return flag;
	}

	// Token: 0x06003B9C RID: 15260 RVA: 0x001357D0 File Offset: 0x001339D0
	private void OnShowWindowSetName()
	{
		if (this._backSubcripter != null)
		{
			this._backSubcripter.Dispose();
		}
		this._backSubcripter = BackSystem.Instance.Register(delegate
		{
		}, null);
		if (Defs.IsDeveloperBuild)
		{
			Debug.Log("+ OnShowWindowSetName");
		}
		AskNameManager.isShow = true;
		this.curChooseName = this.GetNameForAsk();
		this.lbPlayerName.text = this.curChooseName;
		this.inputPlayerName.value = this.curChooseName;
		this.CheckActiveBtnSetName();
		this.objPanelSetName.SetActive(true);
		this.objWindow.SetActive(true);
		this.isAutoName = true;
	}

	// Token: 0x06003B9D RID: 15261 RVA: 0x00135890 File Offset: 0x00133A90
	public void OnShowWindowEnterName()
	{
		this.objPanelEnterName.SetActive(true);
		this.objPanelSetName.SetActive(false);
		this.OnStartEnterName();
	}

	// Token: 0x170009CF RID: 2511
	// (get) Token: 0x06003B9E RID: 15262 RVA: 0x001358B0 File Offset: 0x00133AB0
	private bool CanShowWindow
	{
		get
		{
			return !this.NameAlreadySet && TrainingController.CompletedTrainingStage == TrainingController.NewTrainingCompletedStage.ShopCompleted && (BuildSettings.BuildTargetPlatform != RuntimePlatform.Android || MainMenuController.sharedController.SyncFuture.IsCompleted);
		}
	}

	// Token: 0x170009D0 RID: 2512
	// (get) Token: 0x06003B9F RID: 15263 RVA: 0x001358FC File Offset: 0x00133AFC
	// (set) Token: 0x06003BA0 RID: 15264 RVA: 0x00135924 File Offset: 0x00133B24
	private bool NameAlreadySet
	{
		get
		{
			if (this._NameAlreadySet == -1)
			{
				this._NameAlreadySet = Load.LoadInt("keyNameAlreadySet");
			}
			return this._NameAlreadySet == 1;
		}
		set
		{
			this._NameAlreadySet = ((!value) ? 0 : 1);
			Save.SaveInt("keyNameAlreadySet", this._NameAlreadySet);
		}
	}

	// Token: 0x06003BA1 RID: 15265 RVA: 0x0013594C File Offset: 0x00133B4C
	private string GetNameForAsk()
	{
		return ProfileController.GetPlayerNameOrDefault();
	}

	// Token: 0x170009D1 RID: 2513
	// (get) Token: 0x06003BA2 RID: 15266 RVA: 0x00135954 File Offset: 0x00133B54
	private bool CanSetName
	{
		get
		{
			string value = this.curChooseName.Trim();
			return !string.IsNullOrEmpty(value);
		}
	}

	// Token: 0x06003BA3 RID: 15267 RVA: 0x0013597C File Offset: 0x00133B7C
	private void CheckActiveBtnSetName()
	{
		BoxCollider component = this.btnSetName.GetComponent<BoxCollider>();
		this.objLbWarning.SetActive(false);
		if (this.CanSetName)
		{
			component.enabled = true;
			this.btnSetName.SetState(UIButtonColor.State.Normal, true);
		}
		else
		{
			this.objLbWarning.SetActive(true);
			component.enabled = false;
			this.btnSetName.SetState(UIButtonColor.State.Disabled, true);
		}
	}

	// Token: 0x06003BA4 RID: 15268 RVA: 0x001359E8 File Offset: 0x00133BE8
	public void OnStartEnterName()
	{
		if (this.isAutoName)
		{
			this.inputPlayerName.isSelected = true;
			this.curChooseName = string.Empty;
			this.inputPlayerName.value = this.curChooseName;
			this.CheckActiveBtnSetName();
			this.isAutoName = false;
		}
	}

	// Token: 0x06003BA5 RID: 15269 RVA: 0x00135A38 File Offset: 0x00133C38
	public void OnChangeName()
	{
		this.curChooseName = this.inputPlayerName.value;
		this.CheckActiveBtnSetName();
	}

	// Token: 0x06003BA6 RID: 15270 RVA: 0x00135A54 File Offset: 0x00133C54
	public void SaveChooseName()
	{
		if (ProfileController.Instance != null)
		{
			ProfileController.Instance.SaveNamePlayer(this.curChooseName);
		}
		if (PlayerPanel.instance != null)
		{
			PlayerPanel.instance.UpdateNickPlayer();
		}
		if (MainMenuController.sharedController != null)
		{
		}
		this.NameAlreadySet = true;
		this.OnCloseAllWindow();
	}

	// Token: 0x06003BA7 RID: 15271 RVA: 0x00135AB8 File Offset: 0x00133CB8
	private void OnCloseAllWindow()
	{
		if (this._backSubcripter != null)
		{
			this._backSubcripter.Dispose();
		}
		this.objWindow.SetActive(false);
		AskNameManager.isComplete = true;
		if (AskNameManager.onComplete != null)
		{
			AskNameManager.onComplete();
		}
		AskNameManager.isShow = false;
	}

	// Token: 0x06003BA8 RID: 15272 RVA: 0x00135B08 File Offset: 0x00133D08
	[ContextMenu("Show Window")]
	public void TestShow()
	{
		AskNameManager.isComplete = false;
		this.OnShowWindowSetName();
	}

	// Token: 0x06003BA9 RID: 15273 RVA: 0x00135B18 File Offset: 0x00133D18
	private void OnApplicationPause(bool pauseStatus)
	{
		if (pauseStatus && this.objWindow != null && this.objWindow.activeInHierarchy)
		{
			this.curChooseName = "Player";
			this.SaveChooseName();
		}
	}

	// Token: 0x04002C08 RID: 11272
	public const string keyNameAlreadySet = "keyNameAlreadySet";

	// Token: 0x04002C09 RID: 11273
	public static AskNameManager instance;

	// Token: 0x04002C0A RID: 11274
	public GameObject objWindow;

	// Token: 0x04002C0B RID: 11275
	public GameObject objPanelSetName;

	// Token: 0x04002C0C RID: 11276
	public GameObject objPanelEnterName;

	// Token: 0x04002C0D RID: 11277
	public UILabel lbPlayerName;

	// Token: 0x04002C0E RID: 11278
	public UIInput inputPlayerName;

	// Token: 0x04002C0F RID: 11279
	public UIButton btnSetName;

	// Token: 0x04002C10 RID: 11280
	public GameObject objLbWarning;

	// Token: 0x04002C11 RID: 11281
	private int _NameAlreadySet = -1;

	// Token: 0x04002C12 RID: 11282
	private string curChooseName = string.Empty;

	// Token: 0x04002C13 RID: 11283
	private bool isAutoName;

	// Token: 0x04002C14 RID: 11284
	public static bool isComplete;

	// Token: 0x04002C15 RID: 11285
	public static bool isShow;

	// Token: 0x04002C16 RID: 11286
	private IDisposable _backSubcripter;
}
