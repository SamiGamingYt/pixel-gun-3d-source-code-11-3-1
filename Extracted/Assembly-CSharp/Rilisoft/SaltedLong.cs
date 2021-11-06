using System;

namespace Rilisoft
{
	// Token: 0x02000746 RID: 1862
	public struct SaltedLong
	{
		// Token: 0x0600417B RID: 16763 RVA: 0x0015CE00 File Offset: 0x0015B000
		public SaltedLong(long salt, long value)
		{
			this._salt = salt;
			this._saltedValue = (salt ^ value);
		}

		// Token: 0x0600417C RID: 16764 RVA: 0x0015CE14 File Offset: 0x0015B014
		public SaltedLong(long salt)
		{
			this = new SaltedLong(salt, 0L);
		}

		// Token: 0x17000ADD RID: 2781
		// (get) Token: 0x0600417D RID: 16765 RVA: 0x0015CE20 File Offset: 0x0015B020
		// (set) Token: 0x0600417E RID: 16766 RVA: 0x0015CE30 File Offset: 0x0015B030
		public long Value
		{
			get
			{
				return this._salt ^ this._saltedValue;
			}
			set
			{
				this._saltedValue = (this._salt ^ value);
			}
		}

		// Token: 0x04002FCC RID: 12236
		private readonly long _salt;

		// Token: 0x04002FCD RID: 12237
		private long _saltedValue;
	}
}
