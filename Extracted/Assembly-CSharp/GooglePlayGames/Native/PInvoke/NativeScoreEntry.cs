using System;
using System.Runtime.InteropServices;
using System.Text;
using GooglePlayGames.Native.Cwrapper;

namespace GooglePlayGames.Native.PInvoke
{
	// Token: 0x02000255 RID: 597
	internal class NativeScoreEntry : BaseReferenceHolder
	{
		// Token: 0x06001314 RID: 4884 RVA: 0x0004EDF4 File Offset: 0x0004CFF4
		internal NativeScoreEntry(IntPtr selfPtr) : base(selfPtr)
		{
		}

		// Token: 0x06001315 RID: 4885 RVA: 0x0004EE00 File Offset: 0x0004D000
		protected override void CallDispose(HandleRef selfPointer)
		{
			ScorePage.ScorePage_Entry_Dispose(selfPointer);
		}

		// Token: 0x06001316 RID: 4886 RVA: 0x0004EE08 File Offset: 0x0004D008
		internal ulong GetLastModifiedTime()
		{
			return ScorePage.ScorePage_Entry_LastModifiedTime(base.SelfPtr());
		}

		// Token: 0x06001317 RID: 4887 RVA: 0x0004EE18 File Offset: 0x0004D018
		internal string GetPlayerId()
		{
			return PInvokeUtilities.OutParamsToString((StringBuilder out_string, UIntPtr out_size) => ScorePage.ScorePage_Entry_PlayerId(base.SelfPtr(), out_string, out_size));
		}

		// Token: 0x06001318 RID: 4888 RVA: 0x0004EE2C File Offset: 0x0004D02C
		internal NativeScore GetScore()
		{
			return new NativeScore(ScorePage.ScorePage_Entry_Score(base.SelfPtr()));
		}

		// Token: 0x06001319 RID: 4889 RVA: 0x0004EE40 File Offset: 0x0004D040
		internal PlayGamesScore AsScore(string leaderboardId)
		{
			DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
			ulong num = this.GetLastModifiedTime();
			if (num == 18446744073709551615UL)
			{
				num = 0UL;
			}
			DateTime date = dateTime.AddMilliseconds(num);
			return new PlayGamesScore(date, leaderboardId, this.GetScore().GetRank(), this.GetPlayerId(), this.GetScore().GetValue(), this.GetScore().GetMetadata());
		}

		// Token: 0x04000C05 RID: 3077
		private const ulong MinusOne = 18446744073709551615UL;
	}
}
