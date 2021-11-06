using System;
using UnityEngine;

namespace Rilisoft
{
	// Token: 0x020004FE RID: 1278
	public class AchievementCollectItem : Achievement
	{
		// Token: 0x06002CE0 RID: 11488 RVA: 0x000ED0C0 File Offset: 0x000EB2C0
		public AchievementCollectItem(AchievementData data, AchievementProgressData progressData) : base(data, progressData)
		{
			if (base._data.ItemId.IsNullOrEmpty())
			{
				Debug.LogErrorFormat("achievement '{0}' without value", new object[]
				{
					base._data.Id
				});
				return;
			}
			this.OnStoragerKeyChanged();
			Storager.SubscribeToChanged(base._data.ItemId, new Action(this.OnStoragerKeyChanged));
		}

		// Token: 0x06002CE1 RID: 11489 RVA: 0x000ED130 File Offset: 0x000EB330
		private void OnStoragerKeyChanged()
		{
			int @int = Storager.getInt(base.Data.ItemId, false);
			if (base.Stage > 0)
			{
				int num = base._data.Thresholds[base.Stage - 1];
				if (@int >= num)
				{
					base.SetProgress(@int);
				}
			}
			else if (base.Points != @int)
			{
				base.SetProgress(@int);
			}
		}

		// Token: 0x06002CE2 RID: 11490 RVA: 0x000ED198 File Offset: 0x000EB398
		public override void Dispose()
		{
			Storager.UnSubscribeToChanged(base._data.ItemId, new Action(this.OnStoragerKeyChanged));
		}
	}
}
