using System;
using ExitGames.Client.Photon;
using Rilisoft;
using UnityEngine;

// Token: 0x0200009E RID: 158
public class DuelController : MonoBehaviour
{
	// Token: 0x17000046 RID: 70
	// (get) Token: 0x0600046B RID: 1131 RVA: 0x00025344 File Offset: 0x00023544
	public float timeLeft
	{
		get
		{
			return this._timeLeft;
		}
	}

	// Token: 0x17000047 RID: 71
	// (get) Token: 0x0600046C RID: 1132 RVA: 0x0002534C File Offset: 0x0002354C
	public float playingTime
	{
		get
		{
			return 120f - this._timeLeft;
		}
	}

	// Token: 0x17000048 RID: 72
	// (get) Token: 0x0600046D RID: 1133 RVA: 0x0002535C File Offset: 0x0002355C
	// (set) Token: 0x0600046E RID: 1134 RVA: 0x00025378 File Offset: 0x00023578
	[HideInInspector]
	public DuelController.RoomStatus roomStatus
	{
		get
		{
			return (DuelController.RoomStatus)((int)PhotonNetwork.room.customProperties[ConnectSceneNGUIController.roomStatusProperty]);
		}
		set
		{
			Hashtable hashtable = new Hashtable();
			hashtable["Closed"] = (int)value;
			PhotonNetwork.room.SetCustomProperties(hashtable, null, false);
		}
	}

	// Token: 0x17000049 RID: 73
	// (get) Token: 0x0600046F RID: 1135 RVA: 0x000253AC File Offset: 0x000235AC
	public bool showEnemyCharacter
	{
		get
		{
			return (this.opponentNetworkTable != null || this.opponentLeftInEnd) && this.duelUI != null && this.duelUI.showCharacters && this.gameStatus != DuelController.GameStatus.WaitForOpponent && this.gameStatus != DuelController.GameStatus.OpponentConnected && this.gameStatus != DuelController.GameStatus.ChangeArea;
		}
	}

	// Token: 0x1700004A RID: 74
	// (get) Token: 0x06000470 RID: 1136 RVA: 0x00025420 File Offset: 0x00023620
	public bool showMyCharacter
	{
		get
		{
			return this.duelUI != null && this.duelUI.showCharacters;
		}
	}

	// Token: 0x1700004B RID: 75
	// (get) Token: 0x06000471 RID: 1137 RVA: 0x00025444 File Offset: 0x00023644
	public DuelUIController duelUI
	{
		get
		{
			return (!(NetworkStartTableNGUIController.sharedController != null)) ? null : NetworkStartTableNGUIController.sharedController.duelUI;
		}
	}

	// Token: 0x1700004C RID: 76
	// (get) Token: 0x06000472 RID: 1138 RVA: 0x00025474 File Offset: 0x00023674
	public bool isMaster
	{
		get
		{
			return PhotonNetwork.isMasterClient;
		}
	}

	// Token: 0x06000473 RID: 1139 RVA: 0x0002547C File Offset: 0x0002367C
	private void OnDestroy()
	{
		PhotonObjectCacher.RemoveObject(base.gameObject);
		ShopNGUIController.sharedShop.wearEquipAction = null;
		ShopNGUIController.sharedShop.wearUnequipAction = null;
		ShopNGUIController.sharedShop.equipAction = null;
		ShopNGUIController.sharedShop.onEquipSkinAction = null;
		ShopNGUIController.ShowWearChanged -= this.OnMyWearVisibleChanged;
		ShopNGUIController.ShowArmorChanged -= this.OnMyArmorVisibleChanged;
		if (this.equippedPetActionSet)
		{
			ShopNGUIController.EquippedPet -= this.OnPetEquipAction;
			ShopNGUIController.UnequippedPet -= this.OnPetUnequipAction;
		}
	}

	// Token: 0x06000474 RID: 1140 RVA: 0x00025510 File Offset: 0x00023710
	private void Awake()
	{
		base.enabled = (Defs.isDuel && Defs.isMulti);
		DuelController.instance = this;
		this.photonView = base.GetComponent<PhotonView>();
		PhotonObjectCacher.AddObject(base.gameObject);
	}

	// Token: 0x06000475 RID: 1141 RVA: 0x00025554 File Offset: 0x00023754
	private void Start()
	{
		this.SetShopEvents();
	}

	// Token: 0x06000476 RID: 1142 RVA: 0x0002555C File Offset: 0x0002375C
	public void StartDuelMode()
	{
		if (this.myCharacter == null)
		{
			this.myCharacter = this.CreateCharacter(this.myCharacterPoint);
			this.myCharacter.isDuelInstance = true;
		}
		if (this.enemyCharacter == null)
		{
			this.enemyCharacter = this.CreateCharacter(this.enemyCharacterPoint);
			this.enemyCharacter.isDuelInstance = true;
			this.enemyCharacter.enemyInDuel = true;
		}
		this.SetMySkin();
		this.SetMyCape();
		this.SetMyMask();
		this.SetMyHat();
		this.SetMyBoots();
		this.SetMyWeapon();
		this.SetMyArmor();
		this.SetMyPet();
		if (this.gameStatus == DuelController.GameStatus.None)
		{
			this.gameStatus = DuelController.GameStatus.WaitForOpponent;
		}
	}

	// Token: 0x06000477 RID: 1143 RVA: 0x00025618 File Offset: 0x00023818
	private void StartMatch()
	{
		float num = 120f;
		PhotonNetwork.room.visible = false;
		this.roomHidden = true;
		this.ChangeRoomStatus(DuelController.RoomStatus.MatchStarted);
		this.StartMatchRPC(num, this.myRespawnPoints);
		this.photonView.RPC("StartMatchRPC", PhotonTargets.Others, new object[]
		{
			num,
			(this.myRespawnPoints != 2) ? 2 : 1
		});
	}

	// Token: 0x06000478 RID: 1144 RVA: 0x0002568C File Offset: 0x0002388C
	public void StartRevengeMatch()
	{
		this.gameStatus = DuelController.GameStatus.OpponentConnected;
		this.GoMatch();
	}

	// Token: 0x06000479 RID: 1145 RVA: 0x0002569C File Offset: 0x0002389C
	private void GoMatch()
	{
		float num = 5f;
		this.GoMatchRPC(num);
		this.photonView.RPC("GoMatchRPC", PhotonTargets.Others, new object[]
		{
			num
		});
	}

	// Token: 0x0600047A RID: 1146 RVA: 0x000256D8 File Offset: 0x000238D8
	[PunRPC]
	private void GoMatchRPC(float timer)
	{
		this.ChangeRoomStatus(DuelController.RoomStatus.None);
		this.requestSended = false;
		this.requestReceived = false;
		this.duelUI.revengeButton.GetComponent<UIButton>().isEnabled = true;
		this.duelUI.ShowVersusUI();
		this.gameStatus = DuelController.GameStatus.ReadyToStart;
		this.goTimer = timer;
	}

	// Token: 0x0600047B RID: 1147 RVA: 0x0002572C File Offset: 0x0002392C
	private void ResumeMatch()
	{
		this.gameStatus = DuelController.GameStatus.Playing;
		this.photonView.RPC("ResumeMatchRPC", PhotonTargets.Others, new object[]
		{
			this.timeLeft,
			(this.myRespawnPoints != 2) ? 2 : 1
		});
	}

	// Token: 0x0600047C RID: 1148 RVA: 0x00025780 File Offset: 0x00023980
	[PunRPC]
	public void ResumeMatchRPC(float matchTime, int spawnPoint)
	{
		this.myRespawnPoints = spawnPoint;
		if (this.gameStatus == DuelController.GameStatus.OpponentConnected || this.gameStatus == DuelController.GameStatus.ReadyToStart)
		{
			this.StartMatchRPC(matchTime, spawnPoint);
		}
		else if (this.gameStatus == DuelController.GameStatus.DisconnectInMatch)
		{
			this.gameStatus = DuelController.GameStatus.Playing;
			this._timeLeft = matchTime;
		}
	}

	// Token: 0x0600047D RID: 1149 RVA: 0x000257D4 File Offset: 0x000239D4
	[PunRPC]
	public void StartMatchRPC(float matchTime, int spawnPoint)
	{
		this.myRespawnPoints = spawnPoint;
		this.StartPlayer();
		this._timeLeft = matchTime;
	}

	// Token: 0x0600047E RID: 1150 RVA: 0x000257EC File Offset: 0x000239EC
	private void StartPlayer()
	{
		if (WeaponManager.sharedManager.myPlayerMoveC == null)
		{
			WeaponManager.sharedManager.myNetworkStartTable.StartPlayerButtonClick(0);
		}
		this.duelUI.revengeButton.GetComponent<UIButton>().isEnabled = true;
		this.gameStatus = DuelController.GameStatus.Playing;
		this.duelUI.IngameUI();
	}

	// Token: 0x0600047F RID: 1151 RVA: 0x00025848 File Offset: 0x00023A48
	[PunRPC]
	public void SynchronizeTimeRPC(float matchTime)
	{
		if (this.isMaster)
		{
			return;
		}
		this._timeLeft = matchTime;
	}

	// Token: 0x06000480 RID: 1152 RVA: 0x00025860 File Offset: 0x00023A60
	private void EndMatch()
	{
		this.SetMyWeapon();
		this.SendMyWeapon();
		this.gameStatus = DuelController.GameStatus.End;
		if (WeaponManager.sharedManager.myTable != null)
		{
			WeaponManager.sharedManager.myNetworkStartTable.win(string.Empty, 0, 0, 0);
		}
		this.ChangeRoomStatus(DuelController.RoomStatus.Closed);
	}

	// Token: 0x06000481 RID: 1153 RVA: 0x000258B4 File Offset: 0x00023AB4
	public void OpponentConnected()
	{
		this.SendMyWearInvisible();
		this.SendMySkin();
		this.SendMyCape();
		this.SendMyMask();
		this.SendMyHat();
		this.SendMyBoots();
		this.SendMyWeapon();
		this.SendMyArmor();
		this.SendMyPet();
		if (this.gameStatus == DuelController.GameStatus.WaitForOpponent)
		{
			this.gameStatus = DuelController.GameStatus.OpponentConnected;
			this.goTimer = 2f;
		}
	}

	// Token: 0x06000482 RID: 1154 RVA: 0x00025918 File Offset: 0x00023B18
	public void RevengeRequest()
	{
		if (this.requestSended)
		{
			return;
		}
		this.requestSended = true;
		this.photonView.RPC("RevengeRequestRPC", PhotonTargets.Others, new object[0]);
		this.duelUI.ShowRevengePanel(false, false);
		this.duelUI.revengeButton.GetComponent<UIButton>().isEnabled = false;
		if (this.requestReceived && this.isMaster)
		{
			this.StartRevengeMatch();
		}
	}

	// Token: 0x06000483 RID: 1155 RVA: 0x00025990 File Offset: 0x00023B90
	[PunRPC]
	public void RevengeRequestRPC()
	{
		this.requestReceived = true;
		if (!this.requestSended)
		{
			this.duelUI.ShowRevengePanel(true, false);
		}
		if (this.requestSended && this.isMaster)
		{
			this.StartRevengeMatch();
		}
	}

	// Token: 0x06000484 RID: 1156 RVA: 0x000259D0 File Offset: 0x00023BD0
	[PunRPC]
	public void SetWearIsInvisibleRPC(bool isInvisible)
	{
		if (this._wearIsInvisible == isInvisible)
		{
			return;
		}
		this._wearIsInvisible = isInvisible;
		this.enemyCharacter.UpdateCape(this.enemyCharacter.CurrentCapeId, this.enemyCharacter.CurrentCapeTexture, isInvisible);
		this.enemyCharacter.UpdateBoots(this.enemyCharacter.CurrentBootsId, isInvisible);
		this.enemyCharacter.UpdateHat(this.enemyCharacter.CurrentHatId, isInvisible);
		this.enemyCharacter.UpdateMask(this.enemyCharacter.CurrentMaskId, isInvisible);
	}

	// Token: 0x06000485 RID: 1157 RVA: 0x00025A58 File Offset: 0x00023C58
	[PunRPC]
	public void SetEnemySkin(byte[] skin)
	{
		Texture2D texture2D = new Texture2D(64, 32);
		texture2D.LoadImage(skin);
		texture2D.filterMode = FilterMode.Point;
		texture2D.Apply();
		this.enemyCharacter.SetSkin(texture2D, (!(this.enemyCharacter.weapon != null)) ? null : this.enemyCharacter.weapon.GetComponent<WeaponSounds>(), null);
	}

	// Token: 0x06000486 RID: 1158 RVA: 0x00025AC0 File Offset: 0x00023CC0
	[PunRPC]
	public void SetEnemyCape(string cape)
	{
		this.enemyCharacter.UpdateCape(cape, null, this._wearIsInvisible);
	}

	// Token: 0x06000487 RID: 1159 RVA: 0x00025AD8 File Offset: 0x00023CD8
	[PunRPC]
	public void SetEnemyCape(string cape, byte[] skin)
	{
		Texture2D texture2D = new Texture2D(12, 16);
		texture2D.LoadImage(skin);
		texture2D.filterMode = FilterMode.Point;
		texture2D.Apply();
		this.enemyCharacter.UpdateCape(cape, texture2D, this._wearIsInvisible);
	}

	// Token: 0x06000488 RID: 1160 RVA: 0x00025B18 File Offset: 0x00023D18
	[PunRPC]
	public void SetEnemyArmor(string armor, bool isInvisible)
	{
		this.enemyCharacter.UpdateArmor(armor, this.enemyCharacter.weapon, isInvisible || this._wearIsInvisible);
	}

	// Token: 0x06000489 RID: 1161 RVA: 0x00025B4C File Offset: 0x00023D4C
	[PunRPC]
	public void SetEnemyMask(string mask)
	{
		this.enemyCharacter.UpdateMask(mask, this._wearIsInvisible);
	}

	// Token: 0x0600048A RID: 1162 RVA: 0x00025B60 File Offset: 0x00023D60
	[PunRPC]
	public void SetEnemyBoots(string boots)
	{
		this.enemyCharacter.UpdateBoots(boots, this._wearIsInvisible);
	}

	// Token: 0x0600048B RID: 1163 RVA: 0x00025B74 File Offset: 0x00023D74
	[PunRPC]
	public void SetEnemyHat(string hat)
	{
		this.enemyCharacter.UpdateHat(hat, this._wearIsInvisible);
	}

	// Token: 0x0600048C RID: 1164 RVA: 0x00025B88 File Offset: 0x00023D88
	[PunRPC]
	public void SetEnemyWeapon(string weapon, string altWeapon)
	{
		this.enemyCharacter.SetWeapon(weapon, altWeapon, string.Empty);
	}

	// Token: 0x0600048D RID: 1165 RVA: 0x00025B9C File Offset: 0x00023D9C
	[PunRPC]
	public void SetEnemyWeapon(string weapon, string altWeapon, string skin)
	{
		this.enemyCharacter.SetWeapon(weapon, altWeapon, skin);
	}

	// Token: 0x0600048E RID: 1166 RVA: 0x00025BAC File Offset: 0x00023DAC
	[PunRPC]
	public void SetEnemyPet(string petID)
	{
		this.enemyCharacter.UpdatePet(petID);
	}

	// Token: 0x0600048F RID: 1167 RVA: 0x00025BBC File Offset: 0x00023DBC
	public void SendMyWearInvisible()
	{
		this.photonView.RPC("SetWearIsInvisibleRPC", PhotonTargets.Others, new object[]
		{
			!ShopNGUIController.ShowWear
		});
	}

	// Token: 0x06000490 RID: 1168 RVA: 0x00025BF0 File Offset: 0x00023DF0
	public void SendMySkin()
	{
		this.photonView.RPC("SetEnemySkin", PhotonTargets.Others, new object[]
		{
			SkinsController.currentSkinForPers.EncodeToPNG()
		});
	}

	// Token: 0x06000491 RID: 1169 RVA: 0x00025C24 File Offset: 0x00023E24
	public void SendMyCape()
	{
		string @string = Storager.getString(Defs.CapeEquppedSN, false);
		if (!@string.Equals("cape_Custom"))
		{
			this.photonView.RPC("SetEnemyCape", PhotonTargets.Others, new object[]
			{
				@string
			});
		}
		else
		{
			Texture2D capeUserTexture = SkinsController.capeUserTexture;
			byte[] array = capeUserTexture.EncodeToPNG();
			if (capeUserTexture.width != 12 || capeUserTexture.height != 16)
			{
				return;
			}
			this.photonView.RPC("SetEnemyCape", PhotonTargets.Others, new object[]
			{
				@string,
				array
			});
		}
	}

	// Token: 0x06000492 RID: 1170 RVA: 0x00025CB4 File Offset: 0x00023EB4
	public void SendMyArmor()
	{
		string @string = Storager.getString(Defs.ArmorNewEquppedSN, false);
		bool flag = !ShopNGUIController.ShowArmor;
		this.photonView.RPC("SetEnemyArmor", PhotonTargets.Others, new object[]
		{
			@string,
			flag
		});
	}

	// Token: 0x06000493 RID: 1171 RVA: 0x00025CFC File Offset: 0x00023EFC
	public void SendMyMask()
	{
		string @string = Storager.getString("MaskEquippedSN", false);
		this.photonView.RPC("SetEnemyMask", PhotonTargets.Others, new object[]
		{
			@string
		});
	}

	// Token: 0x06000494 RID: 1172 RVA: 0x00025D30 File Offset: 0x00023F30
	public void SendMyBoots()
	{
		string @string = Storager.getString(Defs.BootsEquppedSN, false);
		this.photonView.RPC("SetEnemyBoots", PhotonTargets.Others, new object[]
		{
			@string
		});
	}

	// Token: 0x06000495 RID: 1173 RVA: 0x00025D64 File Offset: 0x00023F64
	public void SendMyHat()
	{
		string @string = Storager.getString(Defs.HatEquppedSN, false);
		this.photonView.RPC("SetEnemyHat", PhotonTargets.Others, new object[]
		{
			@string
		});
	}

	// Token: 0x06000496 RID: 1174 RVA: 0x00025D98 File Offset: 0x00023F98
	public void SendMyWeapon()
	{
		int index = WeaponManager.sharedManager.CurrentIndexOfLastUsedWeaponInPlayerWeapons();
		Weapon weapon = WeaponManager.sharedManager.playerWeapons[index] as Weapon;
		WeaponSkin skinForWeapon = WeaponSkinsManager.GetSkinForWeapon(weapon.weaponPrefab.name);
		if (skinForWeapon != null)
		{
			this.photonView.RPC("SetEnemyWeapon", PhotonTargets.Others, new object[]
			{
				weapon.weaponPrefab.name,
				weapon.weaponPrefab.GetComponent<WeaponSounds>().alternativeName,
				skinForWeapon.Id
			});
		}
		else
		{
			this.photonView.RPC("SetEnemyWeapon", PhotonTargets.Others, new object[]
			{
				weapon.weaponPrefab.name,
				weapon.weaponPrefab.GetComponent<WeaponSounds>().alternativeName
			});
		}
	}

	// Token: 0x06000497 RID: 1175 RVA: 0x00025E5C File Offset: 0x0002405C
	public void SendMyPet()
	{
		string eqipedPetId = Singleton<PetsManager>.Instance.GetEqipedPetId();
		this.photonView.RPC("SetEnemyPet", PhotonTargets.Others, new object[]
		{
			eqipedPetId
		});
	}

	// Token: 0x06000498 RID: 1176 RVA: 0x00025E90 File Offset: 0x00024090
	public void SetMySkin()
	{
		this.myCharacter.SetSkin(SkinsController.currentSkinForPers, (!(this.myCharacter.weapon != null)) ? null : this.myCharacter.weapon.GetComponent<WeaponSounds>(), null);
	}

	// Token: 0x06000499 RID: 1177 RVA: 0x00025EDC File Offset: 0x000240DC
	public void SetMyCape()
	{
		string @string = Storager.getString(Defs.CapeEquppedSN, false);
		this.myCharacter.UpdateCape(@string, (!@string.Equals("cape_Custom")) ? null : SkinsController.capeUserTexture, !ShopNGUIController.ShowWear);
	}

	// Token: 0x0600049A RID: 1178 RVA: 0x00025F24 File Offset: 0x00024124
	public void SetMyArmor()
	{
		string @string = Storager.getString(Defs.ArmorNewEquppedSN, false);
		bool isInvisible = !ShopNGUIController.ShowArmor;
		this.myCharacter.UpdateArmor(@string, this.myCharacter.weapon, isInvisible);
	}

	// Token: 0x0600049B RID: 1179 RVA: 0x00025F60 File Offset: 0x00024160
	public void SetMyMask()
	{
		string @string = Storager.getString("MaskEquippedSN", false);
		this.myCharacter.UpdateMask(@string, !ShopNGUIController.ShowWear);
	}

	// Token: 0x0600049C RID: 1180 RVA: 0x00025F90 File Offset: 0x00024190
	public void SetMyBoots()
	{
		string @string = Storager.getString(Defs.BootsEquppedSN, false);
		this.myCharacter.UpdateBoots(@string, !ShopNGUIController.ShowWear);
	}

	// Token: 0x0600049D RID: 1181 RVA: 0x00025FC0 File Offset: 0x000241C0
	public void SetMyHat()
	{
		string @string = Storager.getString(Defs.HatEquppedSN, false);
		this.myCharacter.UpdateHat(@string, !ShopNGUIController.ShowWear);
	}

	// Token: 0x0600049E RID: 1182 RVA: 0x00025FF0 File Offset: 0x000241F0
	public void SetMyPet()
	{
		string eqipedPetId = Singleton<PetsManager>.Instance.GetEqipedPetId();
		this.myCharacter.UpdatePet(eqipedPetId);
	}

	// Token: 0x0600049F RID: 1183 RVA: 0x00026014 File Offset: 0x00024214
	public void SetMyWearInvisible()
	{
		this.SetMyCape();
		this.SetMyMask();
		this.SetMyHat();
		this.SetMyBoots();
	}

	// Token: 0x060004A0 RID: 1184 RVA: 0x00026030 File Offset: 0x00024230
	public void SetMyWeapon()
	{
		int index = WeaponManager.sharedManager.CurrentIndexOfLastUsedWeaponInPlayerWeapons();
		Weapon weapon = WeaponManager.sharedManager.playerWeapons[index] as Weapon;
		WeaponSkin skinForWeapon = WeaponSkinsManager.GetSkinForWeapon(weapon.weaponPrefab.name);
		this.myCharacter.SetWeapon(weapon.weaponPrefab.name, weapon.weaponPrefab.name, (skinForWeapon == null) ? string.Empty : skinForWeapon.Id);
	}

	// Token: 0x060004A1 RID: 1185 RVA: 0x000260A8 File Offset: 0x000242A8
	public void SendGameLeft()
	{
		this.photonView.RPC("OpponentLeftGame", PhotonTargets.Others, new object[]
		{
			PhotonNetwork.player
		});
	}

	// Token: 0x060004A2 RID: 1186 RVA: 0x000260CC File Offset: 0x000242CC
	[PunRPC]
	private void OpponentLeftGame(PhotonPlayer player)
	{
		this.OnPhotonPlayerDisconnected(player);
	}

	// Token: 0x060004A3 RID: 1187 RVA: 0x000260D8 File Offset: 0x000242D8
	public void OnPhotonPlayerDisconnected(PhotonPlayer player)
	{
		if (this.gameStatus == DuelController.GameStatus.ReadyToStart || this.gameStatus == DuelController.GameStatus.OpponentConnected)
		{
			if (!this.roomHidden)
			{
				this.gameStatus = DuelController.GameStatus.WaitForOpponent;
				this.duelUI.ShowWaitForOpponentInterface();
			}
			else
			{
				this.gameStatus = DuelController.GameStatus.End;
				this.ChangeRoomStatus(DuelController.RoomStatus.Closed);
				this.duelUI.ShowChangeAreaInterface(true);
			}
		}
		if (this.gameStatus == DuelController.GameStatus.Playing)
		{
			if (InGameGUI.sharedInGameGUI != null)
			{
				InGameGUI.sharedInGameGUI.waitDuelLabel.SetActive(true);
			}
			this.gameStatus = DuelController.GameStatus.WaitForPlayerBack;
			this.waitPlayerBackTime = 30f;
		}
		if (this.gameStatus == DuelController.GameStatus.End)
		{
			this.opponentLeftInEnd = true;
			this.duelUI.ShowRevengePanel(false, true);
		}
	}

	// Token: 0x060004A4 RID: 1188 RVA: 0x00026198 File Offset: 0x00024398
	public void OnDisconnectedFromPhoton()
	{
		if (this.gameStatus == DuelController.GameStatus.Playing)
		{
			this.gameStatus = DuelController.GameStatus.DisconnectInMatch;
		}
		if (this.gameStatus == DuelController.GameStatus.ReadyToStart)
		{
			this.gameStatus = DuelController.GameStatus.WaitForOpponent;
		}
	}

	// Token: 0x060004A5 RID: 1189 RVA: 0x000261CC File Offset: 0x000243CC
	private void ChangeRoomStatus(DuelController.RoomStatus newStatus)
	{
		this.roomStatus = newStatus;
		Debug.Log((newStatus != DuelController.RoomStatus.Closed) ? "Room opened!" : "Room closed!");
	}

	// Token: 0x060004A6 RID: 1190 RVA: 0x000261FC File Offset: 0x000243FC
	private void FindEnemyTable()
	{
		if (WeaponManager.sharedManager.myNetworkStartTable == null)
		{
			return;
		}
		for (int i = 0; i < Initializer.networkTables.Count; i++)
		{
			if (!Initializer.networkTables[i].Equals(WeaponManager.sharedManager.myNetworkStartTable))
			{
				this.opponentNetworkTable = Initializer.networkTables[i];
				this.OpponentConnected();
				break;
			}
		}
	}

	// Token: 0x060004A7 RID: 1191 RVA: 0x00026278 File Offset: 0x00024478
	private void Update()
	{
		if (this.opponentNetworkTable == null)
		{
			this.FindEnemyTable();
		}
		if (this.gameStatus == DuelController.GameStatus.Playing)
		{
			this._timeLeft -= Time.deltaTime;
			if (this._timeLeft < 0f)
			{
				this._timeLeft = 0f;
				this.EndMatch();
			}
			if (this.isMaster && this.nextSynchronizeTime < Time.time)
			{
				this.nextSynchronizeTime = Time.time + 0.5f;
				this.photonView.RPC("SynchronizeTimeRPC", PhotonTargets.Others, new object[]
				{
					this._timeLeft
				});
			}
		}
		if ((this.isMaster && this.gameStatus == DuelController.GameStatus.WaitForPlayerBack) || this.gameStatus == DuelController.GameStatus.RoomClosed)
		{
			this.waitPlayerBackTime -= Time.deltaTime;
			if (InGameGUI.sharedInGameGUI != null)
			{
				InGameGUI.sharedInGameGUI.waitDuelLabelTimer.text = LocalizationStore.Get("Key_1126") + " " + Mathf.RoundToInt(this.waitPlayerBackTime).ToString();
			}
			if (this.waitPlayerBackTime < 5f && this.gameStatus == DuelController.GameStatus.WaitForPlayerBack)
			{
				this.gameStatus = DuelController.GameStatus.RoomClosed;
				this.ChangeRoomStatus(DuelController.RoomStatus.Closed);
			}
			if (this.waitPlayerBackTime < 0f)
			{
				this.waitPlayerBackTime = 0f;
				this._timeLeft = 0f;
				this.EndMatch();
			}
		}
		if (this.gameStatus == DuelController.GameStatus.ReadyToStart)
		{
			this.goTimer -= Time.deltaTime;
			int num = (int)Mathf.Floor(this.goTimer);
			if (this.duelUI != null)
			{
				if (this.lastTimer == -1 || this.lastTimer > num)
				{
					this.lastTimer = num;
					this.duelUI.versusTimer.GetComponent<TweenScale>().ResetToBeginning();
					this.duelUI.versusTimer.GetComponent<TweenScale>().PlayForward();
				}
				this.duelUI.versusTimer.text = ((Mathf.Floor(this.goTimer) > 0f) ? num.ToString() : "GO!");
			}
			if (this.goTimer < 0f)
			{
				this.goTimer = 0f;
				if (this.isMaster)
				{
					this.lastTimer = -1;
					this.StartMatch();
				}
			}
		}
		if (this.gameStatus == DuelController.GameStatus.OpponentConnected)
		{
			this.goTimer -= Time.deltaTime;
			if (this.goTimer < 0f)
			{
				this.goTimer = 0f;
				if (this.isMaster)
				{
					this.GoMatch();
				}
			}
		}
		if (this.enemyCharacter != null)
		{
			this.enemyCharacter.gameObject.SetActive(this.showEnemyCharacter);
		}
		if (this.myCharacter != null)
		{
			this.myCharacter.gameObject.SetActive(this.showMyCharacter);
		}
		if (this.isMaster && (this.gameStatus == DuelController.GameStatus.Playing || this.gameStatus == DuelController.GameStatus.DisconnectInMatch) && this.opponentNetworkTable == null)
		{
			if (InGameGUI.sharedInGameGUI != null)
			{
				InGameGUI.sharedInGameGUI.waitDuelLabel.SetActive(true);
			}
			this.gameStatus = DuelController.GameStatus.WaitForPlayerBack;
			this.waitPlayerBackTime = 30f;
		}
		if (this.waitPlayerBackTime >= 0f && (this.gameStatus == DuelController.GameStatus.WaitForPlayerBack || this.gameStatus == DuelController.GameStatus.RoomClosed) && this.opponentNetworkTable != null)
		{
			if (InGameGUI.sharedInGameGUI != null)
			{
				InGameGUI.sharedInGameGUI.waitDuelLabel.SetActive(false);
			}
			this.gameStatus = DuelController.GameStatus.Playing;
			if (this.isMaster)
			{
				this.ResumeMatch();
			}
		}
	}

	// Token: 0x060004A8 RID: 1192 RVA: 0x0002665C File Offset: 0x0002485C
	private CharacterInterface CreateCharacter(Transform transform)
	{
		GameObject gameObject = Resources.Load<GameObject>("Character_model");
		gameObject.SetActive(false);
		GameObject gameObject2 = UnityEngine.Object.Instantiate(gameObject, transform.position, transform.rotation) as GameObject;
		CharacterInterface component = gameObject2.GetComponent<CharacterInterface>();
		component.usePetFromStorager = false;
		gameObject2.transform.SetParent(transform);
		gameObject.SetActive(true);
		component.useLightprobes = true;
		component.SetCharacterType(false, false, false);
		return component;
	}

	// Token: 0x060004A9 RID: 1193 RVA: 0x000266C8 File Offset: 0x000248C8
	public void SetShopEvents()
	{
		ShopNGUIController.sharedShop.wearEquipAction = delegate(ShopNGUIController.CategoryNames category, string unequippedItem, string equippedItem)
		{
			this.SendMyWearInvisible();
			if (category == ShopNGUIController.CategoryNames.CapesCategory)
			{
				this.SetMyCape();
				this.SendMyCape();
			}
			if (category == ShopNGUIController.CategoryNames.MaskCategory)
			{
				this.SetMyMask();
				this.SendMyMask();
			}
			if (category == ShopNGUIController.CategoryNames.HatsCategory)
			{
				this.SetMyHat();
				this.SendMyHat();
			}
			if (category == ShopNGUIController.CategoryNames.BootsCategory)
			{
				this.SetMyBoots();
				this.SendMyBoots();
			}
			if (category == ShopNGUIController.CategoryNames.ArmorCategory)
			{
				this.SetMyArmor();
				this.SendMyArmor();
			}
		};
		ShopNGUIController.sharedShop.wearUnequipAction = delegate(ShopNGUIController.CategoryNames category, string unequippedItem)
		{
			this.SendMyWearInvisible();
			if (category == ShopNGUIController.CategoryNames.CapesCategory)
			{
				this.SetMyCape();
				this.SendMyCape();
			}
			if (category == ShopNGUIController.CategoryNames.MaskCategory)
			{
				this.SetMyMask();
				this.SendMyMask();
			}
			if (category == ShopNGUIController.CategoryNames.HatsCategory)
			{
				this.SetMyHat();
				this.SendMyHat();
			}
			if (category == ShopNGUIController.CategoryNames.BootsCategory)
			{
				this.SetMyBoots();
				this.SendMyBoots();
			}
			if (category == ShopNGUIController.CategoryNames.ArmorCategory)
			{
				this.SetMyArmor();
				this.SendMyArmor();
			}
		};
		ShopNGUIController.sharedShop.equipAction = delegate(string id)
		{
			this.SetMyWeapon();
			this.SendMyWeapon();
		};
		if (!this.equippedPetActionSet)
		{
			this.equippedPetActionSet = true;
			ShopNGUIController.EquippedPet += this.OnPetEquipAction;
			ShopNGUIController.UnequippedPet += this.OnPetUnequipAction;
		}
		ShopNGUIController.sharedShop.onEquipSkinAction = delegate(string id)
		{
			this.SetMySkin();
			this.SendMySkin();
		};
		ShopNGUIController.ShowWearChanged -= this.OnMyWearVisibleChanged;
		ShopNGUIController.ShowWearChanged += this.OnMyWearVisibleChanged;
		ShopNGUIController.ShowArmorChanged -= this.OnMyArmorVisibleChanged;
		ShopNGUIController.ShowArmorChanged += this.OnMyArmorVisibleChanged;
	}

	// Token: 0x060004AA RID: 1194 RVA: 0x000267A8 File Offset: 0x000249A8
	public void OnPetUnequipAction(string nowId)
	{
		this.SetMyPet();
		this.SendMyPet();
	}

	// Token: 0x060004AB RID: 1195 RVA: 0x000267B8 File Offset: 0x000249B8
	public void OnPetEquipAction(string nowId, string beforeID)
	{
		this.SetMyPet();
		this.SendMyPet();
	}

	// Token: 0x060004AC RID: 1196 RVA: 0x000267C8 File Offset: 0x000249C8
	private void OnMyWearVisibleChanged()
	{
		this.SetMyWearInvisible();
		if (Defs.isMulti)
		{
			this.SendMyWearInvisible();
		}
	}

	// Token: 0x060004AD RID: 1197 RVA: 0x000267E0 File Offset: 0x000249E0
	private void OnMyArmorVisibleChanged()
	{
		this.SetMyArmor();
		if (Defs.isMulti)
		{
			this.SendMyArmor();
		}
	}

	// Token: 0x060004AE RID: 1198 RVA: 0x000267F8 File Offset: 0x000249F8
	[PunRPC]
	private void SyncGameStatusRPC(int state)
	{
	}

	// Token: 0x040004EE RID: 1262
	private const float duelTime = 120f;

	// Token: 0x040004EF RID: 1263
	public static DuelController instance;

	// Token: 0x040004F0 RID: 1264
	[HideInInspector]
	public CharacterInterface myCharacter;

	// Token: 0x040004F1 RID: 1265
	[HideInInspector]
	public CharacterInterface enemyCharacter;

	// Token: 0x040004F2 RID: 1266
	public Transform myCharacterPoint;

	// Token: 0x040004F3 RID: 1267
	public Transform enemyCharacterPoint;

	// Token: 0x040004F4 RID: 1268
	private float _timeLeft;

	// Token: 0x040004F5 RID: 1269
	private float waitPlayerBackTime;

	// Token: 0x040004F6 RID: 1270
	private float nextSynchronizeTime;

	// Token: 0x040004F7 RID: 1271
	private int lastTimer = -1;

	// Token: 0x040004F8 RID: 1272
	private bool opponentLeftInEnd;

	// Token: 0x040004F9 RID: 1273
	private float goTimer;

	// Token: 0x040004FA RID: 1274
	private PhotonView photonView;

	// Token: 0x040004FB RID: 1275
	[HideInInspector]
	public DuelController.GameStatus gameStatus;

	// Token: 0x040004FC RID: 1276
	[HideInInspector]
	public NetworkStartTable opponentNetworkTable;

	// Token: 0x040004FD RID: 1277
	[HideInInspector]
	public int myRespawnPoints = 1;

	// Token: 0x040004FE RID: 1278
	private bool requestSended;

	// Token: 0x040004FF RID: 1279
	private bool requestReceived;

	// Token: 0x04000500 RID: 1280
	private bool roomHidden;

	// Token: 0x04000501 RID: 1281
	private bool _wearIsInvisible;

	// Token: 0x04000502 RID: 1282
	private bool equippedPetActionSet;

	// Token: 0x0200009F RID: 159
	public enum GameStatus
	{
		// Token: 0x04000504 RID: 1284
		None,
		// Token: 0x04000505 RID: 1285
		WaitForOpponent,
		// Token: 0x04000506 RID: 1286
		OpponentConnected,
		// Token: 0x04000507 RID: 1287
		ReadyToStart,
		// Token: 0x04000508 RID: 1288
		Playing,
		// Token: 0x04000509 RID: 1289
		WaitForPlayerBack,
		// Token: 0x0400050A RID: 1290
		RoomClosed,
		// Token: 0x0400050B RID: 1291
		End,
		// Token: 0x0400050C RID: 1292
		DisconnectInMatch,
		// Token: 0x0400050D RID: 1293
		ChangeArea
	}

	// Token: 0x020000A0 RID: 160
	public enum RoomStatus
	{
		// Token: 0x0400050F RID: 1295
		None,
		// Token: 0x04000510 RID: 1296
		Closed,
		// Token: 0x04000511 RID: 1297
		MatchStarted
	}
}
