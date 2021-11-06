using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Rilisoft.MiniJson;
using UnityEngine;

namespace Rilisoft
{
	// Token: 0x02000543 RID: 1347
	internal sealed class PerelivConfigManager
	{
		// Token: 0x06002EDF RID: 11999 RVA: 0x000F5088 File Offset: 0x000F3288
		private PerelivConfigManager()
		{
		}

		// Token: 0x17000815 RID: 2069
		// (get) Token: 0x06002EE1 RID: 12001 RVA: 0x000F50DC File Offset: 0x000F32DC
		public static PerelivConfigManager Instance
		{
			get
			{
				return PerelivConfigManager.s_instance.Value;
			}
		}

		// Token: 0x17000816 RID: 2070
		// (get) Token: 0x06002EE2 RID: 12002 RVA: 0x000F50E8 File Offset: 0x000F32E8
		public Dictionary<string, object> LastLoadedConfig
		{
			get
			{
				return this._lastLoadedConfig;
			}
		}

		// Token: 0x06002EE3 RID: 12003 RVA: 0x000F50F0 File Offset: 0x000F32F0
		internal IEnumerator GetPerelivConfigLoop(Task futureToWait)
		{
			if (futureToWait != null)
			{
				while (!futureToWait.IsCompleted)
				{
					yield return null;
				}
			}
			for (;;)
			{
				float advertGetInfoStartTime = Time.realtimeSinceStartup;
				yield return CoroutineRunner.Instance.StartCoroutine(this.GetPerelivConfigOnce());
				while (Time.realtimeSinceStartup - advertGetInfoStartTime < 960f)
				{
					yield return null;
				}
			}
			yield break;
		}

		// Token: 0x06002EE4 RID: 12004 RVA: 0x000F511C File Offset: 0x000F331C
		internal PerelivSettings GetPerelivSettings(string category)
		{
			if (this.LastLoadedConfig.Count == 0)
			{
				return new PerelivSettings("Last loaded config is empty");
			}
			if (string.IsNullOrEmpty(category))
			{
				return new PerelivSettings("Category is empty");
			}
			object obj;
			if (!this.LastLoadedConfig.TryGetValue("mainMenu", out obj))
			{
				return new PerelivSettings("Root object not found");
			}
			Dictionary<string, object> dictionary = obj as Dictionary<string, object>;
			if (obj == null)
			{
				return new PerelivSettings("Root object is not dictionary");
			}
			object obj2;
			dictionary.TryGetValue("any", out obj2);
			Dictionary<string, object> root = (obj2 as Dictionary<string, object>) ?? PerelivConfigManager.s_emptyDictionary;
			object obj3;
			dictionary.TryGetValue(category, out obj3);
			Dictionary<string, object> overrides = (obj3 as Dictionary<string, object>) ?? PerelivConfigManager.s_emptyDictionary;
			PerelivSettings result;
			try
			{
				object value = PerelivConfigManager.TryGetValue(root, overrides, "enabled");
				object obj4 = PerelivConfigManager.TryGetValue(root, overrides, "imageUrl");
				object obj5 = PerelivConfigManager.TryGetValue(root, overrides, "redirectUrl");
				object obj6 = PerelivConfigManager.TryGetValue(root, overrides, "text");
				object value2 = PerelivConfigManager.TryGetValue(root, overrides, "showAlways");
				object obj7 = PerelivConfigManager.TryGetValue(root, overrides, "minLevel");
				object obj8 = PerelivConfigManager.TryGetValue(root, overrides, "maxLevel");
				object value3 = PerelivConfigManager.TryGetValue(root, overrides, "buttonClose");
				int minLevel = (obj7 == null) ? -1 : Convert.ToInt32(obj7);
				int maxLevel = (obj8 == null) ? -1 : Convert.ToInt32(obj8);
				PerelivSettings perelivSettings = new PerelivSettings(Convert.ToBoolean(value), obj4 as string, obj5 as string, obj6 as string, Convert.ToBoolean(value2), minLevel, maxLevel, Convert.ToBoolean(value3));
				result = perelivSettings;
			}
			catch (Exception ex)
			{
				result = new PerelivSettings(ex.Message);
			}
			return result;
		}

		// Token: 0x06002EE5 RID: 12005 RVA: 0x000F52F4 File Offset: 0x000F34F4
		private static object TryGetValue(Dictionary<string, object> root, Dictionary<string, object> overrides, string key)
		{
			object result;
			if (overrides.TryGetValue(key, out result))
			{
				return result;
			}
			if (root.TryGetValue(key, out result))
			{
				return result;
			}
			return null;
		}

		// Token: 0x06002EE6 RID: 12006 RVA: 0x000F5324 File Offset: 0x000F3524
		private IEnumerator GetPerelivConfigOnce()
		{
			string url = URLs.NewPerelivConfig;
			string cachedResponse = PersistentCacheManager.Instance.GetValue(url);
			string responseText;
			if (!string.IsNullOrEmpty(cachedResponse))
			{
				PersistentCacheManager.DebugReportCacheHit(url);
				responseText = cachedResponse;
			}
			else
			{
				PersistentCacheManager.DebugReportCacheMiss(url);
				WWW response = Tools.CreateWww(url);
				yield return response;
				try
				{
					if (response == null || !string.IsNullOrEmpty(response.error))
					{
						Debug.LogWarningFormat("Pereliv response error: {0}", new object[]
						{
							(response == null) ? "null" : response.error
						});
						yield break;
					}
					responseText = URLs.Sanitize(response);
					if (string.IsNullOrEmpty(responseText))
					{
						Debug.LogWarning("Pereliv response is empty");
						yield break;
					}
					PersistentCacheManager.Instance.SetValue(response.url, responseText);
				}
				finally
				{
					response.Dispose();
				}
			}
			Dictionary<string, object> lastLoadedConfig = Json.Deserialize(responseText) as Dictionary<string, object>;
			this._lastLoadedConfig = (lastLoadedConfig ?? new Dictionary<string, object>(0));
			yield break;
		}

		// Token: 0x0400229C RID: 8860
		private const float AdvertInfoTimeoutInSeconds = 960f;

		// Token: 0x0400229D RID: 8861
		private static readonly Lazy<PerelivConfigManager> s_instance = new Lazy<PerelivConfigManager>(() => new PerelivConfigManager());

		// Token: 0x0400229E RID: 8862
		private static readonly Dictionary<string, object> s_emptyDictionary = new Dictionary<string, object>(0);

		// Token: 0x0400229F RID: 8863
		private Dictionary<string, object> _lastLoadedConfig = new Dictionary<string, object>(0);
	}
}
