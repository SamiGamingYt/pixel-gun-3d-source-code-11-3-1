using System;
using System.Collections;
using System.Diagnostics;
using ExitGames.Client.Photon;
using UnityEngine;

// Token: 0x02000414 RID: 1044
internal class PhotonHandler : MonoBehaviour
{
	// Token: 0x06002524 RID: 9508 RVA: 0x000BA9CC File Offset: 0x000B8BCC
	protected void Awake()
	{
		if (PhotonHandler.SP != null && PhotonHandler.SP != this && PhotonHandler.SP.gameObject != null)
		{
			UnityEngine.Object.DestroyImmediate(PhotonHandler.SP.gameObject);
		}
		PhotonHandler.SP = this;
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		this.updateInterval = 1000 / PhotonNetwork.sendRate;
		this.updateIntervalOnSerialize = 1000 / PhotonNetwork.sendRateOnSerialize;
		PhotonHandler.StartFallbackSendAckThread();
	}

	// Token: 0x06002525 RID: 9509 RVA: 0x000BAA58 File Offset: 0x000B8C58
	protected void OnLevelWasLoaded(int level)
	{
		PhotonNetwork.networkingPeer.NewSceneLoaded();
		PhotonNetwork.networkingPeer.SetLevelInPropsIfSynced(SceneManagerHelper.ActiveSceneName);
	}

	// Token: 0x06002526 RID: 9510 RVA: 0x000BAA74 File Offset: 0x000B8C74
	protected void OnApplicationQuit()
	{
		PhotonHandler.AppQuits = true;
		PhotonHandler.StopFallbackSendAckThread();
		PhotonNetwork.Disconnect();
	}

	// Token: 0x06002527 RID: 9511 RVA: 0x000BAA88 File Offset: 0x000B8C88
	protected void OnApplicationPause(bool pause)
	{
		if (PhotonNetwork.BackgroundTimeout > 0.1f)
		{
			if (PhotonHandler.timerToStopConnectionInBackground == null)
			{
				PhotonHandler.timerToStopConnectionInBackground = new Stopwatch();
			}
			PhotonHandler.timerToStopConnectionInBackground.Reset();
			if (pause)
			{
				PhotonHandler.timerToStopConnectionInBackground.Start();
			}
			else
			{
				PhotonHandler.timerToStopConnectionInBackground.Stop();
			}
		}
	}

	// Token: 0x06002528 RID: 9512 RVA: 0x000BAAE4 File Offset: 0x000B8CE4
	protected void OnDestroy()
	{
		PhotonHandler.StopFallbackSendAckThread();
	}

	// Token: 0x06002529 RID: 9513 RVA: 0x000BAAEC File Offset: 0x000B8CEC
	protected void Update()
	{
		if (PhotonNetwork.networkingPeer == null)
		{
			UnityEngine.Debug.LogError("NetworkPeer broke!");
			return;
		}
		if (PhotonNetwork.connectionStateDetailed == ClientState.PeerCreated || PhotonNetwork.connectionStateDetailed == ClientState.Disconnected || PhotonNetwork.offlineMode)
		{
			return;
		}
		if (!PhotonNetwork.isMessageQueueRunning)
		{
			return;
		}
		Defs.inComingMessagesCounter = 0;
		bool flag = true;
		while (PhotonNetwork.isMessageQueueRunning && flag)
		{
			flag = PhotonNetwork.networkingPeer.DispatchIncomingCommands();
			Defs.inComingMessagesCounter++;
		}
		Defs.inComingMessagesCounter = 0;
		int num = (int)(Time.realtimeSinceStartup * 1000f);
		if (PhotonNetwork.isMessageQueueRunning && num > this.nextSendTickCountOnSerialize)
		{
			PhotonNetwork.networkingPeer.RunViewUpdate();
			this.nextSendTickCountOnSerialize = num + this.updateIntervalOnSerialize;
			this.nextSendTickCount = 0;
		}
		num = (int)(Time.realtimeSinceStartup * 1000f);
		if (num > this.nextSendTickCount)
		{
			bool flag2 = true;
			while (PhotonNetwork.isMessageQueueRunning && flag2)
			{
				flag2 = PhotonNetwork.networkingPeer.SendOutgoingCommands();
			}
			this.nextSendTickCount = num + this.updateInterval;
		}
	}

	// Token: 0x0600252A RID: 9514 RVA: 0x000BAC00 File Offset: 0x000B8E00
	protected void OnJoinedRoom()
	{
		PhotonNetwork.networkingPeer.LoadLevelIfSynced();
	}

	// Token: 0x0600252B RID: 9515 RVA: 0x000BAC0C File Offset: 0x000B8E0C
	protected void OnCreatedRoom()
	{
		PhotonNetwork.networkingPeer.SetLevelInPropsIfSynced(SceneManagerHelper.ActiveSceneName);
	}

	// Token: 0x0600252C RID: 9516 RVA: 0x000BAC20 File Offset: 0x000B8E20
	public static void StartFallbackSendAckThread()
	{
		if (PhotonHandler.sendThreadShouldRun)
		{
			return;
		}
		PhotonHandler.sendThreadShouldRun = true;
		SupportClass.CallInBackground(new Func<bool>(PhotonHandler.FallbackSendAckThread));
	}

	// Token: 0x0600252D RID: 9517 RVA: 0x000BAC50 File Offset: 0x000B8E50
	public static void StopFallbackSendAckThread()
	{
		PhotonHandler.sendThreadShouldRun = false;
	}

	// Token: 0x0600252E RID: 9518 RVA: 0x000BAC58 File Offset: 0x000B8E58
	public static bool FallbackSendAckThread()
	{
		if (PhotonHandler.sendThreadShouldRun && PhotonNetwork.networkingPeer != null)
		{
			if (PhotonHandler.timerToStopConnectionInBackground != null && PhotonNetwork.BackgroundTimeout > 0.1f && (float)PhotonHandler.timerToStopConnectionInBackground.ElapsedMilliseconds > PhotonNetwork.BackgroundTimeout * 1000f)
			{
				if (PhotonNetwork.connected)
				{
					PhotonNetwork.Disconnect();
				}
				PhotonHandler.timerToStopConnectionInBackground.Stop();
				PhotonHandler.timerToStopConnectionInBackground.Reset();
				return PhotonHandler.sendThreadShouldRun;
			}
			if (PhotonNetwork.networkingPeer.ConnectionTime - PhotonNetwork.networkingPeer.LastSendOutgoingTime > 200)
			{
				PhotonNetwork.networkingPeer.SendAcksOnly();
			}
		}
		return PhotonHandler.sendThreadShouldRun;
	}

	// Token: 0x17000685 RID: 1669
	// (get) Token: 0x0600252F RID: 9519 RVA: 0x000BAD08 File Offset: 0x000B8F08
	// (set) Token: 0x06002530 RID: 9520 RVA: 0x000BAD3C File Offset: 0x000B8F3C
	internal static CloudRegionCode BestRegionCodeInPreferences
	{
		get
		{
			string @string = PlayerPrefs.GetString("PUNCloudBestRegion", string.Empty);
			if (!string.IsNullOrEmpty(@string))
			{
				return Region.Parse(@string);
			}
			return CloudRegionCode.none;
		}
		set
		{
			if (value == CloudRegionCode.none)
			{
				PlayerPrefs.DeleteKey("PUNCloudBestRegion");
			}
			else
			{
				PlayerPrefs.SetString("PUNCloudBestRegion", value.ToString());
			}
		}
	}

	// Token: 0x06002531 RID: 9521 RVA: 0x000BAD6C File Offset: 0x000B8F6C
	protected internal static void PingAvailableRegionsAndConnectToBest()
	{
		PhotonHandler.SP.StartCoroutine(PhotonHandler.SP.PingAvailableRegionsCoroutine(true));
	}

	// Token: 0x06002532 RID: 9522 RVA: 0x000BAD84 File Offset: 0x000B8F84
	internal IEnumerator PingAvailableRegionsCoroutine(bool connectToBest)
	{
		PhotonHandler.BestRegionCodeCurrently = CloudRegionCode.none;
		while (PhotonNetwork.networkingPeer.AvailableRegions == null)
		{
			if (PhotonNetwork.connectionStateDetailed != ClientState.ConnectingToNameServer && PhotonNetwork.connectionStateDetailed != ClientState.ConnectedToNameServer)
			{
				UnityEngine.Debug.LogError("Call ConnectToNameServer to ping available regions.");
				yield break;
			}
			UnityEngine.Debug.Log(string.Concat(new object[]
			{
				"Waiting for AvailableRegions. State: ",
				PhotonNetwork.connectionStateDetailed,
				" Server: ",
				PhotonNetwork.Server,
				" PhotonNetwork.networkingPeer.AvailableRegions ",
				PhotonNetwork.networkingPeer.AvailableRegions != null
			}));
			yield return new WaitForSeconds(0.25f);
		}
		if (PhotonNetwork.networkingPeer.AvailableRegions == null || PhotonNetwork.networkingPeer.AvailableRegions.Count == 0)
		{
			UnityEngine.Debug.LogError("No regions available. Are you sure your appid is valid and setup?");
			yield break;
		}
		PhotonPingManager pingManager = new PhotonPingManager();
		foreach (Region region in PhotonNetwork.networkingPeer.AvailableRegions)
		{
			PhotonHandler.SP.StartCoroutine(pingManager.PingSocket(region));
		}
		while (!pingManager.Done)
		{
			yield return new WaitForSeconds(0.1f);
		}
		Region best = pingManager.BestRegion;
		PhotonHandler.BestRegionCodeCurrently = best.Code;
		PhotonHandler.BestRegionCodeInPreferences = best.Code;
		UnityEngine.Debug.Log(string.Concat(new object[]
		{
			"Found best region: ",
			best.Code,
			" ping: ",
			best.Ping,
			". Calling ConnectToRegionMaster() is: ",
			connectToBest
		}));
		if (connectToBest)
		{
			PhotonNetwork.networkingPeer.ConnectToRegionMaster(best.Code);
		}
		yield break;
	}

	// Token: 0x04001A0B RID: 6667
	private const string PlayerPrefsKey = "PUNCloudBestRegion";

	// Token: 0x04001A0C RID: 6668
	public static PhotonHandler SP;

	// Token: 0x04001A0D RID: 6669
	public int updateInterval;

	// Token: 0x04001A0E RID: 6670
	public int updateIntervalOnSerialize;

	// Token: 0x04001A0F RID: 6671
	private int nextSendTickCount;

	// Token: 0x04001A10 RID: 6672
	private int nextSendTickCountOnSerialize;

	// Token: 0x04001A11 RID: 6673
	private static bool sendThreadShouldRun;

	// Token: 0x04001A12 RID: 6674
	private static Stopwatch timerToStopConnectionInBackground;

	// Token: 0x04001A13 RID: 6675
	protected internal static bool AppQuits;

	// Token: 0x04001A14 RID: 6676
	protected internal static Type PingImplementation;

	// Token: 0x04001A15 RID: 6677
	internal static CloudRegionCode BestRegionCodeCurrently = CloudRegionCode.none;
}
