using System;
using UnityEngine;

namespace Rilisoft
{
	// Token: 0x020004FA RID: 1274
	public class AchievementGetCurrency : Achievement
	{
		// Token: 0x06002CD3 RID: 11475 RVA: 0x000ECD58 File Offset: 0x000EAF58
		public AchievementGetCurrency(AchievementData data, AchievementProgressData progressData) : base(data, progressData)
		{
			if (base._data.Currency.IsNullOrEmpty() || (base._data.Currency != "Coins" && base._data.Currency != "GemsCurrency"))
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

		// Token: 0x06002CD4 RID: 11476 RVA: 0x000ECE04 File Offset: 0x000EB004
		private void OnCurrencyChanged()
		{
			int @int = Storager.getInt(base._data.Currency, false);
			if (this._prevValue == -1)
			{
				this._prevValue = @int;
			}
			int num = @int - this._prevValue;
			if (num > 0)
			{
				base.Gain(num);
			}
			this._prevValue = @int;
		}

		// Token: 0x06002CD5 RID: 11477 RVA: 0x000ECE54 File Offset: 0x000EB054
		public override void Dispose()
		{
			Storager.UnSubscribeToChanged(base._data.Currency, new Action(this.OnCurrencyChanged));
		}

		// Token: 0x040021AD RID: 8621
		private int _prevValue = -1;
	}
}
