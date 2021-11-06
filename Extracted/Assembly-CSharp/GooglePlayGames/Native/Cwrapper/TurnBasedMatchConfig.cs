using System;
using System.Runtime.InteropServices;
using System.Text;

namespace GooglePlayGames.Native.Cwrapper
{
	// Token: 0x02000200 RID: 512
	internal static class TurnBasedMatchConfig
	{
		// Token: 0x06001061 RID: 4193
		[DllImport("gpg")]
		internal static extern UIntPtr TurnBasedMatchConfig_PlayerIdsToInvite_Length(HandleRef self);

		// Token: 0x06001062 RID: 4194
		[DllImport("gpg")]
		internal static extern UIntPtr TurnBasedMatchConfig_PlayerIdsToInvite_GetElement(HandleRef self, UIntPtr index, StringBuilder out_arg, UIntPtr out_size);

		// Token: 0x06001063 RID: 4195
		[DllImport("gpg")]
		internal static extern uint TurnBasedMatchConfig_Variant(HandleRef self);

		// Token: 0x06001064 RID: 4196
		[DllImport("gpg")]
		internal static extern long TurnBasedMatchConfig_ExclusiveBitMask(HandleRef self);

		// Token: 0x06001065 RID: 4197
		[DllImport("gpg")]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool TurnBasedMatchConfig_Valid(HandleRef self);

		// Token: 0x06001066 RID: 4198
		[DllImport("gpg")]
		internal static extern uint TurnBasedMatchConfig_MaximumAutomatchingPlayers(HandleRef self);

		// Token: 0x06001067 RID: 4199
		[DllImport("gpg")]
		internal static extern uint TurnBasedMatchConfig_MinimumAutomatchingPlayers(HandleRef self);

		// Token: 0x06001068 RID: 4200
		[DllImport("gpg")]
		internal static extern void TurnBasedMatchConfig_Dispose(HandleRef self);
	}
}
