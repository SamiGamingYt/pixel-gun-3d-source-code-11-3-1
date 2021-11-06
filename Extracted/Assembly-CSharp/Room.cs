using System;
using ExitGames.Client.Photon;
using UnityEngine;

// Token: 0x02000421 RID: 1057
public class Room : RoomInfo
{
	// Token: 0x06002612 RID: 9746 RVA: 0x000BEA90 File Offset: 0x000BCC90
	internal Room(string roomName, RoomOptions options) : base(roomName, null)
	{
		if (options == null)
		{
			options = new RoomOptions();
		}
		this.visibleField = options.IsVisible;
		this.openField = options.IsOpen;
		this.maxPlayersField = options.MaxPlayers;
		this.autoCleanUpField = false;
		base.InternalCacheProperties(options.CustomRoomProperties);
		this.propertiesListedInLobby = options.CustomRoomPropertiesForLobby;
	}

	// Token: 0x170006C6 RID: 1734
	// (get) Token: 0x06002613 RID: 9747 RVA: 0x000BEAF8 File Offset: 0x000BCCF8
	// (set) Token: 0x06002614 RID: 9748 RVA: 0x000BEB00 File Offset: 0x000BCD00
	public new string name
	{
		get
		{
			return this.nameField;
		}
		internal set
		{
			this.nameField = value;
		}
	}

	// Token: 0x170006C7 RID: 1735
	// (get) Token: 0x06002615 RID: 9749 RVA: 0x000BEB0C File Offset: 0x000BCD0C
	// (set) Token: 0x06002616 RID: 9750 RVA: 0x000BEB14 File Offset: 0x000BCD14
	public new bool open
	{
		get
		{
			return this.openField;
		}
		set
		{
			if (!this.Equals(PhotonNetwork.room))
			{
				Debug.LogWarning("Can't set open when not in that room.");
			}
			if (value != this.openField && !PhotonNetwork.offlineMode)
			{
				PhotonNetwork.networkingPeer.OpSetPropertiesOfRoom(new Hashtable
				{
					{
						253,
						value
					}
				}, null, false);
			}
			this.openField = value;
		}
	}

	// Token: 0x170006C8 RID: 1736
	// (get) Token: 0x06002617 RID: 9751 RVA: 0x000BEB84 File Offset: 0x000BCD84
	// (set) Token: 0x06002618 RID: 9752 RVA: 0x000BEB8C File Offset: 0x000BCD8C
	public new bool visible
	{
		get
		{
			return this.visibleField;
		}
		set
		{
			if (!this.Equals(PhotonNetwork.room))
			{
				Debug.LogWarning("Can't set visible when not in that room.");
			}
			if (value != this.visibleField && !PhotonNetwork.offlineMode)
			{
				PhotonNetwork.networkingPeer.OpSetPropertiesOfRoom(new Hashtable
				{
					{
						254,
						value
					}
				}, null, false);
			}
			this.visibleField = value;
		}
	}

	// Token: 0x170006C9 RID: 1737
	// (get) Token: 0x06002619 RID: 9753 RVA: 0x000BEBFC File Offset: 0x000BCDFC
	// (set) Token: 0x0600261A RID: 9754 RVA: 0x000BEC04 File Offset: 0x000BCE04
	public string[] propertiesListedInLobby { get; private set; }

	// Token: 0x170006CA RID: 1738
	// (get) Token: 0x0600261B RID: 9755 RVA: 0x000BEC10 File Offset: 0x000BCE10
	public bool autoCleanUp
	{
		get
		{
			return this.autoCleanUpField;
		}
	}

	// Token: 0x170006CB RID: 1739
	// (get) Token: 0x0600261C RID: 9756 RVA: 0x000BEC18 File Offset: 0x000BCE18
	// (set) Token: 0x0600261D RID: 9757 RVA: 0x000BEC20 File Offset: 0x000BCE20
	public new int maxPlayers
	{
		get
		{
			return (int)this.maxPlayersField;
		}
		set
		{
			if (!this.Equals(PhotonNetwork.room))
			{
				Debug.LogWarning("Can't set MaxPlayers when not in that room.");
			}
			if (value > 255)
			{
				Debug.LogWarning("Can't set Room.MaxPlayers to: " + value + ". Using max value: 255.");
				value = 255;
			}
			if (value != (int)this.maxPlayersField && !PhotonNetwork.offlineMode)
			{
				PhotonNetwork.networkingPeer.OpSetPropertiesOfRoom(new Hashtable
				{
					{
						byte.MaxValue,
						(byte)value
					}
				}, null, false);
			}
			this.maxPlayersField = (byte)value;
		}
	}

	// Token: 0x170006CC RID: 1740
	// (get) Token: 0x0600261E RID: 9758 RVA: 0x000BECBC File Offset: 0x000BCEBC
	public new int playerCount
	{
		get
		{
			if (PhotonNetwork.playerList != null)
			{
				return PhotonNetwork.playerList.Length;
			}
			return 0;
		}
	}

	// Token: 0x170006CD RID: 1741
	// (get) Token: 0x0600261F RID: 9759 RVA: 0x000BECD4 File Offset: 0x000BCED4
	public string[] expectedUsers
	{
		get
		{
			return this.expectedUsersField;
		}
	}

	// Token: 0x170006CE RID: 1742
	// (get) Token: 0x06002620 RID: 9760 RVA: 0x000BECDC File Offset: 0x000BCEDC
	// (set) Token: 0x06002621 RID: 9761 RVA: 0x000BECE4 File Offset: 0x000BCEE4
	protected internal int masterClientId
	{
		get
		{
			return this.masterClientIdField;
		}
		set
		{
			this.masterClientIdField = value;
		}
	}

	// Token: 0x06002622 RID: 9762 RVA: 0x000BECF0 File Offset: 0x000BCEF0
	public void SetCustomProperties(Hashtable propertiesToSet, Hashtable expectedValues = null, bool webForward = false)
	{
		if (propertiesToSet == null)
		{
			return;
		}
		Hashtable hashtable = propertiesToSet.StripToStringKeys();
		Hashtable hashtable2 = expectedValues.StripToStringKeys();
		bool flag = hashtable2 == null || hashtable2.Count == 0;
		if (!PhotonNetwork.offlineMode)
		{
			PhotonNetwork.networkingPeer.OpSetPropertiesOfRoom(hashtable, hashtable2, webForward);
		}
		if (PhotonNetwork.offlineMode || flag)
		{
			base.InternalCacheProperties(hashtable);
			NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnPhotonCustomRoomPropertiesChanged, new object[]
			{
				hashtable
			});
		}
	}

	// Token: 0x06002623 RID: 9763 RVA: 0x000BED68 File Offset: 0x000BCF68
	public void SetPropertiesListedInLobby(string[] propsListedInLobby)
	{
		Hashtable hashtable = new Hashtable();
		hashtable[250] = propsListedInLobby;
		PhotonNetwork.networkingPeer.OpSetPropertiesOfRoom(hashtable, null, false);
		this.propertiesListedInLobby = propsListedInLobby;
	}

	// Token: 0x06002624 RID: 9764 RVA: 0x000BEDA4 File Offset: 0x000BCFA4
	public void ClearExpectedUsers()
	{
		Hashtable hashtable = new Hashtable();
		hashtable[247] = new string[0];
		Hashtable hashtable2 = new Hashtable();
		hashtable2[247] = this.expectedUsers;
		PhotonNetwork.networkingPeer.OpSetPropertiesOfRoom(hashtable, hashtable2, false);
	}

	// Token: 0x06002625 RID: 9765 RVA: 0x000BEDF8 File Offset: 0x000BCFF8
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

	// Token: 0x06002626 RID: 9766 RVA: 0x000BEE74 File Offset: 0x000BD074
	public new string ToStringFull()
	{
		return string.Format("Room: '{0}' {1},{2} {4}/{3} players.\ncustomProps: {5}", new object[]
		{
			this.nameField,
			(!this.visibleField) ? "hidden" : "visible",
			(!this.openField) ? "closed" : "open",
			this.maxPlayersField,
			this.playerCount,
			base.customProperties.ToStringFull()
		});
	}
}
