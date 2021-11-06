using System;
using System.Collections;

namespace Rilisoft
{
	// Token: 0x0200050F RID: 1295
	public class AchievementQuestsComplited : Achievement
	{
		// Token: 0x06002D1C RID: 11548 RVA: 0x000EDE04 File Offset: 0x000EC004
		public AchievementQuestsComplited(AchievementData data, AchievementProgressData progressData) : base(data, progressData)
		{
			AchievementsManager.Awaiter.Register(this.WaitQuestSystem());
		}

		// Token: 0x06002D1D RID: 11549 RVA: 0x000EDE20 File Offset: 0x000EC020
		private IEnumerator WaitQuestSystem()
		{
			while (QuestSystem.Instance == null)
			{
				yield return null;
			}
			QuestSystem.Instance.QuestCompleted += this.QuestSystem_Instance_QuestCompleted;
			yield break;
		}

		// Token: 0x06002D1E RID: 11550 RVA: 0x000EDE3C File Offset: 0x000EC03C
		private void QuestSystem_Instance_QuestCompleted(object sender, QuestCompletedEventArgs e)
		{
			base.Gain(1);
		}

		// Token: 0x06002D1F RID: 11551 RVA: 0x000EDE48 File Offset: 0x000EC048
		public override void Dispose()
		{
			AchievementsManager.Awaiter.Remove(this.WaitQuestSystem());
			if (QuestSystem.Instance != null)
			{
				QuestSystem.Instance.QuestCompleted -= this.QuestSystem_Instance_QuestCompleted;
			}
		}
	}
}
