using System;

namespace Rilisoft
{
	// Token: 0x0200050D RID: 1293
	public class AchievementReturnAfterDays : Achievement
	{
		// Token: 0x06002D14 RID: 11540 RVA: 0x000EDC5C File Offset: 0x000EBE5C
		public AchievementReturnAfterDays(AchievementData data, AchievementProgressData progressData) : base(data, progressData)
		{
			if (base.IsCompleted)
			{
				return;
			}
			this.FriendsController_ServerTimeUpdated();
			FriendsController.ServerTimeUpdated += this.FriendsController_ServerTimeUpdated;
		}

		// Token: 0x06002D15 RID: 11541 RVA: 0x000EDC94 File Offset: 0x000EBE94
		private void FriendsController_ServerTimeUpdated()
		{
			long serverTime = FriendsController.ServerTime;
			if (serverTime < 1L)
			{
				return;
			}
			object obj = base.Progress.CustomDataGet("lv");
			if (obj == null)
			{
				obj = serverTime;
				base.Progress.CustomDataSet("lv", obj);
				return;
			}
			base.Progress.CustomDataSet("lv", serverTime);
			long num = serverTime - (long)obj;
			if (num < 1L)
			{
				return;
			}
			int num2 = (int)(num / 86400L);
			if (base.ToNextStagePoints > 0 && num2 >= base.ToNextStagePoints)
			{
				base.SetProgress(base.ToNextStagePoints);
			}
		}

		// Token: 0x06002D16 RID: 11542 RVA: 0x000EDD38 File Offset: 0x000EBF38
		public override void Dispose()
		{
			FriendsController.ServerTimeUpdated -= this.FriendsController_ServerTimeUpdated;
		}

		// Token: 0x040021BA RID: 8634
		private const string LAST_VISIT_KEY = "lv";
	}
}
