using System;
using System.Collections.Generic;

namespace Facebook.Unity
{
	// Token: 0x020000F5 RID: 245
	internal class AppLinkResult : ResultBase, IAppLinkResult, IResult
	{
		// Token: 0x06000780 RID: 1920 RVA: 0x0002F2F4 File Offset: 0x0002D4F4
		public AppLinkResult(string result) : base(result)
		{
			if (this.ResultDictionary != null)
			{
				string url;
				if (this.ResultDictionary.TryGetValue("url", out url))
				{
					this.Url = url;
				}
				string targetUrl;
				if (this.ResultDictionary.TryGetValue("target_url", out targetUrl))
				{
					this.TargetUrl = targetUrl;
				}
				string @ref;
				if (this.ResultDictionary.TryGetValue("ref", out @ref))
				{
					this.Ref = @ref;
				}
				IDictionary<string, object> extras;
				if (this.ResultDictionary.TryGetValue("extras", out extras))
				{
					this.Extras = extras;
				}
			}
		}

		// Token: 0x170000BF RID: 191
		// (get) Token: 0x06000781 RID: 1921 RVA: 0x0002F38C File Offset: 0x0002D58C
		// (set) Token: 0x06000782 RID: 1922 RVA: 0x0002F394 File Offset: 0x0002D594
		public string Url { get; private set; }

		// Token: 0x170000C0 RID: 192
		// (get) Token: 0x06000783 RID: 1923 RVA: 0x0002F3A0 File Offset: 0x0002D5A0
		// (set) Token: 0x06000784 RID: 1924 RVA: 0x0002F3A8 File Offset: 0x0002D5A8
		public string TargetUrl { get; private set; }

		// Token: 0x170000C1 RID: 193
		// (get) Token: 0x06000785 RID: 1925 RVA: 0x0002F3B4 File Offset: 0x0002D5B4
		// (set) Token: 0x06000786 RID: 1926 RVA: 0x0002F3BC File Offset: 0x0002D5BC
		public string Ref { get; private set; }

		// Token: 0x170000C2 RID: 194
		// (get) Token: 0x06000787 RID: 1927 RVA: 0x0002F3C8 File Offset: 0x0002D5C8
		// (set) Token: 0x06000788 RID: 1928 RVA: 0x0002F3D0 File Offset: 0x0002D5D0
		public IDictionary<string, object> Extras { get; private set; }
	}
}
