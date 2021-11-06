using System;
using System.Runtime.InteropServices;

namespace GooglePlayGames.Native.Cwrapper
{
	// Token: 0x02000201 RID: 513
	internal static class TurnBasedMatchConfigBuilder
	{
		// Token: 0x06001069 RID: 4201
		[DllImport("gpg")]
		internal static extern void TurnBasedMatchConfig_Builder_PopulateFromPlayerSelectUIResponse(HandleRef self, IntPtr response);

		// Token: 0x0600106A RID: 4202
		[DllImport("gpg")]
		internal static extern void TurnBasedMatchConfig_Builder_SetVariant(HandleRef self, uint variant);

		// Token: 0x0600106B RID: 4203
		[DllImport("gpg")]
		internal static extern void TurnBasedMatchConfig_Builder_AddPlayerToInvite(HandleRef self, string player_id);

		// Token: 0x0600106C RID: 4204
		[DllImport("gpg")]
		internal static extern IntPtr TurnBasedMatchConfig_Builder_Construct();

		// Token: 0x0600106D RID: 4205
		[DllImport("gpg")]
		internal static extern void TurnBasedMatchConfig_Builder_SetExclusiveBitMask(HandleRef self, ulong exclusive_bit_mask);

		// Token: 0x0600106E RID: 4206
		[DllImport("gpg")]
		internal static extern void TurnBasedMatchConfig_Builder_SetMaximumAutomatchingPlayers(HandleRef self, uint maximum_automatching_players);

		// Token: 0x0600106F RID: 4207
		[DllImport("gpg")]
		internal static extern IntPtr TurnBasedMatchConfig_Builder_Create(HandleRef self);

		// Token: 0x06001070 RID: 4208
		[DllImport("gpg")]
		internal static extern void TurnBasedMatchConfig_Builder_SetMinimumAutomatchingPlayers(HandleRef self, uint minimum_automatching_players);

		// Token: 0x06001071 RID: 4209
		[DllImport("gpg")]
		internal static extern void TurnBasedMatchConfig_Builder_Dispose(HandleRef self);
	}
}
