using System;
using GooglePlayGames.BasicApi.Events;
using GooglePlayGames.BasicApi.Multiplayer;
using GooglePlayGames.BasicApi.Quests;
using GooglePlayGames.BasicApi.SavedGame;
using UnityEngine.SocialPlatforms;

namespace GooglePlayGames.BasicApi
{
	// Token: 0x0200016F RID: 367
	public interface IPlayGamesClient
	{
		// Token: 0x06000BD7 RID: 3031
		void Authenticate(Action<bool> callback, bool silent);

		// Token: 0x06000BD8 RID: 3032
		bool IsAuthenticated();

		// Token: 0x06000BD9 RID: 3033
		void SignOut();

		// Token: 0x06000BDA RID: 3034
		string GetToken();

		// Token: 0x06000BDB RID: 3035
		string GetUserId();

		// Token: 0x06000BDC RID: 3036
		void LoadFriends(Action<bool> callback);

		// Token: 0x06000BDD RID: 3037
		string GetUserDisplayName();

		// Token: 0x06000BDE RID: 3038
		void GetIdToken(Action<string> idTokenCallback);

		// Token: 0x06000BDF RID: 3039
		string GetAccessToken();

		// Token: 0x06000BE0 RID: 3040
		void GetServerAuthCode(string serverClientId, Action<CommonStatusCodes, string> callback);

		// Token: 0x06000BE1 RID: 3041
		string GetUserEmail();

		// Token: 0x06000BE2 RID: 3042
		void GetUserEmail(Action<CommonStatusCodes, string> callback);

		// Token: 0x06000BE3 RID: 3043
		string GetUserImageUrl();

		// Token: 0x06000BE4 RID: 3044
		void GetPlayerStats(Action<CommonStatusCodes, PlayerStats> callback);

		// Token: 0x06000BE5 RID: 3045
		void LoadUsers(string[] userIds, Action<IUserProfile[]> callback);

		// Token: 0x06000BE6 RID: 3046
		Achievement GetAchievement(string achievementId);

		// Token: 0x06000BE7 RID: 3047
		void LoadAchievements(Action<Achievement[]> callback);

		// Token: 0x06000BE8 RID: 3048
		void UnlockAchievement(string achievementId, Action<bool> successOrFailureCalllback);

		// Token: 0x06000BE9 RID: 3049
		void RevealAchievement(string achievementId, Action<bool> successOrFailureCalllback);

		// Token: 0x06000BEA RID: 3050
		void IncrementAchievement(string achievementId, int steps, Action<bool> successOrFailureCalllback);

		// Token: 0x06000BEB RID: 3051
		void SetStepsAtLeast(string achId, int steps, Action<bool> callback);

		// Token: 0x06000BEC RID: 3052
		void ShowAchievementsUI(Action<UIStatus> callback);

		// Token: 0x06000BED RID: 3053
		void ShowLeaderboardUI(string leaderboardId, LeaderboardTimeSpan span, Action<UIStatus> callback);

		// Token: 0x06000BEE RID: 3054
		void LoadScores(string leaderboardId, LeaderboardStart start, int rowCount, LeaderboardCollection collection, LeaderboardTimeSpan timeSpan, Action<LeaderboardScoreData> callback);

		// Token: 0x06000BEF RID: 3055
		void LoadMoreScores(ScorePageToken token, int rowCount, Action<LeaderboardScoreData> callback);

		// Token: 0x06000BF0 RID: 3056
		int LeaderboardMaxResults();

		// Token: 0x06000BF1 RID: 3057
		void SubmitScore(string leaderboardId, long score, Action<bool> successOrFailureCalllback);

		// Token: 0x06000BF2 RID: 3058
		void SubmitScore(string leaderboardId, long score, string metadata, Action<bool> successOrFailureCalllback);

		// Token: 0x06000BF3 RID: 3059
		IRealTimeMultiplayerClient GetRtmpClient();

		// Token: 0x06000BF4 RID: 3060
		ITurnBasedMultiplayerClient GetTbmpClient();

		// Token: 0x06000BF5 RID: 3061
		ISavedGameClient GetSavedGameClient();

		// Token: 0x06000BF6 RID: 3062
		IEventsClient GetEventsClient();

		// Token: 0x06000BF7 RID: 3063
		IQuestsClient GetQuestsClient();

		// Token: 0x06000BF8 RID: 3064
		void RegisterInvitationDelegate(InvitationReceivedDelegate invitationDelegate);

		// Token: 0x06000BF9 RID: 3065
		IUserProfile[] GetFriends();

		// Token: 0x06000BFA RID: 3066
		IntPtr GetApiClient();
	}
}
