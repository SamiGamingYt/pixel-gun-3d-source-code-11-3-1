using System;
using GooglePlayGames.BasicApi;
using UnityEngine;
using UnityEngine.SocialPlatforms;

namespace GooglePlayGames
{
	// Token: 0x0200019F RID: 415
	internal class PlayGamesAchievement : IAchievementDescription, IAchievement
	{
		// Token: 0x06000D1B RID: 3355 RVA: 0x00042F40 File Offset: 0x00041140
		internal PlayGamesAchievement() : this(new ReportProgress(PlayGamesPlatform.Instance.ReportProgress))
		{
		}

		// Token: 0x06000D1C RID: 3356 RVA: 0x00042F5C File Offset: 0x0004115C
		internal PlayGamesAchievement(ReportProgress progressCallback)
		{
			this.mProgressCallback = progressCallback;
		}

		// Token: 0x06000D1D RID: 3357 RVA: 0x00042FC4 File Offset: 0x000411C4
		internal PlayGamesAchievement(Achievement ach) : this()
		{
			this.mId = ach.Id;
			this.mIsIncremental = ach.IsIncremental;
			this.mCurrentSteps = ach.CurrentSteps;
			this.mTotalSteps = ach.TotalSteps;
			if (ach.IsIncremental)
			{
				if (ach.TotalSteps > 0)
				{
					this.mPercentComplete = (double)ach.CurrentSteps / (double)ach.TotalSteps * 100.0;
				}
				else
				{
					this.mPercentComplete = 0.0;
				}
			}
			else
			{
				this.mPercentComplete = ((!ach.IsUnlocked) ? 0.0 : 100.0);
			}
			this.mCompleted = ach.IsUnlocked;
			this.mHidden = !ach.IsRevealed;
			this.mLastModifiedTime = ach.LastModifiedTime;
			this.mTitle = ach.Name;
			this.mDescription = ach.Description;
			this.mPoints = ach.Points;
			this.mRevealedImageUrl = ach.RevealedImageUrl;
			this.mUnlockedImageUrl = ach.UnlockedImageUrl;
		}

		// Token: 0x06000D1E RID: 3358 RVA: 0x000430E4 File Offset: 0x000412E4
		public void ReportProgress(Action<bool> callback)
		{
			this.mProgressCallback(this.mId, this.mPercentComplete, callback);
		}

		// Token: 0x06000D1F RID: 3359 RVA: 0x00043100 File Offset: 0x00041300
		private Texture2D LoadImage()
		{
			if (this.hidden)
			{
				return null;
			}
			string text = (!this.completed) ? this.mRevealedImageUrl : this.mUnlockedImageUrl;
			if (!string.IsNullOrEmpty(text))
			{
				if (this.mImageFetcher == null || this.mImageFetcher.url != text)
				{
					this.mImageFetcher = new WWW(text);
					this.mImage = null;
				}
				if (this.mImage != null)
				{
					return this.mImage;
				}
				if (this.mImageFetcher.isDone)
				{
					this.mImage = this.mImageFetcher.texture;
					return this.mImage;
				}
			}
			return null;
		}

		// Token: 0x170001CF RID: 463
		// (get) Token: 0x06000D20 RID: 3360 RVA: 0x000431B8 File Offset: 0x000413B8
		// (set) Token: 0x06000D21 RID: 3361 RVA: 0x000431C0 File Offset: 0x000413C0
		public string id
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

		// Token: 0x170001D0 RID: 464
		// (get) Token: 0x06000D22 RID: 3362 RVA: 0x000431CC File Offset: 0x000413CC
		public bool isIncremental
		{
			get
			{
				return this.mIsIncremental;
			}
		}

		// Token: 0x170001D1 RID: 465
		// (get) Token: 0x06000D23 RID: 3363 RVA: 0x000431D4 File Offset: 0x000413D4
		public int currentSteps
		{
			get
			{
				return this.mCurrentSteps;
			}
		}

		// Token: 0x170001D2 RID: 466
		// (get) Token: 0x06000D24 RID: 3364 RVA: 0x000431DC File Offset: 0x000413DC
		public int totalSteps
		{
			get
			{
				return this.mTotalSteps;
			}
		}

		// Token: 0x170001D3 RID: 467
		// (get) Token: 0x06000D25 RID: 3365 RVA: 0x000431E4 File Offset: 0x000413E4
		// (set) Token: 0x06000D26 RID: 3366 RVA: 0x000431EC File Offset: 0x000413EC
		public double percentCompleted
		{
			get
			{
				return this.mPercentComplete;
			}
			set
			{
				this.mPercentComplete = value;
			}
		}

		// Token: 0x170001D4 RID: 468
		// (get) Token: 0x06000D27 RID: 3367 RVA: 0x000431F8 File Offset: 0x000413F8
		public bool completed
		{
			get
			{
				return this.mCompleted;
			}
		}

		// Token: 0x170001D5 RID: 469
		// (get) Token: 0x06000D28 RID: 3368 RVA: 0x00043200 File Offset: 0x00041400
		public bool hidden
		{
			get
			{
				return this.mHidden;
			}
		}

		// Token: 0x170001D6 RID: 470
		// (get) Token: 0x06000D29 RID: 3369 RVA: 0x00043208 File Offset: 0x00041408
		public DateTime lastReportedDate
		{
			get
			{
				return this.mLastModifiedTime;
			}
		}

		// Token: 0x170001D7 RID: 471
		// (get) Token: 0x06000D2A RID: 3370 RVA: 0x00043210 File Offset: 0x00041410
		public string title
		{
			get
			{
				return this.mTitle;
			}
		}

		// Token: 0x170001D8 RID: 472
		// (get) Token: 0x06000D2B RID: 3371 RVA: 0x00043218 File Offset: 0x00041418
		public Texture2D image
		{
			get
			{
				return this.LoadImage();
			}
		}

		// Token: 0x170001D9 RID: 473
		// (get) Token: 0x06000D2C RID: 3372 RVA: 0x00043220 File Offset: 0x00041420
		public string achievedDescription
		{
			get
			{
				return this.mDescription;
			}
		}

		// Token: 0x170001DA RID: 474
		// (get) Token: 0x06000D2D RID: 3373 RVA: 0x00043228 File Offset: 0x00041428
		public string unachievedDescription
		{
			get
			{
				return this.mDescription;
			}
		}

		// Token: 0x170001DB RID: 475
		// (get) Token: 0x06000D2E RID: 3374 RVA: 0x00043230 File Offset: 0x00041430
		public int points
		{
			get
			{
				return (int)this.mPoints;
			}
		}

		// Token: 0x04000A52 RID: 2642
		private readonly ReportProgress mProgressCallback;

		// Token: 0x04000A53 RID: 2643
		private string mId = string.Empty;

		// Token: 0x04000A54 RID: 2644
		private bool mIsIncremental;

		// Token: 0x04000A55 RID: 2645
		private int mCurrentSteps;

		// Token: 0x04000A56 RID: 2646
		private int mTotalSteps;

		// Token: 0x04000A57 RID: 2647
		private double mPercentComplete;

		// Token: 0x04000A58 RID: 2648
		private bool mCompleted;

		// Token: 0x04000A59 RID: 2649
		private bool mHidden;

		// Token: 0x04000A5A RID: 2650
		private DateTime mLastModifiedTime = new DateTime(1970, 1, 1, 0, 0, 0, 0);

		// Token: 0x04000A5B RID: 2651
		private string mTitle = string.Empty;

		// Token: 0x04000A5C RID: 2652
		private string mRevealedImageUrl = string.Empty;

		// Token: 0x04000A5D RID: 2653
		private string mUnlockedImageUrl = string.Empty;

		// Token: 0x04000A5E RID: 2654
		private WWW mImageFetcher;

		// Token: 0x04000A5F RID: 2655
		private Texture2D mImage;

		// Token: 0x04000A60 RID: 2656
		private string mDescription = string.Empty;

		// Token: 0x04000A61 RID: 2657
		private ulong mPoints;
	}
}
