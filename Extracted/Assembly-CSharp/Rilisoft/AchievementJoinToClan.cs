using System;

namespace Rilisoft
{
	// Token: 0x020004FC RID: 1276
	public class AchievementJoinToClan : Achievement
	{
		// Token: 0x06002CD9 RID: 11481 RVA: 0x000ECF68 File Offset: 0x000EB168
		public AchievementJoinToClan(AchievementData data, AchievementProgressData progressData) : base(data, progressData)
		{
			FriendsController.OnClanIdSetted += this.FriendsController_OnClanIdSetted;
		}

		// Token: 0x06002CDA RID: 11482 RVA: 0x000ECF84 File Offset: 0x000EB184
		private void FriendsController_OnClanIdSetted(string obj)
		{
			if (base.Progress.Points < base.PointsLeft && !obj.IsNullOrEmpty())
			{
				base.Gain(1);
			}
		}

		// Token: 0x06002CDB RID: 11483 RVA: 0x000ECFBC File Offset: 0x000EB1BC
		public override void Dispose()
		{
			FriendsController.OnClanIdSetted -= this.FriendsController_OnClanIdSetted;
		}
	}
}
