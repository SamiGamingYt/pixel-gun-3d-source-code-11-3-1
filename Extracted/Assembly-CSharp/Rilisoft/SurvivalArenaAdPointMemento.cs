using System;
using System.Collections.Generic;

namespace Rilisoft
{
	// Token: 0x02000525 RID: 1317
	[Serializable]
	internal sealed class SurvivalArenaAdPointMemento : LevelCompleteAdPointMementoBase
	{
		// Token: 0x06002DE5 RID: 11749 RVA: 0x000F1004 File Offset: 0x000EF204
		public SurvivalArenaAdPointMemento(string id) : base(id)
		{
		}

		// Token: 0x170007D9 RID: 2009
		// (get) Token: 0x06002DE6 RID: 11750 RVA: 0x000F1010 File Offset: 0x000EF210
		// (set) Token: 0x06002DE7 RID: 11751 RVA: 0x000F1018 File Offset: 0x000EF218
		public int WaveMinCount { get; private set; }

		// Token: 0x06002DE8 RID: 11752 RVA: 0x000F1024 File Offset: 0x000EF224
		public int? GetWaveMinCountOverride(string category)
		{
			return base.GetInt32Override("waveMinCount", category);
		}

		// Token: 0x06002DE9 RID: 11753 RVA: 0x000F1034 File Offset: 0x000EF234
		internal static SurvivalArenaAdPointMemento FromObject(string id, object obj)
		{
			if (id == null)
			{
				throw new ArgumentNullException("id");
			}
			if (obj == null)
			{
				return null;
			}
			Dictionary<string, object> dictionary = obj as Dictionary<string, object>;
			if (dictionary == null)
			{
				return null;
			}
			SurvivalArenaAdPointMemento survivalArenaAdPointMemento = new SurvivalArenaAdPointMemento(id);
			survivalArenaAdPointMemento.Reset(dictionary);
			int? @int = ParsingHelper.GetInt32(dictionary, "waveMinCount");
			if (@int != null)
			{
				survivalArenaAdPointMemento.WaveMinCount = @int.Value;
			}
			return survivalArenaAdPointMemento;
		}
	}
}
