using System;

namespace GooglePlayGames.BasicApi.SavedGame
{
	// Token: 0x02000196 RID: 406
	public enum SavedGameRequestStatus
	{
		// Token: 0x04000A30 RID: 2608
		Success = 1,
		// Token: 0x04000A31 RID: 2609
		TimeoutError = -1,
		// Token: 0x04000A32 RID: 2610
		InternalError = -2,
		// Token: 0x04000A33 RID: 2611
		AuthenticationError = -3,
		// Token: 0x04000A34 RID: 2612
		BadInputError = -4
	}
}
