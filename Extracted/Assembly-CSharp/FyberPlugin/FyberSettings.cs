using System;
using UnityEngine;

namespace FyberPlugin
{
	// Token: 0x02000134 RID: 308
	public class FyberSettings : ScriptableObject
	{
		// Token: 0x060009CF RID: 2511 RVA: 0x0003A2F0 File Offset: 0x000384F0
		private void OnEnable()
		{
			FyberSettings.GetInstance();
		}

		// Token: 0x060009D0 RID: 2512 RVA: 0x0003A2F8 File Offset: 0x000384F8
		private static FyberSettings GetInstance()
		{
			if (FyberSettings.instance == null)
			{
				PluginBridge.bridge = new PluginBridgeComponent();
				FyberSettings.instance = (Resources.Load("FyberSettings") as FyberSettings);
				if (FyberSettings.instance == null)
				{
					FyberSettings.instance = ScriptableObject.CreateInstance<FyberSettings>();
				}
			}
			return FyberSettings.instance;
		}

		// Token: 0x17000103 RID: 259
		// (get) Token: 0x060009D1 RID: 2513 RVA: 0x0003A354 File Offset: 0x00038554
		public static FyberSettings Instance
		{
			get
			{
				return FyberSettings.GetInstance();
			}
		}

		// Token: 0x060009D2 RID: 2514 RVA: 0x0003A35C File Offset: 0x0003855C
		internal string BundlesInfoJson()
		{
			return this.bundlesJson;
		}

		// Token: 0x060009D3 RID: 2515 RVA: 0x0003A364 File Offset: 0x00038564
		internal string BundlesConfigJson()
		{
			return this.configJson;
		}

		// Token: 0x060009D4 RID: 2516 RVA: 0x0003A36C File Offset: 0x0003856C
		internal int BundlesCount()
		{
			return this.bundlesCount;
		}

		// Token: 0x040007E7 RID: 2023
		private const string fyberSettingsAssetName = "FyberSettings";

		// Token: 0x040007E8 RID: 2024
		private const string fyberSettingsPath = "Fyber/Resources";

		// Token: 0x040007E9 RID: 2025
		private const string fyberSettingsAssetExtension = ".asset";

		// Token: 0x040007EA RID: 2026
		private static FyberSettings instance;

		// Token: 0x040007EB RID: 2027
		[SerializeField]
		[HideInInspector]
		private string bundlesJson;

		// Token: 0x040007EC RID: 2028
		[HideInInspector]
		[SerializeField]
		private string configJson;

		// Token: 0x040007ED RID: 2029
		[HideInInspector]
		[SerializeField]
		private int bundlesCount;
	}
}
