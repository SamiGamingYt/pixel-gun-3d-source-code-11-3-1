using System;
using System.Collections.Generic;
using GooglePlayGames.BasicApi;
using UnityEngine;
using UnityEngine.SocialPlatforms;

namespace GooglePlayGames
{
	// Token: 0x020001A0 RID: 416
	public class PlayGamesLeaderboard : ILeaderboard
	{
		// Token: 0x06000D2F RID: 3375 RVA: 0x0004323C File Offset: 0x0004143C
		public PlayGamesLeaderboard(string id)
		{
			this.mId = id;
		}

		// Token: 0x06000D30 RID: 3376 RVA: 0x00043258 File Offset: 0x00041458
		public void SetUserFilter(string[] userIDs)
		{
			this.mFilteredUserIds = userIDs;
		}

		// Token: 0x06000D31 RID: 3377 RVA: 0x00043264 File Offset: 0x00041464
		public void LoadScores(Action<bool> callback)
		{
			PlayGamesPlatform.Instance.LoadScores(this, callback);
		}

		// Token: 0x170001DC RID: 476
		// (get) Token: 0x06000D32 RID: 3378 RVA: 0x00043274 File Offset: 0x00041474
		// (set) Token: 0x06000D33 RID: 3379 RVA: 0x0004327C File Offset: 0x0004147C
		public bool loading
		{
			get
			{
				return this.mLoading;
			}
			internal set
			{
				this.mLoading = value;
			}
		}

		// Token: 0x170001DD RID: 477
		// (get) Token: 0x06000D34 RID: 3380 RVA: 0x00043288 File Offset: 0x00041488
		// (set) Token: 0x06000D35 RID: 3381 RVA: 0x00043290 File Offset: 0x00041490
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

		// Token: 0x170001DE RID: 478
		// (get) Token: 0x06000D36 RID: 3382 RVA: 0x0004329C File Offset: 0x0004149C
		// (set) Token: 0x06000D37 RID: 3383 RVA: 0x000432A4 File Offset: 0x000414A4
		public UserScope userScope
		{
			get
			{
				return this.mUserScope;
			}
			set
			{
				this.mUserScope = value;
			}
		}

		// Token: 0x170001DF RID: 479
		// (get) Token: 0x06000D38 RID: 3384 RVA: 0x000432B0 File Offset: 0x000414B0
		// (set) Token: 0x06000D39 RID: 3385 RVA: 0x000432B8 File Offset: 0x000414B8
		public Range range
		{
			get
			{
				return this.mRange;
			}
			set
			{
				this.mRange = value;
			}
		}

		// Token: 0x170001E0 RID: 480
		// (get) Token: 0x06000D3A RID: 3386 RVA: 0x000432C4 File Offset: 0x000414C4
		// (set) Token: 0x06000D3B RID: 3387 RVA: 0x000432CC File Offset: 0x000414CC
		public TimeScope timeScope
		{
			get
			{
				return this.mTimeScope;
			}
			set
			{
				this.mTimeScope = value;
			}
		}

		// Token: 0x170001E1 RID: 481
		// (get) Token: 0x06000D3C RID: 3388 RVA: 0x000432D8 File Offset: 0x000414D8
		public IScore localUserScore
		{
			get
			{
				return this.mLocalUserScore;
			}
		}

		// Token: 0x170001E2 RID: 482
		// (get) Token: 0x06000D3D RID: 3389 RVA: 0x000432E0 File Offset: 0x000414E0
		public uint maxRange
		{
			get
			{
				return this.mMaxRange;
			}
		}

		// Token: 0x170001E3 RID: 483
		// (get) Token: 0x06000D3E RID: 3390 RVA: 0x000432E8 File Offset: 0x000414E8
		public IScore[] scores
		{
			get
			{
				PlayGamesScore[] array = new PlayGamesScore[this.mScoreList.Count];
				this.mScoreList.CopyTo(array);
				return array;
			}
		}

		// Token: 0x170001E4 RID: 484
		// (get) Token: 0x06000D3F RID: 3391 RVA: 0x00043314 File Offset: 0x00041514
		public string title
		{
			get
			{
				return this.mTitle;
			}
		}

		// Token: 0x06000D40 RID: 3392 RVA: 0x0004331C File Offset: 0x0004151C
		internal bool SetFromData(LeaderboardScoreData data)
		{
			if (data.Valid)
			{
				Debug.Log("Setting leaderboard from: " + data);
				this.SetMaxRange(data.ApproximateCount);
				this.SetTitle(data.Title);
				this.SetLocalUserScore((PlayGamesScore)data.PlayerScore);
				foreach (IScore score in data.Scores)
				{
					this.AddScore((PlayGamesScore)score);
				}
				this.mLoading = (data.Scores.Length == 0 || this.HasAllScores());
			}
			return data.Valid;
		}

		// Token: 0x06000D41 RID: 3393 RVA: 0x000433BC File Offset: 0x000415BC
		internal void SetMaxRange(ulong val)
		{
			this.mMaxRange = (uint)val;
		}

		// Token: 0x06000D42 RID: 3394 RVA: 0x000433C8 File Offset: 0x000415C8
		internal void SetTitle(string value)
		{
			this.mTitle = value;
		}

		// Token: 0x06000D43 RID: 3395 RVA: 0x000433D4 File Offset: 0x000415D4
		internal void SetLocalUserScore(PlayGamesScore score)
		{
			this.mLocalUserScore = score;
		}

		// Token: 0x06000D44 RID: 3396 RVA: 0x000433E0 File Offset: 0x000415E0
		internal int AddScore(PlayGamesScore score)
		{
			if (this.mFilteredUserIds == null || this.mFilteredUserIds.Length == 0)
			{
				this.mScoreList.Add(score);
			}
			else
			{
				foreach (string text in this.mFilteredUserIds)
				{
					if (text.Equals(score.userID))
					{
						return this.mScoreList.Count;
					}
				}
				this.mScoreList.Add(score);
			}
			return this.mScoreList.Count;
		}

		// Token: 0x170001E5 RID: 485
		// (get) Token: 0x06000D45 RID: 3397 RVA: 0x0004346C File Offset: 0x0004166C
		public int ScoreCount
		{
			get
			{
				return this.mScoreList.Count;
			}
		}

		// Token: 0x06000D46 RID: 3398 RVA: 0x0004347C File Offset: 0x0004167C
		internal bool HasAllScores()
		{
			return this.mScoreList.Count >= this.mRange.count || (long)this.mScoreList.Count >= (long)((ulong)this.maxRange);
		}

		// Token: 0x04000A62 RID: 2658
		private string mId;

		// Token: 0x04000A63 RID: 2659
		private UserScope mUserScope;

		// Token: 0x04000A64 RID: 2660
		private Range mRange;

		// Token: 0x04000A65 RID: 2661
		private TimeScope mTimeScope;

		// Token: 0x04000A66 RID: 2662
		private string[] mFilteredUserIds;

		// Token: 0x04000A67 RID: 2663
		private bool mLoading;

		// Token: 0x04000A68 RID: 2664
		private IScore mLocalUserScore;

		// Token: 0x04000A69 RID: 2665
		private uint mMaxRange;

		// Token: 0x04000A6A RID: 2666
		private List<PlayGamesScore> mScoreList = new List<PlayGamesScore>();

		// Token: 0x04000A6B RID: 2667
		private string mTitle;
	}
}
