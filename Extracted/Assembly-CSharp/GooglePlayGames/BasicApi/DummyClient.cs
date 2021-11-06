using System;
using GooglePlayGames.BasicApi.Events;
using GooglePlayGames.BasicApi.Multiplayer;
using GooglePlayGames.BasicApi.Quests;
using GooglePlayGames.BasicApi.SavedGame;
using GooglePlayGames.OurUtils;
using UnityEngine.SocialPlatforms;

namespace GooglePlayGames.BasicApi
{
	// Token: 0x0200016B RID: 363
	public class DummyClient : IPlayGamesClient
	{
		// Token: 0x06000BA7 RID: 2983 RVA: 0x00041D50 File Offset: 0x0003FF50
		public void Authenticate(Action<bool> callback, bool silent)
		{
			DummyClient.LogUsage();
			if (callback != null)
			{
				callback(false);
			}
		}

		// Token: 0x06000BA8 RID: 2984 RVA: 0x00041D64 File Offset: 0x0003FF64
		public bool IsAuthenticated()
		{
			DummyClient.LogUsage();
			return false;
		}

		// Token: 0x06000BA9 RID: 2985 RVA: 0x00041D6C File Offset: 0x0003FF6C
		public void SignOut()
		{
			DummyClient.LogUsage();
		}

		// Token: 0x06000BAA RID: 2986 RVA: 0x00041D74 File Offset: 0x0003FF74
		public string GetAccessToken()
		{
			DummyClient.LogUsage();
			return "DummyAccessToken";
		}

		// Token: 0x06000BAB RID: 2987 RVA: 0x00041D80 File Offset: 0x0003FF80
		public void GetIdToken(Action<string> idTokenCallback)
		{
			DummyClient.LogUsage();
			if (idTokenCallback != null)
			{
				idTokenCallback("DummyIdToken");
			}
		}

		// Token: 0x06000BAC RID: 2988 RVA: 0x00041D98 File Offset: 0x0003FF98
		public string GetUserId()
		{
			DummyClient.LogUsage();
			return "DummyID";
		}

		// Token: 0x06000BAD RID: 2989 RVA: 0x00041DA4 File Offset: 0x0003FFA4
		public string GetToken()
		{
			return "DummyToken";
		}

		// Token: 0x06000BAE RID: 2990 RVA: 0x00041DAC File Offset: 0x0003FFAC
		public void GetServerAuthCode(string serverClientId, Action<CommonStatusCodes, string> callback)
		{
			DummyClient.LogUsage();
			if (callback != null)
			{
				callback(CommonStatusCodes.ApiNotConnected, "DummyServerAuthCode");
			}
		}

		// Token: 0x06000BAF RID: 2991 RVA: 0x00041DC8 File Offset: 0x0003FFC8
		public string GetUserEmail()
		{
			return string.Empty;
		}

		// Token: 0x06000BB0 RID: 2992 RVA: 0x00041DD0 File Offset: 0x0003FFD0
		public void GetUserEmail(Action<CommonStatusCodes, string> callback)
		{
			DummyClient.LogUsage();
			if (callback != null)
			{
				callback(CommonStatusCodes.ApiNotConnected, null);
			}
		}

		// Token: 0x06000BB1 RID: 2993 RVA: 0x00041DE8 File Offset: 0x0003FFE8
		public void GetPlayerStats(Action<CommonStatusCodes, PlayerStats> callback)
		{
			DummyClient.LogUsage();
			callback(CommonStatusCodes.ApiNotConnected, new PlayerStats());
		}

		// Token: 0x06000BB2 RID: 2994 RVA: 0x00041DFC File Offset: 0x0003FFFC
		public string GetUserDisplayName()
		{
			DummyClient.LogUsage();
			return "Player";
		}

		// Token: 0x06000BB3 RID: 2995 RVA: 0x00041E08 File Offset: 0x00040008
		public string GetUserImageUrl()
		{
			DummyClient.LogUsage();
			return null;
		}

		// Token: 0x06000BB4 RID: 2996 RVA: 0x00041E10 File Offset: 0x00040010
		public void LoadUsers(string[] userIds, Action<IUserProfile[]> callback)
		{
			DummyClient.LogUsage();
			if (callback != null)
			{
				callback(null);
			}
		}

		// Token: 0x06000BB5 RID: 2997 RVA: 0x00041E24 File Offset: 0x00040024
		public void LoadAchievements(Action<Achievement[]> callback)
		{
			DummyClient.LogUsage();
			if (callback != null)
			{
				callback(null);
			}
		}

		// Token: 0x06000BB6 RID: 2998 RVA: 0x00041E38 File Offset: 0x00040038
		public Achievement GetAchievement(string achId)
		{
			DummyClient.LogUsage();
			return null;
		}

		// Token: 0x06000BB7 RID: 2999 RVA: 0x00041E40 File Offset: 0x00040040
		public void UnlockAchievement(string achId, Action<bool> callback)
		{
			DummyClient.LogUsage();
			if (callback != null)
			{
				callback(false);
			}
		}

		// Token: 0x06000BB8 RID: 3000 RVA: 0x00041E54 File Offset: 0x00040054
		public void RevealAchievement(string achId, Action<bool> callback)
		{
			DummyClient.LogUsage();
			if (callback != null)
			{
				callback(false);
			}
		}

		// Token: 0x06000BB9 RID: 3001 RVA: 0x00041E68 File Offset: 0x00040068
		public void IncrementAchievement(string achId, int steps, Action<bool> callback)
		{
			DummyClient.LogUsage();
			if (callback != null)
			{
				callback(false);
			}
		}

		// Token: 0x06000BBA RID: 3002 RVA: 0x00041E7C File Offset: 0x0004007C
		public void SetStepsAtLeast(string achId, int steps, Action<bool> callback)
		{
			DummyClient.LogUsage();
			if (callback != null)
			{
				callback(false);
			}
		}

		// Token: 0x06000BBB RID: 3003 RVA: 0x00041E90 File Offset: 0x00040090
		public void ShowAchievementsUI(Action<UIStatus> callback)
		{
			DummyClient.LogUsage();
			if (callback != null)
			{
				callback(UIStatus.VersionUpdateRequired);
			}
		}

		// Token: 0x06000BBC RID: 3004 RVA: 0x00041EA8 File Offset: 0x000400A8
		public void ShowLeaderboardUI(string leaderboardId, LeaderboardTimeSpan span, Action<UIStatus> callback)
		{
			DummyClient.LogUsage();
			if (callback != null)
			{
				callback(UIStatus.VersionUpdateRequired);
			}
		}

		// Token: 0x06000BBD RID: 3005 RVA: 0x00041EC0 File Offset: 0x000400C0
		public int LeaderboardMaxResults()
		{
			return 25;
		}

		// Token: 0x06000BBE RID: 3006 RVA: 0x00041EC4 File Offset: 0x000400C4
		public void LoadScores(string leaderboardId, LeaderboardStart start, int rowCount, LeaderboardCollection collection, LeaderboardTimeSpan timeSpan, Action<LeaderboardScoreData> callback)
		{
			DummyClient.LogUsage();
			if (callback != null)
			{
				callback(new LeaderboardScoreData(leaderboardId, ResponseStatus.LicenseCheckFailed));
			}
		}

		// Token: 0x06000BBF RID: 3007 RVA: 0x00041EE0 File Offset: 0x000400E0
		public void LoadMoreScores(ScorePageToken token, int rowCount, Action<LeaderboardScoreData> callback)
		{
			DummyClient.LogUsage();
			if (callback != null)
			{
				callback(new LeaderboardScoreData(token.LeaderboardId, ResponseStatus.LicenseCheckFailed));
			}
		}

		// Token: 0x06000BC0 RID: 3008 RVA: 0x00041F00 File Offset: 0x00040100
		public void SubmitScore(string leaderboardId, long score, Action<bool> callback)
		{
			DummyClient.LogUsage();
			if (callback != null)
			{
				callback(false);
			}
		}

		// Token: 0x06000BC1 RID: 3009 RVA: 0x00041F14 File Offset: 0x00040114
		public void SubmitScore(string leaderboardId, long score, string metadata, Action<bool> callback)
		{
			DummyClient.LogUsage();
			if (callback != null)
			{
				callback(false);
			}
		}

		// Token: 0x06000BC2 RID: 3010 RVA: 0x00041F2C File Offset: 0x0004012C
		public IRealTimeMultiplayerClient GetRtmpClient()
		{
			DummyClient.LogUsage();
			return null;
		}

		// Token: 0x06000BC3 RID: 3011 RVA: 0x00041F34 File Offset: 0x00040134
		public ITurnBasedMultiplayerClient GetTbmpClient()
		{
			DummyClient.LogUsage();
			return null;
		}

		// Token: 0x06000BC4 RID: 3012 RVA: 0x00041F3C File Offset: 0x0004013C
		public ISavedGameClient GetSavedGameClient()
		{
			DummyClient.LogUsage();
			return null;
		}

		// Token: 0x06000BC5 RID: 3013 RVA: 0x00041F44 File Offset: 0x00040144
		public IEventsClient GetEventsClient()
		{
			DummyClient.LogUsage();
			return null;
		}

		// Token: 0x06000BC6 RID: 3014 RVA: 0x00041F4C File Offset: 0x0004014C
		public IQuestsClient GetQuestsClient()
		{
			DummyClient.LogUsage();
			return null;
		}

		// Token: 0x06000BC7 RID: 3015 RVA: 0x00041F54 File Offset: 0x00040154
		public void RegisterInvitationDelegate(InvitationReceivedDelegate invitationDelegate)
		{
			DummyClient.LogUsage();
		}

		// Token: 0x06000BC8 RID: 3016 RVA: 0x00041F5C File Offset: 0x0004015C
		public Invitation GetInvitationFromNotification()
		{
			DummyClient.LogUsage();
			return null;
		}

		// Token: 0x06000BC9 RID: 3017 RVA: 0x00041F64 File Offset: 0x00040164
		public bool HasInvitationFromNotification()
		{
			DummyClient.LogUsage();
			return false;
		}

		// Token: 0x06000BCA RID: 3018 RVA: 0x00041F6C File Offset: 0x0004016C
		public void LoadFriends(Action<bool> callback)
		{
			DummyClient.LogUsage();
			callback(false);
		}

		// Token: 0x06000BCB RID: 3019 RVA: 0x00041F7C File Offset: 0x0004017C
		public IUserProfile[] GetFriends()
		{
			DummyClient.LogUsage();
			return new IUserProfile[0];
		}

		// Token: 0x06000BCC RID: 3020 RVA: 0x00041F8C File Offset: 0x0004018C
		public IntPtr GetApiClient()
		{
			DummyClient.LogUsage();
			return IntPtr.Zero;
		}

		// Token: 0x06000BCD RID: 3021 RVA: 0x00041F98 File Offset: 0x00040198
		private static void LogUsage()
		{
			Logger.d("Received method call on DummyClient - using stub implementation.");
		}
	}
}
