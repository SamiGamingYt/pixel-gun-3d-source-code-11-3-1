using System;
using UnityEngine;

namespace I2.Loc
{
	// Token: 0x020002C8 RID: 712
	[AddComponentMenu("I2/Localization/SetLanguage")]
	public class SetLanguage : MonoBehaviour
	{
		// Token: 0x060018BF RID: 6335 RVA: 0x0005D174 File Offset: 0x0005B374
		private void OnClick()
		{
			this.ApplyLanguage();
		}

		// Token: 0x060018C0 RID: 6336 RVA: 0x0005D17C File Offset: 0x0005B37C
		public void ApplyLanguage()
		{
			if (LocalizationManager.HasLanguage(this._Language, true))
			{
				LocalizationManager.CurrentLanguage = this._Language;
			}
		}

		// Token: 0x04000D34 RID: 3380
		public string _Language;
	}
}
