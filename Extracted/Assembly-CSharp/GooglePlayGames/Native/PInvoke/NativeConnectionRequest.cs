using System;
using System.Runtime.InteropServices;
using System.Text;
using GooglePlayGames.BasicApi.Nearby;
using GooglePlayGames.Native.Cwrapper;

namespace GooglePlayGames.Native.PInvoke
{
	// Token: 0x02000248 RID: 584
	internal class NativeConnectionRequest : BaseReferenceHolder
	{
		// Token: 0x06001284 RID: 4740 RVA: 0x0004DF2C File Offset: 0x0004C12C
		internal NativeConnectionRequest(IntPtr pointer) : base(pointer)
		{
		}

		// Token: 0x06001285 RID: 4741 RVA: 0x0004DF38 File Offset: 0x0004C138
		internal string RemoteEndpointId()
		{
			return PInvokeUtilities.OutParamsToString((StringBuilder out_arg, UIntPtr out_size) => NearbyConnectionTypes.ConnectionRequest_GetRemoteEndpointId(base.SelfPtr(), out_arg, out_size));
		}

		// Token: 0x06001286 RID: 4742 RVA: 0x0004DF4C File Offset: 0x0004C14C
		internal string RemoteDeviceId()
		{
			return PInvokeUtilities.OutParamsToString((StringBuilder out_arg, UIntPtr out_size) => NearbyConnectionTypes.ConnectionRequest_GetRemoteDeviceId(base.SelfPtr(), out_arg, out_size));
		}

		// Token: 0x06001287 RID: 4743 RVA: 0x0004DF60 File Offset: 0x0004C160
		internal string RemoteEndpointName()
		{
			return PInvokeUtilities.OutParamsToString((StringBuilder out_arg, UIntPtr out_size) => NearbyConnectionTypes.ConnectionRequest_GetRemoteEndpointName(base.SelfPtr(), out_arg, out_size));
		}

		// Token: 0x06001288 RID: 4744 RVA: 0x0004DF74 File Offset: 0x0004C174
		internal byte[] Payload()
		{
			return PInvokeUtilities.OutParamsToArray<byte>((byte[] out_arg, UIntPtr out_size) => NearbyConnectionTypes.ConnectionRequest_GetPayload(base.SelfPtr(), out_arg, out_size));
		}

		// Token: 0x06001289 RID: 4745 RVA: 0x0004DF88 File Offset: 0x0004C188
		protected override void CallDispose(HandleRef selfPointer)
		{
			NearbyConnectionTypes.ConnectionRequest_Dispose(selfPointer);
		}

		// Token: 0x0600128A RID: 4746 RVA: 0x0004DF90 File Offset: 0x0004C190
		internal ConnectionRequest AsRequest()
		{
			ConnectionRequest result = new ConnectionRequest(this.RemoteEndpointId(), this.RemoteDeviceId(), this.RemoteEndpointName(), NearbyConnectionsManager.ServiceId, this.Payload());
			return result;
		}

		// Token: 0x0600128B RID: 4747 RVA: 0x0004DFC4 File Offset: 0x0004C1C4
		internal static NativeConnectionRequest FromPointer(IntPtr pointer)
		{
			if (pointer == IntPtr.Zero)
			{
				return null;
			}
			return new NativeConnectionRequest(pointer);
		}
	}
}
