using System;
using System.Runtime.InteropServices;
using System.Text;

namespace GooglePlayGames.Native.Cwrapper
{
	// Token: 0x020001E9 RID: 489
	internal static class RealTimeRoomConfig
	{
		// Token: 0x06000FE3 RID: 4067
		[DllImport("gpg")]
		internal static extern UIntPtr RealTimeRoomConfig_PlayerIdsToInvite_Length(HandleRef self);

		// Token: 0x06000FE4 RID: 4068
		[DllImport("gpg")]
		internal static extern UIntPtr RealTimeRoomConfig_PlayerIdsToInvite_GetElement(HandleRef self, UIntPtr index, StringBuilder out_arg, UIntPtr out_size);

		// Token: 0x06000FE5 RID: 4069
		[DllImport("gpg")]
		internal static extern uint RealTimeRoomConfig_Variant(HandleRef self);

		// Token: 0x06000FE6 RID: 4070
		[DllImport("gpg")]
		internal static extern long RealTimeRoomConfig_ExclusiveBitMask(HandleRef self);

		// Token: 0x06000FE7 RID: 4071
		[DllImport("gpg")]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool RealTimeRoomConfig_Valid(HandleRef self);

		// Token: 0x06000FE8 RID: 4072
		[DllImport("gpg")]
		internal static extern uint RealTimeRoomConfig_MaximumAutomatchingPlayers(HandleRef self);

		// Token: 0x06000FE9 RID: 4073
		[DllImport("gpg")]
		internal static extern uint RealTimeRoomConfig_MinimumAutomatchingPlayers(HandleRef self);

		// Token: 0x06000FEA RID: 4074
		[DllImport("gpg")]
		internal static extern void RealTimeRoomConfig_Dispose(HandleRef self);
	}
}
