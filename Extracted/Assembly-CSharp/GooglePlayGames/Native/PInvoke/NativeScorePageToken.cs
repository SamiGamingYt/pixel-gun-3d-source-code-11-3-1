using System;
using System.Runtime.InteropServices;
using GooglePlayGames.Native.Cwrapper;

namespace GooglePlayGames.Native.PInvoke
{
	// Token: 0x02000257 RID: 599
	internal class NativeScorePageToken : BaseReferenceHolder
	{
		// Token: 0x0600132C RID: 4908 RVA: 0x0004F01C File Offset: 0x0004D21C
		internal NativeScorePageToken(IntPtr selfPtr) : base(selfPtr)
		{
		}

		// Token: 0x0600132D RID: 4909 RVA: 0x0004F028 File Offset: 0x0004D228
		protected override void CallDispose(HandleRef selfPointer)
		{
			ScorePage.ScorePage_ScorePageToken_Dispose(selfPointer);
		}
	}
}
