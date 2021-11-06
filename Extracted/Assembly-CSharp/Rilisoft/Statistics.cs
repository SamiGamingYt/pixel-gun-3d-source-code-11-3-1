using System;
using System.Collections.Generic;
using Rilisoft.MiniJson;
using UnityEngine;

namespace Rilisoft
{
	// Token: 0x02000766 RID: 1894
	internal sealed class Statistics
	{
		// Token: 0x06004285 RID: 17029 RVA: 0x00161670 File Offset: 0x0015F870
		public Statistics()
		{
			this._weaponToPopularity = this.LoadPopularityFromPlayerPrefs("Statistics.WeaponPopularity");
			this._weaponToPopularityForTier = this.LoadPopularityForTierFromPlayerPrefs("Statistics.WeaponPopularityForTier");
			this._armorToPopularity = this.LoadPopularityFromPlayerPrefs("Statistics.ArmorPopularity");
			this._armorToPopularityForTier = this.LoadPopularityForTierFromPlayerPrefs("Statistics.ArmorPopularityForTier");
			this._armorToPopularityForLevel = this.LoadPopularityForLevelFromPlayerPrefs("Statistics.ArmorPopularityForLevel");
		}

		// Token: 0x17000AF6 RID: 2806
		// (get) Token: 0x06004286 RID: 17030 RVA: 0x001616D8 File Offset: 0x0015F8D8
		public static Statistics Instance
		{
			get
			{
				if (Statistics._instance == null)
				{
					Statistics._instance = new Statistics();
				}
				return Statistics._instance;
			}
		}

		// Token: 0x06004287 RID: 17031 RVA: 0x001616F4 File Offset: 0x0015F8F4
		private Dictionary<string, int> LoadPopularityFromPlayerPrefs(string playerPrefsKey)
		{
			Dictionary<string, int> dictionary = new Dictionary<string, int>();
			string @string = PlayerPrefs.GetString(playerPrefsKey, "{}");
			Dictionary<string, object> dictionary2 = Json.Deserialize(@string) as Dictionary<string, object>;
			if (dictionary2 == null)
			{
				return dictionary;
			}
			foreach (KeyValuePair<string, object> keyValuePair in dictionary2)
			{
				dictionary.Add(keyValuePair.Key, Convert.ToInt32(keyValuePair.Value));
			}
			return dictionary;
		}

		// Token: 0x06004288 RID: 17032 RVA: 0x00161790 File Offset: 0x0015F990
		private List<Dictionary<string, int>> LoadPopularityForTierFromPlayerPrefs(string playerPrefsKey)
		{
			List<Dictionary<string, int>> list = new List<Dictionary<string, int>>();
			for (int i = 0; i < ExpController.LevelsForTiers.Length; i++)
			{
				list.Add(new Dictionary<string, int>());
			}
			string @string = PlayerPrefs.GetString(playerPrefsKey, "{}");
			List<object> list2 = Json.Deserialize(@string) as List<object>;
			if (list2 == null)
			{
				return list;
			}
			for (int j = 0; j < ExpController.LevelsForTiers.Length; j++)
			{
				if (j < list2.Count)
				{
					Dictionary<string, object> dictionary = list2[j] as Dictionary<string, object>;
					foreach (KeyValuePair<string, object> keyValuePair in dictionary)
					{
						list[j].Add(keyValuePair.Key, Convert.ToInt32(keyValuePair.Value));
					}
				}
			}
			return list;
		}

		// Token: 0x06004289 RID: 17033 RVA: 0x00161890 File Offset: 0x0015FA90
		private Dictionary<int, Dictionary<string, int>> LoadPopularityForLevelFromPlayerPrefs(string playerPrefsKey)
		{
			Dictionary<int, Dictionary<string, int>> dictionary = new Dictionary<int, Dictionary<string, int>>();
			string @string = PlayerPrefs.GetString(playerPrefsKey, "{}");
			Dictionary<string, object> dictionary2 = Json.Deserialize(@string) as Dictionary<string, object>;
			if (dictionary2 == null)
			{
				return dictionary;
			}
			foreach (KeyValuePair<string, object> keyValuePair in dictionary2)
			{
				int key = Convert.ToInt32(keyValuePair.Key);
				Dictionary<string, int> dictionary3;
				if (!dictionary.TryGetValue(key, out dictionary3))
				{
					dictionary3 = new Dictionary<string, int>();
					dictionary.Add(key, dictionary3);
				}
				Dictionary<string, object> dictionary4 = keyValuePair.Value as Dictionary<string, object>;
				foreach (KeyValuePair<string, object> keyValuePair2 in dictionary4)
				{
					dictionary3.Add(keyValuePair2.Key, Convert.ToInt32(keyValuePair2.Value));
				}
			}
			return dictionary;
		}

		// Token: 0x0600428A RID: 17034 RVA: 0x001619B8 File Offset: 0x0015FBB8
		public string[] GetMostPopularWeapons()
		{
			return this.GetMostPopularFrom(this._weaponToPopularity);
		}

		// Token: 0x0600428B RID: 17035 RVA: 0x001619C8 File Offset: 0x0015FBC8
		public string[] GetMostPopularWeaponsForTier(int tier)
		{
			return this.GetMostPopularFrom(this._weaponToPopularityForTier[tier]);
		}

		// Token: 0x0600428C RID: 17036 RVA: 0x001619DC File Offset: 0x0015FBDC
		public string[] GetMostPopularArmors()
		{
			return this.GetMostPopularFrom(this._armorToPopularity);
		}

		// Token: 0x0600428D RID: 17037 RVA: 0x001619EC File Offset: 0x0015FBEC
		public string[] GetMostPopularArmorsForTier(int tier)
		{
			return this.GetMostPopularFrom(this._armorToPopularityForTier[tier]);
		}

		// Token: 0x0600428E RID: 17038 RVA: 0x00161A00 File Offset: 0x0015FC00
		public string[] GetMostPopularArmorsForLevel(int level)
		{
			Dictionary<string, int> popularityMap;
			if (this._armorToPopularityForLevel.TryGetValue(level, out popularityMap))
			{
				return this.GetMostPopularFrom(popularityMap);
			}
			return new string[0];
		}

		// Token: 0x0600428F RID: 17039 RVA: 0x00161A30 File Offset: 0x0015FC30
		private string[] GetMostPopularFrom(Dictionary<string, int> popularityMap)
		{
			int num = 0;
			foreach (KeyValuePair<string, int> keyValuePair in popularityMap)
			{
				if (keyValuePair.Value > num)
				{
					num = keyValuePair.Value;
				}
			}
			if (num == 0)
			{
				return new string[0];
			}
			List<string> list = new List<string>();
			foreach (KeyValuePair<string, int> keyValuePair2 in popularityMap)
			{
				if (keyValuePair2.Value == num)
				{
					list.Add(keyValuePair2.Key);
				}
			}
			return list.ToArray();
		}

		// Token: 0x06004290 RID: 17040 RVA: 0x00161B20 File Offset: 0x0015FD20
		public void IncrementWeaponPopularity(string key, bool save = true)
		{
			this.IncrementPopularity(this._weaponToPopularity, key);
			int ourTier = ExpController.Instance.OurTier;
			Dictionary<string, int> popularityDict = this._weaponToPopularityForTier[ourTier];
			this.IncrementPopularity(popularityDict, key);
			if (save)
			{
				this.SaveWeaponPopularity();
			}
		}

		// Token: 0x06004291 RID: 17041 RVA: 0x00161B68 File Offset: 0x0015FD68
		public void IncrementArmorPopularity(string key, bool save = true)
		{
			this.IncrementPopularity(this._armorToPopularity, key);
			int ourTier = ExpController.Instance.OurTier;
			Dictionary<string, int> popularityDict = this._armorToPopularityForTier[ourTier];
			this.IncrementPopularity(popularityDict, key);
			int currentLevel = ExperienceController.sharedController.currentLevel;
			Dictionary<string, int> dictionary;
			if (!this._armorToPopularityForLevel.TryGetValue(currentLevel, out dictionary))
			{
				dictionary = new Dictionary<string, int>();
				this._armorToPopularityForLevel.Add(currentLevel, dictionary);
			}
			this.IncrementPopularity(dictionary, key);
			if (save)
			{
				this.SaveArmorPopularity();
			}
		}

		// Token: 0x06004292 RID: 17042 RVA: 0x00161BE8 File Offset: 0x0015FDE8
		private void IncrementPopularity(Dictionary<string, int> popularityDict, string key)
		{
			int num;
			bool flag = popularityDict.TryGetValue(key, out num);
			if (flag)
			{
				popularityDict[key] = num + 1;
			}
			else
			{
				popularityDict.Add(key, 1);
			}
		}

		// Token: 0x06004293 RID: 17043 RVA: 0x00161C1C File Offset: 0x0015FE1C
		public void SaveWeaponPopularity()
		{
			this.SavePopularityInfo("Statistics.WeaponPopularity", this._weaponToPopularity);
			this.SavePopularityInfo("Statistics.WeaponPopularityForTier", this._weaponToPopularityForTier);
			PlayerPrefs.Save();
		}

		// Token: 0x06004294 RID: 17044 RVA: 0x00161C48 File Offset: 0x0015FE48
		public void SaveArmorPopularity()
		{
			this.SavePopularityInfo("Statistics.ArmorPopularity", this._armorToPopularity);
			this.SavePopularityInfo("Statistics.ArmorPopularityForLevel", this._armorToPopularityForLevel);
			this.SavePopularityInfo("Statistics.ArmorPopularityForTier", this._armorToPopularityForTier);
			PlayerPrefs.Save();
		}

		// Token: 0x06004295 RID: 17045 RVA: 0x00161C90 File Offset: 0x0015FE90
		private void SavePopularityInfo(string playerPrefsKey, object popularityInfo)
		{
			string text = Json.Serialize(popularityInfo);
			if (Debug.isDebugBuild)
			{
				Debug.Log(string.Format("Saving: playerPrefsKey: {0}, popularityInfo: {1}", playerPrefsKey, text));
			}
			PlayerPrefs.SetString(playerPrefsKey, text);
		}

		// Token: 0x040030AA RID: 12458
		private readonly Dictionary<string, int> _weaponToPopularity;

		// Token: 0x040030AB RID: 12459
		private readonly List<Dictionary<string, int>> _weaponToPopularityForTier;

		// Token: 0x040030AC RID: 12460
		private readonly Dictionary<string, int> _armorToPopularity;

		// Token: 0x040030AD RID: 12461
		private readonly List<Dictionary<string, int>> _armorToPopularityForTier;

		// Token: 0x040030AE RID: 12462
		private readonly Dictionary<int, Dictionary<string, int>> _armorToPopularityForLevel;

		// Token: 0x040030AF RID: 12463
		private static Statistics _instance;
	}
}
