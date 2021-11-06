using System;

namespace Rilisoft
{
	// Token: 0x020005A8 RID: 1448
	public abstract class CachedProperty<T>
	{
		// Token: 0x06003246 RID: 12870
		protected abstract T GetValue();

		// Token: 0x06003247 RID: 12871
		protected abstract void SetValue(T value);

		// Token: 0x17000869 RID: 2153
		// (get) Token: 0x06003248 RID: 12872 RVA: 0x001057D0 File Offset: 0x001039D0
		// (set) Token: 0x06003249 RID: 12873 RVA: 0x001057D8 File Offset: 0x001039D8
		private protected bool ValueIsReaded { protected get; private set; }

		// Token: 0x1700086A RID: 2154
		// (get) Token: 0x0600324A RID: 12874 RVA: 0x001057E4 File Offset: 0x001039E4
		// (set) Token: 0x0600324B RID: 12875 RVA: 0x00105818 File Offset: 0x00103A18
		public T Value
		{
			get
			{
				if (!this.ValueIsReaded)
				{
					this._value = this.GetValue();
					this.ValueIsReaded = true;
				}
				return this._value;
			}
			set
			{
				this._value = value;
				this.SetValue(this._value);
			}
		}

		// Token: 0x1700086B RID: 2155
		// (get) Token: 0x0600324C RID: 12876 RVA: 0x00105830 File Offset: 0x00103A30
		public bool HasValue
		{
			get
			{
				return this.ValueIsReaded;
			}
		}

		// Token: 0x0400250D RID: 9485
		private T _value;
	}
}
