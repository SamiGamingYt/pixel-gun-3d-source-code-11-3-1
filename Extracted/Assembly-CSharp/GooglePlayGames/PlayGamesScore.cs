using System;
using UnityEngine.SocialPlatforms;

namespace GooglePlayGames
{
	// Token: 0x020001A3 RID: 419
	public class PlayGamesScore : IScore
	{
		// Token: 0x06000D94 RID: 3476 RVA: 0x0004469C File Offset: 0x0004289C
		internal PlayGamesScore(DateTime date, string leaderboardId, ulong rank, string playerId, ulong value, string metadata)
		{
			this.mDate = date;
			this.mLbId = this.leaderboardID;
			this.mRank = rank;
			this.mPlayerId = playerId;
			this.mValue = (long)value;
			this.mMetadata = metadata;
		}

		// Token: 0x06000D95 RID: 3477 RVA: 0x0004470C File Offset: 0x0004290C
		public void ReportScore(Action<bool> callback)
		{
			PlayGamesPlatform.Instance.ReportScore(this.mValue, this.mLbId, this.mMetadata, callback);
		}

		// Token: 0x170001F9 RID: 505
		// (get) Token: 0x06000D96 RID: 3478 RVA: 0x0004472C File Offset: 0x0004292C
		// (set) Token: 0x06000D97 RID: 3479 RVA: 0x00044734 File Offset: 0x00042934
		public string leaderboardID
		{
			get
			{
				return this.mLbId;
			}
			set
			{
				this.mLbId = value;
			}
		}

		// Token: 0x170001FA RID: 506
		// (get) Token: 0x06000D98 RID: 3480 RVA: 0x00044740 File Offset: 0x00042940
		// (set) Token: 0x06000D99 RID: 3481 RVA: 0x00044748 File Offset: 0x00042948
		public long value
		{
			get
			{
				return this.mValue;
			}
			set
			{
				this.mValue = value;
			}
		}

		// Token: 0x170001FB RID: 507
		// (get) Token: 0x06000D9A RID: 3482 RVA: 0x00044754 File Offset: 0x00042954
		public DateTime date
		{
			get
			{
				return this.mDate;
			}
		}

		// Token: 0x170001FC RID: 508
		// (get) Token: 0x06000D9B RID: 3483 RVA: 0x0004475C File Offset: 0x0004295C
		public string formattedValue
		{
			get
			{
				return this.mValue.ToString();
			}
		}

		// Token: 0x170001FD RID: 509
		// (get) Token: 0x06000D9C RID: 3484 RVA: 0x0004476C File Offset: 0x0004296C
		public string userID
		{
			get
			{
				return this.mPlayerId;
			}
		}

		// Token: 0x170001FE RID: 510
		// (get) Token: 0x06000D9D RID: 3485 RVA: 0x00044774 File Offset: 0x00042974
		public int rank
		{
			get
			{
				return (int)this.mRank;
			}
		}

		// Token: 0x04000A77 RID: 2679
		private string mLbId;

		// Token: 0x04000A78 RID: 2680
		private long mValue;

		// Token: 0x04000A79 RID: 2681
		private ulong mRank;

		// Token: 0x04000A7A RID: 2682
		private string mPlayerId = string.Empty;

		// Token: 0x04000A7B RID: 2683
		private string mMetadata = string.Empty;

		// Token: 0x04000A7C RID: 2684
		private DateTime mDate = new DateTime(1970, 1, 1, 0, 0, 0);
	}
}
