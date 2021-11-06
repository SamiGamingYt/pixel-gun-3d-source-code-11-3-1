using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using GooglePlayGames.Native.Cwrapper;

namespace GooglePlayGames.Native.PInvoke
{
	// Token: 0x02000256 RID: 598
	internal class NativeScorePage : BaseReferenceHolder
	{
		// Token: 0x0600131B RID: 4891 RVA: 0x0004EEC0 File Offset: 0x0004D0C0
		internal NativeScorePage(IntPtr selfPtr) : base(selfPtr)
		{
		}

		// Token: 0x0600131C RID: 4892 RVA: 0x0004EECC File Offset: 0x0004D0CC
		protected override void CallDispose(HandleRef selfPointer)
		{
			ScorePage.ScorePage_Dispose(selfPointer);
		}

		// Token: 0x0600131D RID: 4893 RVA: 0x0004EED4 File Offset: 0x0004D0D4
		internal Types.LeaderboardCollection GetCollection()
		{
			return ScorePage.ScorePage_Collection(base.SelfPtr());
		}

		// Token: 0x0600131E RID: 4894 RVA: 0x0004EEE4 File Offset: 0x0004D0E4
		private UIntPtr Length()
		{
			return ScorePage.ScorePage_Entries_Length(base.SelfPtr());
		}

		// Token: 0x0600131F RID: 4895 RVA: 0x0004EEF4 File Offset: 0x0004D0F4
		private NativeScoreEntry GetElement(UIntPtr index)
		{
			if (index.ToUInt64() >= this.Length().ToUInt64())
			{
				throw new ArgumentOutOfRangeException();
			}
			return new NativeScoreEntry(ScorePage.ScorePage_Entries_GetElement(base.SelfPtr(), index));
		}

		// Token: 0x06001320 RID: 4896 RVA: 0x0004EF34 File Offset: 0x0004D134
		public IEnumerator<NativeScoreEntry> GetEnumerator()
		{
			return PInvokeUtilities.ToEnumerator<NativeScoreEntry>(ScorePage.ScorePage_Entries_Length(base.SelfPtr()), (UIntPtr index) => this.GetElement(index));
		}

		// Token: 0x06001321 RID: 4897 RVA: 0x0004EF54 File Offset: 0x0004D154
		internal bool HasNextScorePage()
		{
			return ScorePage.ScorePage_HasNextScorePage(base.SelfPtr());
		}

		// Token: 0x06001322 RID: 4898 RVA: 0x0004EF64 File Offset: 0x0004D164
		internal bool HasPrevScorePage()
		{
			return ScorePage.ScorePage_HasPreviousScorePage(base.SelfPtr());
		}

		// Token: 0x06001323 RID: 4899 RVA: 0x0004EF74 File Offset: 0x0004D174
		internal NativeScorePageToken GetNextScorePageToken()
		{
			return new NativeScorePageToken(ScorePage.ScorePage_NextScorePageToken(base.SelfPtr()));
		}

		// Token: 0x06001324 RID: 4900 RVA: 0x0004EF88 File Offset: 0x0004D188
		internal NativeScorePageToken GetPreviousScorePageToken()
		{
			return new NativeScorePageToken(ScorePage.ScorePage_PreviousScorePageToken(base.SelfPtr()));
		}

		// Token: 0x06001325 RID: 4901 RVA: 0x0004EF9C File Offset: 0x0004D19C
		internal bool Valid()
		{
			return ScorePage.ScorePage_Valid(base.SelfPtr());
		}

		// Token: 0x06001326 RID: 4902 RVA: 0x0004EFAC File Offset: 0x0004D1AC
		internal Types.LeaderboardTimeSpan GetTimeSpan()
		{
			return ScorePage.ScorePage_TimeSpan(base.SelfPtr());
		}

		// Token: 0x06001327 RID: 4903 RVA: 0x0004EFBC File Offset: 0x0004D1BC
		internal Types.LeaderboardStart GetStart()
		{
			return ScorePage.ScorePage_Start(base.SelfPtr());
		}

		// Token: 0x06001328 RID: 4904 RVA: 0x0004EFCC File Offset: 0x0004D1CC
		internal string GetLeaderboardId()
		{
			return PInvokeUtilities.OutParamsToString((StringBuilder out_string, UIntPtr out_size) => ScorePage.ScorePage_LeaderboardId(base.SelfPtr(), out_string, out_size));
		}

		// Token: 0x06001329 RID: 4905 RVA: 0x0004EFE0 File Offset: 0x0004D1E0
		internal static NativeScorePage FromPointer(IntPtr pointer)
		{
			if (pointer.Equals(IntPtr.Zero))
			{
				return null;
			}
			return new NativeScorePage(pointer);
		}
	}
}
