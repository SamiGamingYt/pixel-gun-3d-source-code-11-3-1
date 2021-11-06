using System;
using System.Collections.Generic;

namespace Rilisoft
{
	// Token: 0x02000527 RID: 1319
	[Serializable]
	internal sealed class PolygonAdPointMemento : AdPointMementoBase
	{
		// Token: 0x06002DEC RID: 11756 RVA: 0x000F10F0 File Offset: 0x000EF2F0
		public PolygonAdPointMemento(string id) : base(id)
		{
		}

		// Token: 0x170007DA RID: 2010
		// (get) Token: 0x06002DED RID: 11757 RVA: 0x000F10FC File Offset: 0x000EF2FC
		// (set) Token: 0x06002DEE RID: 11758 RVA: 0x000F1104 File Offset: 0x000EF304
		public int EntryCount { get; private set; }

		// Token: 0x06002DEF RID: 11759 RVA: 0x000F1110 File Offset: 0x000EF310
		public int? GetEntryCountOverride(string category)
		{
			return base.GetInt32Override("entryCount", category);
		}

		// Token: 0x06002DF0 RID: 11760 RVA: 0x000F1120 File Offset: 0x000EF320
		internal static PolygonAdPointMemento FromObject(string id, object obj)
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
			PolygonAdPointMemento polygonAdPointMemento = new PolygonAdPointMemento(id);
			polygonAdPointMemento.Reset(dictionary);
			int? @int = ParsingHelper.GetInt32(dictionary, "entryCount");
			if (@int != null)
			{
				polygonAdPointMemento.EntryCount = @int.Value;
			}
			return polygonAdPointMemento;
		}
	}
}
