using System;
using System.Collections.Generic;

namespace Rilisoft
{
	// Token: 0x02000723 RID: 1827
	public sealed class MapAccumulativeQuest : AccumulativeQuestBase
	{
		// Token: 0x06003FB6 RID: 16310 RVA: 0x0015516C File Offset: 0x0015336C
		public MapAccumulativeQuest(string id, long day, int slot, Difficulty difficulty, Reward reward, bool active, bool rewarded, int requiredCount, string map, int initialCount = 0) : base(id, day, slot, difficulty, reward, active, rewarded, requiredCount, initialCount)
		{
			if (map == null)
			{
				throw new ArgumentNullException("map");
			}
			this._map = map;
		}

		// Token: 0x17000A9C RID: 2716
		// (get) Token: 0x06003FB7 RID: 16311 RVA: 0x001551A8 File Offset: 0x001533A8
		public string Map
		{
			get
			{
				return this._map;
			}
		}

		// Token: 0x06003FB8 RID: 16312 RVA: 0x001551B0 File Offset: 0x001533B0
		protected override void AppendProperties(Dictionary<string, object> properties)
		{
			base.AppendProperties(properties);
			properties["map"] = this._map;
		}

		// Token: 0x04002EDF RID: 11999
		private readonly string _map;
	}
}
