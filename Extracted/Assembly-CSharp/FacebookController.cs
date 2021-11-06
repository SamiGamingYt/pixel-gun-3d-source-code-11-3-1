using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Facebook.Unity;
using Rilisoft;
using Rilisoft.MiniJson;
using UnityEngine;

// Token: 0x020007F0 RID: 2032
public class FacebookController : MonoBehaviour
{
	// Token: 0x140000B1 RID: 177
	// (add) Token: 0x060049D2 RID: 18898 RVA: 0x001995C4 File Offset: 0x001977C4
	// (remove) Token: 0x060049D3 RID: 18899 RVA: 0x001995DC File Offset: 0x001977DC
	public static event Action<bool> SocialGunEventStateChanged;

	// Token: 0x140000B2 RID: 178
	// (add) Token: 0x060049D4 RID: 18900 RVA: 0x001995F4 File Offset: 0x001977F4
	// (remove) Token: 0x060049D5 RID: 18901 RVA: 0x0019960C File Offset: 0x0019780C
	public static event Action<string> PostCompleted;

	// Token: 0x140000B3 RID: 179
	// (add) Token: 0x060049D6 RID: 18902 RVA: 0x00199624 File Offset: 0x00197824
	// (remove) Token: 0x060049D7 RID: 18903 RVA: 0x0019963C File Offset: 0x0019783C
	public static event Action<string> ReceivedSelfID;

	// Token: 0x17000C20 RID: 3104
	// (get) Token: 0x060049D8 RID: 18904 RVA: 0x00199654 File Offset: 0x00197854
	public static bool FacebookSupported
	{
		get
		{
			return BuildSettings.BuildTargetPlatform != RuntimePlatform.MetroPlayerX64;
		}
	}

	// Token: 0x17000C21 RID: 3105
	// (get) Token: 0x060049D9 RID: 18905 RVA: 0x00199664 File Offset: 0x00197864
	// (set) Token: 0x060049DA RID: 18906 RVA: 0x0019966C File Offset: 0x0019786C
	public string AppId
	{
		get
		{
			return this._appId;
		}
		set
		{
			this._appId = (value ?? string.Empty);
		}
	}

	// Token: 0x17000C22 RID: 3106
	// (get) Token: 0x060049DB RID: 18907 RVA: 0x00199684 File Offset: 0x00197884
	public static bool FacebookPost_Old_Supported
	{
		get
		{
			return FacebookController.FacebookSupported && ((BuildSettings.BuildTargetPlatform != RuntimePlatform.MetroPlayerX64 && FacebookController.sharedController != null && !FacebookController.sharedController.CanPostStoryWithPriority(FacebookController.StoryPriority.Green)) || (BuildSettings.BuildTargetPlatform != RuntimePlatform.MetroPlayerX64 && Defs.isMulti && PhotonNetwork.connected && NetworkStartTable.LocalOrPasswordRoom()));
		}
	}

	// Token: 0x060049DC RID: 18908 RVA: 0x001996F8 File Offset: 0x001978F8
	public bool CanPostStoryWithPriority(FacebookController.StoryPriority priority)
	{
		try
		{
			if (priority == FacebookController.StoryPriority.Green)
			{
				return this.storiesPostHistory.Count < FacebookController.StoryPostLimits[priority];
			}
			return (from rec in this.storiesPostHistory
			where int.Parse((string)rec["priority"]) == (int)priority
			select rec).Count<Dictionary<string, object>>() < FacebookController.StoryPostLimits[priority];
		}
		catch (Exception ex)
		{
			Debug.LogErrorFormat("Caught exception in CanPostStoryWithPriority:\n{0}", new object[]
			{
				ex
			});
		}
		return false;
	}

	// Token: 0x060049DD RID: 18909 RVA: 0x001997B4 File Offset: 0x001979B4
	public string GetTimeToEndSocialGunEvent()
	{
		if (!this.SocialGunEventActive)
		{
			return string.Empty;
		}
		TimeSpan timeSpan = this.socialEventStartTime + this.DurationSocialGunEvent - DateTime.UtcNow;
		return string.Format("{0:00}:{1:00}:{2:00}", timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);
	}

	// Token: 0x17000C23 RID: 3107
	// (get) Token: 0x060049DE RID: 18910 RVA: 0x0019981C File Offset: 0x00197A1C
	public bool SocialGunEventActive
	{
		get
		{
			return this.socialGunEventActive;
		}
	}

	// Token: 0x060049DF RID: 18911 RVA: 0x00199824 File Offset: 0x00197A24
	public bool IsNeedShowGunFroLoginWindow()
	{
		int @int = Storager.getInt("FacebookController.CountShownGunForLogin", false);
		return this.socialGunEventActive && @int < 1 && SceneLoader.ActiveSceneName.Equals(Defs.MainMenuScene) && Storager.getInt(Defs.IsFacebookLoginRewardaGained, true) == 0;
	}

	// Token: 0x060049E0 RID: 18912 RVA: 0x00199874 File Offset: 0x00197A74
	public void UpdateCountShownWindowByShowCondition()
	{
		int @int = Storager.getInt("FacebookController.CountShownGunForLogin", false);
		Storager.setString(Defs.LastTimeShowSocialGun, DateTime.UtcNow.ToString("s"), false);
		Storager.setInt("FacebookController.CountShownGunForLogin", @int + 1, false);
	}

	// Token: 0x060049E1 RID: 18913 RVA: 0x001998B8 File Offset: 0x00197AB8
	private void UpdateCountShownSocialGunWindowByTimeCondition()
	{
		if (FacebookController.FacebookSupported)
		{
			string text = string.Empty;
			if (!Storager.hasKey(Defs.LastTimeShowSocialGun))
			{
				Storager.setString(Defs.LastTimeShowSocialGun, text, false);
			}
			else
			{
				text = Storager.getString(Defs.LastTimeShowSocialGun, false);
			}
			if (string.IsNullOrEmpty(text))
			{
				return;
			}
			DateTime d = default(DateTime);
			if (!DateTime.TryParse(text, out d))
			{
				return;
			}
			if (DateTime.UtcNow - d >= this.TimeBetweenSocialGunBannerSeries)
			{
				Storager.setInt("FacebookController.CountShownGunForLogin", 0, false);
			}
		}
	}

	// Token: 0x060049E2 RID: 18914 RVA: 0x0019994C File Offset: 0x00197B4C
	private bool CurrentSocialGunEventState()
	{
		return FacebookController.FacebookSupported && Storager.getInt(Defs.IsFacebookLoginRewardaGained, true) == 0 && DateTime.UtcNow - this.socialEventStartTime < this.DurationSocialGunEvent && ExpController.LobbyLevel > 1 && !MainMenuController.SavedShwonLobbyLevelIsLessThanActual();
	}

	// Token: 0x060049E3 RID: 18915 RVA: 0x001999AC File Offset: 0x00197BAC
	private void Awake()
	{
		this.friendsList = new List<Friend>();
		if (FacebookController.FacebookSupported)
		{
			FriendsController.NewFacebookLimitsAvailable += this.HandleNewFacebookLimitsAvailable;
			string text = string.Empty;
			if (!Storager.hasKey("FacebookControllerSocialGunEventStartedKey"))
			{
				text = DateTime.UtcNow.ToString("s");
				Storager.setString("FacebookControllerSocialGunEventStartedKey", text, false);
			}
			else
			{
				text = Storager.getString("FacebookControllerSocialGunEventStartedKey", false);
				DateTime dateTime = default(DateTime);
				if (!DateTime.TryParse(text, out dateTime))
				{
					text = DateTime.UtcNow.ToString("s");
					Storager.setString("FacebookControllerSocialGunEventStartedKey", text, false);
				}
			}
			if (DateTime.TryParse(text, out this.socialEventStartTime))
			{
				this.socialGunEventActive = this.CurrentSocialGunEventState();
			}
			else
			{
				Debug.LogError("FacebookController: invalid timeStartEvent");
			}
		}
	}

	// Token: 0x060049E4 RID: 18916 RVA: 0x00199A84 File Offset: 0x00197C84
	private void HandleNewFacebookLimitsAvailable(int greenLimit, int redLimit)
	{
		FacebookController.StoryPostLimits[FacebookController.StoryPriority.Green] = greenLimit;
		FacebookController.StoryPostLimits[FacebookController.StoryPriority.Red] = redLimit;
	}

	// Token: 0x060049E5 RID: 18917 RVA: 0x00199AA0 File Offset: 0x00197CA0
	private void Start()
	{
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		FacebookController.sharedController = this;
		if (FacebookController.FacebookSupported)
		{
			this.InitFacebook();
		}
		this.InitStoryPostHistoryKey();
		this.LoadStoryPostHistory();
		this.UpdateCountShownSocialGunWindowByTimeCondition();
	}

	// Token: 0x060049E6 RID: 18918 RVA: 0x00199AE0 File Offset: 0x00197CE0
	private void OnApplicationPause(bool pause)
	{
		if (pause)
		{
			this.SaveStoryPostHistory();
		}
		else
		{
			if (FB.IsInitialized)
			{
				FB.ActivateApp();
			}
			this.LoadStoryPostHistory();
			this.UpdateCountShownSocialGunWindowByTimeCondition();
		}
	}

	// Token: 0x060049E7 RID: 18919 RVA: 0x00199B1C File Offset: 0x00197D1C
	private void OnDestroy()
	{
		this.SaveStoryPostHistory();
		if (FacebookController.FacebookSupported)
		{
			FriendsController.NewFacebookLimitsAvailable -= this.HandleNewFacebookLimitsAvailable;
		}
	}

	// Token: 0x060049E8 RID: 18920 RVA: 0x00199B40 File Offset: 0x00197D40
	private void SaveStoryPostHistory()
	{
		Storager.setString("FacebookControllerStoryPostHistoryKey", Json.Serialize(this.storiesPostHistory), false);
	}

	// Token: 0x060049E9 RID: 18921 RVA: 0x00199B58 File Offset: 0x00197D58
	private void LoadStoryPostHistory()
	{
		try
		{
			List<object> list = Json.Deserialize(Storager.getString("FacebookControllerStoryPostHistoryKey", false)) as List<object>;
			this.storiesPostHistory.Clear();
			foreach (object obj in list)
			{
				Dictionary<string, object> item = obj as Dictionary<string, object>;
				this.storiesPostHistory.Add(item);
			}
			this.CleanStoryPostHistory();
		}
		catch (Exception ex)
		{
			this.storiesPostHistory.Clear();
		}
	}

	// Token: 0x060049EA RID: 18922 RVA: 0x00199C1C File Offset: 0x00197E1C
	private void InitStoryPostHistoryKey()
	{
		if (!Storager.hasKey("FacebookControllerStoryPostHistoryKey"))
		{
			Storager.setString("FacebookControllerStoryPostHistoryKey", "[]", false);
		}
	}

	// Token: 0x060049EB RID: 18923 RVA: 0x00199C40 File Offset: 0x00197E40
	private void CleanStoryPostHistory()
	{
		this._timeSinceLastStoryPostHistoryClean = Time.realtimeSinceStartup;
		try
		{
			long num = 86400L;
			long minimumValidTime = PromoActionsManager.CurrentUnixTime - num;
			this.storiesPostHistory.RemoveAll((Dictionary<string, object> entry) => long.Parse((string)entry["time"]) < minimumValidTime);
		}
		catch (Exception arg)
		{
			Debug.LogError("Exeption in CleanStoryPostHistory:\n" + arg);
		}
	}

	// Token: 0x060049EC RID: 18924 RVA: 0x00199CC4 File Offset: 0x00197EC4
	private void Update()
	{
		bool flag = this.CurrentSocialGunEventState();
		if (this.socialGunEventActive != flag)
		{
			this.socialGunEventActive = flag;
			Action<bool> socialGunEventStateChanged = FacebookController.SocialGunEventStateChanged;
			if (socialGunEventStateChanged != null)
			{
				socialGunEventStateChanged(this.SocialGunEventActive);
			}
		}
		if (Time.realtimeSinceStartup - this._timeSinceLastStoryPostHistoryClean > 10f)
		{
			this.CleanStoryPostHistory();
		}
		if (FacebookController.FacebookSupported && !FB.IsLoggedIn && FriendsController.sharedController != null && !string.IsNullOrEmpty(FriendsController.sharedController.id_fb))
		{
			Action<string> receivedSelfID = FacebookController.ReceivedSelfID;
			if (receivedSelfID != null)
			{
				receivedSelfID(string.Empty);
			}
		}
	}

	// Token: 0x060049ED RID: 18925 RVA: 0x00199D70 File Offset: 0x00197F70
	public static void LogEvent(string eventName, Dictionary<string, object> parameters = null)
	{
		if (!FacebookController.FacebookSupported)
		{
			return;
		}
		try
		{
			FB.LogAppEvent(eventName, null, parameters);
		}
		catch (Exception exception)
		{
			Debug.LogException(exception);
		}
	}

	// Token: 0x060049EE RID: 18926 RVA: 0x00199DC8 File Offset: 0x00197FC8
	public static void ShowPostDialog()
	{
		if (!FacebookController.FacebookSupported)
		{
			return;
		}
		PlayerPrefs.SetInt("PostFacebookCount", PlayerPrefs.GetInt("PostFacebookCount", 0) + 1);
		PlayerPrefs.Save();
		if (PlayerPrefs.GetInt("Active_loyal_users_payed_send", 0) == 0 && PlayerPrefs.GetInt("PostFacebookCount", 0) > 2 && StoreKitEventListener.GetDollarsSpent() > 0 && PlayerPrefs.GetInt("PostVideo", 0) > 0)
		{
			FacebookController.LogEvent("Active_loyal_users_payed", null);
			PlayerPrefs.SetInt("Active_loyal_users_payed_send", 1);
		}
		if (PlayerPrefs.GetInt("Active_loyal_users_send", 0) == 0 && PlayerPrefs.GetInt("PostFacebookCount", 0) > 2 && PlayerPrefs.GetInt("PostVideo", 0) > 0)
		{
			FacebookController.LogEvent("Active_loyal_users", null);
			PlayerPrefs.SetInt("Active_loyal_users_send", 1);
		}
		Dictionary<string, object> dictionary = new Dictionary<string, object>
		{
			{
				"link",
				Defs2.ApplicationUrl
			},
			{
				"name",
				"Pixel Gun 3D"
			},
			{
				"picture",
				"http://pixelgun3d.com/iconforpost.png"
			},
			{
				"caption",
				"I've just played the super battle in Pixel Gun 3D :)"
			},
			{
				"description",
				"DOWNLOAD IT FOR FREE AND JOIN ME NOW!"
			}
		};
		Uri picture = new Uri((string)dictionary["picture"]);
		FB.FeedShare(string.Empty, new Uri((string)dictionary["link"]), (string)dictionary["name"], (string)dictionary["caption"], (string)dictionary["description"], picture, string.Empty, null);
	}

	// Token: 0x060049EF RID: 18927 RVA: 0x00199F5C File Offset: 0x0019815C
	public void PostMessage(string _message, Action<string, object> _completionHandler)
	{
		Debug.Log("Post to Facebook");
	}

	// Token: 0x060049F0 RID: 18928 RVA: 0x00199F68 File Offset: 0x00198168
	public static void PrintFBResult(IResult result)
	{
	}

	// Token: 0x060049F1 RID: 18929 RVA: 0x00199F6C File Offset: 0x0019816C
	public static void FBGet(string graphPath, Action<IDictionary<string, object>> act, Action<IResult> onError = null)
	{
		if (!FacebookController.FacebookSupported)
		{
			return;
		}
		FB.API(graphPath, HttpMethod.GET, delegate(IGraphResult result)
		{
			try
			{
				FacebookController.PrintFBResult(result);
				act(result.ResultDictionary);
			}
			catch (Exception arg)
			{
				if (onError != null)
				{
					onError(result);
				}
				Debug.LogError("Exception FBGet: " + arg);
			}
		}, null);
	}

	// Token: 0x060049F2 RID: 18930 RVA: 0x00199FAC File Offset: 0x001981AC
	internal static void CheckAndGiveFacebookReward(string context)
	{
		if (!FacebookController.FacebookSupported)
		{
			return;
		}
		if (context == null)
		{
			throw new ArgumentNullException("context");
		}
		if (FacebookController.FacebookSupported && Storager.getInt(Defs.IsFacebookLoginRewardaGained, true) == 0 && Storager.getInt(Defs.FacebookRewardGainStarted, false) == 1 && FB.IsLoggedIn)
		{
			Storager.setInt(Defs.FacebookRewardGainStarted, 0, false);
			Storager.setInt(Defs.IsFacebookLoginRewardaGained, 1, true);
			BankController.AddGems(10, true, AnalyticsConstants.AccrualType.Earned);
			TutorialQuestManager.Instance.AddFulfilledQuest("loginFacebook");
			QuestMediator.NotifySocialInteraction("loginFacebook");
			ShopNGUIController.ProvideItem(ShopNGUIController.CategoryNames.SkinsCategory, "super_socialman", 1, false, 0, null, null, false, true, false);
			AnalyticsFacade.SendCustomEvent("Virality", new Dictionary<string, object>
			{
				{
					"Login Facebook",
					context
				}
			});
			AnalyticsFacade.SendCustomEventToAppsFlyer("Virality", new Dictionary<string, string>
			{
				{
					"Login Facebook",
					context
				}
			});
			WeaponManager.AddExclusiveWeaponToWeaponStructures(WeaponManager.SocialGunWN);
		}
	}

	// Token: 0x17000C24 RID: 3108
	// (get) Token: 0x060049F3 RID: 18931 RVA: 0x0019A09C File Offset: 0x0019829C
	// (set) Token: 0x060049F4 RID: 18932 RVA: 0x0019A0A4 File Offset: 0x001982A4
	public static bool LoggingIn { get; private set; }

	// Token: 0x060049F5 RID: 18933 RVA: 0x0019A0AC File Offset: 0x001982AC
	public static void Login(Action onSuccess = null, Action onFailure = null, string context = "Unknown", Action onSuccessAfterPublishPermissions = null)
	{
		if (!FacebookController.FacebookSupported)
		{
			return;
		}
		if (Storager.getInt(Defs.IsFacebookLoginRewardaGained, true) == 0)
		{
			Storager.setInt(Defs.FacebookRewardGainStarted, 1, false);
		}
		FacebookController.LoggingIn = true;
		try
		{
			FB.LogInWithReadPermissions(new List<string>
			{
				"user_friends"
			}, delegate(ILoginResult result)
			{
				FacebookController.LoggingIn = false;
				FacebookController.PrintFBResult(result);
				FacebookController.CheckAndGiveFacebookReward(context);
				if (FB.IsLoggedIn)
				{
					if (onSuccess != null)
					{
						onSuccess();
					}
					try
					{
						Action<string> receivedSelfID = FacebookController.ReceivedSelfID;
						if (receivedSelfID != null)
						{
							receivedSelfID((string)result.ResultDictionary["user_id"]);
						}
					}
					catch (Exception arg2)
					{
						Debug.LogError("FacebookController Login ReceivedSelfID exception: " + arg2);
					}
					try
					{
						if (FacebookController.sharedController != null)
						{
							FacebookController.sharedController.InputFacebookFriends(null, false);
						}
					}
					catch (Exception arg3)
					{
						Debug.LogError("FacebookController Login InputFacebookFriends exception: " + arg3);
					}
					CoroutineRunner.Instance.StartCoroutine(FacebookController.RunActionAfterDelay(delegate
					{
						FacebookController.LoggingIn = true;
						try
						{
							FB.LogInWithPublishPermissions(new List<string>
							{
								"publish_actions"
							}, delegate(ILoginResult publishLoginResult)
							{
								FacebookController.LoggingIn = false;
								FacebookController.PrintFBResult(publishLoginResult);
								if (string.IsNullOrEmpty(publishLoginResult.Error) && !publishLoginResult.Cancelled && onSuccessAfterPublishPermissions != null)
								{
									onSuccessAfterPublishPermissions();
								}
							});
						}
						catch (Exception arg4)
						{
							Debug.LogError("Exception in Facebook Login: " + arg4);
							FacebookController.LoggingIn = false;
						}
					}));
				}
				else if (onFailure != null)
				{
					onFailure();
				}
			});
		}
		catch (Exception arg)
		{
			Debug.LogError("Exception in Facebook Login: " + arg);
			FacebookController.LoggingIn = false;
		}
	}

	// Token: 0x060049F6 RID: 18934 RVA: 0x0019A168 File Offset: 0x00198368
	private static IEnumerator RunActionAfterDelay(Action action)
	{
		for (int i = 0; i < 30; i++)
		{
			yield return null;
		}
		if (action != null)
		{
			action();
		}
		yield break;
	}

	// Token: 0x060049F7 RID: 18935 RVA: 0x0019A18C File Offset: 0x0019838C
	private static void RegisterStoryPostedWithPriority(FacebookController.StoryPriority priority)
	{
		if (FacebookController.sharedController == null)
		{
			return;
		}
		FacebookController.sharedController.RegisterStoryPostedWithPriorityCore(priority);
	}

	// Token: 0x060049F8 RID: 18936 RVA: 0x0019A1AC File Offset: 0x001983AC
	private void RegisterStoryPostedWithPriorityCore(FacebookController.StoryPriority priority)
	{
		if (!FacebookController.FacebookSupported)
		{
			return;
		}
		List<Dictionary<string, object>> list = this.storiesPostHistory;
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		Dictionary<string, object> dictionary2 = dictionary;
		string key = "priority";
		int num = (int)priority;
		dictionary2.Add(key, num.ToString());
		dictionary.Add("time", PromoActionsManager.CurrentUnixTime.ToString());
		list.Add(dictionary);
	}

	// Token: 0x060049F9 RID: 18937 RVA: 0x0019A204 File Offset: 0x00198404
	public static void PostOpenGraphStory(string action, string obj, FacebookController.StoryPriority priority, Dictionary<string, string> pars = null)
	{
		if (!FacebookController.FacebookSupported)
		{
			return;
		}
		FacebookController.RunApiWithAskForPermissions(delegate
		{
			string text = "https://secure.pixelgunserver.com/fb/ogobjects.php?type=" + WWW.EscapeURL(obj);
			if (pars != null)
			{
				foreach (KeyValuePair<string, string> keyValuePair in pars)
				{
					string text2 = text;
					text = string.Concat(new string[]
					{
						text2,
						"&",
						keyValuePair.Key.Replace(" "[0], "_"[0]),
						"=",
						WWW.EscapeURL(keyValuePair.Value)
					});
				}
			}
			FacebookController.FBPost("/me/pixelgun:" + action, new Dictionary<string, string>
			{
				{
					obj,
					text
				},
				{
					"fb:explicitly_shared",
					"true"
				}
			}, delegate(IDictionary<string, object> result)
			{
			}, delegate(IResult result)
			{
				if (result != null && result.Error == null)
				{
					FacebookController.RegisterStoryPostedWithPriority(priority);
				}
			});
		}, "publish_actions", false, null);
	}

	// Token: 0x060049FA RID: 18938 RVA: 0x0019A258 File Offset: 0x00198458
	public static void FBPost(string graphPath, Dictionary<string, string> pars, Action<IDictionary<string, object>> act, Action<IResult> actionWithFBResult = null)
	{
		if (!FacebookController.FacebookSupported)
		{
			return;
		}
		FB.API(graphPath, HttpMethod.POST, delegate(IGraphResult result)
		{
			try
			{
				if (actionWithFBResult != null)
				{
					actionWithFBResult(result);
				}
				FacebookController.PrintFBResult(result);
				act(result.ResultDictionary);
			}
			catch (Exception arg)
			{
				Debug.LogError("Exception FBPost: " + arg);
			}
		}, pars);
	}

	// Token: 0x060049FB RID: 18939 RVA: 0x0019A298 File Offset: 0x00198498
	public void SetMyId(Action onSuccess = null, bool dontRelogin = false)
	{
		if (!FacebookController.FacebookSupported)
		{
			return;
		}
		FacebookController.RunApiWithAskForPermissions(delegate
		{
			string graphPath = "/me/friends?fields=id,name,installed&limit=1000000";
			FacebookController.FBGet(graphPath, delegate(IDictionary<string, object> result)
			{
				IList list = result["data"] as IList;
				if (Defs.IsDeveloperBuild)
				{
					Debug.Log("Result facebook friends" + result.ToString());
				}
				this.friendsList.Clear();
				for (int i = 0; i < list.Count; i++)
				{
					IDictionary dictionary = list[i] as IDictionary;
					this.friendsList.Add(new Friend(dictionary["name"] as string, dictionary["id"].ToString(), dictionary["installed"].ToString().Equals("true", StringComparison.OrdinalIgnoreCase)));
				}
				if (onSuccess != null)
				{
					onSuccess();
				}
			}, null);
		}, "user_friends", dontRelogin, null);
	}

	// Token: 0x060049FC RID: 18940 RVA: 0x0019A2DC File Offset: 0x001984DC
	private void InitFacebook()
	{
		if (!FacebookController.FacebookSupported)
		{
			return;
		}
		InitDelegate onInitComplete = delegate()
		{
			try
			{
				FB.ActivateApp();
				if (FB.IsLoggedIn)
				{
					base.StartCoroutine(this.GetOurFbId());
				}
			}
			catch (Exception ex2)
			{
				Debug.LogException(ex2);
				Debug.LogError("[Rilisoft] Exception in onInitComplete calback of FB.Init() method. Stacktrace: " + ex2.StackTrace);
			}
		};
		try
		{
			FB.Init(onInitComplete, delegate(bool isGameShown)
			{
			}, null);
		}
		catch (NotImplementedException ex)
		{
			Debug.LogWarningFormat("Catching exception during FB.Init(): {0}", new object[]
			{
				ex.Message
			});
		}
	}

	// Token: 0x060049FD RID: 18941 RVA: 0x0019A368 File Offset: 0x00198568
	public static void RunApiWithAskForPermissions(Action runApi, string requiredPermission = "", bool dontRelogin = false, Action onFailToRunApi = null)
	{
		if (!FacebookController.FacebookSupported)
		{
			return;
		}
		if (dontRelogin)
		{
			if (FB.IsLoggedIn && runApi != null)
			{
				runApi();
			}
			return;
		}
		int countTryingUpdateToken = 0;
		Action<bool> loginWithRequiredPermissions = null;
		Action loginWithRequiredPermissionsThroughLoginMethod = null;
		Action<bool> checkPermissionsAndRunApi = delegate(bool loginIfNoPermissions)
		{
			if (!string.IsNullOrEmpty(requiredPermission))
			{
				FacebookController.FBGet("/me/permissions?limit=500", delegate(IDictionary<string, object> result)
				{
					List<object> source = result["data"] as List<object>;
					List<string> list = (from p in source
					where (p as Dictionary<string, object>)["status"].Equals("granted")
					select (string)(p as Dictionary<string, object>)["permission"]).ToList<string>();
					if (list.Contains(requiredPermission))
					{
						runApi();
					}
					else if (loginIfNoPermissions)
					{
						loginWithRequiredPermissions(false);
					}
					else if (onFailToRunApi != null)
					{
						onFailToRunApi();
					}
				}, delegate(IResult result)
				{
					if (result != null && result.RawResult != null && result.RawResult.Contains("OAuthException"))
					{
						loginWithRequiredPermissionsThroughLoginMethod();
						countTryingUpdateToken++;
					}
					else if (onFailToRunApi != null)
					{
						onFailToRunApi();
					}
				});
			}
			else
			{
				runApi();
			}
		};
		loginWithRequiredPermissions = delegate(bool bothReadAndWriteLogins)
		{
			Action loginWithRequiredPermissionsOneStep = delegate()
			{
				FacebookDelegate<ILoginResult> callback = delegate(ILoginResult result)
				{
					FacebookController.LoggingIn = false;
					FacebookController.PrintFBResult(result);
					if (FB.IsLoggedIn)
					{
						checkPermissionsAndRunApi(false);
					}
					else if (onFailToRunApi != null)
					{
						onFailToRunApi();
					}
				};
				if (requiredPermission == "publish_actions")
				{
					FacebookController.LoggingIn = true;
					try
					{
						FB.LogInWithPublishPermissions(new List<string>
						{
							requiredPermission
						}, callback);
					}
					catch (Exception arg2)
					{
						Debug.LogError("Exception in Facebook Login: " + arg2);
						FacebookController.LoggingIn = false;
					}
				}
				else
				{
					FacebookController.LoggingIn = true;
					try
					{
						FB.LogInWithReadPermissions(new List<string>
						{
							requiredPermission
						}, callback);
					}
					catch (Exception arg3)
					{
						Debug.LogError("Exception in Facebook Login: " + arg3);
						FacebookController.LoggingIn = false;
					}
				}
			};
			if (bothReadAndWriteLogins && requiredPermission == "publish_actions")
			{
				FacebookController.LoggingIn = true;
				try
				{
					FB.LogInWithReadPermissions(new List<string>(), delegate(ILoginResult result)
					{
						FacebookController.LoggingIn = false;
						FacebookController.PrintFBResult(result);
						if (string.IsNullOrEmpty(result.Error) && !result.Cancelled)
						{
							CoroutineRunner.Instance.StartCoroutine(FacebookController.RunActionAfterDelay(loginWithRequiredPermissionsOneStep));
						}
						else
						{
							Debug.LogError("LogInWithReadPermissions: ! (string.IsNullOrEmpty(result.Error) && ! result.Cancelled)");
						}
					});
				}
				catch (Exception arg)
				{
					Debug.LogError("Exception in Facebook Login: " + arg);
					FacebookController.LoggingIn = false;
				}
			}
			else
			{
				loginWithRequiredPermissionsOneStep();
			}
		};
		loginWithRequiredPermissionsThroughLoginMethod = delegate()
		{
			FB.LogOut();
			Action onSuccessAfterPublishPermissions = delegate()
			{
				checkPermissionsAndRunApi(false);
			};
			FacebookController.Login(null, null, "Unknown", onSuccessAfterPublishPermissions);
		};
		if (!FB.IsLoggedIn)
		{
			loginWithRequiredPermissions(requiredPermission == "publish_actions");
		}
		else
		{
			checkPermissionsAndRunApi(true);
		}
	}

	// Token: 0x060049FE RID: 18942 RVA: 0x0019A444 File Offset: 0x00198644
	public void InputFacebookFriends(Action onSuccess = null, bool dontRelogin = false)
	{
		if (!FacebookController.FacebookSupported)
		{
			return;
		}
		this.SetMyId(onSuccess, dontRelogin);
	}

	// Token: 0x060049FF RID: 18943 RVA: 0x0019A45C File Offset: 0x0019865C
	public void InvitePlayer(Action onComplete = null)
	{
		if (!FacebookController.FacebookSupported)
		{
			return;
		}
		if (this.InvitePlayerApiIsRunning)
		{
			return;
		}
		this.InvitePlayerApiIsRunning = true;
		Action runApi = delegate()
		{
			FB.AppRequest(this.messageInvite, null, null, null, null, string.Empty, this.titleInvite, delegate(IAppRequestResult result)
			{
				this.InvitePlayerApiIsRunning = false;
				FacebookController.PrintFBResult(result);
				if (onComplete != null)
				{
					onComplete();
				}
			});
		};
		FacebookController.RunApiWithAskForPermissions(runApi, "publish_actions", false, delegate
		{
			this.InvitePlayerApiIsRunning = false;
		});
	}

	// Token: 0x06004A00 RID: 18944 RVA: 0x0019A4C0 File Offset: 0x001986C0
	private IEnumerator GetOurFbId()
	{
		if (!FacebookController.FacebookSupported)
		{
			yield break;
		}
		while (FriendsController.sharedController == null)
		{
			yield return null;
		}
		bool success = false;
		while (FB.IsLoggedIn && !success)
		{
			FacebookController.FBGet("/me", delegate(IDictionary<string, object> dict)
			{
				try
				{
					Action<string> receivedSelfID = FacebookController.ReceivedSelfID;
					if (receivedSelfID != null)
					{
						receivedSelfID((string)dict["id"]);
					}
					success = true;
				}
				catch (Exception arg)
				{
					Debug.LogError("FacebookController GetOurFbId exception: " + arg);
				}
			}, null);
			float startTime = Time.realtimeSinceStartup;
			do
			{
				yield return null;
			}
			while (Time.realtimeSinceStartup - startTime < 30f);
		}
		yield break;
	}

	// Token: 0x040036B1 RID: 14001
	public const int MaxCountShownGunForLogin = 1;

	// Token: 0x040036B2 RID: 14002
	public const int DefaultGreenPriorityLimit = 7;

	// Token: 0x040036B3 RID: 14003
	public const int DefaultRedPriorityLimit = 3;

	// Token: 0x040036B4 RID: 14004
	private const string PriorityKey = "priority";

	// Token: 0x040036B5 RID: 14005
	private const string TimeKey = "time";

	// Token: 0x040036B6 RID: 14006
	private const string StoryPostHistoryKey = "FacebookControllerStoryPostHistoryKey";

	// Token: 0x040036B7 RID: 14007
	public const string FacebookScriptAddress = "https://secure.pixelgunserver.com/fb/ogobjects.php";

	// Token: 0x040036B8 RID: 14008
	private const string SocialGunEventStartedKey = "FacebookControllerSocialGunEventStartedKey";

	// Token: 0x040036B9 RID: 14009
	public static FacebookController sharedController;

	// Token: 0x040036BA RID: 14010
	public string selfID = string.Empty;

	// Token: 0x040036BB RID: 14011
	private Action<string, object> handlerForPost;

	// Token: 0x040036BC RID: 14012
	private bool hasPublishActions;

	// Token: 0x040036BD RID: 14013
	private bool isGetPermitionFromSendMessage;

	// Token: 0x040036BE RID: 14014
	private string postMessage;

	// Token: 0x040036BF RID: 14015
	public List<Friend> friendsList;

	// Token: 0x040036C0 RID: 14016
	private string titleInvite = "Invite a Friend to Play!";

	// Token: 0x040036C1 RID: 14017
	private string messageInvite = "Join me in playing a new game!";

	// Token: 0x040036C2 RID: 14018
	public static readonly Dictionary<FacebookController.StoryPriority, int> StoryPostLimits = new Dictionary<FacebookController.StoryPriority, int>(2, new FacebookController.StoryPriorityComparer())
	{
		{
			FacebookController.StoryPriority.Green,
			7
		},
		{
			FacebookController.StoryPriority.Red,
			3
		}
	};

	// Token: 0x040036C3 RID: 14019
	private string _appId = string.Empty;

	// Token: 0x040036C4 RID: 14020
	private bool socialGunEventActive;

	// Token: 0x040036C5 RID: 14021
	private float _timeSinceLastStoryPostHistoryClean;

	// Token: 0x040036C6 RID: 14022
	public bool InvitePlayerApiIsRunning;

	// Token: 0x040036C7 RID: 14023
	private TimeSpan DurationSocialGunEvent = TimeSpan.FromDays(1000000.0);

	// Token: 0x040036C8 RID: 14024
	private TimeSpan TimeBetweenSocialGunBannerSeries = TimeSpan.FromHours(24.0);

	// Token: 0x040036C9 RID: 14025
	private DateTime socialEventStartTime;

	// Token: 0x040036CA RID: 14026
	private List<Dictionary<string, object>> storiesPostHistory = new List<Dictionary<string, object>>();

	// Token: 0x020007F1 RID: 2033
	public enum StoryPriority
	{
		// Token: 0x040036D1 RID: 14033
		Green,
		// Token: 0x040036D2 RID: 14034
		Red,
		// Token: 0x040036D3 RID: 14035
		MultyWinLimit,
		// Token: 0x040036D4 RID: 14036
		ArenaLimit
	}

	// Token: 0x020007F2 RID: 2034
	internal sealed class StoryPriorityComparer : IEqualityComparer<FacebookController.StoryPriority>
	{
		// Token: 0x06004A04 RID: 18948 RVA: 0x0019A54C File Offset: 0x0019874C
		public bool Equals(FacebookController.StoryPriority x, FacebookController.StoryPriority y)
		{
			return x == y;
		}

		// Token: 0x06004A05 RID: 18949 RVA: 0x0019A554 File Offset: 0x00198754
		public int GetHashCode(FacebookController.StoryPriority obj)
		{
			return (int)obj;
		}
	}
}
