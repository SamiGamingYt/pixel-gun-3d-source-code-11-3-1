using System;
using System.Runtime.InteropServices;

namespace GooglePlayGames.Native.Cwrapper
{
	// Token: 0x020001EA RID: 490
	internal static class RealTimeRoomConfigBuilder
	{
		// Token: 0x06000FEB RID: 4075
		[DllImport("gpg")]
		internal static extern void RealTimeRoomConfig_Builder_PopulateFromPlayerSelectUIResponse(HandleRef self, IntPtr response);

		// Token: 0x06000FEC RID: 4076
		[DllImport("gpg")]
		internal static extern void RealTimeRoomConfig_Builder_SetVariant(HandleRef self, uint variant);

		// Token: 0x06000FED RID: 4077
		[DllImport("gpg")]
		internal static extern void RealTimeRoomConfig_Builder_AddPlayerToInvite(HandleRef self, string player_id);

		// Token: 0x06000FEE RID: 4078
		[DllImport("gpg")]
		internal static extern IntPtr RealTimeRoomConfig_Builder_Construct();

		// Token: 0x06000FEF RID: 4079
		[DllImport("gpg")]
		internal static extern void RealTimeRoomConfig_Builder_SetExclusiveBitMask(HandleRef self, ulong exclusive_bit_mask);

		// Token: 0x06000FF0 RID: 4080
		[DllImport("gpg")]
		internal static extern void RealTimeRoomConfig_Builder_SetMaximumAutomatchingPlayers(HandleRef self, uint maximum_automatching_players);

		// Token: 0x06000FF1 RID: 4081
		[DllImport("gpg")]
		internal static extern IntPtr RealTimeRoomConfig_Builder_Create(HandleRef self);

		// Token: 0x06000FF2 RID: 4082
		[DllImport("gpg")]
		internal static extern void RealTimeRoomConfig_Builder_SetMinimumAutomatchingPlayers(HandleRef self, uint minimum_automatching_players);

		// Token: 0x06000FF3 RID: 4083
		[DllImport("gpg")]
		internal static extern void RealTimeRoomConfig_Builder_Dispose(HandleRef self);
	}
}
