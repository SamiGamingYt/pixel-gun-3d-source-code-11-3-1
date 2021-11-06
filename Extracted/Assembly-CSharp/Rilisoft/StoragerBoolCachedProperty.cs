using System;

namespace Rilisoft
{
	// Token: 0x020005AF RID: 1455
	public class StoragerBoolCachedProperty : StoragerCachedPropertyBase<bool>
	{
		// Token: 0x0600325F RID: 12895 RVA: 0x00105940 File Offset: 0x00103B40
		public StoragerBoolCachedProperty(string prefsKey, bool useICloud = false) : base(prefsKey, useICloud)
		{
		}

		// Token: 0x06003260 RID: 12896 RVA: 0x0010594C File Offset: 0x00103B4C
		protected override bool GetValue()
		{
			return Storager.getInt(base.PrefsKey, base.UseICloud).ToBool();
		}

		// Token: 0x06003261 RID: 12897 RVA: 0x00105964 File Offset: 0x00103B64
		protected override void SetValue(bool value)
		{
			Storager.setInt(base.PrefsKey, value.ToInt(), base.UseICloud);
		}
	}
}
