using System;
using GooglePlayGames.OurUtils;

namespace GooglePlayGames.BasicApi.Nearby
{
	// Token: 0x02000183 RID: 387
	public struct EndpointDetails
	{
		// Token: 0x06000C8A RID: 3210 RVA: 0x00042920 File Offset: 0x00040B20
		public EndpointDetails(string endpointId, string deviceId, string name, string serviceId)
		{
			this.mEndpointId = Misc.CheckNotNull<string>(endpointId);
			this.mDeviceId = Misc.CheckNotNull<string>(deviceId);
			this.mName = Misc.CheckNotNull<string>(name);
			this.mServiceId = Misc.CheckNotNull<string>(serviceId);
		}

		// Token: 0x17000197 RID: 407
		// (get) Token: 0x06000C8B RID: 3211 RVA: 0x00042954 File Offset: 0x00040B54
		public string EndpointId
		{
			get
			{
				return this.mEndpointId;
			}
		}

		// Token: 0x17000198 RID: 408
		// (get) Token: 0x06000C8C RID: 3212 RVA: 0x0004295C File Offset: 0x00040B5C
		public string DeviceId
		{
			get
			{
				return this.mDeviceId;
			}
		}

		// Token: 0x17000199 RID: 409
		// (get) Token: 0x06000C8D RID: 3213 RVA: 0x00042964 File Offset: 0x00040B64
		public string Name
		{
			get
			{
				return this.mName;
			}
		}

		// Token: 0x1700019A RID: 410
		// (get) Token: 0x06000C8E RID: 3214 RVA: 0x0004296C File Offset: 0x00040B6C
		public string ServiceId
		{
			get
			{
				return this.mServiceId;
			}
		}

		// Token: 0x040009D4 RID: 2516
		private readonly string mEndpointId;

		// Token: 0x040009D5 RID: 2517
		private readonly string mDeviceId;

		// Token: 0x040009D6 RID: 2518
		private readonly string mName;

		// Token: 0x040009D7 RID: 2519
		private readonly string mServiceId;
	}
}
