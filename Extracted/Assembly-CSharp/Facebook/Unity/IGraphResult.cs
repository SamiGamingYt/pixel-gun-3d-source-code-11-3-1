using System;
using System.Collections.Generic;
using UnityEngine;

namespace Facebook.Unity
{
	// Token: 0x020000FE RID: 254
	public interface IGraphResult : IResult
	{
		// Token: 0x170000CF RID: 207
		// (get) Token: 0x0600079F RID: 1951
		IList<object> ResultList { get; }

		// Token: 0x170000D0 RID: 208
		// (get) Token: 0x060007A0 RID: 1952
		Texture2D Texture { get; }
	}
}
