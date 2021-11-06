using System;
using System.Collections.Generic;
using System.Linq;

// Token: 0x020007B5 RID: 1973
public static class MyEnumerableExtensions
{
	// Token: 0x06004791 RID: 18321 RVA: 0x0018BE64 File Offset: 0x0018A064
	public static IEnumerable<T> Randomize<T>(this IEnumerable<T> source)
	{
		Random rnd = new Random();
		return from item in source
		orderby rnd.Next()
		select item;
	}
}
