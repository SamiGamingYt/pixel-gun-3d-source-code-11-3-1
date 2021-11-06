using System;
using System.Collections.Generic;

namespace Rilisoft
{
	// Token: 0x02000526 RID: 1318
	[Serializable]
	internal sealed class CampaignAdPointMemento : LevelCompleteAdPointMementoBase
	{
		// Token: 0x06002DEA RID: 11754 RVA: 0x000F10A0 File Offset: 0x000EF2A0
		public CampaignAdPointMemento(string id) : base(id)
		{
		}

		// Token: 0x06002DEB RID: 11755 RVA: 0x000F10AC File Offset: 0x000EF2AC
		internal static CampaignAdPointMemento FromObject(string id, object obj)
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
			CampaignAdPointMemento campaignAdPointMemento = new CampaignAdPointMemento(id);
			campaignAdPointMemento.Reset(dictionary);
			return campaignAdPointMemento;
		}
	}
}
