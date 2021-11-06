using System;

namespace Rilisoft
{
	// Token: 0x02000514 RID: 1300
	public class AchievementDemonKillMech : AchievementBindedToMyPlayer
	{
		// Token: 0x06002D31 RID: 11569 RVA: 0x000EE0A4 File Offset: 0x000EC2A4
		public AchievementDemonKillMech(AchievementData data, AchievementProgressData progressData) : base(data, progressData)
		{
			if (base.IsCompleted)
			{
				return;
			}
		}

		// Token: 0x06002D32 RID: 11570 RVA: 0x000EE0BC File Offset: 0x000EC2BC
		protected override void OnPlayerInstanceSetted()
		{
			if (base.MyPlayer != null)
			{
				base.MyPlayer.OnMyKillMechInDemon += this.MyPlayer_OnMyKillMechInDemon;
			}
		}

		// Token: 0x06002D33 RID: 11571 RVA: 0x000EE0F4 File Offset: 0x000EC2F4
		private void MyPlayer_OnMyKillMechInDemon()
		{
			if (base.IsCompleted)
			{
				return;
			}
			base.Gain(1);
		}

		// Token: 0x06002D34 RID: 11572 RVA: 0x000EE10C File Offset: 0x000EC30C
		public override void Dispose()
		{
			base.Dispose();
			if (base.MyPlayer != null)
			{
				base.MyPlayer.OnMyKillMechInDemon -= this.MyPlayer_OnMyKillMechInDemon;
			}
		}
	}
}
