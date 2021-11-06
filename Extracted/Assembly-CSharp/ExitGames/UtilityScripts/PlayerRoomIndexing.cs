using System;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon;
using UnityEngine;

namespace ExitGames.UtilityScripts
{
	// Token: 0x0200044C RID: 1100
	public class PlayerRoomIndexing : PunBehaviour
	{
		// Token: 0x170006E1 RID: 1761
		// (get) Token: 0x060026E1 RID: 9953 RVA: 0x000C2E4C File Offset: 0x000C104C
		public int[] PlayerIds
		{
			get
			{
				return this._playerIds;
			}
		}

		// Token: 0x060026E2 RID: 9954 RVA: 0x000C2E54 File Offset: 0x000C1054
		public void Awake()
		{
			if (PlayerRoomIndexing.instance != null)
			{
				Debug.LogError("Existing instance of PlayerRoomIndexing found. Only One instance is required at the most. Please correct and have only one at any time.");
			}
			PlayerRoomIndexing.instance = this;
		}

		// Token: 0x060026E3 RID: 9955 RVA: 0x000C2E84 File Offset: 0x000C1084
		public override void OnJoinedRoom()
		{
			if (PhotonNetwork.isMasterClient)
			{
				this.AssignIndex(PhotonNetwork.player);
			}
			else
			{
				this.RefreshData();
			}
		}

		// Token: 0x060026E4 RID: 9956 RVA: 0x000C2EB4 File Offset: 0x000C10B4
		public override void OnLeftRoom()
		{
			this.RefreshData();
		}

		// Token: 0x060026E5 RID: 9957 RVA: 0x000C2EBC File Offset: 0x000C10BC
		public override void OnPhotonPlayerConnected(PhotonPlayer newPlayer)
		{
			if (PhotonNetwork.isMasterClient)
			{
				this.AssignIndex(newPlayer);
			}
		}

		// Token: 0x060026E6 RID: 9958 RVA: 0x000C2ED0 File Offset: 0x000C10D0
		public override void OnPhotonPlayerDisconnected(PhotonPlayer otherPlayer)
		{
			if (PhotonNetwork.isMasterClient)
			{
				this.UnAssignIndex(otherPlayer);
			}
		}

		// Token: 0x060026E7 RID: 9959 RVA: 0x000C2EE4 File Offset: 0x000C10E4
		public override void OnPhotonCustomRoomPropertiesChanged(Hashtable propertiesThatChanged)
		{
			if (propertiesThatChanged.ContainsKey("PlayerIndexes"))
			{
				this.RefreshData();
			}
		}

		// Token: 0x060026E8 RID: 9960 RVA: 0x000C2EFC File Offset: 0x000C10FC
		public int GetRoomIndex(PhotonPlayer player)
		{
			if (this._indexesLUT != null && this._indexesLUT.ContainsKey(player.ID))
			{
				return this._indexesLUT[player.ID];
			}
			return -1;
		}

		// Token: 0x060026E9 RID: 9961 RVA: 0x000C2F40 File Offset: 0x000C1140
		private void RefreshData()
		{
			if (PhotonNetwork.room != null)
			{
				this._playerIds = new int[PhotonNetwork.room.maxPlayers];
				if (PhotonNetwork.room.customProperties.TryGetValue("PlayerIndexes", out this._indexes))
				{
					this._indexesLUT = (this._indexes as Dictionary<int, int>);
					foreach (KeyValuePair<int, int> keyValuePair in this._indexesLUT)
					{
						this._p = PhotonPlayer.Find(keyValuePair.Key);
						this._playerIds[keyValuePair.Value] = this._p.ID;
					}
				}
			}
			else
			{
				this._playerIds = new int[0];
			}
			if (this.OnRoomIndexingChanged != null)
			{
				this.OnRoomIndexingChanged();
			}
		}

		// Token: 0x060026EA RID: 9962 RVA: 0x000C3040 File Offset: 0x000C1240
		private void AssignIndex(PhotonPlayer player)
		{
			if (PhotonNetwork.room.customProperties.TryGetValue("PlayerIndexes", out this._indexes))
			{
				this._indexesLUT = (this._indexes as Dictionary<int, int>);
			}
			else
			{
				this._indexesLUT = new Dictionary<int, int>();
			}
			List<bool> list = new List<bool>(new bool[PhotonNetwork.room.maxPlayers]);
			foreach (KeyValuePair<int, int> keyValuePair in this._indexesLUT)
			{
				list[keyValuePair.Value] = true;
			}
			this._indexesLUT[player.ID] = Mathf.Max(0, list.IndexOf(false));
			PhotonNetwork.room.SetCustomProperties(new Hashtable
			{
				{
					"PlayerIndexes",
					this._indexesLUT
				}
			}, null, false);
			this.RefreshData();
		}

		// Token: 0x060026EB RID: 9963 RVA: 0x000C314C File Offset: 0x000C134C
		private void UnAssignIndex(PhotonPlayer player)
		{
			if (PhotonNetwork.room.customProperties.TryGetValue("PlayerIndexes", out this._indexes))
			{
				this._indexesLUT = (this._indexes as Dictionary<int, int>);
				this._indexesLUT.Remove(player.ID);
				PhotonNetwork.room.SetCustomProperties(new Hashtable
				{
					{
						"PlayerIndexes",
						this._indexesLUT
					}
				}, null, false);
			}
			this.RefreshData();
		}

		// Token: 0x04001B58 RID: 7000
		public const string RoomPlayerIndexedProp = "PlayerIndexes";

		// Token: 0x04001B59 RID: 7001
		public static PlayerRoomIndexing instance;

		// Token: 0x04001B5A RID: 7002
		public PlayerRoomIndexing.RoomIndexingChanged OnRoomIndexingChanged;

		// Token: 0x04001B5B RID: 7003
		private int[] _playerIds;

		// Token: 0x04001B5C RID: 7004
		private object _indexes;

		// Token: 0x04001B5D RID: 7005
		private Dictionary<int, int> _indexesLUT;

		// Token: 0x04001B5E RID: 7006
		private List<bool> _indexesPool;

		// Token: 0x04001B5F RID: 7007
		private PhotonPlayer _p;

		// Token: 0x02000916 RID: 2326
		// (Invoke) Token: 0x060050F8 RID: 20728
		public delegate void RoomIndexingChanged();
	}
}
