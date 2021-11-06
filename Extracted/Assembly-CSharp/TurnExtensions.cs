using System;
using ExitGames.Client.Photon;

// Token: 0x02000459 RID: 1113
public static class TurnExtensions
{
	// Token: 0x0600272C RID: 10028 RVA: 0x000C41A0 File Offset: 0x000C23A0
	public static void SetTurn(this Room room, int turn, bool setStartTime = false)
	{
		if (room == null || room.customProperties == null)
		{
			return;
		}
		Hashtable hashtable = new Hashtable();
		hashtable[TurnExtensions.TurnPropKey] = turn;
		if (setStartTime)
		{
			hashtable[TurnExtensions.TurnStartPropKey] = PhotonNetwork.ServerTimestamp;
		}
		room.SetCustomProperties(hashtable, null, false);
	}

	// Token: 0x0600272D RID: 10029 RVA: 0x000C41FC File Offset: 0x000C23FC
	public static int GetTurn(this RoomInfo room)
	{
		if (room == null || room.customProperties == null || !room.customProperties.ContainsKey(TurnExtensions.TurnPropKey))
		{
			return 0;
		}
		return (int)room.customProperties[TurnExtensions.TurnPropKey];
	}

	// Token: 0x0600272E RID: 10030 RVA: 0x000C4248 File Offset: 0x000C2448
	public static int GetTurnStart(this RoomInfo room)
	{
		if (room == null || room.customProperties == null || !room.customProperties.ContainsKey(TurnExtensions.TurnStartPropKey))
		{
			return 0;
		}
		return (int)room.customProperties[TurnExtensions.TurnStartPropKey];
	}

	// Token: 0x0600272F RID: 10031 RVA: 0x000C4294 File Offset: 0x000C2494
	public static int GetFinishedTurn(this PhotonPlayer player)
	{
		Room room = PhotonNetwork.room;
		if (room == null || room.customProperties == null || !room.customProperties.ContainsKey(TurnExtensions.TurnPropKey))
		{
			return 0;
		}
		string key = TurnExtensions.FinishedTurnPropKey + player.ID;
		return (int)room.customProperties[key];
	}

	// Token: 0x06002730 RID: 10032 RVA: 0x000C42F8 File Offset: 0x000C24F8
	public static void SetFinishedTurn(this PhotonPlayer player, int turn)
	{
		Room room = PhotonNetwork.room;
		if (room == null || room.customProperties == null)
		{
			return;
		}
		string key = TurnExtensions.FinishedTurnPropKey + player.ID;
		Hashtable hashtable = new Hashtable();
		hashtable[key] = turn;
		room.SetCustomProperties(hashtable, null, false);
	}

	// Token: 0x04001B7A RID: 7034
	public static readonly string TurnPropKey = "Turn";

	// Token: 0x04001B7B RID: 7035
	public static readonly string TurnStartPropKey = "TStart";

	// Token: 0x04001B7C RID: 7036
	public static readonly string FinishedTurnPropKey = "FToA";
}
