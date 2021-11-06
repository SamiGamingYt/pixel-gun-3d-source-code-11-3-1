using System;
using ExitGames.Client.Photon;

// Token: 0x02000422 RID: 1058
public class RoomInfo
{
	// Token: 0x06002627 RID: 9767 RVA: 0x000BEF00 File Offset: 0x000BD100
	protected internal RoomInfo(string roomName, Hashtable properties)
	{
		this.InternalCacheProperties(properties);
		this.nameField = roomName;
	}

	// Token: 0x170006CF RID: 1743
	// (get) Token: 0x06002628 RID: 9768 RVA: 0x000BEF48 File Offset: 0x000BD148
	// (set) Token: 0x06002629 RID: 9769 RVA: 0x000BEF50 File Offset: 0x000BD150
	public bool removedFromList { get; internal set; }

	// Token: 0x170006D0 RID: 1744
	// (get) Token: 0x0600262A RID: 9770 RVA: 0x000BEF5C File Offset: 0x000BD15C
	// (set) Token: 0x0600262B RID: 9771 RVA: 0x000BEF64 File Offset: 0x000BD164
	protected internal bool serverSideMasterClient { get; private set; }

	// Token: 0x170006D1 RID: 1745
	// (get) Token: 0x0600262C RID: 9772 RVA: 0x000BEF70 File Offset: 0x000BD170
	public Hashtable customProperties
	{
		get
		{
			return this.customPropertiesField;
		}
	}

	// Token: 0x170006D2 RID: 1746
	// (get) Token: 0x0600262D RID: 9773 RVA: 0x000BEF78 File Offset: 0x000BD178
	public string name
	{
		get
		{
			return this.nameField;
		}
	}

	// Token: 0x170006D3 RID: 1747
	// (get) Token: 0x0600262E RID: 9774 RVA: 0x000BEF80 File Offset: 0x000BD180
	// (set) Token: 0x0600262F RID: 9775 RVA: 0x000BEF88 File Offset: 0x000BD188
	public int playerCount { get; private set; }

	// Token: 0x170006D4 RID: 1748
	// (get) Token: 0x06002630 RID: 9776 RVA: 0x000BEF94 File Offset: 0x000BD194
	// (set) Token: 0x06002631 RID: 9777 RVA: 0x000BEF9C File Offset: 0x000BD19C
	public bool isLocalClientInside { get; set; }

	// Token: 0x170006D5 RID: 1749
	// (get) Token: 0x06002632 RID: 9778 RVA: 0x000BEFA8 File Offset: 0x000BD1A8
	public byte maxPlayers
	{
		get
		{
			return this.maxPlayersField;
		}
	}

	// Token: 0x170006D6 RID: 1750
	// (get) Token: 0x06002633 RID: 9779 RVA: 0x000BEFB0 File Offset: 0x000BD1B0
	public bool open
	{
		get
		{
			return this.openField;
		}
	}

	// Token: 0x170006D7 RID: 1751
	// (get) Token: 0x06002634 RID: 9780 RVA: 0x000BEFB8 File Offset: 0x000BD1B8
	public bool visible
	{
		get
		{
			return this.visibleField;
		}
	}

	// Token: 0x06002635 RID: 9781 RVA: 0x000BEFC0 File Offset: 0x000BD1C0
	public override bool Equals(object other)
	{
		RoomInfo roomInfo = other as RoomInfo;
		return roomInfo != null && this.name.Equals(roomInfo.nameField);
	}

	// Token: 0x06002636 RID: 9782 RVA: 0x000BEFF0 File Offset: 0x000BD1F0
	public override int GetHashCode()
	{
		return this.nameField.GetHashCode();
	}

	// Token: 0x06002637 RID: 9783 RVA: 0x000BF000 File Offset: 0x000BD200
	public override string ToString()
	{
		return string.Format("Room: '{0}' {1},{2} {4}/{3} players.", new object[]
		{
			this.nameField,
			(!this.visibleField) ? "hidden" : "visible",
			(!this.openField) ? "closed" : "open",
			this.maxPlayersField,
			this.playerCount
		});
	}

	// Token: 0x06002638 RID: 9784 RVA: 0x000BF07C File Offset: 0x000BD27C
	public string ToStringFull()
	{
		return string.Format("Room: '{0}' {1},{2} {4}/{3} players.\ncustomProps: {5}", new object[]
		{
			this.nameField,
			(!this.visibleField) ? "hidden" : "visible",
			(!this.openField) ? "closed" : "open",
			this.maxPlayersField,
			this.playerCount,
			this.customPropertiesField.ToStringFull()
		});
	}

	// Token: 0x06002639 RID: 9785 RVA: 0x000BF108 File Offset: 0x000BD308
	protected internal void InternalCacheProperties(Hashtable propertiesToCache)
	{
		if (propertiesToCache == null || propertiesToCache.Count == 0 || this.customPropertiesField.Equals(propertiesToCache))
		{
			return;
		}
		if (propertiesToCache.ContainsKey(251))
		{
			this.removedFromList = (bool)propertiesToCache[251];
			if (this.removedFromList)
			{
				return;
			}
		}
		if (propertiesToCache.ContainsKey(255))
		{
			this.maxPlayersField = (byte)propertiesToCache[byte.MaxValue];
		}
		if (propertiesToCache.ContainsKey(253))
		{
			this.openField = (bool)propertiesToCache[253];
		}
		if (propertiesToCache.ContainsKey(254))
		{
			this.visibleField = (bool)propertiesToCache[254];
		}
		if (propertiesToCache.ContainsKey(252))
		{
			this.playerCount = (int)((byte)propertiesToCache[252]);
		}
		if (propertiesToCache.ContainsKey(249))
		{
			this.autoCleanUpField = (bool)propertiesToCache[249];
		}
		if (propertiesToCache.ContainsKey(248))
		{
			this.serverSideMasterClient = true;
			bool flag = this.masterClientIdField != 0;
			this.masterClientIdField = (int)propertiesToCache[248];
			if (flag)
			{
				PhotonNetwork.networkingPeer.UpdateMasterClient();
			}
		}
		if (propertiesToCache.ContainsKey(247))
		{
			this.expectedUsersField = (string[])propertiesToCache[247];
		}
		this.customPropertiesField.MergeStringKeys(propertiesToCache);
	}

	// Token: 0x04001A84 RID: 6788
	private Hashtable customPropertiesField = new Hashtable();

	// Token: 0x04001A85 RID: 6789
	protected byte maxPlayersField;

	// Token: 0x04001A86 RID: 6790
	protected string[] expectedUsersField;

	// Token: 0x04001A87 RID: 6791
	protected bool openField = true;

	// Token: 0x04001A88 RID: 6792
	protected bool visibleField = true;

	// Token: 0x04001A89 RID: 6793
	protected bool autoCleanUpField = PhotonNetwork.autoCleanUpPlayerObjects;

	// Token: 0x04001A8A RID: 6794
	protected string nameField;

	// Token: 0x04001A8B RID: 6795
	protected internal int masterClientIdField;
}
