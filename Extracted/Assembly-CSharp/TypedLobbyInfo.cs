using System;

// Token: 0x02000401 RID: 1025
public class TypedLobbyInfo : TypedLobby
{
	// Token: 0x0600242C RID: 9260 RVA: 0x000B3D04 File Offset: 0x000B1F04
	public override string ToString()
	{
		return string.Format("TypedLobbyInfo '{0}'[{1}] rooms: {2} players: {3}", new object[]
		{
			this.Name,
			this.Type,
			this.RoomCount,
			this.PlayerCount
		});
	}

	// Token: 0x04001978 RID: 6520
	public int PlayerCount;

	// Token: 0x04001979 RID: 6521
	public int RoomCount;
}
