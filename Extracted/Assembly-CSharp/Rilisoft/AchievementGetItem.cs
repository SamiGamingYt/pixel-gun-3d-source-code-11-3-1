using System;
using UnityEngine;

namespace Rilisoft
{
	// Token: 0x020004FF RID: 1279
	public class AchievementGetItem : Achievement
	{
		// Token: 0x06002CE3 RID: 11491 RVA: 0x000ED1B8 File Offset: 0x000EB3B8
		public AchievementGetItem(AchievementData data, AchievementProgressData progressData) : base(data, progressData)
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

		// Token: 0x06002CE4 RID: 11492 RVA: 0x000ED230 File Offset: 0x000EB430
		private void OnStoragerKeyChanged()
		{
			int @int = Storager.getInt(base.Data.ItemId, false);
			if (@int < 1)
			{
				this._prevVal = @int;
				return;
			}
			if (this._prevVal < 0)
			{
				if (base.Points < @int)
				{
					base.Gain(@int - base.Points);
				}
			}
			else if (@int > this._prevVal)
			{
				base.Gain(@int - this._prevVal);
			}
			this._prevVal = @int;
		}

		// Token: 0x06002CE5 RID: 11493 RVA: 0x000ED2AC File Offset: 0x000EB4AC
		public override void Dispose()
		{
			Storager.UnSubscribeToChanged(base._data.ItemId, new Action(this.OnStoragerKeyChanged));
		}

		// Token: 0x040021AE RID: 8622
		private int _prevVal = -1;
	}
}
