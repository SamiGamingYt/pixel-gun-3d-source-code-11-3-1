using System;
using System.Runtime.InteropServices;
using System.Text;

namespace GooglePlayGames.Native.Cwrapper
{
	// Token: 0x020001DB RID: 475
	internal static class NearbyConnections
	{
		// Token: 0x06000F42 RID: 3906
		[DllImport("gpg")]
		internal static extern void NearbyConnections_StartDiscovery(HandleRef self, string service_id, long duration, IntPtr helper);

		// Token: 0x06000F43 RID: 3907
		[DllImport("gpg")]
		internal static extern void NearbyConnections_RejectConnectionRequest(HandleRef self, string remote_endpoint_id);

		// Token: 0x06000F44 RID: 3908
		[DllImport("gpg")]
		internal static extern void NearbyConnections_Disconnect(HandleRef self, string remote_endpoint_id);

		// Token: 0x06000F45 RID: 3909
		[DllImport("gpg")]
		internal static extern void NearbyConnections_SendUnreliableMessage(HandleRef self, string remote_endpoint_id, byte[] payload, UIntPtr payload_size);

		// Token: 0x06000F46 RID: 3910
		[DllImport("gpg")]
		internal static extern UIntPtr NearbyConnections_GetLocalDeviceId(HandleRef self, StringBuilder out_arg, UIntPtr out_size);

		// Token: 0x06000F47 RID: 3911
		[DllImport("gpg")]
		internal static extern void NearbyConnections_StopAdvertising(HandleRef self);

		// Token: 0x06000F48 RID: 3912
		[DllImport("gpg")]
		internal static extern void NearbyConnections_Dispose(HandleRef self);

		// Token: 0x06000F49 RID: 3913
		[DllImport("gpg")]
		internal static extern UIntPtr NearbyConnections_GetLocalEndpointId(HandleRef self, StringBuilder out_arg, UIntPtr out_size);

		// Token: 0x06000F4A RID: 3914
		[DllImport("gpg")]
		internal static extern void NearbyConnections_SendReliableMessage(HandleRef self, string remote_endpoint_id, byte[] payload, UIntPtr payload_size);

		// Token: 0x06000F4B RID: 3915
		[DllImport("gpg")]
		internal static extern void NearbyConnections_StopDiscovery(HandleRef self, string service_id);

		// Token: 0x06000F4C RID: 3916
		[DllImport("gpg")]
		internal static extern void NearbyConnections_SendConnectionRequest(HandleRef self, string name, string remote_endpoint_id, byte[] payload, UIntPtr payload_size, NearbyConnectionTypes.ConnectionResponseCallback callback, IntPtr callback_arg, IntPtr helper);

		// Token: 0x06000F4D RID: 3917
		[DllImport("gpg")]
		internal static extern void NearbyConnections_StartAdvertising(HandleRef self, string name, IntPtr[] app_identifiers, UIntPtr app_identifiers_size, long duration, NearbyConnectionTypes.StartAdvertisingCallback start_advertising_callback, IntPtr start_advertising_callback_arg, NearbyConnectionTypes.ConnectionRequestCallback request_callback, IntPtr request_callback_arg);

		// Token: 0x06000F4E RID: 3918
		[DllImport("gpg")]
		internal static extern void NearbyConnections_Stop(HandleRef self);

		// Token: 0x06000F4F RID: 3919
		[DllImport("gpg")]
		internal static extern void NearbyConnections_AcceptConnectionRequest(HandleRef self, string remote_endpoint_id, byte[] payload, UIntPtr payload_size, IntPtr helper);
	}
}
