using System;
using System.Globalization;

namespace Rilisoft
{
	// Token: 0x02000531 RID: 1329
	internal sealed class AfterMatchInterstitialRunner : FyberInterstitialRunnerBase
	{
		// Token: 0x06002E56 RID: 11862 RVA: 0x000F2678 File Offset: 0x000F0878
		public static string GetReasonToDismissInterstitialAfterMatch(bool winner, double matchDurationInMinutes)
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
			AfterMatchAdPointMemento afterMatch = lastLoadedConfig.AdPointsConfig.AfterMatch;
			if (afterMatch == null)
			{
				return string.Format("`{0}` config is `null`", afterMatch.Id);
			}
			string playerCategory = AdsConfigManager.GetPlayerCategory(lastLoadedConfig);
			string disabledReason = afterMatch.GetDisabledReason(playerCategory);
			if (!string.IsNullOrEmpty(disabledReason))
			{
				return disabledReason;
			}
			double? matchMinDurationInMinutesOverride = afterMatch.GetMatchMinDurationInMinutesOverride(playerCategory);
			if (matchMinDurationInMinutesOverride != null)
			{
				if (matchDurationInMinutes < matchMinDurationInMinutesOverride.Value)
				{
					return string.Format(CultureInfo.InvariantCulture, "{0:f2} < `matchMinDuration` = {1:f2} in `{2}` for category `{3}`.", new object[]
					{
						matchDurationInMinutes,
						matchMinDurationInMinutesOverride.Value,
						afterMatch.Id,
						playerCategory
					});
				}
			}
			else if (matchDurationInMinutes < afterMatch.MatchMinDurationInMinutes)
			{
				return string.Format(CultureInfo.InvariantCulture, "{0:f2} < `matchMinDuration` = {1:f2} in `{2}`.", new object[]
				{
					matchDurationInMinutes,
					afterMatch.MatchMinDurationInMinutes,
					afterMatch.Id
				});
			}
			bool? flag = (!winner) ? afterMatch.GetEnabledForLoserOverride(playerCategory) : afterMatch.GetEnabledForWinnerOverride(playerCategory);
			if (flag != null)
			{
				if (!flag.Value)
				{
					return string.Format(CultureInfo.InvariantCulture, "Disabled for `winner: {0}` in `{1}` for category `{2}`.", new object[]
					{
						winner,
						afterMatch.Id,
						playerCategory
					});
				}
			}
			else if (!((!winner) ? afterMatch.EnabledForLoser : afterMatch.EnabledForWinner))
			{
				return string.Format(CultureInfo.InvariantCulture, "Disabled for `winner: {0}` in `{1}`.", new object[]
				{
					winner,
					afterMatch.Id
				});
			}
			return string.Empty;
		}

		// Token: 0x06002E57 RID: 11863 RVA: 0x000F2848 File Offset: 0x000F0A48
		protected override string GetReasonToSkip()
		{
			if (Initializer.Instance == null)
			{
				return "Initializer.Instance == null";
			}
			if (ShopNGUIController.GuiActive)
			{
				return "ShopNGUIController.GuiActive";
			}
			return string.Empty;
		}
	}
}
