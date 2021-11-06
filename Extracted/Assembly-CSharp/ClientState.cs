using System;

// Token: 0x02000405 RID: 1029
public enum ClientState
{
	// Token: 0x0400198C RID: 6540
	Uninitialized,
	// Token: 0x0400198D RID: 6541
	PeerCreated,
	// Token: 0x0400198E RID: 6542
	Queued,
	// Token: 0x0400198F RID: 6543
	Authenticated,
	// Token: 0x04001990 RID: 6544
	JoinedLobby,
	// Token: 0x04001991 RID: 6545
	DisconnectingFromMasterserver,
	// Token: 0x04001992 RID: 6546
	ConnectingToGameserver,
	// Token: 0x04001993 RID: 6547
	ConnectedToGameserver,
	// Token: 0x04001994 RID: 6548
	Joining,
	// Token: 0x04001995 RID: 6549
	Joined,
	// Token: 0x04001996 RID: 6550
	Leaving,
	// Token: 0x04001997 RID: 6551
	DisconnectingFromGameserver,
	// Token: 0x04001998 RID: 6552
	ConnectingToMasterserver,
	// Token: 0x04001999 RID: 6553
	QueuedComingFromGameserver,
	// Token: 0x0400199A RID: 6554
	Disconnecting,
	// Token: 0x0400199B RID: 6555
	Disconnected,
	// Token: 0x0400199C RID: 6556
	ConnectedToMaster,
	// Token: 0x0400199D RID: 6557
	ConnectingToNameServer,
	// Token: 0x0400199E RID: 6558
	ConnectedToNameServer,
	// Token: 0x0400199F RID: 6559
	DisconnectingFromNameServer,
	// Token: 0x040019A0 RID: 6560
	Authenticating
}
