using System;
using System.Runtime.InteropServices;
using System.Text;

namespace GooglePlayGames.Native.Cwrapper
{
	// Token: 0x020001E8 RID: 488
	internal static class RealTimeRoom
	{
		// Token: 0x06000FD7 RID: 4055
		[DllImport("gpg")]
		internal static extern Types.RealTimeRoomStatus RealTimeRoom_Status(HandleRef self);

		// Token: 0x06000FD8 RID: 4056
		[DllImport("gpg")]
		internal static extern UIntPtr RealTimeRoom_Description(HandleRef self, StringBuilder out_arg, UIntPtr out_size);

		// Token: 0x06000FD9 RID: 4057
		[DllImport("gpg")]
		internal static extern uint RealTimeRoom_Variant(HandleRef self);

		// Token: 0x06000FDA RID: 4058
		[DllImport("gpg")]
		internal static extern ulong RealTimeRoom_CreationTime(HandleRef self);

		// Token: 0x06000FDB RID: 4059
		[DllImport("gpg")]
		internal static extern UIntPtr RealTimeRoom_Participants_Length(HandleRef self);

		// Token: 0x06000FDC RID: 4060
		[DllImport("gpg")]
		internal static extern IntPtr RealTimeRoom_Participants_GetElement(HandleRef self, UIntPtr index);

		// Token: 0x06000FDD RID: 4061
		[DllImport("gpg")]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool RealTimeRoom_Valid(HandleRef self);

		// Token: 0x06000FDE RID: 4062
		[DllImport("gpg")]
		internal static extern uint RealTimeRoom_RemainingAutomatchingSlots(HandleRef self);

		// Token: 0x06000FDF RID: 4063
		[DllImport("gpg")]
		internal static extern ulong RealTimeRoom_AutomatchWaitEstimate(HandleRef self);

		// Token: 0x06000FE0 RID: 4064
		[DllImport("gpg")]
		internal static extern IntPtr RealTimeRoom_CreatingParticipant(HandleRef self);

		// Token: 0x06000FE1 RID: 4065
		[DllImport("gpg")]
		internal static extern UIntPtr RealTimeRoom_Id(HandleRef self, StringBuilder out_arg, UIntPtr out_size);

		// Token: 0x06000FE2 RID: 4066
		[DllImport("gpg")]
		internal static extern void RealTimeRoom_Dispose(HandleRef self);
	}
}
