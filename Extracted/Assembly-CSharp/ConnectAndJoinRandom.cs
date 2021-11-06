using System;
using Photon;
using UnityEngine;

// Token: 0x02000438 RID: 1080
public class ConnectAndJoinRandom : Photon.MonoBehaviour
{
	// Token: 0x0600267E RID: 9854 RVA: 0x000C0CAC File Offset: 0x000BEEAC
	public virtual void Start()
	{
		PhotonNetwork.autoJoinLobby = false;
	}

	// Token: 0x0600267F RID: 9855 RVA: 0x000C0CB4 File Offset: 0x000BEEB4
	public virtual void Update()
	{
		if (this.ConnectInUpdate && this.AutoConnect && !PhotonNetwork.connected)
		{
			Debug.Log("Update() was called by Unity. Scene is loaded. Let's connect to the Photon Master Server. Calling: PhotonNetwork.ConnectUsingSettings();");
			this.ConnectInUpdate = false;
			PhotonNetwork.ConnectUsingSettings(this.Version + "." + SceneManagerHelper.ActiveSceneBuildIndex);
		}
	}

	// Token: 0x06002680 RID: 9856 RVA: 0x000C0D18 File Offset: 0x000BEF18
	public virtual void OnConnectedToMaster()
	{
		Debug.Log("OnConnectedToMaster() was called by PUN. Now this client is connected and could join a room. Calling: PhotonNetwork.JoinRandomRoom();");
		PhotonNetwork.JoinRandomRoom();
	}

	// Token: 0x06002681 RID: 9857 RVA: 0x000C0D2C File Offset: 0x000BEF2C
	public virtual void OnJoinedLobby()
	{
		Debug.Log("OnJoinedLobby(). This client is connected and does get a room-list, which gets stored as PhotonNetwork.GetRoomList(). This script now calls: PhotonNetwork.JoinRandomRoom();");
		PhotonNetwork.JoinRandomRoom();
	}

	// Token: 0x06002682 RID: 9858 RVA: 0x000C0D40 File Offset: 0x000BEF40
	public virtual void OnPhotonRandomJoinFailed()
	{
		Debug.Log("OnPhotonRandomJoinFailed() was called by PUN. No random room available, so we create one. Calling: PhotonNetwork.CreateRoom(null, new RoomOptions() {maxPlayers = 4}, null);");
		PhotonNetwork.CreateRoom(null, new RoomOptions
		{
			MaxPlayers = 4
		}, null);
	}

	// Token: 0x06002683 RID: 9859 RVA: 0x000C0D70 File Offset: 0x000BEF70
	public virtual void OnFailedToConnectToPhoton(DisconnectCause cause)
	{
		Debug.LogError("Cause: " + cause);
	}

	// Token: 0x06002684 RID: 9860 RVA: 0x000C0D88 File Offset: 0x000BEF88
	public void OnJoinedRoom()
	{
		Debug.Log("OnJoinedRoom() called by PUN. Now this client is in a room. From here on, your game would be running. For reference, all callbacks are listed in enum: PhotonNetworkingMessage");
	}

	// Token: 0x04001B02 RID: 6914
	public bool AutoConnect = true;

	// Token: 0x04001B03 RID: 6915
	public byte Version = 1;

	// Token: 0x04001B04 RID: 6916
	private bool ConnectInUpdate = true;
}
