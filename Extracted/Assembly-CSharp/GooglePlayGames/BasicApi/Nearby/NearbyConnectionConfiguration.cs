using System;
using GooglePlayGames.OurUtils;

namespace GooglePlayGames.BasicApi.Nearby
{
	// Token: 0x02000188 RID: 392
	public struct NearbyConnectionConfiguration
	{
		// Token: 0x06000CA4 RID: 3236 RVA: 0x00042974 File Offset: 0x00040B74
		public NearbyConnectionConfiguration(Action<InitializationStatus> callback, long localClientId)
		{
			this.mInitializationCallback = Misc.CheckNotNull<Action<InitializationStatus>>(callback);
			this.mLocalClientId = localClientId;
		}

		// Token: 0x1700019B RID: 411
		// (get) Token: 0x06000CA5 RID: 3237 RVA: 0x0004298C File Offset: 0x00040B8C
		public long LocalClientId
		{
			get
			{
				return this.mLocalClientId;
			}
		}

		// Token: 0x1700019C RID: 412
		// (get) Token: 0x06000CA6 RID: 3238 RVA: 0x00042994 File Offset: 0x00040B94
		public Action<InitializationStatus> InitializationCallback
		{
			get
			{
				return this.mInitializationCallback;
			}
		}

		// Token: 0x040009DC RID: 2524
		public const int MaxUnreliableMessagePayloadLength = 1168;

		// Token: 0x040009DD RID: 2525
		public const int MaxReliableMessagePayloadLength = 4096;

		// Token: 0x040009DE RID: 2526
		private readonly Action<InitializationStatus> mInitializationCallback;

		// Token: 0x040009DF RID: 2527
		private readonly long mLocalClientId;
	}
}
