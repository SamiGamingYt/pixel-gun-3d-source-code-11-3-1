using System;
using ExitGames.Client.Photon;

// Token: 0x020003F1 RID: 1009
internal class EnterRoomParams
{
	// Token: 0x040018AC RID: 6316
	public string RoomName;

	// Token: 0x040018AD RID: 6317
	public RoomOptions RoomOptions;

	// Token: 0x040018AE RID: 6318
	public TypedLobby Lobby;

	// Token: 0x040018AF RID: 6319
	public Hashtable PlayerProperties;

	// Token: 0x040018B0 RID: 6320
	public bool OnGameServer = true;

	// Token: 0x040018B1 RID: 6321
	public bool CreateIfNotExists;

	// Token: 0x040018B2 RID: 6322
	public bool RejoinOnly;

	// Token: 0x040018B3 RID: 6323
	public string[] ExpectedUsers;
}
