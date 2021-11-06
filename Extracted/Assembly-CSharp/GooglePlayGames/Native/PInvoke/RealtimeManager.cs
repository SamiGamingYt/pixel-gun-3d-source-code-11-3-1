using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using AOT;
using GooglePlayGames.Native.Cwrapper;
using GooglePlayGames.OurUtils;

namespace GooglePlayGames.Native.PInvoke
{
	// Token: 0x02000270 RID: 624
	internal class RealtimeManager
	{
		// Token: 0x06001400 RID: 5120 RVA: 0x00050BBC File Offset: 0x0004EDBC
		internal RealtimeManager(GameServices gameServices)
		{
			this.mGameServices = Misc.CheckNotNull<GameServices>(gameServices);
		}

		// Token: 0x06001401 RID: 5121 RVA: 0x00050BD0 File Offset: 0x0004EDD0
		internal void CreateRoom(RealtimeRoomConfig config, RealTimeEventListenerHelper helper, Action<RealtimeManager.RealTimeRoomResponse> callback)
		{
			RealTimeMultiplayerManager.RealTimeMultiplayerManager_CreateRealTimeRoom(this.mGameServices.AsHandle(), config.AsPointer(), helper.AsPointer(), new RealTimeMultiplayerManager.RealTimeRoomCallback(RealtimeManager.InternalRealTimeRoomCallback), RealtimeManager.ToCallbackPointer(callback));
		}

		// Token: 0x06001402 RID: 5122 RVA: 0x00050C0C File Offset: 0x0004EE0C
		internal void ShowPlayerSelectUI(uint minimumPlayers, uint maxiumPlayers, bool allowAutomatching, Action<PlayerSelectUIResponse> callback)
		{
			RealTimeMultiplayerManager.RealTimeMultiplayerManager_ShowPlayerSelectUI(this.mGameServices.AsHandle(), minimumPlayers, maxiumPlayers, allowAutomatching, new RealTimeMultiplayerManager.PlayerSelectUICallback(RealtimeManager.InternalPlayerSelectUIcallback), Callbacks.ToIntPtr<PlayerSelectUIResponse>(callback, new Func<IntPtr, PlayerSelectUIResponse>(PlayerSelectUIResponse.FromPointer)));
		}

		// Token: 0x06001403 RID: 5123 RVA: 0x00050C4C File Offset: 0x0004EE4C
		[MonoPInvokeCallback(typeof(RealTimeMultiplayerManager.PlayerSelectUICallback))]
		internal static void InternalPlayerSelectUIcallback(IntPtr response, IntPtr data)
		{
			Callbacks.PerformInternalCallback("RealtimeManager#PlayerSelectUICallback", Callbacks.Type.Temporary, response, data);
		}

		// Token: 0x06001404 RID: 5124 RVA: 0x00050C5C File Offset: 0x0004EE5C
		[MonoPInvokeCallback(typeof(RealTimeMultiplayerManager.RealTimeRoomCallback))]
		internal static void InternalRealTimeRoomCallback(IntPtr response, IntPtr data)
		{
			Callbacks.PerformInternalCallback("RealtimeManager#InternalRealTimeRoomCallback", Callbacks.Type.Temporary, response, data);
		}

		// Token: 0x06001405 RID: 5125 RVA: 0x00050C6C File Offset: 0x0004EE6C
		[MonoPInvokeCallback(typeof(RealTimeMultiplayerManager.RoomInboxUICallback))]
		internal static void InternalRoomInboxUICallback(IntPtr response, IntPtr data)
		{
			Callbacks.PerformInternalCallback("RealtimeManager#InternalRoomInboxUICallback", Callbacks.Type.Temporary, response, data);
		}

		// Token: 0x06001406 RID: 5126 RVA: 0x00050C7C File Offset: 0x0004EE7C
		internal void ShowRoomInboxUI(Action<RealtimeManager.RoomInboxUIResponse> callback)
		{
			RealTimeMultiplayerManager.RealTimeMultiplayerManager_ShowRoomInboxUI(this.mGameServices.AsHandle(), new RealTimeMultiplayerManager.RoomInboxUICallback(RealtimeManager.InternalRoomInboxUICallback), Callbacks.ToIntPtr<RealtimeManager.RoomInboxUIResponse>(callback, new Func<IntPtr, RealtimeManager.RoomInboxUIResponse>(RealtimeManager.RoomInboxUIResponse.FromPointer)));
		}

		// Token: 0x06001407 RID: 5127 RVA: 0x00050CB8 File Offset: 0x0004EEB8
		internal void ShowWaitingRoomUI(NativeRealTimeRoom room, uint minimumParticipantsBeforeStarting, Action<RealtimeManager.WaitingRoomUIResponse> callback)
		{
			Misc.CheckNotNull<NativeRealTimeRoom>(room);
			RealTimeMultiplayerManager.RealTimeMultiplayerManager_ShowWaitingRoomUI(this.mGameServices.AsHandle(), room.AsPointer(), minimumParticipantsBeforeStarting, new RealTimeMultiplayerManager.WaitingRoomUICallback(RealtimeManager.InternalWaitingRoomUICallback), Callbacks.ToIntPtr<RealtimeManager.WaitingRoomUIResponse>(callback, new Func<IntPtr, RealtimeManager.WaitingRoomUIResponse>(RealtimeManager.WaitingRoomUIResponse.FromPointer)));
		}

		// Token: 0x06001408 RID: 5128 RVA: 0x00050D04 File Offset: 0x0004EF04
		[MonoPInvokeCallback(typeof(RealTimeMultiplayerManager.WaitingRoomUICallback))]
		internal static void InternalWaitingRoomUICallback(IntPtr response, IntPtr data)
		{
			Callbacks.PerformInternalCallback("RealtimeManager#InternalWaitingRoomUICallback", Callbacks.Type.Temporary, response, data);
		}

		// Token: 0x06001409 RID: 5129 RVA: 0x00050D14 File Offset: 0x0004EF14
		[MonoPInvokeCallback(typeof(RealTimeMultiplayerManager.FetchInvitationsCallback))]
		internal static void InternalFetchInvitationsCallback(IntPtr response, IntPtr data)
		{
			Callbacks.PerformInternalCallback("RealtimeManager#InternalFetchInvitationsCallback", Callbacks.Type.Temporary, response, data);
		}

		// Token: 0x0600140A RID: 5130 RVA: 0x00050D24 File Offset: 0x0004EF24
		internal void FetchInvitations(Action<RealtimeManager.FetchInvitationsResponse> callback)
		{
			RealTimeMultiplayerManager.RealTimeMultiplayerManager_FetchInvitations(this.mGameServices.AsHandle(), new RealTimeMultiplayerManager.FetchInvitationsCallback(RealtimeManager.InternalFetchInvitationsCallback), Callbacks.ToIntPtr<RealtimeManager.FetchInvitationsResponse>(callback, new Func<IntPtr, RealtimeManager.FetchInvitationsResponse>(RealtimeManager.FetchInvitationsResponse.FromPointer)));
		}

		// Token: 0x0600140B RID: 5131 RVA: 0x00050D60 File Offset: 0x0004EF60
		[MonoPInvokeCallback(typeof(RealTimeMultiplayerManager.LeaveRoomCallback))]
		internal static void InternalLeaveRoomCallback(CommonErrorStatus.ResponseStatus response, IntPtr data)
		{
			Logger.d("Entering internal callback for InternalLeaveRoomCallback");
			Action<CommonErrorStatus.ResponseStatus> action = Callbacks.IntPtrToTempCallback<Action<CommonErrorStatus.ResponseStatus>>(data);
			if (action == null)
			{
				return;
			}
			try
			{
				action(response);
			}
			catch (Exception arg)
			{
				Logger.e("Error encountered executing InternalLeaveRoomCallback. Smothering to avoid passing exception into Native: " + arg);
			}
		}

		// Token: 0x0600140C RID: 5132 RVA: 0x00050DC4 File Offset: 0x0004EFC4
		internal void LeaveRoom(NativeRealTimeRoom room, Action<CommonErrorStatus.ResponseStatus> callback)
		{
			RealTimeMultiplayerManager.RealTimeMultiplayerManager_LeaveRoom(this.mGameServices.AsHandle(), room.AsPointer(), new RealTimeMultiplayerManager.LeaveRoomCallback(RealtimeManager.InternalLeaveRoomCallback), Callbacks.ToIntPtr(callback));
		}

		// Token: 0x0600140D RID: 5133 RVA: 0x00050DFC File Offset: 0x0004EFFC
		internal void AcceptInvitation(MultiplayerInvitation invitation, RealTimeEventListenerHelper listener, Action<RealtimeManager.RealTimeRoomResponse> callback)
		{
			RealTimeMultiplayerManager.RealTimeMultiplayerManager_AcceptInvitation(this.mGameServices.AsHandle(), invitation.AsPointer(), listener.AsPointer(), new RealTimeMultiplayerManager.RealTimeRoomCallback(RealtimeManager.InternalRealTimeRoomCallback), RealtimeManager.ToCallbackPointer(callback));
		}

		// Token: 0x0600140E RID: 5134 RVA: 0x00050E38 File Offset: 0x0004F038
		internal void DeclineInvitation(MultiplayerInvitation invitation)
		{
			RealTimeMultiplayerManager.RealTimeMultiplayerManager_DeclineInvitation(this.mGameServices.AsHandle(), invitation.AsPointer());
		}

		// Token: 0x0600140F RID: 5135 RVA: 0x00050E50 File Offset: 0x0004F050
		internal void SendReliableMessage(NativeRealTimeRoom room, MultiplayerParticipant participant, byte[] data, Action<CommonErrorStatus.MultiplayerStatus> callback)
		{
			RealTimeMultiplayerManager.RealTimeMultiplayerManager_SendReliableMessage(this.mGameServices.AsHandle(), room.AsPointer(), participant.AsPointer(), data, PInvokeUtilities.ArrayToSizeT<byte>(data), new RealTimeMultiplayerManager.SendReliableMessageCallback(RealtimeManager.InternalSendReliableMessageCallback), Callbacks.ToIntPtr(callback));
		}

		// Token: 0x06001410 RID: 5136 RVA: 0x00050E94 File Offset: 0x0004F094
		[MonoPInvokeCallback(typeof(RealTimeMultiplayerManager.SendReliableMessageCallback))]
		internal static void InternalSendReliableMessageCallback(CommonErrorStatus.MultiplayerStatus response, IntPtr data)
		{
			Logger.d("Entering internal callback for InternalSendReliableMessageCallback " + response);
			Action<CommonErrorStatus.MultiplayerStatus> action = Callbacks.IntPtrToTempCallback<Action<CommonErrorStatus.MultiplayerStatus>>(data);
			if (action == null)
			{
				return;
			}
			try
			{
				action(response);
			}
			catch (Exception arg)
			{
				Logger.e("Error encountered executing InternalSendReliableMessageCallback. Smothering to avoid passing exception into Native: " + arg);
			}
		}

		// Token: 0x06001411 RID: 5137 RVA: 0x00050F04 File Offset: 0x0004F104
		internal void SendUnreliableMessageToAll(NativeRealTimeRoom room, byte[] data)
		{
			RealTimeMultiplayerManager.RealTimeMultiplayerManager_SendUnreliableMessageToOthers(this.mGameServices.AsHandle(), room.AsPointer(), data, PInvokeUtilities.ArrayToSizeT<byte>(data));
		}

		// Token: 0x06001412 RID: 5138 RVA: 0x00050F30 File Offset: 0x0004F130
		internal void SendUnreliableMessageToSpecificParticipants(NativeRealTimeRoom room, List<MultiplayerParticipant> recipients, byte[] data)
		{
			RealTimeMultiplayerManager.RealTimeMultiplayerManager_SendUnreliableMessage(this.mGameServices.AsHandle(), room.AsPointer(), (from r in recipients
			select r.AsPointer()).ToArray<IntPtr>(), new UIntPtr((ulong)recipients.LongCount<MultiplayerParticipant>()), data, PInvokeUtilities.ArrayToSizeT<byte>(data));
		}

		// Token: 0x06001413 RID: 5139 RVA: 0x00050F90 File Offset: 0x0004F190
		private static IntPtr ToCallbackPointer(Action<RealtimeManager.RealTimeRoomResponse> callback)
		{
			return Callbacks.ToIntPtr<RealtimeManager.RealTimeRoomResponse>(callback, new Func<IntPtr, RealtimeManager.RealTimeRoomResponse>(RealtimeManager.RealTimeRoomResponse.FromPointer));
		}

		// Token: 0x04000C0E RID: 3086
		private readonly GameServices mGameServices;

		// Token: 0x02000271 RID: 625
		internal class RealTimeRoomResponse : BaseReferenceHolder
		{
			// Token: 0x06001415 RID: 5141 RVA: 0x00050FAC File Offset: 0x0004F1AC
			internal RealTimeRoomResponse(IntPtr selfPointer) : base(selfPointer)
			{
			}

			// Token: 0x06001416 RID: 5142 RVA: 0x00050FB8 File Offset: 0x0004F1B8
			internal CommonErrorStatus.MultiplayerStatus ResponseStatus()
			{
				return RealTimeMultiplayerManager.RealTimeMultiplayerManager_RealTimeRoomResponse_GetStatus(base.SelfPtr());
			}

			// Token: 0x06001417 RID: 5143 RVA: 0x00050FC8 File Offset: 0x0004F1C8
			internal bool RequestSucceeded()
			{
				return this.ResponseStatus() > (CommonErrorStatus.MultiplayerStatus)0;
			}

			// Token: 0x06001418 RID: 5144 RVA: 0x00050FD4 File Offset: 0x0004F1D4
			internal NativeRealTimeRoom Room()
			{
				if (!this.RequestSucceeded())
				{
					return null;
				}
				return new NativeRealTimeRoom(RealTimeMultiplayerManager.RealTimeMultiplayerManager_RealTimeRoomResponse_GetRoom(base.SelfPtr()));
			}

			// Token: 0x06001419 RID: 5145 RVA: 0x00050FF4 File Offset: 0x0004F1F4
			protected override void CallDispose(HandleRef selfPointer)
			{
				RealTimeMultiplayerManager.RealTimeMultiplayerManager_RealTimeRoomResponse_Dispose(selfPointer);
			}

			// Token: 0x0600141A RID: 5146 RVA: 0x00050FFC File Offset: 0x0004F1FC
			internal static RealtimeManager.RealTimeRoomResponse FromPointer(IntPtr pointer)
			{
				if (pointer.Equals(IntPtr.Zero))
				{
					return null;
				}
				return new RealtimeManager.RealTimeRoomResponse(pointer);
			}
		}

		// Token: 0x02000272 RID: 626
		internal class RoomInboxUIResponse : BaseReferenceHolder
		{
			// Token: 0x0600141B RID: 5147 RVA: 0x0005101C File Offset: 0x0004F21C
			internal RoomInboxUIResponse(IntPtr selfPointer) : base(selfPointer)
			{
			}

			// Token: 0x0600141C RID: 5148 RVA: 0x00051028 File Offset: 0x0004F228
			internal CommonErrorStatus.UIStatus ResponseStatus()
			{
				return RealTimeMultiplayerManager.RealTimeMultiplayerManager_RoomInboxUIResponse_GetStatus(base.SelfPtr());
			}

			// Token: 0x0600141D RID: 5149 RVA: 0x00051038 File Offset: 0x0004F238
			internal MultiplayerInvitation Invitation()
			{
				if (this.ResponseStatus() != CommonErrorStatus.UIStatus.VALID)
				{
					return null;
				}
				return new MultiplayerInvitation(RealTimeMultiplayerManager.RealTimeMultiplayerManager_RoomInboxUIResponse_GetInvitation(base.SelfPtr()));
			}

			// Token: 0x0600141E RID: 5150 RVA: 0x00051058 File Offset: 0x0004F258
			protected override void CallDispose(HandleRef selfPointer)
			{
				RealTimeMultiplayerManager.RealTimeMultiplayerManager_RoomInboxUIResponse_Dispose(selfPointer);
			}

			// Token: 0x0600141F RID: 5151 RVA: 0x00051060 File Offset: 0x0004F260
			internal static RealtimeManager.RoomInboxUIResponse FromPointer(IntPtr pointer)
			{
				if (PInvokeUtilities.IsNull(pointer))
				{
					return null;
				}
				return new RealtimeManager.RoomInboxUIResponse(pointer);
			}
		}

		// Token: 0x02000273 RID: 627
		internal class WaitingRoomUIResponse : BaseReferenceHolder
		{
			// Token: 0x06001420 RID: 5152 RVA: 0x00051078 File Offset: 0x0004F278
			internal WaitingRoomUIResponse(IntPtr selfPointer) : base(selfPointer)
			{
			}

			// Token: 0x06001421 RID: 5153 RVA: 0x00051084 File Offset: 0x0004F284
			internal CommonErrorStatus.UIStatus ResponseStatus()
			{
				return RealTimeMultiplayerManager.RealTimeMultiplayerManager_WaitingRoomUIResponse_GetStatus(base.SelfPtr());
			}

			// Token: 0x06001422 RID: 5154 RVA: 0x00051094 File Offset: 0x0004F294
			internal NativeRealTimeRoom Room()
			{
				if (this.ResponseStatus() != CommonErrorStatus.UIStatus.VALID)
				{
					return null;
				}
				return new NativeRealTimeRoom(RealTimeMultiplayerManager.RealTimeMultiplayerManager_WaitingRoomUIResponse_GetRoom(base.SelfPtr()));
			}

			// Token: 0x06001423 RID: 5155 RVA: 0x000510B4 File Offset: 0x0004F2B4
			protected override void CallDispose(HandleRef selfPointer)
			{
				RealTimeMultiplayerManager.RealTimeMultiplayerManager_WaitingRoomUIResponse_Dispose(selfPointer);
			}

			// Token: 0x06001424 RID: 5156 RVA: 0x000510BC File Offset: 0x0004F2BC
			internal static RealtimeManager.WaitingRoomUIResponse FromPointer(IntPtr pointer)
			{
				if (PInvokeUtilities.IsNull(pointer))
				{
					return null;
				}
				return new RealtimeManager.WaitingRoomUIResponse(pointer);
			}
		}

		// Token: 0x02000274 RID: 628
		internal class FetchInvitationsResponse : BaseReferenceHolder
		{
			// Token: 0x06001425 RID: 5157 RVA: 0x000510D4 File Offset: 0x0004F2D4
			internal FetchInvitationsResponse(IntPtr selfPointer) : base(selfPointer)
			{
			}

			// Token: 0x06001426 RID: 5158 RVA: 0x000510E0 File Offset: 0x0004F2E0
			internal bool RequestSucceeded()
			{
				return this.ResponseStatus() > (CommonErrorStatus.ResponseStatus)0;
			}

			// Token: 0x06001427 RID: 5159 RVA: 0x000510EC File Offset: 0x0004F2EC
			internal CommonErrorStatus.ResponseStatus ResponseStatus()
			{
				return RealTimeMultiplayerManager.RealTimeMultiplayerManager_FetchInvitationsResponse_GetStatus(base.SelfPtr());
			}

			// Token: 0x06001428 RID: 5160 RVA: 0x000510FC File Offset: 0x0004F2FC
			internal IEnumerable<MultiplayerInvitation> Invitations()
			{
				return PInvokeUtilities.ToEnumerable<MultiplayerInvitation>(RealTimeMultiplayerManager.RealTimeMultiplayerManager_FetchInvitationsResponse_GetInvitations_Length(base.SelfPtr()), (UIntPtr index) => new MultiplayerInvitation(RealTimeMultiplayerManager.RealTimeMultiplayerManager_FetchInvitationsResponse_GetInvitations_GetElement(base.SelfPtr(), index)));
			}

			// Token: 0x06001429 RID: 5161 RVA: 0x0005111C File Offset: 0x0004F31C
			protected override void CallDispose(HandleRef selfPointer)
			{
				RealTimeMultiplayerManager.RealTimeMultiplayerManager_FetchInvitationsResponse_Dispose(selfPointer);
			}

			// Token: 0x0600142A RID: 5162 RVA: 0x00051124 File Offset: 0x0004F324
			internal static RealtimeManager.FetchInvitationsResponse FromPointer(IntPtr pointer)
			{
				if (PInvokeUtilities.IsNull(pointer))
				{
					return null;
				}
				return new RealtimeManager.FetchInvitationsResponse(pointer);
			}
		}
	}
}
