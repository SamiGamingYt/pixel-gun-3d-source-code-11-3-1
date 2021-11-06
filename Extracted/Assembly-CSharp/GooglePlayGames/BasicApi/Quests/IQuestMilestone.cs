using System;

namespace GooglePlayGames.BasicApi.Quests
{
	// Token: 0x0200018F RID: 399
	public interface IQuestMilestone
	{
		// Token: 0x170001B8 RID: 440
		// (get) Token: 0x06000CEA RID: 3306
		string Id { get; }

		// Token: 0x170001B9 RID: 441
		// (get) Token: 0x06000CEB RID: 3307
		string EventId { get; }

		// Token: 0x170001BA RID: 442
		// (get) Token: 0x06000CEC RID: 3308
		string QuestId { get; }

		// Token: 0x170001BB RID: 443
		// (get) Token: 0x06000CED RID: 3309
		ulong CurrentCount { get; }

		// Token: 0x170001BC RID: 444
		// (get) Token: 0x06000CEE RID: 3310
		ulong TargetCount { get; }

		// Token: 0x170001BD RID: 445
		// (get) Token: 0x06000CEF RID: 3311
		byte[] CompletionRewardData { get; }

		// Token: 0x170001BE RID: 446
		// (get) Token: 0x06000CF0 RID: 3312
		MilestoneState State { get; }
	}
}
