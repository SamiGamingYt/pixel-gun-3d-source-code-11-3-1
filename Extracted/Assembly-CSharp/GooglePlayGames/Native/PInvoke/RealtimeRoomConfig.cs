using System;
using System.Runtime.InteropServices;
using GooglePlayGames.Native.Cwrapper;

namespace GooglePlayGames.Native.PInvoke
{
	// Token: 0x02000275 RID: 629
	internal class RealtimeRoomConfig : BaseReferenceHolder
	{
		// Token: 0x0600142C RID: 5164 RVA: 0x00051150 File Offset: 0x0004F350
		internal RealtimeRoomConfig(IntPtr selfPointer) : base(selfPointer)
		{
		}

		// Token: 0x0600142D RID: 5165 RVA: 0x0005115C File Offset: 0x0004F35C
		protected override void CallDispose(HandleRef selfPointer)
		{
			RealTimeRoomConfig.RealTimeRoomConfig_Dispose(selfPointer);
		}

		// Token: 0x0600142E RID: 5166 RVA: 0x00051164 File Offset: 0x0004F364
		internal static RealtimeRoomConfig FromPointer(IntPtr selfPointer)
		{
			if (selfPointer.Equals(IntPtr.Zero))
			{
				return null;
			}
			return new RealtimeRoomConfig(selfPointer);
		}
	}
}
