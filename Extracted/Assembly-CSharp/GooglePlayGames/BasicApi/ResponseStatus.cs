using System;

namespace GooglePlayGames.BasicApi
{
	// Token: 0x02000166 RID: 358
	public enum ResponseStatus
	{
		// Token: 0x04000965 RID: 2405
		Success = 1,
		// Token: 0x04000966 RID: 2406
		SuccessWithStale,
		// Token: 0x04000967 RID: 2407
		LicenseCheckFailed = -1,
		// Token: 0x04000968 RID: 2408
		InternalError = -2,
		// Token: 0x04000969 RID: 2409
		NotAuthorized = -3,
		// Token: 0x0400096A RID: 2410
		VersionUpdateRequired = -4,
		// Token: 0x0400096B RID: 2411
		Timeout = -5
	}
}
