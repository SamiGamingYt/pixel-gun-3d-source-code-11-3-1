using System;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon;
using UnityEngine;

// Token: 0x02000457 RID: 1111
public class PunTurnManager : PunBehaviour
{
	// Token: 0x170006E3 RID: 1763
	// (get) Token: 0x06002718 RID: 10008 RVA: 0x000C3E78 File Offset: 0x000C2078
	// (set) Token: 0x06002719 RID: 10009 RVA: 0x000C3E84 File Offset: 0x000C2084
	public int Turn
	{
		get
		{
			return PhotonNetwork.room.GetTurn();
		}
		private set
		{
			this._isOverCallProcessed = false;
			PhotonNetwork.room.SetTurn(value, true);
		}
	}

	// Token: 0x170006E4 RID: 1764
	// (get) Token: 0x0600271A RID: 10010 RVA: 0x000C3E9C File Offset: 0x000C209C
	public float ElapsedTimeInTurn
	{
		get
		{
			return (float)(PhotonNetwork.ServerTimestamp - PhotonNetwork.room.GetTurnStart()) / 1000f;
		}
	}

	// Token: 0x170006E5 RID: 1765
	// (get) Token: 0x0600271B RID: 10011 RVA: 0x000C3EB8 File Offset: 0x000C20B8
	public float RemainingSecondsInTurn
	{
		get
		{
			return Mathf.Max(0f, this.TurnDuration - this.ElapsedTimeInTurn);
		}
	}

	// Token: 0x170006E6 RID: 1766
	// (get) Token: 0x0600271C RID: 10012 RVA: 0x000C3ED4 File Offset: 0x000C20D4
	public bool IsCompletedByAll
	{
		get
		{
			return PhotonNetwork.room != null && this.Turn > 0 && this.finishedPlayers.Count == PhotonNetwork.room.playerCount;
		}
	}

	// Token: 0x170006E7 RID: 1767
	// (get) Token: 0x0600271D RID: 10013 RVA: 0x000C3F14 File Offset: 0x000C2114
	public bool IsFinishedByMe
	{
		get
		{
			return this.finishedPlayers.Contains(PhotonNetwork.player);
		}
	}

	// Token: 0x170006E8 RID: 1768
	// (get) Token: 0x0600271E RID: 10014 RVA: 0x000C3F28 File Offset: 0x000C2128
	public bool IsOver
	{
		get
		{
			return this.RemainingSecondsInTurn <= 0f;
		}
	}

	// Token: 0x0600271F RID: 10015 RVA: 0x000C3F3C File Offset: 0x000C213C
	private void Start()
	{
		PhotonNetwork.OnEventCall = new PhotonNetwork.EventCallback(this.OnEvent);
	}

	// Token: 0x06002720 RID: 10016 RVA: 0x000C3F50 File Offset: 0x000C2150
	private void Update()
	{
		if (this.Turn > 0 && this.IsOver && !this._isOverCallProcessed)
		{
			this._isOverCallProcessed = true;
			this.TurnManagerListener.OnTurnTimeEnds(this.Turn);
		}
	}

	// Token: 0x06002721 RID: 10017 RVA: 0x000C3F98 File Offset: 0x000C2198
	public void BeginTurn()
	{
		this.Turn++;
	}

	// Token: 0x06002722 RID: 10018 RVA: 0x000C3FA8 File Offset: 0x000C21A8
	public void SendMove(object move, bool finished)
	{
		if (this.IsFinishedByMe)
		{
			Debug.LogWarning("Can't SendMove. Turn is finished by this player.");
			return;
		}
		Hashtable hashtable = new Hashtable();
		hashtable.Add("turn", this.Turn);
		hashtable.Add("move", move);
		byte eventCode = (!finished) ? 1 : 2;
		PhotonNetwork.RaiseEvent(eventCode, hashtable, true, new RaiseEventOptions
		{
			CachingOption = EventCaching.AddToRoomCache
		});
		if (finished)
		{
			PhotonNetwork.player.SetFinishedTurn(this.Turn);
		}
		this.OnEvent(eventCode, hashtable, PhotonNetwork.player.ID);
	}

	// Token: 0x06002723 RID: 10019 RVA: 0x000C4040 File Offset: 0x000C2240
	public bool GetPlayerFinishedTurn(PhotonPlayer player)
	{
		return player != null && this.finishedPlayers != null && this.finishedPlayers.Contains(player);
	}

	// Token: 0x06002724 RID: 10020 RVA: 0x000C4068 File Offset: 0x000C2268
	public void OnEvent(byte eventCode, object content, int senderId)
	{
		PhotonPlayer photonPlayer = PhotonPlayer.Find(senderId);
		if (eventCode != 1)
		{
			if (eventCode == 2)
			{
				Hashtable hashtable = content as Hashtable;
				int num = (int)hashtable["turn"];
				object move = hashtable["move"];
				if (num == this.Turn)
				{
					this.finishedPlayers.Add(photonPlayer);
					this.TurnManagerListener.OnPlayerFinished(photonPlayer, num, move);
				}
				if (this.IsCompletedByAll)
				{
					this.TurnManagerListener.OnTurnCompleted(this.Turn);
				}
			}
		}
		else
		{
			Hashtable hashtable2 = content as Hashtable;
			int turn = (int)hashtable2["turn"];
			object move2 = hashtable2["move"];
			this.TurnManagerListener.OnPlayerMove(photonPlayer, turn, move2);
		}
	}

	// Token: 0x06002725 RID: 10021 RVA: 0x000C4140 File Offset: 0x000C2340
	public override void OnPhotonCustomRoomPropertiesChanged(Hashtable propertiesThatChanged)
	{
		if (propertiesThatChanged.ContainsKey("Turn"))
		{
			this._isOverCallProcessed = false;
			this.finishedPlayers.Clear();
			this.TurnManagerListener.OnTurnBegins(this.Turn);
		}
	}

	// Token: 0x04001B73 RID: 7027
	public const byte TurnManagerEventOffset = 0;

	// Token: 0x04001B74 RID: 7028
	public const byte EvMove = 1;

	// Token: 0x04001B75 RID: 7029
	public const byte EvFinalMove = 2;

	// Token: 0x04001B76 RID: 7030
	public float TurnDuration = 20f;

	// Token: 0x04001B77 RID: 7031
	public IPunTurnManagerCallbacks TurnManagerListener;

	// Token: 0x04001B78 RID: 7032
	private readonly HashSet<PhotonPlayer> finishedPlayers = new HashSet<PhotonPlayer>();

	// Token: 0x04001B79 RID: 7033
	private bool _isOverCallProcessed;
}
