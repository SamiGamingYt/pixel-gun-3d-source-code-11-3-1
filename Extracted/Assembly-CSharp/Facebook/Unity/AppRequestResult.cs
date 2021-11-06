using System;
using System.Collections.Generic;

namespace Facebook.Unity
{
	// Token: 0x020000F6 RID: 246
	internal class AppRequestResult : ResultBase, IAppRequestResult, IResult
	{
		// Token: 0x06000789 RID: 1929 RVA: 0x0002F3DC File Offset: 0x0002D5DC
		public AppRequestResult(string result) : base(result)
		{
			if (this.ResultDictionary != null)
			{
				string requestID;
				if (this.ResultDictionary.TryGetValue("request", out requestID))
				{
					this.RequestID = requestID;
				}
				string text;
				IEnumerable<object> enumerable;
				if (this.ResultDictionary.TryGetValue("to", out text))
				{
					this.To = text.Split(new char[]
					{
						','
					});
				}
				else if (this.ResultDictionary.TryGetValue("to", out enumerable))
				{
					List<string> list = new List<string>();
					foreach (object obj in enumerable)
					{
						string text2 = obj as string;
						if (text2 != null)
						{
							list.Add(text2);
						}
					}
					this.To = list;
				}
			}
		}

		// Token: 0x170000C3 RID: 195
		// (get) Token: 0x0600078A RID: 1930 RVA: 0x0002F4D8 File Offset: 0x0002D6D8
		// (set) Token: 0x0600078B RID: 1931 RVA: 0x0002F4E0 File Offset: 0x0002D6E0
		public string RequestID { get; private set; }

		// Token: 0x170000C4 RID: 196
		// (get) Token: 0x0600078C RID: 1932 RVA: 0x0002F4EC File Offset: 0x0002D6EC
		// (set) Token: 0x0600078D RID: 1933 RVA: 0x0002F4F4 File Offset: 0x0002D6F4
		public IEnumerable<string> To { get; private set; }

		// Token: 0x04000671 RID: 1649
		public const string RequestIDKey = "request";

		// Token: 0x04000672 RID: 1650
		public const string ToKey = "to";
	}
}
