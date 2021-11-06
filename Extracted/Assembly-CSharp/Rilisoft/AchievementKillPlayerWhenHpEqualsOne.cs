using System;
using UnityEngine;

namespace Rilisoft
{
	// Token: 0x02000507 RID: 1287
	public class AchievementKillPlayerWhenHpEqualsOne : Achievement
	{
		// Token: 0x06002CFE RID: 11518 RVA: 0x000ED838 File Offset: 0x000EBA38
		public AchievementKillPlayerWhenHpEqualsOne(AchievementData data, AchievementProgressData progressData) : base(data, progressData)
		{
			if (base.IsCompleted)
			{
				return;
			}
			QuestMediator.Events.KillOtherPlayer += this.QuestMediator_Events_KillOtherPlayer;
		}

		// Token: 0x06002CFF RID: 11519 RVA: 0x000ED870 File Offset: 0x000EBA70
		private void QuestMediator_Events_KillOtherPlayer(object sender, KillOtherPlayerEventArgs e)
		{
			if (WeaponManager.sharedManager != null && WeaponManager.sharedManager.myPlayerMoveC != null && Mathf.RoundToInt(WeaponManager.sharedManager.myPlayerMoveC.CurHealth) == 1)
			{
				base.Gain(1);
			}
		}

		// Token: 0x06002D00 RID: 11520 RVA: 0x000ED8C4 File Offset: 0x000EBAC4
		public override void Dispose()
		{
			QuestMediator.Events.KillOtherPlayer -= this.QuestMediator_Events_KillOtherPlayer;
		}
	}
}
