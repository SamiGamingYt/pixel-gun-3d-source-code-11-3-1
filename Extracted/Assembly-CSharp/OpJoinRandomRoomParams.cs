using System;
using ExitGames.Client.Photon;

// Token: 0x020003F0 RID: 1008
internal class OpJoinRandomRoomParams
{
	// Token: 0x040018A6 RID: 6310
	public Hashtable ExpectedCustomRoomProperties;

	// Token: 0x040018A7 RID: 6311
	public byte ExpectedMaxPlayers;

	// Token: 0x040018A8 RID: 6312
	public MatchmakingMode MatchingType;

	// Token: 0x040018A9 RID: 6313
	public TypedLobby TypedLobby;

	// Token: 0x040018AA RID: 6314
	public string SqlLobbyFilter;

	// Token: 0x040018AB RID: 6315
	public string[] ExpectedUsers;
}
