using System;
using Rilisoft;
using UnityEngine;

// Token: 0x0200012B RID: 299
public sealed class FriendsWindowGUI : MonoBehaviour
{
	// Token: 0x06000971 RID: 2417 RVA: 0x00039650 File Offset: 0x00037850
	private void Awake()
	{
		FriendsWindowGUI.Instance = this;
		this.InterfaceEnabled = false;
	}

	// Token: 0x06000972 RID: 2418 RVA: 0x00039660 File Offset: 0x00037860
	private void OnDestroy()
	{
		FriendsWindowGUI.Instance = null;
	}

	// Token: 0x06000973 RID: 2419 RVA: 0x00039668 File Offset: 0x00037868
	public void ShowInterface(Action _exitCallback = null)
	{
		this.InterfaceEnabled = true;
		this.OnExitCallback = _exitCallback;
		this.friendsWindow.SetStartState();
	}

	// Token: 0x170000FB RID: 251
	// (get) Token: 0x06000974 RID: 2420 RVA: 0x00039684 File Offset: 0x00037884
	// (set) Token: 0x06000975 RID: 2421 RVA: 0x00039694 File Offset: 0x00037894
	public bool InterfaceEnabled
	{
		get
		{
			return this.cameraObject.activeSelf;
		}
		private set
		{
			this.cameraObject.SetActive(value);
			if (this._escapeSubscription != null)
			{
				this._escapeSubscription.Dispose();
			}
			IDisposable escapeSubscription;
			if (value)
			{
				IDisposable disposable = BackSystem.Instance.Register(new Action(this.HandleEscape), "Friends");
				escapeSubscription = disposable;
			}
			else
			{
				escapeSubscription = null;
			}
			this._escapeSubscription = escapeSubscription;
		}
	}

	// Token: 0x06000976 RID: 2422 RVA: 0x000396F4 File Offset: 0x000378F4
	public void HideInterface()
	{
		if (this.OnExitCallback != null)
		{
			this.OnExitCallback();
		}
		ActivityIndicator.IsActiveIndicator = false;
		this.friendsWindow.SetCancelState();
		this.InterfaceEnabled = false;
	}

	// Token: 0x06000977 RID: 2423 RVA: 0x00039730 File Offset: 0x00037930
	private void HandleEscape()
	{
		this.HideInterface();
	}

	// Token: 0x040007BB RID: 1979
	public static FriendsWindowGUI Instance;

	// Token: 0x040007BC RID: 1980
	public GameObject cameraObject;

	// Token: 0x040007BD RID: 1981
	public Action OnExitCallback;

	// Token: 0x040007BE RID: 1982
	public FriendsWindowController friendsWindow;

	// Token: 0x040007BF RID: 1983
	private IDisposable _escapeSubscription;
}
