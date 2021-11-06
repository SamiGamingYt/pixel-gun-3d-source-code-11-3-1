using System;
using System.Runtime.InteropServices;
using System.Text;

namespace GooglePlayGames.Native.Cwrapper
{
	// Token: 0x020001E0 RID: 480
	internal static class Player
	{
		// Token: 0x06000F5D RID: 3933
		[DllImport("gpg")]
		internal static extern IntPtr Player_CurrentLevel(HandleRef self);

		// Token: 0x06000F5E RID: 3934
		[DllImport("gpg")]
		internal static extern UIntPtr Player_Name(HandleRef self, StringBuilder out_arg, UIntPtr out_size);

		// Token: 0x06000F5F RID: 3935
		[DllImport("gpg")]
		internal static extern void Player_Dispose(HandleRef self);

		// Token: 0x06000F60 RID: 3936
		[DllImport("gpg")]
		internal static extern UIntPtr Player_AvatarUrl(HandleRef self, Types.ImageResolution resolution, StringBuilder out_arg, UIntPtr out_size);

		// Token: 0x06000F61 RID: 3937
		[DllImport("gpg")]
		internal static extern ulong Player_LastLevelUpTime(HandleRef self);

		// Token: 0x06000F62 RID: 3938
		[DllImport("gpg")]
		internal static extern UIntPtr Player_Title(HandleRef self, StringBuilder out_arg, UIntPtr out_size);

		// Token: 0x06000F63 RID: 3939
		[DllImport("gpg")]
		internal static extern ulong Player_CurrentXP(HandleRef self);

		// Token: 0x06000F64 RID: 3940
		[DllImport("gpg")]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool Player_Valid(HandleRef self);

		// Token: 0x06000F65 RID: 3941
		[DllImport("gpg")]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool Player_HasLevelInfo(HandleRef self);

		// Token: 0x06000F66 RID: 3942
		[DllImport("gpg")]
		internal static extern IntPtr Player_NextLevel(HandleRef self);

		// Token: 0x06000F67 RID: 3943
		[DllImport("gpg")]
		internal static extern UIntPtr Player_Id(HandleRef self, StringBuilder out_arg, UIntPtr out_size);
	}
}
