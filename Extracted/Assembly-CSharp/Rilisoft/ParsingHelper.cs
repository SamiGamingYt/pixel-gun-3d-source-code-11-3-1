using System;
using System.Collections.Generic;
using UnityEngine;

namespace Rilisoft
{
	// Token: 0x02000542 RID: 1346
	internal sealed class ParsingHelper
	{
		// Token: 0x06002EDA RID: 11994 RVA: 0x000F4E74 File Offset: 0x000F3074
		internal static object GetObject(Dictionary<string, object> dictionary, string key)
		{
			if (dictionary == null)
			{
				throw new ArgumentNullException("dictionary");
			}
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			object result;
			if (!dictionary.TryGetValue(key, out result))
			{
				return null;
			}
			return result;
		}

		// Token: 0x06002EDB RID: 11995 RVA: 0x000F4EB4 File Offset: 0x000F30B4
		internal static bool? GetBoolean(Dictionary<string, object> dictionary, string key)
		{
			object @object = ParsingHelper.GetObject(dictionary, key);
			if (@object == null)
			{
				return null;
			}
			bool? result;
			try
			{
				result = new bool?(Convert.ToBoolean(@object));
			}
			catch (Exception ex)
			{
				Debug.LogWarningFormat("Failed to interpret `\"{0}\": {1}` as bool. {2}", new object[]
				{
					key,
					@object,
					ex.Message
				});
				result = null;
			}
			return result;
		}

		// Token: 0x06002EDC RID: 11996 RVA: 0x000F4F44 File Offset: 0x000F3144
		internal static int? GetInt32(Dictionary<string, object> dictionary, string key)
		{
			object @object = ParsingHelper.GetObject(dictionary, key);
			if (@object == null)
			{
				return null;
			}
			int? result;
			try
			{
				result = new int?(Convert.ToInt32(@object));
			}
			catch (Exception ex)
			{
				Debug.LogWarningFormat("Failed to interpret `\"{0}\": {1}` as int. {2}", new object[]
				{
					key,
					@object,
					ex.Message
				});
				result = null;
			}
			return result;
		}

		// Token: 0x06002EDD RID: 11997 RVA: 0x000F4FD4 File Offset: 0x000F31D4
		internal static double? GetDouble(Dictionary<string, object> dictionary, string key)
		{
			object @object = ParsingHelper.GetObject(dictionary, key);
			if (@object == null)
			{
				return null;
			}
			double? result;
			try
			{
				result = new double?(Convert.ToDouble(@object));
			}
			catch (Exception ex)
			{
				Debug.LogWarningFormat("Failed to interpret `\"{0}\": {1}` as double. {2}", new object[]
				{
					key,
					@object,
					ex.Message
				});
				result = null;
			}
			return result;
		}

		// Token: 0x06002EDE RID: 11998 RVA: 0x000F5064 File Offset: 0x000F3264
		internal static string GetString(Dictionary<string, object> dictionary, string key)
		{
			object @object = ParsingHelper.GetObject(dictionary, key);
			if (@object == null)
			{
				return null;
			}
			return @object as string;
		}
	}
}
