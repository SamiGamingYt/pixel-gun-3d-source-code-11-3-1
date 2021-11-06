using System;
using System.Collections.Generic;

namespace GooglePlayGames.BasicApi.Multiplayer
{
	// Token: 0x02000171 RID: 369
	public interface IRealTimeMultiplayerClient
	{
		// Token: 0x06000C0F RID: 3087
		void CreateQuickGame(uint minOpponents, uint maxOpponents, uint variant, RealTimeMultiplayerListener listener);

		// Token: 0x06000C10 RID: 3088
		void CreateQuickGame(uint minOpponents, uint maxOpponents, uint variant, ulong exclusiveBitMask, RealTimeMultiplayerListener listener);

		// Token: 0x06000C11 RID: 3089
		void CreateWithInvitationScreen(uint minOpponents, uint maxOppponents, uint variant, RealTimeMultiplayerListener listener);

		// Token: 0x06000C12 RID: 3090
		void ShowWaitingRoomUI();

		// Token: 0x06000C13 RID: 3091
		void GetAllInvitations(Action<Invitation[]> callback);

		// Token: 0x06000C14 RID: 3092
		void AcceptFromInbox(RealTimeMultiplayerListener listener);

		// Token: 0x06000C15 RID: 3093
		void AcceptInvitation(string invitationId, RealTimeMultiplayerListener listener);

		// Token: 0x06000C16 RID: 3094
		void SendMessageToAll(bool reliable, byte[] data);

		// Token: 0x06000C17 RID: 3095
		void SendMessageToAll(bool reliable, byte[] data, int offset, int length);

		// Token: 0x06000C18 RID: 3096
		void SendMessage(bool reliable, string participantId, byte[] data);

		// Token: 0x06000C19 RID: 3097
		void SendMessage(bool reliable, string participantId, byte[] data, int offset, int length);

		// Token: 0x06000C1A RID: 3098
		List<Participant> GetConnectedParticipants();

		// Token: 0x06000C1B RID: 3099
		Participant GetSelf();

		// Token: 0x06000C1C RID: 3100
		Participant GetParticipant(string participantId);

		// Token: 0x06000C1D RID: 3101
		Invitation GetInvitation();

		// Token: 0x06000C1E RID: 3102
		void LeaveRoom();

		// Token: 0x06000C1F RID: 3103
		bool IsRoomConnected();

		// Token: 0x06000C20 RID: 3104
		void DeclineInvitation(string invitationId);
	}
}
