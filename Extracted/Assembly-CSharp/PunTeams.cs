using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000454 RID: 1108
public class PunTeams : MonoBehaviour
{
	// Token: 0x0600270D RID: 9997 RVA: 0x000C3C40 File Offset: 0x000C1E40
	public void Start()
	{
		PunTeams.PlayersPerTeam = new Dictionary<PunTeams.Team, List<PhotonPlayer>>();
		Array values = Enum.GetValues(typeof(PunTeams.Team));
		foreach (object obj in values)
		{
			PunTeams.PlayersPerTeam[(PunTeams.Team)((byte)obj)] = new List<PhotonPlayer>();
		}
	}

	// Token: 0x0600270E RID: 9998 RVA: 0x000C3CD0 File Offset: 0x000C1ED0
	public void OnDisable()
	{
		PunTeams.PlayersPerTeam = new Dictionary<PunTeams.Team, List<PhotonPlayer>>();
	}

	// Token: 0x0600270F RID: 9999 RVA: 0x000C3CDC File Offset: 0x000C1EDC
	public void OnJoinedRoom()
	{
		this.UpdateTeams();
	}

	// Token: 0x06002710 RID: 10000 RVA: 0x000C3CE4 File Offset: 0x000C1EE4
	public void OnLeftRoom()
	{
		this.Start();
	}

	// Token: 0x06002711 RID: 10001 RVA: 0x000C3CEC File Offset: 0x000C1EEC
	public void OnPhotonPlayerPropertiesChanged(object[] playerAndUpdatedProps)
	{
		this.UpdateTeams();
	}

	// Token: 0x06002712 RID: 10002 RVA: 0x000C3CF4 File Offset: 0x000C1EF4
	public void OnPhotonPlayerDisconnected(PhotonPlayer otherPlayer)
	{
		this.UpdateTeams();
	}

	// Token: 0x06002713 RID: 10003 RVA: 0x000C3CFC File Offset: 0x000C1EFC
	public void OnPhotonPlayerConnected(PhotonPlayer newPlayer)
	{
		this.UpdateTeams();
	}

	// Token: 0x06002714 RID: 10004 RVA: 0x000C3D04 File Offset: 0x000C1F04
	public void UpdateTeams()
	{
		Array values = Enum.GetValues(typeof(PunTeams.Team));
		foreach (object obj in values)
		{
			PunTeams.PlayersPerTeam[(PunTeams.Team)((byte)obj)].Clear();
		}
		for (int i = 0; i < PhotonNetwork.playerList.Length; i++)
		{
			PhotonPlayer photonPlayer = PhotonNetwork.playerList[i];
			PunTeams.Team team = photonPlayer.GetTeam();
			PunTeams.PlayersPerTeam[team].Add(photonPlayer);
		}
	}

	// Token: 0x04001B6D RID: 7021
	public const string TeamPlayerProp = "team";

	// Token: 0x04001B6E RID: 7022
	public static Dictionary<PunTeams.Team, List<PhotonPlayer>> PlayersPerTeam;

	// Token: 0x02000455 RID: 1109
	public enum Team : byte
	{
		// Token: 0x04001B70 RID: 7024
		none,
		// Token: 0x04001B71 RID: 7025
		red,
		// Token: 0x04001B72 RID: 7026
		blue
	}
}
