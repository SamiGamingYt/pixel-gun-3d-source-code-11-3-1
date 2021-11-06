using System;
using UnityEngine;

namespace Rilisoft
{
	// Token: 0x020005B1 RID: 1457
	public class StoragerStringCachedProperty : StoragerCachedPropertyBase<string>
	{
		// Token: 0x06003265 RID: 12901 RVA: 0x001059BC File Offset: 0x00103BBC
		public StoragerStringCachedProperty(string prefsKey, bool useICloud = false) : base(prefsKey, useICloud)
		{
		}

		// Token: 0x06003266 RID: 12902 RVA: 0x001059C8 File Offset: 0x00103BC8
		protected override string GetValue()
		{
			if (Application.platform == RuntimePlatform.IPhonePlayer && !Storager.hasKey(base.PrefsKey))
			{
				Storager.setString(base.PrefsKey, string.Empty, base.UseICloud);
			}
			return Storager.getString(base.PrefsKey, base.UseICloud);
		}

		// Token: 0x06003267 RID: 12903 RVA: 0x00105A18 File Offset: 0x00103C18
		protected override void SetValue(string value)
		{
			Storager.setString(base.PrefsKey, value, base.UseICloud);
		}
	}
}
