using System;

namespace Rilisoft
{
	// Token: 0x020004F7 RID: 1271
	public class AchievementWin : Achievement
	{
		// Token: 0x06002CCD RID: 11469 RVA: 0x000ECC70 File Offset: 0x000EAE70
		public AchievementWin(AchievementData data, AchievementProgressData progressData) : base(data, progressData)
		{
			QuestMediator.Events.Win += delegate(object sender, WinEventArgs e)
			{
				base.Gain(1);
			};
		}
	}
}
