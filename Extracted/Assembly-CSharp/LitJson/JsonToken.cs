using System;

namespace LitJson
{
	// Token: 0x02000149 RID: 329
	public enum JsonToken
	{
		// Token: 0x04000863 RID: 2147
		None,
		// Token: 0x04000864 RID: 2148
		ObjectStart,
		// Token: 0x04000865 RID: 2149
		PropertyName,
		// Token: 0x04000866 RID: 2150
		ObjectEnd,
		// Token: 0x04000867 RID: 2151
		ArrayStart,
		// Token: 0x04000868 RID: 2152
		ArrayEnd,
		// Token: 0x04000869 RID: 2153
		Int,
		// Token: 0x0400086A RID: 2154
		Long,
		// Token: 0x0400086B RID: 2155
		Double,
		// Token: 0x0400086C RID: 2156
		String,
		// Token: 0x0400086D RID: 2157
		Boolean,
		// Token: 0x0400086E RID: 2158
		Null
	}
}
