using System;
using System.Collections.Generic;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.Events;
using GooglePlayGames.BasicApi.Multiplayer;
using GooglePlayGames.BasicApi.Nearby;
using GooglePlayGames.BasicApi.Quests;
using GooglePlayGames.BasicApi.SavedGame;
using GooglePlayGames.OurUtils;
using UnityEngine;
using UnityEngine.SocialPlatforms;

namespace GooglePlayGames
{
	// Token: 0x020001A2 RID: 418
	public class PlayGamesPlatform : ISocialPlatform
	{
		// Token: 0x06000D57 RID: 3415 RVA: 0x00043798 File Offset: 0x00041998
		internal PlayGamesPlatform(IPlayGamesClient client)
		{
			this.mClient = Misc.CheckNotNull<IPlayGamesClient>(client);
			this.mLocalUser = new PlayGamesLocalUser(this);
			this.mConfiguration = PlayGamesClientConfiguration.DefaultConfiguration;
		}

		// Token: 0x06000D58 RID: 3416 RVA: 0x000437DC File Offset: 0x000419DC
		private PlayGamesPlatform(PlayGamesClientConfiguration configuration)
		{
			this.mLocalUser = new PlayGamesLocalUser(this);
			this.mConfiguration = configuration;
		}

		// Token: 0x170001F0 RID: 496
		// (get) Token: 0x06000D5A RID: 3418 RVA: 0x00043814 File Offset: 0x00041A14
		// (set) Token: 0x06000D5B RID: 3419 RVA: 0x0004381C File Offset: 0x00041A1C
		public static bool DebugLogEnabled
		{
			get
			{
				return GooglePlayGames.OurUtils.Logger.DebugLogEnabled;
			}
			set
			{
				GooglePlayGames.OurUtils.Logger.DebugLogEnabled = value;
			}
		}

		// Token: 0x170001F1 RID: 497
		// (get) Token: 0x06000D5C RID: 3420 RVA: 0x00043824 File Offset: 0x00041A24
		public static PlayGamesPlatform Instance
		{
			get
			{
				if (PlayGamesPlatform.sInstance == null)
				{
					GooglePlayGames.OurUtils.Logger.d("Instance was not initialized, using default configuration.");
					PlayGamesPlatform.InitializeInstance(PlayGamesClientConfiguration.DefaultConfiguration);
				}
				return PlayGamesPlatform.sInstance;
			}
		}

		// Token: 0x170001F2 RID: 498
		// (get) Token: 0x06000D5D RID: 3421 RVA: 0x00043850 File Offset: 0x00041A50
		public static INearbyConnectionClient Nearby
		{
			get
			{
				if (PlayGamesPlatform.sNearbyConnectionClient == null && !PlayGamesPlatform.sNearbyInitializePending)
				{
					PlayGamesPlatform.sNearbyInitializePending = true;
					PlayGamesPlatform.InitializeNearby(null);
				}
				return PlayGamesPlatform.sNearbyConnectionClient;
			}
		}

		// Token: 0x170001F3 RID: 499
		// (get) Token: 0x06000D5E RID: 3422 RVA: 0x00043880 File Offset: 0x00041A80
		public IRealTimeMultiplayerClient RealTime
		{
			get
			{
				return this.mClient.GetRtmpClient();
			}
		}

		// Token: 0x170001F4 RID: 500
		// (get) Token: 0x06000D5F RID: 3423 RVA: 0x00043890 File Offset: 0x00041A90
		public ITurnBasedMultiplayerClient TurnBased
		{
			get
			{
				return this.mClient.GetTbmpClient();
			}
		}

		// Token: 0x170001F5 RID: 501
		// (get) Token: 0x06000D60 RID: 3424 RVA: 0x000438A0 File Offset: 0x00041AA0
		public ISavedGameClient SavedGame
		{
			get
			{
				return this.mClient.GetSavedGameClient();
			}
		}

		// Token: 0x170001F6 RID: 502
		// (get) Token: 0x06000D61 RID: 3425 RVA: 0x000438B0 File Offset: 0x00041AB0
		public IEventsClient Events
		{
			get
			{
				return this.mClient.GetEventsClient();
			}
		}

		// Token: 0x170001F7 RID: 503
		// (get) Token: 0x06000D62 RID: 3426 RVA: 0x000438C0 File Offset: 0x00041AC0
		public IQuestsClient Quests
		{
			get
			{
				return this.mClient.GetQuestsClient();
			}
		}

		// Token: 0x170001F8 RID: 504
		// (get) Token: 0x06000D63 RID: 3427 RVA: 0x000438D0 File Offset: 0x00041AD0
		public ILocalUser localUser
		{
			get
			{
				return this.mLocalUser;
			}
		}

		// Token: 0x06000D64 RID: 3428 RVA: 0x000438D8 File Offset: 0x00041AD8
		public static void InitializeInstance(PlayGamesClientConfiguration configuration)
		{
			if (PlayGamesPlatform.sInstance != null)
			{
				GooglePlayGames.OurUtils.Logger.w("PlayGamesPlatform already initialized. Ignoring this call.");
				return;
			}
			PlayGamesPlatform.sInstance = new PlayGamesPlatform(configuration);
		}

		// Token: 0x06000D65 RID: 3429 RVA: 0x0004390C File Offset: 0x00041B0C
		public static void InitializeNearby(Action<INearbyConnectionClient> callback)
		{
			Debug.Log("Calling InitializeNearby!");
			if (PlayGamesPlatform.sNearbyConnectionClient == null)
			{
				NearbyConnectionClientFactory.Create(delegate(INearbyConnectionClient client)
				{
					Debug.Log("Nearby Client Created!!");
					PlayGamesPlatform.sNearbyConnectionClient = client;
					if (callback != null)
					{
						callback(client);
					}
					else
					{
						Debug.Log("Initialize Nearby callback is null");
					}
				});
			}
			else if (callback != null)
			{
				Debug.Log("Nearby Already initialized: calling callback directly");
				callback(PlayGamesPlatform.sNearbyConnectionClient);
			}
			else
			{
				Debug.Log("Nearby Already initialized");
			}
		}

		// Token: 0x06000D66 RID: 3430 RVA: 0x00043988 File Offset: 0x00041B88
		public static PlayGamesPlatform Activate()
		{
			GooglePlayGames.OurUtils.Logger.d("Activating PlayGamesPlatform.");
			Social.Active = PlayGamesPlatform.Instance;
			GooglePlayGames.OurUtils.Logger.d("PlayGamesPlatform activated: " + Social.Active);
			return PlayGamesPlatform.Instance;
		}

		// Token: 0x06000D67 RID: 3431 RVA: 0x000439B8 File Offset: 0x00041BB8
		public IntPtr GetApiClient()
		{
			return this.mClient.GetApiClient();
		}

		// Token: 0x06000D68 RID: 3432 RVA: 0x000439C8 File Offset: 0x00041BC8
		public void AddIdMapping(string fromId, string toId)
		{
			this.mIdMap[fromId] = toId;
		}

		// Token: 0x06000D69 RID: 3433 RVA: 0x000439D8 File Offset: 0x00041BD8
		public void Authenticate(Action<bool> callback)
		{
			this.Authenticate(callback, false);
		}

		// Token: 0x06000D6A RID: 3434 RVA: 0x000439E4 File Offset: 0x00041BE4
		public void Authenticate(Action<bool> callback, bool silent)
		{
			if (this.mClient == null)
			{
				GooglePlayGames.OurUtils.Logger.d("Creating platform-specific Play Games client.");
				this.mClient = PlayGamesClientFactory.GetPlatformPlayGamesClient(this.mConfiguration);
			}
			this.mClient.Authenticate(callback, silent);
		}

		// Token: 0x06000D6B RID: 3435 RVA: 0x00043A1C File Offset: 0x00041C1C
		public void Authenticate(ILocalUser unused, Action<bool> callback)
		{
			this.Authenticate(callback, false);
		}

		// Token: 0x06000D6C RID: 3436 RVA: 0x00043A28 File Offset: 0x00041C28
		public bool IsAuthenticated()
		{
			return this.mClient != null && this.mClient.IsAuthenticated();
		}

		// Token: 0x06000D6D RID: 3437 RVA: 0x00043A44 File Offset: 0x00041C44
		public void SignOut()
		{
			if (this.mClient != null)
			{
				this.mClient.SignOut();
			}
			this.mLocalUser = new PlayGamesLocalUser(this);
		}

		// Token: 0x06000D6E RID: 3438 RVA: 0x00043A74 File Offset: 0x00041C74
		public void LoadUsers(string[] userIds, Action<IUserProfile[]> callback)
		{
			if (!this.IsAuthenticated())
			{
				GooglePlayGames.OurUtils.Logger.e("GetUserId() can only be called after authentication.");
				callback(new IUserProfile[0]);
				return;
			}
			this.mClient.LoadUsers(userIds, callback);
		}

		// Token: 0x06000D6F RID: 3439 RVA: 0x00043AB0 File Offset: 0x00041CB0
		public string GetUserId()
		{
			if (!this.IsAuthenticated())
			{
				GooglePlayGames.OurUtils.Logger.e("GetUserId() can only be called after authentication.");
				return "0";
			}
			return this.mClient.GetUserId();
		}

		// Token: 0x06000D70 RID: 3440 RVA: 0x00043AE4 File Offset: 0x00041CE4
		public void GetIdToken(Action<string> idTokenCallback)
		{
			if (this.mClient != null)
			{
				this.mClient.GetIdToken(idTokenCallback);
			}
			else
			{
				GooglePlayGames.OurUtils.Logger.e("No client available, calling back with null.");
				idTokenCallback(null);
			}
		}

		// Token: 0x06000D71 RID: 3441 RVA: 0x00043B14 File Offset: 0x00041D14
		public string GetAccessToken()
		{
			if (this.mClient != null)
			{
				return this.mClient.GetAccessToken();
			}
			return null;
		}

		// Token: 0x06000D72 RID: 3442 RVA: 0x00043B30 File Offset: 0x00041D30
		public void GetServerAuthCode(Action<CommonStatusCodes, string> callback)
		{
			if (this.mClient != null && this.mClient.IsAuthenticated())
			{
				if (GameInfo.WebClientIdInitialized())
				{
					this.mClient.GetServerAuthCode(string.Empty, callback);
				}
				else
				{
					GooglePlayGames.OurUtils.Logger.e("GetServerAuthCode requires a webClientId.");
					callback(CommonStatusCodes.DeveloperError, string.Empty);
				}
			}
			else
			{
				GooglePlayGames.OurUtils.Logger.e("GetServerAuthCode can only be called after authentication.");
				callback(CommonStatusCodes.SignInRequired, string.Empty);
			}
		}

		// Token: 0x06000D73 RID: 3443 RVA: 0x00043BAC File Offset: 0x00041DAC
		public string GetUserEmail()
		{
			if (this.mClient != null)
			{
				return this.mClient.GetUserEmail();
			}
			return null;
		}

		// Token: 0x06000D74 RID: 3444 RVA: 0x00043BC8 File Offset: 0x00041DC8
		public void GetUserEmail(Action<CommonStatusCodes, string> callback)
		{
			this.mClient.GetUserEmail(callback);
		}

		// Token: 0x06000D75 RID: 3445 RVA: 0x00043BD8 File Offset: 0x00041DD8
		public void GetPlayerStats(Action<CommonStatusCodes, PlayerStats> callback)
		{
			if (this.mClient != null && this.mClient.IsAuthenticated())
			{
				this.mClient.GetPlayerStats(callback);
			}
			else
			{
				GooglePlayGames.OurUtils.Logger.e("GetPlayerStats can only be called after authentication.");
				callback(CommonStatusCodes.SignInRequired, new PlayerStats());
			}
		}

		// Token: 0x06000D76 RID: 3446 RVA: 0x00043C28 File Offset: 0x00041E28
		public Achievement GetAchievement(string achievementId)
		{
			if (!this.IsAuthenticated())
			{
				GooglePlayGames.OurUtils.Logger.e("GetAchievement can only be called after authentication.");
				return null;
			}
			return this.mClient.GetAchievement(achievementId);
		}

		// Token: 0x06000D77 RID: 3447 RVA: 0x00043C50 File Offset: 0x00041E50
		public string GetUserDisplayName()
		{
			if (!this.IsAuthenticated())
			{
				GooglePlayGames.OurUtils.Logger.e("GetUserDisplayName can only be called after authentication.");
				return string.Empty;
			}
			return this.mClient.GetUserDisplayName();
		}

		// Token: 0x06000D78 RID: 3448 RVA: 0x00043C84 File Offset: 0x00041E84
		public string GetUserImageUrl()
		{
			if (!this.IsAuthenticated())
			{
				GooglePlayGames.OurUtils.Logger.e("GetUserImageUrl can only be called after authentication.");
				return null;
			}
			return this.mClient.GetUserImageUrl();
		}

		// Token: 0x06000D79 RID: 3449 RVA: 0x00043CB4 File Offset: 0x00041EB4
		public void ReportProgress(string achievementID, double progress, Action<bool> callback)
		{
			if (!this.IsAuthenticated())
			{
				GooglePlayGames.OurUtils.Logger.e("ReportProgress can only be called after authentication.");
				if (callback != null)
				{
					callback(false);
				}
				return;
			}
			GooglePlayGames.OurUtils.Logger.d(string.Concat(new object[]
			{
				"ReportProgress, ",
				achievementID,
				", ",
				progress
			}));
			achievementID = this.MapId(achievementID);
			if (progress < 1E-06)
			{
				GooglePlayGames.OurUtils.Logger.d("Progress 0.00 interpreted as request to reveal.");
				this.mClient.RevealAchievement(achievementID, callback);
				return;
			}
			int num = 0;
			int num2 = 0;
			Achievement achievement = this.mClient.GetAchievement(achievementID);
			bool flag;
			if (achievement == null)
			{
				GooglePlayGames.OurUtils.Logger.w("Unable to locate achievement " + achievementID);
				GooglePlayGames.OurUtils.Logger.w("As a quick fix, assuming it's standard.");
				flag = false;
			}
			else
			{
				flag = achievement.IsIncremental;
				num = achievement.CurrentSteps;
				num2 = achievement.TotalSteps;
				GooglePlayGames.OurUtils.Logger.d("Achievement is " + ((!flag) ? "STANDARD" : "INCREMENTAL"));
				if (flag)
				{
					GooglePlayGames.OurUtils.Logger.d(string.Concat(new object[]
					{
						"Current steps: ",
						num,
						"/",
						num2
					}));
				}
			}
			if (flag)
			{
				GooglePlayGames.OurUtils.Logger.d("Progress " + progress + " interpreted as incremental target (approximate).");
				if (progress >= 0.0 && progress <= 1.0)
				{
					GooglePlayGames.OurUtils.Logger.w("Progress " + progress + " is less than or equal to 1. You might be trying to use values in the range of [0,1], while values are expected to be within the range [0,100]. If you are using the latter, you can safely ignore this message.");
				}
				int num3 = (int)Math.Round(progress / 100.0 * (double)num2);
				int num4 = num3 - num;
				GooglePlayGames.OurUtils.Logger.d(string.Concat(new object[]
				{
					"Target steps: ",
					num3,
					", cur steps:",
					num
				}));
				GooglePlayGames.OurUtils.Logger.d("Steps to increment: " + num4);
				if (num4 >= 0)
				{
					this.mClient.IncrementAchievement(achievementID, num4, callback);
				}
			}
			else if (progress >= 100.0)
			{
				GooglePlayGames.OurUtils.Logger.d("Progress " + progress + " interpreted as UNLOCK.");
				this.mClient.UnlockAchievement(achievementID, callback);
			}
			else
			{
				GooglePlayGames.OurUtils.Logger.d("Progress " + progress + " not enough to unlock non-incremental achievement.");
			}
		}

		// Token: 0x06000D7A RID: 3450 RVA: 0x00043F1C File Offset: 0x0004211C
		public void IncrementAchievement(string achievementID, int steps, Action<bool> callback)
		{
			if (!this.IsAuthenticated())
			{
				GooglePlayGames.OurUtils.Logger.e("IncrementAchievement can only be called after authentication.");
				if (callback != null)
				{
					callback(false);
				}
				return;
			}
			GooglePlayGames.OurUtils.Logger.d(string.Concat(new object[]
			{
				"IncrementAchievement: ",
				achievementID,
				", steps ",
				steps
			}));
			achievementID = this.MapId(achievementID);
			this.mClient.IncrementAchievement(achievementID, steps, callback);
		}

		// Token: 0x06000D7B RID: 3451 RVA: 0x00043F90 File Offset: 0x00042190
		public void SetStepsAtLeast(string achievementID, int steps, Action<bool> callback)
		{
			if (!this.IsAuthenticated())
			{
				GooglePlayGames.OurUtils.Logger.e("SetStepsAtLeast can only be called after authentication.");
				if (callback != null)
				{
					callback(false);
				}
				return;
			}
			GooglePlayGames.OurUtils.Logger.d(string.Concat(new object[]
			{
				"SetStepsAtLeast: ",
				achievementID,
				", steps ",
				steps
			}));
			achievementID = this.MapId(achievementID);
			this.mClient.SetStepsAtLeast(achievementID, steps, callback);
		}

		// Token: 0x06000D7C RID: 3452 RVA: 0x00044004 File Offset: 0x00042204
		public void LoadAchievementDescriptions(Action<IAchievementDescription[]> callback)
		{
			if (!this.IsAuthenticated())
			{
				GooglePlayGames.OurUtils.Logger.e("LoadAchievementDescriptions can only be called after authentication.");
				if (callback != null)
				{
					callback(null);
				}
				return;
			}
			this.mClient.LoadAchievements(delegate(Achievement[] ach)
			{
				IAchievementDescription[] array = new IAchievementDescription[ach.Length];
				for (int i = 0; i < array.Length; i++)
				{
					array[i] = new PlayGamesAchievement(ach[i]);
				}
				callback(array);
			});
		}

		// Token: 0x06000D7D RID: 3453 RVA: 0x00044064 File Offset: 0x00042264
		public void LoadAchievements(Action<IAchievement[]> callback)
		{
			if (!this.IsAuthenticated())
			{
				GooglePlayGames.OurUtils.Logger.e("LoadAchievements can only be called after authentication.");
				callback(null);
				return;
			}
			this.mClient.LoadAchievements(delegate(Achievement[] ach)
			{
				IAchievement[] array = new IAchievement[ach.Length];
				for (int i = 0; i < array.Length; i++)
				{
					array[i] = new PlayGamesAchievement(ach[i]);
				}
				callback(array);
			});
		}

		// Token: 0x06000D7E RID: 3454 RVA: 0x000440B8 File Offset: 0x000422B8
		public IAchievement CreateAchievement()
		{
			return new PlayGamesAchievement();
		}

		// Token: 0x06000D7F RID: 3455 RVA: 0x000440C0 File Offset: 0x000422C0
		public void ReportScore(long score, string board, Action<bool> callback)
		{
			if (!this.IsAuthenticated())
			{
				GooglePlayGames.OurUtils.Logger.e("ReportScore can only be called after authentication.");
				if (callback != null)
				{
					callback(false);
				}
				return;
			}
			GooglePlayGames.OurUtils.Logger.d(string.Concat(new object[]
			{
				"ReportScore: score=",
				score,
				", board=",
				board
			}));
			string leaderboardId = this.MapId(board);
			this.mClient.SubmitScore(leaderboardId, score, callback);
		}

		// Token: 0x06000D80 RID: 3456 RVA: 0x00044134 File Offset: 0x00042334
		public void ReportScore(long score, string board, string metadata, Action<bool> callback)
		{
			if (!this.IsAuthenticated())
			{
				GooglePlayGames.OurUtils.Logger.e("ReportScore can only be called after authentication.");
				if (callback != null)
				{
					callback(false);
				}
				return;
			}
			GooglePlayGames.OurUtils.Logger.d(string.Concat(new object[]
			{
				"ReportScore: score=",
				score,
				", board=",
				board,
				" metadata=",
				metadata
			}));
			string leaderboardId = this.MapId(board);
			this.mClient.SubmitScore(leaderboardId, score, metadata, callback);
		}

		// Token: 0x06000D81 RID: 3457 RVA: 0x000441B8 File Offset: 0x000423B8
		public void LoadScores(string leaderboardId, Action<IScore[]> callback)
		{
			this.LoadScores(leaderboardId, LeaderboardStart.PlayerCentered, this.mClient.LeaderboardMaxResults(), LeaderboardCollection.Public, LeaderboardTimeSpan.AllTime, delegate(LeaderboardScoreData scoreData)
			{
				callback(scoreData.Scores);
			});
		}

		// Token: 0x06000D82 RID: 3458 RVA: 0x000441F4 File Offset: 0x000423F4
		public void LoadScores(string leaderboardId, LeaderboardStart start, int rowCount, LeaderboardCollection collection, LeaderboardTimeSpan timeSpan, Action<LeaderboardScoreData> callback)
		{
			if (!this.IsAuthenticated())
			{
				GooglePlayGames.OurUtils.Logger.e("LoadScores can only be called after authentication.");
				callback(new LeaderboardScoreData(leaderboardId, ResponseStatus.NotAuthorized));
				return;
			}
			this.mClient.LoadScores(leaderboardId, start, rowCount, collection, timeSpan, callback);
		}

		// Token: 0x06000D83 RID: 3459 RVA: 0x0004423C File Offset: 0x0004243C
		public void LoadMoreScores(ScorePageToken token, int rowCount, Action<LeaderboardScoreData> callback)
		{
			if (!this.IsAuthenticated())
			{
				GooglePlayGames.OurUtils.Logger.e("LoadMoreScores can only be called after authentication.");
				callback(new LeaderboardScoreData(token.LeaderboardId, ResponseStatus.NotAuthorized));
				return;
			}
			this.mClient.LoadMoreScores(token, rowCount, callback);
		}

		// Token: 0x06000D84 RID: 3460 RVA: 0x00044280 File Offset: 0x00042480
		public ILeaderboard CreateLeaderboard()
		{
			return new PlayGamesLeaderboard(this.mDefaultLbUi);
		}

		// Token: 0x06000D85 RID: 3461 RVA: 0x00044290 File Offset: 0x00042490
		public void ShowAchievementsUI()
		{
			this.ShowAchievementsUI(null);
		}

		// Token: 0x06000D86 RID: 3462 RVA: 0x0004429C File Offset: 0x0004249C
		public void ShowAchievementsUI(Action<UIStatus> callback)
		{
			if (!this.IsAuthenticated())
			{
				GooglePlayGames.OurUtils.Logger.e("ShowAchievementsUI can only be called after authentication.");
				return;
			}
			GooglePlayGames.OurUtils.Logger.d("ShowAchievementsUI callback is " + callback);
			this.mClient.ShowAchievementsUI(callback);
		}

		// Token: 0x06000D87 RID: 3463 RVA: 0x000442DC File Offset: 0x000424DC
		public void ShowLeaderboardUI()
		{
			GooglePlayGames.OurUtils.Logger.d("ShowLeaderboardUI with default ID");
			this.ShowLeaderboardUI(this.MapId(this.mDefaultLbUi), null);
		}

		// Token: 0x06000D88 RID: 3464 RVA: 0x000442FC File Offset: 0x000424FC
		public void ShowLeaderboardUI(string leaderboardId)
		{
			if (leaderboardId != null)
			{
				leaderboardId = this.MapId(leaderboardId);
			}
			this.mClient.ShowLeaderboardUI(leaderboardId, LeaderboardTimeSpan.AllTime, null);
		}

		// Token: 0x06000D89 RID: 3465 RVA: 0x0004431C File Offset: 0x0004251C
		public void ShowLeaderboardUI(string leaderboardId, Action<UIStatus> callback)
		{
			this.ShowLeaderboardUI(leaderboardId, LeaderboardTimeSpan.AllTime, callback);
		}

		// Token: 0x06000D8A RID: 3466 RVA: 0x00044328 File Offset: 0x00042528
		public void ShowLeaderboardUI(string leaderboardId, LeaderboardTimeSpan span, Action<UIStatus> callback)
		{
			if (!this.IsAuthenticated())
			{
				GooglePlayGames.OurUtils.Logger.e("ShowLeaderboardUI can only be called after authentication.");
				if (callback != null)
				{
					callback(UIStatus.NotAuthorized);
				}
				return;
			}
			GooglePlayGames.OurUtils.Logger.d(string.Concat(new object[]
			{
				"ShowLeaderboardUI, lbId=",
				leaderboardId,
				" callback is ",
				callback
			}));
			this.mClient.ShowLeaderboardUI(leaderboardId, span, callback);
		}

		// Token: 0x06000D8B RID: 3467 RVA: 0x00044390 File Offset: 0x00042590
		public void SetDefaultLeaderboardForUI(string lbid)
		{
			GooglePlayGames.OurUtils.Logger.d("SetDefaultLeaderboardForUI: " + lbid);
			if (lbid != null)
			{
				lbid = this.MapId(lbid);
			}
			this.mDefaultLbUi = lbid;
		}

		// Token: 0x06000D8C RID: 3468 RVA: 0x000443C4 File Offset: 0x000425C4
		public void LoadFriends(ILocalUser user, Action<bool> callback)
		{
			if (!this.IsAuthenticated())
			{
				GooglePlayGames.OurUtils.Logger.e("LoadScores can only be called after authentication.");
				if (callback != null)
				{
					callback(false);
				}
				return;
			}
			this.mClient.LoadFriends(callback);
		}

		// Token: 0x06000D8D RID: 3469 RVA: 0x000443F8 File Offset: 0x000425F8
		public void LoadScores(ILeaderboard board, Action<bool> callback)
		{
			if (!this.IsAuthenticated())
			{
				GooglePlayGames.OurUtils.Logger.e("LoadScores can only be called after authentication.");
				if (callback != null)
				{
					callback(false);
				}
				return;
			}
			LeaderboardTimeSpan timeSpan;
			switch (board.timeScope)
			{
			case TimeScope.Today:
				timeSpan = LeaderboardTimeSpan.Daily;
				break;
			case TimeScope.Week:
				timeSpan = LeaderboardTimeSpan.Weekly;
				break;
			case TimeScope.AllTime:
				timeSpan = LeaderboardTimeSpan.AllTime;
				break;
			default:
				timeSpan = LeaderboardTimeSpan.AllTime;
				break;
			}
			((PlayGamesLeaderboard)board).loading = true;
			GooglePlayGames.OurUtils.Logger.d(string.Concat(new object[]
			{
				"LoadScores, board=",
				board,
				" callback is ",
				callback
			}));
			this.mClient.LoadScores(board.id, LeaderboardStart.PlayerCentered, (board.range.count <= 0) ? this.mClient.LeaderboardMaxResults() : board.range.count, (board.userScope != UserScope.FriendsOnly) ? LeaderboardCollection.Public : LeaderboardCollection.Social, timeSpan, delegate(LeaderboardScoreData scoreData)
			{
				this.HandleLoadingScores((PlayGamesLeaderboard)board, scoreData, callback);
			});
		}

		// Token: 0x06000D8E RID: 3470 RVA: 0x00044548 File Offset: 0x00042748
		public bool GetLoading(ILeaderboard board)
		{
			return board != null && board.loading;
		}

		// Token: 0x06000D8F RID: 3471 RVA: 0x0004455C File Offset: 0x0004275C
		public void RegisterInvitationDelegate(InvitationReceivedDelegate deleg)
		{
			this.mClient.RegisterInvitationDelegate(deleg);
		}

		// Token: 0x06000D90 RID: 3472 RVA: 0x0004456C File Offset: 0x0004276C
		public string GetToken()
		{
			return this.mClient.GetToken();
		}

		// Token: 0x06000D91 RID: 3473 RVA: 0x0004457C File Offset: 0x0004277C
		internal void HandleLoadingScores(PlayGamesLeaderboard board, LeaderboardScoreData scoreData, Action<bool> callback)
		{
			bool flag = board.SetFromData(scoreData);
			if (flag && !board.HasAllScores() && scoreData.NextPageToken != null)
			{
				int rowCount = board.range.count - board.ScoreCount;
				this.mClient.LoadMoreScores(scoreData.NextPageToken, rowCount, delegate(LeaderboardScoreData nextScoreData)
				{
					this.HandleLoadingScores(board, nextScoreData, callback);
				});
			}
			else
			{
				callback(flag);
			}
		}

		// Token: 0x06000D92 RID: 3474 RVA: 0x00044624 File Offset: 0x00042824
		internal IUserProfile[] GetFriends()
		{
			if (!this.IsAuthenticated())
			{
				GooglePlayGames.OurUtils.Logger.d("Cannot get friends when not authenticated!");
				return new IUserProfile[0];
			}
			return this.mClient.GetFriends();
		}

		// Token: 0x06000D93 RID: 3475 RVA: 0x00044650 File Offset: 0x00042850
		private string MapId(string id)
		{
			if (id == null)
			{
				return null;
			}
			if (this.mIdMap.ContainsKey(id))
			{
				string text = this.mIdMap[id];
				GooglePlayGames.OurUtils.Logger.d("Mapping alias " + id + " to ID " + text);
				return text;
			}
			return id;
		}

		// Token: 0x04000A6F RID: 2671
		private static volatile PlayGamesPlatform sInstance;

		// Token: 0x04000A70 RID: 2672
		private static volatile bool sNearbyInitializePending;

		// Token: 0x04000A71 RID: 2673
		private static volatile INearbyConnectionClient sNearbyConnectionClient;

		// Token: 0x04000A72 RID: 2674
		private readonly PlayGamesClientConfiguration mConfiguration;

		// Token: 0x04000A73 RID: 2675
		private PlayGamesLocalUser mLocalUser;

		// Token: 0x04000A74 RID: 2676
		private IPlayGamesClient mClient;

		// Token: 0x04000A75 RID: 2677
		private string mDefaultLbUi;

		// Token: 0x04000A76 RID: 2678
		private Dictionary<string, string> mIdMap = new Dictionary<string, string>();
	}
}
