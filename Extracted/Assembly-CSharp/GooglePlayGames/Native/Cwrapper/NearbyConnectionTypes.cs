using System;
using System.Runtime.InteropServices;
using System.Text;

namespace GooglePlayGames.Native.Cwrapper
{
	// Token: 0x020001D9 RID: 473
	internal static class NearbyConnectionTypes
	{
		// Token: 0x06000F2F RID: 3887
		[DllImport("gpg")]
		internal static extern void AppIdentifier_Dispose(HandleRef self);

		// Token: 0x06000F30 RID: 3888
		[DllImport("gpg")]
		internal static extern UIntPtr AppIdentifier_GetIdentifier(HandleRef self, StringBuilder out_arg, UIntPtr out_size);

		// Token: 0x06000F31 RID: 3889
		[DllImport("gpg")]
		internal static extern void StartAdvertisingResult_Dispose(HandleRef self);

		// Token: 0x06000F32 RID: 3890
		[DllImport("gpg")]
		[return: MarshalAs(UnmanagedType.I4)]
		internal static extern int StartAdvertisingResult_GetStatus(HandleRef self);

		// Token: 0x06000F33 RID: 3891
		[DllImport("gpg")]
		internal static extern UIntPtr StartAdvertisingResult_GetLocalEndpointName(HandleRef self, StringBuilder out_arg, UIntPtr out_size);

		// Token: 0x06000F34 RID: 3892
		[DllImport("gpg")]
		internal static extern void EndpointDetails_Dispose(HandleRef self);

		// Token: 0x06000F35 RID: 3893
		[DllImport("gpg")]
		internal static extern UIntPtr EndpointDetails_GetEndpointId(HandleRef self, StringBuilder out_arg, UIntPtr out_size);

		// Token: 0x06000F36 RID: 3894
		[DllImport("gpg")]
		internal static extern UIntPtr EndpointDetails_GetDeviceId(HandleRef self, StringBuilder out_arg, UIntPtr out_size);

		// Token: 0x06000F37 RID: 3895
		[DllImport("gpg")]
		internal static extern UIntPtr EndpointDetails_GetName(HandleRef self, StringBuilder out_arg, UIntPtr out_size);

		// Token: 0x06000F38 RID: 3896
		[DllImport("gpg")]
		internal static extern UIntPtr EndpointDetails_GetServiceId(HandleRef self, StringBuilder out_arg, UIntPtr out_size);

		// Token: 0x06000F39 RID: 3897
		[DllImport("gpg")]
		internal static extern void ConnectionRequest_Dispose(HandleRef self);

		// Token: 0x06000F3A RID: 3898
		[DllImport("gpg")]
		internal static extern UIntPtr ConnectionRequest_GetRemoteEndpointId(HandleRef self, StringBuilder out_arg, UIntPtr out_size);

		// Token: 0x06000F3B RID: 3899
		[DllImport("gpg")]
		internal static extern UIntPtr ConnectionRequest_GetRemoteDeviceId(HandleRef self, StringBuilder out_arg, UIntPtr out_size);

		// Token: 0x06000F3C RID: 3900
		[DllImport("gpg")]
		internal static extern UIntPtr ConnectionRequest_GetRemoteEndpointName(HandleRef self, StringBuilder out_arg, UIntPtr out_size);

		// Token: 0x06000F3D RID: 3901
		[DllImport("gpg")]
		internal static extern UIntPtr ConnectionRequest_GetPayload(HandleRef self, [In] [Out] byte[] out_arg, UIntPtr out_size);

		// Token: 0x06000F3E RID: 3902
		[DllImport("gpg")]
		internal static extern void ConnectionResponse_Dispose(HandleRef self);

		// Token: 0x06000F3F RID: 3903
		[DllImport("gpg")]
		internal static extern UIntPtr ConnectionResponse_GetRemoteEndpointId(HandleRef self, StringBuilder out_arg, UIntPtr out_size);

		// Token: 0x06000F40 RID: 3904
		[DllImport("gpg")]
		internal static extern NearbyConnectionTypes.ConnectionResponse_ResponseCode ConnectionResponse_GetStatus(HandleRef self);

		// Token: 0x06000F41 RID: 3905
		[DllImport("gpg")]
		internal static extern UIntPtr ConnectionResponse_GetPayload(HandleRef self, [In] [Out] byte[] out_arg, UIntPtr out_size);

		// Token: 0x020001DA RID: 474
		internal enum ConnectionResponse_ResponseCode
		{
			// Token: 0x04000AEF RID: 2799
			ACCEPTED = 1,
			// Token: 0x04000AF0 RID: 2800
			REJECTED,
			// Token: 0x04000AF1 RID: 2801
			ERROR_INTERNAL = -1,
			// Token: 0x04000AF2 RID: 2802
			ERROR_NETWORK_NOT_CONNECTED = -2,
			// Token: 0x04000AF3 RID: 2803
			ERROR_ENDPOINT_ALREADY_CONNECTED = -3,
			// Token: 0x04000AF4 RID: 2804
			ERROR_ENDPOINT_NOT_CONNECTED = -4
		}

		// Token: 0x020008B8 RID: 2232
		// (Invoke) Token: 0x06004F80 RID: 20352
		internal delegate void ConnectionRequestCallback(long arg0, IntPtr arg1, IntPtr arg2);

		// Token: 0x020008B9 RID: 2233
		// (Invoke) Token: 0x06004F84 RID: 20356
		internal delegate void StartAdvertisingCallback(long arg0, IntPtr arg1, IntPtr arg2);

		// Token: 0x020008BA RID: 2234
		// (Invoke) Token: 0x06004F88 RID: 20360
		internal delegate void ConnectionResponseCallback(long arg0, IntPtr arg1, IntPtr arg2);
	}
}
