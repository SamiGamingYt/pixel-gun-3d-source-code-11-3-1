using System;
using System.Collections.Generic;

namespace GooglePlayGames.BasicApi.Nearby
{
	// Token: 0x02000184 RID: 388
	public interface INearbyConnectionClient
	{
		// Token: 0x06000C8F RID: 3215
		int MaxUnreliableMessagePayloadLength();

		// Token: 0x06000C90 RID: 3216
		int MaxReliableMessagePayloadLength();

		// Token: 0x06000C91 RID: 3217
		void SendReliable(List<string> recipientEndpointIds, byte[] payload);

		// Token: 0x06000C92 RID: 3218
		void SendUnreliable(List<string> recipientEndpointIds, byte[] payload);

		// Token: 0x06000C93 RID: 3219
		void StartAdvertising(string name, List<string> appIdentifiers, TimeSpan? advertisingDuration, Action<AdvertisingResult> resultCallback, Action<ConnectionRequest> connectionRequestCallback);

		// Token: 0x06000C94 RID: 3220
		void StopAdvertising();

		// Token: 0x06000C95 RID: 3221
		void SendConnectionRequest(string name, string remoteEndpointId, byte[] payload, Action<ConnectionResponse> responseCallback, IMessageListener listener);

		// Token: 0x06000C96 RID: 3222
		void AcceptConnectionRequest(string remoteEndpointId, byte[] payload, IMessageListener listener);

		// Token: 0x06000C97 RID: 3223
		void StartDiscovery(string serviceId, TimeSpan? advertisingTimeout, IDiscoveryListener listener);

		// Token: 0x06000C98 RID: 3224
		void StopDiscovery(string serviceId);

		// Token: 0x06000C99 RID: 3225
		void RejectConnectionRequest(string requestingEndpointId);

		// Token: 0x06000C9A RID: 3226
		void DisconnectFromEndpoint(string remoteEndpointId);

		// Token: 0x06000C9B RID: 3227
		void StopAllConnections();

		// Token: 0x06000C9C RID: 3228
		string LocalEndpointId();

		// Token: 0x06000C9D RID: 3229
		string LocalDeviceId();

		// Token: 0x06000C9E RID: 3230
		string GetAppBundleId();

		// Token: 0x06000C9F RID: 3231
		string GetServiceId();
	}
}
