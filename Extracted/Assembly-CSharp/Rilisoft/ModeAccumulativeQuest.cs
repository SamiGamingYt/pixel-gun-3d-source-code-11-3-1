using System;
using System.Collections.Generic;

namespace Rilisoft
{
	// Token: 0x02000722 RID: 1826
	public sealed class ModeAccumulativeQuest : AccumulativeQuestBase
	{
		// Token: 0x06003FB3 RID: 16307 RVA: 0x00155118 File Offset: 0x00153318
		public ModeAccumulativeQuest(string id, long day, int slot, Difficulty difficulty, Reward reward, bool active, bool rewarded, int requiredCount, ConnectSceneNGUIController.RegimGame mode, int initialCount = 0) : base(id, day, slot, difficulty, reward, active, rewarded, requiredCount, initialCount)
		{
			this._mode = mode;
		}

		// Token: 0x17000A9B RID: 2715
		// (get) Token: 0x06003FB4 RID: 16308 RVA: 0x00155144 File Offset: 0x00153344
		public ConnectSceneNGUIController.RegimGame Mode
		{
			get
			{
				return this._mode;
			}
		}

		// Token: 0x06003FB5 RID: 16309 RVA: 0x0015514C File Offset: 0x0015334C
		protected override void AppendProperties(Dictionary<string, object> properties)
		{
			base.AppendProperties(properties);
			properties["mode"] = this._mode;
		}

		// Token: 0x04002EDE RID: 11998
		private readonly ConnectSceneNGUIController.RegimGame _mode;
	}
}
