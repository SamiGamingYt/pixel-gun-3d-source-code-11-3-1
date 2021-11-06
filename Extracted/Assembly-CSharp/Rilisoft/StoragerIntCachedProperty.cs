using System;

namespace Rilisoft
{
	// Token: 0x020005B0 RID: 1456
	public class StoragerIntCachedProperty : StoragerCachedPropertyBase<int>
	{
		// Token: 0x06003262 RID: 12898 RVA: 0x00105988 File Offset: 0x00103B88
		public StoragerIntCachedProperty(string prefsKey, bool useICloud = false) : base(prefsKey, useICloud)
		{
		}

		// Token: 0x06003263 RID: 12899 RVA: 0x00105994 File Offset: 0x00103B94
		protected override int GetValue()
		{
			return Storager.getInt(base.PrefsKey, base.UseICloud);
		}

		// Token: 0x06003264 RID: 12900 RVA: 0x001059A8 File Offset: 0x00103BA8
		protected override void SetValue(int value)
		{
			Storager.setInt(base.PrefsKey, value, base.UseICloud);
		}
	}
}
