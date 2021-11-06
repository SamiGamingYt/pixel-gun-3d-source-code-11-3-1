using System;

namespace Rilisoft
{
	// Token: 0x02000505 RID: 1285
	public class AchievementTurretKill : Achievement
	{
		// Token: 0x06002CF8 RID: 11512 RVA: 0x000ED79C File Offset: 0x000EB99C
		public AchievementTurretKill(AchievementData data, AchievementProgressData progressData) : base(data, progressData)
		{
			QuestMediator.Events.TurretKill += this.QuestMediator_Events_TurretKill;
		}

		// Token: 0x06002CF9 RID: 11513 RVA: 0x000ED7BC File Offset: 0x000EB9BC
		private void QuestMediator_Events_TurretKill(object sender, EventArgs e)
		{
			base.Gain(1);
		}

		// Token: 0x06002CFA RID: 11514 RVA: 0x000ED7C8 File Offset: 0x000EB9C8
		public override void Dispose()
		{
			QuestMediator.Events.TurretKill -= this.QuestMediator_Events_TurretKill;
		}
	}
}
