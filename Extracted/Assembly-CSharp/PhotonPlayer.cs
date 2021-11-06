using System;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using UnityEngine;

// Token: 0x02000417 RID: 1047
public class PhotonPlayer : IComparable<PhotonPlayer>, IComparable<int>, IEquatable<PhotonPlayer>, IEquatable<int>
{
	// Token: 0x060025C1 RID: 9665 RVA: 0x000BD1A8 File Offset: 0x000BB3A8
	public PhotonPlayer(bool isLocal, int actorID, string name)
	{
		this.customProperties = new Hashtable();
		this.isLocal = isLocal;
		this.actorID = actorID;
		this.nameField = name;
	}

	// Token: 0x060025C2 RID: 9666 RVA: 0x000BD1F0 File Offset: 0x000BB3F0
	protected internal PhotonPlayer(bool isLocal, int actorID, Hashtable properties)
	{
		this.customProperties = new Hashtable();
		this.isLocal = isLocal;
		this.actorID = actorID;
		this.InternalCacheProperties(properties);
	}

	// Token: 0x170006B4 RID: 1716
	// (get) Token: 0x060025C3 RID: 9667 RVA: 0x000BD238 File Offset: 0x000BB438
	public int ID
	{
		get
		{
			return this.actorID;
		}
	}

	// Token: 0x170006B5 RID: 1717
	// (get) Token: 0x060025C4 RID: 9668 RVA: 0x000BD240 File Offset: 0x000BB440
	// (set) Token: 0x060025C5 RID: 9669 RVA: 0x000BD248 File Offset: 0x000BB448
	public string name
	{
		get
		{
			return this.nameField;
		}
		set
		{
			if (!this.isLocal)
			{
				Debug.LogError("Error: Cannot change the name of a remote player!");
				return;
			}
			if (string.IsNullOrEmpty(value) || value.Equals(this.nameField))
			{
				return;
			}
			this.nameField = value;
			PhotonNetwork.playerName = value;
		}
	}

	// Token: 0x170006B6 RID: 1718
	// (get) Token: 0x060025C6 RID: 9670 RVA: 0x000BD298 File Offset: 0x000BB498
	// (set) Token: 0x060025C7 RID: 9671 RVA: 0x000BD2A0 File Offset: 0x000BB4A0
	public string userId { get; internal set; }

	// Token: 0x170006B7 RID: 1719
	// (get) Token: 0x060025C8 RID: 9672 RVA: 0x000BD2AC File Offset: 0x000BB4AC
	public bool isMasterClient
	{
		get
		{
			return PhotonNetwork.networkingPeer.mMasterClientId == this.ID;
		}
	}

	// Token: 0x170006B8 RID: 1720
	// (get) Token: 0x060025C9 RID: 9673 RVA: 0x000BD2C0 File Offset: 0x000BB4C0
	// (set) Token: 0x060025CA RID: 9674 RVA: 0x000BD2C8 File Offset: 0x000BB4C8
	public bool isInactive { get; set; }

	// Token: 0x170006B9 RID: 1721
	// (get) Token: 0x060025CB RID: 9675 RVA: 0x000BD2D4 File Offset: 0x000BB4D4
	// (set) Token: 0x060025CC RID: 9676 RVA: 0x000BD2DC File Offset: 0x000BB4DC
	public Hashtable customProperties { get; internal set; }

	// Token: 0x170006BA RID: 1722
	// (get) Token: 0x060025CD RID: 9677 RVA: 0x000BD2E8 File Offset: 0x000BB4E8
	public Hashtable allProperties
	{
		get
		{
			Hashtable hashtable = new Hashtable();
			hashtable.Merge(this.customProperties);
			hashtable[byte.MaxValue] = this.name;
			return hashtable;
		}
	}

	// Token: 0x060025CE RID: 9678 RVA: 0x000BD320 File Offset: 0x000BB520
	public override bool Equals(object p)
	{
		PhotonPlayer photonPlayer = p as PhotonPlayer;
		return photonPlayer != null && this.GetHashCode() == photonPlayer.GetHashCode();
	}

	// Token: 0x060025CF RID: 9679 RVA: 0x000BD34C File Offset: 0x000BB54C
	public override int GetHashCode()
	{
		return this.ID;
	}

	// Token: 0x060025D0 RID: 9680 RVA: 0x000BD354 File Offset: 0x000BB554
	internal void InternalChangeLocalID(int newID)
	{
		if (!this.isLocal)
		{
			Debug.LogError("ERROR You should never change PhotonPlayer IDs!");
			return;
		}
		this.actorID = newID;
	}

	// Token: 0x060025D1 RID: 9681 RVA: 0x000BD374 File Offset: 0x000BB574
	internal void InternalCacheProperties(Hashtable properties)
	{
		if (properties == null || properties.Count == 0 || this.customProperties.Equals(properties))
		{
			return;
		}
		if (properties.ContainsKey(255))
		{
			this.nameField = (string)properties[byte.MaxValue];
		}
		if (properties.ContainsKey(253))
		{
			this.userId = (string)properties[253];
		}
		if (properties.ContainsKey(254))
		{
			this.isInactive = (bool)properties[254];
		}
		this.customProperties.MergeStringKeys(properties);
		this.customProperties.StripKeysWithNullValues();
	}

	// Token: 0x060025D2 RID: 9682 RVA: 0x000BD44C File Offset: 0x000BB64C
	public void SetCustomProperties(Hashtable propertiesToSet, Hashtable expectedValues = null, bool webForward = false)
	{
		if (propertiesToSet == null)
		{
			return;
		}
		Hashtable hashtable = propertiesToSet.StripToStringKeys();
		Hashtable hashtable2 = expectedValues.StripToStringKeys();
		bool flag = hashtable2 == null || hashtable2.Count == 0;
		bool flag2 = this.actorID > 0 && !PhotonNetwork.offlineMode;
		if (flag2)
		{
			PhotonNetwork.networkingPeer.OpSetPropertiesOfActor(this.actorID, hashtable, hashtable2, webForward);
		}
		if (!flag2 || flag)
		{
			this.InternalCacheProperties(hashtable);
			NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnPhotonPlayerPropertiesChanged, new object[]
			{
				this,
				hashtable
			});
		}
	}

	// Token: 0x060025D3 RID: 9683 RVA: 0x000BD4DC File Offset: 0x000BB6DC
	public static PhotonPlayer Find(int ID)
	{
		if (PhotonNetwork.networkingPeer != null)
		{
			return PhotonNetwork.networkingPeer.GetPlayerWithId(ID);
		}
		return null;
	}

	// Token: 0x060025D4 RID: 9684 RVA: 0x000BD4F8 File Offset: 0x000BB6F8
	public PhotonPlayer Get(int id)
	{
		return PhotonPlayer.Find(id);
	}

	// Token: 0x060025D5 RID: 9685 RVA: 0x000BD500 File Offset: 0x000BB700
	public PhotonPlayer GetNext()
	{
		return this.GetNextFor(this.ID);
	}

	// Token: 0x060025D6 RID: 9686 RVA: 0x000BD510 File Offset: 0x000BB710
	public PhotonPlayer GetNextFor(PhotonPlayer currentPlayer)
	{
		if (currentPlayer == null)
		{
			return null;
		}
		return this.GetNextFor(currentPlayer.ID);
	}

	// Token: 0x060025D7 RID: 9687 RVA: 0x000BD528 File Offset: 0x000BB728
	public PhotonPlayer GetNextFor(int currentPlayerId)
	{
		if (PhotonNetwork.networkingPeer == null || PhotonNetwork.networkingPeer.mActors == null || PhotonNetwork.networkingPeer.mActors.Count < 2)
		{
			return null;
		}
		Dictionary<int, PhotonPlayer> mActors = PhotonNetwork.networkingPeer.mActors;
		int num = int.MaxValue;
		int num2 = currentPlayerId;
		foreach (int num3 in mActors.Keys)
		{
			if (num3 < num2)
			{
				num2 = num3;
			}
			else if (num3 > currentPlayerId && num3 < num)
			{
				num = num3;
			}
		}
		return (num == int.MaxValue) ? mActors[num2] : mActors[num];
	}

	// Token: 0x060025D8 RID: 9688 RVA: 0x000BD608 File Offset: 0x000BB808
	public int CompareTo(PhotonPlayer other)
	{
		if (other == null)
		{
			return 0;
		}
		return this.GetHashCode().CompareTo(other.GetHashCode());
	}

	// Token: 0x060025D9 RID: 9689 RVA: 0x000BD634 File Offset: 0x000BB834
	public int CompareTo(int other)
	{
		return this.GetHashCode().CompareTo(other);
	}

	// Token: 0x060025DA RID: 9690 RVA: 0x000BD650 File Offset: 0x000BB850
	public bool Equals(PhotonPlayer other)
	{
		return other != null && this.GetHashCode().Equals(other.GetHashCode());
	}

	// Token: 0x060025DB RID: 9691 RVA: 0x000BD67C File Offset: 0x000BB87C
	public bool Equals(int other)
	{
		return this.GetHashCode().Equals(other);
	}

	// Token: 0x060025DC RID: 9692 RVA: 0x000BD698 File Offset: 0x000BB898
	public override string ToString()
	{
		if (string.IsNullOrEmpty(this.name))
		{
			return string.Format("#{0:00}{1}{2}", this.ID, (!this.isInactive) ? " " : " (inactive)", (!this.isMasterClient) ? string.Empty : "(master)");
		}
		return string.Format("'{0}'{1}{2}", this.name, (!this.isInactive) ? " " : " (inactive)", (!this.isMasterClient) ? string.Empty : "(master)");
	}

	// Token: 0x060025DD RID: 9693 RVA: 0x000BD744 File Offset: 0x000BB944
	public string ToStringFull()
	{
		return string.Format("#{0:00} '{1}'{2} {3}", new object[]
		{
			this.ID,
			this.name,
			(!this.isInactive) ? string.Empty : " (inactive)",
			this.customProperties.ToStringFull()
		});
	}

	// Token: 0x04001A3E RID: 6718
	private int actorID = -1;

	// Token: 0x04001A3F RID: 6719
	private string nameField = string.Empty;

	// Token: 0x04001A40 RID: 6720
	public readonly bool isLocal;

	// Token: 0x04001A41 RID: 6721
	public object TagObject;
}
