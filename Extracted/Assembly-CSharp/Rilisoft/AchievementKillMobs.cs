using System;

namespace Rilisoft
{
	// Token: 0x020004F3 RID: 1267
	public class AchievementKillMobs : Achievement
	{
		// Token: 0x06002CC5 RID: 11461 RVA: 0x000ECB48 File Offset: 0x000EAD48
		public AchievementKillMobs(AchievementData data, AchievementProgressData progressData) : base(data, progressData)
		{
			QuestMediator.Events.KillMonster += delegate(object sender, KillMonsterEventArgs e)
			{
				base.Gain(1);
			};
		}
	}
}
