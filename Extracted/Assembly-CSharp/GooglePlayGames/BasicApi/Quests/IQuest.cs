using System;

namespace GooglePlayGames.BasicApi.Quests
{
	// Token: 0x0200018D RID: 397
	public interface IQuest
	{
		// Token: 0x170001AE RID: 430
		// (get) Token: 0x06000CE0 RID: 3296
		string Id { get; }

		// Token: 0x170001AF RID: 431
		// (get) Token: 0x06000CE1 RID: 3297
		string Name { get; }

		// Token: 0x170001B0 RID: 432
		// (get) Token: 0x06000CE2 RID: 3298
		string Description { get; }

		// Token: 0x170001B1 RID: 433
		// (get) Token: 0x06000CE3 RID: 3299
		string BannerUrl { get; }

		// Token: 0x170001B2 RID: 434
		// (get) Token: 0x06000CE4 RID: 3300
		string IconUrl { get; }

		// Token: 0x170001B3 RID: 435
		// (get) Token: 0x06000CE5 RID: 3301
		DateTime StartTime { get; }

		// Token: 0x170001B4 RID: 436
		// (get) Token: 0x06000CE6 RID: 3302
		DateTime ExpirationTime { get; }

		// Token: 0x170001B5 RID: 437
		// (get) Token: 0x06000CE7 RID: 3303
		DateTime? AcceptedTime { get; }

		// Token: 0x170001B6 RID: 438
		// (get) Token: 0x06000CE8 RID: 3304
		IQuestMilestone Milestone { get; }

		// Token: 0x170001B7 RID: 439
		// (get) Token: 0x06000CE9 RID: 3305
		QuestState State { get; }
	}
}
