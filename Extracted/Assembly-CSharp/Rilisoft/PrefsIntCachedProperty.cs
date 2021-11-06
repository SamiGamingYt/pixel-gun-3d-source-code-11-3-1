using System;
using UnityEngine;

namespace Rilisoft
{
	// Token: 0x020005AB RID: 1451
	public class PrefsIntCachedProperty : CachedPropertyWithKeyBase<int>
	{
		// Token: 0x06003253 RID: 12883 RVA: 0x00105890 File Offset: 0x00103A90
		public PrefsIntCachedProperty(string prefsKey) : base(prefsKey)
		{
		}

		// Token: 0x06003254 RID: 12884 RVA: 0x0010589C File Offset: 0x00103A9C
		protected override int GetValue()
		{
			return PlayerPrefs.GetInt(base.PrefsKey, 0);
		}

		// Token: 0x06003255 RID: 12885 RVA: 0x001058AC File Offset: 0x00103AAC
		protected override void SetValue(int value)
		{
			PlayerPrefs.SetInt(base.PrefsKey, value);
		}
	}
}
