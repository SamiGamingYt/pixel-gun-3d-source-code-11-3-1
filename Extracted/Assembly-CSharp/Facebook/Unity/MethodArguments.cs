using System;
using System.Collections.Generic;
using Facebook.MiniJSON;

namespace Facebook.Unity
{
	// Token: 0x020000D6 RID: 214
	internal class MethodArguments
	{
		// Token: 0x06000679 RID: 1657 RVA: 0x0002D488 File Offset: 0x0002B688
		public MethodArguments() : this(new Dictionary<string, object>())
		{
		}

		// Token: 0x0600067A RID: 1658 RVA: 0x0002D498 File Offset: 0x0002B698
		public MethodArguments(MethodArguments methodArgs) : this(methodArgs.arguments)
		{
		}

		// Token: 0x0600067B RID: 1659 RVA: 0x0002D4A8 File Offset: 0x0002B6A8
		private MethodArguments(IDictionary<string, object> arguments)
		{
			this.arguments = arguments;
		}

		// Token: 0x0600067C RID: 1660 RVA: 0x0002D4C4 File Offset: 0x0002B6C4
		public void AddPrimative<T>(string argumentName, T value) where T : struct
		{
			this.arguments[argumentName] = value;
		}

		// Token: 0x0600067D RID: 1661 RVA: 0x0002D4D8 File Offset: 0x0002B6D8
		public void AddNullablePrimitive<T>(string argumentName, T? nullable) where T : struct
		{
			if (nullable != null && nullable != null)
			{
				this.arguments[argumentName] = nullable.Value;
			}
		}

		// Token: 0x0600067E RID: 1662 RVA: 0x0002D518 File Offset: 0x0002B718
		public void AddString(string argumentName, string value)
		{
			if (!string.IsNullOrEmpty(value))
			{
				this.arguments[argumentName] = value;
			}
		}

		// Token: 0x0600067F RID: 1663 RVA: 0x0002D534 File Offset: 0x0002B734
		public void AddCommaSeparatedList(string argumentName, IEnumerable<string> value)
		{
			if (value != null)
			{
				this.arguments[argumentName] = value.ToCommaSeparateList();
			}
		}

		// Token: 0x06000680 RID: 1664 RVA: 0x0002D550 File Offset: 0x0002B750
		public void AddDictionary(string argumentName, IDictionary<string, object> dict)
		{
			if (dict != null)
			{
				this.arguments[argumentName] = MethodArguments.ToStringDict(dict);
			}
		}

		// Token: 0x06000681 RID: 1665 RVA: 0x0002D56C File Offset: 0x0002B76C
		public void AddList<T>(string argumentName, IEnumerable<T> list)
		{
			if (list != null)
			{
				this.arguments[argumentName] = list;
			}
		}

		// Token: 0x06000682 RID: 1666 RVA: 0x0002D584 File Offset: 0x0002B784
		public void AddUri(string argumentName, Uri uri)
		{
			if (uri != null && !string.IsNullOrEmpty(uri.AbsoluteUri))
			{
				this.arguments[argumentName] = uri.ToString();
			}
		}

		// Token: 0x06000683 RID: 1667 RVA: 0x0002D5C0 File Offset: 0x0002B7C0
		public string ToJsonString()
		{
			return Json.Serialize(this.arguments);
		}

		// Token: 0x06000684 RID: 1668 RVA: 0x0002D5D0 File Offset: 0x0002B7D0
		private static Dictionary<string, string> ToStringDict(IDictionary<string, object> dict)
		{
			if (dict == null)
			{
				return null;
			}
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			foreach (KeyValuePair<string, object> keyValuePair in dict)
			{
				dictionary[keyValuePair.Key] = keyValuePair.Value.ToString();
			}
			return dictionary;
		}

		// Token: 0x04000641 RID: 1601
		private IDictionary<string, object> arguments = new Dictionary<string, object>();
	}
}
