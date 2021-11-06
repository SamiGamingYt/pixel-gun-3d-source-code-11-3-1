using System;
using GooglePlayGames.OurUtils;

namespace GooglePlayGames.BasicApi.Nearby
{
	// Token: 0x0200017E RID: 382
	public struct AdvertisingResult
	{
		// Token: 0x06000C65 RID: 3173 RVA: 0x000426FC File Offset: 0x000408FC
		public AdvertisingResult(ResponseStatus status, string localEndpointName)
		{
			this.mStatus = status;
			this.mLocalEndpointName = Misc.CheckNotNull<string>(localEndpointName);
		}

		// Token: 0x1700018E RID: 398
		// (get) Token: 0x06000C66 RID: 3174 RVA: 0x00042714 File Offset: 0x00040914
		public bool Succeeded
		{
			get
			{
				return this.mStatus == ResponseStatus.Success;
			}
		}

		// Token: 0x1700018F RID: 399
		// (get) Token: 0x06000C67 RID: 3175 RVA: 0x00042720 File Offset: 0x00040920
		public ResponseStatus Status
		{
			get
			{
				return this.mStatus;
			}
		}

		// Token: 0x17000190 RID: 400
		// (get) Token: 0x06000C68 RID: 3176 RVA: 0x00042728 File Offset: 0x00040928
		public string LocalEndpointName
		{
			get
			{
				return this.mLocalEndpointName;
			}
		}

		// Token: 0x040009C4 RID: 2500
		private readonly ResponseStatus mStatus;

		// Token: 0x040009C5 RID: 2501
		private readonly string mLocalEndpointName;
	}
}
