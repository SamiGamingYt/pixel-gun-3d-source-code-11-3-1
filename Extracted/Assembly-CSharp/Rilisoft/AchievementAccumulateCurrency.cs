using System;
using UnityEngine;

namespace Rilisoft
{
	// Token: 0x020004FB RID: 1275
	public class AchievementAccumulateCurrency : Achievement
	{
		// Token: 0x06002CD6 RID: 11478 RVA: 0x000ECE74 File Offset: 0x000EB074
		public AchievementAccumulateCurrency(AchievementData data, AchievementProgressData progressData) : base(data, progressData)
		{
			if (base._data.Currency != "Coins" && base._data.Currency != "GemsCurrency")
			{
				Debug.LogErrorFormat("achievement '{0}' without value", new object[]
				{
					base._data.Id
				});
				return;
			}
			this.OnCurrencyChanged();
			Storager.SubscribeToChanged(base._data.Currency, new Action(this.OnCurrencyChanged));
		}

		// Token: 0x06002CD7 RID: 11479 RVA: 0x000ECF04 File Offset: 0x000EB104
		private void OnCurrencyChanged()
		{
			int @int = Storager.getInt(base._data.Currency, false);
			if (@int > base.Progress.Points)
			{
				base.Gain(@int - base.Progress.Points);
			}
		}

		// Token: 0x06002CD8 RID: 11480 RVA: 0x000ECF48 File Offset: 0x000EB148
		public override void Dispose()
		{
			Storager.UnSubscribeToChanged(base._data.Currency, new Action(this.OnCurrencyChanged));
		}
	}
}
