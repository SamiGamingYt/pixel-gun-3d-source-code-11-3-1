using System;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using UnityEngine;

// Token: 0x02000424 RID: 1060
[Serializable]
public class ServerSettings : ScriptableObject
{
	// Token: 0x0600263F RID: 9791 RVA: 0x000BF40C File Offset: 0x000BD60C
	public void UseCloudBestRegion(string cloudAppid)
	{
		this.HostType = ServerSettings.HostingOption.BestRegion;
		this.AppID = cloudAppid;
	}

	// Token: 0x06002640 RID: 9792 RVA: 0x000BF41C File Offset: 0x000BD61C
	public void UseCloud(string cloudAppid)
	{
		this.HostType = ServerSettings.HostingOption.PhotonCloud;
		this.AppID = cloudAppid;
	}

	// Token: 0x06002641 RID: 9793 RVA: 0x000BF42C File Offset: 0x000BD62C
	public void UseCloud(string cloudAppid, CloudRegionCode code)
	{
		this.HostType = ServerSettings.HostingOption.PhotonCloud;
		this.AppID = cloudAppid;
		this.PreferredRegion = code;
	}

	// Token: 0x06002642 RID: 9794 RVA: 0x000BF444 File Offset: 0x000BD644
	public void UseMyServer(string serverAddress, int serverPort, string application)
	{
		this.HostType = ServerSettings.HostingOption.SelfHosted;
		this.AppID = ((application == null) ? "master" : application);
		this.ServerAddress = serverAddress;
		this.ServerPort = serverPort;
	}

	// Token: 0x06002643 RID: 9795 RVA: 0x000BF480 File Offset: 0x000BD680
	public override string ToString()
	{
		return string.Concat(new object[]
		{
			"ServerSettings: ",
			this.HostType,
			" ",
			this.ServerAddress
		});
	}

	// Token: 0x04001A93 RID: 6803
	public ServerSettings.HostingOption HostType;

	// Token: 0x04001A94 RID: 6804
	public ConnectionProtocol Protocol;

	// Token: 0x04001A95 RID: 6805
	public string ServerAddress = string.Empty;

	// Token: 0x04001A96 RID: 6806
	public int ServerPort = 5055;

	// Token: 0x04001A97 RID: 6807
	public string AppID = string.Empty;

	// Token: 0x04001A98 RID: 6808
	public string VoiceAppID = string.Empty;

	// Token: 0x04001A99 RID: 6809
	public CloudRegionCode PreferredRegion;

	// Token: 0x04001A9A RID: 6810
	public CloudRegionFlag EnabledRegions = (CloudRegionFlag)(-1);

	// Token: 0x04001A9B RID: 6811
	public bool JoinLobby;

	// Token: 0x04001A9C RID: 6812
	public bool EnableLobbyStatistics;

	// Token: 0x04001A9D RID: 6813
	public List<string> RpcList = new List<string>();

	// Token: 0x04001A9E RID: 6814
	[HideInInspector]
	public bool DisableAutoOpenWizard;

	// Token: 0x02000425 RID: 1061
	public enum HostingOption
	{
		// Token: 0x04001AA0 RID: 6816
		NotSet,
		// Token: 0x04001AA1 RID: 6817
		PhotonCloud,
		// Token: 0x04001AA2 RID: 6818
		SelfHosted,
		// Token: 0x04001AA3 RID: 6819
		OfflineMode,
		// Token: 0x04001AA4 RID: 6820
		BestRegion
	}
}
