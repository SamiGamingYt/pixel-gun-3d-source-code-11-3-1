using System;
using System.Runtime.InteropServices;

namespace GooglePlayGames.Native.Cwrapper
{
	// Token: 0x020001D2 RID: 466
	internal static class InternalHooks
	{
		// Token: 0x06000EEF RID: 3823
		[DllImport("gpg")]
		internal static extern void InternalHooks_ConfigureForUnityPlugin(HandleRef builder);

		// Token: 0x06000EF0 RID: 3824
		[DllImport("gpg")]
		internal static extern IntPtr InternalHooks_GetApiClient(HandleRef services);
	}
}
