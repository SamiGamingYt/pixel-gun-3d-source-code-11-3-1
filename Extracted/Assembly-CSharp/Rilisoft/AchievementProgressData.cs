using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Rilisoft.MiniJson;
using UnityEngine;

namespace Rilisoft
{
	// Token: 0x02000519 RID: 1305
	[Serializable]
	public class AchievementProgressData
	{
		// Token: 0x06002D73 RID: 11635 RVA: 0x000EEB04 File Offset: 0x000ECD04
		public AchievementProgressData()
		{
			this.CustomData = new Dictionary<string, object>();
		}

		// Token: 0x06002D74 RID: 11636 RVA: 0x000EEB18 File Offset: 0x000ECD18
		public override bool Equals(object obj)
		{
			AchievementProgressData o = obj as AchievementProgressData;
			if (o == null)
			{
				return false;
			}
			if (this.AchievementId != o.AchievementId || this.Points != o.Points || this.Stage != o.Stage)
			{
				return false;
			}
			if (this.CustomData == null && o.CustomData == null)
			{
				return true;
			}
			if ((this.CustomData == null && o.CustomData != null) || (this.CustomData != null && o.CustomData == null))
			{
				return false;
			}
			if (this.CustomData.Keys.Count != o.CustomData.Keys.Count)
			{
				return false;
			}
			if (!this.CustomData.Keys.All((string k) => o.CustomData.Keys.Contains(k)) || !o.CustomData.Keys.All((string k) => this.CustomData.Keys.Contains(k)))
			{
				return false;
			}
			int num = 0;
			List<string> list = o.CustomData.Keys.ToList<string>();
			foreach (KeyValuePair<string, object> keyValuePair in this.CustomData)
			{
				object value = keyValuePair.Value;
				object obj2 = o.CustomData[list[num]];
				if (value != null || obj2 != null)
				{
					if ((value == null && obj2 != null) || (value != null && obj2 == null))
					{
						return false;
					}
					if (keyValuePair.Value.ToString() != obj2.ToString())
					{
						return false;
					}
					num++;
				}
			}
			return true;
		}

		// Token: 0x06002D75 RID: 11637 RVA: 0x000EED5C File Offset: 0x000ECF5C
		public static AchievementProgressData Create(string raw)
		{
			if (!raw.IsNullOrEmpty())
			{
				Dictionary<string, object> obj = Json.Deserialize(raw) as Dictionary<string, object>;
				return AchievementProgressData.Create(obj);
			}
			return null;
		}

		// Token: 0x06002D76 RID: 11638 RVA: 0x000EED88 File Offset: 0x000ECF88
		public static AchievementProgressData Create(Dictionary<string, object> obj)
		{
			if (obj == null)
			{
				return null;
			}
			AchievementProgressData achievementProgressData = new AchievementProgressData();
			achievementProgressData.AchievementId = obj.ParseJSONField("id", new Func<object, int>(AchievementProgressData.ConvertToInt32Invariant), false);
			achievementProgressData.Points = obj.ParseJSONField("p", new Func<object, int>(AchievementProgressData.ConvertToInt32Invariant), false);
			achievementProgressData.Stage = obj.ParseJSONField("s", new Func<object, int>(AchievementProgressData.ConvertToInt32Invariant), false);
			achievementProgressData.CustomData = (obj.ParseJSONField("cd", (object o) => o as Dictionary<string, object>, true) ?? new Dictionary<string, object>());
			return achievementProgressData;
		}

		// Token: 0x06002D77 RID: 11639 RVA: 0x000EEE3C File Offset: 0x000ED03C
		private static int ConvertToInt32Invariant(object o)
		{
			return Convert.ToInt32(o, CultureInfo.InvariantCulture);
		}

		// Token: 0x06002D78 RID: 11640 RVA: 0x000EEE4C File Offset: 0x000ED04C
		public static List<AchievementProgressData> ParseAll(string raw)
		{
			if (raw.IsNullOrEmpty())
			{
				return new List<AchievementProgressData>(0);
			}
			List<object> list = Json.Deserialize(raw) as List<object>;
			if (list == null)
			{
				Debug.LogError("[Achievements] parse progresses error");
				return new List<AchievementProgressData>(0);
			}
			return (from d in list.OfType<Dictionary<string, object>>()
			select AchievementProgressData.Create(d)).ToList<AchievementProgressData>();
		}

		// Token: 0x06002D79 RID: 11641 RVA: 0x000EEEC0 File Offset: 0x000ED0C0
		public Dictionary<string, object> ObjectForSave()
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>
			{
				{
					"id",
					this.AchievementId
				},
				{
					"p",
					this.Points
				},
				{
					"s",
					this.Stage
				}
			};
			if (this.CustomData != null && this.CustomData.Any<KeyValuePair<string, object>>())
			{
				dictionary.Add("cd", this.CustomData);
			}
			return dictionary;
		}

		// Token: 0x06002D7A RID: 11642 RVA: 0x000EEF44 File Offset: 0x000ED144
		public static string Merge(string rawListOne, string rawListTwo)
		{
			List<AchievementProgressData> progressData = AchievementProgressData.ParseAll(rawListOne);
			List<AchievementProgressData> progressData2 = AchievementProgressData.ParseAll(rawListTwo);
			AchievementProgressSyncObject achievementProgressSyncObject = AchievementProgressData.Merge(new AchievementProgressSyncObject(progressData), new AchievementProgressSyncObject(progressData2));
			List<Dictionary<string, object>> obj = (from o in achievementProgressSyncObject.ProgressData
			select o.ObjectForSave()).ToList<Dictionary<string, object>>();
			return Json.Serialize(obj);
		}

		// Token: 0x06002D7B RID: 11643 RVA: 0x000EEFAC File Offset: 0x000ED1AC
		public static AchievementProgressSyncObject Merge(AchievementProgressSyncObject one, AchievementProgressSyncObject two)
		{
			AchievementProgressSyncObject res = new AchievementProgressSyncObject();
			AchievementProgressData item;
			foreach (AchievementProgressData item2 in one.ProgressData)
			{
				item = item2;
				AchievementProgressData achievementProgressData = two.ProgressData.FirstOrDefault((AchievementProgressData i) => i.AchievementId == item.AchievementId);
				if (achievementProgressData != null)
				{
					res.ProgressData.Add(AchievementProgressData.Merge(item, achievementProgressData));
				}
				else
				{
					res.ProgressData.Add(item);
				}
			}
			IEnumerable<AchievementProgressData> collection = from i in two.ProgressData
			where res.ProgressData.All((AchievementProgressData ei) => ei.AchievementId != i.AchievementId)
			select i;
			res.ProgressData.AddRange(collection);
			Achievement.LogMsg("[MERGE] [ONE] " + AchievementProgressSyncObject.ToJson(one));
			Achievement.LogMsg("[MERGE] [TWO] " + AchievementProgressSyncObject.ToJson(two));
			Achievement.LogMsg("[MERGE] [RES] " + AchievementProgressSyncObject.ToJson(res));
			return res;
		}

		// Token: 0x06002D7C RID: 11644 RVA: 0x000EF0F4 File Offset: 0x000ED2F4
		public static AchievementProgressData Merge(AchievementProgressData one, AchievementProgressData two)
		{
			if (one == null)
			{
				return two;
			}
			if (two == null)
			{
				return one;
			}
			AchievementProgressData achievementProgressData = null;
			if (one.Stage != two.Stage)
			{
				achievementProgressData = ((one.Stage <= two.Stage) ? two : one);
			}
			else if (one.Points != two.Points)
			{
				achievementProgressData = ((one.Points <= two.Points) ? two : one);
			}
			else
			{
				achievementProgressData = one;
			}
			if (one.CustomData == null || two.CustomData == null)
			{
				achievementProgressData.CustomData = ((one.CustomData == null) ? two.CustomData : one.CustomData);
			}
			else
			{
				List<KeyValuePair<string, object>> all = new List<KeyValuePair<string, object>>();
				one.CustomData.ForEach(delegate(KeyValuePair<string, object> kvp)
				{
					all.Add(kvp);
				});
				two.CustomData.ForEach(delegate(KeyValuePair<string, object> kvp)
				{
					all.Add(kvp);
				});
				Dictionary<string, object> dictionary = new Dictionary<string, object>();
				if (all.Any<KeyValuePair<string, object>>())
				{
					foreach (KeyValuePair<string, object> keyValuePair in all)
					{
						string key = keyValuePair.Key;
						if (!dictionary.Keys.Contains(key))
						{
							KeyValuePair<string, object>[] array = (from k in all
							where k.Key == key
							select k).ToArray<KeyValuePair<string, object>>();
							if (array.Count<KeyValuePair<string, object>>() < 2)
							{
								dictionary.Add(array[0].Key, array[0].Value);
							}
							else
							{
								object value = array[0].Value;
								object value2 = array[1].Value;
								if (value == null || value2 == null)
								{
									dictionary.Add(key, value ?? value2);
								}
								else
								{
									Type type = value.GetType();
									Type type2 = value2.GetType();
									if (type != type2)
									{
										dictionary.Add(key, value);
									}
									else if (value is double)
									{
										dictionary.Add(key, ((double)value <= (double)value2) ? value2 : value);
									}
									else if (value is long)
									{
										dictionary.Add(key, ((long)value <= (long)value2) ? value2 : value);
									}
									else if (value is int)
									{
										dictionary.Add(key, ((int)value <= (int)value2) ? value2 : value);
									}
									else if (value is string)
									{
										dictionary.Add(key, (value.ToString().Length <= value2.ToString().Length) ? value2 : value);
									}
									else if (value is bool)
									{
										dictionary.Add(key, (bool)value || (bool)value2);
									}
									else
									{
										dictionary.Add(key, value);
									}
								}
							}
						}
					}
					achievementProgressData.CustomData = dictionary;
				}
			}
			return achievementProgressData;
		}

		// Token: 0x04002201 RID: 8705
		public int AchievementId;

		// Token: 0x04002202 RID: 8706
		public int Points;

		// Token: 0x04002203 RID: 8707
		public int Stage;

		// Token: 0x04002204 RID: 8708
		public Dictionary<string, object> CustomData;
	}
}
