using System;
using UnityEngine;

namespace Rilisoft
{
	// Token: 0x020005AA RID: 1450
	public class PrefsBoolCachedProperty : CachedPropertyWithKeyBase<bool>
	{
		// Token: 0x06003250 RID: 12880 RVA: 0x0010585C File Offset: 0x00103A5C
		public PrefsBoolCachedProperty(string prefsKey) : base(prefsKey)
		{
		}

		// Token: 0x06003251 RID: 12881 RVA: 0x00105868 File Offset: 0x00103A68
		protected override bool GetValue()
		{
			return PlayerPrefs.GetInt(base.PrefsKey, 0).ToBool();
		}

		// Token: 0x06003252 RID: 12882 RVA: 0x0010587C File Offset: 0x00103A7C
		protected override void SetValue(bool value)
		{
			PlayerPrefs.SetInt(base.PrefsKey, value.ToInt());
		}
	}
}
