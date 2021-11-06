using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace GooglePlayGames.Native.PInvoke
{
	// Token: 0x02000260 RID: 608
	internal static class PInvokeUtilities
	{
		// Token: 0x0600138A RID: 5002 RVA: 0x0004FC90 File Offset: 0x0004DE90
		internal static HandleRef CheckNonNull(HandleRef reference)
		{
			if (PInvokeUtilities.IsNull(reference))
			{
				throw new InvalidOperationException();
			}
			return reference;
		}

		// Token: 0x0600138B RID: 5003 RVA: 0x0004FCA4 File Offset: 0x0004DEA4
		internal static bool IsNull(HandleRef reference)
		{
			return PInvokeUtilities.IsNull(HandleRef.ToIntPtr(reference));
		}

		// Token: 0x0600138C RID: 5004 RVA: 0x0004FCB4 File Offset: 0x0004DEB4
		internal static bool IsNull(IntPtr pointer)
		{
			return pointer.Equals(IntPtr.Zero);
		}

		// Token: 0x0600138D RID: 5005 RVA: 0x0004FCC8 File Offset: 0x0004DEC8
		internal static DateTime FromMillisSinceUnixEpoch(long millisSinceEpoch)
		{
			return PInvokeUtilities.UnixEpoch.Add(TimeSpan.FromMilliseconds((double)millisSinceEpoch));
		}

		// Token: 0x0600138E RID: 5006 RVA: 0x0004FCEC File Offset: 0x0004DEEC
		internal static string OutParamsToString(PInvokeUtilities.OutStringMethod outStringMethod)
		{
			UIntPtr out_size = outStringMethod(null, UIntPtr.Zero);
			if (out_size.Equals(UIntPtr.Zero))
			{
				return null;
			}
			StringBuilder stringBuilder = new StringBuilder((int)out_size.ToUInt32());
			outStringMethod(stringBuilder, out_size);
			return stringBuilder.ToString();
		}

		// Token: 0x0600138F RID: 5007 RVA: 0x0004FD3C File Offset: 0x0004DF3C
		internal static T[] OutParamsToArray<T>(PInvokeUtilities.OutMethod<T> outMethod)
		{
			UIntPtr out_size = outMethod(null, UIntPtr.Zero);
			if (out_size.Equals(UIntPtr.Zero))
			{
				return new T[0];
			}
			T[] array = new T[out_size.ToUInt64()];
			outMethod(array, out_size);
			return array;
		}

		// Token: 0x06001390 RID: 5008 RVA: 0x0004FD8C File Offset: 0x0004DF8C
		internal static IEnumerable<T> ToEnumerable<T>(UIntPtr size, Func<UIntPtr, T> getElement)
		{
			for (ulong i = 0UL; i < size.ToUInt64(); i += 1UL)
			{
				yield return getElement(new UIntPtr(i));
			}
			yield break;
		}

		// Token: 0x06001391 RID: 5009 RVA: 0x0004FDC4 File Offset: 0x0004DFC4
		internal static IEnumerator<T> ToEnumerator<T>(UIntPtr size, Func<UIntPtr, T> getElement)
		{
			return PInvokeUtilities.ToEnumerable<T>(size, getElement).GetEnumerator();
		}

		// Token: 0x06001392 RID: 5010 RVA: 0x0004FDD4 File Offset: 0x0004DFD4
		internal static UIntPtr ArrayToSizeT<T>(T[] array)
		{
			if (array == null)
			{
				return UIntPtr.Zero;
			}
			return new UIntPtr((ulong)((long)array.Length));
		}

		// Token: 0x06001393 RID: 5011 RVA: 0x0004FDEC File Offset: 0x0004DFEC
		internal static long ToMilliseconds(TimeSpan span)
		{
			double totalMilliseconds = span.TotalMilliseconds;
			if (totalMilliseconds > 9.223372036854776E+18)
			{
				return long.MaxValue;
			}
			if (totalMilliseconds < -9.223372036854776E+18)
			{
				return long.MinValue;
			}
			return Convert.ToInt64(totalMilliseconds);
		}

		// Token: 0x04000C08 RID: 3080
		private static readonly DateTime UnixEpoch = DateTime.SpecifyKind(new DateTime(1970, 1, 1), DateTimeKind.Utc);

		// Token: 0x020008E2 RID: 2274
		// (Invoke) Token: 0x06005028 RID: 20520
		internal delegate UIntPtr OutStringMethod(StringBuilder out_string, UIntPtr out_size);

		// Token: 0x020008E3 RID: 2275
		// (Invoke) Token: 0x0600502C RID: 20524
		internal delegate UIntPtr OutMethod<T>([In] [Out] T[] out_bytes, UIntPtr out_size);
	}
}
