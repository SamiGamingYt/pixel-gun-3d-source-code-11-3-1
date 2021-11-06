using System;

namespace GooglePlayGames.BasicApi.Quests
{
	// Token: 0x02000190 RID: 400
	[Flags]
	public enum QuestFetchFlags
	{
		// Token: 0x04000A08 RID: 2568
		Upcoming = 1,
		// Token: 0x04000A09 RID: 2569
		Open = 2,
		// Token: 0x04000A0A RID: 2570
		Accepted = 4,
		// Token: 0x04000A0B RID: 2571
		Completed = 8,
		// Token: 0x04000A0C RID: 2572
		CompletedNotClaimed = 16,
		// Token: 0x04000A0D RID: 2573
		Expired = 32,
		// Token: 0x04000A0E RID: 2574
		EndingSoon = 64,
		// Token: 0x04000A0F RID: 2575
		Failed = 128,
		// Token: 0x04000A10 RID: 2576
		All = -1
	}
}
