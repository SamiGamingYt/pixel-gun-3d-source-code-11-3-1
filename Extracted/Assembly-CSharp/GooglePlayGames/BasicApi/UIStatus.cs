using System;

namespace GooglePlayGames.BasicApi
{
	// Token: 0x02000167 RID: 359
	public enum UIStatus
	{
		// Token: 0x0400096D RID: 2413
		Valid = 1,
		// Token: 0x0400096E RID: 2414
		InternalError = -2,
		// Token: 0x0400096F RID: 2415
		NotAuthorized = -3,
		// Token: 0x04000970 RID: 2416
		VersionUpdateRequired = -4,
		// Token: 0x04000971 RID: 2417
		Timeout = -5,
		// Token: 0x04000972 RID: 2418
		UserClosedUI = -6,
		// Token: 0x04000973 RID: 2419
		UiBusy = -12,
		// Token: 0x04000974 RID: 2420
		LeftRoom = -18
	}
}
