using System;
using System.Runtime.InteropServices;
using GooglePlayGames.Native.Cwrapper;

namespace GooglePlayGames.Native.PInvoke
{
	// Token: 0x02000241 RID: 577
	internal class FetchScorePageResponse : BaseReferenceHolder
	{
		// Token: 0x06001242 RID: 4674 RVA: 0x0004D878 File Offset: 0x0004BA78
		internal FetchScorePageResponse(IntPtr selfPointer) : base(selfPointer)
		{
		}

		// Token: 0x06001243 RID: 4675 RVA: 0x0004D884 File Offset: 0x0004BA84
		protected override void CallDispose(HandleRef selfPointer)
		{
			LeaderboardManager.LeaderboardManager_FetchScorePageResponse_Dispose(base.SelfPtr());
		}

		// Token: 0x06001244 RID: 4676 RVA: 0x0004D894 File Offset: 0x0004BA94
		internal CommonErrorStatus.ResponseStatus GetStatus()
		{
			return LeaderboardManager.LeaderboardManager_FetchScorePageResponse_GetStatus(base.SelfPtr());
		}

		// Token: 0x06001245 RID: 4677 RVA: 0x0004D8A4 File Offset: 0x0004BAA4
		internal NativeScorePage GetScorePage()
		{
			return NativeScorePage.FromPointer(LeaderboardManager.LeaderboardManager_FetchScorePageResponse_GetData(base.SelfPtr()));
		}

		// Token: 0x06001246 RID: 4678 RVA: 0x0004D8B8 File Offset: 0x0004BAB8
		internal static FetchScorePageResponse FromPointer(IntPtr pointer)
		{
			if (pointer.Equals(IntPtr.Zero))
			{
				return null;
			}
			return new FetchScorePageResponse(pointer);
		}
	}
}
