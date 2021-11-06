using System;
using System.Collections.Generic;
using ExitGames.Client.Photon;

namespace Photon
{
	// Token: 0x0200040E RID: 1038
	public class PunBehaviour : MonoBehaviour, IPunCallbacks
	{
		// Token: 0x060024E2 RID: 9442 RVA: 0x000BA1F0 File Offset: 0x000B83F0
		public virtual void OnConnectedToPhoton()
		{
		}

		// Token: 0x060024E3 RID: 9443 RVA: 0x000BA1F4 File Offset: 0x000B83F4
		public virtual void OnLeftRoom()
		{
		}

		// Token: 0x060024E4 RID: 9444 RVA: 0x000BA1F8 File Offset: 0x000B83F8
		public virtual void OnMasterClientSwitched(PhotonPlayer newMasterClient)
		{
		}

		// Token: 0x060024E5 RID: 9445 RVA: 0x000BA1FC File Offset: 0x000B83FC
		public virtual void OnPhotonCreateRoomFailed(object[] codeAndMsg)
		{
		}

		// Token: 0x060024E6 RID: 9446 RVA: 0x000BA200 File Offset: 0x000B8400
		public virtual void OnPhotonJoinRoomFailed(object[] codeAndMsg)
		{
		}

		// Token: 0x060024E7 RID: 9447 RVA: 0x000BA204 File Offset: 0x000B8404
		public virtual void OnCreatedRoom()
		{
		}

		// Token: 0x060024E8 RID: 9448 RVA: 0x000BA208 File Offset: 0x000B8408
		public virtual void OnJoinedLobby()
		{
		}

		// Token: 0x060024E9 RID: 9449 RVA: 0x000BA20C File Offset: 0x000B840C
		public virtual void OnLeftLobby()
		{
		}

		// Token: 0x060024EA RID: 9450 RVA: 0x000BA210 File Offset: 0x000B8410
		public virtual void OnFailedToConnectToPhoton(DisconnectCause cause)
		{
		}

		// Token: 0x060024EB RID: 9451 RVA: 0x000BA214 File Offset: 0x000B8414
		public virtual void OnDisconnectedFromPhoton()
		{
		}

		// Token: 0x060024EC RID: 9452 RVA: 0x000BA218 File Offset: 0x000B8418
		public virtual void OnConnectionFail(DisconnectCause cause)
		{
		}

		// Token: 0x060024ED RID: 9453 RVA: 0x000BA21C File Offset: 0x000B841C
		public virtual void OnPhotonInstantiate(PhotonMessageInfo info)
		{
		}

		// Token: 0x060024EE RID: 9454 RVA: 0x000BA220 File Offset: 0x000B8420
		public virtual void OnReceivedRoomListUpdate()
		{
		}

		// Token: 0x060024EF RID: 9455 RVA: 0x000BA224 File Offset: 0x000B8424
		public virtual void OnJoinedRoom()
		{
		}

		// Token: 0x060024F0 RID: 9456 RVA: 0x000BA228 File Offset: 0x000B8428
		public virtual void OnPhotonPlayerConnected(PhotonPlayer newPlayer)
		{
		}

		// Token: 0x060024F1 RID: 9457 RVA: 0x000BA22C File Offset: 0x000B842C
		public virtual void OnPhotonPlayerDisconnected(PhotonPlayer otherPlayer)
		{
		}

		// Token: 0x060024F2 RID: 9458 RVA: 0x000BA230 File Offset: 0x000B8430
		public virtual void OnPhotonRandomJoinFailed(object[] codeAndMsg)
		{
		}

		// Token: 0x060024F3 RID: 9459 RVA: 0x000BA234 File Offset: 0x000B8434
		public virtual void OnConnectedToMaster()
		{
		}

		// Token: 0x060024F4 RID: 9460 RVA: 0x000BA238 File Offset: 0x000B8438
		public virtual void OnPhotonMaxCccuReached()
		{
		}

		// Token: 0x060024F5 RID: 9461 RVA: 0x000BA23C File Offset: 0x000B843C
		public virtual void OnPhotonCustomRoomPropertiesChanged(Hashtable propertiesThatChanged)
		{
		}

		// Token: 0x060024F6 RID: 9462 RVA: 0x000BA240 File Offset: 0x000B8440
		public virtual void OnPhotonPlayerPropertiesChanged(object[] playerAndUpdatedProps)
		{
		}

		// Token: 0x060024F7 RID: 9463 RVA: 0x000BA244 File Offset: 0x000B8444
		public virtual void OnUpdatedFriendList()
		{
		}

		// Token: 0x060024F8 RID: 9464 RVA: 0x000BA248 File Offset: 0x000B8448
		public virtual void OnCustomAuthenticationFailed(string debugMessage)
		{
		}

		// Token: 0x060024F9 RID: 9465 RVA: 0x000BA24C File Offset: 0x000B844C
		public virtual void OnCustomAuthenticationResponse(Dictionary<string, object> data)
		{
		}

		// Token: 0x060024FA RID: 9466 RVA: 0x000BA250 File Offset: 0x000B8450
		public virtual void OnWebRpcResponse(OperationResponse response)
		{
		}

		// Token: 0x060024FB RID: 9467 RVA: 0x000BA254 File Offset: 0x000B8454
		public virtual void OnOwnershipRequest(object[] viewAndPlayer)
		{
		}

		// Token: 0x060024FC RID: 9468 RVA: 0x000BA258 File Offset: 0x000B8458
		public virtual void OnLobbyStatisticsUpdate()
		{
		}
	}
}
