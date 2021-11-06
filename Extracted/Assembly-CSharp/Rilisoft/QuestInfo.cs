using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Rilisoft
{
	// Token: 0x02000727 RID: 1831
	public sealed class QuestInfo
	{
		// Token: 0x06003FD1 RID: 16337 RVA: 0x00155B80 File Offset: 0x00153D80
		internal QuestInfo(IEnumerable<QuestBase> quests, Func<IList<QuestBase>> skipMethod, bool forcedSkip = false)
		{
			if (quests == null)
			{
				throw new ArgumentNullException("quests");
			}
			this._forcedSkip = forcedSkip;
			this._quests = quests.ToList<QuestBase>();
			this._skipMethod = skipMethod;
		}

		// Token: 0x17000AA3 RID: 2723
		// (get) Token: 0x06003FD2 RID: 16338 RVA: 0x00155BB4 File Offset: 0x00153DB4
		public QuestBase Quest
		{
			get
			{
				return this._quests.FirstOrDefault<QuestBase>();
			}
		}

		// Token: 0x17000AA4 RID: 2724
		// (get) Token: 0x06003FD3 RID: 16339 RVA: 0x00155BC4 File Offset: 0x00153DC4
		public bool CanSkip
		{
			get
			{
				if (this._skipMethod == null)
				{
					return false;
				}
				if (this._quests.Count == 0)
				{
					return this._forcedSkip;
				}
				if (this._quests[0].Rewarded)
				{
					return false;
				}
				if (this._quests[0].CalculateProgress() >= 1m)
				{
					return false;
				}
				if (this._quests.Count < 2)
				{
					if (Defs.IsDeveloperBuild)
					{
						Debug.LogFormat("_quests.Count < 2: {0}", new object[]
						{
							this._quests.Count
						});
					}
					return this._forcedSkip;
				}
				return true;
			}
		}

		// Token: 0x06003FD4 RID: 16340 RVA: 0x00155C78 File Offset: 0x00153E78
		public void Skip()
		{
			if (!this.CanSkip)
			{
				return;
			}
			IList<QuestBase> list = this._skipMethod();
			this._quests.Clear();
			if (list != null)
			{
				foreach (QuestBase item in list)
				{
					this._quests.Add(item);
				}
			}
		}

		// Token: 0x04002F02 RID: 12034
		private readonly bool _forcedSkip;

		// Token: 0x04002F03 RID: 12035
		private readonly IList<QuestBase> _quests;

		// Token: 0x04002F04 RID: 12036
		private readonly Func<IList<QuestBase>> _skipMethod;
	}
}
