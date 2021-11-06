using System;

namespace Rilisoft
{
	// Token: 0x020004F6 RID: 1270
	public class AchievementInflictHeadshot : Achievement
	{
		// Token: 0x06002CCB RID: 11467 RVA: 0x000ECC3C File Offset: 0x000EAE3C
		public AchievementInflictHeadshot(AchievementData data, AchievementProgressData progressData) : base(data, progressData)
		{
			QuestMediator.Events.KillOtherPlayer += delegate(object sender, KillOtherPlayerEventArgs e)
			{
				if (e.Headshot)
				{
					base.Gain(1);
				}
			};
		}
	}
}
