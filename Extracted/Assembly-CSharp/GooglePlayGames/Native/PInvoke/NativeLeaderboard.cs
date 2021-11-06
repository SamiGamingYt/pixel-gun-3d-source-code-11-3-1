using System;
using System.Runtime.InteropServices;
using System.Text;
using GooglePlayGames.Native.Cwrapper;

namespace GooglePlayGames.Native.PInvoke
{
	// Token: 0x0200024D RID: 589
	internal class NativeLeaderboard : BaseReferenceHolder
	{
		// Token: 0x060012B8 RID: 4792 RVA: 0x0004E48C File Offset: 0x0004C68C
		internal NativeLeaderboard(IntPtr selfPtr) : base(selfPtr)
		{
		}

		// Token: 0x060012B9 RID: 4793 RVA: 0x0004E498 File Offset: 0x0004C698
		protected override void CallDispose(HandleRef selfPointer)
		{
			Leaderboard.Leaderboard_Dispose(selfPointer);
		}

		// Token: 0x060012BA RID: 4794 RVA: 0x0004E4A0 File Offset: 0x0004C6A0
		internal string Title()
		{
			return PInvokeUtilities.OutParamsToString((StringBuilder out_string, UIntPtr out_size) => Leaderboard.Leaderboard_Name(base.SelfPtr(), out_string, out_size));
		}

		// Token: 0x060012BB RID: 4795 RVA: 0x0004E4B4 File Offset: 0x0004C6B4
		internal static NativeLeaderboard FromPointer(IntPtr pointer)
		{
			if (pointer.Equals(IntPtr.Zero))
			{
				return null;
			}
			return new NativeLeaderboard(pointer);
		}
	}
}
