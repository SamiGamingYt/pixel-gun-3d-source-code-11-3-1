using System;

namespace Rilisoft
{
	// Token: 0x020004D8 RID: 1240
	public class SaltedFloat
	{
		// Token: 0x06002C4A RID: 11338 RVA: 0x000EB280 File Offset: 0x000E9480
		public SaltedFloat(float value)
		{
			this.value = value;
		}

		// Token: 0x06002C4B RID: 11339 RVA: 0x000EB29C File Offset: 0x000E949C
		public SaltedFloat() : this(0f)
		{
		}

		// Token: 0x17000798 RID: 1944
		// (get) Token: 0x06002C4C RID: 11340 RVA: 0x000EB2AC File Offset: 0x000E94AC
		// (set) Token: 0x06002C4D RID: 11341 RVA: 0x000EB2BC File Offset: 0x000E94BC
		public float value
		{
			get
			{
				return this._values[this._index];
			}
			set
			{
				if (++this._index >= this._values.Length)
				{
					this._index = 0;
				}
				this._values[this._index] = value;
			}
		}

		// Token: 0x04002154 RID: 8532
		private float[] _values = new float[5];

		// Token: 0x04002155 RID: 8533
		private int _index;
	}
}
