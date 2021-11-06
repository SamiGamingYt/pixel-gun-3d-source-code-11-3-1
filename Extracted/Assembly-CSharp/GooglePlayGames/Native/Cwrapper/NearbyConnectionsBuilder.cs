using System;
using System.Runtime.InteropServices;

namespace GooglePlayGames.Native.Cwrapper
{
	// Token: 0x020001DC RID: 476
	internal static class NearbyConnectionsBuilder
	{
		// Token: 0x06000F50 RID: 3920
		[DllImport("gpg")]
		internal static extern void NearbyConnections_Builder_SetOnInitializationFinished(HandleRef self, NearbyConnectionsBuilder.OnInitializationFinishedCallback callback, IntPtr callback_arg);

		// Token: 0x06000F51 RID: 3921
		[DllImport("gpg")]
		internal static extern IntPtr NearbyConnections_Builder_Construct();

		// Token: 0x06000F52 RID: 3922
		[DllImport("gpg")]
		internal static extern void NearbyConnections_Builder_SetClientId(HandleRef self, long client_id);

		// Token: 0x06000F53 RID: 3923
		[DllImport("gpg")]
		internal static extern void NearbyConnections_Builder_SetOnLog(HandleRef self, NearbyConnectionsBuilder.OnLogCallback callback, IntPtr callback_arg, Types.LogLevel min_level);

		// Token: 0x06000F54 RID: 3924
		[DllImport("gpg")]
		internal static extern void NearbyConnections_Builder_SetDefaultOnLog(HandleRef self, Types.LogLevel min_level);

		// Token: 0x06000F55 RID: 3925
		[DllImport("gpg")]
		internal static extern IntPtr NearbyConnections_Builder_Create(HandleRef self, IntPtr platform);

		// Token: 0x06000F56 RID: 3926
		[DllImport("gpg")]
		internal static extern void NearbyConnections_Builder_Dispose(HandleRef self);

		// Token: 0x020008BB RID: 2235
		// (Invoke) Token: 0x06004F8C RID: 20364
		internal delegate void OnInitializationFinishedCallback(NearbyConnectionsStatus.InitializationStatus arg0, IntPtr arg1);

		// Token: 0x020008BC RID: 2236
		// (Invoke) Token: 0x06004F90 RID: 20368
		internal delegate void OnLogCallback(Types.LogLevel arg0, string arg1, IntPtr arg2);
	}
}
