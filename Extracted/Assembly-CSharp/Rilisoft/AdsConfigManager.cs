using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Rilisoft
{
	// Token: 0x0200052D RID: 1325
	internal sealed class AdsConfigManager
	{
		// Token: 0x06002E20 RID: 11808 RVA: 0x000F1870 File Offset: 0x000EFA70
		private AdsConfigManager()
		{
		}

		// Token: 0x1400003E RID: 62
		// (add) Token: 0x06002E22 RID: 11810 RVA: 0x000F18AC File Offset: 0x000EFAAC
		// (remove) Token: 0x06002E23 RID: 11811 RVA: 0x000F18C8 File Offset: 0x000EFAC8
		public event EventHandler ConfigLoaded;

		// Token: 0x170007EA RID: 2026
		// (get) Token: 0x06002E24 RID: 11812 RVA: 0x000F18E4 File Offset: 0x000EFAE4
		public static AdsConfigManager Instance
		{
			get
			{
				return AdsConfigManager.s_instance.Value;
			}
		}

		// Token: 0x170007EB RID: 2027
		// (get) Token: 0x06002E25 RID: 11813 RVA: 0x000F18F0 File Offset: 0x000EFAF0
		public AdsConfigMemento LastLoadedConfig
		{
			get
			{
				return this._lastLoadedConfig;
			}
		}

		// Token: 0x06002E26 RID: 11814 RVA: 0x000F18F8 File Offset: 0x000EFAF8
		internal IEnumerator GetAdvertInfoLoop(Task futureToWait)
		{
			if (futureToWait != null)
			{
				yield return new WaitUntil(() => futureToWait.IsCompleted);
			}
			while (!TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage <= TrainingController.NewTrainingCompletedStage.None)
			{
				yield return null;
			}
			while (!FriendsController.isReadABTestAdvertConfig)
			{
				yield return null;
			}
			for (;;)
			{
				float advertGetInfoStartTime = Time.realtimeSinceStartup;
				yield return CoroutineRunner.Instance.StartCoroutine(this.GetAdvertInfoOnce());
				yield return new WaitWhile(() => Time.realtimeSinceStartup - advertGetInfoStartTime < 960f);
			}
			yield break;
		}

		// Token: 0x06002E27 RID: 11815 RVA: 0x000F1924 File Offset: 0x000EFB24
		private IEnumerator GetAdvertInfoOnce()
		{
			string url = URLs.NewAdvertisingConfig;
			string responseText;
			if (!string.IsNullOrEmpty(AdsConfigManager.configFromABTestAdvert))
			{
				responseText = AdsConfigManager.configFromABTestAdvert;
			}
			else
			{
				string cachedResponse = PersistentCacheManager.Instance.GetValue(url);
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
							Debug.LogWarningFormat("Advert response error: {0}", new object[]
							{
								(response == null) ? "null" : response.error
							});
							yield break;
						}
						responseText = URLs.Sanitize(response);
						if (string.IsNullOrEmpty(responseText))
						{
							Debug.LogWarning("Advert response is empty");
							yield break;
						}
						PersistentCacheManager.Instance.SetValue(response.url, responseText);
					}
					finally
					{
						if (Application.isEditor)
						{
							Debug.Log("<color=teal>AdsConfigManager.GetAdvertInfoOnce(): response.Dispose()</color>");
						}
						response.Dispose();
					}
				}
			}
			this._lastLoadedConfig = AdsConfigMemento.FromJson(responseText);
			EventHandler handler = this.ConfigLoaded;
			if (handler != null)
			{
				handler(this, EventArgs.Empty);
			}
			if (this._lastLoadedConfig.Exception != null)
			{
				Debug.LogWarning(this._lastLoadedConfig.Exception);
			}
			yield break;
		}

		// Token: 0x06002E28 RID: 11816 RVA: 0x000F1940 File Offset: 0x000EFB40
		internal static CheatingMethods GetCheatingMethods(AdsConfigMemento config)
		{
			if (config == null)
			{
				throw new ArgumentNullException("config");
			}
			if (config.Exception != null)
			{
				return CheatingMethods.None;
			}
			if (config.CheaterConfig.CheckSignatureTampering && (Switcher.AbuseMethod & AbuseMetod.AndroidPackageSignature) != AbuseMetod.None)
			{
				return CheatingMethods.SignatureTampering;
			}
			int @int = Storager.getInt("Coins", false);
			if (@int >= config.CheaterConfig.CoinThreshold)
			{
				return CheatingMethods.CoinThreshold;
			}
			int int2 = Storager.getInt("GemsCurrency", false);
			if (int2 >= config.CheaterConfig.GemThreshold)
			{
				return CheatingMethods.GemThreshold;
			}
			return CheatingMethods.None;
		}

		// Token: 0x06002E29 RID: 11817 RVA: 0x000F19CC File Offset: 0x000EFBCC
		internal static string GetPlayerCategory(AdsConfigMemento config)
		{
			if (config == null)
			{
				throw new ArgumentNullException("config");
			}
			if (config.Exception != null)
			{
				return string.Empty;
			}
			if (AdsConfigManager.GetCheatingMethods(config) != CheatingMethods.None)
			{
				return "cheater";
			}
			bool flag = StoreKitEventListener.IsPayingUser();
			int @int = PlayerPrefs.GetInt(Defs.SessionDayNumberKey, 1);
			float num = (!(NotificationController.instance != null)) ? Time.realtimeSinceStartup : NotificationController.instance.currentPlayTime;
			int num2 = Mathf.FloorToInt(num / 60f);
			foreach (KeyValuePair<string, PlayerStateMemento> keyValuePair in config.PlayerStates)
			{
				PlayerStateMemento value = keyValuePair.Value;
				if (value.IsPaying == flag)
				{
					if (value.MinInGameMinutes != null && value.MaxInGameMinutes != null)
					{
						if (num2 < value.MinInGameMinutes.Value)
						{
							continue;
						}
						if (num2 > value.MaxInGameMinutes.Value)
						{
							continue;
						}
					}
					else
					{
						if (@int < value.MinDay)
						{
							continue;
						}
						if (@int > value.MaxDay)
						{
							continue;
						}
					}
					return value.Id;
				}
			}
			return string.Empty;
		}

		// Token: 0x06002E2A RID: 11818 RVA: 0x000F1B60 File Offset: 0x000EFD60
		internal static int GetInterstitialDisabledReasonCode(AdsConfigMemento config)
		{
			if (config == null)
			{
				throw new ArgumentNullException("config");
			}
			if (config.Exception != null)
			{
				return 10;
			}
			if (config.InterstitialConfig == null)
			{
				return 20;
			}
			string playerCategory = AdsConfigManager.GetPlayerCategory(config);
			string deviceModel = SystemInfo.deviceModel;
			double timeSpanSinceLastShowInMinutes = AdsConfigManager.GetTimeSpanSinceLastShowInMinutes();
			int disabledReasonCode = config.InterstitialConfig.GetDisabledReasonCode(playerCategory, deviceModel, timeSpanSinceLastShowInMinutes);
			if (disabledReasonCode != 0)
			{
				return 30 + disabledReasonCode;
			}
			return 0;
		}

		// Token: 0x06002E2B RID: 11819 RVA: 0x000F1BC8 File Offset: 0x000EFDC8
		internal static string GetInterstitialDisabledReason(AdsConfigMemento config)
		{
			if (config == null)
			{
				throw new ArgumentNullException("config");
			}
			if (config.Exception != null)
			{
				return config.Exception.Message;
			}
			if (config.InterstitialConfig == null)
			{
				return "`InterstitialConfig == null` (probably not received yet)";
			}
			string playerCategory = AdsConfigManager.GetPlayerCategory(config);
			string deviceModel = SystemInfo.deviceModel;
			double timeSpanSinceLastShowInMinutes = AdsConfigManager.GetTimeSpanSinceLastShowInMinutes();
			return config.InterstitialConfig.GetDisabledReason(playerCategory, deviceModel, timeSpanSinceLastShowInMinutes);
		}

		// Token: 0x06002E2C RID: 11820 RVA: 0x000F1C34 File Offset: 0x000EFE34
		internal static string GetVideoDisabledReason(AdsConfigMemento config)
		{
			if (config == null)
			{
				throw new ArgumentNullException("config");
			}
			if (config.Exception != null)
			{
				return config.Exception.Message;
			}
			string playerCategory = AdsConfigManager.GetPlayerCategory(config);
			string deviceModel = SystemInfo.deviceModel;
			return config.VideoConfig.GetDisabledReason(playerCategory, deviceModel);
		}

		// Token: 0x06002E2D RID: 11821 RVA: 0x000F1C88 File Offset: 0x000EFE88
		internal static double GetTimeSpanSinceLastShowInMinutes()
		{
			string @string = PlayerPrefs.GetString(Defs.LastTimeShowBanerKey, string.Empty);
			if (string.IsNullOrEmpty(@string))
			{
				return 3.4028234663852886E+38;
			}
			DateTime value;
			if (!DateTime.TryParse(@string, out value))
			{
				return 3.4028234663852886E+38;
			}
			double totalMinutes = DateTime.UtcNow.Subtract(value).TotalMinutes;
			if (totalMinutes < 0.0)
			{
				PlayerPrefs.SetString(Defs.LastTimeShowBanerKey, DateTime.UtcNow.ToString("s"));
				PlayerPrefs.Save();
				return 0.0;
			}
			return totalMinutes;
		}

		// Token: 0x0400224B RID: 8779
		private const float AdvertInfoTimeout = 960f;

		// Token: 0x0400224C RID: 8780
		public static string configFromABTestAdvert = string.Empty;

		// Token: 0x0400224D RID: 8781
		private static readonly Lazy<AdsConfigManager> s_instance = new Lazy<AdsConfigManager>(() => new AdsConfigManager());

		// Token: 0x0400224E RID: 8782
		private AdsConfigMemento _lastLoadedConfig;
	}
}
