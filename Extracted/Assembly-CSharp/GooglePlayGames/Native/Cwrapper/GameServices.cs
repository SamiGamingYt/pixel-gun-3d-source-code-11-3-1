using System;
using System.Runtime.InteropServices;
using System.Text;

namespace GooglePlayGames.Native.Cwrapper
{
	// Token: 0x020001D1 RID: 465
	internal static class GameServices
	{
		// Token: 0x06000EE6 RID: 3814
		[DllImport("gpg")]
		internal static extern void GameServices_Flush(HandleRef self, GameServices.FlushCallback callback, IntPtr callback_arg);

		// Token: 0x06000EE7 RID: 3815
		[DllImport("gpg")]
		internal static extern void GameServices_FetchServerAuthCode(HandleRef self, string server_client_id, GameServices.FetchServerAuthCodeCallback callback, IntPtr callback_arg);

		// Token: 0x06000EE8 RID: 3816
		[DllImport("gpg")]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool GameServices_IsAuthorized(HandleRef self);

		// Token: 0x06000EE9 RID: 3817
		[DllImport("gpg")]
		internal static extern void GameServices_Dispose(HandleRef self);

		// Token: 0x06000EEA RID: 3818
		[DllImport("gpg")]
		internal static extern void GameServices_SignOut(HandleRef self);

		// Token: 0x06000EEB RID: 3819
		[DllImport("gpg")]
		internal static extern void GameServices_StartAuthorizationUI(HandleRef self);

		// Token: 0x06000EEC RID: 3820
		[DllImport("gpg")]
		internal static extern void GameServices_FetchServerAuthCodeResponse_Dispose(HandleRef self);

		// Token: 0x06000EED RID: 3821
		[DllImport("gpg")]
		internal static extern CommonErrorStatus.ResponseStatus GameServices_FetchServerAuthCodeResponse_GetStatus(HandleRef self);

		// Token: 0x06000EEE RID: 3822
		[DllImport("gpg")]
		internal static extern UIntPtr GameServices_FetchServerAuthCodeResponse_GetCode(HandleRef self, StringBuilder out_arg, UIntPtr out_size);

		// Token: 0x020008AD RID: 2221
		// (Invoke) Token: 0x06004F54 RID: 20308
		internal delegate void FlushCallback(CommonErrorStatus.FlushStatus arg0, IntPtr arg1);

		// Token: 0x020008AE RID: 2222
		// (Invoke) Token: 0x06004F58 RID: 20312
		internal delegate void FetchServerAuthCodeCallback(IntPtr arg0, IntPtr arg1);
	}
}
