using System;

namespace Rilisoft
{
	// Token: 0x02000502 RID: 1282
	public class AchievementJump : Achievement
	{
		// Token: 0x06002CEE RID: 11502 RVA: 0x000ED5F4 File Offset: 0x000EB7F4
		public AchievementJump(AchievementData data, AchievementProgressData progressData) : base(data, progressData)
		{
			QuestMediator.Events.Jump += this.QuestMediator_Events_Jump;
		}

		// Token: 0x06002CEF RID: 11503 RVA: 0x000ED614 File Offset: 0x000EB814
		private void QuestMediator_Events_Jump(object sender, EventArgs e)
		{
			base.Gain(1);
		}

		// Token: 0x06002CF0 RID: 11504 RVA: 0x000ED620 File Offset: 0x000EB820
		public override void Dispose()
		{
			QuestMediator.Events.Jump -= this.QuestMediator_Events_Jump;
		}
	}
}
