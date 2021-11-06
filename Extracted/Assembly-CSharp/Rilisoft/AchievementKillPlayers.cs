using System;

namespace Rilisoft
{
	// Token: 0x020004F4 RID: 1268
	public class AchievementKillPlayers : Achievement
	{
		// Token: 0x06002CC7 RID: 11463 RVA: 0x000ECB74 File Offset: 0x000EAD74
		public AchievementKillPlayers(AchievementData data, AchievementProgressData progressData) : base(data, progressData)
		{
			QuestMediator.Events.KillOtherPlayer += delegate(object sender, KillOtherPlayerEventArgs e)
			{
				base.Gain(1);
			};
		}
	}
}
