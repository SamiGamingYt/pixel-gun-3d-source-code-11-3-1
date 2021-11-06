using System;
using System.Runtime.InteropServices;

namespace GooglePlayGames.Native.Cwrapper
{
	// Token: 0x020001F3 RID: 499
	internal static class StatsManager
	{
		// Token: 0x06001045 RID: 4165
		[DllImport("gpg")]
		internal static extern void StatsManager_FetchForPlayer(HandleRef self, Types.DataSource data_source, StatsManager.FetchForPlayerCallback callback, IntPtr callback_arg);

		// Token: 0x06001046 RID: 4166
		[DllImport("gpg")]
		internal static extern void StatsManager_FetchForPlayerResponse_Dispose(HandleRef self);

		// Token: 0x06001047 RID: 4167
		[DllImport("gpg")]
		internal static extern CommonErrorStatus.ResponseStatus StatsManager_FetchForPlayerResponse_GetStatus(HandleRef self);

		// Token: 0x06001048 RID: 4168
		[DllImport("gpg")]
		internal static extern IntPtr StatsManager_FetchForPlayerResponse_GetData(HandleRef self);

		// Token: 0x020008D7 RID: 2263
		// (Invoke) Token: 0x06004FFC RID: 20476
		internal delegate void FetchForPlayerCallback(IntPtr arg0, IntPtr arg1);
	}
}
