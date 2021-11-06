using System;
using System.Runtime.InteropServices;
using GooglePlayGames.Native.Cwrapper;

namespace GooglePlayGames.Native.PInvoke
{
	// Token: 0x02000258 RID: 600
	internal class NativeScoreSummary : BaseReferenceHolder
	{
		// Token: 0x0600132E RID: 4910 RVA: 0x0004F030 File Offset: 0x0004D230
		internal NativeScoreSummary(IntPtr selfPtr) : base(selfPtr)
		{
		}

		// Token: 0x0600132F RID: 4911 RVA: 0x0004F03C File Offset: 0x0004D23C
		protected override void CallDispose(HandleRef selfPointer)
		{
			ScoreSummary.ScoreSummary_Dispose(selfPointer);
		}

		// Token: 0x06001330 RID: 4912 RVA: 0x0004F044 File Offset: 0x0004D244
		internal ulong ApproximateResults()
		{
			return ScoreSummary.ScoreSummary_ApproximateNumberOfScores(base.SelfPtr());
		}

		// Token: 0x06001331 RID: 4913 RVA: 0x0004F054 File Offset: 0x0004D254
		internal NativeScore LocalUserScore()
		{
			return NativeScore.FromPointer(ScoreSummary.ScoreSummary_CurrentPlayerScore(base.SelfPtr()));
		}

		// Token: 0x06001332 RID: 4914 RVA: 0x0004F068 File Offset: 0x0004D268
		internal static NativeScoreSummary FromPointer(IntPtr pointer)
		{
			if (pointer.Equals(IntPtr.Zero))
			{
				return null;
			}
			return new NativeScoreSummary(pointer);
		}
	}
}
