using System;
using System.Collections.Generic;

namespace Rilisoft.Details
{
	// Token: 0x020005D7 RID: 1495
	internal sealed class ByteArrayComparer : IEqualityComparer<byte[]>
	{
		// Token: 0x06003362 RID: 13154 RVA: 0x0010A584 File Offset: 0x00108784
		public bool Equals(byte[] x, byte[] y)
		{
			if (x == null && y == null)
			{
				return true;
			}
			if (x == null || y == null)
			{
				return false;
			}
			if (x.Length != y.Length)
			{
				return false;
			}
			for (int num = 0; num != x.Length; num++)
			{
				if (x[num] != y[num])
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06003363 RID: 13155 RVA: 0x0010A5DC File Offset: 0x001087DC
		public int GetHashCode(byte[] obj)
		{
			if (obj == null)
			{
				return 0;
			}
			int num = 0;
			for (int num2 = 0; num2 != obj.Length; num2++)
			{
				int num3 = num2 % 4;
				num ^= (int)obj[num2] << num3;
			}
			return num;
		}
	}
}
