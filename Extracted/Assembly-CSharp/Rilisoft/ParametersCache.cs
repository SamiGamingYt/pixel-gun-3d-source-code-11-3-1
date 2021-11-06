using System;
using System.Collections.Generic;

namespace Rilisoft
{
	// Token: 0x02000558 RID: 1368
	internal static class ParametersCache
	{
		// Token: 0x06002F93 RID: 12179 RVA: 0x000F8FB8 File Offset: 0x000F71B8
		public static Dictionary<string, object> Acquire(int capacity = 1)
		{
			if (capacity > 10)
			{
				return new Dictionary<string, object>(capacity);
			}
			Dictionary<string, object> dictionary = ParametersCache.s_cachedObjectDictionary;
			if (dictionary == null)
			{
				return new Dictionary<string, object>(capacity);
			}
			ParametersCache.s_cachedObjectDictionary = null;
			dictionary.Clear();
			return dictionary;
		}

		// Token: 0x06002F94 RID: 12180 RVA: 0x000F8FF4 File Offset: 0x000F71F4
		public static void Release(Dictionary<string, object> d)
		{
			if (d == null)
			{
				return;
			}
			if (d.Count > 10)
			{
				d.Clear();
				return;
			}
			d.Clear();
			ParametersCache.s_cachedObjectDictionary = d;
		}

		// Token: 0x040022FC RID: 8956
		private const int MaxCapacity = 10;

		// Token: 0x040022FD RID: 8957
		[ThreadStatic]
		private static Dictionary<string, object> s_cachedObjectDictionary;
	}
}
