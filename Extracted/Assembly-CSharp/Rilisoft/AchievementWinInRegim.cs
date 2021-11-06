using System;
using UnityEngine;

namespace Rilisoft
{
	// Token: 0x020004F8 RID: 1272
	public class AchievementWinInRegim : Achievement
	{
		// Token: 0x06002CCF RID: 11471 RVA: 0x000ECC9C File Offset: 0x000EAE9C
		public AchievementWinInRegim(AchievementData data, AchievementProgressData progressData) : base(data, progressData)
		{
			QuestMediator.Events.Win += delegate(object sender, WinEventArgs e)
			{
				if (base._data.RegimGame == null)
				{
					Debug.LogErrorFormat("achievement '{0}' without value", new object[]
					{
						base._data.Id
					});
					return;
				}
				if (e.Mode == base._data.RegimGame.Value)
				{
					base.Gain(1);
				}
			};
		}
	}
}
