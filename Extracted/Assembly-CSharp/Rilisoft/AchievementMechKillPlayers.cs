using System;
using System.Collections;

namespace Rilisoft
{
	// Token: 0x0200050A RID: 1290
	public class AchievementMechKillPlayers : Achievement
	{
		// Token: 0x06002D07 RID: 11527 RVA: 0x000ED9BC File Offset: 0x000EBBBC
		public AchievementMechKillPlayers(AchievementData data, AchievementProgressData progressData) : base(data, progressData)
		{
			QuestMediator.Events.KillOtherPlayer += this.QuestMediator_Events_KillOtherPlayer;
		}

		// Token: 0x06002D08 RID: 11528 RVA: 0x000ED9DC File Offset: 0x000EBBDC
		private void QuestMediator_Events_KillOtherPlayer(object sender, KillOtherPlayerEventArgs e)
		{
			if (WeaponManager.sharedManager.myPlayerMoveC.isMechActive)
			{
				this._killedCounter++;
				if (!this._meshStateListen)
				{
					this._meshStateListen = true;
					AchievementsManager.Awaiter.Register(this.WaitMechOff());
				}
			}
		}

		// Token: 0x06002D09 RID: 11529 RVA: 0x000EDA30 File Offset: 0x000EBC30
		private IEnumerator WaitMechOff()
		{
			while (WeaponManager.sharedManager != null && WeaponManager.sharedManager.myPlayerMoveC && WeaponManager.sharedManager.myPlayerMoveC.isMechActive)
			{
				yield return null;
			}
			this._meshStateListen = false;
			if (this._killedCounter >= base.ToNextStagePointsLeft)
			{
				int stageIdx = base.MaxStageForPoints(this._killedCounter);
				if (stageIdx > -1)
				{
					base.SetProgress(base.PointsToStage(stageIdx));
				}
			}
			this._killedCounter = 0;
			yield break;
		}

		// Token: 0x06002D0A RID: 11530 RVA: 0x000EDA4C File Offset: 0x000EBC4C
		public override void Dispose()
		{
			QuestMediator.Events.KillOtherPlayer -= this.QuestMediator_Events_KillOtherPlayer;
		}

		// Token: 0x040021B7 RID: 8631
		private bool _meshStateListen;

		// Token: 0x040021B8 RID: 8632
		private int _killedCounter;
	}
}
