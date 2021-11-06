using System;
using System.Collections;
using System.Collections.Generic;
using Rilisoft;
using Rilisoft.MiniJson;
using UnityEngine;

// Token: 0x020006CA RID: 1738
internal sealed class NotificationController : MonoBehaviour
{
	// Token: 0x170009FD RID: 2557
	// (get) Token: 0x06003C7F RID: 15487 RVA: 0x0013A224 File Offset: 0x00138424
	public float currentPlayTimeMatch
	{
		get
		{
			return this.savedPlayTimeInMatch + this.playTimeInMatch;
		}
	}

	// Token: 0x170009FE RID: 2558
	// (get) Token: 0x06003C80 RID: 15488 RVA: 0x0013A234 File Offset: 0x00138434
	public float currentPlayTime
	{
		get
		{
			return this.savedPlayTime + this.playTime;
		}
	}

	// Token: 0x06003C81 RID: 15489 RVA: 0x0013A244 File Offset: 0x00138444
	private void Start()
	{
		using (new ScopeLogger("NotificationController.Start()", false))
		{
			base.gameObject.AddComponent<LocalNotificationController>();
			if (!Load.LoadBool("bilZapuskKey"))
			{
				Save.SaveBool("bilZapuskKey", true);
			}
			else
			{
				base.StartCoroutine(this.appStart());
			}
			NotificationController.instance = this;
			float num;
			if (Storager.hasKey("PlayTime") && float.TryParse(Storager.getString("PlayTime", false), out num))
			{
				this.savedPlayTime = num;
			}
			float num2;
			if (Storager.hasKey("PlayTimeInMatch") && float.TryParse(Storager.getString("PlayTimeInMatch", false), out num2))
			{
				this.savedPlayTimeInMatch = num2;
			}
		}
	}

	// Token: 0x06003C82 RID: 15490 RVA: 0x0013A320 File Offset: 0x00138520
	private void Update()
	{
		if (this.pauserTemp)
		{
			this.pauserTemp = false;
			NotificationController._paused = true;
			PhotonNetwork.isMessageQueueRunning = true;
			PhotonNetwork.Disconnect();
		}
		if (!FriendsController.sharedController.idle)
		{
			this.playTime += Time.deltaTime;
			if (Initializer.Instance != null && (PhotonNetwork.room == null || PhotonNetwork.room.customProperties[ConnectSceneNGUIController.passwordProperty].Equals(string.Empty)) && !Defs.isDaterRegim && !Defs.isHunger && !Defs.isCOOP && !NetworkStartTable.LocalOrPasswordRoom())
			{
				this.playTimeInMatch += Time.deltaTime;
			}
		}
	}

	// Token: 0x06003C83 RID: 15491 RVA: 0x0013A3E8 File Offset: 0x001385E8
	public void SaveTimeValues()
	{
		InGameTimeKeeper.Instance.Save();
		if (this.playTime > 0f)
		{
			this.savedPlayTime += this.playTime;
			Debug.Log(string.Format("PlayTime saved: {0} (+{1})", this.savedPlayTime, this.playTime));
			this.playTime = 0f;
			Storager.setString("PlayTime", this.savedPlayTime.ToString(), false);
		}
		if (this.playTimeInMatch > 0f)
		{
			this.savedPlayTimeInMatch += this.playTimeInMatch;
			Debug.Log(string.Format("PlayTimeInMatch saved: {0} (+{1})", this.savedPlayTimeInMatch, this.playTimeInMatch));
			this.playTimeInMatch = 0f;
			Storager.setString("PlayTimeInMatch", this.savedPlayTimeInMatch.ToString(), false);
		}
	}

	// Token: 0x170009FF RID: 2559
	// (get) Token: 0x06003C84 RID: 15492 RVA: 0x0013A4D4 File Offset: 0x001386D4
	internal static bool Paused
	{
		get
		{
			return NotificationController._paused;
		}
	}

	// Token: 0x06003C85 RID: 15493 RVA: 0x0013A4DC File Offset: 0x001386DC
	internal static void ResetPaused()
	{
		NotificationController._paused = false;
	}

	// Token: 0x06003C86 RID: 15494 RVA: 0x0013A4E4 File Offset: 0x001386E4
	private void appStop()
	{
		bool flag = BankController.Instance != null && BankController.Instance.InterfaceEnabled;
		if (PhotonNetwork.connected)
		{
			NotificationController._paused = true;
		}
		int num = DateTime.Now.Hour;
		int num2 = 82800;
		num += 23;
		if (num > 24)
		{
			num -= 24;
		}
		int num3 = (num <= 16) ? (16 - num) : (24 - num + 16);
		num2 += num3 * 3600;
		DateTime now = DateTime.Now;
		DateTime dateTime = now + TimeSpan.FromHours(23.0);
		DateTime dateTime2 = (dateTime.Hour >= 16) ? dateTime.Date.AddHours(40.0) : dateTime.Date.AddHours(16.0);
		TimeSpan timeSpan = TimeSpan.FromDays(1.0);
		int num4 = 0;
		for (int i = 0; i < num4; i++)
		{
			int num5 = num2 + i * 86400;
			num5 = num5 - 1800 + UnityEngine.Random.Range(0, 3600);
		}
		string text = Json.Serialize(this._notificationIds);
		Debug.Log("Notifications to save: " + text);
		PlayerPrefs.SetString("Scheduled Notifications", text);
		PlayerPrefs.Save();
	}

	// Token: 0x06003C87 RID: 15495 RVA: 0x0013A64C File Offset: 0x0013884C
	private IEnumerator appStart()
	{
		NotificationController.timeStartApp = Time.time;
		yield break;
	}

	// Token: 0x06003C88 RID: 15496 RVA: 0x0013A660 File Offset: 0x00138860
	private IEnumerator OnApplicationPause(bool pauseStatus)
	{
		if (pauseStatus)
		{
			if (Initializer.Instance != null)
			{
				this.SaveTimeValues();
			}
			this.appStop();
			if (PhotonNetwork.connected && TimeGameController.sharedController == null && (Application.platform == RuntimePlatform.Android || !PhotonNetwork.isMessageQueueRunning || ConnectSceneNGUIController.sharedController != null))
			{
				PhotonNetwork.isMessageQueueRunning = true;
				PhotonNetwork.Disconnect();
			}
		}
		else
		{
			base.StartCoroutine(this.appStart());
			yield return null;
			yield return null;
			Tools.AddSessionNumber();
			CoroutineRunner.Instance.StartCoroutine(AnalyticsStuff.WaitInitializationThenLogGameDayCountCoroutine());
		}
		yield break;
	}

	// Token: 0x04002CAA RID: 11434
	private const string ScheduledNotificationsKey = "Scheduled Notifications";

	// Token: 0x04002CAB RID: 11435
	public static bool isGetEveryDayMoney;

	// Token: 0x04002CAC RID: 11436
	public static float timeStartApp;

	// Token: 0x04002CAD RID: 11437
	public bool pauserTemp;

	// Token: 0x04002CAE RID: 11438
	private float playTime;

	// Token: 0x04002CAF RID: 11439
	private float playTimeInMatch;

	// Token: 0x04002CB0 RID: 11440
	public float savedPlayTime;

	// Token: 0x04002CB1 RID: 11441
	public float savedPlayTimeInMatch;

	// Token: 0x04002CB2 RID: 11442
	public static NotificationController instance;

	// Token: 0x04002CB3 RID: 11443
	private static bool _paused;

	// Token: 0x04002CB4 RID: 11444
	private readonly List<int> _notificationIds = new List<int>();
}
