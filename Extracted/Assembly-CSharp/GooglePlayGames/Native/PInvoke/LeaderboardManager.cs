using System;
using AOT;
using GooglePlayGames.BasicApi;
using GooglePlayGames.Native.Cwrapper;
using GooglePlayGames.OurUtils;

namespace GooglePlayGames.Native.PInvoke
{
	// Token: 0x02000240 RID: 576
	internal class LeaderboardManager
	{
		// Token: 0x06001235 RID: 4661 RVA: 0x0004D350 File Offset: 0x0004B550
		internal LeaderboardManager(GameServices services)
		{
			this.mServices = Misc.CheckNotNull<GameServices>(services);
		}

		// Token: 0x1700023B RID: 571
		// (get) Token: 0x06001236 RID: 4662 RVA: 0x0004D364 File Offset: 0x0004B564
		internal int LeaderboardMaxResults
		{
			get
			{
				return 25;
			}
		}

		// Token: 0x06001237 RID: 4663 RVA: 0x0004D368 File Offset: 0x0004B568
		internal void SubmitScore(string leaderboardId, long score, string metadata)
		{
			Misc.CheckNotNull<string>(leaderboardId, "leaderboardId");
			Logger.d(string.Concat(new object[]
			{
				"Native Submitting score: ",
				score,
				" for lb ",
				leaderboardId,
				" with metadata: ",
				metadata
			}));
			LeaderboardManager.LeaderboardManager_SubmitScore(this.mServices.AsHandle(), leaderboardId, (ulong)score, metadata ?? string.Empty);
		}

		// Token: 0x06001238 RID: 4664 RVA: 0x0004D3DC File Offset: 0x0004B5DC
		internal void ShowAllUI(Action<CommonErrorStatus.UIStatus> callback)
		{
			Misc.CheckNotNull<Action<CommonErrorStatus.UIStatus>>(callback);
			LeaderboardManager.LeaderboardManager_ShowAllUI(this.mServices.AsHandle(), new LeaderboardManager.ShowAllUICallback(Callbacks.InternalShowUICallback), Callbacks.ToIntPtr(callback));
		}

		// Token: 0x06001239 RID: 4665 RVA: 0x0004D408 File Offset: 0x0004B608
		internal void ShowUI(string leaderboardId, LeaderboardTimeSpan span, Action<CommonErrorStatus.UIStatus> callback)
		{
			Misc.CheckNotNull<Action<CommonErrorStatus.UIStatus>>(callback);
			LeaderboardManager.LeaderboardManager_ShowUI(this.mServices.AsHandle(), leaderboardId, (Types.LeaderboardTimeSpan)span, new LeaderboardManager.ShowUICallback(Callbacks.InternalShowUICallback), Callbacks.ToIntPtr(callback));
		}

		// Token: 0x0600123A RID: 4666 RVA: 0x0004D440 File Offset: 0x0004B640
		public void LoadLeaderboardData(string leaderboardId, LeaderboardStart start, int rowCount, LeaderboardCollection collection, LeaderboardTimeSpan timeSpan, string playerId, Action<LeaderboardScoreData> callback)
		{
			NativeScorePageToken internalObject = new NativeScorePageToken(LeaderboardManager.LeaderboardManager_ScorePageToken(this.mServices.AsHandle(), leaderboardId, (Types.LeaderboardStart)start, (Types.LeaderboardTimeSpan)timeSpan, (Types.LeaderboardCollection)collection));
			ScorePageToken token = new ScorePageToken(internalObject, leaderboardId, collection, timeSpan);
			LeaderboardManager.LeaderboardManager_Fetch(this.mServices.AsHandle(), Types.DataSource.CACHE_OR_NETWORK, leaderboardId, new LeaderboardManager.FetchCallback(LeaderboardManager.InternalFetchCallback), Callbacks.ToIntPtr<FetchResponse>(delegate(FetchResponse rsp)
			{
				this.HandleFetch(token, rsp, playerId, rowCount, callback);
			}, new Func<IntPtr, FetchResponse>(FetchResponse.FromPointer)));
		}

		// Token: 0x0600123B RID: 4667 RVA: 0x0004D4DC File Offset: 0x0004B6DC
		[MonoPInvokeCallback(typeof(LeaderboardManager.FetchCallback))]
		private static void InternalFetchCallback(IntPtr response, IntPtr data)
		{
			Callbacks.PerformInternalCallback("LeaderboardManager#InternalFetchCallback", Callbacks.Type.Temporary, response, data);
		}

		// Token: 0x0600123C RID: 4668 RVA: 0x0004D4EC File Offset: 0x0004B6EC
		internal void HandleFetch(ScorePageToken token, FetchResponse response, string selfPlayerId, int maxResults, Action<LeaderboardScoreData> callback)
		{
			LeaderboardScoreData data = new LeaderboardScoreData(token.LeaderboardId, (ResponseStatus)response.GetStatus());
			if (response.GetStatus() != CommonErrorStatus.ResponseStatus.VALID && response.GetStatus() != CommonErrorStatus.ResponseStatus.VALID_BUT_STALE)
			{
				Logger.w("Error returned from fetch: " + response.GetStatus());
				callback(data);
				return;
			}
			data.Title = response.Leaderboard().Title();
			data.Id = token.LeaderboardId;
			LeaderboardManager.LeaderboardManager_FetchScoreSummary(this.mServices.AsHandle(), Types.DataSource.CACHE_OR_NETWORK, token.LeaderboardId, (Types.LeaderboardTimeSpan)token.TimeSpan, (Types.LeaderboardCollection)token.Collection, new LeaderboardManager.FetchScoreSummaryCallback(LeaderboardManager.InternalFetchSummaryCallback), Callbacks.ToIntPtr<FetchScoreSummaryResponse>(delegate(FetchScoreSummaryResponse rsp)
			{
				this.HandleFetchScoreSummary(data, rsp, selfPlayerId, maxResults, token, callback);
			}, new Func<IntPtr, FetchScoreSummaryResponse>(FetchScoreSummaryResponse.FromPointer)));
		}

		// Token: 0x0600123D RID: 4669 RVA: 0x0004D60C File Offset: 0x0004B80C
		[MonoPInvokeCallback(typeof(LeaderboardManager.FetchScoreSummaryCallback))]
		private static void InternalFetchSummaryCallback(IntPtr response, IntPtr data)
		{
			Callbacks.PerformInternalCallback("LeaderboardManager#InternalFetchSummaryCallback", Callbacks.Type.Temporary, response, data);
		}

		// Token: 0x0600123E RID: 4670 RVA: 0x0004D61C File Offset: 0x0004B81C
		internal void HandleFetchScoreSummary(LeaderboardScoreData data, FetchScoreSummaryResponse response, string selfPlayerId, int maxResults, ScorePageToken token, Action<LeaderboardScoreData> callback)
		{
			if (response.GetStatus() != CommonErrorStatus.ResponseStatus.VALID && response.GetStatus() != CommonErrorStatus.ResponseStatus.VALID_BUT_STALE)
			{
				Logger.w("Error returned from fetchScoreSummary: " + response);
				data.Status = (ResponseStatus)response.GetStatus();
				callback(data);
				return;
			}
			NativeScoreSummary scoreSummary = response.GetScoreSummary();
			data.ApproximateCount = scoreSummary.ApproximateResults();
			data.PlayerScore = scoreSummary.LocalUserScore().AsScore(data.Id, selfPlayerId);
			if (maxResults <= 0)
			{
				callback(data);
				return;
			}
			this.LoadScorePage(data, maxResults, token, callback);
		}

		// Token: 0x0600123F RID: 4671 RVA: 0x0004D6B0 File Offset: 0x0004B8B0
		public void LoadScorePage(LeaderboardScoreData data, int maxResults, ScorePageToken token, Action<LeaderboardScoreData> callback)
		{
			if (data == null)
			{
				data = new LeaderboardScoreData(token.LeaderboardId);
			}
			NativeScorePageToken nativeScorePageToken = (NativeScorePageToken)token.InternalObject;
			LeaderboardManager.LeaderboardManager_FetchScorePage(this.mServices.AsHandle(), Types.DataSource.CACHE_OR_NETWORK, nativeScorePageToken.AsPointer(), (uint)maxResults, new LeaderboardManager.FetchScorePageCallback(LeaderboardManager.InternalFetchScorePage), Callbacks.ToIntPtr<FetchScorePageResponse>(delegate(FetchScorePageResponse rsp)
			{
				this.HandleFetchScorePage(data, token, rsp, callback);
			}, new Func<IntPtr, FetchScorePageResponse>(FetchScorePageResponse.FromPointer)));
		}

		// Token: 0x06001240 RID: 4672 RVA: 0x0004D754 File Offset: 0x0004B954
		[MonoPInvokeCallback(typeof(LeaderboardManager.FetchScorePageCallback))]
		private static void InternalFetchScorePage(IntPtr response, IntPtr data)
		{
			Callbacks.PerformInternalCallback("LeaderboardManager#InternalFetchScorePage", Callbacks.Type.Temporary, response, data);
		}

		// Token: 0x06001241 RID: 4673 RVA: 0x0004D764 File Offset: 0x0004B964
		internal void HandleFetchScorePage(LeaderboardScoreData data, ScorePageToken token, FetchScorePageResponse rsp, Action<LeaderboardScoreData> callback)
		{
			data.Status = (ResponseStatus)rsp.GetStatus();
			if (rsp.GetStatus() != CommonErrorStatus.ResponseStatus.VALID && rsp.GetStatus() != CommonErrorStatus.ResponseStatus.VALID_BUT_STALE)
			{
				callback(data);
			}
			NativeScorePage scorePage = rsp.GetScorePage();
			if (!scorePage.Valid())
			{
				callback(data);
			}
			if (scorePage.HasNextScorePage())
			{
				data.NextPageToken = new ScorePageToken(scorePage.GetNextScorePageToken(), token.LeaderboardId, token.Collection, token.TimeSpan);
			}
			if (scorePage.HasPrevScorePage())
			{
				data.PrevPageToken = new ScorePageToken(scorePage.GetPreviousScorePageToken(), token.LeaderboardId, token.Collection, token.TimeSpan);
			}
			foreach (NativeScoreEntry nativeScoreEntry in scorePage)
			{
				data.AddScore(nativeScoreEntry.AsScore(data.Id));
			}
			callback(data);
		}

		// Token: 0x04000C00 RID: 3072
		private readonly GameServices mServices;
	}
}
