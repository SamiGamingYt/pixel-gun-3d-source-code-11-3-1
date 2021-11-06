using System;

namespace ExitGames.Client.Photon.Chat
{
	// Token: 0x02000471 RID: 1137
	public enum ChatState
	{
		// Token: 0x04001C04 RID: 7172
		Uninitialized,
		// Token: 0x04001C05 RID: 7173
		ConnectingToNameServer,
		// Token: 0x04001C06 RID: 7174
		ConnectedToNameServer,
		// Token: 0x04001C07 RID: 7175
		Authenticating,
		// Token: 0x04001C08 RID: 7176
		Authenticated,
		// Token: 0x04001C09 RID: 7177
		DisconnectingFromNameServer,
		// Token: 0x04001C0A RID: 7178
		ConnectingToFrontEnd,
		// Token: 0x04001C0B RID: 7179
		ConnectedToFrontEnd,
		// Token: 0x04001C0C RID: 7180
		DisconnectingFromFrontEnd,
		// Token: 0x04001C0D RID: 7181
		QueuedComingFromFrontEnd,
		// Token: 0x04001C0E RID: 7182
		Disconnecting,
		// Token: 0x04001C0F RID: 7183
		Disconnected
	}
}
