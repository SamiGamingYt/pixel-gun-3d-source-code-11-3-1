using System;
using System.Runtime.InteropServices;
using System.Text;

namespace GooglePlayGames.Native.Cwrapper
{
	// Token: 0x020001C1 RID: 449
	internal static class Achievement
	{
		// Token: 0x06000E9F RID: 3743
		[DllImport("gpg")]
		internal static extern uint Achievement_TotalSteps(HandleRef self);

		// Token: 0x06000EA0 RID: 3744
		[DllImport("gpg")]
		internal static extern UIntPtr Achievement_Description(HandleRef self, StringBuilder out_arg, UIntPtr out_size);

		// Token: 0x06000EA1 RID: 3745
		[DllImport("gpg")]
		internal static extern void Achievement_Dispose(HandleRef self);

		// Token: 0x06000EA2 RID: 3746
		[DllImport("gpg")]
		internal static extern UIntPtr Achievement_UnlockedIconUrl(HandleRef self, StringBuilder out_arg, UIntPtr out_size);

		// Token: 0x06000EA3 RID: 3747
		[DllImport("gpg")]
		internal static extern ulong Achievement_LastModifiedTime(HandleRef self);

		// Token: 0x06000EA4 RID: 3748
		[DllImport("gpg")]
		internal static extern UIntPtr Achievement_RevealedIconUrl(HandleRef self, StringBuilder out_arg, UIntPtr out_size);

		// Token: 0x06000EA5 RID: 3749
		[DllImport("gpg")]
		internal static extern uint Achievement_CurrentSteps(HandleRef self);

		// Token: 0x06000EA6 RID: 3750
		[DllImport("gpg")]
		internal static extern Types.AchievementState Achievement_State(HandleRef self);

		// Token: 0x06000EA7 RID: 3751
		[DllImport("gpg")]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool Achievement_Valid(HandleRef self);

		// Token: 0x06000EA8 RID: 3752
		[DllImport("gpg")]
		internal static extern ulong Achievement_LastModified(HandleRef self);

		// Token: 0x06000EA9 RID: 3753
		[DllImport("gpg")]
		internal static extern ulong Achievement_XP(HandleRef self);

		// Token: 0x06000EAA RID: 3754
		[DllImport("gpg")]
		internal static extern Types.AchievementType Achievement_Type(HandleRef self);

		// Token: 0x06000EAB RID: 3755
		[DllImport("gpg")]
		internal static extern UIntPtr Achievement_Id(HandleRef self, StringBuilder out_arg, UIntPtr out_size);

		// Token: 0x06000EAC RID: 3756
		[DllImport("gpg")]
		internal static extern UIntPtr Achievement_Name(HandleRef self, StringBuilder out_arg, UIntPtr out_size);
	}
}
