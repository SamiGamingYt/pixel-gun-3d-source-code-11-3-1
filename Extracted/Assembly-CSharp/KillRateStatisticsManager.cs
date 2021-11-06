using System;
using System.Collections.Generic;
using Rilisoft.MiniJson;
using UnityEngine;

// Token: 0x020002E0 RID: 736
public class KillRateStatisticsManager
{
	// Token: 0x170004A8 RID: 1192
	// (get) Token: 0x060019C0 RID: 6592 RVA: 0x000679D0 File Offset: 0x00065BD0
	public static Dictionary<string, Dictionary<int, int>> WeKillOld
	{
		get
		{
			if (!KillRateStatisticsManager._initialized)
			{
				KillRateStatisticsManager.Initialize();
			}
			return KillRateStatisticsManager.weKillOld;
		}
	}

	// Token: 0x170004A9 RID: 1193
	// (get) Token: 0x060019C1 RID: 6593 RVA: 0x000679E8 File Offset: 0x00065BE8
	public static Dictionary<string, Dictionary<int, int>> WeWereKilledOld
	{
		get
		{
			if (!KillRateStatisticsManager._initialized)
			{
				KillRateStatisticsManager.Initialize();
			}
			return KillRateStatisticsManager.weWereKilledOld;
		}
	}

	// Token: 0x060019C2 RID: 6594 RVA: 0x00067A00 File Offset: 0x00065C00
	private static void Initialize()
	{
		KillRateStatisticsManager.ParseKillRate(ref KillRateStatisticsManager.weKillOld, ref KillRateStatisticsManager.weWereKilledOld);
		KillRateStatisticsManager._initialized = true;
	}

	// Token: 0x060019C3 RID: 6595 RVA: 0x00067A18 File Offset: 0x00065C18
	private static void ParseKillRate(ref Dictionary<string, Dictionary<int, int>> returnWeKill, ref Dictionary<string, Dictionary<int, int>> returnWeWereKilled)
	{
		KillRateStatisticsManager.InitializeKillRateKey();
		Dictionary<string, object> dictionary = Json.Deserialize(Storager.getString("KillRateKeyStatistics", false)) as Dictionary<string, object>;
		if (dictionary.ContainsKey("version"))
		{
			if (!((string)dictionary["version"]).Equals(GlobalGameController.AppVersion))
			{
				KillRateStatisticsManager.WriteDefaultJson();
				dictionary = (Json.Deserialize(Storager.getString("KillRateKeyStatistics", false)) as Dictionary<string, object>);
			}
		}
		else
		{
			Debug.LogError("ParseKillRate: no version key. Please clear your PlayerPrefs");
			KillRateStatisticsManager.WriteDefaultJson();
			dictionary = (Json.Deserialize(Storager.getString("KillRateKeyStatistics", false)) as Dictionary<string, object>);
		}
		Dictionary<string, object> arg = (!dictionary.ContainsKey("wekill")) ? new Dictionary<string, object>() : (dictionary["wekill"] as Dictionary<string, object>);
		Dictionary<string, object> arg2 = (!dictionary.ContainsKey("wewerekilled")) ? new Dictionary<string, object>() : (dictionary["wewerekilled"] as Dictionary<string, object>);
		Action<Dictionary<string, object>, Dictionary<string, Dictionary<int, int>>> action = delegate(Dictionary<string, object> savedDict, Dictionary<string, Dictionary<int, int>> dict)
		{
			foreach (KeyValuePair<string, object> keyValuePair in savedDict)
			{
				Dictionary<string, object> dictionary2 = keyValuePair.Value as Dictionary<string, object>;
				Dictionary<int, int> dictionary3 = new Dictionary<int, int>();
				foreach (KeyValuePair<string, object> keyValuePair2 in dictionary2)
				{
					dictionary3.Add(int.Parse(keyValuePair2.Key), (int)((long)keyValuePair2.Value));
				}
				dict.Add(keyValuePair.Key, dictionary3);
			}
		};
		action(arg, returnWeKill);
		action(arg2, returnWeWereKilled);
	}

	// Token: 0x060019C4 RID: 6596 RVA: 0x00067B3C File Offset: 0x00065D3C
	private static void WriteDefaultJson()
	{
		Storager.setString("KillRateKeyStatistics", Json.Serialize(new Dictionary<string, object>
		{
			{
				"version",
				GlobalGameController.AppVersion
			}
		}), false);
	}

	// Token: 0x060019C5 RID: 6597 RVA: 0x00067B70 File Offset: 0x00065D70
	private static void InitializeKillRateKey()
	{
		if (!Storager.hasKey("KillRateKeyStatistics"))
		{
			KillRateStatisticsManager.WriteDefaultJson();
		}
	}

	// Token: 0x04000EFF RID: 3839
	public const string WeKillKey = "wekill";

	// Token: 0x04000F00 RID: 3840
	public const string WeWereKilledKey = "wewerekilled";

	// Token: 0x04000F01 RID: 3841
	private static Dictionary<string, Dictionary<int, int>> weKillOld = new Dictionary<string, Dictionary<int, int>>();

	// Token: 0x04000F02 RID: 3842
	private static Dictionary<string, Dictionary<int, int>> weWereKilledOld = new Dictionary<string, Dictionary<int, int>>();

	// Token: 0x04000F03 RID: 3843
	private static bool _initialized = false;
}
