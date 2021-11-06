using System;
using System.Collections.Generic;

namespace Facebook.Unity
{
	// Token: 0x02000104 RID: 260
	public interface IResult
	{
		// Token: 0x170000D4 RID: 212
		// (get) Token: 0x060007A4 RID: 1956
		string Error { get; }

		// Token: 0x170000D5 RID: 213
		// (get) Token: 0x060007A5 RID: 1957
		IDictionary<string, object> ResultDictionary { get; }

		// Token: 0x170000D6 RID: 214
		// (get) Token: 0x060007A6 RID: 1958
		string RawResult { get; }

		// Token: 0x170000D7 RID: 215
		// (get) Token: 0x060007A7 RID: 1959
		bool Cancelled { get; }
	}
}
