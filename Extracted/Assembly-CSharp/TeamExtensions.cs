using System;
using ExitGames.Client.Photon;
using UnityEngine;

// Token: 0x02000456 RID: 1110
public static class TeamExtensions
{
	// Token: 0x06002715 RID: 10005 RVA: 0x000C3DC8 File Offset: 0x000C1FC8
	public static PunTeams.Team GetTeam(this PhotonPlayer player)
	{
		object obj;
		if (player.customProperties.TryGetValue("team", out obj))
		{
			return (PunTeams.Team)((byte)obj);
		}
		return PunTeams.Team.none;
	}

	// Token: 0x06002716 RID: 10006 RVA: 0x000C3DF4 File Offset: 0x000C1FF4
	public static void SetTeam(this PhotonPlayer player, PunTeams.Team team)
	{
		if (!PhotonNetwork.connectedAndReady)
		{
			Debug.LogWarning("JoinTeam was called in state: " + PhotonNetwork.connectionStateDetailed + ". Not connectedAndReady.");
			return;
		}
		PunTeams.Team team2 = player.GetTeam();
		if (team2 != team)
		{
			player.SetCustomProperties(new Hashtable
			{
				{
					"team",
					(byte)team
				}
			}, null, false);
		}
	}
}
