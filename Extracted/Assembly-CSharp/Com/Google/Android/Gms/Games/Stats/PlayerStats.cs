using System;

namespace Com.Google.Android.Gms.Games.Stats
{
	// Token: 0x020001B9 RID: 441
	public interface PlayerStats
	{
		// Token: 0x06000E76 RID: 3702
		float getAverageSessionLength();

		// Token: 0x06000E77 RID: 3703
		float getChurnProbability();

		// Token: 0x06000E78 RID: 3704
		int getDaysSinceLastPlayed();

		// Token: 0x06000E79 RID: 3705
		int getNumberOfPurchases();

		// Token: 0x06000E7A RID: 3706
		int getNumberOfSessions();

		// Token: 0x06000E7B RID: 3707
		float getSessionPercentile();

		// Token: 0x06000E7C RID: 3708
		float getSpendPercentile();

		// Token: 0x06000E7D RID: 3709
		float getSpendProbability();

		// Token: 0x06000E7E RID: 3710
		float getHighSpenderProbability();

		// Token: 0x06000E7F RID: 3711
		float getTotalSpendNext28Days();
	}
}
