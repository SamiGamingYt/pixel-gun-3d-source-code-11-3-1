using System;
using System.Globalization;
using UnityEngine.SceneManagement;

namespace Rilisoft
{
	// Token: 0x02000533 RID: 1331
	internal sealed class FiringFieldInterstitialRunner : FyberInterstitialRunnerBase
	{
		// Token: 0x06002E69 RID: 11881 RVA: 0x000F2EB4 File Offset: 0x000F10B4
		public static string GetReasonToDismissInterstitialPolygon(int entryCount)
		{
			AdsConfigMemento lastLoadedConfig = AdsConfigManager.Instance.LastLoadedConfig;
			if (lastLoadedConfig == null)
			{
				return "Interstitials config is `null`.";
			}
			string interstitialDisabledReason = AdsConfigManager.GetInterstitialDisabledReason(lastLoadedConfig);
			if (!string.IsNullOrEmpty(interstitialDisabledReason))
			{
				return interstitialDisabledReason;
			}
			PolygonAdPointMemento polygon = lastLoadedConfig.AdPointsConfig.Polygon;
			if (polygon == null)
			{
				return "Polygon config is `null`";
			}
			string playerCategory = AdsConfigManager.GetPlayerCategory(lastLoadedConfig);
			string disabledReason = polygon.GetDisabledReason(playerCategory);
			if (!string.IsNullOrEmpty(disabledReason))
			{
				return disabledReason;
			}
			int? entryCountOverride = polygon.GetEntryCountOverride(playerCategory);
			if (entryCountOverride != null)
			{
				if (entryCount < entryCountOverride.Value)
				{
					return string.Format(CultureInfo.InvariantCulture, "{0} < `entryCount` = {1} in `{2}` for category `{3}`.", new object[]
					{
						entryCount,
						entryCountOverride.Value,
						polygon.Id,
						playerCategory
					});
				}
			}
			else if (entryCount < polygon.EntryCount)
			{
				return string.Format(CultureInfo.InvariantCulture, "{0} < `waveMinCount` = {1} in `{2}`.", new object[]
				{
					entryCount,
					polygon.EntryCount,
					polygon.Id
				});
			}
			return string.Empty;
		}

		// Token: 0x06002E6A RID: 11882 RVA: 0x000F2FCC File Offset: 0x000F11CC
		protected override string GetReasonToSkip()
		{
			string name = SceneManager.GetActiveScene().name;
			if (name == Defs.MainMenuScene)
			{
				return string.Empty;
			}
			return "Not in main scene: " + name;
		}
	}
}
