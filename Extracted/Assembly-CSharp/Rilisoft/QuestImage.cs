using System;
using System.Collections.Generic;
using UnityEngine;

namespace Rilisoft
{
	// Token: 0x02000726 RID: 1830
	internal struct QuestImage
	{
		// Token: 0x17000A9E RID: 2718
		// (get) Token: 0x06003FC4 RID: 16324 RVA: 0x00155830 File Offset: 0x00153A30
		public static QuestImage Instance
		{
			get
			{
				return QuestImage.s_instance;
			}
		}

		// Token: 0x06003FC5 RID: 16325 RVA: 0x00155838 File Offset: 0x00153A38
		public Color GetColor(QuestBase quest)
		{
			if (quest == null)
			{
				return QuestImage.s_defaultColor;
			}
			if (this.CampaignQuests.Contains(quest.Id))
			{
				return new Color32(byte.MaxValue, 184, 0, byte.MaxValue);
			}
			if (this.ArenaQuests.Contains(quest.Id))
			{
				return new Color32(byte.MaxValue, 121, 0, byte.MaxValue);
			}
			return QuestImage.s_defaultColor;
		}

		// Token: 0x06003FC6 RID: 16326 RVA: 0x001558B8 File Offset: 0x00153AB8
		public string GetSpriteName(QuestBase quest)
		{
			if (quest == null)
			{
				return this.GetSpriteNameForMultiplayer();
			}
			ModeAccumulativeQuest modeAccumulativeQuest = quest as ModeAccumulativeQuest;
			if (modeAccumulativeQuest != null)
			{
				return this.GetSpriteNameForMultiplayer(modeAccumulativeQuest.Mode);
			}
			WeaponSlotAccumulativeQuest weaponSlotAccumulativeQuest = quest as WeaponSlotAccumulativeQuest;
			if (weaponSlotAccumulativeQuest != null)
			{
				if (this.CampaignQuests.Contains(quest.Id))
				{
					return this.GetSpriteNameForCampaign(weaponSlotAccumulativeQuest.WeaponSlot);
				}
				return this.GetSpriteNameForMultiplayer(weaponSlotAccumulativeQuest.WeaponSlot);
			}
			else
			{
				if (this.ArenaQuests.Contains(quest.Id))
				{
					return this.GetSpriteNameForArena();
				}
				if (this.CampaignQuests.Contains(quest.Id))
				{
					return this.GetSpriteNameForCampaign();
				}
				return this.GetSpriteNameForMultiplayer();
			}
		}

		// Token: 0x06003FC7 RID: 16327 RVA: 0x0015596C File Offset: 0x00153B6C
		private string GetSpriteNameForMultiplayer()
		{
			return "battle_now_znachek";
		}

		// Token: 0x06003FC8 RID: 16328 RVA: 0x00155974 File Offset: 0x00153B74
		private string GetSpriteNameForMultiplayer(ConnectSceneNGUIController.RegimGame mode)
		{
			string result;
			if (this.MapModeToSpriteName.TryGetValue(mode, out result))
			{
				return result;
			}
			return this.GetSpriteNameForMultiplayer();
		}

		// Token: 0x06003FC9 RID: 16329 RVA: 0x0015599C File Offset: 0x00153B9C
		private string GetSpriteNameForMultiplayer(ShopNGUIController.CategoryNames weapon)
		{
			string result;
			if (this.MapWeaponToSpriteName.TryGetValue(weapon, out result))
			{
				return result;
			}
			return this.GetSpriteNameForMultiplayer();
		}

		// Token: 0x06003FCA RID: 16330 RVA: 0x001559C4 File Offset: 0x00153BC4
		private string GetSpriteNameForCampaign()
		{
			return "star";
		}

		// Token: 0x06003FCB RID: 16331 RVA: 0x001559CC File Offset: 0x00153BCC
		private string GetSpriteNameForCampaign(ShopNGUIController.CategoryNames weapon)
		{
			string result;
			if (this.MapWeaponToSpriteName.TryGetValue(weapon, out result))
			{
				return result;
			}
			return this.GetSpriteNameForCampaign();
		}

		// Token: 0x06003FCC RID: 16332 RVA: 0x001559F4 File Offset: 0x00153BF4
		private string GetSpriteNameForArena()
		{
			return "mode_arena";
		}

		// Token: 0x17000A9F RID: 2719
		// (get) Token: 0x06003FCD RID: 16333 RVA: 0x001559FC File Offset: 0x00153BFC
		private HashSet<string> CampaignQuests
		{
			get
			{
				if (this._campaignQuests == null)
				{
					this._campaignQuests = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
					this._campaignQuests.Add("killInCampaign");
					this._campaignQuests.Add("killNpcWithWeapon");
				}
				return this._campaignQuests;
			}
		}

		// Token: 0x17000AA0 RID: 2720
		// (get) Token: 0x06003FCE RID: 16334 RVA: 0x00155A4C File Offset: 0x00153C4C
		private HashSet<string> ArenaQuests
		{
			get
			{
				if (this._arenaQuests == null)
				{
					this._arenaQuests = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
					this._arenaQuests.Add("surviveWavesInArena");
				}
				return this._arenaQuests;
			}
		}

		// Token: 0x17000AA1 RID: 2721
		// (get) Token: 0x06003FCF RID: 16335 RVA: 0x00155A8C File Offset: 0x00153C8C
		private Dictionary<ConnectSceneNGUIController.RegimGame, string> MapModeToSpriteName
		{
			get
			{
				if (this._mapModeToSpriteName == null)
				{
					this._mapModeToSpriteName = new Dictionary<ConnectSceneNGUIController.RegimGame, string>(new GameRegimeComparer())
					{
						{
							ConnectSceneNGUIController.RegimGame.Deathmatch,
							"mode_death_znachek"
						},
						{
							ConnectSceneNGUIController.RegimGame.TimeBattle,
							"mode_time_znachek"
						},
						{
							ConnectSceneNGUIController.RegimGame.TeamFight,
							"mode_team_znachek"
						},
						{
							ConnectSceneNGUIController.RegimGame.DeadlyGames,
							"mode_deadly_games_znachek"
						},
						{
							ConnectSceneNGUIController.RegimGame.FlagCapture,
							"mode_flag_znachek"
						},
						{
							ConnectSceneNGUIController.RegimGame.CapturePoints,
							"mode_capture_point"
						}
					};
				}
				return this._mapModeToSpriteName;
			}
		}

		// Token: 0x17000AA2 RID: 2722
		// (get) Token: 0x06003FD0 RID: 16336 RVA: 0x00155B04 File Offset: 0x00153D04
		private Dictionary<ShopNGUIController.CategoryNames, string> MapWeaponToSpriteName
		{
			get
			{
				if (this._mapWeaponToSpriteName == null)
				{
					this._mapWeaponToSpriteName = new Dictionary<ShopNGUIController.CategoryNames, string>(6, ShopNGUIController.CategoryNameComparer.Instance)
					{
						{
							ShopNGUIController.CategoryNames.BackupCategory,
							"shop_icons_backup"
						},
						{
							ShopNGUIController.CategoryNames.MeleeCategory,
							"shop_icons_melee"
						},
						{
							ShopNGUIController.CategoryNames.PremiumCategory,
							"shop_icons_premium"
						},
						{
							ShopNGUIController.CategoryNames.PrimaryCategory,
							"shop_icons_primary"
						},
						{
							ShopNGUIController.CategoryNames.SniperCategory,
							"shop_icons_sniper"
						},
						{
							ShopNGUIController.CategoryNames.SpecilCategory,
							"shop_icons_special"
						}
					};
				}
				return this._mapWeaponToSpriteName;
			}
		}

		// Token: 0x04002EFA RID: 12026
		private HashSet<string> _arenaQuests;

		// Token: 0x04002EFB RID: 12027
		private HashSet<string> _campaignQuests;

		// Token: 0x04002EFC RID: 12028
		private HashSet<string> _modeQuests;

		// Token: 0x04002EFD RID: 12029
		private HashSet<string> _weaponQuests;

		// Token: 0x04002EFE RID: 12030
		private Dictionary<ConnectSceneNGUIController.RegimGame, string> _mapModeToSpriteName;

		// Token: 0x04002EFF RID: 12031
		private Dictionary<ShopNGUIController.CategoryNames, string> _mapWeaponToSpriteName;

		// Token: 0x04002F00 RID: 12032
		private static readonly Color s_defaultColor = new Color32(0, 253, 53, byte.MaxValue);

		// Token: 0x04002F01 RID: 12033
		private static readonly QuestImage s_instance = default(QuestImage);
	}
}
