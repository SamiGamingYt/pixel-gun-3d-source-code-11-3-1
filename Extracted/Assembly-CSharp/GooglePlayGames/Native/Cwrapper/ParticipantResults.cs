using System;
using System.Runtime.InteropServices;

namespace GooglePlayGames.Native.Cwrapper
{
	// Token: 0x020001DF RID: 479
	internal static class ParticipantResults
	{
		// Token: 0x06000F57 RID: 3927
		[DllImport("gpg")]
		internal static extern IntPtr ParticipantResults_WithResult(HandleRef self, string participant_id, uint placing, Types.MatchResult result);

		// Token: 0x06000F58 RID: 3928
		[DllImport("gpg")]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool ParticipantResults_Valid(HandleRef self);

		// Token: 0x06000F59 RID: 3929
		[DllImport("gpg")]
		internal static extern Types.MatchResult ParticipantResults_MatchResultForParticipant(HandleRef self, string participant_id);

		// Token: 0x06000F5A RID: 3930
		[DllImport("gpg")]
		internal static extern uint ParticipantResults_PlaceForParticipant(HandleRef self, string participant_id);

		// Token: 0x06000F5B RID: 3931
		[DllImport("gpg")]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool ParticipantResults_HasResultsForParticipant(HandleRef self, string participant_id);

		// Token: 0x06000F5C RID: 3932
		[DllImport("gpg")]
		internal static extern void ParticipantResults_Dispose(HandleRef self);
	}
}
