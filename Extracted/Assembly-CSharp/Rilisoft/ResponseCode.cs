using System;

namespace Rilisoft
{
	// Token: 0x0200069A RID: 1690
	public enum ResponseCode
	{
		// Token: 0x04002BCD RID: 11213
		Licensed,
		// Token: 0x04002BCE RID: 11214
		NotLicensed,
		// Token: 0x04002BCF RID: 11215
		LicensedOldKey,
		// Token: 0x04002BD0 RID: 11216
		ErrorNotMarketManaged,
		// Token: 0x04002BD1 RID: 11217
		ErrorServerFailure,
		// Token: 0x04002BD2 RID: 11218
		ErrorOverQuota,
		// Token: 0x04002BD3 RID: 11219
		ErrorContactingServer = 257,
		// Token: 0x04002BD4 RID: 11220
		ErrorInvalidPackageName,
		// Token: 0x04002BD5 RID: 11221
		ErrorNonMatchingUid
	}
}
