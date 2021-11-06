using System;

namespace GooglePlayGames.BasicApi.SavedGame
{
	// Token: 0x02000197 RID: 407
	public enum SelectUIStatus
	{
		// Token: 0x04000A36 RID: 2614
		SavedGameSelected = 1,
		// Token: 0x04000A37 RID: 2615
		UserClosedUI,
		// Token: 0x04000A38 RID: 2616
		InternalError = -1,
		// Token: 0x04000A39 RID: 2617
		TimeoutError = -2,
		// Token: 0x04000A3A RID: 2618
		AuthenticationError = -3,
		// Token: 0x04000A3B RID: 2619
		BadInputError = -4
	}
}
