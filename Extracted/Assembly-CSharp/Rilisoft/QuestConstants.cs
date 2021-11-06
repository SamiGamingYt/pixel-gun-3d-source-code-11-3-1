using System;
using System.Collections.Generic;
using System.Linq;
using Rilisoft.NullExtensions;
using UnityEngine;

namespace Rilisoft
{
	// Token: 0x02000725 RID: 1829
	internal static class QuestConstants
	{
		// Token: 0x06003FBD RID: 16317 RVA: 0x00155464 File Offset: 0x00153664
		internal static string GetDifficultyKey(Difficulty difficulty)
		{
			return difficulty.ToString().ToLowerInvariant();
		}

		// Token: 0x06003FBE RID: 16318 RVA: 0x00155478 File Offset: 0x00153678
		internal static bool IsSupported(string id)
		{
			if (id == null)
			{
				throw new ArgumentNullException("id");
			}
			return QuestConstants._supportedQuests.Contains(id);
		}

		// Token: 0x06003FBF RID: 16319 RVA: 0x00155498 File Offset: 0x00153698
		internal static string[] GetSupportedQuests()
		{
			return QuestConstants._supportedQuests.ToArray<string>();
		}

		// Token: 0x06003FC0 RID: 16320 RVA: 0x001554A4 File Offset: 0x001536A4
		internal static ShopNGUIController.CategoryNames? ParseWeaponSlot(string weaponSlot)
		{
			if (string.IsNullOrEmpty(weaponSlot))
			{
				return null;
			}
			ShopNGUIController.CategoryNames value;
			if (QuestConstants._weaponSlots.TryGetValue(weaponSlot, out value))
			{
				return new ShopNGUIController.CategoryNames?(value);
			}
			ShopNGUIController.CategoryNames? result;
			try
			{
				value = (ShopNGUIController.CategoryNames)((int)Enum.Parse(typeof(ShopNGUIController.CategoryNames), weaponSlot));
				result = new ShopNGUIController.CategoryNames?(value);
			}
			catch
			{
				result = null;
			}
			return result;
		}

		// Token: 0x06003FC1 RID: 16321 RVA: 0x00155538 File Offset: 0x00153738
		internal static ConnectSceneNGUIController.RegimGame? ParseMode(string mode)
		{
			if (string.IsNullOrEmpty(mode))
			{
				return null;
			}
			ConnectSceneNGUIController.RegimGame? result;
			try
			{
				ConnectSceneNGUIController.RegimGame value = (ConnectSceneNGUIController.RegimGame)((int)Enum.Parse(typeof(ConnectSceneNGUIController.RegimGame), mode));
				result = new ConnectSceneNGUIController.RegimGame?(value);
			}
			catch
			{
				result = null;
			}
			return result;
		}

		// Token: 0x06003FC2 RID: 16322 RVA: 0x001555B4 File Offset: 0x001537B4
		public static string GetAccumulativeQuestDescriptionByType(AccumulativeQuestBase quest)
		{
			string o;
			QuestConstants.localizationQuests.TryGetValue(quest.Id, out o);
			string text = o.Map(new Func<string, string>(LocalizationStore.Get), "{0}");
			ModeAccumulativeQuest modeAccumulativeQuest = quest as ModeAccumulativeQuest;
			if (modeAccumulativeQuest != null)
			{
				string term;
				if (!ConnectSceneNGUIController.gameModesLocalizeKey.TryGetValue(Convert.ToInt32(modeAccumulativeQuest.Mode).ToString(), out term))
				{
					term = modeAccumulativeQuest.Mode.ToString();
					Debug.LogError("Couldnot find mode name for " + modeAccumulativeQuest.Mode);
				}
				return string.Format(text, string.Format("[fff600]{0}[-]", modeAccumulativeQuest.RequiredCount), string.Format("[ff9600]{0}[-]", LocalizationStore.Get(term)));
			}
			MapAccumulativeQuest mapAccumulativeQuest = quest as MapAccumulativeQuest;
			if (mapAccumulativeQuest != null)
			{
				SceneInfo infoScene = SceneInfoController.instance.GetInfoScene(mapAccumulativeQuest.Map);
				string arg = string.Empty;
				if (infoScene == null)
				{
					arg = mapAccumulativeQuest.Map;
					Debug.LogError("Couldnot find map name for " + mapAccumulativeQuest.Map);
				}
				else
				{
					arg = infoScene.TranslateName;
				}
				return string.Format(text, string.Format("[fff600]{0}[-]", mapAccumulativeQuest.RequiredCount), string.Format("[ff9600]{0}[-]", arg));
			}
			WeaponSlotAccumulativeQuest weaponSlotAccumulativeQuest = quest as WeaponSlotAccumulativeQuest;
			if (weaponSlotAccumulativeQuest != null)
			{
				string term2;
				if (!ShopNGUIController.weaponCategoryLocKeys.TryGetValue(weaponSlotAccumulativeQuest.WeaponSlot.ToString(), out term2))
				{
					term2 = weaponSlotAccumulativeQuest.WeaponSlot.ToString().Replace("Category", string.Empty);
					Debug.LogError("Couldnot find weapon name for " + weaponSlotAccumulativeQuest.WeaponSlot);
				}
				return string.Format(text, string.Format("[fff600]{0}[-]", weaponSlotAccumulativeQuest.RequiredCount), string.Format("[ff9600]{0}[-]", LocalizationStore.Get(term2)));
			}
			string result;
			try
			{
				result = string.Format(text, string.Format("[fff600]{0}[-]", quest.RequiredCount));
			}
			catch (FormatException)
			{
				result = text;
			}
			return result;
		}

		// Token: 0x04002EE1 RID: 12001
		public const string AddFriend = "addFriend";

		// Token: 0x04002EE2 RID: 12002
		public const string GetGotcha = "getGotcha";

		// Token: 0x04002EE3 RID: 12003
		public const string BreakSeries = "breakSeries";

		// Token: 0x04002EE4 RID: 12004
		public const string CaptureFlags = "captureFlags";

		// Token: 0x04002EE5 RID: 12005
		public const string CapturePoints = "capturePoints";

		// Token: 0x04002EE6 RID: 12006
		public const string JoinClan = "joinClan";

		// Token: 0x04002EE7 RID: 12007
		public const string KillFlagCarriers = "killFlagCarriers";

		// Token: 0x04002EE8 RID: 12008
		public const string KillInCampaign = "killInCampaign";

		// Token: 0x04002EE9 RID: 12009
		public const string KillInMode = "killInMode";

		// Token: 0x04002EEA RID: 12010
		public const string KillNpcWithWeapon = "killNpcWithWeapon";

		// Token: 0x04002EEB RID: 12011
		public const string KillViaHeadshot = "killViaHeadshot";

		// Token: 0x04002EEC RID: 12012
		public const string KillWithGrenade = "killWithGrenade";

		// Token: 0x04002EED RID: 12013
		public const string KillWithWeapon = "killWithWeapon";

		// Token: 0x04002EEE RID: 12014
		public const string LikeFacebook = "likeFacebook";

		// Token: 0x04002EEF RID: 12015
		public const string LoginFacebook = "loginFacebook";

		// Token: 0x04002EF0 RID: 12016
		public const string LoginTwitter = "loginTwitter";

		// Token: 0x04002EF1 RID: 12017
		public const string MakeSeries = "makeSeries";

		// Token: 0x04002EF2 RID: 12018
		public const string Revenge = "revenge";

		// Token: 0x04002EF3 RID: 12019
		public const string SurviveWavesInArena = "surviveWavesInArena";

		// Token: 0x04002EF4 RID: 12020
		public const string WinInMap = "winInMap";

		// Token: 0x04002EF5 RID: 12021
		public const string WinInMode = "winInMode";

		// Token: 0x04002EF6 RID: 12022
		public const string AnalyticsEventName = "Daily Quests";

		// Token: 0x04002EF7 RID: 12023
		private static readonly HashSet<string> _supportedQuests = new HashSet<string>(new string[]
		{
			"breakSeries",
			"killFlagCarriers",
			"killInCampaign",
			"killInMode",
			"killNpcWithWeapon",
			"killViaHeadshot",
			"killWithWeapon",
			"makeSeries",
			"revenge",
			"surviveWavesInArena",
			"winInMap",
			"winInMode",
			"captureFlags",
			"capturePoints"
		});

		// Token: 0x04002EF8 RID: 12024
		private static readonly Dictionary<string, ShopNGUIController.CategoryNames> _weaponSlots = new Dictionary<string, ShopNGUIController.CategoryNames>
		{
			{
				"Backup",
				ShopNGUIController.CategoryNames.BackupCategory
			},
			{
				"Melee",
				ShopNGUIController.CategoryNames.MeleeCategory
			},
			{
				"Premium",
				ShopNGUIController.CategoryNames.PremiumCategory
			},
			{
				"Primary",
				ShopNGUIController.CategoryNames.PrimaryCategory
			},
			{
				"Sniper",
				ShopNGUIController.CategoryNames.SniperCategory
			},
			{
				"Special",
				ShopNGUIController.CategoryNames.SpecilCategory
			}
		};

		// Token: 0x04002EF9 RID: 12025
		private static readonly Dictionary<string, string> localizationQuests = new Dictionary<string, string>
		{
			{
				"addFriend",
				"Key_1894"
			},
			{
				"getGotcha",
				"Key_2429"
			},
			{
				"breakSeries",
				"Key_1709"
			},
			{
				"captureFlags",
				"Key_1704"
			},
			{
				"capturePoints",
				"Key_1703"
			},
			{
				"joinClan",
				"Key_1895"
			},
			{
				"killFlagCarriers",
				"Key_1702"
			},
			{
				"killInCampaign",
				"Key_1712"
			},
			{
				"killInMode",
				"Key_1701"
			},
			{
				"killNpcWithWeapon",
				"Key_1713"
			},
			{
				"killViaHeadshot",
				"Key_1706"
			},
			{
				"killWithGrenade",
				"Key_1707"
			},
			{
				"killWithWeapon",
				"Key_1705"
			},
			{
				"likeFacebook",
				"Key_1892"
			},
			{
				"loginFacebook",
				"Key_1891"
			},
			{
				"loginTwitter",
				"Key_1893"
			},
			{
				"makeSeries",
				"Key_1710"
			},
			{
				"revenge",
				"Key_1708"
			},
			{
				"surviveWavesInArena",
				"Key_1711"
			},
			{
				"winInMap",
				"Key_1700"
			},
			{
				"winInMode",
				"Key_1699"
			}
		};
	}
}
