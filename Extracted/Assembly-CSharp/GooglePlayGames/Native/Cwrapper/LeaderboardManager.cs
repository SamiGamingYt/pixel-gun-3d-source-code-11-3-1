using System;
using System.Runtime.InteropServices;

namespace GooglePlayGames.Native.Cwrapper
{
	// Token: 0x020001D5 RID: 469
	internal static class LeaderboardManager
	{
		// Token: 0x06000EFB RID: 3835
		[DllImport("gpg")]
		internal static extern void LeaderboardManager_FetchAll(HandleRef self, Types.DataSource data_source, LeaderboardManager.FetchAllCallback callback, IntPtr callback_arg);

		// Token: 0x06000EFC RID: 3836
		[DllImport("gpg")]
		internal static extern void LeaderboardManager_FetchScoreSummary(HandleRef self, Types.DataSource data_source, string leaderboard_id, Types.LeaderboardTimeSpan time_span, Types.LeaderboardCollection collection, LeaderboardManager.FetchScoreSummaryCallback callback, IntPtr callback_arg);

		// Token: 0x06000EFD RID: 3837
		[DllImport("gpg")]
		internal static extern IntPtr LeaderboardManager_ScorePageToken(HandleRef self, string leaderboard_id, Types.LeaderboardStart start, Types.LeaderboardTimeSpan time_span, Types.LeaderboardCollection collection);

		// Token: 0x06000EFE RID: 3838
		[DllImport("gpg")]
		internal static extern void LeaderboardManager_ShowAllUI(HandleRef self, LeaderboardManager.ShowAllUICallback callback, IntPtr callback_arg);

		// Token: 0x06000EFF RID: 3839
		[DllImport("gpg")]
		internal static extern void LeaderboardManager_FetchScorePage(HandleRef self, Types.DataSource data_source, IntPtr token, uint max_results, LeaderboardManager.FetchScorePageCallback callback, IntPtr callback_arg);

		// Token: 0x06000F00 RID: 3840
		[DllImport("gpg")]
		internal static extern void LeaderboardManager_FetchAllScoreSummaries(HandleRef self, Types.DataSource data_source, string leaderboard_id, LeaderboardManager.FetchAllScoreSummariesCallback callback, IntPtr callback_arg);

		// Token: 0x06000F01 RID: 3841
		[DllImport("gpg")]
		internal static extern void LeaderboardManager_ShowUI(HandleRef self, string leaderboard_id, Types.LeaderboardTimeSpan time_span, LeaderboardManager.ShowUICallback callback, IntPtr callback_arg);

		// Token: 0x06000F02 RID: 3842
		[DllImport("gpg")]
		internal static extern void LeaderboardManager_Fetch(HandleRef self, Types.DataSource data_source, string leaderboard_id, LeaderboardManager.FetchCallback callback, IntPtr callback_arg);

		// Token: 0x06000F03 RID: 3843
		[DllImport("gpg")]
		internal static extern void LeaderboardManager_SubmitScore(HandleRef self, string leaderboard_id, ulong score, string metadata);

		// Token: 0x06000F04 RID: 3844
		[DllImport("gpg")]
		internal static extern void LeaderboardManager_FetchResponse_Dispose(HandleRef self);

		// Token: 0x06000F05 RID: 3845
		[DllImport("gpg")]
		internal static extern CommonErrorStatus.ResponseStatus LeaderboardManager_FetchResponse_GetStatus(HandleRef self);

		// Token: 0x06000F06 RID: 3846
		[DllImport("gpg")]
		internal static extern IntPtr LeaderboardManager_FetchResponse_GetData(HandleRef self);

		// Token: 0x06000F07 RID: 3847
		[DllImport("gpg")]
		internal static extern void LeaderboardManager_FetchAllResponse_Dispose(HandleRef self);

		// Token: 0x06000F08 RID: 3848
		[DllImport("gpg")]
		internal static extern CommonErrorStatus.ResponseStatus LeaderboardManager_FetchAllResponse_GetStatus(HandleRef self);

		// Token: 0x06000F09 RID: 3849
		[DllImport("gpg")]
		internal static extern UIntPtr LeaderboardManager_FetchAllResponse_GetData_Length(HandleRef self);

		// Token: 0x06000F0A RID: 3850
		[DllImport("gpg")]
		internal static extern IntPtr LeaderboardManager_FetchAllResponse_GetData_GetElement(HandleRef self, UIntPtr index);

		// Token: 0x06000F0B RID: 3851
		[DllImport("gpg")]
		internal static extern void LeaderboardManager_FetchScorePageResponse_Dispose(HandleRef self);

		// Token: 0x06000F0C RID: 3852
		[DllImport("gpg")]
		internal static extern CommonErrorStatus.ResponseStatus LeaderboardManager_FetchScorePageResponse_GetStatus(HandleRef self);

		// Token: 0x06000F0D RID: 3853
		[DllImport("gpg")]
		internal static extern IntPtr LeaderboardManager_FetchScorePageResponse_GetData(HandleRef self);

		// Token: 0x06000F0E RID: 3854
		[DllImport("gpg")]
		internal static extern void LeaderboardManager_FetchScoreSummaryResponse_Dispose(HandleRef self);

		// Token: 0x06000F0F RID: 3855
		[DllImport("gpg")]
		internal static extern CommonErrorStatus.ResponseStatus LeaderboardManager_FetchScoreSummaryResponse_GetStatus(HandleRef self);

		// Token: 0x06000F10 RID: 3856
		[DllImport("gpg")]
		internal static extern IntPtr LeaderboardManager_FetchScoreSummaryResponse_GetData(HandleRef self);

		// Token: 0x06000F11 RID: 3857
		[DllImport("gpg")]
		internal static extern void LeaderboardManager_FetchAllScoreSummariesResponse_Dispose(HandleRef self);

		// Token: 0x06000F12 RID: 3858
		[DllImport("gpg")]
		internal static extern CommonErrorStatus.ResponseStatus LeaderboardManager_FetchAllScoreSummariesResponse_GetStatus(HandleRef self);

		// Token: 0x06000F13 RID: 3859
		[DllImport("gpg")]
		internal static extern UIntPtr LeaderboardManager_FetchAllScoreSummariesResponse_GetData_Length(HandleRef self);

		// Token: 0x06000F14 RID: 3860
		[DllImport("gpg")]
		internal static extern IntPtr LeaderboardManager_FetchAllScoreSummariesResponse_GetData_GetElement(HandleRef self, UIntPtr index);

		// Token: 0x020008AF RID: 2223
		// (Invoke) Token: 0x06004F5C RID: 20316
		internal delegate void FetchCallback(IntPtr arg0, IntPtr arg1);

		// Token: 0x020008B0 RID: 2224
		// (Invoke) Token: 0x06004F60 RID: 20320
		internal delegate void FetchAllCallback(IntPtr arg0, IntPtr arg1);

		// Token: 0x020008B1 RID: 2225
		// (Invoke) Token: 0x06004F64 RID: 20324
		internal delegate void FetchScorePageCallback(IntPtr arg0, IntPtr arg1);

		// Token: 0x020008B2 RID: 2226
		// (Invoke) Token: 0x06004F68 RID: 20328
		internal delegate void FetchScoreSummaryCallback(IntPtr arg0, IntPtr arg1);

		// Token: 0x020008B3 RID: 2227
		// (Invoke) Token: 0x06004F6C RID: 20332
		internal delegate void FetchAllScoreSummariesCallback(IntPtr arg0, IntPtr arg1);

		// Token: 0x020008B4 RID: 2228
		// (Invoke) Token: 0x06004F70 RID: 20336
		internal delegate void ShowAllUICallback(CommonErrorStatus.UIStatus arg0, IntPtr arg1);

		// Token: 0x020008B5 RID: 2229
		// (Invoke) Token: 0x06004F74 RID: 20340
		internal delegate void ShowUICallback(CommonErrorStatus.UIStatus arg0, IntPtr arg1);
	}
}
