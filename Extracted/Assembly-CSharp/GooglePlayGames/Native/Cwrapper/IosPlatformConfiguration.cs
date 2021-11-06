using System;
using System.Runtime.InteropServices;

namespace GooglePlayGames.Native.Cwrapper
{
	// Token: 0x020001D3 RID: 467
	internal static class IosPlatformConfiguration
	{
		// Token: 0x06000EF1 RID: 3825
		[DllImport("gpg")]
		internal static extern IntPtr IosPlatformConfiguration_Construct();

		// Token: 0x06000EF2 RID: 3826
		[DllImport("gpg")]
		internal static extern void IosPlatformConfiguration_Dispose(HandleRef self);

		// Token: 0x06000EF3 RID: 3827
		[DllImport("gpg")]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool IosPlatformConfiguration_Valid(HandleRef self);

		// Token: 0x06000EF4 RID: 3828
		[DllImport("gpg")]
		internal static extern void IosPlatformConfiguration_SetClientID(HandleRef self, string client_id);
	}
}
