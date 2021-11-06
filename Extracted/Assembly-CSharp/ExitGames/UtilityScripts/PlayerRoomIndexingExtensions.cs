using System;
using UnityEngine;

namespace ExitGames.UtilityScripts
{
	// Token: 0x0200044D RID: 1101
	public static class PlayerRoomIndexingExtensions
	{
		// Token: 0x060026EC RID: 9964 RVA: 0x000C31CC File Offset: 0x000C13CC
		public static int GetRoomIndex(this PhotonPlayer player)
		{
			if (PlayerRoomIndexing.instance == null)
			{
				Debug.LogError("Missing PlayerRoomIndexing Component in Scene");
				return -1;
			}
			return PlayerRoomIndexing.instance.GetRoomIndex(player);
		}
	}
}
