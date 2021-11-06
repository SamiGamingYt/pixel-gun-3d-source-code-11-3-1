using System;
using ExitGames.Client.Photon;

// Token: 0x020003FD RID: 1021
public class RoomOptions
{
	// Token: 0x17000650 RID: 1616
	// (get) Token: 0x0600240A RID: 9226 RVA: 0x000B3B6C File Offset: 0x000B1D6C
	// (set) Token: 0x0600240B RID: 9227 RVA: 0x000B3B74 File Offset: 0x000B1D74
	public bool IsVisible
	{
		get
		{
			return this.isVisibleField;
		}
		set
		{
			this.isVisibleField = value;
		}
	}

	// Token: 0x17000651 RID: 1617
	// (get) Token: 0x0600240C RID: 9228 RVA: 0x000B3B80 File Offset: 0x000B1D80
	// (set) Token: 0x0600240D RID: 9229 RVA: 0x000B3B88 File Offset: 0x000B1D88
	public bool IsOpen
	{
		get
		{
			return this.isOpenField;
		}
		set
		{
			this.isOpenField = value;
		}
	}

	// Token: 0x17000652 RID: 1618
	// (get) Token: 0x0600240E RID: 9230 RVA: 0x000B3B94 File Offset: 0x000B1D94
	// (set) Token: 0x0600240F RID: 9231 RVA: 0x000B3B9C File Offset: 0x000B1D9C
	public bool CleanupCacheOnLeave
	{
		get
		{
			return this.cleanupCacheOnLeaveField;
		}
		set
		{
			this.cleanupCacheOnLeaveField = value;
		}
	}

	// Token: 0x17000653 RID: 1619
	// (get) Token: 0x06002410 RID: 9232 RVA: 0x000B3BA8 File Offset: 0x000B1DA8
	public bool SuppressRoomEvents
	{
		get
		{
			return this.suppressRoomEventsField;
		}
	}

	// Token: 0x17000654 RID: 1620
	// (get) Token: 0x06002411 RID: 9233 RVA: 0x000B3BB0 File Offset: 0x000B1DB0
	// (set) Token: 0x06002412 RID: 9234 RVA: 0x000B3BB8 File Offset: 0x000B1DB8
	public bool PublishUserId
	{
		get
		{
			return this.publishUserIdField;
		}
		set
		{
			this.publishUserIdField = value;
		}
	}

	// Token: 0x17000655 RID: 1621
	// (get) Token: 0x06002413 RID: 9235 RVA: 0x000B3BC4 File Offset: 0x000B1DC4
	// (set) Token: 0x06002414 RID: 9236 RVA: 0x000B3BCC File Offset: 0x000B1DCC
	[Obsolete("Use property with uppercase naming instead.")]
	public bool isVisible
	{
		get
		{
			return this.isVisibleField;
		}
		set
		{
			this.isVisibleField = value;
		}
	}

	// Token: 0x17000656 RID: 1622
	// (get) Token: 0x06002415 RID: 9237 RVA: 0x000B3BD8 File Offset: 0x000B1DD8
	// (set) Token: 0x06002416 RID: 9238 RVA: 0x000B3BE0 File Offset: 0x000B1DE0
	[Obsolete("Use property with uppercase naming instead.")]
	public bool isOpen
	{
		get
		{
			return this.isOpenField;
		}
		set
		{
			this.isOpenField = value;
		}
	}

	// Token: 0x17000657 RID: 1623
	// (get) Token: 0x06002417 RID: 9239 RVA: 0x000B3BEC File Offset: 0x000B1DEC
	// (set) Token: 0x06002418 RID: 9240 RVA: 0x000B3BF4 File Offset: 0x000B1DF4
	[Obsolete("Use property with uppercase naming instead.")]
	public byte maxPlayers
	{
		get
		{
			return this.MaxPlayers;
		}
		set
		{
			this.MaxPlayers = value;
		}
	}

	// Token: 0x17000658 RID: 1624
	// (get) Token: 0x06002419 RID: 9241 RVA: 0x000B3C00 File Offset: 0x000B1E00
	// (set) Token: 0x0600241A RID: 9242 RVA: 0x000B3C08 File Offset: 0x000B1E08
	[Obsolete("Use property with uppercase naming instead.")]
	public bool cleanupCacheOnLeave
	{
		get
		{
			return this.cleanupCacheOnLeaveField;
		}
		set
		{
			this.cleanupCacheOnLeaveField = value;
		}
	}

	// Token: 0x17000659 RID: 1625
	// (get) Token: 0x0600241B RID: 9243 RVA: 0x000B3C14 File Offset: 0x000B1E14
	// (set) Token: 0x0600241C RID: 9244 RVA: 0x000B3C1C File Offset: 0x000B1E1C
	[Obsolete("Use property with uppercase naming instead.")]
	public Hashtable customRoomProperties
	{
		get
		{
			return this.CustomRoomProperties;
		}
		set
		{
			this.CustomRoomProperties = value;
		}
	}

	// Token: 0x1700065A RID: 1626
	// (get) Token: 0x0600241D RID: 9245 RVA: 0x000B3C28 File Offset: 0x000B1E28
	// (set) Token: 0x0600241E RID: 9246 RVA: 0x000B3C30 File Offset: 0x000B1E30
	[Obsolete("Use property with uppercase naming instead.")]
	public string[] customRoomPropertiesForLobby
	{
		get
		{
			return this.CustomRoomPropertiesForLobby;
		}
		set
		{
			this.CustomRoomPropertiesForLobby = value;
		}
	}

	// Token: 0x1700065B RID: 1627
	// (get) Token: 0x0600241F RID: 9247 RVA: 0x000B3C3C File Offset: 0x000B1E3C
	// (set) Token: 0x06002420 RID: 9248 RVA: 0x000B3C44 File Offset: 0x000B1E44
	[Obsolete("Use property with uppercase naming instead.")]
	public string[] plugins
	{
		get
		{
			return this.Plugins;
		}
		set
		{
			this.Plugins = value;
		}
	}

	// Token: 0x1700065C RID: 1628
	// (get) Token: 0x06002421 RID: 9249 RVA: 0x000B3C50 File Offset: 0x000B1E50
	[Obsolete("Use property with uppercase naming instead.")]
	public bool suppressRoomEvents
	{
		get
		{
			return this.suppressRoomEventsField;
		}
	}

	// Token: 0x1700065D RID: 1629
	// (get) Token: 0x06002422 RID: 9250 RVA: 0x000B3C58 File Offset: 0x000B1E58
	// (set) Token: 0x06002423 RID: 9251 RVA: 0x000B3C60 File Offset: 0x000B1E60
	[Obsolete("Use property with uppercase naming instead.")]
	public bool publishUserId
	{
		get
		{
			return this.publishUserIdField;
		}
		set
		{
			this.publishUserIdField = value;
		}
	}

	// Token: 0x0400195E RID: 6494
	private bool isVisibleField = true;

	// Token: 0x0400195F RID: 6495
	private bool isOpenField = true;

	// Token: 0x04001960 RID: 6496
	public byte MaxPlayers;

	// Token: 0x04001961 RID: 6497
	public int PlayerTtl;

	// Token: 0x04001962 RID: 6498
	public int EmptyRoomTtl;

	// Token: 0x04001963 RID: 6499
	private bool cleanupCacheOnLeaveField = PhotonNetwork.autoCleanUpPlayerObjects;

	// Token: 0x04001964 RID: 6500
	public Hashtable CustomRoomProperties;

	// Token: 0x04001965 RID: 6501
	public string[] CustomRoomPropertiesForLobby = new string[0];

	// Token: 0x04001966 RID: 6502
	public string[] Plugins;

	// Token: 0x04001967 RID: 6503
	private bool suppressRoomEventsField;

	// Token: 0x04001968 RID: 6504
	private bool publishUserIdField;
}
