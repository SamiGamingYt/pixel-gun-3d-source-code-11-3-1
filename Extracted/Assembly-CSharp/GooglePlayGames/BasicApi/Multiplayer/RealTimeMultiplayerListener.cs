using System;

namespace GooglePlayGames.BasicApi.Multiplayer
{
	// Token: 0x0200017A RID: 378
	public interface RealTimeMultiplayerListener
	{
		// Token: 0x06000C4D RID: 3149
		void OnRoomSetupProgress(float percent);

		// Token: 0x06000C4E RID: 3150
		void OnRoomConnected(bool success);

		// Token: 0x06000C4F RID: 3151
		void OnLeftRoom();

		// Token: 0x06000C50 RID: 3152
		void OnParticipantLeft(Participant participant);

		// Token: 0x06000C51 RID: 3153
		void OnPeersConnected(string[] participantIds);

		// Token: 0x06000C52 RID: 3154
		void OnPeersDisconnected(string[] participantIds);

		// Token: 0x06000C53 RID: 3155
		void OnRealTimeMessageReceived(bool isReliable, string senderId, byte[] data);
	}
}
