using System;
using System.Collections;
using ExitGames.Client.Photon;
using UnityEngine;

// Token: 0x02000440 RID: 1088
public class InRoomTime : MonoBehaviour
{
	// Token: 0x170006DC RID: 1756
	// (get) Token: 0x060026B0 RID: 9904 RVA: 0x000C1EB0 File Offset: 0x000C00B0
	public double RoomTime
	{
		get
		{
			uint roomTimestamp = (uint)this.RoomTimestamp;
			double num = roomTimestamp;
			return num / 1000.0;
		}
	}

	// Token: 0x170006DD RID: 1757
	// (get) Token: 0x060026B1 RID: 9905 RVA: 0x000C1ED4 File Offset: 0x000C00D4
	public int RoomTimestamp
	{
		get
		{
			return (!PhotonNetwork.inRoom) ? 0 : (PhotonNetwork.ServerTimestamp - this.roomStartTimestamp);
		}
	}

	// Token: 0x170006DE RID: 1758
	// (get) Token: 0x060026B2 RID: 9906 RVA: 0x000C1EF4 File Offset: 0x000C00F4
	public bool IsRoomTimeSet
	{
		get
		{
			return PhotonNetwork.inRoom && PhotonNetwork.room.customProperties.ContainsKey("#rt");
		}
	}

	// Token: 0x060026B3 RID: 9907 RVA: 0x000C1F18 File Offset: 0x000C0118
	internal IEnumerator SetRoomStartTimestamp()
	{
		if (this.IsRoomTimeSet || !PhotonNetwork.isMasterClient)
		{
			yield break;
		}
		if (PhotonNetwork.ServerTimestamp == 0)
		{
			yield return 0;
		}
		ExitGames.Client.Photon.Hashtable startTimeProp = new ExitGames.Client.Photon.Hashtable();
		startTimeProp["#rt"] = PhotonNetwork.ServerTimestamp;
		PhotonNetwork.room.SetCustomProperties(startTimeProp, null, false);
		yield break;
	}

	// Token: 0x060026B4 RID: 9908 RVA: 0x000C1F34 File Offset: 0x000C0134
	public void OnJoinedRoom()
	{
		base.StartCoroutine("SetRoomStartTimestamp");
	}

	// Token: 0x060026B5 RID: 9909 RVA: 0x000C1F44 File Offset: 0x000C0144
	public void OnMasterClientSwitched(PhotonPlayer newMasterClient)
	{
		base.StartCoroutine("SetRoomStartTimestamp");
	}

	// Token: 0x060026B6 RID: 9910 RVA: 0x000C1F54 File Offset: 0x000C0154
	public void OnPhotonCustomRoomPropertiesChanged(ExitGames.Client.Photon.Hashtable propertiesThatChanged)
	{
		if (propertiesThatChanged.ContainsKey("#rt"))
		{
			this.roomStartTimestamp = (int)propertiesThatChanged["#rt"];
		}
	}

	// Token: 0x04001B32 RID: 6962
	private const string StartTimeKey = "#rt";

	// Token: 0x04001B33 RID: 6963
	private int roomStartTimestamp;
}
