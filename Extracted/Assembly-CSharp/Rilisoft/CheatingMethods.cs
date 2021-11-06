using System;

namespace Rilisoft
{
	// Token: 0x0200052C RID: 1324
	[Flags]
	internal enum CheatingMethods
	{
		// Token: 0x04002247 RID: 8775
		None = 0,
		// Token: 0x04002248 RID: 8776
		SignatureTampering = 1,
		// Token: 0x04002249 RID: 8777
		CoinThreshold = 2,
		// Token: 0x0400224A RID: 8778
		GemThreshold = 4
	}
}
