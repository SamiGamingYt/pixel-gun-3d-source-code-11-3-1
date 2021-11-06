using System;
using System.Collections.Generic;
using System.Linq;
using Rilisoft.DictionaryExtensions;
using Rilisoft.MiniJson;
using Rilisoft.NullExtensions;
using UnityEngine;

namespace Rilisoft
{
	// Token: 0x02000738 RID: 1848
	internal sealed class TutorialQuestManager
	{
		// Token: 0x06004102 RID: 16642 RVA: 0x0015AFB8 File Offset: 0x001591B8
		private TutorialQuestManager()
		{
			this._fulfilledQuests = new HashSet<string>();
		}

		// Token: 0x06004103 RID: 16643 RVA: 0x0015AFCC File Offset: 0x001591CC
		private TutorialQuestManager(TutorialQuestManager.Memento dto)
		{
			Debug.Log("> TutorialQuestManager.TutorialQuestManager()");
			try
			{
				this._fulfilledQuests = ((dto.fulfilledQuests == null) ? new HashSet<string>() : new HashSet<string>(dto.fulfilledQuests));
				this._received = dto.received;
			}
			catch (Exception ex)
			{
				Debug.LogWarningFormat("TutorialQuestManager.TutorialQuestManager(): Exception caught: {0}", new object[]
				{
					ex.GetType().Name
				});
				Debug.LogException(ex);
			}
			finally
			{
				Debug.Log("< TutorialQuestManager.TutorialQuestManager()");
			}
		}

		// Token: 0x17000AC9 RID: 2761
		// (get) Token: 0x06004105 RID: 16645 RVA: 0x0015B0A4 File Offset: 0x001592A4
		public static TutorialQuestManager Instance
		{
			get
			{
				return TutorialQuestManager._instance.Value;
			}
		}

		// Token: 0x06004106 RID: 16646 RVA: 0x0015B0B0 File Offset: 0x001592B0
		public override string ToString()
		{
			TutorialQuestManager.Memento memento = default(TutorialQuestManager.Memento);
			memento.fulfilledQuests = this._fulfilledQuests.ToList<string>();
			memento.received = this._received;
			Dictionary<string, object> obj = new Dictionary<string, object>
			{
				{
					"fulfilledQuests",
					this._fulfilledQuests.ToList<string>()
				},
				{
					"received",
					Convert.ToBoolean(this._received)
				}
			};
			return Json.Serialize(obj);
		}

		// Token: 0x06004107 RID: 16647 RVA: 0x0015B128 File Offset: 0x00159328
		public void AddFulfilledQuest(string questId)
		{
			if (questId == null)
			{
				return;
			}
			this._dirty = this._fulfilledQuests.Add(questId);
		}

		// Token: 0x17000ACA RID: 2762
		// (get) Token: 0x06004108 RID: 16648 RVA: 0x0015B144 File Offset: 0x00159344
		public bool Received
		{
			get
			{
				return this._received;
			}
		}

		// Token: 0x06004109 RID: 16649 RVA: 0x0015B14C File Offset: 0x0015934C
		public void SetReceived()
		{
			this._received = true;
			this._dirty = true;
		}

		// Token: 0x0600410A RID: 16650 RVA: 0x0015B15C File Offset: 0x0015935C
		public bool CheckQuestIfFulfilled(string questId)
		{
			if (questId == null)
			{
				return false;
			}
			if (this._fulfilledQuests.Contains(questId))
			{
				return true;
			}
			if (Storager.getInt(Defs.TrainingCompleted_4_4_Sett, false) == 1)
			{
				return true;
			}
			switch (questId)
			{
			case "loginFacebook":
				return BuildSettings.BuildTargetPlatform == RuntimePlatform.MetroPlayerX64 || (Storager.hasKey(Defs.IsFacebookLoginRewardaGained) && Storager.getInt(Defs.IsFacebookLoginRewardaGained, true) == 1);
			case "loginTwitter":
				return BuildSettings.BuildTargetPlatform == RuntimePlatform.MetroPlayerX64 || Application.isEditor;
			case "likeFacebook":
				return BuildSettings.BuildTargetPlatform == RuntimePlatform.MetroPlayerX64 || Application.isEditor;
			}
			return false;
		}

		// Token: 0x0600410B RID: 16651 RVA: 0x0015B25C File Offset: 0x0015945C
		public void SaveIfDirty()
		{
			if (!this._dirty)
			{
				return;
			}
			string val = this.ToString();
			Storager.setString("TutorialQuestManager", val, false);
			this._dirty = false;
		}

		// Token: 0x0600410C RID: 16652 RVA: 0x0015B290 File Offset: 0x00159490
		public void FillTutorialQuests(IList<object> inputJsons, long day, IList<QuestBase> outputQuests)
		{
			if (inputJsons == null)
			{
				return;
			}
			if (outputQuests == null)
			{
				return;
			}
			foreach (object obj in inputJsons)
			{
				if (obj != null)
				{
					Dictionary<string, object> dictionary = obj as Dictionary<string, object>;
					if (dictionary == null)
					{
						Debug.LogWarningFormat("Skipping bad quest: {0}", new object[]
						{
							Json.Serialize(obj)
						});
					}
					else
					{
						QuestBase questBase = TutorialQuestManager.CreateQuestFromJson(dictionary, day);
						if (questBase != null)
						{
							if (!questBase.Rewarded)
							{
								if (!this.CheckQuestIfFulfilled(questBase.Id) || !(questBase.CalculateProgress() < 1m))
								{
									outputQuests.Add(questBase);
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x0600410D RID: 16653 RVA: 0x0015B37C File Offset: 0x0015957C
		private static TutorialQuestManager Create()
		{
			TutorialQuestManager result;
			try
			{
				if (!Storager.hasKey("TutorialQuestManager"))
				{
					Storager.setString("TutorialQuestManager", "{}", false);
				}
				string text = Storager.getString("TutorialQuestManager", false);
				if (string.IsNullOrEmpty(text))
				{
					text = "{}";
				}
				Debug.LogFormat("TutorialQuestManager.Create(): parsing data transfer object: {0}", new object[]
				{
					text
				});
				TutorialQuestManager.Memento memento = default(TutorialQuestManager.Memento);
				TutorialQuestManager.Memento memento2 = memento;
				memento2.fulfilledQuests = new List<string>();
				memento = memento2;
				Dictionary<string, object> dictionary = Json.Deserialize(text) as Dictionary<string, object>;
				if (dictionary != null)
				{
					object obj;
					if (dictionary.TryGetValue("fulfilledQuests", out obj))
					{
						List<object> list = obj as List<object>;
						memento.fulfilledQuests = ((list == null) ? new List<string>() : list.OfType<string>().ToList<string>());
					}
					object value;
					if (dictionary.TryGetValue("received", out value))
					{
						memento.received = Convert.ToBoolean(value);
					}
				}
				Debug.Log("TutorialQuestManager.Create(): data transfer object parsed.");
				result = new TutorialQuestManager(memento);
			}
			catch (Exception ex)
			{
				Debug.LogWarningFormat("TutorialQuestManager.Create(): Exception caught: {0}", new object[]
				{
					ex.GetType().Name
				});
				Debug.LogException(ex);
				result = new TutorialQuestManager();
			}
			return result;
		}

		// Token: 0x0600410E RID: 16654 RVA: 0x0015B4D8 File Offset: 0x001596D8
		private static QuestBase CreateQuestFromJson(Dictionary<string, object> questJson, long day)
		{
			if (questJson == null)
			{
				throw new ArgumentNullException("questJson");
			}
			QuestBase result;
			try
			{
				string text = questJson.TryGet("id") as string;
				if (text == null)
				{
					Debug.LogWarningFormat("Failed to create quest, id = null: {0}", new object[]
					{
						Json.Serialize(questJson)
					});
					result = null;
				}
				else
				{
					int slot = Convert.ToInt32(questJson.TryGet("slot") ?? 0);
					Difficulty[] array = new Difficulty[]
					{
						Difficulty.Easy,
						Difficulty.Normal,
						Difficulty.Hard
					};
					Difficulty difficulty = Difficulty.None;
					Dictionary<string, object> dictionary = null;
					foreach (Difficulty difficulty2 in array)
					{
						string difficultyKey = QuestConstants.GetDifficultyKey(difficulty2);
						object obj;
						if (questJson.TryGetValue(difficultyKey, out obj))
						{
							difficulty = difficulty2;
							dictionary = (obj as Dictionary<string, object>);
							break;
						}
					}
					if (dictionary == null)
					{
						Debug.LogWarningFormat("Failed to create quest, difficulty = null: {0}", new object[]
						{
							Json.Serialize(questJson)
						});
						result = null;
					}
					else
					{
						Reward reward = Reward.Create(dictionary.TryGet("reward") as List<object>);
						int requiredCount = Convert.ToInt32(dictionary.TryGet("parameter") ?? 1);
						int initialCount = questJson.TryGet("currentCount").Map(new Func<object, int>(Convert.ToInt32));
						day = questJson.TryGet("day").Map(new Func<object, long>(Convert.ToInt64), day);
						bool rewarded = questJson.TryGet("rewarded").Map(new Func<object, bool>(Convert.ToBoolean));
						bool active = questJson.TryGet("active").Map(new Func<object, bool>(Convert.ToBoolean));
						SimpleAccumulativeQuest simpleAccumulativeQuest = new SimpleAccumulativeQuest(text, day, slot, difficulty, reward, active, rewarded, requiredCount, initialCount);
						result = simpleAccumulativeQuest;
					}
				}
			}
			catch (Exception ex)
			{
				Debug.LogErrorFormat("Caught exception while creating quest object: {0}", new object[]
				{
					ex.Message
				});
				result = null;
			}
			return result;
		}

		// Token: 0x04002F85 RID: 12165
		private const string Key = "TutorialQuestManager";

		// Token: 0x04002F86 RID: 12166
		private static readonly Lazy<TutorialQuestManager> _instance = new Lazy<TutorialQuestManager>(new Func<TutorialQuestManager>(TutorialQuestManager.Create));

		// Token: 0x04002F87 RID: 12167
		private bool _dirty;

		// Token: 0x04002F88 RID: 12168
		private readonly HashSet<string> _fulfilledQuests;

		// Token: 0x04002F89 RID: 12169
		private bool _received;

		// Token: 0x02000739 RID: 1849
		[Serializable]
		internal struct Memento
		{
			// Token: 0x04002F8B RID: 12171
			public List<string> fulfilledQuests;

			// Token: 0x04002F8C RID: 12172
			public bool received;
		}
	}
}
