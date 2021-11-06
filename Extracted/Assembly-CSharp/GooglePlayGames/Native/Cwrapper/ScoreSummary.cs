using System;
using System.Runtime.InteropServices;
using System.Text;

namespace GooglePlayGames.Native.Cwrapper
{
	// Token: 0x020001ED RID: 493
	internal static class ScoreSummary
	{
		// Token: 0x0600100D RID: 4109
		[DllImport("gpg")]
		internal static extern ulong ScoreSummary_ApproximateNumberOfScores(HandleRef self);

		// Token: 0x0600100E RID: 4110
		[DllImport("gpg")]
		internal static extern Types.LeaderboardTimeSpan ScoreSummary_TimeSpan(HandleRef self);

		// Token: 0x0600100F RID: 4111
		[DllImport("gpg")]
		internal static extern UIntPtr ScoreSummary_LeaderboardId(HandleRef self, StringBuilder out_arg, UIntPtr out_size);

		// Token: 0x06001010 RID: 4112
		[DllImport("gpg")]
		internal static extern Types.LeaderboardCollection ScoreSummary_Collection(HandleRef self);

		// Token: 0x06001011 RID: 4113
		[DllImport("gpg")]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool ScoreSummary_Valid(HandleRef self);

		// Token: 0x06001012 RID: 4114
		[DllImport("gpg")]
		internal static extern IntPtr ScoreSummary_CurrentPlayerScore(HandleRef self);

		// Token: 0x06001013 RID: 4115
		[DllImport("gpg")]
		internal static extern void ScoreSummary_Dispose(HandleRef self);
	}
}
