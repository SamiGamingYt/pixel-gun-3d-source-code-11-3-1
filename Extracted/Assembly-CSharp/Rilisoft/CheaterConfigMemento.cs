using System;
using System.Collections.Generic;
using UnityEngine;

namespace Rilisoft
{
	// Token: 0x0200052F RID: 1327
	[Serializable]
	internal sealed class CheaterConfigMemento
	{
		// Token: 0x06002E40 RID: 11840 RVA: 0x000F21A4 File Offset: 0x000F03A4
		public CheaterConfigMemento()
		{
			this.CoinThreshold = int.MaxValue;
			this.GemThreshold = int.MaxValue;
		}

		// Token: 0x06002E41 RID: 11841 RVA: 0x000F21C4 File Offset: 0x000F03C4
		internal static CheaterConfigMemento FromDictionary(Dictionary<string, object> dictionary)
		{
			if (dictionary == null)
			{
				throw new ArgumentNullException("dictionary");
			}
			CheaterConfigMemento cheaterConfigMemento = new CheaterConfigMemento();
			object obj;
			if (dictionary.TryGetValue("checkSignatureTampering", out obj))
			{
				try
				{
					cheaterConfigMemento.CheckSignatureTampering = Convert.ToBoolean(obj);
				}
				catch (Exception ex)
				{
					Debug.LogWarningFormat("Failed to interpret `\"{0}\": {1}` as boolean. {2}", new object[]
					{
						"checkSignatureTampering",
						obj,
						ex.Message
					});
				}
			}
			object obj2;
			if (dictionary.TryGetValue("coinThreshold", out obj2))
			{
				try
				{
					cheaterConfigMemento.CoinThreshold = Convert.ToInt32(obj2);
				}
				catch (Exception ex2)
				{
					Debug.LogWarningFormat("Failed to interpret `\"{0}\": {1}` as int. {2}", new object[]
					{
						"coinThreshold",
						obj2,
						ex2.Message
					});
				}
			}
			object obj3;
			if (dictionary.TryGetValue("gemThreshold", out obj3))
			{
				try
				{
					cheaterConfigMemento.GemThreshold = Convert.ToInt32(obj3);
				}
				catch (Exception ex3)
				{
					Debug.LogWarningFormat("Failed to interpret `\"{0}\": {1}` as int. {2}", new object[]
					{
						"gemThreshold",
						obj3,
						ex3.Message
					});
				}
			}
			return cheaterConfigMemento;
		}

		// Token: 0x170007F3 RID: 2035
		// (get) Token: 0x06002E42 RID: 11842 RVA: 0x000F2338 File Offset: 0x000F0538
		// (set) Token: 0x06002E43 RID: 11843 RVA: 0x000F2340 File Offset: 0x000F0540
		public bool CheckSignatureTampering { get; private set; }

		// Token: 0x170007F4 RID: 2036
		// (get) Token: 0x06002E44 RID: 11844 RVA: 0x000F234C File Offset: 0x000F054C
		// (set) Token: 0x06002E45 RID: 11845 RVA: 0x000F2354 File Offset: 0x000F0554
		public int CoinThreshold { get; private set; }

		// Token: 0x170007F5 RID: 2037
		// (get) Token: 0x06002E46 RID: 11846 RVA: 0x000F2360 File Offset: 0x000F0560
		// (set) Token: 0x06002E47 RID: 11847 RVA: 0x000F2368 File Offset: 0x000F0568
		public int GemThreshold { get; private set; }
	}
}
