using System;
using System.Collections.Generic;
using UnityEngine.SocialPlatforms;

namespace GooglePlayGames.BasicApi
{
	// Token: 0x02000170 RID: 368
	public class LeaderboardScoreData
	{
		// Token: 0x06000BFB RID: 3067 RVA: 0x00041FA4 File Offset: 0x000401A4
		internal LeaderboardScoreData(string leaderboardId)
		{
			this.mId = leaderboardId;
		}

		// Token: 0x06000BFC RID: 3068 RVA: 0x00041FC0 File Offset: 0x000401C0
		internal LeaderboardScoreData(string leaderboardId, ResponseStatus status)
		{
			this.mId = leaderboardId;
			this.mStatus = status;
		}

		// Token: 0x1700016D RID: 365
		// (get) Token: 0x06000BFD RID: 3069 RVA: 0x00041FE4 File Offset: 0x000401E4
		public bool Valid
		{
			get
			{
				return this.mStatus == ResponseStatus.Success || this.mStatus == ResponseStatus.SuccessWithStale;
			}
		}

		// Token: 0x1700016E RID: 366
		// (get) Token: 0x06000BFE RID: 3070 RVA: 0x00042000 File Offset: 0x00040200
		// (set) Token: 0x06000BFF RID: 3071 RVA: 0x00042008 File Offset: 0x00040208
		public ResponseStatus Status
		{
			get
			{
				return this.mStatus;
			}
			internal set
			{
				this.mStatus = value;
			}
		}

		// Token: 0x1700016F RID: 367
		// (get) Token: 0x06000C00 RID: 3072 RVA: 0x00042014 File Offset: 0x00040214
		// (set) Token: 0x06000C01 RID: 3073 RVA: 0x0004201C File Offset: 0x0004021C
		public ulong ApproximateCount
		{
			get
			{
				return this.mApproxCount;
			}
			internal set
			{
				this.mApproxCount = value;
			}
		}

		// Token: 0x17000170 RID: 368
		// (get) Token: 0x06000C02 RID: 3074 RVA: 0x00042028 File Offset: 0x00040228
		// (set) Token: 0x06000C03 RID: 3075 RVA: 0x00042030 File Offset: 0x00040230
		public string Title
		{
			get
			{
				return this.mTitle;
			}
			internal set
			{
				this.mTitle = value;
			}
		}

		// Token: 0x17000171 RID: 369
		// (get) Token: 0x06000C04 RID: 3076 RVA: 0x0004203C File Offset: 0x0004023C
		// (set) Token: 0x06000C05 RID: 3077 RVA: 0x00042044 File Offset: 0x00040244
		public string Id
		{
			get
			{
				return this.mId;
			}
			internal set
			{
				this.mId = value;
			}
		}

		// Token: 0x17000172 RID: 370
		// (get) Token: 0x06000C06 RID: 3078 RVA: 0x00042050 File Offset: 0x00040250
		// (set) Token: 0x06000C07 RID: 3079 RVA: 0x00042058 File Offset: 0x00040258
		public IScore PlayerScore
		{
			get
			{
				return this.mPlayerScore;
			}
			internal set
			{
				this.mPlayerScore = value;
			}
		}

		// Token: 0x17000173 RID: 371
		// (get) Token: 0x06000C08 RID: 3080 RVA: 0x00042064 File Offset: 0x00040264
		public IScore[] Scores
		{
			get
			{
				return this.mScores.ToArray();
			}
		}

		// Token: 0x06000C09 RID: 3081 RVA: 0x00042074 File Offset: 0x00040274
		internal int AddScore(PlayGamesScore score)
		{
			this.mScores.Add(score);
			return this.mScores.Count;
		}

		// Token: 0x17000174 RID: 372
		// (get) Token: 0x06000C0A RID: 3082 RVA: 0x00042090 File Offset: 0x00040290
		// (set) Token: 0x06000C0B RID: 3083 RVA: 0x00042098 File Offset: 0x00040298
		public ScorePageToken PrevPageToken
		{
			get
			{
				return this.mPrevPage;
			}
			internal set
			{
				this.mPrevPage = value;
			}
		}

		// Token: 0x17000175 RID: 373
		// (get) Token: 0x06000C0C RID: 3084 RVA: 0x000420A4 File Offset: 0x000402A4
		// (set) Token: 0x06000C0D RID: 3085 RVA: 0x000420AC File Offset: 0x000402AC
		public ScorePageToken NextPageToken
		{
			get
			{
				return this.mNextPage;
			}
			internal set
			{
				this.mNextPage = value;
			}
		}

		// Token: 0x06000C0E RID: 3086 RVA: 0x000420B8 File Offset: 0x000402B8
		public override string ToString()
		{
			return string.Format("[LeaderboardScoreData: mId={0},  mStatus={1}, mApproxCount={2}, mTitle={3}]", new object[]
			{
				this.mId,
				this.mStatus,
				this.mApproxCount,
				this.mTitle
			});
		}

		// Token: 0x04000982 RID: 2434
		private string mId;

		// Token: 0x04000983 RID: 2435
		private ResponseStatus mStatus;

		// Token: 0x04000984 RID: 2436
		private ulong mApproxCount;

		// Token: 0x04000985 RID: 2437
		private string mTitle;

		// Token: 0x04000986 RID: 2438
		private IScore mPlayerScore;

		// Token: 0x04000987 RID: 2439
		private ScorePageToken mPrevPage;

		// Token: 0x04000988 RID: 2440
		private ScorePageToken mNextPage;

		// Token: 0x04000989 RID: 2441
		private List<PlayGamesScore> mScores = new List<PlayGamesScore>();
	}
}
