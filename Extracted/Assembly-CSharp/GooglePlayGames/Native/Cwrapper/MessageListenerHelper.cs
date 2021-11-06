using System;
using System.Runtime.InteropServices;

namespace GooglePlayGames.Native.Cwrapper
{
	// Token: 0x020001D6 RID: 470
	internal static class MessageListenerHelper
	{
		// Token: 0x06000F15 RID: 3861
		[DllImport("gpg")]
		internal static extern void MessageListenerHelper_SetOnMessageReceivedCallback(HandleRef self, MessageListenerHelper.OnMessageReceivedCallback callback, IntPtr callback_arg);

		// Token: 0x06000F16 RID: 3862
		[DllImport("gpg")]
		internal static extern void MessageListenerHelper_SetOnDisconnectedCallback(HandleRef self, MessageListenerHelper.OnDisconnectedCallback callback, IntPtr callback_arg);

		// Token: 0x06000F17 RID: 3863
		[DllImport("gpg")]
		internal static extern IntPtr MessageListenerHelper_Construct();

		// Token: 0x06000F18 RID: 3864
		[DllImport("gpg")]
		internal static extern void MessageListenerHelper_Dispose(HandleRef self);

		// Token: 0x020008B6 RID: 2230
		// (Invoke) Token: 0x06004F78 RID: 20344
		internal delegate void OnMessageReceivedCallback(long arg0, string arg1, IntPtr arg2, UIntPtr arg3, [MarshalAs(UnmanagedType.I1)] bool arg4, IntPtr arg5);

		// Token: 0x020008B7 RID: 2231
		// (Invoke) Token: 0x06004F7C RID: 20348
		internal delegate void OnDisconnectedCallback(long arg0, string arg1, IntPtr arg2);
	}
}
