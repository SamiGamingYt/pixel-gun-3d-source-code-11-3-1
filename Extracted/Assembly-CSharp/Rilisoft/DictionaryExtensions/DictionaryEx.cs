using System;
using System.Collections.Generic;

namespace Rilisoft.DictionaryExtensions
{
	// Token: 0x02000735 RID: 1845
	internal static class DictionaryEx
	{
		// Token: 0x060040D5 RID: 16597 RVA: 0x0015A4F8 File Offset: 0x001586F8
		internal static object TryGet(this Dictionary<string, object> dictionary, string key)
		{
			if (dictionary == null || key == null)
			{
				return null;
			}
			object result = null;
			dictionary.TryGetValue(key, out result);
			return result;
		}
	}
}
