using System;
using System.Collections.Generic;

namespace Facebook.Unity
{
	// Token: 0x020000FD RID: 253
	public interface IAppRequestResult : IResult
	{
		// Token: 0x170000CD RID: 205
		// (get) Token: 0x0600079D RID: 1949
		string RequestID { get; }

		// Token: 0x170000CE RID: 206
		// (get) Token: 0x0600079E RID: 1950
		IEnumerable<string> To { get; }
	}
}
