using System;
using System.Collections.Generic;
using Facebook.Unity;
using Rilisoft.MiniJson;
using UnityEngine;

namespace Rilisoft
{
	// Token: 0x02000556 RID: 1366
	internal sealed class FacebookFacade
	{
		// Token: 0x06002F8D RID: 12173 RVA: 0x000F8E9C File Offset: 0x000F709C
		internal void LogPurchase(float currencyAmount, string currencyIsoCode, Dictionary<string, object> parameters = null)
		{
			if (!FB.IsInitialized)
			{
				Debug.LogWarning("Facebook is not initialized.");
				return;
			}
			FB.LogPurchase(currencyAmount, currencyIsoCode, parameters);
		}

		// Token: 0x06002F8E RID: 12174 RVA: 0x000F8EBC File Offset: 0x000F70BC
		internal void LogAppEvent(string logEvent, float? valueToSum, Dictionary<string, object> parameters)
		{
			if (!FB.IsInitialized)
			{
				Debug.LogWarning("Facebook is not initialized.");
				return;
			}
			if (Application.isEditor)
			{
				string text = (parameters == null) ? "{}" : Json.Serialize(parameters);
				Debug.LogFormat("`{0}`: {1}", new object[]
				{
					logEvent,
					text
				});
			}
			FB.LogAppEvent(logEvent, valueToSum, parameters);
		}
	}
}
