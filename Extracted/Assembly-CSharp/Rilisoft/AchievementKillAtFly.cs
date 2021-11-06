using System;

namespace Rilisoft
{
	// Token: 0x02000506 RID: 1286
	public class AchievementKillAtFly : Achievement
	{
		// Token: 0x06002CFB RID: 11515 RVA: 0x000ED7E0 File Offset: 0x000EB9E0
		public AchievementKillAtFly(AchievementData data, AchievementProgressData progressData) : base(data, progressData)
		{
			QuestMediator.Events.KillOtherPlayerOnFly += this.QuestMediator_Events_KillOtherPlayerOnFly;
		}

		// Token: 0x06002CFC RID: 11516 RVA: 0x000ED800 File Offset: 0x000EBA00
		private void QuestMediator_Events_KillOtherPlayerOnFly(object sender, KillOtherPlayerOnFlyEventArgs e)
		{
			if (e.IamFly && e.KilledPlayerFly)
			{
				base.Gain(1);
			}
		}

		// Token: 0x06002CFD RID: 11517 RVA: 0x000ED820 File Offset: 0x000EBA20
		public override void Dispose()
		{
			QuestMediator.Events.KillOtherPlayerOnFly -= this.QuestMediator_Events_KillOtherPlayerOnFly;
		}
	}
}
