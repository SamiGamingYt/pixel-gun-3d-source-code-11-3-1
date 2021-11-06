using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using GooglePlayGames.Native.Cwrapper;

namespace GooglePlayGames.Native.PInvoke
{
	// Token: 0x02000253 RID: 595
	internal class NativeRealTimeRoom : BaseReferenceHolder
	{
		// Token: 0x06001302 RID: 4866 RVA: 0x0004EC58 File Offset: 0x0004CE58
		internal NativeRealTimeRoom(IntPtr selfPointer) : base(selfPointer)
		{
		}

		// Token: 0x06001303 RID: 4867 RVA: 0x0004EC64 File Offset: 0x0004CE64
		internal string Id()
		{
			return PInvokeUtilities.OutParamsToString((StringBuilder out_string, UIntPtr size) => RealTimeRoom.RealTimeRoom_Id(base.SelfPtr(), out_string, size));
		}

		// Token: 0x06001304 RID: 4868 RVA: 0x0004EC78 File Offset: 0x0004CE78
		internal IEnumerable<MultiplayerParticipant> Participants()
		{
			return PInvokeUtilities.ToEnumerable<MultiplayerParticipant>(RealTimeRoom.RealTimeRoom_Participants_Length(base.SelfPtr()), (UIntPtr index) => new MultiplayerParticipant(RealTimeRoom.RealTimeRoom_Participants_GetElement(base.SelfPtr(), index)));
		}

		// Token: 0x06001305 RID: 4869 RVA: 0x0004EC98 File Offset: 0x0004CE98
		internal uint ParticipantCount()
		{
			return RealTimeRoom.RealTimeRoom_Participants_Length(base.SelfPtr()).ToUInt32();
		}

		// Token: 0x06001306 RID: 4870 RVA: 0x0004ECB8 File Offset: 0x0004CEB8
		internal Types.RealTimeRoomStatus Status()
		{
			return RealTimeRoom.RealTimeRoom_Status(base.SelfPtr());
		}

		// Token: 0x06001307 RID: 4871 RVA: 0x0004ECC8 File Offset: 0x0004CEC8
		protected override void CallDispose(HandleRef selfPointer)
		{
			RealTimeRoom.RealTimeRoom_Dispose(selfPointer);
		}

		// Token: 0x06001308 RID: 4872 RVA: 0x0004ECD0 File Offset: 0x0004CED0
		internal static NativeRealTimeRoom FromPointer(IntPtr selfPointer)
		{
			if (selfPointer.Equals(IntPtr.Zero))
			{
				return null;
			}
			return new NativeRealTimeRoom(selfPointer);
		}
	}
}
