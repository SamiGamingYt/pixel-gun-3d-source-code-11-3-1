using System;
using System.Runtime.InteropServices;
using System.Text;

namespace GooglePlayGames.Native.Cwrapper
{
	// Token: 0x020001E5 RID: 485
	internal static class QuestMilestone
	{
		// Token: 0x06000FAC RID: 4012
		[DllImport("gpg")]
		internal static extern UIntPtr QuestMilestone_EventId(HandleRef self, StringBuilder out_arg, UIntPtr out_size);

		// Token: 0x06000FAD RID: 4013
		[DllImport("gpg")]
		internal static extern ulong QuestMilestone_CurrentCount(HandleRef self);

		// Token: 0x06000FAE RID: 4014
		[DllImport("gpg")]
		internal static extern IntPtr QuestMilestone_Copy(HandleRef self);

		// Token: 0x06000FAF RID: 4015
		[DllImport("gpg")]
		internal static extern void QuestMilestone_Dispose(HandleRef self);

		// Token: 0x06000FB0 RID: 4016
		[DllImport("gpg")]
		internal static extern ulong QuestMilestone_TargetCount(HandleRef self);

		// Token: 0x06000FB1 RID: 4017
		[DllImport("gpg")]
		internal static extern UIntPtr QuestMilestone_QuestId(HandleRef self, StringBuilder out_arg, UIntPtr out_size);

		// Token: 0x06000FB2 RID: 4018
		[DllImport("gpg")]
		internal static extern UIntPtr QuestMilestone_CompletionRewardData(HandleRef self, [In] [Out] byte[] out_arg, UIntPtr out_size);

		// Token: 0x06000FB3 RID: 4019
		[DllImport("gpg")]
		internal static extern Types.QuestMilestoneState QuestMilestone_State(HandleRef self);

		// Token: 0x06000FB4 RID: 4020
		[DllImport("gpg")]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool QuestMilestone_Valid(HandleRef self);

		// Token: 0x06000FB5 RID: 4021
		[DllImport("gpg")]
		internal static extern UIntPtr QuestMilestone_Id(HandleRef self, StringBuilder out_arg, UIntPtr out_size);
	}
}
