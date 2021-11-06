using System;
using System.Runtime.InteropServices;
using System.Text;

namespace GooglePlayGames.Native.Cwrapper
{
	// Token: 0x020001EC RID: 492
	internal static class ScorePage
	{
		// Token: 0x06000FF9 RID: 4089
		[DllImport("gpg")]
		internal static extern void ScorePage_Dispose(HandleRef self);

		// Token: 0x06000FFA RID: 4090
		[DllImport("gpg")]
		internal static extern Types.LeaderboardTimeSpan ScorePage_TimeSpan(HandleRef self);

		// Token: 0x06000FFB RID: 4091
		[DllImport("gpg")]
		internal static extern UIntPtr ScorePage_LeaderboardId(HandleRef self, StringBuilder out_arg, UIntPtr out_size);

		// Token: 0x06000FFC RID: 4092
		[DllImport("gpg")]
		internal static extern Types.LeaderboardCollection ScorePage_Collection(HandleRef self);

		// Token: 0x06000FFD RID: 4093
		[DllImport("gpg")]
		internal static extern Types.LeaderboardStart ScorePage_Start(HandleRef self);

		// Token: 0x06000FFE RID: 4094
		[DllImport("gpg")]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool ScorePage_Valid(HandleRef self);

		// Token: 0x06000FFF RID: 4095
		[DllImport("gpg")]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool ScorePage_HasPreviousScorePage(HandleRef self);

		// Token: 0x06001000 RID: 4096
		[DllImport("gpg")]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool ScorePage_HasNextScorePage(HandleRef self);

		// Token: 0x06001001 RID: 4097
		[DllImport("gpg")]
		internal static extern IntPtr ScorePage_PreviousScorePageToken(HandleRef self);

		// Token: 0x06001002 RID: 4098
		[DllImport("gpg")]
		internal static extern IntPtr ScorePage_NextScorePageToken(HandleRef self);

		// Token: 0x06001003 RID: 4099
		[DllImport("gpg")]
		internal static extern UIntPtr ScorePage_Entries_Length(HandleRef self);

		// Token: 0x06001004 RID: 4100
		[DllImport("gpg")]
		internal static extern IntPtr ScorePage_Entries_GetElement(HandleRef self, UIntPtr index);

		// Token: 0x06001005 RID: 4101
		[DllImport("gpg")]
		internal static extern void ScorePage_Entry_Dispose(HandleRef self);

		// Token: 0x06001006 RID: 4102
		[DllImport("gpg")]
		internal static extern UIntPtr ScorePage_Entry_PlayerId(HandleRef self, StringBuilder out_arg, UIntPtr out_size);

		// Token: 0x06001007 RID: 4103
		[DllImport("gpg")]
		internal static extern ulong ScorePage_Entry_LastModified(HandleRef self);

		// Token: 0x06001008 RID: 4104
		[DllImport("gpg")]
		internal static extern IntPtr ScorePage_Entry_Score(HandleRef self);

		// Token: 0x06001009 RID: 4105
		[DllImport("gpg")]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool ScorePage_Entry_Valid(HandleRef self);

		// Token: 0x0600100A RID: 4106
		[DllImport("gpg")]
		internal static extern ulong ScorePage_Entry_LastModifiedTime(HandleRef self);

		// Token: 0x0600100B RID: 4107
		[DllImport("gpg")]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool ScorePage_ScorePageToken_Valid(HandleRef self);

		// Token: 0x0600100C RID: 4108
		[DllImport("gpg")]
		internal static extern void ScorePage_ScorePageToken_Dispose(HandleRef self);
	}
}
