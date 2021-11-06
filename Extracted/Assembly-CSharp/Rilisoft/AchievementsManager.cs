using System;
using System.Collections.Generic;
using System.Linq;
using Rilisoft.MiniJson;
using UnityEngine;

namespace Rilisoft
{
	// Token: 0x0200051E RID: 1310
	public class AchievementsManager : Singleton<AchievementsManager>
	{
		// Token: 0x170007CB RID: 1995
		// (get) Token: 0x06002D9E RID: 11678 RVA: 0x000EFDAC File Offset: 0x000EDFAC
		// (set) Token: 0x06002D9F RID: 11679 RVA: 0x000EFDB4 File Offset: 0x000EDFB4
		public bool IsReady { get; private set; }

		// Token: 0x170007CC RID: 1996
		// (get) Token: 0x06002DA0 RID: 11680 RVA: 0x000EFDC0 File Offset: 0x000EDFC0
		public ObservableList<Achievement> AvailableAchiements
		{
			get
			{
				return this._achievements;
			}
		}

		// Token: 0x06002DA1 RID: 11681 RVA: 0x000EFDC8 File Offset: 0x000EDFC8
		public static List<AchievementProgressData> ReadLocalProgress()
		{
			return AchievementProgressData.ParseAll(Storager.getString("achievementsProgress", false));
		}

		// Token: 0x06002DA2 RID: 11682 RVA: 0x000EFDE8 File Offset: 0x000EDFE8
		protected override void Awake()
		{
			base.Awake();
			this._achievementsAll.Clear();
			TextAsset textAsset = Resources.Load<TextAsset>("Achievements/achievements_data");
			if (textAsset == null)
			{
				Debug.LogError("Achievements cfg not found");
				return;
			}
			if (Debug.isDebugBuild)
			{
			}
			IEnumerable<AchievementData> enumerable = AchievementData.ParseAllAsEnumerable(textAsset.text);
			if (Debug.isDebugBuild)
			{
			}
			List<AchievementProgressData> source = AchievementsManager.ReadLocalProgress();
			AchievementData data;
			foreach (AchievementData data2 in enumerable)
			{
				data = data2;
				AchievementProgressData achievementProgressData = source.FirstOrDefault((AchievementProgressData p) => p.AchievementId == data.Id);
				if (achievementProgressData == null)
				{
					achievementProgressData = new AchievementProgressData();
					achievementProgressData.AchievementId = data.Id;
				}
				if (Debug.isDebugBuild)
				{
				}
				Achievement ach = this.CreateAchievement(data, achievementProgressData);
				if (Debug.isDebugBuild)
				{
				}
				if (ach != null)
				{
					if (this._achievementsAll.Contains(ach))
					{
						Debug.LogErrorFormat("achievement instancing error, DUPLICATE Id: '{0}'", new object[]
						{
							ach.Id
						});
					}
					else
					{
						this._achievementsAll.Add(ach);
						ach.OnProgressChanged += delegate(bool p, bool s)
						{
							this.OnAchievementProgressChanged(ach, p, s);
						};
					}
				}
			}
			this._achievements.Clear();
			IEnumerable<Achievement> achievementsToShow = this.GetAchievementsToShow();
			this._achievements.AddRange(achievementsToShow, false);
			this.IsReady = true;
			Singleton<SceneLoader>.Instance.OnSceneLoading -= this.OnSceneLoading;
			Singleton<SceneLoader>.Instance.OnSceneLoading += this.OnSceneLoading;
		}

		// Token: 0x06002DA3 RID: 11683 RVA: 0x000EFFF8 File Offset: 0x000EE1F8
		private void Update()
		{
			AchievementsManager.Awaiter.Tick();
		}

		// Token: 0x06002DA4 RID: 11684 RVA: 0x000F0004 File Offset: 0x000EE204
		private Achievement CreateAchievement(AchievementData data, AchievementProgressData progress)
		{
			Achievement result = null;
			switch (data.ClassType)
			{
			case AchievementClassType.Unknown:
				Debug.LogError("detect AchievementClassType.Unknown");
				break;
			case AchievementClassType.KillMobs:
				result = new AchievementKillMobs(data, progress);
				break;
			case AchievementClassType.KillPlayers:
				result = new AchievementKillPlayers(data, progress);
				break;
			case AchievementClassType.KillPlayerThroughWeaponCategory:
				result = new AchievementKillPlayerThroughWeaponCategory(data, progress);
				break;
			case AchievementClassType.InflictHeadshot:
				result = new AchievementInflictHeadshot(data, progress);
				break;
			case AchievementClassType.Win:
				result = new AchievementWin(data, progress);
				break;
			case AchievementClassType.WinInRegim:
				result = new AchievementWinInRegim(data, progress);
				break;
			case AchievementClassType.Gacha:
				result = new AchievementGacha(data, progress);
				break;
			case AchievementClassType.GetCurrency:
				result = new AchievementGetCurrency(data, progress);
				break;
			case AchievementClassType.AccumulateCurrency:
				result = new AchievementAccumulateCurrency(data, progress);
				break;
			case AchievementClassType.JoinToClan:
				result = new AchievementJoinToClan(data, progress);
				break;
			case AchievementClassType.OpenLeague:
				result = new AchievementOpenLeague(data, progress);
				break;
			case AchievementClassType.CollectItem:
				result = new AchievementCollectItem(data, progress);
				break;
			case AchievementClassType.CollectCompagnSecret:
				result = new AchievementCollectCompagnSecret(data, progress);
				break;
			case AchievementClassType.Jump:
				result = new AchievementJump(data, progress);
				break;
			case AchievementClassType.KillInvisiblePlayer:
				result = new AchievementKillInvisiblePlayer(data, progress);
				break;
			case AchievementClassType.KillPlayerOfAllWeaponCategories:
				result = new AchievementKillPlayerOfAllWeaponCategories(data, progress);
				break;
			case AchievementClassType.TurretKill:
				result = new AchievementTurretKill(data, progress);
				break;
			case AchievementClassType.KillAtFly:
				result = new AchievementKillAtFly(data, progress);
				break;
			case AchievementClassType.KillPlayerWhenHpEqualsOne:
				result = new AchievementKillPlayerWhenHpEqualsOne(data, progress);
				break;
			case AchievementClassType.RemainArenaWaves:
				result = new AchievementRemainArenaWaves(data, progress);
				break;
			case AchievementClassType.JetPackFlying:
				result = new AchievementJetPackFlying(data, progress);
				break;
			case AchievementClassType.MechKillPlayers:
				result = new AchievementMechKillPlayers(data, progress);
				break;
			case AchievementClassType.Pacifist:
				result = new AchievementPacifist(data, progress);
				break;
			case AchievementClassType.Shooting:
				result = new AchievementShooting(data, progress);
				break;
			case AchievementClassType.ReturnAfterDays:
				result = new AchievementReturnAfterDays(data, progress);
				break;
			case AchievementClassType.CollectPets:
				result = new AchievementCollectPets(data, progress);
				break;
			case AchievementClassType.QuestsComplited:
				result = new AchievementQuestsComplited(data, progress);
				break;
			case AchievementClassType.ExistsGadgetsInAllCategories:
				result = new AchievementExistsGadgetsInAllCategories(data, progress);
				break;
			case AchievementClassType.Resurection:
				result = new AchievementResurection(data, progress);
				break;
			case AchievementClassType.EggsHatched:
				result = new AchievementEggsHatched(data, progress);
				break;
			case AchievementClassType.DemonKillMech:
				result = new AchievementDemonKillMech(data, progress);
				break;
			case AchievementClassType.GetItem:
				result = new AchievementGetItem(data, progress);
				break;
			default:
				Debug.LogErrorFormat("unsupported AchievementClassType : '{0}'", new object[]
				{
					data.ClassType
				});
				break;
			}
			return result;
		}

		// Token: 0x06002DA5 RID: 11685 RVA: 0x000F027C File Offset: 0x000EE47C
		private void OnAchievementProgressChanged(Achievement ach, bool pointsChanged, bool stageChanged)
		{
			if (!this._achievementsAll.Contains(ach))
			{
				Debug.LogErrorFormat("[Achievement] Unknown achievement: '{0}'", new object[]
				{
					ach.Id
				});
				return;
			}
			if (stageChanged)
			{
				if (!this._achievements.Contains(ach))
				{
					this._achievements.Add(ach, false);
				}
				string text = LocalizationStore.Get(ach.Data.LKeyName);
				Texture bgTexture = AchievementView.BackgroundTextureFor(ach);
				InfoWindowController.ShowAchievementsBox(text, bgTexture, ach.Data.Icon);
				AnalyticsStuff.LogAchievementEarned(ach.Id, ach.Stage);
			}
			else if (ach.Points > 0 && !this._achievements.Contains(ach) && ach.Type != AchievementType.Hidden)
			{
				List<Achievement> list = this.GetAchievementsToShow().ToList<Achievement>();
				int num = list.IndexOf(ach);
				if (num > -1)
				{
					this._achievements.Insert(num, ach, false);
				}
			}
		}

		// Token: 0x06002DA6 RID: 11686 RVA: 0x000F0370 File Offset: 0x000EE570
		private IEnumerable<Achievement> GetAchievementsToShow()
		{
			IEnumerable<Achievement> source = from a in this._achievementsAll
			where a.Type == AchievementType.Common || (a.Points > 0 && a.Type == AchievementType.Openable) || (a.Stage > 0 && a.Type == AchievementType.Hidden)
			select a;
			return from a in source
			orderby a.Type, a.Data.GroupId, a.Id
			select a;
		}

		// Token: 0x06002DA7 RID: 11687 RVA: 0x000F0410 File Offset: 0x000EE610
		public void SetProgress(AchievementProgressData pData)
		{
			Achievement achievement = this._achievementsAll.FirstOrDefault((Achievement a) => a.Id == pData.AchievementId);
			if (achievement != null)
			{
				achievement.ProgressData = pData;
			}
		}

		// Token: 0x06002DA8 RID: 11688 RVA: 0x000F0454 File Offset: 0x000EE654
		private void OnApplicationPause(bool b)
		{
			if (b)
			{
				this.SaveProgresses();
			}
		}

		// Token: 0x06002DA9 RID: 11689 RVA: 0x000F0464 File Offset: 0x000EE664
		private void OnSceneLoading(SceneLoadInfo li)
		{
			this.SaveProgresses();
		}

		// Token: 0x06002DAA RID: 11690 RVA: 0x000F046C File Offset: 0x000EE66C
		public void SaveProgresses()
		{
			List<Dictionary<string, object>> obj = (from a in this._achievementsAll
			select a.ProgressData.ObjectForSave()).ToList<Dictionary<string, object>>();
			string val = Json.Serialize(obj);
			Storager.setString("achievementsProgress", val, false);
		}

		// Token: 0x0400221C RID: 8732
		private const string KEY_PROGRESSES = "achievementsProgress";

		// Token: 0x0400221D RID: 8733
		private readonly List<Achievement> _achievementsAll = new List<Achievement>();

		// Token: 0x0400221E RID: 8734
		private readonly ObservableList<Achievement> _achievements = new ObservableList<Achievement>();

		// Token: 0x0400221F RID: 8735
		public static Awaiter Awaiter = new Awaiter();
	}
}
