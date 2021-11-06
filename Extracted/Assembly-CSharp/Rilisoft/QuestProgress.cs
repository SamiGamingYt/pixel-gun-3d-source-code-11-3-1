using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Rilisoft.DictionaryExtensions;
using Rilisoft.NullExtensions;
using UnityEngine;

namespace Rilisoft
{
	// Token: 0x02000733 RID: 1843
	public sealed class QuestProgress : IDisposable
	{
		// Token: 0x06004059 RID: 16473 RVA: 0x00156D60 File Offset: 0x00154F60
		public QuestProgress(string configVersion, long day, DateTime timestamp, float timeLeftSeconds, QuestProgress oldQuestProgress = null)
		{
			if (string.IsNullOrEmpty(configVersion))
			{
				throw new ArgumentException("ConfigId should not be empty.", "configVersion");
			}
			this._events = QuestMediator.Events;
			this._events.Win += this.HandleWin;
			this._events.KillOtherPlayer += this.HandleKillOtherPlayer;
			this._events.KillOtherPlayerWithFlag += this.HandleKillOtherPlayerWithFlag;
			this._events.Capture += this.HandleCapture;
			this._events.KillMonster += this.HandleKillMonster;
			this._events.BreakSeries += this.HandleBreakSeries;
			this._events.MakeSeries += this.HandleMakeSeries;
			this._events.SurviveWaveInArena += new EventHandler<SurviveWaveInArenaEventArgs>(this.HandleSurviveInArena);
			this._events.GetGotcha += this.HandleGetGotcha;
			this._events.SocialInteraction += this.HandleSocialInteraction;
			this._configVersion = configVersion;
			this._timestamp = timestamp;
			this._timeLeftSeconds = timeLeftSeconds;
			this._day = day;
			if (oldQuestProgress != null)
			{
				this._tutorialQuests = oldQuestProgress._tutorialQuests;
				foreach (QuestBase questBase in this._tutorialQuests)
				{
					questBase.Changed += this.OnQuestChangedCheckCompletion;
				}
			}
			UnityEngine.Random.seed = (int)Tools.CurrentUnixTime;
		}

		// Token: 0x14000090 RID: 144
		// (add) Token: 0x0600405A RID: 16474 RVA: 0x00156F44 File Offset: 0x00155144
		// (remove) Token: 0x0600405B RID: 16475 RVA: 0x00156F60 File Offset: 0x00155160
		public event EventHandler<QuestCompletedEventArgs> QuestCompleted;

		// Token: 0x17000AB9 RID: 2745
		// (get) Token: 0x0600405C RID: 16476 RVA: 0x00156F7C File Offset: 0x0015517C
		public string ConfigVersion
		{
			get
			{
				return this._configVersion;
			}
		}

		// Token: 0x17000ABA RID: 2746
		// (get) Token: 0x0600405D RID: 16477 RVA: 0x00156F84 File Offset: 0x00155184
		public long Day
		{
			get
			{
				return this._day;
			}
		}

		// Token: 0x17000ABB RID: 2747
		// (get) Token: 0x0600405E RID: 16478 RVA: 0x00156F8C File Offset: 0x0015518C
		public DateTime Timestamp
		{
			get
			{
				return this._timestamp;
			}
		}

		// Token: 0x17000ABC RID: 2748
		// (get) Token: 0x0600405F RID: 16479 RVA: 0x00156F94 File Offset: 0x00155194
		public float TimeLeftSeconds
		{
			get
			{
				return this._timeLeftSeconds;
			}
		}

		// Token: 0x06004060 RID: 16480 RVA: 0x00156F9C File Offset: 0x0015519C
		public Dictionary<string, object> ToJson()
		{
			Dictionary<string, List<object>> dictionary = new Dictionary<string, List<object>>(3);
			foreach (KeyValuePair<int, List<QuestBase>> keyValuePair in this._currentQuests)
			{
				string key = keyValuePair.Key.ToString(NumberFormatInfo.InvariantInfo);
				List<object> list = new List<object>(2);
				foreach (QuestBase questBase in keyValuePair.Value)
				{
					list.Add(questBase.ToJson());
				}
				dictionary[key] = list;
			}
			Dictionary<string, List<object>> dictionary2 = new Dictionary<string, List<object>>(3);
			foreach (KeyValuePair<int, List<QuestBase>> keyValuePair2 in this._previousQuests)
			{
				string key2 = keyValuePair2.Key.ToString(NumberFormatInfo.InvariantInfo);
				List<object> list2 = new List<object>(2);
				foreach (QuestBase questBase2 in keyValuePair2.Value)
				{
					list2.Add(questBase2.ToJson());
				}
				dictionary2[key2] = list2;
			}
			List<object> list3 = new List<object>(this._tutorialQuests.Count);
			foreach (QuestBase questBase3 in this._tutorialQuests)
			{
				list3.Add(questBase3.ToJson());
			}
			return new Dictionary<string, object>(3)
			{
				{
					"day",
					this._day
				},
				{
					"timestamp",
					this.Timestamp.ToString("s", CultureInfo.InvariantCulture)
				},
				{
					"timeLeftSeconds",
					this.TimeLeftSeconds.ToString(CultureInfo.InvariantCulture)
				},
				{
					"tutorialQuests",
					list3
				},
				{
					"previousQuests",
					dictionary2
				},
				{
					"currentQuests",
					dictionary
				}
			};
		}

		// Token: 0x06004061 RID: 16481 RVA: 0x00157270 File Offset: 0x00155470
		public void UpdateQuests(long day, Dictionary<string, object> rawQuests, IDictionary<int, List<QuestBase>> newQuests)
		{
			if (newQuests == null)
			{
				return;
			}
			this._day = day;
			IEnumerable<int> source = this._previousQuests.Keys.Concat(this._currentQuests.Keys).Distinct<int>();
			Dictionary<int, IList<QuestBase>> dictionary = source.ToDictionary((int s) => s, (int s) => this.GetActiveQuestsBySlot(s, true));
			this.ClearQuests(this._previousQuests);
			foreach (KeyValuePair<int, IList<QuestBase>> keyValuePair in dictionary)
			{
				int key = keyValuePair.Key;
				IList<QuestBase> value = keyValuePair.Value;
				foreach (QuestBase questBase in value)
				{
					questBase.Changed -= this.OnQuestChangedCheckCompletion;
					if (!questBase.Rewarded)
					{
						questBase.Changed += this.OnQuestChangedCheckCompletion;
					}
				}
				this._previousQuests[key] = new List<QuestBase>(from q in value
				where !q.Rewarded
				select q);
			}
			this.ClearQuests(this._currentQuests);
			foreach (KeyValuePair<int, List<QuestBase>> keyValuePair2 in newQuests)
			{
				int key2 = keyValuePair2.Key;
				List<QuestBase> value2 = keyValuePair2.Value;
				List<QuestBase> source2;
				if (!this._previousQuests.TryGetValue(key2, out source2))
				{
					source2 = new List<QuestBase>();
				}
				if (!source2.FirstOrDefault<QuestBase>().Map((QuestBase q) => q.CalculateProgress() < 1m && !q.Rewarded))
				{
					foreach (QuestBase questBase2 in value2)
					{
						questBase2.Changed -= this.OnQuestChangedCheckCompletion;
						questBase2.Changed += this.OnQuestChangedCheckCompletion;
					}
					this._currentQuests[key2] = new List<QuestBase>(value2);
				}
			}
			if (rawQuests != null)
			{
				Difficulty[] allowedDifficulties = this._previousQuests.SelectMany((KeyValuePair<int, List<QuestBase>> kv) => from q in kv.Value
				select q.Difficulty).Distinct<Difficulty>().ToArray<Difficulty>();
				QuestProgress.ParseQuests(rawQuests, new long?(day), allowedDifficulties, this._previousQuests);
			}
			this._dirty = true;
		}

		// Token: 0x06004062 RID: 16482 RVA: 0x00157590 File Offset: 0x00155790
		public void PopulateQuests(IDictionary<int, List<QuestBase>> currentQuests, IDictionary<int, List<QuestBase>> previousQuests)
		{
			if (currentQuests != null)
			{
				foreach (KeyValuePair<int, List<QuestBase>> keyValuePair in currentQuests)
				{
					foreach (QuestBase questBase in keyValuePair.Value)
					{
						questBase.Changed += this.OnQuestChangedCheckCompletion;
					}
					this._currentQuests[keyValuePair.Key] = new List<QuestBase>(keyValuePair.Value);
				}
			}
			if (previousQuests != null)
			{
				foreach (KeyValuePair<int, List<QuestBase>> keyValuePair2 in previousQuests)
				{
					foreach (QuestBase questBase2 in keyValuePair2.Value)
					{
						questBase2.Changed += this.OnQuestChangedCheckCompletion;
					}
					this._previousQuests[keyValuePair2.Key] = new List<QuestBase>(keyValuePair2.Value);
				}
			}
			this._dirty = true;
		}

		// Token: 0x06004063 RID: 16483 RVA: 0x00157748 File Offset: 0x00155948
		public void FillTutorialQuests(List<object> questJsons)
		{
			if (questJsons == null)
			{
				return;
			}
			TutorialQuestManager.Instance.FillTutorialQuests(questJsons, this.Day, this._tutorialQuests);
			foreach (QuestBase questBase in this._tutorialQuests)
			{
				questBase.Changed -= this.OnQuestChangedCheckCompletion;
				questBase.Changed += this.OnQuestChangedCheckCompletion;
			}
			this._dirty = true;
		}

		// Token: 0x06004064 RID: 16484 RVA: 0x001577F0 File Offset: 0x001559F0
		public static IDictionary<int, List<QuestBase>> RestoreQuests(Dictionary<string, object> rawQuests)
		{
			Difficulty[] allowedDifficulties = new Difficulty[]
			{
				Difficulty.Easy,
				Difficulty.Normal,
				Difficulty.Hard
			};
			return QuestProgress.ParseQuests(rawQuests, null, allowedDifficulties);
		}

		// Token: 0x06004065 RID: 16485 RVA: 0x00157824 File Offset: 0x00155A24
		public static IDictionary<int, List<QuestBase>> CreateQuests(Dictionary<string, object> rawQuests, long day, Difficulty[] allowedDifficulties)
		{
			if (allowedDifficulties == null)
			{
				allowedDifficulties = new Difficulty[]
				{
					Difficulty.Easy,
					Difficulty.Normal,
					Difficulty.Hard
				};
			}
			return QuestProgress.ParseQuests(rawQuests, new long?(day), allowedDifficulties);
		}

		// Token: 0x06004066 RID: 16486 RVA: 0x0015785C File Offset: 0x00155A5C
		internal void DebugDecrementDay()
		{
			long newDay = this._day - 172800L;
			IEnumerable<QuestBase> enumerable = from q in this._previousQuests.Values.SelectMany((List<QuestBase> q) => q).Concat(this._currentQuests.Values.SelectMany((List<QuestBase> q) => q)).Concat(this._tutorialQuests)
			where newDay < q.Day
			select q;
			foreach (QuestBase questBase in enumerable)
			{
				questBase.DebugSetDay(newDay);
			}
			this._day = newDay;
			this._dirty = true;
		}

		// Token: 0x06004067 RID: 16487 RVA: 0x0015796C File Offset: 0x00155B6C
		private static IDictionary<int, List<QuestBase>> ParseQuests(Dictionary<string, object> rawQuests, long? dayOption, Difficulty[] allowedDifficulties)
		{
			Dictionary<int, List<QuestBase>> dictionary = new Dictionary<int, List<QuestBase>>(3);
			QuestProgress.ParseQuests(rawQuests, dayOption, allowedDifficulties, dictionary);
			return dictionary;
		}

		// Token: 0x06004068 RID: 16488 RVA: 0x0015798C File Offset: 0x00155B8C
		private static HashSet<ShopNGUIController.CategoryNames> InitializeExcludedWeaponSlots(int slot)
		{
			HashSet<ShopNGUIController.CategoryNames> hashSet = new HashSet<ShopNGUIController.CategoryNames>();
			if (QuestSystem.Instance == null || QuestSystem.Instance.QuestProgress == null)
			{
				return hashSet;
			}
			QuestBase activeQuestBySlot = QuestSystem.Instance.QuestProgress.GetActiveQuestBySlot(slot);
			WeaponSlotAccumulativeQuest weaponSlotAccumulativeQuest = activeQuestBySlot as WeaponSlotAccumulativeQuest;
			if (weaponSlotAccumulativeQuest != null)
			{
				hashSet.Add(weaponSlotAccumulativeQuest.WeaponSlot);
			}
			return hashSet;
		}

		// Token: 0x06004069 RID: 16489 RVA: 0x001579EC File Offset: 0x00155BEC
		private static void ParseQuests(Dictionary<string, object> rawQuests, long? dayOption, Difficulty[] allowedDifficulties, IDictionary<int, List<QuestBase>> actualResult)
		{
			if (actualResult == null)
			{
				return;
			}
			if (rawQuests == null || rawQuests.Count == 0)
			{
				return;
			}
			if (allowedDifficulties == null)
			{
				throw new ArgumentNullException("allowedDifficulties");
			}
			bool flag = dayOption == null;
			IDictionary<int, List<QuestBase>> dictionary;
			if (QuestSystem.Instance.QuestProgress != null)
			{
				dictionary = QuestSystem.Instance.QuestProgress.GetActiveQuests().ToDictionary((KeyValuePair<int, QuestBase> kv) => kv.Key, (KeyValuePair<int, QuestBase> kv) => new List<QuestBase>
				{
					kv.Value
				});
			}
			else
			{
				dictionary = new Dictionary<int, List<QuestBase>>();
			}
			IDictionary<int, List<QuestBase>> existingQuests = dictionary;
			if (allowedDifficulties.Length == 0)
			{
				allowedDifficulties = new Difficulty[]
				{
					Difficulty.Easy,
					Difficulty.Normal,
					Difficulty.Hard
				};
			}
			IDictionary<int, List<Dictionary<string, object>>> dictionary3;
			if (flag)
			{
				IDictionary<int, List<Dictionary<string, object>>> dictionary2 = QuestProgress.ExtractQuests(rawQuests);
				dictionary3 = dictionary2;
			}
			else
			{
				dictionary3 = QuestProgress.FilterQuests(rawQuests, allowedDifficulties, existingQuests);
			}
			IDictionary<int, List<Dictionary<string, object>>> dictionary4 = dictionary3;
			Difficulty[] array = new Difficulty[]
			{
				Difficulty.Easy,
				Difficulty.Normal,
				Difficulty.Hard
			};
			foreach (KeyValuePair<int, List<Dictionary<string, object>>> keyValuePair in dictionary4)
			{
				int key = keyValuePair.Key;
				List<QuestBase> list;
				if (!actualResult.TryGetValue(key, out list))
				{
					list = new List<QuestBase>(2);
				}
				HashSet<ShopNGUIController.CategoryNames> hashSet = QuestProgress.InitializeExcludedWeaponSlots(key);
				foreach (Dictionary<string, object> dictionary5 in keyValuePair.Value)
				{
					string text = dictionary5.TryGet("id") as string;
					if (text != null)
					{
						if (!QuestConstants.IsSupported(text))
						{
							Debug.LogWarning("Quest is not supported: " + text);
						}
						else
						{
							Difficulty difficulty = Difficulty.None;
							object obj = null;
							foreach (Difficulty difficulty2 in array)
							{
								if (dictionary5.TryGetValue(QuestConstants.GetDifficultyKey(difficulty2), out obj))
								{
									difficulty = difficulty2;
									break;
								}
							}
							Dictionary<string, object> dictionary6 = obj as Dictionary<string, object>;
							if (dictionary6 != null && difficulty != Difficulty.None)
							{
								try
								{
									List<object> reward = dictionary6["reward"] as List<object>;
									Reward reward2 = Reward.Create(reward);
									int requiredCount = Convert.ToInt32(dictionary6.TryGet("parameter") ?? 1);
									object value = dictionary5.TryGet("day");
									long day = (dayOption == null) ? Convert.ToInt64(value) : dayOption.Value;
									bool rewarded = dictionary5.TryGet("rewarded").Map(new Func<object, bool>(Convert.ToBoolean));
									bool active = dictionary5.TryGet("active").Map(new Func<object, bool>(Convert.ToBoolean));
									int initialCount = dictionary5.TryGet("currentCount").Map(new Func<object, int>(Convert.ToInt32));
									string text2 = text;
									switch (text2)
									{
									case "killInMode":
									case "winInMode":
									{
										ConnectSceneNGUIController.RegimGame? regimGame = QuestProgress.ExtractModeFromQuestDescription(dictionary5, flag, text);
										if (regimGame == null)
										{
											continue;
										}
										ModeAccumulativeQuest item = new ModeAccumulativeQuest(text, day, key, difficulty, reward2, active, rewarded, requiredCount, regimGame.Value, initialCount);
										list.Add(item);
										continue;
									}
									case "winInMap":
									{
										string text3 = QuestProgress.ExtractMapFromQuestDescription(dictionary5, flag);
										if (string.IsNullOrEmpty(text3))
										{
											continue;
										}
										MapAccumulativeQuest item2 = new MapAccumulativeQuest(text, day, key, difficulty, reward2, active, rewarded, requiredCount, text3, initialCount);
										list.Add(item2);
										continue;
									}
									case "killWithWeapon":
									case "killNpcWithWeapon":
									{
										ShopNGUIController.CategoryNames? categoryNames = QuestProgress.ExtractWeaponSlotFromQuestDescription(dictionary5, flag, hashSet);
										if (categoryNames == null)
										{
											continue;
										}
										hashSet.Add(categoryNames.Value);
										WeaponSlotAccumulativeQuest item3 = new WeaponSlotAccumulativeQuest(text, day, key, difficulty, reward2, active, rewarded, requiredCount, categoryNames.Value, initialCount);
										list.Add(item3);
										continue;
									}
									}
									SimpleAccumulativeQuest item4 = new SimpleAccumulativeQuest(text, day, key, difficulty, reward2, active, rewarded, requiredCount, initialCount);
									list.Add(item4);
								}
								catch (Exception exception)
								{
									Debug.LogException(exception);
								}
							}
						}
					}
				}
				actualResult[key] = list;
			}
		}

		// Token: 0x0600406A RID: 16490 RVA: 0x00157EF0 File Offset: 0x001560F0
		public QuestBase GetActiveQuestBySlot(int slot)
		{
			QuestBase activeTutorialQuest = this.GetActiveTutorialQuest();
			if (activeTutorialQuest != null && activeTutorialQuest.Slot == slot)
			{
				return activeTutorialQuest;
			}
			List<QuestBase> o = null;
			this._previousQuests.TryGetValue(slot, out o);
			QuestBase questBase = o.Map((List<QuestBase> ps) => ps.FirstOrDefault<QuestBase>());
			if (questBase != null && !questBase.Rewarded)
			{
				return questBase;
			}
			List<QuestBase> o2 = null;
			this._currentQuests.TryGetValue(slot, out o2);
			QuestBase questBase2 = o2.Map((List<QuestBase> cs) => cs.FirstOrDefault<QuestBase>());
			if (questBase2 != null)
			{
				return questBase2;
			}
			return questBase;
		}

		// Token: 0x0600406B RID: 16491 RVA: 0x00157FA4 File Offset: 0x001561A4
		public QuestInfo GetActiveQuestInfoBySlot(int slot)
		{
			IList<QuestBase> activeQuestsBySlot = this.GetActiveQuestsBySlot(slot, false);
			Func<IList<QuestBase>> skipMethod = delegate()
			{
				IEnumerable<int> enumerable = this._currentQuests.Keys.Concat(this._previousQuests.Keys).Concat(from q in this._tutorialQuests
				select q.Slot).Distinct<int>();
				foreach (int num in enumerable)
				{
					List<QuestBase> activeQuestsBySlotReference = this.GetActiveQuestsBySlotReference(num, false);
					if (activeQuestsBySlotReference != null && activeQuestsBySlotReference.Count > 0 && slot == num)
					{
						activeQuestsBySlotReference.RemoveAt(0);
						this._dirty = true;
					}
					List<QuestBase> activeQuestsBySlotReference2 = this.GetActiveQuestsBySlotReference(num, true);
					if (activeQuestsBySlotReference2.Count > 1)
					{
						activeQuestsBySlotReference2.RemoveRange(1, activeQuestsBySlotReference2.Count - 1);
						List<QuestBase> list;
						if (activeQuestsBySlotReference2[0].CalculateProgress() >= 1m && this._currentQuests.TryGetValue(num, out list) && list.Count > 1)
						{
							list.RemoveRange(1, list.Count - 1);
						}
						this._dirty = true;
					}
				}
				return this.GetActiveQuestsBySlot(slot, false);
			};
			bool forcedSkip = object.ReferenceEquals(this._tutorialQuests, this.GetActiveQuestsBySlotReference(slot, false));
			return new QuestInfo(activeQuestsBySlot, skipMethod, forcedSkip);
		}

		// Token: 0x0600406C RID: 16492 RVA: 0x0015800C File Offset: 0x0015620C
		public QuestInfo GetRandomQuestInfo()
		{
			IEnumerable<int> source = this._previousQuests.Keys.Concat(this._currentQuests.Keys).Concat(from q in this._tutorialQuests
			select q.Slot).Distinct<int>();
			List<QuestInfo> list = (from qi in source.Select(new Func<int, QuestInfo>(this.GetActiveQuestInfoBySlot))
			where qi.Quest != null && !qi.Quest.Rewarded
			select qi).ToList<QuestInfo>();
			if (list.Count < 1)
			{
				return null;
			}
			QuestInfo questInfo = list[0];
			for (int i = 1; i < list.Count; i++)
			{
				QuestInfo questInfo2 = list[i];
				if (questInfo2.Quest != null)
				{
					if (questInfo.Quest == null)
					{
						questInfo = list[i];
					}
					else
					{
						AccumulativeQuestBase accumulativeQuestBase = questInfo.Quest as AccumulativeQuestBase;
						AccumulativeQuestBase accumulativeQuestBase2 = questInfo2.Quest as AccumulativeQuestBase;
						if (accumulativeQuestBase != null && accumulativeQuestBase2 != null)
						{
							if (accumulativeQuestBase2.RequiredCount - accumulativeQuestBase2.CurrentCount < accumulativeQuestBase.RequiredCount - accumulativeQuestBase.CurrentCount)
							{
								questInfo = questInfo2;
							}
						}
						else if (questInfo.Quest.CalculateProgress() < questInfo2.Quest.CalculateProgress())
						{
							questInfo = questInfo2;
						}
					}
				}
			}
			return questInfo;
		}

		// Token: 0x17000ABD RID: 2749
		// (get) Token: 0x0600406D RID: 16493 RVA: 0x0015817C File Offset: 0x0015637C
		public bool AnyActiveQuest
		{
			get
			{
				IDictionary<int, QuestBase> activeQuests = this.GetActiveQuests();
				return activeQuests.Any((KeyValuePair<int, QuestBase> q) => q.Value != null && !q.Value.Rewarded);
			}
		}

		// Token: 0x0600406E RID: 16494 RVA: 0x001581B4 File Offset: 0x001563B4
		public IDictionary<int, QuestBase> GetActiveQuests()
		{
			IEnumerable<int> enumerable = this._previousQuests.Keys.Concat(this._currentQuests.Keys).Concat(from q in this._tutorialQuests
			select q.Slot).Distinct<int>();
			Dictionary<int, QuestBase> dictionary = new Dictionary<int, QuestBase>();
			foreach (int num in enumerable)
			{
				QuestBase activeQuestBySlot = this.GetActiveQuestBySlot(num);
				if (activeQuestBySlot != null)
				{
					dictionary[num] = activeQuestBySlot;
				}
			}
			return dictionary;
		}

		// Token: 0x0600406F RID: 16495 RVA: 0x00158280 File Offset: 0x00156480
		internal bool TryRemoveTutorialQuest(string questId)
		{
			if (questId == null)
			{
				return false;
			}
			int num = this._tutorialQuests.FindIndex((QuestBase q) => questId.Equals(q.Id, StringComparison.Ordinal));
			if (num < 0)
			{
				return false;
			}
			this._tutorialQuests.RemoveAt(num);
			this._dirty = true;
			return true;
		}

		// Token: 0x06004070 RID: 16496 RVA: 0x001582DC File Offset: 0x001564DC
		private List<QuestBase> GetActiveQuestsBySlotReference(int slot, bool ignoreTutorialQuests = false)
		{
			if (!ignoreTutorialQuests)
			{
				QuestBase activeTutorialQuest = this.GetActiveTutorialQuest();
				if (activeTutorialQuest != null && activeTutorialQuest.Slot == slot)
				{
					return this._tutorialQuests;
				}
			}
			List<QuestBase> list;
			this._previousQuests.TryGetValue(slot, out list);
			if (list.Map(delegate(List<QuestBase> qs)
			{
				bool result;
				if (qs.Count > 0)
				{
					result = qs.All((QuestBase q) => !q.Rewarded);
				}
				else
				{
					result = false;
				}
				return result;
			}))
			{
				return list;
			}
			List<QuestBase> list2;
			if (this._currentQuests.TryGetValue(slot, out list2))
			{
				if (list2.Map((List<QuestBase> qs) => qs.Count > 0))
				{
					return list2;
				}
			}
			return list;
		}

		// Token: 0x06004071 RID: 16497 RVA: 0x00158388 File Offset: 0x00156588
		private IList<QuestBase> GetActiveQuestsBySlot(int slot, bool ignoreTutorialQuests = false)
		{
			List<QuestBase> activeQuestsBySlotReference = this.GetActiveQuestsBySlotReference(slot, ignoreTutorialQuests);
			if (activeQuestsBySlotReference == null)
			{
				return new List<QuestBase>();
			}
			return new List<QuestBase>(activeQuestsBySlotReference);
		}

		// Token: 0x06004072 RID: 16498 RVA: 0x001583B0 File Offset: 0x001565B0
		private QuestBase GetTutorialQuestById(string id)
		{
			if (id == null)
			{
				return null;
			}
			QuestBase activeTutorialQuest = this.GetActiveTutorialQuest();
			if (activeTutorialQuest == null)
			{
				return null;
			}
			if (!id.Equals(activeTutorialQuest.Id, StringComparison.Ordinal))
			{
				return null;
			}
			return activeTutorialQuest;
		}

		// Token: 0x06004073 RID: 16499 RVA: 0x001583EC File Offset: 0x001565EC
		private QuestBase GetActiveTutorialQuest()
		{
			if (this._tutorialQuests.Count == 0)
			{
				return null;
			}
			foreach (QuestBase questBase in this._tutorialQuests)
			{
				if (!questBase.Rewarded)
				{
					return questBase;
				}
			}
			return null;
		}

		// Token: 0x06004074 RID: 16500 RVA: 0x00158478 File Offset: 0x00156678
		private QuestBase GetQuestById(string id)
		{
			if (id == null)
			{
				throw new ArgumentNullException("id");
			}
			IEnumerable<int> source = this._previousQuests.Keys.Concat(this._currentQuests.Keys).Concat(from q in this._tutorialQuests
			select q.Slot).Distinct<int>();
			IEnumerable<QuestBase> source2 = source.Select(new Func<int, QuestBase>(this.GetActiveQuestBySlot));
			return source2.FirstOrDefault((QuestBase q) => q.Id.Equals(id, StringComparison.Ordinal));
		}

		// Token: 0x06004075 RID: 16501 RVA: 0x00158520 File Offset: 0x00156720
		[Obsolete]
		public AccumulativeQuestBase GetRandomInProgressAccumQuest()
		{
			List<AccumulativeQuestBase> list = (from qs in this._previousQuests.Values
			where qs.Count > 0
			select qs.First<QuestBase>() into q
			where q.CalculateProgress() < 1m
			select q).OfType<AccumulativeQuestBase>().ToList<AccumulativeQuestBase>();
			if (list.Count > 0)
			{
				return list[UnityEngine.Random.Range(0, list.Count)];
			}
			List<AccumulativeQuestBase> list2 = (from qs in this._previousQuests.Values
			where qs.Count > 0
			select qs.First<QuestBase>() into q
			where !q.Rewarded
			select q).OfType<AccumulativeQuestBase>().ToList<AccumulativeQuestBase>();
			if (list2.Count > 0)
			{
				return list2[UnityEngine.Random.Range(0, list2.Count)];
			}
			AccumulativeQuestBase[] array = (from qs in this._currentQuests.Values
			where qs.Count > 0
			select qs.First<QuestBase>() into q
			where q.CalculateProgress() < 1m
			select q).OfType<AccumulativeQuestBase>().ToArray<AccumulativeQuestBase>();
			if (array.Length > 0)
			{
				return array[UnityEngine.Random.Range(0, array.Length)];
			}
			AccumulativeQuestBase[] array2 = (from qs in this._currentQuests.Values
			where qs.Count > 0
			select qs.First<QuestBase>() into q
			where !q.Rewarded
			select q).OfType<AccumulativeQuestBase>().ToArray<AccumulativeQuestBase>();
			if (array2.Length > 0)
			{
				return array2[UnityEngine.Random.Range(0, array2.Length)];
			}
			return null;
		}

		// Token: 0x06004076 RID: 16502 RVA: 0x00158788 File Offset: 0x00156988
		public bool HasUnrewaredAccumQuests()
		{
			IEnumerable<QuestBase> first = from qs in this._currentQuests.Values
			where qs.Count > 0
			select qs[0];
			IEnumerable<QuestBase> second = from qs in this._previousQuests.Values
			where qs.Count > 0
			select qs[0];
			IEnumerable<AccumulativeQuestBase> source = first.Concat(second).Concat(this._tutorialQuests).OfType<AccumulativeQuestBase>();
			return source.Any((AccumulativeQuestBase q) => q.CalculateProgress() >= 1m && !q.Rewarded);
		}

		// Token: 0x17000ABE RID: 2750
		// (get) Token: 0x06004077 RID: 16503 RVA: 0x00158874 File Offset: 0x00156A74
		public int Count
		{
			get
			{
				return this._currentQuests.Count + this._previousQuests.Count;
			}
		}

		// Token: 0x06004078 RID: 16504 RVA: 0x00158890 File Offset: 0x00156A90
		public bool IsDirty()
		{
			if (!this._dirty)
			{
				if (!this._currentQuests.Values.SelectMany((List<QuestBase> q) => q).Any((QuestBase q) => q.Dirty))
				{
					if (!this._previousQuests.Values.SelectMany((List<QuestBase> q) => q).Any((QuestBase q) => q.Dirty))
					{
						return this._tutorialQuests.Any((QuestBase q) => q.Dirty);
					}
				}
			}
			return true;
		}

		// Token: 0x06004079 RID: 16505 RVA: 0x0015897C File Offset: 0x00156B7C
		public void SetClean()
		{
			foreach (List<QuestBase> list in this._currentQuests.Values)
			{
				foreach (QuestBase questBase in list)
				{
					questBase.SetClean();
				}
			}
			foreach (List<QuestBase> list2 in this._previousQuests.Values)
			{
				foreach (QuestBase questBase2 in list2)
				{
					questBase2.SetClean();
				}
			}
			this._dirty = false;
		}

		// Token: 0x0600407A RID: 16506 RVA: 0x00158AE0 File Offset: 0x00156CE0
		private void ClearQuests(IDictionary<int, List<QuestBase>> quests)
		{
			if (quests == null)
			{
				return;
			}
			foreach (KeyValuePair<int, List<QuestBase>> keyValuePair in quests)
			{
				keyValuePair.Value.Clear();
			}
			quests.Clear();
		}

		// Token: 0x0600407B RID: 16507 RVA: 0x00158B50 File Offset: 0x00156D50
		private static IDictionary<int, List<Dictionary<string, object>>> ExtractQuests(Dictionary<string, object> rawQuests)
		{
			Dictionary<int, List<Dictionary<string, object>>> dictionary = new Dictionary<int, List<Dictionary<string, object>>>();
			foreach (KeyValuePair<string, object> keyValuePair in rawQuests)
			{
				int key;
				if (int.TryParse(keyValuePair.Key, NumberStyles.Integer, NumberFormatInfo.InvariantInfo, out key))
				{
					List<object> list = keyValuePair.Value as List<object>;
					if (list == null)
					{
						dictionary[key] = new List<Dictionary<string, object>>();
					}
					else
					{
						List<Dictionary<string, object>> value = list.OfType<Dictionary<string, object>>().ToList<Dictionary<string, object>>();
						dictionary[key] = value;
					}
				}
			}
			return dictionary;
		}

		// Token: 0x0600407C RID: 16508 RVA: 0x00158C0C File Offset: 0x00156E0C
		private static IDictionary<int, List<Dictionary<string, object>>> FilterQuests(Dictionary<string, object> rawQuests, Difficulty[] allowedDifficulties, IDictionary<int, List<QuestBase>> existingQuests)
		{
			if (existingQuests == null)
			{
				existingQuests = new Dictionary<int, List<QuestBase>>();
			}
			Dictionary<int, Dictionary<string, Dictionary<string, object>>> dictionary = new Dictionary<int, Dictionary<string, Dictionary<string, object>>>();
			foreach (KeyValuePair<string, object> keyValuePair in rawQuests)
			{
				Dictionary<string, object> dictionary2 = keyValuePair.Value as Dictionary<string, object>;
				if (dictionary2 != null)
				{
					object value;
					if (dictionary2.TryGetValue("slot", out value))
					{
						if (QuestConstants.IsSupported(keyValuePair.Key))
						{
							try
							{
								int key = Convert.ToInt32(value, NumberFormatInfo.InvariantInfo);
								Dictionary<string, Dictionary<string, object>> dictionary3;
								if (!dictionary.TryGetValue(key, out dictionary3))
								{
									dictionary3 = new Dictionary<string, Dictionary<string, object>>(3);
									dictionary[key] = dictionary3;
								}
								dictionary3[keyValuePair.Key] = dictionary2;
							}
							catch (Exception exception)
							{
								Debug.LogException(exception);
							}
						}
					}
				}
			}
			List<Difficulty> list = new List<Difficulty>(dictionary.Count);
			for (int num = 0; num != dictionary.Count; num++)
			{
				Difficulty item = allowedDifficulties[num % allowedDifficulties.Length];
				list.Add(item);
			}
			QuestProgress.ShuffleInPlace<Difficulty>(list);
			Dictionary<int, List<Dictionary<string, object>>> dictionary4 = new Dictionary<int, List<Dictionary<string, object>>>();
			Dictionary<int, Dictionary<string, Dictionary<string, object>>>.Enumerator enumerator2 = dictionary.GetEnumerator();
			List<Difficulty> source = new List<Difficulty>
			{
				Difficulty.Easy,
				Difficulty.Normal,
				Difficulty.Hard
			};
			int num2 = 0;
			while (enumerator2.MoveNext())
			{
				KeyValuePair<int, Dictionary<string, Dictionary<string, object>>> keyValuePair2 = enumerator2.Current;
				int key2 = keyValuePair2.Key;
				KeyValuePair<int, Dictionary<string, Dictionary<string, object>>> keyValuePair3 = enumerator2.Current;
				Dictionary<string, Dictionary<string, object>> value2 = keyValuePair3.Value;
				List<QuestBase> o;
				bool flag = existingQuests.TryGetValue(key2, out o);
				Difficulty chosenDifficulty = list[num2];
				string chosenDifficultyKey = QuestConstants.GetDifficultyKey(chosenDifficulty);
				List<KeyValuePair<string, Dictionary<string, object>>> list2 = (from kv in value2
				where kv.Value.ContainsKey(chosenDifficultyKey)
				select kv).ToList<KeyValuePair<string, Dictionary<string, object>>>();
				if (list2.Count == 0)
				{
					value2.Clear();
				}
				else
				{
					if (list2.Count > 1)
					{
						string existingQuestId = o.Map((List<QuestBase> l) => l.FirstOrDefault<QuestBase>()).Map((QuestBase q) => q.Id);
						list2.RemoveAll((KeyValuePair<string, Dictionary<string, object>> kv) => StringComparer.OrdinalIgnoreCase.Equals(kv.Key, existingQuestId));
					}
					List<int> list3 = Enumerable.Range(0, list2.Count).ToList<int>();
					QuestProgress.ShuffleInPlace<int>(list3);
					KeyValuePair<string, Dictionary<string, object>> keyValuePair4 = list2[list3[0]];
					keyValuePair4.Value["id"] = keyValuePair4.Key;
					value2.Clear();
					value2[keyValuePair4.Key] = keyValuePair4.Value;
					List<Dictionary<string, object>> list4 = new List<Dictionary<string, object>>(2);
					list4.Add(keyValuePair4.Value.ToDictionary((KeyValuePair<string, object> kv) => kv.Key, (KeyValuePair<string, object> kv) => kv.Value));
					List<Dictionary<string, object>> list5 = list4;
					if (o.Map((List<QuestBase> l) => l.Count == 0, true))
					{
						KeyValuePair<string, Dictionary<string, object>> keyValuePair5 = list2[list3[list3.Count - 1]];
						keyValuePair5.Value["id"] = keyValuePair5.Key;
						list5.Add(keyValuePair5.Value.ToDictionary((KeyValuePair<string, object> kv) => kv.Key, (KeyValuePair<string, object> kv) => kv.Value));
					}
					dictionary4[key2] = list5;
					IEnumerable<Difficulty> enumerable = from d in source
					where d != chosenDifficulty
					select d;
					foreach (Difficulty difficulty in enumerable)
					{
						string difficultyKey = QuestConstants.GetDifficultyKey(difficulty);
						foreach (Dictionary<string, object> dictionary5 in list5)
						{
							dictionary5.Remove(difficultyKey);
						}
					}
				}
				num2++;
			}
			return dictionary4;
		}

		// Token: 0x0600407D RID: 16509 RVA: 0x001590F0 File Offset: 0x001572F0
		private static string ExtractMapFromQuestDescription(Dictionary<string, object> q, bool restore)
		{
			if (q == null || q.Count == 0)
			{
				return string.Empty;
			}
			if (restore)
			{
				object value;
				if (!q.TryGetValue("map", out value))
				{
					return null;
				}
				return Convert.ToString(value);
			}
			else
			{
				string[] supportedMaps = QuestProgress.GetSupportedMaps();
				object obj;
				if (!q.TryGetValue("maps", out obj))
				{
					return supportedMaps[UnityEngine.Random.Range(0, supportedMaps.Length - 1)];
				}
				List<object> list = obj as List<object>;
				if (list == null)
				{
					return string.Empty;
				}
				string[] array = list.OfType<string>().Intersect(supportedMaps).ToArray<string>();
				if (array.Length == 0)
				{
					return string.Empty;
				}
				return array[UnityEngine.Random.Range(0, array.Length - 1)];
			}
		}

		// Token: 0x0600407E RID: 16510 RVA: 0x001591A0 File Offset: 0x001573A0
		private static string[] GetSupportedMaps()
		{
			if (QuestProgress._supportedMapsCache != null && QuestProgress._supportedMapsCache.IsAlive)
			{
				return (string[])QuestProgress._supportedMapsCache.Target;
			}
			int level = ExperienceController.sharedController.Map((ExperienceController xp) => xp.currentLevel, 1);
			HashSet<TypeModeGame> unlockedModesByLevel = SceneInfoController.GetUnlockedModesByLevel(level);
			unlockedModesByLevel.Remove(TypeModeGame.Dater);
			HashSet<string> hashSet = new HashSet<string>();
			foreach (SceneInfo sceneInfo in SceneInfoController.instance.allScenes)
			{
				if (!sceneInfo.isPremium)
				{
					if (!(sceneInfo.NameScene == "Developer_Scene"))
					{
						if (!(sceneInfo.NameScene == "Matrix"))
						{
							foreach (TypeModeGame curMode in unlockedModesByLevel)
							{
								if (sceneInfo.IsAvaliableForMode(curMode))
								{
									hashSet.Add(sceneInfo.NameScene);
								}
							}
						}
					}
				}
			}
			string[] array = hashSet.ToArray<string>();
			QuestProgress._supportedMapsCache = new WeakReference(array, false);
			return array;
		}

		// Token: 0x0600407F RID: 16511 RVA: 0x00159334 File Offset: 0x00157534
		private static ShopNGUIController.CategoryNames? ExtractWeaponSlotFromQuestDescription(Dictionary<string, object> q, bool restore, HashSet<ShopNGUIController.CategoryNames> excluded)
		{
			if (q == null || q.Count == 0)
			{
				return null;
			}
			if (restore)
			{
				object value;
				if (!q.TryGetValue("weaponSlot", out value))
				{
					return null;
				}
				return QuestConstants.ParseWeaponSlot(Convert.ToString(value));
			}
			else
			{
				if (excluded == null)
				{
					excluded = new HashSet<ShopNGUIController.CategoryNames>();
				}
				List<ShopNGUIController.CategoryNames> list = Enum.GetValues(typeof(ShopNGUIController.CategoryNames)).Cast<ShopNGUIController.CategoryNames>().Where(new Func<ShopNGUIController.CategoryNames, bool>(ShopNGUIController.IsWeaponCategory)).ToList<ShopNGUIController.CategoryNames>();
				object obj;
				if (!q.TryGetValue("weaponSlots", out obj))
				{
					List<ShopNGUIController.CategoryNames> list2 = list.Except(excluded).ToList<ShopNGUIController.CategoryNames>();
					list2 = ((list2.Count <= 0) ? list : list2);
					return new ShopNGUIController.CategoryNames?(list2[UnityEngine.Random.Range(0, list2.Count - 1)]);
				}
				List<object> list3 = obj as List<object>;
				if (list3 == null)
				{
					return null;
				}
				IEnumerable<string> enumerable = list3.OfType<string>();
				List<ShopNGUIController.CategoryNames> list4 = new List<ShopNGUIController.CategoryNames>();
				foreach (string weaponSlot in enumerable)
				{
					ShopNGUIController.CategoryNames? categoryNames = QuestConstants.ParseWeaponSlot(weaponSlot);
					if (categoryNames != null)
					{
						if (list.Contains(categoryNames.Value))
						{
							list4.Add(categoryNames.Value);
						}
					}
				}
				if (list4.Count == 0)
				{
					return null;
				}
				List<ShopNGUIController.CategoryNames> list5 = list4.Except(excluded).ToList<ShopNGUIController.CategoryNames>();
				list5 = ((list5.Count <= 0) ? list4 : list5);
				return new ShopNGUIController.CategoryNames?(list5[UnityEngine.Random.Range(0, list5.Count - 1)]);
			}
		}

		// Token: 0x06004080 RID: 16512 RVA: 0x00159524 File Offset: 0x00157724
		private static ConnectSceneNGUIController.RegimGame? ExtractModeFromQuestDescription(Dictionary<string, object> q, bool restore, string questId)
		{
			if (q == null || q.Count == 0)
			{
				return null;
			}
			if (restore)
			{
				object value;
				if (!q.TryGetValue("mode", out value))
				{
					return null;
				}
				return QuestConstants.ParseMode(Convert.ToString(value));
			}
			else
			{
				ConnectSceneNGUIController.RegimGame[] supportedModes = QuestProgress.GetSupportedModes();
				List<ConnectSceneNGUIController.RegimGame> list = new List<ConnectSceneNGUIController.RegimGame>(supportedModes.Length);
				foreach (ConnectSceneNGUIController.RegimGame regimGame in supportedModes)
				{
					bool flag = regimGame == ConnectSceneNGUIController.RegimGame.TimeBattle && StringComparer.OrdinalIgnoreCase.Equals("killInMode", questId);
					if (!flag)
					{
						list.Add(regimGame);
					}
				}
				object obj;
				if (!q.TryGetValue("modes", out obj))
				{
					return (list.Count <= 0) ? null : new ConnectSceneNGUIController.RegimGame?(list[UnityEngine.Random.Range(0, list.Count - 1)]);
				}
				List<object> list2 = obj as List<object>;
				if (list2 == null)
				{
					return null;
				}
				IEnumerable<string> enumerable = list2.OfType<string>();
				List<ConnectSceneNGUIController.RegimGame> list3 = new List<ConnectSceneNGUIController.RegimGame>();
				foreach (string mode in enumerable)
				{
					ConnectSceneNGUIController.RegimGame? regimGame2 = QuestConstants.ParseMode(mode);
					if (regimGame2 != null)
					{
						if (list.Contains(regimGame2.Value))
						{
							list3.Add(regimGame2.Value);
						}
					}
				}
				if (list3.Count == 0)
				{
					return null;
				}
				return new ConnectSceneNGUIController.RegimGame?(list3[UnityEngine.Random.Range(0, list3.Count - 1)]);
			}
		}

		// Token: 0x06004081 RID: 16513 RVA: 0x00159710 File Offset: 0x00157910
		private static ConnectSceneNGUIController.RegimGame[] GetSupportedModes()
		{
			if (QuestProgress._supportedModesCache != null && QuestProgress._supportedModesCache.IsAlive)
			{
				return (ConnectSceneNGUIController.RegimGame[])QuestProgress._supportedModesCache.Target;
			}
			int level = ExperienceController.sharedController.Map((ExperienceController xp) => xp.currentLevel, 1);
			HashSet<TypeModeGame> unlockedModesByLevel = SceneInfoController.GetUnlockedModesByLevel(level);
			HashSet<ConnectSceneNGUIController.RegimGame> source = SceneInfoController.SelectModes(unlockedModesByLevel);
			ConnectSceneNGUIController.RegimGame[] array = source.ToArray<ConnectSceneNGUIController.RegimGame>();
			QuestProgress._supportedModesCache = new WeakReference(array, false);
			return array;
		}

		// Token: 0x06004082 RID: 16514 RVA: 0x00159794 File Offset: 0x00157994
		private static void ShuffleInPlace<T>(List<T> list)
		{
			if (list == null)
			{
				throw new ArgumentNullException("list");
			}
			if (list.Count < 2)
			{
				return;
			}
			for (int i = list.Count - 1; i >= 1; i--)
			{
				int index = UnityEngine.Random.Range(0, i);
				T value = list[index];
				list[index] = list[i];
				list[i] = value;
			}
		}

		// Token: 0x06004083 RID: 16515 RVA: 0x00159800 File Offset: 0x00157A00
		private static List<T> Shuffle<T>(IEnumerable<T> list)
		{
			if (list == null)
			{
				throw new ArgumentNullException("list");
			}
			List<T> list2 = list.ToList<T>();
			QuestProgress.ShuffleInPlace<T>(list2);
			return list2;
		}

		// Token: 0x06004084 RID: 16516 RVA: 0x0015982C File Offset: 0x00157A2C
		public void FilterFulfilledTutorialQuests()
		{
			this._tutorialQuests.RemoveAll((QuestBase tq) => TutorialQuestManager.Instance.CheckQuestIfFulfilled(tq.Id) && tq.CalculateProgress() < 1m);
		}

		// Token: 0x06004085 RID: 16517 RVA: 0x00159858 File Offset: 0x00157A58
		private void OnQuestChangedCheckCompletion(object sender, EventArgs e)
		{
			QuestBase quest = sender as QuestBase;
			if (quest == null)
			{
				return;
			}
			if (quest.CalculateProgress() >= 1m)
			{
				string callee = string.Format(CultureInfo.InvariantCulture, "{0}.OnQuestChangedCheckCompletion({1})", new object[]
				{
					base.GetType().Name,
					quest.Id
				});
				using (new ScopeLogger(callee, Defs.IsDeveloperBuild))
				{
					this.QuestCompleted.Do(delegate(EventHandler<QuestCompletedEventArgs> handler)
					{
						handler(this, new QuestCompletedEventArgs
						{
							Quest = quest
						});
					});
				}
			}
		}

		// Token: 0x06004086 RID: 16518 RVA: 0x00159928 File Offset: 0x00157B28
		private void HandleWin(object sender, WinEventArgs e)
		{
			if (Defs.IsDeveloperBuild)
			{
				Debug.Log("HandleWin(): " + e);
			}
			QuestBase questById = this.GetQuestById("winInMap");
			if (questById != null)
			{
				MapAccumulativeQuest mapAccumulativeQuest = questById as MapAccumulativeQuest;
				if (mapAccumulativeQuest != null)
				{
					mapAccumulativeQuest.IncrementIf(mapAccumulativeQuest.Map.Equals(e.Map, StringComparison.Ordinal), 1);
				}
			}
			questById = this.GetQuestById("winInMode");
			if (questById != null)
			{
				ModeAccumulativeQuest modeAccumulativeQuest = questById as ModeAccumulativeQuest;
				if (modeAccumulativeQuest != null)
				{
					modeAccumulativeQuest.IncrementIf(modeAccumulativeQuest.Mode == e.Mode, 1);
				}
			}
		}

		// Token: 0x06004087 RID: 16519 RVA: 0x001599BC File Offset: 0x00157BBC
		private void HandleKillOtherPlayer(object sender, KillOtherPlayerEventArgs e)
		{
			if (Defs.IsDeveloperBuild)
			{
				Debug.Log("HandleKillOtherPlayer(): " + e);
			}
			QuestBase questById = this.GetQuestById("killInMode");
			if (questById != null)
			{
				(questById as ModeAccumulativeQuest).Do(delegate(ModeAccumulativeQuest quest)
				{
					quest.IncrementIf(e.Mode == quest.Mode, 1);
				});
			}
			questById = this.GetQuestById("killWithWeapon");
			if (questById != null)
			{
				(questById as WeaponSlotAccumulativeQuest).Do(delegate(WeaponSlotAccumulativeQuest quest)
				{
					quest.IncrementIf(e.WeaponSlot == quest.WeaponSlot, 1);
				});
			}
			string[] source = new string[]
			{
				"killViaHeadshot",
				"killWithGrenade",
				"revenge"
			};
			Dictionary<string, SimpleAccumulativeQuest> dictionary = source.Select(new Func<string, QuestBase>(this.GetQuestById)).OfType<SimpleAccumulativeQuest>().ToDictionary((SimpleAccumulativeQuest q) => q.Id, (SimpleAccumulativeQuest q) => q);
			SimpleAccumulativeQuest simpleAccumulativeQuest;
			if (dictionary.TryGetValue("killViaHeadshot", out simpleAccumulativeQuest))
			{
				simpleAccumulativeQuest.IncrementIf(e.Headshot, 1);
			}
			if (dictionary.TryGetValue("killWithGrenade", out simpleAccumulativeQuest))
			{
				simpleAccumulativeQuest.IncrementIf(e.Grenade, 1);
			}
			if (dictionary.TryGetValue("revenge", out simpleAccumulativeQuest))
			{
				simpleAccumulativeQuest.IncrementIf(e.Revenge, 1);
			}
		}

		// Token: 0x06004088 RID: 16520 RVA: 0x00159B34 File Offset: 0x00157D34
		private void HandleKillOtherPlayerWithFlag(object sender, EventArgs e)
		{
			if (Defs.IsDeveloperBuild)
			{
				Debug.Log("HandleKillOtherPlayerWithFlag(): " + e);
			}
			QuestBase questById = this.GetQuestById("killFlagCarriers");
			if (questById != null)
			{
				(questById as SimpleAccumulativeQuest).Do(delegate(SimpleAccumulativeQuest quest)
				{
					quest.Increment(1);
				});
			}
		}

		// Token: 0x06004089 RID: 16521 RVA: 0x00159B98 File Offset: 0x00157D98
		private void HandleCapture(object sender, CaptureEventArgs e)
		{
			if (Defs.IsDeveloperBuild)
			{
				Debug.Log("HandleCapture(): " + e);
			}
			string[] source = new string[]
			{
				"captureFlags",
				"capturePoints"
			};
			Dictionary<string, SimpleAccumulativeQuest> dictionary = source.Select(new Func<string, QuestBase>(this.GetQuestById)).OfType<SimpleAccumulativeQuest>().ToDictionary((SimpleAccumulativeQuest q) => q.Id, (SimpleAccumulativeQuest q) => q);
			SimpleAccumulativeQuest simpleAccumulativeQuest;
			if (dictionary.TryGetValue("capturePoints", out simpleAccumulativeQuest))
			{
				simpleAccumulativeQuest.IncrementIf(e.Mode == ConnectSceneNGUIController.RegimGame.CapturePoints, 1);
			}
			if (dictionary.TryGetValue("captureFlags", out simpleAccumulativeQuest))
			{
				simpleAccumulativeQuest.IncrementIf(e.Mode == ConnectSceneNGUIController.RegimGame.FlagCapture, 1);
			}
		}

		// Token: 0x0600408A RID: 16522 RVA: 0x00159C74 File Offset: 0x00157E74
		private void HandleKillMonster(object sender, KillMonsterEventArgs e)
		{
			if (Defs.IsDeveloperBuild)
			{
				Debug.Log("HandleKillMonster(): " + e);
			}
			QuestBase questById = this.GetQuestById("killInCampaign");
			if (questById != null)
			{
				(questById as SimpleAccumulativeQuest).Do(delegate(SimpleAccumulativeQuest quest)
				{
					quest.IncrementIf(e.Campaign, 1);
				});
			}
			questById = this.GetQuestById("killNpcWithWeapon");
			if (questById != null)
			{
				(questById as WeaponSlotAccumulativeQuest).Do(delegate(WeaponSlotAccumulativeQuest quest)
				{
					quest.IncrementIf(e.WeaponSlot == quest.WeaponSlot, 1);
				});
			}
		}

		// Token: 0x0600408B RID: 16523 RVA: 0x00159D04 File Offset: 0x00157F04
		private void HandleBreakSeries(object sender, EventArgs e)
		{
			if (Defs.IsDeveloperBuild)
			{
				Debug.Log("HandleBreakSeries()");
			}
			QuestBase questById = this.GetQuestById("breakSeries");
			if (questById != null)
			{
				(questById as SimpleAccumulativeQuest).Do(delegate(SimpleAccumulativeQuest quest)
				{
					quest.Increment(1);
				});
			}
		}

		// Token: 0x0600408C RID: 16524 RVA: 0x00159D60 File Offset: 0x00157F60
		private void HandleMakeSeries(object sender, EventArgs e)
		{
			if (Defs.IsDeveloperBuild)
			{
				Debug.Log("HandleMakeSeries()");
			}
			QuestBase questById = this.GetQuestById("makeSeries");
			if (questById != null)
			{
				(questById as SimpleAccumulativeQuest).Do(delegate(SimpleAccumulativeQuest quest)
				{
					quest.Increment(1);
				});
			}
		}

		// Token: 0x0600408D RID: 16525 RVA: 0x00159DBC File Offset: 0x00157FBC
		private void HandleSurviveInArena(object sender, EventArgs e)
		{
			if (Defs.IsDeveloperBuild)
			{
				Debug.Log("HandleSurviveInArena()");
			}
			QuestBase questById = this.GetQuestById("surviveWavesInArena");
			if (questById != null)
			{
				(questById as SimpleAccumulativeQuest).Do(delegate(SimpleAccumulativeQuest quest)
				{
					quest.Increment(1);
				});
			}
		}

		// Token: 0x0600408E RID: 16526 RVA: 0x00159E18 File Offset: 0x00158018
		private void HandleGetGotcha(object sender, EventArgs e)
		{
			if (Defs.IsDeveloperBuild)
			{
				Debug.Log("HandleGetGotcha()");
			}
			QuestBase tutorialQuestById = this.GetTutorialQuestById("getGotcha");
			if (tutorialQuestById != null)
			{
				(tutorialQuestById as SimpleAccumulativeQuest).Do(delegate(SimpleAccumulativeQuest quest)
				{
					quest.Increment(1);
				});
			}
		}

		// Token: 0x0600408F RID: 16527 RVA: 0x00159E74 File Offset: 0x00158074
		private void HandleSocialInteraction(object sender, SocialInteractionEventArgs e)
		{
			if (Defs.IsDeveloperBuild)
			{
				Debug.LogFormat("HandleSocialInteraction('{0}')", new object[]
				{
					e.Kind
				});
			}
			QuestBase tutorialQuestById = this.GetTutorialQuestById(e.Kind);
			if (tutorialQuestById != null)
			{
				(tutorialQuestById as SimpleAccumulativeQuest).Do(delegate(SimpleAccumulativeQuest quest)
				{
					quest.Increment(1);
				});
			}
		}

		// Token: 0x17000ABF RID: 2751
		// (get) Token: 0x06004090 RID: 16528 RVA: 0x00159EE0 File Offset: 0x001580E0
		public bool Disposed
		{
			get
			{
				return this._disposed;
			}
		}

		// Token: 0x06004091 RID: 16529 RVA: 0x00159EE8 File Offset: 0x001580E8
		public void Dispose()
		{
			if (this._disposed)
			{
				return;
			}
			foreach (QuestBase questBase in this._tutorialQuests)
			{
				questBase.Changed -= this.OnQuestChangedCheckCompletion;
			}
			foreach (QuestBase questBase2 in this._currentQuests.SelectMany((KeyValuePair<int, List<QuestBase>> kv) => kv.Value))
			{
				questBase2.Changed -= this.OnQuestChangedCheckCompletion;
			}
			foreach (QuestBase questBase3 in this._previousQuests.SelectMany((KeyValuePair<int, List<QuestBase>> kv) => kv.Value))
			{
				questBase3.Changed -= this.OnQuestChangedCheckCompletion;
			}
			this._events.Win -= this.HandleWin;
			this._events.KillOtherPlayer -= this.HandleKillOtherPlayer;
			this._events.KillOtherPlayerWithFlag -= this.HandleKillOtherPlayerWithFlag;
			this._events.Capture -= this.HandleCapture;
			this._events.KillMonster -= this.HandleKillMonster;
			this._events.BreakSeries -= this.HandleBreakSeries;
			this._events.MakeSeries -= this.HandleMakeSeries;
			this._events.SurviveWaveInArena -= new EventHandler<SurviveWaveInArenaEventArgs>(this.HandleSurviveInArena);
			this.QuestCompleted = null;
			this._disposed = true;
		}

		// Token: 0x04002F26 RID: 12070
		private const bool TutorialQuestsSupported = true;

		// Token: 0x04002F27 RID: 12071
		private bool _disposed;

		// Token: 0x04002F28 RID: 12072
		private static WeakReference _supportedMapsCache;

		// Token: 0x04002F29 RID: 12073
		private static WeakReference _supportedModesCache;

		// Token: 0x04002F2A RID: 12074
		private readonly IDictionary<int, List<QuestBase>> _currentQuests = new Dictionary<int, List<QuestBase>>(3);

		// Token: 0x04002F2B RID: 12075
		private readonly IDictionary<int, List<QuestBase>> _previousQuests = new Dictionary<int, List<QuestBase>>(3);

		// Token: 0x04002F2C RID: 12076
		private readonly List<QuestBase> _tutorialQuests = new List<QuestBase>();

		// Token: 0x04002F2D RID: 12077
		private readonly QuestEvents _events;

		// Token: 0x04002F2E RID: 12078
		private readonly string _configVersion;

		// Token: 0x04002F2F RID: 12079
		private readonly DateTime _timestamp;

		// Token: 0x04002F30 RID: 12080
		private readonly float _timeLeftSeconds;

		// Token: 0x04002F31 RID: 12081
		private bool _dirty;

		// Token: 0x04002F32 RID: 12082
		private long _day;
	}
}
