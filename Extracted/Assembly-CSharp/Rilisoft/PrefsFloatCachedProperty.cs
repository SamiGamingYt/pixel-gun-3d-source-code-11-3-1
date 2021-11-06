using System;
using UnityEngine;

namespace Rilisoft
{
	// Token: 0x020005AD RID: 1453
	public class PrefsFloatCachedProperty : CachedPropertyWithKeyBase<float>
	{
		// Token: 0x06003259 RID: 12889 RVA: 0x001058EC File Offset: 0x00103AEC
		public PrefsFloatCachedProperty(string prefsKey) : base(prefsKey)
		{
		}

		// Token: 0x0600325A RID: 12890 RVA: 0x001058F8 File Offset: 0x00103AF8
		protected override float GetValue()
		{
			return PlayerPrefs.GetFloat(base.PrefsKey, 0f);
		}

		// Token: 0x0600325B RID: 12891 RVA: 0x0010590C File Offset: 0x00103B0C
		protected override void SetValue(float value)
		{
			PlayerPrefs.SetFloat(base.PrefsKey, value);
		}
	}
}
