using System;

namespace I2.Loc
{
	// Token: 0x020002BC RID: 700
	[Serializable]
	public class TermData
	{
		// Token: 0x04000CF9 RID: 3321
		public string Term = string.Empty;

		// Token: 0x04000CFA RID: 3322
		public eTermType TermType;

		// Token: 0x04000CFB RID: 3323
		public string Description = string.Empty;

		// Token: 0x04000CFC RID: 3324
		public string[] Languages = new string[0];
	}
}
