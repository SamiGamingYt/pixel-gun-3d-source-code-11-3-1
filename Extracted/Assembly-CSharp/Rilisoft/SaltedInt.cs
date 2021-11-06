using System;

namespace Rilisoft
{
	// Token: 0x02000745 RID: 1861
	public struct SaltedInt : IEquatable<SaltedInt>
	{
		// Token: 0x06004172 RID: 16754 RVA: 0x0015CD48 File Offset: 0x0015AF48
		public SaltedInt(int salt, int value)
		{
			this._salt = salt;
			this._saltedValue = (salt ^ value);
		}

		// Token: 0x06004173 RID: 16755 RVA: 0x0015CD5C File Offset: 0x0015AF5C
		public SaltedInt(int salt)
		{
			this = new SaltedInt(salt, 0);
		}

		// Token: 0x17000ADC RID: 2780
		// (get) Token: 0x06004175 RID: 16757 RVA: 0x0015CD74 File Offset: 0x0015AF74
		// (set) Token: 0x06004176 RID: 16758 RVA: 0x0015CD84 File Offset: 0x0015AF84
		public int Value
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

		// Token: 0x06004177 RID: 16759 RVA: 0x0015CD94 File Offset: 0x0015AF94
		public bool Equals(SaltedInt other)
		{
			return this.Value == other.Value;
		}

		// Token: 0x06004178 RID: 16760 RVA: 0x0015CDA8 File Offset: 0x0015AFA8
		public override bool Equals(object obj)
		{
			if (!(obj is SaltedInt))
			{
				return false;
			}
			SaltedInt other = (SaltedInt)obj;
			return this.Equals(other);
		}

		// Token: 0x06004179 RID: 16761 RVA: 0x0015CDD0 File Offset: 0x0015AFD0
		public override int GetHashCode()
		{
			return this.Value.GetHashCode();
		}

		// Token: 0x0600417A RID: 16762 RVA: 0x0015CDEC File Offset: 0x0015AFEC
		public static implicit operator SaltedInt(int i)
		{
			return new SaltedInt(SaltedInt.s_prng.Next(), i);
		}

		// Token: 0x04002FC9 RID: 12233
		private static readonly Random s_prng = new Random();

		// Token: 0x04002FCA RID: 12234
		private readonly int _salt;

		// Token: 0x04002FCB RID: 12235
		private int _saltedValue;
	}
}
