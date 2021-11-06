using System;
using UnityEngine;

namespace Rilisoft
{
	// Token: 0x020005AC RID: 1452
	public class PrefsStringCachedProperty : CachedPropertyWithKeyBase<string>
	{
		// Token: 0x06003256 RID: 12886 RVA: 0x001058BC File Offset: 0x00103ABC
		public PrefsStringCachedProperty(string prefsKey) : base(prefsKey)
		{
		}

		// Token: 0x06003257 RID: 12887 RVA: 0x001058C8 File Offset: 0x00103AC8
		protected override string GetValue()
		{
			return PlayerPrefs.GetString(base.PrefsKey, string.Empty);
		}

		// Token: 0x06003258 RID: 12888 RVA: 0x001058DC File Offset: 0x00103ADC
		protected override void SetValue(string value)
		{
			PlayerPrefs.SetString(base.PrefsKey, value);
		}
	}
}
