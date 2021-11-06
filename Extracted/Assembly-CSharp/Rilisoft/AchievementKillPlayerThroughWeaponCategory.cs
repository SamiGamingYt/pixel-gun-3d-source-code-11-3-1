using System;
using UnityEngine;

namespace Rilisoft
{
	// Token: 0x020004F5 RID: 1269
	public class AchievementKillPlayerThroughWeaponCategory : Achievement
	{
		// Token: 0x06002CC9 RID: 11465 RVA: 0x000ECBA0 File Offset: 0x000EADA0
		public AchievementKillPlayerThroughWeaponCategory(AchievementData data, AchievementProgressData progressData) : base(data, progressData)
		{
			if (base._data.WeaponCategory == null)
			{
				Debug.LogErrorFormat("achievement '{0}' without value", new object[]
				{
					base._data.Id
				});
				return;
			}
			QuestMediator.Events.KillOtherPlayer += delegate(object sender, KillOtherPlayerEventArgs e)
			{
				if (e.WeaponSlot == base._data.WeaponCategory.Value)
				{
					base.Gain(1);
				}
			};
		}
	}
}
