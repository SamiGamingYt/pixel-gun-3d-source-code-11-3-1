using System;

namespace GooglePlayGames.OurUtils
{
	// Token: 0x020001A6 RID: 422
	public static class Misc
	{
		// Token: 0x06000DB5 RID: 3509 RVA: 0x00044A64 File Offset: 0x00042C64
		public static bool BuffersAreIdentical(byte[] a, byte[] b)
		{
			if (a == b)
			{
				return true;
			}
			if (a == null || b == null)
			{
				return false;
			}
			if (a.Length != b.Length)
			{
				return false;
			}
			for (int i = 0; i < a.Length; i++)
			{
				if (a[i] != b[i])
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06000DB6 RID: 3510 RVA: 0x00044AB8 File Offset: 0x00042CB8
		public static byte[] GetSubsetBytes(byte[] array, int offset, int length)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			if (offset < 0 || offset >= array.Length)
			{
				throw new ArgumentOutOfRangeException("offset");
			}
			if (length < 0 || array.Length - offset < length)
			{
				throw new ArgumentOutOfRangeException("length");
			}
			if (offset == 0 && length == array.Length)
			{
				return array;
			}
			byte[] array2 = new byte[length];
			Array.Copy(array, offset, array2, 0, length);
			return array2;
		}

		// Token: 0x06000DB7 RID: 3511 RVA: 0x00044B34 File Offset: 0x00042D34
		public static T CheckNotNull<T>(T value)
		{
			if (value == null)
			{
				throw new ArgumentNullException();
			}
			return value;
		}

		// Token: 0x06000DB8 RID: 3512 RVA: 0x00044B48 File Offset: 0x00042D48
		public static T CheckNotNull<T>(T value, string paramName)
		{
			if (value == null)
			{
				throw new ArgumentNullException(paramName);
			}
			return value;
		}
	}
}
