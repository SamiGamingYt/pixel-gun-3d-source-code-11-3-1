using System;
using System.Collections;
using UnityEngine;

namespace Rilisoft
{
	// Token: 0x0200050B RID: 1291
	public class AchievementPacifist : Achievement
	{
		// Token: 0x06002D0B RID: 11531 RVA: 0x000EDA64 File Offset: 0x000EBC64
		public AchievementPacifist(AchievementData data, AchievementProgressData progressData) : base(data, progressData)
		{
			if (base.IsCompleted)
			{
				return;
			}
			Player_move_c.OnMyPlayerMoveCCreated += this.Player_move_c_OnMyPlayerMoveCCreated;
			Player_move_c.OnMyPlayerMoveCDestroyed += this.Player_move_c_OnMyPlayerMoveCDestroyed;
			Player_move_c.OnMyShootingStateSchanged += this.Player_move_c_OnMyShootingStateSchanged;
		}

		// Token: 0x06002D0C RID: 11532 RVA: 0x000EDAB8 File Offset: 0x000EBCB8
		private void Player_move_c_OnMyPlayerMoveCCreated()
		{
			if (Defs.isHunger || Defs.isDaterRegim || !Defs.isMulti)
			{
				return;
			}
			AchievementsManager.Awaiter.Register(this.Update());
		}

		// Token: 0x06002D0D RID: 11533 RVA: 0x000EDAEC File Offset: 0x000EBCEC
		private IEnumerator Update()
		{
			if (base.IsCompleted)
			{
				yield break;
			}
			Player_move_c playerMoveC = WeaponManager.sharedManager.myPlayerMoveC;
			while (!this.isShooting)
			{
				if (playerMoveC == null)
				{
					yield break;
				}
				if (playerMoveC.liveTime >= (float)base.ToNextStagePointsLeft)
				{
					int stageIdx = base.MaxStageForPoints(Mathf.RoundToInt(playerMoveC.liveTime));
					if (stageIdx > -1)
					{
						base.SetProgress(base.PointsToStage(stageIdx));
					}
					if (base.IsCompleted)
					{
						yield break;
					}
				}
				yield return null;
			}
			yield break;
			yield break;
		}

		// Token: 0x06002D0E RID: 11534 RVA: 0x000EDB08 File Offset: 0x000EBD08
		private void Player_move_c_OnMyPlayerMoveCDestroyed(float liveTime)
		{
			this.isShooting = false;
		}

		// Token: 0x06002D0F RID: 11535 RVA: 0x000EDB14 File Offset: 0x000EBD14
		private void Player_move_c_OnMyShootingStateSchanged(bool obj)
		{
			AchievementsManager.Awaiter.Remove(this.Update());
			this.isShooting = obj;
		}

		// Token: 0x06002D10 RID: 11536 RVA: 0x000EDB30 File Offset: 0x000EBD30
		public override void Dispose()
		{
			Player_move_c.OnMyPlayerMoveCCreated -= this.Player_move_c_OnMyPlayerMoveCCreated;
			Player_move_c.OnMyPlayerMoveCDestroyed -= this.Player_move_c_OnMyPlayerMoveCDestroyed;
			Player_move_c.OnMyShootingStateSchanged -= this.Player_move_c_OnMyShootingStateSchanged;
		}

		// Token: 0x040021B9 RID: 8633
		private bool isShooting;
	}
}
