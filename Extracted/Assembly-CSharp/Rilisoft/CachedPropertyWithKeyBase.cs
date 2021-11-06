using System;

namespace Rilisoft
{
	// Token: 0x020005A9 RID: 1449
	public abstract class CachedPropertyWithKeyBase<T> : CachedProperty<T>
	{
		// Token: 0x0600324D RID: 12877 RVA: 0x00105838 File Offset: 0x00103A38
		protected CachedPropertyWithKeyBase(string prefsKey)
		{
			this.PrefsKey = prefsKey;
		}

		// Token: 0x1700086C RID: 2156
		// (get) Token: 0x0600324E RID: 12878 RVA: 0x00105848 File Offset: 0x00103A48
		// (set) Token: 0x0600324F RID: 12879 RVA: 0x00105850 File Offset: 0x00103A50
		public string PrefsKey { get; protected set; }
	}
}
