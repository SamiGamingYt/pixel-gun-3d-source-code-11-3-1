using System;
using System.Runtime.InteropServices;

namespace GooglePlayGames.Native.Cwrapper
{
	// Token: 0x020001E6 RID: 486
	internal static class RealTimeEventListenerHelper
	{
		// Token: 0x06000FB6 RID: 4022
		[DllImport("gpg")]
		internal static extern void RealTimeEventListenerHelper_SetOnParticipantStatusChangedCallback(HandleRef self, RealTimeEventListenerHelper.OnParticipantStatusChangedCallback callback, IntPtr callback_arg);

		// Token: 0x06000FB7 RID: 4023
		[DllImport("gpg")]
		internal static extern IntPtr RealTimeEventListenerHelper_Construct();

		// Token: 0x06000FB8 RID: 4024
		[DllImport("gpg")]
		internal static extern void RealTimeEventListenerHelper_SetOnP2PDisconnectedCallback(HandleRef self, RealTimeEventListenerHelper.OnP2PDisconnectedCallback callback, IntPtr callback_arg);

		// Token: 0x06000FB9 RID: 4025
		[DllImport("gpg")]
		internal static extern void RealTimeEventListenerHelper_SetOnDataReceivedCallback(HandleRef self, RealTimeEventListenerHelper.OnDataReceivedCallback callback, IntPtr callback_arg);

		// Token: 0x06000FBA RID: 4026
		[DllImport("gpg")]
		internal static extern void RealTimeEventListenerHelper_SetOnRoomStatusChangedCallback(HandleRef self, RealTimeEventListenerHelper.OnRoomStatusChangedCallback callback, IntPtr callback_arg);

		// Token: 0x06000FBB RID: 4027
		[DllImport("gpg")]
		internal static extern void RealTimeEventListenerHelper_SetOnP2PConnectedCallback(HandleRef self, RealTimeEventListenerHelper.OnP2PConnectedCallback callback, IntPtr callback_arg);

		// Token: 0x06000FBC RID: 4028
		[DllImport("gpg")]
		internal static extern void RealTimeEventListenerHelper_SetOnRoomConnectedSetChangedCallback(HandleRef self, RealTimeEventListenerHelper.OnRoomConnectedSetChangedCallback callback, IntPtr callback_arg);

		// Token: 0x06000FBD RID: 4029
		[DllImport("gpg")]
		internal static extern void RealTimeEventListenerHelper_Dispose(HandleRef self);

		// Token: 0x020008C5 RID: 2245
		// (Invoke) Token: 0x06004FB4 RID: 20404
		internal delegate void OnRoomStatusChangedCallback(IntPtr arg0, IntPtr arg1);

		// Token: 0x020008C6 RID: 2246
		// (Invoke) Token: 0x06004FB8 RID: 20408
		internal delegate void OnRoomConnectedSetChangedCallback(IntPtr arg0, IntPtr arg1);

		// Token: 0x020008C7 RID: 2247
		// (Invoke) Token: 0x06004FBC RID: 20412
		internal delegate void OnP2PConnectedCallback(IntPtr arg0, IntPtr arg1, IntPtr arg2);

		// Token: 0x020008C8 RID: 2248
		// (Invoke) Token: 0x06004FC0 RID: 20416
		internal delegate void OnP2PDisconnectedCallback(IntPtr arg0, IntPtr arg1, IntPtr arg2);

		// Token: 0x020008C9 RID: 2249
		// (Invoke) Token: 0x06004FC4 RID: 20420
		internal delegate void OnParticipantStatusChangedCallback(IntPtr arg0, IntPtr arg1, IntPtr arg2);

		// Token: 0x020008CA RID: 2250
		// (Invoke) Token: 0x06004FC8 RID: 20424
		internal delegate void OnDataReceivedCallback(IntPtr arg0, IntPtr arg1, IntPtr arg2, UIntPtr arg3, [MarshalAs(UnmanagedType.I1)] bool arg4, IntPtr arg5);
	}
}
