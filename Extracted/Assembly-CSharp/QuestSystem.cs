using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Rilisoft;
using Rilisoft.DictionaryExtensions;
using Rilisoft.MiniJson;
using Rilisoft.NullExtensions;
using UnityEngine;

// Token: 0x02000736 RID: 1846
internal sealed class QuestSystem : MonoBehaviour
{
	// Token: 0x14000091 RID: 145
	// (add) Token: 0x060040D8 RID: 16600 RVA: 0x0015A540 File Offset: 0x00158740
	// (remove) Token: 0x060040D9 RID: 16601 RVA: 0x0015A55C File Offset: 0x0015875C
	public event EventHandler Updated;

	// Token: 0x14000092 RID: 146
	// (add) Token: 0x060040DA RID: 16602 RVA: 0x0015A578 File Offset: 0x00158778
	// (remove) Token: 0x060040DB RID: 16603 RVA: 0x0015A594 File Offset: 0x00158794
	public event EventHandler<QuestCompletedEventArgs> QuestCompleted;

	// Token: 0x17000AC1 RID: 2753
	// (get) Token: 0x060040DC RID: 16604 RVA: 0x0015A5B0 File Offset: 0x001587B0
	public static QuestSystem Instance
	{
		get
		{
			return QuestSystem._instance.Value;
		}
	}

	// Token: 0x060040DD RID: 16605 RVA: 0x0015A5BC File Offset: 0x001587BC
	public void Initialize()
	{
	}

	// Token: 0x17000AC2 RID: 2754
	// (get) Token: 0x060040DE RID: 16606 RVA: 0x0015A5C0 File Offset: 0x001587C0
	public QuestProgress QuestProgress
	{
		get
		{
			return this._questProgress;
		}
	}

	// Token: 0x17000AC3 RID: 2755
	// (get) Token: 0x060040DF RID: 16607 RVA: 0x0015A5C8 File Offset: 0x001587C8
	// (set) Token: 0x060040E0 RID: 16608 RVA: 0x0015A5D0 File Offset: 0x001587D0
	internal bool Enabled
	{
		get
		{
			return this._enabled;
		}
		set
		{
			PlayerPrefs.SetInt("QuestSystem.DefaultAvailability", Convert.ToInt32(value));
			if (this._enabled == value)
			{
				return;
			}
			this._enabled = value;
			if (value)
			{
				this.InitializeQuestProgress();
			}
			else if (this._questProgress != null)
			{
				this._questProgress.Dispose();
				this._questProgress = null;
			}
			EventHandler updated = this.Updated;
			if (updated != null)
			{
				updated(this, EventArgs.Empty);
			}
		}
	}

	// Token: 0x17000AC4 RID: 2756
	// (get) Token: 0x060040E1 RID: 16609 RVA: 0x0015A648 File Offset: 0x00158848
	public bool AnyActiveQuest
	{
		get
		{
			return this.Enabled && this.QuestProgress != null && this.QuestProgress.AnyActiveQuest;
		}
	}

	// Token: 0x060040E2 RID: 16610 RVA: 0x0015A67C File Offset: 0x0015887C
	private void Start()
	{
		if (!this.Enabled)
		{
			Debug.Log("QuestSystem.Start(): disabled");
			return;
		}
		this.InitializeQuestProgress();
	}

	// Token: 0x060040E3 RID: 16611 RVA: 0x0015A69C File Offset: 0x0015889C
	private void InitializeQuestProgress()
	{
		this._questProgress = this.LoadQuestProgress();
		if (this._questProgress != null)
		{
			this._questProgress.QuestCompleted += this.HandleQuestCompleted;
			if (!TutorialQuestManager.Instance.Received)
			{
				this._getTutorialQuestsConfigLoopCoroutine.Do(new Action<Coroutine>(base.StopCoroutine));
				this._getTutorialQuestsConfigLoopCoroutine = base.StartCoroutine(this.GetTutorialQuestConfigLoopCoroutine());
			}
		}
		this.Updated.Do(delegate(EventHandler handler)
		{
			handler(this, EventArgs.Empty);
		});
		this._getConfigLoopCoroutine = base.StartCoroutine(this.GetConfigLoopCoroutine(false));
	}

	// Token: 0x060040E4 RID: 16612 RVA: 0x0015A73C File Offset: 0x0015893C
	private void OnApplicationPause(bool pauseStatus)
	{
		if (!this.Enabled)
		{
			Debug.LogFormat("QuestSystem.OnApplicationPause({0}): disabled", new object[]
			{
				pauseStatus
			});
			return;
		}
		if (pauseStatus)
		{
			QuestSystem.SaveQuestProgress(this._questProgress);
			TutorialQuestManager.Instance.SaveIfDirty();
		}
		else
		{
			this._getConfigLoopCoroutine.Do(new Action<Coroutine>(base.StopCoroutine));
			this._getConfigLoopCoroutine = base.StartCoroutine(this.GetConfigLoopCoroutine(true));
		}
	}

	// Token: 0x17000AC5 RID: 2757
	// (get) Token: 0x060040E5 RID: 16613 RVA: 0x0015A7BC File Offset: 0x001589BC
	internal int QuestConfigClientVersion
	{
		get
		{
			return 28;
		}
	}

	// Token: 0x060040E6 RID: 16614 RVA: 0x0015A7C0 File Offset: 0x001589C0
	internal void DebugDecrementDay()
	{
		if (!this.Enabled)
		{
			return;
		}
		if (this._questProgress != null)
		{
			this._questProgress.DebugDecrementDay();
		}
		this.Updated.Do(delegate(EventHandler handler)
		{
			handler(this, EventArgs.Empty);
		});
	}

	// Token: 0x060040E7 RID: 16615 RVA: 0x0015A808 File Offset: 0x00158A08
	internal void ForceGetConfig()
	{
		this._getConfigLoopCoroutine.Do(new Action<Coroutine>(base.StopCoroutine));
		this._getConfigLoopCoroutine = base.StartCoroutine(this.GetConfigLoopCoroutine(false));
	}

	// Token: 0x060040E8 RID: 16616 RVA: 0x0015A838 File Offset: 0x00158A38
	private void HandleQuestCompleted(object sender, QuestCompletedEventArgs e)
	{
		if (!this.Enabled)
		{
			Debug.LogFormat("QuestSystem.HandleQuestCompleted('{0}'): disabled", new object[]
			{
				e.Quest.Id
			});
			return;
		}
		this.SaveQuestProgressIfDirty();
		TutorialQuestManager.Instance.SaveIfDirty();
		this.QuestCompleted.Do(delegate(EventHandler<QuestCompletedEventArgs> handler)
		{
			handler(sender, e);
		});
	}

	// Token: 0x060040E9 RID: 16617 RVA: 0x0015A8B0 File Offset: 0x00158AB0
	private Task<string> GetQuestConfig()
	{
		TaskCompletionSource<string> taskCompletionSource = new TaskCompletionSource<string>();
		base.StartCoroutine(this.GetQuestConfigCoroutine(taskCompletionSource));
		return taskCompletionSource.Task;
	}

	// Token: 0x060040EA RID: 16618 RVA: 0x0015A8D8 File Offset: 0x00158AD8
	private IEnumerator GetQuestConfigCoroutine(TaskCompletionSource<string> tcs)
	{
		WWW response = Tools.CreateWwwIfNotConnected(URLs.QuestConfig);
		if (response == null)
		{
			tcs.TrySetException(new InvalidOperationException("Skipped quest config request because the player is connected."));
			yield break;
		}
		yield return response;
		try
		{
			if (string.IsNullOrEmpty(response.error))
			{
				string responseText = (response.text == null) ? string.Empty : URLs.Sanitize(response);
				tcs.TrySetResult(responseText);
			}
			else
			{
				tcs.TrySetException(new InvalidOperationException(response.error));
			}
		}
		finally
		{
			if (Application.isEditor)
			{
				Debug.Log("<color=teal>QuestSystem.GetQuestConfigCoroutine(): response.Dispose()</color>");
			}
			response.Dispose();
		}
		yield break;
	}

	// Token: 0x060040EB RID: 16619 RVA: 0x0015A904 File Offset: 0x00158B04
	private Task<string> GetConfigUpdate()
	{
		TaskCompletionSource<string> taskCompletionSource = new TaskCompletionSource<string>();
		base.StartCoroutine(this.GetConfigUpdateCoroutine(taskCompletionSource));
		return taskCompletionSource.Task;
	}

	// Token: 0x060040EC RID: 16620 RVA: 0x0015A92C File Offset: 0x00158B2C
	private IEnumerator GetConfigUpdateCoroutine(TaskCompletionSource<string> tcs)
	{
		for (;;)
		{
			if (!string.IsNullOrEmpty(FriendsController.sharedController.Map((FriendsController fc) => fc.id)))
			{
				break;
			}
			yield return null;
		}
		WWWForm form = new WWWForm();
		form.AddField("action", "get_quest_version_info");
		form.AddField("app_version", ProtocolListGetter.CurrentPlatform + ":" + GlobalGameController.AppVersion);
		form.AddField("uniq_id", FriendsController.sharedController.id);
		form.AddField("auth", FriendsController.Hash("get_quest_version_info", null));
		WWW response = Tools.CreateWwwIfNotConnected(FriendsController.actionAddress, form, string.Empty, null);
		if (response == null)
		{
			tcs.TrySetException(new InvalidOperationException("Cannot send request while connected."));
			yield break;
		}
		yield return response;
		try
		{
			if (string.IsNullOrEmpty(response.error))
			{
				string responseText = (response.text == null) ? string.Empty : URLs.Sanitize(response);
				tcs.TrySetResult(responseText);
			}
			else
			{
				tcs.TrySetException(new InvalidOperationException(response.error));
			}
		}
		finally
		{
			if (Application.isEditor)
			{
				Debug.Log("<color=teal>QuestSystem.GetConfigUpdateCoroutine(): response.Dispose()</color>");
			}
			response.Dispose();
		}
		yield break;
	}

	// Token: 0x060040ED RID: 16621 RVA: 0x0015A958 File Offset: 0x00158B58
	private IEnumerator GetTutorialQuestsConfigOnceCoroutine()
	{
		WWW response = Tools.CreateWwwIfNotConnected(URLs.TutorialQuestConfig);
		if (response == null)
		{
			yield break;
		}
		yield return response;
		try
		{
			if (!string.IsNullOrEmpty(response.error))
			{
				Debug.LogWarningFormat("Failed to load tutorial quests: {0}", new object[]
				{
					response.error
				});
				yield break;
			}
			string responseText = (response.text == null) ? string.Empty : URLs.Sanitize(response);
			Dictionary<string, object> config = Json.Deserialize(responseText) as Dictionary<string, object>;
			if (config == null)
			{
				Debug.LogWarningFormat("Failed to parse config: '{0}'", new object[]
				{
					responseText
				});
				yield break;
			}
			List<object> tutorialQuestJsons = config.TryGet("quests") as List<object>;
			if (this._questProgress != null && !TutorialQuestManager.Instance.Received)
			{
				if (tutorialQuestJsons != null)
				{
					TutorialQuestManager.Instance.SetReceived();
				}
				this._questProgress.FillTutorialQuests(tutorialQuestJsons);
				this.Updated.Do(delegate(EventHandler handler)
				{
					handler(this, EventArgs.Empty);
				});
				this.SaveQuestProgressIfDirty();
				TutorialQuestManager.Instance.SaveIfDirty();
			}
		}
		finally
		{
			if (Application.isEditor)
			{
				Debug.Log("<color=teal>QuestSystem.GetTutorialQuestsConfigOnceCoroutine(): response.Dispose()</color>");
			}
			response.Dispose();
		}
		yield break;
	}

	// Token: 0x060040EE RID: 16622 RVA: 0x0015A974 File Offset: 0x00158B74
	private IEnumerator GetConfigOnceCoroutine(bool resumed)
	{
		if (!this.Enabled)
		{
			Debug.LogFormat("QuestSystem.GetConfigOnceCoroutine({0}): disabled", new object[]
			{
				resumed
			});
			yield break;
		}
		Task<string> configUpdateRequest = this.GetConfigUpdate();
		while (!configUpdateRequest.IsCompleted)
		{
			yield return null;
		}
		float responceReceivedTime = Time.realtimeSinceStartup;
		if (configUpdateRequest.IsFaulted)
		{
			Debug.LogWarning(configUpdateRequest.Exception.InnerException ?? configUpdateRequest.Exception);
			yield break;
		}
		Dictionary<string, object> response = Json.Deserialize(configUpdateRequest.Result) as Dictionary<string, object>;
		if (response == null)
		{
			Debug.LogWarning("GetConfigOnceCoroutine(): Bad update response: " + configUpdateRequest.Result);
			yield break;
		}
		string version = string.Empty;
		long day = 0L;
		float timeLeftSeconds = 0f;
		DateTime timestamp = default(DateTime);
		try
		{
			int serverVersion = Convert.ToInt32(response["version"]);
			int clientVersion = this.QuestConfigClientVersion;
			version = string.Format("{0}.{1}", serverVersion, clientVersion);
			day = Convert.ToInt64(response["day"]);
			timeLeftSeconds = (float)Convert.ToDouble(response["timeLeftSeconds"], CultureInfo.InvariantCulture);
			long timestampUnix = Convert.ToInt64(response["timestamp"], CultureInfo.InvariantCulture);
			timestamp = Tools.GetCurrentTimeByUnixTime(timestampUnix);
			this._startupTimeAccordingToServer = new DateTime?(timestamp - TimeSpan.FromSeconds((double)responceReceivedTime));
		}
		catch (Exception ex2)
		{
			Exception ex = ex2;
			Debug.LogException(ex);
			yield break;
		}
		if (this._questProgress != null && this._questProgress.ConfigVersion == version && this._questProgress.Day == day)
		{
			yield break;
		}
		if (!this.Enabled)
		{
			Debug.LogFormat("QuestSystem.GetConfigOnceCoroutine({0}): disabled", new object[]
			{
				resumed
			});
			yield break;
		}
		Task<string> questConfigRequest = this.GetQuestConfig();
		while (!questConfigRequest.IsCompleted)
		{
			yield return null;
		}
		if (questConfigRequest.IsFaulted)
		{
			Debug.LogWarning(questConfigRequest.Exception);
			yield break;
		}
		Dictionary<string, object> rawQuests = Json.Deserialize(questConfigRequest.Result) as Dictionary<string, object>;
		if (rawQuests == null)
		{
			Debug.LogWarning("GetConfigOnceCoroutine(): Bad config response: " + questConfigRequest.Result);
			yield break;
		}
		List<Difficulty> allowedDifficulties = new List<Difficulty>
		{
			Difficulty.Easy,
			Difficulty.Normal,
			Difficulty.Hard
		};
		if (ExperienceController.sharedController != null && ExperienceController.sharedController.currentLevel < 3 && allowedDifficulties.Remove(Difficulty.Hard))
		{
			allowedDifficulties.Add(Difficulty.Normal);
		}
		Lazy<IDictionary<int, List<QuestBase>>> newQuests = new Lazy<IDictionary<int, List<QuestBase>>>(() => QuestProgress.CreateQuests(rawQuests, day, allowedDifficulties.ToArray()));
		if (this._questProgress == null)
		{
			this._questProgress = new QuestProgress(version, day, timestamp, timeLeftSeconds, null);
			this._getTutorialQuestsConfigLoopCoroutine.Do(new Action<Coroutine>(base.StopCoroutine));
			this._getTutorialQuestsConfigLoopCoroutine = base.StartCoroutine(this.GetTutorialQuestConfigLoopCoroutine());
			this._questProgress.QuestCompleted += this.HandleQuestCompleted;
			this._questProgress.PopulateQuests(newQuests.Value, null);
			this.Updated.Do(delegate(EventHandler handler)
			{
				handler(this, EventArgs.Empty);
			});
		}
		else if (!this._questProgress.ConfigVersion.Equals(version, StringComparison.Ordinal))
		{
			this._questProgress.Dispose();
			this._questProgress.QuestCompleted -= this.HandleQuestCompleted;
			this._questProgress = new QuestProgress(version, day, timestamp, timeLeftSeconds, this._questProgress);
			this._getTutorialQuestsConfigLoopCoroutine.Do(new Action<Coroutine>(base.StopCoroutine));
			this._getTutorialQuestsConfigLoopCoroutine = base.StartCoroutine(this.GetTutorialQuestConfigLoopCoroutine());
			this._questProgress.QuestCompleted += this.HandleQuestCompleted;
			this._questProgress.PopulateQuests(newQuests.Value, null);
			this.Updated.Do(delegate(EventHandler handler)
			{
				handler(this, EventArgs.Empty);
			});
		}
		else if (this._questProgress.Day < day)
		{
			this._questProgress.UpdateQuests(day, rawQuests, newQuests.Value);
			this.Updated.Do(delegate(EventHandler handler)
			{
				handler(this, EventArgs.Empty);
			});
		}
		this.SaveQuestProgressIfDirty();
		TutorialQuestManager.Instance.SaveIfDirty();
		yield break;
	}

	// Token: 0x060040EF RID: 16623 RVA: 0x0015A9A0 File Offset: 0x00158BA0
	public void SaveQuestProgressIfDirty()
	{
		if (this._questProgress == null)
		{
			return;
		}
		if (!this._questProgress.IsDirty())
		{
			return;
		}
		try
		{
			QuestSystem.SaveQuestProgress(this._questProgress);
		}
		catch (Exception exception)
		{
			Debug.LogException(exception);
		}
	}

	// Token: 0x060040F0 RID: 16624 RVA: 0x0015AA04 File Offset: 0x00158C04
	private IEnumerator GetConfigLoopCoroutine(bool resumed)
	{
		while (!TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage < TrainingController.NewTrainingCompletedStage.ShootingRangeCompleted)
		{
			yield return null;
		}
		float delaySeconds = (!Application.isEditor) ? 600f : 30f;
		Coroutine configCoroutine = null;
		while (this.Enabled)
		{
			if (configCoroutine != null)
			{
				base.StopCoroutine(configCoroutine);
			}
			configCoroutine = base.StartCoroutine(this.GetConfigOnceCoroutine(resumed));
			yield return new WaitForRealSeconds(delaySeconds);
		}
		Debug.LogFormat("QuestSystem.GetConfigLoopCoroutine({0}): disabled", new object[]
		{
			resumed
		});
		yield break;
		yield break;
	}

	// Token: 0x060040F1 RID: 16625 RVA: 0x0015AA30 File Offset: 0x00158C30
	private IEnumerator GetTutorialQuestConfigLoopCoroutine()
	{
		float delaySeconds = (!Application.isEditor) ? 600f : 30f;
		Coroutine configCoroutine = null;
		while (this._questProgress == null || !TutorialQuestManager.Instance.Received)
		{
			if (!this.Enabled)
			{
				Debug.Log("QuestSystem.GetTutorialQuestConfigLoopCoroutine({0}): disabled");
				yield break;
			}
			if (configCoroutine != null)
			{
				base.StopCoroutine(configCoroutine);
			}
			configCoroutine = base.StartCoroutine(this.GetTutorialQuestsConfigOnceCoroutine());
			yield return new WaitForRealSeconds(delaySeconds);
		}
		yield break;
	}

	// Token: 0x060040F2 RID: 16626 RVA: 0x0015AA4C File Offset: 0x00158C4C
	private QuestProgress LoadQuestProgress()
	{
		if (!Storager.hasKey("QuestProgress"))
		{
			return null;
		}
		string @string = Storager.getString("QuestProgress", false);
		if (string.IsNullOrEmpty(@string))
		{
			return null;
		}
		Dictionary<string, object> dictionary = Json.Deserialize(@string) as Dictionary<string, object>;
		if (dictionary == null)
		{
			return null;
		}
		if (dictionary.Count == 0)
		{
			return null;
		}
		Func<string, Version> createVersion = delegate(string v)
		{
			Version result;
			try
			{
				result = new Version(v);
			}
			catch
			{
				result = new Version(0, 0, 0, 0);
			}
			return result;
		};
		string text;
		if (dictionary.Count == 1)
		{
			text = dictionary.Keys.First<string>();
		}
		else
		{
			text = (from k in dictionary.Keys
			select new KeyValuePair<string, Version>(k, createVersion(k))).Aggregate((KeyValuePair<string, Version> l, KeyValuePair<string, Version> r) => (!(l.Value > r.Value)) ? r : l).Key;
		}
		string text2 = text;
		Dictionary<string, object> dictionary2 = dictionary[text2] as Dictionary<string, object>;
		if (dictionary2 == null)
		{
			return null;
		}
		object value;
		if (!dictionary2.TryGetValue("day", out value))
		{
			return null;
		}
		object value2;
		if (!dictionary2.TryGetValue("timeLeftSeconds", out value2))
		{
			return null;
		}
		object value3;
		if (!dictionary2.TryGetValue("timestamp", out value3))
		{
			return null;
		}
		QuestProgress questProgress = null;
		try
		{
			long day = Convert.ToInt64(value, CultureInfo.InvariantCulture);
			DateTime timestamp = Convert.ToDateTime(value3, CultureInfo.InvariantCulture);
			float timeLeftSeconds = (float)Convert.ToDouble(value2, CultureInfo.InvariantCulture);
			questProgress = new QuestProgress(text2, day, timestamp, timeLeftSeconds, null);
			Dictionary<string, object> dictionary3 = dictionary2["currentQuests"] as Dictionary<string, object>;
			if (dictionary3 == null)
			{
				return questProgress;
			}
			Dictionary<string, object> dictionary4 = dictionary2["previousQuests"] as Dictionary<string, object>;
			if (dictionary4 == null)
			{
				return questProgress;
			}
			IDictionary<int, List<QuestBase>> currentQuests = QuestProgress.RestoreQuests(dictionary3);
			IDictionary<int, List<QuestBase>> previousQuests = QuestProgress.RestoreQuests(dictionary4);
			questProgress.PopulateQuests(currentQuests, previousQuests);
			List<object> questJsons = dictionary2.TryGet("tutorialQuests") as List<object>;
			questProgress.FillTutorialQuests(questJsons);
		}
		catch (Exception exception)
		{
			Debug.LogException(exception);
		}
		return questProgress;
	}

	// Token: 0x060040F3 RID: 16627 RVA: 0x0015AC70 File Offset: 0x00158E70
	private static void SaveQuestProgress(QuestProgress questProgress)
	{
		if (questProgress == null)
		{
			return;
		}
		Dictionary<string, object> value = questProgress.ToJson();
		Dictionary<string, object> obj = new Dictionary<string, object>
		{
			{
				questProgress.ConfigVersion,
				value
			}
		};
		string text = Json.Serialize(obj);
		if (questProgress.Count == 0)
		{
			Debug.LogWarning("SaveQuestProgress(): Bad progress: " + text);
			Storager.setString("QuestProgress", "{}", false);
			return;
		}
		Storager.setString("QuestProgress", text, false);
		questProgress.SetClean();
	}

	// Token: 0x060040F4 RID: 16628 RVA: 0x0015ACE8 File Offset: 0x00158EE8
	private static QuestSystem InitializeInstance()
	{
		QuestSystem questSystem = UnityEngine.Object.FindObjectOfType<QuestSystem>();
		if (questSystem != null)
		{
			UnityEngine.Object.DontDestroyOnLoad(questSystem.gameObject);
			return questSystem;
		}
		GameObject gameObject = new GameObject("Rilisoft.QuestSystem");
		UnityEngine.Object.DontDestroyOnLoad(gameObject);
		QuestSystem questSystem2 = gameObject.AddComponent<QuestSystem>();
		int @int = PlayerPrefs.GetInt("QuestSystem.DefaultAvailability", 1);
		questSystem2._enabled = Convert.ToBoolean(@int);
		return questSystem2;
	}

	// Token: 0x04002F75 RID: 12149
	internal const string QuestProgressKey = "QuestProgress";

	// Token: 0x04002F76 RID: 12150
	internal const string DefaultAvailabilityKey = "QuestSystem.DefaultAvailability";

	// Token: 0x04002F77 RID: 12151
	private const int _questConfigClientVersion = 28;

	// Token: 0x04002F78 RID: 12152
	private bool _enabled;

	// Token: 0x04002F79 RID: 12153
	private static readonly Lazy<QuestSystem> _instance = new Lazy<QuestSystem>(new Func<QuestSystem>(QuestSystem.InitializeInstance));

	// Token: 0x04002F7A RID: 12154
	private Coroutine _getConfigLoopCoroutine;

	// Token: 0x04002F7B RID: 12155
	private Coroutine _getTutorialQuestsConfigLoopCoroutine;

	// Token: 0x04002F7C RID: 12156
	private QuestProgress _questProgress;

	// Token: 0x04002F7D RID: 12157
	private DateTime? _startupTimeAccordingToServer;
}
