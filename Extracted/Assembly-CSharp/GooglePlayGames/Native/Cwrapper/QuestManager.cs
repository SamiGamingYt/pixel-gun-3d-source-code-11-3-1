using System;
using System.Runtime.InteropServices;

namespace GooglePlayGames.Native.Cwrapper
{
	// Token: 0x020001E4 RID: 484
	internal static class QuestManager
	{
		// Token: 0x06000F94 RID: 3988
		[DllImport("gpg")]
		internal static extern void QuestManager_FetchList(HandleRef self, Types.DataSource data_source, int fetch_flags, QuestManager.FetchListCallback callback, IntPtr callback_arg);

		// Token: 0x06000F95 RID: 3989
		[DllImport("gpg")]
		internal static extern void QuestManager_Accept(HandleRef self, IntPtr quest, QuestManager.AcceptCallback callback, IntPtr callback_arg);

		// Token: 0x06000F96 RID: 3990
		[DllImport("gpg")]
		internal static extern void QuestManager_ShowAllUI(HandleRef self, QuestManager.QuestUICallback callback, IntPtr callback_arg);

		// Token: 0x06000F97 RID: 3991
		[DllImport("gpg")]
		internal static extern void QuestManager_ShowUI(HandleRef self, IntPtr quest, QuestManager.QuestUICallback callback, IntPtr callback_arg);

		// Token: 0x06000F98 RID: 3992
		[DllImport("gpg")]
		internal static extern void QuestManager_ClaimMilestone(HandleRef self, IntPtr milestone, QuestManager.ClaimMilestoneCallback callback, IntPtr callback_arg);

		// Token: 0x06000F99 RID: 3993
		[DllImport("gpg")]
		internal static extern void QuestManager_Fetch(HandleRef self, Types.DataSource data_source, string quest_id, QuestManager.FetchCallback callback, IntPtr callback_arg);

		// Token: 0x06000F9A RID: 3994
		[DllImport("gpg")]
		internal static extern void QuestManager_FetchResponse_Dispose(HandleRef self);

		// Token: 0x06000F9B RID: 3995
		[DllImport("gpg")]
		internal static extern CommonErrorStatus.ResponseStatus QuestManager_FetchResponse_GetStatus(HandleRef self);

		// Token: 0x06000F9C RID: 3996
		[DllImport("gpg")]
		internal static extern IntPtr QuestManager_FetchResponse_GetData(HandleRef self);

		// Token: 0x06000F9D RID: 3997
		[DllImport("gpg")]
		internal static extern void QuestManager_FetchListResponse_Dispose(HandleRef self);

		// Token: 0x06000F9E RID: 3998
		[DllImport("gpg")]
		internal static extern CommonErrorStatus.ResponseStatus QuestManager_FetchListResponse_GetStatus(HandleRef self);

		// Token: 0x06000F9F RID: 3999
		[DllImport("gpg")]
		internal static extern UIntPtr QuestManager_FetchListResponse_GetData_Length(HandleRef self);

		// Token: 0x06000FA0 RID: 4000
		[DllImport("gpg")]
		internal static extern IntPtr QuestManager_FetchListResponse_GetData_GetElement(HandleRef self, UIntPtr index);

		// Token: 0x06000FA1 RID: 4001
		[DllImport("gpg")]
		internal static extern void QuestManager_AcceptResponse_Dispose(HandleRef self);

		// Token: 0x06000FA2 RID: 4002
		[DllImport("gpg")]
		internal static extern CommonErrorStatus.QuestAcceptStatus QuestManager_AcceptResponse_GetStatus(HandleRef self);

		// Token: 0x06000FA3 RID: 4003
		[DllImport("gpg")]
		internal static extern IntPtr QuestManager_AcceptResponse_GetAcceptedQuest(HandleRef self);

		// Token: 0x06000FA4 RID: 4004
		[DllImport("gpg")]
		internal static extern void QuestManager_ClaimMilestoneResponse_Dispose(HandleRef self);

		// Token: 0x06000FA5 RID: 4005
		[DllImport("gpg")]
		internal static extern CommonErrorStatus.QuestClaimMilestoneStatus QuestManager_ClaimMilestoneResponse_GetStatus(HandleRef self);

		// Token: 0x06000FA6 RID: 4006
		[DllImport("gpg")]
		internal static extern IntPtr QuestManager_ClaimMilestoneResponse_GetClaimedMilestone(HandleRef self);

		// Token: 0x06000FA7 RID: 4007
		[DllImport("gpg")]
		internal static extern IntPtr QuestManager_ClaimMilestoneResponse_GetQuest(HandleRef self);

		// Token: 0x06000FA8 RID: 4008
		[DllImport("gpg")]
		internal static extern void QuestManager_QuestUIResponse_Dispose(HandleRef self);

		// Token: 0x06000FA9 RID: 4009
		[DllImport("gpg")]
		internal static extern CommonErrorStatus.UIStatus QuestManager_QuestUIResponse_GetStatus(HandleRef self);

		// Token: 0x06000FAA RID: 4010
		[DllImport("gpg")]
		internal static extern IntPtr QuestManager_QuestUIResponse_GetAcceptedQuest(HandleRef self);

		// Token: 0x06000FAB RID: 4011
		[DllImport("gpg")]
		internal static extern IntPtr QuestManager_QuestUIResponse_GetMilestoneToClaim(HandleRef self);

		// Token: 0x020008C0 RID: 2240
		// (Invoke) Token: 0x06004FA0 RID: 20384
		internal delegate void FetchCallback(IntPtr arg0, IntPtr arg1);

		// Token: 0x020008C1 RID: 2241
		// (Invoke) Token: 0x06004FA4 RID: 20388
		internal delegate void FetchListCallback(IntPtr arg0, IntPtr arg1);

		// Token: 0x020008C2 RID: 2242
		// (Invoke) Token: 0x06004FA8 RID: 20392
		internal delegate void AcceptCallback(IntPtr arg0, IntPtr arg1);

		// Token: 0x020008C3 RID: 2243
		// (Invoke) Token: 0x06004FAC RID: 20396
		internal delegate void ClaimMilestoneCallback(IntPtr arg0, IntPtr arg1);

		// Token: 0x020008C4 RID: 2244
		// (Invoke) Token: 0x06004FB0 RID: 20400
		internal delegate void QuestUICallback(IntPtr arg0, IntPtr arg1);
	}
}
