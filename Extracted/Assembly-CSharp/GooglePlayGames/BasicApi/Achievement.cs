using System;

namespace GooglePlayGames.BasicApi
{
	// Token: 0x02000163 RID: 355
	public class Achievement
	{
		// Token: 0x06000B8D RID: 2957 RVA: 0x00041B90 File Offset: 0x0003FD90
		public override string ToString()
		{
			return string.Format("[Achievement] id={0}, name={1}, desc={2}, type={3}, revealed={4}, unlocked={5}, steps={6}/{7}", new object[]
			{
				this.mId,
				this.mName,
				this.mDescription,
				(!this.mIsIncremental) ? "STANDARD" : "INCREMENTAL",
				this.mIsRevealed,
				this.mIsUnlocked,
				this.mCurrentSteps,
				this.mTotalSteps
			});
		}

		// Token: 0x1700015B RID: 347
		// (get) Token: 0x06000B8E RID: 2958 RVA: 0x00041C20 File Offset: 0x0003FE20
		// (set) Token: 0x06000B8F RID: 2959 RVA: 0x00041C28 File Offset: 0x0003FE28
		public bool IsIncremental
		{
			get
			{
				return this.mIsIncremental;
			}
			set
			{
				this.mIsIncremental = value;
			}
		}

		// Token: 0x1700015C RID: 348
		// (get) Token: 0x06000B90 RID: 2960 RVA: 0x00041C34 File Offset: 0x0003FE34
		// (set) Token: 0x06000B91 RID: 2961 RVA: 0x00041C3C File Offset: 0x0003FE3C
		public int CurrentSteps
		{
			get
			{
				return this.mCurrentSteps;
			}
			set
			{
				this.mCurrentSteps = value;
			}
		}

		// Token: 0x1700015D RID: 349
		// (get) Token: 0x06000B92 RID: 2962 RVA: 0x00041C48 File Offset: 0x0003FE48
		// (set) Token: 0x06000B93 RID: 2963 RVA: 0x00041C50 File Offset: 0x0003FE50
		public int TotalSteps
		{
			get
			{
				return this.mTotalSteps;
			}
			set
			{
				this.mTotalSteps = value;
			}
		}

		// Token: 0x1700015E RID: 350
		// (get) Token: 0x06000B94 RID: 2964 RVA: 0x00041C5C File Offset: 0x0003FE5C
		// (set) Token: 0x06000B95 RID: 2965 RVA: 0x00041C64 File Offset: 0x0003FE64
		public bool IsUnlocked
		{
			get
			{
				return this.mIsUnlocked;
			}
			set
			{
				this.mIsUnlocked = value;
			}
		}

		// Token: 0x1700015F RID: 351
		// (get) Token: 0x06000B96 RID: 2966 RVA: 0x00041C70 File Offset: 0x0003FE70
		// (set) Token: 0x06000B97 RID: 2967 RVA: 0x00041C78 File Offset: 0x0003FE78
		public bool IsRevealed
		{
			get
			{
				return this.mIsRevealed;
			}
			set
			{
				this.mIsRevealed = value;
			}
		}

		// Token: 0x17000160 RID: 352
		// (get) Token: 0x06000B98 RID: 2968 RVA: 0x00041C84 File Offset: 0x0003FE84
		// (set) Token: 0x06000B99 RID: 2969 RVA: 0x00041C8C File Offset: 0x0003FE8C
		public string Id
		{
			get
			{
				return this.mId;
			}
			set
			{
				this.mId = value;
			}
		}

		// Token: 0x17000161 RID: 353
		// (get) Token: 0x06000B9A RID: 2970 RVA: 0x00041C98 File Offset: 0x0003FE98
		// (set) Token: 0x06000B9B RID: 2971 RVA: 0x00041CA0 File Offset: 0x0003FEA0
		public string Description
		{
			get
			{
				return this.mDescription;
			}
			set
			{
				this.mDescription = value;
			}
		}

		// Token: 0x17000162 RID: 354
		// (get) Token: 0x06000B9C RID: 2972 RVA: 0x00041CAC File Offset: 0x0003FEAC
		// (set) Token: 0x06000B9D RID: 2973 RVA: 0x00041CB4 File Offset: 0x0003FEB4
		public string Name
		{
			get
			{
				return this.mName;
			}
			set
			{
				this.mName = value;
			}
		}

		// Token: 0x17000163 RID: 355
		// (get) Token: 0x06000B9E RID: 2974 RVA: 0x00041CC0 File Offset: 0x0003FEC0
		// (set) Token: 0x06000B9F RID: 2975 RVA: 0x00041CE4 File Offset: 0x0003FEE4
		public DateTime LastModifiedTime
		{
			get
			{
				return Achievement.UnixEpoch.AddMilliseconds((double)this.mLastModifiedTime);
			}
			set
			{
				this.mLastModifiedTime = (long)(value - Achievement.UnixEpoch).TotalMilliseconds;
			}
		}

		// Token: 0x17000164 RID: 356
		// (get) Token: 0x06000BA0 RID: 2976 RVA: 0x00041D0C File Offset: 0x0003FF0C
		// (set) Token: 0x06000BA1 RID: 2977 RVA: 0x00041D14 File Offset: 0x0003FF14
		public ulong Points
		{
			get
			{
				return this.mPoints;
			}
			set
			{
				this.mPoints = value;
			}
		}

		// Token: 0x17000165 RID: 357
		// (get) Token: 0x06000BA2 RID: 2978 RVA: 0x00041D20 File Offset: 0x0003FF20
		// (set) Token: 0x06000BA3 RID: 2979 RVA: 0x00041D28 File Offset: 0x0003FF28
		public string RevealedImageUrl
		{
			get
			{
				return this.mRevealedImageUrl;
			}
			set
			{
				this.mRevealedImageUrl = value;
			}
		}

		// Token: 0x17000166 RID: 358
		// (get) Token: 0x06000BA4 RID: 2980 RVA: 0x00041D34 File Offset: 0x0003FF34
		// (set) Token: 0x06000BA5 RID: 2981 RVA: 0x00041D3C File Offset: 0x0003FF3C
		public string UnlockedImageUrl
		{
			get
			{
				return this.mUnlockedImageUrl;
			}
			set
			{
				this.mUnlockedImageUrl = value;
			}
		}

		// Token: 0x0400093B RID: 2363
		private static readonly DateTime UnixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

		// Token: 0x0400093C RID: 2364
		private string mId = string.Empty;

		// Token: 0x0400093D RID: 2365
		private bool mIsIncremental;

		// Token: 0x0400093E RID: 2366
		private bool mIsRevealed;

		// Token: 0x0400093F RID: 2367
		private bool mIsUnlocked;

		// Token: 0x04000940 RID: 2368
		private int mCurrentSteps;

		// Token: 0x04000941 RID: 2369
		private int mTotalSteps;

		// Token: 0x04000942 RID: 2370
		private string mDescription = string.Empty;

		// Token: 0x04000943 RID: 2371
		private string mName = string.Empty;

		// Token: 0x04000944 RID: 2372
		private long mLastModifiedTime;

		// Token: 0x04000945 RID: 2373
		private ulong mPoints;

		// Token: 0x04000946 RID: 2374
		private string mRevealedImageUrl;

		// Token: 0x04000947 RID: 2375
		private string mUnlockedImageUrl;
	}
}
