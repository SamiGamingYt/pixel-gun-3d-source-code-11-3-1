using System;
using System.Linq;

namespace Rilisoft
{
	// Token: 0x02000508 RID: 1288
	public class AchievementRemainArenaWaves : Achievement
	{
		// Token: 0x06002D01 RID: 11521 RVA: 0x000ED8DC File Offset: 0x000EBADC
		public AchievementRemainArenaWaves(AchievementData data, AchievementProgressData progressData) : base(data, progressData)
		{
			QuestMediator.Events.SurviveWaveInArena += this.QuestMediator_Events_SurviveWaveInArena;
		}

		// Token: 0x06002D02 RID: 11522 RVA: 0x000ED8FC File Offset: 0x000EBAFC
		private void QuestMediator_Events_SurviveWaveInArena(object sender, SurviveWaveInArenaEventArgs e)
		{
			if (base._data.Thresholds.Contains(e.WaveNumber))
			{
				int stageIdx = base._data.Thresholds.ToList<int>().IndexOf(e.WaveNumber);
				int num = base.PointsToStage(stageIdx);
				if (num > 0)
				{
					base.Gain(num);
				}
			}
		}

		// Token: 0x06002D03 RID: 11523 RVA: 0x000ED958 File Offset: 0x000EBB58
		public override void Dispose()
		{
			QuestMediator.Events.SurviveWaveInArena -= this.QuestMediator_Events_SurviveWaveInArena;
		}
	}
}
