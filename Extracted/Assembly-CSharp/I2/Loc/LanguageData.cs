using System;

namespace I2.Loc
{
	// Token: 0x020002BD RID: 701
	[Serializable]
	public class LanguageData
	{
		// Token: 0x04000CFD RID: 3325
		public string Name;

		// Token: 0x04000CFE RID: 3326
		public string Code;

		// Token: 0x04000CFF RID: 3327
		[NonSerialized]
		public bool Compressed;
	}
}
