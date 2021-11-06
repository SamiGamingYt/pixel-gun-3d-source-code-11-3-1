using System;
using System.Collections.Generic;

namespace GooglePlayGames.BasicApi.Quests
{
	// Token: 0x02000194 RID: 404
	public interface IQuestsClient
	{
		// Token: 0x06000CF1 RID: 3313
		void Fetch(DataSource source, string questId, Action<ResponseStatus, IQuest> callback);

		// Token: 0x06000CF2 RID: 3314
		void FetchMatchingState(DataSource source, QuestFetchFlags flags, Action<ResponseStatus, List<IQuest>> callback);

		// Token: 0x06000CF3 RID: 3315
		void ShowAllQuestsUI(Action<QuestUiResult, IQuest, IQuestMilestone> callback);

		// Token: 0x06000CF4 RID: 3316
		void ShowSpecificQuestUI(IQuest quest, Action<QuestUiResult, IQuest, IQuestMilestone> callback);

		// Token: 0x06000CF5 RID: 3317
		void Accept(IQuest quest, Action<QuestAcceptStatus, IQuest> callback);

		// Token: 0x06000CF6 RID: 3318
		void ClaimMilestone(IQuestMilestone milestone, Action<QuestClaimMilestoneStatus, IQuest, IQuestMilestone> callback);
	}
}
