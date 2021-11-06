using System;
using GooglePlayGames.OurUtils;

namespace GooglePlayGames.BasicApi.SavedGame
{
	// Token: 0x0200019B RID: 411
	public struct SavedGameMetadataUpdate
	{
		// Token: 0x06000D05 RID: 3333 RVA: 0x00042D1C File Offset: 0x00040F1C
		private SavedGameMetadataUpdate(SavedGameMetadataUpdate.Builder builder)
		{
			this.mDescriptionUpdated = builder.mDescriptionUpdated;
			this.mNewDescription = builder.mNewDescription;
			this.mCoverImageUpdated = builder.mCoverImageUpdated;
			this.mNewPngCoverImage = builder.mNewPngCoverImage;
			this.mNewPlayedTime = builder.mNewPlayedTime;
		}

		// Token: 0x170001C5 RID: 453
		// (get) Token: 0x06000D06 RID: 3334 RVA: 0x00042D6C File Offset: 0x00040F6C
		public bool IsDescriptionUpdated
		{
			get
			{
				return this.mDescriptionUpdated;
			}
		}

		// Token: 0x170001C6 RID: 454
		// (get) Token: 0x06000D07 RID: 3335 RVA: 0x00042D74 File Offset: 0x00040F74
		public string UpdatedDescription
		{
			get
			{
				return this.mNewDescription;
			}
		}

		// Token: 0x170001C7 RID: 455
		// (get) Token: 0x06000D08 RID: 3336 RVA: 0x00042D7C File Offset: 0x00040F7C
		public bool IsCoverImageUpdated
		{
			get
			{
				return this.mCoverImageUpdated;
			}
		}

		// Token: 0x170001C8 RID: 456
		// (get) Token: 0x06000D09 RID: 3337 RVA: 0x00042D84 File Offset: 0x00040F84
		public byte[] UpdatedPngCoverImage
		{
			get
			{
				return this.mNewPngCoverImage;
			}
		}

		// Token: 0x170001C9 RID: 457
		// (get) Token: 0x06000D0A RID: 3338 RVA: 0x00042D8C File Offset: 0x00040F8C
		public bool IsPlayedTimeUpdated
		{
			get
			{
				return this.mNewPlayedTime != null;
			}
		}

		// Token: 0x170001CA RID: 458
		// (get) Token: 0x06000D0B RID: 3339 RVA: 0x00042DA8 File Offset: 0x00040FA8
		public TimeSpan? UpdatedPlayedTime
		{
			get
			{
				return this.mNewPlayedTime;
			}
		}

		// Token: 0x04000A3C RID: 2620
		private readonly bool mDescriptionUpdated;

		// Token: 0x04000A3D RID: 2621
		private readonly string mNewDescription;

		// Token: 0x04000A3E RID: 2622
		private readonly bool mCoverImageUpdated;

		// Token: 0x04000A3F RID: 2623
		private readonly byte[] mNewPngCoverImage;

		// Token: 0x04000A40 RID: 2624
		private readonly TimeSpan? mNewPlayedTime;

		// Token: 0x0200019C RID: 412
		public struct Builder
		{
			// Token: 0x06000D0C RID: 3340 RVA: 0x00042DB0 File Offset: 0x00040FB0
			public SavedGameMetadataUpdate.Builder WithUpdatedDescription(string description)
			{
				this.mNewDescription = Misc.CheckNotNull<string>(description);
				this.mDescriptionUpdated = true;
				return this;
			}

			// Token: 0x06000D0D RID: 3341 RVA: 0x00042DCC File Offset: 0x00040FCC
			public SavedGameMetadataUpdate.Builder WithUpdatedPngCoverImage(byte[] newPngCoverImage)
			{
				this.mCoverImageUpdated = true;
				this.mNewPngCoverImage = newPngCoverImage;
				return this;
			}

			// Token: 0x06000D0E RID: 3342 RVA: 0x00042DE4 File Offset: 0x00040FE4
			public SavedGameMetadataUpdate.Builder WithUpdatedPlayedTime(TimeSpan newPlayedTime)
			{
				if (newPlayedTime.TotalMilliseconds > 1.8446744073709552E+19)
				{
					throw new InvalidOperationException("Timespans longer than ulong.MaxValue milliseconds are not allowed");
				}
				this.mNewPlayedTime = new TimeSpan?(newPlayedTime);
				return this;
			}

			// Token: 0x06000D0F RID: 3343 RVA: 0x00042E24 File Offset: 0x00041024
			public SavedGameMetadataUpdate Build()
			{
				return new SavedGameMetadataUpdate(this);
			}

			// Token: 0x04000A41 RID: 2625
			internal bool mDescriptionUpdated;

			// Token: 0x04000A42 RID: 2626
			internal string mNewDescription;

			// Token: 0x04000A43 RID: 2627
			internal bool mCoverImageUpdated;

			// Token: 0x04000A44 RID: 2628
			internal byte[] mNewPngCoverImage;

			// Token: 0x04000A45 RID: 2629
			internal TimeSpan? mNewPlayedTime;
		}
	}
}
