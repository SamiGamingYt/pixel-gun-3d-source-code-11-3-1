using System;

namespace Rilisoft
{
	// Token: 0x020005AE RID: 1454
	public abstract class StoragerCachedPropertyBase<T> : CachedPropertyWithKeyBase<T>
	{
		// Token: 0x0600325C RID: 12892 RVA: 0x0010591C File Offset: 0x00103B1C
		protected StoragerCachedPropertyBase(string prefsKey, bool useICloud = false) : base(prefsKey)
		{
			this.UseICloud = useICloud;
		}

		// Token: 0x1700086D RID: 2157
		// (get) Token: 0x0600325D RID: 12893 RVA: 0x0010592C File Offset: 0x00103B2C
		// (set) Token: 0x0600325E RID: 12894 RVA: 0x00105934 File Offset: 0x00103B34
		public bool UseICloud { get; private set; }
	}
}
