using System;
using GooglePlayGames.OurUtils;

namespace GooglePlayGames.BasicApi.Nearby
{
	// Token: 0x0200017F RID: 383
	public struct ConnectionRequest
	{
		// Token: 0x06000C69 RID: 3177 RVA: 0x00042730 File Offset: 0x00040930
		public ConnectionRequest(string remoteEndpointId, string remoteDeviceId, string remoteEndpointName, string serviceId, byte[] payload)
		{
			Logger.d("Constructing ConnectionRequest");
			this.mRemoteEndpoint = new EndpointDetails(remoteEndpointId, remoteDeviceId, remoteEndpointName, serviceId);
			this.mPayload = Misc.CheckNotNull<byte[]>(payload);
		}

		// Token: 0x17000191 RID: 401
		// (get) Token: 0x06000C6A RID: 3178 RVA: 0x0004275C File Offset: 0x0004095C
		public EndpointDetails RemoteEndpoint
		{
			get
			{
				return this.mRemoteEndpoint;
			}
		}

		// Token: 0x17000192 RID: 402
		// (get) Token: 0x06000C6B RID: 3179 RVA: 0x00042764 File Offset: 0x00040964
		public byte[] Payload
		{
			get
			{
				return this.mPayload;
			}
		}

		// Token: 0x040009C6 RID: 2502
		private readonly EndpointDetails mRemoteEndpoint;

		// Token: 0x040009C7 RID: 2503
		private readonly byte[] mPayload;
	}
}
