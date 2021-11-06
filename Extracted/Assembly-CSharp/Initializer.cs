using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using Rilisoft;
using Rilisoft.NullExtensions;
using RilisoftBot;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x020002CE RID: 718
public sealed class Initializer : MonoBehaviour
{
	// Token: 0x1400001B RID: 27
	// (add) Token: 0x0600191B RID: 6427 RVA: 0x00061CEC File Offset: 0x0005FEEC
	// (remove) Token: 0x0600191C RID: 6428 RVA: 0x00061D04 File Offset: 0x0005FF04
	public static event Action PlayerAddedEvent;

	// Token: 0x170004A4 RID: 1188
	// (get) Token: 0x0600191D RID: 6429 RVA: 0x00061D1C File Offset: 0x0005FF1C
	// (set) Token: 0x0600191E RID: 6430 RVA: 0x00061D24 File Offset: 0x0005FF24
	public static Initializer Instance { get; private set; }

	// Token: 0x0600191F RID: 6431 RVA: 0x00061D2C File Offset: 0x0005FF2C
	public static bool IsEnemyTarget(Transform _target, Player_move_c forPlayer = null)
	{
		if (forPlayer == null)
		{
			forPlayer = WeaponManager.sharedManager.myPlayerMoveC;
		}
		if (forPlayer == null || _target == null || _target.Equals(forPlayer.myPlayerTransform))
		{
			return false;
		}
		IDamageable component = _target.GetComponent<IDamageable>();
		return component != null && component.IsEnemyTo(forPlayer);
	}

	// Token: 0x06001920 RID: 6432 RVA: 0x00061D98 File Offset: 0x0005FF98
	public static Player_move_c GetPlayerMoveCWithPhotonOwnerID(int id)
	{
		foreach (Player_move_c player_move_c in Initializer.players)
		{
			if (player_move_c.mySkinName.photonView != null && player_move_c.mySkinName.photonView.ownerId == id)
			{
				return player_move_c;
			}
		}
		return null;
	}

	// Token: 0x06001921 RID: 6433 RVA: 0x00061E2C File Offset: 0x0006002C
	public static Player_move_c GetPlayerMoveCWithLocalPlayerID(NetworkViewID id)
	{
		foreach (Player_move_c player_move_c in Initializer.players)
		{
			if (player_move_c.myPlayerIDLocal.Equals(id))
			{
				return player_move_c;
			}
		}
		return null;
	}

	// Token: 0x06001922 RID: 6434 RVA: 0x00061EAC File Offset: 0x000600AC
	private void Awake()
	{
		string callee = string.Format(CultureInfo.InvariantCulture, "{0}.Awake()", new object[]
		{
			base.GetType().Name
		});
		using (new ScopeLogger(callee, Defs.IsDeveloperBuild))
		{
			this.abTestConnect = (Defs.isActivABTestBuffSystem && ABTestController.useBuffSystem);
			Initializer.Instance = this;
			this.isMulti = Defs.isMulti;
			this.isInet = Defs.isInet;
			Initializer.lastGameMode = -1;
			if (Device.IsLoweMemoryDevice)
			{
				ActivityIndicator.ClearMemory();
			}
			if (!Defs.IsSurvival && (TrainingController.TrainingCompleted || TrainingController.CompletedTrainingStage > TrainingController.NewTrainingCompletedStage.ShootingRangeCompleted))
			{
				this.networkTablePref = Resources.Load("NetworkTable");
			}
			Defs.typeDisconnectGame = Defs.DisconectGameType.Reconnect;
			string name = SceneManager.GetActiveScene().name;
			GameObject gameObject;
			if (Defs.isMulti)
			{
				SceneInfo infoScene = SceneInfoController.instance.GetInfoScene(name);
				if (infoScene != null)
				{
					GlobalGameController.currentLevel = infoScene.indexMap;
				}
				gameObject = (Resources.Load("BackgroundMusic/BackgroundMusic_Level" + GlobalGameController.currentLevel) as GameObject);
			}
			else if (CurrentCampaignGame.currentLevel == 0)
			{
				string path = "BackgroundMusic/" + ((!Defs.IsSurvival) ? "Background_Training" : "BackgroundMusic_Level0");
				gameObject = (Resources.Load(path) as GameObject);
			}
			else
			{
				gameObject = (Resources.Load("BackgroundMusic/BackgroundMusic_Level" + CurrentCampaignGame.currentLevel) as GameObject);
			}
			if (gameObject)
			{
				UnityEngine.Object.Instantiate<GameObject>(gameObject);
			}
			if (!Defs.isMulti && !Defs.IsSurvival)
			{
				StoreKitEventListener.State.PurchaseKey = "In game";
				StoreKitEventListener.State.Parameters.Clear();
				StoreKitEventListener.State.Parameters.Add("Level", name + " In game");
				GameObject[] array = GameObject.FindGameObjectsWithTag("Configurator");
				if (array.Length > 0)
				{
					bool flag = !TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage == TrainingController.NewTrainingCompletedStage.None;
					for (int num = 0; num != array.Length; num++)
					{
						GameObject gameObject2 = array[num];
						CoinConfigurator component = gameObject2.GetComponent<CoinConfigurator>();
						if (!(component == null) && component.CoinIsPresent)
						{
							VirtualCurrencyBonusType bonusType = (component.BonusType != VirtualCurrencyBonusType.None) ? component.BonusType : VirtualCurrencyBonusType.Coin;
							List<string> levelsWhereGotBonus = CoinBonus.GetLevelsWhereGotBonus(bonusType);
							if (!levelsWhereGotBonus.Contains(name) || flag)
							{
								Vector3 position = (!(component.coinCreatePoint == null)) ? component.coinCreatePoint.position : component.pos;
								Initializer.CreateBonusAtPosition(position, bonusType);
							}
						}
					}
				}
				Initializer.lastGameMode = Initializer.GameModeCampaign;
			}
			else if (!Defs.isMulti && Defs.IsSurvival)
			{
				Initializer.lastGameMode = Initializer.GameModeSurvival;
			}
			else if (Defs.isMulti)
			{
				Initializer.lastGameMode = (int)ConnectSceneNGUIController.regim;
			}
			string abuseKey_d4d3cbab = Initializer.GetAbuseKey_d4d3cbab(3570650027U);
			if (Storager.hasKey(abuseKey_d4d3cbab))
			{
				string @string = Storager.getString(abuseKey_d4d3cbab, false);
				if (!string.IsNullOrEmpty(@string) && @string != "0")
				{
					long num2 = DateTime.UtcNow.Ticks >> 1;
					long num3 = num2;
					if (long.TryParse(@string, out num3))
					{
						num3 = Math.Min(num2, num3);
						Storager.setString(abuseKey_d4d3cbab, num3.ToString(), false);
					}
					else
					{
						Storager.setString(abuseKey_d4d3cbab, num2.ToString(), false);
					}
					TimeSpan timeSpan = TimeSpan.FromTicks(num2 - num3);
					bool flag2 = (!Defs.IsDeveloperBuild) ? (timeSpan.TotalDays >= 1.0) : (timeSpan.TotalMinutes >= 3.0);
					Player_move_c.NeedApply = flag2;
					Player_move_c.AnotherNeedApply = flag2;
				}
			}
			PhotonObjectCacher.AddObject(base.gameObject);
		}
	}

	// Token: 0x06001923 RID: 6435 RVA: 0x000622D8 File Offset: 0x000604D8
	private static string GetAbuseKey_d4d3cbab(uint pad)
	{
		return (272218770U ^ pad).ToString("x");
	}

	// Token: 0x06001924 RID: 6436 RVA: 0x000622FC File Offset: 0x000604FC
	internal static GameObject CreateBonusAtPosition(Vector3 position, VirtualCurrencyBonusType bonusType)
	{
		string text = string.Empty;
		if (bonusType != VirtualCurrencyBonusType.Coin)
		{
			if (bonusType != VirtualCurrencyBonusType.Gem)
			{
				UnityEngine.Debug.LogErrorFormat("Failed to determine resource for '{0}'", new object[]
				{
					bonusType
				});
				return null;
			}
			text = "gem";
		}
		else
		{
			text = "coin";
		}
		UnityEngine.Object @object = Resources.Load(text);
		if (@object == null)
		{
			UnityEngine.Debug.LogErrorFormat("Failed to load '{0}'", new object[]
			{
				text
			});
			return null;
		}
		UnityEngine.Object object2 = UnityEngine.Object.Instantiate(@object, position, Quaternion.Euler(270f, 0f, 0f));
		if (object2 == null)
		{
			UnityEngine.Debug.LogErrorFormat("Failed to instantiate '{0}'", new object[]
			{
				text
			});
			return null;
		}
		GameObject gameObject = object2 as GameObject;
		if (gameObject == null)
		{
			return gameObject;
		}
		CoinBonus component = gameObject.GetComponent<CoinBonus>();
		if (component == null)
		{
			UnityEngine.Debug.LogErrorFormat("Cannot find '{0}' script.", new object[]
			{
				typeof(CoinBonus).Name
			});
			return gameObject;
		}
		component.BonusType = bonusType;
		return gameObject;
	}

	// Token: 0x06001925 RID: 6437 RVA: 0x00062418 File Offset: 0x00060618
	private bool CheckRoom()
	{
		if (PhotonNetwork.room != null)
		{
			if (PhotonNetwork.room.maxPlayers < 2 || PhotonNetwork.room.maxPlayers > ((!Defs.isCOOP) ? ((!Defs.isDuel) ? 10 : 2) : 4))
			{
				this.goToConnect();
			}
			if (Defs.isDuel && (int)PhotonNetwork.room.customProperties[ConnectSceneNGUIController.roomStatusProperty] == 1)
			{
				Defs.typeDisconnectGame = Defs.DisconectGameType.Reconnect;
				this.isDisconnect = true;
				this.isLeavingRoom = true;
				PhotonNetwork.LeaveRoom();
				this.countConnectToRoom = 6;
				this.OnPhotonJoinRoomFailed();
				return false;
			}
		}
		return true;
	}

	// Token: 0x06001926 RID: 6438 RVA: 0x000624CC File Offset: 0x000606CC
	private void Start()
	{
		ConnectSceneNGUIController.isReturnFromGame = true;
		FriendsController.sharedController.profileInfo.Clear();
		FriendsController.sharedController.notShowAddIds.Clear();
		FacebookController.LogEvent("Campaign_ACHIEVED_LEVEL", null);
		Defs.inRespawnWindow = false;
		NetworkStartTable.StartAfterDisconnect = false;
		PhotonNetwork.isMessageQueueRunning = true;
		this._isMultiplayer = Defs.isMulti;
		this._weaponManager = WeaponManager.sharedManager;
		this._weaponManager.players.Clear();
		this.CheckRoom();
		if (PhotonNetwork.room != null)
		{
			SceneInfo infoScene = SceneInfoController.instance.GetInfoScene(int.Parse(PhotonNetwork.room.customProperties[ConnectSceneNGUIController.mapProperty].ToString()));
			this.goMapName = infoScene.NameScene;
		}
		if (!this._isMultiplayer)
		{
			this._initPlayerPositions.Add(new Vector3(12f, 1f, 9f));
			this._initPlayerPositions.Add(new Vector3(17f, 1f, -15f));
			this._initPlayerPositions.Add(new Vector3(-42f, 1f, -10.487f));
			this._initPlayerPositions.Add(new Vector3(0f, 1f, 19.5f));
			this._initPlayerPositions.Add(new Vector3(-33f, 1.2f, -13f));
			this._initPlayerPositions.Add(new Vector3(-2.67f, 1f, 2.67f));
			this._initPlayerPositions.Add(new Vector3(0f, 1f, 0f));
			this._initPlayerPositions.Add(new Vector3(19f, 1f, -0.8f));
			this._initPlayerPositions.Add(new Vector3(-28.5f, 1.75f, -3.73f));
			this._initPlayerPositions.Add(new Vector3(-2.5f, 1.75f, 0f));
			this._initPlayerPositions.Add(new Vector3(-1.596549f, 2.5f, 2.684792f));
			this._initPlayerPositions.Add(new Vector3(-6.611357f, 1.5f, -105.2573f));
			this._initPlayerPositions.Add(new Vector3(-20.3f, 2f, 17.6f));
			this._initPlayerPositions.Add(new Vector3(5f, 2.5f, 0f));
			this._initPlayerPositions.Add(new Vector3(0f, 2.5f, 0f));
			this._initPlayerPositions.Add(new Vector3(-7.3f, 3.6f, 6.46f));
			this._initPlayerPositions.Add(new Vector3(17f, 1f, -15f));
			this._initPlayerPositions.Add(new Vector3(17f, 1f, 0f));
			this._initPlayerPositions.Add(new Vector3(0.2f, 11.2f, -0.28f));
			this._initPlayerPositions.Add(new Vector3(-1.76f, 100.9f, 20.8f));
			this._initPlayerPositions.Add(new Vector3(20f, -0.4f, 17f));
			this._rots.Add(0f);
			this._rots.Add(0f);
			this._rots.Add(0f);
			this._rots.Add(180f);
			this._rots.Add(180f);
			this._rots.Add(0f);
			this._rots.Add(0f);
			this._rots.Add(270f);
			this._rots.Add(270f);
			this._rots.Add(270f);
			this._rots.Add(0f);
			this._rots.Add(0f);
			this._rots.Add(90f);
			this._rots.Add(0f);
			this._rots.Add(0f);
			this._rots.Add(90f);
			this._rots.Add(0f);
			this._rots.Add(0f);
			this._rots.Add(0f);
			this._rots.Add(0f);
			this._rots.Add(0f);
			int @int = Storager.getInt(Defs.EarnedCoins, false);
			if (@int > 0)
			{
				GameObject original = Resources.Load("MessageCoinsObject") as GameObject;
				UnityEngine.Object.Instantiate<GameObject>(original);
			}
			this.AddPlayer();
		}
		else
		{
			this.tc = (UnityEngine.Object.Instantiate(this.tempCam, Vector3.zero, Quaternion.identity) as GameObject);
			if (!Defs.isInet)
			{
				if (PlayerPrefs.GetString("TypeGame").Equals("client"))
				{
					bool useNat = !Network.HavePublicAddress();
					Network.useNat = useNat;
					UnityEngine.Debug.Log(Defs.ServerIp + " " + Network.Connect(Defs.ServerIp, 25002));
				}
				else
				{
					this._weaponManager.myTable = (GameObject)Network.Instantiate(this.networkTablePref, base.transform.position, base.transform.rotation, 0);
					this._weaponManager.myNetworkStartTable = this._weaponManager.myTable.GetComponent<NetworkStartTable>();
				}
			}
			else
			{
				this._weaponManager.myTable = PhotonNetwork.Instantiate("NetworkTable", base.transform.position, base.transform.rotation, 0);
				if (this._weaponManager.myTable != null)
				{
					this._weaponManager.myNetworkStartTable = this._weaponManager.myTable.GetComponent<NetworkStartTable>();
				}
				else
				{
					this.OnConnectionFail(DisconnectCause.DisconnectByClientTimeout);
				}
			}
		}
		this._gameSessionStopwatch.Start();
	}

	// Token: 0x06001927 RID: 6439 RVA: 0x00062AEC File Offset: 0x00060CEC
	[PunRPC]
	[RPC]
	private void SpawnOnNetwork(Vector3 pos, Quaternion rot, int id1, PhotonPlayer np)
	{
		if (this.networkTablePref != null)
		{
			Transform transform = UnityEngine.Object.Instantiate(this.networkTablePref, pos, rot) as Transform;
			PhotonView component = transform.GetComponent<PhotonView>();
			component.viewID = id1;
		}
	}

	// Token: 0x06001928 RID: 6440 RVA: 0x00062B2C File Offset: 0x00060D2C
	private void AddPlayer()
	{
		this._playerPrefab = Resources.Load<GameObject>("Player");
		GameObject gameObject = UnityEngine.Object.Instantiate(this._playerPrefab, Vector3.zero, Quaternion.identity) as GameObject;
		this.SetPlayerInStartPoint(gameObject);
		NickLabelController.currentCamera = gameObject.GetComponent<SkinName>().camPlayer.GetComponent<Camera>();
		base.Invoke("SetupObjectThatNeedsPlayer", 0.01f);
	}

	// Token: 0x06001929 RID: 6441 RVA: 0x00062B90 File Offset: 0x00060D90
	public void SetPlayerInStartPoint(GameObject player)
	{
		Vector3 position;
		float y;
		if (Defs.IsSurvival)
		{
			if (SceneLoader.ActiveSceneName.Equals("Arena_Underwater"))
			{
				position = new Vector3(0f, 3.5f, 0f);
				y = 0f;
			}
			else if (SceneLoader.ActiveSceneName.Equals("Pizza"))
			{
				position = new Vector3(-32.48f, 2.46f, 2.01f);
				y = 90f;
			}
			else
			{
				position = new Vector3(0f, 2.5f, 0f);
				y = 0f;
			}
		}
		else if (!TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage == TrainingController.NewTrainingCompletedStage.None)
		{
			TrainingController trainingController = UnityEngine.Object.FindObjectOfType<TrainingController>();
			position = ((!(trainingController != null)) ? TrainingController.PlayerDefaultPosition : trainingController.PlayerDesiredPosition);
			y = 0f;
		}
		else
		{
			int index = Mathf.Max(0, CurrentCampaignGame.currentLevel - 1);
			position = ((CurrentCampaignGame.currentLevel != 0) ? this._initPlayerPositions[index] : new Vector3(-0.72f, 1.75f, -13.23f));
			y = ((CurrentCampaignGame.currentLevel != 0) ? this._rots[index] : 0f);
			GameObject gameObject = GameObject.FindGameObjectWithTag("PlayerRespawnPoint");
			if (gameObject != null)
			{
				position = gameObject.transform.position;
				y = gameObject.transform.rotation.eulerAngles.y;
			}
		}
		player.transform.position = position;
		player.transform.rotation = Quaternion.Euler(0f, y, 0f);
	}

	// Token: 0x0600192A RID: 6442 RVA: 0x00062D40 File Offset: 0x00060F40
	[Obfuscation(Exclude = true)]
	public void SetupObjectThatNeedsPlayer()
	{
		if (Defs.isMulti)
		{
			if (Initializer.PlayerAddedEvent != null)
			{
				Initializer.PlayerAddedEvent();
			}
			return;
		}
		GameObject[] array = GameObject.FindGameObjectsWithTag("CoinBonus");
		for (int num = 0; num != array.Length; num++)
		{
			GameObject gameObject = array[num];
			CoinBonus component = gameObject.GetComponent<CoinBonus>();
			if (component != null)
			{
				component.SetPlayer();
			}
		}
		if (TrainingController.TrainingCompleted)
		{
			ZombieCreator.sharedCreator.BeganCreateEnemies();
		}
		base.GetComponent<BonusCreator>().BeginCreateBonuses();
		if (Initializer.PlayerAddedEvent != null)
		{
			Initializer.PlayerAddedEvent();
		}
	}

	// Token: 0x0600192B RID: 6443 RVA: 0x00062DDC File Offset: 0x00060FDC
	private void ShowDescriptionLabel(string text)
	{
		this.descriptionLabel.gameObject.SetActive(true);
		this.descriptionLabel.text = text;
	}

	// Token: 0x0600192C RID: 6444 RVA: 0x00062DFC File Offset: 0x00060FFC
	public void HideReconnectInterface()
	{
		this.descriptionLabel.gameObject.SetActive(false);
		this.buttonCancel.gameObject.SetActive(false);
		if (this.someWindowBackFromReconnectSubscription != null)
		{
			this.someWindowBackFromReconnectSubscription.Dispose();
			this.someWindowBackFromReconnectSubscription = null;
		}
	}

	// Token: 0x0600192D RID: 6445 RVA: 0x00062E48 File Offset: 0x00061048
	[Obfuscation(Exclude = true)]
	public void OnCancelButtonClick()
	{
		bool guiActive = ShopNGUIController.GuiActive;
		bool interfaceEnabled = BankController.Instance.InterfaceEnabled;
		bool interfaceEnabled2 = ProfileController.Instance.InterfaceEnabled;
		if (guiActive || interfaceEnabled || interfaceEnabled2 || ExpController.Instance.experienceView.LevelUpPanelOpened || (NetworkStartTableNGUIController.sharedController != null && NetworkStartTableNGUIController.sharedController.isRewardShow))
		{
			base.Invoke("OnCancelButtonClick", 60f);
			return;
		}
		this.isCancelReConnect = true;
		this.goToConnect();
	}

	// Token: 0x0600192E RID: 6446 RVA: 0x00062ED4 File Offset: 0x000610D4
	private void ReconnectGUI()
	{
		bool guiActive = ShopNGUIController.GuiActive;
		bool interfaceEnabled = BankController.Instance.InterfaceEnabled;
		bool flag = ProfileController.Instance.Map((ProfileController p) => p.InterfaceEnabled);
		if (guiActive || interfaceEnabled || flag)
		{
			return;
		}
		if (this.isDisconnect && (!(NetworkStartTableNGUIController.sharedController != null) || !NetworkStartTableNGUIController.sharedController.isRewardShow))
		{
			if (this.timerShowNotConnectToRoom > 0f)
			{
				this.timerShowNotConnectToRoom -= Time.deltaTime;
				if (this.timerShowNotConnectToRoom > 0f)
				{
					if (!this._needReconnectShow)
					{
						this._needReconnectShow = true;
						this.ShowDescriptionLabel(LocalizationStore.Get("Key_1005"));
						this.buttonCancel.gameObject.SetActive(false);
						if (this.someWindowBackFromReconnectSubscription != null)
						{
							this.someWindowBackFromReconnectSubscription.Dispose();
							this.someWindowBackFromReconnectSubscription = null;
						}
					}
				}
				else
				{
					SceneInfo infoScene = SceneInfoController.instance.GetInfoScene(this.goMapName);
					if (infoScene != null)
					{
						this.isDisconnect = false;
						this.JoinRandomRoom(infoScene);
					}
					else
					{
						this.goToConnect();
					}
				}
			}
			else if (!this._roomNotExistShow)
			{
				this._roomNotExistShow = true;
				this.ShowDescriptionLabel(LocalizationStore.Get("Key_1004"));
				bool flag2 = !ShopNGUIController.GuiActive && !ProfileController.Instance.InterfaceEnabled;
				this.buttonCancel.gameObject.SetActive(flag2);
				if (flag2 && this.someWindowBackFromReconnectSubscription == null)
				{
					this.someWindowBackFromReconnectSubscription = BackSystem.Instance.Register(new Action(this.OnCancelButtonClick), "Cancel from reconnect");
				}
				if (!flag2 && this.someWindowBackFromReconnectSubscription != null)
				{
					this.someWindowBackFromReconnectSubscription.Dispose();
					this.someWindowBackFromReconnectSubscription = null;
				}
			}
		}
	}

	// Token: 0x0600192F RID: 6447 RVA: 0x000630C0 File Offset: 0x000612C0
	private void Update()
	{
		if (this._onGUIDrawer)
		{
			this._onGUIDrawer.gameObject.SetActive(this.isDisconnect || this.showLoading);
		}
		if (this.timerShow > 0f)
		{
			this.timerShow -= Time.deltaTime;
			this.showLoading = true;
			this.fonLoadingScene = null;
			base.Invoke("goToConnect", 0.1f);
		}
		this.ReconnectGUI();
	}

	// Token: 0x06001930 RID: 6448 RVA: 0x00063148 File Offset: 0x00061348
	private void OnConnectedToServer()
	{
		this._weaponManager.myTable = (GameObject)Network.Instantiate(this.networkTablePref, base.transform.position, base.transform.rotation, 0);
		this._weaponManager.myNetworkStartTable = this._weaponManager.myTable.GetComponent<NetworkStartTable>();
	}

	// Token: 0x06001931 RID: 6449 RVA: 0x000631A4 File Offset: 0x000613A4
	private void OnFailedToConnect(NetworkConnectionError error)
	{
		if (error == NetworkConnectionError.TooManyConnectedPlayers)
		{
			this.ShowDescriptionLabel(LocalizationStore.Get("Key_0992"));
		}
		if (error == NetworkConnectionError.ConnectionFailed)
		{
			this.ShowDescriptionLabel(LocalizationStore.Get("Key_0993"));
		}
		this.timerShow = 5f;
		if (this._weaponManager == null)
		{
			return;
		}
		if (this._weaponManager.myTable == null)
		{
			return;
		}
		this._weaponManager.myTable.GetComponent<NetworkStartTable>().isShowNickTable = false;
		this._weaponManager.myTable.GetComponent<NetworkStartTable>().showTable = false;
	}

	// Token: 0x06001932 RID: 6450 RVA: 0x00063244 File Offset: 0x00061444
	private void OnDestroy()
	{
		QuestSystem.Instance.SaveQuestProgressIfDirty();
		Initializer.Instance = null;
		Initializer.players.Clear();
		Initializer.bluePlayers.Clear();
		Initializer.redPlayers.Clear();
		Defs.showTableInNetworkStartTable = false;
		Defs.showNickTableInNetworkStartTable = false;
		if (this._onGUIDrawer)
		{
			this._onGUIDrawer.act = null;
		}
		this._gameSessionStopwatch.Stop();
		if (Initializer.lastGameMode == Initializer.GameModeCampaign || Initializer.lastGameMode == Initializer.GameModeSurvival)
		{
			NetworkStartTable.IncreaseTimeInMode(Initializer.lastGameMode, this._gameSessionStopwatch.Elapsed.TotalMinutes);
		}
		ExperienceController.sharedController.isShowRanks = false;
		if (ReviewController.IsNeedActive)
		{
			ConnectSceneNGUIController.NeedShowReviewInConnectScene = true;
		}
		else if (Defs.isMulti)
		{
			string reasonToDismissFakeInterstitial = ConnectSceneNGUIController.GetReasonToDismissFakeInterstitial();
			bool flag = string.IsNullOrEmpty(reasonToDismissFakeInterstitial) && ReplaceAdmobPerelivController.sharedController != null;
			if (flag)
			{
				ReplaceAdmobPerelivController.IncreaseTimesCounter();
			}
			if (flag)
			{
				if (!ReplaceAdmobPerelivController.sharedController.DataLoading)
				{
					ReplaceAdmobPerelivController.sharedController.LoadPerelivData();
				}
				ConnectSceneNGUIController.ReplaceAdmobWithPerelivRequest = true;
			}
			else
			{
				string reasonToDismissInterstitialConnectScene = ConnectSceneNGUIController.GetReasonToDismissInterstitialConnectScene();
				if (Defs.IsDeveloperBuild)
				{
					UnityEngine.Debug.LogFormat("Setting request for interstitial advertisement. {0}", new object[]
					{
						reasonToDismissInterstitialConnectScene
					});
				}
				if (string.IsNullOrEmpty(reasonToDismissInterstitialConnectScene))
				{
					ConnectSceneNGUIController.InterstitialRequest = true;
				}
			}
		}
		PhotonObjectCacher.RemoveObject(base.gameObject);
		Defs.inComingMessagesCounter = 0;
	}

	// Token: 0x06001933 RID: 6451 RVA: 0x000633B4 File Offset: 0x000615B4
	[Obfuscation(Exclude = true)]
	public void goToConnect()
	{
		UnityEngine.Debug.Log("goToConnect()");
		ConnectSceneNGUIController.Local();
	}

	// Token: 0x06001934 RID: 6452 RVA: 0x000633C8 File Offset: 0x000615C8
	private void GoToRandomRoom()
	{
	}

	// Token: 0x06001935 RID: 6453 RVA: 0x000633CC File Offset: 0x000615CC
	private void ShowLoadingGUI(string _mapName)
	{
		this._loadingNGUIController = UnityEngine.Object.Instantiate<GameObject>(Resources.Load<GameObject>("LoadingGUI")).GetComponent<LoadingNGUIController>();
		this._loadingNGUIController.SceneToLoad = _mapName;
		this._loadingNGUIController.loadingNGUITexture.mainTexture = (Resources.Load((!_mapName.Equals("main_loading")) ? ("LevelLoadings" + ((!Device.isRetinaAndStrong) ? string.Empty : "/Hi") + "/Loading_" + _mapName) : string.Empty) as Texture2D);
		this._loadingNGUIController.transform.localPosition = Vector3.zero;
		this._loadingNGUIController.Init();
		ExperienceController.sharedController.isShowRanks = false;
	}

	// Token: 0x06001936 RID: 6454 RVA: 0x00063488 File Offset: 0x00061688
	public void OnLeftRoom()
	{
		UnityEngine.Debug.Log("OnLeftRoom (local) init");
		NickLabelController.currentCamera = null;
		if (Defs.typeDisconnectGame == Defs.DisconectGameType.Exit)
		{
			this.showLoading = true;
			this.fonLoadingScene = null;
			base.Invoke("goToConnect", 0.1f);
			this.ShowLoadingGUI("main_loading");
			if (this._weaponManager == null)
			{
				return;
			}
			if (this._weaponManager.myTable == null)
			{
				return;
			}
			this._weaponManager.myTable.GetComponent<NetworkStartTable>().isShowNickTable = false;
			this._weaponManager.myTable.GetComponent<NetworkStartTable>().showTable = false;
			WeaponManager.sharedManager.myNetworkStartTable.CalculateMatchRatingOnDisconnect();
		}
		if (Defs.typeDisconnectGame == Defs.DisconectGameType.SelectNewMap)
		{
			bool guiActive = ShopNGUIController.GuiActive;
			bool interfaceEnabled = BankController.Instance.InterfaceEnabled;
			bool interfaceEnabled2 = ProfileController.Instance.InterfaceEnabled;
			if (!guiActive && !interfaceEnabled && !interfaceEnabled2)
			{
				ActivityIndicator.IsActiveIndicator = true;
			}
			if (string.IsNullOrEmpty(this.goMapName))
			{
				this.ShowLoadingGUI(this.goMapName);
			}
		}
	}

	// Token: 0x06001937 RID: 6455 RVA: 0x00063598 File Offset: 0x00061798
	public void OnDisconnectedFromPhoton()
	{
		UnityEngine.Debug.Log("OnDisconnectedFromPhotoninit");
		this.OnConnectionFail((DisconnectCause)0);
	}

	// Token: 0x06001938 RID: 6456 RVA: 0x000635AC File Offset: 0x000617AC
	private void OnConnectionFail(DisconnectCause cause)
	{
		BankController.canShowIndication = true;
		Defs.inRespawnWindow = false;
		QuestSystem.Instance.SaveQuestProgressIfDirty();
		if (Defs.typeDisconnectGame == Defs.DisconectGameType.SelectNewMap)
		{
			if (this._loadingNGUIController != null)
			{
				UnityEngine.Object.Destroy(this._loadingNGUIController.gameObject);
				this._loadingNGUIController = null;
			}
			Defs.typeDisconnectGame = Defs.DisconectGameType.Reconnect;
		}
		if (Defs.typeDisconnectGame == Defs.DisconectGameType.Exit)
		{
			this.goToConnect();
			return;
		}
		if (WeaponManager.sharedManager.myNetworkStartTable != null)
		{
			WeaponManager.sharedManager.myNetworkStartTable.CalculateMatchRatingOnDisconnect();
		}
		this.timerShowNotConnectToRoom = -1f;
		this.isCancelReConnect = false;
		this.isNotConnectRoom = false;
		this.countConnectToRoom = 0;
		PlayerPrefs.SetString("TypeGame", "client");
		UnityEngine.Debug.Log("OnConnectionFail " + cause);
		this.tc.SetActive(true);
		BonusController.sharedController.ClearBonuses();
		for (int i = 0; i < Initializer.enemiesObj.Count; i++)
		{
			UnityEngine.Object.Destroy(Initializer.enemiesObj[i]);
		}
		GameObject gameObject = (!(InGameGUI.sharedInGameGUI != null)) ? GameObject.FindGameObjectWithTag("InGameGUI") : InGameGUI.sharedInGameGUI.gameObject;
		if (gameObject != null)
		{
			UnityEngine.Object.Destroy(gameObject);
		}
		GameObject gameObject2 = GameObject.FindGameObjectWithTag("ChatViewer");
		if (gameObject2 != null)
		{
			UnityEngine.Object.Destroy(gameObject2);
		}
		this.isDisconnect = true;
		base.Invoke("ConnectToPhoton", 3f);
		base.Invoke("OnCancelButtonClick", 60f);
		bool guiActive = ShopNGUIController.GuiActive;
		bool interfaceEnabled = BankController.Instance.InterfaceEnabled;
		bool interfaceEnabled2 = ProfileController.Instance.InterfaceEnabled;
		if (!guiActive && !interfaceEnabled && !interfaceEnabled2 && !ExpController.Instance.experienceView.LevelUpPanelOpened && (!(NetworkStartTableNGUIController.sharedController != null) || !NetworkStartTableNGUIController.sharedController.isRewardShow))
		{
			ActivityIndicator.IsActiveIndicator = true;
			ExperienceController.sharedController.isShowRanks = false;
			if (NetworkStartTableNGUIController.sharedController != null && NetworkStartTableNGUIController.sharedController.shopAnchor != null)
			{
				NetworkStartTableNGUIController.sharedController.shopAnchor.SetActive(false);
			}
			return;
		}
	}

	// Token: 0x06001939 RID: 6457 RVA: 0x000637F0 File Offset: 0x000619F0
	[Obfuscation(Exclude = true)]
	private void ConnectToPhoton()
	{
		if (this.isCancelReConnect)
		{
			return;
		}
		bool guiActive = ShopNGUIController.GuiActive;
		bool interfaceEnabled = BankController.Instance.InterfaceEnabled;
		bool interfaceEnabled2 = ProfileController.Instance.InterfaceEnabled;
		if (guiActive || interfaceEnabled || interfaceEnabled2 || ExpController.Instance.experienceView.LevelUpPanelOpened || (NetworkStartTableNGUIController.sharedController != null && NetworkStartTableNGUIController.sharedController.isRewardShow) || ExpController.Instance.WaitingForLevelUpView)
		{
			base.Invoke("ConnectToPhoton", 3f);
			return;
		}
		UnityEngine.Debug.Log("ConnectToPhoton ");
		ActivityIndicator.IsActiveIndicator = true;
		ExperienceController.sharedController.isShowRanks = false;
		if (NetworkStartTableNGUIController.sharedController != null)
		{
			NetworkStartTableNGUIController.sharedController.shopAnchor.SetActive(false);
		}
		PhotonNetwork.autoJoinLobby = false;
		PhotonNetwork.ConnectUsingSettings(string.Concat(new string[]
		{
			Initializer.Separator,
			ConnectSceneNGUIController.regim.ToString(),
			(!Defs.isDaterRegim) ? ((!Defs.isHunger) ? ConnectSceneNGUIController.gameTier.ToString() : "0") : "Dater",
			"v",
			GlobalGameController.MultiplayerProtocolVersion
		}));
	}

	// Token: 0x170004A5 RID: 1189
	// (get) Token: 0x0600193A RID: 6458 RVA: 0x0006393C File Offset: 0x00061B3C
	internal static string Separator
	{
		get
		{
			return Initializer._separator.Value;
		}
	}

	// Token: 0x0600193B RID: 6459 RVA: 0x00063948 File Offset: 0x00061B48
	private static string InitialiseSeparatorWrapper()
	{
		return Initializer.InitializeSeparator();
	}

	// Token: 0x0600193C RID: 6460 RVA: 0x0006395C File Offset: 0x00061B5C
	private static string InitializeSeparator()
	{
		if (Defs.AndroidEdition == Defs.RuntimeAndroidEdition.Amazon)
		{
			return "bada8a20";
		}
		AndroidJavaObject currentActivity = AndroidSystem.Instance.CurrentActivity;
		if (currentActivity == null)
		{
			return "deadac71";
		}
		AndroidJavaObject androidJavaObject = currentActivity.Call<AndroidJavaObject>("getPackageManager", new object[0]);
		if (androidJavaObject == null)
		{
			return "dead3a9a";
		}
		AndroidJavaObject androidJavaObject2 = androidJavaObject.Call<AndroidJavaObject>("getPackageInfo", new object[]
		{
			"com.pixel.gun3d",
			64
		});
		if (androidJavaObject2 == null)
		{
			return "dead6ac5";
		}
		AndroidJavaObject[] array = androidJavaObject2.Get<AndroidJavaObject[]>("signatures");
		if (array == null)
		{
			return "deadc199";
		}
		if (array.Length != 1)
		{
			return "dead139c";
		}
		AndroidJavaObject androidJavaObject3 = array[0];
		byte[] buffer = androidJavaObject3.Call<byte[]>("toByteArray", new object[0]);
		string result;
		using (SHA1Managed sha1Managed = new SHA1Managed())
		{
			byte[] source = sha1Managed.ComputeHash(buffer);
			string text = BitConverter.ToString(source.Take(4).ToArray<byte>()).Replace("-", string.Empty).ToLower();
			result = text;
		}
		return result;
	}

	// Token: 0x0600193D RID: 6461 RVA: 0x00063A94 File Offset: 0x00061C94
	private void OnFailedToConnectToPhoton(object parameters)
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("NetworkStartTableNGUI");
		if (gameObject != null)
		{
			NetworkStartTableNGUIController component = gameObject.GetComponent<NetworkStartTableNGUIController>();
			if (component != null)
			{
				component.shopAnchor.SetActive(false);
			}
		}
		UnityEngine.Debug.Log("OnFailedToConnectToPhoton. StatusCode: " + parameters);
		if (this.isCancelReConnect)
		{
			return;
		}
		base.Invoke("ConnectToPhoton", 3f);
	}

	// Token: 0x0600193E RID: 6462 RVA: 0x00063B04 File Offset: 0x00061D04
	public void OnConnectedToMaster()
	{
		if (this.isLeavingRoom)
		{
			this.isLeavingRoom = false;
			return;
		}
		this.ConnectToRoom();
	}

	// Token: 0x0600193F RID: 6463 RVA: 0x00063B20 File Offset: 0x00061D20
	public void OnJoinedLobby()
	{
		UnityEngine.Debug.Log("OnJoinedLobby()");
		this.ConnectToRoom();
	}

	// Token: 0x06001940 RID: 6464 RVA: 0x00063B34 File Offset: 0x00061D34
	[Obfuscation(Exclude = true)]
	private void ConnectToRoom()
	{
		base.CancelInvoke("OnCancelButtonClick");
		SceneInfo sceneInfo = string.IsNullOrEmpty(this.goMapName) ? null : SceneInfoController.instance.GetInfoScene(this.goMapName);
		if (Defs.typeDisconnectGame == Defs.DisconectGameType.RandomGameInHunger)
		{
			UnityEngine.Debug.Log("JoinRandomRoom");
			this.isCancelReConnect = true;
			int num = UnityEngine.Random.Range(0, SceneInfoController.instance.GetCountScenesForMode(TypeModeGame.DeadlyGames));
			PlayerPrefs.SetString("TypeGame", "client");
			PlayerPrefs.SetInt("CustomGame", 0);
			ConnectSceneNGUIController.JoinRandomGameRoom((!(sceneInfo != null)) ? -1 : sceneInfo.indexMap, ConnectSceneNGUIController.RegimGame.DeadlyGames, this.joinNewRoundTries, this.abTestConnect);
			ActivityIndicator.IsActiveIndicator = true;
			return;
		}
		if (Defs.typeDisconnectGame == Defs.DisconectGameType.RandomGameInDuel)
		{
			UnityEngine.Debug.Log("JoinRandomRoom");
			this.isCancelReConnect = true;
			int num2 = UnityEngine.Random.Range(0, SceneInfoController.instance.GetCountScenesForMode(TypeModeGame.Duel));
			PlayerPrefs.SetString("TypeGame", "client");
			PlayerPrefs.SetInt("CustomGame", 0);
			ConnectSceneNGUIController.JoinRandomGameRoom((!(sceneInfo != null)) ? -1 : sceneInfo.indexMap, ConnectSceneNGUIController.RegimGame.Duel, this.joinNewRoundTries, this.abTestConnect);
			ActivityIndicator.IsActiveIndicator = true;
			return;
		}
		if (Defs.typeDisconnectGame == Defs.DisconectGameType.SelectNewMap)
		{
			UnityEngine.Debug.Log("ConnectToRoom() " + this.goMapName);
			this.JoinRandomRoom(sceneInfo);
			return;
		}
		UnityEngine.Debug.Log("ConnectToRoom " + PlayerPrefs.GetString("RoomName"));
		if (this.isCancelReConnect)
		{
			return;
		}
		PhotonNetwork.JoinRoom(PlayerPrefs.GetString("RoomName"));
	}

	// Token: 0x06001941 RID: 6465 RVA: 0x00063CC4 File Offset: 0x00061EC4
	private void OnPhotonJoinRoomFailed()
	{
		if (ExpController.Instance != null && ConnectSceneNGUIController.gameTier != ExpController.Instance.OurTier)
		{
			ConnectSceneNGUIController.gameTier = ExpController.Instance.OurTier;
			PhotonNetwork.isMessageQueueRunning = true;
			PhotonNetwork.Disconnect();
			return;
		}
		this.countConnectToRoom++;
		UnityEngine.Debug.Log("OnPhotonJoinRoomFailed - init");
		this.isNotConnectRoom = true;
		if (this.countConnectToRoom < 6)
		{
			base.Invoke("ConnectToRoom", 3f);
		}
		else
		{
			this.timerShowNotConnectToRoom = 3f;
		}
	}

	// Token: 0x06001942 RID: 6466 RVA: 0x00063D5C File Offset: 0x00061F5C
	private void JoinRandomRoom(SceneInfo _map)
	{
		if (Defs.typeDisconnectGame != Defs.DisconectGameType.SelectNewMap)
		{
			this.goMapName = _map.NameScene;
		}
		UnityEngine.Debug.Log("JoinRandomRoom " + this.goMapName);
		if (WeaponManager.sharedManager != null)
		{
			WeaponManager.sharedManager.Reset((!Defs.filterMaps.ContainsKey(this.goMapName)) ? 0 : Defs.filterMaps[this.goMapName]);
		}
		ActivityIndicator.IsActiveIndicator = true;
		ConnectSceneNGUIController.JoinRandomGameRoom((!(_map != null)) ? -1 : _map.indexMap, ConnectSceneNGUIController.regim, this.joinNewRoundTries, this.abTestConnect);
	}

	// Token: 0x06001943 RID: 6467 RVA: 0x00063E10 File Offset: 0x00062010
	private void OnPhotonRandomJoinFailed()
	{
		UnityEngine.Debug.Log("OnPhotonJoinRoomFailed");
		PlayerPrefs.SetString("TypeGame", "server");
		SceneInfo infoScene = SceneInfoController.instance.GetInfoScene(this.goMapName);
		if (this.joinNewRoundTries >= 2 && this.abTestConnect)
		{
			this.abTestConnect = false;
			this.joinNewRoundTries = 0;
		}
		if (this.joinNewRoundTries < 2)
		{
			UnityEngine.Debug.Log("No rooms with new round: " + this.joinNewRoundTries + ((!this.abTestConnect) ? string.Empty : " AbTestSeparate"));
			this.joinNewRoundTries++;
			ConnectSceneNGUIController.JoinRandomGameRoom(infoScene.indexMap, ConnectSceneNGUIController.regim, this.joinNewRoundTries, this.abTestConnect);
			return;
		}
		if (WeaponManager.sharedManager != null)
		{
			WeaponManager.sharedManager.Reset((!Defs.filterMaps.ContainsKey(this.goMapName)) ? 0 : Defs.filterMaps[this.goMapName]);
		}
		int playerLimit = (!Defs.isCOOP) ? ((!Defs.isCompany) ? ((!Defs.isHunger) ? ((!Defs.isDuel) ? 10 : 2) : 6) : 10) : 4;
		int num = (!(ExperienceController.sharedController != null) || ExperienceController.sharedController.currentLevel > 2) ? ((!(ExperienceController.sharedController != null) || ExperienceController.sharedController.currentLevel > 5) ? 2 : 1) : 0;
		int maxKill = (ConnectSceneNGUIController.regim != ConnectSceneNGUIController.RegimGame.DeadlyGames) ? ((!Defs.isDaterRegim) ? 4 : 5) : 10;
		ConnectSceneNGUIController.CreateGameRoom(null, playerLimit, infoScene.indexMap, maxKill, string.Empty, ConnectSceneNGUIController.regim);
	}

	// Token: 0x06001944 RID: 6468 RVA: 0x00063FEC File Offset: 0x000621EC
	[Obfuscation(Exclude = true)]
	private void StartGameAfterDisconnectInvoke()
	{
		if (ConnectSceneNGUIController.regim != ConnectSceneNGUIController.RegimGame.TimeBattle && ConnectSceneNGUIController.regim != ConnectSceneNGUIController.RegimGame.FlagCapture && ConnectSceneNGUIController.regim != ConnectSceneNGUIController.RegimGame.TeamFight && ConnectSceneNGUIController.regim != ConnectSceneNGUIController.RegimGame.CapturePoints && !Defs.showTableInNetworkStartTable && !Defs.showNickTableInNetworkStartTable)
		{
			NetworkStartTable.StartAfterDisconnect = true;
		}
		this._weaponManager.myTable = PhotonNetwork.Instantiate("NetworkTable", base.transform.position, base.transform.rotation, 0);
		this._weaponManager.myNetworkStartTable = this._weaponManager.myTable.GetComponent<NetworkStartTable>();
		ActivityIndicator.IsActiveIndicator = false;
	}

	// Token: 0x06001945 RID: 6469 RVA: 0x0006408C File Offset: 0x0006228C
	private void OnJoinedRoom()
	{
		if (!this.isDisconnect)
		{
			AnalyticsStuff.LogMultiplayer();
		}
		if (!this.CheckRoom())
		{
			return;
		}
		UnityEngine.Debug.Log("OnJoinedRoom - init");
		PlayerPrefs.SetString("RoomName", PhotonNetwork.room.name);
		SceneInfo infoScene = SceneInfoController.instance.GetInfoScene(int.Parse(PhotonNetwork.room.customProperties[ConnectSceneNGUIController.mapProperty].ToString()));
		Initializer.Instance.goMapName = infoScene.NameScene;
		if (WeaponManager.sharedManager != null)
		{
			WeaponManager.sharedManager.Reset((!Defs.filterMaps.ContainsKey(Initializer.Instance.goMapName)) ? 0 : Defs.filterMaps[Initializer.Instance.goMapName]);
		}
		if (this.isDisconnect && (ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.Deathmatch || ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.DeadlyGames || ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.Duel))
		{
			base.Invoke("StartGameAfterDisconnectInvoke", 3f);
		}
		else
		{
			GlobalGameController.healthMyPlayer = 0f;
			NetworkStartTable.StartAfterDisconnect = false;
			PhotonNetwork.isMessageQueueRunning = false;
			base.StartCoroutine(this.MoveToGameScene());
		}
		this.isDisconnect = false;
		this._roomNotExistShow = false;
		this._needReconnectShow = false;
		this.HideReconnectInterface();
	}

	// Token: 0x06001946 RID: 6470 RVA: 0x000641D8 File Offset: 0x000623D8
	private IEnumerator MoveToGameScene()
	{
		if (Defs.typeDisconnectGame != Defs.DisconectGameType.SelectNewMap && WeaponManager.sharedManager != null)
		{
			WeaponManager.sharedManager.Reset((!Defs.filterMaps.ContainsKey(this.goMapName)) ? 0 : Defs.filterMaps[this.goMapName]);
		}
		UnityEngine.Debug.Log("MoveToGameScene");
		while (PhotonNetwork.room == null)
		{
			yield return 0;
		}
		PhotonNetwork.isMessageQueueRunning = false;
		SceneInfo scInfo = SceneInfoController.instance.GetInfoScene(int.Parse(PhotonNetwork.room.customProperties[ConnectSceneNGUIController.mapProperty].ToString()));
		UnityEngine.Debug.Log(scInfo.NameScene);
		LoadConnectScene.textureToShow = (Resources.Load("LevelLoadings" + ((!Device.isRetinaAndStrong) ? string.Empty : "/Hi") + "/Loading_" + scInfo.NameScene) as Texture2D);
		LoadingInAfterGame.loadingTexture = LoadConnectScene.textureToShow;
		UnityEngine.Debug.Log("LoadConnectScene.textureToShow " + LoadConnectScene.textureToShow.name);
		LoadConnectScene.sceneToLoad = scInfo.NameScene;
		LoadConnectScene.noteToShow = null;
		AsyncOperation async = Singleton<SceneLoader>.Instance.LoadSceneAsync("PromScene", LoadSceneMode.Single);
		FriendsController.sharedController.GetFriendsData(false);
		yield return async;
		yield break;
	}

	// Token: 0x06001947 RID: 6471 RVA: 0x000641F4 File Offset: 0x000623F4
	public void OnPhotonPlayerConnected(PhotonPlayer player)
	{
	}

	// Token: 0x06001948 RID: 6472 RVA: 0x000641F8 File Offset: 0x000623F8
	public void OnPhotonPlayerDisconnected(PhotonPlayer player)
	{
	}

	// Token: 0x06001949 RID: 6473 RVA: 0x000641FC File Offset: 0x000623FC
	public void OnReceivedRoomList()
	{
	}

	// Token: 0x0600194A RID: 6474 RVA: 0x00064200 File Offset: 0x00062400
	public void OnReceivedRoomListUpdate()
	{
	}

	// Token: 0x0600194B RID: 6475 RVA: 0x00064204 File Offset: 0x00062404
	public void OnConnectedToPhoton()
	{
		UnityEngine.Debug.Log("OnConnectedToPhotoninit");
	}

	// Token: 0x0600194C RID: 6476 RVA: 0x00064210 File Offset: 0x00062410
	public void OnFailedToConnectToPhoton()
	{
		UnityEngine.Debug.Log("OnFailedToConnectToPhotoninit");
	}

	// Token: 0x0600194D RID: 6477 RVA: 0x0006421C File Offset: 0x0006241C
	public void OnPhotonInstantiate(PhotonMessageInfo info)
	{
		UnityEngine.Debug.Log("OnPhotonInstantiate init" + info.sender);
	}

	// Token: 0x04000E25 RID: 3621
	public Texture killstreakeAtlas;

	// Token: 0x04000E26 RID: 3622
	public GameObject tc;

	// Token: 0x04000E27 RID: 3623
	public GameObject tempCam;

	// Token: 0x04000E28 RID: 3624
	public bool isDisconnect;

	// Token: 0x04000E29 RID: 3625
	public int countConnectToRoom;

	// Token: 0x04000E2A RID: 3626
	public float timerShowNotConnectToRoom;

	// Token: 0x04000E2B RID: 3627
	public UIButton buttonCancel;

	// Token: 0x04000E2C RID: 3628
	public UILabel descriptionLabel;

	// Token: 0x04000E2D RID: 3629
	public bool isCancelReConnect;

	// Token: 0x04000E2E RID: 3630
	private GameObject _playerPrefab;

	// Token: 0x04000E2F RID: 3631
	private UnityEngine.Object networkTablePref;

	// Token: 0x04000E30 RID: 3632
	private bool _isMultiplayer;

	// Token: 0x04000E31 RID: 3633
	private bool isLeavingRoom;

	// Token: 0x04000E32 RID: 3634
	public bool isNotConnectRoom;

	// Token: 0x04000E33 RID: 3635
	private Vector2 scrollPosition = Vector2.zero;

	// Token: 0x04000E34 RID: 3636
	private List<Vector3> _initPlayerPositions = new List<Vector3>();

	// Token: 0x04000E35 RID: 3637
	private List<float> _rots = new List<float>();

	// Token: 0x04000E36 RID: 3638
	public static List<NetworkStartTable> networkTables = new List<NetworkStartTable>();

	// Token: 0x04000E37 RID: 3639
	public static readonly List<Player_move_c> players = new List<Player_move_c>();

	// Token: 0x04000E38 RID: 3640
	public static List<Player_move_c> bluePlayers = new List<Player_move_c>();

	// Token: 0x04000E39 RID: 3641
	public static List<Player_move_c> redPlayers = new List<Player_move_c>();

	// Token: 0x04000E3A RID: 3642
	public static List<GameObject> playersObj = new List<GameObject>();

	// Token: 0x04000E3B RID: 3643
	public static List<GameObject> enemiesObj = new List<GameObject>();

	// Token: 0x04000E3C RID: 3644
	public static List<GameObject> turretsObj = new List<GameObject>();

	// Token: 0x04000E3D RID: 3645
	public static List<GameObject> petsObj = new List<GameObject>();

	// Token: 0x04000E3E RID: 3646
	public static List<GameObject> damageableObjects = new List<GameObject>();

	// Token: 0x04000E3F RID: 3647
	public static List<SingularityHole> singularities = new List<SingularityHole>();

	// Token: 0x04000E40 RID: 3648
	public static FlagController flag1;

	// Token: 0x04000E41 RID: 3649
	public static FlagController flag2;

	// Token: 0x04000E42 RID: 3650
	private float koofScreen = (float)Screen.height / 768f;

	// Token: 0x04000E43 RID: 3651
	public WeaponManager _weaponManager;

	// Token: 0x04000E44 RID: 3652
	public float timerShow = -1f;

	// Token: 0x04000E45 RID: 3653
	public Transform playerPrefab;

	// Token: 0x04000E46 RID: 3654
	public Texture fonLoadingScene;

	// Token: 0x04000E47 RID: 3655
	private bool showLoading;

	// Token: 0x04000E48 RID: 3656
	private bool isMulti;

	// Token: 0x04000E49 RID: 3657
	private bool isInet;

	// Token: 0x04000E4A RID: 3658
	private PauseONGuiDrawer _onGUIDrawer;

	// Token: 0x04000E4B RID: 3659
	public static int GameModeCampaign = 100;

	// Token: 0x04000E4C RID: 3660
	public static int GameModeSurvival = 101;

	// Token: 0x04000E4D RID: 3661
	public static int lastGameMode = -1;

	// Token: 0x04000E4E RID: 3662
	private Stopwatch _gameSessionStopwatch = new Stopwatch();

	// Token: 0x04000E4F RID: 3663
	public string goMapName = string.Empty;

	// Token: 0x04000E50 RID: 3664
	private bool _needReconnectShow;

	// Token: 0x04000E51 RID: 3665
	private bool _roomNotExistShow;

	// Token: 0x04000E52 RID: 3666
	private IDisposable someWindowBackFromReconnectSubscription;

	// Token: 0x04000E53 RID: 3667
	public LoadingNGUIController _loadingNGUIController;

	// Token: 0x04000E54 RID: 3668
	private static readonly Lazy<string> _separator = new Lazy<string>(new Func<string>(Initializer.InitialiseSeparatorWrapper));

	// Token: 0x04000E55 RID: 3669
	private int joinNewRoundTries;

	// Token: 0x04000E56 RID: 3670
	private bool abTestConnect;

	// Token: 0x020002CF RID: 719
	public class TargetsList
	{
		// Token: 0x0600194F RID: 6479 RVA: 0x0006423C File Offset: 0x0006243C
		public TargetsList()
		{
			this.forPlayer = WeaponManager.sharedManager.myPlayerMoveC;
			this.includeSelf = false;
			this.includeExplosions = true;
		}

		// Token: 0x06001950 RID: 6480 RVA: 0x00064270 File Offset: 0x00062470
		public TargetsList(Player_move_c forPlayer, bool includeMyPlayer = false, bool includeExplosions = true)
		{
			this.forPlayer = forPlayer;
			this.includeSelf = includeMyPlayer;
			this.includeExplosions = includeExplosions;
		}

		// Token: 0x06001951 RID: 6481 RVA: 0x00064290 File Offset: 0x00062490
		public IEnumerator<Transform> GetEnumerator()
		{
			if (this.includeSelf)
			{
				yield return this.forPlayer.myPlayerTransform;
			}
			if (Defs.isMulti && ConnectSceneNGUIController.regim != ConnectSceneNGUIController.RegimGame.TimeBattle)
			{
				for (int i = 0; i < Initializer.players.Count; i++)
				{
					if (!Initializer.players[i].Equals(this.forPlayer))
					{
						if (!Initializer.players[i].isKilled)
						{
							if (Initializer.IsEnemyTarget(Initializer.players[i].myPlayerTransform, this.forPlayer))
							{
								yield return Initializer.players[i].myPlayerTransform;
							}
						}
					}
				}
				for (int j = 0; j < Initializer.turretsObj.Count; j++)
				{
					TurretController currentTurret = Initializer.turretsObj[j].GetComponent<TurretController>();
					if (!currentTurret.isKilled)
					{
						if (Initializer.IsEnemyTarget(Initializer.turretsObj[j].transform, this.forPlayer))
						{
							yield return currentTurret.transform;
						}
					}
				}
				for (int k = 0; k < Initializer.petsObj.Count; k++)
				{
					if (Initializer.IsEnemyTarget(Initializer.petsObj[k].transform, this.forPlayer))
					{
						PetEngine currentPet = Initializer.petsObj[k].GetComponent<PetEngine>();
						if (currentPet.IsAlive)
						{
							yield return currentPet.transform;
						}
					}
				}
			}
			else
			{
				for (int l = 0; l < Initializer.enemiesObj.Count; l++)
				{
					if (!Initializer.enemiesObj[l].GetComponent<BaseBot>().IsDeath)
					{
						yield return Initializer.enemiesObj[l].transform;
					}
				}
			}
			for (int m = 0; m < Initializer.damageableObjects.Count; m++)
			{
				IDamageable target = Initializer.damageableObjects[m].GetComponent<IDamageable>();
				if (!target.IsDead() && target.IsEnemyTo(this.forPlayer))
				{
					if (this.includeExplosions || !(target is DamagedExplosionObject))
					{
						yield return Initializer.damageableObjects[m].transform;
					}
				}
			}
			yield break;
		}

		// Token: 0x04000E5A RID: 3674
		private Player_move_c forPlayer;

		// Token: 0x04000E5B RID: 3675
		private bool includeSelf;

		// Token: 0x04000E5C RID: 3676
		private bool includeExplosions;
	}
}
