using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Rilisoft;
using Rilisoft.MiniJson;
using UnityEngine;

// Token: 0x020004A4 RID: 1188
public sealed class PromoActionsManager : MonoBehaviour
{
	// Token: 0x14000032 RID: 50
	// (add) Token: 0x06002A70 RID: 10864 RVA: 0x000E1248 File Offset: 0x000DF448
	// (remove) Token: 0x06002A71 RID: 10865 RVA: 0x000E1260 File Offset: 0x000DF460
	public static event Action OnLockedItemsUpdated;

	// Token: 0x14000033 RID: 51
	// (add) Token: 0x06002A72 RID: 10866 RVA: 0x000E1278 File Offset: 0x000DF478
	// (remove) Token: 0x06002A73 RID: 10867 RVA: 0x000E1290 File Offset: 0x000DF490
	public static event Action ActionsUUpdated;

	// Token: 0x14000034 RID: 52
	// (add) Token: 0x06002A74 RID: 10868 RVA: 0x000E12A8 File Offset: 0x000DF4A8
	// (remove) Token: 0x06002A75 RID: 10869 RVA: 0x000E12C0 File Offset: 0x000DF4C0
	public static event Action EventX3Updated;

	// Token: 0x14000035 RID: 53
	// (add) Token: 0x06002A76 RID: 10870 RVA: 0x000E12D8 File Offset: 0x000DF4D8
	// (remove) Token: 0x06002A77 RID: 10871 RVA: 0x000E12F0 File Offset: 0x000DF4F0
	public static event Action EventAmazonX3Updated;

	// Token: 0x14000036 RID: 54
	// (add) Token: 0x06002A78 RID: 10872 RVA: 0x000E1308 File Offset: 0x000DF508
	// (remove) Token: 0x06002A79 RID: 10873 RVA: 0x000E1320 File Offset: 0x000DF520
	public static event Action BestBuyStateUpdate;

	// Token: 0x14000037 RID: 55
	// (add) Token: 0x06002A7A RID: 10874 RVA: 0x000E1338 File Offset: 0x000DF538
	// (remove) Token: 0x06002A7B RID: 10875 RVA: 0x000E1350 File Offset: 0x000DF550
	public static event PromoActionsManager.OnDayOfValorEnableDelegate OnDayOfValorEnable;

	// Token: 0x1700073E RID: 1854
	// (get) Token: 0x06002A7C RID: 10876 RVA: 0x000E1368 File Offset: 0x000DF568
	public List<string> news
	{
		get
		{
			return this.m_news;
		}
	}

	// Token: 0x1700073F RID: 1855
	// (get) Token: 0x06002A7D RID: 10877 RVA: 0x000E1370 File Offset: 0x000DF570
	public bool IsEventX3Active
	{
		get
		{
			return this._eventX3Active;
		}
	}

	// Token: 0x06002A7E RID: 10878 RVA: 0x000E1378 File Offset: 0x000DF578
	public static void FireUnlockedItemsUpdated()
	{
		Action onLockedItemsUpdated = PromoActionsManager.OnLockedItemsUpdated;
		if (onLockedItemsUpdated != null)
		{
			onLockedItemsUpdated();
		}
	}

	// Token: 0x06002A7F RID: 10879 RVA: 0x000E1398 File Offset: 0x000DF598
	public void RemoveItemFromUnlocked(string item)
	{
		try
		{
			this.UnlockedItems.Remove(item);
			this.ItemsToRemoveFromUnlocked.Remove(item);
		}
		catch (Exception ex)
		{
			Debug.LogErrorFormat("Exception in RemoveItemFromUnlocked: {0}", new object[]
			{
				ex
			});
		}
	}

	// Token: 0x06002A80 RID: 10880 RVA: 0x000E13FC File Offset: 0x000DF5FC
	public void ReplaceUnlockedItemsWith(List<string> itemsViewed)
	{
		try
		{
			this.UnlockedItems.Clear();
			this.UnlockedItems.AddRange(itemsViewed);
		}
		catch (Exception ex)
		{
			Debug.LogErrorFormat("Exception in ReplaceUnlockedItemsWith: {0}", new object[]
			{
				ex
			});
		}
	}

	// Token: 0x06002A81 RID: 10881 RVA: 0x000E145C File Offset: 0x000DF65C
	public void RemoveViewedUnlockedItems()
	{
		this.ItemsToRemoveFromUnlocked.Clear();
	}

	// Token: 0x06002A82 RID: 10882 RVA: 0x000E146C File Offset: 0x000DF66C
	public int ItemsViewed(List<string> itemsViewed)
	{
		int result;
		try
		{
			List<string> itemsToRemoveFromUnlocked = this.UnlockedItems.Intersect(itemsViewed).ToList<string>();
			int num = itemsToRemoveFromUnlocked.Count<string>();
			if (num > 0)
			{
				this.UnlockedItems.RemoveAll((string item) => itemsToRemoveFromUnlocked.Contains(item));
				List<string> second = itemsToRemoveFromUnlocked.ToList<string>();
				this.ItemsToRemoveFromUnlocked = this.ItemsToRemoveFromUnlocked.Union(second).ToList<string>();
			}
			result = num;
		}
		catch (Exception ex)
		{
			Debug.LogErrorFormat("Exception in ItemsViewed: {0}", new object[]
			{
				ex
			});
			result = 0;
		}
		return result;
	}

	// Token: 0x17000740 RID: 1856
	// (get) Token: 0x06002A83 RID: 10883 RVA: 0x000E1534 File Offset: 0x000DF734
	// (set) Token: 0x06002A84 RID: 10884 RVA: 0x000E153C File Offset: 0x000DF73C
	public List<string> UnlockedItems
	{
		get
		{
			return this.m_unlockedItems;
		}
		private set
		{
			this.m_unlockedItems = value;
		}
	}

	// Token: 0x17000741 RID: 1857
	// (get) Token: 0x06002A85 RID: 10885 RVA: 0x000E1548 File Offset: 0x000DF748
	// (set) Token: 0x06002A86 RID: 10886 RVA: 0x000E1550 File Offset: 0x000DF750
	public List<string> ItemsToRemoveFromUnlocked
	{
		get
		{
			return this.m_itemsToRemoveFromUnlocked;
		}
		private set
		{
			this.m_itemsToRemoveFromUnlocked = value;
		}
	}

	// Token: 0x17000742 RID: 1858
	// (get) Token: 0x06002A87 RID: 10887 RVA: 0x000E155C File Offset: 0x000DF75C
	public bool IsAmazonEventX3Active
	{
		get
		{
			if (this._amazonEventInfo == null)
			{
				return false;
			}
			if (this._amazonEventInfo.DurationSeconds <= 1E-45f)
			{
				return false;
			}
			if (!PromoActionsManager.CheckTimezone(this._amazonEventInfo.Timezones))
			{
				return false;
			}
			DateTime utcNow = DateTime.UtcNow;
			return this._amazonEventInfo.StartTime <= utcNow && utcNow <= this._amazonEventInfo.EndTime;
		}
	}

	// Token: 0x17000743 RID: 1859
	// (get) Token: 0x06002A88 RID: 10888 RVA: 0x000E15D8 File Offset: 0x000DF7D8
	public long EventX3RemainedTime
	{
		get
		{
			if (this.IsEventX3Active)
			{
				return this._eventX3StartTime + this._eventX3Duration - PromoActionsManager.CurrentUnixTime;
			}
			return 0L;
		}
	}

	// Token: 0x17000744 RID: 1860
	// (get) Token: 0x06002A89 RID: 10889 RVA: 0x000E15FC File Offset: 0x000DF7FC
	public static PromoActionsManager.AdvertInfo Advert
	{
		get
		{
			return (!StoreKitEventListener.IsPayingUser()) ? PromoActionsManager._freeAdvert : PromoActionsManager._paidAdvert;
		}
	}

	// Token: 0x17000745 RID: 1861
	// (get) Token: 0x06002A8A RID: 10890 RVA: 0x000E1618 File Offset: 0x000DF818
	internal static PromoActionsManager.ReplaceAdmobPerelivInfo ReplaceAdmobPereliv
	{
		get
		{
			return PromoActionsManager._replaceAdmobPereliv;
		}
	}

	// Token: 0x17000746 RID: 1862
	// (get) Token: 0x06002A8B RID: 10891 RVA: 0x000E1620 File Offset: 0x000DF820
	public static PromoActionsManager.MobileAdvertInfo MobileAdvert
	{
		get
		{
			return PromoActionsManager._mobileAdvert;
		}
	}

	// Token: 0x17000747 RID: 1863
	// (get) Token: 0x06002A8C RID: 10892 RVA: 0x000E1628 File Offset: 0x000DF828
	// (set) Token: 0x06002A8D RID: 10893 RVA: 0x000E1630 File Offset: 0x000DF830
	public static bool MobileAdvertIsReady { get; private set; }

	// Token: 0x06002A8E RID: 10894 RVA: 0x000E1638 File Offset: 0x000DF838
	private void Awake()
	{
		this.LoadUnlockedItems();
		PromoActionsManager.startupTime = Time.realtimeSinceStartup;
		this.promoActionAddress = URLs.PromoActions;
	}

	// Token: 0x06002A8F RID: 10895 RVA: 0x000E1658 File Offset: 0x000DF858
	private void Start()
	{
		PromoActionsManager.sharedManager = this;
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		Task futureToWait = PersistentCacheManager.Instance.StartDownloadSignaturesLoop();
		base.StartCoroutine(this.GetActionsLoop(futureToWait));
		base.StartCoroutine(this.GetEventX3InfoLoop());
		base.StartCoroutine(this.GetAdvertInfoLoop(futureToWait));
		base.StartCoroutine(AdsConfigManager.Instance.GetAdvertInfoLoop(futureToWait));
		base.StartCoroutine(PerelivConfigManager.Instance.GetPerelivConfigLoop(futureToWait));
		base.StartCoroutine(this.GetBestBuyInfoLoop(futureToWait));
		base.StartCoroutine(this.GetDayOfValorInfoLoop());
	}

	// Token: 0x06002A90 RID: 10896 RVA: 0x000E16EC File Offset: 0x000DF8EC
	private void Update()
	{
		if (Time.realtimeSinceStartup - this._eventX3LastCheckTime >= 1f)
		{
			this.CheckEventX3Active();
			if (Time.frameCount % 120 == 0)
			{
				this.RefreshAmazonEvent();
			}
			this.CheckDayOfValorActive();
			this._eventX3LastCheckTime = Time.realtimeSinceStartup;
		}
	}

	// Token: 0x06002A91 RID: 10897 RVA: 0x000E173C File Offset: 0x000DF93C
	private IEnumerator OnApplicationPause(bool pause)
	{
		if (!pause)
		{
			yield return null;
			yield return null;
			yield return null;
			base.StartCoroutine(this.GetActions());
			base.StartCoroutine(this.GetEventX3Info());
			base.StartCoroutine(this.GetAmazonEventCoroutine());
			base.StartCoroutine(this.GetAdvertInfo());
			base.StartCoroutine(this.DownloadBestBuyInfo());
			base.StartCoroutine(this.DownloadDayOfValorInfo());
		}
		else
		{
			this.SaveUnlockedItems();
		}
		yield break;
	}

	// Token: 0x06002A92 RID: 10898 RVA: 0x000E1768 File Offset: 0x000DF968
	private IEnumerator GetActionsLoop(Task futureToWait)
	{
		yield return new WaitUntil(() => futureToWait.IsCompleted);
		while (!TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage < TrainingController.NewTrainingCompletedStage.ShootingRangeCompleted)
		{
			yield return null;
		}
		for (;;)
		{
			base.StartCoroutine(this.GetActions());
			while (Time.realtimeSinceStartup - this.startTime < 900f)
			{
				yield return null;
			}
		}
		yield break;
	}

	// Token: 0x06002A93 RID: 10899 RVA: 0x000E1794 File Offset: 0x000DF994
	private IEnumerator GetEventX3InfoLoop()
	{
		this.UpdateNewbieEventX3Info();
		for (;;)
		{
			yield return base.StartCoroutine(this.GetEventX3Info());
			yield return base.StartCoroutine(this.GetAmazonEventCoroutine());
			while (Time.realtimeSinceStartup - this._eventX3GetInfoStartTime < 930f)
			{
				yield return null;
			}
		}
		yield break;
	}

	// Token: 0x06002A94 RID: 10900 RVA: 0x000E17B0 File Offset: 0x000DF9B0
	private IEnumerator GetAdvertInfoLoop(Task futureToWait)
	{
		yield return new WaitUntil(() => futureToWait.IsCompleted);
		while (!TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage <= TrainingController.NewTrainingCompletedStage.None)
		{
			yield return null;
		}
		for (;;)
		{
			yield return base.StartCoroutine(this.GetAdvertInfo());
			while (Time.realtimeSinceStartup - this._advertGetInfoStartTime < 960f)
			{
				yield return null;
			}
		}
		yield break;
	}

	// Token: 0x06002A95 RID: 10901 RVA: 0x000E17DC File Offset: 0x000DF9DC
	private IEnumerator GetAmazonEventCoroutine()
	{
		if (BuildSettings.BuildTargetPlatform != RuntimePlatform.Android)
		{
			yield break;
		}
		if (Defs.AndroidEdition != Defs.RuntimeAndroidEdition.Amazon)
		{
			yield break;
		}
		WWW response = Tools.CreateWwwIfNotConnected(URLs.AmazonEvent);
		if (response == null)
		{
			yield break;
		}
		yield return response;
		string responseText = URLs.Sanitize(response);
		if (!string.IsNullOrEmpty(response.error))
		{
			Debug.LogWarning("Amazon event response error: " + response.error);
			yield break;
		}
		if (string.IsNullOrEmpty(responseText))
		{
			Debug.LogWarning("Amazon event response is empty");
			yield break;
		}
		Dictionary<string, object> amazonEvent = Json.Deserialize(responseText) as Dictionary<string, object>;
		if (amazonEvent == null)
		{
			Debug.LogWarning("Amazon event bad response: " + responseText);
			yield break;
		}
		if (this.IsNeedCheckAmazonEventX3())
		{
			this._amazonEventInfo = new PromoActionsManager.AmazonEventInfo();
			object startTimeObj;
			if (amazonEvent.TryGetValue("startTime", out startTimeObj))
			{
				try
				{
					this._amazonEventInfo.StartTime = Convert.ToDateTime(startTimeObj, CultureInfo.InvariantCulture);
				}
				catch (Exception ex6)
				{
					Exception ex = ex6;
					Debug.LogException(ex);
				}
			}
			object durationSecondsObj;
			if (amazonEvent.TryGetValue("durationSeconds", out durationSecondsObj))
			{
				try
				{
					this._amazonEventInfo.DurationSeconds = Convert.ToSingle(durationSecondsObj);
				}
				catch (Exception ex7)
				{
					Exception ex2 = ex7;
					Debug.LogException(ex2);
				}
			}
			object timezonesObj;
			if (amazonEvent.TryGetValue("timezones", out timezonesObj))
			{
				List<object> timezonesRaw = (timezonesObj as List<object>) ?? new List<object>();
				try
				{
					this._amazonEventInfo.Timezones = timezonesRaw.ConvertAll<int>(new Converter<object, int>(Convert.ToInt32));
				}
				catch (Exception ex8)
				{
					Exception ex3 = ex8;
					Debug.LogException(ex3);
				}
			}
			object percentageObj;
			if (amazonEvent.TryGetValue("percentage", out percentageObj))
			{
				try
				{
					this._amazonEventInfo.Percentage = Convert.ToSingle(percentageObj);
				}
				catch (Exception ex9)
				{
					Exception ex4 = ex9;
					Debug.LogException(ex4);
				}
			}
			object captionObj;
			if (amazonEvent.TryGetValue("caption", out captionObj))
			{
				try
				{
					this._amazonEventInfo.Caption = (Convert.ToString(captionObj, CultureInfo.InvariantCulture) ?? string.Empty);
				}
				catch (Exception ex10)
				{
					Exception ex5 = ex10;
					Debug.LogException(ex5);
				}
			}
			this.RefreshAmazonEvent();
		}
		yield break;
	}

	// Token: 0x17000748 RID: 1864
	// (get) Token: 0x06002A96 RID: 10902 RVA: 0x000E17F8 File Offset: 0x000DF9F8
	internal PromoActionsManager.AmazonEventInfo AmazonEvent
	{
		get
		{
			return this._amazonEventInfo;
		}
	}

	// Token: 0x06002A97 RID: 10903 RVA: 0x000E1800 File Offset: 0x000DFA00
	private IEnumerator GetEventX3Info()
	{
		if (this._isGetEventX3InfoRunning)
		{
			yield break;
		}
		this._eventX3GetInfoStartTime = Time.realtimeSinceStartup;
		this._isGetEventX3InfoRunning = true;
		if (string.IsNullOrEmpty(URLs.EventX3))
		{
			this._isGetEventX3InfoRunning = false;
			yield break;
		}
		WWW response = Tools.CreateWwwIfNotConnected(URLs.EventX3);
		yield return response;
		string responseText = URLs.Sanitize(response);
		if (response == null || !string.IsNullOrEmpty(response.error))
		{
			if (Application.isEditor)
			{
				Debug.LogWarningFormat("EventX3 response error: {0}", new object[]
				{
					(response == null) ? "null" : response.error
				});
			}
			this._isGetEventX3InfoRunning = false;
			this._eventX3GetInfoStartTime = Time.realtimeSinceStartup - 930f + 15f;
			yield break;
		}
		if (string.IsNullOrEmpty(responseText))
		{
			Debug.LogWarning("EventX3 response is empty");
			this._isGetEventX3InfoRunning = false;
			yield break;
		}
		object eventX3InfoObj = Json.Deserialize(responseText);
		Dictionary<string, object> eventX3Info = eventX3InfoObj as Dictionary<string, object>;
		if (eventX3Info == null || !eventX3Info.ContainsKey("start") || !eventX3Info.ContainsKey("duration"))
		{
			Debug.LogWarning("EventX3 response is bad");
			this._isGetEventX3InfoRunning = false;
			yield break;
		}
		long startTime = (long)eventX3Info["start"];
		long duration = (long)eventX3Info["duration"];
		this._eventX3StartTime = startTime;
		this._eventX3Duration = duration;
		this.CheckEventX3Active();
		PromoActionsManager.x3InfoDownloadaedOnceDuringCurrentRun = true;
		this._isGetEventX3InfoRunning = false;
		yield break;
	}

	// Token: 0x06002A98 RID: 10904 RVA: 0x000E181C File Offset: 0x000DFA1C
	private bool IsNeedCheckAmazonEventX3()
	{
		return Defs.IsDeveloperBuild || (BuildSettings.BuildTargetPlatform == RuntimePlatform.Android && Defs.AndroidEdition == Defs.RuntimeAndroidEdition.Amazon);
	}

	// Token: 0x06002A99 RID: 10905 RVA: 0x000E1854 File Offset: 0x000DFA54
	private static bool CheckTimezone(List<int> timezones)
	{
		return timezones != null && timezones.Any(new Func<int, bool>(DateTimeOffset.Now.Offset.Hours.Equals));
	}

	// Token: 0x06002A9A RID: 10906 RVA: 0x000E1898 File Offset: 0x000DFA98
	private bool CheckAvailabelTimeZoneForAmazonEvent()
	{
		if (!this._eventX3Active)
		{
			return false;
		}
		if (this._eventX3AmazonEventValidTimeZone == null || this._eventX3AmazonEventValidTimeZone.Count == 0)
		{
			return false;
		}
		TimeSpan offset = DateTimeOffset.Now.Offset;
		for (int i = 0; i < this._eventX3AmazonEventValidTimeZone.Count; i++)
		{
			long num = this._eventX3AmazonEventValidTimeZone[i];
			if (num == (long)offset.Hours)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06002A9B RID: 10907 RVA: 0x000E1918 File Offset: 0x000DFB18
	private void ParseAmazonEventData(Dictionary<string, object> jsonData)
	{
		if (jsonData.ContainsKey("startAmazonEventTime"))
		{
			this._eventX3AmazonEventStartTime = (long)jsonData["startAmazonEventTime"];
		}
		if (jsonData.ContainsKey("endAmazonEventTime"))
		{
			this._eventX3AmazonEventEndTime = (long)jsonData["endAmazonEventTime"];
		}
		if (jsonData.ContainsKey("timeZonesForEventAmazon"))
		{
			List<object> list = jsonData["timeZonesForEventAmazon"] as List<object>;
			for (int i = 0; i < list.Count; i++)
			{
				this._eventX3AmazonEventValidTimeZone.Add((long)list[i]);
			}
		}
	}

	// Token: 0x06002A9C RID: 10908 RVA: 0x000E19C0 File Offset: 0x000DFBC0
	private void RefreshAmazonEvent()
	{
		if (PromoActionsManager.EventAmazonX3Updated != null)
		{
			PromoActionsManager.EventAmazonX3Updated();
		}
	}

	// Token: 0x06002A9D RID: 10909 RVA: 0x000E19D8 File Offset: 0x000DFBD8
	[Obsolete]
	private void CheckAmazonEventX3Active()
	{
		if (!this._eventX3Active || !this.CheckAvailabelTimeZoneForAmazonEvent())
		{
			this._eventX3AmazonEventActive = false;
			return;
		}
		bool eventX3AmazonEventActive = this._eventX3AmazonEventActive;
		if (this._eventX3AmazonEventStartTime != 0L && this._eventX3AmazonEventEndTime != 0L)
		{
			long currentUnixTime = PromoActionsManager.CurrentUnixTime;
			this._eventX3AmazonEventActive = (this._eventX3StartTime < currentUnixTime && currentUnixTime < this._eventX3AmazonEventEndTime);
		}
		else
		{
			this._eventX3AmazonEventStartTime = 0L;
			this._eventX3AmazonEventEndTime = 0L;
			this._eventX3AmazonEventActive = false;
		}
		if (this._eventX3AmazonEventActive != eventX3AmazonEventActive && PromoActionsManager.EventAmazonX3Updated != null)
		{
			PromoActionsManager.EventAmazonX3Updated();
		}
	}

	// Token: 0x06002A9E RID: 10910 RVA: 0x000E1A80 File Offset: 0x000DFC80
	public void ForceCheckEventX3Active()
	{
		this.CheckEventX3Active();
	}

	// Token: 0x06002A9F RID: 10911 RVA: 0x000E1A88 File Offset: 0x000DFC88
	private void CheckEventX3Active()
	{
		bool eventX3Active = this._eventX3Active;
		if (this.IsNewbieEventX3Active)
		{
			this._eventX3StartTime = this._newbieEventX3StartTime;
			this._eventX3Duration = 259200L;
			this._eventX3Active = true;
		}
		else if (this._eventX3StartTime != 0L && this._eventX3Duration != 0L && this.IsX3StartTimeAfterNewbieX3TimeoutEndTime)
		{
			long currentUnixTime = PromoActionsManager.CurrentUnixTime;
			this._eventX3Active = (this._eventX3StartTime < currentUnixTime && currentUnixTime < this._eventX3StartTime + this._eventX3Duration);
		}
		else
		{
			this._eventX3StartTime = 0L;
			this._eventX3Duration = 0L;
			this._eventX3Active = false;
		}
		if (this._eventX3Active != eventX3Active)
		{
			if (this._eventX3Active)
			{
				PlayerPrefs.SetInt(Defs.EventX3WindowShownCount, 1);
				PlayerPrefs.Save();
			}
			if (PromoActionsManager.EventX3Updated != null)
			{
				PromoActionsManager.EventX3Updated();
			}
		}
	}

	// Token: 0x17000749 RID: 1865
	// (get) Token: 0x06002AA0 RID: 10912 RVA: 0x000E1B70 File Offset: 0x000DFD70
	public bool IsNewbieEventX3Active
	{
		get
		{
			if (this._newbieEventX3StartTime == 0L)
			{
				return false;
			}
			long currentUnixTime = PromoActionsManager.CurrentUnixTime;
			long num = this._newbieEventX3StartTime + 259200L + 259200L;
			if (currentUnixTime >= num)
			{
				this.ResetNewbieX3StartTime();
				return false;
			}
			return this._newbieEventX3StartTime < currentUnixTime && currentUnixTime < this._newbieEventX3StartTime + 259200L;
		}
	}

	// Token: 0x1700074A RID: 1866
	// (get) Token: 0x06002AA1 RID: 10913 RVA: 0x000E1BD8 File Offset: 0x000DFDD8
	private bool IsX3StartTimeAfterNewbieX3TimeoutEndTime
	{
		get
		{
			if (this._newbieEventX3StartTimeAdditional == 0L)
			{
				return true;
			}
			long num = this._newbieEventX3StartTimeAdditional + 259200L + 259200L;
			return this._eventX3StartTime >= num;
		}
	}

	// Token: 0x06002AA2 RID: 10914 RVA: 0x000E1C14 File Offset: 0x000DFE14
	private void ResetNewbieX3StartTime()
	{
		if (this._newbieEventX3StartTime == 0L)
		{
			return;
		}
		Storager.setString(Defs.NewbieEventX3StartTime, 0L.ToString(), false);
		this._newbieEventX3StartTime = 0L;
	}

	// Token: 0x06002AA3 RID: 10915 RVA: 0x000E1C4C File Offset: 0x000DFE4C
	public static long GetUnixTimeFromStorage(string storageId)
	{
		long result = 0L;
		if (Storager.hasKey(storageId))
		{
			string @string = Storager.getString(storageId, false);
			long.TryParse(@string, out result);
		}
		return result;
	}

	// Token: 0x06002AA4 RID: 10916 RVA: 0x000E1C7C File Offset: 0x000DFE7C
	public void UpdateNewbieEventX3Info()
	{
		this._newbieEventX3StartTime = PromoActionsManager.GetUnixTimeFromStorage(Defs.NewbieEventX3StartTime);
		this._newbieEventX3StartTimeAdditional = PromoActionsManager.GetUnixTimeFromStorage(Defs.NewbieEventX3StartTimeAdditional);
	}

	// Token: 0x06002AA5 RID: 10917 RVA: 0x000E1CAC File Offset: 0x000DFEAC
	private long GetNewbieEventX3LastLoggedTime()
	{
		if (this._newbieEventX3StartTime != 0L)
		{
			return PromoActionsManager.GetUnixTimeFromStorage(Defs.NewbieEventX3LastLoggedTime);
		}
		return 0L;
	}

	// Token: 0x06002AA6 RID: 10918 RVA: 0x000E1CC8 File Offset: 0x000DFEC8
	private IEnumerator GetAdvertInfo()
	{
		if (this._isGetAdvertInfoRunning)
		{
			yield break;
		}
		this._advertGetInfoStartTime = Time.realtimeSinceStartup;
		this._isGetAdvertInfoRunning = true;
		PromoActionsManager._paidAdvert.enabled = false;
		PromoActionsManager._freeAdvert.enabled = false;
		string url = URLs.Advert;
		if (string.IsNullOrEmpty(url))
		{
			this._isGetAdvertInfoRunning = false;
			yield break;
		}
		string cachedResponse = PersistentCacheManager.Instance.GetValue(url);
		string responseText;
		if (!string.IsNullOrEmpty(cachedResponse))
		{
			responseText = cachedResponse;
		}
		else
		{
			WWW response = Tools.CreateWwwIfNotConnected(url);
			yield return response;
			if (response == null || !string.IsNullOrEmpty(response.error))
			{
				Debug.LogWarningFormat("Advert response error: {0}", new object[]
				{
					(response == null) ? "null" : response.error
				});
				this._isGetAdvertInfoRunning = false;
				yield break;
			}
			responseText = URLs.Sanitize(response);
			if (string.IsNullOrEmpty(responseText))
			{
				Debug.LogWarning("Advert response is empty");
				this._isGetAdvertInfoRunning = false;
				yield break;
			}
			PersistentCacheManager.Instance.SetValue(response.url, responseText);
		}
		object advertInfoObj = Json.Deserialize(responseText);
		Dictionary<string, object> advertInfo = advertInfoObj as Dictionary<string, object>;
		if (advertInfoObj == null)
		{
			this._isGetAdvertInfoRunning = false;
			yield break;
		}
		if (advertInfo.ContainsKey("paid"))
		{
			this.ParseAdvertInfo(advertInfo["paid"], PromoActionsManager._paidAdvert);
		}
		if (advertInfo.ContainsKey("free"))
		{
			this.ParseAdvertInfo(advertInfo["free"], PromoActionsManager._freeAdvert);
		}
		if (advertInfo.ContainsKey("replace_admob_pereliv_10_2_0"))
		{
			Dictionary<string, object> replaceAdmob = advertInfo["replace_admob_pereliv_10_2_0"] as Dictionary<string, object>;
			PromoActionsManager.ParseReplaceAdmobPereliv(replaceAdmob, PromoActionsManager._replaceAdmobPereliv);
		}
		else
		{
			Debug.Log("Advert response doesn't contain “replace_admob_pereliv_10_2_0” property.");
		}
		this._isGetAdvertInfoRunning = false;
		PromoActionsManager.MobileAdvertIsReady = true;
		yield break;
	}

	// Token: 0x06002AA7 RID: 10919 RVA: 0x000E1CE4 File Offset: 0x000DFEE4
	private static void ParseReplaceAdmobPereliv(Dictionary<string, object> replaceAdmob, PromoActionsManager.ReplaceAdmobPerelivInfo replaceAdmobObj)
	{
		if (replaceAdmob != null)
		{
			try
			{
				List<string> list = (replaceAdmob["imageUrls"] as List<object>).OfType<string>().ToList<string>();
				foreach (string item in list)
				{
					replaceAdmobObj.imageUrls.Add(item);
				}
				List<string> list2 = (replaceAdmob["adUrls"] as List<object>).OfType<string>().ToList<string>();
				foreach (string item2 in list2)
				{
					replaceAdmobObj.adUrls.Add(item2);
				}
				replaceAdmobObj.enabled = Convert.ToBoolean(replaceAdmob["enabled"]);
				replaceAdmobObj.ShowEveryTimes = Mathf.Max(Convert.ToInt32(replaceAdmob["showEveryTimes"]), 1);
				replaceAdmobObj.ShowTimesTotal = Mathf.Max(Convert.ToInt32(replaceAdmob["showTimesTotal"]), 0);
				replaceAdmobObj.ShowToPaying = Convert.ToBoolean(replaceAdmob["showToPaying"]);
				replaceAdmobObj.ShowToNew = Convert.ToBoolean(replaceAdmob["showToNew"]);
				try
				{
					replaceAdmobObj.MinLevel = Convert.ToInt32(replaceAdmob["minLevel"]);
				}
				catch
				{
					replaceAdmobObj.MinLevel = -1;
				}
				try
				{
					replaceAdmobObj.MaxLevel = Convert.ToInt32(replaceAdmob["maxLevel"]);
				}
				catch
				{
					replaceAdmobObj.MaxLevel = -1;
				}
			}
			catch (Exception message)
			{
				Debug.LogWarning(message);
			}
		}
		else
		{
			Debug.LogWarning("replaceAdmob == null");
		}
	}

	// Token: 0x06002AA8 RID: 10920 RVA: 0x000E1F14 File Offset: 0x000E0114
	private void ParseAdvertInfo(object advertInfoObj, PromoActionsManager.AdvertInfo advertInfo)
	{
		Dictionary<string, object> dictionary = advertInfoObj as Dictionary<string, object>;
		if (dictionary == null)
		{
			return;
		}
		advertInfo.imageUrl = Convert.ToString(dictionary["imageUrl"]);
		advertInfo.adUrl = Convert.ToString(dictionary["adUrl"]);
		advertInfo.message = Convert.ToString(dictionary["text"]);
		advertInfo.showAlways = Convert.ToBoolean(dictionary["showAlways"]);
		advertInfo.btnClose = Convert.ToBoolean(dictionary["btnClose"]);
		advertInfo.minLevel = Convert.ToInt32(dictionary["minLevel"]);
		advertInfo.maxLevel = Convert.ToInt32(dictionary["maxLevel"]);
		advertInfo.enabled = Convert.ToBoolean(dictionary["enabled"]);
	}

	// Token: 0x1700074B RID: 1867
	// (get) Token: 0x06002AA9 RID: 10921 RVA: 0x000E1FE0 File Offset: 0x000E01E0
	public static long CurrentUnixTime
	{
		get
		{
			DateTime d = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
			return (long)(DateTime.UtcNow - d).TotalSeconds;
		}
	}

	// Token: 0x06002AAA RID: 10922 RVA: 0x000E2018 File Offset: 0x000E0218
	private void ClearAll()
	{
		this.discounts.Clear();
		this.topSellers.Clear();
	}

	// Token: 0x06002AAB RID: 10923 RVA: 0x000E2030 File Offset: 0x000E0230
	public static List<string> AllIdsForPromosExceptArmor()
	{
		IEnumerable<string> second = from kvp in WeaponManager.tagToStoreIDMapping
		where kvp.Value != null && WeaponManager.storeIDtoDefsSNMapping.ContainsKey(kvp.Value)
		select kvp.Key;
		return Wear.wear.SelectMany((KeyValuePair<ShopNGUIController.CategoryNames, List<List<string>>> kvp) => (kvp.Key == ShopNGUIController.CategoryNames.ArmorCategory) ? new List<List<string>>() : kvp.Value).SelectMany((List<string> list) => list).Except(new List<string>
		{
			"hat_Adamant_3"
		}).Except(Wear.wear[ShopNGUIController.CategoryNames.HatsCategory][0]).Concat(SkinsController.shopKeyFromNameSkin.Values).Concat(second).Concat(from info in WeaponSkinsManager.AllSkins
		select info.Id).Concat(GadgetsInfo.info.Keys).Distinct<string>().ToList<string>();
	}

	// Token: 0x06002AAC RID: 10924 RVA: 0x000E215C File Offset: 0x000E035C
	public IEnumerator GetActions()
	{
		this.startTime = Time.realtimeSinceStartup;
		string cachedResponse = PersistentCacheManager.Instance.GetValue(this.promoActionAddress);
		string responseText;
		if (!string.IsNullOrEmpty(cachedResponse))
		{
			responseText = cachedResponse;
		}
		else
		{
			WWW download = Tools.CreateWwwIfNotConnected(this.promoActionAddress);
			if (download == null)
			{
				yield break;
			}
			yield return download;
			string response = URLs.Sanitize(download);
			if (string.IsNullOrEmpty(download.error) && !string.IsNullOrEmpty(response) && Debug.isDebugBuild)
			{
				Debug.Log("GetActions response:    " + response);
			}
			if (!string.IsNullOrEmpty(download.error))
			{
				if (Debug.isDebugBuild)
				{
					Debug.LogWarning("GetActions error:    " + download.error);
				}
				this.ClearAll();
				PromoActionsManager.ActionsAvailable = false;
				yield break;
			}
			responseText = response;
			PersistentCacheManager.Instance.SetValue(download.url, responseText);
		}
		if (this._previousResponseText != null && responseText != null && responseText == this._previousResponseText)
		{
			if (Debug.isDebugBuild)
			{
				Debug.Log("GetActions:    responseText == _previousResponseText");
			}
			yield break;
		}
		this._previousResponseText = responseText;
		PromoActionsManager.ActionsAvailable = true;
		this.ClearAll();
		Dictionary<string, object> actions = Json.Deserialize(responseText) as Dictionary<string, object>;
		if (actions == null)
		{
			if (Debug.isDebugBuild || Application.isEditor)
			{
				Debug.LogWarning(" GetActions actions = null");
			}
			yield break;
		}
		object discountsObj;
		if (actions.TryGetValue("discounts_up", out discountsObj))
		{
			List<object> discountsObjList = discountsObj as List<object>;
			if (discountsObjList != null)
			{
				try
				{
					var discountsAsObjects = (from list in discountsObjList.OfType<List<object>>()
					where list.Count > 1
					select new
					{
						ItemID = ((list[0] as string) ?? string.Empty),
						Discount = Convert.ToInt32((long)list[1])
					}).ToList();
					var shmotEntry = discountsAsObjects.FirstOrDefault(entry => entry.ItemID == "shmot");
					var armorEntry = discountsAsObjects.FirstOrDefault(entry => entry.ItemID == "armor");
					if (shmotEntry != null)
					{
						IEnumerable<string> idsNotInDiscounts = PromoActionsManager.AllIdsForPromosExceptArmor().Except(from item in discountsAsObjects
						select item.ItemID);
						foreach (string item2 in idsNotInDiscounts)
						{
							discountsAsObjects.Add(new
							{
								ItemID = item2,
								Discount = shmotEntry.Discount
							});
						}
						discountsAsObjects.RemoveAll(item => item.ItemID == "shmot");
					}
					if (armorEntry != null)
					{
						IEnumerable<string> armorIdsNotInDiscounts = Wear.wear[ShopNGUIController.CategoryNames.ArmorCategory][0].Except(from item in discountsAsObjects
						select item.ItemID);
						foreach (string armorItem in armorIdsNotInDiscounts)
						{
							discountsAsObjects.Add(new
							{
								ItemID = armorItem,
								Discount = armorEntry.Discount
							});
						}
						discountsAsObjects.RemoveAll(item => item.ItemID == "armor");
					}
					foreach (var discount in discountsAsObjects)
					{
						List<SaltedInt> vals = new List<SaltedInt>
						{
							new SaltedInt(11259645, discount.Discount)
						};
						this.discounts.Add(discount.ItemID, vals);
					}
				}
				catch (Exception ex)
				{
					Exception e = ex;
					Debug.LogError("Exception in processing discounts from server: " + e);
				}
			}
		}
		object topSellersObj;
		if (actions.TryGetValue("topSellers_up", out topSellersObj))
		{
			List<object> topSellersObjList = topSellersObj as List<object>;
			if (topSellersObjList != null)
			{
				foreach (object obj in topSellersObjList)
				{
					string tg = (string)obj;
					this.topSellers.Add(tg);
				}
			}
		}
		if (PromoActionsManager.ActionsUUpdated != null)
		{
			PromoActionsManager.ActionsUUpdated();
		}
		yield break;
	}

	// Token: 0x06002AAD RID: 10925 RVA: 0x000E2178 File Offset: 0x000E0378
	private IEnumerator DownloadBestBuyInfo()
	{
		if (this._isGetBestBuyInfoRunning)
		{
			yield break;
		}
		this._bestBuyGetInfoStartTime = Time.realtimeSinceStartup;
		this._isGetBestBuyInfoRunning = true;
		string url = URLs.BestBuy;
		if (string.IsNullOrEmpty(url))
		{
			this._isGetBestBuyInfoRunning = false;
			yield break;
		}
		string cachedResponse = PersistentCacheManager.Instance.GetValue(url);
		string responseText;
		if (!string.IsNullOrEmpty(cachedResponse))
		{
			responseText = cachedResponse;
		}
		else
		{
			WWW response = Tools.CreateWwwIfNotConnected(URLs.BestBuy);
			yield return response;
			if (response == null || !string.IsNullOrEmpty(response.error))
			{
				Debug.LogWarningFormat("Best buy response error: {0}", new object[]
				{
					(response == null) ? "null" : response.error
				});
				this._bestBuyIds.Clear();
				this._isGetBestBuyInfoRunning = false;
				yield break;
			}
			responseText = URLs.Sanitize(response);
			if (string.IsNullOrEmpty(responseText))
			{
				Debug.LogWarning("Best buy response is empty");
				this._bestBuyIds.Clear();
				this._isGetBestBuyInfoRunning = false;
				yield break;
			}
			PersistentCacheManager.Instance.SetValue(url, responseText);
		}
		object bestBuyInfoObj = Json.Deserialize(responseText);
		Dictionary<string, object> bestBuyInfo = bestBuyInfoObj as Dictionary<string, object>;
		if (bestBuyInfo == null || !bestBuyInfo.ContainsKey("bestBuy"))
		{
			Debug.LogWarning("Best buy response is bad");
			this._bestBuyIds.Clear();
			this._isGetBestBuyInfoRunning = false;
			yield break;
		}
		List<object> bestBuyItemObjects = bestBuyInfo["bestBuy"] as List<object>;
		if (bestBuyItemObjects != null)
		{
			this._bestBuyIds.Clear();
			for (int i = 0; i < bestBuyItemObjects.Count; i++)
			{
				string bestBuyId = (string)bestBuyItemObjects[i];
				this._bestBuyIds.Add(bestBuyId);
			}
		}
		if (PromoActionsManager.BestBuyStateUpdate != null)
		{
			PromoActionsManager.BestBuyStateUpdate();
		}
		this._isGetBestBuyInfoRunning = false;
		yield break;
	}

	// Token: 0x06002AAE RID: 10926 RVA: 0x000E2194 File Offset: 0x000E0394
	public bool IsBankItemBestBuy(PurchaseEventArgs purchaseInfo)
	{
		if (this._bestBuyIds.Count == 0 || purchaseInfo == null)
		{
			return false;
		}
		string arg = (!(purchaseInfo.Currency == "GemsCurrency")) ? "coins" : "gems";
		string item = string.Format("{0}_{1}", arg, purchaseInfo.Index + 1);
		return this._bestBuyIds.Contains(item);
	}

	// Token: 0x06002AAF RID: 10927 RVA: 0x000E2204 File Offset: 0x000E0404
	private IEnumerator GetBestBuyInfoLoop(Task futureToWait)
	{
		yield return new WaitUntil(() => futureToWait.IsCompleted);
		for (;;)
		{
			yield return base.StartCoroutine(this.DownloadBestBuyInfo());
			while (Time.realtimeSinceStartup - this._bestBuyGetInfoStartTime < 1020f)
			{
				yield return null;
			}
		}
		yield break;
	}

	// Token: 0x1700074C RID: 1868
	// (get) Token: 0x06002AB1 RID: 10929 RVA: 0x000E223C File Offset: 0x000E043C
	// (set) Token: 0x06002AB0 RID: 10928 RVA: 0x000E2230 File Offset: 0x000E0430
	public bool IsDayOfValorEventActive { get; private set; }

	// Token: 0x1700074D RID: 1869
	// (get) Token: 0x06002AB2 RID: 10930 RVA: 0x000E2244 File Offset: 0x000E0444
	public int DayOfValorMultiplyerForExp
	{
		get
		{
			if (!this.IsDayOfValorEventActive)
			{
				return 1;
			}
			return (int)this._dayOfValorMultiplyerForExp;
		}
	}

	// Token: 0x1700074E RID: 1870
	// (get) Token: 0x06002AB3 RID: 10931 RVA: 0x000E225C File Offset: 0x000E045C
	public int DayOfValorMultiplyerForMoney
	{
		get
		{
			if (!this.IsDayOfValorEventActive)
			{
				return 1;
			}
			return (int)this._dayOfValorMultiplyerForMoney;
		}
	}

	// Token: 0x06002AB4 RID: 10932 RVA: 0x000E2274 File Offset: 0x000E0474
	private void ClearDataDayOfValor()
	{
		this._dayOfValorStartTime = 0L;
		this._dayOfValorEndTime = 0L;
		this._dayOfValorMultiplyerForExp = 1L;
		this._dayOfValorMultiplyerForMoney = 1L;
	}

	// Token: 0x06002AB5 RID: 10933 RVA: 0x000E22A4 File Offset: 0x000E04A4
	private IEnumerator DownloadDayOfValorInfo()
	{
		if (this._isGetDayOfValorInfoRunning)
		{
			yield break;
		}
		this._dayOfValorGetInfoStartTime = Time.realtimeSinceStartup;
		this._isGetDayOfValorInfoRunning = true;
		if (string.IsNullOrEmpty(URLs.DayOfValor))
		{
			this._isGetDayOfValorInfoRunning = false;
			yield break;
		}
		WWW response = Tools.CreateWwwIfNotConnected(URLs.DayOfValor);
		yield return response;
		string responseText = URLs.Sanitize(response);
		if (response == null || !string.IsNullOrEmpty(response.error))
		{
			Debug.LogWarningFormat("Day of valor response error: {0}", new object[]
			{
				(response == null) ? "null" : response.error
			});
			this._isGetDayOfValorInfoRunning = false;
			this.ClearDataDayOfValor();
			yield break;
		}
		if (string.IsNullOrEmpty(responseText))
		{
			Debug.LogWarning("Best buy response is empty");
			this._isGetDayOfValorInfoRunning = false;
			this.ClearDataDayOfValor();
			yield break;
		}
		object dayOfValorInfoObj = Json.Deserialize(responseText);
		Dictionary<string, object> dayOfValorInfo = dayOfValorInfoObj as Dictionary<string, object>;
		if (dayOfValorInfo == null)
		{
			Debug.LogWarning("Day of valor response is bad");
			this._isGetDayOfValorInfoRunning = false;
			this.ClearDataDayOfValor();
			yield break;
		}
		this.ClearDataDayOfValor();
		if (dayOfValorInfo.ContainsKey("startTime"))
		{
			this._dayOfValorStartTime = (long)dayOfValorInfo["startTime"];
		}
		if (dayOfValorInfo.ContainsKey("endTime"))
		{
			this._dayOfValorEndTime = (long)dayOfValorInfo["endTime"];
		}
		if (dayOfValorInfo.ContainsKey("multiplyerForExp"))
		{
			this._dayOfValorMultiplyerForExp = (long)dayOfValorInfo["multiplyerForExp"];
		}
		if (dayOfValorInfo.ContainsKey("multiplyerForMoney"))
		{
			this._dayOfValorMultiplyerForMoney = (long)dayOfValorInfo["multiplyerForMoney"];
		}
		this._isGetDayOfValorInfoRunning = false;
		yield break;
	}

	// Token: 0x06002AB6 RID: 10934 RVA: 0x000E22C0 File Offset: 0x000E04C0
	private IEnumerator GetDayOfValorInfoLoop()
	{
		while (!TrainingController.TrainingCompleted)
		{
			yield return null;
		}
		for (;;)
		{
			yield return base.StartCoroutine(this.DownloadDayOfValorInfo());
			while (Time.realtimeSinceStartup - this._dayOfValorGetInfoStartTime < 1050f)
			{
				yield return null;
			}
		}
		yield break;
	}

	// Token: 0x06002AB7 RID: 10935 RVA: 0x000E22DC File Offset: 0x000E04DC
	private void CheckDayOfValorActive()
	{
		bool isDayOfValorEventActive = this.IsDayOfValorEventActive;
		if (this._dayOfValorStartTime != 0L && this._dayOfValorEndTime != 0L && ExpController.LobbyLevel >= 1)
		{
			long currentUnixTime = PromoActionsManager.CurrentUnixTime;
			this.IsDayOfValorEventActive = (this._dayOfValorStartTime < currentUnixTime && currentUnixTime < this._dayOfValorEndTime);
			this._timeToEndDayOfValor = TimeSpan.FromSeconds((double)(this._dayOfValorEndTime - currentUnixTime));
		}
		else
		{
			this.ClearDataDayOfValor();
			this.IsDayOfValorEventActive = false;
		}
		if (this.IsDayOfValorEventActive != isDayOfValorEventActive && PromoActionsManager.OnDayOfValorEnable != null)
		{
			PromoActionsManager.OnDayOfValorEnable(this.IsDayOfValorEventActive);
		}
	}

	// Token: 0x06002AB8 RID: 10936 RVA: 0x000E2384 File Offset: 0x000E0584
	public static void UpdateDaysOfValorShownCondition()
	{
		string @string = PlayerPrefs.GetString("LastTimeShowDaysOfValor", string.Empty);
		if (string.IsNullOrEmpty(@string))
		{
			return;
		}
		DateTime d = default(DateTime);
		if (!DateTime.TryParse(@string, out d))
		{
			return;
		}
		if (DateTime.UtcNow - d >= PromoActionsManager.TimeToShowDaysOfValor)
		{
			PlayerPrefs.SetInt("DaysOfValorShownCount", 1);
		}
	}

	// Token: 0x06002AB9 RID: 10937 RVA: 0x000E23E8 File Offset: 0x000E05E8
	public string GetTimeToEndDaysOfValor()
	{
		if (!this.IsDayOfValorEventActive)
		{
			return string.Empty;
		}
		if (this._timeToEndDayOfValor.Days > 0)
		{
			return string.Format("{0} days {1:00}:{2:00}:{3:00}", new object[]
			{
				this._timeToEndDayOfValor.Days,
				this._timeToEndDayOfValor.Hours,
				this._timeToEndDayOfValor.Minutes,
				this._timeToEndDayOfValor.Seconds
			});
		}
		return string.Format("{0:00}:{1:00}:{2:00}", this._timeToEndDayOfValor.Hours, this._timeToEndDayOfValor.Minutes, this._timeToEndDayOfValor.Seconds);
	}

	// Token: 0x06002ABA RID: 10938 RVA: 0x000E24B0 File Offset: 0x000E06B0
	internal void SaveUnlockedItems()
	{
		try
		{
			Dictionary<string, List<string>> obj = new Dictionary<string, List<string>>
			{
				{
					"UnlockedItems",
					this.UnlockedItems
				}
			};
			Storager.setString("PromoActionsManager.UNLOCKED_ITEMS_MANAGEMENT_STORAGER_KEY", Json.Serialize(obj), false);
		}
		catch (Exception ex)
		{
			Debug.LogErrorFormat("Exception in SaveUnlockedItems: {0}", new object[]
			{
				ex
			});
		}
	}

	// Token: 0x06002ABB RID: 10939 RVA: 0x000E2524 File Offset: 0x000E0724
	private void LoadUnlockedItems()
	{
		try
		{
			if (!Storager.hasKey("PromoActionsManager.UNLOCKED_ITEMS_MANAGEMENT_STORAGER_KEY"))
			{
				Dictionary<string, List<string>> obj = new Dictionary<string, List<string>>
				{
					{
						"UnlockedItems",
						new List<string>()
					}
				};
				Storager.setString("PromoActionsManager.UNLOCKED_ITEMS_MANAGEMENT_STORAGER_KEY", Json.Serialize(obj), false);
			}
			string @string = Storager.getString("PromoActionsManager.UNLOCKED_ITEMS_MANAGEMENT_STORAGER_KEY", false);
			Dictionary<string, object> dictionary = Json.Deserialize(@string) as Dictionary<string, object>;
			this.UnlockedItems = (dictionary["UnlockedItems"] as List<object>).OfType<string>().ToList<string>();
		}
		catch (Exception ex)
		{
			Debug.LogErrorFormat("Exception in LoadUnlockedItems: {0}", new object[]
			{
				ex
			});
			this.m_unlockedItems = new List<string>();
		}
	}

	// Token: 0x04001F7A RID: 8058
	private const float OffersUpdateTimeout = 900f;

	// Token: 0x04001F7B RID: 8059
	private const float EventX3UpdateTimeout = 930f;

	// Token: 0x04001F7C RID: 8060
	private const float AdvertInfoTimeout = 960f;

	// Token: 0x04001F7D RID: 8061
	public const long NewbieEventX3Duration = 259200L;

	// Token: 0x04001F7E RID: 8062
	public const long NewbieEventX3Timeout = 259200L;

	// Token: 0x04001F7F RID: 8063
	private const float BestBuyInfoTimeout = 1020f;

	// Token: 0x04001F80 RID: 8064
	public const int ShownCountDaysOfValor = 1;

	// Token: 0x04001F81 RID: 8065
	private const float DayOfValorInfoTimeout = 1050f;

	// Token: 0x04001F82 RID: 8066
	private const string UNLOCKED_ITEMS_MANAGEMENT_STORAGER_KEY = "PromoActionsManager.UNLOCKED_ITEMS_MANAGEMENT_STORAGER_KEY";

	// Token: 0x04001F83 RID: 8067
	private const string UNLOCKED_ITEMS_KEY = "UnlockedItems";

	// Token: 0x04001F84 RID: 8068
	public static PromoActionsManager sharedManager;

	// Token: 0x04001F85 RID: 8069
	public static bool ActionsAvailable = true;

	// Token: 0x04001F86 RID: 8070
	public Dictionary<string, List<SaltedInt>> discounts = new Dictionary<string, List<SaltedInt>>();

	// Token: 0x04001F87 RID: 8071
	public List<string> topSellers = new List<string>();

	// Token: 0x04001F88 RID: 8072
	private float startTime;

	// Token: 0x04001F89 RID: 8073
	private string promoActionAddress = URLs.PromoActionsTest;

	// Token: 0x04001F8A RID: 8074
	private float _eventX3GetInfoStartTime;

	// Token: 0x04001F8B RID: 8075
	private float _eventX3LastCheckTime;

	// Token: 0x04001F8C RID: 8076
	private long _newbieEventX3StartTime;

	// Token: 0x04001F8D RID: 8077
	private long _newbieEventX3StartTimeAdditional;

	// Token: 0x04001F8E RID: 8078
	private long _eventX3StartTime;

	// Token: 0x04001F8F RID: 8079
	private long _eventX3Duration;

	// Token: 0x04001F90 RID: 8080
	private bool _eventX3Active;

	// Token: 0x04001F91 RID: 8081
	private long _eventX3AmazonEventStartTime;

	// Token: 0x04001F92 RID: 8082
	private long _eventX3AmazonEventEndTime;

	// Token: 0x04001F93 RID: 8083
	private readonly List<long> _eventX3AmazonEventValidTimeZone = new List<long>();

	// Token: 0x04001F94 RID: 8084
	private bool _eventX3AmazonEventActive;

	// Token: 0x04001F95 RID: 8085
	private float _advertGetInfoStartTime;

	// Token: 0x04001F96 RID: 8086
	private static readonly PromoActionsManager.AdvertInfo _paidAdvert = new PromoActionsManager.AdvertInfo();

	// Token: 0x04001F97 RID: 8087
	private static readonly PromoActionsManager.AdvertInfo _freeAdvert = new PromoActionsManager.AdvertInfo();

	// Token: 0x04001F98 RID: 8088
	private static readonly PromoActionsManager.ReplaceAdmobPerelivInfo _replaceAdmobPereliv = new PromoActionsManager.ReplaceAdmobPerelivInfo();

	// Token: 0x04001F99 RID: 8089
	private static readonly PromoActionsManager.MobileAdvertInfo _mobileAdvert = new PromoActionsManager.MobileAdvertInfo();

	// Token: 0x04001F9A RID: 8090
	public static float startupTime = 0f;

	// Token: 0x04001F9B RID: 8091
	private bool _isGetEventX3InfoRunning;

	// Token: 0x04001F9C RID: 8092
	private PromoActionsManager.AmazonEventInfo _amazonEventInfo;

	// Token: 0x04001F9D RID: 8093
	public static bool x3InfoDownloadaedOnceDuringCurrentRun = false;

	// Token: 0x04001F9E RID: 8094
	private bool _isGetAdvertInfoRunning;

	// Token: 0x04001F9F RID: 8095
	private string _previousResponseText;

	// Token: 0x04001FA0 RID: 8096
	private List<string> _bestBuyIds = new List<string>();

	// Token: 0x04001FA1 RID: 8097
	private bool _isGetBestBuyInfoRunning;

	// Token: 0x04001FA2 RID: 8098
	private float _bestBuyGetInfoStartTime;

	// Token: 0x04001FA3 RID: 8099
	private long _dayOfValorStartTime;

	// Token: 0x04001FA4 RID: 8100
	private long _dayOfValorEndTime;

	// Token: 0x04001FA5 RID: 8101
	private long _dayOfValorMultiplyerForExp;

	// Token: 0x04001FA6 RID: 8102
	private long _dayOfValorMultiplyerForMoney;

	// Token: 0x04001FA7 RID: 8103
	private bool _isGetDayOfValorInfoRunning;

	// Token: 0x04001FA8 RID: 8104
	private float _dayOfValorGetInfoStartTime;

	// Token: 0x04001FA9 RID: 8105
	private static TimeSpan TimeToShowDaysOfValor = TimeSpan.FromHours(12.0);

	// Token: 0x04001FAA RID: 8106
	private TimeSpan _timeToEndDayOfValor;

	// Token: 0x04001FAB RID: 8107
	private List<string> m_itemsToRemoveFromUnlocked = new List<string>();

	// Token: 0x04001FAC RID: 8108
	private List<string> m_unlockedItems = new List<string>();

	// Token: 0x04001FAD RID: 8109
	private List<string> m_news = new List<string>
	{
		"mercenary",
		"mercenary_up1",
		"mercenary_up2",
		"photon_sniper_rifle",
		"photon_sniper_rifle_up1",
		"photon_sniper_rifle_up2",
		"subzero",
		"subzero_up1",
		"subzero_up2",
		"gadget_ninjashurickens",
		"gadget_ninjashurickens_up1"
	};

	// Token: 0x020004A5 RID: 1189
	public sealed class AdvertInfo
	{
		// Token: 0x04001FBB RID: 8123
		public bool enabled;

		// Token: 0x04001FBC RID: 8124
		public string imageUrl;

		// Token: 0x04001FBD RID: 8125
		public string adUrl;

		// Token: 0x04001FBE RID: 8126
		public string message;

		// Token: 0x04001FBF RID: 8127
		public bool showAlways;

		// Token: 0x04001FC0 RID: 8128
		public bool btnClose;

		// Token: 0x04001FC1 RID: 8129
		public int minLevel;

		// Token: 0x04001FC2 RID: 8130
		public int maxLevel;
	}

	// Token: 0x020004A6 RID: 1190
	public class MobileAdvertInfo
	{
		// Token: 0x1700074F RID: 1871
		// (get) Token: 0x06002AC3 RID: 10947 RVA: 0x000E26C0 File Offset: 0x000E08C0
		// (set) Token: 0x06002AC4 RID: 10948 RVA: 0x000E26C8 File Offset: 0x000E08C8
		[Obsolete]
		public bool ImageEnabled { get; set; }

		// Token: 0x17000750 RID: 1872
		// (get) Token: 0x06002AC5 RID: 10949 RVA: 0x000E26D4 File Offset: 0x000E08D4
		// (set) Token: 0x06002AC6 RID: 10950 RVA: 0x000E26DC File Offset: 0x000E08DC
		public bool VideoEnabled { get; set; }

		// Token: 0x17000751 RID: 1873
		// (get) Token: 0x06002AC7 RID: 10951 RVA: 0x000E26E8 File Offset: 0x000E08E8
		// (set) Token: 0x06002AC8 RID: 10952 RVA: 0x000E26F0 File Offset: 0x000E08F0
		public bool VideoShowPaying { get; set; }

		// Token: 0x17000752 RID: 1874
		// (get) Token: 0x06002AC9 RID: 10953 RVA: 0x000E26FC File Offset: 0x000E08FC
		// (set) Token: 0x06002ACA RID: 10954 RVA: 0x000E2704 File Offset: 0x000E0904
		public bool VideoShowNonpaying { get; set; }

		// Token: 0x17000753 RID: 1875
		// (get) Token: 0x06002ACB RID: 10955 RVA: 0x000E2710 File Offset: 0x000E0910
		// (set) Token: 0x06002ACC RID: 10956 RVA: 0x000E2718 File Offset: 0x000E0918
		public int TimeoutBetweenShowInterstitial { get; set; }

		// Token: 0x17000754 RID: 1876
		// (get) Token: 0x06002ACD RID: 10957 RVA: 0x000E2724 File Offset: 0x000E0924
		// (set) Token: 0x06002ACE RID: 10958 RVA: 0x000E272C File Offset: 0x000E092C
		public int CountSessionNewPlayer { get; set; }

		// Token: 0x17000755 RID: 1877
		// (get) Token: 0x06002ACF RID: 10959 RVA: 0x000E2738 File Offset: 0x000E0938
		// (set) Token: 0x06002AD0 RID: 10960 RVA: 0x000E2740 File Offset: 0x000E0940
		public int CountRoundReplaceProviders { get; set; }

		// Token: 0x17000756 RID: 1878
		// (get) Token: 0x06002AD1 RID: 10961 RVA: 0x000E274C File Offset: 0x000E094C
		// (set) Token: 0x06002AD2 RID: 10962 RVA: 0x000E2754 File Offset: 0x000E0954
		public int TimeoutSkipVideoPaying { get; set; }

		// Token: 0x17000757 RID: 1879
		// (get) Token: 0x06002AD3 RID: 10963 RVA: 0x000E2760 File Offset: 0x000E0960
		// (set) Token: 0x06002AD4 RID: 10964 RVA: 0x000E2768 File Offset: 0x000E0968
		public int TimeoutSkipVideoNonpaying { get; set; }

		// Token: 0x17000758 RID: 1880
		// (get) Token: 0x06002AD5 RID: 10965 RVA: 0x000E2774 File Offset: 0x000E0974
		// (set) Token: 0x06002AD6 RID: 10966 RVA: 0x000E277C File Offset: 0x000E097C
		public double ConnectSceneDelaySeconds { get; set; }

		// Token: 0x17000759 RID: 1881
		// (get) Token: 0x06002AD7 RID: 10967 RVA: 0x000E2788 File Offset: 0x000E0988
		// (set) Token: 0x06002AD8 RID: 10968 RVA: 0x000E2790 File Offset: 0x000E0990
		public double DaysOfBeingPayingUser
		{
			get
			{
				return this._daysOfBeingPayingUser;
			}
			set
			{
				this._daysOfBeingPayingUser = Math.Max(0.0, value);
			}
		}

		// Token: 0x1700075A RID: 1882
		// (get) Token: 0x06002AD9 RID: 10969 RVA: 0x000E27A8 File Offset: 0x000E09A8
		// (set) Token: 0x06002ADA RID: 10970 RVA: 0x000E27B0 File Offset: 0x000E09B0
		public string AdmobVideoAdUnitId { get; set; }

		// Token: 0x1700075B RID: 1883
		// (get) Token: 0x06002ADB RID: 10971 RVA: 0x000E27BC File Offset: 0x000E09BC
		// (set) Token: 0x06002ADC RID: 10972 RVA: 0x000E27C4 File Offset: 0x000E09C4
		public List<string> AdmobImageAdUnitIds
		{
			get
			{
				return this._admobImageAdUnitIds;
			}
			set
			{
				this._admobImageAdUnitIds = (value ?? new List<string>());
			}
		}

		// Token: 0x1700075C RID: 1884
		// (get) Token: 0x06002ADD RID: 10973 RVA: 0x000E27DC File Offset: 0x000E09DC
		// (set) Token: 0x06002ADE RID: 10974 RVA: 0x000E27E4 File Offset: 0x000E09E4
		public List<string> AdmobVideoAdUnitIds
		{
			get
			{
				return this._admobVideoAdUnitIds;
			}
			set
			{
				this._admobVideoAdUnitIds = (value ?? new List<string>());
			}
		}

		// Token: 0x1700075D RID: 1885
		// (get) Token: 0x06002ADF RID: 10975 RVA: 0x000E27FC File Offset: 0x000E09FC
		// (set) Token: 0x06002AE0 RID: 10976 RVA: 0x000E2804 File Offset: 0x000E0A04
		public List<List<string>> AdmobImageIdGroups
		{
			get
			{
				return this._admobImageIdGroups;
			}
			set
			{
				this._admobImageIdGroups = (value ?? new List<List<string>>());
			}
		}

		// Token: 0x1700075E RID: 1886
		// (get) Token: 0x06002AE1 RID: 10977 RVA: 0x000E281C File Offset: 0x000E0A1C
		// (set) Token: 0x06002AE2 RID: 10978 RVA: 0x000E2824 File Offset: 0x000E0A24
		public List<List<string>> AdmobVideoIdGroups
		{
			get
			{
				return this._admobVideoIdGroups;
			}
			set
			{
				this._admobVideoIdGroups = (value ?? new List<List<string>>());
			}
		}

		// Token: 0x1700075F RID: 1887
		// (get) Token: 0x06002AE3 RID: 10979 RVA: 0x000E283C File Offset: 0x000E0A3C
		// (set) Token: 0x06002AE4 RID: 10980 RVA: 0x000E2844 File Offset: 0x000E0A44
		public int AdProvider { get; set; }

		// Token: 0x17000760 RID: 1888
		// (get) Token: 0x06002AE5 RID: 10981 RVA: 0x000E2850 File Offset: 0x000E0A50
		// (set) Token: 0x06002AE6 RID: 10982 RVA: 0x000E2858 File Offset: 0x000E0A58
		public List<int> AdProviders
		{
			get
			{
				return this._adProviders;
			}
			set
			{
				this._adProviders = (value ?? new List<int>());
			}
		}

		// Token: 0x17000761 RID: 1889
		// (get) Token: 0x06002AE7 RID: 10983 RVA: 0x000E2870 File Offset: 0x000E0A70
		// (set) Token: 0x06002AE8 RID: 10984 RVA: 0x000E2878 File Offset: 0x000E0A78
		public List<int> InterstitialProviders
		{
			get
			{
				return this._interstitialProviders;
			}
			set
			{
				this._interstitialProviders = (value ?? new List<int>());
			}
		}

		// Token: 0x17000762 RID: 1890
		// (get) Token: 0x06002AE9 RID: 10985 RVA: 0x000E2890 File Offset: 0x000E0A90
		// (set) Token: 0x06002AEA RID: 10986 RVA: 0x000E2898 File Offset: 0x000E0A98
		public float TimeoutWaitingInterstitialAfterMatchSeconds { get; set; }

		// Token: 0x17000763 RID: 1891
		// (get) Token: 0x06002AEB RID: 10987 RVA: 0x000E28A4 File Offset: 0x000E0AA4
		// (set) Token: 0x06002AEC RID: 10988 RVA: 0x000E28AC File Offset: 0x000E0AAC
		public double MinMatchDurationMinutes { get; set; }

		// Token: 0x04001FC3 RID: 8131
		private List<string> _admobImageAdUnitIds = new List<string>();

		// Token: 0x04001FC4 RID: 8132
		private List<string> _admobVideoAdUnitIds = new List<string>();

		// Token: 0x04001FC5 RID: 8133
		private List<int> _adProviders = new List<int>();

		// Token: 0x04001FC6 RID: 8134
		private double _daysOfBeingPayingUser = double.MaxValue;

		// Token: 0x04001FC7 RID: 8135
		private List<int> _interstitialProviders = new List<int>();

		// Token: 0x04001FC8 RID: 8136
		private List<List<string>> _admobImageIdGroups = new List<List<string>>();

		// Token: 0x04001FC9 RID: 8137
		private List<List<string>> _admobVideoIdGroups = new List<List<string>>();
	}

	// Token: 0x020004A7 RID: 1191
	internal sealed class ReplaceAdmobPerelivInfo
	{
		// Token: 0x17000764 RID: 1892
		// (get) Token: 0x06002AEE RID: 10990 RVA: 0x000E28D8 File Offset: 0x000E0AD8
		// (set) Token: 0x06002AEF RID: 10991 RVA: 0x000E28E0 File Offset: 0x000E0AE0
		public int ShowEveryTimes { get; set; }

		// Token: 0x17000765 RID: 1893
		// (get) Token: 0x06002AF0 RID: 10992 RVA: 0x000E28EC File Offset: 0x000E0AEC
		// (set) Token: 0x06002AF1 RID: 10993 RVA: 0x000E28F4 File Offset: 0x000E0AF4
		public int ShowTimesTotal { get; set; }

		// Token: 0x17000766 RID: 1894
		// (get) Token: 0x06002AF2 RID: 10994 RVA: 0x000E2900 File Offset: 0x000E0B00
		// (set) Token: 0x06002AF3 RID: 10995 RVA: 0x000E2908 File Offset: 0x000E0B08
		public bool ShowToPaying { get; set; }

		// Token: 0x17000767 RID: 1895
		// (get) Token: 0x06002AF4 RID: 10996 RVA: 0x000E2914 File Offset: 0x000E0B14
		// (set) Token: 0x06002AF5 RID: 10997 RVA: 0x000E291C File Offset: 0x000E0B1C
		public bool ShowToNew { get; set; }

		// Token: 0x17000768 RID: 1896
		// (get) Token: 0x06002AF6 RID: 10998 RVA: 0x000E2928 File Offset: 0x000E0B28
		// (set) Token: 0x06002AF7 RID: 10999 RVA: 0x000E2930 File Offset: 0x000E0B30
		public int MinLevel { get; set; }

		// Token: 0x17000769 RID: 1897
		// (get) Token: 0x06002AF8 RID: 11000 RVA: 0x000E293C File Offset: 0x000E0B3C
		// (set) Token: 0x06002AF9 RID: 11001 RVA: 0x000E2944 File Offset: 0x000E0B44
		public int MaxLevel { get; set; }

		// Token: 0x04001FD8 RID: 8152
		public bool enabled;

		// Token: 0x04001FD9 RID: 8153
		public readonly List<string> imageUrls = new List<string>();

		// Token: 0x04001FDA RID: 8154
		public readonly List<string> adUrls = new List<string>();
	}

	// Token: 0x020004A8 RID: 1192
	internal sealed class AmazonEventInfo
	{
		// Token: 0x06002AFA RID: 11002 RVA: 0x000E2950 File Offset: 0x000E0B50
		public AmazonEventInfo()
		{
			this.StartTime = DateTime.MaxValue;
			this.Timezones = new List<int>();
			this.Caption = string.Empty;
		}

		// Token: 0x1700076A RID: 1898
		// (get) Token: 0x06002AFB RID: 11003 RVA: 0x000E2984 File Offset: 0x000E0B84
		// (set) Token: 0x06002AFC RID: 11004 RVA: 0x000E298C File Offset: 0x000E0B8C
		public DateTime StartTime { get; set; }

		// Token: 0x1700076B RID: 1899
		// (get) Token: 0x06002AFD RID: 11005 RVA: 0x000E2998 File Offset: 0x000E0B98
		public DateTime EndTime
		{
			get
			{
				return this.StartTime + TimeSpan.FromSeconds((double)this.DurationSeconds);
			}
		}

		// Token: 0x1700076C RID: 1900
		// (get) Token: 0x06002AFE RID: 11006 RVA: 0x000E29B4 File Offset: 0x000E0BB4
		// (set) Token: 0x06002AFF RID: 11007 RVA: 0x000E29BC File Offset: 0x000E0BBC
		public float DurationSeconds { get; set; }

		// Token: 0x1700076D RID: 1901
		// (get) Token: 0x06002B00 RID: 11008 RVA: 0x000E29C8 File Offset: 0x000E0BC8
		// (set) Token: 0x06002B01 RID: 11009 RVA: 0x000E29D0 File Offset: 0x000E0BD0
		public List<int> Timezones { get; set; }

		// Token: 0x1700076E RID: 1902
		// (get) Token: 0x06002B02 RID: 11010 RVA: 0x000E29DC File Offset: 0x000E0BDC
		// (set) Token: 0x06002B03 RID: 11011 RVA: 0x000E29E4 File Offset: 0x000E0BE4
		public float Percentage { get; set; }

		// Token: 0x1700076F RID: 1903
		// (get) Token: 0x06002B04 RID: 11012 RVA: 0x000E29F0 File Offset: 0x000E0BF0
		// (set) Token: 0x06002B05 RID: 11013 RVA: 0x000E29F8 File Offset: 0x000E0BF8
		public string Caption { get; set; }
	}

	// Token: 0x02000918 RID: 2328
	// (Invoke) Token: 0x06005100 RID: 20736
	public delegate void OnDayOfValorEnableDelegate(bool enable);
}
