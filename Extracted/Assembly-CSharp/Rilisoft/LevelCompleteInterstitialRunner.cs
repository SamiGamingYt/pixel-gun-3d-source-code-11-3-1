using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine.SceneManagement;

namespace Rilisoft
{
	// Token: 0x02000541 RID: 1345
	internal sealed class LevelCompleteInterstitialRunner : FyberInterstitialRunnerBase
	{
		// Token: 0x06002ED3 RID: 11987 RVA: 0x000F4ABC File Offset: 0x000F2CBC
		public LevelCompleteInterstitialRunner()
		{
			this._allowedScenes = new HashSet<string>
			{
				Defs.MainMenuScene,
				"LevelComplete",
				"ChooseLevel",
				"CampaignChooseBox",
				"PromScene",
				"LevelToCompleteProm",
				SceneManager.GetActiveScene().name
			};
		}

		// Token: 0x06002ED4 RID: 11988 RVA: 0x000F4B38 File Offset: 0x000F2D38
		public static string GetReasonToDismissInterstitialCampaign(bool afterDeath)
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
			CampaignAdPointMemento campaign = lastLoadedConfig.AdPointsConfig.Campaign;
			if (campaign == null)
			{
				return "Campaign config is `null`";
			}
			string playerCategory = AdsConfigManager.GetPlayerCategory(lastLoadedConfig);
			string disabledReason = campaign.GetDisabledReason(playerCategory);
			if (!string.IsNullOrEmpty(disabledReason))
			{
				return disabledReason;
			}
			string reasonToDismissInterstitialLevelComplete = LevelCompleteInterstitialRunner.GetReasonToDismissInterstitialLevelComplete(campaign, playerCategory, afterDeath);
			if (!string.IsNullOrEmpty(reasonToDismissInterstitialLevelComplete))
			{
				return reasonToDismissInterstitialLevelComplete;
			}
			return string.Empty;
		}

		// Token: 0x06002ED5 RID: 11989 RVA: 0x000F4BC8 File Offset: 0x000F2DC8
		public static string GetReasonToDismissInterstitialSurvivalArena(bool afterDeath)
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
			SurvivalArenaAdPointMemento survivalArena = lastLoadedConfig.AdPointsConfig.SurvivalArena;
			if (survivalArena == null)
			{
				return string.Format("`{0}` config is `null`", survivalArena.Id);
			}
			string playerCategory = AdsConfigManager.GetPlayerCategory(lastLoadedConfig);
			string disabledReason = survivalArena.GetDisabledReason(playerCategory);
			if (!string.IsNullOrEmpty(disabledReason))
			{
				return disabledReason;
			}
			string reasonToDismissInterstitialLevelComplete = LevelCompleteInterstitialRunner.GetReasonToDismissInterstitialLevelComplete(survivalArena, playerCategory, afterDeath);
			if (!string.IsNullOrEmpty(reasonToDismissInterstitialLevelComplete))
			{
				return reasonToDismissInterstitialLevelComplete;
			}
			int? waveMinCountOverride = survivalArena.GetWaveMinCountOverride(playerCategory);
			if (waveMinCountOverride != null)
			{
				if (WavesSurvivedStat.SurvivedWaveCount < waveMinCountOverride.Value)
				{
					return string.Format(CultureInfo.InvariantCulture, "{0} < `waveMinCount` = {1} in `{2}` for category `{3}`.", new object[]
					{
						WavesSurvivedStat.SurvivedWaveCount,
						waveMinCountOverride.Value,
						survivalArena.Id,
						playerCategory
					});
				}
			}
			else if (WavesSurvivedStat.SurvivedWaveCount < survivalArena.WaveMinCount)
			{
				return string.Format(CultureInfo.InvariantCulture, "{0} < `waveMinCount` = {1} in `{2}`.", new object[]
				{
					WavesSurvivedStat.SurvivedWaveCount,
					survivalArena.WaveMinCount,
					survivalArena.Id
				});
			}
			return string.Empty;
		}

		// Token: 0x06002ED6 RID: 11990 RVA: 0x000F4D14 File Offset: 0x000F2F14
		private static string GetReasonToDismissInterstitialLevelComplete(LevelCompleteAdPointMementoBase levelCompleteConfig, string category, bool afterDeath)
		{
			if (afterDeath)
			{
				bool? deathOverride = levelCompleteConfig.GetDeathOverride(category);
				if (deathOverride != null)
				{
					if (!deathOverride.Value)
					{
						return string.Format(CultureInfo.InvariantCulture, "`death` in `{0}` explicitely disabled for category `{1}`.", new object[]
						{
							levelCompleteConfig.Id,
							category
						});
					}
				}
				else if (!levelCompleteConfig.Death)
				{
					return string.Format(CultureInfo.InvariantCulture, "`death` in `{0}` disabled.", new object[]
					{
						levelCompleteConfig.Id
					});
				}
			}
			else
			{
				bool? quitOverride = levelCompleteConfig.GetQuitOverride(category);
				if (quitOverride != null)
				{
					if (!quitOverride.Value)
					{
						return string.Format(CultureInfo.InvariantCulture, "`quit` in `{0}` explicitely disabled for category `{1}`.", new object[]
						{
							levelCompleteConfig.Id,
							category
						});
					}
				}
				else if (!levelCompleteConfig.Quit)
				{
					return string.Format(CultureInfo.InvariantCulture, "`quit` in `{0}` disabled.", new object[]
					{
						levelCompleteConfig.Id
					});
				}
			}
			return string.Empty;
		}

		// Token: 0x06002ED7 RID: 11991 RVA: 0x000F4E18 File Offset: 0x000F3018
		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
			this._allowedScenes.Clear();
		}

		// Token: 0x06002ED8 RID: 11992 RVA: 0x000F4E2C File Offset: 0x000F302C
		protected override string GetReasonToSkip()
		{
			string name = SceneManager.GetActiveScene().name;
			if (this._allowedScenes.Contains(name))
			{
				return string.Empty;
			}
			return "Scene is not allowed: " + name;
		}

		// Token: 0x0400229B RID: 8859
		private readonly HashSet<string> _allowedScenes;
	}
}
