using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Rilisoft;
using Rilisoft.MiniJson;
using Rilisoft.NullExtensions;
using SimpleJSON;
using UnityEngine;

// Token: 0x02000124 RID: 292
public sealed class FriendsController : MonoBehaviour
{
	// Token: 0x06000865 RID: 2149 RVA: 0x00033CC4 File Offset: 0x00031EC4
	// Note: this type is marked as 'beforefieldinit'.
	static FriendsController()
	{
		FriendsController.FriendsUpdated = null;
		FriendsController.FullInfoUpdated = null;
		FriendsController.ServerTimeUpdated = null;
	}

	// Token: 0x1400000C RID: 12
	// (add) Token: 0x06000866 RID: 2150 RVA: 0x00033D74 File Offset: 0x00031F74
	// (remove) Token: 0x06000867 RID: 2151 RVA: 0x00033D8C File Offset: 0x00031F8C
	public static event Action FriendsUpdated;

	// Token: 0x1400000D RID: 13
	// (add) Token: 0x06000868 RID: 2152 RVA: 0x00033DA4 File Offset: 0x00031FA4
	// (remove) Token: 0x06000869 RID: 2153 RVA: 0x00033DBC File Offset: 0x00031FBC
	public static event Action ClanUpdated;

	// Token: 0x1400000E RID: 14
	// (add) Token: 0x0600086A RID: 2154 RVA: 0x00033DD4 File Offset: 0x00031FD4
	// (remove) Token: 0x0600086B RID: 2155 RVA: 0x00033DEC File Offset: 0x00031FEC
	public static event Action FullInfoUpdated;

	// Token: 0x1400000F RID: 15
	// (add) Token: 0x0600086C RID: 2156 RVA: 0x00033E04 File Offset: 0x00032004
	// (remove) Token: 0x0600086D RID: 2157 RVA: 0x00033E1C File Offset: 0x0003201C
	public static event Action ServerTimeUpdated;

	// Token: 0x14000010 RID: 16
	// (add) Token: 0x0600086E RID: 2158 RVA: 0x00033E34 File Offset: 0x00032034
	// (remove) Token: 0x0600086F RID: 2159 RVA: 0x00033E50 File Offset: 0x00032050
	public event Action FailedSendNewClan;

	// Token: 0x14000011 RID: 17
	// (add) Token: 0x06000870 RID: 2160 RVA: 0x00033E6C File Offset: 0x0003206C
	// (remove) Token: 0x06000871 RID: 2161 RVA: 0x00033E88 File Offset: 0x00032088
	public event Action<int> ReturnNewIDClan;

	// Token: 0x14000012 RID: 18
	// (add) Token: 0x06000872 RID: 2162 RVA: 0x00033EA4 File Offset: 0x000320A4
	// (remove) Token: 0x06000873 RID: 2163 RVA: 0x00033EBC File Offset: 0x000320BC
	public static event Action<string> OnClanIdSetted;

	// Token: 0x14000013 RID: 19
	// (add) Token: 0x06000874 RID: 2164 RVA: 0x00033ED4 File Offset: 0x000320D4
	// (remove) Token: 0x06000875 RID: 2165 RVA: 0x00033EEC File Offset: 0x000320EC
	public static event Action<int, int> NewFacebookLimitsAvailable;

	// Token: 0x14000014 RID: 20
	// (add) Token: 0x06000876 RID: 2166 RVA: 0x00033F04 File Offset: 0x00032104
	// (remove) Token: 0x06000877 RID: 2167 RVA: 0x00033F1C File Offset: 0x0003211C
	public static event Action<int, int, int, int> NewTwitterLimitsAvailable;

	// Token: 0x14000015 RID: 21
	// (add) Token: 0x06000878 RID: 2168 RVA: 0x00033F34 File Offset: 0x00032134
	// (remove) Token: 0x06000879 RID: 2169 RVA: 0x00033F4C File Offset: 0x0003214C
	public static event Action<int, int, int, int> NewCheaterDetectParametersAvailable;

	// Token: 0x14000016 RID: 22
	// (add) Token: 0x0600087A RID: 2170 RVA: 0x00033F64 File Offset: 0x00032164
	// (remove) Token: 0x0600087B RID: 2171 RVA: 0x00033F7C File Offset: 0x0003217C
	public static event Action OurInfoUpdated;

	// Token: 0x170000E7 RID: 231
	// (get) Token: 0x0600087C RID: 2172 RVA: 0x00033F94 File Offset: 0x00032194
	// (set) Token: 0x0600087D RID: 2173 RVA: 0x00033FC8 File Offset: 0x000321C8
	public int currentCompetition
	{
		get
		{
			if (this._currentCompetition < 0)
			{
				this._currentCompetition = Storager.getInt(this.currentCompetitionKey, false);
			}
			return this._currentCompetition;
		}
		internal set
		{
			this._currentCompetition = value;
			Storager.setInt(this.currentCompetitionKey, this._currentCompetition, false);
		}
	}

	// Token: 0x170000E8 RID: 232
	// (get) Token: 0x0600087E RID: 2174 RVA: 0x00033FE4 File Offset: 0x000321E4
	// (set) Token: 0x0600087F RID: 2175 RVA: 0x00034014 File Offset: 0x00032214
	public float expirationTimeCompetition
	{
		get
		{
			if (!FriendsController._expirationTimeCompetitionInit)
			{
				int @int = PlayerPrefs.GetInt(FriendsController.expirationTimeCompetitionKey, 0);
			}
			FriendsController._expirationTimeCompetitionInit = true;
			return this._expirationTimeCompetition;
		}
		private set
		{
			this._expirationTimeCompetition = value + Time.realtimeSinceStartup;
			PlayerPrefs.SetInt(FriendsController.expirationTimeCompetitionKey, Mathf.RoundToInt(value));
			FriendsController._expirationTimeCompetitionInit = true;
		}
	}

	// Token: 0x170000E9 RID: 233
	// (get) Token: 0x06000880 RID: 2176 RVA: 0x0003403C File Offset: 0x0003223C
	public bool ClanLimitReached
	{
		get
		{
			FriendsController friendsController = FriendsController.sharedController;
			return friendsController.clanMembers.Count + friendsController.ClanSentInvites.Count + friendsController.clanSentInvitesLocal.Count >= friendsController.ClanLimit;
		}
	}

	// Token: 0x170000EA RID: 234
	// (get) Token: 0x06000881 RID: 2177 RVA: 0x00034080 File Offset: 0x00032280
	public int ClanLimit
	{
		get
		{
			return Defs.maxMemberClanCount;
		}
	}

	// Token: 0x170000EB RID: 235
	// (get) Token: 0x06000882 RID: 2178 RVA: 0x00034088 File Offset: 0x00032288
	internal static bool PolygonEnabled
	{
		get
		{
			return Defs.IsDeveloperBuild;
		}
	}

	// Token: 0x170000EC RID: 236
	// (get) Token: 0x06000883 RID: 2179 RVA: 0x00034090 File Offset: 0x00032290
	// (set) Token: 0x06000884 RID: 2180 RVA: 0x00034098 File Offset: 0x00032298
	internal static bool AdvertEnabled
	{
		get
		{
			return FriendsController._advertEnabled;
		}
		set
		{
			FriendsController._advertEnabled = value;
		}
	}

	// Token: 0x170000ED RID: 237
	// (get) Token: 0x06000885 RID: 2181 RVA: 0x000340A0 File Offset: 0x000322A0
	// (set) Token: 0x06000886 RID: 2182 RVA: 0x000340A8 File Offset: 0x000322A8
	public static bool ClanDataSettted { get; private set; }

	// Token: 0x170000EE RID: 238
	// (get) Token: 0x06000887 RID: 2183 RVA: 0x000340B0 File Offset: 0x000322B0
	public static int CurrentPlatform
	{
		get
		{
			return ProtocolListGetter.CurrentPlatform;
		}
	}

	// Token: 0x170000EF RID: 239
	// (get) Token: 0x06000888 RID: 2184 RVA: 0x000340B8 File Offset: 0x000322B8
	// (set) Token: 0x06000889 RID: 2185 RVA: 0x000340C0 File Offset: 0x000322C0
	public string ClanID
	{
		get
		{
			return this._clanId;
		}
		set
		{
			this._clanId = value;
			if (FriendsController.OnClanIdSetted != null)
			{
				FriendsController.OnClanIdSetted(this._clanId);
			}
		}
	}

	// Token: 0x170000F0 RID: 240
	// (get) Token: 0x0600088A RID: 2186 RVA: 0x000340E4 File Offset: 0x000322E4
	public static long ServerTime
	{
		get
		{
			if (FriendsController.isUpdateServerTimeAfterRun)
			{
				return FriendsController.localServerTime;
			}
			return -1L;
		}
	}

	// Token: 0x170000F1 RID: 241
	// (get) Token: 0x0600088B RID: 2187 RVA: 0x000340F8 File Offset: 0x000322F8
	public static string actionAddress
	{
		get
		{
			return URLs.Friends;
		}
	}

	// Token: 0x170000F2 RID: 242
	// (get) Token: 0x0600088C RID: 2188 RVA: 0x00034100 File Offset: 0x00032300
	// (set) Token: 0x0600088D RID: 2189 RVA: 0x00034108 File Offset: 0x00032308
	public string id
	{
		get
		{
			return this._id;
		}
		set
		{
			this._id = value;
		}
	}

	// Token: 0x0600088E RID: 2190 RVA: 0x00034114 File Offset: 0x00032314
	public void FastGetPixelbookSettings()
	{
		this.timerUpdatePixelbookSetting = -1f;
	}

	// Token: 0x0600088F RID: 2191 RVA: 0x00034124 File Offset: 0x00032324
	private IEnumerator GetPixelbookSettingsLoop(Task futureToWait)
	{
		yield return new WaitUntil(() => futureToWait.IsCompleted);
		this.timerUpdatePixelbookSetting = Defs.timeUpdatePixelbookInfo;
		for (;;)
		{
			yield return base.StartCoroutine(this.GetPixelbookSettings());
			while (this.timerUpdatePixelbookSetting > 0f)
			{
				this.timerUpdatePixelbookSetting -= Time.unscaledDeltaTime;
				yield return null;
			}
			this.timerUpdatePixelbookSetting = Defs.timeUpdatePixelbookInfo;
		}
		yield break;
	}

	// Token: 0x06000890 RID: 2192 RVA: 0x00034150 File Offset: 0x00032350
	private IEnumerator GetNewsLoop(Task futureToWait)
	{
		yield return new WaitUntil(() => futureToWait.IsCompleted);
		while (!TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage <= TrainingController.NewTrainingCompletedStage.ShopCompleted)
		{
			yield return null;
		}
		for (;;)
		{
			yield return base.StartCoroutine(this.GetLobbyNews(false));
			yield return new WaitForSeconds(Defs.timeUpdateNews);
		}
		yield break;
	}

	// Token: 0x06000891 RID: 2193 RVA: 0x0003417C File Offset: 0x0003237C
	private IEnumerator GetFiltersSettings()
	{
		WWW response = Tools.CreateWwwIfNotConnected(URLs.FilterBadWord);
		if (response == null)
		{
			yield break;
		}
		yield return response;
		if (!string.IsNullOrEmpty(response.error))
		{
			Debug.LogWarning("FilterBadWord response error: " + response.error);
			yield break;
		}
		string responseText = URLs.Sanitize(response);
		if (string.IsNullOrEmpty(responseText))
		{
			Debug.LogWarning("FilterBadWord response is empty");
			yield break;
		}
		Dictionary<string, object> filterDict = Json.Deserialize(responseText) as Dictionary<string, object>;
		string wordsList = Json.Serialize(filterDict["Words"]);
		string symbolsList = Json.Serialize(filterDict["Symbols"]);
		PlayerPrefs.SetString("PixelFilterWordsKey", wordsList);
		PlayerPrefs.SetString("PixelFilterSymbolsKey", symbolsList);
		PlayerPrefs.Save();
		FilterBadWorld.InitBadLists();
		yield break;
	}

	// Token: 0x06000892 RID: 2194 RVA: 0x00034190 File Offset: 0x00032390
	private IEnumerator GetBuffSettings(Task futureToWait)
	{
		yield return new WaitUntil(() => futureToWait.IsCompleted);
		string url = (!ABTestController.useBuffSystem) ? URLs.BuffSettings1031 : URLs.BuffSettings1050;
		string cachedResponse = PersistentCacheManager.Instance.GetValue(url);
		string responseText;
		if (!string.IsNullOrEmpty(cachedResponse))
		{
			responseText = cachedResponse;
		}
		else
		{
			WWW response;
			for (;;)
			{
				response = Tools.CreateWwwIfNotConnected(url);
				if (response == null)
				{
					yield return new WaitForSeconds(20f);
				}
				else
				{
					yield return response;
					if (!string.IsNullOrEmpty(response.error))
					{
						Debug.LogWarning("GetBuffSettings response error: " + response.error);
						yield return new WaitForSeconds(20f);
					}
					else
					{
						responseText = URLs.Sanitize(response);
						if (!string.IsNullOrEmpty(responseText))
						{
							break;
						}
						Debug.LogWarning("GetBuffSettings response is empty");
						yield return new WaitForSeconds(20f);
					}
				}
			}
			PersistentCacheManager.Instance.SetValue(response.url, responseText);
		}
		Storager.setString("BuffsParam", responseText, false);
		if (ABTestController.useBuffSystem)
		{
			BuffSystem.instance.TryLoadConfig();
		}
		yield break;
	}

	// Token: 0x06000893 RID: 2195 RVA: 0x000341B4 File Offset: 0x000323B4
	private IEnumerator GetRatingSystemConfig()
	{
		for (;;)
		{
			WWWForm form = new WWWForm();
			WWW download = Tools.CreateWwwIfNotConnected(URLs.RatingSystemConfigURL);
			if (download == null)
			{
				yield return base.StartCoroutine(this.MyWaitForSeconds(60f));
			}
			else
			{
				yield return download;
				if (!string.IsNullOrEmpty(download.error))
				{
					if (Debug.isDebugBuild || Application.isEditor)
					{
						Debug.LogWarning("GetRatingSystemConfig error: " + download.error);
					}
					yield return base.StartCoroutine(this.MyWaitForSeconds(600f));
				}
				else
				{
					string responseText = URLs.Sanitize(download);
					if (!string.IsNullOrEmpty(responseText))
					{
						Storager.setString("rSCKeyV2", responseText, false);
						RatingSystem.instance.ParseConfig();
					}
					yield return base.StartCoroutine(this.MyWaitForSeconds(1800f));
				}
			}
		}
		yield break;
	}

	// Token: 0x06000894 RID: 2196 RVA: 0x000341D0 File Offset: 0x000323D0
	private IEnumerator GetLobbyNews(bool fromPause)
	{
		string url = URLs.LobbyNews;
		string cachedResponse = PersistentCacheManager.Instance.GetValue(url);
		string responseText;
		if (!string.IsNullOrEmpty(cachedResponse))
		{
			responseText = cachedResponse;
		}
		else
		{
			WWW response = Tools.CreateWwwIfNotConnected(url);
			if (response == null)
			{
				yield break;
			}
			yield return response;
			if (!string.IsNullOrEmpty(response.error))
			{
				Debug.LogWarning("GetLobbyNews response error: " + response.error);
				yield break;
			}
			responseText = URLs.Sanitize(response);
			PersistentCacheManager.Instance.SetValue(response.url, responseText);
		}
		if (string.IsNullOrEmpty(responseText))
		{
			Debug.LogWarning("GetLobbyNews response is empty");
			yield break;
		}
		string oldNews = PlayerPrefs.GetString("LobbyNewsKey", "[]");
		bool isAnyNews = false;
		List<object> oldNewsAsList = Json.Deserialize(oldNews) as List<object>;
		List<Dictionary<string, object>> oldNewsAll = (oldNewsAsList == null) ? new List<Dictionary<string, object>>() : oldNewsAsList.OfType<Dictionary<string, object>>().ToList<Dictionary<string, object>>();
		List<object> responseTextAsList = Json.Deserialize(responseText) as List<object>;
		List<Dictionary<string, object>> newsAll = (responseTextAsList == null) ? new List<Dictionary<string, object>>() : responseTextAsList.OfType<Dictionary<string, object>>().ToList<Dictionary<string, object>>();
		if (newsAll.Count == 0)
		{
			isAnyNews = false;
		}
		else
		{
			for (int i = 0; i < newsAll.Count; i++)
			{
				newsAll[i]["readed"] = 0;
				bool isOld = false;
				for (int o = 0; o < oldNewsAll.Count; o++)
				{
					if (Convert.ToInt32(oldNewsAll[o]["date"]) == Convert.ToInt32(newsAll[i]["date"]))
					{
						isOld = true;
						if (oldNewsAll[o].ContainsKey("readed"))
						{
							newsAll[i]["readed"] = oldNewsAll[o]["readed"];
						}
						break;
					}
				}
				try
				{
					if (!isOld)
					{
						AnalyticsFacade.SendCustomEvent("News", new Dictionary<string, object>
						{
							{
								"CTR",
								"Show"
							},
							{
								"Conversion Total",
								"Show"
							}
						});
					}
				}
				catch (Exception ex)
				{
					Exception e = ex;
					Debug.LogError("Exception in log News (CTR = Show, Conversion Total = Show): " + e);
				}
				if (Convert.ToInt32(newsAll[i]["readed"]) == 0)
				{
					isAnyNews = true;
				}
			}
		}
		PlayerPrefs.SetString("LobbyNewsKey", Json.Serialize(newsAll));
		PlayerPrefs.SetInt("LobbyIsAnyNewsKey", (!isAnyNews) ? 0 : 1);
		PlayerPrefs.Save();
		if (isAnyNews && MainMenuController.sharedController != null)
		{
			MainMenuController.sharedController.newsIndicator.SetActive(true);
		}
		yield break;
	}

	// Token: 0x06000895 RID: 2197 RVA: 0x000341E4 File Offset: 0x000323E4
	private IEnumerator GetTimeFromServerLoop()
	{
		FriendsController.isUpdateServerTimeAfterRun = false;
		while (!TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage < TrainingController.NewTrainingCompletedStage.ShootingRangeCompleted)
		{
			yield return null;
		}
		while (string.IsNullOrEmpty(this.id))
		{
			yield return null;
		}
		while (!FriendsController.isUpdateServerTimeAfterRun)
		{
			yield return base.StartCoroutine(this.GetTimeFromServer());
			float timerUpdate = Defs.timeUpdateServerTime;
			while (!FriendsController.isUpdateServerTimeAfterRun && timerUpdate > 0f)
			{
				timerUpdate -= Time.unscaledDeltaTime;
				yield return null;
			}
		}
		if (FriendsController.ServerTimeUpdated != null)
		{
			FriendsController.ServerTimeUpdated();
		}
		yield break;
	}

	// Token: 0x06000896 RID: 2198 RVA: 0x00034200 File Offset: 0x00032400
	private IEnumerator GetTimeFromServer()
	{
		WWWForm wwwForm = new WWWForm();
		wwwForm.AddField("action", "get_time");
		wwwForm.AddField("app_version", string.Format("{0}:{1}", ProtocolListGetter.CurrentPlatform, GlobalGameController.AppVersion));
		wwwForm.AddField("uniq_id", FriendsController.sharedController.id);
		wwwForm.AddField("auth", FriendsController.Hash("get_time", null));
		WWW download = Tools.CreateWww((!this.isGetServerTimeFromMainUrl) ? URLs.TimeOnSecure : URLs.Friends, wwwForm, string.Empty);
		if (download == null)
		{
			yield break;
		}
		yield return download;
		string response = URLs.Sanitize(download);
		if (!string.IsNullOrEmpty(download.error))
		{
			Debug.LogWarning("get_time error:    " + download.error);
			this.isGetServerTimeFromMainUrl = !this.isGetServerTimeFromMainUrl;
			yield break;
		}
		long currentServerTime;
		if (long.TryParse(response, out currentServerTime))
		{
			FriendsController.localServerTime = currentServerTime;
			FriendsController.tickForServerTime = 0f;
			FriendsController.isUpdateServerTimeAfterRun = true;
		}
		else
		{
			Debug.LogError("Could not parse response: " + response);
			this.isGetServerTimeFromMainUrl = !this.isGetServerTimeFromMainUrl;
		}
		yield break;
	}

	// Token: 0x06000897 RID: 2199 RVA: 0x0003421C File Offset: 0x0003241C
	private IEnumerator GetPixelbookSettings()
	{
		string url = URLs.PixelbookSettings;
		string cachedResponse = PersistentCacheManager.Instance.GetValue(url);
		string responseText;
		if (!string.IsNullOrEmpty(cachedResponse))
		{
			responseText = cachedResponse;
		}
		else
		{
			WWW response = Tools.CreateWwwIfNotConnected(url);
			if (response == null)
			{
				yield break;
			}
			yield return response;
			if (!string.IsNullOrEmpty(response.error))
			{
				Debug.LogWarning("PixelbookSettings response error: " + response.error);
				yield break;
			}
			responseText = URLs.Sanitize(response);
			if (string.IsNullOrEmpty(responseText))
			{
				Debug.LogWarning("PixelbookSettings response is empty");
				yield break;
			}
			PersistentCacheManager.Instance.SetValue(url, responseText);
		}
		PlayerPrefs.SetString("PixelbookSettingsKey", responseText);
		PlayerPrefs.Save();
		FriendsController.UpdatePixelbookSettingsFromPrefs();
		FriendsController.isInitPixelbookSettingsFromServer = true;
		yield break;
	}

	// Token: 0x06000898 RID: 2200 RVA: 0x00034230 File Offset: 0x00032430
	public static void UpdatePixelbookSettingsFromPrefs()
	{
		string @string = PlayerPrefs.GetString("PixelbookSettingsKey", "{}");
		Dictionary<string, object> dictionary = Json.Deserialize(@string) as Dictionary<string, object>;
		if (dictionary == null)
		{
			return;
		}
		if (dictionary.ContainsKey("MaxFriendCount"))
		{
			if (dictionary.ContainsKey("FriendsUrl"))
			{
				URLs.Friends = Convert.ToString(dictionary["FriendsUrl"]);
			}
			if (dictionary.ContainsKey("MaxFriendCount"))
			{
				Defs.maxCountFriend = Convert.ToInt32(dictionary["MaxFriendCount"]);
			}
			if (dictionary.ContainsKey("MaxMemberClanCount"))
			{
				Defs.maxMemberClanCount = Convert.ToInt32(dictionary["MaxMemberClanCount"]);
			}
			if (dictionary.ContainsKey("TimeUpdateFriendInfo"))
			{
				Defs.timeUpdateFriendInfo = (float)Convert.ToInt32(dictionary["TimeUpdateFriendInfo"]);
			}
			if (dictionary.ContainsKey("TimeUpdateOnlineInGame"))
			{
				Defs.timeUpdateOnlineInGame = (float)Convert.ToInt32(dictionary["TimeUpdateOnlineInGame"]);
			}
			if (dictionary.ContainsKey("TimeUpdateInfoInProfile"))
			{
				Defs.timeUpdateInfoInProfile = (float)Convert.ToInt32(dictionary["TimeUpdateInfoInProfile"]);
			}
			if (dictionary.ContainsKey("TimeUpdateLeaderboardIfNullResponce"))
			{
				Defs.timeUpdateLeaderboardIfNullResponce = (float)Convert.ToInt32(dictionary["TimeUpdateLeaderboardIfNullResponce"]);
			}
			if (dictionary.ContainsKey("TimeBlockRefreshFriendDate"))
			{
				Defs.timeBlockRefreshFriendDate = (float)Convert.ToInt32(dictionary["TimeBlockRefreshFriendDate"]);
			}
			if (dictionary.ContainsKey("TimeWaitLoadPossibleFriends"))
			{
				Defs.timeWaitLoadPossibleFriends = (float)Convert.ToInt32(dictionary["TimeWaitLoadPossibleFriends"]);
			}
			if (dictionary.ContainsKey("PauseUpdateLeaderboard"))
			{
				Defs.pauseUpdateLeaderboard = (float)Convert.ToInt32(dictionary["PauseUpdateLeaderboard"]);
			}
			if (dictionary.ContainsKey("TimeUpdatePixelbookInfo"))
			{
				Defs.timeUpdatePixelbookInfo = (float)Convert.ToInt32(dictionary["TimeUpdatePixelbookInfo"]);
			}
			if (dictionary.ContainsKey("HistoryPrivateMessageLength"))
			{
				Defs.historyPrivateMessageLength = Convert.ToInt32(dictionary["HistoryPrivateMessageLength"]);
			}
			if (dictionary.ContainsKey("OutgoingInviteTimeoutMinutes"))
			{
				BattleInviteListener.Instance.OutgoingInviteTimeout = TimeSpan.FromMinutes((double)Convert.ToSingle(dictionary["OutgoingInviteTimeoutMinutes"]));
			}
			if (dictionary.ContainsKey("TimerIntervalDelaysFirstsEggs"))
			{
				List<object> list = dictionary["TimerIntervalDelaysFirstsEggs"] as List<object>;
				Nest.timerIntervalDelays.Clear();
				for (int i = 0; i < list.Count; i++)
				{
					Nest.timerIntervalDelays.Add(Convert.ToInt64(list[i]));
				}
			}
			if (dictionary.ContainsKey("TimeUpdateStartCheckIfNullResponce"))
			{
				Defs.timeUpdateStartCheckIfNullResponce = (float)Convert.ToInt32(dictionary["TimeUpdateStartCheckIfNullResponce"]);
			}
			if (dictionary.ContainsKey("TimeoutSendUpdatePlayerFromConnectScene"))
			{
				FriendsController.timeOutSendUpdatePlayerFromConnectScene = (float)Convert.ToInt32(dictionary["TimeoutSendUpdatePlayerFromConnectScene"]);
			}
			if (dictionary.ContainsKey("EnableLogForIDs") && FriendsController.sharedController != null && !string.IsNullOrEmpty(FriendsController.sharedController.id))
			{
				List<object> list2 = dictionary["EnableLogForIDs"] as List<object>;
				foreach (object obj in list2)
				{
					if (FriendsController.sharedController.id == obj.ToString())
					{
						LogsManager.EnableLogsFromServer();
						break;
					}
				}
			}
			if (dictionary.ContainsKey("FacebookLimits"))
			{
				try
				{
					object obj2 = dictionary["FacebookLimits"];
					Dictionary<string, object> dictionary2 = obj2 as Dictionary<string, object>;
					int arg = (int)((long)dictionary2["GreenLimit"]);
					int arg2 = (int)((long)dictionary2["RedLimit"]);
					Action<int, int> newFacebookLimitsAvailable = FriendsController.NewFacebookLimitsAvailable;
					if (newFacebookLimitsAvailable != null)
					{
						newFacebookLimitsAvailable(arg, arg2);
					}
				}
				catch (Exception exception)
				{
					Debug.LogException(exception);
				}
			}
			if (dictionary.ContainsKey("TwitterLimits"))
			{
				try
				{
					object obj3 = dictionary["TwitterLimits"];
					Dictionary<string, object> dictionary3 = obj3 as Dictionary<string, object>;
					int arg3 = (int)((long)dictionary3["GreenLimit"]);
					int arg4 = (int)((long)dictionary3["RedLimit"]);
					int arg5 = (int)((long)dictionary3["MultyWinLimit"]);
					int arg6 = (int)((long)dictionary3["ArenaLimit"]);
					Action<int, int, int, int> newTwitterLimitsAvailable = FriendsController.NewTwitterLimitsAvailable;
					if (newTwitterLimitsAvailable != null)
					{
						newTwitterLimitsAvailable(arg3, arg4, arg5, arg6);
					}
				}
				catch (Exception exception2)
				{
					Debug.LogException(exception2);
				}
			}
			if (dictionary.ContainsKey("CheaterDetectParameters"))
			{
				try
				{
					object obj4 = dictionary["CheaterDetectParameters"];
					Dictionary<string, object> dictionary4 = obj4 as Dictionary<string, object>;
					Dictionary<string, object> dictionary5 = dictionary4["Paying"] as Dictionary<string, object>;
					int arg7 = (int)((long)dictionary5["Coins"]);
					int arg8 = (int)((long)dictionary5["GemsCurrency"]);
					Dictionary<string, object> dictionary6 = dictionary4["NonPaying"] as Dictionary<string, object>;
					int arg9 = (int)((long)dictionary6["Coins"]);
					int arg10 = (int)((long)dictionary6["GemsCurrency"]);
					Action<int, int, int, int> newCheaterDetectParametersAvailable = FriendsController.NewCheaterDetectParametersAvailable;
					if (newCheaterDetectParametersAvailable != null)
					{
						newCheaterDetectParametersAvailable(arg7, arg8, arg9, arg10);
					}
				}
				catch (Exception exception3)
				{
					Debug.LogException(exception3);
				}
			}
			if (dictionary.ContainsKey("UseSqlLobby1031"))
			{
				Defs.useSqlLobby = (Convert.ToInt32(dictionary["UseSqlLobby1031"]) == 1);
			}
		}
	}

	// Token: 0x06000899 RID: 2201 RVA: 0x0003482C File Offset: 0x00032A2C
	private static void FillListDictionary(string key, List<Dictionary<string, string>> list)
	{
		string @string = PlayerPrefs.GetString(key, "[]");
		List<object> list2 = Json.Deserialize(@string) as List<object>;
		if (list2 != null && list2.Count > 0)
		{
			foreach (Dictionary<string, object> dictionary in list2.OfType<Dictionary<string, object>>())
			{
				Dictionary<string, string> dictionary2 = new Dictionary<string, string>();
				foreach (KeyValuePair<string, object> keyValuePair in dictionary)
				{
					string text = keyValuePair.Value as string;
					if (text != null)
					{
						dictionary2.Add(keyValuePair.Key, text);
					}
				}
				list.Add(dictionary2);
			}
		}
	}

	// Token: 0x0600089A RID: 2202 RVA: 0x00034934 File Offset: 0x00032B34
	private static List<string> FillList(string key)
	{
		List<string> list = new List<string>();
		string @string = PlayerPrefs.GetString(key, "[]");
		List<object> list2 = Json.Deserialize(@string) as List<object>;
		if (list2 != null && list2.Count > 0)
		{
			foreach (string item in list2.OfType<string>())
			{
				list.Add(item);
			}
		}
		return list;
	}

	// Token: 0x0600089B RID: 2203 RVA: 0x000349D0 File Offset: 0x00032BD0
	private static void FillDictionary(string key, Dictionary<string, Dictionary<string, object>> dictionary)
	{
		string text = string.Empty;
		using (new StopwatchLogger("Storager extracting " + key))
		{
			text = PlayerPrefs.GetString(key, "{}");
		}
		Debug.Log(key + " (length): " + text.Length);
		Dictionary<string, object> dictionary2 = null;
		using (new StopwatchLogger("Json decoding " + key))
		{
			dictionary2 = (Json.Deserialize(text) as Dictionary<string, object>);
		}
		if (dictionary2 != null && dictionary2.Count > 0)
		{
			Debug.Log(key + " (count): " + dictionary2.Count);
			using (new StopwatchLogger("Dictionary copying " + key))
			{
				foreach (KeyValuePair<string, object> keyValuePair in dictionary2)
				{
					Dictionary<string, object> dictionary3 = keyValuePair.Value as Dictionary<string, object>;
					if (dictionary3 != null)
					{
						dictionary.Add(keyValuePair.Key, dictionary3);
					}
				}
			}
		}
	}

	// Token: 0x0600089C RID: 2204 RVA: 0x00034B70 File Offset: 0x00032D70
	private void Awake()
	{
		if (!Storager.hasKey(this.FacebookIDKey))
		{
			Storager.setString(this.FacebookIDKey, string.Empty, false);
		}
		this.id_fb = Storager.getString(this.FacebookIDKey, false);
		FriendsController.sharedController = this;
	}

	// Token: 0x0600089D RID: 2205 RVA: 0x00034BAC File Offset: 0x00032DAC
	public IEnumerable<float> InitController()
	{
		string secret = this.alphaIvory ?? string.Empty;
		if (string.IsNullOrEmpty(secret))
		{
			Debug.LogError("Secret is empty!");
		}
		FriendsController._hmac = new HMACSHA1(Encoding.UTF8.GetBytes(secret), true);
		base.StartCoroutine("GetABTestAdvertConfig");
		base.StartCoroutine("GetCurrentcompetition");
		if (!TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage < TrainingController.NewTrainingCompletedStage.ShopCompleted && (!Storager.hasKey("abTestBalansConfig2Key") || string.IsNullOrEmpty(Storager.getString("abTestBalansConfig2Key", false))))
		{
			Storager.setString("abTestBalansConfig2Key", string.Empty, false);
			base.StartCoroutine(this.GetABTestBalansConfigName());
		}
		else
		{
			this.getCohortInfo = true;
			if (Defs.isABTestBalansCohortActual)
			{
				base.StartCoroutine(this.GetABTestBalansCohortNameActual());
			}
		}
		Task futureToWait = PersistentCacheManager.Instance.FirstResponse;
		base.StopCoroutine("GetBanList");
		base.StartCoroutine("GetBanList");
		base.StartCoroutine("UpdatePopularityMapsLoop");
		base.StartCoroutine(this.GetTimeFromServerLoop());
		base.StartCoroutine(this.SendGameTimeLoop());
		FriendsController.UpdatePixelbookSettingsFromPrefs();
		base.StartCoroutine(this.GetPixelbookSettingsLoop(futureToWait));
		base.StartCoroutine(this.GetNewsLoop(futureToWait));
		ProfileController.LoadStatisticFromKeychain();
		TrafficForwardingScript trafficForwardingScript = base.GetComponent<TrafficForwardingScript>() ?? base.gameObject.AddComponent<TrafficForwardingScript>();
		base.StartCoroutine(trafficForwardingScript.GetTrafficForwardingConfigLoopCoroutine());
		base.StartCoroutine(this.GetFiltersSettings());
		base.StartCoroutine(this.GetBuffSettings(futureToWait));
		base.StartCoroutine(this.GetRatingSystemConfig());
		if (FacebookController.FacebookSupported)
		{
			FacebookController.ReceivedSelfID += this.HandleReceivedSelfID;
		}
		this.lastTouchTm = Time.realtimeSinceStartup + 15f;
		this.friends = FriendsController.FillList("FriendsKey");
		FriendsController.StartSendReview();
		yield return 0f;
		this.invitesToUs = FriendsController.FillList("ToUsKey");
		yield return 0f;
		FriendsController.FillDictionary("PlayerInfoKey", this.playersInfo);
		FriendsController.FillDictionary("FriendsInfoKey", this.friendsInfo);
		FriendsController.FillDictionary("ClanFriendsInfoKey", this.clanFriendsInfo);
		yield return 0f;
		FriendsController.FillListDictionary("ClanInvitesKey", this.ClanInvites);
		yield return 0f;
		this.FillClickJoinFriendsListByCachedValue();
		yield return 0f;
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		if (!Storager.hasKey(this.AccountCreated))
		{
			Storager.setString(this.AccountCreated, string.Empty, false);
		}
		this.id = Storager.getString(this.AccountCreated, false);
		if (string.IsNullOrEmpty(this.id))
		{
			Debug.Log("Account id: null or empty    Calling CreatePlayer()...");
			base.StartCoroutine(this.CreatePlayer());
		}
		else
		{
			Debug.LogFormat("Account id: {0}    Calling CheckOurIdExists()...", new object[]
			{
				this.id
			});
			base.StartCoroutine(this.CheckOurIDExists());
		}
		this.SyncClickJoinFriendsListWithListFriends();
		yield break;
	}

	// Token: 0x0600089E RID: 2206 RVA: 0x00034BD0 File Offset: 0x00032DD0
	private void HandleReceivedSelfID(string idfb)
	{
		if (idfb == null)
		{
			return;
		}
		if (string.IsNullOrEmpty(this.id_fb) || !idfb.Equals(this.id_fb))
		{
			this.id_fb = idfb;
			if (!Storager.hasKey(this.FacebookIDKey))
			{
				Storager.setString(this.FacebookIDKey, string.Empty, false);
			}
			Storager.setString(this.FacebookIDKey, this.id_fb, false);
			this.SendOurData(false);
		}
	}

	// Token: 0x0600089F RID: 2207 RVA: 0x00034C48 File Offset: 0x00032E48
	public void UnbanUs(Action onSuccess)
	{
	}

	// Token: 0x060008A0 RID: 2208 RVA: 0x00034C4C File Offset: 0x00032E4C
	public void ChangeClanLogo()
	{
		if (!FriendsController.readyToOperate)
		{
			return;
		}
		base.StartCoroutine(this._ChangeClanLogo());
	}

	// Token: 0x060008A1 RID: 2209 RVA: 0x00034C68 File Offset: 0x00032E68
	public void GetOurWins()
	{
		if (!FriendsController.readyToOperate)
		{
			return;
		}
		base.StartCoroutine(this._GetOurWins());
	}

	// Token: 0x060008A2 RID: 2210 RVA: 0x00034C84 File Offset: 0x00032E84
	public void SendRoundWon()
	{
		if (!FriendsController.readyToOperate)
		{
			return;
		}
		int num = -1;
		if (PhotonNetwork.room != null)
		{
			num = (int)ConnectSceneNGUIController.regim;
		}
		if (num != -1)
		{
			base.StartCoroutine(this._SendRoundWon(num));
		}
	}

	// Token: 0x060008A3 RID: 2211 RVA: 0x00034CC4 File Offset: 0x00032EC4
	public static string Hash(string action, string token = null)
	{
		if (action == null)
		{
			Debug.LogWarning("Hash: action is null");
			return string.Empty;
		}
		string text = token ?? ((!(FriendsController.sharedController != null)) ? null : FriendsController.sharedController.id);
		if (text == null)
		{
			Debug.LogWarning("Hash: Token is null");
			return string.Empty;
		}
		string str = (!action.Equals("get_player_online")) ? (ProtocolListGetter.CurrentPlatform + ":" + GlobalGameController.AppVersion) : "*:*.*.*";
		string s = str + text + action;
		byte[] bytes = Encoding.UTF8.GetBytes(s);
		byte[] value = FriendsController._hmac.ComputeHash(bytes);
		string text2 = BitConverter.ToString(value);
		return text2.Replace("-", string.Empty).ToLower();
	}

	// Token: 0x060008A4 RID: 2212 RVA: 0x00034DA0 File Offset: 0x00032FA0
	public static string HashForPush(byte[] responceData)
	{
		if (responceData == null)
		{
			Debug.LogWarning("HashForPush: responceData is null");
			return string.Empty;
		}
		if (FriendsController._hmac == null)
		{
			throw new InvalidOperationException("Hmac is not initialized yet.");
		}
		byte[] value = FriendsController._hmac.ComputeHash(responceData);
		string text = BitConverter.ToString(value);
		return text.Replace("-", string.Empty).ToLower();
	}

	// Token: 0x060008A5 RID: 2213 RVA: 0x00034E00 File Offset: 0x00033000
	public bool IsShowAdd(string _pixelBookID)
	{
		return this.friends.Count < Defs.maxCountFriend && !_pixelBookID.Equals("-1") && !_pixelBookID.Equals(FriendsController.sharedController.id) && !FriendsController.sharedController.friends.Contains(_pixelBookID) && !FriendsController.sharedController.notShowAddIds.Contains(_pixelBookID);
	}

	// Token: 0x060008A6 RID: 2214 RVA: 0x00034E84 File Offset: 0x00033084
	private IEnumerator _GetOurWins()
	{
		while (string.IsNullOrEmpty(FriendsController.sharedController.id) || !TrainingController.TrainingCompleted)
		{
			yield return null;
		}
		string appVersionString = ProtocolListGetter.CurrentPlatform + ":" + GlobalGameController.AppVersion;
		WWWForm form = new WWWForm();
		form.AddField("action", "get_info_by_id");
		form.AddField("app_version", appVersionString);
		form.AddField("id", this.id);
		form.AddField("uniq_id", FriendsController.sharedController.id);
		form.AddField("auth", FriendsController.Hash("get_info_by_id", null));
		string response;
		for (;;)
		{
			WWW download = Tools.CreateWwwIfNotConnected(FriendsController.actionAddress, form, string.Empty, null);
			if (download == null)
			{
				yield return base.StartCoroutine(this.MyWaitForSeconds(10f));
			}
			else
			{
				yield return download;
				response = URLs.Sanitize(download);
				if (!string.IsNullOrEmpty(download.error) || !string.IsNullOrEmpty(response))
				{
				}
				if (!string.IsNullOrEmpty(download.error))
				{
					if (Debug.isDebugBuild)
					{
						Debug.LogWarning("_GetOurWins error: " + download.error);
					}
					yield return base.StartCoroutine(this.MyWaitForSeconds(10f));
				}
				else
				{
					if (string.IsNullOrEmpty(response) || !response.Equals("fail"))
					{
						break;
					}
					if (Debug.isDebugBuild || Application.isEditor)
					{
						Debug.LogWarning("_GetOurWins fail.");
					}
					yield return base.StartCoroutine(this.MyWaitForSeconds(10f));
				}
			}
		}
		Dictionary<string, object> __newInfo = this.ParseInfo(response);
		if (__newInfo == null)
		{
			if (Debug.isDebugBuild || Application.isEditor)
			{
				Debug.LogWarning(" _GetOurWins newInfo = null");
			}
			yield break;
		}
		this.ourInfo = __newInfo;
		this.SaveProfileData();
		if (FriendsController.OurInfoUpdated != null)
		{
			FriendsController.OurInfoUpdated();
		}
		yield break;
	}

	// Token: 0x060008A7 RID: 2215 RVA: 0x00034EA0 File Offset: 0x000330A0
	private void SaveProfileData()
	{
		if (this.ourInfo != null && this.ourInfo.ContainsKey("wincount"))
		{
			int val = 0;
			Dictionary<string, object> dict = this.ourInfo["wincount"] as Dictionary<string, object>;
			val = 0;
			dict.TryGetValue("0", out val);
			Storager.setInt(Defs.RatingDeathmatch, val, false);
			val = 0;
			dict.TryGetValue("2", out val);
			Storager.setInt(Defs.RatingTeamBattle, val, false);
			val = 0;
			dict.TryGetValue("3", out val);
			Storager.setInt(Defs.RatingHunger, val, false);
			val = 0;
			dict.TryGetValue("4", out val);
			Storager.setInt(Defs.RatingCapturePoint, val, false);
		}
	}

	// Token: 0x060008A8 RID: 2216 RVA: 0x00034F58 File Offset: 0x00033158
	private IEnumerator _SendRoundWon(int mode)
	{
		string appVersionField = ProtocolListGetter.CurrentPlatform + ":" + GlobalGameController.AppVersion;
		for (;;)
		{
			WWWForm form = new WWWForm();
			form.AddField("action", "round_won");
			form.AddField("app_version", appVersionField);
			form.AddField("uniq_id", this.id);
			form.AddField("mode", mode);
			form.AddField("auth", FriendsController.Hash("round_won", null));
			WWW roundWonRequest = Tools.CreateWww(FriendsController.actionAddress, form, string.Empty);
			yield return roundWonRequest;
			string response = URLs.Sanitize(roundWonRequest);
			if (string.IsNullOrEmpty(roundWonRequest.error) && !string.IsNullOrEmpty(response) && Debug.isDebugBuild)
			{
				Debug.Log("_SendRoundWon: " + response);
			}
			if (!string.IsNullOrEmpty(roundWonRequest.error))
			{
				if (Debug.isDebugBuild)
				{
					Debug.LogWarning("_SendRoundWon error: " + roundWonRequest.error);
				}
				yield return base.StartCoroutine(this.MyWaitForSeconds(10f));
			}
			else
			{
				if (string.IsNullOrEmpty(response) || !response.Equals("fail"))
				{
					break;
				}
				if (Debug.isDebugBuild)
				{
					Debug.LogWarning("_SendRoundWon fail.");
				}
				yield return base.StartCoroutine(this.MyWaitForSeconds(10f));
			}
		}
		PlayerPrefs.SetInt("TotalWinsForLeaderboards", PlayerPrefs.GetInt("TotalWinsForLeaderboards", 0) + 1);
		yield break;
	}

	// Token: 0x060008A9 RID: 2217 RVA: 0x00034F84 File Offset: 0x00033184
	private IEnumerator _ChangeClanLogo()
	{
		string appVersionField = ProtocolListGetter.CurrentPlatform + ":" + GlobalGameController.AppVersion;
		for (;;)
		{
			WWWForm form = new WWWForm();
			form.AddField("action", "change_logo");
			form.AddField("app_version", appVersionField);
			form.AddField("id_clan", this.ClanID);
			form.AddField("logo", this.clanLogo);
			form.AddField("id", this.id);
			form.AddField("uniq_id", this.id);
			form.AddField("auth", FriendsController.Hash("change_logo", null));
			WWW download = Tools.CreateWwwIfNotConnected(FriendsController.actionAddress, form, string.Empty, null);
			if (download == null)
			{
				yield return base.StartCoroutine(this.MyWaitForSeconds(10f));
			}
			else
			{
				yield return download;
				string response = URLs.Sanitize(download);
				if (string.IsNullOrEmpty(download.error) && !string.IsNullOrEmpty(response) && Debug.isDebugBuild)
				{
					Debug.Log("_ChangeClanLogo: " + response);
				}
				if (!string.IsNullOrEmpty(download.error))
				{
					if (Debug.isDebugBuild)
					{
						Debug.LogWarning("_ChangeClanLogo error: " + download.error);
					}
					yield return base.StartCoroutine(this.MyWaitForSeconds(10f));
				}
				else
				{
					if (string.IsNullOrEmpty(response) || !response.Equals("fail"))
					{
						break;
					}
					if (Debug.isDebugBuild)
					{
						Debug.LogWarning("_ChangeClanLogo fail.");
					}
					yield return base.StartCoroutine(this.MyWaitForSeconds(10f));
				}
			}
		}
		yield break;
	}

	// Token: 0x060008AA RID: 2218 RVA: 0x00034FA0 File Offset: 0x000331A0
	public void ChangeClanName(string newNm, Action onSuccess, Action<string> onFailure)
	{
		if (!FriendsController.readyToOperate)
		{
			return;
		}
		base.StartCoroutine(this._ChangeClanName(newNm, onSuccess, onFailure));
	}

	// Token: 0x060008AB RID: 2219 RVA: 0x00034FC0 File Offset: 0x000331C0
	private IEnumerator _ChangeClanName(string newNm, Action onSuccess, Action<string> onFailure)
	{
		WWWForm form = new WWWForm();
		form.AddField("action", "change_clan_name");
		form.AddField("app_version", ProtocolListGetter.CurrentPlatform + ":" + GlobalGameController.AppVersion);
		form.AddField("id_clan", this.ClanID);
		form.AddField("id", this.id);
		string filteredNick = newNm;
		filteredNick = FilterBadWorld.FilterString(newNm);
		form.AddField("name", filteredNick);
		form.AddField("uniq_id", FriendsController.sharedController.id);
		form.AddField("auth", FriendsController.Hash("change_clan_name", null));
		WWW download = Tools.CreateWwwIfNotConnected(FriendsController.actionAddress, form, string.Empty, null);
		if (download == null)
		{
			if (onFailure != null)
			{
				onFailure("Request skipped.");
			}
			yield break;
		}
		yield return download;
		string response = URLs.Sanitize(download);
		if (string.IsNullOrEmpty(download.error) && !string.IsNullOrEmpty(response) && Debug.isDebugBuild)
		{
			Debug.Log("_ChangeClanName: " + response);
		}
		if (!string.IsNullOrEmpty(download.error))
		{
			if (Debug.isDebugBuild)
			{
				Debug.LogWarning("_ChangeClanName error: " + download.error);
			}
			if (onFailure != null)
			{
				onFailure(download.error);
			}
			yield break;
		}
		if (!string.IsNullOrEmpty(response) && response.Equals("fail"))
		{
			if (Debug.isDebugBuild)
			{
				Debug.LogWarning("_ChangeClanName fail.");
			}
			if (onFailure != null)
			{
				onFailure(response);
			}
			yield break;
		}
		if (onSuccess != null)
		{
			onSuccess();
		}
		yield break;
	}

	// Token: 0x060008AC RID: 2220 RVA: 0x00035008 File Offset: 0x00033208
	private IEnumerator UpdatePopularityMapsLoop()
	{
		while (!TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage < TrainingController.NewTrainingCompletedStage.ShootingRangeCompleted)
		{
			yield return null;
		}
		for (;;)
		{
			this.UpdatePopularityMaps();
			yield return base.StartCoroutine(this.MyWaitForSeconds(1800f));
		}
		yield break;
	}

	// Token: 0x060008AD RID: 2221 RVA: 0x00035024 File Offset: 0x00033224
	public void UpdatePopularityMaps()
	{
		base.StopCoroutine("GetPopularityMap");
		base.StartCoroutine("GetPopularityMap");
	}

	// Token: 0x060008AE RID: 2222 RVA: 0x00035040 File Offset: 0x00033240
	private IEnumerator GetPopularityMap()
	{
		while (!TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage <= TrainingController.NewTrainingCompletedStage.None)
		{
			yield return null;
		}
		Dictionary<string, object> dict;
		for (;;)
		{
			WWW download = Tools.CreateWwwIfNotConnected(URLs.PopularityMapUrl);
			if (download == null)
			{
				break;
			}
			yield return download;
			string response = URLs.Sanitize(download);
			if (!string.IsNullOrEmpty(download.error))
			{
				if (Debug.isDebugBuild || Application.isEditor)
				{
					Debug.LogWarning("CheckMapPopularity error: " + download.error);
				}
				yield return base.StartCoroutine(this.MyWaitForSeconds(18f));
			}
			else if (!string.IsNullOrEmpty(response) && response.Equals("fail"))
			{
				if (Debug.isDebugBuild || Application.isEditor)
				{
					Debug.LogWarning("CheckMapPopularity fail.");
				}
				yield return base.StartCoroutine(this.MyWaitForSeconds(18f));
			}
			else
			{
				object o = Json.Deserialize(response);
				dict = (o as Dictionary<string, object>);
				if (dict != null)
				{
					goto IL_20E;
				}
				if (Application.isEditor || Debug.isDebugBuild)
				{
					Debug.LogWarning(" GetPopularityMap dict = null");
				}
				yield return base.StartCoroutine(this.MyWaitForSeconds(20f));
			}
		}
		yield break;
		IL_20E:
		foreach (KeyValuePair<string, object> kvp in dict)
		{
			Dictionary<string, string> _mapPopularityInRegim = new Dictionary<string, string>();
			Dictionary<string, object> dict2 = kvp.Value as Dictionary<string, object>;
			if (dict2 != null)
			{
				foreach (KeyValuePair<string, object> kvp2 in dict2)
				{
					_mapPopularityInRegim.Add(kvp2.Key, kvp2.Value.ToString());
				}
				if (_mapPopularityInRegim.Count > 0 && !FriendsController.mapPopularityDictionary.ContainsKey(kvp.Key))
				{
					FriendsController.mapPopularityDictionary.Add(kvp.Key, _mapPopularityInRegim);
				}
			}
		}
		yield break;
	}

	// Token: 0x060008AF RID: 2223 RVA: 0x0003505C File Offset: 0x0003325C
	private IEnumerator GetABTestBalansConfigName()
	{
		string response;
		for (;;)
		{
			WWWForm form = new WWWForm();
			form.AddField("action", "get_cohort_name");
			form.AddField("app_version", ProtocolListGetter.CurrentPlatform + ":" + GlobalGameController.AppVersion);
			form.AddField("device", SystemInfo.deviceUniqueIdentifier);
			WWW download = Tools.CreateWwwIfNotConnected(FriendsController.actionAddress, form, string.Empty, null);
			if (download == null)
			{
				yield return base.StartCoroutine(this.MyWaitForSeconds(1f));
			}
			else
			{
				yield return download;
				response = URLs.Sanitize(download);
				if (!string.IsNullOrEmpty(download.error))
				{
					if (Debug.isDebugBuild)
					{
						Debug.LogWarning("get_cohort_name error: " + download.error);
					}
					yield return base.StartCoroutine(this.MyWaitForSeconds(1f));
				}
				else
				{
					if (!"fail".Equals(response))
					{
						break;
					}
					if (Debug.isDebugBuild)
					{
						Debug.LogWarning("get_cohort_name fail.");
					}
					yield return base.StartCoroutine(this.MyWaitForSeconds(1f));
				}
			}
		}
		if ("skip".Equals(response))
		{
			if (Debug.isDebugBuild)
			{
				Debug.LogWarning("get_cohort_name skip");
			}
			this.getCohortInfo = true;
			yield break;
		}
		response = response.Replace("/", "-");
		base.StartCoroutine(this.GetABTestBalansConfig(response, true));
		yield break;
	}

	// Token: 0x060008B0 RID: 2224 RVA: 0x00035078 File Offset: 0x00033278
	private IEnumerator GetABTestBalansConfig(string nameConfig, bool isFirst)
	{
		WWW download;
		for (;;)
		{
			WWWForm form = new WWWForm();
			download = Tools.CreateWwwIfNotConnected((!nameConfig.Equals("none")) ? (URLs.ABTestBalansFolderURL + nameConfig + ".json") : URLs.ABTestBalansURL);
			if (download == null)
			{
				yield return base.StartCoroutine(this.MyWaitForSeconds(1f));
			}
			else
			{
				yield return download;
				if (nameConfig.Equals("none") && (TrainingController.TrainingCompleted || TrainingController.CompletedTrainingStage >= TrainingController.NewTrainingCompletedStage.ShopCompleted))
				{
					break;
				}
				if (string.IsNullOrEmpty(download.error))
				{
					goto IL_178;
				}
				if (Debug.isDebugBuild || Application.isEditor)
				{
					Debug.LogWarning("GetABTestBalansConfig error: " + download.error);
				}
				yield return base.StartCoroutine(this.MyWaitForSeconds(1f));
			}
		}
		this.getCohortInfo = true;
		goto IL_247;
		IL_178:
		string responseText = URLs.Sanitize(download);
		if (!string.IsNullOrEmpty(responseText) && responseText != Storager.getString("abTestBalansConfig2Key", false))
		{
			Storager.setString("abTestBalansConfig2Key", responseText, false);
			this.getCohortInfo = true;
			if (!nameConfig.Equals("none"))
			{
				Defs.isABTestBalansNoneSkip = true;
			}
			BalanceController.sharedController.ParseConfig(isFirst);
			if (isFirst && Defs.abTestBalansCohort == Defs.ABTestCohortsType.B)
			{
				base.StartCoroutine(this.GetABTestBalansCohortNameActual());
			}
			if (Debug.isDebugBuild)
			{
				Debug.Log("GetConfigABtestBalans");
			}
		}
		IL_247:
		yield break;
	}

	// Token: 0x060008B1 RID: 2225 RVA: 0x000350B0 File Offset: 0x000332B0
	private IEnumerator GetABTestBalansCohortNameActual()
	{
		for (;;)
		{
			WWWForm form = new WWWForm();
			WWW download = Tools.CreateWwwIfNotConnected(URLs.ABTestBalansActualCohortNameURL);
			if (download == null)
			{
				yield return base.StartCoroutine(this.MyWaitForSeconds(60f));
			}
			else
			{
				yield return download;
				if (!string.IsNullOrEmpty(download.error))
				{
					if (Application.isEditor)
					{
						Debug.LogWarning("GetABTestBalansCohortNameActual error: " + download.error);
					}
					yield return base.StartCoroutine(this.MyWaitForSeconds(60f));
				}
				else
				{
					string responseText = URLs.Sanitize(download);
					if (!string.IsNullOrEmpty(responseText))
					{
						Dictionary<string, object> _nameCohortDict = Json.Deserialize(responseText) as Dictionary<string, object>;
						if (_nameCohortDict == null)
						{
							if (Application.isEditor)
							{
								Debug.LogWarning("GetABTestBalansCohortNameActual parse error");
							}
							yield return base.StartCoroutine(this.MyWaitForSeconds(60f));
						}
						else
						{
							if (Debug.isDebugBuild)
							{
								Debug.Log("GetConfigABtestBalans");
							}
							if (!Convert.ToString(_nameCohortDict["ActualCohortNameB"]).Equals(Defs.abTestBalansCohortName))
							{
								break;
							}
							base.StartCoroutine(this.GetABTestBalansConfig(Defs.abTestBalansCohortName, false));
							yield return base.StartCoroutine(this.MyWaitForSeconds(900f));
						}
					}
					else
					{
						yield return base.StartCoroutine(this.MyWaitForSeconds(60f));
					}
				}
			}
		}
		Defs.isABTestBalansCohortActual = false;
		FriendsController.ResetABTestsBalans();
		yield break;
	}

	// Token: 0x060008B2 RID: 2226 RVA: 0x000350CC File Offset: 0x000332CC
	public static void ResetABTestsBalans()
	{
		Storager.setString("abTestBalansConfig2Key", string.Empty, false);
		if (BalanceController.sharedController != null)
		{
			BalanceController.sharedController.ParseConfig(false);
		}
		Defs.abTestBalansCohort = Defs.ABTestCohortsType.NONE;
		Defs.abTestBalansCohortName = string.Empty;
		Defs.isABTestBalansCohortActual = false;
	}

	// Token: 0x060008B3 RID: 2227 RVA: 0x0003511C File Offset: 0x0003331C
	private IEnumerator GetBanList()
	{
		string appVersionField = ProtocolListGetter.CurrentPlatform + ":" + GlobalGameController.AppVersion;
		int ban;
		for (;;)
		{
			while (string.IsNullOrEmpty(this.id))
			{
				yield return null;
			}
			WWWForm form = new WWWForm();
			form.AddField("app_version", appVersionField);
			form.AddField("id", this.id);
			WWW download = Tools.CreateWwwIfNotConnected(URLs.BanURL, form, string.Empty, null);
			if (download == null)
			{
				yield return base.StartCoroutine(this.MyWaitForSeconds(10f));
			}
			else
			{
				yield return download;
				if (!string.IsNullOrEmpty(download.error))
				{
					if (Debug.isDebugBuild || Application.isEditor)
					{
						Debug.LogWarning("GetBanList error: " + download.error);
					}
					yield return base.StartCoroutine(this.MyWaitForSeconds(10f));
				}
				else
				{
					string responseText = URLs.Sanitize(download);
					if (int.TryParse(responseText, out ban))
					{
						break;
					}
					Debug.LogWarning("GetBanList cannot parse ban!");
					yield return base.StartCoroutine(this.MyWaitForSeconds(10f));
				}
			}
		}
		this.Banned = ban;
		if (Debug.isDebugBuild)
		{
			Debug.Log("GetBanList Banned: " + this.Banned);
		}
		yield break;
	}

	// Token: 0x060008B4 RID: 2228 RVA: 0x00035138 File Offset: 0x00033338
	private IEnumerator CheckOurIDExists()
	{
		string appVersionField = ProtocolListGetter.CurrentPlatform + ":" + GlobalGameController.AppVersion;
		string response;
		for (;;)
		{
			WWWForm form = new WWWForm();
			form.AddField("action", "start_check");
			form.AddField("app_version", appVersionField);
			form.AddField("platform", ProtocolListGetter.CurrentPlatform.ToString());
			form.AddField("uniq_id", FriendsController.sharedController.id);
			form.AddField("device_model", SystemInfo.deviceModel);
			form.AddField("type_device", (!Device.isPixelGunLow) ? 2 : 1);
			form.AddField("auth", FriendsController.Hash("start_check", null));
			form.AddField("abuse_method", Storager.getInt("AbuseMethod", false));
			if (Launcher.PackageInfo != null)
			{
			}
			WWW download = Tools.CreateWwwIfNotConnected(FriendsController.actionAddress, form, string.Empty, null);
			if (download == null)
			{
				yield return base.StartCoroutine(this.MyWaitForSeconds(Defs.timeUpdateStartCheckIfNullResponce));
			}
			else
			{
				yield return download;
				response = URLs.Sanitize(download);
				if (!string.IsNullOrEmpty(download.error))
				{
					if (Debug.isDebugBuild)
					{
						Debug.LogWarning("CheckOurIDExists error: " + download.error);
					}
					yield return base.StartCoroutine(this.MyWaitForSeconds(Defs.timeUpdateStartCheckIfNullResponce));
				}
				else
				{
					if (!"fail".Equals(response))
					{
						break;
					}
					if (Debug.isDebugBuild)
					{
						Debug.LogWarning("CheckOurIDExists fail.");
					}
					yield return base.StartCoroutine(this.MyWaitForSeconds(Defs.timeUpdateStartCheckIfNullResponce));
				}
			}
		}
		int newId;
		if (!int.TryParse(response, out newId))
		{
			Dictionary<string, object> clanInfo = Json.Deserialize(response) as Dictionary<string, object>;
			if (clanInfo == null)
			{
				Debug.LogWarning("CheckOurIDExists cannot parse clan info!");
			}
			else
			{
				object clanIDObj;
				if (clanInfo.TryGetValue("id", out clanIDObj) && clanIDObj != null && !clanIDObj.Equals("null"))
				{
					this.ClanID = Convert.ToString(clanIDObj);
				}
				object clanCreatorObj;
				if (clanInfo.TryGetValue("creator_id", out clanCreatorObj) && clanCreatorObj != null && !clanCreatorObj.Equals("null"))
				{
					this.clanLeaderID = (clanCreatorObj as string);
				}
				object clanNameObj;
				if (clanInfo.TryGetValue("name", out clanNameObj) && clanNameObj != null && !clanNameObj.Equals("null"))
				{
					this._prevClanName = this.clanName;
					this.clanName = (clanNameObj as string);
					if (!this._prevClanName.Equals(this.clanName) && this.onChangeClanName != null)
					{
						this.onChangeClanName(this.clanName);
					}
				}
				object clanLogoObj;
				if (clanInfo.TryGetValue("logo", out clanLogoObj) && clanLogoObj != null && !clanLogoObj.Equals("null"))
				{
					this.clanLogo = (clanLogoObj as string);
				}
			}
		}
		else
		{
			Storager.setString(this.AccountCreated, response, false);
			this.id = response;
			this.onlineInfo.Clear();
			this.friends.Clear();
			this.invitesFromUs.Clear();
			this.playersInfo.Clear();
			this.invitesToUs.Clear();
			this.ClanInvites.Clear();
			this.notShowAddIds.Clear();
			this.SaveCurrentState();
			PlayerPrefs.Save();
		}
		FriendsController.readyToOperate = true;
		base.StartCoroutine(this.GetFriendsDataLoop());
		base.StartCoroutine(this.GetClanDataLoop());
		this.GetOurLAstOnline();
		base.StartCoroutine(this.RequestWinCountTimestampCoroutine());
		this.GetOurWins();
		yield break;
	}

	// Token: 0x060008B5 RID: 2229 RVA: 0x00035154 File Offset: 0x00033354
	public void InitOurInfo()
	{
		this.nick = ProfileController.GetPlayerNameOrDefault();
		byte[] inArray = SkinsController.currentSkinForPers.EncodeToPNG();
		this.skin = Convert.ToBase64String(inArray);
		this.rank = ExperienceController.sharedController.currentLevel;
		this.wins = Storager.getInt("Rating", false);
		this.survivalScore = PlayerPrefs.GetInt(Defs.SurvivalScoreSett, 0);
		this.coopScore = PlayerPrefs.GetInt(Defs.COOPScore, 0);
		this.infoLoaded = true;
	}

	// Token: 0x060008B6 RID: 2230 RVA: 0x000351D4 File Offset: 0x000333D4
	public IEnumerator WaitForReadyToOperateAndUpdatePlayer()
	{
		while (!FriendsController.readyToOperate)
		{
			yield return null;
		}
		base.StartCoroutine(this.UpdatePlayer(true));
		yield break;
	}

	// Token: 0x060008B7 RID: 2231 RVA: 0x000351F0 File Offset: 0x000333F0
	public void SendOurData(bool SendSkin = false)
	{
		if (!FriendsController.readyToOperate)
		{
			return;
		}
		base.StartCoroutine(this.UpdatePlayer(SendSkin));
	}

	// Token: 0x060008B8 RID: 2232 RVA: 0x0003520C File Offset: 0x0003340C
	public void SendOurDataInConnectScene()
	{
		if (Time.realtimeSinceStartup - this.timeSendUpdatePlayer < FriendsController.timeOutSendUpdatePlayerFromConnectScene)
		{
			return;
		}
		if (!FriendsController.readyToOperate)
		{
			return;
		}
		base.StartCoroutine(this.UpdatePlayer(false));
	}

	// Token: 0x060008B9 RID: 2233 RVA: 0x0003524C File Offset: 0x0003344C
	private void SaveCurrentState()
	{
		if (this.friends != null)
		{
			string text = Json.Serialize(this.friends);
			PlayerPrefs.SetString("FriendsKey", text ?? "[]");
		}
		if (this.invitesToUs != null)
		{
			string text2 = Json.Serialize(this.invitesToUs);
			PlayerPrefs.SetString("ToUsKey", text2 ?? "[]");
		}
		if (this.playersInfo != null)
		{
			string text3 = Json.Serialize(this.playersInfo);
			PlayerPrefs.SetString("PlayerInfoKey", text3 ?? "{}");
		}
		if (this.friendsInfo != null)
		{
			string text4 = Json.Serialize(this.friendsInfo);
			PlayerPrefs.SetString("FriendsInfoKey", text4 ?? "{}");
		}
		if (this.clanFriendsInfo != null)
		{
			string text5 = Json.Serialize(this.clanFriendsInfo);
			PlayerPrefs.SetString("ClanFriendsInfoKey", text5 ?? "{}");
		}
		if (this.ClanInvites != null)
		{
			string text6 = Json.Serialize(this.ClanInvites);
			PlayerPrefs.SetString("ClanInvitesKey", text6 ?? "[]");
		}
		this.UpdateCachedClickJoinListValue();
	}

	// Token: 0x060008BA RID: 2234 RVA: 0x00035378 File Offset: 0x00033578
	private void DumpCurrentState()
	{
	}

	// Token: 0x060008BB RID: 2235 RVA: 0x0003537C File Offset: 0x0003357C
	private IEnumerator OnApplicationPause(bool pause)
	{
		if (pause)
		{
			this.DumpCurrentState();
			AnalyticsStuff.SaveTrainingStep();
		}
		else
		{
			FriendsController.isUpdateServerTimeAfterRun = false;
			this.firstUpdateAfterApplicationPause = true;
			yield return null;
			yield return null;
			yield return null;
			FriendsController.StartSendReview();
			if (GiftBannerWindow.instance != null)
			{
				GiftBannerWindow.instance.ForceCloseAll();
			}
			base.StopCoroutine("GetBanList");
			base.StartCoroutine("GetBanList");
			if (Defs.isABTestBalansCohortActual)
			{
				base.StopCoroutine(this.GetABTestBalansCohortNameActual());
				base.StartCoroutine(this.GetABTestBalansCohortNameActual());
			}
			base.StopCoroutine("GetABTestAdvertConfig");
			base.StartCoroutine("GetABTestAdvertConfig");
			base.StartCoroutine("GetCurrentcompetition");
			base.StartCoroutine("GetCurrentcompetition");
			this.UpdatePopularityMaps();
			base.StartCoroutine(this.GetTimeFromServerLoop());
			base.StartCoroutine(this.GetFiltersSettings());
			base.StartCoroutine(this.GetLobbyNews(true));
			Task futureToWait = PersistentCacheManager.Instance.FirstResponse;
			base.StartCoroutine(this.GetBuffSettings(futureToWait));
			base.StartCoroutine(this.GetRatingSystemConfig());
			this.GetFriendsData(true);
			this.FastGetPixelbookSettings();
			if (SceneLoader.ActiveSceneName.Equals("Friends") && FriendsGUIController.ShowProfile && FriendProfileController.currentFriendId != null && FriendsController.readyToOperate)
			{
				base.StartCoroutine(this.UpdatePlayerInfoById(FriendProfileController.currentFriendId));
			}
			if (SceneLoader.ActiveSceneName.Equals("Clans"))
			{
				if (!string.IsNullOrEmpty(this.ClanID))
				{
					base.StartCoroutine(this.GetClanDataOnce());
				}
				if (ClansGUIController.AtAddPanel)
				{
					base.StartCoroutine(this.GetAllPlayersOnline());
				}
				else
				{
					base.StartCoroutine(this.GetClanPlayersOnline());
				}
				if (ClansGUIController.ShowProfile && FriendProfileController.currentFriendId != null && FriendsController.readyToOperate)
				{
					base.StartCoroutine(this.UpdatePlayerInfoById(FriendProfileController.currentFriendId));
				}
			}
		}
		yield break;
	}

	// Token: 0x060008BC RID: 2236 RVA: 0x000353A8 File Offset: 0x000335A8
	private void OnDestroy()
	{
		this.DumpCurrentState();
	}

	// Token: 0x060008BD RID: 2237 RVA: 0x000353B0 File Offset: 0x000335B0
	public void SendInvitation(string personId, Dictionary<string, object> socialEventParameters)
	{
		if (!string.IsNullOrEmpty(personId) && FriendsController.readyToOperate)
		{
			if (socialEventParameters == null)
			{
				throw new ArgumentNullException("socialEventParameters");
			}
			base.StartCoroutine(this.FriendRequest(personId, socialEventParameters, null));
		}
	}

	// Token: 0x060008BE RID: 2238 RVA: 0x000353F4 File Offset: 0x000335F4
	public void SendCreateClan(string personId, string nameClan, string skinClan, Action<string> ErrorHandler)
	{
		if (!string.IsNullOrEmpty(personId) && !string.IsNullOrEmpty(nameClan) && !string.IsNullOrEmpty(skinClan) && FriendsController.readyToOperate)
		{
			base.StartCoroutine(this._SendCreateClan(personId, nameClan, skinClan, ErrorHandler));
		}
		else if (ErrorHandler != null)
		{
			ErrorHandler("Error: FALSE:  ! string.IsNullOrEmpty (personId) && ! string.IsNullOrEmpty (nameClan) && ! string.IsNullOrEmpty (skinClan) && readyToOperate");
		}
	}

	// Token: 0x060008BF RID: 2239 RVA: 0x00035458 File Offset: 0x00033658
	public static void SendPlayerInviteToClan(string personId, Action<bool, bool> callbackResult = null)
	{
		if (FriendsController.sharedController == null)
		{
			return;
		}
		if (!string.IsNullOrEmpty(personId) && FriendsController.readyToOperate)
		{
			FriendsController.sharedController.StartCoroutine(FriendsController.sharedController.SendClanInvitation(personId, callbackResult));
		}
	}

	// Token: 0x060008C0 RID: 2240 RVA: 0x00035498 File Offset: 0x00033698
	public void AcceptInvite(string accepteeId, Action<bool> action = null)
	{
		if (!string.IsNullOrEmpty(accepteeId) && FriendsController.readyToOperate)
		{
			base.StartCoroutine(this.AcceptFriend(accepteeId, action));
		}
	}

	// Token: 0x060008C1 RID: 2241 RVA: 0x000354CC File Offset: 0x000336CC
	public void AcceptClanInvite(string recordId)
	{
		if (!string.IsNullOrEmpty(recordId) && FriendsController.readyToOperate)
		{
			base.StartCoroutine(this._AcceptClanInvite(recordId));
		}
	}

	// Token: 0x060008C2 RID: 2242 RVA: 0x000354F4 File Offset: 0x000336F4
	private IEnumerator _AcceptClanInvite(string recordId)
	{
		WWWForm form = new WWWForm();
		form.AddField("action", "accept_invite");
		form.AddField("app_version", ProtocolListGetter.CurrentPlatform + ":" + GlobalGameController.AppVersion);
		form.AddField("id_player", this.id);
		form.AddField("id_clan", recordId);
		form.AddField("uniq_id", FriendsController.sharedController.id);
		form.AddField("auth", FriendsController.Hash("accept_invite", null));
		WWW acceptInviteRequest = Tools.CreateWww(FriendsController.actionAddress, form, string.Empty);
		this.JoinClanSent = recordId;
		yield return acceptInviteRequest;
		string response = URLs.Sanitize(acceptInviteRequest);
		if (string.IsNullOrEmpty(acceptInviteRequest.error) && !string.IsNullOrEmpty(response) && Debug.isDebugBuild)
		{
			Debug.Log("Accept clan invite: " + response);
		}
		if (!string.IsNullOrEmpty(acceptInviteRequest.error))
		{
			if (Debug.isDebugBuild)
			{
				Debug.LogWarning("_AcceptClanInvite error: " + acceptInviteRequest.error);
			}
			this.JoinClanSent = null;
			yield break;
		}
		if (!string.IsNullOrEmpty(response) && response.Equals("fail"))
		{
			if (Debug.isDebugBuild)
			{
				Debug.LogWarning("_AcceptClanInvite fail.");
			}
			this.JoinClanSent = null;
			yield break;
		}
		this.clanLogo = this.tempClanLogo;
		this.ClanID = this.tempClanID;
		this.clanName = this.tempClanName;
		this.clanLeaderID = this.tempClanCreatorID;
		yield break;
	}

	// Token: 0x060008C3 RID: 2243 RVA: 0x00035520 File Offset: 0x00033720
	public void StartRefreshingOnline()
	{
		if (!FriendsController.readyToOperate)
		{
			return;
		}
		this._shouldStopOnline = false;
		base.StartCoroutine(this.RefreshOnlinePlayer());
	}

	// Token: 0x060008C4 RID: 2244 RVA: 0x00035544 File Offset: 0x00033744
	public void StopRefreshingOnline()
	{
		if (!FriendsController.readyToOperate)
		{
			return;
		}
		this._shouldStopOnline = true;
	}

	// Token: 0x060008C5 RID: 2245 RVA: 0x00035558 File Offset: 0x00033758
	public void StartRefreshingOnlineWithClanInfo()
	{
		if (!FriendsController.readyToOperate)
		{
			return;
		}
		this._shouldStopOnlineWithClanInfo = false;
		base.StartCoroutine(this.RefreshOnlinePlayerWithClanInfo());
	}

	// Token: 0x060008C6 RID: 2246 RVA: 0x0003557C File Offset: 0x0003377C
	public void StopRefreshingOnlineWithClanInfo()
	{
		if (!FriendsController.readyToOperate)
		{
			return;
		}
		this._shouldStopOnlineWithClanInfo = true;
	}

	// Token: 0x060008C7 RID: 2247 RVA: 0x00035590 File Offset: 0x00033790
	private IEnumerator RefreshOnlinePlayerWithClanInfo()
	{
		do
		{
			while (this.idle)
			{
				yield return null;
			}
			base.StartCoroutine(this.GetAllPlayersOnlineWithClanInfo());
			float startTime = Time.realtimeSinceStartup;
			do
			{
				yield return null;
			}
			while (Time.realtimeSinceStartup - startTime < 20f && !this._shouldStopOnlineWithClanInfo);
		}
		while (!this._shouldStopOnlineWithClanInfo);
		this._shouldStopOnlineWithClanInfo = false;
		yield break;
	}

	// Token: 0x060008C8 RID: 2248 RVA: 0x000355AC File Offset: 0x000337AC
	private IEnumerator RefreshOnlinePlayer()
	{
		do
		{
			while (this.idle)
			{
				yield return null;
			}
			base.StartCoroutine(this.GetAllPlayersOnline());
			float startTime = Time.realtimeSinceStartup;
			do
			{
				yield return null;
			}
			while (Time.realtimeSinceStartup - startTime < 20f && !this._shouldStopOnline);
		}
		while (!this._shouldStopOnline);
		this._shouldStopOnline = false;
		yield break;
	}

	// Token: 0x060008C9 RID: 2249 RVA: 0x000355C8 File Offset: 0x000337C8
	public void StartRefreshingClanOnline()
	{
		if (!FriendsController.readyToOperate)
		{
			return;
		}
		this._shouldStopRefrClanOnline = false;
		base.StartCoroutine(this.RefreshClanOnline());
	}

	// Token: 0x060008CA RID: 2250 RVA: 0x000355EC File Offset: 0x000337EC
	public void StopRefreshingClanOnline()
	{
		if (!FriendsController.readyToOperate)
		{
			return;
		}
		this._shouldStopRefrClanOnline = true;
	}

	// Token: 0x060008CB RID: 2251 RVA: 0x00035600 File Offset: 0x00033800
	private IEnumerator RefreshClanOnline()
	{
		do
		{
			while (this.idle)
			{
				yield return null;
			}
			base.StartCoroutine(this.GetClanPlayersOnline());
			float startTime = Time.realtimeSinceStartup;
			do
			{
				yield return null;
			}
			while (Time.realtimeSinceStartup - startTime < 20f && !this._shouldStopRefrClanOnline);
		}
		while (!this._shouldStopRefrClanOnline);
		this._shouldStopRefrClanOnline = false;
		yield break;
	}

	// Token: 0x060008CC RID: 2252 RVA: 0x0003561C File Offset: 0x0003381C
	private IEnumerator GetClanPlayersOnline()
	{
		if (!FriendsController.readyToOperate)
		{
			yield break;
		}
		List<string> ids = new List<string>();
		foreach (Dictionary<string, string> fr in this.clanMembers)
		{
			string firIdStr;
			if (fr.TryGetValue("id", out firIdStr))
			{
				ids.Add(firIdStr);
			}
		}
		yield return base.StartCoroutine(this._GetOnlineForPlayerIDs(ids));
		yield break;
	}

	// Token: 0x060008CD RID: 2253 RVA: 0x00035638 File Offset: 0x00033838
	private IEnumerator GetAllPlayersOnline()
	{
		if (!FriendsController.readyToOperate)
		{
			yield break;
		}
		yield return base.StartCoroutine(this._GetOnlineForPlayerIDs(this.friends));
		yield break;
	}

	// Token: 0x060008CE RID: 2254 RVA: 0x00035654 File Offset: 0x00033854
	private IEnumerator GetAllPlayersOnlineWithClanInfo()
	{
		if (!FriendsController.readyToOperate)
		{
			yield break;
		}
		yield return base.StartCoroutine(this._GetOnlineWithClanInfoForPlayerIDs(this.friends));
		yield break;
	}

	// Token: 0x060008CF RID: 2255 RVA: 0x00035670 File Offset: 0x00033870
	private IEnumerator _GetOnlineForPlayerIDs(List<string> ids)
	{
		if (ids.Count == 0)
		{
			yield break;
		}
		string json = Json.Serialize(ids);
		if (json == null)
		{
			yield break;
		}
		WWWForm form = new WWWForm();
		form.AddField("action", "get_all_players_online");
		form.AddField("app_version", ProtocolListGetter.CurrentPlatform + ":" + GlobalGameController.AppVersion);
		form.AddField("id", this.id);
		form.AddField("ids", json);
		form.AddField("uniq_id", FriendsController.sharedController.id);
		form.AddField("auth", FriendsController.Hash("get_all_players_online", null));
		WWW download = Tools.CreateWwwIfNotConnected(FriendsController.actionAddress, form, string.Empty, null);
		if (download == null)
		{
			yield break;
		}
		yield return download;
		string response = URLs.Sanitize(download);
		if (string.IsNullOrEmpty(download.error) && !string.IsNullOrEmpty(response) && (Debug.isDebugBuild || Application.isEditor))
		{
			Debug.Log(response);
		}
		if (!string.IsNullOrEmpty(download.error))
		{
			if (Debug.isDebugBuild || Application.isEditor)
			{
				Debug.LogWarning("GetAllPlayersOnline error: " + download.error);
			}
			yield break;
		}
		Dictionary<string, object> __list = Json.Deserialize(response) as Dictionary<string, object>;
		if (__list == null)
		{
			if (Debug.isDebugBuild || Application.isEditor)
			{
				Debug.LogWarning(" GetAllPlayersOnline info = null");
			}
			yield break;
		}
		Dictionary<string, Dictionary<string, string>> list = new Dictionary<string, Dictionary<string, string>>();
		foreach (string key in __list.Keys)
		{
			Dictionary<string, object> d = __list[key] as Dictionary<string, object>;
			Dictionary<string, string> newd = new Dictionary<string, string>();
			foreach (KeyValuePair<string, object> kvp in d)
			{
				newd.Add(kvp.Key, kvp.Value as string);
			}
			list.Add(key, newd);
		}
		this.onlineInfo.Clear();
		foreach (string key2 in list.Keys)
		{
			Dictionary<string, string> d2 = list[key2];
			int _game_mode = int.Parse(d2["game_mode"]);
			int _regim = _game_mode - _game_mode / 10 * 10;
			if (_regim != 3 && _regim != 8)
			{
				if (!this.onlineInfo.ContainsKey(key2))
				{
					this.onlineInfo.Add(key2, d2);
				}
				else
				{
					this.onlineInfo[key2] = d2;
				}
			}
		}
		yield break;
	}

	// Token: 0x060008D0 RID: 2256 RVA: 0x0003569C File Offset: 0x0003389C
	private IEnumerator _GetOnlineWithClanInfoForPlayerIDs(List<string> ids)
	{
		if (ids.Count == 0)
		{
			yield break;
		}
		string json = Json.Serialize(ids);
		if (json == null)
		{
			yield break;
		}
		WWWForm form = new WWWForm();
		form.AddField("action", "get_all_players_online_with_clan_info");
		form.AddField("app_version", ProtocolListGetter.CurrentPlatform + ":" + GlobalGameController.AppVersion);
		form.AddField("id", this.id);
		form.AddField("ids", json);
		form.AddField("uniq_id", FriendsController.sharedController.id);
		form.AddField("auth", FriendsController.Hash("get_all_players_online_with_clan_info", null));
		WWW download = Tools.CreateWwwIfNotConnected(FriendsController.actionAddress, form, string.Empty, null);
		if (download == null)
		{
			yield break;
		}
		yield return download;
		string response = URLs.Sanitize(download);
		if (string.IsNullOrEmpty(download.error) && !string.IsNullOrEmpty(response) && Debug.isDebugBuild)
		{
			Debug.Log(response);
		}
		if (!string.IsNullOrEmpty(download.error))
		{
			if (Debug.isDebugBuild)
			{
				Debug.LogWarning("_GetOnlineWithClanInfoForPlayerIDs error: " + download.error);
			}
			yield break;
		}
		Dictionary<string, object> allDict = Json.Deserialize(response) as Dictionary<string, object>;
		if (allDict == null)
		{
			if (Debug.isDebugBuild)
			{
				Debug.LogWarning(" _GetOnlineWithClanInfoForPlayerIDs allDict = null");
			}
			yield break;
		}
		Dictionary<string, object> __list = allDict["online"] as Dictionary<string, object>;
		if (__list == null)
		{
			if (Debug.isDebugBuild)
			{
				Debug.LogWarning(" _GetOnlineWithClanInfoForPlayerIDs __list = null");
			}
			yield break;
		}
		Dictionary<string, Dictionary<string, string>> list = new Dictionary<string, Dictionary<string, string>>();
		foreach (string key in __list.Keys)
		{
			Dictionary<string, object> d = __list[key] as Dictionary<string, object>;
			Dictionary<string, string> newd = new Dictionary<string, string>();
			foreach (KeyValuePair<string, object> kvp in d)
			{
				newd.Add(kvp.Key, kvp.Value as string);
			}
			list.Add(key, newd);
		}
		this.onlineInfo.Clear();
		foreach (string key2 in list.Keys)
		{
			Dictionary<string, string> d2 = list[key2];
			int _game_mode = int.Parse(d2["game_mode"]);
			int _regim = _game_mode - _game_mode / 10 * 10;
			if (_regim != 3 && _regim != 8)
			{
				if (!this.onlineInfo.ContainsKey(key2))
				{
					this.onlineInfo.Add(key2, d2);
				}
				else
				{
					this.onlineInfo[key2] = d2;
				}
			}
		}
		Dictionary<string, object> clanInfo = allDict["clan_info"] as Dictionary<string, object>;
		if (clanInfo == null)
		{
			if (Debug.isDebugBuild || Application.isEditor)
			{
				Debug.LogWarning(" _GetOnlineWithClanInfoForPlayerIDs clanInfo = null");
			}
			yield break;
		}
		Dictionary<string, Dictionary<string, string>> convertedClanInfo = new Dictionary<string, Dictionary<string, string>>();
		foreach (string key3 in clanInfo.Keys)
		{
			Dictionary<string, object> d3 = clanInfo[key3] as Dictionary<string, object>;
			Dictionary<string, string> newd2 = new Dictionary<string, string>();
			foreach (KeyValuePair<string, object> kvp2 in d3)
			{
				newd2.Add(kvp2.Key, Convert.ToString(kvp2.Value));
			}
			convertedClanInfo.Add(key3, newd2);
		}
		foreach (string playerID in convertedClanInfo.Keys)
		{
			Dictionary<string, string> playerClanInfo = convertedClanInfo[playerID];
			if (FriendsController.sharedController.playersInfo.ContainsKey(playerID))
			{
				Dictionary<string, object> pl = FriendsController.sharedController.playersInfo[playerID];
				if (pl.ContainsKey("player"))
				{
					Dictionary<string, object> plpl = pl["player"] as Dictionary<string, object>;
					if (plpl.ContainsKey("clan_creator_id"))
					{
						plpl["clan_creator_id"] = playerClanInfo["clan_creator_id"];
					}
					else
					{
						plpl.Add("clan_creator_id", playerClanInfo["clan_creator_id"]);
					}
					if (plpl.ContainsKey("clan_name"))
					{
						plpl["clan_name"] = playerClanInfo["clan_name"];
					}
					else
					{
						plpl.Add("clan_name", playerClanInfo["clan_name"]);
					}
					if (plpl.ContainsKey("clan_logo"))
					{
						plpl["clan_logo"] = playerClanInfo["clan_logo"];
					}
					else
					{
						plpl.Add("clan_logo", playerClanInfo["clan_logo"]);
					}
				}
			}
		}
		yield break;
	}

	// Token: 0x060008D1 RID: 2257 RVA: 0x000356C8 File Offset: 0x000338C8
	public void GetFacebookFriendsInfo(Action callb)
	{
		if (!FriendsController.readyToOperate)
		{
			return;
		}
		base.StartCoroutine(this._GetFacebookFriendsInfo(callb));
	}

	// Token: 0x060008D2 RID: 2258 RVA: 0x000356E4 File Offset: 0x000338E4
	private IEnumerator _GetFacebookFriendsInfo(Action callb)
	{
		if (!FacebookController.FacebookSupported || FacebookController.sharedController.friendsList == null)
		{
			yield break;
		}
		this.GetFacebookFriendsCallback = callb;
		List<string> ids = new List<string>();
		foreach (Friend f in FacebookController.sharedController.friendsList)
		{
			ids.Add(f.id);
		}
		WWWForm form = new WWWForm();
		form.AddField("action", "get_info_by_facebook_ids");
		form.AddField("app_version", ProtocolListGetter.CurrentPlatform + ":" + GlobalGameController.AppVersion);
		form.AddField("platform", ProtocolListGetter.CurrentPlatform.ToString());
		form.AddField("id", this.id);
		string json = Json.Serialize(ids);
		form.AddField("ids", json);
		form.AddField("uniq_id", FriendsController.sharedController.id);
		form.AddField("auth", FriendsController.Hash("get_info_by_facebook_ids", null));
		Debug.LogFormat("Facebook json: {0}", new object[]
		{
			json
		});
		WWW download = Tools.CreateWwwIfNotConnected(FriendsController.actionAddress, form, string.Empty, null);
		if (download == null)
		{
			this.GetFacebookFriendsCallback = null;
			yield break;
		}
		yield return download;
		string response = URLs.Sanitize(download);
		if (string.IsNullOrEmpty(download.error) && !string.IsNullOrEmpty(response) && (Debug.isDebugBuild || Application.isEditor))
		{
			Debug.Log(response);
		}
		if (!string.IsNullOrEmpty(download.error))
		{
			if (Debug.isDebugBuild || Application.isEditor)
			{
				Debug.LogWarning("_GetFacebookFriendsInfo error: " + download.error);
			}
			this.GetFacebookFriendsCallback = null;
			yield break;
		}
		List<object> __info = Json.Deserialize(response) as List<object>;
		if (__info == null)
		{
			if (Debug.isDebugBuild || Application.isEditor)
			{
				Debug.LogWarning(" _GetFacebookFriendsInfo info = null");
			}
			this.GetFacebookFriendsCallback = null;
			yield break;
		}
		foreach (object obj in __info)
		{
			Dictionary<string, object> d = (Dictionary<string, object>)obj;
			Dictionary<string, object> ff = new Dictionary<string, object>();
			foreach (KeyValuePair<string, object> i in d)
			{
				ff.Add(i.Key, i.Value);
			}
			object ffid;
			if (ff.TryGetValue("id", out ffid))
			{
				this.facebookFriendsInfo.Add(ffid as string, ff);
			}
		}
		if (this.GetFacebookFriendsCallback != null)
		{
			this.GetFacebookFriendsCallback();
		}
		this.GetFacebookFriendsCallback = null;
		yield break;
	}

	// Token: 0x060008D3 RID: 2259 RVA: 0x00035710 File Offset: 0x00033910
	private IEnumerator UpdatePlayerOnlineLoop()
	{
		for (;;)
		{
			while (this.idle)
			{
				yield return null;
			}
			int gameMode = -1;
			int platform = (int)ConnectSceneNGUIController.myPlatformConnect;
			if (PhotonNetwork.room != null)
			{
				gameMode = (int)ConnectSceneNGUIController.regim;
				if (!string.IsNullOrEmpty(PhotonNetwork.room.customProperties[ConnectSceneNGUIController.passwordProperty].ToString()))
				{
					platform = 3;
				}
			}
			if (gameMode != -1)
			{
				base.StartCoroutine(this.UpdatePlayerOnline(platform * 100 + ConnectSceneNGUIController.gameTier * 10 + gameMode));
			}
			yield return base.StartCoroutine(this.MyWaitForSeconds(Defs.timeUpdateOnlineInGame));
		}
		yield break;
	}

	// Token: 0x060008D4 RID: 2260 RVA: 0x0003572C File Offset: 0x0003392C
	public void SendAddPurchaseEvent(string purchaseId, string transactionId, float parsedPrice, string currencyCode, string countryCode)
	{
		int inapp = 0;
		int num = Array.IndexOf<string>(StoreKitEventListener.coinIds, purchaseId);
		if (num != -1)
		{
			inapp = VirtualCurrencyHelper.coinPriceIds[num];
		}
		else
		{
			num = Array.IndexOf<string>(StoreKitEventListener.gemsIds, purchaseId);
			if (num != -1)
			{
				inapp = VirtualCurrencyHelper.gemsPriceIds[num];
			}
		}
		base.StartCoroutine(this.AddPurchaseEvent(inapp, purchaseId, transactionId, parsedPrice, currencyCode, countryCode));
	}

	// Token: 0x060008D5 RID: 2261 RVA: 0x00035790 File Offset: 0x00033990
	private IEnumerator AddPurchaseEvent(int inapp, string purchaseId, string transactionId, float parsedPrice, string currencyCode, string countryCode)
	{
		WaitForSeconds awaiter = new WaitForSeconds(5f);
		for (;;)
		{
			WWWForm form = new WWWForm();
			form.AddField("action", "add_purchase");
			form.AddField("auth", FriendsController.Hash("add_purchase", null));
			form.AddField("app_version", ProtocolListGetter.CurrentPlatform + ":" + GlobalGameController.AppVersion);
			form.AddField("uniq_id", this.id);
			form.AddField("platform", ProtocolListGetter.CurrentPlatform.ToString());
			form.AddField("type_device", (!Device.isPixelGunLow) ? 2 : 1);
			int playerLevel = (!(ExperienceController.sharedController != null)) ? 1 : ExperienceController.sharedController.currentLevel;
			form.AddField("rank", playerLevel);
			form.AddField("inapp", inapp);
			form.AddField("transactionId", transactionId);
			form.AddField("parsedPrice", Mathf.RoundToInt(parsedPrice * 1000f).ToString());
			form.AddField("currencyCode", currencyCode);
			form.AddField("countryCode", countryCode);
			form.AddField("tier", ExpController.OurTierForAnyPlace());
			if (Defs.abTestBalansCohort != Defs.ABTestCohortsType.NONE && Defs.abTestBalansCohort != Defs.ABTestCohortsType.SKIP)
			{
				form.AddField("cohortName", Defs.abTestBalansCohortName);
			}
			if (Defs.cohortABTestAdvert == Defs.ABTestCohortsType.A || Defs.cohortABTestAdvert == Defs.ABTestCohortsType.B)
			{
				string _currentConfigAdvertNameForEvent = ((Defs.cohortABTestAdvert != Defs.ABTestCohortsType.A) ? "AdvertB_" : "AdvertA_") + FriendsController.configNameABTestAdvert;
				form.AddField("cohort_ad", _currentConfigAdvertNameForEvent);
			}
			foreach (ABTestBase abtest in ABTestController.currentABTests)
			{
				if (abtest.cohort == ABTestController.ABTestCohortsType.A || abtest.cohort == ABTestController.ABTestCohortsType.B)
				{
					form.AddField(abtest.currentFolder, abtest.cohortName);
				}
			}
			form.AddField("purchaseId", purchaseId);
			float savedPlayTime = 0f;
			float savedPlayTimeInMatch = 0f;
			if (Storager.hasKey("PlayTime") && float.TryParse(Storager.getString("PlayTime", false), out savedPlayTime))
			{
				form.AddField("playTime", Mathf.RoundToInt(savedPlayTime));
			}
			if (Storager.hasKey("PlayTimeInMatch") && float.TryParse(Storager.getString("PlayTimeInMatch", false), out savedPlayTimeInMatch))
			{
				form.AddField("playTimeInMatch", Mathf.RoundToInt(savedPlayTimeInMatch));
			}
			WWW addPurchaseEventRequest = Tools.CreateWww(FriendsController.actionAddress, form, string.Empty);
			yield return addPurchaseEventRequest;
			string response = URLs.Sanitize(addPurchaseEventRequest);
			if (!string.IsNullOrEmpty(addPurchaseEventRequest.error))
			{
				if (Debug.isDebugBuild)
				{
					Debug.LogWarning("AddPurchaseEvent error: " + addPurchaseEventRequest.error);
				}
				yield return awaiter;
			}
			else
			{
				if (string.IsNullOrEmpty(response) || !response.Equals("fail"))
				{
					break;
				}
				if (Debug.isDebugBuild)
				{
					Debug.LogWarning("AddPurchaseEvent fail.");
				}
				yield return awaiter;
			}
		}
		yield break;
	}

	// Token: 0x060008D6 RID: 2262 RVA: 0x00035808 File Offset: 0x00033A08
	public static void SendToturialEvent(int _event, string _progress)
	{
		CoroutineRunner.Instance.StartCoroutine(FriendsController.AddToturialEvent(_event, _progress));
	}

	// Token: 0x060008D7 RID: 2263 RVA: 0x0003581C File Offset: 0x00033A1C
	private static IEnumerator AddToturialEvent(int _event, string _progress)
	{
		WWWForm form = new WWWForm();
		form.AddField("event_id", _event);
		form.AddField("progress", _progress);
		form.AddField("device_id", SystemInfo.deviceUniqueIdentifier);
		form.AddField("device_model", SystemInfo.deviceModel);
		form.AddField("type_device", (!Device.isPixelGunLow) ? 2 : 1);
		form.AddField("platform", ProtocolListGetter.CurrentPlatform.ToString());
		form.AddField("version", GlobalGameController.AppVersion);
		form.AddField("release", (!Defs.IsDeveloperBuild) ? 1 : 0);
		WWW addToturialEventRequest = Tools.CreateWww("https://acct.pixelgunserver.com/events/add_event.php", form, string.Empty);
		yield return addToturialEventRequest;
		string response = URLs.Sanitize(addToturialEventRequest);
		if (!string.IsNullOrEmpty(addToturialEventRequest.error))
		{
			if (Debug.isDebugBuild)
			{
				Debug.LogWarning("Toturial Event error: " + addToturialEventRequest.error);
			}
		}
		else if (!string.IsNullOrEmpty(response) && response.Equals("fail") && Debug.isDebugBuild)
		{
			Debug.LogWarning("Toturial Event fail.");
		}
		yield break;
	}

	// Token: 0x060008D8 RID: 2264 RVA: 0x0003584C File Offset: 0x00033A4C
	public void SendRequestGetCurrentcompetition()
	{
		base.StartCoroutine("GetCurrentcompetition");
	}

	// Token: 0x060008D9 RID: 2265 RVA: 0x0003585C File Offset: 0x00033A5C
	public IEnumerator GetCurrentcompetition()
	{
		string response;
		for (;;)
		{
			while (string.IsNullOrEmpty(this.id))
			{
				yield return null;
			}
			WWWForm form = new WWWForm();
			form.AddField("action", "get_current_competition");
			form.AddField("auth", FriendsController.Hash("get_current_competition", null));
			form.AddField("app_version", ProtocolListGetter.CurrentPlatform + ":" + GlobalGameController.AppVersion);
			form.AddField("uniq_id", this.id);
			WWW getCurrentSeasonRequest = Tools.CreateWww(FriendsController.actionAddress, form, string.Empty);
			yield return getCurrentSeasonRequest;
			if (!string.IsNullOrEmpty(getCurrentSeasonRequest.error))
			{
				if (Debug.isDebugBuild)
				{
					Debug.LogWarning("GetCurrentcompetitionRequest error: " + getCurrentSeasonRequest.error);
				}
				yield return new WaitForSeconds(20f);
			}
			else
			{
				response = URLs.Sanitize(getCurrentSeasonRequest);
				if (string.IsNullOrEmpty(response) || !response.Equals("fail"))
				{
					break;
				}
				if (Debug.isDebugBuild)
				{
					Debug.LogWarning("GetCurrentcompetitionnRequest fail.");
				}
				yield return new WaitForSeconds(20f);
			}
		}
		this.ParseResponseCurrenCompetion(response);
		yield break;
	}

	// Token: 0x060008DA RID: 2266 RVA: 0x00035878 File Offset: 0x00033A78
	public IEnumerator SynchRating(int rating)
	{
		WWWForm form = new WWWForm();
		form.AddField("action", "synch_rating_tiers");
		form.AddField("auth", FriendsController.Hash("synch_rating_tiers", null));
		form.AddField("app_version", ProtocolListGetter.CurrentPlatform + ":" + GlobalGameController.AppVersion);
		form.AddField("uniq_id", this.id);
		form.AddField("rating", rating);
		form.AddField("abuse_method", Storager.getInt("AbuseMethod", false));
		form.AddField("tier", ExpController.OurTierForAnyPlace());
		form.AddField("competition_id", this.currentCompetition);
		WWW synchRatingRequest = Tools.CreateWww(FriendsController.actionAddress, form, string.Empty);
		yield return synchRatingRequest;
		string response = URLs.Sanitize(synchRatingRequest);
		if (!string.IsNullOrEmpty(synchRatingRequest.error))
		{
			if (Debug.isDebugBuild)
			{
				Debug.LogWarning("synchRatingRequest error: " + synchRatingRequest.error);
			}
			yield break;
		}
		if (!string.IsNullOrEmpty(response) && (response.Equals("fail") || response.Equals("ok")))
		{
			yield break;
		}
		this.ParseResponseCurrenCompetion(response);
		yield break;
	}

	// Token: 0x060008DB RID: 2267 RVA: 0x000358A4 File Offset: 0x00033AA4
	private void ParseResponseCurrenCompetion(string _response)
	{
		Dictionary<string, object> dictionary = Json.Deserialize(_response) as Dictionary<string, object>;
		bool flag = false;
		int num;
		if (dictionary.ContainsKey("competition_id") && int.TryParse(dictionary["competition_id"].ToString(), out num) && this.currentCompetition != num)
		{
			this.StartNewCompetion();
			flag = true;
			this.currentCompetition = num;
		}
		if (dictionary.ContainsKey("competition_time"))
		{
			this.expirationTimeCompetition = Convert.ToSingle(dictionary["competition_time"]);
		}
		bool flag2 = false;
		if (dictionary.ContainsKey("reward"))
		{
			Dictionary<string, object> dictionary2 = dictionary["reward"] as Dictionary<string, object>;
			if (dictionary2 != null && dictionary2.ContainsKey("place"))
			{
				int num2 = Convert.ToInt32(dictionary2["place"]);
				if (num2 <= BalanceController.countPlaceAwardInCompetion)
				{
					flag2 = true;
				}
			}
		}
		if (flag)
		{
			if (flag2)
			{
				TournamentWinnerBannerWindow.CanShow = true;
			}
			else if (RatingSystem.instance.currentLeague == RatingSystem.RatingLeague.Adamant)
			{
				TournamentLooserBannerWindow.CanShow = true;
			}
			int trophiesSeasonThreshold = RatingSystem.instance.TrophiesSeasonThreshold;
			if (RatingSystem.instance.currentRating > trophiesSeasonThreshold)
			{
				int num3 = RatingSystem.instance.currentRating - trophiesSeasonThreshold;
				RatingSystem.instance.negativeRating += num3;
				RatingSystem.instance.UpdateLeagueEvent(null, null);
			}
		}
	}

	// Token: 0x060008DC RID: 2268 RVA: 0x00035A04 File Offset: 0x00033C04
	private void StartNewCompetion()
	{
		if (!string.IsNullOrEmpty(this.id))
		{
			LeaderboardScript.RequestLeaderboards(this.id);
		}
	}

	// Token: 0x060008DD RID: 2269 RVA: 0x00035A24 File Offset: 0x00033C24
	private IEnumerator UpdatePlayerOnline(int gameMode)
	{
		WWWForm form = new WWWForm();
		form.AddField("action", "update_player_online");
		form.AddField("app_version", ProtocolListGetter.CurrentPlatform + ":" + GlobalGameController.AppVersion);
		form.AddField("id", this.id);
		form.AddField("game_mode", gameMode.ToString("D3"));
		form.AddField("room_name", (PhotonNetwork.room == null || PhotonNetwork.room.name == null) ? string.Empty : PhotonNetwork.room.name);
		form.AddField("map", (PhotonNetwork.room == null || PhotonNetwork.room.customProperties == null) ? string.Empty : PhotonNetwork.room.customProperties[ConnectSceneNGUIController.mapProperty].ToString());
		form.AddField("protocol", GlobalGameController.MultiplayerProtocolVersion);
		form.AddField("uniq_id", FriendsController.sharedController.id);
		form.AddField("auth", FriendsController.Hash("update_player_online", null));
		form.AddField("platform", ProtocolListGetter.CurrentPlatform.ToString());
		form.AddField("type_device", (!Device.isPixelGunLow) ? 2 : 1);
		form.AddField("game_time", Mathf.RoundToInt(this.deltaTimeInGame));
		this.sendingTime = (float)Mathf.RoundToInt(this.deltaTimeInGame);
		form.AddField("tier", ExpController.OurTierForAnyPlace());
		if (Defs.abTestBalansCohort != Defs.ABTestCohortsType.NONE && Defs.abTestBalansCohort != Defs.ABTestCohortsType.SKIP)
		{
			form.AddField("cohortName", Defs.abTestBalansCohortName);
		}
		if (Defs.cohortABTestAdvert == Defs.ABTestCohortsType.A || Defs.cohortABTestAdvert == Defs.ABTestCohortsType.B)
		{
			string _currentConfigAdvertNameForEvent = ((Defs.cohortABTestAdvert != Defs.ABTestCohortsType.A) ? "AdvertB_" : "AdvertA_") + FriendsController.configNameABTestAdvert;
			form.AddField("cohort_ad", _currentConfigAdvertNameForEvent);
		}
		foreach (ABTestBase abtest in ABTestController.currentABTests)
		{
			if (abtest.cohort == ABTestController.ABTestCohortsType.A || abtest.cohort == ABTestController.ABTestCohortsType.B)
			{
				form.AddField(abtest.currentFolder, abtest.cohortName);
			}
		}
		form.AddField("paying", Storager.getInt("PayingUser", true).ToString());
		int playerLevel = (!(ExperienceController.sharedController != null)) ? 1 : ExperienceController.sharedController.currentLevel;
		form.AddField("rank", playerLevel);
		WWW updatePlayerOnlineRequest = Tools.CreateWww(FriendsController.actionAddress, form, string.Empty);
		yield return updatePlayerOnlineRequest;
		string response = URLs.Sanitize(updatePlayerOnlineRequest);
		if (!string.IsNullOrEmpty(updatePlayerOnlineRequest.error))
		{
			if (Debug.isDebugBuild)
			{
				Debug.LogWarning("UpdatePlayerOnline error: " + updatePlayerOnlineRequest.error);
			}
			yield break;
		}
		if (!string.IsNullOrEmpty(response) && response.Equals("fail"))
		{
			if (Debug.isDebugBuild)
			{
				Debug.LogWarning("UpdatePlayerOnline fail.");
			}
			yield break;
		}
		this.deltaTimeInGame -= this.sendingTime;
		if (!string.IsNullOrEmpty(response) && !response.Equals("ok"))
		{
			Dictionary<string, object> cacheResponse = Json.Deserialize(response) as Dictionary<string, object>;
			if (cacheResponse != null && cacheResponse.ContainsKey("fight_invites"))
			{
				this.ParseFightInvite(cacheResponse["fight_invites"] as List<object>);
			}
		}
		yield break;
	}

	// Token: 0x060008DE RID: 2270 RVA: 0x00035A50 File Offset: 0x00033C50
	private IEnumerator GetToken()
	{
		string appVersionField = ProtocolListGetter.CurrentPlatform + ":" + GlobalGameController.AppVersion;
		WWWForm form = new WWWForm();
		form.AddField("action", "create_player_intent");
		form.AddField("app_version", appVersionField);
		string response;
		for (;;)
		{
			WWW download = Tools.CreateWwwIfNotConnected(FriendsController.actionAddress, form, string.Empty, null);
			if (download == null)
			{
				yield return base.StartCoroutine(this.MyWaitForSeconds(10f));
			}
			else
			{
				yield return download;
				response = URLs.Sanitize(download);
				if (!string.IsNullOrEmpty(download.error))
				{
					if (Debug.isDebugBuild || Application.isEditor)
					{
						Debug.LogWarning("create_player_intent error: " + download.error);
					}
					yield return base.StartCoroutine(this.MyWaitForSeconds(10f));
				}
				else if (!string.IsNullOrEmpty(response) && response.Equals("fail"))
				{
					if (Debug.isDebugBuild || Application.isEditor)
					{
						Debug.LogWarning("create_player_intent fail.");
					}
					yield return base.StartCoroutine(this.MyWaitForSeconds(10f));
				}
				else
				{
					if (response != null)
					{
						break;
					}
					if (Debug.isDebugBuild || Application.isEditor)
					{
						Debug.LogWarning("create_player_intent response == null");
					}
					yield return base.StartCoroutine(this.MyWaitForSeconds(10f));
				}
			}
		}
		this._inputToken = response;
		yield break;
	}

	// Token: 0x060008DF RID: 2271 RVA: 0x00035A6C File Offset: 0x00033C6C
	private IEnumerator CreatePlayer()
	{
		string appVersionField = ProtocolListGetter.CurrentPlatform + ":" + GlobalGameController.AppVersion;
		string response;
		for (;;)
		{
			yield return base.StartCoroutine(this.GetToken());
			while (string.IsNullOrEmpty(this._inputToken))
			{
				yield return null;
			}
			WWWForm form = new WWWForm();
			form.AddField("action", "create_player");
			form.AddField("platform", ProtocolListGetter.CurrentPlatform.ToString());
			form.AddField("device", SystemInfo.deviceUniqueIdentifier);
			form.AddField("device_model", SystemInfo.deviceModel);
			form.AddField("app_version", appVersionField);
			string hash = FriendsController.Hash("create_player", this._inputToken);
			form.AddField("auth", hash);
			form.AddField("token", this._inputToken);
			if (Defs.IsDeveloperBuild)
			{
				form.AddField("dev", 1);
			}
			string tokenHashString = string.Format("token:hash = {0}:{1}", this._inputToken, hash);
			this._inputToken = null;
			bool canPrintSecuritySensitiveInfo = Debug.isDebugBuild || Defs.IsDeveloperBuild;
			if (canPrintSecuritySensitiveInfo)
			{
				Debug.Log("CreatePlayer: Trying to perform request for “" + tokenHashString + "”...");
			}
			WWW download = Tools.CreateWwwIfNotConnected(FriendsController.actionAddress, form, string.Empty, null);
			if (download == null)
			{
				yield return base.StartCoroutine(this.MyWaitForSeconds(10f));
			}
			else
			{
				yield return download;
				response = URLs.Sanitize(download);
				if (canPrintSecuritySensitiveInfo)
				{
					Debug.LogFormat("CreatePlayer: Response for '{0}' received:    '{1}'", new object[]
					{
						tokenHashString,
						response
					});
				}
				long resultId;
				if (!string.IsNullOrEmpty(download.error))
				{
					Debug.LogWarning("CreatePlayer error:    " + download.error);
					yield return base.StartCoroutine(this.MyWaitForSeconds(10f));
				}
				else if (!string.IsNullOrEmpty(response) && response.Equals("fail"))
				{
					Debug.LogWarning("CreatePlayer failed.");
					yield return base.StartCoroutine(this.MyWaitForSeconds(10f));
				}
				else if (string.IsNullOrEmpty(response))
				{
					Debug.LogWarning("CreatePlayer response is empty.");
					yield return base.StartCoroutine(this.MyWaitForSeconds(10f));
				}
				else if (!long.TryParse(response, out resultId))
				{
					Debug.LogWarning("CreatePlayer parsing error in response:    “" + response + "”");
					yield return base.StartCoroutine(this.MyWaitForSeconds(10f));
				}
				else
				{
					if (resultId >= 1L)
					{
						break;
					}
					Debug.LogWarning("CreatePlayer bad id:    “" + response + "”");
					yield return base.StartCoroutine(this.MyWaitForSeconds(10f));
				}
			}
		}
		Debug.Log("CreatePlayer succeeded with response:    “" + response + "”");
		Storager.setString(this.AccountCreated, response, false);
		this.id = response;
		FriendsController.readyToOperate = true;
		base.StartCoroutine(this.GetFriendsDataLoop());
		base.StartCoroutine(this.GetClanDataLoop());
		this.GetOurLAstOnline();
		this.GetOurWins();
		yield break;
	}

	// Token: 0x170000F3 RID: 243
	// (get) Token: 0x060008E0 RID: 2272 RVA: 0x00035A88 File Offset: 0x00033C88
	public KeyValuePair<string, int>? WinCountTimestamp
	{
		get
		{
			return this._winCountTimestamp;
		}
	}

	// Token: 0x060008E1 RID: 2273 RVA: 0x00035A90 File Offset: 0x00033C90
	private void SetWinCountTimestamp(string timestamp, int winCount)
	{
		this._winCountTimestamp = new KeyValuePair<string, int>?(new KeyValuePair<string, int>(timestamp, winCount));
		string text = string.Format("{{ \"{0}\": {1} }}", timestamp, winCount);
		Storager.setString("Win Count Timestamp", text, false);
		if (Application.isEditor)
		{
			Debug.Log("Setting win count timestamp:    " + text);
		}
	}

	// Token: 0x060008E2 RID: 2274 RVA: 0x00035AE8 File Offset: 0x00033CE8
	public bool TryIncrementWinCountTimestamp()
	{
		if (this._winCountTimestamp == null)
		{
			return false;
		}
		this._winCountTimestamp = new KeyValuePair<string, int>?(new KeyValuePair<string, int>(this._winCountTimestamp.Value.Key, this._winCountTimestamp.Value.Value + 1));
		return true;
	}

	// Token: 0x060008E3 RID: 2275 RVA: 0x00035B40 File Offset: 0x00033D40
	private IEnumerator RequestWinCountTimestampCoroutine()
	{
		yield break;
	}

	// Token: 0x060008E4 RID: 2276 RVA: 0x00035B58 File Offset: 0x00033D58
	private void GetOurLAstOnline()
	{
		base.StartCoroutine(this.GetInfoByEverydayDelta());
		this.ReceivedLastOnline = true;
		base.StartCoroutine(this.UpdatePlayerOnlineLoop());
	}

	// Token: 0x060008E5 RID: 2277 RVA: 0x00035B88 File Offset: 0x00033D88
	public void DownloadInfoByEverydayDelta()
	{
		base.StartCoroutine(this.GetInfoByEverydayDelta());
	}

	// Token: 0x060008E6 RID: 2278 RVA: 0x00035B98 File Offset: 0x00033D98
	private IEnumerator GetInfoByEverydayDelta()
	{
		bool needTakeMarathonBonus = false;
		WWWForm form = new WWWForm();
		form.AddField("action", "get_player_online");
		form.AddField("id", this.id);
		form.AddField("app_version", "*:*.*.*");
		form.AddField("uniq_id", FriendsController.sharedController.id);
		form.AddField("auth", FriendsController.Hash("get_player_online", null));
		WWW getPlayerOnlineRequest = Tools.CreateWwwIfNotConnected(FriendsController.actionAddress, form, "*:*.*.*", null);
		if (getPlayerOnlineRequest == null)
		{
			yield return base.StartCoroutine(this.MyWaitForSeconds(120f));
		}
		else
		{
			yield return getPlayerOnlineRequest;
			string response = URLs.Sanitize(getPlayerOnlineRequest);
			if (!string.IsNullOrEmpty(getPlayerOnlineRequest.error))
			{
				Debug.LogWarning("GetInfoByEverydayDelta()    Error: " + getPlayerOnlineRequest.error);
				yield return base.StartCoroutine(this.MyWaitForSeconds(120f));
			}
			else if ("fail".Equals(response))
			{
				Debug.LogWarning("GetInfoByEverydayDelta()    Fail returned.");
				yield return base.StartCoroutine(this.MyWaitForSeconds(120f));
			}
			else
			{
				JSONNode data = JSON.Parse(response);
				if (data == null)
				{
					Debug.LogWarning("GetInfoByEverydayDelta()    Cannot deserialize response: " + response);
					yield return base.StartCoroutine(this.MyWaitForSeconds(120f));
				}
				else
				{
					string deltaData = data["delta"].Value;
					float deltaValue;
					if (float.TryParse(deltaData, out deltaValue))
					{
						if (deltaValue > 82800f)
						{
							NotificationController.isGetEveryDayMoney = true;
							needTakeMarathonBonus = (Storager.getInt(Defs.NeedTakeMarathonBonus, false) == 0);
							if (needTakeMarathonBonus)
							{
								Storager.setInt(Defs.NeedTakeMarathonBonus, 1, false);
							}
						}
					}
					else
					{
						Debug.LogWarning("GetInfoByEverydayDelta()    Cannot parse delta: " + deltaData);
						yield return base.StartCoroutine(this.MyWaitForSeconds(120f));
					}
				}
			}
		}
		yield break;
	}

	// Token: 0x060008E7 RID: 2279 RVA: 0x00035BB4 File Offset: 0x00033DB4
	private string GetAccesoriesString()
	{
		string @string = Storager.getString(Defs.CapeEquppedSN, false);
		string value;
		if (@string == "cape_Custom")
		{
			string string2 = PlayerPrefs.GetString("NewUserCape");
			value = Tools.DeserializeJson<CapeMemento>(string2).Cape;
			if (string.IsNullOrEmpty(value))
			{
				value = SkinsController.StringFromTexture(Resources.Load<Texture2D>("cape_CustomTexture"));
			}
		}
		else
		{
			value = string.Empty;
		}
		string string3 = Storager.getString(Defs.HatEquppedSN, false);
		string string4 = Storager.getString(Defs.BootsEquppedSN, false);
		string string5 = Storager.getString("MaskEquippedSN", false);
		string string6 = Storager.getString(Defs.ArmorNewEquppedSN, false);
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		dictionary.Add("type", "0");
		dictionary.Add("name", @string);
		dictionary.Add("skin", value);
		Dictionary<string, string> dictionary2 = new Dictionary<string, string>();
		dictionary2.Add("type", "1");
		dictionary2.Add("name", string3);
		dictionary2.Add("skin", string.Empty);
		Dictionary<string, string> dictionary3 = new Dictionary<string, string>();
		dictionary3.Add("type", "2");
		dictionary3.Add("name", string4);
		dictionary3.Add("skin", string.Empty);
		Dictionary<string, string> dictionary4 = new Dictionary<string, string>();
		dictionary4.Add("type", "3");
		dictionary4.Add("name", string6);
		dictionary4.Add("skin", string.Empty);
		Dictionary<string, string> dictionary5 = new Dictionary<string, string>();
		dictionary5.Add("type", "4");
		dictionary5.Add("name", string5);
		dictionary5.Add("skin", string.Empty);
		return Json.Serialize(new List<Dictionary<string, string>>
		{
			dictionary,
			dictionary2,
			dictionary3,
			dictionary4,
			dictionary5
		});
	}

	// Token: 0x060008E8 RID: 2280 RVA: 0x00035D9C File Offset: 0x00033F9C
	public void SendAccessories()
	{
		if (!FriendsController.readyToOperate)
		{
			return;
		}
		base.StartCoroutine(this.UpdateAccessories());
	}

	// Token: 0x060008E9 RID: 2281 RVA: 0x00035DB8 File Offset: 0x00033FB8
	private IEnumerator UpdateAccessories()
	{
		if (string.IsNullOrEmpty(this.id))
		{
			yield break;
		}
		WWWForm form = new WWWForm();
		form.AddField("action", "update_accessories");
		form.AddField("app_version", ProtocolListGetter.CurrentPlatform + ":" + GlobalGameController.AppVersion);
		form.AddField("auth", FriendsController.Hash("update_accessories", null));
		form.AddField("uniq_id", this.id);
		form.AddField("accessories", this.GetAccesoriesString());
		WWW updateAccessoriesRequest = Tools.CreateWww(FriendsController.actionAddress, form, string.Empty);
		yield return updateAccessoriesRequest;
		string response = URLs.Sanitize(updateAccessoriesRequest);
		if (string.IsNullOrEmpty(updateAccessoriesRequest.error) && !string.IsNullOrEmpty(response) && Debug.isDebugBuild)
		{
			Debug.Log("UpdateAccessories: " + response);
		}
		if (!string.IsNullOrEmpty(updateAccessoriesRequest.error))
		{
			if (Debug.isDebugBuild)
			{
				Debug.LogWarning("UpdateAccessories error: " + updateAccessoriesRequest.error);
			}
		}
		else if (!string.IsNullOrEmpty(response) && response.Equals("fail") && (Debug.isDebugBuild || Application.isEditor))
		{
			Debug.LogWarning("UpdateAccessories fail.");
		}
		yield break;
	}

	// Token: 0x060008EA RID: 2282 RVA: 0x00035DD4 File Offset: 0x00033FD4
	private IEnumerator UpdatePlayer(bool sendSkin)
	{
		while (!this.ReceivedLastOnline || !this.infoLoaded || !this.getCohortInfo)
		{
			yield return null;
		}
		this.timeSendUpdatePlayer = Time.realtimeSinceStartup;
		this.InitOurInfo();
		WWWForm form = new WWWForm();
		form.AddField("action", "update_player");
		form.AddField("id", this.id);
		string filteredNick = this.nick;
		filteredNick = FilterBadWorld.FilterString(this.nick);
		if (filteredNick.Length > 20)
		{
			filteredNick = filteredNick.Substring(0, 20);
		}
		form.AddField("nick", filteredNick);
		form.AddField("skin", (!sendSkin) ? string.Empty : this.skin);
		form.AddField("rank", this.rank);
		form.AddField("wins", this.wins.Value);
		if (Defs.IsDeveloperBuild)
		{
			form.AddField("developer", 1);
		}
		form.AddField("cohortName", Defs.abTestBalansCohortName);
		if (Defs.cohortABTestAdvert == Defs.ABTestCohortsType.A || Defs.cohortABTestAdvert == Defs.ABTestCohortsType.B)
		{
			string _currentConfigAdvertNameForEvent = ((Defs.cohortABTestAdvert != Defs.ABTestCohortsType.A) ? "AdvertB_" : "AdvertA_") + FriendsController.configNameABTestAdvert;
			form.AddField("cohort_ad", _currentConfigAdvertNameForEvent);
		}
		foreach (ABTestBase abtest in ABTestController.currentABTests)
		{
			if (abtest.cohort == ABTestController.ABTestCohortsType.A || abtest.cohort == ABTestController.ABTestCohortsType.B)
			{
				form.AddField(abtest.currentFolder, abtest.cohortName);
			}
		}
		int totalWinCount = PlayerPrefs.GetInt("TotalWinsForLeaderboards", 0);
		form.AddField("total_wins", totalWinCount);
		form.AddField("id_fb", this.id_fb ?? string.Empty);
		form.AddField("device", SystemInfo.deviceUniqueIdentifier);
		form.AddField("app_version", ProtocolListGetter.CurrentPlatform + ":" + GlobalGameController.AppVersion);
		form.AddField("coins", Storager.getInt("Coins", false).ToString());
		form.AddField("gems", Storager.getInt("GemsCurrency", false).ToString());
		form.AddField("paying", Storager.getInt("PayingUser", true).ToString());
		form.AddField("kill_cnt", ProfileController.countGameTotalKills.Value);
		form.AddField("death_cnt", ProfileController.countGameTotalDeaths.Value);
		float savedPlayTime = 0f;
		float savedPlayTimeInMatch = 0f;
		if (Storager.hasKey("PlayTime") && float.TryParse(Storager.getString("PlayTime", false), out savedPlayTime))
		{
			form.AddField("playTime", Mathf.RoundToInt(savedPlayTime));
		}
		if (Storager.hasKey("PlayTimeInMatch") && float.TryParse(Storager.getString("PlayTimeInMatch", false), out savedPlayTimeInMatch))
		{
			form.AddField("playTimeInMatch", Mathf.RoundToInt(savedPlayTimeInMatch));
		}
		string killRatesString = Storager.getString("LastKillRates", false);
		List<object> killRateList = Json.Deserialize(killRatesString) as List<object>;
		if (killRateList != null && killRateList.Count == 2)
		{
			int[] kills = (from o in killRateList[0] as List<object>
			select Convert.ToInt32(o)).ToArray<int>();
			int[] deaths = (from o in killRateList[1] as List<object>
			select Convert.ToInt32(o)).ToArray<int>();
			int allKills = 0;
			int allDeath = 0;
			for (int i = 0; i < kills.Length; i++)
			{
				allKills += kills[i];
			}
			for (int j = 0; j < deaths.Length; j++)
			{
				allDeath += deaths[j];
			}
			form.AddField("kill_cnt_month", allKills);
			form.AddField("death_cnt_month", allDeath);
		}
		if (ProfileController.countGameTotalHit.Value != 0)
		{
			int _accuracy = Mathf.RoundToInt(100f * (float)ProfileController.countGameTotalHit.Value / (float)ProfileController.countGameTotalShoot.Value);
			form.AddField("accuracy", _accuracy);
		}
		if (Application.platform == RuntimePlatform.Android)
		{
			string advertisingId = AndroidSystem.Instance.GetAdvertisingId();
			if (Defs.IsDeveloperBuild)
			{
				Debug.Log("Android advertising id: " + advertisingId);
			}
			form.AddField("ad_id", advertisingId);
		}
		else if (Application.platform == RuntimePlatform.IPhonePlayer)
		{
			string advertisingId2 = string.Empty;
			if (Defs.IsDeveloperBuild)
			{
				Debug.Log("iOS advertising id: " + advertisingId2);
			}
			form.AddField("ad_id", advertisingId2);
		}
		form.AddField("accessories", this.GetAccesoriesString());
		Dictionary<string, string> scoresdictOne = new Dictionary<string, string>();
		scoresdictOne.Add("game", "0");
		scoresdictOne.Add("max_score", this.survivalScore.ToString());
		Dictionary<string, string> scoresdictTwo = new Dictionary<string, string>();
		scoresdictTwo.Add("game", "1");
		scoresdictTwo.Add("max_score", this.coopScore.ToString());
		Dictionary<string, string> scoresdictThree = new Dictionary<string, string>();
		scoresdictThree.Add("game", "2");
		scoresdictThree.Add("max_score", Storager.getInt("DaterDayLived", false).ToString());
		string serializedScores = Json.Serialize(new List<Dictionary<string, string>>
		{
			scoresdictOne,
			scoresdictTwo,
			scoresdictThree
		});
		form.AddField("scores", serializedScores);
		form.AddField("uniq_id", FriendsController.sharedController.id);
		form.AddField("auth", FriendsController.Hash("update_player", null));
		form.AddField("coins_bought", Storager.getInt(Defs.AllCurrencyBought + "Coins", false).ToString());
		form.AddField("gems_bought", Storager.getInt(Defs.AllCurrencyBought + "GemsCurrency", false).ToString());
		bool killRateStatisticsWasSent = false;
		try
		{
			bool shouldSendKillRate = true;
			string lastSendKillRateString = PlayerPrefs.GetString(Defs.LastSendKillRateTimeKey, string.Empty);
			DateTime lastSendKillRate;
			if (!string.IsNullOrEmpty(lastSendKillRateString) && DateTime.TryParse(lastSendKillRateString, out lastSendKillRate))
			{
				TimeSpan timeout = TimeSpan.FromHours(20.0);
				shouldSendKillRate = (DateTime.UtcNow - lastSendKillRate >= timeout);
			}
			if (shouldSendKillRate)
			{
				Dictionary<string, Dictionary<int, int>>.KeyCollection keysIntersection = KillRateStatisticsManager.WeWereKilledOld.Keys;
				Dictionary<string, Dictionary<int, int>> calculatedStatistics = new Dictionary<string, Dictionary<int, int>>();
				foreach (string weapon in keysIntersection)
				{
					Dictionary<int, int> weKill = (!KillRateStatisticsManager.WeKillOld.ContainsKey(weapon)) ? new Dictionary<int, int>() : KillRateStatisticsManager.WeKillOld[weapon];
					Dictionary<int, int> weWereKilled = KillRateStatisticsManager.WeWereKilledOld[weapon];
					if (weKill == null)
					{
						Debug.LogError("Exception adding kill_rate to update_player: weKill == null  " + weapon);
					}
					else if (weWereKilled == null)
					{
						Debug.LogError("Exception adding kill_rate to update_player: weWereKilled == null  " + weapon);
					}
					else
					{
						Dictionary<int, int>.KeyCollection tiersIntersecion = weWereKilled.Keys;
						Dictionary<int, int> calculatedInnerDictionary = new Dictionary<int, int>();
						foreach (int tier in tiersIntersecion)
						{
							int weWereKilledTier = weWereKilled[tier];
							if (weWereKilledTier == 0)
							{
								Debug.LogError("Exception adding kill_rate to update_player: weWereKilledTier == 0 " + weapon);
							}
							else
							{
								int result = ((!weKill.ContainsKey(tier)) ? 0 : weKill[tier]) * 1000 / weWereKilledTier;
								calculatedInnerDictionary.Add(tier, result);
							}
						}
						WeaponSounds weaponInfo = ItemDb.GetWeaponInfoByPrefabName(weapon);
						if (weaponInfo == null)
						{
							Debug.LogError("Exception adding kill_rate to update_player: weaponInfo == null  " + weapon);
						}
						else
						{
							string readableWeaponName = weaponInfo.shopNameNonLocalized;
							if (readableWeaponName == null)
							{
								Debug.LogError("Exception adding kill_rate to update_player: readableWeaponName == null  " + weapon);
							}
							else
							{
								string weaponInfoTag = null;
								try
								{
									weaponInfoTag = ItemDb.GetByPrefabName(weaponInfo.name.Replace("(Clone)", string.Empty)).Tag;
								}
								catch (Exception ex)
								{
									Exception e = ex;
									if (Application.isEditor)
									{
										Debug.LogWarning("Exception  weaponInfoTag = ItemDb.GetByPrefabName(weaponInfo.name.Replace(\"(Clone)\",\"\")).Tag:  " + e);
									}
								}
								if (weaponInfoTag == null)
								{
									Debug.LogError("Exception adding kill_rate to update_player: weaponInfo.tag == null  " + weapon);
								}
								else
								{
									if (weapon == WeaponManager.SocialGunWN || WeaponManager.GotchaGuns.Contains(weaponInfoTag))
									{
										readableWeaponName = readableWeaponName + "__DPS_TIER_" + (Storager.getInt("RememberedTierWhenObtainGun_" + weapon, false) + 1).ToString();
									}
									calculatedStatistics.Add(readableWeaponName, calculatedInnerDictionary);
								}
							}
						}
					}
				}
				if (calculatedStatistics.Count > 0)
				{
					Dictionary<string, object> dictToSent = new Dictionary<string, object>
					{
						{
							"version",
							GlobalGameController.AppVersion
						},
						{
							"kill_rate",
							calculatedStatistics
						}
					};
					string killRateJson = Json.Serialize(dictToSent);
					form.AddField("kill_rate", killRateJson);
					killRateStatisticsWasSent = true;
					if (Debug.isDebugBuild)
					{
						string modifyLog = string.Format("<color=white>kill_rate: {0}</color>", killRateJson);
						Debug.Log(modifyLog);
					}
				}
			}
		}
		catch (Exception ex2)
		{
			Exception e2 = ex2;
			Debug.LogError("Exception adding kill_rate to update_player: " + e2);
		}
		WWW updatePlayerRequest = Tools.CreateWww(FriendsController.actionAddress, form, string.Empty);
		yield return updatePlayerRequest;
		string response = URLs.Sanitize(updatePlayerRequest);
		if (string.IsNullOrEmpty(updatePlayerRequest.error) && !string.IsNullOrEmpty(response) && Debug.isDebugBuild)
		{
			Debug.Log("Update: " + response);
		}
		if (!string.IsNullOrEmpty(updatePlayerRequest.error))
		{
			if (Debug.isDebugBuild)
			{
				Debug.LogWarning("Update error: " + updatePlayerRequest.error);
			}
		}
		else if (!string.IsNullOrEmpty(response) && response.Equals("fail"))
		{
			if (Debug.isDebugBuild || Application.isEditor)
			{
				Debug.LogWarning("Update fail.");
			}
		}
		else if (killRateStatisticsWasSent)
		{
			PlayerPrefs.SetString(Defs.LastSendKillRateTimeKey, DateTime.UtcNow.ToString("s"));
		}
		yield break;
	}

	// Token: 0x060008EB RID: 2283 RVA: 0x00035E00 File Offset: 0x00034000
	private IEnumerator GetClanDataLoop()
	{
		for (;;)
		{
			while (this.idle || !SceneLoader.ActiveSceneName.Equals("Clans") || string.IsNullOrEmpty(this.ClanID))
			{
				yield return null;
			}
			base.StartCoroutine(this.GetClanDataOnce());
			yield return base.StartCoroutine(this.MyWaitForSeconds(20f));
		}
		yield break;
	}

	// Token: 0x060008EC RID: 2284 RVA: 0x00035E1C File Offset: 0x0003401C
	public IEnumerator MyWaitForSeconds(float tm)
	{
		float startTime = Time.realtimeSinceStartup;
		do
		{
			yield return null;
		}
		while (Time.realtimeSinceStartup - startTime < tm);
		yield break;
	}

	// Token: 0x060008ED RID: 2285 RVA: 0x00035E40 File Offset: 0x00034040
	private void TrySendEventShowBoxProcessFriendsData()
	{
		if (FriendsController.OnShowBoxProcessFriendsData == null)
		{
			return;
		}
		FriendsController.OnShowBoxProcessFriendsData();
	}

	// Token: 0x060008EE RID: 2286 RVA: 0x00035E58 File Offset: 0x00034058
	private void TrySendEventHideBoxProcessFriendsData()
	{
		if (FriendsController.OnHideBoxProcessFriendsData == null)
		{
			return;
		}
		FriendsController.OnHideBoxProcessFriendsData();
	}

	// Token: 0x060008EF RID: 2287 RVA: 0x00035E70 File Offset: 0x00034070
	private IEnumerator GetClanDataOnce()
	{
		if (FriendsController.readyToOperate)
		{
			WWWForm form = new WWWForm();
			form.AddField("action", "get_clan_info");
			form.AddField("app_version", ProtocolListGetter.CurrentPlatform + ":" + GlobalGameController.AppVersion);
			form.AddField("id_player", this.id);
			form.AddField("uniq_id", FriendsController.sharedController.id);
			form.AddField("auth", FriendsController.Hash("get_clan_info", null));
			WWW download = Tools.CreateWwwIfNotConnected(FriendsController.actionAddress, form, string.Empty, null);
			if (download == null)
			{
				yield break;
			}
			this.NumberOfClanInfoRequests++;
			try
			{
				yield return download;
			}
			finally
			{
				this.NumberOfClanInfoRequests--;
			}
			string response = URLs.Sanitize(download);
			int code;
			if (!string.IsNullOrEmpty(download.error))
			{
				if (Debug.isDebugBuild)
				{
					Debug.LogWarning("GetClanDataOnce error: " + download.error);
				}
			}
			else if (response.Equals("fail"))
			{
				if (Debug.isDebugBuild)
				{
					Debug.LogWarning("GetClanDataOnce fail.");
				}
			}
			else if (string.IsNullOrEmpty(response) || int.TryParse(response, out code))
			{
				this.ClearClanData();
			}
			else
			{
				this._UpdateClanMembers(response);
				if (FriendsController.ClanUpdated != null)
				{
					FriendsController.ClanUpdated();
				}
			}
		}
		yield break;
	}

	// Token: 0x060008F0 RID: 2288 RVA: 0x00035E8C File Offset: 0x0003408C
	public void ClearClanData()
	{
		this.ClanID = null;
		this.clanName = string.Empty;
		this.clanLogo = string.Empty;
		this.clanLeaderID = string.Empty;
		this.clanMembers.Clear();
		this.ClanSentInvites.Clear();
		this.clanSentInvitesLocal.Clear();
	}

	// Token: 0x060008F1 RID: 2289 RVA: 0x00035EE4 File Offset: 0x000340E4
	private void _UpdateClanMembers(string text)
	{
		object obj7 = Json.Deserialize(text);
		Dictionary<string, object> dictionary = obj7 as Dictionary<string, object>;
		if (dictionary == null)
		{
			if (Application.isEditor || Debug.isDebugBuild)
			{
				Debug.LogWarning(" _UpdateClanMembers dict = null");
			}
			return;
		}
		foreach (KeyValuePair<string, object> keyValuePair in dictionary)
		{
			string key = keyValuePair.Key;
			switch (key)
			{
			case "info":
			{
				Dictionary<string, object> dictionary2 = keyValuePair.Value as Dictionary<string, object>;
				if (dictionary2 != null)
				{
					object obj2;
					if (dictionary2.TryGetValue("name", out obj2))
					{
						this._prevClanName = this.clanName;
						this.clanName = (obj2 as string);
						if (!this._prevClanName.Equals(this.clanName) && this.onChangeClanName != null)
						{
							this.onChangeClanName(this.clanName);
						}
					}
					object obj3;
					if (dictionary2.TryGetValue("logo", out obj3))
					{
						this.clanLogo = (obj3 as string);
					}
					object obj4;
					if (dictionary2.TryGetValue("creator_id", out obj4))
					{
						this.clanLeaderID = (obj4 as string);
					}
				}
				break;
			}
			case "players":
			{
				List<object> list = keyValuePair.Value as List<object>;
				if (list != null)
				{
					this.clanMembers.Clear();
					foreach (object obj5 in list)
					{
						Dictionary<string, object> dictionary3 = (Dictionary<string, object>)obj5;
						Dictionary<string, string> dictionary4 = new Dictionary<string, string>();
						foreach (KeyValuePair<string, object> keyValuePair2 in dictionary3)
						{
							if (keyValuePair2.Value is string)
							{
								dictionary4.Add(keyValuePair2.Key, keyValuePair2.Value as string);
							}
						}
						this.clanMembers.Add(dictionary4);
					}
				}
				List<string> toRem__ = new List<string>();
				foreach (string text2 in this.clanDeletedLocal)
				{
					bool flag = false;
					foreach (Dictionary<string, string> dictionary5 in this.clanMembers)
					{
						if (dictionary5.ContainsKey("id") && dictionary5["id"].Equals(text2))
						{
							flag = true;
							break;
						}
					}
					if (!flag)
					{
						toRem__.Add(text2);
					}
				}
				this.clanDeletedLocal.RemoveAll((string obj) => toRem__.Contains(obj));
				break;
			}
			case "invites":
			{
				this.ClanSentInvites.Clear();
				List<object> list2 = keyValuePair.Value as List<object>;
				if (list2 != null)
				{
					foreach (object obj6 in list2)
					{
						string s = (string)obj6;
						int num2;
						if (int.TryParse(s, out num2) && !this.ClanSentInvites.Contains(num2.ToString()))
						{
							this.ClanSentInvites.Add(num2.ToString());
							this.clanSentInvitesLocal.Remove(num2.ToString());
						}
					}
				}
				break;
			}
			}
		}
		List<string> toRem = new List<string>();
		foreach (string item in this.clanCancelledInvitesLocal)
		{
			if (!this.ClanSentInvites.Contains(item))
			{
				toRem.Add(item);
			}
		}
		this.clanCancelledInvitesLocal.RemoveAll((string obj) => toRem.Contains(obj));
		if (FriendsController.ClanUpdated != null)
		{
			FriendsController.ClanUpdated();
		}
		FriendsController.ClanDataSettted = true;
		if (this.clanMembers.Count > 3 && this.clanLeaderID == this.id)
		{
			AnalyticsStuff.TrySendOnceToFacebook("create_clan_3", null, null);
		}
	}

	// Token: 0x060008F2 RID: 2290 RVA: 0x00036470 File Offset: 0x00034670
	private void _UpdateFriends(string text, bool requestAllInfo)
	{
		if (string.IsNullOrEmpty(text))
		{
			return;
		}
		this.invitesFromUs.Clear();
		this.invitesToUs.Clear();
		this.friends.Clear();
		this.ClanInvites.Clear();
		this.friendsDeletedLocal.Clear();
		object obj = Json.Deserialize(text);
		Dictionary<string, object> dictionary = obj as Dictionary<string, object>;
		if (dictionary == null)
		{
			if (Debug.isDebugBuild)
			{
				Debug.LogWarning(" _UpdateFriends dict = null");
			}
			return;
		}
		object obj2;
		if (!dictionary.TryGetValue("friends", out obj2))
		{
			Debug.LogWarning(" _UpdateFriends friendsObj!");
			return;
		}
		List<object> list = obj2 as List<object>;
		if (list == null)
		{
			if (Application.isEditor || Debug.isDebugBuild)
			{
				Debug.LogWarning(" _UpdateFriends __list = null");
			}
			return;
		}
		this._ProcessFriendsList(list, requestAllInfo);
		object obj3;
		if (!dictionary.TryGetValue("clans_invites", out obj3))
		{
			Debug.LogWarning(" _UpdateFriends clanInvObj!");
			return;
		}
		List<object> list2 = obj3 as List<object>;
		if (list2 == null)
		{
			if (Application.isEditor || Debug.isDebugBuild)
			{
				Debug.LogWarning(" _UpdateFriends clanInv = null");
			}
			return;
		}
		this._ProcessClanInvitesList(list2);
	}

	// Token: 0x060008F3 RID: 2291 RVA: 0x00036598 File Offset: 0x00034798
	private void _ProcessClanInvitesList(List<object> clanInv)
	{
		List<Dictionary<string, string>> list = new List<Dictionary<string, string>>();
		foreach (object obj in clanInv)
		{
			Dictionary<string, object> dictionary = (Dictionary<string, object>)obj;
			Dictionary<string, string> dictionary2 = new Dictionary<string, string>();
			foreach (KeyValuePair<string, object> keyValuePair in dictionary)
			{
				dictionary2.Add(keyValuePair.Key, keyValuePair.Value as string);
			}
			list.Add(dictionary2);
		}
		this.ClanInvites.Clear();
		this.ClanInvites = list;
	}

	// Token: 0x060008F4 RID: 2292 RVA: 0x00036684 File Offset: 0x00034884
	private void _ProcessFriendsList(List<object> __list, bool requestAllInfo)
	{
		List<Dictionary<string, string>> list = new List<Dictionary<string, string>>();
		foreach (object obj in __list)
		{
			Dictionary<string, object> dictionary = (Dictionary<string, object>)obj;
			Dictionary<string, string> dictionary2 = new Dictionary<string, string>();
			foreach (KeyValuePair<string, object> keyValuePair in dictionary)
			{
				dictionary2.Add(keyValuePair.Key, keyValuePair.Value as string);
			}
			list.Add(dictionary2);
		}
		foreach (Dictionary<string, string> dictionary3 in list)
		{
			Dictionary<string, string> dictionary4 = new Dictionary<string, string>();
			if (dictionary3["whom"].Equals(this.id) && dictionary3["status"].Equals("0"))
			{
				foreach (string text in dictionary3.Keys)
				{
					if (!text.Equals("whom") && !text.Equals("status"))
					{
						try
						{
							dictionary4.Add((!text.Equals("who")) ? text : "friend", dictionary3[text]);
						}
						catch
						{
						}
					}
				}
				this.invitesToUs.Add(dictionary4["friend"]);
				this.notShowAddIds.Remove(dictionary3["who"]);
			}
			if (dictionary3["status"].Equals("1"))
			{
				string text2 = (!dictionary3["who"].Equals(this.id)) ? "whom" : "who";
				string text3 = (!text2.Equals("who")) ? "who" : "whom";
				foreach (string text4 in dictionary3.Keys)
				{
					if (!text4.Equals(text2) && !text4.Equals("status"))
					{
						dictionary4.Add((!text4.Equals(text3)) ? text4 : "friend", dictionary3[text4]);
					}
				}
				this.friends.Add(dictionary4["friend"]);
				this.notShowAddIds.Remove(dictionary3[text3]);
			}
		}
		if (requestAllInfo)
		{
			this.UpdatePLayersInfo();
		}
		else
		{
			this._UpdatePlayersInfo();
		}
	}

	// Token: 0x060008F5 RID: 2293 RVA: 0x00036A2C File Offset: 0x00034C2C
	private void _UpdatePlayersInfo()
	{
		List<string> list = new List<string>();
		list.AddRange(this.friends);
		list.AddRange(this.invitesToUs);
		if (list.Count > 0)
		{
			base.StartCoroutine(this.GetInfoAboutNPlayers(list));
		}
	}

	// Token: 0x060008F6 RID: 2294 RVA: 0x00036A74 File Offset: 0x00034C74
	private IEnumerator GetInfoAboutNPlayers()
	{
		List<string> allFriends = new List<string>();
		allFriends.AddRange(this.friends);
		allFriends.AddRange(this.invitesToUs);
		if (allFriends.Count == 0)
		{
			yield break;
		}
		yield return base.StartCoroutine(this.GetInfoAboutNPlayers(allFriends));
		yield break;
	}

	// Token: 0x060008F7 RID: 2295 RVA: 0x00036A90 File Offset: 0x00034C90
	public void GetInfoAboutPlayers(List<string> ids)
	{
		base.StartCoroutine(this.GetInfoAboutNPlayers(ids));
	}

	// Token: 0x060008F8 RID: 2296 RVA: 0x00036AA0 File Offset: 0x00034CA0
	public IEnumerator GetInfoAboutNPlayers(List<string> ids)
	{
		string json = Json.Serialize(ids);
		if (json == null)
		{
			yield break;
		}
		WWWForm form = new WWWForm();
		form.AddField("action", "get_all_short_info_by_id");
		form.AddField("app_version", ProtocolListGetter.CurrentPlatform + ":" + GlobalGameController.AppVersion);
		form.AddField("ids", json);
		form.AddField("id", this.id);
		form.AddField("uniq_id", FriendsController.sharedController.id);
		form.AddField("auth", FriendsController.Hash("get_all_short_info_by_id", null));
		WWW download = Tools.CreateWwwIfNotConnected(FriendsController.actionAddress, form, string.Empty, null);
		if (download == null)
		{
			yield break;
		}
		yield return download;
		string response = URLs.Sanitize(download);
		this.TrySendEventHideBoxProcessFriendsData();
		if (!string.IsNullOrEmpty(download.error))
		{
			if (Debug.isDebugBuild)
			{
				Debug.LogWarning("GetInfoAboutNPlayers error: " + download.error);
			}
			yield break;
		}
		if (response.Equals("fail"))
		{
			if (Debug.isDebugBuild)
			{
				Debug.LogWarning("GetInfoAboutNPlayers fail.");
			}
			yield break;
		}
		Dictionary<string, object> __list = Json.Deserialize(response) as Dictionary<string, object>;
		if (__list == null)
		{
			if (Debug.isDebugBuild)
			{
				Debug.LogWarning(" GetInfoAboutNPlayers info = null");
			}
			yield break;
		}
		Dictionary<string, Dictionary<string, object>> list = new Dictionary<string, Dictionary<string, object>>();
		foreach (string key in __list.Keys)
		{
			Dictionary<string, object> d = __list[key] as Dictionary<string, object>;
			Dictionary<string, object> newd = new Dictionary<string, object>();
			foreach (KeyValuePair<string, object> kvp in d)
			{
				newd.Add(kvp.Key, kvp.Value);
			}
			list.Add(key, newd);
		}
		foreach (string key2 in list.Keys)
		{
			Dictionary<string, object> d2 = list[key2];
			bool _isAdd = false;
			if (this.friends.Contains(key2))
			{
				_isAdd = true;
				if (this.friendsInfo.ContainsKey(key2))
				{
					this.friendsInfo[key2] = d2;
				}
				else
				{
					this.friendsInfo.Add(key2, d2);
				}
			}
			if (!_isAdd)
			{
				if (this.profileInfo.ContainsKey(key2))
				{
					this.profileInfo[key2] = d2;
				}
				else
				{
					this.profileInfo.Add(key2, d2);
				}
				if (!FriendsController.sharedController.id.Equals(key2) && FindFriendsFromLocalLAN.lanPlayerInfo.Contains(key2) && !this.getPossibleFriendsResult.ContainsKey(key2))
				{
					this.getPossibleFriendsResult.Add(key2, FriendsController.PossiblleOrigin.Local);
				}
			}
			if (this.playersInfo.ContainsKey(key2))
			{
				this.playersInfo[key2] = d2;
			}
			else
			{
				this.playersInfo.Add(key2, d2);
			}
		}
		this.isUpdateInfoAboutAllFriends = false;
		if (FriendsController.FriendsUpdated != null)
		{
			FriendsController.FriendsUpdated();
		}
		this.SaveCurrentState();
		yield break;
	}

	// Token: 0x060008F9 RID: 2297 RVA: 0x00036ACC File Offset: 0x00034CCC
	public void UpdatePLayersInfo()
	{
		if (!FriendsController.readyToOperate)
		{
			return;
		}
		base.StartCoroutine(this.GetInfoAboutNPlayers());
	}

	// Token: 0x060008FA RID: 2298 RVA: 0x00036AE8 File Offset: 0x00034CE8
	public void StartRefreshingInfo(string playerId)
	{
		if (!FriendsController.readyToOperate)
		{
			return;
		}
		this._shouldStopRefreshingInfo = false;
		base.StartCoroutine(this.GetIfnoAboutPlayerLoop(playerId));
	}

	// Token: 0x060008FB RID: 2299 RVA: 0x00036B18 File Offset: 0x00034D18
	public void StopRefreshingInfo()
	{
		if (!FriendsController.readyToOperate)
		{
			return;
		}
		this._shouldStopRefreshingInfo = true;
	}

	// Token: 0x060008FC RID: 2300 RVA: 0x00036B2C File Offset: 0x00034D2C
	private IEnumerator GetIfnoAboutPlayerLoop(string playerId)
	{
		do
		{
			while (this.idle)
			{
				yield return null;
			}
			base.StartCoroutine(this.UpdatePlayerInfoById(playerId));
			float startTime = Time.realtimeSinceStartup;
			do
			{
				yield return null;
			}
			while (Time.realtimeSinceStartup - startTime < 20f && !this._shouldStopRefreshingInfo);
		}
		while (!this._shouldStopRefreshingInfo);
		yield break;
	}

	// Token: 0x170000F4 RID: 244
	// (get) Token: 0x060008FE RID: 2302 RVA: 0x00036B64 File Offset: 0x00034D64
	// (set) Token: 0x060008FD RID: 2301 RVA: 0x00036B58 File Offset: 0x00034D58
	public Dictionary<string, object> getInfoPlayerResult { get; private set; }

	// Token: 0x060008FF RID: 2303 RVA: 0x00036B6C File Offset: 0x00034D6C
	public IEnumerator GetInfoByIdCoroutine(string playerId)
	{
		this.getInfoPlayerResult = null;
		if (string.IsNullOrEmpty(playerId))
		{
			yield break;
		}
		WWWForm form = new WWWForm();
		form.AddField("action", "get_info_by_id");
		form.AddField("app_version", ProtocolListGetter.CurrentPlatform + ":" + GlobalGameController.AppVersion);
		form.AddField("id", playerId);
		form.AddField("uniq_id", FriendsController.sharedController.id);
		form.AddField("auth", FriendsController.Hash("get_info_by_id", null));
		WWW download = Tools.CreateWwwIfNotConnected(FriendsController.actionAddress, form, string.Empty, null);
		if (download == null)
		{
			yield break;
		}
		this.NumberOffFullInfoRequests++;
		try
		{
			yield return download;
		}
		finally
		{
			this.NumberOffFullInfoRequests--;
		}
		string response = URLs.Sanitize(download);
		if (string.IsNullOrEmpty(download.error) && !string.IsNullOrEmpty(response) && Debug.isDebugBuild)
		{
			Debug.Log("Info for id " + playerId + ": " + response);
		}
		if (!string.IsNullOrEmpty(download.error))
		{
			if (Debug.isDebugBuild)
			{
				Debug.LogWarning("GetInfoById error: " + download.error);
			}
			yield break;
		}
		if (response.Equals("fail"))
		{
			if (Debug.isDebugBuild)
			{
				Debug.LogWarning("GetInfoById fail.");
			}
			yield break;
		}
		Dictionary<string, object> responseData = this.ParseInfo(response);
		if (responseData == null)
		{
			if (Debug.isDebugBuild || Application.isEditor)
			{
				Debug.LogWarning(" GetInfoById newInfo = null");
			}
			yield break;
		}
		this.getInfoPlayerResult = responseData;
		yield break;
	}

	// Token: 0x170000F5 RID: 245
	// (get) Token: 0x06000901 RID: 2305 RVA: 0x00036BA4 File Offset: 0x00034DA4
	// (set) Token: 0x06000900 RID: 2304 RVA: 0x00036B98 File Offset: 0x00034D98
	public List<string> findPlayersByParamResult { get; private set; }

	// Token: 0x06000902 RID: 2306 RVA: 0x00036BAC File Offset: 0x00034DAC
	public IEnumerator GetInfoByParamCoroutine(string param)
	{
		this.findPlayersByParamResult = null;
		if (string.IsNullOrEmpty(param))
		{
			yield break;
		}
		WWWForm form = new WWWForm();
		form.AddField("action", "get_users_info_by_param");
		form.AddField("app_version", ProtocolListGetter.CurrentPlatform + ":" + GlobalGameController.AppVersion);
		form.AddField("param", param);
		form.AddField("uniq_id", FriendsController.sharedController.id);
		form.AddField("auth", FriendsController.Hash("get_users_info_by_param", null));
		WWW download = Tools.CreateWwwIfNotConnected(FriendsController.actionAddress, form, string.Empty, null);
		if (download == null)
		{
			yield break;
		}
		this.TrySendEventShowBoxProcessFriendsData();
		yield return download;
		this.TrySendEventHideBoxProcessFriendsData();
		string response = URLs.Sanitize(download);
		if (!string.IsNullOrEmpty(download.error))
		{
			if (Debug.isDebugBuild)
			{
				Debug.LogWarning("GetInfoById error: " + download.error);
			}
			yield break;
		}
		if (response.Equals("fail"))
		{
			if (Debug.isDebugBuild)
			{
				Debug.LogWarning("GetInfoById fail.");
			}
			yield break;
		}
		if (!string.IsNullOrEmpty(download.error) || string.IsNullOrEmpty(response) || Debug.isDebugBuild)
		{
		}
		List<object> responseData = Json.Deserialize(response) as List<object>;
		if (responseData == null)
		{
			if (Debug.isDebugBuild || Application.isEditor)
			{
				Debug.LogWarning(" GetInfoByParam newInfo = null");
			}
			yield break;
		}
		if (responseData != null && responseData.Count > 0)
		{
			this.findPlayersByParamResult = new List<string>();
			foreach (object _playerInfoObj in responseData)
			{
				Dictionary<string, object> _playerInfoDict = _playerInfoObj as Dictionary<string, object>;
				string _id = Convert.ToString(_playerInfoDict["id"]);
				this.findPlayersByParamResult.Add(_id);
				if (this.profileInfo.ContainsKey(_id))
				{
					this.profileInfo[_id]["player"] = _playerInfoDict;
				}
				else
				{
					Dictionary<string, object> _infoPlayer = new Dictionary<string, object>();
					_infoPlayer.Add("player", _playerInfoDict);
					this.profileInfo.Add(_id, _infoPlayer);
				}
			}
		}
		yield break;
	}

	// Token: 0x06000903 RID: 2307 RVA: 0x00036BD8 File Offset: 0x00034DD8
	private IEnumerator UpdatePlayerInfoById(string playerId)
	{
		yield return base.StartCoroutine(this.GetInfoByIdCoroutine(playerId));
		if (this.getInfoPlayerResult == null)
		{
			yield break;
		}
		this.playersInfo[playerId] = this.getInfoPlayerResult;
		bool _addInfo = false;
		if (this.friends.Contains(playerId))
		{
			_addInfo = true;
			if (this.friendsInfo.ContainsKey(playerId))
			{
				this.friendsInfo[playerId] = this.getInfoPlayerResult;
			}
			else
			{
				this.friendsInfo.Add(playerId, this.getInfoPlayerResult);
			}
		}
		if (this.clanFriendsInfo.ContainsKey(playerId))
		{
			this.clanFriendsInfo[playerId] = this.getInfoPlayerResult;
			_addInfo = true;
		}
		if (!_addInfo)
		{
			if (this.profileInfo.ContainsKey(playerId))
			{
				this.profileInfo[playerId] = this.getInfoPlayerResult;
			}
			else
			{
				this.profileInfo.Add(playerId, this.getInfoPlayerResult);
			}
		}
		if (this.playersInfo.ContainsKey(playerId))
		{
			this.playersInfo[playerId] = this.getInfoPlayerResult;
		}
		else
		{
			this.playersInfo.Add(playerId, this.getInfoPlayerResult);
		}
		if (FriendsController.FullInfoUpdated != null)
		{
			FriendsController.FullInfoUpdated();
		}
		yield break;
	}

	// Token: 0x06000904 RID: 2308 RVA: 0x00036C04 File Offset: 0x00034E04
	private Dictionary<string, object> ParseInfo(string info)
	{
		return Json.Deserialize(info) as Dictionary<string, object>;
	}

	// Token: 0x06000905 RID: 2309 RVA: 0x00036C20 File Offset: 0x00034E20
	public IEnumerator FriendRequest(string personId, Dictionary<string, object> socialEventParameters, Action<bool, bool> callbackAnswer = null)
	{
		if (socialEventParameters == null)
		{
			throw new ArgumentNullException("socialEventParameters");
		}
		WWWForm form = new WWWForm();
		form.AddField("action", "friend_request");
		form.AddField("app_version", ProtocolListGetter.CurrentPlatform + ":" + GlobalGameController.AppVersion);
		form.AddField("id", this.id);
		form.AddField("whom", personId);
		form.AddField("type", 0);
		form.AddField("uniq_id", FriendsController.sharedController.id);
		form.AddField("auth", FriendsController.Hash("friend_request", null));
		WWW friendRequest = Tools.CreateWww(FriendsController.actionAddress, form, string.Empty);
		this.TrySendEventShowBoxProcessFriendsData();
		yield return friendRequest;
		string response = URLs.Sanitize(friendRequest);
		this.TrySendEventHideBoxProcessFriendsData();
		if (string.IsNullOrEmpty(friendRequest.error) && !string.IsNullOrEmpty(response) && Debug.isDebugBuild)
		{
			Debug.Log("Friend request: " + response);
		}
		bool isCallbackAnswerRecive = false;
		if (!string.IsNullOrEmpty(friendRequest.error))
		{
			if (Debug.isDebugBuild)
			{
				Debug.LogWarning("FriendRequest error: " + friendRequest.error);
			}
			if (callbackAnswer != null)
			{
				callbackAnswer(false, false);
				isCallbackAnswerRecive = true;
			}
		}
		else if (response.Equals("fail"))
		{
			if (Debug.isDebugBuild)
			{
				Debug.LogWarning("FriendRequest fail.");
			}
			if (callbackAnswer != null)
			{
				callbackAnswer(false, false);
				isCallbackAnswerRecive = true;
			}
		}
		if (response.Equals("ok"))
		{
			TutorialQuestManager.Instance.AddFulfilledQuest("addFriend");
			QuestMediator.NotifySocialInteraction("addFriend");
			if (this.invitesToUs.Contains(personId))
			{
				this.invitesToUs.Remove(personId);
				this.friends.Add(personId);
			}
			else
			{
				this.invitesFromUs.Add(personId);
			}
			AnalyticsFacade.SendCustomEvent("Social", socialEventParameters);
		}
		if (callbackAnswer != null && !isCallbackAnswerRecive)
		{
			callbackAnswer(true, response.Equals("exist"));
		}
		yield break;
	}

	// Token: 0x06000906 RID: 2310 RVA: 0x00036C68 File Offset: 0x00034E68
	private IEnumerator _SendCreateClan(string personId, string nameClan, string skinClan, Action<string> ErrorHandler)
	{
		WWWForm form = new WWWForm();
		form.AddField("action", "create_clan");
		form.AddField("app_version", ProtocolListGetter.CurrentPlatform + ":" + GlobalGameController.AppVersion);
		form.AddField("id", personId);
		string filteredNick = nameClan;
		filteredNick = FilterBadWorld.FilterString(nameClan);
		form.AddField("name", filteredNick);
		form.AddField("logo", skinClan);
		form.AddField("uniq_id", FriendsController.sharedController.id);
		form.AddField("auth", FriendsController.Hash("create_clan", null));
		WWW download = Tools.CreateWwwIfNotConnected(FriendsController.actionAddress, form, string.Empty, null);
		if (download == null)
		{
			if (ErrorHandler != null)
			{
				ErrorHandler("Request skipped.");
			}
			yield break;
		}
		this.NumberOfCreateClanRequests++;
		float tm = Time.realtimeSinceStartup;
		try
		{
			while (!download.isDone && string.IsNullOrEmpty(download.error) && Time.realtimeSinceStartup - tm < 25f)
			{
				yield return null;
			}
		}
		finally
		{
			this.NumberOfCreateClanRequests--;
		}
		bool timeout = !download.isDone && string.IsNullOrEmpty(download.error) && Time.realtimeSinceStartup - tm >= 25f;
		string response = (!timeout) ? URLs.Sanitize(download) : string.Empty;
		if (!timeout && string.IsNullOrEmpty(download.error) && !string.IsNullOrEmpty(response) && Debug.isDebugBuild)
		{
			Debug.Log("_SendCreateClan request: " + response);
		}
		if (timeout || !string.IsNullOrEmpty(download.error))
		{
			string errorMessage = (!timeout) ? download.error : "TIMEOUT";
			if (ErrorHandler != null)
			{
				ErrorHandler(errorMessage);
			}
			if (Debug.isDebugBuild)
			{
				Debug.LogWarning("_SendCreateClan error: " + errorMessage);
			}
			if (timeout)
			{
				download.Dispose();
				download = null;
			}
			yield break;
		}
		if ("fail".Equals(response))
		{
			if (ErrorHandler != null)
			{
				ErrorHandler("fail");
			}
			if (Debug.isDebugBuild)
			{
				Debug.LogWarning("_SendCreateClan fail.");
			}
			yield break;
		}
		int newClanID;
		if (int.TryParse(response, out newClanID))
		{
			if (newClanID != -1)
			{
				this.ClanID = newClanID.ToString();
			}
			if (this.ReturnNewIDClan != null)
			{
				this.ReturnNewIDClan(newClanID);
			}
		}
		yield break;
	}

	// Token: 0x06000907 RID: 2311 RVA: 0x00036CC0 File Offset: 0x00034EC0
	public void ExitClan(string who = null)
	{
		if (FriendsController.readyToOperate)
		{
			base.StartCoroutine(this._ExitClan(who));
		}
	}

	// Token: 0x06000908 RID: 2312 RVA: 0x00036CDC File Offset: 0x00034EDC
	private IEnumerator _ExitClan(string who)
	{
		WWWForm form = new WWWForm();
		form.AddField("action", "exit_clan");
		form.AddField("app_version", ProtocolListGetter.CurrentPlatform + ":" + GlobalGameController.AppVersion);
		form.AddField("id_player", who ?? this.id);
		form.AddField("id_clan", this.ClanID);
		form.AddField("id", this.id);
		form.AddField("uniq_id", FriendsController.sharedController.id);
		form.AddField("auth", FriendsController.Hash("exit_clan", null));
		WWW download = Tools.CreateWwwIfNotConnected(FriendsController.actionAddress, form, string.Empty, null);
		if (download == null)
		{
			yield break;
		}
		yield return download;
		string response = URLs.Sanitize(download);
		if (string.IsNullOrEmpty(download.error) && !string.IsNullOrEmpty(response) && Debug.isDebugBuild)
		{
			Debug.Log("_ExitClan: " + response);
		}
		if (!string.IsNullOrEmpty(download.error))
		{
			if (Debug.isDebugBuild)
			{
				Debug.LogWarning("_ExitClan error: " + download.error);
			}
		}
		else if ("fail".Equals(response) && Debug.isDebugBuild)
		{
			Debug.LogWarning("_ExitClan fail.");
		}
		yield break;
	}

	// Token: 0x06000909 RID: 2313 RVA: 0x00036D08 File Offset: 0x00034F08
	public void DeleteClan()
	{
		if (FriendsController.readyToOperate && this.ClanID != null)
		{
			base.StartCoroutine(this._DeleteClan());
		}
	}

	// Token: 0x0600090A RID: 2314 RVA: 0x00036D38 File Offset: 0x00034F38
	private IEnumerator _DeleteClan()
	{
		WWWForm form = new WWWForm();
		form.AddField("action", "delete_clan");
		form.AddField("app_version", ProtocolListGetter.CurrentPlatform + ":" + GlobalGameController.AppVersion);
		form.AddField("id_clan", this.ClanID);
		form.AddField("id", this.id);
		form.AddField("uniq_id", FriendsController.sharedController.id);
		form.AddField("auth", FriendsController.Hash("delete_clan", null));
		WWW download = Tools.CreateWwwIfNotConnected(FriendsController.actionAddress, form, string.Empty, null);
		if (download == null)
		{
			yield break;
		}
		yield return download;
		string response = URLs.Sanitize(download);
		if (string.IsNullOrEmpty(download.error) && !string.IsNullOrEmpty(response) && Debug.isDebugBuild)
		{
			Debug.Log("_DeleteClan: " + response);
		}
		if (!string.IsNullOrEmpty(download.error))
		{
			if (Debug.isDebugBuild)
			{
				Debug.LogWarning("_DeleteClan error: " + download.error);
			}
		}
		else if ("fail".Equals(response) && Debug.isDebugBuild)
		{
			Debug.LogWarning("_DeleteClan fail.");
		}
		yield break;
	}

	// Token: 0x0600090B RID: 2315 RVA: 0x00036D54 File Offset: 0x00034F54
	private IEnumerator SendClanInvitation(string personID, Action<bool, bool> callbackResult = null)
	{
		WWWForm form = new WWWForm();
		form.AddField("action", "invite_to_clan");
		form.AddField("app_version", ProtocolListGetter.CurrentPlatform + ":" + GlobalGameController.AppVersion);
		form.AddField("id_player", personID);
		form.AddField("id_clan", this.ClanID);
		form.AddField("id", this.id);
		form.AddField("uniq_id", FriendsController.sharedController.id);
		form.AddField("auth", FriendsController.Hash("invite_to_clan", null));
		WWW download = Tools.CreateWwwIfNotConnected(FriendsController.actionAddress, form, string.Empty, null);
		if (download == null)
		{
			if (callbackResult != null)
			{
				callbackResult(false, false);
			}
			this.clanSentInvitesLocal.Remove(personID);
			yield break;
		}
		yield return download;
		string response = URLs.Sanitize(download);
		if (string.IsNullOrEmpty(download.error) && !string.IsNullOrEmpty(response) && Debug.isDebugBuild)
		{
			Debug.Log("SendClanInvitation: " + response);
		}
		bool isCallbackCall = false;
		if (!string.IsNullOrEmpty(download.error))
		{
			if (Debug.isDebugBuild)
			{
				Debug.LogWarning("SendClanInvitation error: " + download.error);
			}
			if (callbackResult != null)
			{
				isCallbackCall = true;
				callbackResult(false, false);
			}
			this.clanSentInvitesLocal.Remove(personID);
		}
		else if ("fail".Equals(response))
		{
			if (Debug.isDebugBuild)
			{
				Debug.LogWarning("SendClanInvitation fail.");
			}
			if (callbackResult != null)
			{
				isCallbackCall = true;
				callbackResult(false, false);
			}
			this.clanSentInvitesLocal.Remove(personID);
		}
		if (response.Equals("ok") && !this.ClanSentInvites.Contains(personID))
		{
			this.ClanSentInvites.Add(personID);
		}
		if (callbackResult != null && !isCallbackCall)
		{
			callbackResult(true, response.Equals("exist"));
		}
		yield break;
	}

	// Token: 0x0600090C RID: 2316 RVA: 0x00036D8C File Offset: 0x00034F8C
	private IEnumerator AcceptFriend(string accepteeId, Action<bool> action = null)
	{
		WWWForm form = new WWWForm();
		form.AddField("action", "accept_friend");
		form.AddField("app_version", ProtocolListGetter.CurrentPlatform + ":" + GlobalGameController.AppVersion);
		form.AddField("player_id", this.id);
		form.AddField("acceptee_id", accepteeId);
		form.AddField("uniq_id", FriendsController.sharedController.id);
		form.AddField("auth", FriendsController.Hash("accept_friend", null));
		WWW acceptFriendRequest = Tools.CreateWww(FriendsController.actionAddress, form, string.Empty);
		this.TrySendEventShowBoxProcessFriendsData();
		yield return acceptFriendRequest;
		string response = URLs.Sanitize(acceptFriendRequest);
		this.TrySendEventHideBoxProcessFriendsData();
		if (!string.IsNullOrEmpty(acceptFriendRequest.error))
		{
			if (Debug.isDebugBuild)
			{
				Debug.LogWarning("AcceptFriend error: " + acceptFriendRequest.error);
			}
			if (action != null)
			{
				action(false);
			}
			yield break;
		}
		if ("fail".Equals(response))
		{
			if (Debug.isDebugBuild)
			{
				Debug.LogWarning("AcceptFriend fail.");
			}
			if (action != null)
			{
				action(false);
			}
			yield break;
		}
		if (string.IsNullOrEmpty(acceptFriendRequest.error) && !string.IsNullOrEmpty(response))
		{
			if (Debug.isDebugBuild)
			{
				Debug.Log("Accept friend: " + response);
			}
			if (this.invitesToUs.Contains(accepteeId))
			{
				this.invitesToUs.Remove(accepteeId);
				FriendsWindowController.Instance.UpdateCurrentFriendsArrayAndItems();
			}
			if (!this.friends.Contains(accepteeId))
			{
				this.friends.Add(accepteeId);
			}
			QuestMediator.NotifySocialInteraction("addFriend");
			if (action != null)
			{
				action(true);
			}
		}
		yield break;
	}

	// Token: 0x0600090D RID: 2317 RVA: 0x00036DC4 File Offset: 0x00034FC4
	public static void DeleteFriend(string rejecteeId, Action<bool> action = null)
	{
		if (FriendsController.sharedController == null)
		{
			return;
		}
		if (FriendsController.readyToOperate)
		{
			FriendsController.sharedController.StartCoroutine(FriendsController.sharedController.DeleteFriendCoroutine(rejecteeId, action));
		}
	}

	// Token: 0x0600090E RID: 2318 RVA: 0x00036E04 File Offset: 0x00035004
	private IEnumerator DeleteFriendCoroutine(string rejecteeId, Action<bool> action = null)
	{
		WWWForm form = new WWWForm();
		form.AddField("action", "reject_friendship");
		form.AddField("app_version", ProtocolListGetter.CurrentPlatform + ":" + GlobalGameController.AppVersion);
		form.AddField("rejectee_id", rejecteeId);
		form.AddField("uniq_id", FriendsController.sharedController.id);
		form.AddField("auth", FriendsController.Hash("reject_friendship", null));
		WWW rejectFriendshipRequest = Tools.CreateWww(FriendsController.actionAddress, form, string.Empty);
		yield return rejectFriendshipRequest;
		string response = URLs.Sanitize(rejectFriendshipRequest);
		if (!string.IsNullOrEmpty(rejectFriendshipRequest.error))
		{
			if (Debug.isDebugBuild)
			{
				Debug.LogWarning("Reject_friendship error: " + rejectFriendshipRequest.error);
			}
			if (action != null)
			{
				action(false);
			}
			yield break;
		}
		if ("fail".Equals(response))
		{
			if (Debug.isDebugBuild)
			{
				Debug.LogWarning("Reject_friendship fail.");
			}
			if (action != null)
			{
				action(false);
			}
			yield break;
		}
		if (string.IsNullOrEmpty(rejectFriendshipRequest.error) && !string.IsNullOrEmpty(response))
		{
			if (Debug.isDebugBuild)
			{
				Debug.Log("reject_friendship: " + response);
			}
			if (this.friends.Contains(rejecteeId))
			{
				this.friends.Remove(rejecteeId);
				FriendsWindowController.Instance.UpdateCurrentFriendsArrayAndItems();
			}
			if (action != null)
			{
				action(true);
			}
		}
		yield break;
	}

	// Token: 0x0600090F RID: 2319 RVA: 0x00036E3C File Offset: 0x0003503C
	public void RejectInvite(string rejecteeId, Action<bool> action = null)
	{
		if (FriendsController.readyToOperate)
		{
			base.StartCoroutine(this.RejectInviteFriendCoroutine(rejecteeId, action));
		}
	}

	// Token: 0x06000910 RID: 2320 RVA: 0x00036E58 File Offset: 0x00035058
	public void RejectClanInvite(string clanID, string playerID = null)
	{
		if (!string.IsNullOrEmpty(clanID) && FriendsController.readyToOperate)
		{
			base.StartCoroutine(this._RejectClanInvite(clanID, playerID));
		}
	}

	// Token: 0x06000911 RID: 2321 RVA: 0x00036E8C File Offset: 0x0003508C
	private IEnumerator RejectInviteFriendCoroutine(string rejecteeId, Action<bool> action = null)
	{
		WWWForm form = new WWWForm();
		form.AddField("action", "reject_invite_friendship");
		form.AddField("app_version", ProtocolListGetter.CurrentPlatform + ":" + GlobalGameController.AppVersion);
		form.AddField("rejectee_id", rejecteeId);
		form.AddField("uniq_id", FriendsController.sharedController.id);
		form.AddField("auth", FriendsController.Hash("reject_invite_friendship", null));
		WWW rejectInviteFriendshipRequest = Tools.CreateWww(FriendsController.actionAddress, form, string.Empty);
		this.TrySendEventShowBoxProcessFriendsData();
		yield return rejectInviteFriendshipRequest;
		this.TrySendEventHideBoxProcessFriendsData();
		string response = URLs.Sanitize(rejectInviteFriendshipRequest);
		if (!string.IsNullOrEmpty(rejectInviteFriendshipRequest.error))
		{
			if (Debug.isDebugBuild)
			{
				Debug.LogWarning("RejectFriend error: " + rejectInviteFriendshipRequest.error);
			}
			if (action != null)
			{
				action(false);
			}
			yield break;
		}
		if ("fail".Equals(response))
		{
			if (Debug.isDebugBuild)
			{
				Debug.LogWarning("RejectFriend fail.");
			}
			if (action != null)
			{
				action(false);
			}
			yield break;
		}
		if (string.IsNullOrEmpty(rejectInviteFriendshipRequest.error) && !string.IsNullOrEmpty(response))
		{
			if (Debug.isDebugBuild)
			{
				Debug.Log("Reject friend: " + response);
			}
			if (this.invitesToUs.Contains(rejecteeId))
			{
				this.invitesToUs.Remove(rejecteeId);
				FriendsWindowController.Instance.UpdateCurrentFriendsArrayAndItems();
			}
			if (action != null)
			{
				action(true);
			}
		}
		yield break;
	}

	// Token: 0x06000912 RID: 2322 RVA: 0x00036EC4 File Offset: 0x000350C4
	private IEnumerator _RejectClanInvite(string clanID, string playerID)
	{
		WWWForm form = new WWWForm();
		form.AddField("action", "reject_invite");
		form.AddField("app_version", ProtocolListGetter.CurrentPlatform + ":" + GlobalGameController.AppVersion);
		form.AddField("id_player", playerID ?? this.id);
		form.AddField("id_clan", clanID);
		form.AddField("id", this.id);
		form.AddField("uniq_id", FriendsController.sharedController.id);
		form.AddField("auth", FriendsController.Hash("reject_invite", null));
		WWW download = Tools.CreateWwwIfNotConnected(FriendsController.actionAddress, form, string.Empty, null);
		if (download == null)
		{
			this.clanCancelledInvitesLocal.Remove(playerID);
			yield break;
		}
		yield return download;
		string response = URLs.Sanitize(download);
		if (!string.IsNullOrEmpty(download.error))
		{
			if (Debug.isDebugBuild)
			{
				Debug.LogWarning("_RejectClanInvite error: " + download.error);
			}
			this.clanCancelledInvitesLocal.Remove(playerID);
		}
		else if ("fail".Equals(response))
		{
			if (Debug.isDebugBuild)
			{
				Debug.LogWarning("_RejectClanInvite fail.");
			}
			this.clanCancelledInvitesLocal.Remove(playerID);
		}
		if (string.IsNullOrEmpty(download.error) && !string.IsNullOrEmpty(response) && Debug.isDebugBuild)
		{
			Debug.Log("_RejectClanInvite: " + response);
		}
		yield break;
	}

	// Token: 0x06000913 RID: 2323 RVA: 0x00036EFC File Offset: 0x000350FC
	public void DeleteClanMember(string memebrID)
	{
		if (!string.IsNullOrEmpty(memebrID) && FriendsController.readyToOperate)
		{
			base.StartCoroutine(this._DeleteClanMember(memebrID));
		}
	}

	// Token: 0x06000914 RID: 2324 RVA: 0x00036F24 File Offset: 0x00035124
	private IEnumerator _DeleteClanMember(string memberID)
	{
		WWWForm form = new WWWForm();
		form.AddField("action", "delete_clan_member");
		form.AddField("app_version", ProtocolListGetter.CurrentPlatform + ":" + GlobalGameController.AppVersion);
		form.AddField("id_player", memberID);
		form.AddField("id_clan", this.ClanID);
		form.AddField("id", this.id);
		form.AddField("uniq_id", FriendsController.sharedController.id);
		form.AddField("auth", FriendsController.Hash("delete_clan_member", null));
		WWW download = Tools.CreateWwwIfNotConnected(FriendsController.actionAddress, form, string.Empty, null);
		if (download == null)
		{
			this.clanDeletedLocal.Remove(memberID);
			yield break;
		}
		yield return download;
		string response = URLs.Sanitize(download);
		if (!string.IsNullOrEmpty(download.error))
		{
			if (Debug.isDebugBuild)
			{
				Debug.LogWarning("_DeleteClanMember error: " + download.error);
			}
			this.clanDeletedLocal.Remove(memberID);
		}
		else if ("fail".Equals(response))
		{
			if (Debug.isDebugBuild)
			{
				Debug.LogWarning("_DeleteClanMember fail.");
			}
			this.clanDeletedLocal.Remove(memberID);
		}
		if (string.IsNullOrEmpty(download.error) && !string.IsNullOrEmpty(response) && Debug.isDebugBuild)
		{
			Debug.Log("_DeleteClanMember: " + response);
		}
		yield break;
	}

	// Token: 0x06000915 RID: 2325 RVA: 0x00036F50 File Offset: 0x00035150
	private void Update()
	{
		if (FriendsController.isUpdateServerTimeAfterRun)
		{
			FriendsController.tickForServerTime += Time.unscaledDeltaTime;
			if (FriendsController.tickForServerTime >= 1f)
			{
				FriendsController.localServerTime += 1L;
				FriendsController.tickForServerTime -= 1f;
			}
		}
		if (!this.firstUpdateAfterApplicationPause)
		{
			this.deltaTimeInGame += Time.unscaledDeltaTime;
		}
		else
		{
			this.firstUpdateAfterApplicationPause = false;
		}
		if (this.Banned == 1 && PhotonNetwork.connected)
		{
			PhotonNetwork.isMessageQueueRunning = true;
			PhotonNetwork.Disconnect();
		}
		if (Input.touchCount > 0)
		{
			if (Time.realtimeSinceStartup - this.lastTouchTm > 30f)
			{
				this.idle = true;
			}
		}
		else
		{
			this.lastTouchTm = Time.realtimeSinceStartup;
			this.idle = false;
		}
	}

	// Token: 0x170000F6 RID: 246
	// (get) Token: 0x06000916 RID: 2326 RVA: 0x0003702C File Offset: 0x0003522C
	public static bool HasFriends
	{
		get
		{
			string @string = PlayerPrefs.GetString("FriendsKey", "[]");
			return !string.IsNullOrEmpty(@string) && @string != "[]";
		}
	}

	// Token: 0x06000917 RID: 2327 RVA: 0x00037064 File Offset: 0x00035264
	private string GetJsonIdsFacebookFriends()
	{
		if (Defs.IsDeveloperBuild)
		{
			Debug.Log("Start GetJsonIdsFacebookFriends");
		}
		FacebookController facebookController = FacebookController.sharedController;
		if (facebookController == null)
		{
			return "[]";
		}
		if (facebookController.friendsList == null || facebookController.friendsList.Count == 0)
		{
			return "[]";
		}
		List<string> list = new List<string>();
		for (int i = 0; i < facebookController.friendsList.Count; i++)
		{
			Friend friend = facebookController.friendsList[i];
			list.Add(friend.id);
		}
		string text = Json.Serialize(list);
		if (Defs.IsDeveloperBuild)
		{
			Debug.Log("GetJsonIdsFacebookFriends: " + text);
		}
		return text;
	}

	// Token: 0x06000918 RID: 2328 RVA: 0x00037120 File Offset: 0x00035320
	private IEnumerator GetPossibleFriendsList(int playerLevel, int platformId, string clientVersion)
	{
		WWWForm wwwForm = new WWWForm();
		string requestName = "possible_friends_list";
		string appVersion = string.Format("{0}:{1}", ProtocolListGetter.CurrentPlatform, GlobalGameController.AppVersion);
		wwwForm.AddField("action", requestName);
		wwwForm.AddField("app_version", appVersion);
		wwwForm.AddField("uniq_id", this.id);
		wwwForm.AddField("auth", FriendsController.Hash(requestName, null));
		if (FindFriendsFromLocalLAN.lanPlayerInfo.Count > 0)
		{
			wwwForm.AddField("local_ids", Json.Serialize(FindFriendsFromLocalLAN.lanPlayerInfo));
		}
		string facebookFriendsJsonIds = this.GetJsonIdsFacebookFriends();
		wwwForm.AddField("ids", facebookFriendsJsonIds);
		wwwForm.AddField("rank", playerLevel.ToString());
		wwwForm.AddField("platform_id", platformId.ToString());
		wwwForm.AddField("version", clientVersion);
		WWW download = Tools.CreateWwwIfNotConnected(FriendsController.actionAddress, wwwForm, string.Empty, null);
		if (download == null)
		{
			yield break;
		}
		yield return download;
		string response = URLs.Sanitize(download);
		if (!string.IsNullOrEmpty(download.error))
		{
			if (Debug.isDebugBuild)
			{
				Debug.LogWarning("GetPossibleFriendsList error: " + download.error);
			}
			yield break;
		}
		if (response.Equals("fail"))
		{
			if (Debug.isDebugBuild)
			{
				Debug.LogWarning("GetPossibleFriendsList fail.");
			}
			yield break;
		}
		Dictionary<string, object> dataList = Json.Deserialize(response) as Dictionary<string, object>;
		if (dataList == null)
		{
			yield break;
		}
		this.getPossibleFriendsResult.Clear();
		if (dataList.ContainsKey("local_users"))
		{
			List<object> userList = dataList["local_users"] as List<object>;
			if (userList != null && userList.Count > 0)
			{
				foreach (Dictionary<string, object> dictListItem in userList.OfType<Dictionary<string, object>>())
				{
					Dictionary<string, object> clone = new Dictionary<string, object>();
					foreach (KeyValuePair<string, object> dictItem in dictListItem)
					{
						clone.Add(dictItem.Key, dictItem.Value);
					}
					string userId = Convert.ToString(clone["id"]);
					if (this.profileInfo.ContainsKey(userId))
					{
						Dictionary<string, object> _cache = this.profileInfo[userId]["player"] as Dictionary<string, object>;
						_cache["nick"] = clone["nick"];
						_cache["rank"] = clone["rank"];
						_cache["clan_logo"] = clone["clan_logo"];
						_cache["clan_name"] = clone["clan_name"];
						_cache["skin"] = clone["skin"];
						this.profileInfo[userId]["player"] = _cache;
					}
					else
					{
						Dictionary<string, object> _cache = new Dictionary<string, object>();
						_cache.Add("nick", clone["nick"]);
						_cache.Add("rank", clone["rank"]);
						_cache.Add("clan_logo", clone["clan_logo"]);
						_cache.Add("clan_name", clone["clan_name"]);
						_cache.Add("skin", clone["skin"]);
						Dictionary<string, object> _infoPlayer = new Dictionary<string, object>();
						_infoPlayer.Add("player", _cache);
						this.profileInfo.Add(userId, _infoPlayer);
					}
					if (!this.getPossibleFriendsResult.ContainsKey(userId) && !this.friends.Contains(userId))
					{
						this.getPossibleFriendsResult.Add(userId, FriendsController.PossiblleOrigin.Local);
					}
				}
			}
		}
		if (dataList.ContainsKey("facebook_users"))
		{
			List<object> facebookUsers = dataList["facebook_users"] as List<object>;
			if (facebookUsers != null && facebookUsers.Count > 0)
			{
				foreach (Dictionary<string, object> dictListItem2 in facebookUsers.OfType<Dictionary<string, object>>())
				{
					string userId2 = Convert.ToString(dictListItem2["id"]);
					if (!FriendsController.IsPlayerOurFriend(userId2))
					{
						Dictionary<string, object> clone2 = new Dictionary<string, object>();
						foreach (KeyValuePair<string, object> dictItem2 in dictListItem2)
						{
							clone2.Add(dictItem2.Key, dictItem2.Value);
						}
						if (this.profileInfo.ContainsKey(userId2))
						{
							Dictionary<string, object> _cache2 = this.profileInfo[userId2]["player"] as Dictionary<string, object>;
							_cache2["nick"] = clone2["nick"];
							_cache2["rank"] = clone2["rank"];
							_cache2["clan_logo"] = clone2["clan_logo"];
							_cache2["clan_name"] = clone2["clan_name"];
							_cache2["skin"] = clone2["skin"];
							this.profileInfo[userId2]["player"] = _cache2;
						}
						else
						{
							Dictionary<string, object> _cache2 = new Dictionary<string, object>();
							_cache2.Add("nick", clone2["nick"]);
							_cache2.Add("rank", clone2["rank"]);
							_cache2.Add("clan_logo", clone2["clan_logo"]);
							_cache2.Add("clan_name", clone2["clan_name"]);
							_cache2.Add("skin", clone2["skin"]);
							Dictionary<string, object> _infoPlayer2 = new Dictionary<string, object>();
							_infoPlayer2.Add("player", _cache2);
							this.profileInfo.Add(userId2, _infoPlayer2);
						}
						if (!this.getPossibleFriendsResult.ContainsKey(userId2) && !this.friends.Contains(userId2))
						{
							this.getPossibleFriendsResult.Add(userId2, FriendsController.PossiblleOrigin.Facebook);
						}
					}
				}
			}
		}
		if (dataList.ContainsKey("users"))
		{
			List<object> userList2 = dataList["users"] as List<object>;
			if (userList2 != null && userList2.Count > 0)
			{
				foreach (Dictionary<string, object> dictListItem3 in userList2.OfType<Dictionary<string, object>>())
				{
					Dictionary<string, object> clone3 = new Dictionary<string, object>();
					foreach (KeyValuePair<string, object> dictItem3 in dictListItem3)
					{
						clone3.Add(dictItem3.Key, dictItem3.Value);
					}
					string userId3 = Convert.ToString(clone3["id"]);
					if (this.profileInfo.ContainsKey(userId3))
					{
						Dictionary<string, object> _cache3 = this.profileInfo[userId3]["player"] as Dictionary<string, object>;
						_cache3["nick"] = clone3["nick"];
						_cache3["rank"] = clone3["rank"];
						_cache3["clan_logo"] = clone3["clan_logo"];
						_cache3["clan_name"] = clone3["clan_name"];
						_cache3["skin"] = clone3["skin"];
						this.profileInfo[userId3]["player"] = _cache3;
					}
					else
					{
						Dictionary<string, object> _cache3 = new Dictionary<string, object>();
						_cache3.Add("nick", clone3["nick"]);
						_cache3.Add("rank", clone3["rank"]);
						_cache3.Add("clan_logo", clone3["clan_logo"]);
						_cache3.Add("clan_name", clone3["clan_name"]);
						_cache3.Add("skin", clone3["skin"]);
						Dictionary<string, object> _infoPlayer3 = new Dictionary<string, object>();
						_infoPlayer3.Add("player", _cache3);
						this.profileInfo.Add(userId3, _infoPlayer3);
					}
					if (!this.getPossibleFriendsResult.ContainsKey(userId3) && !this.friends.Contains(userId3))
					{
						this.getPossibleFriendsResult.Add(userId3, FriendsController.PossiblleOrigin.RandomPlayer);
					}
				}
			}
		}
		if (FriendsController.FriendsUpdated != null)
		{
			FriendsController.FriendsUpdated();
		}
		yield break;
	}

	// Token: 0x06000919 RID: 2329 RVA: 0x00037168 File Offset: 0x00035368
	public void DownloadDataAboutPossibleFriends()
	{
		int currentLevel = ExperienceController.GetCurrentLevel();
		int myPlatformConnect = (int)ConnectSceneNGUIController.myPlatformConnect;
		string multiplayerProtocolVersion = GlobalGameController.MultiplayerProtocolVersion;
		base.StartCoroutine(this.GetPossibleFriendsList(currentLevel, myPlatformConnect, multiplayerProtocolVersion));
	}

	// Token: 0x0600091A RID: 2330 RVA: 0x00037198 File Offset: 0x00035398
	private IEnumerator ClearAllFriendsInvitesCoroutine()
	{
		WWWForm wwwForm = new WWWForm();
		string requestName = "delete_friend_invites";
		string appVersion = string.Format("{0}:{1}", ProtocolListGetter.CurrentPlatform, GlobalGameController.AppVersion);
		wwwForm.AddField("action", requestName);
		wwwForm.AddField("app_version", appVersion);
		wwwForm.AddField("uniq_id", this.id);
		wwwForm.AddField("auth", FriendsController.Hash(requestName, null));
		WWW download = Tools.CreateWwwIfNotConnected(FriendsController.actionAddress, wwwForm, string.Empty, null);
		if (download == null)
		{
			yield break;
		}
		this.TrySendEventShowBoxProcessFriendsData();
		yield return download;
		this.TrySendEventHideBoxProcessFriendsData();
		if (FriendsWindowController.Instance != null)
		{
			FriendsWindowController.Instance.statusBar.clearAllInviteButton.isEnabled = true;
		}
		string response = URLs.Sanitize(download);
		if (!string.IsNullOrEmpty(download.error))
		{
			if (Debug.isDebugBuild)
			{
				Debug.LogWarning("ClearAllFriendsInvites error: " + download.error);
			}
			yield break;
		}
		if (response.Equals("fail"))
		{
			if (Debug.isDebugBuild)
			{
				Debug.LogWarning("ClearAllFriendsInvites fail.");
			}
			yield break;
		}
		if (string.IsNullOrEmpty(download.error) && !string.IsNullOrEmpty(response))
		{
			if (Debug.isDebugBuild)
			{
				Debug.Log("ClearAllFriendsInvites: " + response);
			}
			if (this.invitesToUs != null)
			{
				this.invitesToUs.Clear();
			}
			if (FriendsController.FriendsUpdated != null)
			{
				FriendsController.FriendsUpdated();
			}
		}
		yield break;
	}

	// Token: 0x0600091B RID: 2331 RVA: 0x000371B4 File Offset: 0x000353B4
	public void GetFriendsData(bool _isUpdateInfoAboutAllFriends = false)
	{
		this.timerUpdateFriend = -1f;
		if (_isUpdateInfoAboutAllFriends)
		{
			this.isUpdateInfoAboutAllFriends = true;
		}
	}

	// Token: 0x0600091C RID: 2332 RVA: 0x000371D0 File Offset: 0x000353D0
	private IEnumerator SendGameTimeLoop()
	{
		float timerSendTimeGame = 30f;
		for (;;)
		{
			while (!FriendsController.readyToOperate || this.idle || string.IsNullOrEmpty(FriendsController.sharedController.id))
			{
				yield return null;
			}
			while (timerSendTimeGame > 0f)
			{
				if (PhotonNetwork.room == null)
				{
					timerSendTimeGame -= Time.unscaledDeltaTime;
				}
				else
				{
					timerSendTimeGame = 30f;
				}
				yield return null;
			}
			yield return base.StartCoroutine(this.SendGameTime());
			timerSendTimeGame = 30f;
		}
		yield break;
	}

	// Token: 0x0600091D RID: 2333 RVA: 0x000371EC File Offset: 0x000353EC
	private IEnumerator SendGameTime()
	{
		WWWForm form = new WWWForm();
		form.AddField("action", "update_game_time");
		form.AddField("auth", FriendsController.Hash("update_game_time", null));
		form.AddField("app_version", ProtocolListGetter.CurrentPlatform + ":" + GlobalGameController.AppVersion);
		form.AddField("uniq_id", FriendsController.sharedController.id);
		this.sendingTime = (float)Mathf.RoundToInt(this.deltaTimeInGame);
		form.AddField("game_time", Mathf.RoundToInt(this.sendingTime));
		form.AddField("tier", ExpController.OurTierForAnyPlace());
		form.AddField("platform", ProtocolListGetter.CurrentPlatform.ToString());
		if (Defs.abTestBalansCohort != Defs.ABTestCohortsType.NONE && Defs.abTestBalansCohort != Defs.ABTestCohortsType.SKIP)
		{
			form.AddField("cohortName", Defs.abTestBalansCohortName);
		}
		if (Defs.cohortABTestAdvert == Defs.ABTestCohortsType.A || Defs.cohortABTestAdvert == Defs.ABTestCohortsType.B)
		{
			string _currentConfigAdvertNameForEvent = ((Defs.cohortABTestAdvert != Defs.ABTestCohortsType.A) ? "AdvertB_" : "AdvertA_") + FriendsController.configNameABTestAdvert;
			form.AddField("cohort_ad", _currentConfigAdvertNameForEvent);
		}
		foreach (ABTestBase abtest in ABTestController.currentABTests)
		{
			if (abtest.cohort == ABTestController.ABTestCohortsType.A || abtest.cohort == ABTestController.ABTestCohortsType.B)
			{
				form.AddField(abtest.currentFolder, abtest.cohortName);
			}
		}
		if (Defs.IsDeveloperBuild)
		{
			Debug.Log("Send delta time: " + this.sendingTime.ToString());
		}
		WWW updateGameTimeRequest = Tools.CreateWww(FriendsController.actionAddress, form, string.Empty);
		yield return updateGameTimeRequest;
		string response = URLs.Sanitize(updateGameTimeRequest);
		if (!string.IsNullOrEmpty(updateGameTimeRequest.error))
		{
			if (Debug.isDebugBuild)
			{
				Debug.LogWarning("updateGameTimeRequest error: " + updateGameTimeRequest.error);
			}
			yield break;
		}
		if (!string.IsNullOrEmpty(response) && response.Equals("fail"))
		{
			if (Debug.isDebugBuild)
			{
				Debug.LogWarning("updateGameTimeRequest fail.");
			}
			yield break;
		}
		this.deltaTimeInGame -= this.sendingTime;
		if (!string.IsNullOrEmpty(response) && !response.Equals("ok"))
		{
			Dictionary<string, object> cacheResponse = Json.Deserialize(response) as Dictionary<string, object>;
			if (cacheResponse != null && cacheResponse.ContainsKey("fight_invites"))
			{
				this.ParseFightInvite(cacheResponse["fight_invites"] as List<object>);
			}
		}
		yield break;
	}

	// Token: 0x0600091E RID: 2334 RVA: 0x00037208 File Offset: 0x00035408
	private IEnumerator GetFriendsDataLoop()
	{
		for (;;)
		{
			while (!FriendsController.readyToOperate || this.idle || string.IsNullOrEmpty(FriendsController.sharedController.id) || !TrainingController.TrainingCompleted)
			{
				yield return null;
			}
			yield return base.StartCoroutine(this.UpdateFriendsInfo(this.isUpdateInfoAboutAllFriends));
			while (this.timerUpdateFriend > 0f)
			{
				if (FriendsWindowGUI.Instance != null && FriendsWindowGUI.Instance.InterfaceEnabled)
				{
					this.timerUpdateFriend -= Time.unscaledDeltaTime;
				}
				yield return null;
			}
			this.timerUpdateFriend = Defs.timeUpdateFriendInfo;
		}
		yield break;
	}

	// Token: 0x0600091F RID: 2335 RVA: 0x00037224 File Offset: 0x00035424
	private IEnumerator UpdateFriendsInfo(bool _isUpdateInfoAboutAllFriends)
	{
		WWWForm wwwForm = new WWWForm();
		string requestName = "update_friends_info";
		string appVersion = string.Format("{0}:{1}", ProtocolListGetter.CurrentPlatform, GlobalGameController.AppVersion);
		wwwForm.AddField("action", requestName);
		wwwForm.AddField("app_version", appVersion);
		wwwForm.AddField("uniq_id", this.id);
		wwwForm.AddField("auth", FriendsController.Hash(requestName, null));
		bool isFromFromFriendsScene = FriendsWindowGUI.Instance != null && FriendsWindowGUI.Instance.InterfaceEnabled;
		wwwForm.AddField("from_friends", (!isFromFromFriendsScene) ? 0 : 1);
		bool isGetOnlineFriends = FriendsWindowController.IsActiveFriendListTab();
		if (isGetOnlineFriends && this.friends.Count > 0)
		{
			string json = Json.Serialize(this.friends);
			if (json != null)
			{
				wwwForm.AddField("get_all_players_online", json);
			}
		}
		wwwForm.AddField("private_messages", ChatController.GetPrivateChatJsonForSend());
		WWW updateFriendsInfoRequest = Tools.CreateWww(FriendsController.actionAddress, wwwForm, "from_friends: " + isFromFromFriendsScene);
		yield return updateFriendsInfoRequest;
		string response = URLs.Sanitize(updateFriendsInfoRequest);
		this.invitesToUs.Clear();
		if (!string.IsNullOrEmpty(updateFriendsInfoRequest.error))
		{
			if (Debug.isDebugBuild)
			{
				Debug.LogWarning("update_frieds_info error: " + updateFriendsInfoRequest.error);
			}
			this.TrySendEventHideBoxProcessFriendsData();
			yield break;
		}
		if (response.Equals("fail"))
		{
			if (Debug.isDebugBuild)
			{
				Debug.LogWarning("update_frieds_info fail.");
			}
			this.TrySendEventHideBoxProcessFriendsData();
			yield break;
		}
		if (string.IsNullOrEmpty(updateFriendsInfoRequest.error) && !string.IsNullOrEmpty(response))
		{
			this.ParseUpdateFriendsInfoResponse(response, _isUpdateInfoAboutAllFriends);
			this.notShowAddIds.Clear();
			if (FriendsController.UpdateFriendsInfoAction != null)
			{
				FriendsController.UpdateFriendsInfoAction();
			}
		}
		yield break;
	}

	// Token: 0x06000920 RID: 2336 RVA: 0x00037250 File Offset: 0x00035450
	public void SendInviteFightToPlayer(string _idFriend)
	{
		base.StartCoroutine(this.SendInviteFightToPlayerCoroutine(_idFriend));
	}

	// Token: 0x06000921 RID: 2337 RVA: 0x00037260 File Offset: 0x00035460
	private IEnumerator SendInviteFightToPlayerCoroutine(string _idFriend)
	{
		string _nick = ProfileController.GetPlayerNameOrDefault();
		_nick = FilterBadWorld.FilterString(_nick);
		WWWForm form = new WWWForm();
		string appVersion = string.Format("{0}:{1}", ProtocolListGetter.CurrentPlatform, GlobalGameController.AppVersion);
		form.AddField("action", "send_fight_invite");
		form.AddField("app_version", appVersion);
		form.AddField("uniq_id", this.id ?? string.Empty);
		form.AddField("name", _nick ?? string.Empty);
		form.AddField("reciever_id", _idFriend ?? string.Empty);
		form.AddField("auth", FriendsController.Hash("send_fight_invite", null));
		if (Application.isEditor)
		{
			Debug.LogFormat("`HandleCallFriend to Action Server()`: `{0}`", new object[]
			{
				Encoding.UTF8.GetString(form.data, 0, form.data.Length)
			});
		}
		WWW request = Tools.CreateWww(FriendsController.actionAddress, form, string.Empty);
		yield return request;
		string response = URLs.Sanitize(request);
		if (!string.IsNullOrEmpty(request.error))
		{
			if (Debug.isDebugBuild)
			{
				Debug.LogWarning("send_fight_invite error: " + request.error);
			}
			yield break;
		}
		if (response.Equals("fail"))
		{
			if (Debug.isDebugBuild)
			{
				Debug.LogWarning("send_fight_invite fail.");
			}
			yield break;
		}
		if (Debug.isDebugBuild)
		{
			Debug.Log("send_fight_invite: " + response);
		}
		yield break;
	}

	// Token: 0x06000922 RID: 2338 RVA: 0x0003728C File Offset: 0x0003548C
	public void ParseFightInvite(List<object> _invites)
	{
		if (!Defs.isEnableLocalInviteFromFriends)
		{
			return;
		}
		for (int i = 0; i < _invites.Count; i++)
		{
			Dictionary<string, object> dictionary = _invites[i] as Dictionary<string, object>;
			if (dictionary.ContainsKey("id") && dictionary.ContainsKey("name"))
			{
				string friendId = dictionary["id"].ToString();
				string nickname = dictionary["name"].ToString();
				BattleInviteListener.Instance.NotifyBattleIncomingInvite(friendId, nickname);
			}
		}
	}

	// Token: 0x06000923 RID: 2339 RVA: 0x00037318 File Offset: 0x00035518
	private void ParseUpdateFriendsInfoResponse(string response, bool _isUpdateInfoAboutAllFriends)
	{
		Dictionary<string, object> dictionary = Json.Deserialize(response) as Dictionary<string, object>;
		List<string> list = new List<string>();
		HashSet<string> hashSet = new HashSet<string>(this.friends);
		HashSet<string> hashSet2 = new HashSet<string>(this.invitesToUs);
		if (dictionary.ContainsKey("friends"))
		{
			this.friends.Clear();
			List<object> list2 = dictionary["friends"] as List<object>;
			for (int i = 0; i < list2.Count; i++)
			{
				string text = list2[i] as string;
				if (this.getPossibleFriendsResult.ContainsKey(text))
				{
					this.getPossibleFriendsResult.Remove(text);
				}
				this.friends.Add(text);
				if ((!_isUpdateInfoAboutAllFriends && !this.friendsInfo.ContainsKey(text)) || _isUpdateInfoAboutAllFriends)
				{
					list.Add(text);
				}
			}
		}
		if (dictionary.ContainsKey("invites"))
		{
			this.invitesToUs.Clear();
			List<object> list3 = dictionary["invites"] as List<object>;
			for (int j = 0; j < list3.Count; j++)
			{
				string text2 = list3[j] as string;
				if (!this.friends.Contains(text2))
				{
					this.invitesToUs.Add(text2);
				}
				if ((!_isUpdateInfoAboutAllFriends && !this.friendsInfo.ContainsKey(text2) && !this.clanFriendsInfo.ContainsKey(text2) && !this.profileInfo.ContainsKey(text2)) || _isUpdateInfoAboutAllFriends)
				{
					list.Add(text2);
				}
			}
		}
		if (dictionary.ContainsKey("invites_outcoming"))
		{
			this.invitesFromUs.Clear();
			List<object> list4 = dictionary["invites_outcoming"] as List<object>;
			for (int k = 0; k < list4.Count; k++)
			{
				string item = list4[k] as string;
				if (!this.friends.Contains(item))
				{
					this.invitesFromUs.Add(item);
				}
			}
		}
		if (_isUpdateInfoAboutAllFriends)
		{
			List<string> list5 = new List<string>(this.friends);
			List<string> list6 = new List<string>();
			list5.AddRange(this.invitesToUs);
			foreach (KeyValuePair<string, Dictionary<string, object>> keyValuePair in this.friendsInfo)
			{
				if (!list5.Contains(keyValuePair.Key))
				{
					list6.Add(keyValuePair.Key);
				}
			}
			if (list6.Count > 0)
			{
				for (int l = 0; l < list6.Count; l++)
				{
					this.friendsInfo.Remove(list6[l]);
				}
				this.SaveCurrentState();
			}
		}
		if (dictionary.ContainsKey("onLines"))
		{
			string response2 = Json.Serialize(dictionary["onLines"]);
			this.ParseOnlinesResponse(response2);
		}
		if (list.Count > 0)
		{
			base.StartCoroutine(this.GetInfoAboutNPlayers(list));
		}
		else
		{
			bool flag = !hashSet.SetEquals(this.friends) || !hashSet2.SetEquals(this.invitesToUs);
			if (flag && FriendsController.FriendsUpdated != null)
			{
				FriendsController.FriendsUpdated();
			}
			this.TrySendEventHideBoxProcessFriendsData();
		}
		if (dictionary.ContainsKey("chat"))
		{
			string response3 = Json.Serialize(dictionary["chat"]);
			if (ChatController.sharedController != null)
			{
				ChatController.sharedController.ParseUpdateChatMessageResponse(response3);
			}
		}
		if (dictionary.ContainsKey("fight_invites"))
		{
			this.ParseFightInvite(dictionary["fight_invites"] as List<object>);
		}
	}

	// Token: 0x06000924 RID: 2340 RVA: 0x0003770C File Offset: 0x0003590C
	private void ParseOnlinesResponse(string response)
	{
		Dictionary<string, object> dictionary = Json.Deserialize(response) as Dictionary<string, object>;
		if (dictionary == null)
		{
			if (Debug.isDebugBuild || Application.isEditor)
			{
				Debug.LogWarning(" GetAllPlayersOnline info = null");
			}
			return;
		}
		Dictionary<string, Dictionary<string, string>> dictionary2 = new Dictionary<string, Dictionary<string, string>>();
		foreach (string key in dictionary.Keys)
		{
			Dictionary<string, object> dictionary3 = dictionary[key] as Dictionary<string, object>;
			Dictionary<string, string> dictionary4 = new Dictionary<string, string>();
			foreach (KeyValuePair<string, object> keyValuePair in dictionary3)
			{
				dictionary4.Add(keyValuePair.Key, keyValuePair.Value as string);
			}
			dictionary2.Add(key, dictionary4);
		}
		this.onlineInfo.Clear();
		foreach (string key2 in dictionary2.Keys)
		{
			Dictionary<string, string> dictionary5 = dictionary2[key2];
			int num = int.Parse(dictionary5["game_mode"]);
			int num2 = num - num / 10 * 10;
			if (num2 != 3 && num2 != 8)
			{
				if (!this.onlineInfo.ContainsKey(key2))
				{
					this.onlineInfo.Add(key2, dictionary5);
				}
				else
				{
					this.onlineInfo[key2] = dictionary5;
				}
			}
		}
	}

	// Token: 0x06000925 RID: 2341 RVA: 0x000378F4 File Offset: 0x00035AF4
	public void ClearAllFriendsInvites()
	{
		base.StartCoroutine(this.ClearAllFriendsInvitesCoroutine());
	}

	// Token: 0x06000926 RID: 2342 RVA: 0x00037904 File Offset: 0x00035B04
	public void UpdateRecordByFriendsJoinClick(string friendId)
	{
		if (this.clicksJoinByFriends.ContainsKey(friendId))
		{
			this.clicksJoinByFriends[friendId] = DateTime.UtcNow.ToString("s");
			return;
		}
		this.clicksJoinByFriends.Add(friendId, DateTime.UtcNow.ToString("s"));
	}

	// Token: 0x06000927 RID: 2343 RVA: 0x00037960 File Offset: 0x00035B60
	public DateTime GetDateLastClickJoinFriend(string friendId)
	{
		if (!this.clicksJoinByFriends.ContainsKey(friendId))
		{
			return DateTime.MaxValue;
		}
		DateTime dateTime;
		bool flag = DateTime.TryParse(this.clicksJoinByFriends[friendId], out dateTime);
		return (!flag) ? dateTime : DateTime.MaxValue;
	}

	// Token: 0x06000928 RID: 2344 RVA: 0x000379AC File Offset: 0x00035BAC
	private void ClearListClickJoinFriends()
	{
		this.clicksJoinByFriends.Clear();
		PlayerPrefs.SetString("CachedFriendsJoinClickList", string.Empty);
	}

	// Token: 0x06000929 RID: 2345 RVA: 0x000379C8 File Offset: 0x00035BC8
	private void UpdateCachedClickJoinListValue()
	{
		if (this.clicksJoinByFriends.Count == 0)
		{
			return;
		}
		string text = Json.Serialize(this.clicksJoinByFriends);
		PlayerPrefs.SetString("CachedFriendsJoinClickList", text ?? string.Empty);
	}

	// Token: 0x0600092A RID: 2346 RVA: 0x00037A0C File Offset: 0x00035C0C
	private void FillClickJoinFriendsListByCachedValue()
	{
		string @string = PlayerPrefs.GetString("CachedFriendsJoinClickList", string.Empty);
		if (string.IsNullOrEmpty(@string))
		{
			return;
		}
		Dictionary<string, object> dictionary = Json.Deserialize(@string) as Dictionary<string, object>;
		if (dictionary == null)
		{
			return;
		}
		foreach (KeyValuePair<string, object> keyValuePair in dictionary)
		{
			this.clicksJoinByFriends.Add(keyValuePair.Key, Convert.ToString(keyValuePair.Value));
		}
	}

	// Token: 0x0600092B RID: 2347 RVA: 0x00037AB4 File Offset: 0x00035CB4
	private void SyncClickJoinFriendsListWithListFriends()
	{
		if (this.clicksJoinByFriends.Count == 0)
		{
			return;
		}
		if (this.friends.Count == 0)
		{
			this.ClearListClickJoinFriends();
			return;
		}
		List<string> list = new List<string>();
		foreach (KeyValuePair<string, string> keyValuePair in this.clicksJoinByFriends)
		{
			if (!this.playersInfo.ContainsKey(keyValuePair.Key))
			{
				list.Add(keyValuePair.Key);
			}
		}
		if (list.Count == 0)
		{
			return;
		}
		for (int i = 0; i < list.Count; i++)
		{
			string key = list[i];
			this.clicksJoinByFriends.Remove(key);
		}
		this.UpdateCachedClickJoinListValue();
	}

	// Token: 0x0600092C RID: 2348 RVA: 0x00037BAC File Offset: 0x00035DAC
	public static FriendsController.ResultParseOnlineData ParseOnlineData(Dictionary<string, string> onlineData)
	{
		string gameModeString = onlineData["game_mode"];
		string protocolString = onlineData["protocol"];
		string mapIndex = string.Empty;
		if (onlineData.ContainsKey("map"))
		{
			mapIndex = onlineData["map"];
		}
		return FriendsController.ParseOnlineData(gameModeString, protocolString, mapIndex);
	}

	// Token: 0x0600092D RID: 2349 RVA: 0x00037BFC File Offset: 0x00035DFC
	private static FriendsController.ResultParseOnlineData ParseOnlineData(string gameModeString, string protocolString, string mapIndex)
	{
		int num = int.Parse(gameModeString);
		if (num > 99)
		{
			num /= 100;
		}
		else
		{
			num = -1;
		}
		int num2;
		if (!int.TryParse(gameModeString, out num2))
		{
			num2 = -1;
		}
		else
		{
			if (num2 > 99)
			{
				num2 -= num * 100;
			}
			num2 /= 10;
		}
		FriendsController.ResultParseOnlineData resultParseOnlineData = new FriendsController.ResultParseOnlineData();
		bool flag = num == -1 || num != (int)ConnectSceneNGUIController.myPlatformConnect;
		bool flag2 = num2 == -1 || ExpController.GetOurTier() != num2;
		bool flag3 = num == 3;
		int num3 = Convert.ToInt32(gameModeString);
		string multiplayerProtocolVersion = GlobalGameController.MultiplayerProtocolVersion;
		resultParseOnlineData.gameMode = gameModeString;
		resultParseOnlineData.mapIndex = mapIndex;
		bool flag4 = num3 == 6;
		SceneInfo infoScene = SceneInfoController.instance.GetInfoScene(int.Parse(mapIndex));
		bool flag5 = infoScene != null && infoScene.IsAvaliableForMode(TypeModeGame.Dater);
		if (flag4)
		{
			resultParseOnlineData.notConnectCondition = FriendsController.NotConnectCondition.InChat;
		}
		else if (!flag3 && flag && !flag5)
		{
			resultParseOnlineData.notConnectCondition = FriendsController.NotConnectCondition.platform;
		}
		else if (!flag3 && flag2 && !flag5)
		{
			resultParseOnlineData.notConnectCondition = FriendsController.NotConnectCondition.level;
		}
		else if (multiplayerProtocolVersion != protocolString)
		{
			resultParseOnlineData.notConnectCondition = FriendsController.NotConnectCondition.clientVersion;
		}
		else if (infoScene == null)
		{
			resultParseOnlineData.notConnectCondition = FriendsController.NotConnectCondition.map;
		}
		else
		{
			resultParseOnlineData.notConnectCondition = FriendsController.NotConnectCondition.None;
		}
		resultParseOnlineData.isPlayerInChat = flag4;
		return resultParseOnlineData;
	}

	// Token: 0x0600092E RID: 2350 RVA: 0x00037D78 File Offset: 0x00035F78
	private static void SendEmailWithMyId()
	{
		MailUrlBuilder mailUrlBuilder = new MailUrlBuilder();
		mailUrlBuilder.to = string.Empty;
		mailUrlBuilder.subject = LocalizationStore.Get("Key_1565");
		string text = (!(FriendsController.sharedController != null)) ? string.Empty : FriendsController.sharedController.id;
		string format = LocalizationStore.Get("Key_1508");
		mailUrlBuilder.body = string.Format(format, FriendsController.sharedController.id);
		Application.OpenURL(mailUrlBuilder.GetUrl());
	}

	// Token: 0x0600092F RID: 2351 RVA: 0x00037DF8 File Offset: 0x00035FF8
	public static void SendMyIdByEmail()
	{
		MainMenuController.DoMemoryConsumingTaskInEmptyScene(delegate
		{
			InfoWindowController.ShowDialogBox(LocalizationStore.Get("Key_1572"), new Action(FriendsController.SendEmailWithMyId), null);
		}, null);
	}

	// Token: 0x06000930 RID: 2352 RVA: 0x00037E20 File Offset: 0x00036020
	public static void CopyMyIdToClipboard()
	{
		FriendsController.CopyPlayerIdToClipboard(FriendsController.sharedController.id);
	}

	// Token: 0x06000931 RID: 2353 RVA: 0x00037E34 File Offset: 0x00036034
	public static void CopyPlayerIdToClipboard(string playerId)
	{
		UniPasteBoard.SetClipBoardString(playerId);
		InfoWindowController.ShowInfoBox(LocalizationStore.Get("Key_1618"));
	}

	// Token: 0x06000932 RID: 2354 RVA: 0x00037E4C File Offset: 0x0003604C
	public static void JoinToFriendRoom(string friendId)
	{
		if (FriendsController.sharedController == null)
		{
			return;
		}
		if (!FriendsController.sharedController.onlineInfo.ContainsKey(friendId))
		{
			return;
		}
		Dictionary<string, string> dictionary = FriendsController.sharedController.onlineInfo[friendId];
		int gameModeCode;
		int.TryParse(dictionary["game_mode"], out gameModeCode);
		string nameRoom = dictionary["room_name"];
		string text = dictionary["map"];
		JoinToFriendRoomController instance = JoinToFriendRoomController.Instance;
		SceneInfo infoScene = SceneInfoController.instance.GetInfoScene(int.Parse(text));
		if (infoScene != null && instance != null)
		{
			instance.ConnectToRoom(gameModeCode, nameRoom, text);
			FriendsController.sharedController.UpdateRecordByFriendsJoinClick(friendId);
		}
	}

	// Token: 0x06000933 RID: 2355 RVA: 0x00037F04 File Offset: 0x00036104
	public static bool IsPlayerOurFriend(string playerId)
	{
		return !(FriendsController.sharedController == null) && FriendsController.sharedController.friends.Contains(playerId);
	}

	// Token: 0x06000934 RID: 2356 RVA: 0x00037F34 File Offset: 0x00036134
	public static bool IsPlayerOurClanMember(string playerId)
	{
		if (FriendsController.sharedController == null)
		{
			return false;
		}
		for (int i = 0; i < FriendsController.sharedController.clanMembers.Count; i++)
		{
			Dictionary<string, string> dictionary = FriendsController.sharedController.clanMembers[i];
			if (dictionary["id"].Equals(playerId))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06000935 RID: 2357 RVA: 0x00037FA0 File Offset: 0x000361A0
	public static bool IsSelfClanLeader()
	{
		return !(FriendsController.sharedController == null) && !string.IsNullOrEmpty(FriendsController.sharedController.clanLeaderID) && FriendsController.sharedController.clanLeaderID.Equals(FriendsController.sharedController.id);
	}

	// Token: 0x06000936 RID: 2358 RVA: 0x00037FF0 File Offset: 0x000361F0
	public static void SendFriendshipRequest(string playerId, Dictionary<string, object> socialEventParameters, Action<bool, bool> callbackResult)
	{
		if (FriendsController.sharedController == null)
		{
			return;
		}
		if (socialEventParameters == null)
		{
			throw new ArgumentNullException("socialEventParameters");
		}
		FriendsController.sharedController.StartCoroutine(FriendsController.sharedController.FriendRequest(playerId, socialEventParameters, callbackResult));
	}

	// Token: 0x06000937 RID: 2359 RVA: 0x00038038 File Offset: 0x00036238
	public static Dictionary<string, object> GetFullPlayerDataById(string playerId)
	{
		if (FriendsController.sharedController == null)
		{
			return null;
		}
		Dictionary<string, object> result;
		bool flag = FriendsController.sharedController.friendsInfo.TryGetValue(playerId, out result);
		if (flag)
		{
			return result;
		}
		flag = FriendsController.sharedController.clanFriendsInfo.TryGetValue(playerId, out result);
		if (flag)
		{
			return result;
		}
		flag = FriendsController.sharedController.profileInfo.TryGetValue(playerId, out result);
		if (flag)
		{
			return result;
		}
		return null;
	}

	// Token: 0x06000938 RID: 2360 RVA: 0x000380AC File Offset: 0x000362AC
	public static bool IsFriendsMax()
	{
		return !(FriendsController.sharedController == null) && FriendsController.sharedController.friends.Count >= Defs.maxCountFriend;
	}

	// Token: 0x06000939 RID: 2361 RVA: 0x000380DC File Offset: 0x000362DC
	public static bool IsFriendsDataExist()
	{
		return !(FriendsController.sharedController == null) && FriendsController.sharedController.friends.Count > 0 && FriendsController.sharedController.friendsInfo.Count > 0;
	}

	// Token: 0x0600093A RID: 2362 RVA: 0x00038128 File Offset: 0x00036328
	public static bool IsFriendsOrLocalDataExist()
	{
		if (FriendsController.sharedController == null)
		{
			return false;
		}
		List<string> list = new List<string>();
		foreach (KeyValuePair<string, FriendsController.PossiblleOrigin> keyValuePair in FriendsController.sharedController.getPossibleFriendsResult)
		{
			if (FriendsController.sharedController.profileInfo.ContainsKey(keyValuePair.Key) && keyValuePair.Value == FriendsController.PossiblleOrigin.Local)
			{
				list.Add(keyValuePair.Key);
			}
		}
		return (FriendsController.sharedController.friends.Count > 0 && FriendsController.sharedController.friendsInfo.Count > 0) || list.Count > 0;
	}

	// Token: 0x0600093B RID: 2363 RVA: 0x00038210 File Offset: 0x00036410
	public static bool IsPossibleFriendsDataExist()
	{
		return !(FriendsController.sharedController == null) && FriendsController.sharedController.getPossibleFriendsResult.Count > 0 && FriendsController.sharedController.profileInfo.Count > 0;
	}

	// Token: 0x0600093C RID: 2364 RVA: 0x0003825C File Offset: 0x0003645C
	public static bool IsFriendInvitesDataExist()
	{
		return !(FriendsController.sharedController == null) && FriendsController.sharedController.invitesToUs.Count > 0 && (FriendsController.sharedController.clanFriendsInfo.Count > 0 || FriendsController.sharedController.profileInfo.Count > 0);
	}

	// Token: 0x0600093D RID: 2365 RVA: 0x000382C0 File Offset: 0x000364C0
	public static bool IsDataAboutFriendDownload(string playerId)
	{
		return !(FriendsController.sharedController == null) && (FriendsController.sharedController.friendsInfo.ContainsKey(playerId) || FriendsController.sharedController.clanFriendsInfo.ContainsKey(playerId) || FriendsController.sharedController.profileInfo.ContainsKey(playerId));
	}

	// Token: 0x0600093E RID: 2366 RVA: 0x00038328 File Offset: 0x00036528
	public static void ShowProfile(string id, ProfileWindowType type, Action<bool> onCloseEvent = null)
	{
		if (FriendsController._friendProfileController == null)
		{
			FriendsController._friendProfileController = new FriendProfileController(onCloseEvent);
		}
		FriendsController._friendProfileController.HandleProfileClickedCore(id, type, onCloseEvent);
	}

	// Token: 0x170000F7 RID: 247
	// (get) Token: 0x0600093F RID: 2367 RVA: 0x00038358 File Offset: 0x00036558
	public bool ProfileInterfaceActive
	{
		get
		{
			if (FriendsController._friendProfileController == null)
			{
				return false;
			}
			return FriendsController._friendProfileController.FriendProfileGo.Map((GameObject g) => g.activeInHierarchy);
		}
	}

	// Token: 0x06000940 RID: 2368 RVA: 0x000383A0 File Offset: 0x000365A0
	public static void DisposeProfile()
	{
		if (FriendsController._friendProfileController == null)
		{
			return;
		}
		FriendsController._friendProfileController.Dispose();
		FriendsController._friendProfileController = null;
	}

	// Token: 0x06000941 RID: 2369 RVA: 0x000383C0 File Offset: 0x000365C0
	public static bool IsMyPlayerId(string playerId)
	{
		return !(FriendsController.sharedController == null) && !string.IsNullOrEmpty(FriendsController.sharedController.id) && FriendsController.sharedController.id.Equals(playerId);
	}

	// Token: 0x06000942 RID: 2370 RVA: 0x00038408 File Offset: 0x00036608
	public static bool IsAlreadySendInvitePlayer(string playerId)
	{
		return !(FriendsController.sharedController == null) && FriendsController.sharedController.invitesFromUs.Contains(playerId);
	}

	// Token: 0x06000943 RID: 2371 RVA: 0x00038438 File Offset: 0x00036638
	public static FriendsController.PossiblleOrigin GetPossibleFriendFindOrigin(string playerId)
	{
		if (FriendsController.sharedController == null)
		{
			return FriendsController.PossiblleOrigin.None;
		}
		if (!FriendsController.sharedController.getPossibleFriendsResult.ContainsKey(playerId))
		{
			return FriendsController.PossiblleOrigin.None;
		}
		return FriendsController.sharedController.getPossibleFriendsResult[playerId];
	}

	// Token: 0x06000944 RID: 2372 RVA: 0x00038474 File Offset: 0x00036674
	public static bool IsAlreadySendClanInvitePlayer(string playerId)
	{
		return !(FriendsController.sharedController == null) && FriendsController.sharedController.ClanSentInvites.Contains(playerId);
	}

	// Token: 0x06000945 RID: 2373 RVA: 0x000384A4 File Offset: 0x000366A4
	public static bool IsMaxClanMembers()
	{
		return !(FriendsController.sharedController == null) && FriendsController.sharedController.clanMembers.Count >= Defs.maxMemberClanCount;
	}

	// Token: 0x06000946 RID: 2374 RVA: 0x000384D4 File Offset: 0x000366D4
	public static void StartSendReview()
	{
		if (FriendsController.sharedController != null)
		{
			FriendsController.sharedController.StopCoroutine("WaitReviewAndSend");
			FriendsController.sharedController.StartCoroutine("WaitReviewAndSend");
		}
	}

	// Token: 0x06000947 RID: 2375 RVA: 0x00038508 File Offset: 0x00036708
	private IEnumerator WaitReviewAndSend()
	{
		while (ReviewController.ExistReviewForSend)
		{
			yield return base.StartCoroutine(this.SendReviewForPlayerWithID(ReviewController.ReviewRating, ReviewController.ReviewMsg));
			yield return new WaitForSeconds(600f);
		}
		yield break;
	}

	// Token: 0x06000948 RID: 2376 RVA: 0x00038524 File Offset: 0x00036724
	public IEnumerator SendReviewForPlayerWithID(int rating, string msgReview)
	{
		if (ReviewController.isSending)
		{
			yield break;
		}
		ReviewController.isSending = true;
		string playerId = FriendsController.sharedController.id;
		if (string.IsNullOrEmpty(playerId))
		{
			ReviewController.isSending = false;
			yield break;
		}
		WWWForm form = new WWWForm();
		form.AddField("action", "set_feedback");
		form.AddField("text", msgReview);
		form.AddField("rating", rating);
		form.AddField("version", GlobalGameController.AppVersion);
		form.AddField("app_version", ProtocolListGetter.CurrentPlatform + ":" + GlobalGameController.AppVersion);
		form.AddField("platform", ProtocolListGetter.CurrentPlatform);
		form.AddField("device_model", SystemInfo.deviceModel);
		form.AddField("auth", FriendsController.Hash("set_feedback", null));
		form.AddField("uniq_id", playerId);
		WWW download = Tools.CreateWwwIfNotConnected(FriendsController.actionAddress, form, string.Empty, null);
		if (download == null)
		{
			ReviewController.isSending = false;
			yield break;
		}
		yield return download;
		string response = URLs.Sanitize(download);
		if (!string.IsNullOrEmpty(download.error))
		{
			ReviewController.isSending = false;
			Debug.LogFormat("Error send review: {0}", new object[]
			{
				download.error
			});
			yield break;
		}
		if (Debug.isDebugBuild)
		{
			Debug.Log("Review send for id " + playerId + ": " + response);
		}
		ReviewController.isSending = false;
		ReviewController.ExistReviewForSend = false;
		yield break;
	}

	// Token: 0x06000949 RID: 2377 RVA: 0x00038554 File Offset: 0x00036754
	public static void LogPromoTrafficForwarding(FriendsController.TypeTrafficForwardingLog type)
	{
		if (type == FriendsController.TypeTrafficForwardingLog.view && (DateTime.Now - FriendsController.timeSendTrafficForwarding).TotalMinutes < 60.0)
		{
			return;
		}
		if (type == FriendsController.TypeTrafficForwardingLog.view || type == FriendsController.TypeTrafficForwardingLog.newView)
		{
			FriendsController.timeSendTrafficForwarding = DateTime.Now;
		}
		if (FriendsController.sharedController != null)
		{
			FriendsController.sharedController.StartCoroutine(FriendsController.sharedController.SendPromoTrafficForwarding(type));
		}
	}

	// Token: 0x0600094A RID: 2378 RVA: 0x000385CC File Offset: 0x000367CC
	public IEnumerator SendPromoTrafficForwarding(FriendsController.TypeTrafficForwardingLog type)
	{
		while (!string.IsNullOrEmpty(this.id))
		{
			if (Application.isEditor)
			{
				Debug.Log("SendPromoTrafficForwarding:" + type.ToString());
			}
			WWWForm form = new WWWForm();
			form.AddField("action", "promo_pgw_stat_update");
			form.AddField("auth", FriendsController.Hash("promo_pgw_stat_update", null));
			form.AddField("app_version", ProtocolListGetter.CurrentPlatform + ":" + GlobalGameController.AppVersion);
			form.AddField("uniq_id", this.id);
			form.AddField("is_paying", Storager.getInt("PayingUser", true));
			form.AddField("platform", ProtocolListGetter.CurrentPlatform);
			if (type == FriendsController.TypeTrafficForwardingLog.click)
			{
				form.AddField("add_click", 1);
			}
			if (type == FriendsController.TypeTrafficForwardingLog.newView)
			{
				form.AddField("add_new_view", 1);
			}
			if (type == FriendsController.TypeTrafficForwardingLog.newView || type == FriendsController.TypeTrafficForwardingLog.view)
			{
				form.AddField("add_view", 1);
			}
			WWW download = Tools.CreateWwwIfNotConnected(FriendsController.actionAddress, form, string.Empty, null);
			if (download == null)
			{
				yield break;
			}
			yield return download;
			if (download != null && string.IsNullOrEmpty(download.error))
			{
				if (Application.isEditor)
				{
					string response = URLs.Sanitize(download);
					Debug.Log("SendPromoTrafficForwarding(" + type.ToString() + "):" + response);
				}
				yield break;
			}
			if (Application.isEditor && download != null && !string.IsNullOrEmpty(download.error))
			{
				Debug.LogWarning("Error send log promo_pgw_stat_update: " + download.error);
			}
			yield return new WaitForSeconds(600f);
		}
		yield break;
		yield break;
	}

	// Token: 0x170000F8 RID: 248
	// (get) Token: 0x0600094B RID: 2379 RVA: 0x000385F8 File Offset: 0x000367F8
	// (set) Token: 0x0600094C RID: 2380 RVA: 0x00038624 File Offset: 0x00036824
	public static string configNameABTestAdvert
	{
		get
		{
			if (!FriendsController._isConfigNameAdvertInit)
			{
				FriendsController._configNameABTestAdvert = PlayerPrefs.GetString("CNAdvert", "none");
				FriendsController._isConfigNameAdvertInit = true;
			}
			return FriendsController._configNameABTestAdvert;
		}
		set
		{
			FriendsController._isConfigNameAdvertInit = true;
			FriendsController._configNameABTestAdvert = value;
			PlayerPrefs.SetString("CNAdvert", FriendsController._configNameABTestAdvert);
		}
	}

	// Token: 0x0600094D RID: 2381 RVA: 0x00038644 File Offset: 0x00036844
	private IEnumerator GetABTestAdvertConfig()
	{
		while (!TrainingController.TrainingCompleted)
		{
			yield return null;
		}
		if (Defs.cohortABTestAdvert == Defs.ABTestCohortsType.SKIP)
		{
			yield break;
		}
		string responseText;
		for (;;)
		{
			WWW download = Tools.CreateWww(URLs.ABTestAdvertURL);
			if (download == null)
			{
				yield return base.StartCoroutine(this.MyWaitForSeconds(30f));
			}
			else
			{
				yield return download;
				if (!string.IsNullOrEmpty(download.error))
				{
					if (Debug.isDebugBuild || Application.isEditor)
					{
						Debug.LogWarning("GetABTestAdvertConfig error: " + download.error);
					}
					yield return base.StartCoroutine(this.MyWaitForSeconds(30f));
				}
				else
				{
					responseText = URLs.Sanitize(download);
					if (!string.IsNullOrEmpty(responseText))
					{
						break;
					}
				}
			}
		}
		Storager.setString("abTestAdvertConfigKey", responseText, false);
		FriendsController.ParseABTestAdvertConfig(false);
		yield break;
	}

	// Token: 0x0600094E RID: 2382 RVA: 0x00038660 File Offset: 0x00036860
	public static void ParseABTestAdvertConfig(bool isFromReset = false)
	{
		if (Defs.cohortABTestAdvert == Defs.ABTestCohortsType.SKIP)
		{
			FriendsController.isReadABTestAdvertConfig = true;
		}
		if (!Storager.hasKey("abTestAdvertConfigKey") || string.IsNullOrEmpty(Storager.getString("abTestAdvertConfigKey", false)))
		{
			return;
		}
		string @string = Storager.getString("abTestAdvertConfigKey", false);
		Dictionary<string, object> dictionary = Json.Deserialize(@string) as Dictionary<string, object>;
		if (dictionary != null && dictionary.ContainsKey("enableABTest"))
		{
			int num = Convert.ToInt32(dictionary["enableABTest"]);
			if (num == 1 && Defs.cohortABTestAdvert != Defs.ABTestCohortsType.SKIP)
			{
				FriendsController.configNameABTestAdvert = Convert.ToString(dictionary["configName"]);
				if (Defs.cohortABTestAdvert == Defs.ABTestCohortsType.NONE)
				{
					int cohortABTestAdvert = UnityEngine.Random.Range(1, 3);
					Defs.cohortABTestAdvert = (Defs.ABTestCohortsType)cohortABTestAdvert;
					string nameCohort = ((Defs.cohortABTestAdvert != Defs.ABTestCohortsType.A) ? "AdvertB_" : "AdvertA_") + FriendsController.configNameABTestAdvert;
					AnalyticsStuff.LogABTest("Advert", nameCohort, true);
					if (FriendsController.sharedController != null)
					{
						FriendsController.sharedController.SendOurData(false);
					}
				}
				if (Defs.cohortABTestAdvert == Defs.ABTestCohortsType.B)
				{
					Dictionary<string, object> dictionary2 = dictionary["settings-b"] as Dictionary<string, object>;
					FreeAwardController.appId = Convert.ToString(dictionary2["appId"]);
					FreeAwardController.securityToken = Convert.ToString(dictionary2["token"]);
					AdsConfigManager.configFromABTestAdvert = Json.Serialize(dictionary["config"]);
				}
				else
				{
					Dictionary<string, object> dictionary3 = dictionary["settings-a"] as Dictionary<string, object>;
					FreeAwardController.appId = Convert.ToString(dictionary3["appId"]);
					FreeAwardController.securityToken = Convert.ToString(dictionary3["token"]);
				}
			}
			else if (!isFromReset)
			{
				FriendsController.ResetABTestAdvert();
			}
		}
		FriendsController.isReadABTestAdvertConfig = true;
	}

	// Token: 0x0600094F RID: 2383 RVA: 0x00038828 File Offset: 0x00036A28
	public static void ResetABTestAdvert()
	{
		if (Defs.cohortABTestAdvert != Defs.ABTestCohortsType.SKIP)
		{
			if (Defs.cohortABTestAdvert == Defs.ABTestCohortsType.A || Defs.cohortABTestAdvert == Defs.ABTestCohortsType.B)
			{
				string nameCohort = ((Defs.cohortABTestAdvert != Defs.ABTestCohortsType.A) ? "AdvertB_" : "AdvertA_") + FriendsController.configNameABTestAdvert;
				AnalyticsStuff.LogABTest("Advert", nameCohort, false);
				if (FriendsController.sharedController != null)
				{
					FriendsController.sharedController.SendOurData(false);
				}
			}
			Defs.cohortABTestAdvert = Defs.ABTestCohortsType.SKIP;
			FriendsController.ParseABTestAdvertConfig(true);
			FreeAwardController.appId = "32897";
			FreeAwardController.securityToken = "cf77aeadd83faf98e0cad61a1f1403c8";
			AdsConfigManager.configFromABTestAdvert = string.Empty;
		}
		FriendsController.isReadABTestAdvertConfig = true;
	}

	// Token: 0x04000711 RID: 1809
	private const string FriendsKey = "FriendsKey";

	// Token: 0x04000712 RID: 1810
	private const string ToUsKey = "ToUsKey";

	// Token: 0x04000713 RID: 1811
	private const string PlayerInfoKey = "PlayerInfoKey";

	// Token: 0x04000714 RID: 1812
	private const string FriendsInfoKey = "FriendsInfoKey";

	// Token: 0x04000715 RID: 1813
	private const string ClanFriendsInfoKey = "ClanFriendsInfoKey";

	// Token: 0x04000716 RID: 1814
	private const string ClanInvitesKey = "ClanInvitesKey";

	// Token: 0x04000717 RID: 1815
	private const string PixelbookSettingsKey = "PixelbookSettingsKey";

	// Token: 0x04000718 RID: 1816
	public const string LobbyNewsKey = "LobbyNewsKey";

	// Token: 0x04000719 RID: 1817
	public const string LobbyIsAnyNewsKey = "LobbyIsAnyNewsKey";

	// Token: 0x0400071A RID: 1818
	public const string PixelFilterWordsKey = "PixelFilterWordsKey";

	// Token: 0x0400071B RID: 1819
	public const string PixelFilterSymbolsKey = "PixelFilterSymbolsKey";

	// Token: 0x0400071C RID: 1820
	public const float TimeUpdateFriendAndClanData = 20f;

	// Token: 0x0400071D RID: 1821
	public static bool isDebugLogWWW = true;

	// Token: 0x0400071E RID: 1822
	public int Banned = -1;

	// Token: 0x0400071F RID: 1823
	public static float onlineDelta = 60f;

	// Token: 0x04000720 RID: 1824
	public static Dictionary<string, Dictionary<string, string>> mapPopularityDictionary = new Dictionary<string, Dictionary<string, string>>();

	// Token: 0x04000721 RID: 1825
	public static bool readyToOperate = false;

	// Token: 0x04000722 RID: 1826
	public static FriendsController sharedController = null;

	// Token: 0x04000723 RID: 1827
	private string currentCompetitionKey = "currentCompetitionKey";

	// Token: 0x04000724 RID: 1828
	private int _currentCompetition = -1;

	// Token: 0x04000725 RID: 1829
	private float _expirationTimeCompetition = -1f;

	// Token: 0x04000726 RID: 1830
	private static bool _expirationTimeCompetitionInit = false;

	// Token: 0x04000727 RID: 1831
	private static string expirationTimeCompetitionKey = "expirationTimeCompetitionKey";

	// Token: 0x04000728 RID: 1832
	private static bool _advertEnabled = false;

	// Token: 0x04000729 RID: 1833
	private bool friendsReceivedOnce;

	// Token: 0x0400072A RID: 1834
	[SerializeField]
	[ReadOnly]
	private string _clanId;

	// Token: 0x0400072B RID: 1835
	public string clanLeaderID;

	// Token: 0x0400072C RID: 1836
	public string clanLogo;

	// Token: 0x0400072D RID: 1837
	public string clanName;

	// Token: 0x0400072E RID: 1838
	public int NumberOfFriendsRequests;

	// Token: 0x0400072F RID: 1839
	public int NumberOffFullInfoRequests;

	// Token: 0x04000730 RID: 1840
	public int NumberOfBestPlayersRequests;

	// Token: 0x04000731 RID: 1841
	public int NumberOfClanInfoRequests;

	// Token: 0x04000732 RID: 1842
	public int NumberOfCreateClanRequests;

	// Token: 0x04000733 RID: 1843
	private float lastTouchTm;

	// Token: 0x04000734 RID: 1844
	public bool idle;

	// Token: 0x04000735 RID: 1845
	private List<int> ids = new List<int>();

	// Token: 0x04000736 RID: 1846
	public List<string> friendsDeletedLocal = new List<string>();

	// Token: 0x04000737 RID: 1847
	public string JoinClanSent;

	// Token: 0x04000738 RID: 1848
	private string AccountCreated = "AccountCreated";

	// Token: 0x04000739 RID: 1849
	private string _id;

	// Token: 0x0400073A RID: 1850
	internal List<string> friends = new List<string>();

	// Token: 0x0400073B RID: 1851
	internal readonly List<Dictionary<string, string>> clanMembers = new List<Dictionary<string, string>>();

	// Token: 0x0400073C RID: 1852
	internal List<string> invitesFromUs = new List<string>();

	// Token: 0x0400073D RID: 1853
	internal List<string> invitesToUs = new List<string>();

	// Token: 0x0400073E RID: 1854
	internal List<Dictionary<string, string>> ClanInvites = new List<Dictionary<string, string>>();

	// Token: 0x0400073F RID: 1855
	internal readonly List<string> ClanSentInvites = new List<string>();

	// Token: 0x04000740 RID: 1856
	internal readonly List<string> clanSentInvitesLocal = new List<string>();

	// Token: 0x04000741 RID: 1857
	internal readonly List<string> clanCancelledInvitesLocal = new List<string>();

	// Token: 0x04000742 RID: 1858
	internal readonly List<string> clanDeletedLocal = new List<string>();

	// Token: 0x04000743 RID: 1859
	internal readonly Dictionary<string, Dictionary<string, object>> playersInfo = new Dictionary<string, Dictionary<string, object>>();

	// Token: 0x04000744 RID: 1860
	internal readonly Dictionary<string, Dictionary<string, object>> friendsInfo = new Dictionary<string, Dictionary<string, object>>();

	// Token: 0x04000745 RID: 1861
	internal readonly Dictionary<string, Dictionary<string, object>> clanFriendsInfo = new Dictionary<string, Dictionary<string, object>>();

	// Token: 0x04000746 RID: 1862
	internal readonly Dictionary<string, Dictionary<string, object>> profileInfo = new Dictionary<string, Dictionary<string, object>>();

	// Token: 0x04000747 RID: 1863
	internal readonly Dictionary<string, Dictionary<string, string>> onlineInfo = new Dictionary<string, Dictionary<string, string>>();

	// Token: 0x04000748 RID: 1864
	internal readonly List<string> notShowAddIds = new List<string>();

	// Token: 0x04000749 RID: 1865
	internal readonly Dictionary<string, Dictionary<string, object>> facebookFriendsInfo = new Dictionary<string, Dictionary<string, object>>();

	// Token: 0x0400074A RID: 1866
	public string alphaIvory;

	// Token: 0x0400074B RID: 1867
	private static HMAC _hmac;

	// Token: 0x0400074C RID: 1868
	public string nick;

	// Token: 0x0400074D RID: 1869
	public string skin;

	// Token: 0x0400074E RID: 1870
	public int rank;

	// Token: 0x0400074F RID: 1871
	public int coopScore;

	// Token: 0x04000750 RID: 1872
	public int survivalScore;

	// Token: 0x04000751 RID: 1873
	internal SaltedInt wins = new SaltedInt(641227346);

	// Token: 0x04000752 RID: 1874
	public Dictionary<string, object> ourInfo;

	// Token: 0x04000753 RID: 1875
	public string id_fb;

	// Token: 0x04000754 RID: 1876
	private float timerUpdatePixelbookSetting = 900f;

	// Token: 0x04000755 RID: 1877
	private static long localServerTime;

	// Token: 0x04000756 RID: 1878
	private static float tickForServerTime = 0f;

	// Token: 0x04000757 RID: 1879
	private static bool isUpdateServerTimeAfterRun;

	// Token: 0x04000758 RID: 1880
	private bool isGetServerTimeFromMainUrl = true;

	// Token: 0x04000759 RID: 1881
	public static bool isInitPixelbookSettingsFromServer = false;

	// Token: 0x0400075A RID: 1882
	private string FacebookIDKey = "FacebookIDKey";

	// Token: 0x0400075B RID: 1883
	public FriendsController.OnChangeClanName onChangeClanName;

	// Token: 0x0400075C RID: 1884
	private string _prevClanName;

	// Token: 0x0400075D RID: 1885
	public bool dataSent;

	// Token: 0x0400075E RID: 1886
	private bool infoLoaded;

	// Token: 0x0400075F RID: 1887
	public static float timeOutSendUpdatePlayerFromConnectScene = (!Defs.IsDeveloperBuild) ? 360f : 36f;

	// Token: 0x04000760 RID: 1888
	public string tempClanID;

	// Token: 0x04000761 RID: 1889
	public string tempClanLogo;

	// Token: 0x04000762 RID: 1890
	public string tempClanName;

	// Token: 0x04000763 RID: 1891
	public string tempClanCreatorID;

	// Token: 0x04000764 RID: 1892
	private bool _shouldStopOnline;

	// Token: 0x04000765 RID: 1893
	private bool _shouldStopOnlineWithClanInfo;

	// Token: 0x04000766 RID: 1894
	private bool _shouldStopRefrClanOnline;

	// Token: 0x04000767 RID: 1895
	public Action GetFacebookFriendsCallback;

	// Token: 0x04000768 RID: 1896
	private string _inputToken;

	// Token: 0x04000769 RID: 1897
	private KeyValuePair<string, int>? _winCountTimestamp;

	// Token: 0x0400076A RID: 1898
	private bool ReceivedLastOnline;

	// Token: 0x0400076B RID: 1899
	private bool getCohortInfo;

	// Token: 0x0400076C RID: 1900
	private float timeSendUpdatePlayer;

	// Token: 0x0400076D RID: 1901
	public float timerUpdateFriend = 20f;

	// Token: 0x0400076E RID: 1902
	public static Action OnShowBoxProcessFriendsData;

	// Token: 0x0400076F RID: 1903
	public static Action OnHideBoxProcessFriendsData;

	// Token: 0x04000770 RID: 1904
	private bool _shouldStopRefreshingInfo;

	// Token: 0x04000771 RID: 1905
	private float deltaTimeInGame;

	// Token: 0x04000772 RID: 1906
	private float sendingTime;

	// Token: 0x04000773 RID: 1907
	private bool firstUpdateAfterApplicationPause;

	// Token: 0x04000774 RID: 1908
	public Dictionary<string, FriendsController.PossiblleOrigin> getPossibleFriendsResult = new Dictionary<string, FriendsController.PossiblleOrigin>();

	// Token: 0x04000775 RID: 1909
	private bool isUpdateInfoAboutAllFriends;

	// Token: 0x04000776 RID: 1910
	public static Action UpdateFriendsInfoAction;

	// Token: 0x04000777 RID: 1911
	public Dictionary<string, string> clicksJoinByFriends = new Dictionary<string, string>();

	// Token: 0x04000778 RID: 1912
	private static FriendProfileController _friendProfileController;

	// Token: 0x04000779 RID: 1913
	private static DateTime timeSendTrafficForwarding = new DateTime(2000, 1, 1, 12, 0, 0);

	// Token: 0x0400077A RID: 1914
	private static bool _isConfigNameAdvertInit;

	// Token: 0x0400077B RID: 1915
	private static string _configNameABTestAdvert = "none";

	// Token: 0x0400077C RID: 1916
	public static bool isReadABTestAdvertConfig = false;

	// Token: 0x02000125 RID: 293
	public enum PossiblleOrigin
	{
		// Token: 0x0400078F RID: 1935
		None,
		// Token: 0x04000790 RID: 1936
		Local,
		// Token: 0x04000791 RID: 1937
		Facebook,
		// Token: 0x04000792 RID: 1938
		RandomPlayer
	}

	// Token: 0x02000126 RID: 294
	public enum NotConnectCondition
	{
		// Token: 0x04000794 RID: 1940
		level,
		// Token: 0x04000795 RID: 1941
		platform,
		// Token: 0x04000796 RID: 1942
		map,
		// Token: 0x04000797 RID: 1943
		clientVersion,
		// Token: 0x04000798 RID: 1944
		InChat,
		// Token: 0x04000799 RID: 1945
		None
	}

	// Token: 0x02000127 RID: 295
	public class ResultParseOnlineData
	{
		// Token: 0x170000F9 RID: 249
		// (get) Token: 0x06000953 RID: 2387 RVA: 0x00038904 File Offset: 0x00036B04
		// (set) Token: 0x06000954 RID: 2388 RVA: 0x0003890C File Offset: 0x00036B0C
		public string gameMode
		{
			get
			{
				return this._gameMode;
			}
			set
			{
				this._gameMode = value;
				this._gameRegim = this._gameMode.Substring(this._gameMode.Length - 1);
			}
		}

		// Token: 0x06000955 RID: 2389 RVA: 0x00038934 File Offset: 0x00036B34
		public OnlineState GetOnlineStatus()
		{
			int num = Convert.ToInt32(this._gameRegim);
			if (num == 6)
			{
				return OnlineState.inFriends;
			}
			if (num == 7)
			{
				return OnlineState.inClans;
			}
			return OnlineState.playing;
		}

		// Token: 0x06000956 RID: 2390 RVA: 0x00038960 File Offset: 0x00036B60
		public string GetGameModeName()
		{
			IDictionary<string, string> gameModesLocalizeKey = ConnectSceneNGUIController.gameModesLocalizeKey;
			if (!gameModesLocalizeKey.ContainsKey(this._gameRegim))
			{
				return string.Empty;
			}
			SceneInfo infoScene = SceneInfoController.instance.GetInfoScene(int.Parse(this.mapIndex));
			if (infoScene != null && infoScene.IsAvaliableForMode(TypeModeGame.Dater))
			{
				return LocalizationStore.Get("Key_1567");
			}
			return LocalizationStore.Get(gameModesLocalizeKey[this._gameRegim]);
		}

		// Token: 0x06000957 RID: 2391 RVA: 0x000389D4 File Offset: 0x00036BD4
		public string GetMapName()
		{
			SceneInfo infoScene = SceneInfoController.instance.GetInfoScene(int.Parse(this.mapIndex));
			if (infoScene == null)
			{
				return string.Empty;
			}
			return infoScene.TranslateName;
		}

		// Token: 0x170000FA RID: 250
		// (get) Token: 0x06000958 RID: 2392 RVA: 0x00038A10 File Offset: 0x00036C10
		public bool IsCanConnect
		{
			get
			{
				return this.notConnectCondition == FriendsController.NotConnectCondition.None;
			}
		}

		// Token: 0x06000959 RID: 2393 RVA: 0x00038A1C File Offset: 0x00036C1C
		public string GetNotConnectConditionString()
		{
			if (this.IsCanConnect)
			{
				return string.Empty;
			}
			string result = string.Empty;
			switch (this.notConnectCondition)
			{
			case FriendsController.NotConnectCondition.level:
				result = LocalizationStore.Get("Key_1420");
				break;
			case FriendsController.NotConnectCondition.platform:
				result = LocalizationStore.Get("Key_1414");
				break;
			case FriendsController.NotConnectCondition.map:
				result = LocalizationStore.Get("Key_1419");
				break;
			case FriendsController.NotConnectCondition.clientVersion:
				result = LocalizationStore.Get("Key_1418");
				break;
			}
			return result;
		}

		// Token: 0x0600095A RID: 2394 RVA: 0x00038AA4 File Offset: 0x00036CA4
		public string GetNotConnectConditionShortString()
		{
			if (this.IsCanConnect)
			{
				return string.Empty;
			}
			string result = string.Empty;
			switch (this.notConnectCondition)
			{
			case FriendsController.NotConnectCondition.level:
				result = LocalizationStore.Get("Key_1574");
				break;
			case FriendsController.NotConnectCondition.platform:
				result = LocalizationStore.Get("Key_1576");
				break;
			case FriendsController.NotConnectCondition.map:
				result = LocalizationStore.Get("Key_1575");
				break;
			case FriendsController.NotConnectCondition.clientVersion:
				result = LocalizationStore.Get("Key_1573");
				break;
			case FriendsController.NotConnectCondition.InChat:
				result = LocalizationStore.Get("Key_1577");
				break;
			}
			return result;
		}

		// Token: 0x0400079A RID: 1946
		public string mapIndex;

		// Token: 0x0400079B RID: 1947
		public bool isPlayerInChat;

		// Token: 0x0400079C RID: 1948
		public FriendsController.NotConnectCondition notConnectCondition;

		// Token: 0x0400079D RID: 1949
		private string _gameRegim;

		// Token: 0x0400079E RID: 1950
		private string _gameMode;
	}

	// Token: 0x02000128 RID: 296
	public enum TypeTrafficForwardingLog
	{
		// Token: 0x040007A0 RID: 1952
		newView,
		// Token: 0x040007A1 RID: 1953
		view,
		// Token: 0x040007A2 RID: 1954
		click
	}

	// Token: 0x0200089B RID: 2203
	// (Invoke) Token: 0x06004F0C RID: 20236
	public delegate void OnChangeClanName(string newName);
}
