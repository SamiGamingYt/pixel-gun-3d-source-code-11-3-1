using System;
using System.Collections.Generic;
using System.Linq;
using Rilisoft;
using Rilisoft.MiniJson;
using UnityEngine;

// Token: 0x020005B3 RID: 1459
public sealed class CampaignProgress
{
	// Token: 0x06003273 RID: 12915 RVA: 0x00105AE4 File Offset: 0x00103CE4
	static CampaignProgress()
	{
		Storager.hasKey("CampaignProgress");
		CampaignProgress.LoadCampaignProgress();
		if (CampaignProgress.boxesLevelsAndStars.Keys.Count == 0)
		{
			CampaignProgress.boxesLevelsAndStars.Add(LevelBox.campaignBoxes[0].name, new Dictionary<string, int>());
			CampaignProgress.SaveCampaignProgress();
		}
	}

	// Token: 0x06003274 RID: 12916 RVA: 0x00105B44 File Offset: 0x00103D44
	public static void OpenNewBoxIfPossible()
	{
		int num = 0;
		for (int i = 1; i < LevelBox.campaignBoxes.Count; i++)
		{
			LevelBox levelBox = LevelBox.campaignBoxes[i];
			if (CampaignProgress.boxesLevelsAndStars.ContainsKey(levelBox.name))
			{
				num = i;
			}
		}
		int num2 = 0;
		foreach (KeyValuePair<string, Dictionary<string, int>> keyValuePair in CampaignProgress.boxesLevelsAndStars)
		{
			foreach (KeyValuePair<string, int> keyValuePair2 in keyValuePair.Value)
			{
				num2 += keyValuePair2.Value;
			}
		}
		int num3 = num + 1;
		if (num3 < LevelBox.campaignBoxes.Count)
		{
			string name = LevelBox.campaignBoxes[num3].name;
			if (LevelBox.campaignBoxes[num3].starsToOpen <= num2 && !CampaignProgress.boxesLevelsAndStars.ContainsKey(name))
			{
				CampaignProgress.boxesLevelsAndStars.Add(name, new Dictionary<string, int>());
				CampaignProgress.SaveCampaignProgress();
			}
		}
		CampaignProgress.SaveCampaignProgress();
	}

	// Token: 0x06003275 RID: 12917 RVA: 0x00105CB0 File Offset: 0x00103EB0
	public static bool FirstTimeCompletesLevel(string lev)
	{
		return !CampaignProgress.boxesLevelsAndStars[CurrentCampaignGame.boXName].ContainsKey(lev);
	}

	// Token: 0x06003276 RID: 12918 RVA: 0x00105CCC File Offset: 0x00103ECC
	public static void SaveCampaignProgress()
	{
		CampaignProgress.SetProgressDictionary("CampaignProgress", CampaignProgress.boxesLevelsAndStars, false);
	}

	// Token: 0x06003277 RID: 12919 RVA: 0x00105CE0 File Offset: 0x00103EE0
	public static string GetCampaignProgressString()
	{
		return Storager.getString("CampaignProgress", false);
	}

	// Token: 0x06003278 RID: 12920 RVA: 0x00105CF0 File Offset: 0x00103EF0
	public static void LoadCampaignProgress()
	{
		Dictionary<string, Dictionary<string, int>> progressDictionary = CampaignProgress.GetProgressDictionary("CampaignProgress", false);
		CampaignProgress.boxesLevelsAndStars = progressDictionary;
	}

	// Token: 0x06003279 RID: 12921 RVA: 0x00105D10 File Offset: 0x00103F10
	internal static string SerializeProgress(Dictionary<string, Dictionary<string, int>> progress)
	{
		string result;
		try
		{
			string text = Json.Serialize(progress);
			result = text;
		}
		catch (Exception message)
		{
			Debug.LogError(message);
			result = "{ }";
		}
		return result;
	}

	// Token: 0x0600327A RID: 12922 RVA: 0x00105D64 File Offset: 0x00103F64
	internal static Dictionary<string, Dictionary<string, int>> DeserializeProgress(string serializedProgress)
	{
		Dictionary<string, Dictionary<string, int>> dictionary = new Dictionary<string, Dictionary<string, int>>();
		object obj = Json.Deserialize(serializedProgress);
		if (Debug.isDebugBuild && obj != null && BuildSettings.BuildTargetPlatform == RuntimePlatform.MetroPlayerX64)
		{
			Type type = obj.GetType();
			Debug.Log("Deserialized progress type: " + type);
			Debug.Log("##### Serialized campaign progress:\n" + serializedProgress);
		}
		Dictionary<string, object> dictionary2 = obj as Dictionary<string, object>;
		if (dictionary2 != null)
		{
			foreach (KeyValuePair<string, object> keyValuePair in dictionary2)
			{
				Dictionary<string, int> dictionary3 = new Dictionary<string, int>();
				IDictionary<string, object> dictionary4 = keyValuePair.Value as IDictionary<string, object>;
				if (dictionary4 != null)
				{
					foreach (KeyValuePair<string, object> keyValuePair2 in dictionary4)
					{
						try
						{
							int value = Convert.ToInt32(keyValuePair2.Value);
							dictionary3.Add(keyValuePair2.Key, value);
						}
						catch (InvalidCastException)
						{
							Debug.LogWarning("Cannot convert " + keyValuePair2.Value + " to int.");
						}
					}
				}
				else if (Debug.isDebugBuild)
				{
					Debug.LogWarning("boxProgressDictionary == null");
				}
				dictionary.Add(keyValuePair.Key, dictionary3);
			}
		}
		else if (Debug.isDebugBuild)
		{
			Debug.LogWarning("campaignProgressDictionary == null,    serializedProgress == " + serializedProgress);
		}
		return dictionary;
	}

	// Token: 0x0600327B RID: 12923 RVA: 0x00105F2C File Offset: 0x0010412C
	private static void SetProgressDictionary(string key, Dictionary<string, Dictionary<string, int>> dictionary, bool useCloud)
	{
		string val = CampaignProgress.SerializeProgress(dictionary);
		Storager.setString(key, val, useCloud);
		if (Application.platform != RuntimePlatform.IPhonePlayer)
		{
			PlayerPrefs.Save();
		}
	}

	// Token: 0x0600327C RID: 12924 RVA: 0x00105F58 File Offset: 0x00104158
	private static Dictionary<string, Dictionary<string, int>> GetProgressDictionary(string key, bool useCloud)
	{
		string @string = Storager.getString(key, useCloud);
		return CampaignProgress.DeserializeProgress(@string);
	}

	// Token: 0x0600327D RID: 12925 RVA: 0x00105F78 File Offset: 0x00104178
	internal static Dictionary<string, Dictionary<string, int>> DeserializeTestDictionary()
	{
		string serializedProgress = "{\"Box_11\": { \"Level_02\": 1, \"Level_05\": 3 },\"Box_13\": { \"Level_03\": 1, \"Level_08\": 3, \"Level_21\": 2 },\"Box_34\": { },\"Box_99\": { \"Level_55\": 2 },}";
		return CampaignProgress.DeserializeProgress(serializedProgress);
	}

	// Token: 0x0600327E RID: 12926 RVA: 0x00105F94 File Offset: 0x00104194
	public static void ActualizeComicsViews()
	{
		try
		{
			if (CampaignProgress.boxesLevelsAndStars == null)
			{
				Debug.LogError("ActualizeComicsViews: boxesLevelsAndStars = null");
			}
			else
			{
				IEnumerable<string> source = CampaignProgress.boxesLevelsAndStars.SelectMany((KeyValuePair<string, Dictionary<string, int>> boxKvp) => boxKvp.Value.Keys).Concat(Load.LoadStringArray(Defs.ArtLevsS) ?? new string[0]).Distinct<string>();
				Save.SaveStringArray(Defs.ArtLevsS, source.ToArray<string>());
				IEnumerable<string> source2 = (from boxName in CampaignProgress.boxesLevelsAndStars.Keys
				orderby LevelBox.campaignBoxes.FindIndex((LevelBox levelBox) => levelBox.name == boxName)
				select boxName).WithoutLast<string>().Concat(Load.LoadStringArray(Defs.ArtBoxS) ?? new string[0]).Distinct<string>();
				Save.SaveStringArray(Defs.ArtBoxS, source2.ToArray<string>());
			}
		}
		catch (Exception arg)
		{
			Debug.LogError("ActualizeComicsViews: exception: " + arg);
		}
	}

	// Token: 0x04002516 RID: 9494
	public const string CampaignProgressKey = "CampaignProgress";

	// Token: 0x04002517 RID: 9495
	public static Dictionary<string, Dictionary<string, int>> boxesLevelsAndStars = new Dictionary<string, Dictionary<string, int>>();
}
