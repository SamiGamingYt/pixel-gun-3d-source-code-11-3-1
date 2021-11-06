using System;
using GooglePlayGames.OurUtils;

namespace GooglePlayGames.BasicApi.Nearby
{
	// Token: 0x02000180 RID: 384
	public struct ConnectionResponse
	{
		// Token: 0x06000C6C RID: 3180 RVA: 0x0004276C File Offset: 0x0004096C
		private ConnectionResponse(long localClientId, string remoteEndpointId, ConnectionResponse.Status code, byte[] payload)
		{
			this.mLocalClientId = localClientId;
			this.mRemoteEndpointId = Misc.CheckNotNull<string>(remoteEndpointId);
			this.mResponseStatus = code;
			this.mPayload = Misc.CheckNotNull<byte[]>(payload);
		}

		// Token: 0x17000193 RID: 403
		// (get) Token: 0x06000C6E RID: 3182 RVA: 0x000427A8 File Offset: 0x000409A8
		public long LocalClientId
		{
			get
			{
				return this.mLocalClientId;
			}
		}

		// Token: 0x17000194 RID: 404
		// (get) Token: 0x06000C6F RID: 3183 RVA: 0x000427B0 File Offset: 0x000409B0
		public string RemoteEndpointId
		{
			get
			{
				return this.mRemoteEndpointId;
			}
		}

		// Token: 0x17000195 RID: 405
		// (get) Token: 0x06000C70 RID: 3184 RVA: 0x000427B8 File Offset: 0x000409B8
		public ConnectionResponse.Status ResponseStatus
		{
			get
			{
				return this.mResponseStatus;
			}
		}

		// Token: 0x17000196 RID: 406
		// (get) Token: 0x06000C71 RID: 3185 RVA: 0x000427C0 File Offset: 0x000409C0
		public byte[] Payload
		{
			get
			{
				return this.mPayload;
			}
		}

		// Token: 0x06000C72 RID: 3186 RVA: 0x000427C8 File Offset: 0x000409C8
		public static ConnectionResponse Rejected(long localClientId, string remoteEndpointId)
		{
			return new ConnectionResponse(localClientId, remoteEndpointId, ConnectionResponse.Status.Rejected, ConnectionResponse.EmptyPayload);
		}

		// Token: 0x06000C73 RID: 3187 RVA: 0x000427D8 File Offset: 0x000409D8
		public static ConnectionResponse NetworkNotConnected(long localClientId, string remoteEndpointId)
		{
			return new ConnectionResponse(localClientId, remoteEndpointId, ConnectionResponse.Status.ErrorNetworkNotConnected, ConnectionResponse.EmptyPayload);
		}

		// Token: 0x06000C74 RID: 3188 RVA: 0x000427E8 File Offset: 0x000409E8
		public static ConnectionResponse InternalError(long localClientId, string remoteEndpointId)
		{
			return new ConnectionResponse(localClientId, remoteEndpointId, ConnectionResponse.Status.ErrorInternal, ConnectionResponse.EmptyPayload);
		}

		// Token: 0x06000C75 RID: 3189 RVA: 0x000427F8 File Offset: 0x000409F8
		public static ConnectionResponse EndpointNotConnected(long localClientId, string remoteEndpointId)
		{
			return new ConnectionResponse(localClientId, remoteEndpointId, ConnectionResponse.Status.ErrorEndpointNotConnected, ConnectionResponse.EmptyPayload);
		}

		// Token: 0x06000C76 RID: 3190 RVA: 0x00042808 File Offset: 0x00040A08
		public static ConnectionResponse Accepted(long localClientId, string remoteEndpointId, byte[] payload)
		{
			return new ConnectionResponse(localClientId, remoteEndpointId, ConnectionResponse.Status.Accepted, payload);
		}

		// Token: 0x06000C77 RID: 3191 RVA: 0x00042814 File Offset: 0x00040A14
		public static ConnectionResponse AlreadyConnected(long localClientId, string remoteEndpointId)
		{
			return new ConnectionResponse(localClientId, remoteEndpointId, ConnectionResponse.Status.ErrorAlreadyConnected, ConnectionResponse.EmptyPayload);
		}

		// Token: 0x040009C8 RID: 2504
		private static readonly byte[] EmptyPayload = new byte[0];

		// Token: 0x040009C9 RID: 2505
		private readonly long mLocalClientId;

		// Token: 0x040009CA RID: 2506
		private readonly string mRemoteEndpointId;

		// Token: 0x040009CB RID: 2507
		private readonly ConnectionResponse.Status mResponseStatus;

		// Token: 0x040009CC RID: 2508
		private readonly byte[] mPayload;

		// Token: 0x02000181 RID: 385
		public enum Status
		{
			// Token: 0x040009CE RID: 2510
			Accepted,
			// Token: 0x040009CF RID: 2511
			Rejected,
			// Token: 0x040009D0 RID: 2512
			ErrorInternal,
			// Token: 0x040009D1 RID: 2513
			ErrorNetworkNotConnected,
			// Token: 0x040009D2 RID: 2514
			ErrorEndpointNotConnected,
			// Token: 0x040009D3 RID: 2515
			ErrorAlreadyConnected
		}
	}
}
