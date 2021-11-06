using System;
using System.Collections.Generic;
using System.Linq;

namespace Rilisoft
{
	// Token: 0x02000711 RID: 1809
	internal sealed class DictionaryLoadedListener
	{
		// Token: 0x06003F2A RID: 16170 RVA: 0x00152458 File Offset: 0x00150658
		internal static string MergeProgress(string localDataString, string serverDataString)
		{
			Dictionary<string, Dictionary<string, int>> dictionary = CampaignProgress.DeserializeProgress(localDataString);
			if (dictionary == null)
			{
				dictionary = new Dictionary<string, Dictionary<string, int>>();
			}
			Dictionary<string, Dictionary<string, int>> dictionary2 = CampaignProgress.DeserializeProgress(serverDataString);
			if (dictionary2 == null)
			{
				dictionary2 = new Dictionary<string, Dictionary<string, int>>();
			}
			Dictionary<string, Dictionary<string, int>> dictionary3 = new Dictionary<string, Dictionary<string, int>>();
			foreach (string key in dictionary.Keys.Concat(dictionary2.Keys).Distinct<string>())
			{
				dictionary3.Add(key, new Dictionary<string, int>());
			}
			foreach (KeyValuePair<string, Dictionary<string, int>> keyValuePair in dictionary3)
			{
				Dictionary<string, int> dictionary4;
				if (dictionary.TryGetValue(keyValuePair.Key, out dictionary4))
				{
					foreach (KeyValuePair<string, int> keyValuePair2 in dictionary4)
					{
						keyValuePair.Value.Add(keyValuePair2.Key, keyValuePair2.Value);
					}
				}
				Dictionary<string, int> dictionary5;
				if (dictionary2.TryGetValue(keyValuePair.Key, out dictionary5))
				{
					foreach (KeyValuePair<string, int> keyValuePair3 in dictionary5)
					{
						int val;
						if (keyValuePair.Value.TryGetValue(keyValuePair3.Key, out val))
						{
							keyValuePair.Value[keyValuePair3.Key] = Math.Max(val, keyValuePair3.Value);
						}
						else
						{
							keyValuePair.Value.Add(keyValuePair3.Key, keyValuePair3.Value);
						}
					}
				}
			}
			return CampaignProgress.SerializeProgress(dictionary3);
		}
	}
}
