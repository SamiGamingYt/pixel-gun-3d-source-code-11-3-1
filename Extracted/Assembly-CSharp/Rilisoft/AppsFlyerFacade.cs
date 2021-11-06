using System;
using System.Collections.Generic;
using UnityEngine;

namespace Rilisoft
{
	// Token: 0x02000552 RID: 1362
	internal sealed class AppsFlyerFacade
	{
		// Token: 0x06002F7A RID: 12154 RVA: 0x000F8BD4 File Offset: 0x000F6DD4
		public AppsFlyerFacade(string appKey, string appId)
		{
			if (appKey == null)
			{
				throw new ArgumentNullException("appKey");
			}
			if (appId == null)
			{
				throw new ArgumentNullException(appId);
			}
			this._appId = appId;
			if (Application.isEditor)
			{
				return;
			}
			AppsFlyer.setAppsFlyerKey(appKey);
			AppsFlyer.setAppID(appId);
		}

		// Token: 0x06002F7B RID: 12155 RVA: 0x000F8C24 File Offset: 0x000F6E24
		public void TrackEvent(string eventName, string eventValue)
		{
			if (eventName == null)
			{
				throw new ArgumentNullException("eventName");
			}
			if (eventValue == null)
			{
				throw new ArgumentNullException("eventValue");
			}
			if (string.IsNullOrEmpty(eventName))
			{
				throw new ArgumentException("Event name must not be empty.", "eventName");
			}
			if (string.IsNullOrEmpty(this._appId))
			{
				return;
			}
			if (Application.isEditor)
			{
				return;
			}
			AppsFlyer.trackEvent(eventName, eventValue);
		}

		// Token: 0x06002F7C RID: 12156 RVA: 0x000F8C94 File Offset: 0x000F6E94
		public void TrackRichEvent(string eventName, Dictionary<string, string> eventValues)
		{
			if (eventName == null)
			{
				throw new ArgumentNullException("eventName");
			}
			if (eventValues == null)
			{
				throw new ArgumentNullException("eventValues");
			}
			if (string.IsNullOrEmpty(eventName))
			{
				throw new ArgumentException("Event name must not be empty.", "eventName");
			}
			if (string.IsNullOrEmpty(this._appId))
			{
				return;
			}
			if (Application.isEditor)
			{
				return;
			}
			AppsFlyer.trackRichEvent(eventName, eventValues);
		}

		// Token: 0x06002F7D RID: 12157 RVA: 0x000F8D04 File Offset: 0x000F6F04
		public void TrackAppLaunch()
		{
			if (string.IsNullOrEmpty(this._appId))
			{
				return;
			}
			if (Application.isEditor)
			{
				return;
			}
			AppsFlyer.trackAppLaunch();
		}

		// Token: 0x17000831 RID: 2097
		// (set) Token: 0x06002F7E RID: 12158 RVA: 0x000F8D28 File Offset: 0x000F6F28
		public static bool LoggingEnabled
		{
			set
			{
				if (Application.isEditor)
				{
					return;
				}
				AppsFlyer.setIsDebug(value);
			}
		}

		// Token: 0x040022F4 RID: 8948
		private readonly string _appId;
	}
}
