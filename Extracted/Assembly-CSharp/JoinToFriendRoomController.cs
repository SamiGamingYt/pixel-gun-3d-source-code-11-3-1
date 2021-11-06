using System;
using System.Collections;
using System.Reflection;
using Rilisoft;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x0200061E RID: 1566
public class JoinToFriendRoomController : MonoBehaviour
{
	// Token: 0x06003626 RID: 13862 RVA: 0x00117A24 File Offset: 0x00115C24
	private void Awake()
	{
		this.inputPasswordLabel.onSubmit.Add(new EventDelegate(delegate()
		{
			this.EnterPassword(this.inputPasswordLabel.value);
		}));
	}

	// Token: 0x06003627 RID: 13863 RVA: 0x00117A48 File Offset: 0x00115C48
	private void Start()
	{
		JoinToFriendRoomController.Instance = this;
	}

	// Token: 0x06003628 RID: 13864 RVA: 0x00117A50 File Offset: 0x00115C50
	private void OnEnable()
	{
		PhotonObjectCacher.AddObject(base.gameObject);
	}

	// Token: 0x06003629 RID: 13865 RVA: 0x00117A60 File Offset: 0x00115C60
	private void OnEsc()
	{
		PhotonNetwork.isMessageQueueRunning = true;
		PhotonNetwork.Disconnect();
		this.closeConnectPanel();
	}

	// Token: 0x0600362A RID: 13866 RVA: 0x00117A74 File Offset: 0x00115C74
	private void OnDisable()
	{
		PhotonObjectCacher.RemoveObject(base.gameObject);
	}

	// Token: 0x0600362B RID: 13867 RVA: 0x00117A84 File Offset: 0x00115C84
	private void OnDestroy()
	{
		JoinToFriendRoomController.Instance = null;
		PhotonObjectCacher.RemoveObject(base.gameObject);
	}

	// Token: 0x0600362C RID: 13868 RVA: 0x00117A98 File Offset: 0x00115C98
	public void BackFromPasswordButton()
	{
		this._isBackFromPassword = true;
		this.SetEnabledPasswordPanel(false);
		PhotonNetwork.isMessageQueueRunning = true;
		PhotonNetwork.Disconnect();
	}

	// Token: 0x0600362D RID: 13869 RVA: 0x00117AB4 File Offset: 0x00115CB4
	public void OnClickAcceptPassword()
	{
		this.EnterPassword(this.inputPasswordLabel.value);
	}

	// Token: 0x0600362E RID: 13870 RVA: 0x00117AC8 File Offset: 0x00115CC8
	public void EnterPassword(string pass)
	{
		if (pass == this._passwordRoom)
		{
			PhotonNetwork.isMessageQueueRunning = false;
			base.StartCoroutine(this.MoveToGameScene());
			ActivityIndicator.IsActiveIndicator = true;
		}
		else
		{
			this._timerShowWrongPassword = 3f;
			this.wrongPasswordLabel.SetActive(true);
		}
	}

	// Token: 0x0600362F RID: 13871 RVA: 0x00117B1C File Offset: 0x00115D1C
	private void ShowLoadingGUI(string _mapName)
	{
		this._loadingNGUIController = UnityEngine.Object.Instantiate<GameObject>(Resources.Load<GameObject>("LoadingGUI")).GetComponent<LoadingNGUIController>();
		this._loadingNGUIController.SceneToLoad = _mapName;
		this._loadingNGUIController.loadingNGUITexture.mainTexture = Resources.Load<Texture2D>("LevelLoadings" + ((!Device.isRetinaAndStrong) ? string.Empty : "/Hi") + "/Loading_" + _mapName);
		this._loadingNGUIController.transform.parent = this.backgroundConnectTexture.transform.parent;
		this._loadingNGUIController.transform.localPosition = Vector3.zero;
		this._loadingNGUIController.Init();
	}

	// Token: 0x06003630 RID: 13872 RVA: 0x00117BD0 File Offset: 0x00115DD0
	private void RemoveLoadingGUI()
	{
		if (this._loadingNGUIController != null)
		{
			UnityEngine.Object.Destroy(this._loadingNGUIController.gameObject);
			this._loadingNGUIController = null;
		}
	}

	// Token: 0x06003631 RID: 13873 RVA: 0x00117C08 File Offset: 0x00115E08
	private IEnumerator SetFonLoadingWaitForReset(string _mapName = "", bool isAddCountRun = false)
	{
		this.RemoveLoadingGUI();
		while (WeaponManager.sharedManager == null)
		{
			yield return null;
		}
		while (WeaponManager.sharedManager.LockGetWeaponPrefabs > 0)
		{
			yield return null;
		}
		this.ShowLoadingGUI(_mapName);
		yield break;
	}

	// Token: 0x06003632 RID: 13874 RVA: 0x00117C34 File Offset: 0x00115E34
	public void ConnectToRoom(int gameModeCode, string nameRoom, string map)
	{
		if (this._backSubscription != null)
		{
			this._backSubscription.Dispose();
		}
		this._backSubscription = BackSystem.Instance.Register(new Action(this.OnEsc), "Connect To Friend");
		InfoWindowController.HideCurrentWindow();
		this.SetEnabledPasswordPanel(false);
		SceneInfo infoScene = SceneInfoController.instance.GetInfoScene(int.Parse(map));
		bool isPremium = infoScene.isPremium;
		if (isPremium && Storager.getInt(infoScene.NameScene + "Key", true) != 1 && !PremiumAccountController.MapAvailableDueToPremiumAccount(infoScene.NameScene))
		{
			Action successfulUnlockCallback = delegate()
			{
			};
			this.ShowUnlockMapDialog(successfulUnlockCallback, infoScene.NameScene);
			return;
		}
		int gameTier = (gameModeCode <= 99) ? (gameModeCode / 10) : (gameModeCode % 100 / 10);
		this.gameMode = gameModeCode % 10;
		this.roomName = nameRoom;
		Defs.isMulti = true;
		Defs.isInet = true;
		Defs.isFlag = false;
		Defs.isCOOP = false;
		Defs.isCompany = false;
		Defs.isHunger = false;
		Defs.isCapturePoints = false;
		Defs.isDuel = false;
		switch (this.gameMode)
		{
		case 0:
			StoreKitEventListener.State.Mode = "Deathmatch Wordwide";
			ConnectSceneNGUIController.regim = ConnectSceneNGUIController.RegimGame.Deathmatch;
			goto IL_264;
		case 1:
			StoreKitEventListener.State.Mode = "Time Survival";
			Defs.isCOOP = true;
			ConnectSceneNGUIController.regim = ConnectSceneNGUIController.RegimGame.TimeBattle;
			goto IL_264;
		case 2:
			StoreKitEventListener.State.Mode = "Team Battle";
			Defs.isCompany = true;
			ConnectSceneNGUIController.regim = ConnectSceneNGUIController.RegimGame.TeamFight;
			goto IL_264;
		case 3:
		{
			bool flag = true;
			if (flag)
			{
				Defs.isHunger = true;
				StoreKitEventListener.State.Mode = "Deadly Games";
				ConnectSceneNGUIController.regim = ConnectSceneNGUIController.RegimGame.DeadlyGames;
				goto IL_264;
			}
			if (ShowNoJoinConnectFromRanks.sharedController != null)
			{
				ShowNoJoinConnectFromRanks.sharedController.resetShow(3);
			}
			return;
		}
		case 4:
		{
			bool flag2 = true;
			if (flag2)
			{
				Defs.isFlag = true;
				ConnectSceneNGUIController.regim = ConnectSceneNGUIController.RegimGame.FlagCapture;
				goto IL_264;
			}
			StoreKitEventListener.State.Mode = "Flag Capture";
			if (ShowNoJoinConnectFromRanks.sharedController != null)
			{
				ShowNoJoinConnectFromRanks.sharedController.resetShow(4);
			}
			return;
		}
		case 5:
			Defs.isCapturePoints = true;
			ConnectSceneNGUIController.regim = ConnectSceneNGUIController.RegimGame.CapturePoints;
			goto IL_264;
		case 8:
			Defs.isDuel = true;
			ConnectSceneNGUIController.regim = ConnectSceneNGUIController.RegimGame.Duel;
			goto IL_264;
		}
		return;
		IL_264:
		ActivityIndicator.IsActiveIndicator = true;
		this.connectPanel.SetActive(true);
		this.infoBoxLabel.gameObject.SetActive(false);
		this.infoBoxContainer.SetActive(false);
		WeaponManager.sharedManager.Reset((int)infoScene.AvaliableWeapon);
		base.StartCoroutine(this.SetFonLoadingWaitForReset(infoScene.NameScene, false));
		Defs.isDaterRegim = (infoScene.AvaliableWeapon == ModeWeapon.dater);
		string gameVersion = string.Concat(new string[]
		{
			Initializer.Separator,
			ConnectSceneNGUIController.regim.ToString(),
			(!Defs.isDaterRegim) ? ((!Defs.isHunger) ? gameTier.ToString() : "0") : "Dater",
			"v",
			GlobalGameController.MultiplayerProtocolVersion
		});
		ConnectSceneNGUIController.gameTier = gameTier;
		PhotonNetwork.autoJoinLobby = false;
		PhotonNetwork.ConnectUsingSettings(gameVersion);
	}

	// Token: 0x06003633 RID: 13875 RVA: 0x00117F84 File Offset: 0x00116184
	private void Update()
	{
		if (coinsPlashka.thisScript != null)
		{
			coinsPlashka.thisScript.enabled = (coinsShop.thisScript != null && coinsShop.thisScript.enabled);
		}
		if (this._timerShowWrongPassword > 0f && this.wrongPasswordLabel.activeSelf)
		{
			this._timerShowWrongPassword -= Time.deltaTime;
		}
		if (this._timerShowWrongPassword <= 0f && this.wrongPasswordLabel.activeSelf)
		{
			this.wrongPasswordLabel.SetActive(false);
		}
	}

	// Token: 0x06003634 RID: 13876 RVA: 0x00118028 File Offset: 0x00116228
	private void ShowUnlockMapDialog(Action successfulUnlockCallback, string levelName)
	{
		if (string.IsNullOrEmpty(levelName))
		{
			Debug.LogWarning("Level name shoul not be empty.");
			return;
		}
		UnityEngine.Object original = Resources.Load("UnlockPremiumMapView");
		GameObject gameObject = UnityEngine.Object.Instantiate(original) as GameObject;
		gameObject.transform.parent = base.transform;
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.localScale = Vector3.one;
		Tools.SetLayerRecursively(gameObject, base.gameObject.layer);
		ActivityIndicator.IsActiveIndicator = false;
		UnlockPremiumMapView unlockPremiumMapView = gameObject.GetComponent<UnlockPremiumMapView>();
		if (unlockPremiumMapView == null)
		{
			Debug.LogError("UnlockPremiumMapView should not be null.");
			return;
		}
		int price = 0;
		Defs.PremiumMaps.TryGetValue(levelName, out price);
		unlockPremiumMapView.Price = price;
		EventHandler value = delegate(object sender, EventArgs e)
		{
			this.HandleCloseUnlockDialog(unlockPremiumMapView);
		};
		EventHandler value2 = delegate(object sender, EventArgs e)
		{
			this.HandleUnlockPressed(unlockPremiumMapView, successfulUnlockCallback, levelName);
		};
		unlockPremiumMapView.ClosePressed += value;
		unlockPremiumMapView.UnlockPressed += value2;
	}

	// Token: 0x06003635 RID: 13877 RVA: 0x00118154 File Offset: 0x00116354
	private void HandleCloseUnlockDialog(UnlockPremiumMapView unlockPremiumMapView)
	{
		this.closeConnectPanel();
		UnityEngine.Object.Destroy(unlockPremiumMapView.gameObject);
	}

	// Token: 0x06003636 RID: 13878 RVA: 0x00118168 File Offset: 0x00116368
	private void HandleUnlockPressed(UnlockPremiumMapView unlockPremiumMapView, Action successfulUnlockCallback, string levelName)
	{
		int priceAmount = unlockPremiumMapView.Price;
		ShopNGUIController.TryToBuy(base.transform.root.gameObject, new ItemPrice(unlockPremiumMapView.Price, "Coins"), delegate
		{
			Storager.setInt(levelName + "Key", 1, true);
			if (Application.platform != RuntimePlatform.IPhonePlayer)
			{
				PlayerPrefs.Save();
			}
			ShopNGUIController.SynchronizeAndroidPurchases("Friend's map unlocked: " + levelName);
			AnalyticsStuff.LogSales(levelName, "Premium Maps", false);
			AnalyticsFacade.InAppPurchase(levelName, "Premium Maps", 1, priceAmount, "Coins");
			if (coinsPlashka.thisScript != null)
			{
				coinsPlashka.thisScript.enabled = false;
			}
			successfulUnlockCallback();
			UnityEngine.Object.Destroy(unlockPremiumMapView.gameObject);
		}, delegate
		{
			StoreKitEventListener.State.PurchaseKey = "In map selection In Friends";
		}, null, null, null, null);
	}

	// Token: 0x06003637 RID: 13879 RVA: 0x001181F8 File Offset: 0x001163F8
	[Obfuscation(Exclude = true)]
	public void closeConnectPanel()
	{
		if (this._backSubscription != null)
		{
			this._backSubscription.Dispose();
			this._backSubscription = null;
		}
		this.backgroundConnectTexture.mainTexture = null;
		this.RemoveLoadingGUI();
		this.connectPanel.SetActive(false);
		this.infoBoxLabel.gameObject.SetActive(false);
		this.infoBoxContainer.SetActive(false);
		ActivityIndicator.IsActiveIndicator = false;
	}

	// Token: 0x06003638 RID: 13880 RVA: 0x00118264 File Offset: 0x00116464
	private void ShowLabel(string text)
	{
		this.infoBoxLabel.text = text;
		this.infoBoxLabel.gameObject.SetActive(true);
		this.infoBoxContainer.SetActive(true);
		ActivityIndicator.IsActiveIndicator = false;
		base.Invoke("closeConnectPanel", 3f);
	}

	// Token: 0x06003639 RID: 13881 RVA: 0x001182B0 File Offset: 0x001164B0
	private void OnDisconnectedFromPhoton()
	{
		if (this._isFaledConnectToRoom)
		{
			this.ShowLabel(LocalizationStore.Get("Key_1410"));
		}
		else if (this._isBackFromPassword)
		{
			this.closeConnectPanel();
		}
		else
		{
			this.ShowLabel(LocalizationStore.Get("Key_1410"));
		}
		this._isFaledConnectToRoom = false;
		this._isBackFromPassword = false;
	}

	// Token: 0x0600363A RID: 13882 RVA: 0x00118314 File Offset: 0x00116514
	private void OnFailedToConnectToPhoton(object parameters)
	{
		this.ShowLabel(LocalizationStore.Get("Key_1411"));
		Debug.Log("OnFailedToConnectToPhoton. StatusCode: " + parameters);
	}

	// Token: 0x0600363B RID: 13883 RVA: 0x00118344 File Offset: 0x00116544
	public void OnConnectedToMaster()
	{
		this.ConnectToRoom();
	}

	// Token: 0x0600363C RID: 13884 RVA: 0x0011834C File Offset: 0x0011654C
	public void OnJoinedLobby()
	{
		this.ConnectToRoom();
	}

	// Token: 0x0600363D RID: 13885 RVA: 0x00118354 File Offset: 0x00116554
	[Obfuscation(Exclude = true)]
	private void ConnectToRoom()
	{
		PhotonNetwork.playerName = ProfileController.GetPlayerNameOrDefault();
		Debug.Log("OnJoinedLobby " + this.roomName);
		PhotonNetwork.JoinRoom(this.roomName);
		PlayerPrefs.SetString("RoomName", this.roomName);
	}

	// Token: 0x0600363E RID: 13886 RVA: 0x00118394 File Offset: 0x00116594
	private void OnPhotonJoinRoomFailed()
	{
		Debug.Log("OnPhotonJoinRoomFailed - init");
		this._isFaledConnectToRoom = true;
		PhotonNetwork.isMessageQueueRunning = true;
		PhotonNetwork.Disconnect();
		InfoWindowController.ShowInfoBox(LocalizationStore.Get("Key_0137"));
		InfoWindowController.HideProcessing(3f);
		Defs.isFlag = false;
		Defs.isCOOP = false;
		Defs.isCompany = false;
		Defs.isHunger = false;
		Defs.isDuel = false;
		Defs.isCapturePoints = false;
		Defs.isDaterRegim = false;
		WeaponManager.sharedManager.Reset(0);
	}

	// Token: 0x0600363F RID: 13887 RVA: 0x0011840C File Offset: 0x0011660C
	private void SetEnabledPasswordPanel(bool enabled)
	{
		this.passwordPanel.SetActive(enabled);
		if (this._loadingNGUIController != null)
		{
			this.backgroundConnectTexture.mainTexture = ((!enabled) ? null : this._loadingNGUIController.loadingNGUITexture.mainTexture);
			this._loadingNGUIController.gameObject.SetActive(!enabled);
		}
	}

	// Token: 0x06003640 RID: 13888 RVA: 0x00118474 File Offset: 0x00116674
	private void OnJoinedRoom()
	{
		Debug.Log("OnJoinedRoom - init");
		if (PhotonNetwork.room != null)
		{
			this._passwordRoom = PhotonNetwork.room.customProperties[ConnectSceneNGUIController.passwordProperty].ToString();
			PhotonNetwork.isMessageQueueRunning = false;
			if (this._passwordRoom.Equals(string.Empty))
			{
				PhotonNetwork.isMessageQueueRunning = false;
				base.StartCoroutine(this.MoveToGameScene());
			}
			else
			{
				Debug.Log("Show Password Panel " + this._passwordRoom);
				ActivityIndicator.IsActiveIndicator = false;
				this.inputPasswordLabel.value = string.Empty;
				this.SetEnabledPasswordPanel(true);
			}
		}
		else
		{
			PhotonNetwork.isMessageQueueRunning = true;
			PhotonNetwork.Disconnect();
			this.ShowLabel(LocalizationStore.Get("Key_1410"));
		}
	}

	// Token: 0x06003641 RID: 13889 RVA: 0x0011853C File Offset: 0x0011673C
	private IEnumerator MoveToGameScene()
	{
		AnalyticsStuff.LogMultiplayer();
		if (SceneLoader.ActiveSceneName.Equals("Clans"))
		{
			Defs.isGameFromFriends = false;
			Defs.isGameFromClans = true;
		}
		else
		{
			Defs.isGameFromFriends = true;
			Defs.isGameFromClans = false;
		}
		SceneInfo scInfo = SceneInfoController.instance.GetInfoScene(int.Parse(PhotonNetwork.room.customProperties[ConnectSceneNGUIController.mapProperty].ToString()));
		string mapName = scInfo.NameScene;
		WeaponManager.sharedManager.Reset((int)scInfo.AvaliableWeapon);
		while (PhotonNetwork.room == null)
		{
			yield return 0;
		}
		Debug.Log("map = " + PhotonNetwork.room.customProperties[ConnectSceneNGUIController.mapProperty].ToString());
		Debug.Log(mapName);
		LoadConnectScene.textureToShow = (Resources.Load("LevelLoadings" + ((!Device.isRetinaAndStrong) ? string.Empty : "/Hi") + "/Loading_" + mapName) as Texture2D);
		LoadingInAfterGame.loadingTexture = LoadConnectScene.textureToShow;
		LoadConnectScene.sceneToLoad = mapName;
		LoadConnectScene.noteToShow = null;
		AsyncOperation async = Singleton<SceneLoader>.Instance.LoadSceneAsync("PromScene", LoadSceneMode.Single);
		yield return async;
		yield break;
	}

	// Token: 0x040027C5 RID: 10181
	public int gameMode;

	// Token: 0x040027C6 RID: 10182
	public string roomName;

	// Token: 0x040027C7 RID: 10183
	public GameObject connectPanel;

	// Token: 0x040027C8 RID: 10184
	public UITexture backgroundConnectTexture;

	// Token: 0x040027C9 RID: 10185
	public UILabel infoBoxLabel;

	// Token: 0x040027CA RID: 10186
	public GameObject infoBoxContainer;

	// Token: 0x040027CB RID: 10187
	public GameObject passwordPanel;

	// Token: 0x040027CC RID: 10188
	public GameObject wrongPasswordLabel;

	// Token: 0x040027CD RID: 10189
	public UIInput inputPasswordLabel;

	// Token: 0x040027CE RID: 10190
	public static JoinToFriendRoomController Instance;

	// Token: 0x040027CF RID: 10191
	private bool _isFaledConnectToRoom;

	// Token: 0x040027D0 RID: 10192
	private string _passwordRoom;

	// Token: 0x040027D1 RID: 10193
	private float _timerShowWrongPassword;

	// Token: 0x040027D2 RID: 10194
	private bool _isBackFromPassword;

	// Token: 0x040027D3 RID: 10195
	private IDisposable _backSubscription;

	// Token: 0x040027D4 RID: 10196
	private LoadingNGUIController _loadingNGUIController;
}
