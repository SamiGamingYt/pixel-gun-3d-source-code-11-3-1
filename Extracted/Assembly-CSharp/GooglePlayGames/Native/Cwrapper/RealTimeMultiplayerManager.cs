using System;
using System.Runtime.InteropServices;

namespace GooglePlayGames.Native.Cwrapper
{
	// Token: 0x020001E7 RID: 487
	internal static class RealTimeMultiplayerManager
	{
		// Token: 0x06000FBE RID: 4030
		[DllImport("gpg")]
		internal static extern void RealTimeMultiplayerManager_CreateRealTimeRoom(HandleRef self, IntPtr config, IntPtr helper, RealTimeMultiplayerManager.RealTimeRoomCallback callback, IntPtr callback_arg);

		// Token: 0x06000FBF RID: 4031
		[DllImport("gpg")]
		internal static extern void RealTimeMultiplayerManager_LeaveRoom(HandleRef self, IntPtr room, RealTimeMultiplayerManager.LeaveRoomCallback callback, IntPtr callback_arg);

		// Token: 0x06000FC0 RID: 4032
		[DllImport("gpg")]
		internal static extern void RealTimeMultiplayerManager_SendUnreliableMessage(HandleRef self, IntPtr room, IntPtr[] participants, UIntPtr participants_size, byte[] data, UIntPtr data_size);

		// Token: 0x06000FC1 RID: 4033
		[DllImport("gpg")]
		internal static extern void RealTimeMultiplayerManager_ShowWaitingRoomUI(HandleRef self, IntPtr room, uint min_participants_to_start, RealTimeMultiplayerManager.WaitingRoomUICallback callback, IntPtr callback_arg);

		// Token: 0x06000FC2 RID: 4034
		[DllImport("gpg")]
		internal static extern void RealTimeMultiplayerManager_ShowPlayerSelectUI(HandleRef self, uint minimum_players, uint maximum_players, [MarshalAs(UnmanagedType.I1)] bool allow_automatch, RealTimeMultiplayerManager.PlayerSelectUICallback callback, IntPtr callback_arg);

		// Token: 0x06000FC3 RID: 4035
		[DllImport("gpg")]
		internal static extern void RealTimeMultiplayerManager_DismissInvitation(HandleRef self, IntPtr invitation);

		// Token: 0x06000FC4 RID: 4036
		[DllImport("gpg")]
		internal static extern void RealTimeMultiplayerManager_DeclineInvitation(HandleRef self, IntPtr invitation);

		// Token: 0x06000FC5 RID: 4037
		[DllImport("gpg")]
		internal static extern void RealTimeMultiplayerManager_SendReliableMessage(HandleRef self, IntPtr room, IntPtr participant, byte[] data, UIntPtr data_size, RealTimeMultiplayerManager.SendReliableMessageCallback callback, IntPtr callback_arg);

		// Token: 0x06000FC6 RID: 4038
		[DllImport("gpg")]
		internal static extern void RealTimeMultiplayerManager_AcceptInvitation(HandleRef self, IntPtr invitation, IntPtr helper, RealTimeMultiplayerManager.RealTimeRoomCallback callback, IntPtr callback_arg);

		// Token: 0x06000FC7 RID: 4039
		[DllImport("gpg")]
		internal static extern void RealTimeMultiplayerManager_FetchInvitations(HandleRef self, RealTimeMultiplayerManager.FetchInvitationsCallback callback, IntPtr callback_arg);

		// Token: 0x06000FC8 RID: 4040
		[DllImport("gpg")]
		internal static extern void RealTimeMultiplayerManager_SendUnreliableMessageToOthers(HandleRef self, IntPtr room, byte[] data, UIntPtr data_size);

		// Token: 0x06000FC9 RID: 4041
		[DllImport("gpg")]
		internal static extern void RealTimeMultiplayerManager_ShowRoomInboxUI(HandleRef self, RealTimeMultiplayerManager.RoomInboxUICallback callback, IntPtr callback_arg);

		// Token: 0x06000FCA RID: 4042
		[DllImport("gpg")]
		internal static extern void RealTimeMultiplayerManager_RealTimeRoomResponse_Dispose(HandleRef self);

		// Token: 0x06000FCB RID: 4043
		[DllImport("gpg")]
		internal static extern CommonErrorStatus.MultiplayerStatus RealTimeMultiplayerManager_RealTimeRoomResponse_GetStatus(HandleRef self);

		// Token: 0x06000FCC RID: 4044
		[DllImport("gpg")]
		internal static extern IntPtr RealTimeMultiplayerManager_RealTimeRoomResponse_GetRoom(HandleRef self);

		// Token: 0x06000FCD RID: 4045
		[DllImport("gpg")]
		internal static extern void RealTimeMultiplayerManager_RoomInboxUIResponse_Dispose(HandleRef self);

		// Token: 0x06000FCE RID: 4046
		[DllImport("gpg")]
		internal static extern CommonErrorStatus.UIStatus RealTimeMultiplayerManager_RoomInboxUIResponse_GetStatus(HandleRef self);

		// Token: 0x06000FCF RID: 4047
		[DllImport("gpg")]
		internal static extern IntPtr RealTimeMultiplayerManager_RoomInboxUIResponse_GetInvitation(HandleRef self);

		// Token: 0x06000FD0 RID: 4048
		[DllImport("gpg")]
		internal static extern void RealTimeMultiplayerManager_WaitingRoomUIResponse_Dispose(HandleRef self);

		// Token: 0x06000FD1 RID: 4049
		[DllImport("gpg")]
		internal static extern CommonErrorStatus.UIStatus RealTimeMultiplayerManager_WaitingRoomUIResponse_GetStatus(HandleRef self);

		// Token: 0x06000FD2 RID: 4050
		[DllImport("gpg")]
		internal static extern IntPtr RealTimeMultiplayerManager_WaitingRoomUIResponse_GetRoom(HandleRef self);

		// Token: 0x06000FD3 RID: 4051
		[DllImport("gpg")]
		internal static extern void RealTimeMultiplayerManager_FetchInvitationsResponse_Dispose(HandleRef self);

		// Token: 0x06000FD4 RID: 4052
		[DllImport("gpg")]
		internal static extern CommonErrorStatus.ResponseStatus RealTimeMultiplayerManager_FetchInvitationsResponse_GetStatus(HandleRef self);

		// Token: 0x06000FD5 RID: 4053
		[DllImport("gpg")]
		internal static extern UIntPtr RealTimeMultiplayerManager_FetchInvitationsResponse_GetInvitations_Length(HandleRef self);

		// Token: 0x06000FD6 RID: 4054
		[DllImport("gpg")]
		internal static extern IntPtr RealTimeMultiplayerManager_FetchInvitationsResponse_GetInvitations_GetElement(HandleRef self, UIntPtr index);

		// Token: 0x020008CB RID: 2251
		// (Invoke) Token: 0x06004FCC RID: 20428
		internal delegate void RealTimeRoomCallback(IntPtr arg0, IntPtr arg1);

		// Token: 0x020008CC RID: 2252
		// (Invoke) Token: 0x06004FD0 RID: 20432
		internal delegate void LeaveRoomCallback(CommonErrorStatus.ResponseStatus arg0, IntPtr arg1);

		// Token: 0x020008CD RID: 2253
		// (Invoke) Token: 0x06004FD4 RID: 20436
		internal delegate void SendReliableMessageCallback(CommonErrorStatus.MultiplayerStatus arg0, IntPtr arg1);

		// Token: 0x020008CE RID: 2254
		// (Invoke) Token: 0x06004FD8 RID: 20440
		internal delegate void RoomInboxUICallback(IntPtr arg0, IntPtr arg1);

		// Token: 0x020008CF RID: 2255
		// (Invoke) Token: 0x06004FDC RID: 20444
		internal delegate void PlayerSelectUICallback(IntPtr arg0, IntPtr arg1);

		// Token: 0x020008D0 RID: 2256
		// (Invoke) Token: 0x06004FE0 RID: 20448
		internal delegate void WaitingRoomUICallback(IntPtr arg0, IntPtr arg1);

		// Token: 0x020008D1 RID: 2257
		// (Invoke) Token: 0x06004FE4 RID: 20452
		internal delegate void FetchInvitationsCallback(IntPtr arg0, IntPtr arg1);
	}
}
