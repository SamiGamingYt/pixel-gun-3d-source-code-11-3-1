using System;
using System.Collections.Generic;

namespace Facebook.Unity
{
	// Token: 0x020000FC RID: 252
	public interface IAppLinkResult : IResult
	{
		// Token: 0x170000C9 RID: 201
		// (get) Token: 0x06000799 RID: 1945
		string Url { get; }

		// Token: 0x170000CA RID: 202
		// (get) Token: 0x0600079A RID: 1946
		string TargetUrl { get; }

		// Token: 0x170000CB RID: 203
		// (get) Token: 0x0600079B RID: 1947
		string Ref { get; }

		// Token: 0x170000CC RID: 204
		// (get) Token: 0x0600079C RID: 1948
		IDictionary<string, object> Extras { get; }
	}
}
