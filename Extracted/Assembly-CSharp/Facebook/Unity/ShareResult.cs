using System;

namespace Facebook.Unity
{
	// Token: 0x02000109 RID: 265
	internal class ShareResult : ResultBase, IResult, IShareResult
	{
		// Token: 0x060007BF RID: 1983 RVA: 0x0002F94C File Offset: 0x0002DB4C
		internal ShareResult(string result) : base(result)
		{
			object obj;
			if (this.ResultDictionary != null && this.ResultDictionary.TryGetValue("id", out obj))
			{
				this.PostId = (obj as string);
			}
		}

		// Token: 0x170000DF RID: 223
		// (get) Token: 0x060007C0 RID: 1984 RVA: 0x0002F990 File Offset: 0x0002DB90
		// (set) Token: 0x060007C1 RID: 1985 RVA: 0x0002F998 File Offset: 0x0002DB98
		public string PostId { get; private set; }
	}
}
