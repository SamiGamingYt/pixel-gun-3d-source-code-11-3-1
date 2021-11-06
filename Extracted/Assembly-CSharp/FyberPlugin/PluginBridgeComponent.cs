using System;
using UnityEngine;

namespace FyberPlugin
{
	// Token: 0x02000131 RID: 305
	internal class PluginBridgeComponent : IPluginBridge
	{
		// Token: 0x06000988 RID: 2440 RVA: 0x00039940 File Offset: 0x00037B40
		static PluginBridgeComponent()
		{
			FyberGameObject.Init();
		}

		// Token: 0x06000989 RID: 2441 RVA: 0x00039948 File Offset: 0x00037B48
		public void StartSDK(string json)
		{
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.fyber.mediation.MediationAdapterStarter"))
			{
				FyberSettings instance = FyberSettings.Instance;
				androidJavaClass.CallStatic("setup", new object[]
				{
					instance.BundlesInfoJson(),
					instance.BundlesCount()
				});
			}
			using (AndroidJavaClass androidJavaClass2 = new AndroidJavaClass("com.fyber.mediation.MediationConfigProvider"))
			{
				FyberSettings instance2 = FyberSettings.Instance;
				androidJavaClass2.CallStatic("setup", new object[]
				{
					instance2.BundlesConfigJson()
				});
			}
			using (AndroidJavaObject androidJavaObject = new AndroidJavaObject("com.fyber.unity.FyberPlugin", new object[0]))
			{
				androidJavaObject.CallStatic("setPluginParameters", new object[]
				{
					"8.6.0",
					Application.unityVersion
				});
				androidJavaObject.CallStatic("start", new object[]
				{
					json
				});
			}
		}

		// Token: 0x0600098A RID: 2442 RVA: 0x00039A88 File Offset: 0x00037C88
		public bool Cache(string action)
		{
			bool result;
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.fyber.unity.cache.CacheWrapper"))
			{
				result = androidJavaClass.CallStatic<bool>(action, new object[0]);
			}
			return result;
		}

		// Token: 0x0600098B RID: 2443 RVA: 0x00039AE4 File Offset: 0x00037CE4
		public void Request(string json)
		{
			using (AndroidJavaObject androidJavaObject = new AndroidJavaObject("com.fyber.unity.requesters.RequesterWrapper", new object[0]))
			{
				androidJavaObject.CallStatic("request", new object[]
				{
					json
				});
			}
		}

		// Token: 0x0600098C RID: 2444 RVA: 0x00039B48 File Offset: 0x00037D48
		public void StartAd(string json)
		{
			using (AndroidJavaObject androidJavaObject = new AndroidJavaObject("com.fyber.unity.ads.AdWrapper", new object[0]))
			{
				androidJavaObject.CallStatic("start", new object[]
				{
					json
				});
			}
		}

		// Token: 0x0600098D RID: 2445 RVA: 0x00039BAC File Offset: 0x00037DAC
		public bool Banner(string json)
		{
			bool result;
			using (AndroidJavaObject androidJavaObject = new AndroidJavaObject("com.fyber.unity.ads.AdWrapper", new object[0]))
			{
				result = androidJavaObject.CallStatic<bool>("performAdActions", new object[]
				{
					json
				});
			}
			return result;
		}

		// Token: 0x0600098E RID: 2446 RVA: 0x00039C14 File Offset: 0x00037E14
		public void Report(string json)
		{
			using (AndroidJavaObject androidJavaObject = new AndroidJavaObject("com.fyber.unity.reporters.ReporterWrapper", new object[0]))
			{
				androidJavaObject.CallStatic("report", new object[]
				{
					json
				});
			}
		}

		// Token: 0x0600098F RID: 2447 RVA: 0x00039C78 File Offset: 0x00037E78
		public string Settings(string json)
		{
			string result;
			using (AndroidJavaObject androidJavaObject = new AndroidJavaObject("com.fyber.unity.settings.SettingsWrapper", new object[0]))
			{
				result = androidJavaObject.CallStatic<string>("perform", new object[]
				{
					json
				});
			}
			return result;
		}

		// Token: 0x06000990 RID: 2448 RVA: 0x00039CE0 File Offset: 0x00037EE0
		public void EnableLogging(bool shouldLog)
		{
			using (AndroidJavaObject androidJavaObject = new AndroidJavaObject("com.fyber.utils.FyberLogger", new object[0]))
			{
				androidJavaObject.CallStatic<bool>("enableLogging", new object[]
				{
					shouldLog
				});
			}
		}

		// Token: 0x06000991 RID: 2449 RVA: 0x00039D48 File Offset: 0x00037F48
		public void GameObjectStarted()
		{
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.fyber.unity.helpers.NativeMessage"))
			{
				androidJavaClass.CallStatic("resendFailedMessages", new object[0]);
			}
		}

		// Token: 0x06000992 RID: 2450 RVA: 0x00039DA0 File Offset: 0x00037FA0
		public void ApplicationQuit()
		{
			this.Cache("unregisterOnVideoCacheListener");
		}
	}
}
