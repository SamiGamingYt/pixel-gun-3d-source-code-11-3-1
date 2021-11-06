using System;
using System.Runtime.InteropServices;
using System.Text;

namespace GooglePlayGames.Native.Cwrapper
{
	// Token: 0x020001D8 RID: 472
	internal static class MultiplayerParticipant
	{
		// Token: 0x06000F23 RID: 3875
		[DllImport("gpg")]
		internal static extern Types.ParticipantStatus MultiplayerParticipant_Status(HandleRef self);

		// Token: 0x06000F24 RID: 3876
		[DllImport("gpg")]
		internal static extern uint MultiplayerParticipant_MatchRank(HandleRef self);

		// Token: 0x06000F25 RID: 3877
		[DllImport("gpg")]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool MultiplayerParticipant_IsConnectedToRoom(HandleRef self);

		// Token: 0x06000F26 RID: 3878
		[DllImport("gpg")]
		internal static extern UIntPtr MultiplayerParticipant_DisplayName(HandleRef self, StringBuilder out_arg, UIntPtr out_size);

		// Token: 0x06000F27 RID: 3879
		[DllImport("gpg")]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool MultiplayerParticipant_HasPlayer(HandleRef self);

		// Token: 0x06000F28 RID: 3880
		[DllImport("gpg")]
		internal static extern UIntPtr MultiplayerParticipant_AvatarUrl(HandleRef self, Types.ImageResolution resolution, StringBuilder out_arg, UIntPtr out_size);

		// Token: 0x06000F29 RID: 3881
		[DllImport("gpg")]
		internal static extern Types.MatchResult MultiplayerParticipant_MatchResult(HandleRef self);

		// Token: 0x06000F2A RID: 3882
		[DllImport("gpg")]
		internal static extern IntPtr MultiplayerParticipant_Player(HandleRef self);

		// Token: 0x06000F2B RID: 3883
		[DllImport("gpg")]
		internal static extern void MultiplayerParticipant_Dispose(HandleRef self);

		// Token: 0x06000F2C RID: 3884
		[DllImport("gpg")]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool MultiplayerParticipant_Valid(HandleRef self);

		// Token: 0x06000F2D RID: 3885
		[DllImport("gpg")]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool MultiplayerParticipant_HasMatchResult(HandleRef self);

		// Token: 0x06000F2E RID: 3886
		[DllImport("gpg")]
		internal static extern UIntPtr MultiplayerParticipant_Id(HandleRef self, StringBuilder out_arg, UIntPtr out_size);
	}
}
