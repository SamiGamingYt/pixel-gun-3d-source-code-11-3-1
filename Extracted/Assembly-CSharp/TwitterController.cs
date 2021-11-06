using System;
using System.Collections.Generic;
using System.Linq;
using Prime31;
using Rilisoft;
using Rilisoft.MiniJson;
using UnityEngine;

// Token: 0x02000873 RID: 2163
internal sealed class TwitterController : MonoBehaviour
{
	// Token: 0x06004E27 RID: 20007 RVA: 0x001C5EF4 File Offset: 0x001C40F4
	private void Awake()
	{
		TwitterController.Instance = this;
		if (TwitterController.TwitterSupported)
		{
			string consumerKey = string.Empty;
			string consumerSecret = string.Empty;
			consumerKey = "13K6E5YAJvXSEaig0GVVFd68K";
			consumerSecret = "I4DtR7TC0OU26OMYI0hLmP1jhVHfNuPRMskbYDIoOS2xYBBWdS";
			TwitterController.TwitterFacade.Init(consumerKey, consumerSecret);
			TwitterManager.loginSucceededEvent += this.OnTwitterLogin;
			TwitterManager.loginFailedEvent += this.OnTwitterLoginFailed;
			TwitterManager.requestDidFinishEvent += this.OnTwitterPost;
			TwitterManager.requestDidFailEvent += this.OnTwitterPostFailed;
			FriendsController.NewTwitterLimitsAvailable += this.HandleNewTwitterLimitsAvailable;
		}
	}

	// Token: 0x06004E28 RID: 20008 RVA: 0x001C5F8C File Offset: 0x001C418C
	private void Start()
	{
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		this.InitStoryPostHistoryKey();
		this.LoadStoryPostHistory();
	}

	// Token: 0x17000CBC RID: 3260
	// (get) Token: 0x06004E29 RID: 20009 RVA: 0x001C5FA8 File Offset: 0x001C41A8
	public static bool IsLoggedIn
	{
		get
		{
			return TwitterController.TwitterFacade.IsLoggedIn();
		}
	}

	// Token: 0x06004E2A RID: 20010 RVA: 0x001C5FB4 File Offset: 0x001C41B4
	public void Login(Action onSuccess = null, Action onFailure = null, string context = "Unknown")
	{
		if (!TwitterController.TwitterSupported)
		{
			return;
		}
		Action<string> onSuccessCallback = null;
		Action<string> onFailCallback = null;
		onSuccessCallback = delegate(string unusedResult)
		{
			if (onSuccess != null)
			{
				onSuccess();
			}
			TwitterManager.loginSucceededEvent -= onSuccessCallback;
			TwitterManager.loginFailedEvent -= onFailCallback;
		};
		onFailCallback = delegate(string unusedResult)
		{
			if (onFailure != null)
			{
				onFailure();
			}
			TwitterManager.loginSucceededEvent -= onSuccessCallback;
			TwitterManager.loginFailedEvent -= onFailCallback;
		};
		TwitterManager.loginSucceededEvent += onSuccessCallback;
		TwitterManager.loginFailedEvent += onFailCallback;
		if (Storager.getInt(Defs.IsTwitterLoginRewardaGained, true) == 0)
		{
			Storager.setInt(Defs.TwitterRewardGainStarted, 1, false);
		}
		this._loginContext = context;
		TwitterController.TwitterFacade.ShowLoginDialog(null);
	}

	// Token: 0x06004E2B RID: 20011 RVA: 0x001C6058 File Offset: 0x001C4258
	public void Logout()
	{
		if (!TwitterController.TwitterSupported)
		{
			return;
		}
		TwitterController.TwitterFacade.Logout();
	}

	// Token: 0x06004E2C RID: 20012 RVA: 0x001C6070 File Offset: 0x001C4270
	public void PostStatusUpdate(string status, FacebookController.StoryPriority priority)
	{
		if (!TwitterController.TwitterSupported)
		{
			return;
		}
		this.PostStatusUpdate(status, delegate()
		{
			this.RegisterStoryPostedWithPriorityCore(priority);
		});
	}

	// Token: 0x06004E2D RID: 20013 RVA: 0x001C60B0 File Offset: 0x001C42B0
	public void PostStatusUpdate(string status, Action customOnSuccess = null)
	{
		if (!TwitterController.TwitterSupported)
		{
			return;
		}
		Action<string> postAction = null;
		Action<string> loginFailedAction = null;
		postAction = delegate(string unusedString)
		{
			TwitterManager.loginSucceededEvent -= postAction;
			TwitterManager.loginFailedEvent -= loginFailedAction;
			if (this._postInProcess)
			{
				return;
			}
			if (customOnSuccess != null)
			{
				Action<object> onSuccessPost = null;
				Action<string> onFailedPost = null;
				onSuccessPost = delegate(object unused)
				{
					customOnSuccess();
					TwitterManager.requestDidFinishEvent -= onSuccessPost;
					TwitterManager.requestDidFailEvent -= onFailedPost;
				};
				onFailedPost = delegate(string unused)
				{
					TwitterManager.requestDidFinishEvent -= onSuccessPost;
					TwitterManager.requestDidFailEvent -= onFailedPost;
				};
				TwitterManager.requestDidFinishEvent += onSuccessPost;
				TwitterManager.requestDidFailEvent += onFailedPost;
			}
			this._postInProcess = true;
			TwitterController.TwitterFacade.PostStatusUpdate(status);
		};
		loginFailedAction = delegate(string unused)
		{
			TwitterManager.loginSucceededEvent -= postAction;
			TwitterManager.loginFailedEvent -= loginFailedAction;
		};
		if (TwitterController.TwitterFacade.IsLoggedIn())
		{
			postAction(string.Empty);
		}
		else
		{
			TwitterManager.loginSucceededEvent += postAction;
			TwitterManager.loginFailedEvent += loginFailedAction;
			TwitterController.TwitterFacade.ShowLoginDialog(delegate
			{
				postAction(string.Empty);
			});
		}
	}

	// Token: 0x06004E2E RID: 20014 RVA: 0x001C6168 File Offset: 0x001C4368
	public bool CanPostStatusUpdateWithPriority(FacebookController.StoryPriority priority)
	{
		try
		{
			if (priority == FacebookController.StoryPriority.Green)
			{
				return this.storiesPostHistory.Count < TwitterController.StoryPostLimits[priority];
			}
			return (from rec in this.storiesPostHistory
			where int.Parse((string)rec["priority"]) == (int)priority
			select rec).Count<Dictionary<string, object>>() < TwitterController.StoryPostLimits[priority];
		}
		catch (Exception arg)
		{
			Debug.LogError("Exeption in CanPostStoryWithPriority:\n" + arg);
		}
		return false;
	}

	// Token: 0x06004E2F RID: 20015 RVA: 0x001C6220 File Offset: 0x001C4420
	public static void CheckAndGiveTwitterReward(string context)
	{
		if (!TwitterController.TwitterSupported)
		{
			return;
		}
		if (Storager.getInt(Defs.IsTwitterLoginRewardaGained, true) == 0 && Storager.getInt(Defs.TwitterRewardGainStarted, false) == 1 && TwitterController.TwitterFacade.IsLoggedIn())
		{
			Storager.setInt(Defs.TwitterRewardGainStarted, 0, false);
			Storager.setInt(Defs.IsTwitterLoginRewardaGained, 1, true);
			BankController.AddGems(10, true, AnalyticsConstants.AccrualType.Earned);
			TutorialQuestManager.Instance.AddFulfilledQuest("loginTwitter");
			QuestMediator.NotifySocialInteraction("loginTwitter");
			AnalyticsFacade.SendCustomEvent("Virality", new Dictionary<string, object>
			{
				{
					"Login Twitter",
					context ?? "Unknown"
				}
			});
			AnalyticsFacade.SendCustomEventToAppsFlyer("Virality", new Dictionary<string, string>
			{
				{
					"Login Twitter",
					context ?? "Unknown"
				}
			});
		}
	}

	// Token: 0x17000CBD RID: 3261
	// (get) Token: 0x06004E30 RID: 20016 RVA: 0x001C62F4 File Offset: 0x001C44F4
	public static bool TwitterSupported
	{
		get
		{
			return BuildSettings.BuildTargetPlatform != RuntimePlatform.MetroPlayerX64;
		}
	}

	// Token: 0x17000CBE RID: 3262
	// (get) Token: 0x06004E31 RID: 20017 RVA: 0x001C6304 File Offset: 0x001C4504
	public static bool TwitterSupported_OldPosts
	{
		get
		{
			return TwitterController.TwitterSupported && BuildSettings.BuildTargetPlatform != RuntimePlatform.MetroPlayerX64 && TwitterController.Instance != null && !TwitterController.Instance.CanPostStatusUpdateWithPriority(FacebookController.StoryPriority.Green);
		}
	}

	// Token: 0x06004E32 RID: 20018 RVA: 0x001C6340 File Offset: 0x001C4540
	private void OnTwitterLogin(string result)
	{
		TwitterController.CheckAndGiveTwitterReward(this._loginContext);
		string message = string.Format("TwitterController.OnTwitterLogin(“{0}”)    {1}", result, this._loginContext);
		Debug.Log(message);
		this._loginContext = "Unknown";
	}

	// Token: 0x06004E33 RID: 20019 RVA: 0x001C637C File Offset: 0x001C457C
	private void OnTwitterLoginFailed(string error)
	{
		string message = string.Format("TwitterController.OnTwitterLoginFailed(“{0}”)    {1}", error, this._loginContext);
		Debug.Log(message);
		this._loginContext = "Unknown";
	}

	// Token: 0x06004E34 RID: 20020 RVA: 0x001C63AC File Offset: 0x001C45AC
	private void OnTwitterPost(object result)
	{
		this._postInProcess = false;
		Debug.Log(("TwitterController: OnTwitterPost: " + result) ?? string.Empty);
	}

	// Token: 0x06004E35 RID: 20021 RVA: 0x001C63D4 File Offset: 0x001C45D4
	private void OnTwitterPostFailed(string _error)
	{
		this._postInProcess = false;
		Debug.Log(("TwitterController: OnTwitterPostFailed: " + _error) ?? string.Empty);
	}

	// Token: 0x06004E36 RID: 20022 RVA: 0x001C63FC File Offset: 0x001C45FC
	private void HandleNewTwitterLimitsAvailable(int greenLimit, int redLimit, int multyWinLimit, int arenaLimit)
	{
		TwitterController.StoryPostLimits[FacebookController.StoryPriority.Green] = greenLimit;
		TwitterController.StoryPostLimits[FacebookController.StoryPriority.Red] = redLimit;
		TwitterController.StoryPostLimits[FacebookController.StoryPriority.MultyWinLimit] = multyWinLimit;
		TwitterController.StoryPostLimits[FacebookController.StoryPriority.ArenaLimit] = arenaLimit;
	}

	// Token: 0x06004E37 RID: 20023 RVA: 0x001C643C File Offset: 0x001C463C
	private void OnApplicationPause(bool pause)
	{
		if (pause)
		{
			this.SaveStoryPostHistory();
		}
		else
		{
			this.LoadStoryPostHistory();
		}
	}

	// Token: 0x06004E38 RID: 20024 RVA: 0x001C6458 File Offset: 0x001C4658
	private void Update()
	{
		if (Time.realtimeSinceStartup - this._timeSinceLastStoryPostHistoryClean > 10f)
		{
			this.CleanStoryPostHistory();
		}
	}

	// Token: 0x06004E39 RID: 20025 RVA: 0x001C6478 File Offset: 0x001C4678
	private void OnDestroy()
	{
		this.SaveStoryPostHistory();
		if (TwitterController.TwitterSupported)
		{
			FriendsController.NewTwitterLimitsAvailable -= this.HandleNewTwitterLimitsAvailable;
		}
	}

	// Token: 0x06004E3A RID: 20026 RVA: 0x001C649C File Offset: 0x001C469C
	private void SaveStoryPostHistory()
	{
		Storager.setString("TwitterControllerStoryPostHistoryKey", Rilisoft.MiniJson.Json.Serialize(this.storiesPostHistory), false);
	}

	// Token: 0x06004E3B RID: 20027 RVA: 0x001C64B4 File Offset: 0x001C46B4
	private void LoadStoryPostHistory()
	{
		try
		{
			List<object> list = Rilisoft.MiniJson.Json.Deserialize(Storager.getString("TwitterControllerStoryPostHistoryKey", false)) as List<object>;
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

	// Token: 0x06004E3C RID: 20028 RVA: 0x001C6578 File Offset: 0x001C4778
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
			Debug.LogError("TwitterController Exeption in CleanStoryPostHistory:\n" + arg);
		}
	}

	// Token: 0x06004E3D RID: 20029 RVA: 0x001C65FC File Offset: 0x001C47FC
	private void RegisterStoryPostedWithPriorityCore(FacebookController.StoryPriority priority)
	{
		if (!TwitterController.TwitterSupported)
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

	// Token: 0x06004E3E RID: 20030 RVA: 0x001C6654 File Offset: 0x001C4854
	private void InitStoryPostHistoryKey()
	{
		if (!Storager.hasKey("TwitterControllerStoryPostHistoryKey"))
		{
			Storager.setString("TwitterControllerStoryPostHistoryKey", "[]", false);
		}
	}

	// Token: 0x06004E3F RID: 20031 RVA: 0x001C6678 File Offset: 0x001C4878
	private static TwitterController.TwitterFacadeBase InitializeFacade()
	{
		if (!TwitterController.TwitterSupported)
		{
			return new TwitterController.DummyTwitterFacade();
		}
		switch (BuildSettings.BuildTargetPlatform)
		{
		case RuntimePlatform.IPhonePlayer:
			return new TwitterController.IosTwitterFacade();
		case RuntimePlatform.Android:
			return new TwitterController.AndroidTwitterFacade();
		}
		return new TwitterController.DummyTwitterFacade();
	}

	// Token: 0x17000CBF RID: 3263
	// (get) Token: 0x06004E40 RID: 20032 RVA: 0x001C66CC File Offset: 0x001C48CC
	private static TwitterController.TwitterFacadeBase TwitterFacade
	{
		get
		{
			return TwitterController._twitterFacade.Value;
		}
	}

	// Token: 0x04003CE9 RID: 15593
	public const int DefaultGreenPriorityLimit = 7;

	// Token: 0x04003CEA RID: 15594
	public const int DefaultRedPriorityLimit = 3;

	// Token: 0x04003CEB RID: 15595
	public const int DefaultMultyWinPriorityLimit = 1;

	// Token: 0x04003CEC RID: 15596
	public const int DefaultArenaPriorityLimit = 1;

	// Token: 0x04003CED RID: 15597
	private const string DefaultCallerContext = "Unknown";

	// Token: 0x04003CEE RID: 15598
	private const string StoryPostHistoryKey = "TwitterControllerStoryPostHistoryKey";

	// Token: 0x04003CEF RID: 15599
	private const string PriorityKey = "priority";

	// Token: 0x04003CF0 RID: 15600
	private const string TimeKey = "time";

	// Token: 0x04003CF1 RID: 15601
	public static TwitterController Instance;

	// Token: 0x04003CF2 RID: 15602
	public static readonly Dictionary<FacebookController.StoryPriority, int> StoryPostLimits = new Dictionary<FacebookController.StoryPriority, int>(4, new FacebookController.StoryPriorityComparer())
	{
		{
			FacebookController.StoryPriority.Green,
			7
		},
		{
			FacebookController.StoryPriority.Red,
			3
		},
		{
			FacebookController.StoryPriority.MultyWinLimit,
			1
		},
		{
			FacebookController.StoryPriority.ArenaLimit,
			1
		}
	};

	// Token: 0x04003CF3 RID: 15603
	private string _loginContext = "Unknown";

	// Token: 0x04003CF4 RID: 15604
	private static readonly Lazy<TwitterController.TwitterFacadeBase> _twitterFacade = new Lazy<TwitterController.TwitterFacadeBase>(new Func<TwitterController.TwitterFacadeBase>(TwitterController.InitializeFacade));

	// Token: 0x04003CF5 RID: 15605
	private bool _postInProcess;

	// Token: 0x04003CF6 RID: 15606
	private float _timeSinceLastStoryPostHistoryClean;

	// Token: 0x04003CF7 RID: 15607
	private List<Dictionary<string, object>> storiesPostHistory = new List<Dictionary<string, object>>();

	// Token: 0x02000874 RID: 2164
	private abstract class TwitterFacadeBase
	{
		// Token: 0x06004E42 RID: 20034
		public abstract void Init(string consumerKey, string consumerSecret);

		// Token: 0x06004E43 RID: 20035
		public abstract bool IsLoggedIn();

		// Token: 0x06004E44 RID: 20036
		public abstract void PostStatusUpdate(string status);

		// Token: 0x06004E45 RID: 20037
		public abstract void ShowLoginDialog(Action WP8customOnSuccessLogin = null);

		// Token: 0x06004E46 RID: 20038
		public abstract void Logout();
	}

	// Token: 0x02000875 RID: 2165
	private class IosTwitterFacade : TwitterController.TwitterFacadeBase
	{
		// Token: 0x06004E48 RID: 20040 RVA: 0x001C66E8 File Offset: 0x001C48E8
		public override void Init(string consumerKey, string consumerSecret)
		{
		}

		// Token: 0x06004E49 RID: 20041 RVA: 0x001C66EC File Offset: 0x001C48EC
		public override bool IsLoggedIn()
		{
			throw new NotSupportedException();
		}

		// Token: 0x06004E4A RID: 20042 RVA: 0x001C66F4 File Offset: 0x001C48F4
		public string LoggedInUsername()
		{
			throw new NotSupportedException();
		}

		// Token: 0x06004E4B RID: 20043 RVA: 0x001C66FC File Offset: 0x001C48FC
		public override void PostStatusUpdate(string status)
		{
		}

		// Token: 0x06004E4C RID: 20044 RVA: 0x001C6700 File Offset: 0x001C4900
		public override void ShowLoginDialog(Action WP8customOnSuccessLogin = null)
		{
		}

		// Token: 0x06004E4D RID: 20045 RVA: 0x001C6704 File Offset: 0x001C4904
		public override void Logout()
		{
		}
	}

	// Token: 0x02000876 RID: 2166
	private class AndroidTwitterFacade : TwitterController.TwitterFacadeBase
	{
		// Token: 0x06004E4F RID: 20047 RVA: 0x001C6710 File Offset: 0x001C4910
		public override void Init(string consumerKey, string consumerSecret)
		{
			TwitterAndroid.init(consumerKey, consumerSecret, "twitterplugin");
		}

		// Token: 0x06004E50 RID: 20048 RVA: 0x001C6720 File Offset: 0x001C4920
		public override bool IsLoggedIn()
		{
			return TwitterAndroid.isLoggedIn();
		}

		// Token: 0x06004E51 RID: 20049 RVA: 0x001C6728 File Offset: 0x001C4928
		public override void PostStatusUpdate(string status)
		{
			TwitterAndroid.postStatusUpdate(status);
		}

		// Token: 0x06004E52 RID: 20050 RVA: 0x001C6730 File Offset: 0x001C4930
		public override void ShowLoginDialog(Action WP8customOnSuccessLogin = null)
		{
			TwitterAndroid.showLoginDialog(false);
		}

		// Token: 0x06004E53 RID: 20051 RVA: 0x001C6738 File Offset: 0x001C4938
		public override void Logout()
		{
			TwitterAndroid.logout();
		}
	}

	// Token: 0x02000877 RID: 2167
	private class DummyTwitterFacade : TwitterController.TwitterFacadeBase
	{
		// Token: 0x06004E55 RID: 20053 RVA: 0x001C6748 File Offset: 0x001C4948
		public override void Init(string consumerKey, string consumerSecret)
		{
		}

		// Token: 0x06004E56 RID: 20054 RVA: 0x001C674C File Offset: 0x001C494C
		public override bool IsLoggedIn()
		{
			return BuildSettings.BuildTargetPlatform != RuntimePlatform.MetroPlayerX64;
		}

		// Token: 0x06004E57 RID: 20055 RVA: 0x001C675C File Offset: 0x001C495C
		public override void PostStatusUpdate(string status)
		{
		}

		// Token: 0x06004E58 RID: 20056 RVA: 0x001C6760 File Offset: 0x001C4960
		public override void ShowLoginDialog(Action WP8customOnSuccessLogin = null)
		{
		}

		// Token: 0x06004E59 RID: 20057 RVA: 0x001C6764 File Offset: 0x001C4964
		public override void Logout()
		{
		}
	}
}
