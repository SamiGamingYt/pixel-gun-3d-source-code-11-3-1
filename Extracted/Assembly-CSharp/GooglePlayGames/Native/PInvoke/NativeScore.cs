using System;
using System.Runtime.InteropServices;
using System.Text;
using GooglePlayGames.Native.Cwrapper;

namespace GooglePlayGames.Native.PInvoke
{
	// Token: 0x02000254 RID: 596
	internal class NativeScore : BaseReferenceHolder
	{
		// Token: 0x0600130B RID: 4875 RVA: 0x0004ED14 File Offset: 0x0004CF14
		internal NativeScore(IntPtr selfPtr) : base(selfPtr)
		{
		}

		// Token: 0x0600130C RID: 4876 RVA: 0x0004ED20 File Offset: 0x0004CF20
		protected override void CallDispose(HandleRef selfPointer)
		{
			Score.Score_Dispose(base.SelfPtr());
		}

		// Token: 0x0600130D RID: 4877 RVA: 0x0004ED30 File Offset: 0x0004CF30
		internal ulong GetDate()
		{
			return ulong.MaxValue;
		}

		// Token: 0x0600130E RID: 4878 RVA: 0x0004ED34 File Offset: 0x0004CF34
		internal string GetMetadata()
		{
			return PInvokeUtilities.OutParamsToString((StringBuilder out_string, UIntPtr out_size) => Score.Score_Metadata(base.SelfPtr(), out_string, out_size));
		}

		// Token: 0x0600130F RID: 4879 RVA: 0x0004ED48 File Offset: 0x0004CF48
		internal ulong GetRank()
		{
			return Score.Score_Rank(base.SelfPtr());
		}

		// Token: 0x06001310 RID: 4880 RVA: 0x0004ED58 File Offset: 0x0004CF58
		internal ulong GetValue()
		{
			return Score.Score_Value(base.SelfPtr());
		}

		// Token: 0x06001311 RID: 4881 RVA: 0x0004ED68 File Offset: 0x0004CF68
		internal PlayGamesScore AsScore(string leaderboardId, string selfPlayerId)
		{
			DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
			ulong num = this.GetDate();
			if (num == 18446744073709551615UL)
			{
				num = 0UL;
			}
			DateTime date = dateTime.AddMilliseconds(num);
			return new PlayGamesScore(date, leaderboardId, this.GetRank(), selfPlayerId, this.GetValue(), this.GetMetadata());
		}

		// Token: 0x06001312 RID: 4882 RVA: 0x0004EDC4 File Offset: 0x0004CFC4
		internal static NativeScore FromPointer(IntPtr pointer)
		{
			if (pointer.Equals(IntPtr.Zero))
			{
				return null;
			}
			return new NativeScore(pointer);
		}

		// Token: 0x04000C04 RID: 3076
		private const ulong MinusOne = 18446744073709551615UL;
	}
}
