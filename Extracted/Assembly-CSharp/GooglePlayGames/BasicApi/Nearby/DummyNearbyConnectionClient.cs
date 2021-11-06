using System;
using System.Collections.Generic;
using UnityEngine;

namespace GooglePlayGames.BasicApi.Nearby
{
	// Token: 0x02000182 RID: 386
	public class DummyNearbyConnectionClient : INearbyConnectionClient
	{
		// Token: 0x06000C79 RID: 3193 RVA: 0x0004282C File Offset: 0x00040A2C
		public int MaxUnreliableMessagePayloadLength()
		{
			return 1168;
		}

		// Token: 0x06000C7A RID: 3194 RVA: 0x00042834 File Offset: 0x00040A34
		public int MaxReliableMessagePayloadLength()
		{
			return 4096;
		}

		// Token: 0x06000C7B RID: 3195 RVA: 0x0004283C File Offset: 0x00040A3C
		public void SendReliable(List<string> recipientEndpointIds, byte[] payload)
		{
			Debug.LogError("SendReliable called from dummy implementation");
		}

		// Token: 0x06000C7C RID: 3196 RVA: 0x00042848 File Offset: 0x00040A48
		public void SendUnreliable(List<string> recipientEndpointIds, byte[] payload)
		{
			Debug.LogError("SendUnreliable called from dummy implementation");
		}

		// Token: 0x06000C7D RID: 3197 RVA: 0x00042854 File Offset: 0x00040A54
		public void StartAdvertising(string name, List<string> appIdentifiers, TimeSpan? advertisingDuration, Action<AdvertisingResult> resultCallback, Action<ConnectionRequest> connectionRequestCallback)
		{
			AdvertisingResult obj = new AdvertisingResult(ResponseStatus.LicenseCheckFailed, string.Empty);
			resultCallback(obj);
		}

		// Token: 0x06000C7E RID: 3198 RVA: 0x00042878 File Offset: 0x00040A78
		public void StopAdvertising()
		{
			Debug.LogError("StopAvertising in dummy implementation called");
		}

		// Token: 0x06000C7F RID: 3199 RVA: 0x00042884 File Offset: 0x00040A84
		public void SendConnectionRequest(string name, string remoteEndpointId, byte[] payload, Action<ConnectionResponse> responseCallback, IMessageListener listener)
		{
			Debug.LogError("SendConnectionRequest called from dummy implementation");
			if (responseCallback != null)
			{
				ConnectionResponse obj = ConnectionResponse.Rejected(0L, string.Empty);
				responseCallback(obj);
			}
		}

		// Token: 0x06000C80 RID: 3200 RVA: 0x000428B8 File Offset: 0x00040AB8
		public void AcceptConnectionRequest(string remoteEndpointId, byte[] payload, IMessageListener listener)
		{
			Debug.LogError("AcceptConnectionRequest in dummy implementation called");
		}

		// Token: 0x06000C81 RID: 3201 RVA: 0x000428C4 File Offset: 0x00040AC4
		public void StartDiscovery(string serviceId, TimeSpan? advertisingTimeout, IDiscoveryListener listener)
		{
			Debug.LogError("StartDiscovery in dummy implementation called");
		}

		// Token: 0x06000C82 RID: 3202 RVA: 0x000428D0 File Offset: 0x00040AD0
		public void StopDiscovery(string serviceId)
		{
			Debug.LogError("StopDiscovery in dummy implementation called");
		}

		// Token: 0x06000C83 RID: 3203 RVA: 0x000428DC File Offset: 0x00040ADC
		public void RejectConnectionRequest(string requestingEndpointId)
		{
			Debug.LogError("RejectConnectionRequest in dummy implementation called");
		}

		// Token: 0x06000C84 RID: 3204 RVA: 0x000428E8 File Offset: 0x00040AE8
		public void DisconnectFromEndpoint(string remoteEndpointId)
		{
			Debug.LogError("DisconnectFromEndpoint in dummy implementation called");
		}

		// Token: 0x06000C85 RID: 3205 RVA: 0x000428F4 File Offset: 0x00040AF4
		public void StopAllConnections()
		{
			Debug.LogError("StopAllConnections in dummy implementation called");
		}

		// Token: 0x06000C86 RID: 3206 RVA: 0x00042900 File Offset: 0x00040B00
		public string LocalEndpointId()
		{
			return string.Empty;
		}

		// Token: 0x06000C87 RID: 3207 RVA: 0x00042908 File Offset: 0x00040B08
		public string LocalDeviceId()
		{
			return "DummyDevice";
		}

		// Token: 0x06000C88 RID: 3208 RVA: 0x00042910 File Offset: 0x00040B10
		public string GetAppBundleId()
		{
			return "dummy.bundle.id";
		}

		// Token: 0x06000C89 RID: 3209 RVA: 0x00042918 File Offset: 0x00040B18
		public string GetServiceId()
		{
			return "dummy.service.id";
		}
	}
}
