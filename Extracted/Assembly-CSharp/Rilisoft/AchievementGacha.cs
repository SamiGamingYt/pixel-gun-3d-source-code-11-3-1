using System;

namespace Rilisoft
{
	// Token: 0x020004F9 RID: 1273
	public class AchievementGacha : Achievement
	{
		// Token: 0x06002CD1 RID: 11473 RVA: 0x000ECD2C File Offset: 0x000EAF2C
		public AchievementGacha(AchievementData data, AchievementProgressData progressData) : base(data, progressData)
		{
			QuestMediator.Events.GetGotcha += delegate(object sender, EventArgs e)
			{
				base.Gain(1);
			};
		}
	}
}
