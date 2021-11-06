using System;

namespace GooglePlayGames.BasicApi.Multiplayer
{
	// Token: 0x02000172 RID: 370
	public interface ITurnBasedMultiplayerClient
	{
		// Token: 0x06000C21 RID: 3105
		void CreateQuickMatch(uint minOpponents, uint maxOpponents, uint variant, Action<bool, TurnBasedMatch> callback);

		// Token: 0x06000C22 RID: 3106
		void CreateQuickMatch(uint minOpponents, uint maxOpponents, uint variant, ulong exclusiveBitmask, Action<bool, TurnBasedMatch> callback);

		// Token: 0x06000C23 RID: 3107
		void CreateWithInvitationScreen(uint minOpponents, uint maxOpponents, uint variant, Action<bool, TurnBasedMatch> callback);

		// Token: 0x06000C24 RID: 3108
		void CreateWithInvitationScreen(uint minOpponents, uint maxOpponents, uint variant, Action<UIStatus, TurnBasedMatch> callback);

		// Token: 0x06000C25 RID: 3109
		void GetAllInvitations(Action<Invitation[]> callback);

		// Token: 0x06000C26 RID: 3110
		void GetAllMatches(Action<TurnBasedMatch[]> callback);

		// Token: 0x06000C27 RID: 3111
		void AcceptFromInbox(Action<bool, TurnBasedMatch> callback);

		// Token: 0x06000C28 RID: 3112
		void AcceptInvitation(string invitationId, Action<bool, TurnBasedMatch> callback);

		// Token: 0x06000C29 RID: 3113
		void RegisterMatchDelegate(MatchDelegate del);

		// Token: 0x06000C2A RID: 3114
		void TakeTurn(TurnBasedMatch match, byte[] data, string pendingParticipantId, Action<bool> callback);

		// Token: 0x06000C2B RID: 3115
		int GetMaxMatchDataSize();

		// Token: 0x06000C2C RID: 3116
		void Finish(TurnBasedMatch match, byte[] data, MatchOutcome outcome, Action<bool> callback);

		// Token: 0x06000C2D RID: 3117
		void AcknowledgeFinished(TurnBasedMatch match, Action<bool> callback);

		// Token: 0x06000C2E RID: 3118
		void Leave(TurnBasedMatch match, Action<bool> callback);

		// Token: 0x06000C2F RID: 3119
		void LeaveDuringTurn(TurnBasedMatch match, string pendingParticipantId, Action<bool> callback);

		// Token: 0x06000C30 RID: 3120
		void Cancel(TurnBasedMatch match, Action<bool> callback);

		// Token: 0x06000C31 RID: 3121
		void Rematch(TurnBasedMatch match, Action<bool, TurnBasedMatch> callback);

		// Token: 0x06000C32 RID: 3122
		void DeclineInvitation(string invitationId);
	}
}
