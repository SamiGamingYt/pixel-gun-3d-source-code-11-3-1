using System;
using System.Collections.Generic;
using UnityEngine;

namespace Rilisoft
{
	// Token: 0x0200053F RID: 1343
	[Serializable]
	internal sealed class InterstitialOverrideMemento
	{
		// Token: 0x06002EC2 RID: 11970 RVA: 0x000F47E0 File Offset: 0x000F29E0
		internal static InterstitialOverrideMemento FromDictionary(Dictionary<string, object> dictionary)
		{
			if (dictionary == null)
			{
				throw new ArgumentNullException("dictionary");
			}
			InterstitialOverrideMemento interstitialOverrideMemento = new InterstitialOverrideMemento();
			object obj;
			if (dictionary.TryGetValue("enabled", out obj))
			{
				try
				{
					interstitialOverrideMemento.Enabled = new bool?(Convert.ToBoolean(obj));
				}
				catch (Exception ex)
				{
					Debug.LogWarningFormat("Failed to interpret `\"{0}\": {1}` as boolean. {2}", new object[]
					{
						"enabled",
						obj,
						ex.Message
					});
				}
			}
			object obj2;
			if (dictionary.TryGetValue("disableDevices", out obj2))
			{
				List<object> list = obj2 as List<object>;
				if (list != null)
				{
					foreach (object obj3 in list)
					{
						string text = obj3 as string;
						if (!string.IsNullOrEmpty(text))
						{
							if (interstitialOverrideMemento.DisabledDevices == null)
							{
								interstitialOverrideMemento.DisabledDevices = new HashSet<string>();
							}
							interstitialOverrideMemento.DisabledDevices.Add(text);
						}
					}
				}
			}
			object obj4;
			if (dictionary.TryGetValue("timeoutBetweenShowInMinutes", out obj4))
			{
				try
				{
					interstitialOverrideMemento.TimeoutBetweenShowInMinutes = new double?(Convert.ToDouble(obj4));
				}
				catch (Exception ex2)
				{
					Debug.LogWarningFormat("Failed to interpret `\"{0}\": {1}` as double. {2}", new object[]
					{
						"timeoutBetweenShowInMinutes",
						obj4,
						ex2.Message
					});
				}
			}
			return interstitialOverrideMemento;
		}

		// Token: 0x1700080E RID: 2062
		// (get) Token: 0x06002EC3 RID: 11971 RVA: 0x000F49A4 File Offset: 0x000F2BA4
		// (set) Token: 0x06002EC4 RID: 11972 RVA: 0x000F49AC File Offset: 0x000F2BAC
		public bool? Enabled { get; private set; }

		// Token: 0x1700080F RID: 2063
		// (get) Token: 0x06002EC5 RID: 11973 RVA: 0x000F49B8 File Offset: 0x000F2BB8
		// (set) Token: 0x06002EC6 RID: 11974 RVA: 0x000F49C0 File Offset: 0x000F2BC0
		public HashSet<string> DisabledDevices { get; private set; }

		// Token: 0x17000810 RID: 2064
		// (get) Token: 0x06002EC7 RID: 11975 RVA: 0x000F49CC File Offset: 0x000F2BCC
		// (set) Token: 0x06002EC8 RID: 11976 RVA: 0x000F49D4 File Offset: 0x000F2BD4
		public double? TimeoutBetweenShowInMinutes { get; private set; }
	}
}
