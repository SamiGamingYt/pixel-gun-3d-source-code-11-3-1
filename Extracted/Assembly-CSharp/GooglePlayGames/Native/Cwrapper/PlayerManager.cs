using System;
using System.Runtime.InteropServices;

namespace GooglePlayGames.Native.Cwrapper
{
	// Token: 0x020001E1 RID: 481
	internal static class PlayerManager
	{
		// Token: 0x06000F68 RID: 3944
		[DllImport("gpg")]
		internal static extern void PlayerManager_FetchInvitable(HandleRef self, Types.DataSource data_source, PlayerManager.FetchListCallback callback, IntPtr callback_arg);

		// Token: 0x06000F69 RID: 3945
		[DllImport("gpg")]
		internal static extern void PlayerManager_FetchConnected(HandleRef self, Types.DataSource data_source, PlayerManager.FetchListCallback callback, IntPtr callback_arg);

		// Token: 0x06000F6A RID: 3946
		[DllImport("gpg")]
		internal static extern void PlayerManager_Fetch(HandleRef self, Types.DataSource data_source, string player_id, PlayerManager.FetchCallback callback, IntPtr callback_arg);

		// Token: 0x06000F6B RID: 3947
		[DllImport("gpg")]
		internal static extern void PlayerManager_FetchRecentlyPlayed(HandleRef self, Types.DataSource data_source, PlayerManager.FetchListCallback callback, IntPtr callback_arg);

		// Token: 0x06000F6C RID: 3948
		[DllImport("gpg")]
		internal static extern void PlayerManager_FetchSelf(HandleRef self, Types.DataSource data_source, PlayerManager.FetchSelfCallback callback, IntPtr callback_arg);

		// Token: 0x06000F6D RID: 3949
		[DllImport("gpg")]
		internal static extern void PlayerManager_FetchSelfResponse_Dispose(HandleRef self);

		// Token: 0x06000F6E RID: 3950
		[DllImport("gpg")]
		internal static extern CommonErrorStatus.ResponseStatus PlayerManager_FetchSelfResponse_GetStatus(HandleRef self);

		// Token: 0x06000F6F RID: 3951
		[DllImport("gpg")]
		internal static extern IntPtr PlayerManager_FetchSelfResponse_GetData(HandleRef self);

		// Token: 0x06000F70 RID: 3952
		[DllImport("gpg")]
		internal static extern void PlayerManager_FetchResponse_Dispose(HandleRef self);

		// Token: 0x06000F71 RID: 3953
		[DllImport("gpg")]
		internal static extern CommonErrorStatus.ResponseStatus PlayerManager_FetchResponse_GetStatus(HandleRef self);

		// Token: 0x06000F72 RID: 3954
		[DllImport("gpg")]
		internal static extern IntPtr PlayerManager_FetchResponse_GetData(HandleRef self);

		// Token: 0x06000F73 RID: 3955
		[DllImport("gpg")]
		internal static extern void PlayerManager_FetchListResponse_Dispose(HandleRef self);

		// Token: 0x06000F74 RID: 3956
		[DllImport("gpg")]
		internal static extern CommonErrorStatus.ResponseStatus PlayerManager_FetchListResponse_GetStatus(HandleRef self);

		// Token: 0x06000F75 RID: 3957
		[DllImport("gpg")]
		internal static extern UIntPtr PlayerManager_FetchListResponse_GetData_Length(HandleRef self);

		// Token: 0x06000F76 RID: 3958
		[DllImport("gpg")]
		internal static extern IntPtr PlayerManager_FetchListResponse_GetData_GetElement(HandleRef self, UIntPtr index);

		// Token: 0x020008BD RID: 2237
		// (Invoke) Token: 0x06004F94 RID: 20372
		internal delegate void FetchSelfCallback(IntPtr arg0, IntPtr arg1);

		// Token: 0x020008BE RID: 2238
		// (Invoke) Token: 0x06004F98 RID: 20376
		internal delegate void FetchCallback(IntPtr arg0, IntPtr arg1);

		// Token: 0x020008BF RID: 2239
		// (Invoke) Token: 0x06004F9C RID: 20380
		internal delegate void FetchListCallback(IntPtr arg0, IntPtr arg1);
	}
}
