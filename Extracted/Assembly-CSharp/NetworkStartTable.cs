using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using ExitGames.Client.Photon;
using FyberPlugin;
using Prime31;
using Rilisoft;
using Rilisoft.MiniJson;
using UnityEngine;

// Token: 0x020003C0 RID: 960
public sealed class NetworkStartTable : MonoBehaviour
{
	// Token: 0x17000638 RID: 1592
	// (get) Token: 0x06002266 RID: 8806 RVA: 0x000A60D8 File Offset: 0x000A42D8
	// (set) Token: 0x06002267 RID: 8807 RVA: 0x000A60E8 File Offset: 0x000A42E8
	public int scoreCommandFlag1
	{
		get
		{
			return this._scoreCommandFlag1.Value;
		}
		set
		{
			this._scoreCommandFlag1 = value;
		}
	}

	// Token: 0x17000639 RID: 1593
	// (get) Token: 0x06002268 RID: 8808 RVA: 0x000A60F8 File Offset: 0x000A42F8
	// (set) Token: 0x06002269 RID: 8809 RVA: 0x000A6108 File Offset: 0x000A4308
	public int scoreCommandFlag2
	{
		get
		{
			return this._scoreCommandFlag2.Value;
		}
		set
		{
			this._scoreCommandFlag2 = value;
		}
	}

	// Token: 0x0600226A RID: 8810 RVA: 0x000A6118 File Offset: 0x000A4318
	private string _SocialMessage()
	{
		int @int = Storager.getInt(Defs.COOPScore, false);
		bool flag = Defs.isCOOP;
		int int2 = Storager.getInt("Rating", false);
		string arg = "http://goo.gl/dQMf4n";
		if (this.isIwin)
		{
			return (!flag) ? string.Format("I won the match in @PixelGun3D ! Try it right now!\n#pixelgun3d #pg3d #pixelgun", int2, arg) : string.Format(" Now I have {0} score in @PixelGun3D ! Try it right now!\n#pixelgun3d #pg3d #pixelgun", @int, arg);
		}
		return (!flag) ? string.Format("I played a match in @PixelGun3D ! Try it right now!\n#pixelgun3d #pg3d #pixelgun", int2, arg) : string.Format("I received {0} points in @PixelGun3D ! Try it right now!\n#pixelgun3d #pg3d #pixelgun", @int, arg);
	}

	// Token: 0x0600226B RID: 8811 RVA: 0x000A61B0 File Offset: 0x000A43B0
	private string _SocialSentSuccess(string SocialName)
	{
		return "Message was sent to " + SocialName;
	}

	// Token: 0x1700063A RID: 1594
	// (get) Token: 0x0600226C RID: 8812 RVA: 0x000A61C0 File Offset: 0x000A43C0
	// (set) Token: 0x0600226D RID: 8813 RVA: 0x000A61C8 File Offset: 0x000A43C8
	public bool showTable
	{
		get
		{
			return this._showTable;
		}
		set
		{
			this._showTable = value;
			if (this.isMine)
			{
				Defs.showTableInNetworkStartTable = value;
			}
		}
	}

	// Token: 0x1700063B RID: 1595
	// (get) Token: 0x0600226E RID: 8814 RVA: 0x000A61E4 File Offset: 0x000A43E4
	// (set) Token: 0x0600226F RID: 8815 RVA: 0x000A61EC File Offset: 0x000A43EC
	public bool isShowNickTable
	{
		get
		{
			return this._isShowNickTable;
		}
		set
		{
			this._isShowNickTable = value;
			if (this.isMine)
			{
				Defs.showNickTableInNetworkStartTable = value;
			}
		}
	}

	// Token: 0x1700063C RID: 1596
	// (get) Token: 0x06002270 RID: 8816 RVA: 0x000A6208 File Offset: 0x000A4408
	// (set) Token: 0x06002271 RID: 8817 RVA: 0x000A6218 File Offset: 0x000A4418
	public int score
	{
		get
		{
			return this._score.Value;
		}
		set
		{
			this._score = new SaltedInt(NetworkStartTable._prng.Next(), value);
		}
	}

	// Token: 0x1700063D RID: 1597
	// (get) Token: 0x06002272 RID: 8818 RVA: 0x000A6230 File Offset: 0x000A4430
	// (set) Token: 0x06002273 RID: 8819 RVA: 0x000A6268 File Offset: 0x000A4468
	public int gameRating
	{
		get
		{
			return (!Defs.isMulti || !this.isMine) ? this._gameRating : RatingSystem.instance.currentRating;
		}
		set
		{
			this._gameRating = value;
		}
	}

	// Token: 0x06002274 RID: 8820 RVA: 0x000A6274 File Offset: 0x000A4474
	private void completionHandler(string error, object result)
	{
		if (error != null)
		{
			UnityEngine.Debug.LogError(error);
		}
		else
		{
			Prime31.Utils.logObject(result);
			this.showMessagFacebook = true;
			base.Invoke("hideMessag", 3f);
		}
	}

	// Token: 0x06002275 RID: 8821 RVA: 0x000A62B0 File Offset: 0x000A44B0
	private void Awake()
	{
		this.isLocal = !Defs.isInet;
		this.isInet = Defs.isInet;
		this.isCOOP = (ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.TimeBattle);
		if (this.isInet)
		{
			this.isServer = PhotonNetwork.isMasterClient;
		}
		else
		{
			this.isServer = PlayerPrefs.GetString("TypeGame").Equals("server");
		}
		this.isMulti = Defs.isMulti;
		this.isCompany = (ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.TeamFight);
		this.isHunger = (ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.DeadlyGames);
		this.experienceController = GameObject.FindGameObjectWithTag("ExperienceController").GetComponent<ExperienceController>();
		if (ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.TimeBattle)
		{
			string[] array = new string[]
			{
				"1",
				"15",
				"14",
				"2",
				"3",
				"9",
				"11",
				"12",
				"10",
				"16"
			};
			for (int i = 0; i < array.Length; i++)
			{
				GameObject item = Resources.Load("Enemies/Enemy" + array[i] + "_go") as GameObject;
				this.zombiePrefabs.Add(item);
			}
		}
		if (ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.FlagCapture)
		{
			this.maxTimerFlag = (float)int.Parse(PhotonNetwork.room.customProperties[ConnectSceneNGUIController.maxKillProperty].ToString()) * 60f;
		}
		this.photonView = PhotonView.Get(this);
		Initializer.networkTables.Add(this);
		if (this.photonView && this.photonView.isMine)
		{
			PhotonObjectCacher.AddObject(base.gameObject);
		}
	}

	// Token: 0x06002276 RID: 8822 RVA: 0x000A6470 File Offset: 0x000A4670
	public void ImDeadInHungerGames()
	{
		if (Defs.isInet && NetworkStartTableNGUIController.sharedController != null)
		{
			NetworkStartTableNGUIController.sharedController.UpdateGoMapButtons(true);
			this.isSetNewMapButton = true;
		}
		this._matchStopwatch.Stop();
		int @int = PlayerPrefs.GetInt("CountMatch", 0);
		int num = @int + 1;
		PlayerPrefs.SetInt("CountMatch", num);
		Dictionary<string, object> parameters = new Dictionary<string, object>
		{
			{
				"count",
				num
			}
		};
		AnalyticsFacade.SendCustomEventToFacebook("games_multiplayer_count", parameters);
		if (ExperienceController.sharedController != null)
		{
			string key = "Statistics.MatchCount.Level" + ExperienceController.sharedController.currentLevel;
			int int2 = PlayerPrefs.GetInt(key, 0);
			PlayerPrefs.SetInt(key, int2 + 1);
		}
		NetworkStartTable.IncreaseTimeInMode(3, this._matchStopwatch.Elapsed.TotalMinutes);
		StoreKitEventListener.State.PurchaseKey = "End match";
		if (this._cam != null)
		{
			this._cam.SetActive(true);
		}
		NickLabelController.currentCamera = Initializer.Instance.tc.GetComponent<Camera>();
		this.showTable = true;
		RatingSystem.RatingChange ratingChange = this.CalculateMatchRating(false);
		this.photonView.RPC("ImDeadInHungerGamesRPC", PhotonTargets.Others, new object[0]);
		this.isDeadInHungerGame = true;
		if (NetworkStartTableNGUIController.sharedController != null)
		{
			NetworkStartTableNGUIController.sharedController.ShowEndInterfaceDeadInHunger(LocalizationStore.Get("Key_1116"), ratingChange);
		}
		this.inGameGUI.ResetScope();
	}

	// Token: 0x06002277 RID: 8823 RVA: 0x000A65EC File Offset: 0x000A47EC
	[PunRPC]
	[RPC]
	public void ImDeadInHungerGamesRPC()
	{
		this.isDeadInHungerGame = true;
	}

	// Token: 0x06002278 RID: 8824 RVA: 0x000A65F8 File Offset: 0x000A47F8
	public void setScoreFromGlobalGameController()
	{
		this.score = GlobalGameController.Score;
		this.SynhScore();
	}

	// Token: 0x06002279 RID: 8825 RVA: 0x000A660C File Offset: 0x000A480C
	[PunRPC]
	[RPC]
	private void RunGame()
	{
		GameObject[] array = GameObject.FindGameObjectsWithTag("NetworkTable");
		foreach (GameObject gameObject in array)
		{
			gameObject.GetComponent<NetworkStartTable>().runGame = true;
		}
	}

	// Token: 0x0600227A RID: 8826 RVA: 0x000A664C File Offset: 0x000A484C
	public void RemoveShop(bool disable = true)
	{
		ShopTapReceiver.ShopClicked -= this.HandleShopButton;
		if (this._shopInstance != null)
		{
			if (disable)
			{
				ShopNGUIController.GuiActive = false;
			}
			this._shopInstance.resumeAction = delegate()
			{
			};
			this._shopInstance = null;
		}
	}

	// Token: 0x0600227B RID: 8827 RVA: 0x000A66B8 File Offset: 0x000A48B8
	public void HandleShopButton()
	{
		NetworkStartTableNGUIController sharedController = NetworkStartTableNGUIController.sharedController;
		if (sharedController != null)
		{
			if (!(sharedController.goMapInEndGameButtons.FirstOrDefault((GoMapInEndGame button) => button.IsLeavingRoom) != null))
			{
				if (!(sharedController.goMapInEndGameButtonsDuel.FirstOrDefault((GoMapInEndGame button) => button.IsLeavingRoom) != null))
				{
					goto IL_79;
				}
			}
			return;
		}
		IL_79:
		if (this._shopInstance == null && (!(BankController.Instance != null) || !BankController.Instance.InterfaceEnabled))
		{
			this._shopInstance = ShopNGUIController.sharedShop;
			if (this._shopInstance != null)
			{
				this._shopInstance.SetInGame(false);
				ShopNGUIController.GuiActive = true;
				this._shopInstance.resumeAction = new Action(this.HandleResumeFromShop);
			}
			else
			{
				UnityEngine.Debug.LogWarning("sharedShop == null");
			}
		}
	}

	// Token: 0x0600227C RID: 8828 RVA: 0x000A67C4 File Offset: 0x000A49C4
	public void HandleResumeFromShop()
	{
		if (this._shopInstance != null)
		{
			this.expController.isShowRanks = true;
			ShopNGUIController.GuiActive = false;
			this._shopInstance.resumeAction = delegate()
			{
			};
			this._shopInstance = null;
		}
	}

	// Token: 0x0600227D RID: 8829 RVA: 0x000A6824 File Offset: 0x000A4A24
	public void BackButtonPress()
	{
		if (ExpController.Instance.IsLevelUpShown)
		{
			return;
		}
		NetworkStartTableNGUIController sharedController = NetworkStartTableNGUIController.sharedController;
		if (sharedController != null && sharedController.CheckHideInternalPanel())
		{
			return;
		}
		this.networkStartTableNGUIController.shopAnchor.SetActive(false);
		this.RemoveShop(true);
		if (!this.isInet)
		{
			if (this.isServer)
			{
				Network.Disconnect(200);
				GameObject.FindGameObjectWithTag("NetworkTable").GetComponent<LANBroadcastService>().StopBroadCasting();
			}
			else if (Network.connections.Length == 1)
			{
				UnityEngine.Debug.Log(string.Concat(new object[]
				{
					"Disconnecting: ",
					Network.connections[0].ipAddress,
					":",
					Network.connections[0].port
				}));
				Network.CloseConnection(Network.connections[0], true);
			}
			ActivityIndicator.IsActiveIndicator = false;
			ConnectSceneNGUIController.Local();
		}
		else
		{
			ActivityIndicator.IsActiveIndicator = false;
			Defs.typeDisconnectGame = Defs.DisconectGameType.Exit;
			PhotonNetwork.isMessageQueueRunning = true;
			PhotonNetwork.Disconnect();
		}
	}

	// Token: 0x0600227E RID: 8830 RVA: 0x000A6948 File Offset: 0x000A4B48
	public void StartPlayerButtonClick(int _command)
	{
		if (!this.notSendAnaliticStartBattle)
		{
			int @int = PlayerPrefs.GetInt("CountMatch", 0);
			int num = @int + 1;
			if (!this.notSendAnaliticStartBattle && num <= 5)
			{
				AnalyticsStuff.Tutorial(AnalyticsConstants.TutorialState.Battle_Start, num);
			}
			else
			{
				this.notSendAnaliticStartBattle = true;
			}
		}
		if (NetworkStartTableNGUIController.sharedController != null)
		{
			NetworkStartTableNGUIController.sharedController.HideEndInterface();
		}
		this.isShowNickTable = false;
		this.CountKills = 0;
		this.score = 0;
		GlobalGameController.Score = 0;
		GlobalGameController.CountKills = 0;
		this.myCommand = _command;
		this.SynhCommand(null);
		this.SynhCountKills(null);
		this.SynhScore();
		this.startPlayer();
		this.countMigZagolovok = 0;
		this.timeTomig = 0.7f;
		this.isMigZag = false;
	}

	// Token: 0x0600227F RID: 8831 RVA: 0x000A6A0C File Offset: 0x000A4C0C
	public void RandomRoomClickBtnInHunger()
	{
		this.isGoRandomRoom = true;
		if (this.isRegimVidos)
		{
			this.isRegimVidos = false;
			if (this.inGameGUI != null)
			{
				this.inGameGUI.ResetScope();
			}
		}
		Defs.typeDisconnectGame = Defs.DisconectGameType.RandomGameInHunger;
		PhotonNetwork.LeaveRoom();
	}

	// Token: 0x06002280 RID: 8832 RVA: 0x000A6A5C File Offset: 0x000A4C5C
	public void RandomRoomClickBtnInDuel()
	{
		this.isGoRandomRoom = true;
		Defs.typeDisconnectGame = Defs.DisconectGameType.RandomGameInDuel;
		PhotonNetwork.LeaveRoom();
	}

	// Token: 0x06002281 RID: 8833 RVA: 0x000A6A74 File Offset: 0x000A4C74
	public void SetRegimVidos(bool _isRegimVidos)
	{
		bool flag = this.isRegimVidos;
		this.isRegimVidos = _isRegimVidos;
		if (this.isRegimVidos != flag && !this.isRegimVidos && this.inGameGUI != null)
		{
			this.inGameGUI.ResetScope();
		}
	}

	// Token: 0x06002282 RID: 8834 RVA: 0x000A6AC4 File Offset: 0x000A4CC4
	private void playersTable()
	{
		if (!this.isShowAvard)
		{
			ShopTapReceiver.AddClickHndIfNotExist(new Action(this.HandleShopButton));
			this.networkStartTableNGUIController.shopAnchor.SetActive(!this.isShowFinished && !this.isHunger && this._shopInstance == null && (this.expController == null || !this.expController.isShowNextPlashka));
			if (this._shopInstance != null)
			{
				this._shopInstance.SetInGame(false);
				return;
			}
		}
	}

	// Token: 0x06002283 RID: 8835 RVA: 0x000A6B68 File Offset: 0x000A4D68
	public void PostFacebookBtnClick()
	{
		UnityEngine.Debug.Log("show facebook dialog");
		FacebookController.ShowPostDialog();
	}

	// Token: 0x06002284 RID: 8836 RVA: 0x000A6B7C File Offset: 0x000A4D7C
	public void PostTwitterBtnClick()
	{
		if (TwitterController.Instance != null)
		{
			TwitterController.Instance.PostStatusUpdate(this._SocialMessage(), null);
		}
	}

	// Token: 0x06002285 RID: 8837 RVA: 0x000A6BA0 File Offset: 0x000A4DA0
	private IEnumerator StartPlayerCoroutine()
	{
		Defs.inRespawnWindow = false;
		if (Defs.isMulti && Defs.isInet)
		{
			this.photonView.RPC("SynchGameRating", PhotonTargets.Others, new object[]
			{
				this.gameRating
			});
		}
		if (Defs.isMulti && Defs.isDuel)
		{
			ShopNGUIController.sharedShop.onEquipSkinAction = delegate(string id)
			{
				this.sendMySkin();
			};
		}
		if (this.isStartPlayerCoroutine)
		{
			yield break;
		}
		this.isStartPlayerCoroutine = true;
		while (Defs.isMulti && Defs.isInet && PhotonNetwork.time > -0.01 && PhotonNetwork.time < 0.01)
		{
			yield return null;
		}
		this.isStartPlayerCoroutine = false;
		if (Defs.isMulti && !Defs.isHunger && !Defs.isDuel)
		{
			TimeGameController.sharedController.StartMatch();
		}
		if (Defs.isDaterRegim)
		{
			int _timeGame = 5;
			if (Defs.isInet)
			{
				_timeGame = (int)PhotonNetwork.room.customProperties[ConnectSceneNGUIController.maxKillProperty];
			}
			else
			{
				_timeGame = (PlayerPrefs.GetString("MaxKill", "9").Equals(string.Empty) ? 5 : int.Parse(PlayerPrefs.GetString("MaxKill", "5")));
			}
			AnalyticsStuff.LogSandboxTimeGamePopularity(_timeGame, true);
		}
		this.isDrawInDeathMatch = false;
		this._matchStopwatch.Start();
		StoreKitEventListener.State.PurchaseKey = "In game";
		StoreKitEventListener.State.Parameters.Clear();
		this.networkStartTableNGUIController.shopAnchor.SetActive(false);
		this.RemoveShop(!BankController.Instance.InterfaceEnabled);
		if (ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.FlagCapture)
		{
			this.timerFlag = (double)this.maxTimerFlag;
		}
		if (this.myRanks != this.expController.currentLevel)
		{
			this.SetRanks();
		}
		this._cam = GameObject.FindGameObjectWithTag("CamTemp");
		this._cam.SetActive(false);
		this._weaponManager.useCam = null;
		this.zoneCreatePlayer = GameObject.FindGameObjectsWithTag((ConnectSceneNGUIController.regim != ConnectSceneNGUIController.RegimGame.TimeBattle) ? ((ConnectSceneNGUIController.regim != ConnectSceneNGUIController.RegimGame.TeamFight) ? ((ConnectSceneNGUIController.regim != ConnectSceneNGUIController.RegimGame.FlagCapture) ? ((ConnectSceneNGUIController.regim != ConnectSceneNGUIController.RegimGame.CapturePoints) ? ((ConnectSceneNGUIController.regim != ConnectSceneNGUIController.RegimGame.Duel) ? "MultyPlayerCreateZone" : ("MultyPlayerCreateZoneDuel" + DuelController.instance.myRespawnPoints)) : ("MultyPlayerCreateZonePointZone" + this.myCommand)) : ("MultyPlayerCreateZoneFlagCommand" + this.myCommand)) : ("MultyPlayerCreateZoneCommand" + this.myCommand)) : "MultyPlayerCreateZoneCOOP");
		GameObject chestSpawnZone = null;
		int numberSpawnZone = 0;
		int numberZoneChest = 0;
		if (this.isHunger)
		{
			if (!NetworkStartTable.StartAfterDisconnect)
			{
				this._weaponManager.Reset(0);
			}
			int _myId = this.photonView.owner.ID;
			GameObject[] tabMas = GameObject.FindGameObjectsWithTag("NetworkTable");
			for (int i = 0; i < tabMas.Length; i++)
			{
				PhotonPlayer owner = tabMas[i].transform.GetComponent<PhotonView>().owner;
				if (owner != null && owner.ID < _myId)
				{
					numberSpawnZone++;
				}
			}
			numberZoneChest = numberSpawnZone;
			for (int j = 0; j < this.zoneCreatePlayer.Length; j++)
			{
				if (this.zoneCreatePlayer[j].GetComponent<NumberZone>().numberZone == numberSpawnZone)
				{
					numberSpawnZone = j;
					break;
				}
			}
			if (!NetworkStartTable.StartAfterDisconnect)
			{
				GameObject[] chestCreateZones = GameObject.FindGameObjectsWithTag("ChestCreateZone");
				for (int k = 0; k < chestCreateZones.Length; k++)
				{
					if (chestCreateZones[k].GetComponent<NumberZone>().numberZone == numberZoneChest)
					{
						chestSpawnZone = chestCreateZones[k];
						this.photonView.RPC("CreateChestRPC", PhotonTargets.MasterClient, new object[]
						{
							chestSpawnZone.transform.position,
							chestSpawnZone.transform.rotation
						});
						break;
					}
				}
			}
			this.playerCountInHunger = Initializer.networkTables.Count;
		}
		GameObject spawnZone = this.zoneCreatePlayer[(!this.isHunger) ? UnityEngine.Random.Range(0, this.zoneCreatePlayer.Length - 1) : numberSpawnZone];
		BoxCollider spawnZoneCollider = spawnZone.GetComponent<BoxCollider>();
		Vector2 sz = new Vector2(spawnZoneCollider.size.x * spawnZone.transform.localScale.x, spawnZoneCollider.size.z * spawnZone.transform.localScale.z);
		Rect zoneRect = new Rect(spawnZone.transform.position.x - sz.x / 2f, spawnZone.transform.position.z - sz.y / 2f, sz.x, sz.y);
		Vector3 pos;
		if (this.isHunger)
		{
			pos = spawnZone.transform.position;
		}
		else
		{
			pos = new Vector3(zoneRect.x + UnityEngine.Random.Range(0f, zoneRect.width), spawnZone.transform.position.y, zoneRect.y + UnityEngine.Random.Range(0f, zoneRect.height));
		}
		Quaternion rot = spawnZone.transform.rotation;
		if (NetworkStartTable.StartAfterDisconnect && GlobalGameController.healthMyPlayer > 0f)
		{
			pos = GlobalGameController.posMyPlayer;
		}
		GameObject pl;
		if (this.isInet)
		{
			pl = PhotonNetwork.Instantiate("Player", pos, rot, 0);
		}
		else
		{
			if (this._playerPrefab == null)
			{
				this._playerPrefab = (Resources.Load("Player") as GameObject);
			}
			pl = (GameObject)Network.Instantiate(this._playerPrefab, pos, rot, 0);
			pl.GetComponent<SkinName>().playerMoveC.SetIDMyTable(base.GetComponent<NetworkView>().viewID.ToString());
		}
		NickLabelController.currentCamera = pl.GetComponent<SkinName>().camPlayer.GetComponent<Camera>();
		this._weaponManager.myPlayer = pl;
		this._weaponManager.myPlayerMoveC = pl.GetComponent<SkinName>().playerMoveC;
		if (!this.isInet && this.isServer)
		{
			UnityEngine.Debug.Log("networkView.RPC(RunGame, RPCMode.OthersBuffered);");
			base.GetComponent<NetworkView>().RPC("RunGame", RPCMode.OthersBuffered, new object[0]);
		}
		Initializer.Instance.SetupObjectThatNeedsPlayer();
		if (NetworkStartTableNGUIController.sharedController != null)
		{
			NetworkStartTableNGUIController.sharedController.HideStartInterface();
		}
		this.showTable = false;
		if (ABTestController.useBuffSystem)
		{
			BuffSystem.instance.BuffsActive(!Defs.isDaterRegim && !Defs.isHunger && !Defs.isCOOP && Defs.isMulti && Defs.isInet && !NetworkStartTable.LocalOrPasswordRoom());
		}
		else
		{
			KillRateCheck.instance.SetActive(!Defs.isDaterRegim && !Defs.isHunger && !Defs.isCOOP && Defs.isMulti && Defs.isInet && !NetworkStartTable.LocalOrPasswordRoom() && WeaponManager.sharedManager._currentFilterMap == 0, TimeGameController.sharedController != null && TimeGameController.sharedController.timerToEndMatch > 30.0);
		}
		yield break;
	}

	// Token: 0x06002286 RID: 8838 RVA: 0x000A6BBC File Offset: 0x000A4DBC
	[Obfuscation(Exclude = true)]
	public void startPlayer()
	{
		base.StartCoroutine(this.StartPlayerCoroutine());
	}

	// Token: 0x06002287 RID: 8839 RVA: 0x000A6BCC File Offset: 0x000A4DCC
	[RPC]
	[PunRPC]
	public void CreateChestRPC(Vector3 pos, Quaternion rot)
	{
		PhotonNetwork.InstantiateSceneObject("HungerGames/Chest", pos, rot, 0, null);
	}

	// Token: 0x06002288 RID: 8840 RVA: 0x000A6BE0 File Offset: 0x000A4DE0
	[PunRPC]
	[RPC]
	private void SetPixelBookID(string _pixelBookID)
	{
		this.pixelBookID = _pixelBookID;
	}

	// Token: 0x06002289 RID: 8841 RVA: 0x000A6BEC File Offset: 0x000A4DEC
	public void OnPhotonPlayerConnected(PhotonPlayer player)
	{
		if (this.photonView && this.photonView.isMine)
		{
			if (Defs.isFlag && !this.isShowFinished)
			{
				this.photonView.RPC("SynchScoreCommandRPC", player, new object[]
				{
					1,
					this.scoreCommandFlag1
				});
				this.photonView.RPC("SynchScoreCommandRPC", player, new object[]
				{
					2,
					this.scoreCommandFlag2
				});
			}
			this.SynhCommand(player);
			this.SynhCountKills(player);
			this.SendSynhScore(player);
			if (Defs.isMulti && Defs.isInet && this.isMine)
			{
				this.photonView.RPC("SynchGameRating", player, new object[]
				{
					this.gameRating
				});
			}
		}
	}

	// Token: 0x0600228A RID: 8842 RVA: 0x000A6CE4 File Offset: 0x000A4EE4
	public void SetNewNick()
	{
		this.NamePlayer = ProfileController.GetPlayerNameOrDefault();
		if (Defs.isInet)
		{
			PhotonNetwork.playerName = this.NamePlayer;
			this.photonView.RPC("SynhNickNameRPC", PhotonTargets.OthersBuffered, new object[]
			{
				this.NamePlayer
			});
		}
		else
		{
			base.GetComponent<NetworkView>().RPC("SynhNickNameRPC", RPCMode.OthersBuffered, new object[]
			{
				this.NamePlayer
			});
		}
	}

	// Token: 0x0600228B RID: 8843 RVA: 0x000A6D58 File Offset: 0x000A4F58
	[RPC]
	[PunRPC]
	private void SynhNickNameRPC(string _nick)
	{
		if (!this.isMine && WeaponManager.sharedManager.myPlayerMoveC != null)
		{
			WeaponManager.sharedManager.myPlayerMoveC.AddSystemMessage(_nick + " " + LocalizationStore.Get("Key_0995"), new Color(1f, 0.7f, 0f));
		}
		this.NamePlayer = _nick;
	}

	// Token: 0x0600228C RID: 8844 RVA: 0x000A6DC4 File Offset: 0x000A4FC4
	public void UpdateRanks()
	{
		if (this.myRanks != this.expController.currentLevel)
		{
			this.SetRanks();
		}
	}

	// Token: 0x0600228D RID: 8845 RVA: 0x000A6DE4 File Offset: 0x000A4FE4
	public void SetRanks()
	{
		this.myRanks = this.expController.currentLevel;
		if (Defs.isInet)
		{
			this.photonView.RPC("SynhRanksRPC", PhotonTargets.OthersBuffered, new object[]
			{
				this.myRanks
			});
		}
		else
		{
			base.GetComponent<NetworkView>().RPC("SynhRanksRPC", RPCMode.OthersBuffered, new object[]
			{
				this.myRanks
			});
		}
	}

	// Token: 0x0600228E RID: 8846 RVA: 0x000A6E5C File Offset: 0x000A505C
	[PunRPC]
	[RPC]
	private void SynhRanksRPC(int _ranks)
	{
		this.myRanks = _ranks;
	}

	// Token: 0x0600228F RID: 8847 RVA: 0x000A6E68 File Offset: 0x000A5068
	public void SynhCommand(PhotonPlayer player = null)
	{
		if (Defs.isInet)
		{
			if (player == null)
			{
				this.photonView.RPC("SynhCommandRPC", PhotonTargets.Others, new object[]
				{
					this.myCommand,
					this.myCommandOld
				});
			}
			else
			{
				this.photonView.RPC("SynhCommandRPC", player, new object[]
				{
					this.myCommand,
					this.myCommandOld
				});
			}
		}
		else
		{
			base.GetComponent<NetworkView>().RPC("SynhCommandRPC", RPCMode.Others, new object[]
			{
				this.myCommand,
				this.myCommandOld
			});
		}
	}

	// Token: 0x06002290 RID: 8848 RVA: 0x000A6F28 File Offset: 0x000A5128
	[PunRPC]
	[RPC]
	private void SynhCommandRPC(int _command, int _oldCommand)
	{
		this.myCommand = _command;
		this.myCommandOld = _oldCommand;
		if (this.myPlayerMoveC != null)
		{
			this.myPlayerMoveC.myCommand = this.myCommand;
			if (Initializer.redPlayers.Contains(this.myPlayerMoveC) && this.myCommand == 1)
			{
				Initializer.redPlayers.Remove(this.myPlayerMoveC);
			}
			if (Initializer.bluePlayers.Contains(this.myPlayerMoveC) && this.myCommand == 2)
			{
				Initializer.bluePlayers.Remove(this.myPlayerMoveC);
			}
			if (this.myCommand == 1 && !Initializer.bluePlayers.Contains(this.myPlayerMoveC))
			{
				Initializer.bluePlayers.Add(this.myPlayerMoveC);
			}
			if (this.myCommand == 2 && !Initializer.redPlayers.Contains(this.myPlayerMoveC))
			{
				Initializer.redPlayers.Add(this.myPlayerMoveC);
			}
			if (this.myPlayerMoveC.myNickLabelController != null)
			{
				this.myPlayerMoveC.myNickLabelController.SetCommandColor(this.myCommand);
			}
		}
	}

	// Token: 0x06002291 RID: 8849 RVA: 0x000A7058 File Offset: 0x000A5258
	public void SynhCountKills(PhotonPlayer player = null)
	{
		if (Defs.isInet)
		{
			if (player == null)
			{
				this.photonView.RPC("SynhCountKillsRPC", PhotonTargets.Others, new object[]
				{
					this.CountKills,
					this.oldCountKills
				});
			}
			else
			{
				this.photonView.RPC("SynhCountKillsRPC", player, new object[]
				{
					this.CountKills,
					this.oldCountKills
				});
			}
		}
		else
		{
			base.GetComponent<NetworkView>().RPC("SynhCountKillsRPC", RPCMode.Others, new object[]
			{
				this.CountKills,
				this.oldCountKills
			});
		}
	}

	// Token: 0x06002292 RID: 8850 RVA: 0x000A7118 File Offset: 0x000A5318
	[PunRPC]
	[RPC]
	private void SynhCountKillsRPC(int _countKills, int _oldCountKills)
	{
		this.CountKills = _countKills;
		this.oldCountKills = _oldCountKills;
	}

	// Token: 0x06002293 RID: 8851 RVA: 0x000A7128 File Offset: 0x000A5328
	public void SynhScore()
	{
		if (this.timerSynchScore < 0f)
		{
			this.timerSynchScore = 1f;
		}
	}

	// Token: 0x06002294 RID: 8852 RVA: 0x000A7148 File Offset: 0x000A5348
	public void ResetOldScore()
	{
		this.scoreOld = 0;
		this.score = 0;
		this.SynhScore();
		this.oldCountKills = 0;
		this.CountKills = 0;
		this.SynhCountKills(null);
		this.GetMyTeam();
	}

	// Token: 0x06002295 RID: 8853 RVA: 0x000A717C File Offset: 0x000A537C
	public void SendSynhScore(PhotonPlayer player = null)
	{
		if (Defs.isInet)
		{
			if (player == null)
			{
				this.photonView.RPC("SynhScoreRPC", PhotonTargets.Others, new object[]
				{
					this.score,
					this.scoreOld
				});
			}
			else
			{
				this.photonView.RPC("SynhScoreRPC", player, new object[]
				{
					this.score,
					this.scoreOld
				});
			}
		}
		else
		{
			base.GetComponent<NetworkView>().RPC("SynhScoreRPC", RPCMode.Others, new object[]
			{
				this.score,
				this.scoreOld
			});
		}
	}

	// Token: 0x06002296 RID: 8854 RVA: 0x000A723C File Offset: 0x000A543C
	[PunRPC]
	[RPC]
	private void SynhScoreRPC(int _score, int _oldScore)
	{
		this.score = _score;
		this.scoreOld = _oldScore;
	}

	// Token: 0x06002297 RID: 8855 RVA: 0x000A724C File Offset: 0x000A544C
	[Obfuscation(Exclude = true)]
	private void hideMessag()
	{
		this.showMessagFacebook = false;
	}

	// Token: 0x06002298 RID: 8856 RVA: 0x000A7258 File Offset: 0x000A5458
	private void Start()
	{
		this.waitingPlayerLocalize = LocalizationStore.Key_0565;
		this.matchLocalize = LocalizationStore.Key_0566;
		this.preparingLocalize = LocalizationStore.Key_0567;
		this.lanScan = base.GetComponent<LANBroadcastService>();
		try
		{
			this.StartUnsafe();
		}
		catch (Exception message)
		{
			UnityEngine.Debug.LogError(message);
		}
		if (this.isMine && !TrainingController.TrainingCompleted)
		{
			AnalyticsStuff.Tutorial(AnalyticsConstants.TutorialState.Table_Battle, 0);
		}
	}

	// Token: 0x1700063E RID: 1598
	// (get) Token: 0x06002299 RID: 8857 RVA: 0x000A72E4 File Offset: 0x000A54E4
	public static Vector2 ExperiencePosRanks
	{
		get
		{
			return new Vector2(21f * Defs.Coef, 21f * Defs.Coef);
		}
	}

	// Token: 0x0600229A RID: 8858 RVA: 0x000A7304 File Offset: 0x000A5504
	private void StartUnsafe()
	{
		Stopwatch stopwatch = Stopwatch.StartNew();
		if (this.isMulti)
		{
			if (this.isLocal)
			{
				this.isMine = base.GetComponent<NetworkView>().isMine;
			}
			else
			{
				this.isMine = this.photonView.isMine;
			}
		}
		if (this.isMine)
		{
			this.networkStartTableNGUIController = ((GameObject)UnityEngine.Object.Instantiate(Resources.Load("NetworkStartTableNGUI"))).GetComponent<NetworkStartTableNGUIController>();
			this._cam = GameObject.FindGameObjectWithTag("CamTemp");
			StoreKitEventListener.State.PurchaseKey = "Start table";
			if (FriendsController.sharedController.clanLogo != null && !string.IsNullOrEmpty(FriendsController.sharedController.clanLogo))
			{
				byte[] data = Convert.FromBase64String(FriendsController.sharedController.clanLogo);
				Texture2D texture2D = new Texture2D(Defs.LogoWidth, Defs.LogoWidth);
				texture2D.LoadImage(data);
				texture2D.filterMode = FilterMode.Point;
				texture2D.Apply();
				this.myClanTexture = texture2D;
				if (this.isInet)
				{
					this.photonView.RPC("SetMyClanTexture", PhotonTargets.AllBuffered, new object[]
					{
						FriendsController.sharedController.clanLogo,
						FriendsController.sharedController.ClanID,
						FriendsController.sharedController.clanName,
						FriendsController.sharedController.clanLeaderID
					});
				}
				else
				{
					base.GetComponent<NetworkView>().RPC("SetMyClanTexture", RPCMode.AllBuffered, new object[]
					{
						FriendsController.sharedController.clanLogo,
						FriendsController.sharedController.ClanID,
						FriendsController.sharedController.clanName,
						FriendsController.sharedController.clanLeaderID
					});
				}
			}
			base.Invoke("GetMyTeam", 1.5f);
			if (Defs.isMulti && Defs.isInet)
			{
				this.photonView.RPC("SynchGameRating", PhotonTargets.Others, new object[]
				{
					this.gameRating
				});
			}
			if (Defs.isDuel)
			{
				DuelController.instance.StartDuelMode();
			}
		}
		if (this.isHunger)
		{
			this.hungerGameController = HungerGameController.Instance;
		}
		this.expController = ExperienceController.sharedController;
		this.expController.posRanks = NetworkStartTable.ExperiencePosRanks;
		this._weaponManager = WeaponManager.sharedManager;
		if (this.isMulti && this.isMine)
		{
			if (!NetworkStartTable.StartAfterDisconnect)
			{
				if (NetworkStartTableNGUIController.sharedController != null)
				{
					NetworkStartTableNGUIController.sharedController.ShowStartInterface();
				}
				this.showTable = true;
			}
			else
			{
				this.showTable = GlobalGameController.showTableMyPlayer;
				this.isDeadInHungerGame = GlobalGameController.imDeadInHungerGame;
				if (this.showTable || this.isEndInHunger || (Defs.isDuel && DuelController.instance.roomStatus == DuelController.RoomStatus.None))
				{
					if (!this.isDeadInHungerGame && !this.isEndInHunger)
					{
						if (NetworkStartTableNGUIController.sharedController != null)
						{
							NetworkStartTableNGUIController.sharedController.ShowStartInterface();
						}
					}
					else if (NetworkStartTableNGUIController.sharedController != null)
					{
						NetworkStartTableNGUIController.sharedController.ShowEndInterface(string.Empty, 0, false);
					}
				}
				else
				{
					if (NetworkStartTableNGUIController.sharedController != null)
					{
						NetworkStartTableNGUIController.sharedController.HideStartInterface();
					}
					base.Invoke("startPlayer", 0.1f);
				}
			}
			NickLabelController.currentCamera = Initializer.Instance.tc.GetComponent<Camera>();
			this.tempCam.SetActive(true);
			string namePlayer = FilterBadWorld.FilterString(ProfileController.GetPlayerNameOrDefault());
			this.NamePlayer = namePlayer;
			this.pixelBookID = FriendsController.sharedController.id;
			if (!this.isInet)
			{
				base.GetComponent<NetworkView>().RPC("SetPixelBookID", RPCMode.OthersBuffered, new object[]
				{
					this.pixelBookID
				});
			}
			else
			{
				this.photonView.RPC("SetPixelBookID", PhotonTargets.OthersBuffered, new object[]
				{
					this.pixelBookID
				});
			}
			if (this.isServer && !this.isInet)
			{
				this.lanScan.serverMessage.name = PlayerPrefs.GetString("ServerName");
				this.lanScan.serverMessage.map = PlayerPrefs.GetString("MapName");
				this.lanScan.serverMessage.connectedPlayers = 0;
				this.lanScan.serverMessage.playerLimit = int.Parse(PlayerPrefs.GetString("PlayersLimits"));
				this.lanScan.serverMessage.comment = PlayerPrefs.GetString("MaxKill");
				this.lanScan.serverMessage.regim = (int)ConnectSceneNGUIController.regim;
				this.lanScan.StartAnnounceBroadCasting();
				UnityEngine.Debug.Log("lanScan.serverMessage.regim=" + this.lanScan.serverMessage.regim);
			}
			else
			{
				this.lanScan.enabled = false;
			}
			if (NetworkStartTable.StartAfterDisconnect)
			{
				this.CountKills = GlobalGameController.CountKills;
				this.score = GlobalGameController.Score;
				base.Invoke("synchState", 1f);
			}
			else
			{
				this.CountKills = -1;
				this.score = -1;
				GlobalGameController.CountKills = 0;
				GlobalGameController.Score = 0;
				base.Invoke("synchState", 1f);
			}
			this.expController = ExperienceController.sharedController;
			this.SetNewNick();
			this.SetRanks();
			this.SynhCountKills(null);
			this.SynhScore();
			this.sendMySkin();
			ShopNGUIController.sharedShop.onEquipSkinAction = delegate(string id)
			{
				this.sendMySkin();
			};
		}
		else
		{
			this.showTable = false;
		}
		stopwatch.Stop();
	}

	// Token: 0x0600229B RID: 8859 RVA: 0x000A7878 File Offset: 0x000A5A78
	private void GetMyTeam()
	{
		if (this.isMine && !NetworkStartTable.LocalOrPasswordRoom() && (ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.CapturePoints || ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.TeamFight || ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.FlagCapture))
		{
			this.myCommand = this.GetMyCommandOnStart();
			this.SynhCommand(null);
		}
	}

	// Token: 0x0600229C RID: 8860 RVA: 0x000A78D0 File Offset: 0x000A5AD0
	[RPC]
	[PunRPC]
	private void SetMyClanTexture(string str, string _clanID, string _clanName, string _clanLeaderId)
	{
		try
		{
			byte[] data = Convert.FromBase64String(str);
			Texture2D texture2D = new Texture2D(Defs.LogoWidth, Defs.LogoWidth);
			texture2D.LoadImage(data);
			texture2D.filterMode = FilterMode.Point;
			texture2D.Apply();
			this.myClanTexture = texture2D;
		}
		catch (Exception message)
		{
			UnityEngine.Debug.Log(message);
		}
		this.myClanID = _clanID;
		this.myClanName = _clanName;
		this.myClanLeaderID = _clanLeaderId;
	}

	// Token: 0x0600229D RID: 8861 RVA: 0x000A7954 File Offset: 0x000A5B54
	[PunRPC]
	[RPC]
	private void setMySkin(byte[] _skinByte)
	{
		if (this.photonView == null || !Defs.isMulti)
		{
			return;
		}
		Texture2D texture2D = new Texture2D(64, 32);
		texture2D.LoadImage(_skinByte);
		texture2D.filterMode = FilterMode.Point;
		texture2D.Apply();
		this.mySkin = texture2D;
		foreach (Player_move_c player_move_c in Initializer.players)
		{
			if (player_move_c.mySkinName.photonView.owner != null && player_move_c.mySkinName.photonView.owner.Equals(this.photonView.owner))
			{
				if (player_move_c.myNetworkStartTable == null)
				{
					player_move_c.setMyTamble(base.gameObject);
				}
				else
				{
					player_move_c._skin = this.mySkin;
					player_move_c.SetTextureForBodyPlayer(player_move_c._skin);
				}
			}
		}
	}

	// Token: 0x0600229E RID: 8862 RVA: 0x000A7A68 File Offset: 0x000A5C68
	[RPC]
	[PunRPC]
	private void setMySkinLocal(string str1, string str2)
	{
		byte[] data = Convert.FromBase64String(str1 + str2);
		Texture2D texture2D = new Texture2D(64, 32);
		texture2D.LoadImage(data);
		texture2D.filterMode = FilterMode.Point;
		texture2D.Apply();
		this.mySkin = texture2D;
		if (base.GetComponent<NetworkView>().isMine && WeaponManager.sharedManager.myPlayer != null)
		{
			WeaponManager.sharedManager.myPlayerMoveC.SetIDMyTable(base.GetComponent<NetworkView>().viewID.ToString());
		}
	}

	// Token: 0x0600229F RID: 8863 RVA: 0x000A7AF0 File Offset: 0x000A5CF0
	public void sendMySkin()
	{
		this.mySkin = SkinsController.currentSkinForPers;
		Texture2D texture2D = this.mySkin as Texture2D;
		byte[] array = texture2D.EncodeToPNG();
		if (this.isInet)
		{
			this.photonView.RPC("setMySkin", PhotonTargets.AllBuffered, new object[]
			{
				array
			});
		}
		else
		{
			string text = Convert.ToBase64String(array);
			base.GetComponent<NetworkView>().RPC("setMySkinLocal", RPCMode.AllBuffered, new object[]
			{
				text.Substring(0, text.Length / 2),
				text.Substring(text.Length / 2, text.Length / 2)
			});
		}
	}

	// Token: 0x060022A0 RID: 8864 RVA: 0x000A7B90 File Offset: 0x000A5D90
	public void ResetCamPlayer(int _nextPrev = 0)
	{
		if (_nextPrev != 0 && Initializer.players.Count == 1)
		{
			return;
		}
		if (Initializer.players.Count > 0)
		{
			if (_nextPrev == 0)
			{
				this.numberPlayerCun = UnityEngine.Random.Range(0, Initializer.players.Count);
				this.numberPlayerCunId = Initializer.players[this.numberPlayerCun].mySkinName.photonView.ownerId;
			}
			if (_nextPrev == 1)
			{
				int num = 10000000;
				int num2 = Initializer.players[0].mySkinName.photonView.ownerId;
				foreach (Player_move_c player_move_c in Initializer.players)
				{
					int ownerId = player_move_c.mySkinName.photonView.ownerId;
					if (ownerId < num2)
					{
						num2 = ownerId;
					}
					if (ownerId > this.numberPlayerCunId && ownerId < num)
					{
						num = ownerId;
					}
				}
				if (num == 10000000)
				{
					this.numberPlayerCunId = num2;
				}
				else
				{
					this.numberPlayerCunId = num;
				}
				for (int i = 0; i < Initializer.players.Count; i++)
				{
					if (Initializer.players[i].mySkinName.photonView.ownerId == this.numberPlayerCunId)
					{
						this.numberPlayerCun = i;
						break;
					}
				}
			}
			if (_nextPrev == -1)
			{
				int num3 = -1;
				int num4 = Initializer.players[0].mySkinName.photonView.ownerId;
				foreach (Player_move_c player_move_c2 in Initializer.players)
				{
					int ownerId2 = player_move_c2.mySkinName.photonView.ownerId;
					if (ownerId2 > num4)
					{
						num4 = ownerId2;
					}
					if (ownerId2 < this.numberPlayerCunId)
					{
						num3 = ownerId2;
					}
				}
				if (num3 == -1)
				{
					this.numberPlayerCunId = num4;
				}
				else
				{
					this.numberPlayerCunId = num3;
				}
				for (int j = 0; j < Initializer.players.Count; j++)
				{
					if (Initializer.players[j].mySkinName.photonView.ownerId == this.numberPlayerCunId)
					{
						this.numberPlayerCun = j;
						break;
					}
				}
			}
			if (this.currentCamPlayer != null)
			{
				this.currentCamPlayer.SetActive(false);
				if (!this.currentPlayerMoveCVidos.isMechActive)
				{
					this.currentFPSPlayer.SetActive(true);
				}
				if (this.currentBodyMech != null)
				{
					this.currentBodyMech.SetActive(true);
				}
				Player_move_c.SetLayerRecursively(this.currentGameObjectPlayer.transform.GetChild(0).gameObject, 0);
				this.currentGameObjectPlayer.GetComponent<InterolationGameObject>().sglajEnabled = false;
				this.currentCamPlayer.transform.parent.GetComponent<ThirdPersonNetwork1>().sglajEnabledVidos = false;
				this.currentCamPlayer = null;
				this.currentFPSPlayer = null;
				this.currentBodyMech = null;
				this.currentGameObjectPlayer = null;
				this.currentPlayerMoveCVidos = null;
			}
			SkinName mySkinName = Initializer.players[this.numberPlayerCun].mySkinName;
			mySkinName.camPlayer.SetActive(true);
			this.playerVidosNick = mySkinName.NickName;
			this.playerVidosClanName = mySkinName.playerMoveC.myTable.GetComponent<NetworkStartTable>().myClanName;
			this.playerVidosClanTexture = mySkinName.playerMoveC.myTable.GetComponent<NetworkStartTable>().myClanTexture;
			this.currentPlayerMoveCVidos = mySkinName.playerMoveC;
			this.currentCamPlayer = mySkinName.camPlayer;
			this.currentFPSPlayer = mySkinName.FPSplayerObject;
			this.currentBodyMech = ((!Defs.isDaterRegim) ? ((!(mySkinName.playerMoveC.currentMech != null)) ? null : mySkinName.playerMoveC.currentMech.body) : mySkinName.playerMoveC.mechBearBody);
			Initializer.players[this.numberPlayerCun].myPersonNetwork.sglajEnabledVidos = true;
			this.currentGameObjectPlayer = mySkinName.playerGameObject;
			this.currentGameObjectPlayer.GetComponent<InterolationGameObject>().sglajEnabled = true;
			this.currentFPSPlayer.SetActive(false);
			if (this.currentBodyMech != null)
			{
				this.currentBodyMech.SetActive(false);
			}
			NickLabelController.currentCamera = mySkinName.camPlayer.GetComponent<Camera>();
			Player_move_c.SetLayerRecursively(this.currentGameObjectPlayer.transform.GetChild(0).gameObject, 9);
		}
		else
		{
			this._cam.SetActive(true);
			this.showTable = true;
			this.isRegimVidos = false;
			NickLabelController.currentCamera = this._cam.GetComponent<Camera>();
			if (this.inGameGUI != null)
			{
				this.inGameGUI.ResetScope();
			}
		}
	}

	// Token: 0x060022A1 RID: 8865 RVA: 0x000A80B4 File Offset: 0x000A62B4
	private int GetMyCommandOnStart()
	{
		if (this.myCommand > 0)
		{
			return this.myCommand;
		}
		int num = 0;
		int num2 = 0;
		for (int i = 0; i < Initializer.networkTables.Count; i++)
		{
			if (Initializer.networkTables[i].myCommand == 1)
			{
				num++;
			}
			if (Initializer.networkTables[i].myCommand == 2)
			{
				num2++;
			}
		}
		if (num2 < num)
		{
			return 2;
		}
		if (num2 > num)
		{
			return 1;
		}
		float num3 = (!ABTestController.useBuffSystem) ? KillRateCheck.instance.GetKillRate() : BuffSystem.instance.GetKillrateByInteractions();
		int winningTeam = this.GetWinningTeam();
		if (winningTeam == 0)
		{
			return UnityEngine.Random.Range(1, 3);
		}
		if (num3 < 1f)
		{
			return winningTeam;
		}
		return (winningTeam != 1) ? 1 : 2;
	}

	// Token: 0x060022A2 RID: 8866 RVA: 0x000A8194 File Offset: 0x000A6394
	private void ReplaceCommand()
	{
		this.myCommand = ((this.myCommand != 1) ? 1 : 2);
		this.SynhCommand(null);
		this.score = 0;
		this.CountKills = 0;
		GlobalGameController.Score = 0;
		GlobalGameController.CountKills = 0;
		if (WeaponManager.sharedManager.myPlayerMoveC != null)
		{
			WeaponManager.sharedManager.myPlayerMoveC.countKills = 0;
			WeaponManager.sharedManager.myPlayerMoveC.myCommand = this.myCommand;
			WeaponManager.sharedManager.myPlayerMoveC.myBaza = null;
			WeaponManager.sharedManager.myPlayerMoveC.myFlag = null;
			WeaponManager.sharedManager.myPlayerMoveC.enemyFlag = null;
			if (Initializer.redPlayers.Contains(WeaponManager.sharedManager.myPlayerMoveC) && this.myCommand == 1)
			{
				Initializer.redPlayers.Remove(WeaponManager.sharedManager.myPlayerMoveC);
			}
			if (Initializer.bluePlayers.Contains(WeaponManager.sharedManager.myPlayerMoveC) && this.myCommand == 2)
			{
				Initializer.bluePlayers.Remove(WeaponManager.sharedManager.myPlayerMoveC);
			}
			if (this.myCommand == 1 && !Initializer.bluePlayers.Contains(WeaponManager.sharedManager.myPlayerMoveC))
			{
				Initializer.bluePlayers.Add(WeaponManager.sharedManager.myPlayerMoveC);
			}
			if (this.myCommand == 2 && !Initializer.redPlayers.Contains(WeaponManager.sharedManager.myPlayerMoveC))
			{
				Initializer.redPlayers.Add(WeaponManager.sharedManager.myPlayerMoveC);
			}
		}
	}

	// Token: 0x060022A3 RID: 8867 RVA: 0x000A832C File Offset: 0x000A652C
	private void Update()
	{
		if (this.isMine)
		{
			if (this.inGameGUI == null)
			{
				this.inGameGUI = InGameGUI.sharedInGameGUI;
			}
			if (this.timerSynchScore > 0f)
			{
				this.timerSynchScore -= Time.deltaTime;
				if (this.timerSynchScore < 0f)
				{
					this.SendSynhScore(null);
				}
			}
			bool flag = this.isShowNickTable || this.showDisconnectFromServer || this.showDisconnectFromMasterServer || this.showTable || this.showMessagFacebook;
			if (this.guiObj.activeSelf != flag)
			{
				this.guiObj.SetActive(flag);
			}
			if (this.inGameGUI == null)
			{
				this.inGameGUI = InGameGUI.sharedInGameGUI;
			}
			if (this._pauser == null)
			{
				this._pauser = Pauser.sharedPauser;
			}
			if (ShopNGUIController.GuiActive || (ProfileController.Instance != null && ProfileController.Instance.InterfaceEnabled))
			{
				this.expController.isShowRanks = (SkinEditorController.sharedController == null && (!(BankController.Instance != null) || !BankController.Instance.InterfaceEnabled));
			}
			else if (BankController.Instance != null && BankController.Instance.InterfaceEnabled)
			{
				this.expController.isShowRanks = false;
			}
			else if (this._pauser != null && this._pauser.paused)
			{
				if (PauseNGUIController.sharedController != null)
				{
					this.expController.isShowRanks = !PauseNGUIController.sharedController.SettingsJoysticksPanel.activeInHierarchy;
				}
			}
			else if ((this.showTable || this.isShowNickTable) && !this.isRegimVidos && this._shopInstance == null && !LoadingInAfterGame.isShowLoading && !this.isGoRandomRoom)
			{
				this.expController.isShowRanks = (!this.isShowFinished && (!(this.networkStartTableNGUIController != null) || this.networkStartTableNGUIController.rentScreenPoint.childCount == 0));
			}
			else
			{
				this.expController.isShowRanks = false;
			}
			if (this.isRegimVidos && this.isDeadInHungerGame && this._cam.activeInHierarchy && Initializer.players.Count > 0)
			{
				this._cam.SetActive(false);
				this.ResetCamPlayer(0);
			}
			if (this.isRegimVidos && this.isDeadInHungerGame && this.currentCamPlayer == null)
			{
				this.ResetCamPlayer(0);
			}
			if (!this.isRegimVidos && this.isDeadInHungerGame && this.currentCamPlayer != null)
			{
				this.currentCamPlayer.SetActive(false);
				if (!this.currentPlayerMoveCVidos.isMechActive)
				{
					this.currentFPSPlayer.SetActive(true);
				}
				if (this.currentBodyMech != null)
				{
					this.currentBodyMech.SetActive(true);
				}
				this.currentCamPlayer = null;
				this.currentFPSPlayer = null;
				this.currentBodyMech = null;
				this._cam.SetActive(true);
			}
			if (this.isRegimVidos && this.inGameGUI != null && this.currentPlayerMoveCVidos.isZooming != this.oldIsZomming)
			{
				this.oldIsZomming = this.currentPlayerMoveCVidos.isZooming;
				if (this.oldIsZomming)
				{
					string text = string.Empty;
					float fieldOfView = 60f;
					if (this.currentGameObjectPlayer.transform.childCount > 0)
					{
						try
						{
							text = ItemDb.GetByPrefabName(this.currentGameObjectPlayer.transform.GetChild(0).name.Replace("(Clone)", string.Empty)).Tag;
						}
						catch (Exception arg)
						{
							if (Application.isEditor)
							{
								UnityEngine.Debug.LogWarning("Exception  tagWeapon = ItemDb.GetByPrefabName(currentGameObjectPlayer.transform.GetChild(0).name.Replace(\"(Clone)\",\"\")).Tag:  " + arg);
							}
						}
						fieldOfView = this.currentGameObjectPlayer.transform.GetChild(0).GetComponent<WeaponSounds>().fieldOfViewZomm;
					}
					if (!text.Equals(string.Empty))
					{
						this.inGameGUI.SetScopeForWeapon(string.Empty + this.currentGameObjectPlayer.transform.GetChild(0).GetComponent<WeaponSounds>().scopeNum);
					}
					this.currentPlayerMoveCVidos.myCamera.fieldOfView = fieldOfView;
					this.currentPlayerMoveCVidos.gunCamera.fieldOfView = 1f;
				}
				else
				{
					this.currentPlayerMoveCVidos.myCamera.fieldOfView = 44f;
					this.currentPlayerMoveCVidos.gunCamera.fieldOfView = 75f;
					this.inGameGUI.ResetScope();
				}
			}
			if (Defs.isFlag || Defs.isCompany || Defs.isCapturePoints)
			{
				if (Defs.isInet && this.myCommand > 0)
				{
					int num = 0;
					for (int i = 0; i < Initializer.networkTables.Count; i++)
					{
						if (Initializer.networkTables[i] != null && Initializer.networkTables[i].myCommand == this.myCommand)
						{
							num++;
						}
					}
					if (num > 5)
					{
						int num2 = -1;
						for (int j = 0; j < Initializer.networkTables.Count; j++)
						{
							if (Initializer.networkTables[j] != null && Initializer.networkTables[j].myCommand == this.myCommand && Initializer.networkTables[j].photonView.ownerId > num2)
							{
								num2 = Initializer.networkTables[j].photonView.ownerId;
							}
						}
						if (num2 == this.photonView.ownerId)
						{
							this.ReplaceCommand();
						}
					}
				}
				if (Defs.isFlag)
				{
					this.timerFlag = TimeGameController.sharedController.timerToEndMatch;
					if (this.timerFlag < 0.0)
					{
						this.timerFlag = 0.0;
					}
					if (this.timerFlag < 0.10000000149011612)
					{
						if (WeaponManager.sharedManager.myPlayerMoveC != null && WeaponManager.sharedManager.myPlayerMoveC.enabled)
						{
							WeaponManager.sharedManager.myPlayerMoveC.enabled = false;
							InGameGUI.sharedInGameGUI.gameObject.SetActive(false);
							base.Invoke("ClearScoreCommandInFlagGame", 0.5f);
							ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
							hashtable["TimeMatchEnd"] = -9000000.0;
							PhotonNetwork.room.SetCustomProperties(hashtable, null, false);
							if (this.scoreCommandFlag1 > this.scoreCommandFlag2)
							{
								this.win(string.Empty, 1, this.scoreCommandFlag1, this.scoreCommandFlag2);
							}
							else if (this.scoreCommandFlag1 < this.scoreCommandFlag2)
							{
								this.win(string.Empty, 2, this.scoreCommandFlag1, this.scoreCommandFlag2);
							}
							else
							{
								this.win(string.Empty, 0, this.scoreCommandFlag1, this.scoreCommandFlag2);
							}
						}
					}
					else if (this.inGameGUI != null && this.inGameGUI.message_draw.activeSelf)
					{
						this.inGameGUI.message_draw.SetActive(false);
					}
				}
			}
			if (this.isHunger && this.hungerGameController != null && this.hungerGameController.isStartGame && !this.hungerGameController.isRunPlayer && !this.isEndInHunger)
			{
				UnityEngine.Debug.Log("Start hunger player");
				this.hungerGameController.isRunPlayer = true;
				this.isShowNickTable = false;
				this.CountKills = 0;
				this.score = 0;
				GlobalGameController.Score = 0;
				this.isDrawInHanger = false;
				this.startPlayer();
				this.countMigZagolovok = 0;
				this.timeTomig = 0.7f;
				this.isMigZag = false;
				this.SynhCountKills(null);
				this.SynhScore();
				return;
			}
			if (this.isHunger && this.hungerGameController != null && !this.hungerGameController.isStartGame)
			{
				string text2 = string.Empty;
				if (!this.hungerGameController.isStartTimer)
				{
					text2 = this.waitingPlayerLocalize;
				}
				else
				{
					if (this.hungerGameController.startTimer > 0f && !this.hungerGameController.isStartGame)
					{
						float startTimer = this.hungerGameController.startTimer;
						text2 = string.Concat(new object[]
						{
							this.matchLocalize,
							" ",
							Mathf.FloorToInt(startTimer / 60f),
							":",
							(Mathf.FloorToInt(startTimer - (float)(Mathf.FloorToInt(startTimer / 60f) * 60)) >= 10) ? string.Empty : "0",
							Mathf.FloorToInt(startTimer - (float)(Mathf.FloorToInt(startTimer / 60f) * 60))
						});
					}
					if (this.hungerGameController.startTimer < 0f && !this.hungerGameController.isStartGame)
					{
						text2 = this.preparingLocalize;
					}
				}
				if (NetworkStartTableNGUIController.sharedController != null)
				{
					NetworkStartTableNGUIController.sharedController.HungerStartLabel.text = text2;
				}
			}
			if (Defs.isFlag && this.isInet && PhotonNetwork.isMasterClient && Initializer.flag1 == null)
			{
				this.AddFlag();
			}
		}
		if (!this.isLocal && this.isMine)
		{
			GlobalGameController.showTableMyPlayer = this.showTable;
			GlobalGameController.imDeadInHungerGame = this.isDeadInHungerGame;
		}
		if (this.isLocal && this.isServer && this.lanScan != null)
		{
			this.lanScan.serverMessage.connectedPlayers = GameObject.FindGameObjectsWithTag("NetworkTable").Length;
		}
		if (this.timerShow >= 0f)
		{
			this.timerShow -= Time.deltaTime;
			if (this.timerShow < 0f)
			{
				ActivityIndicator.IsActiveIndicator = false;
				ConnectSceneNGUIController.Local();
			}
		}
	}

	// Token: 0x060022A4 RID: 8868 RVA: 0x000A8E04 File Offset: 0x000A7004
	private void OnPlayerConnected(NetworkPlayer player)
	{
		if (base.GetComponent<NetworkView>().isMine)
		{
			this.SynhCommand(null);
			this.SynhCountKills(null);
			this.SynhScore();
		}
	}

	// Token: 0x060022A5 RID: 8869 RVA: 0x000A8E38 File Offset: 0x000A7038
	private void OnDisconnectedFromServer(NetworkDisconnection info)
	{
		UnityEngine.Debug.Log("OnDisconnectedFromServer");
		this.showDisconnectFromServer = true;
		this.timerShow = 3f;
	}

	// Token: 0x060022A6 RID: 8870 RVA: 0x000A8E58 File Offset: 0x000A7058
	private void OnPlayerDisconnected(NetworkPlayer player)
	{
		Network.RemoveRPCs(player);
		Network.DestroyPlayerObjects(player);
		foreach (Player_move_c player_move_c in Initializer.players)
		{
			if (player.ipAddress.Equals(player_move_c.myIp) && NickLabelStack.sharedStack != null)
			{
				NickLabelController[] lables = NickLabelStack.sharedStack.lables;
				foreach (NickLabelController nickLabelController in lables)
				{
					if (nickLabelController.target == player_move_c.transform)
					{
						nickLabelController.target = null;
						break;
					}
				}
				UnityEngine.Object.Destroy(player_move_c.mySkinName.gameObject);
			}
		}
	}

	// Token: 0x060022A7 RID: 8871 RVA: 0x000A8F48 File Offset: 0x000A7148
	private void OnFailedToConnectToMasterServer(NetworkConnectionError info)
	{
		UnityEngine.Debug.Log("Could not connect to master server: " + info);
		this.showDisconnectFromMasterServer = true;
		this.timerShow = 3f;
	}

	// Token: 0x060022A8 RID: 8872 RVA: 0x000A8F74 File Offset: 0x000A7174
	public void WinInHunger()
	{
		this.isIwin = true;
		this.photonView.RPC("winInHungerRPC", PhotonTargets.AllBuffered, new object[]
		{
			this.NamePlayer
		});
	}

	// Token: 0x060022A9 RID: 8873 RVA: 0x000A8FA0 File Offset: 0x000A71A0
	public IEnumerator DrawInHanger()
	{
		while (ShopNGUIController.GuiActive || BankController.Instance.uiRoot.gameObject.activeInHierarchy || (this._pauser != null && this._pauser.paused))
		{
			yield return null;
		}
		this.isEndInHunger = true;
		this.isDrawInHanger = true;
		this.showTable = true;
		if (this._cam != null)
		{
			this._cam.SetActive(true);
		}
		if (!this.isSetNewMapButton)
		{
			this.isSetNewMapButton = true;
			NetworkStartTableNGUIController.sharedController.UpdateGoMapButtons(true);
		}
		NickLabelController.currentCamera = Initializer.Instance.tc.GetComponent<Camera>();
		for (int i = 0; i < Initializer.players.Count; i++)
		{
			UnityEngine.Object.Destroy(Initializer.players[i].mySkinName.gameObject);
		}
		if (NetworkStartTableNGUIController.sharedController != null)
		{
			NetworkStartTableNGUIController.sharedController.ShowEndInterface("Time's out!", 0, false);
		}
		yield break;
	}

	// Token: 0x060022AA RID: 8874 RVA: 0x000A8FBC File Offset: 0x000A71BC
	[PunRPC]
	[RPC]
	public void DrawInHangerRPC()
	{
	}

	// Token: 0x060022AB RID: 8875 RVA: 0x000A8FC0 File Offset: 0x000A71C0
	[RPC]
	[PunRPC]
	public void winInHungerRPC(string winner)
	{
		this.isEndInHunger = true;
		if (this._weaponManager != null && this._weaponManager.myTable != null)
		{
			this._weaponManager.myTable.GetComponent<NetworkStartTable>().win(winner, 0, 0, 0);
		}
		if (NetworkStartTableNGUIController.sharedController != null)
		{
			NetworkStartTableNGUIController.sharedController.ranksTable.isShowRanks = false;
		}
	}

	// Token: 0x060022AC RID: 8876 RVA: 0x000A9034 File Offset: 0x000A7234
	public static void IncreaseTimeInMode(int mode, double minutes)
	{
		if (ExperienceController.sharedController != null)
		{
			string key = mode.ToString();
			string key2 = "Statistics.TimeInMode.Level" + ExperienceController.sharedController.currentLevel;
			if (PlayerPrefs.HasKey(key2))
			{
				string @string = PlayerPrefs.GetString(key2, "{}");
				UnityEngine.Debug.Log("Time in mode string:    " + @string);
				try
				{
					Dictionary<string, object> dictionary = (Rilisoft.MiniJson.Json.Deserialize(@string) as Dictionary<string, object>) ?? new Dictionary<string, object>();
					object value;
					if (dictionary.TryGetValue(key, out value))
					{
						double num = Convert.ToDouble(value) + minutes;
						dictionary[key] = num;
					}
					else
					{
						dictionary.Add(key, minutes);
					}
					string value2 = Rilisoft.MiniJson.Json.Serialize(dictionary);
					PlayerPrefs.SetString(key2, value2);
				}
				catch (OverflowException exception)
				{
					UnityEngine.Debug.LogError("Cannot deserialize time-in-mode:    " + @string);
					UnityEngine.Debug.LogException(exception);
				}
				catch (Exception exception2)
				{
					UnityEngine.Debug.LogError("Unknown exception:    " + @string);
					UnityEngine.Debug.LogException(exception2);
				}
			}
			string key3 = "Statistics.RoundsInMode.Level" + ExperienceController.sharedController.currentLevel;
			if (PlayerPrefs.HasKey(key3))
			{
				string string2 = PlayerPrefs.GetString(key3);
				Dictionary<string, object> dictionary2 = (Rilisoft.MiniJson.Json.Deserialize(string2) as Dictionary<string, object>) ?? new Dictionary<string, object>();
				object value3;
				if (dictionary2.TryGetValue(key, out value3))
				{
					int num2 = Convert.ToInt32(value3) + 1;
					dictionary2[key] = num2;
				}
				else
				{
					dictionary2.Add(key, 1);
				}
				string value4 = Rilisoft.MiniJson.Json.Serialize(dictionary2);
				PlayerPrefs.SetString(key3, value4);
			}
			PlayerPrefs.Save();
		}
	}

	// Token: 0x060022AD RID: 8877 RVA: 0x000A9210 File Offset: 0x000A7410
	private IEnumerator WaitInterstitialRequestAndShowCoroutine(Task<Ad> request)
	{
		if (Defs.IsDeveloperBuild)
		{
			UnityEngine.Debug.Log("Waiting until interstitial request is completed...");
		}
		while (!request.IsCompleted)
		{
			yield return null;
		}
		if (request.IsFaulted)
		{
			UnityEngine.Debug.LogWarning("Interstitial request after match failed: " + request.Exception.InnerException.Message);
			yield break;
		}
		if (Defs.IsDeveloperBuild)
		{
			UnityEngine.Debug.Log("Interstitial request after match succeeded. Trying to show interstitial...");
		}
		yield return null;
		if (WeaponManager.sharedManager.myPlayer != null)
		{
			UnityEngine.Debug.LogWarning("Stop waiting: WeaponManager.sharedManager.myPlayer != null");
			yield break;
		}
		yield return null;
		if (NetworkStartTableNGUIController.sharedController.rewardWindow != null)
		{
			if (Defs.IsDeveloperBuild)
			{
				UnityEngine.Debug.Log("Waiting until Reward panel is closed...");
			}
			while (NetworkStartTableNGUIController.sharedController.isRewardShow)
			{
				yield return null;
			}
			yield return null;
			while (ExpController.Instance.WaitingForLevelUpView)
			{
				yield return null;
			}
			yield return null;
			if (Defs.IsDeveloperBuild)
			{
				UnityEngine.Debug.Log(string.Format("Waiting until Level up panel is closed if displayed ({0})...", ExpController.Instance.IsLevelUpShown));
			}
			while (ExpController.Instance.IsLevelUpShown)
			{
				yield return null;
			}
		}
		while (ShopNGUIController.GuiActive)
		{
			yield return null;
		}
		Dictionary<string, string> attributes = new Dictionary<string, string>
		{
			{
				"af_content_type",
				"Interstitial"
			},
			{
				"af_content_id",
				"Interstitial (NetworkTable)"
			}
		};
		AnalyticsFacade.SendCustomEventToAppsFlyer("af_content_view", attributes);
		MenuBackgroundMusic.sharedMusic.Stop();
		Task<AdResult> future = FyberFacade.Instance.ShowInterstitial(new Dictionary<string, string>
		{
			{
				"Context",
				"Multiplayer Table"
			}
		}, "NetworkStartTable.WaitInterstitialRequestAndShow()");
		while (!future.IsCompleted)
		{
			yield return null;
		}
		MenuBackgroundMusic.sharedMusic.Start();
		if (future.IsFaulted)
		{
			UnityEngine.Debug.LogWarningFormat("Interstitial show after match failed: {0}", new object[]
			{
				future.Exception.InnerException.Message
			});
		}
		else
		{
			UnityEngine.Debug.LogFormat("Interstitial show finished with status {0}: {1}", new object[]
			{
				future.Result.Status,
				future.Result.Message
			});
		}
		yield break;
	}

	// Token: 0x060022AE RID: 8878 RVA: 0x000A9234 File Offset: 0x000A7434
	public static bool LocalOrPasswordRoom()
	{
		return !Defs.isInet || (PhotonNetwork.room != null && !PhotonNetwork.room.customProperties[ConnectSceneNGUIController.passwordProperty].Equals(string.Empty));
	}

	// Token: 0x060022AF RID: 8879 RVA: 0x000A927C File Offset: 0x000A747C
	private bool CheckForDeadheatInDuel()
	{
		bool result;
		if (DuelController.instance.opponentNetworkTable != null)
		{
			NetworkStartTable myNetworkStartTable = WeaponManager.sharedManager.myNetworkStartTable;
			NetworkStartTable opponentNetworkTable = DuelController.instance.opponentNetworkTable;
			int num = (opponentNetworkTable.CountKills == -1) ? opponentNetworkTable.oldCountKills : opponentNetworkTable.CountKills;
			int num2 = (opponentNetworkTable.score == -1) ? opponentNetworkTable.scoreOld : opponentNetworkTable.score;
			result = (myNetworkStartTable.CountKills == num && myNetworkStartTable.score == num2);
		}
		else
		{
			result = (Initializer.networkTables.Count < 2 && (DuelController.instance.playingTime < 30f || WeaponManager.sharedManager.myNetworkStartTable.score < 5));
		}
		return result;
	}

	// Token: 0x060022B0 RID: 8880 RVA: 0x000A9350 File Offset: 0x000A7550
	private bool CheckForWin(int myPlace, int winnerTeam, int killCount, int myscore, bool scoreMatterForTeam = true)
	{
		switch (ConnectSceneNGUIController.regim)
		{
		case ConnectSceneNGUIController.RegimGame.Deathmatch:
		case ConnectSceneNGUIController.RegimGame.TimeBattle:
			return myPlace == 0 && myscore > 0;
		case ConnectSceneNGUIController.RegimGame.TeamFight:
		case ConnectSceneNGUIController.RegimGame.FlagCapture:
		case ConnectSceneNGUIController.RegimGame.CapturePoints:
			return this.myCommand == winnerTeam && (myscore > 0 || !scoreMatterForTeam);
		case ConnectSceneNGUIController.RegimGame.DeadlyGames:
			return killCount > 0 && this.isIwin;
		case ConnectSceneNGUIController.RegimGame.Duel:
			return myPlace == 0 && myscore > 0 && !this.CheckForDeadheatInDuel();
		}
		return false;
	}

	// Token: 0x060022B1 RID: 8881 RVA: 0x000A93F0 File Offset: 0x000A75F0
	public void win(string winner, int _commandWin = 0, int blueCount = 0, int redCount = 0)
	{
		if (NetworkStartTableNGUIController.sharedController.isRewardShow || this.isShowFinished)
		{
			return;
		}
		QuestSystem.Instance.SaveQuestProgressIfDirty();
		if (WeaponManager.sharedManager.myPlayerMoveC != null)
		{
			WeaponManager.sharedManager.myPlayerMoveC.GadgetsOnMatchEnd();
		}
		if (Defs.isInet)
		{
			PhotonNetwork.FetchServerTimestamp();
		}
		this._matchStopwatch.Stop();
		double totalMinutes = this._matchStopwatch.Elapsed.TotalMinutes;
		if (Defs.isHunger)
		{
			this.isEndInHunger = true;
		}
		if (Defs.isDaterRegim)
		{
			Storager.setInt("DaterDayLived", Storager.getInt("DaterDayLived", false) + 1, false);
		}
		if (Defs.isDaterRegim)
		{
			int timeGame;
			if (Defs.isInet)
			{
				timeGame = (int)PhotonNetwork.room.customProperties[ConnectSceneNGUIController.maxKillProperty];
			}
			else
			{
				timeGame = (PlayerPrefs.GetString("MaxKill", "9").Equals(string.Empty) ? 5 : int.Parse(PlayerPrefs.GetString("MaxKill", "5")));
			}
			AnalyticsStuff.LogSandboxTimeGamePopularity(timeGame, false);
		}
		StoreKitEventListener.State.PurchaseKey = "End match";
		if (!Defs.isHunger)
		{
			int @int = PlayerPrefs.GetInt("CountMatch", 0);
			int num = @int + 1;
			PlayerPrefs.SetInt("CountMatch", num);
			if (num <= 5)
			{
				AnalyticsStuff.Tutorial(AnalyticsConstants.TutorialState.Battle_End, num);
			}
			Dictionary<string, object> parameters = new Dictionary<string, object>
			{
				{
					"count",
					num
				}
			};
			AnalyticsFacade.SendCustomEventToFacebook("games_multiplayer_count", parameters);
			if (ExperienceController.sharedController != null)
			{
				string key = "Statistics.MatchCount.Level" + ExperienceController.sharedController.currentLevel;
				int int2 = PlayerPrefs.GetInt(key, 0);
				PlayerPrefs.SetInt(key, int2 + 1);
			}
			NetworkStartTable.IncreaseTimeInMode((int)ConnectSceneNGUIController.regim, this._matchStopwatch.Elapsed.TotalMinutes);
			this._matchStopwatch.Reset();
		}
		this.isShowAvard = false;
		this.commandWinner = _commandWin;
		GlobalGameController.countKillsBlue = 0;
		GlobalGameController.countKillsRed = 0;
		this.nickPobeditelya = winner;
		GameObject[] array = GameObject.FindGameObjectsWithTag("NetworkTable");
		List<GameObject> list = new List<GameObject>();
		List<GameObject> list2 = new List<GameObject>();
		List<GameObject> list3 = new List<GameObject>();
		if (ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.Deathmatch)
		{
			this.isDrawInDeathMatch = true;
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i].GetComponent<NetworkStartTable>().score >= AdminSettingsController.minScoreDeathMath)
				{
					this.isDrawInDeathMatch = false;
				}
			}
		}
		for (int j = 1; j < array.Length; j++)
		{
			NetworkStartTable component = array[j].GetComponent<NetworkStartTable>();
			for (int k = 0; k < j; k++)
			{
				NetworkStartTable component2 = array[k].GetComponent<NetworkStartTable>();
				int num2 = (component.score < 0) ? component.scoreOld : component.score;
				int num3 = (component.CountKills < 0) ? component.oldCountKills : component.CountKills;
				int num4 = (component2.score < 0) ? component2.scoreOld : component2.score;
				int num5 = (component2.CountKills < 0) ? component2.oldCountKills : component2.CountKills;
				if ((!Defs.isDuel && !Defs.isFlag && !Defs.isCapturePoints && (num2 > num4 || (num2 == num4 && num3 > num5))) || ((Defs.isDuel || Defs.isFlag || Defs.isCapturePoints) && (num3 > num5 || (num3 == num5 && num2 > num4))))
				{
					GameObject gameObject = array[j];
					for (int l = j - 1; l >= k; l--)
					{
						array[l + 1] = array[l];
					}
					array[k] = gameObject;
					break;
				}
			}
		}
		int num6 = 0;
		for (int m = 0; m < array.Length; m++)
		{
			int num7 = array[m].GetComponent<NetworkStartTable>().myCommand;
			if (num7 == -1)
			{
				num7 = array[m].GetComponent<NetworkStartTable>().myCommandOld;
			}
			if (num7 == 0)
			{
				if (array[m].Equals(base.gameObject))
				{
					num6 = list3.Count;
				}
				list3.Add(array[m]);
			}
			if (num7 == 1)
			{
				if (array[m].Equals(base.gameObject))
				{
					num6 = list.Count;
				}
				list.Add(array[m]);
			}
			if (num7 == 2)
			{
				if (array[m].Equals(base.gameObject))
				{
					num6 = list2.Count;
				}
				list2.Add(array[m]);
			}
		}
		this.oldSpisokName = new string[list3.Count];
		this.oldScoreSpisok = new string[list3.Count];
		this.oldCountLilsSpisok = new string[list3.Count];
		this.oldSpisokRanks = new int[list3.Count];
		this.oldIsDeadInHungerGame = new bool[list3.Count];
		this.oldSpisokPixelBookID = new string[list3.Count];
		this.oldSpisokMyClanLogo = new Texture[list3.Count];
		this.oldSpisokNameBlue = new string[list.Count];
		this.oldCountLilsSpisokBlue = new string[list.Count];
		this.oldSpisokRanksBlue = new int[list.Count];
		this.oldSpisokPixelBookIDBlue = new string[list.Count];
		this.oldSpisokMyClanLogoBlue = new Texture[list.Count];
		this.oldScoreSpisokBlue = new string[list.Count];
		this.oldSpisokNameRed = new string[list2.Count];
		this.oldCountLilsSpisokRed = new string[list2.Count];
		this.oldSpisokRanksRed = new int[list2.Count];
		this.oldSpisokPixelBookIDRed = new string[list2.Count];
		this.oldSpisokMyClanLogoRed = new Texture[list2.Count];
		this.oldScoreSpisokRed = new string[list2.Count];
		this.addCoins = 0;
		this.addExperience = 0;
		bool flag = false;
		if (Defs.isDuel)
		{
			flag = this.CheckForDeadheatInDuel();
			if (flag)
			{
				num6 = 1;
			}
		}
		bool flag2 = this.CheckForWin(num6, _commandWin, this.CountKills, this.score, true);
		bool iAmWinnerInTeam = this.CheckForWin(num6, _commandWin, this.CountKills, this.score, false);
		KillRateCheck.instance.LogFirstBattlesResult(flag2);
		RatingSystem.RatingChange ratingChange = this.CalculateMatchRating(false);
		Singleton<EggsManager>.Instance.OnMathEnded(flag2);
		if (Defs.isDuel)
		{
			SceneInfoController.instance.UpdateListAvaliableMap();
		}
		if (this.isInet)
		{
			if (Defs.abTestBalansCohort == Defs.ABTestCohortsType.B || this.myCommand == _commandWin || (!Defs.isCompany && !Defs.isFlag && !Defs.isCapturePoints) || ExperienceController.sharedController.currentLevel < 2)
			{
				if (Defs.abTestBalansCohort == Defs.ABTestCohortsType.B && (Defs.isCompany || Defs.isFlag || Defs.isCapturePoints))
				{
					this.isIwin = (this.myCommand == _commandWin);
				}
				int timeGame2 = int.Parse(PhotonNetwork.room.customProperties[ConnectSceneNGUIController.maxKillProperty].ToString());
				AdminSettingsController.Avard avardAfterMatch = AdminSettingsController.GetAvardAfterMatch(ConnectSceneNGUIController.regim, timeGame2, num6, this.score, this.CountKills, this.isIwin);
				this.addCoins = avardAfterMatch.coin;
				this.addExperience = avardAfterMatch.expierense;
			}
			if (this.isMine)
			{
				double num8 = totalMinutes;
				string reasonToDismissInterstitialAfterMatch = AfterMatchInterstitialRunner.GetReasonToDismissInterstitialAfterMatch(flag2, num8);
				if (string.IsNullOrEmpty(reasonToDismissInterstitialAfterMatch))
				{
					if (Application.isEditor)
					{
						UnityEngine.Debug.LogFormat("<color=magenta>{0}.win(), winner: {1}, matchDuration: {2:f2}</color>", new object[]
						{
							base.GetType().Name,
							flag2,
							num8
						});
					}
					AfterMatchInterstitialRunner afterMatchInterstitialRunner = new AfterMatchInterstitialRunner();
					afterMatchInterstitialRunner.Run();
				}
				else
				{
					string format = (!Application.isEditor) ? "Dismissing interstitial. {0}" : "<color=magenta>Dismissing interstitial. {0}</color>";
					UnityEngine.Debug.LogFormat(format, new object[]
					{
						reasonToDismissInterstitialAfterMatch
					});
				}
			}
			if (WeaponManager.sharedManager != null)
			{
				WeaponManager.sharedManager.DecreaseTryGunsMatchesCount();
			}
			if (flag2)
			{
				if (!NetworkStartTable.LocalOrPasswordRoom())
				{
					QuestMediator.NotifyWin(ConnectSceneNGUIController.regim, Application.loadedLevelName);
				}
				if (Defs.isFlag)
				{
					int val = Storager.getInt(Defs.RatingFlag, false) + 1;
					Storager.setInt(Defs.RatingFlag, val, false);
				}
				if (Defs.isCompany)
				{
					int val2 = Storager.getInt(Defs.RatingTeamBattle, false) + 1;
					Storager.setInt(Defs.RatingTeamBattle, val2, false);
				}
				if (Defs.isCapturePoints)
				{
					int val3 = Storager.getInt(Defs.RatingCapturePoint, false) + 1;
					Storager.setInt(Defs.RatingCapturePoint, val3, false);
				}
				if (Defs.isDuel)
				{
					int val4 = Storager.getInt(Defs.RatingDuel, false) + 1;
					Storager.setInt(Defs.RatingDuel, val4, false);
				}
				if (ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.Deathmatch)
				{
					int val5 = Storager.getInt(Defs.RatingDeathmatch, false) + 1;
					Storager.setInt(Defs.RatingDeathmatch, val5, false);
				}
				if (ExperienceController.sharedController != null)
				{
					string key2 = "Statistics.WinCount.Level" + ExperienceController.sharedController.currentLevel;
					int int3 = PlayerPrefs.GetInt(key2, 0);
					PlayerPrefs.SetInt(key2, int3 + 1);
				}
				if (!Defs.isCOOP)
				{
					FriendsController.sharedController.SendRoundWon();
					if (PlayerPrefs.GetInt("LogCountMatch", 0) == 1)
					{
						PlayerPrefs.SetInt("LogCountMatch", 0);
						if (Social.localUser.authenticated)
						{
							Social.ReportProgress("CgkIr8rGkPIJEAIQAg", 100.0, delegate(bool success)
							{
								UnityEngine.Debug.Log("Achievement First Win completed: " + success);
							});
						}
					}
				}
			}
			if (this.addCoins > 0 || (ExperienceController.sharedController.currentLevel < 31 && this.addExperience > 0))
			{
				this.isShowAvard = true;
				if (PhotonNetwork.room != null && !PhotonNetwork.room.customProperties[ConnectSceneNGUIController.passwordProperty].Equals(string.Empty))
				{
					this.addCoins = 0;
					this.addExperience = 0;
					this.isShowAvard = false;
				}
			}
		}
		bool flag3 = false;
		int num9 = 0;
		NetworkStartTable networkStartTable = null;
		for (int n = 0; n < list3.Count; n++)
		{
			if (this._weaponManager && list3[n].Equals(this._weaponManager.myTable))
			{
				this.oldIndexMy = n;
			}
			NetworkStartTable component3 = list3[n].GetComponent<NetworkStartTable>();
			this.oldSpisokName[n] = component3.NamePlayer;
			this.oldSpisokRanks[n] = component3.myRanks;
			this.oldSpisokPixelBookID[n] = component3.pixelBookID;
			this.oldSpisokMyClanLogo[n] = component3.myClanTexture;
			this.oldScoreSpisok[n] = ((component3.score == -1) ? component3.scoreOld.ToString() : component3.score.ToString());
			int num10 = (component3.CountKills == -1) ? component3.oldCountKills : component3.CountKills;
			this.oldCountLilsSpisok[n] = num10.ToString();
			this.oldIsDeadInHungerGame[n] = component3.isDeadInHungerGame;
			if (Defs.isDaterRegim)
			{
				if (num10 > num9)
				{
					networkStartTable = component3;
					flag3 = false;
					num9 = num10;
				}
				else if (num10 > 0 && num10 == num9)
				{
					flag3 = true;
				}
			}
		}
		for (int num11 = 0; num11 < list.Count; num11++)
		{
			if (this._weaponManager && list[num11].Equals(this._weaponManager.myTable))
			{
				this.oldIndexMy = num11;
			}
			this.oldSpisokNameBlue[num11] = list[num11].GetComponent<NetworkStartTable>().NamePlayer;
			this.oldSpisokRanksBlue[num11] = list[num11].GetComponent<NetworkStartTable>().myRanks;
			this.oldSpisokPixelBookIDBlue[num11] = list[num11].GetComponent<NetworkStartTable>().pixelBookID;
			this.oldSpisokMyClanLogoBlue[num11] = list[num11].GetComponent<NetworkStartTable>().myClanTexture;
			this.oldScoreSpisokBlue[num11] = ((list[num11].GetComponent<NetworkStartTable>().score == -1) ? (string.Empty + list[num11].GetComponent<NetworkStartTable>().scoreOld) : (string.Empty + list[num11].GetComponent<NetworkStartTable>().score));
			this.oldCountLilsSpisokBlue[num11] = ((list[num11].GetComponent<NetworkStartTable>().CountKills == -1) ? (string.Empty + list[num11].GetComponent<NetworkStartTable>().oldCountKills) : (string.Empty + list[num11].GetComponent<NetworkStartTable>().CountKills));
		}
		for (int num12 = 0; num12 < list2.Count; num12++)
		{
			if (this._weaponManager && list2[num12].Equals(this._weaponManager.myTable))
			{
				this.oldIndexMy = num12;
			}
			this.oldSpisokNameRed[num12] = list2[num12].GetComponent<NetworkStartTable>().NamePlayer;
			this.oldSpisokRanksRed[num12] = list2[num12].GetComponent<NetworkStartTable>().myRanks;
			this.oldSpisokPixelBookIDRed[num12] = list2[num12].GetComponent<NetworkStartTable>().pixelBookID;
			this.oldSpisokMyClanLogoRed[num12] = list2[num12].GetComponent<NetworkStartTable>().myClanTexture;
			this.oldScoreSpisokRed[num12] = ((list2[num12].GetComponent<NetworkStartTable>().score == -1) ? (string.Empty + list2[num12].GetComponent<NetworkStartTable>().scoreOld) : (string.Empty + list2[num12].GetComponent<NetworkStartTable>().score));
			this.oldCountLilsSpisokRed[num12] = ((list2[num12].GetComponent<NetworkStartTable>().CountKills == -1) ? (string.Empty + list2[num12].GetComponent<NetworkStartTable>().oldCountKills) : (string.Empty + list2[num12].GetComponent<NetworkStartTable>().CountKills));
		}
		this.myCommandOld = this.myCommand;
		this.oldCountKills = this.CountKills;
		this.scoreOld = this.score;
		this.score = -1;
		GlobalGameController.Score = -1;
		this.scoreCommandFlag1 = 0;
		this.scoreCommandFlag2 = 0;
		this.CountKills = -1;
		if (this.isCompany || Defs.isFlag || Defs.isCapturePoints)
		{
			this.myCommand = -1;
		}
		this.SynhCommand(null);
		this.SynhCountKills(null);
		this.SynhScore();
		if (WeaponManager.sharedManager.myPlayerMoveC != null && WeaponManager.sharedManager.myPlayerMoveC.showRanks)
		{
			NetworkStartTableNGUIController.sharedController.BackPressFromRanksTable(true);
		}
		GameObject gameObject2 = GameObject.FindGameObjectWithTag("DamageFrame");
		if (gameObject2 != null)
		{
			UnityEngine.Object.Destroy(gameObject2);
		}
		int winnerCommand = 0;
		string winner2;
		if (Defs.isDaterRegim)
		{
			if (networkStartTable != null)
			{
				if (!flag3)
				{
					if (networkStartTable.Equals(this))
					{
						winner2 = LocalizationStore.Get("Key_1762");
					}
					else
					{
						winner2 = string.Format(LocalizationStore.Get("Key_1763"), networkStartTable.NamePlayer);
					}
				}
				else
				{
					winner2 = LocalizationStore.Get("Key_1764");
				}
			}
			else
			{
				winner2 = LocalizationStore.Get("Key_1427");
			}
		}
		else if (this.isCompany || Defs.isFlag || Defs.isCapturePoints)
		{
			string key_ = LocalizationStore.Key_0571;
			winner2 = ((this.commandWinner != 0) ? ((this.commandWinner != this.myCommandOld) ? LocalizationStore.Get("Key_1794") : LocalizationStore.Get("Key_1793")) : key_);
			winnerCommand = ((this.commandWinner != 0) ? ((this.commandWinner != this.myCommandOld) ? 2 : 1) : 0);
		}
		else if ((this.isHunger && this.isDrawInHanger) || this.isDrawInDeathMatch)
		{
			winner2 = LocalizationStore.Key_0568;
		}
		else
		{
			bool flag4 = flag2;
			if (flag4)
			{
				winner2 = LocalizationStore.Get("Key_1115");
			}
			else
			{
				winner2 = LocalizationStore.Get("Key_1116");
			}
		}
		this.isShowFinished = true;
		if (NetworkStartTableNGUIController.sharedController != null)
		{
			if (!Defs.isDaterRegim && Defs.isInet && !this.isSetNewMapButton)
			{
				NetworkStartTableNGUIController.sharedController.UpdateGoMapButtons(true);
			}
			if (Defs.isDuel)
			{
				NetworkStartTableNGUIController.sharedController.StartCoroutine(NetworkStartTableNGUIController.sharedController.MatchFinishedInDuelInterface(ratingChange, this.isShowAvard, this.addCoins, this.addExperience, NetworkStartTable.LocalOrPasswordRoom(), num6 == 0, flag));
			}
			else if (!Defs.isHunger || !this.isDeadInHungerGame)
			{
				NetworkStartTableNGUIController.sharedController.StartCoroutine(NetworkStartTableNGUIController.sharedController.MatchFinishedInterface(winner2, ratingChange, this.isShowAvard, this.addCoins, this.addExperience, NetworkStartTable.LocalOrPasswordRoom(), (!Defs.isHunger) ? (num6 == 0) : this.isIwin, iAmWinnerInTeam, winnerCommand, blueCount, redCount, false));
			}
			else
			{
				NetworkStartTableNGUIController.sharedController.MathFinishedDeadInHunger();
			}
		}
		this.isShowAvard = false;
		this.showTable = false;
		this.isShowNickTable = true;
	}

	// Token: 0x060022B2 RID: 8882 RVA: 0x000AA630 File Offset: 0x000A8830
	public int GetPlaceInTable()
	{
		int result = 0;
		this._tabsBuffer.Clear();
		int count = Initializer.networkTables.Count;
		for (int num = 0; num != count; num++)
		{
			NetworkStartTable item = Initializer.networkTables[num];
			this._tabsBuffer.Add(item);
		}
		List<NetworkStartTable> tabsBuffer = this._tabsBuffer;
		int count2 = tabsBuffer.Count;
		for (int i = 1; i < count2; i++)
		{
			NetworkStartTable networkStartTable = tabsBuffer[i];
			for (int j = 0; j < i; j++)
			{
				NetworkStartTable networkStartTable2 = tabsBuffer[j];
				if ((!Defs.isDuel && !Defs.isFlag && !Defs.isCapturePoints && (networkStartTable.score > networkStartTable2.score || (networkStartTable.score == networkStartTable2.score && networkStartTable.CountKills > networkStartTable2.CountKills))) || ((Defs.isDuel || Defs.isFlag || Defs.isCapturePoints) && (networkStartTable.CountKills > networkStartTable2.CountKills || (networkStartTable.CountKills == networkStartTable2.CountKills && networkStartTable.score > networkStartTable2.score))))
				{
					NetworkStartTable value = tabsBuffer[i];
					for (int k = i - 1; k >= j; k--)
					{
						tabsBuffer[k + 1] = tabsBuffer[k];
					}
					tabsBuffer[j] = value;
					break;
				}
			}
		}
		int num2 = 0;
		int num3 = 0;
		int num4 = 0;
		for (int l = 0; l < count2; l++)
		{
			if (tabsBuffer[l].myCommand == 0)
			{
				if (tabsBuffer[l] == WeaponManager.sharedManager.myNetworkStartTable)
				{
					result = num2;
				}
				num2++;
			}
			if (tabsBuffer[l].myCommand == 1)
			{
				if (tabsBuffer[l] == WeaponManager.sharedManager.myNetworkStartTable)
				{
					result = num3;
				}
				num3++;
			}
			if (tabsBuffer[l].myCommand == 2)
			{
				if (tabsBuffer[l] == WeaponManager.sharedManager.myNetworkStartTable)
				{
					result = num4;
				}
				num4++;
			}
		}
		this._tabsBuffer.Clear();
		return result;
	}

	// Token: 0x060022B3 RID: 8883 RVA: 0x000AA8A0 File Offset: 0x000A8AA0
	public int GetWinningTeam()
	{
		int result = 0;
		if (Defs.isFlag)
		{
			if (WeaponManager.sharedManager.myNetworkStartTable != null)
			{
				if (WeaponManager.sharedManager.myNetworkStartTable.scoreCommandFlag1 > WeaponManager.sharedManager.myNetworkStartTable.scoreCommandFlag2)
				{
					result = 1;
				}
				else if (WeaponManager.sharedManager.myNetworkStartTable.scoreCommandFlag2 > WeaponManager.sharedManager.myNetworkStartTable.scoreCommandFlag1)
				{
					result = 2;
				}
			}
		}
		else if (ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.CapturePoints)
		{
			if (CapturePointController.sharedController.scoreBlue > CapturePointController.sharedController.scoreRed)
			{
				result = 1;
			}
			else if (CapturePointController.sharedController.scoreRed > CapturePointController.sharedController.scoreBlue)
			{
				result = 2;
			}
		}
		else if (this.myPlayerMoveC != null)
		{
			if (WeaponManager.sharedManager.myPlayerMoveC.countKillsCommandBlue > WeaponManager.sharedManager.myPlayerMoveC.countKillsCommandRed)
			{
				result = 1;
			}
			else if (WeaponManager.sharedManager.myPlayerMoveC.countKillsCommandRed > WeaponManager.sharedManager.myPlayerMoveC.countKillsCommandBlue)
			{
				result = 2;
			}
		}
		else if (GlobalGameController.countKillsBlue > GlobalGameController.countKillsRed)
		{
			result = 1;
		}
		else if (GlobalGameController.countKillsRed > GlobalGameController.countKillsBlue)
		{
			result = 2;
		}
		return result;
	}

	// Token: 0x060022B4 RID: 8884 RVA: 0x000AA9FC File Offset: 0x000A8BFC
	public void CalculateMatchRatingOnDisconnect()
	{
		if (this.myPlayerMoveC != null && ((!Defs.isCOOP && !Defs.isCompany && !Defs.isFlag && !Defs.isCapturePoints) || this.myPlayerMoveC.liveTime > 90f))
		{
			this.CalculateMatchRating(true);
		}
	}

	// Token: 0x060022B5 RID: 8885 RVA: 0x000AAA60 File Offset: 0x000A8C60
	public void IncrementKills()
	{
		this.killCountMatch++;
	}

	// Token: 0x060022B6 RID: 8886 RVA: 0x000AAA70 File Offset: 0x000A8C70
	public void IncrementDeath()
	{
		this.deathCountMatch++;
	}

	// Token: 0x060022B7 RID: 8887 RVA: 0x000AAA80 File Offset: 0x000A8C80
	public float GetMatchKillrate()
	{
		if (Defs.isCOOP)
		{
			return 1f;
		}
		if (this.deathCountMatch != 0)
		{
			return (float)this.killCountMatch / (float)this.deathCountMatch;
		}
		return (float)this.killCountMatch;
	}

	// Token: 0x060022B8 RID: 8888 RVA: 0x000AAAC0 File Offset: 0x000A8CC0
	public void ClearKillrate()
	{
		this.killCountMatch = 0;
		this.deathCountMatch = 0;
	}

	// Token: 0x060022B9 RID: 8889 RVA: 0x000AAAD0 File Offset: 0x000A8CD0
	public bool IsRatingMatch()
	{
		return !NetworkStartTable.LocalOrPasswordRoom() && RatingSystem.instance.ratingMatch && !(this.myPlayerMoveC == null) && !Defs.isDaterRegim && (!Defs.isHunger || !this.isDeadInHungerGame);
	}

	// Token: 0x060022BA RID: 8890 RVA: 0x000AAB30 File Offset: 0x000A8D30
	public bool CheckNeedRatingChange(bool ratingWinner, bool ratingDeadHeat)
	{
		return (!Defs.isDuel || (!ratingDeadHeat && (ratingWinner || DuelController.instance.playingTime >= 60f))) && (!Defs.isHunger || this.CountKills > 0 || !ratingWinner) && (Defs.isHunger || Defs.isDuel || ratingWinner || this.myPlayerMoveC.liveTime >= 90f);
	}

	// Token: 0x060022BB RID: 8891 RVA: 0x000AABBC File Offset: 0x000A8DBC
	public RatingSystem.MatchStat GetCurrentRatingMatchStat()
	{
		int placeInTable = this.GetPlaceInTable();
		int count = Initializer.networkTables.Count;
		int winningTeam = this.GetWinningTeam();
		bool flag = this.CheckForWin(placeInTable, winningTeam, this.CountKills, this.score, false);
		int num4;
		int place;
		bool deadHeat;
		bool winner;
		if (this.isCompany || Defs.isFlag || Defs.isCapturePoints)
		{
			int num = 0;
			int num2 = 0;
			for (int i = 0; i < Initializer.networkTables.Count; i++)
			{
				if (Initializer.networkTables[i].myCommand == 1)
				{
					num2++;
				}
				else
				{
					num++;
				}
			}
			int num3 = (num <= num2) ? num2 : num;
			num4 = num3 * 2;
			place = placeInTable + (flag ? 0 : num3);
			deadHeat = (winningTeam == 0);
			winner = flag;
		}
		else if (Defs.isHunger)
		{
			int num5 = (!flag) ? (Initializer.players.Count - 1) : 0;
			num4 = this.playerCountInHunger;
			place = Mathf.Clamp(placeInTable, 0, this.playerCountInHunger - 1);
			deadHeat = false;
			winner = (placeInTable < Mathf.CeilToInt((float)(num4 / 2)));
		}
		else
		{
			num4 = count;
			place = placeInTable;
			deadHeat = (Defs.isDuel && this.CheckForDeadheatInDuel());
			winner = (placeInTable < Mathf.CeilToInt((float)(num4 / 2)));
		}
		return new RatingSystem.MatchStat(place, num4, winner, deadHeat);
	}

	// Token: 0x060022BC RID: 8892 RVA: 0x000AAD38 File Offset: 0x000A8F38
	public int GetCurrentRatingChange(bool onExit)
	{
		if (!this.IsRatingMatch())
		{
			return 0;
		}
		if (!onExit && !Defs.isHunger && this.score <= 0 && !this.myPlayerMoveC.killedInMatch)
		{
			return 0;
		}
		RatingSystem.MatchStat matchStat = (!onExit) ? this.GetCurrentRatingMatchStat() : RatingSystem.MatchStat.LooseStat;
		if (!onExit && !this.CheckNeedRatingChange(matchStat.winner, matchStat.deadHeat))
		{
			return 0;
		}
		return RatingSystem.instance.GetRatingValueForParams(matchStat.playerCount, matchStat.place, this.GetMatchKillrate(), matchStat.deadHeat);
	}

	// Token: 0x060022BD RID: 8893 RVA: 0x000AADE0 File Offset: 0x000A8FE0
	public RatingSystem.RatingChange CalculateMatchRating(bool disconnecting)
	{
		RatingSystem.RatingChange result = RatingSystem.instance.currentRatingChange;
		if (!this.IsRatingMatch())
		{
			return result;
		}
		if (!this.exitFromMenu && !Defs.isHunger && this.score <= 0 && !this.myPlayerMoveC.killedInMatch)
		{
			return result;
		}
		RatingSystem.MatchStat matchStat = (!this.exitFromMenu) ? this.GetCurrentRatingMatchStat() : RatingSystem.MatchStat.LooseStat;
		if (disconnecting && matchStat.winner)
		{
			return result;
		}
		if (!this.exitFromMenu && !this.CheckNeedRatingChange(matchStat.winner, matchStat.deadHeat))
		{
			return result;
		}
		result = RatingSystem.instance.CalculateRating(matchStat.playerCount, matchStat.place, this.GetMatchKillrate(), matchStat.deadHeat);
		if (Defs.isDuel && disconnecting && result.addRating < 0)
		{
			PlayerPrefs.SetInt("leave_from_duel_penalty", result.addRating);
			PlayerPrefs.Save();
		}
		return result;
	}

	// Token: 0x060022BE RID: 8894 RVA: 0x000AAEE8 File Offset: 0x000A90E8
	public RatingSystem.RatingChange CalculateMatchRatingOld(bool disconnecting)
	{
		RatingSystem.RatingChange currentRatingChange = RatingSystem.instance.currentRatingChange;
		if (NetworkStartTable.LocalOrPasswordRoom() || !RatingSystem.instance.ratingMatch)
		{
			return currentRatingChange;
		}
		if (Defs.isHunger && this.isDeadInHungerGame)
		{
			return currentRatingChange;
		}
		int num = this.GetPlaceInTable();
		int winningTeam = this.GetWinningTeam();
		bool flag = this.CheckForWin(num, winningTeam, this.CountKills, this.score, false);
		if (this.myPlayerMoveC != null && (Defs.isHunger || this.score > 0 || this.myPlayerMoveC.killedInMatch))
		{
			List<int> list = new List<int>();
			if (this.isCompany || Defs.isFlag || Defs.isCapturePoints)
			{
				for (int i = 0; i < Initializer.networkTables.Count; i++)
				{
					if (Initializer.networkTables[i] != this)
					{
						list.Add((Initializer.networkTables[i].gameRating == -1) ? this.gameRating : Initializer.networkTables[i].gameRating);
					}
				}
				if (list.Count == 0)
				{
					list.Add(this.gameRating);
				}
			}
			else if (!Defs.isHunger)
			{
				for (int j = 0; j < Initializer.networkTables.Count; j++)
				{
					if (Initializer.networkTables[j] != this)
					{
						list.Add((Initializer.networkTables[j].gameRating == -1) ? this.gameRating : Initializer.networkTables[j].gameRating);
					}
				}
				if (list.Count == 0)
				{
					return currentRatingChange;
				}
				float num2 = (float)Initializer.networkTables.Count / 2f;
				flag = ((float)(num + 1) <= num2);
				if (!flag)
				{
					num -= Mathf.FloorToInt(num2);
				}
			}
			UnityEngine.Debug.Log(string.Format("<color=orange>My place: {0}, team winner: {1}, rating winner - {2}</color>", num.ToString(), winningTeam.ToString(), flag.ToString()));
			if (!flag && !Defs.isHunger && this.myPlayerMoveC.liveTime < 60f)
			{
				return currentRatingChange;
			}
			if (disconnecting && flag)
			{
				return currentRatingChange;
			}
		}
		return currentRatingChange;
	}

	// Token: 0x060022BF RID: 8895 RVA: 0x000AB154 File Offset: 0x000A9354
	public void DestroyPlayer()
	{
		this.isShowFinished = false;
		NickLabelController.currentCamera = Initializer.Instance.tc.GetComponent<Camera>();
		if (this._cam != null)
		{
			this._cam.SetActive(true);
			this._cam.GetComponent<RPG_Camera>().enabled = false;
		}
		if (!this.isInet)
		{
			this.DestroyMyPlayer();
		}
		else if (this._weaponManager && this._weaponManager.myPlayer)
		{
			PhotonNetwork.Destroy(this._weaponManager.myPlayer);
		}
	}

	// Token: 0x060022C0 RID: 8896 RVA: 0x000AB1F8 File Offset: 0x000A93F8
	[Obfuscation(Exclude = true)]
	private void DestroyMyPlayer()
	{
		if (WeaponManager.sharedManager.myPlayer != null)
		{
			Network.RemoveRPCs(this._weaponManager.myPlayer.GetComponent<NetworkView>().viewID);
			Network.Destroy(this._weaponManager.myPlayer);
		}
	}

	// Token: 0x060022C1 RID: 8897 RVA: 0x000AB244 File Offset: 0x000A9444
	private void finishTable()
	{
		this.playersTable();
	}

	// Token: 0x060022C2 RID: 8898 RVA: 0x000AB24C File Offset: 0x000A944C
	public void MyOnGUI()
	{
		if (this.experienceController.isShowAdd)
		{
			GUI.enabled = false;
		}
		if (this.showDisconnectFromServer)
		{
			GUI.DrawTexture(new Rect((float)(Screen.width / 2) - (float)this.serverLeftTheGame.width * 0.5f * this.koofScreen, (float)(Screen.height / 2) - (float)this.serverLeftTheGame.height * 0.5f * this.koofScreen, (float)this.serverLeftTheGame.width * this.koofScreen, (float)this.serverLeftTheGame.height * this.koofScreen), this.serverLeftTheGame);
			GUI.enabled = false;
		}
		if (this.showDisconnectFromMasterServer)
		{
			GUI.DrawTexture(new Rect((float)(Screen.width / 2) - (float)this.serverLeftTheGame.width * 0.5f * this.koofScreen, (float)(Screen.height / 2) - (float)this.serverLeftTheGame.height * 0.5f * this.koofScreen, (float)this.serverLeftTheGame.width * this.koofScreen, (float)this.serverLeftTheGame.height * this.koofScreen), this.serverLeftTheGame);
		}
		if (this.showTable)
		{
			this.playersTable();
		}
		if (this.isShowNickTable)
		{
			this.finishTable();
		}
		if (this.showMessagFacebook)
		{
			this.labelStyle.fontSize = Player_move_c.FontSizeForMessages;
			GUI.Label(Tools.SuccessMessageRect(), this._SocialSentSuccess("Facebook"), this.labelStyle);
		}
		GUI.enabled = true;
	}

	// Token: 0x060022C3 RID: 8899 RVA: 0x000AB3E0 File Offset: 0x000A95E0
	[Obfuscation(Exclude = true)]
	public void ClearScoreCommandInFlagGame()
	{
		this.photonView.RPC("ClearScoreCommandInFlagGameRPC", PhotonTargets.Others, new object[0]);
	}

	// Token: 0x060022C4 RID: 8900 RVA: 0x000AB3FC File Offset: 0x000A95FC
	[RPC]
	[PunRPC]
	public void ClearScoreCommandInFlagGameRPC()
	{
		if (WeaponManager.sharedManager.myTable != null)
		{
			WeaponManager.sharedManager.myTable.GetComponent<NetworkStartTable>().scoreCommandFlag1 = 0;
			WeaponManager.sharedManager.myTable.GetComponent<NetworkStartTable>().scoreCommandFlag2 = 0;
		}
	}

	// Token: 0x060022C5 RID: 8901 RVA: 0x000AB448 File Offset: 0x000A9648
	private void AddFlag()
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("BazaZoneCommand1");
		GameObject gameObject2 = GameObject.FindGameObjectWithTag("BazaZoneCommand2");
		PhotonNetwork.InstantiateSceneObject("Flags/Flag1", gameObject.transform.position, gameObject.transform.rotation, 0, null);
		PhotonNetwork.InstantiateSceneObject("Flags/Flag2", gameObject2.transform.position, gameObject2.transform.rotation, 0, null);
	}

	// Token: 0x060022C6 RID: 8902 RVA: 0x000AB4B4 File Offset: 0x000A96B4
	[RPC]
	[PunRPC]
	private void AddPaticleBazeRPC(int _command)
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("BazaZoneCommand" + _command);
		UnityEngine.Object.Instantiate(Resources.Load((_command != WeaponManager.sharedManager.myNetworkStartTable.myCommand) ? "Ring_Particle_Red" : "Ring_Particle_Blue"), new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 0.22f, gameObject.transform.position.z), gameObject.transform.rotation);
	}

	// Token: 0x060022C7 RID: 8903 RVA: 0x000AB558 File Offset: 0x000A9758
	public void AddScore()
	{
		this.CountKills++;
		GlobalGameController.CountKills = this.CountKills;
		this.photonView.RPC("AddPaticleBazeRPC", PhotonTargets.All, new object[]
		{
			this.myCommand
		});
		if (this.myCommand == 1)
		{
			this.photonView.RPC("SynchScoreCommandRPC", PhotonTargets.All, new object[]
			{
				1,
				this.scoreCommandFlag1 + 1
			});
		}
		else
		{
			this.photonView.RPC("SynchScoreCommandRPC", PhotonTargets.All, new object[]
			{
				2,
				this.scoreCommandFlag2 + 1
			});
		}
		this.SynhCountKills(null);
	}

	// Token: 0x060022C8 RID: 8904 RVA: 0x000AB61C File Offset: 0x000A981C
	[PunRPC]
	[RPC]
	private void SynchScoreCommandRPC(int _command, int _score)
	{
		GameObject[] array = GameObject.FindGameObjectsWithTag("NetworkTable");
		for (int i = 0; i < array.Length; i++)
		{
			if (_command == 1)
			{
				array[i].GetComponent<NetworkStartTable>().scoreCommandFlag1 = _score;
			}
			else
			{
				array[i].GetComponent<NetworkStartTable>().scoreCommandFlag2 = _score;
			}
		}
	}

	// Token: 0x060022C9 RID: 8905 RVA: 0x000AB670 File Offset: 0x000A9870
	private void OnDestroy()
	{
		if (this.isMine)
		{
			ShopNGUIController.sharedShop.onEquipSkinAction = null;
			this.RemoveShop(false);
			if (this.networkStartTableNGUIController != null && !this.networkStartTableNGUIController.isRewardShow)
			{
				UnityEngine.Object.Destroy(this.networkStartTableNGUIController.gameObject);
			}
			if (ShopNGUIController.sharedShop != null)
			{
				ShopNGUIController.sharedShop.resumeAction = null;
			}
		}
		if (!this.isMine && WeaponManager.sharedManager.myPlayerMoveC != null)
		{
			WeaponManager.sharedManager.myPlayerMoveC.AddSystemMessage(this.NamePlayer + " " + LocalizationStore.Get("Key_0996"), new Color(1f, 0f, 0f));
		}
		if (Initializer.networkTables.Contains(this))
		{
			Initializer.networkTables.Remove(this);
		}
		PhotonObjectCacher.RemoveObject(base.gameObject);
	}

	// Token: 0x060022CA RID: 8906 RVA: 0x000AB76C File Offset: 0x000A996C
	[PunRPC]
	[RPC]
	private void SynchGameRating(int _rating)
	{
		this.gameRating = _rating;
	}

	// Token: 0x04001669 RID: 5737
	public static bool StartAfterDisconnect = false;

	// Token: 0x0400166A RID: 5738
	public string pixelBookID = "-1";

	// Token: 0x0400166B RID: 5739
	private SaltedInt _scoreCommandFlag1 = new SaltedInt(818919);

	// Token: 0x0400166C RID: 5740
	private SaltedInt _scoreCommandFlag2 = new SaltedInt(823016);

	// Token: 0x0400166D RID: 5741
	public double timerFlag;

	// Token: 0x0400166E RID: 5742
	private float maxTimerFlag = 150f;

	// Token: 0x0400166F RID: 5743
	private float timerUpdateTimerFlag;

	// Token: 0x04001670 RID: 5744
	private float maxTimerUpdateTimerFlag = 1f;

	// Token: 0x04001671 RID: 5745
	public bool isShowAvard;

	// Token: 0x04001672 RID: 5746
	public bool isShowFinished;

	// Token: 0x04001673 RID: 5747
	private bool isEndInHunger;

	// Token: 0x04001674 RID: 5748
	private int addExperience;

	// Token: 0x04001675 RID: 5749
	public GameObject guiObj;

	// Token: 0x04001676 RID: 5750
	public NetworkStartTableNGUIController networkStartTableNGUIController;

	// Token: 0x04001677 RID: 5751
	public bool isRegimVidos;

	// Token: 0x04001678 RID: 5752
	private int numberPlayerCun;

	// Token: 0x04001679 RID: 5753
	private int numberPlayerCunId;

	// Token: 0x0400167A RID: 5754
	public Player_move_c currentPlayerMoveCVidos;

	// Token: 0x0400167B RID: 5755
	private bool oldIsZomming;

	// Token: 0x0400167C RID: 5756
	private InGameGUI inGameGUI;

	// Token: 0x0400167D RID: 5757
	public string playerVidosNick;

	// Token: 0x0400167E RID: 5758
	public string playerVidosClanName;

	// Token: 0x0400167F RID: 5759
	public Texture playerVidosClanTexture;

	// Token: 0x04001680 RID: 5760
	public GameObject currentCamPlayer;

	// Token: 0x04001681 RID: 5761
	public GameObject currentFPSPlayer;

	// Token: 0x04001682 RID: 5762
	private GameObject currentBodyMech;

	// Token: 0x04001683 RID: 5763
	public GameObject currentGameObjectPlayer;

	// Token: 0x04001684 RID: 5764
	public bool isGoRandomRoom;

	// Token: 0x04001685 RID: 5765
	public Texture mySkin;

	// Token: 0x04001686 RID: 5766
	public List<GameObject> zombiePrefabs = new List<GameObject>();

	// Token: 0x04001687 RID: 5767
	private GameObject _playerPrefab;

	// Token: 0x04001688 RID: 5768
	public GameObject tempCam;

	// Token: 0x04001689 RID: 5769
	public GameObject zombieManagerPrefab;

	// Token: 0x0400168A RID: 5770
	public Texture2D serverLeftTheGame;

	// Token: 0x0400168B RID: 5771
	public ExperienceController experienceController;

	// Token: 0x0400168C RID: 5772
	private int addCoins;

	// Token: 0x0400168D RID: 5773
	public bool isDeadInHungerGame;

	// Token: 0x0400168E RID: 5774
	private bool showMessagFacebook;

	// Token: 0x0400168F RID: 5775
	private bool clickButtonFacebook;

	// Token: 0x04001690 RID: 5776
	public bool isIwin;

	// Token: 0x04001691 RID: 5777
	public int myCommand;

	// Token: 0x04001692 RID: 5778
	public int myCommandOld;

	// Token: 0x04001693 RID: 5779
	private bool isLocal;

	// Token: 0x04001694 RID: 5780
	private bool isMine;

	// Token: 0x04001695 RID: 5781
	private bool isCOOP;

	// Token: 0x04001696 RID: 5782
	private bool isServer;

	// Token: 0x04001697 RID: 5783
	private bool isCompany;

	// Token: 0x04001698 RID: 5784
	private bool isMulti;

	// Token: 0x04001699 RID: 5785
	private bool isInet;

	// Token: 0x0400169A RID: 5786
	private float timeNotRunZombiManager;

	// Token: 0x0400169B RID: 5787
	private bool isSendZaprosZombiManager;

	// Token: 0x0400169C RID: 5788
	private bool isGetZaprosZombiManager;

	// Token: 0x0400169D RID: 5789
	private ExperienceController expController;

	// Token: 0x0400169E RID: 5790
	public Texture myClanTexture;

	// Token: 0x0400169F RID: 5791
	public string myClanID = string.Empty;

	// Token: 0x040016A0 RID: 5792
	public string myClanName = string.Empty;

	// Token: 0x040016A1 RID: 5793
	public string myClanLeaderID = string.Empty;

	// Token: 0x040016A2 RID: 5794
	private LANBroadcastService lanScan;

	// Token: 0x040016A3 RID: 5795
	private bool isSetNewMapButton;

	// Token: 0x040016A4 RID: 5796
	public bool exitFromMenu;

	// Token: 0x040016A5 RID: 5797
	public bool isDrawInHanger;

	// Token: 0x040016A6 RID: 5798
	public List<NetworkStartTable.infoClient> players = new List<NetworkStartTable.infoClient>();

	// Token: 0x040016A7 RID: 5799
	public GUIStyle labelStyle;

	// Token: 0x040016A8 RID: 5800
	public GUIStyle messagesStyle;

	// Token: 0x040016A9 RID: 5801
	public GUIStyle ozidanieStyle;

	// Token: 0x040016AA RID: 5802
	private Vector2 scrollPosition = Vector2.zero;

	// Token: 0x040016AB RID: 5803
	private float koofScreen = (float)Screen.height / 768f;

	// Token: 0x040016AC RID: 5804
	public WeaponManager _weaponManager;

	// Token: 0x040016AD RID: 5805
	public bool _showTable;

	// Token: 0x040016AE RID: 5806
	public string nickPobeditelya;

	// Token: 0x040016AF RID: 5807
	public bool _isShowNickTable;

	// Token: 0x040016B0 RID: 5808
	public bool runGame = true;

	// Token: 0x040016B1 RID: 5809
	public GameObject[] zoneCreatePlayer;

	// Token: 0x040016B2 RID: 5810
	private GameObject _cam;

	// Token: 0x040016B3 RID: 5811
	public bool isDrawInDeathMatch;

	// Token: 0x040016B4 RID: 5812
	public bool showDisconnectFromServer;

	// Token: 0x040016B5 RID: 5813
	public bool showDisconnectFromMasterServer;

	// Token: 0x040016B6 RID: 5814
	private float timerShow = -1f;

	// Token: 0x040016B7 RID: 5815
	public string NamePlayer = "Player";

	// Token: 0x040016B8 RID: 5816
	public int CountKills;

	// Token: 0x040016B9 RID: 5817
	public int oldCountKills;

	// Token: 0x040016BA RID: 5818
	public string[] oldSpisokName;

	// Token: 0x040016BB RID: 5819
	public string[] oldCountLilsSpisok;

	// Token: 0x040016BC RID: 5820
	public string[] oldScoreSpisok;

	// Token: 0x040016BD RID: 5821
	public int[] oldSpisokRanks;

	// Token: 0x040016BE RID: 5822
	public string[] oldSpisokNameBlue;

	// Token: 0x040016BF RID: 5823
	public string[] oldCountLilsSpisokBlue;

	// Token: 0x040016C0 RID: 5824
	public int[] oldSpisokRanksBlue;

	// Token: 0x040016C1 RID: 5825
	public string[] oldSpisokNameRed;

	// Token: 0x040016C2 RID: 5826
	public string[] oldCountLilsSpisokRed;

	// Token: 0x040016C3 RID: 5827
	public string[] oldScoreSpisokRed;

	// Token: 0x040016C4 RID: 5828
	public string[] oldScoreSpisokBlue;

	// Token: 0x040016C5 RID: 5829
	public int[] oldSpisokRanksRed;

	// Token: 0x040016C6 RID: 5830
	public bool[] oldIsDeadInHungerGame;

	// Token: 0x040016C7 RID: 5831
	public string[] oldSpisokPixelBookID;

	// Token: 0x040016C8 RID: 5832
	public string[] oldSpisokPixelBookIDBlue;

	// Token: 0x040016C9 RID: 5833
	public string[] oldSpisokPixelBookIDRed;

	// Token: 0x040016CA RID: 5834
	public Texture[] oldSpisokMyClanLogo;

	// Token: 0x040016CB RID: 5835
	public Texture[] oldSpisokMyClanLogoBlue;

	// Token: 0x040016CC RID: 5836
	public Texture[] oldSpisokMyClanLogoRed;

	// Token: 0x040016CD RID: 5837
	public int oldIndexMy;

	// Token: 0x040016CE RID: 5838
	private GameObject tc;

	// Token: 0x040016CF RID: 5839
	public int scoreOld;

	// Token: 0x040016D0 RID: 5840
	public PhotonView photonView;

	// Token: 0x040016D1 RID: 5841
	private float timeTomig = 0.5f;

	// Token: 0x040016D2 RID: 5842
	private float timerSynchScore = -1f;

	// Token: 0x040016D3 RID: 5843
	private int countMigZagolovok;

	// Token: 0x040016D4 RID: 5844
	private int commandWinner;

	// Token: 0x040016D5 RID: 5845
	private bool isMigZag;

	// Token: 0x040016D6 RID: 5846
	private HungerGameController hungerGameController;

	// Token: 0x040016D7 RID: 5847
	private bool _canUserUseFacebookComposer;

	// Token: 0x040016D8 RID: 5848
	private bool _hasPublishPermission;

	// Token: 0x040016D9 RID: 5849
	private bool _hasPublishActions;

	// Token: 0x040016DA RID: 5850
	private SaltedInt _score = default(SaltedInt);

	// Token: 0x040016DB RID: 5851
	private static System.Random _prng = new System.Random(19937);

	// Token: 0x040016DC RID: 5852
	public int myRanks = 1;

	// Token: 0x040016DD RID: 5853
	public Player_move_c myPlayerMoveC;

	// Token: 0x040016DE RID: 5854
	private bool isHunger;

	// Token: 0x040016DF RID: 5855
	private int _gameRating = -1;

	// Token: 0x040016E0 RID: 5856
	private ShopNGUIController _shopInstance;

	// Token: 0x040016E1 RID: 5857
	private bool notSendAnaliticStartBattle;

	// Token: 0x040016E2 RID: 5858
	private int playerCountInHunger;

	// Token: 0x040016E3 RID: 5859
	private bool isStartPlayerCoroutine;

	// Token: 0x040016E4 RID: 5860
	private string waitingPlayerLocalize;

	// Token: 0x040016E5 RID: 5861
	private string matchLocalize;

	// Token: 0x040016E6 RID: 5862
	private string preparingLocalize;

	// Token: 0x040016E7 RID: 5863
	private Pauser _pauser;

	// Token: 0x040016E8 RID: 5864
	private Stopwatch _matchStopwatch = new Stopwatch();

	// Token: 0x040016E9 RID: 5865
	private readonly List<NetworkStartTable> _tabsBuffer = new List<NetworkStartTable>();

	// Token: 0x040016EA RID: 5866
	private int killCountMatch;

	// Token: 0x040016EB RID: 5867
	private int deathCountMatch;

	// Token: 0x020003C1 RID: 961
	public struct infoClient
	{
		// Token: 0x040016F1 RID: 5873
		public string ipAddress;

		// Token: 0x040016F2 RID: 5874
		public string name;

		// Token: 0x040016F3 RID: 5875
		public string coments;
	}
}
