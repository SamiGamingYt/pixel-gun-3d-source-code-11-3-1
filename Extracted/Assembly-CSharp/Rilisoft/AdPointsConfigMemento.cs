using System;
using System.Collections.Generic;

namespace Rilisoft
{
	// Token: 0x02000522 RID: 1314
	[Serializable]
	internal sealed class AdPointsConfigMemento
	{
		// Token: 0x06002DC0 RID: 11712 RVA: 0x000F0998 File Offset: 0x000EEB98
		internal static AdPointsConfigMemento FromDictionary(Dictionary<string, object> dictionary)
		{
			if (dictionary == null)
			{
				throw new ArgumentNullException("dictionary");
			}
			AdPointsConfigMemento adPointsConfigMemento = new AdPointsConfigMemento();
			object obj;
			dictionary.TryGetValue("chestInLobby", out obj);
			adPointsConfigMemento.ChestInLobby = ChestInLobbyPointMemento.FromObject("chestInLobby", obj);
			object obj2;
			dictionary.TryGetValue("freeSpin", out obj2);
			adPointsConfigMemento.FreeSpin = FreeSpinPointMemento.FromObject("freeSpin", obj2);
			object obj3;
			dictionary.TryGetValue("returnInConnectScene", out obj3);
			adPointsConfigMemento.ReturnInConnectScene = ReturnInConnectSceneAdPointMemento.FromObject("returnInConnectScene", obj3);
			object obj4;
			dictionary.TryGetValue("campaign", out obj4);
			adPointsConfigMemento.Campaign = CampaignAdPointMemento.FromObject("campaign", obj4);
			object obj5;
			dictionary.TryGetValue("survivalArena", out obj5);
			adPointsConfigMemento.SurvivalArena = SurvivalArenaAdPointMemento.FromObject("survivalArena", obj5);
			object obj6;
			dictionary.TryGetValue("afterMatch", out obj6);
			adPointsConfigMemento.AfterMatch = AfterMatchAdPointMemento.FromObject("afterMatch", obj6);
			object obj7;
			dictionary.TryGetValue("polygon", out obj7);
			adPointsConfigMemento.Polygon = PolygonAdPointMemento.FromObject("polygon", obj7);
			return adPointsConfigMemento;
		}

		// Token: 0x170007CD RID: 1997
		// (get) Token: 0x06002DC1 RID: 11713 RVA: 0x000F0A9C File Offset: 0x000EEC9C
		// (set) Token: 0x06002DC2 RID: 11714 RVA: 0x000F0AA4 File Offset: 0x000EECA4
		public ChestInLobbyPointMemento ChestInLobby { get; private set; }

		// Token: 0x170007CE RID: 1998
		// (get) Token: 0x06002DC3 RID: 11715 RVA: 0x000F0AB0 File Offset: 0x000EECB0
		// (set) Token: 0x06002DC4 RID: 11716 RVA: 0x000F0AB8 File Offset: 0x000EECB8
		public FreeSpinPointMemento FreeSpin { get; private set; }

		// Token: 0x170007CF RID: 1999
		// (get) Token: 0x06002DC5 RID: 11717 RVA: 0x000F0AC4 File Offset: 0x000EECC4
		// (set) Token: 0x06002DC6 RID: 11718 RVA: 0x000F0ACC File Offset: 0x000EECCC
		public ReturnInConnectSceneAdPointMemento ReturnInConnectScene { get; private set; }

		// Token: 0x170007D0 RID: 2000
		// (get) Token: 0x06002DC7 RID: 11719 RVA: 0x000F0AD8 File Offset: 0x000EECD8
		// (set) Token: 0x06002DC8 RID: 11720 RVA: 0x000F0AE0 File Offset: 0x000EECE0
		public CampaignAdPointMemento Campaign { get; private set; }

		// Token: 0x170007D1 RID: 2001
		// (get) Token: 0x06002DC9 RID: 11721 RVA: 0x000F0AEC File Offset: 0x000EECEC
		// (set) Token: 0x06002DCA RID: 11722 RVA: 0x000F0AF4 File Offset: 0x000EECF4
		public SurvivalArenaAdPointMemento SurvivalArena { get; private set; }

		// Token: 0x170007D2 RID: 2002
		// (get) Token: 0x06002DCB RID: 11723 RVA: 0x000F0B00 File Offset: 0x000EED00
		// (set) Token: 0x06002DCC RID: 11724 RVA: 0x000F0B08 File Offset: 0x000EED08
		public AfterMatchAdPointMemento AfterMatch { get; private set; }

		// Token: 0x170007D3 RID: 2003
		// (get) Token: 0x06002DCD RID: 11725 RVA: 0x000F0B14 File Offset: 0x000EED14
		// (set) Token: 0x06002DCE RID: 11726 RVA: 0x000F0B1C File Offset: 0x000EED1C
		public PolygonAdPointMemento Polygon { get; private set; }
	}
}
