using System;

namespace Rilisoft
{
	// Token: 0x02000503 RID: 1283
	public class AchievementKillInvisiblePlayer : Achievement
	{
		// Token: 0x06002CF1 RID: 11505 RVA: 0x000ED638 File Offset: 0x000EB838
		public AchievementKillInvisiblePlayer(AchievementData data, AchievementProgressData progressData) : base(data, progressData)
		{
			QuestMediator.Events.KillOtherPlayer += this.QuestMediator_Events_KillOtherPlayer;
		}

		// Token: 0x06002CF2 RID: 11506 RVA: 0x000ED658 File Offset: 0x000EB858
		private void QuestMediator_Events_KillOtherPlayer(object sender, KillOtherPlayerEventArgs e)
		{
			if (e.IsInvisible)
			{
				base.Gain(1);
			}
		}

		// Token: 0x06002CF3 RID: 11507 RVA: 0x000ED66C File Offset: 0x000EB86C
		public override void Dispose()
		{
			QuestMediator.Events.KillOtherPlayer -= this.QuestMediator_Events_KillOtherPlayer;
		}
	}
}
