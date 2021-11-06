using System;
using System.Runtime.InteropServices;
using System.Text;
using GooglePlayGames.BasicApi.Nearby;
using GooglePlayGames.Native.Cwrapper;

namespace GooglePlayGames.Native.PInvoke
{
	// Token: 0x02000249 RID: 585
	internal class NativeConnectionResponse : BaseReferenceHolder
	{
		// Token: 0x06001290 RID: 4752 RVA: 0x0004E020 File Offset: 0x0004C220
		internal NativeConnectionResponse(IntPtr pointer) : base(pointer)
		{
		}

		// Token: 0x06001291 RID: 4753 RVA: 0x0004E02C File Offset: 0x0004C22C
		internal string RemoteEndpointId()
		{
			return PInvokeUtilities.OutParamsToString((StringBuilder out_arg, UIntPtr out_size) => NearbyConnectionTypes.ConnectionResponse_GetRemoteEndpointId(base.SelfPtr(), out_arg, out_size));
		}

		// Token: 0x06001292 RID: 4754 RVA: 0x0004E040 File Offset: 0x0004C240
		internal NearbyConnectionTypes.ConnectionResponse_ResponseCode ResponseCode()
		{
			return NearbyConnectionTypes.ConnectionResponse_GetStatus(base.SelfPtr());
		}

		// Token: 0x06001293 RID: 4755 RVA: 0x0004E050 File Offset: 0x0004C250
		internal byte[] Payload()
		{
			return PInvokeUtilities.OutParamsToArray<byte>((byte[] out_arg, UIntPtr out_size) => NearbyConnectionTypes.ConnectionResponse_GetPayload(base.SelfPtr(), out_arg, out_size));
		}

		// Token: 0x06001294 RID: 4756 RVA: 0x0004E064 File Offset: 0x0004C264
		protected override void CallDispose(HandleRef selfPointer)
		{
			NearbyConnectionTypes.ConnectionResponse_Dispose(selfPointer);
		}

		// Token: 0x06001295 RID: 4757 RVA: 0x0004E06C File Offset: 0x0004C26C
		internal ConnectionResponse AsResponse(long localClientId)
		{
			NearbyConnectionTypes.ConnectionResponse_ResponseCode connectionResponse_ResponseCode = this.ResponseCode();
			switch (connectionResponse_ResponseCode + 4)
			{
			case (NearbyConnectionTypes.ConnectionResponse_ResponseCode)0:
				return ConnectionResponse.EndpointNotConnected(localClientId, this.RemoteEndpointId());
			case NearbyConnectionTypes.ConnectionResponse_ResponseCode.ACCEPTED:
				return ConnectionResponse.AlreadyConnected(localClientId, this.RemoteEndpointId());
			case NearbyConnectionTypes.ConnectionResponse_ResponseCode.REJECTED:
				return ConnectionResponse.NetworkNotConnected(localClientId, this.RemoteEndpointId());
			case (NearbyConnectionTypes.ConnectionResponse_ResponseCode)3:
				return ConnectionResponse.InternalError(localClientId, this.RemoteEndpointId());
			case (NearbyConnectionTypes.ConnectionResponse_ResponseCode)5:
				return ConnectionResponse.Accepted(localClientId, this.RemoteEndpointId(), this.Payload());
			case (NearbyConnectionTypes.ConnectionResponse_ResponseCode)6:
				return ConnectionResponse.Rejected(localClientId, this.RemoteEndpointId());
			}
			throw new InvalidOperationException("Found connection response of unknown type: " + this.ResponseCode());
		}

		// Token: 0x06001296 RID: 4758 RVA: 0x0004E118 File Offset: 0x0004C318
		internal static NativeConnectionResponse FromPointer(IntPtr pointer)
		{
			if (pointer == IntPtr.Zero)
			{
				return null;
			}
			return new NativeConnectionResponse(pointer);
		}
	}
}
