using System;
using System.Collections.Generic;

namespace Rilisoft
{
	// Token: 0x02000524 RID: 1316
	[Serializable]
	internal abstract class LevelCompleteAdPointMementoBase : AdPointMementoBase
	{
		// Token: 0x06002DDD RID: 11741 RVA: 0x000F0F40 File Offset: 0x000EF140
		public LevelCompleteAdPointMementoBase(string id) : base(id)
		{
		}

		// Token: 0x170007D7 RID: 2007
		// (get) Token: 0x06002DDE RID: 11742 RVA: 0x000F0F4C File Offset: 0x000EF14C
		// (set) Token: 0x06002DDF RID: 11743 RVA: 0x000F0F54 File Offset: 0x000EF154
		public bool Quit { get; private set; }

		// Token: 0x170007D8 RID: 2008
		// (get) Token: 0x06002DE0 RID: 11744 RVA: 0x000F0F60 File Offset: 0x000EF160
		// (set) Token: 0x06002DE1 RID: 11745 RVA: 0x000F0F68 File Offset: 0x000EF168
		public bool Death { get; private set; }

		// Token: 0x06002DE2 RID: 11746 RVA: 0x000F0F74 File Offset: 0x000EF174
		public bool? GetQuitOverride(string category)
		{
			return base.GetBooleanOverride("quit", category);
		}

		// Token: 0x06002DE3 RID: 11747 RVA: 0x000F0F84 File Offset: 0x000EF184
		public bool? GetDeathOverride(string category)
		{
			return base.GetBooleanOverride("death", category);
		}

		// Token: 0x06002DE4 RID: 11748 RVA: 0x000F0F94 File Offset: 0x000EF194
		protected new void Reset(Dictionary<string, object> dictionary)
		{
			if (dictionary == null)
			{
				throw new ArgumentNullException("dictionary");
			}
			base.Reset(dictionary);
			bool? boolean = ParsingHelper.GetBoolean(dictionary, "quit");
			if (boolean != null)
			{
				this.Quit = boolean.Value;
			}
			bool? boolean2 = ParsingHelper.GetBoolean(dictionary, "death");
			if (boolean2 != null)
			{
				this.Death = boolean2.Value;
			}
		}
	}
}
