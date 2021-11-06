using System;
using System.Runtime.InteropServices;
using System.Text;

namespace GooglePlayGames.Native.Cwrapper
{
	// Token: 0x020001E3 RID: 483
	internal static class Quest
	{
		// Token: 0x06000F87 RID: 3975
		[DllImport("gpg")]
		internal static extern UIntPtr Quest_Description(HandleRef self, StringBuilder out_arg, UIntPtr out_size);

		// Token: 0x06000F88 RID: 3976
		[DllImport("gpg")]
		internal static extern UIntPtr Quest_BannerUrl(HandleRef self, StringBuilder out_arg, UIntPtr out_size);

		// Token: 0x06000F89 RID: 3977
		[DllImport("gpg")]
		internal static extern long Quest_ExpirationTime(HandleRef self);

		// Token: 0x06000F8A RID: 3978
		[DllImport("gpg")]
		internal static extern UIntPtr Quest_IconUrl(HandleRef self, StringBuilder out_arg, UIntPtr out_size);

		// Token: 0x06000F8B RID: 3979
		[DllImport("gpg")]
		internal static extern Types.QuestState Quest_State(HandleRef self);

		// Token: 0x06000F8C RID: 3980
		[DllImport("gpg")]
		internal static extern IntPtr Quest_Copy(HandleRef self);

		// Token: 0x06000F8D RID: 3981
		[DllImport("gpg")]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool Quest_Valid(HandleRef self);

		// Token: 0x06000F8E RID: 3982
		[DllImport("gpg")]
		internal static extern long Quest_StartTime(HandleRef self);

		// Token: 0x06000F8F RID: 3983
		[DllImport("gpg")]
		internal static extern void Quest_Dispose(HandleRef self);

		// Token: 0x06000F90 RID: 3984
		[DllImport("gpg")]
		internal static extern IntPtr Quest_CurrentMilestone(HandleRef self);

		// Token: 0x06000F91 RID: 3985
		[DllImport("gpg")]
		internal static extern long Quest_AcceptedTime(HandleRef self);

		// Token: 0x06000F92 RID: 3986
		[DllImport("gpg")]
		internal static extern UIntPtr Quest_Id(HandleRef self, StringBuilder out_arg, UIntPtr out_size);

		// Token: 0x06000F93 RID: 3987
		[DllImport("gpg")]
		internal static extern UIntPtr Quest_Name(HandleRef self, StringBuilder out_arg, UIntPtr out_size);
	}
}
