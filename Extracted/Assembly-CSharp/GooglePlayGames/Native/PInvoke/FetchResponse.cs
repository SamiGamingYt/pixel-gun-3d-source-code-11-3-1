using System;
using System.Runtime.InteropServices;
using GooglePlayGames.Native.Cwrapper;

namespace GooglePlayGames.Native.PInvoke
{
	// Token: 0x02000242 RID: 578
	internal class FetchResponse : BaseReferenceHolder
	{
		// Token: 0x06001247 RID: 4679 RVA: 0x0004D8D8 File Offset: 0x0004BAD8
		internal FetchResponse(IntPtr selfPointer) : base(selfPointer)
		{
		}

		// Token: 0x06001248 RID: 4680 RVA: 0x0004D8E4 File Offset: 0x0004BAE4
		protected override void CallDispose(HandleRef selfPointer)
		{
			LeaderboardManager.LeaderboardManager_FetchResponse_Dispose(base.SelfPtr());
		}

		// Token: 0x06001249 RID: 4681 RVA: 0x0004D8F4 File Offset: 0x0004BAF4
		internal NativeLeaderboard Leaderboard()
		{
			return NativeLeaderboard.FromPointer(LeaderboardManager.LeaderboardManager_FetchResponse_GetData(base.SelfPtr()));
		}

		// Token: 0x0600124A RID: 4682 RVA: 0x0004D908 File Offset: 0x0004BB08
		internal CommonErrorStatus.ResponseStatus GetStatus()
		{
			return LeaderboardManager.LeaderboardManager_FetchResponse_GetStatus(base.SelfPtr());
		}

		// Token: 0x0600124B RID: 4683 RVA: 0x0004D918 File Offset: 0x0004BB18
		internal static FetchResponse FromPointer(IntPtr pointer)
		{
			if (pointer.Equals(IntPtr.Zero))
			{
				return null;
			}
			return new FetchResponse(pointer);
		}
	}
}
