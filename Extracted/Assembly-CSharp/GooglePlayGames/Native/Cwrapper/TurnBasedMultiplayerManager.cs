using System;
using System.Runtime.InteropServices;
using System.Text;

namespace GooglePlayGames.Native.Cwrapper
{
	// Token: 0x02000202 RID: 514
	internal static class TurnBasedMultiplayerManager
	{
		// Token: 0x06001072 RID: 4210
		[DllImport("gpg")]
		internal static extern void TurnBasedMultiplayerManager_ShowPlayerSelectUI(HandleRef self, uint minimum_players, uint maximum_players, [MarshalAs(UnmanagedType.I1)] bool allow_automatch, TurnBasedMultiplayerManager.PlayerSelectUICallback callback, IntPtr callback_arg);

		// Token: 0x06001073 RID: 4211
		[DllImport("gpg")]
		internal static extern void TurnBasedMultiplayerManager_CancelMatch(HandleRef self, IntPtr match, TurnBasedMultiplayerManager.MultiplayerStatusCallback callback, IntPtr callback_arg);

		// Token: 0x06001074 RID: 4212
		[DllImport("gpg")]
		internal static extern void TurnBasedMultiplayerManager_DismissMatch(HandleRef self, IntPtr match);

		// Token: 0x06001075 RID: 4213
		[DllImport("gpg")]
		internal static extern void TurnBasedMultiplayerManager_ShowMatchInboxUI(HandleRef self, TurnBasedMultiplayerManager.MatchInboxUICallback callback, IntPtr callback_arg);

		// Token: 0x06001076 RID: 4214
		[DllImport("gpg")]
		internal static extern void TurnBasedMultiplayerManager_SynchronizeData(HandleRef self);

		// Token: 0x06001077 RID: 4215
		[DllImport("gpg")]
		internal static extern void TurnBasedMultiplayerManager_Rematch(HandleRef self, IntPtr match, TurnBasedMultiplayerManager.TurnBasedMatchCallback callback, IntPtr callback_arg);

		// Token: 0x06001078 RID: 4216
		[DllImport("gpg")]
		internal static extern void TurnBasedMultiplayerManager_DismissInvitation(HandleRef self, IntPtr invitation);

		// Token: 0x06001079 RID: 4217
		[DllImport("gpg")]
		internal static extern void TurnBasedMultiplayerManager_FetchMatch(HandleRef self, string match_id, TurnBasedMultiplayerManager.TurnBasedMatchCallback callback, IntPtr callback_arg);

		// Token: 0x0600107A RID: 4218
		[DllImport("gpg")]
		internal static extern void TurnBasedMultiplayerManager_DeclineInvitation(HandleRef self, IntPtr invitation);

		// Token: 0x0600107B RID: 4219
		[DllImport("gpg")]
		internal static extern void TurnBasedMultiplayerManager_FinishMatchDuringMyTurn(HandleRef self, IntPtr match, byte[] match_data, UIntPtr match_data_size, IntPtr results, TurnBasedMultiplayerManager.TurnBasedMatchCallback callback, IntPtr callback_arg);

		// Token: 0x0600107C RID: 4220
		[DllImport("gpg")]
		internal static extern void TurnBasedMultiplayerManager_FetchMatches(HandleRef self, TurnBasedMultiplayerManager.TurnBasedMatchesCallback callback, IntPtr callback_arg);

		// Token: 0x0600107D RID: 4221
		[DllImport("gpg")]
		internal static extern void TurnBasedMultiplayerManager_CreateTurnBasedMatch(HandleRef self, IntPtr config, TurnBasedMultiplayerManager.TurnBasedMatchCallback callback, IntPtr callback_arg);

		// Token: 0x0600107E RID: 4222
		[DllImport("gpg")]
		internal static extern void TurnBasedMultiplayerManager_AcceptInvitation(HandleRef self, IntPtr invitation, TurnBasedMultiplayerManager.TurnBasedMatchCallback callback, IntPtr callback_arg);

		// Token: 0x0600107F RID: 4223
		[DllImport("gpg")]
		internal static extern void TurnBasedMultiplayerManager_TakeMyTurn(HandleRef self, IntPtr match, byte[] match_data, UIntPtr match_data_size, IntPtr results, IntPtr next_participant, TurnBasedMultiplayerManager.TurnBasedMatchCallback callback, IntPtr callback_arg);

		// Token: 0x06001080 RID: 4224
		[DllImport("gpg")]
		internal static extern void TurnBasedMultiplayerManager_ConfirmPendingCompletion(HandleRef self, IntPtr match, TurnBasedMultiplayerManager.TurnBasedMatchCallback callback, IntPtr callback_arg);

		// Token: 0x06001081 RID: 4225
		[DllImport("gpg")]
		internal static extern void TurnBasedMultiplayerManager_LeaveMatchDuringMyTurn(HandleRef self, IntPtr match, IntPtr next_participant, TurnBasedMultiplayerManager.MultiplayerStatusCallback callback, IntPtr callback_arg);

		// Token: 0x06001082 RID: 4226
		[DllImport("gpg")]
		internal static extern void TurnBasedMultiplayerManager_LeaveMatchDuringTheirTurn(HandleRef self, IntPtr match, TurnBasedMultiplayerManager.MultiplayerStatusCallback callback, IntPtr callback_arg);

		// Token: 0x06001083 RID: 4227
		[DllImport("gpg")]
		internal static extern void TurnBasedMultiplayerManager_TurnBasedMatchResponse_Dispose(HandleRef self);

		// Token: 0x06001084 RID: 4228
		[DllImport("gpg")]
		internal static extern CommonErrorStatus.MultiplayerStatus TurnBasedMultiplayerManager_TurnBasedMatchResponse_GetStatus(HandleRef self);

		// Token: 0x06001085 RID: 4229
		[DllImport("gpg")]
		internal static extern IntPtr TurnBasedMultiplayerManager_TurnBasedMatchResponse_GetMatch(HandleRef self);

		// Token: 0x06001086 RID: 4230
		[DllImport("gpg")]
		internal static extern void TurnBasedMultiplayerManager_TurnBasedMatchesResponse_Dispose(HandleRef self);

		// Token: 0x06001087 RID: 4231
		[DllImport("gpg")]
		internal static extern CommonErrorStatus.MultiplayerStatus TurnBasedMultiplayerManager_TurnBasedMatchesResponse_GetStatus(HandleRef self);

		// Token: 0x06001088 RID: 4232
		[DllImport("gpg")]
		internal static extern UIntPtr TurnBasedMultiplayerManager_TurnBasedMatchesResponse_GetInvitations_Length(HandleRef self);

		// Token: 0x06001089 RID: 4233
		[DllImport("gpg")]
		internal static extern IntPtr TurnBasedMultiplayerManager_TurnBasedMatchesResponse_GetInvitations_GetElement(HandleRef self, UIntPtr index);

		// Token: 0x0600108A RID: 4234
		[DllImport("gpg")]
		internal static extern UIntPtr TurnBasedMultiplayerManager_TurnBasedMatchesResponse_GetMyTurnMatches_Length(HandleRef self);

		// Token: 0x0600108B RID: 4235
		[DllImport("gpg")]
		internal static extern IntPtr TurnBasedMultiplayerManager_TurnBasedMatchesResponse_GetMyTurnMatches_GetElement(HandleRef self, UIntPtr index);

		// Token: 0x0600108C RID: 4236
		[DllImport("gpg")]
		internal static extern UIntPtr TurnBasedMultiplayerManager_TurnBasedMatchesResponse_GetTheirTurnMatches_Length(HandleRef self);

		// Token: 0x0600108D RID: 4237
		[DllImport("gpg")]
		internal static extern IntPtr TurnBasedMultiplayerManager_TurnBasedMatchesResponse_GetTheirTurnMatches_GetElement(HandleRef self, UIntPtr index);

		// Token: 0x0600108E RID: 4238
		[DllImport("gpg")]
		internal static extern UIntPtr TurnBasedMultiplayerManager_TurnBasedMatchesResponse_GetCompletedMatches_Length(HandleRef self);

		// Token: 0x0600108F RID: 4239
		[DllImport("gpg")]
		internal static extern IntPtr TurnBasedMultiplayerManager_TurnBasedMatchesResponse_GetCompletedMatches_GetElement(HandleRef self, UIntPtr index);

		// Token: 0x06001090 RID: 4240
		[DllImport("gpg")]
		internal static extern void TurnBasedMultiplayerManager_MatchInboxUIResponse_Dispose(HandleRef self);

		// Token: 0x06001091 RID: 4241
		[DllImport("gpg")]
		internal static extern CommonErrorStatus.UIStatus TurnBasedMultiplayerManager_MatchInboxUIResponse_GetStatus(HandleRef self);

		// Token: 0x06001092 RID: 4242
		[DllImport("gpg")]
		internal static extern IntPtr TurnBasedMultiplayerManager_MatchInboxUIResponse_GetMatch(HandleRef self);

		// Token: 0x06001093 RID: 4243
		[DllImport("gpg")]
		internal static extern void TurnBasedMultiplayerManager_PlayerSelectUIResponse_Dispose(HandleRef self);

		// Token: 0x06001094 RID: 4244
		[DllImport("gpg")]
		internal static extern CommonErrorStatus.UIStatus TurnBasedMultiplayerManager_PlayerSelectUIResponse_GetStatus(HandleRef self);

		// Token: 0x06001095 RID: 4245
		[DllImport("gpg")]
		internal static extern UIntPtr TurnBasedMultiplayerManager_PlayerSelectUIResponse_GetPlayerIds_Length(HandleRef self);

		// Token: 0x06001096 RID: 4246
		[DllImport("gpg")]
		internal static extern UIntPtr TurnBasedMultiplayerManager_PlayerSelectUIResponse_GetPlayerIds_GetElement(HandleRef self, UIntPtr index, StringBuilder out_arg, UIntPtr out_size);

		// Token: 0x06001097 RID: 4247
		[DllImport("gpg")]
		internal static extern uint TurnBasedMultiplayerManager_PlayerSelectUIResponse_GetMinimumAutomatchingPlayers(HandleRef self);

		// Token: 0x06001098 RID: 4248
		[DllImport("gpg")]
		internal static extern uint TurnBasedMultiplayerManager_PlayerSelectUIResponse_GetMaximumAutomatchingPlayers(HandleRef self);

		// Token: 0x020008D8 RID: 2264
		// (Invoke) Token: 0x06005000 RID: 20480
		internal delegate void TurnBasedMatchCallback(IntPtr arg0, IntPtr arg1);

		// Token: 0x020008D9 RID: 2265
		// (Invoke) Token: 0x06005004 RID: 20484
		internal delegate void MultiplayerStatusCallback(CommonErrorStatus.MultiplayerStatus arg0, IntPtr arg1);

		// Token: 0x020008DA RID: 2266
		// (Invoke) Token: 0x06005008 RID: 20488
		internal delegate void TurnBasedMatchesCallback(IntPtr arg0, IntPtr arg1);

		// Token: 0x020008DB RID: 2267
		// (Invoke) Token: 0x0600500C RID: 20492
		internal delegate void MatchInboxUICallback(IntPtr arg0, IntPtr arg1);

		// Token: 0x020008DC RID: 2268
		// (Invoke) Token: 0x06005010 RID: 20496
		internal delegate void PlayerSelectUICallback(IntPtr arg0, IntPtr arg1);
	}
}
