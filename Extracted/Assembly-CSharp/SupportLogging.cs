using System;
using System.Text;
using UnityEngine;

// Token: 0x02000460 RID: 1120
public class SupportLogging : MonoBehaviour
{
	// Token: 0x06002742 RID: 10050 RVA: 0x000C48A4 File Offset: 0x000C2AA4
	public void Start()
	{
		if (this.LogTrafficStats)
		{
			base.InvokeRepeating("LogStats", 10f, 10f);
		}
	}

	// Token: 0x06002743 RID: 10051 RVA: 0x000C48D4 File Offset: 0x000C2AD4
	protected void OnApplicationPause(bool pause)
	{
		Debug.Log(string.Concat(new object[]
		{
			"SupportLogger OnApplicationPause: ",
			pause,
			" connected: ",
			PhotonNetwork.connected
		}));
	}

	// Token: 0x06002744 RID: 10052 RVA: 0x000C4918 File Offset: 0x000C2B18
	public void OnApplicationQuit()
	{
		base.CancelInvoke();
	}

	// Token: 0x06002745 RID: 10053 RVA: 0x000C4920 File Offset: 0x000C2B20
	public void LogStats()
	{
		if (this.LogTrafficStats)
		{
			Debug.Log("SupportLogger " + PhotonNetwork.NetworkStatisticsToString());
		}
	}

	// Token: 0x06002746 RID: 10054 RVA: 0x000C4944 File Offset: 0x000C2B44
	private void LogBasics()
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.AppendFormat("SupportLogger Info: PUN {0}: ", "1.79");
		stringBuilder.AppendFormat("AppID: {0}*** GameVersion: {1} ", PhotonNetwork.networkingPeer.AppId.Substring(0, 8), PhotonNetwork.networkingPeer.AppVersion);
		stringBuilder.AppendFormat("Server: {0}. Region: {1} ", PhotonNetwork.ServerAddress, PhotonNetwork.networkingPeer.CloudRegion);
		stringBuilder.AppendFormat("HostType: {0} ", PhotonNetwork.PhotonServerSettings.HostType);
		Debug.Log(stringBuilder.ToString());
	}

	// Token: 0x06002747 RID: 10055 RVA: 0x000C49D8 File Offset: 0x000C2BD8
	public void OnConnectedToPhoton()
	{
		Debug.Log("SupportLogger OnConnectedToPhoton().");
		this.LogBasics();
		if (this.LogTrafficStats)
		{
			PhotonNetwork.NetworkStatisticsEnabled = true;
		}
	}

	// Token: 0x06002748 RID: 10056 RVA: 0x000C49FC File Offset: 0x000C2BFC
	public void OnFailedToConnectToPhoton(DisconnectCause cause)
	{
		Debug.Log("SupportLogger OnFailedToConnectToPhoton(" + cause + ").");
		this.LogBasics();
	}

	// Token: 0x06002749 RID: 10057 RVA: 0x000C4A2C File Offset: 0x000C2C2C
	public void OnJoinedLobby()
	{
		Debug.Log("SupportLogger OnJoinedLobby(" + PhotonNetwork.lobby + ").");
	}

	// Token: 0x0600274A RID: 10058 RVA: 0x000C4A48 File Offset: 0x000C2C48
	public void OnJoinedRoom()
	{
		Debug.Log(string.Concat(new object[]
		{
			"SupportLogger OnJoinedRoom(",
			PhotonNetwork.room,
			"). ",
			PhotonNetwork.lobby,
			" GameServer:",
			PhotonNetwork.ServerAddress
		}));
	}

	// Token: 0x0600274B RID: 10059 RVA: 0x000C4A98 File Offset: 0x000C2C98
	public void OnCreatedRoom()
	{
		Debug.Log(string.Concat(new object[]
		{
			"SupportLogger OnCreatedRoom(",
			PhotonNetwork.room,
			"). ",
			PhotonNetwork.lobby,
			" GameServer:",
			PhotonNetwork.ServerAddress
		}));
	}

	// Token: 0x0600274C RID: 10060 RVA: 0x000C4AE8 File Offset: 0x000C2CE8
	public void OnLeftRoom()
	{
		Debug.Log("SupportLogger OnLeftRoom().");
	}

	// Token: 0x0600274D RID: 10061 RVA: 0x000C4AF4 File Offset: 0x000C2CF4
	public void OnDisconnectedFromPhoton()
	{
		Debug.Log("SupportLogger OnDisconnectedFromPhoton().");
	}

	// Token: 0x04001B87 RID: 7047
	public bool LogTrafficStats;
}
