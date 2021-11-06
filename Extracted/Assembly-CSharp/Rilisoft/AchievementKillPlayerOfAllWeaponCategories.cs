using System;
using System.Linq;

namespace Rilisoft
{
	// Token: 0x02000504 RID: 1284
	public class AchievementKillPlayerOfAllWeaponCategories : Achievement
	{
		// Token: 0x06002CF4 RID: 11508 RVA: 0x000ED684 File Offset: 0x000EB884
		public AchievementKillPlayerOfAllWeaponCategories(AchievementData data, AchievementProgressData progressData) : base(data, progressData)
		{
			if (base.IsCompleted)
			{
				return;
			}
			QuestMediator.Events.KillOtherPlayer += this.QuestMediator_Events_KillOtherPlayer;
		}

		// Token: 0x06002CF5 RID: 11509 RVA: 0x000ED6F8 File Offset: 0x000EB8F8
		private void QuestMediator_Events_KillOtherPlayer(object sender, KillOtherPlayerEventArgs e)
		{
			if (!base.IsActive)
			{
				return;
			}
			if (base.Progress.CustomDataExists(e.WeaponSlot.ToString()))
			{
				return;
			}
			base.Progress.CustomDataSet(e.WeaponSlot.ToString(), null);
			if (this._allGameModes.All((string gm) => base.Progress.CustomDataExists(gm)))
			{
				base.Gain(1);
			}
		}

		// Token: 0x06002CF6 RID: 11510 RVA: 0x000ED774 File Offset: 0x000EB974
		public override void Dispose()
		{
			QuestMediator.Events.KillOtherPlayer -= this.QuestMediator_Events_KillOtherPlayer;
		}

		// Token: 0x040021B5 RID: 8629
		private readonly string[] _allGameModes = new string[]
		{
			"PrimaryCategory",
			"BackupCategory",
			"MeleeCategory",
			"SpecilCategory",
			"SniperCategory",
			"PremiumCategory"
		};
	}
}
