using System;

namespace LitJson
{
	// Token: 0x02000150 RID: 336
	internal enum ParserToken
	{
		// Token: 0x040008A7 RID: 2215
		None = 65536,
		// Token: 0x040008A8 RID: 2216
		Number,
		// Token: 0x040008A9 RID: 2217
		True,
		// Token: 0x040008AA RID: 2218
		False,
		// Token: 0x040008AB RID: 2219
		Null,
		// Token: 0x040008AC RID: 2220
		CharSeq,
		// Token: 0x040008AD RID: 2221
		Char,
		// Token: 0x040008AE RID: 2222
		Text,
		// Token: 0x040008AF RID: 2223
		Object,
		// Token: 0x040008B0 RID: 2224
		ObjectPrime,
		// Token: 0x040008B1 RID: 2225
		Pair,
		// Token: 0x040008B2 RID: 2226
		PairRest,
		// Token: 0x040008B3 RID: 2227
		Array,
		// Token: 0x040008B4 RID: 2228
		ArrayPrime,
		// Token: 0x040008B5 RID: 2229
		Value,
		// Token: 0x040008B6 RID: 2230
		ValueRest,
		// Token: 0x040008B7 RID: 2231
		String,
		// Token: 0x040008B8 RID: 2232
		End,
		// Token: 0x040008B9 RID: 2233
		Epsilon
	}
}
