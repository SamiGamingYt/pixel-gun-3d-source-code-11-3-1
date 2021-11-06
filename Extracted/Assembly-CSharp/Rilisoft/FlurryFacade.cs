using System;
using System.Collections.Generic;
using Prime31;

namespace Rilisoft
{
	// Token: 0x02000557 RID: 1367
	internal sealed class FlurryFacade
	{
		// Token: 0x06002F8F RID: 12175 RVA: 0x000F8F20 File Offset: 0x000F7120
		public FlurryFacade(string apiKey, bool enableLogging)
		{
			if (apiKey == null)
			{
				throw new ArgumentNullException("apiKey");
			}
			FlurryAnalytics.startSession(apiKey, enableLogging);
			FlurryAnalytics.setLogEnabled(enableLogging);
		}

		// Token: 0x06002F90 RID: 12176 RVA: 0x000F8F54 File Offset: 0x000F7154
		public void LogEvent(string eventName)
		{
			if (eventName == null)
			{
				throw new ArgumentNullException("eventName");
			}
			FlurryAnalytics.logEvent(eventName);
		}

		// Token: 0x06002F91 RID: 12177 RVA: 0x000F8F70 File Offset: 0x000F7170
		public void LogEventWithParameters(string eventName, Dictionary<string, string> parameters)
		{
			if (eventName == null)
			{
				throw new ArgumentNullException("eventName");
			}
			if (parameters == null)
			{
				throw new ArgumentNullException("parameters");
			}
			FlurryAnalytics.logEvent(eventName, parameters);
		}

		// Token: 0x06002F92 RID: 12178 RVA: 0x000F8F9C File Offset: 0x000F719C
		public void SetUserId(string userId)
		{
			if (userId == null)
			{
				throw new ArgumentNullException("userId");
			}
			FlurryAnalytics.setUserID(userId);
		}
	}
}
