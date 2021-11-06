using System;
using System.Collections;
using System.Reflection;
using ExitGames.Client.Photon;
using Rilisoft;
using UnityEngine;

// Token: 0x0200085D RID: 2141
public class TimeGameController : MonoBehaviour
{
	// Token: 0x06004D6A RID: 19818 RVA: 0x001BF974 File Offset: 0x001BDB74
	private void OnApplicationPause(bool pauseStatus)
	{
		if (PhotonNetwork.connected)
		{
			if (pauseStatus)
			{
				if (Application.platform == RuntimePlatform.IPhonePlayer && !Defs.isDuel)
				{
					this.paused = true;
					this.wasPaused = true;
					PhotonNetwork.isMessageQueueRunning = false;
				}
				else
				{
					PhotonNetwork.isMessageQueueRunning = true;
					PhotonNetwork.Disconnect();
				}
			}
			else
			{
				if (Application.platform == RuntimePlatform.IPhonePlayer && !Defs.isDuel)
				{
					this.CheckPause();
				}
				PhotonNetwork.FetchServerTimestamp();
			}
		}
	}

	// Token: 0x06004D6B RID: 19819 RVA: 0x001BF9F0 File Offset: 0x001BDBF0
	private void Awake()
	{
		if (!Defs.isMulti || Defs.isHunger || Defs.isDuel)
		{
			base.enabled = false;
			return;
		}
		base.StartCoroutine(this.FetchServerTimestamp());
	}

	// Token: 0x06004D6C RID: 19820 RVA: 0x001BFA28 File Offset: 0x001BDC28
	private IEnumerator FetchServerTimestamp()
	{
		for (;;)
		{
			PhotonNetwork.FetchServerTimestamp();
			yield return new WaitForRealSeconds(60f);
		}
		yield break;
	}

	// Token: 0x06004D6D RID: 19821 RVA: 0x001BFA3C File Offset: 0x001BDC3C
	private void Start()
	{
		TimeGameController.sharedController = this;
		if (Defs.isMulti && !Defs.isInet && Network.isServer)
		{
			base.InvokeRepeating("SinchServerTimeInvoke", 0.1f, 2f);
			Debug.Log("TimeGameController: Start synch server time");
		}
	}

	// Token: 0x06004D6E RID: 19822 RVA: 0x001BFA8C File Offset: 0x001BDC8C
	[Obfuscation(Exclude = true)]
	public void SinchServerTimeInvoke()
	{
		base.GetComponent<NetworkView>().RPC("SynchTimeServer", RPCMode.Others, new object[]
		{
			(float)Network.time
		});
	}

	// Token: 0x06004D6F RID: 19823 RVA: 0x001BFAC0 File Offset: 0x001BDCC0
	public void StartMatch()
	{
		bool flag = false;
		this.matchEnding = false;
		if (CapturePointController.sharedController != null)
		{
			CapturePointController.sharedController.isEndMatch = false;
		}
		if (Defs.isCapturePoints || Defs.isFlag)
		{
			double num = Convert.ToDouble(PhotonNetwork.room.customProperties["TimeMatchEnd"]);
			if (num < -5000000.0)
			{
				flag = true;
			}
		}
		if (Defs.isInet && ((this.timeEndMatch < PhotonNetwork.time && !Defs.isFlag) || Initializer.players.Count == 0 || (Defs.isFlag && flag)))
		{
			ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
			double num2 = PhotonNetwork.time + (double)(((!Defs.isCOOP) ? ((int)PhotonNetwork.room.customProperties[ConnectSceneNGUIController.maxKillProperty]) : 4) * 60);
			if (num2 > 4294967.0 && PhotonNetwork.time < 4294967.0)
			{
				num2 = 4294967.0;
			}
			hashtable["TimeMatchEnd"] = num2;
			hashtable[ConnectSceneNGUIController.endingProperty] = 0;
			PhotonNetwork.room.SetCustomProperties(hashtable, null, false);
			this.matchEndingPos = 0;
			this.timerToEndMatch = num2 - PhotonNetwork.time;
		}
		if (!Defs.isInet && (this.timeEndMatch < this.networkTime || Initializer.players.Count == 0))
		{
			this.timeEndMatch = this.networkTime + (double)((PlayerPrefs.GetString("MaxKill", "9").Equals(string.Empty) ? 5 : int.Parse(PlayerPrefs.GetString("MaxKill", "5"))) * 60);
			base.GetComponent<NetworkView>().RPC("SynchTimeEnd", RPCMode.Others, new object[]
			{
				(float)this.timeEndMatch
			});
		}
	}

	// Token: 0x06004D70 RID: 19824 RVA: 0x001BFCB8 File Offset: 0x001BDEB8
	private void CheckPause()
	{
		this.paused = false;
		long currentUnixTime = Tools.CurrentUnixTime;
		if (this.pauseTime > currentUnixTime || this.pauseTime + 60L < currentUnixTime)
		{
			PhotonNetwork.isMessageQueueRunning = true;
			PhotonNetwork.Disconnect();
		}
	}

	// Token: 0x06004D71 RID: 19825 RVA: 0x001BFCFC File Offset: 0x001BDEFC
	private void Update()
	{
		if (this.paused && Defs.isInet && Application.platform == RuntimePlatform.IPhonePlayer)
		{
			this.CheckPause();
			if (!PhotonNetwork.connected)
			{
				return;
			}
		}
		this.ipServera = PhotonNetwork.ServerAddress;
		if (Defs.isInet && PhotonNetwork.room != null && PhotonNetwork.room.customProperties["TimeMatchEnd"] != null)
		{
			double num = this.networkTime - PhotonNetwork.time;
			if (WeaponManager.sharedManager.myPlayerMoveC != null && num > 6.0 && num < 600.0)
			{
				Debug.LogError(string.Concat(new object[]
				{
					"Speedhack detected! Delta: ",
					num,
					", Photon time: ",
					PhotonNetwork.time,
					", Last time: ",
					this.networkTime
				}));
				PhotonNetwork.isMessageQueueRunning = true;
				PhotonNetwork.Disconnect();
			}
			this.networkTime = PhotonNetwork.time;
			if (this.networkTime < 0.1)
			{
				return;
			}
			this.timeEndMatch = Convert.ToDouble(PhotonNetwork.room.customProperties["TimeMatchEnd"]);
			if (WeaponManager.sharedManager != null && WeaponManager.sharedManager.myPlayerMoveC != null && this.timeEndMatch > PhotonNetwork.time + 1500.0)
			{
				Initializer.Instance.goToConnect();
			}
			if (PhotonNetwork.room.customProperties.ContainsKey(ConnectSceneNGUIController.endingProperty))
			{
				this.matchEndingPos = (int)PhotonNetwork.room.customProperties[ConnectSceneNGUIController.endingProperty];
			}
			this.writtedMatchEndingPos = this.matchEndingPos;
			switch (this.matchEndingPos)
			{
			case 0:
				if (this.timeEndMatch < PhotonNetwork.time + (double)((!PhotonNetwork.isMasterClient) ? 110 : 130))
				{
					this.matchEndingPos = 2;
					Debug.Log("two minutes remain");
				}
				break;
			case 2:
				if (this.timeEndMatch < PhotonNetwork.time + (double)((!PhotonNetwork.isMasterClient) ? 50 : 70))
				{
					Debug.Log("one minute remain");
					this.matchEndingPos = 1;
				}
				break;
			}
			if (this.writtedMatchEndingPos != this.matchEndingPos)
			{
				Debug.Log("Write ending: " + this.matchEndingPos);
				ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
				hashtable[ConnectSceneNGUIController.endingProperty] = this.matchEndingPos;
				PhotonNetwork.room.SetCustomProperties(hashtable, null, false);
			}
			if (this.timeEndMatch > 4290000.0 && this.networkTime < 2000000.0)
			{
				ExitGames.Client.Photon.Hashtable hashtable2 = new ExitGames.Client.Photon.Hashtable();
				double num2 = this.networkTime + 60.0;
				hashtable2["TimeMatchEnd"] = num2;
				PhotonNetwork.room.SetCustomProperties(hashtable2, null, false);
			}
			if (this.timeEndMatch > 0.0)
			{
				this.timerToEndMatch = this.timeEndMatch - this.networkTime;
			}
			else
			{
				this.timerToEndMatch = -1.0;
			}
		}
		if (!Defs.isInet)
		{
			if (Network.isServer)
			{
				this.networkTime = Network.time;
			}
			else
			{
				this.networkTime += (double)Time.deltaTime;
			}
			this.timerToEndMatch = this.timeEndMatch - this.networkTime;
		}
		if (this.timerToEndMatch < 0.0 && !Defs.isFlag)
		{
			if (ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.CapturePoints)
			{
				if (CapturePointController.sharedController != null && !this.isEndMatch)
				{
					CapturePointController.sharedController.EndMatch();
					this.isEndMatch = true;
				}
			}
			else if (ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.TimeBattle)
			{
				if (!this.isEndMatch)
				{
					ZombiManager.sharedManager.EndMatch();
				}
			}
			else if (WeaponManager.sharedManager.myPlayer != null)
			{
				WeaponManager.sharedManager.myPlayerMoveC.WinFromTimer();
			}
			else
			{
				GlobalGameController.countKillsRed = 0;
				GlobalGameController.countKillsBlue = 0;
			}
		}
		else
		{
			this.isEndMatch = false;
		}
		if (this.wasPaused)
		{
			this.wasPaused = false;
			base.StartCoroutine(this.OnUnpause());
		}
		this.pauseTime = Tools.CurrentUnixTime;
	}

	// Token: 0x06004D72 RID: 19826 RVA: 0x001C0190 File Offset: 0x001BE390
	private IEnumerator OnUnpause()
	{
		yield return null;
		yield return null;
		PhotonNetwork.isMessageQueueRunning = true;
		yield break;
	}

	// Token: 0x06004D73 RID: 19827 RVA: 0x001C01A4 File Offset: 0x001BE3A4
	private void OnPlayerConnected(NetworkPlayer player)
	{
		if (Network.isServer)
		{
			base.GetComponent<NetworkView>().RPC("SynchTimeEnd", RPCMode.Others, new object[]
			{
				(float)this.timeEndMatch
			});
			base.GetComponent<NetworkView>().RPC("SynchTimeServer", RPCMode.Others, new object[]
			{
				(float)Network.time
			});
		}
	}

	// Token: 0x06004D74 RID: 19828 RVA: 0x001C0208 File Offset: 0x001BE408
	[RPC]
	[PunRPC]
	private void SynchTimeEnd(float synchTime)
	{
		this.timeEndMatch = (double)synchTime;
	}

	// Token: 0x06004D75 RID: 19829 RVA: 0x001C0214 File Offset: 0x001BE414
	[PunRPC]
	[RPC]
	private void SynchTimeServer(float synchTime)
	{
		if (this.networkTime < (double)synchTime)
		{
			this.networkTime = (double)synchTime;
		}
	}

	// Token: 0x06004D76 RID: 19830 RVA: 0x001C022C File Offset: 0x001BE42C
	private void OnDestroy()
	{
		TimeGameController.sharedController = null;
	}

	// Token: 0x04003BDF RID: 15327
	public static TimeGameController sharedController;

	// Token: 0x04003BE0 RID: 15328
	public double timeEndMatch;

	// Token: 0x04003BE1 RID: 15329
	public double timerToEndMatch;

	// Token: 0x04003BE2 RID: 15330
	public double networkTime;

	// Token: 0x04003BE3 RID: 15331
	public PhotonView photonView;

	// Token: 0x04003BE4 RID: 15332
	public double timeLocalServer;

	// Token: 0x04003BE5 RID: 15333
	public string ipServera;

	// Token: 0x04003BE6 RID: 15334
	private long pauseTime;

	// Token: 0x04003BE7 RID: 15335
	private bool paused;

	// Token: 0x04003BE8 RID: 15336
	private bool wasPaused;

	// Token: 0x04003BE9 RID: 15337
	public bool isEndMatch = true;

	// Token: 0x04003BEA RID: 15338
	private bool matchEnding;

	// Token: 0x04003BEB RID: 15339
	private int matchEndingPos;

	// Token: 0x04003BEC RID: 15340
	private int writtedMatchEndingPos;
}
