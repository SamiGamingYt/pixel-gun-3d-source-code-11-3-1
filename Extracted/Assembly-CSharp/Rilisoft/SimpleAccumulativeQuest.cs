using System;

namespace Rilisoft
{
	// Token: 0x02000721 RID: 1825
	public sealed class SimpleAccumulativeQuest : AccumulativeQuestBase
	{
		// Token: 0x06003FB2 RID: 16306 RVA: 0x001550F4 File Offset: 0x001532F4
		public SimpleAccumulativeQuest(string id, long day, int slot, Difficulty difficulty, Reward reward, bool active, bool rewarded, int requiredCount, int initialCount = 0) : base(id, day, slot, difficulty, reward, active, rewarded, requiredCount, initialCount)
		{
		}
	}
}
