using System;
using System.Collections.Generic;
using UnityEngine;

namespace Rilisoft
{
	// Token: 0x02000720 RID: 1824
	public abstract class AccumulativeQuestBase : QuestBase
	{
		// Token: 0x06003FAA RID: 16298 RVA: 0x00154FE4 File Offset: 0x001531E4
		public AccumulativeQuestBase(string id, long day, int slot, Difficulty difficulty, Reward reward, bool active, bool rewarded, int requiredCount, int initialCound) : base(id, day, slot, difficulty, reward, active, rewarded)
		{
			if (requiredCount < 1)
			{
				throw new ArgumentOutOfRangeException("requiredCount", requiredCount, "Requires at least 1.");
			}
			this._requiredCount = requiredCount;
			this._currentCount = Mathf.Clamp(initialCound, 0, requiredCount);
		}

		// Token: 0x17000A99 RID: 2713
		// (get) Token: 0x06003FAB RID: 16299 RVA: 0x0015503C File Offset: 0x0015323C
		public int CurrentCount
		{
			get
			{
				return this._currentCount;
			}
		}

		// Token: 0x17000A9A RID: 2714
		// (get) Token: 0x06003FAC RID: 16300 RVA: 0x00155044 File Offset: 0x00153244
		public int RequiredCount
		{
			get
			{
				return this._requiredCount;
			}
		}

		// Token: 0x06003FAD RID: 16301 RVA: 0x0015504C File Offset: 0x0015324C
		public void IncrementIf(bool condition, int count = 1)
		{
			if (!condition)
			{
				return;
			}
			decimal d = this.CalculateProgress();
			this._currentCount = Mathf.Clamp(this._currentCount + count, 0, this._requiredCount);
			if (d < 1m)
			{
				base.SetDirty();
			}
		}

		// Token: 0x06003FAE RID: 16302 RVA: 0x00155098 File Offset: 0x00153298
		public void Increment(int count = 1)
		{
			this.IncrementIf(true, count);
		}

		// Token: 0x06003FAF RID: 16303 RVA: 0x001550A4 File Offset: 0x001532A4
		public override decimal CalculateProgress()
		{
			return this._currentCount / this.RequiredCount;
		}

		// Token: 0x06003FB0 RID: 16304 RVA: 0x001550C4 File Offset: 0x001532C4
		protected override void ApppendDifficultyProperties(Dictionary<string, object> difficultyProperties)
		{
			difficultyProperties["parameter"] = this._requiredCount;
		}

		// Token: 0x06003FB1 RID: 16305 RVA: 0x001550DC File Offset: 0x001532DC
		protected override void AppendProperties(Dictionary<string, object> properties)
		{
			properties["currentCount"] = this._currentCount;
		}

		// Token: 0x04002EDC RID: 11996
		private readonly int _requiredCount;

		// Token: 0x04002EDD RID: 11997
		private int _currentCount;
	}
}
