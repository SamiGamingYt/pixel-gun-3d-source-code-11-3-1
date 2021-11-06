using System;

namespace GooglePlayGames.BasicApi
{
	// Token: 0x0200019D RID: 413
	public class ScorePageToken
	{
		// Token: 0x06000D10 RID: 3344 RVA: 0x00042E34 File Offset: 0x00041034
		internal ScorePageToken(object internalObject, string id, LeaderboardCollection collection, LeaderboardTimeSpan timespan)
		{
			this.mInternalObject = internalObject;
			this.mId = id;
			this.mCollection = collection;
			this.mTimespan = timespan;
		}

		// Token: 0x170001CB RID: 459
		// (get) Token: 0x06000D11 RID: 3345 RVA: 0x00042E5C File Offset: 0x0004105C
		public LeaderboardCollection Collection
		{
			get
			{
				return this.mCollection;
			}
		}

		// Token: 0x170001CC RID: 460
		// (get) Token: 0x06000D12 RID: 3346 RVA: 0x00042E64 File Offset: 0x00041064
		public LeaderboardTimeSpan TimeSpan
		{
			get
			{
				return this.mTimespan;
			}
		}

		// Token: 0x170001CD RID: 461
		// (get) Token: 0x06000D13 RID: 3347 RVA: 0x00042E6C File Offset: 0x0004106C
		public string LeaderboardId
		{
			get
			{
				return this.mId;
			}
		}

		// Token: 0x170001CE RID: 462
		// (get) Token: 0x06000D14 RID: 3348 RVA: 0x00042E74 File Offset: 0x00041074
		internal object InternalObject
		{
			get
			{
				return this.mInternalObject;
			}
		}

		// Token: 0x04000A46 RID: 2630
		private string mId;

		// Token: 0x04000A47 RID: 2631
		private object mInternalObject;

		// Token: 0x04000A48 RID: 2632
		private LeaderboardCollection mCollection;

		// Token: 0x04000A49 RID: 2633
		private LeaderboardTimeSpan mTimespan;
	}
}
