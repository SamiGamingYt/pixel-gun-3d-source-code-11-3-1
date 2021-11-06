using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using AOT;
using GooglePlayGames.Native.Cwrapper;
using GooglePlayGames.OurUtils;

namespace GooglePlayGames.Native.PInvoke
{
	// Token: 0x0200027F RID: 639
	internal class TurnBasedManager
	{
		// Token: 0x06001472 RID: 5234 RVA: 0x00051898 File Offset: 0x0004FA98
		internal TurnBasedManager(GameServices services)
		{
			this.mGameServices = services;
		}

		// Token: 0x06001473 RID: 5235 RVA: 0x000518A8 File Offset: 0x0004FAA8
		internal void GetMatch(string matchId, Action<TurnBasedManager.TurnBasedMatchResponse> callback)
		{
			TurnBasedMultiplayerManager.TurnBasedMultiplayerManager_FetchMatch(this.mGameServices.AsHandle(), matchId, new TurnBasedMultiplayerManager.TurnBasedMatchCallback(TurnBasedManager.InternalTurnBasedMatchCallback), TurnBasedManager.ToCallbackPointer(callback));
		}

		// Token: 0x06001474 RID: 5236 RVA: 0x000518D0 File Offset: 0x0004FAD0
		[MonoPInvokeCallback(typeof(TurnBasedMultiplayerManager.TurnBasedMatchCallback))]
		internal static void InternalTurnBasedMatchCallback(IntPtr response, IntPtr data)
		{
			Callbacks.PerformInternalCallback("TurnBasedManager#InternalTurnBasedMatchCallback", Callbacks.Type.Temporary, response, data);
		}

		// Token: 0x06001475 RID: 5237 RVA: 0x000518E0 File Offset: 0x0004FAE0
		internal void CreateMatch(TurnBasedMatchConfig config, Action<TurnBasedManager.TurnBasedMatchResponse> callback)
		{
			TurnBasedMultiplayerManager.TurnBasedMultiplayerManager_CreateTurnBasedMatch(this.mGameServices.AsHandle(), config.AsPointer(), new TurnBasedMultiplayerManager.TurnBasedMatchCallback(TurnBasedManager.InternalTurnBasedMatchCallback), TurnBasedManager.ToCallbackPointer(callback));
		}

		// Token: 0x06001476 RID: 5238 RVA: 0x00051918 File Offset: 0x0004FB18
		internal void ShowPlayerSelectUI(uint minimumPlayers, uint maxiumPlayers, bool allowAutomatching, Action<PlayerSelectUIResponse> callback)
		{
			TurnBasedMultiplayerManager.TurnBasedMultiplayerManager_ShowPlayerSelectUI(this.mGameServices.AsHandle(), minimumPlayers, maxiumPlayers, allowAutomatching, new TurnBasedMultiplayerManager.PlayerSelectUICallback(TurnBasedManager.InternalPlayerSelectUIcallback), Callbacks.ToIntPtr<PlayerSelectUIResponse>(callback, new Func<IntPtr, PlayerSelectUIResponse>(PlayerSelectUIResponse.FromPointer)));
		}

		// Token: 0x06001477 RID: 5239 RVA: 0x00051958 File Offset: 0x0004FB58
		[MonoPInvokeCallback(typeof(TurnBasedMultiplayerManager.PlayerSelectUICallback))]
		internal static void InternalPlayerSelectUIcallback(IntPtr response, IntPtr data)
		{
			Callbacks.PerformInternalCallback("TurnBasedManager#PlayerSelectUICallback", Callbacks.Type.Temporary, response, data);
		}

		// Token: 0x06001478 RID: 5240 RVA: 0x00051968 File Offset: 0x0004FB68
		internal void GetAllTurnbasedMatches(Action<TurnBasedManager.TurnBasedMatchesResponse> callback)
		{
			TurnBasedMultiplayerManager.TurnBasedMultiplayerManager_FetchMatches(this.mGameServices.AsHandle(), new TurnBasedMultiplayerManager.TurnBasedMatchesCallback(TurnBasedManager.InternalTurnBasedMatchesCallback), Callbacks.ToIntPtr<TurnBasedManager.TurnBasedMatchesResponse>(callback, new Func<IntPtr, TurnBasedManager.TurnBasedMatchesResponse>(TurnBasedManager.TurnBasedMatchesResponse.FromPointer)));
		}

		// Token: 0x06001479 RID: 5241 RVA: 0x000519A4 File Offset: 0x0004FBA4
		[MonoPInvokeCallback(typeof(TurnBasedMultiplayerManager.TurnBasedMatchesCallback))]
		internal static void InternalTurnBasedMatchesCallback(IntPtr response, IntPtr data)
		{
			Callbacks.PerformInternalCallback("TurnBasedManager#TurnBasedMatchesCallback", Callbacks.Type.Temporary, response, data);
		}

		// Token: 0x0600147A RID: 5242 RVA: 0x000519B4 File Offset: 0x0004FBB4
		internal void AcceptInvitation(MultiplayerInvitation invitation, Action<TurnBasedManager.TurnBasedMatchResponse> callback)
		{
			Logger.d("Accepting invitation: " + invitation.AsPointer().ToInt64());
			TurnBasedMultiplayerManager.TurnBasedMultiplayerManager_AcceptInvitation(this.mGameServices.AsHandle(), invitation.AsPointer(), new TurnBasedMultiplayerManager.TurnBasedMatchCallback(TurnBasedManager.InternalTurnBasedMatchCallback), TurnBasedManager.ToCallbackPointer(callback));
		}

		// Token: 0x0600147B RID: 5243 RVA: 0x00051A0C File Offset: 0x0004FC0C
		internal void DeclineInvitation(MultiplayerInvitation invitation)
		{
			TurnBasedMultiplayerManager.TurnBasedMultiplayerManager_DeclineInvitation(this.mGameServices.AsHandle(), invitation.AsPointer());
		}

		// Token: 0x0600147C RID: 5244 RVA: 0x00051A24 File Offset: 0x0004FC24
		internal void TakeTurn(NativeTurnBasedMatch match, byte[] data, MultiplayerParticipant nextParticipant, Action<TurnBasedManager.TurnBasedMatchResponse> callback)
		{
			TurnBasedMultiplayerManager.TurnBasedMultiplayerManager_TakeMyTurn(this.mGameServices.AsHandle(), match.AsPointer(), data, new UIntPtr((uint)data.Length), match.Results().AsPointer(), nextParticipant.AsPointer(), new TurnBasedMultiplayerManager.TurnBasedMatchCallback(TurnBasedManager.InternalTurnBasedMatchCallback), TurnBasedManager.ToCallbackPointer(callback));
		}

		// Token: 0x0600147D RID: 5245 RVA: 0x00051A74 File Offset: 0x0004FC74
		[MonoPInvokeCallback(typeof(TurnBasedMultiplayerManager.MatchInboxUICallback))]
		internal static void InternalMatchInboxUICallback(IntPtr response, IntPtr data)
		{
			Callbacks.PerformInternalCallback("TurnBasedManager#MatchInboxUICallback", Callbacks.Type.Temporary, response, data);
		}

		// Token: 0x0600147E RID: 5246 RVA: 0x00051A84 File Offset: 0x0004FC84
		internal void ShowInboxUI(Action<TurnBasedManager.MatchInboxUIResponse> callback)
		{
			TurnBasedMultiplayerManager.TurnBasedMultiplayerManager_ShowMatchInboxUI(this.mGameServices.AsHandle(), new TurnBasedMultiplayerManager.MatchInboxUICallback(TurnBasedManager.InternalMatchInboxUICallback), Callbacks.ToIntPtr<TurnBasedManager.MatchInboxUIResponse>(callback, new Func<IntPtr, TurnBasedManager.MatchInboxUIResponse>(TurnBasedManager.MatchInboxUIResponse.FromPointer)));
		}

		// Token: 0x0600147F RID: 5247 RVA: 0x00051AC0 File Offset: 0x0004FCC0
		[MonoPInvokeCallback(typeof(TurnBasedMultiplayerManager.MultiplayerStatusCallback))]
		internal static void InternalMultiplayerStatusCallback(CommonErrorStatus.MultiplayerStatus status, IntPtr data)
		{
			Logger.d("InternalMultiplayerStatusCallback: " + status);
			Action<CommonErrorStatus.MultiplayerStatus> action = Callbacks.IntPtrToTempCallback<Action<CommonErrorStatus.MultiplayerStatus>>(data);
			try
			{
				action(status);
			}
			catch (Exception arg)
			{
				Logger.e("Error encountered executing InternalMultiplayerStatusCallback. Smothering to avoid passing exception into Native: " + arg);
			}
		}

		// Token: 0x06001480 RID: 5248 RVA: 0x00051B28 File Offset: 0x0004FD28
		internal void LeaveDuringMyTurn(NativeTurnBasedMatch match, MultiplayerParticipant nextParticipant, Action<CommonErrorStatus.MultiplayerStatus> callback)
		{
			TurnBasedMultiplayerManager.TurnBasedMultiplayerManager_LeaveMatchDuringMyTurn(this.mGameServices.AsHandle(), match.AsPointer(), nextParticipant.AsPointer(), new TurnBasedMultiplayerManager.MultiplayerStatusCallback(TurnBasedManager.InternalMultiplayerStatusCallback), Callbacks.ToIntPtr(callback));
		}

		// Token: 0x06001481 RID: 5249 RVA: 0x00051B64 File Offset: 0x0004FD64
		internal void FinishMatchDuringMyTurn(NativeTurnBasedMatch match, byte[] data, ParticipantResults results, Action<TurnBasedManager.TurnBasedMatchResponse> callback)
		{
			TurnBasedMultiplayerManager.TurnBasedMultiplayerManager_FinishMatchDuringMyTurn(this.mGameServices.AsHandle(), match.AsPointer(), data, new UIntPtr((uint)data.Length), results.AsPointer(), new TurnBasedMultiplayerManager.TurnBasedMatchCallback(TurnBasedManager.InternalTurnBasedMatchCallback), TurnBasedManager.ToCallbackPointer(callback));
		}

		// Token: 0x06001482 RID: 5250 RVA: 0x00051BAC File Offset: 0x0004FDAC
		internal void ConfirmPendingCompletion(NativeTurnBasedMatch match, Action<TurnBasedManager.TurnBasedMatchResponse> callback)
		{
			TurnBasedMultiplayerManager.TurnBasedMultiplayerManager_ConfirmPendingCompletion(this.mGameServices.AsHandle(), match.AsPointer(), new TurnBasedMultiplayerManager.TurnBasedMatchCallback(TurnBasedManager.InternalTurnBasedMatchCallback), TurnBasedManager.ToCallbackPointer(callback));
		}

		// Token: 0x06001483 RID: 5251 RVA: 0x00051BE4 File Offset: 0x0004FDE4
		internal void LeaveMatchDuringTheirTurn(NativeTurnBasedMatch match, Action<CommonErrorStatus.MultiplayerStatus> callback)
		{
			TurnBasedMultiplayerManager.TurnBasedMultiplayerManager_LeaveMatchDuringTheirTurn(this.mGameServices.AsHandle(), match.AsPointer(), new TurnBasedMultiplayerManager.MultiplayerStatusCallback(TurnBasedManager.InternalMultiplayerStatusCallback), Callbacks.ToIntPtr(callback));
		}

		// Token: 0x06001484 RID: 5252 RVA: 0x00051C1C File Offset: 0x0004FE1C
		internal void CancelMatch(NativeTurnBasedMatch match, Action<CommonErrorStatus.MultiplayerStatus> callback)
		{
			TurnBasedMultiplayerManager.TurnBasedMultiplayerManager_CancelMatch(this.mGameServices.AsHandle(), match.AsPointer(), new TurnBasedMultiplayerManager.MultiplayerStatusCallback(TurnBasedManager.InternalMultiplayerStatusCallback), Callbacks.ToIntPtr(callback));
		}

		// Token: 0x06001485 RID: 5253 RVA: 0x00051C54 File Offset: 0x0004FE54
		internal void Rematch(NativeTurnBasedMatch match, Action<TurnBasedManager.TurnBasedMatchResponse> callback)
		{
			TurnBasedMultiplayerManager.TurnBasedMultiplayerManager_Rematch(this.mGameServices.AsHandle(), match.AsPointer(), new TurnBasedMultiplayerManager.TurnBasedMatchCallback(TurnBasedManager.InternalTurnBasedMatchCallback), TurnBasedManager.ToCallbackPointer(callback));
		}

		// Token: 0x06001486 RID: 5254 RVA: 0x00051C8C File Offset: 0x0004FE8C
		private static IntPtr ToCallbackPointer(Action<TurnBasedManager.TurnBasedMatchResponse> callback)
		{
			return Callbacks.ToIntPtr<TurnBasedManager.TurnBasedMatchResponse>(callback, new Func<IntPtr, TurnBasedManager.TurnBasedMatchResponse>(TurnBasedManager.TurnBasedMatchResponse.FromPointer));
		}

		// Token: 0x04000C12 RID: 3090
		private readonly GameServices mGameServices;

		// Token: 0x02000280 RID: 640
		internal class MatchInboxUIResponse : BaseReferenceHolder
		{
			// Token: 0x06001487 RID: 5255 RVA: 0x00051CA0 File Offset: 0x0004FEA0
			internal MatchInboxUIResponse(IntPtr selfPointer) : base(selfPointer)
			{
			}

			// Token: 0x06001488 RID: 5256 RVA: 0x00051CAC File Offset: 0x0004FEAC
			internal CommonErrorStatus.UIStatus UiStatus()
			{
				return TurnBasedMultiplayerManager.TurnBasedMultiplayerManager_MatchInboxUIResponse_GetStatus(base.SelfPtr());
			}

			// Token: 0x06001489 RID: 5257 RVA: 0x00051CBC File Offset: 0x0004FEBC
			internal NativeTurnBasedMatch Match()
			{
				if (this.UiStatus() != CommonErrorStatus.UIStatus.VALID)
				{
					return null;
				}
				return new NativeTurnBasedMatch(TurnBasedMultiplayerManager.TurnBasedMultiplayerManager_MatchInboxUIResponse_GetMatch(base.SelfPtr()));
			}

			// Token: 0x0600148A RID: 5258 RVA: 0x00051CDC File Offset: 0x0004FEDC
			protected override void CallDispose(HandleRef selfPointer)
			{
				TurnBasedMultiplayerManager.TurnBasedMultiplayerManager_MatchInboxUIResponse_Dispose(selfPointer);
			}

			// Token: 0x0600148B RID: 5259 RVA: 0x00051CE4 File Offset: 0x0004FEE4
			internal static TurnBasedManager.MatchInboxUIResponse FromPointer(IntPtr pointer)
			{
				if (pointer.Equals(IntPtr.Zero))
				{
					return null;
				}
				return new TurnBasedManager.MatchInboxUIResponse(pointer);
			}
		}

		// Token: 0x02000281 RID: 641
		internal class TurnBasedMatchResponse : BaseReferenceHolder
		{
			// Token: 0x0600148C RID: 5260 RVA: 0x00051D04 File Offset: 0x0004FF04
			internal TurnBasedMatchResponse(IntPtr selfPointer) : base(selfPointer)
			{
			}

			// Token: 0x0600148D RID: 5261 RVA: 0x00051D10 File Offset: 0x0004FF10
			internal CommonErrorStatus.MultiplayerStatus ResponseStatus()
			{
				return TurnBasedMultiplayerManager.TurnBasedMultiplayerManager_TurnBasedMatchResponse_GetStatus(base.SelfPtr());
			}

			// Token: 0x0600148E RID: 5262 RVA: 0x00051D20 File Offset: 0x0004FF20
			internal bool RequestSucceeded()
			{
				return this.ResponseStatus() > (CommonErrorStatus.MultiplayerStatus)0;
			}

			// Token: 0x0600148F RID: 5263 RVA: 0x00051D2C File Offset: 0x0004FF2C
			internal NativeTurnBasedMatch Match()
			{
				if (!this.RequestSucceeded())
				{
					return null;
				}
				return new NativeTurnBasedMatch(TurnBasedMultiplayerManager.TurnBasedMultiplayerManager_TurnBasedMatchResponse_GetMatch(base.SelfPtr()));
			}

			// Token: 0x06001490 RID: 5264 RVA: 0x00051D4C File Offset: 0x0004FF4C
			protected override void CallDispose(HandleRef selfPointer)
			{
				TurnBasedMultiplayerManager.TurnBasedMultiplayerManager_TurnBasedMatchResponse_Dispose(selfPointer);
			}

			// Token: 0x06001491 RID: 5265 RVA: 0x00051D54 File Offset: 0x0004FF54
			internal static TurnBasedManager.TurnBasedMatchResponse FromPointer(IntPtr pointer)
			{
				if (pointer.Equals(IntPtr.Zero))
				{
					return null;
				}
				return new TurnBasedManager.TurnBasedMatchResponse(pointer);
			}
		}

		// Token: 0x02000282 RID: 642
		internal class TurnBasedMatchesResponse : BaseReferenceHolder
		{
			// Token: 0x06001492 RID: 5266 RVA: 0x00051D74 File Offset: 0x0004FF74
			internal TurnBasedMatchesResponse(IntPtr selfPointer) : base(selfPointer)
			{
			}

			// Token: 0x06001493 RID: 5267 RVA: 0x00051D80 File Offset: 0x0004FF80
			protected override void CallDispose(HandleRef selfPointer)
			{
				TurnBasedMultiplayerManager.TurnBasedMultiplayerManager_TurnBasedMatchesResponse_Dispose(base.SelfPtr());
			}

			// Token: 0x06001494 RID: 5268 RVA: 0x00051D90 File Offset: 0x0004FF90
			internal CommonErrorStatus.MultiplayerStatus Status()
			{
				return TurnBasedMultiplayerManager.TurnBasedMultiplayerManager_TurnBasedMatchesResponse_GetStatus(base.SelfPtr());
			}

			// Token: 0x06001495 RID: 5269 RVA: 0x00051DA0 File Offset: 0x0004FFA0
			internal IEnumerable<MultiplayerInvitation> Invitations()
			{
				return PInvokeUtilities.ToEnumerable<MultiplayerInvitation>(TurnBasedMultiplayerManager.TurnBasedMultiplayerManager_TurnBasedMatchesResponse_GetInvitations_Length(base.SelfPtr()), (UIntPtr index) => new MultiplayerInvitation(TurnBasedMultiplayerManager.TurnBasedMultiplayerManager_TurnBasedMatchesResponse_GetInvitations_GetElement(base.SelfPtr(), index)));
			}

			// Token: 0x06001496 RID: 5270 RVA: 0x00051DC0 File Offset: 0x0004FFC0
			internal int InvitationCount()
			{
				return (int)TurnBasedMultiplayerManager.TurnBasedMultiplayerManager_TurnBasedMatchesResponse_GetInvitations_Length(base.SelfPtr()).ToUInt32();
			}

			// Token: 0x06001497 RID: 5271 RVA: 0x00051DE0 File Offset: 0x0004FFE0
			internal IEnumerable<NativeTurnBasedMatch> MyTurnMatches()
			{
				return PInvokeUtilities.ToEnumerable<NativeTurnBasedMatch>(TurnBasedMultiplayerManager.TurnBasedMultiplayerManager_TurnBasedMatchesResponse_GetMyTurnMatches_Length(base.SelfPtr()), (UIntPtr index) => new NativeTurnBasedMatch(TurnBasedMultiplayerManager.TurnBasedMultiplayerManager_TurnBasedMatchesResponse_GetMyTurnMatches_GetElement(base.SelfPtr(), index)));
			}

			// Token: 0x06001498 RID: 5272 RVA: 0x00051E00 File Offset: 0x00050000
			internal int MyTurnMatchesCount()
			{
				return (int)TurnBasedMultiplayerManager.TurnBasedMultiplayerManager_TurnBasedMatchesResponse_GetMyTurnMatches_Length(base.SelfPtr()).ToUInt32();
			}

			// Token: 0x06001499 RID: 5273 RVA: 0x00051E20 File Offset: 0x00050020
			internal IEnumerable<NativeTurnBasedMatch> TheirTurnMatches()
			{
				return PInvokeUtilities.ToEnumerable<NativeTurnBasedMatch>(TurnBasedMultiplayerManager.TurnBasedMultiplayerManager_TurnBasedMatchesResponse_GetTheirTurnMatches_Length(base.SelfPtr()), (UIntPtr index) => new NativeTurnBasedMatch(TurnBasedMultiplayerManager.TurnBasedMultiplayerManager_TurnBasedMatchesResponse_GetTheirTurnMatches_GetElement(base.SelfPtr(), index)));
			}

			// Token: 0x0600149A RID: 5274 RVA: 0x00051E40 File Offset: 0x00050040
			internal int TheirTurnMatchesCount()
			{
				return (int)TurnBasedMultiplayerManager.TurnBasedMultiplayerManager_TurnBasedMatchesResponse_GetTheirTurnMatches_Length(base.SelfPtr()).ToUInt32();
			}

			// Token: 0x0600149B RID: 5275 RVA: 0x00051E60 File Offset: 0x00050060
			internal IEnumerable<NativeTurnBasedMatch> CompletedMatches()
			{
				return PInvokeUtilities.ToEnumerable<NativeTurnBasedMatch>(TurnBasedMultiplayerManager.TurnBasedMultiplayerManager_TurnBasedMatchesResponse_GetCompletedMatches_Length(base.SelfPtr()), (UIntPtr index) => new NativeTurnBasedMatch(TurnBasedMultiplayerManager.TurnBasedMultiplayerManager_TurnBasedMatchesResponse_GetCompletedMatches_GetElement(base.SelfPtr(), index)));
			}

			// Token: 0x0600149C RID: 5276 RVA: 0x00051E80 File Offset: 0x00050080
			internal int CompletedMatchesCount()
			{
				return (int)TurnBasedMultiplayerManager.TurnBasedMultiplayerManager_TurnBasedMatchesResponse_GetCompletedMatches_Length(base.SelfPtr()).ToUInt32();
			}

			// Token: 0x0600149D RID: 5277 RVA: 0x00051EA0 File Offset: 0x000500A0
			internal static TurnBasedManager.TurnBasedMatchesResponse FromPointer(IntPtr pointer)
			{
				if (PInvokeUtilities.IsNull(pointer))
				{
					return null;
				}
				return new TurnBasedManager.TurnBasedMatchesResponse(pointer);
			}
		}

		// Token: 0x020008E4 RID: 2276
		// (Invoke) Token: 0x06005030 RID: 20528
		internal delegate void TurnBasedMatchCallback(TurnBasedManager.TurnBasedMatchResponse response);
	}
}
