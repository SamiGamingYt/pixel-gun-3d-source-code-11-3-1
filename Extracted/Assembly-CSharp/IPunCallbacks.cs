using System;
using System.Collections.Generic;
using ExitGames.Client.Photon;

// Token: 0x0200040B RID: 1035
public interface IPunCallbacks
{
	// Token: 0x060024C2 RID: 9410
	void OnConnectedToPhoton();

	// Token: 0x060024C3 RID: 9411
	void OnLeftRoom();

	// Token: 0x060024C4 RID: 9412
	void OnMasterClientSwitched(PhotonPlayer newMasterClient);

	// Token: 0x060024C5 RID: 9413
	void OnPhotonCreateRoomFailed(object[] codeAndMsg);

	// Token: 0x060024C6 RID: 9414
	void OnPhotonJoinRoomFailed(object[] codeAndMsg);

	// Token: 0x060024C7 RID: 9415
	void OnCreatedRoom();

	// Token: 0x060024C8 RID: 9416
	void OnJoinedLobby();

	// Token: 0x060024C9 RID: 9417
	void OnLeftLobby();

	// Token: 0x060024CA RID: 9418
	void OnFailedToConnectToPhoton(DisconnectCause cause);

	// Token: 0x060024CB RID: 9419
	void OnConnectionFail(DisconnectCause cause);

	// Token: 0x060024CC RID: 9420
	void OnDisconnectedFromPhoton();

	// Token: 0x060024CD RID: 9421
	void OnPhotonInstantiate(PhotonMessageInfo info);

	// Token: 0x060024CE RID: 9422
	void OnReceivedRoomListUpdate();

	// Token: 0x060024CF RID: 9423
	void OnJoinedRoom();

	// Token: 0x060024D0 RID: 9424
	void OnPhotonPlayerConnected(PhotonPlayer newPlayer);

	// Token: 0x060024D1 RID: 9425
	void OnPhotonPlayerDisconnected(PhotonPlayer otherPlayer);

	// Token: 0x060024D2 RID: 9426
	void OnPhotonRandomJoinFailed(object[] codeAndMsg);

	// Token: 0x060024D3 RID: 9427
	void OnConnectedToMaster();

	// Token: 0x060024D4 RID: 9428
	void OnPhotonMaxCccuReached();

	// Token: 0x060024D5 RID: 9429
	void OnPhotonCustomRoomPropertiesChanged(Hashtable propertiesThatChanged);

	// Token: 0x060024D6 RID: 9430
	void OnPhotonPlayerPropertiesChanged(object[] playerAndUpdatedProps);

	// Token: 0x060024D7 RID: 9431
	void OnUpdatedFriendList();

	// Token: 0x060024D8 RID: 9432
	void OnCustomAuthenticationFailed(string debugMessage);

	// Token: 0x060024D9 RID: 9433
	void OnCustomAuthenticationResponse(Dictionary<string, object> data);

	// Token: 0x060024DA RID: 9434
	void OnWebRpcResponse(OperationResponse response);

	// Token: 0x060024DB RID: 9435
	void OnOwnershipRequest(object[] viewAndPlayer);

	// Token: 0x060024DC RID: 9436
	void OnLobbyStatisticsUpdate();
}
