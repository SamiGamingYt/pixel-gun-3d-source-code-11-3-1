using System;
using System.Collections.Generic;

namespace Rilisoft
{
	// Token: 0x02000555 RID: 1365
	internal sealed class DevToDevFacade
	{
		// Token: 0x06002F80 RID: 12160 RVA: 0x000F8D44 File Offset: 0x000F6F44
		public DevToDevFacade(string appKey, string secretKey)
		{
			if (appKey == null)
			{
				throw new ArgumentNullException("appKey");
			}
			if (secretKey == null)
			{
				throw new ArgumentNullException("secretKey");
			}
			this._appKey = appKey;
		}

		// Token: 0x17000832 RID: 2098
		// (get) Token: 0x06002F81 RID: 12161 RVA: 0x000F8D78 File Offset: 0x000F6F78
		public static string Version
		{
			get
			{
				return string.Empty;
			}
		}

		// Token: 0x06002F82 RID: 12162 RVA: 0x000F8D80 File Offset: 0x000F6F80
		public void SendCustomEvent(string eventName)
		{
			if (eventName == null)
			{
				throw new ArgumentNullException("eventName");
			}
			if (string.IsNullOrEmpty(eventName))
			{
				throw new ArgumentException("Event name must not be empty.", "eventName");
			}
			if (string.IsNullOrEmpty(this._appKey))
			{
				return;
			}
		}

		// Token: 0x06002F83 RID: 12163 RVA: 0x000F8DC0 File Offset: 0x000F6FC0
		public void InAppPurchase(string purchaseId, string purchaseType, int purchaseAmount, int purchasePrice, string purchaseCurrency)
		{
			purchaseId = purchaseId.Substring(0, Math.Min(purchaseId.Length, 32));
			purchaseType = purchaseType.Substring(0, Math.Min(purchaseType.Length, 96));
		}

		// Token: 0x06002F84 RID: 12164 RVA: 0x000F8DFC File Offset: 0x000F6FFC
		public void RealPayment(string paymentId, float inAppPrice, string inAppName, string inAppCurrencyISOCode)
		{
		}

		// Token: 0x06002F85 RID: 12165 RVA: 0x000F8E00 File Offset: 0x000F7000
		public void CurrencyAccrual(int amount, string currencyName, AnalyticsConstants.AccrualType accrualType)
		{
		}

		// Token: 0x06002F86 RID: 12166 RVA: 0x000F8E04 File Offset: 0x000F7004
		public void LevelUp(int level, Dictionary<string, int> resources)
		{
		}

		// Token: 0x06002F87 RID: 12167 RVA: 0x000F8E08 File Offset: 0x000F7008
		public void Tutorial(AnalyticsConstants.TutorialState step)
		{
		}

		// Token: 0x06002F88 RID: 12168 RVA: 0x000F8E0C File Offset: 0x000F700C
		public void SendCustomEvent(string eventName, IDictionary<string, object> eventParams)
		{
			if (eventName == null)
			{
				throw new ArgumentNullException("eventName");
			}
			if (eventParams == null)
			{
				throw new ArgumentNullException("eventParams");
			}
			if (string.IsNullOrEmpty(eventName))
			{
				throw new ArgumentException("Event name must not be empty.", "eventName");
			}
			if (string.IsNullOrEmpty(this._appKey))
			{
				return;
			}
		}

		// Token: 0x17000833 RID: 2099
		// (set) Token: 0x06002F89 RID: 12169 RVA: 0x000F8E68 File Offset: 0x000F7068
		public bool UserIsCheater
		{
			set
			{
				if (string.IsNullOrEmpty(this._appKey))
				{
					return;
				}
			}
		}

		// Token: 0x17000834 RID: 2100
		// (set) Token: 0x06002F8A RID: 12170 RVA: 0x000F8E7C File Offset: 0x000F707C
		public static bool LoggingEnabled
		{
			set
			{
			}
		}

		// Token: 0x06002F8B RID: 12171 RVA: 0x000F8E80 File Offset: 0x000F7080
		public void SendBufferedEvents()
		{
			if (string.IsNullOrEmpty(this._appKey))
			{
				return;
			}
		}

		// Token: 0x040022FB RID: 8955
		private readonly string _appKey;
	}
}
