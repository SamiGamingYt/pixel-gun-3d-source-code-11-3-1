using System;
using System.Runtime.InteropServices;
using GooglePlayGames.Native.Cwrapper;

namespace GooglePlayGames.Native.PInvoke
{
	// Token: 0x02000243 RID: 579
	internal class FetchScoreSummaryResponse : BaseReferenceHolder
	{
		// Token: 0x0600124C RID: 4684 RVA: 0x0004D938 File Offset: 0x0004BB38
		internal FetchScoreSummaryResponse(IntPtr selfPointer) : base(selfPointer)
		{
		}

		// Token: 0x0600124D RID: 4685 RVA: 0x0004D944 File Offset: 0x0004BB44
		protected override void CallDispose(HandleRef selfPointer)
		{
			LeaderboardManager.LeaderboardManager_FetchScoreSummaryResponse_Dispose(selfPointer);
		}

		// Token: 0x0600124E RID: 4686 RVA: 0x0004D94C File Offset: 0x0004BB4C
		internal CommonErrorStatus.ResponseStatus GetStatus()
		{
			return LeaderboardManager.LeaderboardManager_FetchScoreSummaryResponse_GetStatus(base.SelfPtr());
		}

		// Token: 0x0600124F RID: 4687 RVA: 0x0004D95C File Offset: 0x0004BB5C
		internal NativeScoreSummary GetScoreSummary()
		{
			return NativeScoreSummary.FromPointer(LeaderboardManager.LeaderboardManager_FetchScoreSummaryResponse_GetData(base.SelfPtr()));
		}

		// Token: 0x06001250 RID: 4688 RVA: 0x0004D970 File Offset: 0x0004BB70
		internal static FetchScoreSummaryResponse FromPointer(IntPtr pointer)
		{
			if (pointer.Equals(IntPtr.Zero))
			{
				return null;
			}
			return new FetchScoreSummaryResponse(pointer);
		}
	}
}
