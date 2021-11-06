using System;
using System.Runtime.InteropServices;

namespace GooglePlayGames.Native.Cwrapper
{
	// Token: 0x020001CE RID: 462
	internal static class EndpointDiscoveryListenerHelper
	{
		// Token: 0x06000ED0 RID: 3792
		[DllImport("gpg")]
		internal static extern IntPtr EndpointDiscoveryListenerHelper_Construct();

		// Token: 0x06000ED1 RID: 3793
		[DllImport("gpg")]
		internal static extern void EndpointDiscoveryListenerHelper_SetOnEndpointLostCallback(HandleRef self, EndpointDiscoveryListenerHelper.OnEndpointLostCallback callback, IntPtr callback_arg);

		// Token: 0x06000ED2 RID: 3794
		[DllImport("gpg")]
		internal static extern void EndpointDiscoveryListenerHelper_SetOnEndpointFoundCallback(HandleRef self, EndpointDiscoveryListenerHelper.OnEndpointFoundCallback callback, IntPtr callback_arg);

		// Token: 0x06000ED3 RID: 3795
		[DllImport("gpg")]
		internal static extern void EndpointDiscoveryListenerHelper_Dispose(HandleRef self);

		// Token: 0x020008A9 RID: 2217
		// (Invoke) Token: 0x06004F44 RID: 20292
		internal delegate void OnEndpointFoundCallback(long arg0, IntPtr arg1, IntPtr arg2);

		// Token: 0x020008AA RID: 2218
		// (Invoke) Token: 0x06004F48 RID: 20296
		internal delegate void OnEndpointLostCallback(long arg0, string arg1, IntPtr arg2);
	}
}
